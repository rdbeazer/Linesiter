namespace nx09SitingTool
{
    partial class frmPointSave
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
            this.lblProgress = new System.Windows.Forms.Label();
            this.chkCreateShapefile = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtEndCoords = new System.Windows.Forms.TextBox();
            this.lblStartCoords = new System.Windows.Forms.Label();
            this.lblEndCoords = new System.Windows.Forms.Label();
            this.txtStartCoords = new System.Windows.Forms.TextBox();
            this.cboLayers = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSaveLocation = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtSaveLocation = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rtbHelpBox = new System.Windows.Forms.RichTextBox();
            this.txtBndingExt = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtBndingExt);
            this.panel1.Controls.Add(this.lblProgress);
            this.panel1.Controls.Add(this.chkCreateShapefile);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.cboLayers);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblSaveLocation);
            this.panel1.Controls.Add(this.btnBrowse);
            this.panel1.Controls.Add(this.txtSaveLocation);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(319, 342);
            this.panel1.TabIndex = 0;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(74, 271);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(0, 13);
            this.lblProgress.TabIndex = 38;
            // 
            // chkCreateShapefile
            // 
            this.chkCreateShapefile.AutoSize = true;
            this.chkCreateShapefile.Location = new System.Drawing.Point(27, 177);
            this.chkCreateShapefile.Name = "chkCreateShapefile";
            this.chkCreateShapefile.Size = new System.Drawing.Size(104, 17);
            this.chkCreateShapefile.TabIndex = 37;
            this.chkCreateShapefile.Text = "Create Shapefile";
            this.chkCreateShapefile.UseVisualStyleBackColor = true;
            this.chkCreateShapefile.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtEndCoords);
            this.groupBox1.Controls.Add(this.lblStartCoords);
            this.groupBox1.Controls.Add(this.lblEndCoords);
            this.groupBox1.Controls.Add(this.txtStartCoords);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(27, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(273, 97);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Source Cell Coordinates";
            // 
            // txtEndCoords
            // 
            this.txtEndCoords.Location = new System.Drawing.Point(167, 56);
            this.txtEndCoords.Name = "txtEndCoords";
            this.txtEndCoords.Size = new System.Drawing.Size(100, 26);
            this.txtEndCoords.TabIndex = 31;
            // 
            // lblStartCoords
            // 
            this.lblStartCoords.AutoSize = true;
            this.lblStartCoords.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartCoords.Location = new System.Drawing.Point(3, 34);
            this.lblStartCoords.Name = "lblStartCoords";
            this.lblStartCoords.Size = new System.Drawing.Size(102, 13);
            this.lblStartCoords.TabIndex = 28;
            this.lblStartCoords.Text = "Starting Coordinates";
            // 
            // lblEndCoords
            // 
            this.lblEndCoords.AutoSize = true;
            this.lblEndCoords.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEndCoords.Location = new System.Drawing.Point(164, 34);
            this.lblEndCoords.Name = "lblEndCoords";
            this.lblEndCoords.Size = new System.Drawing.Size(99, 13);
            this.lblEndCoords.TabIndex = 29;
            this.lblEndCoords.Text = "Ending Coordinates";
            // 
            // txtStartCoords
            // 
            this.txtStartCoords.Location = new System.Drawing.Point(6, 56);
            this.txtStartCoords.Name = "txtStartCoords";
            this.txtStartCoords.Size = new System.Drawing.Size(100, 26);
            this.txtStartCoords.TabIndex = 30;
            // 
            // cboLayers
            // 
            this.cboLayers.FormattingEnabled = true;
            this.cboLayers.Location = new System.Drawing.Point(27, 263);
            this.cboLayers.Name = "cboLayers";
            this.cboLayers.Size = new System.Drawing.Size(273, 21);
            this.cboLayers.TabIndex = 36;
            this.cboLayers.Text = "Select a Bounding Raster File";
            this.cboLayers.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 35;
            this.label1.Text = "Bounding Extent:";
            // 
            // lblSaveLocation
            // 
            this.lblSaveLocation.AutoSize = true;
            this.lblSaveLocation.Location = new System.Drawing.Point(24, 211);
            this.lblSaveLocation.Name = "lblSaveLocation";
            this.lblSaveLocation.Size = new System.Drawing.Size(79, 13);
            this.lblSaveLocation.TabIndex = 34;
            this.lblSaveLocation.Text = "Save Location:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Image = global::LineSiterSitingTool.Properties.Resources.document_open_2__1_;
            this.btnBrowse.Location = new System.Drawing.Point(268, 205);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(32, 23);
            this.btnBrowse.TabIndex = 33;
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtSaveLocation
            // 
            this.txtSaveLocation.Location = new System.Drawing.Point(27, 230);
            this.txtSaveLocation.Name = "txtSaveLocation";
            this.txtSaveLocation.Size = new System.Drawing.Size(273, 20);
            this.txtSaveLocation.TabIndex = 32;
            this.txtSaveLocation.Text = "Please Select a Location and Filename";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(144, 310);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 26;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(225, 310);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 25;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rtbHelpBox);
            this.panel2.Location = new System.Drawing.Point(345, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(203, 342);
            this.panel2.TabIndex = 1;
            // 
            // rtbHelpBox
            // 
            this.rtbHelpBox.Location = new System.Drawing.Point(0, 0);
            this.rtbHelpBox.Name = "rtbHelpBox";
            this.rtbHelpBox.Size = new System.Drawing.Size(203, 342);
            this.rtbHelpBox.TabIndex = 0;
            this.rtbHelpBox.Text = "";
            // 
            // txtBndingExt
            // 
            this.txtBndingExt.Enabled = false;
            this.txtBndingExt.Location = new System.Drawing.Point(27, 137);
            this.txtBndingExt.Name = "txtBndingExt";
            this.txtBndingExt.Size = new System.Drawing.Size(273, 20);
            this.txtBndingExt.TabIndex = 39;
            // 
            // frmPointSave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(560, 366);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmPointSave";
            this.Text = "Save Points to Shapefile";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cboLayers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSaveLocation;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtSaveLocation;
        private System.Windows.Forms.TextBox txtEndCoords;
        private System.Windows.Forms.TextBox txtStartCoords;
        private System.Windows.Forms.Label lblEndCoords;
        private System.Windows.Forms.Label lblStartCoords;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox rtbHelpBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkCreateShapefile;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.TextBox txtBndingExt;


    }
}