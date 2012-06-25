using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DotSpatial.Analysis;
using DotSpatial.Data;
//using DotSpatial.Tools;
using DotSpatial.Controls;
//using DotSpatial.Controls.Docking;
using DotSpatial.Controls.Header;
//using DotSpatial.Controls.RibbonControls;
using DotSpatial.Topology;
using DotSpatial.Modeling;
using DotSpatial.Symbology;
using System.Reflection;
using System.Data.OleDb;
using System.Xml;

namespace nx09SitingTool
{
    public partial class frmMain : Form
    {
        bool mouseUPBool = false;
        int seInt = 1;
        double startP = 0;
        double endP = 0;
        int endR =0;
        int endC = 0;
        int startR = 0;
        int startC = 0;
        private readonly LayoutForm _layout;

        [Export("Shell", typeof(ContainerControl))]
        private static ContainerControl Shell;

        public frmMain()
        {
            InitializeComponent();
            _layout = new LayoutForm { MapControl = mpMain };
            //apManMain.LoadExtensions();
            Shell = this;
            mpMain.GeoMouseMove += mpMain_GeoMouseMove;

            
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //lstMain.DrawSubItem += new DrawListViewSubItemEventHandler(lstMain_DrawSubItem);
            frmMain.ActiveForm.Text = "Line Siter  " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        //private void rbtnStart_Click(object sender, EventArgs e)
        //{
        //    frmToolExecute newExecute = new frmToolExecute(mpMain, clsLCPCoords clslcp);
        //    newExecute.ShowDialog();
        //}

        double[] answersPercent = new double[7];
        string[] answersName = new string[7];
        

    #region Operation Controls
        
        private void btnMonteCarlo_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Choose two points to figure the least cost path.", "Least Cost Path", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }
            seInt = 1;
            //MessageBox.Show("Monte Carlo simulation to begin here.", "Info", MessageBoxButtons.OK);
            mouseUPBool = true;
            tssLCPA.Text = "Least Cost Path Analysis: ";

        }



