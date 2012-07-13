using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Symbology;
using System.Windows.Forms;

namespace LineSiterSitingTool
{
    public partial class selectionDiag : Form
    {
        IMap _mapLayer = null;
        System.Data.DataSet dsQuesSets = new System.Data.DataSet();

        public selectionDiag(IMap mapLayer)
        {
            InitializeComponent();
            foreach (ILayer items in mapLayer.Layers)
            {
                cboLayers.Items.Add(items.LegendText);
            }
            _mapLayer = mapLayer;

        }
        
        public class retLayer
        {
            public string layerName;
            public string layerAttribute;
            public string layerAttrValue;
        }

        public retLayer getRLayer
        {
            get
            {
                retLayer rlayer = new retLayer();
                rlayer.layerName = Convert.ToString(cboLayers.Text);
                rlayer.layerAttribute = Convert.ToString(cboAttribute.SelectedValue);
                rlayer.layerAttrValue = txtValue.Text;
                return rlayer;
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            //IFeatureSet lFeatureset = null;

            //_mapLayer.GetLayers(cboLayers.Text);

            //cboAttribute.Items.Add(ToString(_mapLayer.Layers.
        }
    }
}
