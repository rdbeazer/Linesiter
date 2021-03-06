﻿using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Symbology;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LineSiterSitingTool
{
    public partial class frmCreateUtilityRaster : Form
    {
        private IMap _MW = null;
        private clsRasterOps paRaster;
        private string progress = string.Empty;

        public frmCreateUtilityRaster(IMap mapLayer, string projSavePath)
        {
            InitializeComponent();
            _MW = mapLayer;
            paRaster = new clsRasterOps(_MW);
            loadRtb();
            //if (projSavePath != string.Empty)
            //{
            //    txtSaveLocation.Text = projSavePath;
            //}
        }

        private void loadRtb()
        {
            try
            {
                rtbHelpBox.SelectionFont = new Font("Arial", 12f, FontStyle.Bold);
                rtbHelpBox.AppendText("Create Utility Cost Raster \n \n");
                rtbHelpBox.SelectionFont = new Font("Arial", 10f, FontStyle.Regular);
                rtbHelpBox.AppendText("This process creates a cost raster from shapefiles loaded in the project.  ");
                rtbHelpBox.AppendText("To create the raster, check the box next to the relevant layers and select the cost ");
                rtbHelpBox.AppendText("cost attribute column you wish to apply to the resulting raster.  NOTE: ***  ");
                rtbHelpBox.AppendText("Relevant shapefiles must have a cost attribute in order to create the utility cost raster. *** ");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + " has occured.", "Generic Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnOpenBounds_Click(object sender, EventArgs e)
        {
            OpenFileDialog opBRas = new OpenFileDialog();
            opBRas.Filter = "DotSpatial Raster (*.bgd) | *.bgd";
            opBRas.ShowDialog();
            txtOpenBoundsRaster.Text = opBRas.FileName;
        }

        private void tracker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            /*  int percent = (int)(((double)ProgressBar1.Value / (double)ProgressBar1.Maximum) * 100);
              ProgressBar1.CreateGraphics().DrawString(percent.ToString() + "%", new Font("Arial", (float)8.25, FontStyle.Regular),
              Brushes.Black, new PointF(ProgressBar1.Width / 2 - 10, ProgressBar1.Height / 2 - 7));
              this.ProgressBar1.Size = new System.Drawing.Size(224,23);*/

            this.ProgressBar1.Value = e.ProgressPercentage;
            lblprogress.Text = progress;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            SaveFileDialog svRas = new SaveFileDialog();
            svRas.Filter = "DotSpatial Raster (*.bgd) | *.bgd";
            svRas.ShowDialog();
            txtSaveLocation.Text = svRas.FileName;
        }

        private void btnBegin_Click(object sender, EventArgs e)
        {
            bool okToProceed = false;
            FileInfo utFile = new FileInfo(txtSaveLocation.Text);
            BackgroundWorker tracker = new BackgroundWorker();
            tracker.WorkerSupportsCancellation = true;
            tracker.WorkerReportsProgress = true;
            tracker.ProgressChanged += new ProgressChangedEventHandler(tracker_ProgressChanged);
            try
            {
                string savePath = txtSaveLocation.Text;
                if ((savePath == null) || (savePath == "Please Select a Location and Filename"))
                {
                    MessageBox.Show("Please Select a Save Location.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //this.lblprogress.Text = "Performing Process...Please Wait.";
                progress = "Begining process..please wait...";
                int cellSize = (int)txtCellSize.Value;
                IRaster boundingRaster = Raster.OpenFile(txtOpenBoundsRaster.Text);
                Extent boundingExtent = boundingRaster.Extent;
                int rasterCol = boundingRaster.NumColumns, rasterRow = boundingRaster.NumRows;
                frmAssignLayerCosts frmalc = new frmAssignLayerCosts(_MW);
                frmalc.ShowDialog();
                okToProceed = frmalc.okToProceed;
                if (okToProceed == false)
                {
                    this.Close();
                    return;
                }
                List<IRaster> lRaster = new List<IRaster>();
                clsRasterOps rasterMA = new clsRasterOps(_MW);
                string[] ras1 = new string[1];
                IRaster fOutputRaster = Raster.CreateRaster(savePath, null, rasterCol, rasterRow, 1, typeof(double), ras1);
                fOutputRaster.CellHeight = boundingRaster.CellHeight;
                fOutputRaster.CellWidth = boundingRaster.CellWidth;
                fOutputRaster.Projection = _MW.Projection;
                fOutputRaster.Save();
                tracker.ReportProgress(30, "Raster template saved");
                IRaster outputRaster = new Raster();
                progress = "Creating temp directory";
                //if (!Directory.Exists(@"c:\temp\linesiter\LSProcessing\UTRasters\"))
                if (!Directory.Exists(txtSaveLocation.Text))
                {
                    //Directory.CreateDirectory(@"c:\temp\linesiter\LSProcessing\UTRasters\");
                    //Directory.CreateDirectory(txtSaveLocation.Text);
                    Directory.CreateDirectory(utFile.DirectoryName);
                }

                foreach (KeyValuePair<string, string> dL in frmalc.layerList)
                {
                    foreach (Layer lay in _MW.Layers)
                    {
                        int i = 1;
                        double inProgress = (i / _MW.Layers.Count) * 100;
                        tracker.ReportProgress(Convert.ToInt32(inProgress), "Processing layer " + lay.LegendText);
                        i++;
                        if (lay.LegendText == dL.Key)
                        {
                            MapRasterLayer mras = lay as MapRasterLayer;
                            if (mras == null)
                            {
                                FeatureSet fs = (FeatureSet)lay.DataSet;
                                string fNameS = utFile.DirectoryName + @"\" + lay.LegendText + ".bgd";

                                outputRaster = DotSpatial.Analysis.VectorToRaster.ToRaster(fs, boundingExtent, cellSize, dL.Value, fNameS, "", ras1, null);
                                outputRaster.Bounds = boundingRaster.Bounds;
                                paRaster.createPA(outputRaster, utFile.DirectoryName + @"\" + lay.LegendText + ".bgd", -98);
                                //create method to add rasters together
                                IRaster sendRaster = Raster.Open(utFile.DirectoryName + @"\" + lay.LegendText + "pa.bgd");
                                sendRaster.Projection = _MW.Projection;
                                sendRaster.Save();
                                fOutputRaster.Bounds = sendRaster.Bounds;
                                fOutputRaster.Projection = _MW.Projection;
                                fOutputRaster.Save();

                                //MessageBox.Show("Raster: " + (string)dL.Key + " is loaded.  Its row and column count is: " + Convert.ToString(sendRaster.NumRows) + " " + Convert.ToString(sendRaster.NumColumns) + ".", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lRaster.Add(sendRaster);
                            }
                            else if (mras != null)
                            {
                                lRaster.Add(mras.DataSet);
                            }
                        }
                    }
                }
                rasterMA.rasterAddition(lRaster, fOutputRaster);
                //paRaster.removeProcessingFiles();
                _MW.Layers.Add(fOutputRaster);
                MessageBox.Show("Process Complete.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}