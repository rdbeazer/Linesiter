using DotSpatial.Data;
using System;
using System.Collections.Generic;

namespace LineSiterSitingTool.MonteCarlo
{
    internal class clsMonteCarlo
    {
        private int timesSaved = 1;

        #region Properties

        public int NumPasses { get; set; }

        public double socialWeight { get; set; }

        public double[] assignedWeights { get; set; }

        public string wRaster { get; set; }

        public int oGreaterThanZeroCount { get; set; }

        public int mGreaterThanZeroCount { get; set; }

        public long numCells { get; set; }

        public int changedVals { get; set; }

        public int mValueChangedVals { get; set; }

        public int oValueChangedVals { get; set; }

        public int numZeros { get; set; }

        public bool errorCondition { get; set; }

        public string passType { get; set; }

        # endregion

        public void calculateWeight(double rv, double[] processArray)
        {
            try
            {
                if (rv < processArray[0])
                {
                    this.socialWeight = this.assignedWeights[0];
                    this.wRaster = "LSHigh";
                }
                else if (rv > processArray[0] & rv <= processArray[1])
                {
                    this.socialWeight = this.assignedWeights[1];
                    this.wRaster = "LSMedHigh";
                }
                else if (rv > processArray[1] & rv <= processArray[2])
                {
                    this.socialWeight = this.assignedWeights[2];
                    this.wRaster = "LSMedium";
                }
                else if (rv > processArray[2] & rv <= processArray[3])
                {
                    this.socialWeight = this.assignedWeights[3];
                    this.wRaster = "LSMedLow";
                }
                else if (rv >= processArray[4])
                {
                    this.socialWeight = this.assignedWeights[4];
                    this.wRaster = "LSLow";
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        public IRaster _calcRaster(IRaster cRaster, IRaster mRaster, string savePath)
        {
            double cValue = 0;
            double wValue = 0;
            oGreaterThanZeroCount = 0;
            mGreaterThanZeroCount = 0;
            numCells = 0;
            changedVals = 0;
            mValueChangedVals = 0;
            oValueChangedVals = 0;
            numZeros = 0;
            string _savePath = savePath;
            string[] calcOptions = new string[1];
            IRaster calcRaster = Raster.CreateRaster(String.Format(@"{0}\linesiter\LSProcessing\{1}.bgd", _savePath, timesSaved), null, cRaster.NumColumns, cRaster.NumRows, 1, typeof(double), calcOptions);
            calcRaster.Bounds = cRaster.Bounds.Copy();

            for (int oRows = 0; oRows < cRaster.NumRows - 1; oRows++)
            {
                for (int oCols = 0; oCols < cRaster.NumColumns - 1; oCols++)
                {
                    numCells++;
                    cValue = cRaster.Value[oRows, oCols];
                    wValue = mRaster.Value[oRows, oCols];

                    if (cValue == 0)
                    {
                        numZeros++;
                    }
                    if (cValue > 0 & cValue != 1)
                    {
                        oGreaterThanZeroCount++;
                    }

                    if (wValue > 0 & wValue != 1)
                    {
                        mGreaterThanZeroCount++;
                    }

                    if (cValue != cRaster.NoDataValue)
                    {
                        //determine and set minimum value

                        if (cValue == -99)//check for -99 in the bounds raster and reset to first weight raster value (wValue)
                        {
                            calcRaster.Value[oRows, oCols] = wValue;
                        }
                        else if (cValue < wValue)//if cummulative raster has smaller value than weight raster (wValue) leave cummulative raster value (cValue)
                        {
                            calcRaster.Value[oRows, oCols] = cValue;
                            changedVals++;
                            oValueChangedVals++;
                        }
                        else if (cValue > wValue)//if weight raster (wValue) has a smaller value to cummulative raster (cValue) set cummulative raster to weight raster (wValue)
                        {
                            calcRaster.Value[oRows, oCols] = wValue;
                            changedVals++;
                            mValueChangedVals++;
                        }
                        else if (cValue == 1 & wValue == 1)
                        {
                            calcRaster.Value[oRows, oCols] = 1;
                        }
                    }
                    else
                    {
                        calcRaster.Value[oRows, oCols] = 50;
                    }
                }
            }

            calcRaster.Save();
            timesSaved++;
            return calcRaster;
        }

        public IRaster calcRaster2(List<IRaster> mcwRasters, IRaster passedRaster)
        {
            double[] mcw = new double[mcwRasters.Count];
            int x = 0;
            for (int oRows = 0; oRows < passedRaster.NumRows - 1; oRows++)
            {
                for (int oCols = 0; oCols < passedRaster.NumColumns - 1; oCols++)
                {
                    x = 0;
                    foreach (IRaster xras in mcwRasters)
                    {
                        mcw[x] = xras.Value[oRows, oCols];
                        x++;
                    }
                    Array.Sort(mcw);
                    passedRaster.Value[oRows, oCols] = mcw[0];
                    Array.Clear(mcw, 0, mcwRasters.Count);
                }
            }
            passedRaster.Save();
            return passedRaster;
        }
    }
}