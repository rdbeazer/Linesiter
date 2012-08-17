using DotSpatial.Data;
using LineSiterSitingTool.Whitebox;
using System.Windows.Forms;

namespace LineSiterSitingTool
{
    internal class clsprepgatraster
    {
        public void prepareGATRasters(string savePath, IRaster backlink, IRaster outAccumRaster, IRaster outPathRaster, string outputPathFilename)
        {
            clsGATGridConversions prepareGATs = new clsGATGridConversions();
            prepareGATs._rasterToConvert = backlink;
            prepareGATs._statusMessage = "Preparing GAT Rasters, Please Wait";
            prepareGATs.convertToGAT();
            prepareGATs._rasterToConvert = outAccumRaster;
            prepareGATs.convertToGAT();
            outputPathFilename = savePath + @"\outputPathRaster";
            prepareGATs._rasterToConvert = outPathRaster;
            outPathRaster.Filename = outputPathFilename + ".bgd";
            outPathRaster.Save();
            prepareGATs.convertToGAT();
        }
    }
}