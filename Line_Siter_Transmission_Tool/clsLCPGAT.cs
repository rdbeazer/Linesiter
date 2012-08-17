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
using System.Diagnostics;

namespace LineSiterSitingTool
{
    class clsLCPGAT
    {
        BackgroundWorker _worker = new BackgroundWorker();
        ToolStripStatusLabel _tslStatus = new ToolStripStatusLabel();
        ToolStripProgressBar _tspProgress = new ToolStripProgressBar();
        ProgressBar _progres1 = new ProgressBar();
        string[] _paraString;
        string[] _costPathString;
                
        public clsLCPGAT(ToolStripStatusLabel tslStatus, string[] paraString, string[] costPathString)
        {
            //_worker = worker;
            _tslStatus = tslStatus;
            _paraString = paraString;
            _costPathString = costPathString;
            //_tspProgress = tspProgress;
            //_progres1 = progres1;
        }

        public void leastCostPath(BackgroundWorker worker)
        {
            try
            {
                clsWBHost wbHost = new clsWBHost(_tslStatus);
                GISTools.CostAccumulation ca = new GISTools.CostAccumulation();
                GISTools.CostPathway cp = new GISTools.CostPathway();
                ca.Initialize(wbHost);
                ca.Execute(_paraString, worker);
                cp.Initialize(wbHost);
                cp.Execute(_costPathString, worker);
            }

            catch (System.InvalidOperationException ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
