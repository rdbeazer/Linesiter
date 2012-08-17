using System;
using System.Collections.Generic;
using DotSpatial.Data;
using System.Linq;
using System.Windows.Forms;


namespace LineSiterSitingTool
{
    class clsprepgatraster
    {
        string backlinkFilename = "";
        string outputAccumFilename = "";

        public void prepareGATRasters(string savePath, Cursor xcurs, IRaster backlink, IRaster outAccumRaster, IRaster outPathRaster, string outputPathFilename)
        {

            backlinkFilename = savePath + @"\backlink";
            clsGATGridConversions prepareGATs = new clsGATGridConversions();
            prepareGATs._rasterToConvert = backlink;
            prepareGATs._statusMessage = "Preparing GAT Rasters, Please Wait";
            xcurs = Cursors.WaitCursor;
            prepareGATs.convertToGAT();
            outputAccumFilename = savePath + @"\outputAccumRaster";
            prepareGATs._rasterToConvert = outAccumRaster;
            prepareGATs.convertToGAT();
            outputPathFilename = savePath + @"\outputPathRaster";
            prepareGATs._rasterToConvert = outPathRaster;
            outPathRaster.Filename = outputPathFilename + ".bgd";
            outPathRaster.Save();
            prepareGATs.convertToGAT();
            xcurs = Cursors.Default;
        }
    }
}
