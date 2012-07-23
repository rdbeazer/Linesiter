namespace LineSiterSitingTool
{
    partial class frmToolExecute
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tslProjectSaveLocation = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tspProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.tslPass = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblProgress = new System.Windows.Forms.Label();
            this.picStartEnd = new System.Windows.Forms.PictureBox();
            this.cboStartEndPoints = new System.Windows.Forms.ComboBox();
            this.progressbar1 = new System.Windows.Forms.ProgressBar();
            this.pictRej2 = new System.Windows.Forms.PictureBox();
            this.picAcpt2 = new System.Windows.Forms.PictureBox();
            this.cboSelectUtilityRaster = new System.Windows.Forms.ComboBox();
            this.txtSaveLocation = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAlterWeights = new System.Windows.Forms.Button();
            this.btnAbort = new System.Windows.Forms.Button();
            this.btnBegin = new System.Windows.Forms.Button();
            this.lblNumPasses = new System.Windows.Forms.Label();
            this.numPasses = new System.Windows.Forms.NumericUpDown();
            this.dgvSelectLayers = new System.Windows.Forms.DataGridView();
            this.btnConvert = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picStartEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictRej2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAcpt2)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPasses)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectLayers)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 516);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(933, 28);
            this.panel1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslProjectSaveLocation,
            this.toolStripStatusLabel2,
            this.tslStatus,
            this.tspProgress,
            this.tslPass});
            this.statusStrip1.Location = new System.Drawing.Point(0, 6);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(933, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tslProjectSaveLocation
            // 
            this.tslProjectSaveLocation.Name = "tslProjectSaveLocation";
            this.tslProjectSaveLocation.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(777, 17);
            this.toolStripStatusLabel2.Spring = true;
            // 
            // tslStatus
            // 
            this.tslStatus.Name = "tslStatus";
            this.tslStatus.Size = new System.Drawing.Size(39, 17);
            this.tslStatus.Text = "Ready";
            // 
            // tspProgress
            // 
            this.tspProgress.Name = "tspProgress";
            this.tspProgress.Size = new System.Drawing.Size(100, 16);
            // 
            // tslPass
            // 
            this.tslPass.Name = "tslPass";
            this.tslPass.Size = new System.Drawing.Size(0, 17);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(933, 146);
            this.panel2.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.groupBox2);
            this.panel5.Controls.Add(this.groupBox1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(933, 146);
            this.panel5.TabIndex = 8;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblProgress);
            this.groupBox2.Controls.Add(this.picStartEnd);
            this.groupBox2.Controls.Add(this.cboStartEndPoints);
            this.groupBox2.Controls.Add(this.progressbar1);
            this.groupBox2.Controls.Add(this.pictRej2);
            this.groupBox2.Controls.Add(this.picAcpt2);
            this.groupBox2.Controls.Add(this.cboSelectUtilityRaster);
            this.groupBox2.Controls.Add(this.txtSaveLocation);
            this.groupBox2.Controls.Add(this.btnBrowse);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(682, 116);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Shapefiles/Rasters";
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(285, 61);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(0, 13);
            this.lblProgress.TabIndex = 10;
            // 
            // picStartEnd
            // 
            this.picStartEnd.Image = global::LineSiterSitingTool.Properties.Resources.dialog_accept;
            this.picStartEnd.Location = new System.Drawing.Point(241, 83);
            this.picStartEnd.Name = "picStartEnd";
            this.picStartEnd.Size = new System.Drawing.Size(28, 21);
            this.picStartEnd.TabIndex = 31;
            this.picStartEnd.TabStop = false;
            this.picStartEnd.Visible = false;
            // 
            // cboStartEndPoints
            // 
            this.cboStartEndPoints.Enabled = false;
            this.cboStartEndPoints.FormattingEnabled = true;
            this.cboStartEndPoints.Location = new System.Drawing.Point(9, 85);
            this.cboStartEndPoints.Name = "cboStartEndPoints";
            this.cboStartEndPoints.Size = new System.Drawing.Size(226, 21);
            this.cboStartEndPoints.TabIndex = 30;
            this.cboStartEndPoints.Text = "Select Shapefile for Start and End Points";
            this.cboStartEndPoints.SelectedIndexChanged += new System.EventHandler(this.cboStartEndPoints_SelectedIndexChanged);
            // 
            // progressbar1
            // 
            this.progressbar1.Location = new System.Drawing.Point(284, 83);
            this.progressbar1.MarqueeAnimationSpeed = 10;
            this.progressbar1.Name = "progressbar1";
            this.progressbar1.Size = new System.Drawing.Size(392, 21);
            this.progressbar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressbar1.TabIndex = 1;
            // 
            // pictRej2
            // 
            this.pictRej2.Image = global::LineSiterSitingTool.Properties.Resources.dialog_cancel;
            this.pictRej2.Location = new System.Drawing.Point(241, 58);
            this.pictRej2.Name = "pictRej2";
            this.pictRej2.Size = new System.Drawing.Size(28, 21);
            this.pictRej2.TabIndex = 24;
            this.pictRej2.TabStop = false;
            this.pictRej2.Visible = false;
            // 
            // picAcpt2
            // 
            this.picAcpt2.Image = global::LineSiterSitingTool.Properties.Resources.dialog_accept;
            this.picAcpt2.Location = new System.Drawing.Point(241, 58);
            this.picAcpt2.Name = "picAcpt2";
            this.picAcpt2.Size = new System.Drawing.Size(28, 21);
            this.picAcpt2.TabIndex = 23;
            this.picAcpt2.TabStop = false;
            this.picAcpt2.Visible = false;
            // 
            // cboSelectUtilityRaster
            // 
            this.cboSelectUtilityRaster.FormattingEnabled = true;
            this.cboSelectUtilityRaster.Location = new System.Drawing.Point(9, 58);
            this.cboSelectUtilityRaster.Name = "cboSelectUtilityRaster";
            this.cboSelectUtilityRaster.Size = new System.Drawing.Size(226, 21);
            this.cboSelectUtilityRaster.TabIndex = 20;
            this.cboSelectUtilityRaster.Text = "Select a Utility Cost Raster";
            this.cboSelectUtilityRaster.SelectedIndexChanged += new System.EventHandler(this.cboSelectUtilityRaster_SelectedIndexChanged);
            // 
            // txtSaveLocation
            // 
            this.txtSaveLocation.Enabled = false;
            this.txtSaveLocation.Location = new System.Drawing.Point(6, 32);
            this.txtSaveLocation.Name = "txtSaveLocation";
            this.txtSaveLocation.Size = new System.Drawing.Size(575, 20);
            this.txtSaveLocation.TabIndex = 19;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Image = global::LineSiterSitingTool.Properties.Resources.document_open_2__1_;
            this.btnBrowse.Location = new System.Drawing.Point(587, 29);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(31, 23);
            this.btnBrowse.TabIndex = 18;
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Select a Location to Save Analysis Files";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnConvert);
            this.groupBox1.Controls.Add(this.btnAlterWeights);
            this.groupBox1.Controls.Add(this.btnAbort);
            this.groupBox1.Controls.Add(this.btnBegin);
            this.groupBox1.Controls.Add(this.lblNumPasses);
            this.groupBox1.Controls.Add(this.numPasses);
            this.groupBox1.Location = new System.Drawing.Point(700, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(221, 116);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Monte Carlo Simulation";
            // 
            // btnAlterWeights
            // 
            this.btnAlterWeights.Location = new System.Drawing.Point(120, 18);
            this.btnAlterWeights.Name = "btnAlterWeights";
            this.btnAlterWeights.Size = new System.Drawing.Size(95, 23);
            this.btnAlterWeights.TabIndex = 9;
            this.btnAlterWeights.Text = "Assign Weights";
            this.btnAlterWeights.UseVisualStyleBackColor = true;
            this.btnAlterWeights.Click += new System.EventHandler(this.btnAlterWeights_Click);
            // 
            // btnAbort
            // 
            this.btnAbort.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAbort.Location = new System.Drawing.Point(121, 74);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(95, 23);
            this.btnAbort.TabIndex = 4;
            this.btnAbort.Text = "Cancel";
            this.btnAbort.UseVisualStyleBackColor = true;
            // 
            // btnBegin
            // 
            this.btnBegin.Enabled = false;
            this.btnBegin.Location = new System.Drawing.Point(120, 45);
            this.btnBegin.Name = "btnBegin";
            this.btnBegin.Size = new System.Drawing.Size(95, 23);
            this.btnBegin.TabIndex = 5;
            this.btnBegin.Text = "Begin";
            this.btnBegin.UseVisualStyleBackColor = true;
            this.btnBegin.Click += new System.EventHandler(this.btnBegin_Click);
            // 
            // lblNumPasses
            // 
            this.lblNumPasses.AutoSize = true;
            this.lblNumPasses.Location = new System.Drawing.Point(7, 21);
            this.lblNumPasses.Name = "lblNumPasses";
            this.lblNumPasses.Size = new System.Drawing.Size(93, 13);
            this.lblNumPasses.TabIndex = 7;
            this.lblNumPasses.Text = "Number of Passes";
            // 
            // numPasses
            // 
            this.numPasses.Location = new System.Drawing.Point(11, 37);
            this.numPasses.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numPasses.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPasses.Name = "numPasses";
            this.numPasses.Size = new System.Drawing.Size(80, 20);
            this.numPasses.TabIndex = 8;
            this.numPasses.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // dgvSelectLayers
            // 
            this.dgvSelectLayers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvSelectLayers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSelectLayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSelectLayers.Location = new System.Drawing.Point(0, 146);
            this.dgvSelectLayers.Name = "dgvSelectLayers";
            this.dgvSelectLayers.Size = new System.Drawing.Size(933, 370);
            this.dgvSelectLayers.TabIndex = 2;
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(1, 73);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(75, 23);
            this.btnConvert.TabIndex = 10;
            this.btnConvert.Text = "1121165";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // frmToolExecute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnAbort;
            this.ClientSize = new System.Drawing.Size(933, 544);
            this.Controls.Add(this.dgvSelectLayers);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(352, 501);
            this.Name = "frmToolExecute";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picStartEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictRej2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAcpt2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPasses)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectLayers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tslProjectSaveLocation;
        private System.Windows.Forms.ToolStripProgressBar tspProgress;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel tslStatus;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pictRej2;
        private System.Windows.Forms.PictureBox picAcpt2;
        private System.Windows.Forms.ComboBox cboSelectUtilityRaster;
        private System.Windows.Forms.TextBox txtSaveLocation;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnAlterWeights;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.Button btnBegin;
        private System.Windows.Forms.Label lblNumPasses;
        private System.Windows.Forms.NumericUpDown numPasses;
        private System.Windows.Forms.ToolStripStatusLabel tslPass;
        private System.Windows.Forms.DataGridView dgvSelectLayers;
        private System.Windows.Forms.ComboBox cboStartEndPoints;
        private System.Windows.Forms.ProgressBar progressbar1;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.PictureBox picStartEnd;
        private System.Windows.Forms.Button btnConvert;
    }
}