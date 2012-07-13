namespace LineSiterSitingTool
{
    partial class frmCreateUtilityRaster
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.rtbHelpBox = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblprogress = new System.Windows.Forms.Label();
            this.ProgressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnOpenBounds = new System.Windows.Forms.Button();
            this.txtOpenBoundsRaster = new System.Windows.Forms.TextBox();
            this.lblCellSize = new System.Windows.Forms.Label();
            this.txtCellSize = new System.Windows.Forms.NumericUpDown();
            this.lblSelectBoundFile = new System.Windows.Forms.Label();
            this.lblSaveLocation = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtSaveLocation = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnBegin = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCellSize)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rtbHelpBox);
            this.panel2.Location = new System.Drawing.Point(346, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(203, 304);
            this.panel2.TabIndex = 5;
            // 
            // rtbHelpBox
            // 
            this.rtbHelpBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbHelpBox.Location = new System.Drawing.Point(0, 0);
            this.rtbHelpBox.Name = "rtbHelpBox";
            this.rtbHelpBox.Size = new System.Drawing.Size(203, 304);
            this.rtbHelpBox.TabIndex = 0;
            this.rtbHelpBox.Text = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblprogress);
            this.panel1.Controls.Add(this.ProgressBar1);
            this.panel1.Controls.Add(this.btnOpenBounds);
            this.panel1.Controls.Add(this.txtOpenBoundsRaster);
            this.panel1.Controls.Add(this.lblCellSize);
            this.panel1.Controls.Add(this.txtCellSize);
            this.panel1.Controls.Add(this.lblSelectBoundFile);
            this.panel1.Controls.Add(this.lblSaveLocation);
            this.panel1.Controls.Add(this.btnBrowse);
            this.panel1.Controls.Add(this.txtSaveLocation);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnBegin);
            this.panel1.Location = new System.Drawing.Point(13, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(319, 304);
            this.panel1.TabIndex = 4;
            // 
            // lblprogress
            // 
            this.lblprogress.AutoSize = true;
            this.lblprogress.Location = new System.Drawing.Point(24, 212);
            this.lblprogress.Name = "lblprogress";
            this.lblprogress.Size = new System.Drawing.Size(0, 13);
            this.lblprogress.TabIndex = 42;
            // 
            // ProgressBar1
            // 
            this.ProgressBar1.Location = new System.Drawing.Point(27, 228);
            this.ProgressBar1.Name = "ProgressBar1";
            this.ProgressBar1.Size = new System.Drawing.Size(224, 23);
            this.ProgressBar1.TabIndex = 41;
            // 
            // btnOpenBounds
            // 
            this.btnOpenBounds.Image = global::LineSiterSitingTool.Properties.Resources.document_open_2__1_;
            this.btnOpenBounds.Location = new System.Drawing.Point(257, 35);
            this.btnOpenBounds.Name = "btnOpenBounds";
            this.btnOpenBounds.Size = new System.Drawing.Size(25, 23);
            this.btnOpenBounds.TabIndex = 40;
            this.btnOpenBounds.UseVisualStyleBackColor = true;
            this.btnOpenBounds.Click += new System.EventHandler(this.btnOpenBounds_Click);
            // 
            // txtOpenBoundsRaster
            // 
            this.txtOpenBoundsRaster.Location = new System.Drawing.Point(27, 37);
            this.txtOpenBoundsRaster.Name = "txtOpenBoundsRaster";
            this.txtOpenBoundsRaster.Size = new System.Drawing.Size(224, 20);
            this.txtOpenBoundsRaster.TabIndex = 39;
            this.txtOpenBoundsRaster.Text = "Open a Raster File for Project Bounds";
            // 
            // lblCellSize
            // 
            this.lblCellSize.AutoSize = true;
            this.lblCellSize.Location = new System.Drawing.Point(24, 86);
            this.lblCellSize.Name = "lblCellSize";
            this.lblCellSize.Size = new System.Drawing.Size(67, 13);
            this.lblCellSize.TabIndex = 38;
            this.lblCellSize.Text = "Cell Size (m):";
            // 
            // txtCellSize
            // 
            this.txtCellSize.Location = new System.Drawing.Point(27, 102);
            this.txtCellSize.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.txtCellSize.Name = "txtCellSize";
            this.txtCellSize.Size = new System.Drawing.Size(69, 20);
            this.txtCellSize.TabIndex = 37;
            this.txtCellSize.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // lblSelectBoundFile
            // 
            this.lblSelectBoundFile.AutoSize = true;
            this.lblSelectBoundFile.Location = new System.Drawing.Point(24, 20);
            this.lblSelectBoundFile.Name = "lblSelectBoundFile";
            this.lblSelectBoundFile.Size = new System.Drawing.Size(122, 13);
            this.lblSelectBoundFile.TabIndex = 35;
            this.lblSelectBoundFile.Text = "Bounding Raster Extent:";
            // 
            // lblSaveLocation
            // 
            this.lblSaveLocation.AutoSize = true;
            this.lblSaveLocation.Location = new System.Drawing.Point(24, 163);
            this.lblSaveLocation.Name = "lblSaveLocation";
            this.lblSaveLocation.Size = new System.Drawing.Size(79, 13);
            this.lblSaveLocation.TabIndex = 34;
            this.lblSaveLocation.Text = "Save Location:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Image = global::LineSiterSitingTool.Properties.Resources.document_open_2__1_;
            this.btnBrowse.Location = new System.Drawing.Point(257, 179);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(25, 23);
            this.btnBrowse.TabIndex = 33;
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtSaveLocation
            // 
            this.txtSaveLocation.Location = new System.Drawing.Point(27, 182);
            this.txtSaveLocation.Name = "txtSaveLocation";
            this.txtSaveLocation.Size = new System.Drawing.Size(224, 20);
            this.txtSaveLocation.TabIndex = 32;
            this.txtSaveLocation.Text = "Please Select a Location and Filename";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(144, 269);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 26;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnBegin
            // 
            this.btnBegin.Location = new System.Drawing.Point(225, 269);
            this.btnBegin.Name = "btnBegin";
            this.btnBegin.Size = new System.Drawing.Size(75, 23);
            this.btnBegin.TabIndex = 25;
            this.btnBegin.Text = "Begin";
            this.btnBegin.UseVisualStyleBackColor = true;
            this.btnBegin.Click += new System.EventHandler(this.btnBegin_Click);
            // 
            // frmCreateUtilityRaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(561, 334);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmCreateUtilityRaster";
            this.Text = "Create Utility Raster";
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCellSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox rtbHelpBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblCellSize;
        private System.Windows.Forms.NumericUpDown txtCellSize;
        private System.Windows.Forms.Label lblSelectBoundFile;
        private System.Windows.Forms.Label lblSaveLocation;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtSaveLocation;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnBegin;
        private System.Windows.Forms.Button btnOpenBounds;
        private System.Windows.Forms.TextBox txtOpenBoundsRaster;
        private System.Windows.Forms.ProgressBar ProgressBar1;
        private System.Windows.Forms.Label lblprogress;

    }
}