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

namespace LineSiterSitingTool
{
    class clsCreateWeightedRasters
    {
        public void createWeightedRasters(string newPath, IRaster oRaster, clsMonteCarlo MC, IRaster bounds)
        {
            int curWeight = 0;
            string[] weights = new string[5] { "LSHigh", "LSMedHigh", "LSMedium", "LSMedLow", "LSLow" };
            foreach (double weight in MC.assignedWeights)
            {
                IRaster weightedRaster = Raster.CreateRaster(newPath + "\\" + weights[curWeight] + ".bgd", null, oRaster.NumColumns, oRaster.NumRows, 1, typeof(double), null);
                weightedRaster.Bounds = bounds.Bounds;
                weightedRaster.NoDataValue = bounds.NoDataValue;
                weightedRaster.Projection = bounds.Projection;
                double oValue = 0;

                for (int oRows = 0; oRows < oRaster.NumRows - 1; oRows++)
                {
                    for (int oCols = 0; oCols < oRaster.NumColumns - 1; oCols++)
                    {
                        oValue = oRaster.Value[oRows, oCols];

                        if (oValue != oRaster.NoDataValue)
                        {
                            if (oValue == -1)
                            {
                                //weightedRaster.Value[oRows, oCols] = Math.Abs(oValue) * weight;
                                weightedRaster.Value[oRows, oCols] = weight;
                            }
                            else
                            {
                                weightedRaster.Value[oRows, oCols] = 1;
                            }
                        }
                        else
                        {
                            weightedRaster.Value[oRows, oCols] = 1;
                        }
                    }
                }
                weightedRaster.Save();
                curWeight++;
            }
        }
    }
}
