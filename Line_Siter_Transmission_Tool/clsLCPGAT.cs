using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using GeospatialFiles;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace LineSiterSitingTool
{
    class clsLCPGAT
    {
        BackgroundWorker _worker = new BackgroundWorker();
        ToolStripStatusLabel _tslStatus = new ToolStripStatusLabel();
        ToolStripProgressBar _tspProgress = new ToolStripProgressBar();
        string[] _paraString;
        string[] _costPathString;
                
        public clsLCPGAT(ToolStripStatusLabel tslStatus, BackgroundWorker worker, string[] paraString, string[] costPathString, ToolStripProgressBar tspProgress)
        {
            _worker = worker;
            _tslStatus = tslStatus;
            _paraString = paraString;
            _costPathString = costPathString;
            _tspProgress = tspProgress;
        }

        public void leastCostPath()
        {
            clsWBHost wbHost = new clsWBHost(_tslStatus);
            GISTools.CostAccumulation ca = new GISTools.CostAccumulation();
            GISTools.CostPathway cp = new GISTools.CostPathway();
            ca.Initialize(wbHost);
            ca.Execute(_paraString, _worker);
            cp.Initialize(wbHost);
            cp.Execute(_costPathString, _worker);
        }
    }
}
