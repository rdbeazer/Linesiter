using System;
using System.Windows.Forms;

namespace LineSiterSitingTool
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

        private void txtHigh_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToDouble(txtHigh.Text) > 1 || Convert.ToDouble(txtHigh.Text) < 0)
            {
                txtHigh.Text = Convert.ToString(0.9);
                MessageBox.Show("Since the value is above 1, The value is set to its default value.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtMedHigh_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToDouble(txtMedHigh.Text) > 1 || Convert.ToDouble(txtMedHigh.Text) < 0)
            {
                txtMedHigh.Text = Convert.ToString(0.7);
                MessageBox.Show("Since the value is above 1, The value is set to its default value.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtMedium_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToDouble(txtMedium.Text) > 1 || Convert.ToDouble(txtMedium.Text) < 0)
            {
                txtMedium.Text = Convert.ToString(0.5);
                MessageBox.Show("Since the value is above 1, The value is set to its default value.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtMedLow_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToDouble(txtMedLow.Text) > 1 || Convert.ToDouble(txtMedLow.Text) < 0)
            {
                txtMedLow.Text = Convert.ToString(0.3);
                MessageBox.Show("Since the value is above 1, The value is set to its default value.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtLow_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToDouble(txtLow.Text) > 1 || Convert.ToDouble(txtLow.Text) < 0)
            {
                txtLow.Text = Convert.ToString(0.1);
                MessageBox.Show("Since the value is above 1, The value is set to its default value.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}