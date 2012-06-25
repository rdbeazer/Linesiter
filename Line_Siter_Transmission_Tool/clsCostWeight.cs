using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using DotSpatial.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;
using System.IO;
using DotSpatial.Symbology;
using DotSpatial.Controls;
using DotSpatial.Topology;
using DotSpatial.Projections;
using System.Reflection;
using GeospatialFiles;
using System.Threading;

namespace nx09SitingTool
{
    class clsCostWeight
    {
        //IMap _mapLayer = null;
        //int currentPass = 1;
        clsMonteCarlo MC = new clsMonteCarlo();
        clsRasterOps pa;
        clsLCPCoords lc = new clsLCPCoords();
        FeatureSet projectFS = new FeatureSet();
        double[] aw = new double[5];
        string[] awTitles = new string[5] { "LSHigh", "LSMedHigh", "LSMedium", "LSMedLow", "LSLow" };
        double cellSize = 0;
        //string saveLocation = null;
        int rasterRow = 0;
        int rasterCol = 0;
       //IRaster utilityCosts = new Raster();
        //IRaster bounds = new Raster();
        IRaster startPoint = new Raster();
        string startFileName = "";
        IRaster endPoint = new Raster();
        string endFileName = "";
        string backlinkFilename = "";
        IRaster backlink = new Raster();
        GATGrid backlinkGATRaster = new GATGrid();
        string outputAccumFilename = "";
        IRaster outAccumRaster = new Raster();
        GATGrid outAccumGATRaster = new GATGrid();
        string outputPathFilename = "";
        IRaster outPathRaster = new Raster();
        GATGrid outPathGATRaster = new GATGrid();
        string[] paraString;
        IRaster rasterToConvert;
        string costFileName = "";
        string currentQuesPath = "";
        IRaster outPath;
        FeatureSet pathLines = new FeatureSet(FeatureType.Line);
        FeatureSet utPathLine = new FeatureSet(FeatureType.Line);
        IFeatureSet fst = new FeatureSet();
        string shapefileSavePath;
        string additveCostsFilePath;
        //IRaster additiveCosts;
        int timesHit2 = 1;
        List<string> finalStatOutput = new List<string>();
        string _surveyPath = string.Empty;
        string lcpaShapeName = string.Empty;
        IFeatureSet utilLCPA;
        List<IRaster> mcRasterList = new List<IRaster>();
        string progress = string.Empty;

        public void calculate_Cost_Weight(string savePath, IRaster bounds, IMap _mapLayer,int currentPass, IRaster additiveCosts, IRaster utilityCosts)
        {
            try
            {
                clsRasterOps mapAlgCW = new clsRasterOps(_mapLayer);
                IRaster addUt = additiveCosts;
                IRaster div1 = utilityCosts;
                IRaster div2 = Raster.Open(savePath + @"\mcraster.bgd");
                IRaster costWeightRaster = Raster.CreateRaster(savePath + @"\CostWeightRaster_" + currentPass + ".bgd", null, bounds.NumColumns, bounds.NumRows, 1, typeof(double), null);
                costFileName = savePath + @"\CostWeightRaster_" + currentPass + ".bgd";
                costWeightRaster.Bounds = bounds.Bounds;
                costWeightRaster.Projection = _mapLayer.Projection;
                costWeightRaster.Save();
                mapAlgCW.rasterDivision(div1, div2, costWeightRaster);
                rasterToConvert = costWeightRaster;
                List<IRaster> additive = new List<IRaster>();
                additive.Add(addUt);
                additive.Add(costWeightRaster);
                mapAlgCW.rasterAddition(additive, additiveCosts);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex + " has occured.", "Generic Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
