using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Symbology;
using DotSpatial.Topology;

namespace LineSiterSitingTool
{
    class clsCreateStartEndRasters
    {
        IFeatureSet fst = new FeatureSet();
        public IRaster startPoint { get; set; }
        public IRaster endPoint { get; set; }

        public void createRasters(IMap MW, string selectedItem, clsLCPCoords LC, IRaster utilCosts)
        {
            startPoint.Bounds = utilCosts.Bounds;
            startPoint.Projection = MW.Projection;
            startPoint.Save();
            endPoint.Bounds = utilCosts.Bounds;
            endPoint.Projection = MW.Projection;
            endPoint.Save();

            foreach (Layer lay in MW.Layers)
            {
                if (lay.LegendText == selectedItem)
                {
                    if (lay.GetType() == typeof(DotSpatial.Controls.MapPointLayer))
                    {
                        int x = 0;
                        fst = (IFeatureSet)lay.DataSet;
                        foreach (Feature fcd in fst.Features)
                        {
                            if (x == 0)
                            {
                                Coordinate cd;
                                cd = new Coordinate((fcd.Coordinates[0]).X, (fcd.Coordinates[0]).Y);
                                RcIndex rSCood = utilCosts.Bounds.ProjToCell(cd);
                                LC.startCol = rSCood.Column;
                                LC.startRow = rSCood.Row;
                            }
                            if (x == 1)
                            {
                                Coordinate cd;
                                cd = new Coordinate((fcd.Coordinates[0]).X, (fcd.Coordinates[0]).Y);
                                RcIndex rECood = utilCosts.Bounds.ProjToCell(cd);
                                LC.EndCol = rECood.Column;
                                LC.EndRow = rECood.Row;
                            }
                            if (x > 1)
                            {
                                MessageBox.Show("This shapefile contains more than a starting and ending point and cannot be used. \n Please select a proper file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            x++;
                        }

                        for (int oRows = 0; oRows < utilCosts.NumRows - 1; oRows++)
                        {
                            for (int oCols = 0; oCols < utilCosts.NumColumns - 1; oCols++)
                            {
                                if (oRows == LC.startRow & oCols == LC.startCol)
                                {
                                    startPoint.Value[oRows, oCols] = 1234567890;
                                }
                                else if (oRows == LC.EndRow & oCols == LC.EndCol)
                                {
                                    endPoint.Value[oRows, oCols] = 0987654321;
                                }
                                else
                                {
                                    startPoint.Value[oRows, oCols] = 0;
                                    endPoint.Value[oRows, oCols] = 0;
                                }
                            }
                        }
                        startPoint.Save();
                        GATConversions(startPoint, utilCosts);
                        endPoint.Save();
                        GATConversions(endPoint, utilCosts);
                    }
                    else
                    {
                        MessageBox.Show("This application currently only accepts point shapefiles for starting and ending point locations.", "Shapefile Operations", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }
        }


        private void GATConversions(IRaster convertRas, IRaster utilityCosts)
        {
            try
            {
                if (convertRas.Bounds == utilityCosts.Bounds)
                {
                    clsGATGridConversions destinationRaster = new clsGATGridConversions();
                    destinationRaster._rasterToConvert = convertRas;
                    destinationRaster._statusMessage = "Converting starting and ending points rasters. ";
                    destinationRaster.convertToGAT();
                }
                else
                {
                    MessageBox.Show("End point raster bounds do not match the project bounds raster.  \nPlease select a destination point raster with bounds identical to the project bounds raster.", "Bounds Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(Convert.ToString(ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
