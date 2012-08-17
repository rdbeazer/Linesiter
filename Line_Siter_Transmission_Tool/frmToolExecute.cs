using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows;


namespace LineSiterSitingTool
{
    public partial class frmToolExecute : Form
    {
        #region classVariables
        private System.Data.DataSet dsQuesSets = new System.Data.DataSet();
        IMap _mapLayer = null;
        int currentPass = 1;
        clsMonteCarlo MC = new clsMonteCarlo();
        clsprepgatraster gr = new clsprepgatraster();
        clsRasterOps pa;
        clsLCPCoords lc = new clsLCPCoords();
        clsBuildDirectory b1 = new clsBuildDirectory();
        FeatureSet projectFS = new FeatureSet();
        double[] aw = new double[5];
        string[] awTitles = new string[5] { "LSHigh", "LSMedHigh", "LSMedium", "LSMedLow", "LSLow" };
        double cellSize = 0;
        string saveLocation;
        IRaster utilityCosts = new Raster();
        string startFileName = "";
        string endFileName = "";
        IRaster rasterToConvert;
        string costFileName = "";
        IRaster outPath;
        FeatureSet pathLines = new FeatureSet(FeatureType.Line);
        IFeatureSet fst = new FeatureSet();
        string shapefileSavePath = string.Empty;
        IRaster additiveCosts;
        List<string> finalStatOutput = new List<string>();
        string _surveyPath = string.Empty;
        string lcpaShapeName = string.Empty;
        List<string> headers = new List<string>();
        List<string> attributes = new List<string>();
        string progress = string.Empty;
        string process = string.Empty;
        BackgroundWorker tracker = new BackgroundWorker();
        Stopwatch stopWatch = new Stopwatch();
        #endregion

        clsdoTheProcess p1 = new clsdoTheProcess();

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
                dataSetFill.ShowDialog();
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
            OleDbConnection myConnection = new OleDbConnection(ConnectionString);
            myConnection.Open();
            OleDbDataAdapter oleDa = new OleDbDataAdapter(importDataFile, ConnectionString);
            oleDa.Fill(dgvDSet);
            myConnection.Close();
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
        #endregion



