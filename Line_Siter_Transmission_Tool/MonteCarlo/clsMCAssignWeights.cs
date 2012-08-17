using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Topology;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace LineSiterSitingTool.MonteCarlo
{
    internal class clsMCAssignWeights
    {
        public IRaster additiveCosts { get; set; }

        public List<string> finalStatOutput { get; set; }


        private FeatureSet pathLines = new FeatureSet(FeatureType.Line);
        private clsMonteCarlo MC = new clsMonteCarlo();
        private clsCreateWeightedRasters cwr = new clsCreateWeightedRasters();
        private Random random = new Random();
        private double rv = 0;
        private string currentQuesPath = "";
        private List<IRaster> mcRasterList = new List<IRaster>();
        private clsCostWeight costWeight = new clsCostWeight();
        private clsprepgatraster gr = new clsprepgatraster();
        private clsBuildDirectory bdir = new clsBuildDirectory();

        public void MCAssignWeights(ToolStripStatusLabel tslStatus, 
            BackgroundWorker tracker, 
            IRaster backlink, 
            IRaster outAccumRaster, 
            IRaster outPathRaster, 
            int currentPass, 
            clsMonteCarlo _mc, 
            DataGridView dgvSelectLayers, 
            IRaster bounds, 
            string saveLocation, 
            IMap _mapLayer, 
            ref string outputPathFilename, 
            IRaster utilityCosts)
        {
            MC = _mc;
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
            tracker.ReportProgress(40, "Current Pass " + Convert.ToString(currentPass));
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
                                bdir.buildDirectory(newPath);
                                //load raster file
                                IRaster oRaster = Raster.OpenFile(rasterPath);
                                cwr.createWeightedRasters(newPath, oRaster, MC, bounds);
                            }
                        }
                    }

                    rv = random.NextDouble();
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
            costWeight.calculate_Cost_Weight(mcRasSavePath, bounds, _mapLayer, currentPass, additiveCosts, utilityCosts);
            tracker.ReportProgress(90, "Pass " + Convert.ToString(currentPass) + " is complete.");
            if (MC.passType == "Monte Carlo")
            {
                outputPathFilename = saveLocation + @"\linesiter\LSProcessing\Pass_" + Convert.ToString(currentPass) + @"\outputPathRaster";
            }
            gr.prepareGATRasters(mcRasSavePath, backlink, outAccumRaster, outPathRaster, outputPathFilename);
        }
    }
}