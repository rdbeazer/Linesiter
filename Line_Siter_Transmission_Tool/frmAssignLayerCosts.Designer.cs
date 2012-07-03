namespace nx09SitingTool
{
    partial class frmAssignLayerCosts
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
            this.rtbHelpBox = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnFinish = new System.Windows.Forms.Button();
            this.gbSelectLayerCosts = new System.Windows.Forms.GroupBox();
            this.dgvSelectLayers = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.gbSelectLayerCosts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectLayers)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rtbHelpBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(425, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 340);
            this.panel1.TabIndex = 0;
            // 
            // rtbHelpBox
            // 
            this.rtbHelpBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbHelpBox.Location = new System.Drawing.Point(0, 0);
            this.rtbHelpBox.Name = "rtbHelpBox";
            this.rtbHelpBox.Size = new System.Drawing.Size(200, 340);
            this.rtbHelpBox.TabIndex = 1;
            this.rtbHelpBox.Text = "";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnFinish);
            this.panel2.Controls.Add(this.gbSelectLayerCosts);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(425, 340);
            this.panel2.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(262, 305);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnFinish
            // 
            this.btnFinish.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnFinish.Location = new System.Drawing.Point(343, 305);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(75, 23);
            this.btnFinish.TabIndex = 2;
            this.btnFinish.Text = "Finish";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // gbSelectLayerCosts
            // 
            this.gbSelectLayerCosts.Controls.Add(this.dgvSelectLayers);
            this.gbSelectLayerCosts.Location = new System.Drawing.Point(13, 45);
            this.gbSelectLayerCosts.Name = "gbSelectLayerCosts";
            this.gbSelectLayerCosts.Size = new System.Drawing.Size(406, 248);
            this.gbSelectLayerCosts.TabIndex = 1;
            this.gbSelectLayerCosts.TabStop = false;
            this.gbSelectLayerCosts.Text = "Select layers to include along with a cost field";
            // 
            // dgvSelectLayers
            // 
            this.dgvSelectLayers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSelectLayers.Location = new System.Drawing.Point(6, 19);
            this.dgvSelectLayers.Name = "dgvSelectLayers";
            this.dgvSelectLayers.Size = new System.Drawing.Size(394, 223);
            this.dgvSelectLayers.TabIndex = 0;
            // 
            // frmAssignLayerCosts
            // 
            this.AcceptButton = this.btnFinish;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(625, 340);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmAssignLayerCosts";
            this.Text = "Choose Layer and Costs";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.gbSelectLayerCosts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectLayers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox rtbHelpBox;
        private System.Windows.Forms.GroupBox gbSelectLayerCosts;
        private System.Windows.Forms.DataGridView dgvSelectLayers;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnFinish;

    }
}