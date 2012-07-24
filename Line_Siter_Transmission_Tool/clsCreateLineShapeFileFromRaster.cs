using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Data;
using DotSpatial.Controls;
using DotSpatial.Topology;
using System.Windows.Forms;
using System.IO;

namespace LineSiterSitingTool
{ 
    class clsCreateLineShapeFileFromRaster
    {

        public clsCreateLineShapeFileFromRaster()
        {

        }
        const int NoData = -32768;
        const double lnOf2 = 0.693147180559945;

        public void createShapefile(IRaster rastConvert, int rastVal, string saveLocation, List<string> headers, List<string> attributes, IMap mw, string name, FeatureSet lineFS)
        {
            try
            {
                List<Coordinate> pthXYs = new List<Coordinate>();

                for (int nRows = 0; nRows < rastConvert.NumRows; nRows++)
                {
                    for (int nCols = 0; nCols < rastConvert.NumColumns; nCols++)
                    {
                        if (rastConvert.Value[nRows, nCols] == rastVal)
                        {
                            Coordinate xy = new Coordinate();
                            xy = rastConvert.CellToProj(nRows, nCols);
                            pthXYs.Add(xy);
                        }
                    }
                }
                LineString pathString = new LineString(pthXYs);
                IFeature pathLine = lineFS.AddFeature(pathString);
                int a = 0;
                foreach (string head in headers)
                    {
                        pathLine.DataRow[head] = attributes[a];
                        a++;
                    }
                lineFS.Name = name;
                lineFS.Extent = rastConvert.Extent;
                lineFS.Projection = mw.Projection;
                //lineFS.Save();
                lineFS.SaveAs(saveLocation, true);
            }
            /*catch (Exception ex)
            {
                MessageBox.Show("Error :" + ex + " has occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/

            catch (System.ArgumentException ag)
            {
                MessageBox.Show("Shapefile input does not have the required number of points. \n Please check the input parameters. \n" + ag, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        public void createLineFromBacklink(IRaster bklink, string saveLocation, List<string> headers, List<string> attributes, IMap mw, string name, FeatureSet lineFS, int startX, int startY, int endX, int endY)
        {
            try
            {
                //int X = startX;
                //int Y = startY;
                int X = endX;
                int Y = endY;
                double cellValue = 0;
                int[] dX = new int[8] { 1, 1, 1, 0, -1, -1, -1, 0 };
                int[] dY = new int[8] { -1, 0, 1, 1, 1, 0, -1, -1 };
                bool cont = true;
                int c = 0;

                List<Coordinate> pthXYs = new List<Coordinate>();
                do
                {
                    cellValue = bklink.Value[X, Y];
                    Coordinate xy = new Coordinate();
                    xy = bklink.CellToProj(X, Y);
                    pthXYs.Add(xy);
                    if (pthXYs.Count > 250)
                    {
                        cont = false;
                    }
                    if (cellValue > 0)
                    {
                        c = Convert.ToInt32(Math.Log(cellValue) / lnOf2);
                        X += dX[c];
                        Y += dY[c];
                    }
                    else
                    {
                        cont = false;
                    }
                    //if (cellValue == 1)
                    //{
                    //    X = X + 1;
                    //    Y = Y - 1;
                    //}
                    //else if (cellValue == 2)
                    //{
                    //    Y = Y - 1;
                    //}
                    //else if (cellValue == 4)
                    //{
                    //    X = X - 1;
                    //    Y = Y - 1;
                    //}
                    //else if (cellValue == 8)
                    //{
                    //    X = X - 1;
                    //}
                    //else if (cellValue == 16)
                    //{
                    //    X = X - 1;
                    //    Y = Y + 1;
                    //}
                    //else if (cellValue == 32)
                    //{
                    //    Y = Y + 1;
                    //}
                    //else if (cellValue == 64)
                    //{
                    //    X = X + 1;
                    //    Y = Y + 1;
                    //}
                    //else if (cellValue == 128)
                    //{
                    //    X = X + 1;
                    //}
                    //else if (cellValue == 0)
                    //{
                    //    cont = false;
                    //}
                    //Coordinate xy = new Coordinate();
                    //xy = bklink.CellToProj(X, Y);
                    //pthXYs.Add(xy);
                    //LineString pathString = new LineString(pthXYs);
                    //IFeature pathLine = lineFS.AddFeature(pathString);
                    //lineFS.Name = name;
                    //lineFS.Extent = bklink.Extent;
                    //lineFS.Projection = mw.Projection;
                    //lineFS.SaveAs(saveLocation, true);
                }

                while (cont == true);

                //Coordinate xy2 = new Coordinate();
                //xy2 = bklink.CellToProj(X, Y);
                //pthXYs.Add(xy2);
                LineString pathString = new LineString(pthXYs);
                IFeature pathLine = lineFS.AddFeature(pathString);
                int a = 0;
                foreach (string head in headers)
                {
                    pathLine.DataRow[head] = attributes[a];
                    a++;
                }
                lineFS.Name = name;
                lineFS.Extent = bklink.Extent;
                lineFS.Projection = mw.Projection;
                lineFS.SaveAs(saveLocation, true);
            }

            catch (System.OutOfMemoryException oome)
            {
                MessageBox.Show("Out of memory error. \n" + oome, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