        private void btnBegin_Click(object sender, EventArgs e)
        {
            try
            {

                if (cboStartEndPoints.Text == "Select Shapefile for Start and End Points")
                {
                    MessageBox.Show("Select the shapefile that has the starting and ending points.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                progress = "Performing analysis...please wait.";
                this.Cursor = Cursors.WaitCursor;
                this.progressbar1.Style = ProgressBarStyle.Marquee;
                this.progressbar1.MarqueeAnimationSpeed = 60;
                stopWatch.Start();
                utCostLine();

                tracker.DoWork += new DoWorkEventHandler(tracker_DoWork);
                tracker.ProgressChanged += new ProgressChangedEventHandler(tracker_ProgressChanged);
                tracker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(tracker_RunWorkerCompleted);
                tracker.WorkerSupportsCancellation = true;
                tracker.WorkerReportsProgress = true;

                MC.NumPasses = (int)numPasses.Value;
                process = "Setting number of passes.";
                MC.errorCondition = false;
                tracker.ReportProgress(5);
                tracker.RunWorkerAsync();
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
                    p1.doTheProcess(tslStatus, worker, utilityCosts, saveLocation, _mapLayer, currentPass, dgvSelectLayers, utilityCosts, MC, progress, additiveCosts, ref rasterToConvert, ref costFileName);
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

        public void tracker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressbar1.Value = e.ProgressPercentage;
            lblProgress.Text = progress;
            lblProcess.Text = process;
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}",
                ts.Hours, ts.Minutes, ts.Seconds);
            lblTimeElapsed.Text = "Time Elapsed: " + elapsedTime;
        }

        public void tracker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                //lblProgress.Text = "Finishing Up.";
                finishingUp();
                stopWatch.Stop();
                //tslStatus.Visible = true;
                //tslStatus.Text = "Finishing Up";
                //tracker.ReportProgress(100);
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
            gr.prepareGATRasters(passSave + @"\Pass_" + Convert.ToString(currentPass), curs, mcBacklink, mcOutAccumRaster, mcOutPathRaster, mcOutPathRaster.Filename);
            string[] mcParaString = new string[6] { startFileName.Substring(0, startFileName.Length - 4) + ".dep", mcCosts.Filename.Substring(0, mcCosts.Filename.Length - 4) + ".dep", mcOutAccumRaster.Filename.Substring(0, mcOutAccumRaster.Filename.Length - 4) + ".dep", mcBacklink.Filename.Substring(0, mcBacklink.Filename.Length - 4) + ".dep", "not specified", "not specified" }; //outputFilename + ".dep", backlinkFilename + ".dep", "not specified", "not specified" };
            string[] mcCostPath = new string[3] { endFileName.Substring(0, endFileName.Length - 4) + ".dep", mcBacklink.Filename.Substring(0, mcBacklink.Filename.Length - 4) + ".dep", mcOutPathRaster.Filename.Substring(0, mcOutPathRaster.Filename.Length - 4) + ".dep" };
            clsLCPGAT mcLCPA = new clsLCPGAT(tslStatus, mcParaString, mcCostPath);
            process = "Performing least cost path analysis for pass " + Convert.ToString(currentPass);
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
                gr.prepareGATRasters(saveLocation + @"\UT", curs, utBacklink, utOutAccumRaster, utOutPathRaster, utOutPathRaster.Filename);
                string[] utParaString = new string[6] { startFileName.Substring(0, startFileName.Length - 4) + ".dep", utilsCosts.Filename.Substring(0, utilsCosts.Filename.Length - 4) + ".dep", utOutAccumRaster.Filename.Substring(0, utOutAccumRaster.Filename.Length - 4) + ".dep", utBacklink.Filename.Substring(0, utBacklink.Filename.Length - 4) + ".dep", "not specified", "not specified" };
                string[] utCostPath = new string[3] { endFileName.Substring(0, endFileName.Length - 4) + ".dep", utBacklink.Filename.Substring(0, utBacklink.Filename.Length - 4) + ".dep", utOutPathRaster.Filename.Substring(0, utOutPathRaster.Filename.Length - 4) + ".dep" };
                clsLCPGAT utLCPA = new clsLCPGAT(tslStatus, utParaString, utCostPath);
                utConvert._rasterToConvert = rasterToConvert;
                utConvert._statusMessage = "Converting cost raster. ";
                utConvert.convertToGAT();
                process = "Creating utility least cost path";
                utLCPA.leastCostPath(worker);
                string utOprFilename = utOutPathRaster.Filename;
                convertCostPathwayToBGD(utOprFilename);
                IRaster outPath = Raster.OpenFile(utOprFilename.Substring(0, utOprFilename.Length - 4) + "new.bgd");
                headers.Add("Pass");
                attributes.Add("UT");
                ggc._conversionRaster = utBacklink.Filename.Substring(0, utBacklink.Filename.Length - 4);
                ggc._gridToConvert = utBacklink.Filename.Substring(0, utBacklink.Filename.Length - 4);
                ggc._bnds = utBacklink;
                ggc.convertBGD();
                clsCreateLineShapeFileFromRaster clsf = new clsCreateLineShapeFileFromRaster();
                //clsf.createShapefile(outPath, 1, shapefileSavePath, headers, attributes, _mapLayer, "MCLCPA", pathLines);
                process = "Generating utility costs point shapefile";
                clsf.createShapefile(outPath, 1, shapefileSavePath, _mapLayer, "utLCPA");
                //clsf.createLineFromBacklink(newBklink, shapefileSavePath, headers, attributes, _mapLayer, "MCLCPA", pathLines, lc.startRow, lc.startCol, lc.EndRow, lc.EndCol);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error :" + ex + " has occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                mcLCPA = FeatureSet.OpenFile(saveLocation +  @"\linesiter\LSProcessing\Pass_" + Convert.ToString(i) + @"\MCLCPA.shp");
                _mapLayer.Layers.Add(mcLCPA).LegendText = "MCLCPA Pass: " + Convert.ToString(i);
            }
            IRaster utLCPA = Raster.OpenFile(saveLocation + @"\UT\outputpathrasternew.bgd");
            finalStatOutput = p1.finalStatOutput;
            lblProcess.Text = "Finishing up...please wait for results window.";
            frmResults result = new frmResults(utLCPA, additiveCosts, utilityCosts, mcLCPA, fst, MC.NumPasses, finalStatOutput, saveLocation);
            result.ShowDialog();
            //_mapLayer.Layers.Add(pathLines);
            this.Close();
        }

        private void utCostLine()
        {
            try
            {
                BackgroundWorker utCosts = new BackgroundWorker();
                utCosts.WorkerReportsProgress = true;
                utCosts.WorkerSupportsCancellation = false;
                utCosts.DoWork += new DoWorkEventHandler(utCosts_DoWork);
                utCosts.ProgressChanged += new ProgressChangedEventHandler(utCosts_ProgressChanged);
                utCosts.RunWorkerCompleted += new RunWorkerCompletedEventHandler(utCosts_RunWorkerCompleted);
                utCosts.RunWorkerAsync();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + " \n has occured." + "\n" + "Current Pass: " + Convert.ToString(currentPass), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            currentPass = 0;
        }



        private void utCosts_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            createUTLCPA(worker);
            worker.Dispose();

        }

        private void utCosts_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            tspProgress.Value = e.ProgressPercentage;
        }

        private void utCosts_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + " \n has occured." + "\n" + "Current Pass: " + Convert.ToString(currentPass), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MC.errorCondition = true;
            }
        }

        #endregion


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

        #endregion

    }
}