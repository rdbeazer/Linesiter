using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nx09SitingTool
{
    public class clsLCPCoords
    {
        private int _startRow;
        public int startRow
        {
            get { return _startRow; }
            set { _startRow = value; }
        }

        private int _startCol;
        public int startCol
        {
            get { return _startCol; }
            set { _startCol = value; }
        }

        private int _EndRow;
        public int EndRow
        {
            get { return _EndRow; }
            set { _EndRow = value; }
        }

        private int _EndCol;
        public int EndCol
        {
            get { return _EndCol; }
            set { _EndCol = value; }
        }
    }
}
