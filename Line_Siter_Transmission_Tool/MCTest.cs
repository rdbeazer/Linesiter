using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace nx09SitingTool
{
    public partial class MCTest : Form
    {
        string _surveypath = string.Empty;


        public MCTest(string surveyPath)
        {
            InitializeComponent();
            _surveypath = surveyPath;
            fillDataSet();
            chart1.Invalidate();
        }

        Random random = new Random();

        private void btnBegin_Click(object sender, EventArgs e)
        {
            chart2.Series[0].Points.Clear();
            chart3.Series[0].Points.Clear();
            chart3.Series[1].Points.Clear();
            chart2.Series[0].Name = "Summed Results";
            chart3.Series[0].Name = "Monte Carlo Results";
            chart3.Series[1].Name = "Original Distribution";
            chart3.Series[0].ChartType = SeriesChartType.Spline;
            chart3.Series[1].ChartType = SeriesChartType.Spline;
            chart2.ChartAreas[0].AxisX.Maximum = 5.5;
            chart2.ChartAreas[0].AxisX.Minimum = 0;
            chart2.ChartAreas[0].AxisY.Maximum = 1;
            chart3.ChartAreas[0].AxisX.Maximum = 5.5;
            chart3.ChartAreas[0].AxisX.Minimum = 1;
            //chart3.ChartAreas[0].AxisY.Maximum = 1;
            clsMonteCarlo mc = new clsMonteCarlo();
            double[] asWeight = new double[5] {0,0,0,0,0};
            mc.assignedWeights = asWeight;
            int mcHigh = 0;
            int mcMedHigh = 0;
            int mcMedium = 0;
            int mcMedLow = 0;
            int mcLow = 0;
            int currentPass = 1;
            if (txtIts.Text != string.Empty)
            {
                mc.NumPasses = Convert.ToInt32(txtIts.Text);
            }
            else
            {
                MessageBox.Show("Please enter a number of iterations to continue.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            double xval = 0;
            double[] responsePercents = new double[5];
            responsePercents[0] = Convert.ToDouble(txtA.Text);
            responsePercents[1] = responsePercents[0] + Convert.ToDouble(txtB.Text);
            responsePercents[2] = responsePercents[1] + Convert.ToDouble(txtC.Text);
            responsePercents[3] = responsePercents[2] + Convert.ToDouble(txtD.Text);
            responsePercents[4] = responsePercents[3] + Convert.ToDouble(txtE.Text);
            for (int x = 0; x <= 4; x++)
            {
                xval = responsePercents[x];
                chart2.Series[0].Points.AddXY(x + 1, responsePercents[x]);
            }
            do
            {
                currentPass++;
                double rv = RandomNumber();
                mc.calculateWeight(rv, responsePercents);
                if (mc.wRaster == "LSHigh")
                {
                    mcHigh++;
                }
                else if (mc.wRaster == "LSMedHigh")
                {
                    mcMedHigh++;
                }
                else if (mc.wRaster == "LSMedium")
                {
                    mcMedium++;
                }
                else if (mc.wRaster == "LSMedLow")
                {
                    mcMedLow++;
                }
                else if (mc.wRaster == "LSLow")
                {
                    mcLow++;
                }
            }
            while (currentPass <= mc.NumPasses);
            chart3.Series[0].Points.Clear();
            double mcH = Convert.ToDouble(mcHigh)/Convert.ToDouble(mc.NumPasses);
            double mcMH = Convert.ToDouble(mcMedHigh) / Convert.ToDouble(mc.NumPasses);
            double mcM = Convert.ToDouble(mcMedium) / Convert.ToDouble(mc.NumPasses);
            double mcML = Convert.ToDouble(mcMedLow) / Convert.ToDouble(mc.NumPasses);
            double mcL = Convert.ToDouble(mcLow) / Convert.ToDouble(mc.NumPasses);
            chart3.Series[0].Points.AddXY(1, mcH);
            chart3.Series[0].Points.AddXY(2, mcMH);
            chart3.Series[0].Points.AddXY(3, mcM);
            chart3.Series[0].Points.AddXY(4, mcML);
            chart3.Series[0].Points.AddXY(5, mcL);
            chart3.Series[1].Points.AddXY(1, Convert.ToDouble(txtA.Text));
            chart3.Series[1].Points.AddXY(2, Convert.ToDouble(txtB.Text));
            chart3.Series[1].Points.AddXY(3, Convert.ToDouble(txtC.Text));
            chart3.Series[1].Points.AddXY(4, Convert.ToDouble(txtD.Text));
            chart3.Series[1].Points.AddXY(5, Convert.ToDouble(txtE.Text));
        }

        private double RandomNumber()
        {
            return random.NextDouble();
        }

        private void fillDataSet()
        {
            string importDataFile;
            string pathLoc = string.Empty;
            string ConnectionString = string.Empty;
            if (_surveypath != string.Empty)
            {
                FileInfo pathDir = new FileInfo(_surveypath);
                pathLoc = pathDir.DirectoryName;
            }
            else
            {
                OpenFileDialog dataSetFill = new OpenFileDialog();
                dataSetFill.Title = "Open Question Set for Analysis";
                dataSetFill.ShowDialog();
                bool exists = dataSetFill.CheckFileExists;
                if (exists)
                {
                    _surveypath = dataSetFill.FileName;
                    FileInfo pathDir = new FileInfo(dataSetFill.FileName);
                    pathLoc = pathDir.DirectoryName;
                }
                else
                {
                    MessageBox.Show("A proper questionset file needs to be selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + pathLoc + @"; Extended Properties=""text;HDR=YES;FMT=Delimited;"";";
            importDataFile = "select * from " + Path.GetFileName(_surveypath);
            OleDbConnection myConnection = new OleDbConnection(ConnectionString);
            myConnection.Open();
            OleDbDataAdapter oleDa = new OleDbDataAdapter(importDataFile, ConnectionString);

            oleDa.Fill(dsQues);
            DataTable tb = dsQues.Tables[0];
            cboQues.DataSource = tb;
            cboQues.DisplayMember = "Question Text";
            bindBoxes();
            //distChart();
            myConnection.Close();
        }

        private void bindBoxes()
        {
            try
            {
                BindingSource dq = new BindingSource();
                dq.DataSource = dsQues;
                DataTable tb = dsQues.Tables[0];
                txtA.DataBindings.Add(new Binding("Text", tb, "LSHigh"));
                txtB.DataBindings.Add(new Binding("Text", tb, "LSMedHigh"));
                txtC.DataBindings.Add(new Binding("Text", tb, "LSMedium"));
                txtD.DataBindings.Add(new Binding("Text", tb, "LSMedLow"));
                txtE.DataBindings.Add(new Binding("Text", tb, "LSLow"));
            }

            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex));
            }
        }

        private void distChart()
        {
            int s = 100;
            double a = Convert.ToDouble(txtA.Text) * s;
            double b = Convert.ToDouble(txtB.Text) * s;
            double c = Convert.ToDouble(txtC.Text) * s;
            double d = Convert.ToDouble(txtD.Text) * s;
            double e = Convert.ToDouble(txtE.Text) * s;
            double mean = 0;
            double mode = 0;
            double median = 0;
            int[] sList = new int[s];
            int[] modeArray = new int[5];
            int sListx = -1;

            for (double sa = 1; sa <= a; sa++)
            {
                sListx++;
                sList[sListx] = 1;
                modeArray[0] = Convert.ToInt32(sa);
            }
            for (double sb = 1; sb <= b; sb++)
            {
                sListx++;
                sList[sListx] = 2;
                modeArray[1] = Convert.ToInt32(sb);
            }
            for (double sc = 1; sc <= c; sc++)
            {
                sListx++;
                sList[sListx] = 3;
                modeArray[2] = Convert.ToInt32(sc);
            }
            for (double sd = 1; sd <= d; sd++)
            {
                sListx++;
                sList[sListx] = 4;
                modeArray[3] = Convert.ToInt32(sd);
            }
            for (double se = 1; se <= e; se++)
            {
                sListx++;
                sList[sListx] = 5;
                modeArray[4] = Convert.ToInt32(se);
            }
            int mx = 0;
            foreach (int x in sList)
            {
                int medSpot = s / 2;
                if (mx == medSpot)
                {
                    median = sList[mx];
                }
                mean+=sList[mx];
                mx++;
            }
            //Array.Sort(modeArray);
            mode = (Array.IndexOf(modeArray, modeArray.Max()))+1;
            //mode = modeArray[4];
            mean = mean / s;
            mx = 0;
            double variance = 0;
            double[] variationArray = new double[s];
            foreach (int x in sList)
            {
                variationArray[mx] = Math.Pow((sList[mx] - mean), 2);
                variance = variance + variationArray[mx];
                mx++;
            }
            mx = 0;
            variance = variance / (s - 1);
            double standardDeviation = Math.Sqrt(variance);
            double skewness = 0;
            double[] skewArray = new double[s];
            foreach (int x in sList)
            {
                skewArray[mx] = Math.Pow((sList[mx] - mean), 3);
                skewness = skewness + skewArray[mx];
                mx++;
            }
            skewness = skewness / Math.Pow(standardDeviation, 3);
            chart1.Invalidate();
            chart1.Series[0].Points.Clear();
            //chart1.ChartAreas.Clear();
            string seriesName = "Series1";
            chart1.Series[0].ChartType = SeriesChartType.Column;
            chart1.Series[0].Color = System.Drawing.Color.DarkRed;
            chart1.Series[0].Points.AddXY(1, a);
            chart1.Series[0].Points.AddXY(2, b);
            chart1.Series[0].Points.AddXY(3, c);
            chart1.Series[0].Points.AddXY(4, d);
            chart1.Series[0].Points.AddXY(5, e);
            chart1.ChartAreas[0].AxisX.Maximum = 6;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = s;
            lblN.Text = "n: " + Convert.ToString(s);
            lblMode.Text = "Mode: " + Convert.ToString(mode);
            //double variance = chart1.DataManipulator.Statistics.Variance(seriesName, true);
            lblVariance.Text = "Variance: " + variance;
            //double standardDeviation = Math.Sqrt(variance);
            lblStDev.Text = "Standard Deviation: " + standardDeviation;
            lblSkew.Text = "Skewness: " + skewness;


            ////Calculate Mean
            lblMean.Text = "Mean: " + Convert.ToString(mean);
            lblMedian.Text = "Median: " + median;

            #region striplines
            //Set Strip line item
            StripLine stripLineStdev = new StripLine();
            StripLine stripLineMedian = new StripLine();
            StripLine stripLineMean = new StripLine();
            chart1.ChartAreas[0].AxisX.StripLines.Clear();

            stripLineStdev.IntervalOffset = mean - standardDeviation;
            stripLineStdev.StripWidth = 2.0 * standardDeviation;
            stripLineStdev.BackColor = System.Drawing.Color.AliceBlue; /*FromArgb(64, 241, 185, 168);*/
            stripLineStdev.ForeColor = System.Drawing.Color.DarkBlue;
            stripLineStdev.Text = "Standard Deviation";
            stripLineStdev.TextLineAlignment = System.Drawing.StringAlignment.Near;
            chart1.ChartAreas[0].AxisX.StripLines.Add(stripLineStdev);

            stripLineMedian.IntervalOffset = median;
            stripLineMedian.BackColor = System.Drawing.Color.DarkGreen;
            stripLineMedian.ForeColor = System.Drawing.Color.DarkGreen;
            stripLineMedian.Text = "Median";
            stripLineMedian.TextLineAlignment = System.Drawing.StringAlignment.Near;
            stripLineMedian.StripWidth = 0.001;
            chart1.ChartAreas[0].AxisX.StripLines.Add(stripLineMedian);

            stripLineMean.IntervalOffset = mean;
            stripLineMean.BackColor = System.Drawing.Color.DarkGoldenrod;
            stripLineMean.ForeColor = System.Drawing.Color.DarkGoldenrod;
            stripLineMean.Text = "Mean";
            stripLineMean.TextAlignment = System.Drawing.StringAlignment.Near;
            stripLineMean.StripWidth = 0.001;
            chart1.ChartAreas[0].AxisX.StripLines.Add(stripLineMean);
            #endregion
        }

        private void txtE_TextChanged(object sender, EventArgs e)
        {
            distChart();
            chart1.Invalidate();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
