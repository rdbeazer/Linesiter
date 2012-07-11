using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Symbology;
using DotSpatial.Topology;
using DotSpatial.Projections;
using System.IO;

namespace nx09SitingTool
{
    public partial class frmPointSave : Form
    {

        int _startRow = 0, _startCol = 0, _endRow = 0, _endCol = 0;
        IMap _MW;
        FileInfo pathInfo = null;
        List<Coordinate> shCoords = new List<Coordinate>();
        BackgroundWorker worker = new BackgroundWorker();
        string statusMessage;
        FeatureSet startEndPoints = new FeatureSet(FeatureType.Point);
        string savedElements = "";
        string bndRasterText = string.Empty;
        
                
        public frmPointSave(int startRow, int startCol, int endRow, int endCol, IMap mappingWindow, Coordinate startXY, Coordinate endXY, string projSavePath, string seLayerTxt)
        {
            shCoords.Clear();
            InitializeComponent();
            _startRow = startRow;
            _startCol = startCol;
            _endRow = endRow;
            _endCol = endCol;
            _MW = mappingWindow;
            shCoords.Add(startXY);
            shCoords.Add(endXY);
            txtEndCoords.Enabled = false;
            txtStartCoords.Enabled = false;
            setTextboxes();
            fillCombo();
            loadRtb();
            bndRasterText = seLayerTxt;
            txtBndingExt.Text = seLayerTxt;
            if (projSavePath != string.Empty)
            {
                txtSaveLocation.Text = projSavePath;
                FileInfo _pathInfo = new FileInfo(projSavePath);
                pathInfo = _pathInfo;
            }
        }

        private void setTextboxes()
        {
            txtStartCoords.Text = Convert.ToString(_startRow) + ", " + Convert.ToString(_startCol);
            txtEndCoords.Text = Convert.ToString(_endRow) + ", " + Convert.ToString(_endCol);
        }

        private void fillCombo()
        {
            foreach (Layer lay in _MW.Layers)
            {
                if (lay.GetType() == typeof(DotSpatial.Controls.MapRasterLayer))
                {
                    cboLayers.Items.Add(lay.LegendText);
                }
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.MyDocuments;
            fbd.ShowDialog();
            //SaveFileDialog svRas = new SaveFileDialog();
            ////svRas.Filter = "DotSpatial Raster (*.bgd) | *.bgd";
            //svRas.ShowDialog();
            FileInfo _pathInfo =  new FileInfo(fbd.SelectedPath);
            pathInfo = _pathInfo;
            txtSaveLocation.Text = fbd.SelectedPath;
            //saveShapeFile();
        }

        private void loadRtb()
        {
            rtbHelpBox.SelectionFont = new Font("Arial", 12f, FontStyle.Bold);
            rtbHelpBox.AppendText("Save Source Cell Raster \n \n");
            rtbHelpBox.SelectionFont = new Font("Arial", 10f, FontStyle.Regular);
            rtbHelpBox.AppendText("This process creates a raster identifying the starting and ending points selected from  ");
            rtbHelpBox.AppendText("the map interface in the previous step.  Coordinates for each point are shown in row,column ");
            rtbHelpBox.AppendText("format.  The bounding raster defines the number of rows and columns to be used ");
            rtbHelpBox.AppendText("and provides an extent for the creation of the raster.  Finally, the user is given  ");
            rtbHelpBox.AppendText("the options of creating a shapefile in addition to the raster and where to save the ");
            rtbHelpBox.AppendText("created files.  This raster is needed in the cost accumulation function of the least cost path analysis.");
        }

        private void saveShapeFile()
        {
            try
            {
                string pathS = pathInfo.FullName + @"\startEndPoints.shp";
                startEndPoints.Name = "start end points";
                statusMessage = "Creating Shapefile";
                DataColumn pointID = new DataColumn("PointID");
                int pID = 0;
                startEndPoints.DataTable.Columns.Add(pointID);

                //startEndPoints.Projection = KnownCoordinateSystems.Projected.UtmNad1983.NAD1983UTMZone12N;

                startEndPoints.Projection = _MW.Projection;
                foreach (Coordinate xys in shCoords)
                {
                    DotSpatial.Topology.Point shpPoint = new DotSpatial.Topology.Point(xys);
                    IFeature fs = startEndPoints.AddFeature(shpPoint);
                    fs.DataRow["PointID"] = pID;
                    pID++;
                }
                statusMessage = "Saving Shapefile";
                startEndPoints.SaveAs(pathS, true);
                addShapeFileToMap(startEndPoints);
                //saveRasterFile();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + "\n has occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void addShapeFileToMap(FeatureSet shapefileToAdd)
        {
            _MW.Layers.Add(shapefileToAdd);
            _MW.ResetBuffer();
        }

        private void addRasterToMap(IRaster rasterToAdd)
        {
            
            _MW.Layers.Add(rasterToAdd);
            _MW.ResetBuffer();
        }

        private void saveRasterFile()
        {
            try
            {
                IRaster newRast = new Raster();
                string[] rasOps = new string[1];
             //int progress = 0;
                foreach (Layer lay in _MW.GetLayers())
                {
                    if (lay.LegendText == cboLayers.SelectedItem)
                    {
                        newRast = (IRaster)lay.DataSet;
                    }

                }
                string pathS = pathInfo.FullName;
                statusMessage = "Creating Rasters";
                IRaster startPoint = Raster.CreateRaster(pathS + @"\startPoint.bgd", null, newRast.NumColumns, newRast.NumRows, 1, typeof(int), rasOps);
                IRaster endPoint = Raster.CreateRaster(pathS + @"\endPoint.bgd", null, newRast.NumColumns, newRast.NumRows, 1, typeof(int), rasOps);
                startPoint.Bounds = newRast.Bounds;
                startPoint.Projection = _MW.Projection;
                startPoint.Save();
                endPoint.Bounds = newRast.Bounds;
                endPoint.Projection = _MW.Projection;
                endPoint.Save();

                //int q = 0;

                for (int oRows = 0; oRows < newRast.NumRows - 1; oRows++)
                {
                    for (int oCols = 0; oCols < newRast.NumColumns - 1; oCols++)
                    {
                        //if (q >= 10000)
                        //{
                        //    q = 0;
                        //    startPoint.Save();
                        //    endPoint.Save();
                        //}
                        if (oRows == _startRow & oCols == _startCol)
                        {
                            startPoint.Value[oRows, oCols] = 1234567890;
                        }
                        else if (oRows == _endRow & oCols == _endCol)
                        {
                            endPoint.Value[oRows, oCols] = 0987654321;
                        }
                        else
                        {
                            startPoint.Value[oRows, oCols] = 0;
                            endPoint.Value[oRows, oCols] = 0;
                        }
                        //q++;
                    }
                }
                startPoint.Save();
                endPoint.Save();
                addRasterToMap(startPoint);
                addRasterToMap(endPoint);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + "\n has occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblProgress.Text = "Creating Files....Please Wait";
                //if (chkCreateShapefile.Checked == true)
                //{
                //    saveShapeFile();
                //    savedElements = "\n startEndPoints.shp";
                //}

                saveShapeFile();
                savedElements = "\n startEndPoints.shp";
                //saveRasterFile();
                //savedElements += ", \n startPoint.bgd, \n endPoint.bgd";
                MessageBox.Show("The shapefile was saved successfully at location: " + pathInfo.FullName + savedElements, "Files Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + "\n has occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
