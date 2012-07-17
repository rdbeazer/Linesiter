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

namespace LineSiterSitingTool
{
    public partial class frmSelectLayers : Form
    {
        IMap mapControl = null;

        public frmSelectLayers(IMap mapContols)
        {
            InitializeComponent();
            mapControl = mapContols;
            loadLayers();
            loadRtb();
        }

        private void loadLayers()
        {
            foreach (Layer lay in mapControl.Layers)
            {
                lbxFromList.Items.Add(lay.LegendText);
            }
        }

        private void btnAddOne_Click(object sender, EventArgs e)
        {
            try
            {
                lbxToList.Items.Add(lbxFromList.SelectedItem);
                lbxFromList.Items.Remove(lbxFromList.SelectedItem);
            }

            catch (ArgumentNullException)
            {
                MessageBox.Show("No item was selected to add to the list.  Please select an item and retry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            //foreach (string listItems in lbxToList.Items)
            //{
            //    if (listItems == (string)lbxFromList.SelectedItem)
            //    {
            //        MessageBox.Show("That item has already been added to the list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            //        break;
            //    }
            //    else
            //    {
            //        lbxToList.Items.Add(lbxFromList.SelectedItem);
            //    }
            //}
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                lbxFromList.Items.Add(lbxToList.SelectedItem);
                lbxToList.Items.Remove(lbxToList.SelectedItem);
            }

            catch (ArgumentNullException)
            {
                MessageBox.Show("No item was selected to remove from the list.  Please select an item and retry.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void loadRtb()
        {
            rtbHelpBox.SelectionFont = new Font("Arial", 12f, FontStyle.Bold);
            rtbHelpBox.AppendText("Create Utility Cost Raster \n \n");
            rtbHelpBox.SelectionFont = new Font("Arial", 10f, FontStyle.Regular);
            rtbHelpBox.AppendText("This process creates a cost raster from shapefiles loaded in the project.  ");
            rtbHelpBox.AppendText("To create the raster, add the relevant shapefiles from the left selection box to the right box.  ");
            rtbHelpBox.AppendText("Relevant shapefiles must have a cost passNumber in order to create the utility cost raster.  ");

        }

        private string[] _sendLayers;
        public string[] sendLayers
        {
            get { return _sendLayers; }
            set { _sendLayers = value; }
        }


        public void btnFinish_Click(object sender, EventArgs e)
        {
            string[] sLayers = new string[lbxToList.Items.Count];
            int strAdd = 0;
            foreach (object lst in lbxToList.Items)
            {
                sLayers[strAdd] = lst.ToString();
                strAdd++;
            }
            _sendLayers = sLayers;
            this.Close();
        }

    }
}
