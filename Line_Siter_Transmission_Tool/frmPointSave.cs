﻿using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Symbology;
using DotSpatial.Topology;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LineSiterSitingTool
{
    public partial class frmPointSave : Form
    {
        private int _startRow = 0, _startCol = 0, _endRow = 0, _endCol = 0;
        private IMap _MW;
        private FileInfo pathInfo = null;
        private List<Coordinate> shCoords = new List<Coordinate>();
        private FeatureSet startEndPoints = new FeatureSet(FeatureType.Point);
        private string savedElements = "";
        private string bndRasterText = string.Empty;

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
            FileInfo _pathInfo = new FileInfo(fbd.SelectedPath);
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
                startEndPoints.SaveAs(pathS, true);
                addShapeFileToMap(startEndPoints);
                //saveRasterFile();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + "\n has occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        public void addShapeFileToMap(FeatureSet shapefileToAdd)
        {
            _MW.Layers.Add(shapefileToAdd);
            _MW.ResetBuffer();
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
                //savedElements += ", \n startPoint.bgd, \n endPoint.bgd";
                MessageBox.Show("The shapefile was saved successfully at location: " + pathInfo.FullName + savedElements, "Files Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + "\n has occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}