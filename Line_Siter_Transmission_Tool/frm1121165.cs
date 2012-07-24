using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using DotSpatial.Data;

namespace LineSiterSitingTool
{
    public partial class frm1121165 : Form
    {
        public frm1121165()
        {
            InitializeComponent();
        }

        //private void frm1121165_Load(object sender, EventArgs e)
        //{
        //    //long x = 0;
        //    //do
        //    //{
        //    //    progressBar1.Value++;
        //    //    x++;
        //    //    //Thread.Sleep(1000);
        //    //}
        //    //while (x < 1000000);
        //}

        clsGATGridConversions ggc = new clsGATGridConversions();

        private void btnBack_Click(object sender, EventArgs e)
        {
            openBrowse();
            saveBrowse();
            convertFromBGD();
        }

        private void convertFromBGD()
        {
            ggc.convertBGD();
        }

        private void saveBrowse()
        {
            SaveFileDialog svRas = new SaveFileDialog();
            svRas.Filter = "DotSpatial Raster (*.bgd) | *.bgd";
            svRas.ShowDialog();
            ggc._conversionRaster = svRas.FileName.Substring(0, svRas.FileName.Length - 4);
        }

        private void openBrowse()
        {
            OpenFileDialog opBRas = new OpenFileDialog();
            opBRas.Filter = "Whitebox Raster (*.tas) | *.tas";
            opBRas.ShowDialog();
            ggc._gridToConvert = opBRas.FileName.Substring(0, opBRas.FileName.Length - 4);
            ggc._bnds = Raster.OpenFile(opBRas.FileName.Substring(0, opBRas.FileName.Length - 4) + ".bgd");
        }
    }
}
