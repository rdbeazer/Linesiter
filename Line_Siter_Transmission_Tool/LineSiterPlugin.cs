namespace nx09SitingTool
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DotSpatial.Controls;
    using DotSpatial.Controls.Header;
    using System.IO;
    using nx09SitingTool.Properties;
    using System.Windows.Forms;
    using DotSpatial.Topology;
    using DotSpatial.Data;
    using DotSpatial.Analysis;

    public class LineSiterPlugin : Extension
    {
        bool mouseUPBool = false;
        int seInt = 1;
        double startP = 0;
        double endP = 0;
        int endR = 0;
        int endC = 0;
        int startR = 0;
        int startC = 0;
        clsLCPCoords lc = new clsLCPCoords();
        Coordinate startXY = new Coordinate();
        Coordinate endXY = new Coordinate();
    
        public override void Activate()
        {
            App.HeaderControl.Add(new RootItem("kTools", "LineSiter"));
            App.HeaderControl.Add(new SimpleActionItem("Least Cost Path With Monte Carlo", LeastCostPathMonteCarloButtonClick) { RootKey = "kTools", SmallImage = Resources.MonteCarlo32a, LargeImage = Resources.MonteCarlo32a, GroupCaption = "Path Analysis" });
            App.HeaderControl.Add(new SimpleActionItem("Least Cost Path", LeastCostPathButtonClick) { RootKey = "kTools", SmallImage = Resources.LCPdollar, LargeImage = Resources.LCPdollar, GroupCaption = "Path Analysis" });
            App.HeaderControl.Add(new SimpleActionItem("Select Start and End Points", SelectPointsButtonClick) { RootKey = "kTools", SmallImage = Resources.format_add_node, LargeImage = Resources.format_add_node, GroupCaption = "Path Analysis" });
            App.HeaderControl.Add(new SimpleActionItem("Reclassify", ReclassifyRasterButtonClick) { RootKey = "kTools", SmallImage = Resources.convert_gray_to_color_icon, LargeImage = Resources.convert_gray_to_color_icon, GroupCaption = "Raster Tools" });
            App.HeaderControl.Add(new SimpleActionItem("Create Bounding Raster", CreateBoundingRasterClick) { RootKey = "kTools", SmallImage = Resources.edit_5, LargeImage = Resources.edit_5, GroupCaption = "Raster Tools" });
            App.HeaderControl.Add(new SimpleActionItem("Create Utility Raster", CreateUtilityRasterButtonClick) { RootKey = "kTools", SmallImage = Resources.money, LargeImage = Resources.money, GroupCaption = "Raster Tools" });

            base.Activate();
        }

        
        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }
        public void ReclassifyRasterButtonClick(object sender, EventArgs e)
        {
            frmReclass rcls = new frmReclass(App.Map);
            rcls.ShowDialog();
        }

        public void CreateUtilityRasterButtonClick(object sender, EventArgs e)
        {
            frmCreateUtilityRaster uc = new frmCreateUtilityRaster(App.Map);
            uc.ShowDialog();
        }

        public void CreateBoundingRasterClick(object sender, EventArgs e)
        {
            frmCreateBoundsRaster cbr = new frmCreateBoundsRaster(App.Map);
            cbr.ShowDialog();
        }

        public void SelectPointsButtonClick(object sender, EventArgs e)
        {
            mouseUPBool = true;
            var map = App.Map as DotSpatial.Controls.Map;
            map.MouseUp += new MouseEventHandler(MapMouseUp);
        }

        public void LeastCostPathMonteCarloButtonClick(object sender, EventArgs e)
        {
            if (!Directory.Exists(@"c:\temp\LineSiter"))
            {
                Directory.CreateDirectory(@"c:\temp\LineSiter");
            }
            frmToolExecute newExecute = new frmToolExecute(App.Map, App.ProgressHandler, lc);
            newExecute.ShowDialog();
        }

        public void LeastCostPathButtonClick(object se, EventArgs e)
        {
            frmTLCP newTLCP = new frmTLCP(App.Map, lc);
            newTLCP.ShowDialog();
        }


        public void MapMouseUp(object sender, MouseEventArgs e)
        {
            Coordinate xy = new Coordinate();
            int seRow = 0;
            int seCol = 0;
            //double seCellValue = 0;

            if (mouseUPBool == true)
            {

                if (App.Map.Layers.SelectedLayer as MapRasterLayer == null)
                {
                    MessageBox.Show("Please select a raster layer to select start and end points.", "Wrong Layer Selected", MessageBoxButtons.OK);
                    return;
                }

                IMapRasterLayer seLayer = (IMapRasterLayer)App.Map.Layers.SelectedLayer;

                if (seLayer == null)
                {
                    MessageBox.Show("Please select a layer to set start and end points.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    IRaster seRaster = seLayer.DataSet;
                    xy = App.Map.PixelToProj(e.Location);
                    RcIndex rasterXY = DotSpatial.Data.RasterExt.ProjToCell(seRaster, xy);
                    seRow = rasterXY.Row;
                    seCol = rasterXY.Column;
                    int numCols = seRaster.NumColumns;
                    int numRows = seRaster.NumRows;
                    if (seCol > 0 & seCol < seRaster.NumColumns & seRow > 0 & seRow < seRaster.NumRows)
                    {
                        if (seInt == 1)
                        {
                            startR = seRow;
                            startC = seCol;
                            startP = seRaster.Value[seRow, seCol];
                            startXY = xy;
                            // todo, update to use custom panels.
                            //App.ProgressHandler.Progress(" tsslStart", 0, "Start- Row: " + Convert.ToString(seRow) + " Column: " + Convert.ToString(seCol));
                            lc.startRow = rasterXY.Row;
                            lc.startCol = rasterXY.Column;
                            seInt++;
                        }
                        else if (seInt == 2)
                        {
                            endR = seRow;
                            endC = seCol;
                            endP = seRaster.Value[seRow, seCol];
                            endXY = xy;
                            // todo, update to use custom panels.
                            //App.ProgressHandler.Progress(" tsslEnd", 0, "End- Row: " + Convert.ToString(seRow) + " Column: " + Convert.ToString(seCol));
                            lc.EndRow = rasterXY.Row;
                            lc.EndCol = rasterXY.Column;
                            if (lc.startRow != null & lc.startCol != null & lc.EndRow != null & lc.EndCol != null & seInt == 2)
                            {
                                frmPointSave ps = new frmPointSave(lc.startRow, lc.startCol, lc.EndRow, lc.EndCol, App.Map, startXY, endXY);
                                ps.ShowDialog();
                                //MessageBox.Show("The beginning coordinates you chose are: " + Convert.ToString(lc.startRow) + ", " + Convert.ToString(lc.startCol) + " and the ending coordinates are: " + Convert.ToString(lc.EndRow) + ", " + Convert.ToString(lc.EndCol) + "\n\n  Coordinates saved.", "Raster Coordinates", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            seInt = 1;
                            mouseUPBool = false;
                        }
                        //seCellValue= seRaster.Value[seRow,seCol];
                    }
                    else
                    {
                        MessageBox.Show("Value is outside raster bounds.  Please select an area on the map.", "Error 120: Selection Out of Bounds", MessageBoxButtons.OK);
                    }
                }

                //MessageBox.Show("Point \r\n X: " + seRow +" Y: " + seCol+ " with a value of: " + seCellValue +" was clicked.", "Info", MessageBoxButtons.OK);
            }
        }

    }
}
