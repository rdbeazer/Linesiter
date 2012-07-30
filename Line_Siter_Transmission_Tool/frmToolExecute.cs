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
        IRaster rasterToConvert;
        string costFileName = "";
        IRaster outPath;
        FeatureSet pathLines = new FeatureSet(FeatureType.Line);
        FeatureSet lcpPoints = new FeatureSet(FeatureType.Point);
        FeatureSet utPathLine = new FeatureSet(FeatureType.Line);
        IFeatureSet fst = new FeatureSet();
        string shapefileSavePath = string.Empty;
        IRaster additiveCosts;
        int timesHit2 = 1;
        List<string> finalStatOutput = new List<string>();
        string _surveyPath = string.Empty;
        string lcpaShapeName = string.Empty;
        IFeatureSet utilLCPA;
        List<IRaster> mcRasterList = new List<IRaster>();
        List<string> headers = new List<string>();
        List<string> attributes = new List<string>();
        string progress = string.Empty;
        //BackgroundWorker worker = new BackgroundWorker();
        Random random = new Random();
        BackgroundWorker tracker = new BackgroundWorker();
        #endregion

        clsdoTheProcess p1 = new clsdoTheProcess();
        clsBuildDirectory _b1 = new clsBuildDirectory();
        clsProcess1 p2 = new clsProcess1();
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
                MessageBox.Show("An error has occurred with the survey dataset.  It appears to be in an incompatible format.  Please choose a correctly formatted dataset."/* + "\n" + oledb*/, "Survey Data Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                //this.Close();
                this.Shown += new EventHandler (closeOnStart);
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

           
                MC.NumPasses = (int)numPasses.Value;
                MC.errorCondition = false;
                MC.passType = "Utility";
                utCostLine();
                MC.passType = "Monte Carlo";
               this.lblProgress.Text = "Performing Process...Please Wait.";
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
           tracker.WorkerSupportsCancellation = true;
           tracker.WorkerReportsProgress = true; 
            p2.progress(e.ProgressPercentage, progressbar1, tracker);
            
       }
        
       private void createAccumCostRaster(string outputPathFilename)
        {
            try
            {
                BackgroundWorker bkwork = new BackgroundWorker();
                 paraString = new string[6] { startFileName + ".dep", costFileName.Substring(0, costFileName.Length - 4) + ".dep", outputAccumFilename + ".dep", backlinkFilename + ".dep", "not specified", "not specified" }; //outputFilename + ".dep", backlinkFilename + ".dep", "not specified", "not specified" };
                bkwork.WorkerReportsProgress = true;
                bkwork.WorkerSupportsCancellation = false;
                bkwork.DoWork += new DoWorkEventHandler(bkwork_DoWork);
                //bkwork.ProgressChanged += new ProgressChangedEventHandler(bkwork_ProgressChanged);
                bkwork.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bkwork_RunWorkerCompleted);
                bkwork.RunWorkerAsync();
                //bkwork.Dispose();

            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + "has occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            } 
        }

        private void bkwork_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            newWork(worker, ref outputPathFilename,currentPass);
        }

        private void newWork(BackgroundWorker worker, ref string outputPathFilename, int currentPass)
        {
            try
            {
                clsWBHost wbHost = new clsWBHost(tslStatus);
                clsGATGridConversions ggc = new clsGATGridConversions();
                GISTools.CostAccumulation ac = new GISTools.CostAccumulation();
                GISTools.CostPathway cp = new GISTools.CostPathway();
                clsGATGridConversions utConvert = new clsGATGridConversions();
                utConvert._rasterToConvert = rasterToConvert;
                utConvert._statusMessage = "Converting cost raster. ";
                utConvert.convertToGAT();
                ac.Initialize(wbHost);
                ac.Execute(paraString, worker);
                string[] costPath = new string[3] {endFileName.Substring(0, endFileName.Length - 4) + ".dep", backlink.Filename.Substring(0, backlink.Filename.Length - 4) + ".dep", outputPathFilename + ".dep" };
                string bk = backlinkFilename;
                cp.Initialize(wbHost);
                cp.Execute(costPath, worker);
                convertCostPathwayToBGD();
                IRaster outPath = Raster.OpenFile(outputPathFilename + "new.bgd");
                headers.Add("Pass");
                attributes.Add(Convert.ToString(currentPass));
                ggc._conversionRaster = backlink.Filename.Substring(0, backlink.Filename.Length - 4);
                ggc._gridToConvert = backlink.Filename.Substring(0, backlink.Filename.Length - 4);
                ggc._bnds = backlink;
                ggc.convertBGD();
                IRaster newBklink = Raster.OpenFile(backlink.Filename.Substring(0, backlink.Filename.Length - 4) + "new.bgd");
                clsCreateLineShapeFileFromRaster clsf = new clsCreateLineShapeFileFromRaster();
                //clsf.createShapefile(outPath, 1, shapefileSavePath, headers, attributes, _mapLayer, "MCLCPA", pathLines);
                clsf.createShapefile(outPath, 1, shapefileSavePath, headers, attributes, _mapLayer, "MCLCPA", lcpPoints);
                //clsf.createLineFromBacklink(newBklink, shapefileSavePath, headers, attributes, _mapLayer, "MCLCPA", pathLines, lc.startRow, lc.startCol, lc.EndRow, lc.EndCol);
                shapefileSavePath = saveLocation + @"\MCLCPA.shp";
                _mapLayer.Layers.Add(lcpPoints);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error :" + ex + " has occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
        }

        private void bkwork_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            tspProgress.Value = e.ProgressPercentage;

        }

        private void bkwork_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            try
            {
                finishingUp();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + " \n has occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

        }

        private void convertCostPathwayToBGD()
        {
            clsGATGridConversions cpRas = new clsGATGridConversions();
            cpRas._statusMessage = "Converting Cost Path";
            cpRas._bnds = utilityCosts;
            cpRas._gridToConvert = outputPathFilename;
            cpRas._conversionRaster = outputPathFilename;
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

        private void frmToolExecute_Load(object sender, EventArgs e)
        {
            this.Text = "Line Siter  " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void cboSelectUtilityRaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Layer lay in _mapLayer.Layers)
            {
                if (lay.LegendText == cboSelectUtilityRaster.SelectedItem)
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
            timesHit2++;
            if (timesHit2 == (int)(numPasses.Value))
            {
                IFeatureSet mcLCPA = FeatureSet.OpenFile(saveLocation + @"\MCLCPA.shp");
                IRaster utLCPA = Raster.OpenFile(saveLocation + @"\UT\outputpathrasternew.bgd");
                this.progressbar1.Value = 0;
                this.progressbar1.Style = ProgressBarStyle.Blocks;
                finalStatOutput = p1.finalStatOutput;

                frmResults result = new frmResults(utLCPA, additiveCosts, utilityCosts, mcLCPA, fst, MC.NumPasses, finalStatOutput, saveLocation);
                result.ShowDialog();
                _mapLayer.Layers.Add(pathLines);
                this.Close();
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

                cser.createRasters(_mapLayer, selectedItem, lc, utilityCosts, saveLocation);
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

        private void utCostLine()
        {
            try
            {
                FileInfo utilCosts = new FileInfo(utilityCosts.Filename);
                Cursor curs = Cursors.Arrow;
                string utilFileName = utilCosts.Name;
                clsCreateBackgroundRasters cbrUT = new clsCreateBackgroundRasters();
                shapefileSavePath = saveLocation + @"\UT\utilityCostsLCPA.shp";
                lcpaShapeName = "Utility Costs LCPA";
                DataColumn pass = new DataColumn("Pass");
                pathLines.Projection = _mapLayer.Projection;
                pathLines.DataTable.Columns.Add(pass);
                pathLines.SaveAs(shapefileSavePath, true);
                b1.buildDirectory(saveLocation + @"\UT");
                IRaster utilsCosts = utilityCosts;
                costFileName = saveLocation + @"\UT\utilCosts.bgd";
                utilsCosts.SaveAs(saveLocation + @"\UT\utilCosts.bgd");
                rasterToConvert = Raster.OpenFile(saveLocation + @"\UT\utilCosts.bgd");
                backlink = cbrUT.saveRaster(saveLocation + @"\UT\", "backlink", utilityCosts);
                outAccumRaster = cbrUT.saveRaster(saveLocation + @"\UT\", "outAccumRaster", utilityCosts);
                outPathRaster = cbrUT.saveRaster(saveLocation + @"\UT\", "outPath", utilityCosts);
                gr.prepareGATRasters(saveLocation + @"\UT", curs, backlink, outAccumRaster, ref outPathRaster, ref outputPathFilename);
                BackgroundWorker utCosts = new BackgroundWorker();
                paraString = new string[6] { startFileName.Substring(0, startFileName.Length - 4) + ".dep", costFileName.Substring(0, costFileName.Length - 4) + ".dep", outAccumRaster.Filename.Substring(0, outAccumRaster.Filename.Length - 4) + ".dep", backlink.Filename.Substring(0, backlink.Filename.Length - 4) + ".dep", "not specified", "not specified" }; //outputFilename + ".dep", backlinkFilename + ".dep", "not specified", "not specified" };
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
            newWork(worker, ref outputPathFilename,currentPass);
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
                utilLCPA = FeatureSet.OpenFile(saveLocation + @"\UT\utilityCostsLCPA.shp");
                this.progressbar1.Style = ProgressBarStyle.Continuous;
                //b1.buildDirectory(saveLocation + @"\Pass_" + Convert.ToString(currentPass));
                //clsCreateBackgroundRasters cbrMC = new clsCreateBackgroundRasters();
                //backlink = cbrMC.saveRaster(saveLocation + @"\Pass_" + Convert.ToString(currentPass), @"\backlink", bounds);
                //outAccumRaster = cbrMC.saveRaster(saveLocation + @"\Pass_" + Convert.ToString(currentPass), @"\outAccumRaster", bounds);
                //outPathRaster = cbrMC.saveRaster(saveLocation + @"\Pass_" + Convert.ToString(currentPass), @"\outputPathRaster", bounds);
                for (currentPass = 1; currentPass <= numPasses.Value; currentPass++)
                {
                    tracker.ProgressChanged += new ProgressChangedEventHandler(tracker_ProgressChanged);
                    p1.doTheProcess(tslStatus, tracker, utilityCosts, saveLocation, _mapLayer, currentPass, dgvSelectLayers, utilityCosts, MC, progress, ref outputPathFilename, additiveCosts, _b1, ref backlinkFilename, ref outputAccumFilename, tracker_ProgressChanged, ref rasterToConvert, ref costFileName);
                    additiveCosts = p1.additiveCosts;
                    createAccumCostRaster(outputPathFilename);
                    tslStatus.Visible = true;
                    tslStatus.Text = "Finishing Up";
                    tracker.ReportProgress(100);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + " \n has occured." + "\n" + "Current Pass: " + Convert.ToString(currentPass), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MC.errorCondition = true;
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            frm1121165 x12 = new frm1121165();
            x12.ShowDialog();
            
        }
       }
}