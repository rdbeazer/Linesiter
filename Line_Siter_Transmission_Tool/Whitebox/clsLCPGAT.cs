using LineSiterSitingTool.Whitebox;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace LineSiterSitingTool.Whitebox
{
    internal class clsLCPGAT
    {
        private ToolStripStatusLabel _tslStatus = new ToolStripStatusLabel();
        private string[] _paraString;
        private string[] _costPathString;

        public clsLCPGAT(ToolStripStatusLabel tslStatus, string[] paraString, string[] costPathString)
        {
            _tslStatus = tslStatus;
            _paraString = paraString;
            _costPathString = costPathString;
        }

        public void leastCostPath(BackgroundWorker worker)
        {
            try
            {
                clsWBHost wbHost = new clsWBHost(_tslStatus);
                worker.ReportProgress(60, "Whitebox: creating cost accumulation raster");
                GISTools.CostAccumulation ca = new GISTools.CostAccumulation();
                worker.ReportProgress(70, "Whitebox: creating cost pathway");
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