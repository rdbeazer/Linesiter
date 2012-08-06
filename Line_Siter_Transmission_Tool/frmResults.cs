using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DotSpatial.Data;

namespace LineSiterSitingTool
{
    public partial class frmResults : Form
    {
        IRaster _additive;
        IRaster _UTCosts;
        FeatureSet _MTCosts;
        FeatureSet _StartEndPoints;
        //FeatureSet _UTPath;
        IRaster _UTPath;
        //FeatureSet _MTPath;
        int _mtPasses;
        List<string> _results = new List<string>();
        string _saveLocation;


        public frmResults(IRaster UTPath, IRaster additive, IRaster UTCosts, IFeatureSet MTCosts, IFeatureSet startEndPoints, int mtPasses, List<string> results, string saveLocation)
        {
            InitializeComponent();
            _additive = additive;
            _UTCosts = UTCosts;
            _MTCosts = (FeatureSet)MTCosts;
            _mtPasses = mtPasses;
            _UTPath = UTPath;
            _StartEndPoints = (FeatureSet)startEndPoints;
            _results = results;
            _saveLocation = saveLocation;
            addResults();
        }

        private void addResults()
        {
            mpMTCosts.ProjectionModeReproject = DotSpatial.Controls.ActionMode.Never;
            mpUTCosts.ProjectionModeReproject = DotSpatial.Controls.ActionMode.Never;
            mpRasterResults.ProjectionModeReproject = DotSpatial.Controls.ActionMode.Never;
            mpUTCosts.Layers.Add(_UTCosts);
            mpUTCosts.Layers.Add(_StartEndPoints);
            mpUTCosts.Layers.Add(_UTPath);
            mpUTCosts.ResetBuffer();
            for (int i = 1; i <= _mtPasses; i++)
            {
                //mpMTCosts.Layers.Add(_MTCosts);
                IFeatureSet mcLCPA = FeatureSet.OpenFile(_saveLocation + @"\linesiter\LSProcessing\Pass_" + Convert.ToString(i) + @"\MCLCPA.shp");
                mpMTCosts.Layers.Add(mcLCPA);
                mpMTCosts.Layers.Add(_StartEndPoints);
                mpMTCosts.ResetBuffer();
            }
            mpRasterResults.Layers.Add(_additive);
            //---Add green to red symbolization here....
            mpRasterResults.Layers.Add(_StartEndPoints);
            mpRasterResults.ResetBuffer();
            foreach (string item in _results)
            {
                lbxStats.Items.Add(item);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            System.IO.StreamWriter extOut = new StreamWriter(_saveLocation + @"\exportOut.txt");
            string lstLine = null;
            for (int lstItems = 0; lstItems < lbxStats.Items.Count; lstItems++)
            {
                lstLine += lbxStats.Items[lstItems].ToString() + ";";
            }
            extOut.Write(lstLine);
            extOut.Flush();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
