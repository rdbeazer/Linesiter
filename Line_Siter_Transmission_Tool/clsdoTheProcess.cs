﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using DotSpatial.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;
using System.IO;
using DotSpatial.Symbology;
using DotSpatial.Controls;
using DotSpatial.Topology;
using DotSpatial.Projections;
using System.Reflection;
using GeospatialFiles;
using System.Threading;
using System.Diagnostics;


namespace LineSiterSitingTool
{
    class clsdoTheProcess
    {

        private IRaster _additiveCosts;
        public IRaster additiveCosts
        {
            get { return _additiveCosts; }
            set { _additiveCosts = value; }
        }


        private List<string> _finalStatOutput;
        public List<string> finalStatOutput
        {
            get { return _finalStatOutput; }
            set { _finalStatOutput = value; }
        }
        clsMonteCarlo MC = new clsMonteCarlo();
        clsLCPCoords lc = new clsLCPCoords();
        clsprepgatraster gr = new clsprepgatraster();
        FeatureSet projectFS = new FeatureSet();
        double[] aw = new double[5];
        string[] awTitles = new string[5] { "LSHigh", "LSMedHigh", "LSMedium", "LSMedLow", "LSLow" };
        double cellSize = 0;
        int rasterRow = 0;
        int rasterCol = 0;
        IRaster startPoint = new Raster();
        IRaster endPoint = new Raster();
        IRaster backlink = new Raster();
        GATGrid backlinkGATRaster = new GATGrid();
        IRaster outAccumRaster = new Raster();
        GATGrid outAccumGATRaster = new GATGrid();
        IRaster outPathRaster = new Raster();
        GATGrid outPathGATRaster = new GATGrid();
        FeatureSet utPathLine = new FeatureSet(FeatureType.Line);
        IFeatureSet fst = new FeatureSet();
        string shapefileSavePath;
        string _surveyPath = string.Empty;
        string lcpaShapeName = string.Empty;
        List<IRaster> mcRasterList = new List<IRaster>();
        clsCreateWeightedRasters c1 = new clsCreateWeightedRasters();
        Randomnumber r1 = new Randomnumber();
        clsMCAssignWeights mcAssign = new clsMCAssignWeights();
        clsCostWeight costW = new clsCostWeight();
        clsBuildDirectory bdir = new clsBuildDirectory();
        string progress = string.Empty;
        
        public void doTheProcess(ToolStripStatusLabel tslStatus, BackgroundWorker tracker, IRaster bounds, string saveLocation, IMap _mapLayer, int currentPass, DataGridView dgvSelectLayers, IRaster utilityCosts, clsMonteCarlo _MC, string progress, ref string outputPathFilename, IRaster additivecosts, ref string backlinkFilename, ref string outputAccumFilename, ProgressChangedEventHandler tracker_ProgressChanged, ref IRaster rasterToConvert, ref string costFileName)
        {
            try
            {
                MC = _MC;
                if (MC.errorCondition == true)
                {
                    return;
                }
                cellSize = bounds.CellHeight;
                clsCreateBackgroundRasters cbrMC = new clsCreateBackgroundRasters();
                tslStatus.Visible = false;
                string path = saveLocation + @"\linesiter\LSProcessing";
                shapefileSavePath = saveLocation + @"\outputPaths.shp";
                utilityCosts.Bounds = bounds.Bounds;
                IRaster utilsCosts = utilityCosts;
                progress = "Creating Weighted Rasters";
                tracker.ReportProgress(10);
                bdir.buildDirectory(path + @"\Pass_" + Convert.ToString(currentPass));
                rasterRow = utilityCosts.NumRows;
                rasterCol = utilityCosts.NumColumns;
                costFileName = saveLocation + @"\costSurfaceRaster.bgd";
                rasterToConvert = Raster.CreateRaster(costFileName, null, bounds.NumColumns, bounds.NumRows, 1, typeof(double), null);
                backlink = cbrMC.saveRaster(path + @"\Pass_" + Convert.ToString(currentPass), @"\backlink", bounds);
                outAccumRaster = cbrMC.saveRaster(path + @"\Pass_" + Convert.ToString(currentPass), @"\outAccumRaster", bounds);
                outPathRaster = cbrMC.saveRaster(path + @"\Pass_" + Convert.ToString(currentPass), @"\outputPathRaster", bounds);
                additiveCosts = cbrMC.saveRaster(saveLocation, @"\additiveCostsRaster", bounds);
                mcAssign.additiveCosts = additiveCosts;
                mcAssign.finalStatOutput = finalStatOutput;
                int newQIDValue = 0;
                progress = "Building Temp Directories";
                tracker.ReportProgress(20);

                foreach (DataGridViewRow dx in dgvSelectLayers.Rows)
                {
                    newQIDValue++;
                    dx.Cells[2].Value = newQIDValue;
                    string convertPath = saveLocation + @"\linesiter\LSProcessing\QuesID_" + Convert.ToString(newQIDValue);

                    string cellValue = Convert.ToString(dx.Cells[0].Value);
                    if (Convert.ToString(dx.Cells[0].Value) != "False")
                    {
                        if (dx.Cells[0].Value != null)
                        {


                            //get feature layer from map interface
                            foreach (Layer lay in _mapLayer.Layers)
                            {


                                if (dx.Cells[1].Value != null)
                                {
                                    if (lay.LegendText == Convert.ToString(dx.Cells[1].Value))
                                    {
                                        clsRasterOps paRaster = new clsRasterOps(_mapLayer);
                                        //check to see if feature layer is shapefile or raster
                                        if (lay.GetType() == typeof(DotSpatial.Controls.MapPointLayer) || lay.GetType() == typeof(DotSpatial.Controls.MapPolygonLayer) || lay.GetType() == typeof(DotSpatial.Controls.MapLineLayer))
                                        {
                                            //if shapefile, convert to raster
                                            //insert shapefile conversion logic
                                            IRaster outputRaster = new Raster();
                                            FeatureSet fs = (FeatureSet)lay.DataSet;
                                            Extent prjExtent = bounds.Extent;
                                            //Extent prjExtent = projectFS.Extent;
                                            string fNameS = convertPath + lay.LegendText + ".bgd";
                                            string fNameR = convertPath + lay.LegendText + ".bgd";
                                            outputRaster = DotSpatial.Analysis.VectorToRaster.ToRaster(fs, prjExtent, cellSize, "FID", fNameS, "", new string[0], null);
                                            paRaster.createPA(outputRaster, fNameR, -1);
                                        }
                                        else if (lay.GetType() == typeof(DotSpatial.Controls.MapRasterLayer))
                                        {
                                            //go directly to boolean raster creation logic
                                            IRaster bRaster = (DotSpatial.Data.Raster)lay.DataSet;
                                            string fNameR = convertPath + lay.LegendText + "PA.bgd";
                                            paRaster.createPA(bRaster, fNameR, -1);
                                        }
                                        break;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("All selected rows for analysis must contain a loaded feature layer.  \n Please verify all rows are assigned a feature layer and restart the process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;

                                }
                            }
                        }
                    }
                }

                progress = "Beginning Monte Carlo Process";
                tracker.ReportProgress(30);

                mcAssign.MCAssignWeights(tslStatus, tracker, backlink, outAccumRaster, outPathRaster, currentPass, MC, dgvSelectLayers, bounds, saveLocation, _mapLayer, progress, ref outputPathFilename, utilityCosts, ref rasterToConvert, ref costFileName);
                additivecosts = mcAssign.additiveCosts;
                finalStatOutput = mcAssign.finalStatOutput;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + "has occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MC.errorCondition = true;
            }
        }
    }
    
}
