using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Data;

namespace LineSiterSitingTool
{
    class clsCreateBackgroundRasters
    {

        public IRaster saveRaster(string path, string fileName, IRaster bounds)
        {
            IRaster saveRaster = Raster.CreateRaster(path + fileName + ".bgd", null, bounds.NumColumns, bounds.NumRows, 1, typeof(float), null);
            saveRaster.Bounds = bounds.Bounds;
            saveRaster.Projection = bounds.Projection;
            saveRaster.Save();
            return saveRaster;
        }

    }
}
