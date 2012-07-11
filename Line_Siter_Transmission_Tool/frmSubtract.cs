using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Controls;
using DotSpatial.Symbology;
using System.IO;
namespace nx09SitingTool
{
    public partial class frmSubtract : Form
    {
        IMap _MW;
        public frmSubtract(IMap mapframe, string projSavePath)
        {
            InitializeComponent();
            _MW = mapframe;
            fillCombos();
            if (projSavePath != string.Empty)
            {
                txtSaveLocation.Text = projSavePath;
                FileInfo _pathInfo = new FileInfo(projSavePath);
            }
        }

        IRaster S1;
        IRaster S2;
        IRaster oRaster = new Raster();
        string saveLocation;

        private void fillCombos()
        {
            foreach (Layer lay in _MW.Layers)
            {
                if (lay.GetType() == typeof(DotSpatial.Controls.MapRasterLayer))
                {
                    cboS1.Items.Add(lay.LegendText);
                    cboS2.Items.Add(lay.LegendText);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSubtract_Click(object sender, EventArgs e)
        {
            try
            {
                clsRasterOps sub1 = new clsRasterOps(_MW);
                string[] rasOps = new string[1];
                oRaster = Raster.CreateRaster(saveLocation + @"\subRas.bgd", null, S1.NumColumns, S1.NumRows, 1, typeof(double), rasOps);
                oRaster.Bounds = S1.Bounds;
                oRaster.Projection = _MW.Projection;
                if (S1.Bounds != S2.Bounds && S1.NumRows != S2.NumRows && S1.NumColumns != S2.NumColumns && S1.CellHeight != S2.CellHeight)
                {
                    MessageBox.Show("Error: /n The two selected rasters are not of the same size and bounds.  Please select appropriate rasters for this calculation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                sub1.rasterSubtraction(S1, S2, oRaster);
                _MW.Layers.Add(oRaster);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + " has occured.", "Generic Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void cboS1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Layer lay in _MW.Layers)
            {
                if (cboS1.Text == lay.LegendText)
                {
                    S1 = (IRaster)lay.DataSet;
                }
            }
        }

        private void cboS2_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Layer lay in _MW.Layers)
            {
                if (cboS2.Text == lay.LegendText)
                {
                    S2 = (IRaster)lay.DataSet;
                }
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.MyDocuments;
            fbd.ShowDialog();
            txtSaveLocation.Text = fbd.SelectedPath;
            saveLocation = fbd.SelectedPath;
        }
    }
}
