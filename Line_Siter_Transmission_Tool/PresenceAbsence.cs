using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Data;

namespace linesiter.tools
{
    public class PresenceAbsence
    {
        public void createPA(IRaster oRaster, string rasterSaveFN)
        {
            try
            {
                string[] rasterParameters = null;
                int len = rasterSaveFN.Length - 4;
                string rasterSaveFN2 = rasterSaveFN.Insert(len, "PA");
                IRaster paRaster = Raster.CreateRaster(rasterSaveFN2, "", oRaster.NumColumns, oRaster.NumRows, 1, oRaster.DataType, rasterParameters);
                paRaster.Bounds = oRaster.Bounds.Copy();
                paRaster.NoDataValue = oRaster.NoDataValue;
                paRaster.Projection = oRaster.Projection;

                double oValue = 0;

                for (int oRows = 0; oRows < oRaster.NumRows - 1; oRows++)
                {
                    for (int oCols = 0; oCols < oRaster.NumColumns - 1; oCols++)
                    {
                        oValue = oRaster.Value[oRows, oCols];

                        if (oValue > 0)
                        {
                            paRaster.Value[oRows, oCols] = oValue;
                        }
                        else
                        {
                            paRaster.Value[oRows, oCols] = 1;
                        }
                    }
                }
                paRaster.Save();
            }

            catch(Exception ex)
            {

            }
        }
    }
}
