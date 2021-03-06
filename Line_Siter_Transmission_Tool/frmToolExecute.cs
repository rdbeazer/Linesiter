﻿using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Symbology;
using DotSpatial.Topology;
using LineSiterSitingTool.MonteCarlo;
using LineSiterSitingTool.Whitebox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace LineSiterSitingTool
{
    public partial class frmToolExecute : Form
    {
        #region classVariables

        private System.Data.DataSet dsQuesSets = new System.Data.DataSet();
        private IMap _mapLayer = null;
        private int currentPass = 1;
        private clsMonteCarlo MC = new clsMonteCarlo();
        private clsprepgatraster gr = new clsprepgatraster();
        private clsRasterOps pa;
        private clsLCPCoords lc = new clsLCPCoords();
        private clsBuildDirectory b1 = new clsBuildDirectory();
        private FeatureSet projectFS = new FeatureSet();
        private double[] aw = new double[5];
        private string[] awTitles = new string[5] { "LSHigh", "LSMedHigh", "LSMedium", "LSMedLow", "LSLow" };
        private double cellSize = 0;
        private string saveLocation;
        private IRaster utilityCosts = new Raster();
        private string startFileName = "";
        private string endFileName = "";
        private IRaster rasterToConvert;
        private string costFileName = "";
        private IRaster outPath;
        private FeatureSet pathLines = new FeatureSet(FeatureType.Line);
        private IFeatureSet fst = new FeatureSet();
        private string shapefileSavePath = string.Empty;
        private IRaster additiveCosts;
        private List<string> finalStatOutput = new List<string>();
        private string _surveyPath = string.Empty;
        private string lcpaShapeName = string.Empty;
        private List<string> headers = new List<string>();
        private List<string> attributes = new List<string>();
        private string process = string.Empty;
        private BackgroundWorker tracker = new BackgroundWorker();
        private BackgroundWorker utCostsWorker = new BackgroundWorker();
        private Stopwatch stopWatch = new Stopwatch();

        #endregion classVariables

        private clsdoTheProcess p1 = new clsdoTheProcess();

        #region Methods

        public frmToolExecute(IMap MapLayer, clsLCPCoords _lc, string _projSavePath, string surveyPath)
        {
            InitializeComponent();
            _mapLayer = MapLayer;
            _surveyPath = surveyPath;
            fillInDataView();
            pa = new clsRasterOps(_mapLayer);
            fillUtilityCombo();
            fillStartEndCombo();
            lc = _lc;
            pathLines.Projection = _mapLayer.Projection;
            if (_projSavePath != string.Empty)
            {
                cboStartEndPoints.Enabled = true;
                txtSaveLocation.Text = _projSavePath;
                saveLocation = _projSavePath;
            }
            p1.additiveCosts = additiveCosts;
        }

        private void fillInDataView()
        {
            try
            {
                dsQuesSets.Clear();
                dsQuesSets.Dispose();
                dgvSelectLayers.DataSource = null;
                DataGridViewCheckBoxColumn dgvcheck = new DataGridViewCheckBoxColumn();
                DataGridViewTextBoxColumn QID = new DataGridViewTextBoxColumn();
                DataGridViewComboBoxColumn fLayers = new DataGridViewComboBoxColumn();
                foreach (Layer lay in _mapLayer.Layers)
                {
                    fLayers.Items.Add(lay.LegendText);
                }
                dgvcheck.HeaderText = "Include";
                fLayers.HeaderText = "FeatureLayer";
                QID.HeaderText = "Question ID";
                dgvSelectLayers.Columns.Add(dgvcheck);
                dgvSelectLayers.Columns.Add(fLayers);
                dgvSelectLayers.Columns.Add(QID);
                fillDataSet(dsQuesSets);
                dgvSelectLayers.DataSource = dsQuesSets.Tables[0];
                dgvSelectLayers.AutoResizeColumns();
            }

            /*catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + "\n has occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/

            catch (System.Data.OleDb.OleDbException oledb)
            {
                Debug.WriteLine(oledb);
                MessageBox.Show("An error has occurred with the survey dataset.  It appears to be in an incompatible format.  Please choose a correctly formatted dataset."/* + "\n" + oledb*/, "Survey Data Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                //this.Close();
                this.Shown += new EventHandler(closeOnStart);
                //return;
            }
        }

        private void closeOnStart(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fillDataSet(System.Data.DataSet dgvDSet)
        {
            string importDataFile;
            string pathLoc = string.Empty;
            string ConnectionString = string.Empty;
            if (_surveyPath != string.Empty)
            {
                FileInfo pathDir = new FileInfo(_surveyPath);
                pathLoc = pathDir.DirectoryName;
            }
            else
            {
                OpenFileDialog dataSetFill = new OpenFileDialog();
                dataSetFill.Title = "Open Question Set for Analysis";
                var result = dataSetFill.ShowDialog();
                if (result != System.Windows.Forms.DialogResult.OK)
                    return;

                bool exists = dataSetFill.CheckFileExists;
                if (exists == true)
                {
                    _surveyPath = dataSetFill.FileName;
                    FileInfo pathDir = new FileInfo(dataSetFill.FileName);
                    pathLoc = pathDir.DirectoryName;
                }
                else
                {
                    MessageBox.Show("A proper questionset file needs to be selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Close();
                }
            }
            ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + pathLoc + @"; Extended Properties=""text;HDR=YES;FMT=Delimited;"";";
            importDataFile = "select * from " + Path.GetFileName(_surveyPath);
            using (OleDbConnection myConnection = new OleDbConnection(ConnectionString))
            {
                myConnection.Open();
                using (OleDbDataAdapter oleDa = new OleDbDataAdapter(importDataFile, ConnectionString))
                {
                    oleDa.Fill(dgvDSet);
                }
            }
        }

        #region fillCombos

        private void fillUtilityCombo()
        {
            foreach (Layer lay in _mapLayer.Layers)
            {
                if (lay.GetType() == typeof(MapRasterLayer))
                {
                    cboSelectUtilityRaster.Items.Add(lay.LegendText);
                }
            }
        }

        private void fillStartEndCombo()
        {
            foreach (Layer lay in _mapLayer.Layers)
            {
                if (lay.GetType() == typeof(MapPointLayer))
                {
                    cboStartEndPoints.Items.Add(lay.LegendText);
                }
            }
        }

        #endregion fillCombos

        private void btnBegin_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboStartEndPoints.Text == "Select Shapefile for Start and End Points")
                {
                    MessageBox.Show("Select the shapefile that has the starting and ending points.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                lblProgress.Text = "Performing analysis... please wait.";
                this.Cursor = Cursors.WaitCursor;
                this.progressbar1.Style = ProgressBarStyle.Marquee;
                this.progressbar1.MarqueeAnimationSpeed = 60;
                stopWatch.Start();

                utCostsWorker.WorkerReportsProgress = true;
                utCostsWorker.WorkerSupportsCancellation = true;
                utCostsWorker.DoWork += new DoWorkEventHandler(utCosts_DoWork);
                utCostsWorker.ProgressChanged += new ProgressChangedEventHandler(utCosts_ProgressChanged);
                utCostsWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(utCosts_RunWorkerCompleted);
                utCostsWorker.RunWorkerAsync();

                this.Cursor = Cursors.Default;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + "\n has occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MC.errorCondition = true;
                this.Close();
                return;
            }
        }

        public void tracker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                for (currentPass = 1; currentPass <= numPasses.Value; currentPass++)
                {
                    if (worker.CancellationPending) return;

                    p1.doTheProcess(tslStatus, worker, utilityCosts, saveLocation, _mapLayer, currentPass, dgvSelectLayers, utilityCosts, MC, additiveCosts, ref rasterToConvert, ref costFileName);

                    if (worker.CancellationPending) return;

                    additiveCosts = p1.additiveCosts;
                    //createAccumCostRaster(outputPathFilename);
                    createMCLCPA();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + "\n has occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MC.errorCondition = true;
                this.Close();
                return;
            }
        }

        private void UpdateClockDisplay()
        {
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}",
                ts.Hours, ts.Minutes, ts.Seconds);
            lblTimeElapsed.Text = "Time Elapsed: " + elapsedTime;
        }

        public void tracker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressbar1.Value = e.ProgressPercentage;
            string newMessage = (string)e.UserState;
            if (newMessage != null)
            {
                lblProgress.Text = newMessage;
            }
            lblProcess.Text = process;
            UpdateClockDisplay();
        }

        public void tracker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                finishingUp();
                stopWatch.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + "\n has occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MC.errorCondition = true;
                this.Close();
                return;
            }
        }

        private void createMCLCPA()
        {
            process = "Beginning Monte Carlo least cost path analysis.";
            clsBuildDirectory bdir = new clsBuildDirectory();
            string passSave = saveLocation + @"\linesiter\LSProcessing";
            bdir.buildDirectory(passSave + @"\Pass_" + Convert.ToString(currentPass));
            //clsWBHost wbHost = new clsWBHost(tslStatus);
            clsGATGridConversions ggc = new clsGATGridConversions();
            clsGATGridConversions mcConvert = new clsGATGridConversions();
            Cursor curs = Cursors.Arrow;
            shapefileSavePath = passSave + @"\Pass_" + Convert.ToString(currentPass) + @"\MCLCPA.shp";
            lcpaShapeName = "Monte Carlo LCPA" + @" Pass_" + Convert.ToString(currentPass);
            IRaster mcCosts = Raster.OpenFile(passSave + @"\Pass_" + Convert.ToString(currentPass) + @"\costweightraster_" + Convert.ToString(currentPass) + ".bgd");
            rasterToConvert = mcCosts;
            mcConvert._rasterToConvert = rasterToConvert;
            mcConvert._statusMessage = "Converting cost raster. ";
            mcConvert.convertToGAT();
            process = "Creating background rasters for pass: " + Convert.ToString(currentPass);
            IRaster mcBacklink = Raster.OpenFile(passSave + @"\Pass_" + Convert.ToString(currentPass) + @"\backlink.bgd");
            IRaster mcOutAccumRaster = Raster.OpenFile(passSave + @"\Pass_" + Convert.ToString(currentPass) + @"\outAccumRaster.bgd");
            IRaster mcOutPathRaster = Raster.OpenFile(passSave + @"\Pass_" + Convert.ToString(currentPass) + @"\outputPathRaster.bgd");
            gr.prepareGATRasters(passSave + @"\Pass_" + Convert.ToString(currentPass), mcBacklink, mcOutAccumRaster, mcOutPathRaster, mcOutPathRaster.Filename);
            string[] mcParaString = new string[6] { startFileName.Substring(0, startFileName.Length - 4) + ".dep", mcCosts.Filename.Substring(0, mcCosts.Filename.Length - 4) + ".dep", mcOutAccumRaster.Filename.Substring(0, mcOutAccumRaster.Filename.Length - 4) + ".dep", mcBacklink.Filename.Substring(0, mcBacklink.Filename.Length - 4) + ".dep", "not specified", "not specified" }; //outputFilename + ".dep", backlinkFilename + ".dep", "not specified", "not specified" };
            string[] mcCostPath = new string[3] { endFileName.Substring(0, endFileName.Length - 4) + ".dep", mcBacklink.Filename.Substring(0, mcBacklink.Filename.Length - 4) + ".dep", mcOutPathRaster.Filename.Substring(0, mcOutPathRaster.Filename.Length - 4) + ".dep" };
            clsLCPGAT mcLCPA = new clsLCPGAT(tslStatus, mcParaString, mcCostPath);
            tracker.ReportProgress(60, "Performing least cost path analysis for pass " + Convert.ToString(currentPass));
            mcLCPA.leastCostPath(tracker);
            string mcOprFilename = mcOutPathRaster.Filename;
            convertCostPathwayToBGD(mcOprFilename);
            IRaster outPath = Raster.OpenFile(mcOprFilename.Substring(0, mcOprFilename.Length - 4) + "new.bgd");
            headers.Add("Pass");
            attributes.Add(Convert.ToString(currentPass));
            ggc._conversionRaster = mcBacklink.Filename.Substring(0, mcBacklink.Filename.Length - 4);
            ggc._gridToConvert = mcBacklink.Filename.Substring(0, mcBacklink.Filename.Length - 4);
            ggc._bnds = mcBacklink;
            ggc.convertBGD();
            //IRaster newBklink = Raster.OpenFile(backlink.Filename.Substring(0, backlink.Filename.Length - 4) + "new.bgd");
            clsCreateLineShapeFileFromRaster clsf = new clsCreateLineShapeFileFromRaster();
            //clsf.createShapefile(outPath, 1, shapefileSavePath, headers, attributes, _mapLayer, "MCLCPA", pathLines);
            process = "Creating least cost path point file for pass " + Convert.ToString(currentPass);
            clsf.createShapefile(outPath, 1, shapefileSavePath, _mapLayer, lcpaShapeName);
            //clsf.createLineFromBacklink(newBklink, shapefileSavePath, headers, attributes, _mapLayer, "MCLCPA", pathLines, lc.startRow, lc.startCol, lc.EndRow, lc.EndCol);
        }

        private void createUTLCPA(BackgroundWorker worker)
        {
            try
            {
                process = "Beginning utility costs pass";
                clsGATGridConversions ggc = new clsGATGridConversions();
                clsGATGridConversions utConvert = new clsGATGridConversions();
                FileInfo utilCosts = new FileInfo(utilityCosts.Filename);
                Cursor curs = Cursors.Arrow;
                clsCreateBackgroundRasters cbrUT = new clsCreateBackgroundRasters();
                shapefileSavePath = saveLocation + @"\UT\utilityCostsLCPA.shp";
                lcpaShapeName = "Utility Costs LCPA";
                b1.buildDirectory(saveLocation + @"\UT");
                utilityCosts.SaveAs(saveLocation + @"\UT\utilCosts.bgd");
                IRaster utilsCosts = Raster.OpenFile(saveLocation + @"\UT\utilCosts.bgd");
                rasterToConvert = Raster.OpenFile(saveLocation + @"\UT\utilCosts.bgd");
                IRaster utBacklink = cbrUT.saveRaster(saveLocation + @"\UT\", "backlink", utilityCosts);
                IRaster utOutAccumRaster = cbrUT.saveRaster(saveLocation + @"\UT\", "outAccumRaster", utilityCosts);
                IRaster utOutPathRaster = cbrUT.saveRaster(saveLocation + @"\UT\", "outPath", utilityCosts);
                gr.prepareGATRasters(saveLocation + @"\UT", utBacklink, utOutAccumRaster, utOutPathRaster, utOutPathRaster.Filename);
                string[] utParaString = new string[6] { startFileName.Substring(0, startFileName.Length - 4) + ".dep", utilsCosts.Filename.Substring(0, utilsCosts.Filename.Length - 4) + ".dep", utOutAccumRaster.Filename.Substring(0, utOutAccumRaster.Filename.Length - 4) + ".dep", utBacklink.Filename.Substring(0, utBacklink.Filename.Length - 4) + ".dep", "not specified", "not specified" };
                string[] utCostPath = new string[3] { endFileName.Substring(0, endFileName.Length - 4) + ".dep", utBacklink.Filename.Substring(0, utBacklink.Filename.Length - 4) + ".dep", utOutPathRaster.Filename.Substring(0, utOutPathRaster.Filename.Length - 4) + ".dep" };
                clsLCPGAT utLCPA = new clsLCPGAT(tslStatus, utParaString, utCostPath);
                utConvert._rasterToConvert = rasterToConvert;
                utConvert._statusMessage = "Converting cost raster. ";
                utConvert.convertToGAT();
                if (worker.CancellationPending) return;

                process = "Creating utility least cost path";
                utLCPA.leastCostPath(worker);
                if (worker.CancellationPending) return;

                string utOprFilename = utOutPathRaster.Filename;
                convertCostPathwayToBGD(utOprFilename);
                IRaster outPath = Raster.OpenFile(utOprFilename.Substring(0, utOprFilename.Length - 4) + "new.bgd");
                headers.Add("Pass");
                attributes.Add("UT");
                ggc._conversionRaster = utBacklink.Filename.Substring(0, utBacklink.Filename.Length - 4);
                ggc._gridToConvert = utBacklink.Filename.Substring(0, utBacklink.Filename.Length - 4);
                ggc._bnds = utBacklink;
                ggc.convertBGD();

                if (worker.CancellationPending) return;

                clsCreateLineShapeFileFromRaster clsf = new clsCreateLineShapeFileFromRaster();
                //clsf.createShapefile(outPath, 1, shapefileSavePath, headers, attributes, _mapLayer, "MCLCPA", pathLines);
                process = "Generating utility costs point shapefile";
                clsf.createShapefile(outPath, 1, shapefileSavePath, _mapLayer, "utLCPA");
                //clsf.createLineFromBacklink(newBklink, shapefileSavePath, headers, attributes, _mapLayer, "MCLCPA", pathLines, lc.startRow, lc.startCol, lc.EndRow, lc.EndCol);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :" + ex + " has occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                worker.CancelAsync();
                this.Close();
                return;
            }
        }

        private void convertCostPathwayToBGD(string oprFileName)
        {
            clsGATGridConversions cpRas = new clsGATGridConversions();
            cpRas._statusMessage = "Converting Cost Path";
            cpRas._bnds = utilityCosts;
            cpRas._gridToConvert = oprFileName.Substring(0, oprFileName.Length - 4);
            cpRas._conversionRaster = oprFileName.Substring(0, oprFileName.Length - 4) + ".dep";
            cpRas.convertBGD();
            outPath = cpRas.conRaster;
        }

        private void btnAlterWeights_Click(object sender, EventArgs e)
        {
            frmWeights frmWeights = new frmWeights();
            frmWeights.ShowDialog();

            if (frmWeights.DialogResult == DialogResult.OK)
            {
                aw[0] = frmWeights.getWeight.LSHigh;
                aw[1] = frmWeights.getWeight.LSMedHigh;
                aw[2] = frmWeights.getWeight.LSMedium;
                aw[3] = frmWeights.getWeight.LSMedLow;
                aw[4] = frmWeights.getWeight.LSLow;
                MC.assignedWeights = aw;
            }
            addWeightsToList();
            btnBegin.Enabled = true;
        }

        private void addWeightsToList()
        {
            int awT = 0;
            finalStatOutput.Add("Weights used in this analysis: ");
            foreach (double a in MC.assignedWeights)
            {
                finalStatOutput.Add(Convert.ToString(awTitles[awT]) + ": " + Convert.ToString(a));
                awT++;
            }
            p1.finalStatOutput = finalStatOutput;
        }

        private void removeProcessingFiles()
        {
            try
            {
                DirectoryInfo df = new DirectoryInfo(saveLocation + @"\linesiter\LSProcessing\");
                foreach (FileInfo file in df.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in df.GetDirectories())
                {
                    dir.Delete(true);
                }
                Directory.Delete(saveLocation + @"\linesiter\LSProcessing\");
            }

            catch (Exception)
            {
                MessageBox.Show("Error: " + "has occured.", "Generic Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }
        }

        private void btnDelWorkSpace_Click(object sender, EventArgs e)
        {
            removeProcessingFiles();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            fbd.ShowDialog();
            txtSaveLocation.Text = fbd.SelectedPath;
            saveLocation = fbd.SelectedPath;
            cboStartEndPoints.Enabled = true;
        }

        private void cboSelectUtilityRaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Layer lay in _mapLayer.Layers)
            {
                if (lay.LegendText == cboSelectUtilityRaster.SelectedItem.ToString())
                {
                    utilityCosts = (IRaster)lay.DataSet;
                    projectFS.Extent = utilityCosts.Extent;
                    cellSize = utilityCosts.CellHeight;
                    picAcpt2.Visible = true;
                }
            }
        }

        private void finishingUp()
        {
            IFeatureSet mcLCPA = new FeatureSet();
            lblProcess.Text = "Adding cost layers to map";
            for (int i = 1; i <= MC.NumPasses; i++)
            {
                mcLCPA = FeatureSet.OpenFile(saveLocation + @"\linesiter\LSProcessing\Pass_" + Convert.ToString(i) + @"\MCLCPA.shp");
                _mapLayer.Layers.Add(mcLCPA).LegendText = "MCLCPA Pass: " + Convert.ToString(i);
            }
            IRaster utLCPA = Raster.OpenFile(saveLocation + @"\UT\outputpathrasternew.bgd");
            finalStatOutput = p1.finalStatOutput;
            lblProcess.Text = "Finishing up...please wait for results window.";
            frmResults result = new frmResults(utLCPA, additiveCosts, utilityCosts, mcLCPA, fst, MC.NumPasses, finalStatOutput, saveLocation);
            result.ShowDialog();
            //_mapLayer.Layers.Add(pathLines);
            utCostsWorker.Dispose();
            tracker.Dispose();
            this.Close();
        }

        private void utCosts_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            createUTLCPA(worker);
        }

        private void utCosts_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            tspProgress.Value = e.ProgressPercentage;
            UpdateClockDisplay();
        }

        private void utCosts_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                MC.NumPasses = (int)numPasses.Value;
                process = "Setting number of passes.";
                MC.errorCondition = false;

                tracker.DoWork += new DoWorkEventHandler(tracker_DoWork);
                tracker.ProgressChanged += new ProgressChangedEventHandler(tracker_ProgressChanged);
                tracker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(tracker_RunWorkerCompleted);
                tracker.WorkerSupportsCancellation = true;
                tracker.WorkerReportsProgress = true;
                tracker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + " \n has occured." + "\n" + "Current Pass: " + Convert.ToString(currentPass), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MC.errorCondition = true;
            }
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {

            if (tracker.WorkerSupportsCancellation)
            {
                tracker.CancelAsync();
            }
            if (utCostsWorker.WorkerSupportsCancellation)
            {
                utCostsWorker.CancelAsync();
            }

            this.Close();
        }

        #endregion Methods

        #region StartandEndPoints

        private void cboStartEndPoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string[] rasOps = new string[1];
                Cursor = Cursors.WaitCursor;
                clsCreateStartEndRasters cser = new clsCreateStartEndRasters();
                string selectedItem = Convert.ToString(cboStartEndPoints.SelectedItem);
                IRaster startPoint = Raster.CreateRaster(saveLocation + @"\startPoint.bgd", null, utilityCosts.NumColumns, utilityCosts.NumRows, 1, typeof(int), rasOps);
                IRaster endPoint = Raster.CreateRaster(saveLocation + @"\endPoint.bgd", null, utilityCosts.NumColumns, utilityCosts.NumRows, 1, typeof(int), rasOps);

                cser.startPoint = startPoint;
                cser.endPoint = endPoint;

                cser.createRasters(_mapLayer, selectedItem, lc, utilityCosts);
                startFileName = cser.startPoint.Filename;
                endFileName = cser.endPoint.Filename;
                picStartEnd.Visible = true;
                Cursor = Cursors.Default;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Convert.ToString(ex) + " /n has occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
        }

        #endregion StartandEndPoints


    }
}