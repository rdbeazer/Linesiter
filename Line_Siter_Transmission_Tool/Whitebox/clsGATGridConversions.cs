using DotSpatial.Data;
using GeospatialFiles;
using System;
using System.Windows.Forms;

namespace LineSiterSitingTool.Whitebox
{
    internal class clsGATGridConversions
    {
        public IRaster _rasterToConvert { get; set; }

        public string _statusMessage { get; set; }

        public string _gridToConvert { get; set; }

        public string _conversionRaster { get; set; }

        public IRaster _bnds { get; set; }

        public IRaster conRaster;

        public void convertToGAT()
        {
            GATGrid newRaster = new GATGrid();
            newRaster.WriteChangesToFile = true;
            newRaster.HeaderFileName = _rasterToConvert.Filename.Substring(0, _rasterToConvert.Filename.Length - 4);
            newRaster.DataFileName = _rasterToConvert.Filename.Substring(0, _rasterToConvert.Filename.Length - 4) + ".tas";
            newRaster.GridResolution = _rasterToConvert.CellHeight;
            newRaster.DataType = "float";
            newRaster.DataScale = "continuous";
            newRaster.North = _rasterToConvert.Bounds.Top();
            newRaster.South = _rasterToConvert.Bounds.Bottom();
            newRaster.West = _rasterToConvert.Bounds.Left();
            newRaster.East = _rasterToConvert.Bounds.Right();
            newRaster.Minimum = (float)_rasterToConvert.Minimum;
            newRaster.Maximum = (float)_rasterToConvert.Maximum;
            newRaster.InitializeGrid(_rasterToConvert.NumColumns, _rasterToConvert.NumRows);
            newRaster.WriteHeaderFile();
            newRaster.SetBlockData();
            //       MessageBox.Show(Convert.ToString(rasterToConvert.NumRows - 1));
            for (int nRow = 0; nRow < _rasterToConvert.NumRows - 1; nRow++)
            {
                for (int nCol = 0; nCol < _rasterToConvert.NumColumns - 1; nCol++)
                {
                    newRaster[nCol, nRow] = (float)_rasterToConvert.Value[nRow, nCol];
                }
            }
            newRaster.WriteDataInMemoryToFile();
            newRaster.ReleaseMemoryResources();
            return;
        }

        public void convertBGD()
        {
            try
            {
                GATGrid gt = new GATGrid();
                gt.HeaderFileName = _gridToConvert + ".dep";
                gt.DataFileName = _gridToConvert + ".tas";
                conRaster = Raster.CreateRaster(_conversionRaster.Substring(0, _conversionRaster.Length - 4) + "new.bgd", null, gt.NumberColumns, gt.NumberRows, 1, typeof(float), null);
                conRaster.CellHeight = gt.GridResolution;
                conRaster.Bounds = _bnds.Bounds;
                conRaster.NoDataValue = -32768;
                //conRaster.Save();
                //MessageBox.Show(Convert.ToString(gt.NumberRows - 1));
                for (int nCol = 0; nCol < gt.NumberColumns - 1; nCol++)
                {
                    for (int nRow = 0; nRow < gt.NumberRows - 1; nRow++)
                    {
                        conRaster.Value[nRow, nCol] = (float)gt[nCol, nRow];
                    }
                }
                conRaster.Save();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error: " + ex + "\n has occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}