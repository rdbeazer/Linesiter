using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using DotSpatial.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;
using System.IO;
using DotSpatial.Symbology;
using DotSpatial.Controls;
using DotSpatial.Topology;
using DotSpatial.Projections;
using System.Reflection;
using GeospatialFiles;
using System.Threading;


namespace nx09SitingTool
{
    class clsProcess1
    {
        private IRaster _additiveCosts;
        public IRaster additiveCosts
        {
            get { return _additiveCosts; }
            set { _additiveCosts = value; }
        }

        private List<string> _finalStatOutput;
        public List<string> finalStatOutput
        {
            get { return _finalStatOutput; }
            set { _finalStatOutput = value; }
        }

        //private System.Windows.Forms.NumericUpDown numPasses;
        //private System.Windows.Forms.DataGridView dgvSelectLayers;
        private System.Data.DataSet dsQuesSets = new System.Data.DataSet();
        //IMap _mapLayer = null;
        int currentPass = 1;
        Cursor curs = Cursors.Arrow;
        int newQIDValue = 0;
        //string saveLocation = null;
        FeatureSet projectFS = new FeatureSet();
        double cellSize = 0;
        FeatureSet pathLines = new FeatureSet(FeatureType.Line);
        //IRaster bounds = new Raster();
        //string progress = string.Empty;
        // List<string> finalStatOutput = new List<string>();
        clsMonteCarlo MC = new clsMonteCarlo();
        clscreateWeightedRaster c1 = new clscreateWeightedRaster();
        Randomnumber r1 = new Randomnumber();
        double rv = 0;
        string currentQuesPath = "";
        List<IRaster> mcRasterList = new List<IRaster>();
        clsCostWeight c2 = new clsCostWeight();
        clsprepgatraster gr = new clsprepgatraster();
        Cursor xcurs;
        BackgroundWorker worker = new BackgroundWorker();
        clsBuildDirectory b1 = new clsBuildDirectory();


