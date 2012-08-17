using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Data;
using DotSpatial.Controls;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace LineSiterSitingTool
{
    class clsRasterOps
    {
        IMap _MW;

        public clsRasterOps(IMap mapFrame)
        {
            _MW = mapFrame;
        }

        public void createPA(IRaster oRaster, string rasterSaveFN, double convertValue)
        {
            try
            {
                string[] rasterParameters = null;
                Extent ext = oRaster.Extent;
                int len = rasterSaveFN.Length - 4;
                string rasterSaveFN2 = rasterSaveFN.Insert(len, "PA");
                IRaster paRaster = Raster.CreateRaster(rasterSaveFN2, "", oRaster.NumColumns, oRaster.NumRows, 1, oRaster.DataType, rasterParameters);
                paRaster.Bounds = oRaster.Bounds.Copy();
                paRaster.NoDataValue = oRaster.NoDataValue;
                paRaster.Projection = _MW.Projection;
                //paRaster.Projection = oRaster.Projection;
                int timesNeg = 0;
                int timesZero = 0;
                int timesOther = 0;
                int timesNoData = 0;

                double oValue = 0;

                for (int oRows = 0; oRows < oRaster.NumRows - 1; oRows++)
                {
                    for (int oCols = 0; oCols < oRaster.NumColumns - 1; oCols++)
                    {
                        oValue = oRaster.Value[oRows, oCols];

                        if (oValue >= 0 & convertValue == -99)
                        {
                            paRaster.Value[oRows, oCols] = convertValue;
                            timesOther++;
                        }
                        else if (oValue >= 0 & convertValue == -1)
                        {
                            paRaster.Value[oRows, oCols] = convertValue;
                            timesNeg++;
                        }
                        else if (oValue > 0 & convertValue == -98)
                        {
                            paRaster.Value[oRows, oCols] = oValue;
                            timesOther++;
                        }
                        else if (/*oValue == 0 | */ oValue == oRaster.NoDataValue)
                        {
                            paRaster.Value[oRows, oCols] = 0;
                            timesNoData++;
                        }
                        else
                        {
                            paRaster.Value[oRows, oCols] = 1;
                            timesZero++;
                        }
                    }
                }
                paRaster.Save();
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }


        public void rasterAddition(List<IRaster> lraster, IRaster oRaster)
        {
            foreach (Raster addRaster in lraster)
            {
                for (int oRows = 0; oRows < oRaster.NumRows - 1; oRows++)
                {
                    for (int oCols = 0; oCols < oRaster.NumColumns - 1; oCols++)
                    {
                        double oValue = oRaster.Value[oRows, oCols];
                        oRaster.Value[oRows, oCols] = oValue + addRaster.Value[oRows, oCols];

                    }
                    oRaster.Save();
                }
            }
            //_MW.Layers.Add(oRaster);
        }

        public void rasterMultiplication(List<IRaster> lraster, IRaster oRaster)
        {

            foreach (Raster addRaster in lraster)
            {
                for (int oRows = 0; oRows < oRaster.NumRows - 1; oRows++)
                {
                    for (int oCols = 0; oCols < oRaster.NumColumns - 1; oCols++)
                    {
                        double oValue = oRaster.Value[oRows, oCols];
                        oRaster.Value[oRows, oCols] = oValue * addRaster.Value[oRows, oCols];

                    }
                    oRaster.Save();
                }
            }
        }

        public void rasterSubtraction(IRaster S1, IRaster S2, IRaster oRaster)
        {
                for (int oRows = 0; oRows < oRaster.NumRows - 1; oRows++)
                {
                    for (int oCols = 0; oCols < oRaster.NumColumns - 1; oCols++)
                    {
                        oRaster.Value[oRows, oCols] = S1.Value[oRows, oCols] - S2.Value[oRows, oCols];
                    }
                }
                oRaster.Save();
        }

        public void rasterDivision(IRaster div1, IRaster div2, IRaster divFinal)
        {
            int x = 0;
            for (int oRows = 0; oRows < div1.NumRows - 1; oRows++)
            {
                for (int oCols = 0; oCols < div1.NumColumns - 1; oCols++)
                {
                    double val1 = div1.Value[oRows, oCols];
                    double val2 = div2.Value[oRows, oCols];
                    if (val1 > 0 | val2 > 0)
                    {
                        x++;
                    }
                    if (div2.Value[oRows, oCols] == 0)
                    {
                        divFinal.Value[oRows, oCols] = 0;
                    }
                    else
                    {
                        divFinal.Value[oRows, oCols] = div1.Value[oRows, oCols] / div2.Value[oRows, oCols];
                    }
                    double val3 = divFinal.Value[oRows, oCols];
                }
                divFinal.Save();
            }
            //_MW.Layers.Add(divFinal);
        }

        public void rasterDoubleReclassify(IRaster psdRaster, IRaster saveRaster, Dictionary<string, double> rasterVals, ref bool success)
        {
            double passedVal = 0;
            double origVal1 = 0;
            double origVal2 = 0;
            string[] splitVal = new string[2];

            for (int oRows = 0; oRows < psdRaster.NumRows - 1; oRows++)
            {
                for (int oCols = 0; oCols < psdRaster.NumColumns - 1; oCols++)
                {
                    passedVal = psdRaster.Value[oRows, oCols];
                    foreach (KeyValuePair<string, double> element in rasterVals)
                    {
                        splitVal = element.Key.Split('-');
                        origVal1 = Convert.ToDouble(splitVal[0]);
                        origVal2 = Convert.ToDouble(splitVal[1]);
                        if (passedVal >= origVal1 & passedVal <= origVal2)
                        {
                            saveRaster.Value[oRows, oCols] = element.Value;
                            break;
                        }
                    }
                }

            }
            saveRaster.Save();
            _MW.Layers.Add(saveRaster);
            success = true;
        }

        public void removeProcessingFiles()
        {
                DirectoryInfo df = new DirectoryInfo(@"c:\temp\linesiter");
                foreach (FileInfo file in df.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in df.GetDirectories())
                {
                    dir.Delete(true);
                }
                Directory.Delete(@"c:\temp\linesiter");
        }


    }
}
