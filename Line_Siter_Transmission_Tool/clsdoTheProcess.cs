using DotSpatial.Analysis;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Symbology;
using LineSiterSitingTool.MonteCarlo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace LineSiterSitingTool
{
    internal class clsdoTheProcess
    {
        public IRaster additiveCosts { get; set; }

        public List<string> finalStatOutput { get; set; }

        private clsMonteCarlo MC = new clsMonteCarlo();
        private double cellSize = 0;
        private int rasterRow = 0;
        private int rasterCol = 0;
        private IRaster backlink = new Raster();
        private IRaster outAccumRaster = new Raster();
        private IRaster outPathRaster = new Raster();
        private string shapefileSavePath;
        private clsMCAssignWeights mcAssign = new clsMCAssignWeights();
        private clsBuildDirectory bdir = new clsBuildDirectory();

        public void doTheProcess(ToolStripStatusLabel tslStatus, 
            BackgroundWorker worker, 
            IRaster bounds, 
            string saveLocation, 
            IMap _mapLayer, 
            int currentPass, 
            DataGridView dgvSelectLayers, 
            IRaster utilityCosts, 
            clsMonteCarlo _MC, 
            IRaster additivecosts, 
            ref IRaster rasterToConvert, 
            ref string costFileName)
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

                worker.ReportProgress(10, "Creating Weighted Rasters");
                bdir.buildDirectory(path + @"\Pass_" + Convert.ToString(currentPass));
                rasterRow = utilityCosts.NumRows;
                rasterCol = utilityCosts.NumColumns;
                costFileName = path + @"\Pass_" + Convert.ToString(currentPass) + @"\costSurfaceRaster.bgd";
                rasterToConvert = Raster.CreateRaster(costFileName, null, bounds.NumColumns, bounds.NumRows, 1, typeof(double), null);
                backlink = cbrMC.saveRaster(path + @"\Pass_" + Convert.ToString(currentPass), @"\backlink", bounds);
                outAccumRaster = cbrMC.saveRaster(path + @"\Pass_" + Convert.ToString(currentPass), @"\outAccumRaster", bounds);
                outPathRaster = cbrMC.saveRaster(path + @"\Pass_" + Convert.ToString(currentPass), @"\outputPathRaster", bounds);
                additiveCosts = cbrMC.saveRaster(saveLocation, @"\additiveCostsRaster", bounds);
                mcAssign.additiveCosts = additiveCosts;
                mcAssign.finalStatOutput = finalStatOutput;
                int newQIDValue = 0;
                worker.ReportProgress(20, "Building Temp Directories");

                foreach (DataGridViewRow dx in dgvSelectLayers.Rows)
                {
                    if (worker.CancellationPending) return;

                    newQIDValue++;
                    dx.Cells[2].Value = newQIDValue;
                    string convertPath = saveLocation + @"\linesiter\LSProcessing\QuesID_" + Convert.ToString(newQIDValue);

                    string cellValue = Convert.ToString(dx.Cells[0].Value);
                    if (cellValue != "False")
                    {
                        if (dx.Cells[0].Value != null)
                        {
                            //get feature layer from map interface
                            foreach (Layer lay in _mapLayer.Layers)
                            {
                                if (worker.CancellationPending) return;

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
                                            outputRaster = VectorToRaster.ToRaster(fs, prjExtent, cellSize, "FID", fNameS, "", new string[0], null);
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

                worker.ReportProgress(30,"Beginning Monte Carlo Process");

                string mcOutputPathFilename = outPathRaster.Filename;
                mcAssign.MCAssignWeights(tslStatus, worker, backlink, outAccumRaster, outPathRaster, currentPass, MC, dgvSelectLayers, bounds, saveLocation, _mapLayer, ref mcOutputPathFilename, utilityCosts);
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