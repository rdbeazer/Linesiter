using DotSpatial.Data;
using LineSiterSitingTool.Whitebox;
using System.Windows.Forms;

namespace LineSiterSitingTool
{
    internal class clsprepgatraster
    {
        private string backlinkFilename = "";
        private string outputAccumFilename = "";

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