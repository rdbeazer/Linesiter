using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using GeospatialFiles;
using System.Windows.Forms;
using DotSpatial.Data;

namespace nx09SitingTool
{
    class clsGATGridConversions
    {

        private IRaster rasterToConvert;
        public IRaster _rasterToConvert
        {
            get {return rasterToConvert; }
            set {rasterToConvert = value; }
        }

        private string statusMessage;
        public string _statusMessage
        {
            get { return statusMessage; }
            set {statusMessage = value; }
        }

        private string gridToConvert;
        public string _gridToConvert
        {
            get { return gridToConvert; }
            set { gridToConvert = value; }
        }

        private string conversionRaster;
        public string _conversionRaster
        {
            get { return conversionRaster; }
            set { conversionRaster = value; }
        }

        private IRaster bnds;
        public IRaster _bnds
        {
            get { return bnds; }
            set { bnds = value; }
        }

        public IRaster conRaster;

        public void convertToGAT()
        {
            GATGrid newRaster = new GATGrid();
            newRaster.WriteChangesToFile = true;
            newRaster.HeaderFileName = rasterToConvert.Filename.Substring(0, rasterToConvert.Filename.Length - 4);
            newRaster.DataFileName = rasterToConvert.Filename.Substring(0, rasterToConvert.Filename.Length - 4) + ".tas";
            newRaster.GridResolution = rasterToConvert.CellHeight;
            newRaster.DataType = "float";
            newRaster.DataScale = "continuous";
            newRaster.North = rasterToConvert.Bounds.Top();
            newRaster.South = rasterToConvert.Bounds.Bottom();
            newRaster.West = rasterToConvert.Bounds.Left();
            newRaster.East = rasterToConvert.Bounds.Right();
            newRaster.Minimum = (float)rasterToConvert.Minimum;
            newRaster.Maximum = (float)rasterToConvert.Maximum;
            newRaster.InitializeGrid(rasterToConvert.NumColumns, rasterToConvert.NumRows);
            newRaster.WriteHeaderFile();
            newRaster.SetBlockData();
            MessageBox.Show(Convert.ToString(rasterToConvert.NumRows - 1));
            for (int nRow = 0; nRow < rasterToConvert.NumRows - 1; nRow++)
            {
                for (int nCol = 0; nCol < rasterToConvert.NumColumns - 1; nCol++)
                {
                    newRaster[nCol, nRow] = (float)rasterToConvert.Value[nRow, nCol];
                }
            }
            newRaster.WriteDataInMemoryToFile();
          //  newRaster.ReleaseMemoryResources();
            return;
        }


        public void convertBGD()
        {
            try
            {
                GATGrid gt = new GATGrid();
                gt.HeaderFileName = gridToConvert + ".dep";
                gt.DataFileName = gridToConvert + ".tas";
                conRaster = Raster.CreateRaster(conversionRaster + "new.bgd", null, gt.NumberColumns, gt.NumberRows, 1, typeof(float), null);
                conRaster.CellHeight = gt.GridResolution;
                conRaster.Bounds = bnds.Bounds;
                conRaster.NoDataValue = -32768;
                //conRaster.Save();
                MessageBox.Show(Convert.ToString(gt.NumberRows - 1));
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
            }
        }
    }
}
