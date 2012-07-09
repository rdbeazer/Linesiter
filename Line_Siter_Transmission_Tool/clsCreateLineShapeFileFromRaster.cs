using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Data;
using DotSpatial.Controls;
using DotSpatial.Topology;
using System.Windows.Forms;

namespace nx09SitingTool
{
    class clsCreateLineShapeFileFromRaster
    {

        public clsCreateLineShapeFileFromRaster()
        {

        }

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
                lineFS.Save();
                //lineFS.SaveAs(saveLocation, true);
            }
            /*catch (Exception ex)
            {
                MessageBox.Show("Error :" + ex + " has occured.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/

            catch (System.ArgumentException ag)
            {
                MessageBox.Show("Shapefile input does not have the required number of points. \n Please check the input parameters. \n" + ag, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
