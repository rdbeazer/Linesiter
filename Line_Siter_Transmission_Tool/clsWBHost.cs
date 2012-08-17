using Interfaces;
using System;
using System.IO;
using System.Windows.Forms;

namespace LineSiterSitingTool
{
    internal class clsWBHost : IHost
    {
        private System.Windows.Forms.ToolStripStatusLabel status = new ToolStripStatusLabel();

        public string[] parameters
        {
            get;
            set;
        }

        public clsWBHost(ToolStripStatusLabel _status)
        {
            status = _status;
        }

        public void ShowFeedback(string strFeedback, string Caption = "GAT Message")
        {
            MessageBox.Show(strFeedback, Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ProgressBarLabel(string label)
        {
            //status.Text = label;
        }

        public void SetParameters(string[] ParameterArray)
        {
        }

        public void RunPlugin(string PluginClassName)
        {
        }

        public Boolean RunInSynchronousMode
        {
            get;
            set;
        }

        public string ApplicationDirectory
        {
            get { return Path.GetDirectoryName(Application.ExecutablePath); }
        }

        public string RecentDirectory
        {
            get;
            set;
        }
    }
}