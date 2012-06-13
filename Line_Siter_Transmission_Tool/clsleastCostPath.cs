using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DotSpatial.Data;
using DotSpatial.Controls;
using DotSpatial.Symbology;
using DotSpatial.Topology;
using DotSpatial.Projections;
using System.ComponentModel;
using System.IO;

namespace nx09SitingTool
{
    class clsleastCostPath
    {


        #region variables

        Dictionary<string, double> costValues = new Dictionary<string, double>();
        Dictionary<string, double> VisitedList = new Dictionary<string, double>();
        IRaster accumCostRaster = new Raster();
        DotSpatial.Controls.Header.HeaderControl  appsts;
        //IRaster backlinkRaster = new Raster();
        FileInfo pathInfo = null;
        //double noDataValue = -999999;
        //double startValue = 1234567890;
        //int passNumber = 0;
        string[] directionalArray = new string[9] { "NW", "N   ", "NE", "W  ", "C   ", "E  ", "SW", "S   ", "SE" };
        int[] numDirectionalArray = new int[8] { 0, 1, 2, 3, 4, 5, 6, 7 };
        string[] nDirectionalArray = new string[8] { "E", "NE", "N", "NW", "W", "SW", "S", "SE" };
        int[] bklnkDirectionalArray = new int[8] {4, 5, 6, 7, 0, 1, 2, 3};
        string[] bDirectionalArray = new string[8] { "W>>E", "SW>>NE", "S>>N", "SE>>NW", "E>>W", "NE>>SW", "N>>S", "NW>>SE" };
        //int[] rowOffset = new int[8] { 0, 1, 1, 1, 0, -1, -1, -1};
        //int[] colOffset = new int[8] {1, 1, 0, -1, -1, -1, 0, 1};
        int[] rowOffset = new int[8] { 0, -1, -1, -1, 0, 1, 1, 1 };
        int[] colOffset = new int[8] { 1, 1, 0, -1, -1, -1, 0, 1 };
        double[] costModifier = new double[8] { 0, 1.41, 0, 1.41, 0, 1.41, 0, 1.41 };
        //bool[] isDiag = new bool[8] { true, false, true, false, false, true, false, true };
        double dirAccumCost = 0;
        private int OldRowVal = 0;
        private int OldColVal = 0;
        private int RowVal = 0;
        private int ColVal = 0;
        private double AccumCost = 0.0;
        private bool endCell = false;
        private bool firstRun = true;
        private int endCelDir = 0;
        IMap _MW;
        BackgroundWorker _BW;
        int progress = 0;
        System.Windows.Forms.ToolStripStatusLabel tslStatus = new System.Windows.Forms.ToolStripStatusLabel();
        System.Windows.Forms.ToolStripStatusLabel tslPass = new System.Windows.Forms.ToolStripStatusLabel();
        List<Coordinate> shCoords = new List<Coordinate>();
        //private int startCoords = 0;
        //private int endCoords = 0;
        //private int beginRow = 0;
        clsLCPCoords lc = new clsLCPCoords();
        //StreamWriter sw = new StreamWriter(@"c:\temp\LineSiter\accumLog.txt", true);
        //string streamString = "";
        //private int endRow = 0;
        //private int endCol = 0;
        //private int startRow = 0;
        //private int startCol = 0;


        public bool[] validCellBlock(int startCol, int startRow, int numRows, int numCols)
        {
            //Checks cells in raster to verify not at edge of the raster
            //Assigns values to the boolean array--false for edge cells true for valid cells
            //RDB 5.20.2011

            // 4  3  2  | NW  N  NE 5 6 7
            // 5  0  1  |  W   O   E  4 0 8 
            // 6  7  8  | SW  S  SE 3 2 1

            bool[] validCellBlock = new bool[4];

            //initialize array to carry all false values
            //North cellblock 2,3,4 if true--any cell in row is within raster
            validCellBlock[0] = true;
            //West Cellblock 4,5,6 if true--any cell in row is within raster
            validCellBlock[1] = true;
            //South Cellblock 6,7,8 if true--any cell in row is within raster
            validCellBlock[2] = true;
            //East Cellblock 1,2,8 if true--any cell in row is within raster
            validCellBlock[3] = true;

            //Check North
            if (startRow - 1 < 0)
            {
                validCellBlock[0] = false;
            }
            //Check West
            if (startCol - 1 < 0)
            {
                validCellBlock[1] = false;
            }
            //Check South
            if (startRow > numRows - 2)
            {
                validCellBlock[2] = false;
            }
            //Check East
            if (startCol > numCols - 2)
            {
                validCellBlock[3] = false;
            }
            return validCellBlock;
        }

