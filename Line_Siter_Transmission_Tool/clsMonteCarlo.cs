using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Data;

namespace LineSiterSitingTool
{
    class clsMonteCarlo
    {
        int timesSaved = 1;

        #region Properties
        private int numPasses;
        public int NumPasses
        {
            get { return numPasses; }
            set { numPasses = value; }
        }

        private double _socialWeight;
        public double socialWeight
        {
            get { return _socialWeight; }
            set { _socialWeight = value; }
        }

        private double[] _assignedWeights;
        public double[] assignedWeights
        {
            get { return _assignedWeights; }
            set { _assignedWeights = value; }
        }

        private string _wRaster;
        public string wRaster
        {
            get { return _wRaster; }
            set { _wRaster = value; }
        }

        private int _oGreaterThanZeroCount;
        public int oGreaterThanZeroCount
        {
            get { return _oGreaterThanZeroCount; }
            set { _oGreaterThanZeroCount = value; }
        }

        private int _mGreaterThanZeroCount;
        public int mGreaterThanZeroCount
        {
            get { return _mGreaterThanZeroCount; }
            set { _mGreaterThanZeroCount = value; }
        }

        private long _numCells;
        public long numCells
        {
            get { return _numCells; }
            set { _numCells = value; }
        }

        private int _changedVals;
        public int changedVals
        {
            get { return _changedVals; }
            set { _changedVals = value; }
        }

        private int _mValuechangedVals;
        public int mValueChangedVals
        {
            get { return _mValuechangedVals; }
            set { _mValuechangedVals = value; }
        }

        private int _oValueChangedVals;
        public int oValueChangedVals
        {
            get { return _oValueChangedVals; }
            set { _oValueChangedVals = value; }
        }

        private int _numZeros;
        public int numZeros
        {
            get { return _numZeros; }
            set { _numZeros = value; }
        }

        # endregion

        public void calculateWeight(double rv, double[] processArray)
        {
            try
            {
                if (rv < processArray[0])
                {
                    this.socialWeight = this._assignedWeights[0];
                    this._wRaster = "LSHigh";
                }
                else if (rv > processArray[0] & rv <= processArray[1])
                {
                    this.socialWeight = this._assignedWeights[1];
                    this._wRaster = "LSMedHigh";
                }
                else if (rv > processArray[1] & rv <= processArray[2])
                {
                    this.socialWeight = this._assignedWeights[2];
                    this._wRaster = "LSMedium";
                }
                else if (rv > processArray[2] & rv <= processArray[3])
                {
                    this.socialWeight = this._assignedWeights[3];
                    this._wRaster = "LSMedLow";
                }
                else if (rv >= processArray[4])
                {
                    this.socialWeight = this._assignedWeights[4];
                    this._wRaster = "LSLow";
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
            _oGreaterThanZeroCount = 0;
            _mGreaterThanZeroCount = 0;
            _numCells = 0;
            _changedVals = 0;
            _mValuechangedVals = 0;
            _oValueChangedVals = 0;
            _numZeros = 0;
            string _savePath = savePath;
            string[] calcOptions = new string[1];
            IRaster calcRaster = Raster.CreateRaster(_savePath + @"\linesiter\LSProcessing\" + timesSaved.ToString() + ".bgd", null, cRaster.NumColumns, cRaster.NumRows, 1, typeof(double), calcOptions);
            calcRaster.Bounds = cRaster.Bounds.Copy();

            for (int oRows = 0; oRows < cRaster.NumRows - 1; oRows++)
            {
                for (int oCols = 0; oCols < cRaster.NumColumns - 1; oCols++)
                {
                    _numCells++;
                    cValue = cRaster.Value[oRows, oCols];
                    wValue = mRaster.Value[oRows,oCols];
                    int xvalue = mRaster.NumRows;
                    int yvalue = mRaster.NumColumns;
                    if (cValue == 0)
                    {
                        _numZeros++;
                    }
                    if (cValue > 0 & cValue != 1)
                    {
                        _oGreaterThanZeroCount++;
                    }

                    if (wValue > 0 & wValue != 1)
                    {
                        _mGreaterThanZeroCount++;
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
                            _changedVals++;
                            _oValueChangedVals++;
                        }
                        else if (cValue > wValue)//if weight raster (wValue) has a smaller value to cummulative raster (cValue) set cummulative raster to weight raster (wValue)
                        {
                            calcRaster.Value[oRows, oCols] = wValue;
                            _changedVals++;
                            _mValuechangedVals++;
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
