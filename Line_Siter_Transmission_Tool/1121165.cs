using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace LineSiterSitingTool
{
    public partial class _1121165 : Form
    {
        public _1121165()
        {
            InitializeComponent();
        }

        private void _1121165_Load(object sender, EventArgs e)
        {
            long x =0;
            do
            {
                progressBar1.Value++;
                x++;
                //Thread.Sleep(1000);
            }
            while (x < 1000000);
        }
    }
}
