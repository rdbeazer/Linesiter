using System;
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

namespace nx09SitingTool
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
        clsRasterOps pa;
        clsLCPCoords lc = new clsLCPCoords();
        clsprepgatraster gr = new clsprepgatraster();
        FeatureSet projectFS = new FeatureSet();
        double[] aw = new double[5];
        string[] awTitles = new string[5] { "LSHigh", "LSMedHigh", "LSMedium", "LSMedLow", "LSLow" };
        double cellSize = 0;
        int rasterRow = 0;
        int rasterCol = 0;

        //IRaster bounds = new Raster();
        IRaster startPoint = new Raster();
        string startFileName = "";
        IRaster endPoint = new Raster();
        string endFileName = "";
        string backlinkFilename = "";
        IRaster backlink = new Raster();
        GATGrid backlinkGATRaster = new GATGrid();
        string outputAccumFilename = "";
        IRaster outAccumRaster = new Raster();
        GATGrid outAccumGATRaster = new GATGrid();
        string outputPathFilename = "";
        IRaster outPathRaster = new Raster();
        GATGrid outPathGATRaster = new GATGrid();
        string[] paraString;
        //IRaster rasterToConvert;
        string costFileName = "";
        string currentQuesPath = "";
        IRaster outPath;
        FeatureSet pathLines = new FeatureSet(FeatureType.Line);
        FeatureSet utPathLine = new FeatureSet(FeatureType.Line);
        IFeatureSet fst = new FeatureSet();
        string shapefileSavePath;
        string additveCostsFilePath;
        //IRaster additiveCosts;
        int timesHit2 = 1;
        //List<string> finalStatOutput = new List<string>();
        string _surveyPath = string.Empty;
        string lcpaShapeName = string.Empty;
        IFeatureSet utilLCPA;
        List<IRaster> mcRasterList = new List<IRaster>();
        //string progress = string.Empty;
        int temp3 = 0;


        //private System.Windows.Forms.NumericUpDown numPasses;
        //private System.Windows.Forms.DataGridView dgvSelectLayers;
        private System.Windows.Forms.ProgressBar progressbar1;
        private System.Windows.Forms.Label lblProgress;
        clscreateWeightedRaster c1 = new clscreateWeightedRaster();
        Randomnumber r1 = new Randomnumber();
        clsProcess1 pr = new clsProcess1();
        clsCostWeight c2 = new clsCostWeight();
        clsBuildDirectory b1 = new clsBuildDirectory();
        string progress = string.Empty;


        public void doTheProcess(ToolStripStatusLabel tslStatus, BackgroundWorker tracker, IRaster bounds, string saveLocation, IMap _mapLayer, int currentPass, DataGridView dgvSelectLayers, IRaster utilityCosts, clsMonteCarlo _MC, string progress, ref string outputPathFilename, IRaster additivecosts, clsBuildDirectory _b1, ref string backlinkFilename, ref string outputAccumFilename, ProgressChangedEventHandler tracker_ProgressChanged, ref IRaster rasterToConvert, ref string costFileName)
        {

            tracker.WorkerSupportsCancellation = true;
            tracker.WorkerReportsProgress = true;
            tracker.ProgressChanged += new ProgressChangedEventHandler(tracker_ProgressChanged);
            clsBuildDirectory b1 = new clsBuildDirectory();
            try
            {
                MC = _MC;
                b1 = _b1;
                tslStatus.Visible = false;
                string path = saveLocation + @"\linesiter\LSProcessing";
                shapefileSavePath = saveLocation + @"\outputPaths.shp";
                lcpaShapeName = "Monte Carlo LCPA";
                //tslPass.Text = "Current Monte Carlo Pass: " + Convert.ToString(currentPass);
                utilityCosts.Bounds = bounds.Bounds;
                IRaster utilsCosts = utilityCosts;
                progress = "Creating Weighted Rasters";
                tracker.ReportProgress(10);

                b1.buildDirectory(path + @"\Pass_" + Convert.ToString(currentPass));
                rasterRow = utilityCosts.NumRows;
                rasterCol = utilityCosts.NumColumns;
                costFileName = saveLocation + @"\costSurfaceRaster.bgd";
                additveCostsFilePath = saveLocation + @"\additveCostsRaster.bgd";
                backlinkFilename = path + @"\Pass_" + Convert.ToString(currentPass) + @"\backlink";
                outputAccumFilename = path + @"\Pass_" + Convert.ToString(currentPass) + @"\outAccumRaster";
                outputPathFilename = path + @"\Pass_" + Convert.ToString(currentPass) + @"\outputPathRaster";
                rasterToConvert = Raster.CreateRaster(costFileName, null, bounds.NumColumns, bounds.NumRows, 1, typeof(double), null);
                backlink = Raster.CreateRaster(backlinkFilename + ".bgd", null, bounds.NumColumns, bounds.NumRows, 1, typeof(float), null);
                backlink.Bounds = bounds.Bounds;
                backlink.Projection = bounds.Projection;
                backlink.Save();
                outAccumRaster = Raster.CreateRaster(outputAccumFilename + ".bgd", null, bounds.NumColumns, bounds.NumRows, 1, typeof(float), null);
                outAccumRaster.Bounds = bounds.Bounds;
                outAccumRaster.Projection = bounds.Projection;
                outAccumRaster.Save();
                outPathRaster = Raster.CreateRaster(outputPathFilename + ".bgd", null, bounds.NumColumns, bounds.NumRows, 1, typeof(float), null);
                outPathRaster.Bounds = bounds.Bounds;
                outPathRaster.Projection = bounds.Projection;

                outPathRaster.Save();

                pathLines.Projection = _mapLayer.Projection;
                pathLines.SaveAs(shapefileSavePath, true);
                additiveCosts = Raster.CreateRaster(additveCostsFilePath, null, bounds.NumColumns, bounds.NumRows, 1, typeof(float), null);
                additiveCosts.Bounds = bounds.Bounds;
                additiveCosts.Projection = _mapLayer.Projection;
                additiveCosts.Save();
                pr.additiveCosts = additiveCosts;
                pr.finalStatOutput = finalStatOutput;
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
                                            Extent prjExtent = projectFS.Extent;
                                            string fNameS = convertPath + lay.LegendText + ".bgd";
                                            string fNameR = convertPath + lay.LegendText + ".bgd";
                                            outputRaster = DotSpatial.Analysis.VectorToRaster.ToRaster(fs, prjExtent, cellSize, "FID", fNameS, "", new string[0], null);
                                            paRaster.createPA(outputRaster, fNameR, -1);
                                            //MessageBox.Show("File is a shapefile of type: " + Convert.ToString(lay.GetType()), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        else if (lay.GetType() == typeof(DotSpatial.Controls.MapRasterLayer))
                                        {
                                            //go directly to boolean raster creation logic
                                            IRaster bRaster = (DotSpatial.Data.Raster)lay.DataSet;
                                            string fNameR = convertPath + lay.LegendText + "PA.bgd";
                                            paRaster.createPA(bRaster, fNameR, -1);
                                            //MessageBox.Show("File is a shapefile of type: " + Convert.ToString(lay.GetType()), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        break;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("All selected rows for analysis must contain a loaded feature layer.  \n Please verify all rows are assigned a feature layer and restart the process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    pathLines.DataTable.Columns.Remove("pass");
                                    return;

                                }
                            }
                        }
                    }
                }


                progress = "Beginning Monte Carlo Process";
                tracker.ReportProgress(30);

                pr.clsprocess1(tslStatus, tracker, backlink, outAccumRaster, outPathRaster, currentPass, MC, dgvSelectLayers, bounds, saveLocation, _mapLayer, progress, ref outputPathFilename, utilityCosts, _b1, ref rasterToConvert, ref costFileName);
                additivecosts = pr.additiveCosts;
                finalStatOutput = pr.finalStatOutput;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + "has occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
    }
}