        public void clsprocess1(ToolStripStatusLabel tslStatus, BackgroundWorker tracker, IRaster backlink, IRaster outAccumRaster, IRaster outPathRaster, int currentPass, clsMonteCarlo _mc, DataGridView dgvSelectLayers, IRaster bounds, string saveLocation, IMap _mapLayer, string progress, ref string outputPathFilename, IRaster utilityCosts,clsBuildDirectory _b1, ref IRaster rasterToConvert, ref string costFileName)
        {
            MC = _mc;
            b1 = _b1;
            tslStatus.Visible = false;
            finalStatOutput.Add("Monte Carlo Pass: " + Convert.ToString(currentPass));
            int questNum = 1;
            IRaster mcRaster = bounds;
            string mcRasSavePath = saveLocation + @"\linesiter\LSProcessing\Pass_" + Convert.ToString(currentPass);
            Directory.CreateDirectory(mcRasSavePath);
            mcRaster.SaveAs(mcRasSavePath + @"\mcRaster.bgd");
            mcRaster = null;
            mcRaster = Raster.Open(mcRasSavePath + @"\mcRaster.bgd");
            double[] AnswerPercents = new double[5];
            double[] mcPercents = new double[5];
            progress = "Current Pass " + Convert.ToString(currentPass);
            tracker.ReportProgress(40);
            foreach (DataGridViewRow dr in dgvSelectLayers.Rows)
            {
                if (Convert.ToString(dr.Cells[0].Value) == "True")
                {
                    if (dr.Cells[0].Value != null)
                    {
                        if (currentPass <= MC.NumPasses)
                        {
                            if (dr.Cells[1].Value == DBNull.Value)
                            {
                                //write exception
                                finalStatOutput.Add("Question: " + Convert.ToString(dr.Cells[3].Value) + " has no associated raster layer.  It will not be considered.");
                            }
                            else
                            {
                                //create directory for each question
                                string newPath = saveLocation + @"\linesiter\LSProcessing\Quesid_" + Convert.ToString(dr.Cells[3].Value);
                                string rasterPath = saveLocation + @"\linesiter\LSProcessing\Quesid_" + Convert.ToString(dr.Cells[2].Value) + Convert.ToString(dr.Cells[1].Value) + "pa.bgd";
                                b1.buildDirectory(newPath);
                                //load raster file
                                IRaster oRaster = Raster.OpenFile(rasterPath);
                                c1.createWeightedRasters(newPath, rasterPath, oRaster, bounds, _mc, currentPass);
                            }
                        }
                    }

                    rv = 0;
                    rv = r1.RandomNumber();
                    finalStatOutput.Add("Question: ");
                    if (dr.Cells[4].Value != DBNull.Value)
                    {
                        finalStatOutput.Add((string)dr.Cells[4].Value);
                    }
                    finalStatOutput.Add("Associated Feature Class: ");
                    if (dr.Cells[1].Value != DBNull.Value)
                    {
                        finalStatOutput.Add(Convert.ToString(dr.Cells[1].Value));
                    }
                    finalStatOutput.Add("Random Variable: " + Convert.ToString(rv));

                    if ((double)dr.Cells[5].Value == 0.0 && (double)dr.Cells[6].Value == 0 && (double)dr.Cells[7].Value == 0 && (double)dr.Cells[8].Value == 0 && (double)dr.Cells[9].Value == 0)
                    {
                        MessageBox.Show("Check the questions you specified.  One or more questions have an invalid response.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        pathLines.DataTable.Columns.Remove("pass");
                        return;
                    }
                    if (dr.Cells[5].Value != DBNull.Value)
                    {
                        AnswerPercents[0] = (double)dr.Cells[5].Value;
                    }
                    if (dr.Cells[6].Value != DBNull.Value)
                    {
                        AnswerPercents[1] = AnswerPercents[0] + (double)dr.Cells[6].Value;
                    }
                    if (dr.Cells[7].Value != DBNull.Value)
                    {
                        AnswerPercents[2] = AnswerPercents[1] + (double)dr.Cells[7].Value;
                    }
                    if (dr.Cells[8].Value != DBNull.Value)
                    {
                        AnswerPercents[3] = AnswerPercents[2] + (double)dr.Cells[8].Value;
                    }
                    if (dr.Cells[9].Value != DBNull.Value)
                    {
                        AnswerPercents[4] = AnswerPercents[3] + (double)dr.Cells[9].Value;
                    }
                    MC.calculateWeight(rv, AnswerPercents);
                    tracker.ReportProgress(60);
                    finalStatOutput.Add("LSHigh: " + Convert.ToString(AnswerPercents[0]) + " | LSMedHigh: " + Convert.ToString(AnswerPercents[1]) + " | LSMed: " + Convert.ToString(AnswerPercents[2]) + " | LSMedLow: " + Convert.ToString(AnswerPercents[3]) + " | LSLow: " + Convert.ToString(AnswerPercents[4]));
                    finalStatOutput.Add("Weight: " + (Convert.ToString(MC.socialWeight)));
                    finalStatOutput.Add(MC.wRaster);
                    currentQuesPath = saveLocation + @"\linesiter\LSProcessing\Quesid_" + Convert.ToString(dr.Cells[3].Value) + "\\" + MC.wRaster + ".bgd";
                    IRaster mRaster = Raster.OpenFile(currentQuesPath);
                    mcRaster = MC._calcRaster(mcRaster, mRaster, saveLocation);
                    mcRaster.SaveAs(mcRasSavePath + @"\mcRaster.bgd");
                    mcRasterList.Add(mRaster);
                    questNum++;
                    MC.calcRaster2(mcRasterList, mcRaster);
                    mcRaster.Save();
                }
            }


            c2.calculate_Cost_Weight(mcRasSavePath, bounds, _mapLayer, currentPass, additiveCosts, utilityCosts);

            progress = "Pass " + Convert.ToString(currentPass) + " is complete.";
            tracker.ReportProgress(90);

            gr.prepareGATRasters(mcRasSavePath, worker, curs, backlink, outAccumRaster, ref outPathRaster, ref outputPathFilename);
        


        }


    }

}


