namespace LineSiterSitingTool
{
    partial class frmResults
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
            this.gbxUTCosts = new System.Windows.Forms.GroupBox();
            this.mpUTCosts = new DotSpatial.Controls.Map();
            this.label1 = new System.Windows.Forms.Label();
            this.gbxMTCosts = new System.Windows.Forms.GroupBox();
            this.mpMTCosts = new DotSpatial.Controls.Map();
            this.gbxStats = new System.Windows.Forms.GroupBox();
            this.lbxStats = new System.Windows.Forms.ListBox();
            this.gbxAdditive = new System.Windows.Forms.GroupBox();
            this.mpRasterResults = new DotSpatial.Controls.Map();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.gbxUTCosts.SuspendLayout();
            this.gbxMTCosts.SuspendLayout();
            this.gbxStats.SuspendLayout();
            this.gbxAdditive.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxUTCosts
            // 
            this.gbxUTCosts.Controls.Add(this.mpUTCosts);
            this.gbxUTCosts.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxUTCosts.Location = new System.Drawing.Point(12, 69);
            this.gbxUTCosts.Name = "gbxUTCosts";
            this.gbxUTCosts.Size = new System.Drawing.Size(368, 216);
            this.gbxUTCosts.TabIndex = 0;
            this.gbxUTCosts.TabStop = false;
            this.gbxUTCosts.Text = "LCPA Utility Costs Only";
            // 
            // mpUTCosts
            // 
            this.mpUTCosts.AllowDrop = true;
            this.mpUTCosts.BackColor = System.Drawing.Color.White;
            this.mpUTCosts.CollectAfterDraw = false;
            this.mpUTCosts.CollisionDetection = false;
            this.mpUTCosts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mpUTCosts.ExtendBuffer = false;
            this.mpUTCosts.FunctionMode = DotSpatial.Controls.FunctionMode.None;
            this.mpUTCosts.IsBusy = false;
            this.mpUTCosts.Legend = null;
            this.mpUTCosts.Location = new System.Drawing.Point(3, 22);
            this.mpUTCosts.Name = "mpUTCosts";
            this.mpUTCosts.ProgressHandler = null;
            this.mpUTCosts.ProjectionModeDefine = DotSpatial.Controls.ActionMode.Never;
            this.mpUTCosts.ProjectionModeReproject = DotSpatial.Controls.ActionMode.Never;
            this.mpUTCosts.RedrawLayersWhileResizing = false;
            this.mpUTCosts.SelectionEnabled = true;
            this.mpUTCosts.Size = new System.Drawing.Size(362, 191);
            this.mpUTCosts.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(181, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Summary of Results:";
            // 
            // gbxMTCosts
            // 
            this.gbxMTCosts.Controls.Add(this.mpMTCosts);
            this.gbxMTCosts.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxMTCosts.Location = new System.Drawing.Point(386, 69);
            this.gbxMTCosts.Name = "gbxMTCosts";
            this.gbxMTCosts.Size = new System.Drawing.Size(368, 216);
            this.gbxMTCosts.TabIndex = 2;
            this.gbxMTCosts.TabStop = false;
            this.gbxMTCosts.Text = "LCPA Monte Carlo Results";
            // 
            // mpMTCosts
            // 
            this.mpMTCosts.AllowDrop = true;
            this.mpMTCosts.BackColor = System.Drawing.Color.White;
            this.mpMTCosts.CollectAfterDraw = false;
            this.mpMTCosts.CollisionDetection = false;
            this.mpMTCosts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mpMTCosts.ExtendBuffer = false;
            this.mpMTCosts.FunctionMode = DotSpatial.Controls.FunctionMode.None;
            this.mpMTCosts.IsBusy = false;
            this.mpMTCosts.Legend = null;
            this.mpMTCosts.Location = new System.Drawing.Point(3, 22);
            this.mpMTCosts.Name = "mpMTCosts";
            this.mpMTCosts.ProgressHandler = null;
            this.mpMTCosts.ProjectionModeDefine = DotSpatial.Controls.ActionMode.Never;
            this.mpMTCosts.ProjectionModeReproject = DotSpatial.Controls.ActionMode.Never;
            this.mpMTCosts.RedrawLayersWhileResizing = false;
            this.mpMTCosts.SelectionEnabled = true;
            this.mpMTCosts.Size = new System.Drawing.Size(362, 191);
            this.mpMTCosts.TabIndex = 0;
            // 
            // gbxStats
            // 
            this.gbxStats.Controls.Add(this.lbxStats);
            this.gbxStats.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxStats.Location = new System.Drawing.Point(12, 291);
            this.gbxStats.Name = "gbxStats";
            this.gbxStats.Size = new System.Drawing.Size(368, 216);
            this.gbxStats.TabIndex = 3;
            this.gbxStats.TabStop = false;
            this.gbxStats.Text = "Statistics";
            // 
            // lbxStats
            // 
            this.lbxStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxStats.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbxStats.FormattingEnabled = true;
            this.lbxStats.ItemHeight = 16;
            this.lbxStats.Location = new System.Drawing.Point(3, 22);
            this.lbxStats.Name = "lbxStats";
            this.lbxStats.Size = new System.Drawing.Size(362, 191);
            this.lbxStats.TabIndex = 0;
            // 
            // gbxAdditive
            // 
            this.gbxAdditive.Controls.Add(this.mpRasterResults);
            this.gbxAdditive.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxAdditive.Location = new System.Drawing.Point(386, 291);
            this.gbxAdditive.Name = "gbxAdditive";
            this.gbxAdditive.Size = new System.Drawing.Size(368, 216);
            this.gbxAdditive.TabIndex = 4;
            this.gbxAdditive.TabStop = false;
            this.gbxAdditive.Text = "Additive Raster Results";
            // 
            // mpRasterResults
            // 
            this.mpRasterResults.AllowDrop = true;
            this.mpRasterResults.BackColor = System.Drawing.Color.White;
            this.mpRasterResults.CollectAfterDraw = false;
            this.mpRasterResults.CollisionDetection = false;
            this.mpRasterResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mpRasterResults.ExtendBuffer = false;
            this.mpRasterResults.FunctionMode = DotSpatial.Controls.FunctionMode.None;
            this.mpRasterResults.IsBusy = false;
            this.mpRasterResults.Legend = null;
            this.mpRasterResults.Location = new System.Drawing.Point(3, 22);
            this.mpRasterResults.Name = "mpRasterResults";
            this.mpRasterResults.ProgressHandler = null;
            this.mpRasterResults.ProjectionModeDefine = DotSpatial.Controls.ActionMode.Never;
            this.mpRasterResults.ProjectionModeReproject = DotSpatial.Controls.ActionMode.Never;
            this.mpRasterResults.RedrawLayersWhileResizing = false;
            this.mpRasterResults.SelectionEnabled = true;
            this.mpRasterResults.Size = new System.Drawing.Size(362, 191);
            this.mpRasterResults.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(676, 521);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(521, 521);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(149, 23);
            this.btnExport.TabIndex = 6;
            this.btnExport.Text = "Export Statistics To Text File";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // frmResults
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 554);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbxAdditive);
            this.Controls.Add(this.gbxStats);
            this.Controls.Add(this.gbxMTCosts);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gbxUTCosts);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmResults";
            this.Text = "Process Complete";
            this.gbxUTCosts.ResumeLayout(false);
            this.gbxMTCosts.ResumeLayout(false);
            this.gbxStats.ResumeLayout(false);
            this.gbxAdditive.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxUTCosts;
        private DotSpatial.Controls.Map mpUTCosts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbxMTCosts;
        private DotSpatial.Controls.Map mpMTCosts;
        private System.Windows.Forms.GroupBox gbxStats;
        private System.Windows.Forms.GroupBox gbxAdditive;
        private DotSpatial.Controls.Map mpRasterResults;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ListBox lbxStats;
        private System.Windows.Forms.Button btnExport;
    }
}