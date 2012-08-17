using DotSpatial.Controls;
using DotSpatial.Data;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LineSiterSitingTool
{
    internal class clsCostWeight
    {
        private IRaster rasterToConvert;
        private string costFileName = "";

        public void calculate_Cost_Weight(string savePath, IRaster bounds, IMap _mapLayer, int currentPass, IRaster additiveCosts, IRaster utilityCosts)
        {
            try
            {
                clsRasterOps mapAlgCW = new clsRasterOps(_mapLayer);
                IRaster addUt = additiveCosts;
                IRaster div1 = utilityCosts;
                IRaster div2 = Raster.Open(savePath + @"\mcraster.bgd");
                IRaster costWeightRaster = Raster.CreateRaster(savePath + @"\CostWeightRaster_" + currentPass + ".bgd", null, bounds.NumColumns, bounds.NumRows, 1, typeof(double), null);
                costFileName = savePath + @"\CostWeightRaster_" + currentPass + ".bgd";
                costWeightRaster.Bounds = bounds.Bounds;
                costWeightRaster.Projection = _mapLayer.Projection;
                costWeightRaster.Save();
                mapAlgCW.rasterDivision(div1, div2, costWeightRaster);
                rasterToConvert = costWeightRaster;
                List<IRaster> additive = new List<IRaster>();
                additive.Add(addUt);
                additive.Add(costWeightRaster);
                mapAlgCW.rasterAddition(additive, additiveCosts);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + " has occured.", "Generic Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}