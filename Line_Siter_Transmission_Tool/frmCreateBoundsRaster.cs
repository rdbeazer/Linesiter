using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Symbology;
using DotSpatial.Controls;
using DotSpatial.Analysis;
using System.IO;

namespace LineSiterSitingTool
{
    public partial class frmCreateBoundsRaster : Form
    {
        IMap _MW;
        clsRasterOps pa;


        public frmCreateBoundsRaster(IMap mapFrame, string projSavePath)
        {
            InitializeComponent();
            _MW = mapFrame;
            pa = new clsRasterOps(_MW);
            fillCombo();
            loadRtb();
            if (projSavePath != string.Empty)
            {
                txtSaveLocation.Text = projSavePath;
                FileInfo _pathInfo = new FileInfo(projSavePath);
            }
        }

        private void loadRtb()
        {
            try
            {
                rtbHelpBox.SelectionFont = new Font("Arial", 12f, FontStyle.Bold);
                rtbHelpBox.AppendText("Create A Bounds Raster \n \n");
                rtbHelpBox.SelectionFont = new Font("Arial", 10f, FontStyle.Regular);
                rtbHelpBox.AppendText("This dialog provides the ability to create a bounding raster  ");
                rtbHelpBox.AppendText("for a project boundry from a shapefile. Two rasters are created ");
                rtbHelpBox.AppendText("the first is a preliminary raster and the second is a boolean ");
                rtbHelpBox.AppendText("raster that is the actual raster used in the calculations.  The ");
                rtbHelpBox.AppendText("second raster carries a 'pa' appended to the name assigned ");
                rtbHelpBox.AppendText("in the save dialog.  Load only this file for the caculations.");
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + " has occured.", "Generic Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        private void fillCombo()
        {
            foreach (Layer lay in _MW.Layers)
            {
                if (lay.GetType() != typeof(DotSpatial.Controls.MapRasterLayer))
                {
                    cboLayers.Items.Add(lay.LegendText);
                }
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            SaveFileDialog svRas = new SaveFileDialog();
            svRas.Filter = "DotSpatial Raster (*.bgd) | *.bgd";
            svRas.ShowDialog();
            txtSaveLocation.Text = svRas.FileName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                //create raster featureset with project bounds information
                if (txtSaveLocation.Text == string.Empty)
                {
                    MessageBox.Show("Please Select a Save Location.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                double cellSize = (double)txtCellSize.Value;
                txtCellSize.Enabled = false;
                string selectedItem = (string)cboLayers.SelectedItem;
                string[] ras2 = new string[1];
                foreach (Layer fsLayer in _MW.Layers)
                {
                    if (fsLayer.LegendText == selectedItem)
                    {
                        if (fsLayer.GetType() == typeof(DotSpatial.Controls.MapPolygonLayer))
                        {
                            FeatureSet projectFS = (FeatureSet)fsLayer.DataSet;
                            IRaster prjOutRS = DotSpatial.Analysis.VectorToRaster.ToRaster(projectFS, projectFS.Extent, cellSize, "FID", txtSaveLocation.Text, null, ras2, null);
                            pa.createPA(prjOutRS, txtSaveLocation.Text, -99);                        
                        }
                        else if (fsLayer.GetType() == typeof(DotSpatial.Controls.MapRasterLayer))
                        {
                            IRaster prjOutRS = (IRaster)fsLayer.DataSet;
                            cellSize = prjOutRS.CellHeight;
                            //_MW.Layers.Add(prjOutRS);
                            //MessageBox.Show("Please create a project bounding shapefile and covert to bounds raster.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }

                        //create custom exception and throw
                    }
                }
                IRaster nPrjOutRS = Raster.Open(txtSaveLocation.Text.Substring(0, txtSaveLocation.Text.Length - 4) + "pa.bgd");
                _MW.Layers.Add(nPrjOutRS);              
                MessageBox.Show("Process Complete. \n Bounding Raster has been added to map frame.", "Bounding Raster Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();

                

            }
            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void frmCreateBoundsRaster_Load(object sender, EventArgs e)
        {

        }

    }
}
