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
    public partial class frmToolExecute : Form
    {

        #region classVariables
        private System.Data.DataSet dsQuesSets = new System.Data.DataSet();
        IMap _mapLayer = null;
        int currentPass = 1;
        //int currentQuestion = 1;
        clsMonteCarlo MC = new clsMonteCarlo();
        clsRasterOps pa;
        //clsleastCostPath lcpProc = new clsleastCostPath();
        clsLCPCoords lc = new clsLCPCoords();
        FeatureSet projectFS = new FeatureSet();
        double[] aw = new double[5];
        string[] awTitles = new string[5] { "LSHigh", "LSMedHigh", "LSMedium", "LSMedLow", "LSLow" };
        double cellSize = 0;
        string saveLocation = null;
        int rasterRow = 0;
        int rasterCol = 0;
        IRaster utilityCosts = new Raster();
        IRaster bounds = new Raster();
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
        string currentQuesPath = "";
        IRaster outPath;
        FeatureSet pathLines = new FeatureSet(FeatureType.Line);
        FeatureSet utPathLine = new FeatureSet(FeatureType.Line);
        IFeatureSet fst = new FeatureSet();
        string shapefileSavePath;
        string additveCostsFilePath;
        IRaster additiveCosts;
        int timesHit2 = 1;
        List<string> finalStatOutput = new List<string>();
        string _surveyPath = string.Empty;
        string lcpaShapeName = string.Empty;
        IFeatureSet utilLCPA;
        List<IRaster> mcRasterList = new List<IRaster>();
        string progress = string.Empty;

        #endregion

        #region Methods

        public frmToolExecute(IMap MapLayer, clsLCPCoords _lc, string _projSavePath, string surveyPath)
        {
            InitializeComponent();
            _mapLayer = MapLayer;
            _surveyPath = surveyPath;
            fillInDataView();
            pa = new clsRasterOps(_mapLayer);
            fillBoundCombo();
            fillUtilityCombo();
            fillStartCombo();
            fillEndCombo();
            fillStartEndCombo();
            lc = _lc;
            pathLines.Projection = _mapLayer.Projection;
            if (_projSavePath != string.Empty)
            {
                cboStartEndPoints.Enabled = true;
                txtSaveLocation.Text = _projSavePath;
                saveLocation = _projSavePath;
            }
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

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + "\n has occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void fillBoundCombo()
        {
            foreach (Layer lay in _mapLayer.Layers)
            {
                if (lay.GetType() == typeof(MapRasterLayer))
                {
                    cboSelectBoundingRaster.Items.Add(lay.LegendText);
                }
            }
        }

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

        private void buildDirectory(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }

        private void fillStartCombo()
        {
            foreach (Layer lay in _mapLayer.Layers)
            {
                if (lay.GetType() == typeof(MapRasterLayer))
                {
                    cboStartPoint.Items.Add(lay.LegendText);
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

        private void fillEndCombo()
        {
            foreach (Layer lay in _mapLayer.Layers)
            {
                if (lay.GetType() == typeof(MapRasterLayer))
                {
                    cboEndPoint.Items.Add(lay.LegendText);
                }
            }
        }

        private void btnBegin_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboStartEndPoints.Text == "Select Shapefile for Start and End Points")
                {
                    MessageBox.Show("Select the shapefile that has the starting and ending points.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                utCostLine();
                this.lblProgress.Text = "Performing Process...Please Wait.";
                //doTheProcess();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + "\n has occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tracker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int percent = (int)(((double)progressbar1.Value / (double)progressbar1.Maximum) * 100);
            progressbar1.CreateGraphics().DrawString(percent.ToString() + "%", new Font("Arial", (float)8.25, FontStyle.Regular),
            Brushes.Black, new PointF(progressbar1.Width / 2 - 10, progressbar1.Height / 2 - 7));
            this.progressbar1.Size = new System.Drawing.Size(670, 21);
         
            this.progressbar1.Value = e.ProgressPercentage;
            //Thread.Sleep(1000);
            lblProgress.Text = progress;
        }

        private void doTheProcess()
        {

            BackgroundWorker tracker = new BackgroundWorker();
            tracker.WorkerSupportsCancellation = true;
            tracker.WorkerReportsProgress = true;
            tracker.ProgressChanged += new ProgressChangedEventHandler(tracker_ProgressChanged);
            
            try
            {
                tslStatus.Visible = false;
                string path = saveLocation + @"\linesiter\LSProcessing";
                shapefileSavePath = saveLocation + @"\outputPaths.shp";
                MC.NumPasses = (int)numPasses.Value;
                lcpaShapeName = "Monte Carlo LCPA";
                //tslPass.Text = "Current Monte Carlo Pass: " + Convert.ToString(currentPass);
                utilityCosts.Bounds = bounds.Bounds;
                progress = "Creating Weighted Rasters";
                tracker.ReportProgress(10);
                //create directory "LSProcessing" for storing and processing temp raster data
                buildDirectory(path + @"\Pass_" + Convert.ToString(currentPass));
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
                //outPathRaster = path + @"\Pass_" + Convert.ToString(currentPass) + @"\outputRasternew";
                outPathRaster.Save();
                //DataColumn pass = new DataColumn("Pass");
                //DataColumn weight = new DataColumn("Weight");
                pathLines.Projection = _mapLayer.Projection;
                //pathLines.DataTable.Columns.Add(pass);
                //pathLines.DataTable.Columns.Add(weight);
                pathLines.SaveAs(shapefileSavePath, true);
                additiveCosts = Raster.CreateRaster(additveCostsFilePath, null, bounds.NumColumns, bounds.NumRows, 1, typeof(float), null);
                additiveCosts.Bounds = bounds.Bounds;
                additiveCosts.Projection = _mapLayer.Projection;
                additiveCosts.Save();
                int newQIDValue = 0;
                double rv = 0;
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
                    if(dx.Cells[0].Value != null)
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
                //return;
                 for( int i= 1; i<= MC.NumPasses;)
                 {
                     //perform the Monte Carlo process
                     tslStatus.Visible = false;
                     finalStatOutput.Add("Monte Carlo Pass: " + Convert.ToString(i)); //Convert.ToString(currentPass));
                     int questNum = 1;
                     IRaster mcRaster = bounds;
                     string mcRasSavePath = saveLocation + @"\linesiter\LSProcessing\Pass_" + Convert.ToString(i); //Convert.ToString(currentPass);
                     Directory.CreateDirectory(mcRasSavePath);
                     mcRaster.SaveAs(mcRasSavePath + @"\mcRaster.bgd");
                     mcRaster = null;
                     mcRaster = Raster.Open(mcRasSavePath + @"\mcRaster.bgd");
                     double[] AnswerPercents = new double[5];
                     double[] mcPercents = new double[5];
                     progress = "Current Pass " + Convert.ToString(i); //Convert.ToString(currentPass);
                     tracker.ReportProgress(40);
                     foreach (DataGridViewRow dr in dgvSelectLayers.Rows)
                     {
                         if (Convert.ToString(dr.Cells[0].Value) == "True")
                         {
                             if (dr.Cells[0].Value != null)
                             {
                                 if (/*currentPass*/ i <= MC.NumPasses)
                                 {
                                     if (dr.Cells[1].Value == DBNull.Value)
                                     {
                                         //write exception
                                         finalStatOutput.Add("Question: " + Convert.ToString(dr.Cells[3].Value) + " has no associated raster layer.  It will not be considered.");
                                     }
                                     else
                                     {
                                         //create directory for each question
                                         string newPath = saveLocation + @"\linesiter\LSProcessing\Quesid_" + Convert.ToString(dr.Cells[3].Value);
                                         string rasterPath = saveLocation + @"\linesiter\LSProcessing\Quesid_" + Convert.ToString(dr.Cells[2].Value) + Convert.ToString(dr.Cells[1].Value) + "pa.bgd";
                                         buildDirectory(newPath);
                                         //load raster file
                                         IRaster oRaster = Raster.OpenFile(rasterPath);
                                         //create weighted rasters for each of the weights stored in the monte carlo class
                                         createWeightedRasters(newPath, rasterPath, oRaster);
                                     }
                                 }
                             }
                             rv = 0;
                             rv = RandomNumber();
                             finalStatOutput.Add("Question: ");
                             if (dr.Cells[4].Value != DBNull.Value)
                             {
                                 finalStatOutput.Add((string)dr.Cells[4].Value);
                             }
                             finalStatOutput.Add("Associated Feature Class: ");
                             if (dr.Cells[1].Value != DBNull.Value)
                             {
                                 finalStatOutput.Add(Convert.ToString(dr.Cells[1].Value));
                             }
                             finalStatOutput.Add("Random Variable: " + Convert.ToString(rv));

                             if ((double)dr.Cells[5].Value == 0.0 && (double)dr.Cells[6].Value == 0 && (double)dr.Cells[7].Value == 0 && (double)dr.Cells[8].Value == 0 && (double)dr.Cells[9].Value == 0)
                             {
                                 MessageBox.Show("Check the questions you specified.  One or more questions have an invalid response.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                 pathLines.DataTable.Columns.Remove("pass");
                                 return;
                             }
                             if (dr.Cells[5].Value != DBNull.Value)
                             {
                                 AnswerPercents[0] = (double)dr.Cells[5].Value;
                             }
                             if (dr.Cells[6].Value != DBNull.Value)
                             {
                                 AnswerPercents[1] = AnswerPercents[0] + (double)dr.Cells[6].Value;
                             }
                             if (dr.Cells[7].Value != DBNull.Value)
                             {
                                 AnswerPercents[2] = AnswerPercents[1] + (double)dr.Cells[7].Value;
                             }
                             if (dr.Cells[8].Value != DBNull.Value)
                             {
                                 AnswerPercents[3] = AnswerPercents[2] + (double)dr.Cells[8].Value;
                             }
                             if (dr.Cells[9].Value != DBNull.Value)
                             {
                                 AnswerPercents[4] = AnswerPercents[3] + (double)dr.Cells[9].Value;
                             }
                             MC.calculateWeight(rv, AnswerPercents);
                             tracker.ReportProgress(60);
                             finalStatOutput.Add("LSHigh: " + Convert.ToString(AnswerPercents[0]) + " | LSMedHigh: " + Convert.ToString(AnswerPercents[1]) + " | LSMed: " + Convert.ToString(AnswerPercents[2]) + " | LSMedLow: " + Convert.ToString(AnswerPercents[3]) + " | LSLow: " + Convert.ToString(AnswerPercents[4]));
                             finalStatOutput.Add("Weight: " + (Convert.ToString(MC.socialWeight)));
                             finalStatOutput.Add(MC.wRaster);
                             currentQuesPath = saveLocation + @"\linesiter\LSProcessing\Quesid_" + Convert.ToString(dr.Cells[3].Value) + "\\" + MC.wRaster + ".bgd";
                             IRaster mRaster = Raster.OpenFile(currentQuesPath);
                             mcRaster = MC._calcRaster(mcRaster, mRaster, saveLocation);
                             mcRaster.SaveAs(mcRasSavePath + @"\mcRaster.bgd");
                             mcRasterList.Add(mRaster);
                             questNum++;
                             MC.calcRaster2(mcRasterList, mcRaster);
                             mcRaster.Save();
                         }
                     }

                     i++;
                     calculate_Cost_Weight(mcRasSavePath);
                     progress = "Pass " + Convert.ToString(i) /*Convert.ToString(currentPass)*/ + " is complete.";
                     tracker.ReportProgress(90);
                     prepareGATRasters(mcRasSavePath);
                     createAccumCostRaster();

                     tslStatus.Visible = true;
                     tslStatus.Text = "Finishing Up";
                     tracker.ReportProgress(100);
                     currentPass = i;
                     //currentPass++;
                 }

                 //while (currentPass <= MC.NumPasses);
                    
                   
                
                    //cleanup
                //remove all necessary files
                //removeProcessingFiles();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + "has occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
            }
        }

        private void createAccumCostRaster()
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
            }
        }

        private void bkwork_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            newWork(worker);

       
        }

        private void newWork(BackgroundWorker worker)
        {
            try
            {
                clsWBHost wbHost = new clsWBHost(tslStatus);
                GISTools.CostAccumulation ac = new GISTools.CostAccumulation();
                GISTools.CostPathway cp = new GISTools.CostPathway();
                clsGATGridConversions utConvert = new clsGATGridConversions();
                utConvert._rasterToConvert = rasterToConvert;
                utConvert._statusMessage = "Converting cost raster. ";
                utConvert.convertToGAT();                
                ac.Initialize(wbHost);
                ac.Execute(paraString, worker);
                string[] costPath = new string[3] { endFileName + ".dep", backlinkFilename + ".dep", outputPathFilename + ".dep" };
                cp.Initialize(wbHost);                   
                cp.Execute(costPath, worker);
                convertCostPathwayToBGD();
                IRaster outPath = Raster.OpenFile(outputPathFilename + "new.bgd");
                outPath.Save();
                createPathShapefile(outPath);
               
               
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error :" + ex + " has occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            }
        }

        private void convertCostPathwayToBGD()
        {
            clsGATGridConversions cpRas = new clsGATGridConversions();
            cpRas._statusMessage = "Converting Cost Path";
            cpRas._bnds = bounds;
            cpRas._gridToConvert = outputPathFilename;
            cpRas._conversionRaster = outputPathFilename;
            cpRas.convertBGD();
            outPath = cpRas.conRaster;
        }

        private void createWeightedRasters(string newPath, string rasterPath, IRaster oRaster)
        {
            int curWeight = 0;
            string[] weights = new string[5] { "LSHigh", "LSMedHigh", "LSMedium", "LSMedLow", "LSLow" };
            foreach (double weight in MC.assignedWeights)
            {
                IRaster weightedRaster = Raster.CreateRaster(newPath + "\\" + weights[curWeight] + ".bgd", null, oRaster.NumColumns, oRaster.NumRows, 1, typeof(double), null);
                weightedRaster.Bounds = bounds.Bounds;
                weightedRaster.NoDataValue = bounds.NoDataValue;
                weightedRaster.Projection = bounds.Projection;
                double oValue = 0;

                for (int oRows = 0; oRows < oRaster.NumRows - 1; oRows++)
                {
                    for (int oCols = 0; oCols < oRaster.NumColumns - 1; oCols++)
                    {
                        oValue = oRaster.Value[oRows, oCols];

                        if (oValue != oRaster.NoDataValue)
                        {
                            if (oValue == -1)
                            {
                                //weightedRaster.Value[oRows, oCols] = Math.Abs(oValue) * weight;
                                weightedRaster.Value[oRows, oCols] = weight;
                            }
                            else
                            {
                                weightedRaster.Value[oRows, oCols] = 1;
                            }
                        }
                        else
                        {
                            weightedRaster.Value[oRows, oCols] = 1;
                        }
                    }
                }
                weightedRaster.Save();
                curWeight++;
            }
        }

        Random random = new Random();

        private double RandomNumber()
        {
            return random.NextDouble();
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
            }
        }

        private void btnDelWorkSpace_Click(object sender, EventArgs e)
        {
            removeProcessingFiles();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.MyDocuments;
            fbd.ShowDialog();
            txtSaveLocation.Text = fbd.SelectedPath;
            saveLocation = fbd.SelectedPath;
            cboStartEndPoints.Enabled = true;
        }

        private void frmToolExecute_Load(object sender, EventArgs e)
        {
            this.Text = "Line Siter  " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void calculate_Cost_Weight(string savePath)
        {
            try
            {
                clsRasterOps mapAlgCW = new clsRasterOps(_mapLayer);
                IRaster addUt = additiveCosts;
                IRaster div1 = utilityCosts;
                IRaster div2 = Raster.Open(savePath + @"\mcraster.bgd");
                IRaster costWeightRaster = Raster.CreateRaster(savePath + @"\CostWeightRaster_" + currentPass + ".bgd", null, rasterCol, rasterRow, 1, typeof(double), null);
                costFileName = savePath + @"\CostWeightRaster_" + currentPass + ".bgd";
                costWeightRaster.Bounds = bounds.Bounds;
                costWeightRaster.Projection = _mapLayer.Projection;
                costWeightRaster.Save();
                mapAlgCW.rasterDivision(div1, div2, costWeightRaster);
                rasterToConvert = costWeightRaster;
                List<IRaster> additive = new List<IRaster>();
                additive.Add(addUt);
                additive.Add(costWeightRaster);
                mapAlgCW.rasterAddition(additive, additiveCosts);
               }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + " has occured.", "Generic Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void cboSelectBoundingRaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Layer lay in _mapLayer.Layers)
            {
                if (lay.LegendText == cboSelectBoundingRaster.SelectedItem)
                {
                    if (lay.GetType() == typeof(MapRasterLayer))
                    {
                        bounds = (IRaster)lay.DataSet;
                        cboSelectBoundingRaster.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show("Wrong layer type selected.  Please select a bounding file of raster type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
            }
            projectFS.Extent = bounds.Extent;
            cellSize = bounds.CellHeight;
            picAcpt1.Visible = true;
        }

        private void cboSelectUtilityRaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Layer lay in _mapLayer.Layers)
            {
                if (lay.LegendText == cboSelectUtilityRaster.SelectedItem)
                {
                    utilityCosts = (IRaster)lay.DataSet;
                }
            }
            if ((utilityCosts.NumRows == bounds.NumRows) && (utilityCosts.NumColumns == bounds.NumColumns))
            {
                picAcpt2.Visible = true;
                pictRej1.Visible = false;
                pictRej2.Visible = false;
            }
            else
            {
                pictRej1.Visible = true;
                pictRej2.Visible = true;
                cboSelectBoundingRaster.Enabled = true;
                MessageBox.Show("Utility Raster and Bounding Raster have incompatible boundaries.  \n Utilities Raster's bounds are: " + Convert.ToString(utilityCosts.NumRows) + ", " + Convert.ToString(utilityCosts.NumColumns) + "\n Bounding Raster's bounds are: " + Convert.ToString(bounds.NumRows) + ", " + Convert.ToString(bounds.NumColumns) + " \n Please check and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void cboStartPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                IRaster srs = new Raster();
                foreach (Layer lay in _mapLayer.Layers)
                {
                    BackgroundWorker worker = new BackgroundWorker();
                    if (lay.LegendText == cboStartPoint.SelectedItem)
                    {
                        srs = (IRaster)lay.DataSet;
                        if (srs.Bounds == bounds.Bounds)
                        {
                            clsGATGridConversions sourceRaster = new clsGATGridConversions();
                            sourceRaster._rasterToConvert = srs;
                            sourceRaster._statusMessage = "Converting Source Point Raster. ";
                            sourceRaster.convertToGAT();
                            startFileName = srs.Filename.Substring(0, srs.Filename.Length - 4);
                            picStart.Visible = true;
                        }
                        else
                        {
                            MessageBox.Show("Source raster bounds do not match the project bounds raster.  \nPlease select a source point raster with bounds identical to the project bounds raster.", "Bounds Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }
                    }
                }
                Cursor = Cursors.Default;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + " has occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cboEndPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                IRaster ers = new Raster();
                BackgroundWorker worker = new BackgroundWorker();
                foreach (Layer lay in _mapLayer.Layers)
                {
                    if (lay.LegendText == cboEndPoint.SelectedItem)
                    {
                        ers = (IRaster)lay.DataSet;
                        if (ers.Bounds == bounds.Bounds)
                        {
                            clsGATGridConversions destinationRaster = new clsGATGridConversions();
                            destinationRaster._rasterToConvert = ers;
                            destinationRaster._statusMessage = "Converting destination point raster. ";
                            destinationRaster.convertToGAT();
                            endFileName = ers.Filename.Substring(0, ers.Filename.Length - 4);
                            picEnd.Visible = true;
                        }
                        else
                        {
                            MessageBox.Show("Destination raster bounds do not match the project bounds raster.  \nPlease select a destination point raster with bounds identical to the project bounds raster.", "Bounds Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }
                    }
                }
                //lbxProgress.Items.Add("End: " + Convert.ToString(ers.NumRows) + ", " + Convert.ToString(ers.NumColumns));
                Cursor = Cursors.Default;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + " has occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void prepareGATRasters(string savePath)
        {
            BackgroundWorker worker = new BackgroundWorker();
            backlinkFilename = savePath + @"\backlink";
              clsGATGridConversions prepareGATs = new clsGATGridConversions();
            prepareGATs._rasterToConvert = backlink;
            prepareGATs._statusMessage = "Preparing GAT Rasters, Please Wait";
            Cursor = Cursors.WaitCursor;
            prepareGATs.convertToGAT();
            outputAccumFilename = savePath + @"\outputAccumRaster";
            prepareGATs._rasterToConvert = outAccumRaster;
            prepareGATs.convertToGAT();
            outputPathFilename = savePath + @"\outputPathRaster";
            prepareGATs._rasterToConvert = outPathRaster;
            outPathRaster.Save();
            prepareGATs.convertToGAT();
            Cursor = Cursors.Default;
        }

        private void createPathShapefile(IRaster pathCon)
        { 
            List<Coordinate> pthXYs = new List<Coordinate>();
          
                for (int nRows = 0; nRows < pathCon.NumRows; nRows++)
                {
                    for (int nCols = 0; nCols < pathCon.NumColumns; nCols++)
                    {
                        if (pathCon.Value[nRows, nCols] == 1)
                        {
                            Coordinate xy = new Coordinate();
                            xy = pathCon.CellToProj(nRows, nCols);
                            pthXYs.Add(xy);
                        }

                    }

                }
                 
             
                LineString pathString = new LineString(pthXYs);
                IFeature pathLine = pathLines.AddFeature(pathString);
                pathLine.DataRow["Pass"] = Convert.ToString(currentPass);
                //pathLine.DataRow["Weight"] = Convert.ToString(MC.socialWeight);
                pathLines.Name = lcpaShapeName;
                pathLines.Extent = outPath.Extent;
                pathLines.Projection = _mapLayer.Projection;
                pathLines.SaveAs(shapefileSavePath, true);
       
          }

        private void finishingUp()
        {
            timesHit2++;
            if (timesHit2 == (int)(numPasses.Value))
            {
                IFeatureSet mcLCPA = FeatureSet.OpenFile(saveLocation + @"\outputPaths.shp");
                IRaster utLCPA = Raster.OpenFile(saveLocation + @"\UT\outputpathrasternew.bgd");
                this.progressbar1.Value = 0;
                this.progressbar1.Style = ProgressBarStyle.Blocks;
                frmResults result = new frmResults(utLCPA, additiveCosts, utilityCosts, mcLCPA, fst, MC.NumPasses, finalStatOutput, saveLocation);
                result.ShowDialog();
                _mapLayer.Layers.Add(pathLines);
                this.Close();
            }
        }

        #endregion

        private void cboStartEndPoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //IRaster newRast = new Raster();
                string[] rasOps = new string[1];
                //newRast.Bounds = bounds.Bounds;
                //newRast.NumRows = bounds.NumRows;
                //newRast.NumColumns = bounds.NumColumns;
                //newRast.Projection = bounds.Projection;
                string pathS = saveLocation;
                IRaster startPoint = Raster.CreateRaster(pathS + @"\startPoint.bgd", null, bounds.NumColumns, bounds.NumRows, 1, typeof(int), rasOps);
                IRaster endPoint = Raster.CreateRaster(pathS + @"\endPoint.bgd", null, bounds.NumColumns, bounds.NumRows, 1, typeof(int), rasOps);
                startPoint.Bounds = bounds.Bounds;
                startPoint.Projection = _mapLayer.Projection;
                startPoint.Save();
                endPoint.Bounds = bounds.Bounds;
                endPoint.Projection = _mapLayer.Projection;
                endPoint.Save();
                Cursor = Cursors.WaitCursor;
                foreach (Layer lay in _mapLayer.Layers)
                {
                    if (lay.LegendText == cboStartEndPoints.SelectedItem)
                    {
                        if (lay.GetType() == typeof(DotSpatial.Controls.MapPointLayer))
                        {
                            int x = 0;
                            fst = (IFeatureSet)lay.DataSet;
                            foreach (Feature fcd in fst.Features)
                            {
                                if (x == 0)
                                {
                                    Coordinate cd;
                                    cd = new Coordinate((fcd.Coordinates[0]).X, (fcd.Coordinates[0]).Y);
                                    RcIndex rSCood = bounds.Bounds.ProjToCell(cd);
                                    lc.startCol = rSCood.Column;
                                    lc.startRow = rSCood.Row;
                                }
                                if (x == 1)
                                {
                                    Coordinate cd;
                                    cd = new Coordinate((fcd.Coordinates[0]).X, (fcd.Coordinates[0]).Y);
                                    RcIndex rECood = bounds.Bounds.ProjToCell(cd);
                                    lc.EndCol = rECood.Column;
                                    lc.EndRow = rECood.Row;
                                }
                                if (x > 1)
                                {
                                    MessageBox.Show("This shapefile contains more than a starting and ending point and cannot be used. \n Please select a proper file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                                x++;
                            }

                            for (int oRows = 0; oRows < bounds.NumRows - 1; oRows++)
                            {
                                for (int oCols = 0; oCols < bounds.NumColumns - 1; oCols++)
                                {
                                    if (oRows == lc.startRow & oCols == lc.startCol)
                                    {
                                        startPoint.Value[oRows, oCols] = 1234567890;
                                    }
                                    else if (oRows == lc.EndRow & oCols == lc.EndCol)
                                    {
                                        endPoint.Value[oRows, oCols] = 0987654321;
                                    }
                                    else
                                    {
                                        startPoint.Value[oRows, oCols] = 0;
                                        endPoint.Value[oRows, oCols] = 0;
                                    }
                                }
                            }
                            startPoint.Save();
                            startGAT(startPoint);
                            endPoint.Save();
                            endGAT(endPoint);
                        }
                        else
                        {
                            MessageBox.Show("This application currently only accepts point shapefiles for starting and ending point locations.", "Shapefile Operations", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }
                picStartEnd.Visible = true;
                Cursor = Cursors.Default;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + Convert.ToString(ex) + " /n has occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void endGAT(IRaster endConvertRas)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (endConvertRas.Bounds == bounds.Bounds)
                {
                    clsGATGridConversions destinationRaster = new clsGATGridConversions();
                    destinationRaster._rasterToConvert = endConvertRas;
                    destinationRaster._statusMessage = "Converting end point raster. ";
                    destinationRaster.convertToGAT();
                    endFileName = endConvertRas.Filename.Substring(0, endConvertRas.Filename.Length - 4);
                    picEnd.Visible = true;
                }
                else
                {
                    MessageBox.Show("End point raster bounds do not match the project bounds raster.  \nPlease select a destination point raster with bounds identical to the project bounds raster.", "Bounds Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void startGAT(IRaster startConvertRas)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (startConvertRas.NumRows == bounds.NumRows && startConvertRas.NumColumns == bounds.NumColumns)
                //if (startConvertRas.Bounds == bounds.Bounds)
                {
                    clsGATGridConversions destinationRaster = new clsGATGridConversions();
                    destinationRaster._rasterToConvert = startConvertRas;
                    destinationRaster._statusMessage = "Converting start point raster. ";
                    destinationRaster.convertToGAT();
                    startFileName = startConvertRas.Filename.Substring(0, startConvertRas.Filename.Length - 4);
                    picEnd.Visible = true;
                }
                else
                {
                    MessageBox.Show("Start point raster bounds do not match the project bounds raster.  \nPlease select a destination point raster with bounds identical to the project bounds raster.", "Bounds Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
           int temp3=0;
        private void utCostLine()
        {
            try
            {
               // frmProgMC();
                temp3 = 0;
                FileInfo utilCosts = new FileInfo(utilityCosts.Filename);
                string utilFileName = utilCosts.Name;
                shapefileSavePath = saveLocation + @"\UT\utilityCostsLCPA.shp";
                lcpaShapeName = "Utility Costs LCPA";
                DataColumn pass = new DataColumn("Pass");
                pathLines.Projection = _mapLayer.Projection;
                pathLines.DataTable.Columns.Add(pass);                       
                pathLines.SaveAs(shapefileSavePath, true);
              //currentPass = 1;
                buildDirectory(saveLocation + @"\UT");
                IRaster utilsCosts = utilityCosts;
                //shapeFileName = "Utility Pass";
                shapefileSavePath = saveLocation + @"\UT\OutPath.shp";
                costFileName = saveLocation + @"\UT\utilCosts.bgd";
                backlinkFilename = saveLocation + @"\UT\backlink";
                outputAccumFilename = saveLocation + @"\UT\outputAccum";
                outputPathFilename = saveLocation + @"\UT\outPath";
                utilsCosts.SaveAs(saveLocation + @"\UT\utilCosts.bgd");
                rasterToConvert = Raster.OpenFile(saveLocation + @"\UT\utilCosts.bgd");
                //rasterToConvert = Raster.CreateRaster(costFileName + ".bgd", null, bounds.NumColumns, bounds.NumRows, 1, typeof(double), null);
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
                prepareGATRasters(saveLocation + @"\UT");
                BackgroundWorker utCosts = new BackgroundWorker();
                paraString = new string[6] { startFileName + ".dep", costFileName.Substring(0, costFileName.Length - 4) + ".dep", outputAccumFilename + ".dep", backlinkFilename + ".dep", "not specified", "not specified" }; //outputFilename + ".dep", backlinkFilename + ".dep", "not specified", "not specified" };
                utCosts.WorkerReportsProgress = true;
                utCosts.WorkerSupportsCancellation = false;
                utCosts.DoWork += new DoWorkEventHandler(utCosts_DoWork);
                utCosts.ProgressChanged += new ProgressChangedEventHandler(utCosts_ProgressChanged);
                utCosts.RunWorkerCompleted += new RunWorkerCompletedEventHandler(utCosts_RunWorkerCompleted);
                utCosts.RunWorkerAsync();
                //utCosts.Dispose();
            }

            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
            }
        }

        private void utCosts_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            newWork(worker);
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
        //        currentPass ++;
            
                this.progressbar1.Style = ProgressBarStyle.Continuous;
                doTheProcess();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + " \n has occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

           

    }
}