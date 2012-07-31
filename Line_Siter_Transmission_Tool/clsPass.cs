using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Data;

namespace LineSiterSitingTool
{
    public class clsPass
    {
        
        private IRaster _backlink;
        public IRaster backlink
        {
            get{return _backlink;}
            set {_backlink = value;}
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

        private string _savePath;
        public string savePath
        {
            get { return _savePath; }
            set { _savePath = value; }
        }

        private FeatureSet _LCPAShpe;
        public FeatureSet LCPA
        {
            get { return _LCPAShpe; }
            set { _LCPAShpe = value; }
        }



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
