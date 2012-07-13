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
using System.IO;

namespace LineSiterSitingTool
{
    public partial class frmReclass : Form
    {
        IMap _MW; 

        public frmReclass(IMap mapFrame, string projSavePath)
        {
            InitializeComponent();
            _MW = mapFrame;
            loadRtb();
            if (projSavePath != string.Empty)
            {
                txtSaveLocation.Text = projSavePath;
                FileInfo _pathInfo = new FileInfo(projSavePath);
            }
        }

        IRaster passingRaster = new Raster();
        IRaster savingRaster = new Raster();

        private void btnRasterOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.InitialDirectory = @"C:\Users\Robert\Documents\LDRD\Data\Test Data" ;
            //ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*" 

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtRasterOpen.Text = ofd.FileName;
                passingRaster = Raster.OpenFile(ofd.FileName);
            }
        }

        private void loadRtb()
        {
            try
            {
                rtbHelpBox.SelectionFont = new Font("Arial", 12f, FontStyle.Bold);
                rtbHelpBox.AppendText("Raster Reclassification \n \n");
                rtbHelpBox.SelectionFont = new Font("Arial", 10f, FontStyle.Regular);
                rtbHelpBox.AppendText("This tool allows the reclassification of values inside  ");
                rtbHelpBox.AppendText("a DotSpatial raster grid.  To reclassify values, enter a value range ");
                rtbHelpBox.AppendText("separated by a dash '-' with no spaces.  Then enter a single value ");
                rtbHelpBox.AppendText("under the 'New Value' window as the relevant reclassified value. ");
                rtbHelpBox.AppendText("To begin the process, click 'Classify.'");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + " has occured.", "Generic Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnReclassSave_Click(object sender, EventArgs e)
        {
            if (txtRasterOpen.Text == "")
            {
                MessageBox.Show("Please select a raster to reclassify before selecting a save file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            string[] rasops = new string[1];

            sfd.InitialDirectory = @"C:\Users\Robert\Documents\LDRD\Data\Test Data";
            sfd.Filter = "DotSpatial Raster Files (*.bgd)|*.bgd";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                txtRasterSave.Text = sfd.FileName;
                savingRaster = Raster.CreateRaster(sfd.FileName, null, passingRaster.NumColumns, passingRaster.NumRows, 1, typeof(double), null);
            }
        }

        private void btnClassify_Click(object sender, EventArgs e)
        {
            clsRasterOps cro = new clsRasterOps(_MW);
            Dictionary<string, double> reclassifyDict = new Dictionary<string,double>();
            bool success = false;
            savingRaster.Bounds = passingRaster.Bounds;
            savingRaster.Projection = passingRaster.Projection;
            savingRaster.Save();

            foreach (DataGridViewRow dgvr in dgvReclassify.Rows)
            {
                if (dgvr.Cells[0].Value != null)
                {
                    reclassifyDict.Add(Convert.ToString(dgvr.Cells[0].Value), Convert.ToDouble(dgvr.Cells[1].Value));
                }
            }
            cro.rasterDoubleReclassify(passingRaster, savingRaster, reclassifyDict, ref success);
            if (success == true)
            {
                MessageBox.Show("Reclassification Complete.  \n The new raster has been added to the map.", "Reclassification Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            btnCancel.Text = "Done";
        }
    }
}
