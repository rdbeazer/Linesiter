using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Data;

namespace LineSiterSitingTool
{
    class clsUtilityCosts
    {
        private IRaster _backlink;
        public IRaster backlink
        {
            get { return _backlink; }
            set { _backlink = value; }
        }

        private IRaster _outAccumRaster;
        public IRaster outAccumRaster
        {
            get { return _outAccumRaster; }
            set { _outAccumRaster = value; }
        }

        private IRaster _outPathRaster;
        public IRaster outPathRaster
        {
            get { return _outPathRaster; }
            set { _outPathRaster = value; }
        }

        private IFeatureSet _outPathUtility;
        public IFeatureSet outPathUtility
        {
            get { return _outPathUtility; }
            set { _outPathUtility = value; }
        }


    }
}
