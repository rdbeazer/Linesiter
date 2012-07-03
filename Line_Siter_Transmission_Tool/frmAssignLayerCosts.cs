using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Symbology;
using DotSpatial.Data;
using System.Collections;

namespace nx09SitingTool
{
    public partial class frmAssignLayerCosts : Form
    {
        private bool _okToProceed;
        public bool okToProceed
        {
            get { return _okToProceed; }
            set { _okToProceed = value; }
        }

        IMap mapLayers = null;
        public Dictionary <string, string> layerList = new Dictionary<string, string>();

        public frmAssignLayerCosts(IMap mlayers)
        {
            InitializeComponent();
            mapLayers = mlayers;
            loadLayersDV();
            loadRtb();
        }

        private void loadRtb()
        {
            try
            {
                rtbHelpBox.SelectionFont = new Font("Arial", 12f, FontStyle.Bold);
                rtbHelpBox.AppendText("Associate Cost Layers \n \n");
                rtbHelpBox.SelectionFont = new Font("Arial", 10f, FontStyle.Regular);
                rtbHelpBox.AppendText("This selection box gives users the ability to choose layers and their  ");
                rtbHelpBox.AppendText("associated cost attributes. ");
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + " has occured.", "Generic Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        private void loadLayersDV()
        {
            try
            {

                int rowNum = 0;
                DataGridViewCheckBoxColumn dgvcheck = new DataGridViewCheckBoxColumn();
                DataGridViewTextBoxColumn fLayer = new DataGridViewTextBoxColumn();
                DataGridViewComboBoxColumn layerAtts = new DataGridViewComboBoxColumn();
                dgvcheck.HeaderText = "Include";
                fLayer.HeaderText = "FeatureLayer";
                layerAtts.HeaderText = " Cost Attribute";
                dgvSelectLayers.Columns.Add(dgvcheck);
                dgvSelectLayers.Columns.Add(fLayer);
                dgvSelectLayers.Columns.Add(layerAtts);


                foreach (Layer lay in mapLayers.Layers)
                {
                    if (lay.GetType() == typeof(DotSpatial.Controls.MapPointLayer) | lay.GetType() == typeof(DotSpatial.Controls.MapLineLayer) | lay.GetType() == typeof(DotSpatial.Controls.MapPolygonLayer))
                    {
                        DataGridViewTextBoxCell layerNameText = new DataGridViewTextBoxCell();
                        DataGridViewComboBoxCell layAttrs = new DataGridViewComboBoxCell();
                        DataGridViewCheckBoxCell layChecks = new DataGridViewCheckBoxCell();
                        DataGridViewRow newLayerRow = new DataGridViewRow();
                        layerNameText.Value = lay.LegendText;
                        FeatureSet proFS = (FeatureSet)lay.DataSet;
                        foreach (object col in proFS.DataTable.Columns)
                        {
                            layAttrs.Items.Add((object)col.ToString());
                        }
                        newLayerRow.Cells.Add(layChecks);
                        newLayerRow.Cells.Add(layerNameText);
                        newLayerRow.Cells.Add(layAttrs);
                        dgvSelectLayers.Rows.Add(newLayerRow);
                        rowNum++;
                    }
                    
                    if (lay.GetType() == typeof(DotSpatial.Controls.MapRasterLayer))
                    {
                        DataGridViewTextBoxCell layerNameText = new DataGridViewTextBoxCell();
                        DataGridViewTextBoxCell rasterIdText = new DataGridViewTextBoxCell();
                        DataGridViewCheckBoxCell layChecks = new DataGridViewCheckBoxCell();
                        DataGridViewRow newLayerRow = new DataGridViewRow();
                        layerNameText.Value = lay.LegendText;
                        rasterIdText.Value = "N/A--Is Raster"; 
                        newLayerRow.Cells.Add(layChecks);
                        newLayerRow.Cells.Add(layerNameText);
                        newLayerRow.Cells.Add(rasterIdText);
                        dgvSelectLayers.Rows.Add(newLayerRow);
                        rowNum++;
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + " has occured.", "Generic Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow rw in dgvSelectLayers.Rows)
                {
                    if (rw.Cells[0].Value != null)
                    {
                        layerList.Add((string)rw.Cells[1].Value, (string)rw.Cells[2].Value);
                    }
                }
                okToProceed = true;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + " has occured.", "Generic Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            okToProceed = false;
        }


    }
}
