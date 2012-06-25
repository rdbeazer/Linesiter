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
    class clsprepgatraster
    {
        IMap _mapLayer = null;
         double cellSize = 0;
        
        int rasterRow = 0;
        int rasterCol = 0;
        IRaster utilityCosts = new Raster();
        IRaster bounds = new Raster();
        IRaster startPoint = new Raster();
        string startFileName = "";
        IRaster endPoint = new Raster();
        string endFileName = "";
        string backlinkFilename = "";
        GATGrid backlinkGATRaster = new GATGrid();
        string outputAccumFilename = "";
       
        GATGrid outAccumGATRaster = new GATGrid();
        string outputPathFilename = "";
        
        GATGrid outPathGATRaster = new GATGrid();
                
             
        
        List<string> finalStatOutput = new List<string>();
        string _surveyPath = string.Empty;
        string lcpaShapeName = string.Empty;
          
         public void prepareGATRasters(string savePath,BackgroundWorker worker, Cursor xcurs, IRaster backlink,IRaster outAccumRaster,ref IRaster outPathRaster, ref string outputPathFilename)
        {
             
            BackgroundWorker pWorker = worker;
            backlinkFilename = savePath + @"\backlink";
            clsGATGridConversions prepareGATs = new clsGATGridConversions();
            prepareGATs._rasterToConvert = backlink;
            prepareGATs._statusMessage = "Preparing GAT Rasters, Please Wait";
            xcurs = Cursors.WaitCursor;
            prepareGATs.convertToGAT();
            outputAccumFilename = savePath + @"\outputAccumRaster";
            prepareGATs._rasterToConvert = outAccumRaster;
            prepareGATs.convertToGAT();
            outputPathFilename = savePath + @"\outputPathRaster";
            prepareGATs._rasterToConvert = outPathRaster;
            outPathRaster.Filename = outputPathFilename;
            outPathRaster.Save();
            prepareGATs.convertToGAT();
            xcurs= Cursors.Default;
        }
    }
}