        public bool validCells(int startCol, int startRow, int numRows, int numCols, int direction)
        {
            switch (direction)
            {

                case 0:
                    //Check East
                    if (startCol > numCols - 2)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case 1:
                    //check northeast
                    if (startCol > numCols - 2 | startRow - 1 < 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case 2:
                    //check north
                    if (startRow - 1 < 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case 3:
                    //check northwest
                    if (startRow - 1 < 0 | startCol - 1 < 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case 4:
                    //Check West
                    if (startCol - 1 < 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case 5:
                    //Check Southwest
                    if (startCol - 1 < 0 | startRow > numRows - 2)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case 6:
                    //Check South
                    if (startRow > numRows - 2)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case 7:
                    //Check Southeast
                    if (startRow > numRows - 2 | startCol > numCols - 2)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                default:
                    return false;
            }
        }

        #endregion

        #region properties

        private string _streamString;
        public string streamString
        {
            get { return _streamString; }
            set { _streamString = value; }
        }

        //raster passed in with weighted cost values
        private IRaster _lcpRaster;
        public IRaster LCPRaster
        {
            get { return _lcpRaster; }
            set { _lcpRaster = value; }
        }

        //passed in file location for saving raster information
        private string _saveRasterLoc;
        public string saveRasterLoc
        {
            get { return _saveRasterLoc; }
            set { _saveRasterLoc = value; }
        }

        //passed in location for save least cost path information
        private string _saveLCPLoc;
        public string saveLCPLoc
        {
            get { return _saveLCPLoc; }
            set { _saveLCPLoc = value; }
        }

        private IRaster _backlinkRaster;
        public IRaster backlinkRaster
        {
            get { return _backlinkRaster; }
            set { _backlinkRaster = value; }
        }
        #endregion


        #region methods

        private void calculateCells(double centerCellCost, clsLCPCoords lc)
        {
            //calculate the cost values within a 3x3 moving window
            bool[] validCBlock = new bool[4];

            //assign center cell accumulated costs for starting cell
            centerCellCost = 0;

            //first run sets the starting coordinates and ending coordinates
            if (firstRun == true)
            {
                string startCoords = Convert.ToString(lc.startRow) + "," + Convert.ToString(lc.startCol);
                string endCoords = Convert.ToString(lc.EndRow) + "," + Convert.ToString(lc.EndCol);
                //check for position of coordinates to make sure it is not outside of the raster's bounds
                validCBlock = validCellBlock(lc.startCol, lc.startRow, _lcpRaster.NumRows, _lcpRaster.NumColumns);

                //assign centercell value of zero in accumulative cost raster
                accumCostRaster.Value[lc.startRow, lc.startCol] = 0;

                //check to make sure dictionary does not contain start value then add it to dictionary
                if (!VisitedList.ContainsKey(startCoords))
                {
                    VisitedList.Add(startCoords, 1234567890);
                }

                //bring in value from cost raster based on start row/col
                centerCellCost = _lcpRaster.Value[lc.startRow, lc.startCol];

                //Initialize AccumCost value to zero
                AccumCost = 0;

                //Assign initial row and column values and toggle first run to false
                RowVal = lc.startRow;
                ColVal = lc.startCol;
                addToBacklinkRaster();
                firstRun = false;
            }
            else if (firstRun == false)
            {
                validCBlock = validCellBlock(ColVal, RowVal, _lcpRaster.NumRows, _lcpRaster.NumColumns);
                dirAccumCost = VisitedList[Convert.ToString(RowVal) + "," + Convert.ToString(ColVal)];
                centerCellCost = LCPRaster.Value[RowVal, ColVal];
                AccumCost = 0;
            }
            string coord = null;
            foreach(int x in numDirectionalArray)
            {
                if (validCells(ColVal, RowVal, LCPRaster.NumRows, LCPRaster.NumColumns, x) == true)
                {
                    double cellValue = _lcpRaster.Value[RowVal + rowOffset[x], ColVal + colOffset[x]];
                    if (cellValue != -999999)
                    {
                        double newCellValue = Math.Round(dirAccumCost + (costModifier[x] + ((centerCellCost + cellValue) / 2)), 2);
                        coord = Convert.ToString(RowVal + rowOffset[x]) + "," + Convert.ToString(ColVal + colOffset[x]);
                        addCostToAccumCostRaster(coord, newCellValue, rowOffset[x], colOffset[x]);
                        //addToBacklinkRaster(x, rowOffset[x], colOffset[x], coord);
                    }
                }
            }
            //remove current center cell from the least cost values--it no longer should be considered
            costValues.Remove(Convert.ToString(RowVal) + "," + Convert.ToString(ColVal));
        }

        private void addCostToAccumCostRaster(string coord, double cellValue, int offsetX, int offsetY)
        {
            try
            {
                if (!VisitedList.ContainsKey(coord))
                {
                    VisitedList.Add(coord, cellValue);
                    if (!costValues.ContainsKey(coord))
                    {
                        costValues.Add(coord, cellValue);
                    }
                    accumCostRaster.Value[RowVal + offsetX, ColVal + offsetY] = cellValue;
                }
            }

            catch (Exception ex)
            {
                string exc = Convert.ToString(ex);
            }
        }

        private void addToBacklinkRaster()
        {
            try
            {
                for (int bklnkCells = 0; bklnkCells < 8; bklnkCells++)
                {
                    if (validCells(ColVal + colOffset[bklnkCells], RowVal + rowOffset[bklnkCells], LCPRaster.NumRows, LCPRaster.NumColumns, bklnkCells) == true)
                    {
                        if (backlinkRaster.Value[RowVal + rowOffset[bklnkCells], ColVal + colOffset[bklnkCells]] == -1)
                        {
                            int brow = RowVal + rowOffset[bklnkCells];
                            int bCol = ColVal + colOffset[bklnkCells];
                            int bdir = bklnkDirectionalArray[bklnkCells];
                            backlinkRaster.Value[RowVal + rowOffset[bklnkCells], ColVal + colOffset[bklnkCells]] = bklnkDirectionalArray[bklnkCells];
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                string exc = Convert.ToString(ex);
            }
        }

        public void runLCP(clsLCPCoords lc, IMap mw, BackgroundWorker bw, System.Windows.Forms.ToolStripStatusLabel status)
        {
            try
            {
                double centerCellCost = 0;
                _MW = mw;
                _BW = bw;
                tslStatus = status;
                //string[] rasOps = null;
                accumCostRaster = Raster.CreateRaster(_saveRasterLoc + "\\ac_Raster.bgd", null, _lcpRaster.NumColumns, _lcpRaster.NumRows, 1, typeof(double), null);
                backlinkRaster = Raster.CreateRaster(_saveRasterLoc + "\\bl_Raster.bgd", null, _lcpRaster.NumColumns, _lcpRaster.NumRows, 1, typeof(int), null);
                accumCostRaster.Bounds = _lcpRaster.Bounds;
                accumCostRaster.Projection = _lcpRaster.Projection;
                accumCostRaster.NoDataValue = -999999;
                accumCostRaster.Save();
                backlinkRaster.Bounds = _lcpRaster.Bounds;
                backlinkRaster.Projection = _lcpRaster.Projection;
                for (int lrows = 0; lrows < backlinkRaster.NumRows - 1; lrows++)
                {
                    for (int lcols = 0; lcols < backlinkRaster.NumColumns - 1; lcols++)
                    {
                        backlinkRaster.Value[lrows, lcols] = -1;
                    }
                }
                backlinkRaster.Save();
                //int tCell = LCPRaster.NumRows * LCPRaster.NumColumns; // need to account for no data cells
                int tCell = (int)LCPRaster.NumValueCells - 10;
                int itNum = 0;
                while (itNum <= tCell)
                {
                    if (firstRun == true)
                    {
                        calculateCells(centerCellCost, lc);
                        firstRun = false;
                    }
                    else
                    {
                        calculateCells(centerCellCost, lc);
                    }
                    itNum++;
                    progress = (int)(((float)itNum/(float)tCell)*100);
                    tslStatus.Text = "Creating Cost and Backlink Rasters";
                    _BW.ReportProgress(progress);
                    processLeastCost(centerCellCost);
                }
                accumCostRaster.Save();
                backlinkRaster.Save();
                getPath(lc);
                //progress = 100;
                //tslStatus.Text = "Process Complete";
                //_BW.ReportProgress(progress);
            }

            catch (Exception ex)
            {
                string exc = Convert.ToString(ex);
            }
        }

        private void processLeastCost(double centerCell)
        {
            double lowestValue = 0;
            //find the lowest cost in the list
            lowestValue = (from lcvs in costValues
                           select lcvs.Value)
                           .Min();

            //get coordinates for the lowest cost cell(s) in the list
            var lowValCell = (from lcpEntries in costValues
                              where lcpEntries.Value == lowestValue
                              select lcpEntries).First();

            string lcpKeys = null;
            int i = 0;
            int xCoord = 0;
            int yCoord = 0;

            //remove the least cost value from the visited listed so it doesn't become the center cell again.  It's cost is final.
            foreach (var visitEntry in VisitedList)
            {
                costValues.Remove(Convert.ToString(visitEntry.Key) + "  Value: " + Convert.ToString(visitEntry.Value));
            }


            //get the coordinate back out
            string lKey = Convert.ToString(lowValCell.Key);
            string[] keySplit = lKey.Split(new char[] { ',' });
            xCoord = Convert.ToInt32(keySplit[0]);
            yCoord = Convert.ToInt32(keySplit[1]);
            OldColVal = ColVal;
            OldRowVal = RowVal;
            RowVal = xCoord;
            ColVal = yCoord;
            addToBacklinkRaster();
            AccumCost = centerCell + lowestValue;
            lcpKeys = lKey;
            i++;
            costValues.Remove(lcpKeys);
            string coords = Convert.ToString(RowVal) + "," + Convert.ToString(ColVal);
            costValues[coords] = AccumCost;
        }

        public void getPath(clsLCPCoords lc)
        {
            endCell = false;
            string[] lcpOps = new string[1];
            double cellValue1 = 0;
            RowVal = lc.EndRow;
            ColVal = lc.EndCol;
            IRaster bklnkRaster = backlinkRaster;
            //IRaster bklnkRaster = Raster.OpenFile(_saveRasterLoc + "\\bl_Raster.bgd");
            int direction = (int)bklnkRaster.Value[lc.EndRow, lc.EndCol];
            int newCenter;
            string coords = null;
            int xcoord = 0;
            int ycoord = 0;
            Coordinate shCoordAdd = new Coordinate();
            int its = 0;
            try
            {
                //create LCP raster

                IRaster lcpSavedRaster = Raster.CreateRaster(saveLCPLoc + "\\lcpRas.bgd", null, LCPRaster.NumColumns, LCPRaster.NumRows, 1, typeof(int), lcpOps);
                lcpSavedRaster.Bounds = LCPRaster.Bounds.Copy();
                lcpSavedRaster.Projection = LCPRaster.Projection;
                lcpSavedRaster.NoDataValue = -1;
                lcpSavedRaster.Save();

                for (int lrows = 0; lrows < lcpSavedRaster.NumRows - 1; lrows++)
                {
                    for (int lcols = 0; lcols < lcpSavedRaster.NumColumns - 1; lcols++)
                    {
                        lcpSavedRaster.Value[lrows, lcols] = -1;
                    }
                }

                lcpSavedRaster.Save();

                traverseBacklink(bklnkRaster, lcpSavedRaster, LCPRaster, lc);
                exportBacklinkToText();
                lcpSavedRaster.Save();
            }

            catch (Exception ex)
            {
                string exc = Convert.ToString(ex);
            }
        }

        private void exportBacklinkToText()
        {
            StreamWriter bklnk = new StreamWriter(saveLCPLoc + @"\bklinkText.txt");
            string bklnkString;
            progress = 0;
            for (int bRow = 0; bRow < backlinkRaster.NumRows; bRow++)
            {
                bklnkString = string.Empty;
                for (int bCol = 0; bCol < backlinkRaster.NumColumns; bCol++)
                {
                    bklnkString += Convert.ToString(backlinkRaster.Value[bRow, bCol]) + ", ";
                }
                //bklnk.Write(bklnkString);
                bklnk.WriteLine(bklnkString);
                progress = (int)(((float)bRow / (float)backlinkRaster.NumRows) * 100);
                tslStatus.Text = "Creating Backlink Text File";
                _BW.ReportProgress(progress);
            }
        }

        private void traverseBacklink(IRaster bklnkRaster, IRaster LCPSavedRaster, IRaster lcpRaster, clsLCPCoords lc)
        {
            int failSafe = 0;
            int bkRowVal = lc.EndRow, bkColVal = lc.EndCol;
            int[] yOffset = new int[8] { -1, -1, 0, 1, 1, 1, 0, -1 };
            int[] xOffset = new int[8] { 0, 1, 1, 1, 0, -1, -1, -1 };
            int direction = (int)bklnkRaster.Value[bkRowVal, bkColVal];
            LCPSavedRaster.Value[bkRowVal, bkColVal] = 1;
            LCPSavedRaster.Save();
            int failSafeCeiling = 33000;
            _streamString = streamString + "Beginning cost value: " + Convert.ToString(lcpRaster.Value[bkRowVal, bkColVal]);
            //move to next cell
            while (direction != 1234567890 && failSafe < failSafeCeiling)
            {
                _streamString = streamString + "\n x: " + Convert.ToString(bkRowVal) + "(" + Convert.ToString(xOffset[direction])+ ")" + " y: " + Convert.ToString(bkColVal) + "(" +  Convert.ToString(yOffset[direction]) + ")" + " Direction: " + Convert.ToString(direction) + " Cost value: " + Convert.ToString(accumCostRaster.Value[bkRowVal, bkColVal]);
                direction = (int)bklnkRaster.Value[bkRowVal + xOffset[direction], bkColVal + yOffset[direction]];
                bkRowVal = bkRowVal + xOffset[direction];
                bkColVal = bkColVal + yOffset[direction];
                LCPSavedRaster.Value[bkRowVal, bkColVal] = 1;
                failSafe++;
                progress = (int)(((float)failSafe/(float)failSafeCeiling)*100);
                tslStatus.Text = "Creating Least Cost Path";
                _BW.ReportProgress(progress);
            }
            LCPSavedRaster.Save();
        }

        private void saveShapeFile()
        {
            string pathS = pathInfo.DirectoryName + @"\startEndPoints.shp";
            FeatureSet startEndPoints = new FeatureSet(FeatureType.Point);
            DataColumn pointID = new DataColumn("PointID");
            int pID = 0;
            startEndPoints.DataTable.Columns.Add(pointID);

            startEndPoints.Projection = KnownCoordinateSystems.Projected.UtmNad1983.NAD1983UTMZone12N;

            //startEndPoints.Projection = _MW.Projection;
            foreach (Coordinate xys in shCoords)
            {
                DotSpatial.Topology.Point shpPoint = new DotSpatial.Topology.Point(xys);
                IFeature fs = startEndPoints.AddFeature(shpPoint);
                fs.DataRow["PointID"] = pID;
                pID++;
            }
            startEndPoints.SaveAs(pathS, true);
            _MW.Layers.Add(startEndPoints);
            _MW.ResetBuffer();
        }
        #endregion

    }
}
