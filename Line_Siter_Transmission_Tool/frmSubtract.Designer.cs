namespace nx09SitingTool
{
    partial class frmSubtract
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSubtract));
            this.cboS1 = new System.Windows.Forms.ComboBox();
            this.cboS2 = new System.Windows.Forms.ComboBox();
            this.btnSubtract = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.txtSaveLocation = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblS1 = new System.Windows.Forms.Label();
            this.lblS2 = new System.Windows.Forms.Label();
            this.lblSaveLoc = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cboS1
            // 
            this.cboS1.FormattingEnabled = true;
            this.cboS1.Location = new System.Drawing.Point(12, 32);
            this.cboS1.Name = "cboS1";
            this.cboS1.Size = new System.Drawing.Size(287, 21);
            this.cboS1.TabIndex = 0;
            this.cboS1.SelectedIndexChanged += new System.EventHandler(this.cboS1_SelectedIndexChanged);
            // 
            // cboS2
            // 
            this.cboS2.FormattingEnabled = true;
            this.cboS2.Location = new System.Drawing.Point(12, 78);
            this.cboS2.Name = "cboS2";
            this.cboS2.Size = new System.Drawing.Size(287, 21);
            this.cboS2.TabIndex = 1;
            this.cboS2.SelectedIndexChanged += new System.EventHandler(this.cboS2_SelectedIndexChanged);
            // 
            // btnSubtract
            // 
            this.btnSubtract.Image = ((System.Drawing.Image)(resources.GetObject("btnSubtract.Image")));
            this.btnSubtract.Location = new System.Drawing.Point(329, 12);
            this.btnSubtract.Name = "btnSubtract";
            this.btnSubtract.Size = new System.Drawing.Size(52, 41);
            this.btnSubtract.TabIndex = 2;
            this.btnSubtract.UseVisualStyleBackColor = true;
            this.btnSubtract.Click += new System.EventHandler(this.btnSubtract_Click);
            // 
            // btnExit
            // 
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.Location = new System.Drawing.Point(329, 59);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(52, 40);
            this.btnExit.TabIndex = 3;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // txtSaveLocation
            // 
            this.txtSaveLocation.Location = new System.Drawing.Point(12, 138);
            this.txtSaveLocation.Name = "txtSaveLocation";
            this.txtSaveLocation.Size = new System.Drawing.Size(249, 20);
            this.txtSaveLocation.TabIndex = 4;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Image = global::nx09SitingTool.Properties.Resources.document_open_2__1_;
            this.btnBrowse.Location = new System.Drawing.Point(267, 135);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(32, 23);
            this.btnBrowse.TabIndex = 34;
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lblS1
            // 
            this.lblS1.AutoSize = true;
            this.lblS1.Location = new System.Drawing.Point(12, 13);
            this.lblS1.Name = "lblS1";
            this.lblS1.Size = new System.Drawing.Size(214, 13);
            this.lblS1.TabIndex = 35;
            this.lblS1.Text = "Select First Raster for Subtraction Operation";
            // 
            // lblS2
            // 
            this.lblS2.AutoSize = true;
            this.lblS2.Location = new System.Drawing.Point(12, 62);
            this.lblS2.Name = "lblS2";
            this.lblS2.Size = new System.Drawing.Size(232, 13);
            this.lblS2.TabIndex = 36;
            this.lblS2.Text = "Select Second Raster for Subtraction Operation";
            // 
            // lblSaveLoc
            // 
            this.lblSaveLoc.AutoSize = true;
            this.lblSaveLoc.Location = new System.Drawing.Point(12, 122);
            this.lblSaveLoc.Name = "lblSaveLoc";
            this.lblSaveLoc.Size = new System.Drawing.Size(164, 13);
            this.lblSaveLoc.TabIndex = 37;
            this.lblSaveLoc.Text = "Select Location for Saved Raster";
            // 
            // frmSubtract
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 197);
            this.Controls.Add(this.lblSaveLoc);
            this.Controls.Add(this.lblS2);
            this.Controls.Add(this.lblS1);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtSaveLocation);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSubtract);
            this.Controls.Add(this.cboS2);
            this.Controls.Add(this.cboS1);
            this.Name = "frmSubtract";
            this.Text = "Subtract Two Rasters";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboS1;
        private System.Windows.Forms.ComboBox cboS2;
        private System.Windows.Forms.Button btnSubtract;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox txtSaveLocation;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblS1;
        private System.Windows.Forms.Label lblS2;
        private System.Windows.Forms.Label lblSaveLoc;
    }
}