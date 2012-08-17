using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Symbology;
using DotSpatial.Topology;
using System;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

//using System.Deployment.Application;
//using DevExpress.XtraBars;

namespace LineSiterSitingTool
{
    public partial class frmNewMain : Form
    {
        public frmNewMain()
        {
            InitializeComponent();
            appManager1.LoadExtensions();
            mpMain.GeoMouseMove += mpMain_GeoMouseMove;
            //tspProg1.Visible = false;
        }

        private bool mouseUPBool = false;
        private int seInt = 1;
        private double startP = 0;
        private double endP = 0;
        private int endR = 0;
        private int endC = 0;
        private int startR = 0;
        private int startC = 0;
        private clsLCPCoords lc = new clsLCPCoords();
        private string projSaveFile = null;
        private string projSavePath = string.Empty;
        private Coordinate startXY = new Coordinate();
        private Coordinate endXY = new Coordinate();
        private string surveyFile = string.Empty;
        private bool wired = false;

        #region Ribbon Controls

        private void qButAddLayer_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            mpMain.AddLayers();
        }

        private void qButZoomIn_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            mpMain.FunctionMode = DotSpatial.Controls.FunctionMode.ZoomIn;
        }

        private void qButZoomOut_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            mpMain.FunctionMode = DotSpatial.Controls.FunctionMode.ZoomOut;
        }

        private void qButFixedZoomIn_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            mpMain.ZoomIn();
        }

        private void qButFixedZoomOut_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            mpMain.ZoomOut();
            //DotSpatial.Controls.LayoutForm pp = new LayoutForm();
            //pp.MapControl = mpMain;
            //pp.ShowDialog();
        }

        private void qButPan_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            mpMain.FunctionMode = DotSpatial.Controls.FunctionMode.Pan;
        }

        private void qButMaxExtent_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            mpMain.ZoomToMaxExtent();
            mpMain.FunctionMode = DotSpatial.Controls.FunctionMode.None;
        }

        private void qButInfo_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            mpMain.FunctionMode = DotSpatial.Controls.FunctionMode.Info;
        }

        private void qButSelect_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            mpMain.FunctionMode = DotSpatial.Controls.FunctionMode.Select;
        }

        private void qButLoadAtts_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            DataTable curLay = null;
            if (mpMain.Layers.SelectedLayer as MapPolygonLayer != null)
            {
                IMapPolygonLayer currentLayer = (IMapPolygonLayer)mpMain.Layers.SelectedLayer;
                curLay = currentLayer.DataSet.DataTable;
            }
            else if (mpMain.Layers.SelectedLayer as MapLineLayer != null)
            {
                IMapLineLayer currentLayer = (IMapLineLayer)mpMain.Layers.SelectedLayer;
                curLay = currentLayer.DataSet.DataTable;
            }
            else
            {
                MessageBox.Show("Please select a polygon layer for attributes.", "Improper Layer Selected", MessageBoxButtons.OK);
                return;
            }
            dgAttributes.DataSource = curLay;
        }

        private void qButLCP_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            //if (!Directory.Exists(@"c:\temp\LineSiter"))
            //{
            //    Directory.CreateDirectory(@"c:\temp\LineSiter");
            //}
            frmToolExecute newExecute = new frmToolExecute(mpMain, lc, projSavePath, surveyFile);
            newExecute.ShowDialog();
        }

        private void qButReclass_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            frmReclass rcls = new frmReclass(mpMain, projSavePath);
            rcls.ShowDialog();
        }

        private void qButExit_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            Application.Exit();
        }

        #endregion Ribbon Controls

        private void mpMain_GeoMouseMove(object sender, GeoMouseArgs e)
        {
            tsslXCoord.Text = "X: " + Math.Round(e.GeographicLocation.X, 4);
            tsslYCoord.Text = " Y: " + Math.Round(e.GeographicLocation.Y, 4);
        }

        private void frmNewMain_Load(object sender, EventArgs e)
        {
            this.Text = "LineSiter  " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public void mpMain_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Coordinate xy = null;
            int seRow = 0;
            int seCol = 0;
            //  double seCellValue = 0;
            string seLayerText;
            if (mouseUPBool == true)
            {
                if (mpMain.Layers.SelectedLayer as MapRasterLayer == null)
                {
                    MessageBox.Show("Please select a raster layer to select start and end points.", "Wrong Layer Selected", MessageBoxButtons.OK);
                    return;
                }

                IMapRasterLayer seLayer = (IMapRasterLayer)mpMain.Layers.SelectedLayer;
                seLayerText = seLayer.LegendText;

                if (seLayer == null)
                {
                    MessageBox.Show("Please select a layer to set start and end points.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    IRaster seRaster = seLayer.DataSet;
                    xy = mpMain.PixelToProj(e.Location);
                    RcIndex rasterXY = DotSpatial.Data.RasterExt.ProjToCell(seRaster, xy);
                    seRow = rasterXY.Row;
                    seCol = rasterXY.Column;
                    if (seCol > 0 & seCol < seRaster.NumColumns & seRow > 0 & seRow < seRaster.NumRows)
                    {
                        if (seInt == 1)
                        {
                            startR = seRow;
                            startC = seCol;
                            startP = seRaster.Value[seRow, seCol];
                            startXY = xy;
                            // todo, update to use custom panels.
                            //App.ProgressHandler.Progress(" tsslStart", 0, "Start- Row: " + Convert.ToString(seRow) + " Column: " + Convert.ToString(seCol));
                            lc.startRow = rasterXY.Row;
                            lc.startCol = rasterXY.Column;
                            seInt++;
                        }
                        else if (seInt == 2)
                        {
                            endR = seRow;
                            endC = seCol;
                            endP = seRaster.Value[seRow, seCol];
                            endXY = xy;
                            // todo, update to use custom panels.
                            //App.ProgressHandler.Progress(" tsslEnd", 0, "End- Row: " + Convert.ToString(seRow) + " Column: " + Convert.ToString(seCol));
                            lc.EndRow = rasterXY.Row;
                            lc.EndCol = rasterXY.Column;
                            if (seInt == 2)
                            {
                                frmPointSave ps = new frmPointSave(lc.startRow, lc.startCol, lc.EndRow, lc.EndCol, mpMain, startXY, endXY, projSavePath, seLayerText);
                                ps.ShowDialog();
                                mpMain.MouseUp -= mpMain_MouseUp;
                                Cursor = Cursors.Arrow;
                                //MessageBox.Show("The beginning coordinates you chose are: " + Convert.ToString(lc.startRow) + ", " + Convert.ToString(lc.startCol) + " and the ending coordinates are: " + Convert.ToString(lc.EndRow) + ", " + Convert.ToString(lc.EndCol) + "\n\n  Coordinates saved.", "Raster Coordinates", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
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
                e = null;
                xy = null;
                //MessageBox.Show("Point \r\n X: " + seRow +" Y: " + seCol+ " with a value of: " + seCellValue +" was clicked.", "Info", MessageBoxButtons.OK);
            }
        }

        private void qButStartEnd_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            mouseUPBool = true;
            //lc.startCol = -999;
            //lc.startRow = -999;
            //lc.EndCol = -999;
            //lc.EndRow = -999;
            if (wired)
            {
                mpMain.MouseUp -= mpMain_MouseUp;
            }
            mpMain.MouseUp += new MouseEventHandler(mpMain_MouseUp);
            Cursor = Cursors.Cross;
        }

        private void qButOpen_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            tsslOpen.Visible = true;
            tsslProjName.Text = "Project Loading Please Wait...";
            OpenFileDialog opProjFile = new OpenFileDialog();
            opProjFile.Filter = "DotSpatial Project Files (*.dspx)|*.dspx";
            if (opProjFile.ShowDialog() == DialogResult.OK)
            {
                appManager1.SerializationManager.OpenProject(opProjFile.FileName);
                projSaveFile = opProjFile.FileName;
            }
            else
            {
                tsslOpen.Visible = false;
                tsslProjName.Text = "No Project Loaded";
                return;
            }
            tsslProjName.Text = "Project: " + projSaveFile;
            mpMain.Refresh();
            mpMain.ZoomToMaxExtent();
            tsslOpen.Visible = false;
        }

        private void qButSave_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            tsslSave.Visible = true;
            if (projSaveFile != null)
            {
                appManager1.SerializationManager.SaveProject(projSaveFile);
            }
            else
            {
                SaveFileDialog svProjFile = new SaveFileDialog();
                svProjFile.Filter = "DotSpatial Project Files (*.dspx)|*.dspx";
                if (svProjFile.ShowDialog() == DialogResult.OK)
                {
                    appManager1.SerializationManager.SaveProject(svProjFile.FileName);
                    projSaveFile = svProjFile.FileName;
                }
                tsslProjName.Text = projSaveFile;
            }
            tsslSave.Visible = false;
        }

        private void qButSaveAs_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            tsslSave.Visible = true;
            SaveFileDialog svProjFile = new SaveFileDialog();
            svProjFile.Filter = "DotSpatial Project Files (*.dspx)|*.dspx";
            svProjFile.Title = "Save LineSiter Project File";
            svProjFile.CreatePrompt = true;
            svProjFile.OverwritePrompt = true;
            if (svProjFile.ShowDialog() == DialogResult.OK)
            {
                appManager1.SerializationManager.SaveProject(svProjFile.FileName);
                projSaveFile = svProjFile.FileName;
            }
            tsslProjName.Text = "Project:" + projSaveFile;
            tsslSave.Visible = false;
        }

        private void qButUtilCost_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            frmCreateUtilityRaster cut = new frmCreateUtilityRaster(mpMain, projSavePath);
            cut.ShowDialog();
        }

        private void qButSetPath_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            fbd.ShowDialog();
            projSavePath = fbd.SelectedPath;
        }

        private void qButBRaster_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            try
            {
                frmCreateBoundsRaster frmCBR = new frmCreateBoundsRaster(mpMain, projSavePath);
                frmCBR.ShowDialog();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + "\n has occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void qButSETest_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            try
            {
                MCTest mc = new MCTest(surveyFile);
                mc.ShowDialog();
            }

            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex));
                return;
            }

            #region OLD

            //if (mpMain.Layers.SelectedLayer as MapRasterLayer == null)
            //{
            //    MessageBox.Show("Please select a raster layer to test start and end points.", "Wrong Layer Selected", MessageBoxButtons.OK);
            //    return;
            //}

            //IMapRasterLayer testLayer = (IMapRasterLayer)mpMain.Layers.SelectedLayer;
            //IRaster tLayer = testLayer.DataSet;

            //for (int oRows = 0; oRows < testLayer.NumRows - 1; oRows++)
            //{
            //    for (int oCols = 0; oCols < testLayer.NumColumns - 1; oCols++)
            //    {
            //        if (tLayer.Value[oRows, oCols] == 1234567890)
            //        {
            //            MessageBox.Show("Start point found at \n " + " Row: " + Convert.ToString(oRows) + " Column: " + Convert.ToString(oCols), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //        else if (tLayer.Value[oRows, oCols] == 0987654321)
            //        {
            //            MessageBox.Show("End point found at " + " Row: " + Convert.ToString(oRows) + " Column: " + Convert.ToString(oCols), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //    }
            //}

            #endregion OLD
        }

        private void qButNormal_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            mpMain.FunctionMode = DotSpatial.Controls.FunctionMode.None;
            mpMain.ClearSelection();
        }

        private void qButSurFile_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            try
            {
                OpenFileDialog dataSetFill = new OpenFileDialog();
                dataSetFill.Title = "Select File Name for Project Survey Dataset";
                /*Nullable <bool> result = dataSetFill.ShowDialog();*/
                bool exists = dataSetFill.CheckFileExists;
                if (dataSetFill.ShowDialog() == DialogResult.OK)
                {
                    if (exists)
                    {
                        surveyFile = dataSetFill.FileName;
                        tslSurveyData.Text = "Survey Data File: " + surveyFile;
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + "has occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void qButSubtract1_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            frmSubtract sub = new frmSubtract(mpMain, projSavePath);
            sub.ShowDialog();
        }

        private void qButNew_ItemActivated(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            appManager1.SerializationManager.New();
        }

        private void qButPrint_ItemActivated_1(object sender, Qios.DevSuite.Components.QCompositeEventArgs e)
        {
            DotSpatial.Controls.LayoutForm pp = new LayoutForm();
            pp.MapControl = mpMain;
            pp.ShowDialog();
        }
    }
}