using DotSpatial.Data;

namespace LineSiterSitingTool
{
    internal class clsUtilityCosts
    {
        public IRaster backlink { get; set; }

        public IRaster outAccumRaster { get; set; }

        public IRaster outPathRaster { get; set; }

        public IFeatureSet outPathUtility { get; set; }
    }
}