using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace nx09SitingTool
{
    public partial class frmWeights : Form
    {
        public frmWeights()
        {
            InitializeComponent();
        }

        private void trkHigh_Scroll(object sender, EventArgs e)
        {
            txtHigh.Text = Convert.ToString(trkHigh.Value * 0.1);
        }

        private void trkMedHigh_Scroll(object sender, EventArgs e)
        {
            txtMedHigh.Text = Convert.ToString(trkMedHigh.Value * 0.1);
        }

        private void trkMedium_Scroll(object sender, EventArgs e)
        {
            txtMedium.Text = Convert.ToString(trkMedium.Value * 0.1);
        }

        private void trkMedLow_Scroll(object sender, EventArgs e)
        {
            txtMedLow.Text = Convert.ToString(trkMedLow.Value * 0.1);
        }

        private void trkLow_Scroll(object sender, EventArgs e)
        {
            txtLow.Text = Convert.ToString(trkLow.Value * 0.1);
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public class assignedWeights
        {
            public double LSHigh;
            public double LSMedHigh;
            public double LSMedium;
            public double LSMedLow;
            public double LSLow;
        }


        public assignedWeights getWeight
        {
            get
            {
                assignedWeights aw = new assignedWeights();
                aw.LSHigh = Convert.ToDouble(txtHigh.Text);
                aw.LSMedHigh = Convert.ToDouble(txtMedHigh.Text);
                aw.LSMedium = Convert.ToDouble(txtMedium.Text);
                aw.LSMedLow = Convert.ToDouble(txtMedLow.Text);
                aw.LSLow = Convert.ToDouble(txtLow.Text);
                return aw;
            }
        }

    }
}