        private void btnVectorToRaster_Click(object sender, EventArgs e)
        {
            OpenFileDialog fTOvOd = new OpenFileDialog();
            fTOvOd.Filter = "Shape Files |*.shp";
            IFeatureSet fs = null;
            SaveFileDialog rTOvSd = new SaveFileDialog();
            rTOvSd.Filter = "Raster Files|*.bgd";
            string fName = rTOvSd.FileName;
            IRaster outputRaster = null;
            if (fTOvOd.ShowDialog() == DialogResult.OK)
            {
                fs = FeatureSet.Open(fTOvOd.FileName);
                //fs.Open(fTOvOd.FileName);
            }
            if (rTOvSd.ShowDialog() == DialogResult.OK)
            {
                 fName= rTOvSd.FileName;
            }

            var mess = MessageBox.Show("Convert " + fTOvOd.FileName + " to raster format?", "Raster Conversion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (mess == DialogResult.No)
            {
                return;
            }
            outputRaster = DotSpatial.Analysis.VectorToRaster.ToRaster(fs, 0, "featureWeig", fName);
            outputRaster.Save();
            createPA(outputRaster, fName);
        }

        private void btnPresenceAbsence_Click(object sender, EventArgs e)
        {
            try
            {
               if (mpMain.Layers.SelectedLayer as MapRasterLayer == null)
                {
                        MessageBox.Show("Please select a raster layer to reclassify.", "Wrong Layer Selected", MessageBoxButtons.OK);
                        return;
                }    
                IMapRasterLayer mapLayer = (IMapRasterLayer)mpMain.Layers.SelectedLayer;
                string rasterSaveFN = "";
                if (mapLayer == null)
                {
                    MessageBox.Show("Please select a layer to convert to a presence/absence raster.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    IRaster oRaster = mapLayer.DataSet;
                    SaveFileDialog rasterSaveDlg = new SaveFileDialog();
                    rasterSaveDlg.Filter = "DotSpatial Raster Files | *.bgd";
                    if (rasterSaveDlg.ShowDialog() == DialogResult.OK)
                    {
                        rasterSaveFN = rasterSaveDlg.FileName;
                    }
                    else
                    {
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + " has occurred.", "Error 99: Generic Exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void createPA(IRaster oRaster, string rasterSaveFN)
        {
            try
            {
                DialogResult Result = (MessageBox.Show("Format: " + Convert.ToString(oRaster.DataType), "Info", MessageBoxButtons.OKCancel));


                if (Result == DialogResult.Cancel)
                {

                }

                string[] rasterParameters = null;
                int len = rasterSaveFN.Length - 4;
                string rasterSaveFN2 = rasterSaveFN.Insert(len, "PA");
                IRaster paRaster = Raster.CreateRaster(rasterSaveFN2, "", oRaster.NumColumns, oRaster.NumRows, 1, oRaster.DataType, rasterParameters);
                paRaster.Bounds = oRaster.Bounds.Copy();
                paRaster.NoDataValue = oRaster.NoDataValue;
                paRaster.Projection = oRaster.Projection;

                double oValue = 0;

                for (int oRows = 0; oRows < oRaster.NumRows - 1; oRows++)
                {
                    for (int oCols = 0; oCols < oRaster.NumColumns - 1; oCols++)
                    {
                        oValue = oRaster.Value[oRows, oCols];

                        if (oValue > 0)
                        {
                            paRaster.Value[oRows, oCols] = oValue;
                        }
                        else
                        {
                            paRaster.Value[oRows, oCols] = 1;
                        }
                    }
                }
                paRaster.Save();
                MessageBox.Show("Raster conversion and reclassification complete.  \n\n Add to display?", "Raster Operations", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }

            catch (System.InvalidCastException )
            {
                MessageBox.Show("Error 100: Wrong layer type selected. \r\n \r\nPlease choose a raster layer to convert.", "Error 100: Invalid Cast Exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

    #endregion

    #region Map Controls

        private void mpMain_GeoMouseMove(object sender, GeoMouseArgs e)
        {
            tsslCoordX.Text = "X: " + Math.Round(e.GeographicLocation.X, 4);
            tsslCoordY.Text = " Y: " + Math.Round(e.GeographicLocation.Y, 4);
        }

        private void rbtnPrintLayout_Click(object sender, EventArgs e)
        {
            _layout.ShowDialog(this);
        }

        private void rbtnLoadAttributes_Click(object sender, EventArgs e)
        {
            if (mpMain.Layers.SelectedLayer as MapPolygonLayer == null)
            {
                MessageBox.Show("Please select a polygon layer for attributes.", "Improper Layer Selected", MessageBoxButtons.OK);
                return;
            }
            IMapPolygonLayer  currentLayer = (IMapPolygonLayer)mpMain.Layers.SelectedLayer;

            DataTable curLay = null;
            curLay = currentLayer.DataSet.DataTable;
            dgAttributes.DataSource = curLay;
        }

        private void rbtnSaveAttChange_Click(object sender, EventArgs e)
        {

        }
        
        private void rbtnSelect_Click(object sender, EventArgs e)
        {
            mpMain.FunctionMode = DotSpatial.Controls.FunctionMode.Select;
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            mpMain.FunctionMode = DotSpatial.Controls.FunctionMode.ZoomIn;
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            mpMain.FunctionMode = DotSpatial.Controls.FunctionMode.ZoomOut;
        }

        private void btnPan_Click(object sender, EventArgs e)
        {
            mpMain.FunctionMode = DotSpatial.Controls.FunctionMode.Pan;
        }

        private void btnMaxExtents_Click(object sender, EventArgs e)
        {
            mpMain.ZoomToMaxExtent();
            mpMain.FunctionMode = DotSpatial.Controls.FunctionMode.None;
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            mpMain.FunctionMode = DotSpatial.Controls.FunctionMode.Info;
        }

        private void btnFixedZoomOut_Click(object sender, EventArgs e)
        {
            mpMain.ZoomOut();
        }

        private void btnFixedZoomIn_Click(object sender, EventArgs e)
        {
            mpMain.ZoomIn();
        }

        private void btnAddLayer_Click(object sender, EventArgs e)
        {
            mpMain.AddLayer();
        }

        //private void dgAttributes_SelectionChanged(object sender, EventArgs e)
        //{
        //    if (ribMain.ActiveTab.Text == "Map")
        //    {

        //        foreach (DataGridViewRow row in dgAttributes.SelectedRows)
        //        {
        //            MapPolygonLayer currentLayer = default(MapPolygonLayer);
        //            currentLayer = (MapPolygonLayer)mpMain.Layers[0];

        //            if (currentLayer == null) { MessageBox.Show("Please Select a Valid Shapefile", "Shapefile Error", MessageBoxButtons.OK, MessageBoxIcon.Error); break; }
        //            else { currentLayer.SelectByAttribute("[Id] = " + "'" + row.Cells["Id"].Value + "'"); }
        //        }
        //    }
        //    else if (ribMain.ActiveTab.Text == "Tool")
        //    {
        //        foreach (DataGridViewRow row in dgAttributes.SelectedRows)
        //        {
        //            MessageBox.Show("The Question is: \n\r" + row.Cells[1].Value, "Selected Row Example", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //    }
        //}

        //private void btnStartEnd_Click(object sender, EventArgs e)
        //{
        //    MessageBox.Show("Code to create Start and End point raster to go here.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //}

    #endregion

        #region OrbMenu Controls

        private void btnAbout_Click(object sender, EventArgs e)
        {
            frmAboutTool aboutTool = new frmAboutTool();
            aboutTool.ShowDialog();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Code for Help files to be placed here.", "Code Entry", MessageBoxButtons.OK);
        }

        private void ribOExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    #endregion

        #region Ribbon Tabs

        //private void ribMain_ActiveTabChanged(object sender, EventArgs e)
        //{
        //    var caseText = ribMain.ActiveTab.Text;

        //    switch (caseText)
        //    {
        //        case "Map":
        //        //    //lgMapMain.Visible = true;
        //        //    mpMain.Visible = true;
        //        //    //spltMapGraph.Panel2Collapsed = true;
        //        //    //spltMapGraph.Panel1Collapsed = false;
        //        //    //spltMapLegend.Panel1Collapsed = false;
        //        //    //spltCharts.Panel2Collapsed = true;
        //        //    //dgAttributes.Visible = true;
        //        //    //listBox1.Visible = false;
        //            break;
        //        //case "Operations":
        //        //    //spltMapGraph.Panel1Collapsed = true;
        //        //    //spltMapLegend.Panel1Collapsed = true;
        //        //    //spltCharts.Panel2Collapsed = false;
        //        //    //dgAttributes.Visible = false;
        //        //    //listBox1.Visible = true;
        //        //    break;
        //        case "Tool":
        //        //    //mpMain.Visible = true;
        //        //    //spltMapGraph.Panel1Collapsed = false;
        //        //    //spltMapLegend.Panel1Collapsed = false;
        //        //    //spltCharts.Panel2Collapsed = true;
        //        //    //dgAttributes.Visible = true;
        //        //    //listBox1.Visible = false;

        //            break;
        //        default:
        //        //    //lgMapMain.Visible = true;
        //        //    mpMain.Visible = true;
        //        //    //spltMapGraph.Panel2Collapsed = false;
        //        //    //spltMapLegend.Panel1Collapsed = false;
        //        //    //listBox1.Visible = false;
        //            break;
        //    }
        //}

        private void tabMapQuest_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPageIndex == 1)
            {
                dgQuesSets.AutoResizeColumns();
                dgQuesSets.Refresh();
            }
        }

        #endregion

        #region Monte Carlo Subs

        private void rbtnSelectWeights_Click(object sender, EventArgs e)
        {
            frmWeights addWeights = new frmWeights();
            addWeights.ShowDialog();
        }
        

        private double RandomNumber()
        {
            Random random = new Random();
            return random.NextDouble();
        }



#endregion

        #region LCP

        //regional variables
        //clsleastCostPath lcp = new clsleastCostPath();
        double accumCost = 0;
        Dictionary<string, double> lcv = new Dictionary<string, double>();
        Dictionary<string, double> VisitedList = new Dictionary<string, double>();
        double noDataValue = -999999;
        double startValue = 1234567890;
        int passNumber = 0;
        int numRows = 0;
        int numCols = 0;
        int endRow = 0;
        int endCol = 0;
        string aCostRas = "";

        public void mpMain_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {


            Coordinate xy = null;
            int seRow = 0;
            int seCol = 0;
            //double seCellValue = 0;

            if (mouseUPBool == true)
            {

                if (mpMain.Layers.SelectedLayer as MapRasterLayer == null)
                {
                    MessageBox.Show("Please select a raster layer to select start and end points.", "Wrong Layer Selected", MessageBoxButtons.OK);
                    return;
                }

                IMapRasterLayer seLayer = (IMapRasterLayer)mpMain.Layers.SelectedLayer;

                if (seLayer == null)
                {
                    MessageBox.Show("Please select a layer to set start and end points.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    IRaster seRaster = seLayer.DataSet;
                    xy = mpMain.PixelToProj(e.Location);
                    RcIndex  rasterXY = DotSpatial.Data.RasterExt.ProjToCell(seRaster, xy);
                    seRow = rasterXY.Row;
                    seCol = rasterXY.Column;
                    numCols = seRaster.NumColumns;
                    numRows = seRaster.NumRows;
                    if (seCol > 0 & seCol < seRaster.NumColumns & seRow > 0 & seRow < seRaster.NumRows)
                    {
                        if (seInt == 1)
                        {
                            startR = seRow;
                            startC = seCol;
                            startP = seRaster.Value[seRow, seCol];
                            tssStart.Text = "Start- Row: " + Convert.ToString(seRow) + " Column: " + Convert.ToString(seCol);
                            seInt++;
                        }
                        else if (seInt == 2)
                        {
                            endR = seRow;
                            endC = seCol;
                            endP = seRaster.Value[seRow, seCol];
                            tssEnd.Text = "End- Row: " + Convert.ToString(seRow) +" Column: " + Convert.ToString(seCol);
                            seInt = 1;
                            mouseUPBool = false;
                        }
                        //seCellValue= seRaster.Value[seRow,seCol];
                    }
                    else
                    {
                        MessageBox.Show("Value is outside raster bounds.  Please select an area on the map.", "Error 120: Selection Out of Bounds", MessageBoxButtons.OK);
                    }
                    
                }

                //MessageBox.Show("Point \r\n X: " + seRow +" Y: " + seCol+ " with a value of: " + seCellValue +" was clicked.", "Info", MessageBoxButtons.OK);
            }

        }

        public void ribLCP_Click(object sender, EventArgs e)
        {
            //create new raster layer
            int numIts  = 1;

            SaveFileDialog sfRaster = new SaveFileDialog();
            sfRaster.Filter = "DotSpatial Raster|*.bdg";
            sfRaster.Title = "Save an Image File";
            sfRaster.ShowDialog();
            aCostRas = sfRaster.FileName;
            string[] rasOptions = new string[1];
            IRaster accumCost = Raster.CreateRaster(aCostRas, null, numCols, numRows, 1, typeof (System.Double), rasOptions);
            for (int n =0; n <= numIts; n++)
            {
                //for each iteration through get accumulative cost raster.

                //call Monte Carlo Process and apply weights to raster

                //keep minimum weight value in accumulative cost raster

                //bring in utility costs and divide values

                //create LCP



            }


            accumCost.Save();
            mpMain.Layers.Add(accumCost);

        }
        
        System.Data.DataSet dsQuesSets = new System.Data.DataSet();

        public void rbtnImportDataFile_Click(object sender, EventArgs e)
        {
            //frmCreateAnalysisFile createAnalysisFile = new frmCreateAnalysisFile();
            //createAnalysisFile.ShowDialog();
            dsQuesSets.Clear();
            dsQuesSets.Dispose();
            dgQuesSets.DataSource = null;
            string importDataFile = "select * from " + "XMLTrial2.csv";
            string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=C:\Users\Robert\Documents\LDRD\Data\XMLTrials; Extended Properties=""text;HDR=YES;FMT=Delimited;"";";
            OleDbConnection myConnection = new OleDbConnection(ConnectionString);
            myConnection.Open();
            OleDbDataAdapter oleDa = new OleDbDataAdapter(importDataFile, ConnectionString);

            oleDa.Fill(dsQuesSets);
            if (dsQuesSets.Tables[0].Columns.Contains("RasterLayer"))
            {

            }
            else
            {
                dsQuesSets.Tables[0].Columns.Add("RasterLayer", typeof(System.String));
            }

            dgQuesSets.DataSource = dsQuesSets.Tables[0];
            dgQuesSets.AutoResizeColumns();
            //rbtnStart.Enabled = true;
            //rbtnSelectWeights.Enabled = true;

            //myConnection.Close();
        }

        public void rbtnSaveDataFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog dataSetSF = new SaveFileDialog();
            dataSetSF.Filter = "LineSiter Data File|*.lsx";
            dataSetSF.Title = "Save an Image File";
            if (dataSetSF.ShowDialog() == DialogResult.OK)
            {
                dsQuesSets.WriteXml(dataSetSF.FileName);
            }
        }

        //private string colHeaderName = null;
        //private DataGridViewCell clickedCell;

        //private void dgQuesSets_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    // If the user right-clicks a cell, store it for use by the shortcut menu.
        //    int colHeader = 0;
        //    string rsFileName = null;
        //    DataGridView.HitTestInfo hit = dgQuesSets.HitTest(e.X, e.Y);
        //    if (hit.Type == DataGridViewHitTestType.Cell)
        //    {
        //        colHeader = hit.ColumnIndex;
        //        colHeaderName = dgQuesSets.Columns[colHeader].HeaderText;
        //        clickedCell = dgQuesSets.Rows[hit.RowIndex].Cells[hit.ColumnIndex];

        //        //MessageBox.Show("Header: " + colHeaderName + " was just clicked.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //    if (colHeaderName == "RasterLayer")
        //    {
        //        selectionDiag newLayerSelect = new selectionDiag(mpMain);
        //        newLayerSelect.ShowDialog();

        //        if (newLayerSelect.DialogResult == DialogResult.OK)
        //        {
        //            clickedCell.Value = newLayerSelect.getRLayer.layerName;
        //        }

                //foreach (ILayer items in mpMain.Layers)
                //{
                //    MessageBox.Show(Convert.ToString(items.LegendText), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                //OpenFileDialog RasterOFD = new OpenFileDialog();
                ////RasterOFD.Filter = "DotSpatial Raster | *.bdg";
                //RasterOFD.Title = "Add Raster File to Dataset";
                //if (RasterOFD.ShowDialog() == DialogResult.OK)
                //{
                //    clickedCell.Value = RasterOFD.FileName;
                //}
        //    }
        //}

        //private void rbtnEditRasterCells_Click(object sender, EventArgs e)
        //{

        //}


        
        
        //private void calculateCells(int lcpRow, int lcpCol, double centerCell)
        //{
        //    //calculate the cost values within a moving window
            
        //    bool[] validCBlock = new bool[4];
        //    passNumber++;
        //    listBox1.Items.Add("Starting Center Cell is: " + Convert.ToString(lcpRow) + "," + Convert.ToString(lcpCol));
        //    listBox1.Items.Add("************************************************************************************");
        //    listBox1.Items.Add("Passnumber: " + Convert.ToString(passNumber));
        //    listBox1.Items.Add("-----------------------------------------------------");
        //    validCBlock = lcp.validCellBlock(lcpCol, lcpRow, numRows, numCols);

        //    //assign center cell accumulated costs for starting cell
        //    centerCell = 0;
        //    if (lcp.FirstRun == true)
        //    {
        //        string startCoords = Convert.ToString(lcpRow) + "," + Convert.ToString(lcpCol);
        //        string endCoords = Convert.ToString(endRow) + "," + Convert.ToString(endCol);
        //        textArray2[lcpRow, lcpCol].Text = Convert.ToString(0);
        //        if (!(VisitedList.ContainsKey(startCoords)))
        //        {
        //            VisitedList.Add(startCoords, 1234567890);
        //        }
        //        //if (!(VisitedList.ContainsKey(endCoords)))
        //        //{
        //        //    VisitedList.Add(endCoords, 9876543210);
        //        //}
        //        centerCell = Convert.ToDouble(textArray1[lcpRow, lcpCol].Text);
        //        listBox1.Items.Add("Center Cell (" + Convert.ToString(lcpRow) + "," + Convert.ToString(lcpCol) + "): " + Convert.ToString(Convert.ToDouble(textArray1[lcpRow, lcpCol].Text)) + " = " + Convert.ToString(centerCell));
        //        lcp.AccumCost = 0;
        //    }
        //    else if (lcp.FirstRun == false)
        //    {
        //        if (VisitedList[Convert.ToString(lcpRow) + "," + Convert.ToString(lcpCol)] == 1234567890)
        //        {
        //            accumCost = 0;
        //        }
        //        else
        //        {
        //            accumCost = VisitedList[Convert.ToString(lcpRow) + "," + Convert.ToString(lcpCol)];
        //        }
        //        centerCell = Convert.ToDouble(textArray1[lcpRow, lcpCol].Text);
        //        listBox1.Items.Add("Center Cell (" + Convert.ToString(lcpRow) + "," + Convert.ToString(lcpCol) + "): " + Convert.ToString(Convert.ToDouble(textArray1[lcpRow, lcpCol].Text)) + " = " + Convert.ToString(centerCell));
        //        lcp.AccumCost = 0;
        //    }

        //    //Check cell blocks to find edge of grid

        //    //South Cellblock 3,2,1
        //    if (validCBlock[2] == true)
        //    {
        //        string coord = null;
        //        //S
        //        double S = Convert.ToDouble(textArray1[lcpRow + 1, lcpCol].Text);
        //        if (S != -999999)
        //        {
        //            double sval = ((Math.Round(accumCost + ((S + centerCell) / 2), 2)));
        //            coord = Convert.ToString(lcpRow + 1) + "," + Convert.ToString(lcpCol);
        //            if (!(VisitedList.ContainsKey(coord)))
        //            {
        //                listBox1.Items.Add("South (" + Convert.ToString(lcpRow + 1) + "," + Convert.ToString(lcpCol) + "): " + Convert.ToString(accumCost) + " (" + Convert.ToString(S) + " + " + Convert.ToString(centerCell) + ")) / 2 = " + Convert.ToString(sval));
        //                VisitedList.Add(coord, sval);
        //                if (!(lcv.ContainsKey(coord)))
        //                {
        //                    lcv.Add(coord, sval);
        //                }
        //                textArray2[lcpRow + 1, lcpCol].Text = Convert.ToString(sval);
        //            }
        //        }

        //        //SE
        //        if (validCBlock[3] == true)
        //        {
        //            double SE = Convert.ToDouble(textArray1[lcpRow + 1, lcpCol + 1].Text);
        //            if (SE != -999999)
        //            {
        //                double SEVal = Math.Round(accumCost + ((Math.Sqrt(2) * (Convert.ToInt32(SE + centerCell))) / 2), 2);
        //                coord = Convert.ToString(lcpRow + 1) + "," + Convert.ToString(lcpCol + 1);
        //                if (!(VisitedList.ContainsKey(coord)))
        //                {
        //                    listBox1.Items.Add("Southeast (" + Convert.ToString(lcpRow + 1) + "," + Convert.ToString(lcpCol + 1) + "): " + Convert.ToString(accumCost) + "(√2 * (" + Convert.ToString(SE) + " + " + Convert.ToString(centerCell) + ")) / 2 = " + Convert.ToString(SEVal));
        //                    VisitedList.Add(coord, SEVal);
        //                    if (!(lcv.ContainsKey(coord)))
        //                    {
        //                        lcv.Add(coord, SEVal);
        //                    }
        //                    textArray2[lcpRow + 1, lcpCol + 1].Text = Convert.ToString(SEVal);
        //                }
        //            }
        //        }

        //        //SW
        //        if (validCBlock[1] == true)
        //        {
        //            double SW = Convert.ToDouble(textArray1[lcpRow + 1, lcpCol - 1].Text);
        //            if (SW != -999999)
        //            {
        //                double SWVal = Math.Round(accumCost + ((Math.Sqrt(2) * (Convert.ToInt32(SW + centerCell))) / 2), 2);
        //                coord = Convert.ToString(lcpRow + 1) + "," + Convert.ToString(lcpCol - 1);
        //                if (!(VisitedList.ContainsKey(coord)))
        //                {
        //                    listBox1.Items.Add("Southwest (" + Convert.ToString(lcpRow + 1) + "," + Convert.ToString(lcpCol - 1) + "): " + Convert.ToString(accumCost) + " ( √2 *(" + Convert.ToString(SW) + " + " + Convert.ToString(centerCell) + ")) / 2 = " + Convert.ToString(SWVal));
        //                    VisitedList.Add(coord, SWVal);
        //                    if (!(lcv.ContainsKey(coord)))
        //                    {
        //                        lcv.Add(coord, SWVal);
        //                    }
        //                    textArray2[lcpRow + 1, lcpCol - 1].Text = Convert.ToString(SWVal);
        //                }
        //            }
        //        }
        //    }

        //    //North Cellblock 5,6,7
        //    if (validCBlock[0] == true)
        //    {
        //        string coord = null;
        //        //North
        //        double N = Convert.ToDouble(textArray1[lcpRow - 1, lcpCol].Text);
        //        if (N != -999999)
        //        {
        //            double NVal = (Math.Round(accumCost + ((N + centerCell) / 2), 2));
        //            coord = Convert.ToString(lcpRow - 1) + "," + Convert.ToString(lcpCol);
        //            if (!(VisitedList.ContainsKey(coord)))
        //            {
        //                listBox1.Items.Add("North (" + Convert.ToString(lcpRow - 1) + "," + Convert.ToString(lcpCol) + "):" + Convert.ToString(accumCost) + " (" + Convert.ToString(N) + " + " + Convert.ToString(centerCell) + ") / 2 = " + Convert.ToString(NVal));
        //                VisitedList.Add(coord, NVal);
        //                if (!(lcv.ContainsKey(coord)))
        //                {
        //                    lcv.Add(coord, NVal);
        //                }
        //                textArray2[lcpRow - 1, lcpCol].Text = Convert.ToString(NVal);
        //            }
        //        }

        //        //Northwest
        //        if (validCBlock[1] == true)
        //        {
        //            double NW = Convert.ToDouble(textArray1[lcpRow - 1, lcpCol - 1].Text);
        //            if (NW != -999999)
        //            {
        //                double NWVal = Math.Round(accumCost + (Math.Sqrt(2) * (Convert.ToInt32(NW + centerCell)) / 2), 2);
        //                coord = Convert.ToString(lcpRow - 1) + "," + Convert.ToString(lcpCol - 1);
        //                if (!(VisitedList.ContainsKey(coord)))
        //                {
        //                    listBox1.Items.Add("Northwest (" + Convert.ToString(lcpRow - 1) + "," + Convert.ToString(lcpCol - 1) + "): " + Convert.ToString(accumCost) + " + (√2 * (" + Convert.ToString(NW) + " + " + Convert.ToString(centerCell) + ")) / 2 = " + Convert.ToString(NWVal));
        //                    VisitedList.Add(coord, NWVal);
        //                    if (!(lcv.ContainsKey(coord)))
        //                    {
        //                        lcv.Add(coord, NWVal);
        //                    }
        //                    textArray2[lcpRow - 1, lcpCol - 1].Text = Convert.ToString(NWVal);
        //                }
        //            }
        //        }

        //        //Northeast
        //        if (validCBlock[3] == true)
        //        {
        //            double NE = Convert.ToDouble(textArray1[lcpRow - 1, lcpCol + 1].Text);
        //            if (NE != -999999)
        //            {
        //                double NEVal = Math.Round(accumCost + (((Math.Sqrt(2) * (Convert.ToInt32(NE + centerCell))) / 2)), 2);
        //                coord = Convert.ToString(lcpRow - 1) + "," + Convert.ToString(lcpCol + 1);
        //                if (!(VisitedList.ContainsKey(coord)))
        //                {
        //                    listBox1.Items.Add("Northeast (" + Convert.ToString(lcpRow - 1) + "," + Convert.ToString(lcpCol + 1) + "): " + Convert.ToString(accumCost) + " + ( (√2 * (" + Convert.ToString(NE) + " + " + Convert.ToString(centerCell) + ") /  2) = " + Convert.ToString(NEVal));
        //                    VisitedList.Add(coord, NEVal);
        //                    if (!(lcv.ContainsKey(coord)))
        //                    {
        //                        lcv.Add(coord, NEVal);
        //                    }
        //                    textArray2[lcpRow - 1, lcpCol + 1].Text = Convert.ToString(NEVal);
        //                }
        //            }
        //        }
        //    }

        //    //East Center Cell 8
        //    if (validCBlock[3] == true)
        //    {
        //        double E = Convert.ToDouble(textArray1[lcpRow, lcpCol + 1].Text);
        //        if (E != -999999)
        //        {
        //            double EVal = (Math.Round((accumCost + ((E + centerCell) / 2)), 2));
        //            string coord = Convert.ToString(lcpRow) + "," + Convert.ToString(lcpCol + 1);
        //            if (!(VisitedList.ContainsKey(coord)))
        //            {
        //                listBox1.Items.Add("East (" + Convert.ToString(lcpRow) + "," + Convert.ToString(lcpCol + 1) + "):  " + Convert.ToString(accumCost) + " + (" + Convert.ToString(E) + " + " + Convert.ToString(centerCell) + ") / 2 = " + Convert.ToString(EVal));
        //                VisitedList.Add(coord, EVal);
        //                if (!(lcv.ContainsKey(coord)))
        //                {
        //                    lcv.Add(coord, EVal);
        //                }
        //                textArray2[lcpRow, lcpCol + 1].Text = Convert.ToString(EVal);
        //            }
        //        }
        //    }

        //    //West Center Cell 4
        //    if (validCBlock[1] == true)
        //    {
        //        double W = Convert.ToDouble(textArray1[lcpRow, lcpCol - 1].Text);
        //        if (W != -999999)
        //        {
        //            double WVal = (Math.Round((accumCost + ((W + centerCell) / 2)), 2));
        //            string coord = Convert.ToString(lcpRow) + "," + Convert.ToString(lcpCol - 1);
        //            if (!(VisitedList.ContainsKey(coord)))
        //            {
        //                listBox1.Items.Add("West (" + Convert.ToString(lcpRow) + "," + Convert.ToString(lcpCol - 1) + "): (" + Convert.ToString(W) + " + " + Convert.ToString(centerCell) + ") / 2 = " + Convert.ToString(WVal));
        //                VisitedList.Add(coord, WVal);
        //                if (!lcv.ContainsKey(coord))
        //                {
        //                    lcv.Add(coord, WVal);
        //                }
        //                textArray2[lcpRow, lcpCol - 1].Text = Convert.ToString(WVal);
        //            }
        //        }
        //    }
        //    string lcpValues = "The cell values in the list are: \n";
        //    listBox1.Items.Add("-----------------------------------------------------");
        //    listBox1.Items.Add("The cell values in the costs list are: ");
        //    int numberIts = 0;

        //    foreach (var lcpEntry in lcv)
        //    {
        //        numberIts++;
        //        listBox1.Items.Add("Coordinate: " + Convert.ToString(lcpEntry.Key) + "  Value: " + Convert.ToString(lcpEntry.Value));
        //    }

        //    lcpValues += "The values in the visits table are: \n";
        //    listBox1.Items.Add("--------------------------------------------------");
        //    listBox1.Items.Add("The values in the visits table are: ");
        //    foreach (var visitEntry in VisitedList)
        //    {
        //        listBox1.Items.Add("Coordinate: " + Convert.ToString(visitEntry.Key) + "  Value: " + Convert.ToString(visitEntry.Value));
        //    }

        //    lcv.Remove(Convert.ToString(lcpRow) + "," + Convert.ToString(lcpCol));
        //    listBox1.Items.Add("Removed Center Cell from costs list.  Center Cell is at Coordinate: " + Convert.ToString(lcpRow) + "," + Convert.ToString(lcpCol));


        //}
        #endregion

    }
}
