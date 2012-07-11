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
        clsprepgatraster gr = new clsprepgatraster();
        clsRasterOps pa;
        clsLCPCoords lc = new clsLCPCoords();
        clsBuildDirectory b1 = new clsBuildDirectory();
        FeatureSet projectFS = new FeatureSet();
        double[] aw = new double[5];
        string[] awTitles = new string[5] { "LSHigh", "LSMedHigh", "LSMedium", "LSMedLow", "LSLow" };
        double cellSize = 0;
        string saveLocation;
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
        string shapefileSavePath = string.Empty;
        string additveCostsFilePath;
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
        BackgroundWorker worker = new BackgroundWorker();
        Cursor xcurs;
        #endregion

        clsdoTheProcess p1 = new clsdoTheProcess();
        clsBuildDirectory _b1 = new clsBuildDirectory();

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
                utCostLine();
                //shapefileSavePath = saveLocation + @"\outputPaths.shp";
                //pathLines.SaveAs(shapefileSavePath, true);
           
                this.lblProgress.Text = "Performing Process...Please Wait.";
                //doTheProcess();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + "\n has occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }




        public void tracker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
           /*int percent = (int)(((double)progressbar1.Value / (double)progressbar1.Maximum) * 100);
          //  progressbar1.CreateGraphics().DrawString(percent.ToString() + "%", new Font("Arial", (float)8.25, FontStyle.Regular),
           //Brushes.Black, new PointF(progressbar1.Width / 2 - 10, progressbar1.Height / 2 - 7));
            this.progressbar1.Size = new System.Drawing.Size(670, 21);

            this.progressbar1.Value = e.ProgressPercentage;
            lblProgress.Text = progress;*/
            tracker.WorkerSupportsCancellation = true;
            tracker.WorkerReportsProgress = true;
            tracker.ProgressChanged += new ProgressChangedEventHandler(tracker_ProgressChanged);
        }

        BackgroundWorker tracker = new BackgroundWorker();


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
                GISTools.CostAccumulation ac = new GISTools.CostAccumulation();
                GISTools.CostPathway cp = new GISTools.CostPathway();
                clsGATGridConversions utConvert = new clsGATGridConversions();
                utConvert._rasterToConvert = rasterToConvert;
                utConvert._statusMessage = "Converting cost raster. ";
                utConvert.convertToGAT();
                ac.Initialize(wbHost);
                ac.Execute(paraString, worker);
                string[] costPath = new string[3] { endFileName + ".dep", backlink.Filename.Substring(0, backlink.Filename.Length -4) + ".dep", outputPathFilename + ".dep" };
                cp.Initialize(wbHost);
                cp.Execute(costPath, worker);
                convertCostPathwayToBGD();
                IRaster outPath = Raster.OpenFile(outputPathFilename + "new.bgd");
                //outPath.Save();
               headers.Add("Pass");
               attributes.Add(Convert.ToString(currentPass));
                clsCreateLineShapeFileFromRaster clsf = new clsCreateLineShapeFileFromRaster(); 
                clsf.createShapefile(outPath, 1, /*saveLocation + @"\MCLCPA.shp"*/ shapefileSavePath, headers, attributes, _mapLayer, "MCLCPA", pathLines);
                shapefileSavePath = saveLocation + @"\MCLCPA.shp";
                //createPathShapefile(outPath);
                
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error :" + ex + " has occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
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


        Random random = new Random();



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
                this.Close();
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
                            this.Close();
                        }

                    }
                }
                //lbxProgress.Items.Add("End: " + Convert.ToString(ers.NumRows) + ", " + Convert.ToString(ers.NumColumns));
                Cursor = Cursors.Default;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + " has occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
            }
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

                this.Close();

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
               this.Close();
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
                this.Close();
            }
        }

        private void utCostLine()
        {
            try
            {
               
                int currentPass = 0;
                FileInfo utilCosts = new FileInfo(utilityCosts.Filename);
                Cursor curs = Cursors.Arrow;
                string utilFileName = utilCosts.Name;
                clsCreateBackgroundRasters cbr = new clsCreateBackgroundRasters();
                shapefileSavePath = saveLocation + @"\UT\utilityCostsLCPA.shp";
                lcpaShapeName = "Utility Costs LCPA";
                //shapefileSavePath = saveLocation + @"\UT\OutPath.shp";
                DataColumn pass = new DataColumn("Pass");
                pathLines.Projection = _mapLayer.Projection;
                pathLines.DataTable.Columns.Add(pass);
                pathLines.SaveAs(shapefileSavePath, true);
                b1.buildDirectory(saveLocation + @"\UT");
                IRaster utilsCosts = utilityCosts;
                costFileName = saveLocation + @"\UT\utilCosts.bgd";
                utilsCosts.SaveAs(saveLocation + @"\UT\utilCosts.bgd");
                rasterToConvert = Raster.OpenFile(saveLocation + @"\UT\utilCosts.bgd");
                backlink = cbr.saveRaster(saveLocation + @"\UT\", "backlink", bounds);
                outAccumRaster = cbr.saveRaster(saveLocation + @"\UT\", "outAccumRaster", bounds);
                outPathRaster = cbr.saveRaster(saveLocation + @"\UT\", "outPath", bounds);
                gr.prepareGATRasters(saveLocation + @"\UT", worker, curs, backlink, outAccumRaster, ref outPathRaster, ref outputPathFilename);
                BackgroundWorker utCosts = new BackgroundWorker();
                paraString = new string[6] { startFileName + ".dep", costFileName.Substring(0, costFileName.Length - 4) + ".dep", outAccumRaster.Filename.Substring(0, outAccumRaster.Filename.Length - 4) + ".dep", backlink.Filename.Substring(0, backlink.Filename.Length - 4) + ".dep", "not specified", "not specified" }; //outputFilename + ".dep", backlinkFilename + ".dep", "not specified", "not specified" };
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

                for (currentPass = 1; currentPass <= numPasses.Value; currentPass++)
                {
                    p1.doTheProcess(tslStatus, tracker, bounds, saveLocation, _mapLayer, currentPass, dgvSelectLayers, utilityCosts, MC, progress, ref outputPathFilename, additiveCosts, _b1, ref backlinkFilename, ref outputAccumFilename, tracker_ProgressChanged, ref rasterToConvert, ref costFileName);
                    if (p1.erroroc == true)
                    {
                        this.Close();
                    }
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
                this.Close();
            }
        }

    }
}