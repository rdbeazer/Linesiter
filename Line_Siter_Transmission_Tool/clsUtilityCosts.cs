using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Data;

namespace LineSiterSitingTool
{
    class clsUtilityCosts
    {
        public IRaster backlink { get; set; }
        public IRaster outAccumRaster { get; set; }
        public IRaster outPathRaster { get; set; }
        public IFeatureSet outPathUtility { get; set; }


    }
}
