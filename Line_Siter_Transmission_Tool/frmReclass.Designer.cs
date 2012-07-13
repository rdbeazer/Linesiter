namespace LineSiterSitingTool
{
    partial class frmReclass
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReclass));
            this.txtRasterOpen = new System.Windows.Forms.TextBox();
            this.grpReclass = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClassify = new System.Windows.Forms.Button();
            this.dgvReclassify = new System.Windows.Forms.DataGridView();
            this.dgvcOldValues = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcNewValues = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtRasterSave = new System.Windows.Forms.TextBox();
            this.btnRasterOpen = new System.Windows.Forms.Button();
            this.btnReclassSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rtbHelpBox = new System.Windows.Forms.RichTextBox();
            this.grpReclass.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReclassify)).BeginInit();
            this.SuspendLayout();
            // 
            // txtRasterOpen
            // 
            this.txtRasterOpen.Location = new System.Drawing.Point(33, 49);
            this.txtRasterOpen.Name = "txtRasterOpen";
            this.txtRasterOpen.Size = new System.Drawing.Size(338, 20);
            this.txtRasterOpen.TabIndex = 0;
            // 
            // grpReclass
            // 
            this.grpReclass.Controls.Add(this.btnCancel);
            this.grpReclass.Controls.Add(this.btnClassify);
            this.grpReclass.Controls.Add(this.dgvReclassify);
            this.grpReclass.Location = new System.Drawing.Point(33, 93);
            this.grpReclass.Name = "grpReclass";
            this.grpReclass.Size = new System.Drawing.Size(367, 178);
            this.grpReclass.TabIndex = 2;
            this.grpReclass.TabStop = false;
            this.grpReclass.Text = "Reclassification";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(286, 46);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnClassify
            // 
            this.btnClassify.Location = new System.Drawing.Point(286, 16);
            this.btnClassify.Name = "btnClassify";
            this.btnClassify.Size = new System.Drawing.Size(75, 23);
            this.btnClassify.TabIndex = 2;
            this.btnClassify.Text = "Classify";
            this.btnClassify.UseVisualStyleBackColor = true;
            this.btnClassify.Click += new System.EventHandler(this.btnClassify_Click);
            // 
            // dgvReclassify
            // 
            this.dgvReclassify.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvReclassify.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dgvReclassify.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReclassify.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcOldValues,
            this.dgvcNewValues});
            this.dgvReclassify.Location = new System.Drawing.Point(6, 19);
            this.dgvReclassify.Name = "dgvReclassify";
            this.dgvReclassify.Size = new System.Drawing.Size(243, 150);
            this.dgvReclassify.TabIndex = 0;
            // 
            // dgvcOldValues
            // 
            this.dgvcOldValues.HeaderText = "Old Values";
            this.dgvcOldValues.Name = "dgvcOldValues";
            // 
            // dgvcNewValues
            // 
            this.dgvcNewValues.HeaderText = "New Values";
            this.dgvcNewValues.Name = "dgvcNewValues";
            // 
            // txtRasterSave
            // 
            this.txtRasterSave.Location = new System.Drawing.Point(33, 307);
            this.txtRasterSave.Name = "txtRasterSave";
            this.txtRasterSave.Size = new System.Drawing.Size(338, 20);
            this.txtRasterSave.TabIndex = 3;
            // 
            // btnRasterOpen
            // 
            this.btnRasterOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnRasterOpen.Image")));
            this.btnRasterOpen.Location = new System.Drawing.Point(378, 49);
            this.btnRasterOpen.Name = "btnRasterOpen";
            this.btnRasterOpen.Size = new System.Drawing.Size(32, 24);
            this.btnRasterOpen.TabIndex = 4;
            this.btnRasterOpen.UseVisualStyleBackColor = true;
            this.btnRasterOpen.Click += new System.EventHandler(this.btnRasterOpen_Click);
            // 
            // btnReclassSave
            // 
            this.btnReclassSave.Image = ((System.Drawing.Image)(resources.GetObject("btnReclassSave.Image")));
            this.btnReclassSave.Location = new System.Drawing.Point(378, 307);
            this.btnReclassSave.Name = "btnReclassSave";
            this.btnReclassSave.Size = new System.Drawing.Size(32, 24);
            this.btnReclassSave.TabIndex = 5;
            this.btnReclassSave.UseVisualStyleBackColor = true;
            this.btnReclassSave.Click += new System.EventHandler(this.btnReclassSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Select Raster for Reclassification";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 291);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(218, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Select Save Location for Reclassified Raster";
            // 
            // rtbHelpBox
            // 
            this.rtbHelpBox.Location = new System.Drawing.Point(439, 30);
            this.rtbHelpBox.Name = "rtbHelpBox";
            this.rtbHelpBox.Size = new System.Drawing.Size(224, 301);
            this.rtbHelpBox.TabIndex = 9;
            this.rtbHelpBox.Text = "";
            // 
            // frmReclass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(684, 360);
            this.Controls.Add(this.rtbHelpBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnReclassSave);
            this.Controls.Add(this.btnRasterOpen);
            this.Controls.Add(this.txtRasterSave);
            this.Controls.Add(this.grpReclass);
            this.Controls.Add(this.txtRasterOpen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmReclass";
            this.Text = "Raster Reclassification";
            this.grpReclass.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReclassify)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtRasterOpen;
        private System.Windows.Forms.GroupBox grpReclass;
        private System.Windows.Forms.DataGridView dgvReclassify;
        private System.Windows.Forms.Button btnClassify;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcOldValues;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcNewValues;
        private System.Windows.Forms.TextBox txtRasterSave;
        private System.Windows.Forms.Button btnRasterOpen;
        private System.Windows.Forms.Button btnReclassSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox rtbHelpBox;
        private System.Windows.Forms.TextBox txtSaveLocation;
    }
}