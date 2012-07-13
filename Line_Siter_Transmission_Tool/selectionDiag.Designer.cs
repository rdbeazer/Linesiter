namespace LineSiterSitingTool
{
    partial class selectionDiag
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(selectionDiag));
            this.cboLayers = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.cboAttribute = new System.Windows.Forms.ComboBox();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.lblSelectALayer = new System.Windows.Forms.Label();
            this.lblSelectAttribute = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cboLayers
            // 
            this.cboLayers.FormattingEnabled = true;
            this.cboLayers.Location = new System.Drawing.Point(43, 40);
            this.cboLayers.Name = "cboLayers";
            this.cboLayers.Size = new System.Drawing.Size(258, 21);
            this.cboLayers.TabIndex = 0;
            this.cboLayers.SelectedIndexChanged += new System.EventHandler(this.cboLayers_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(322, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(36, 43);
            this.button1.TabIndex = 1;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(257, 157);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSelect
            // 
            this.btnSelect.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSelect.Location = new System.Drawing.Point(338, 157);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 3;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // cboAttribute
            // 
            this.cboAttribute.FormattingEnabled = true;
            this.cboAttribute.Location = new System.Drawing.Point(43, 84);
            this.cboAttribute.Name = "cboAttribute";
            this.cboAttribute.Size = new System.Drawing.Size(258, 21);
            this.cboAttribute.TabIndex = 4;
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(43, 128);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(156, 20);
            this.txtValue.TabIndex = 5;
            // 
            // lblSelectALayer
            // 
            this.lblSelectALayer.AutoSize = true;
            this.lblSelectALayer.Location = new System.Drawing.Point(40, 24);
            this.lblSelectALayer.Name = "lblSelectALayer";
            this.lblSelectALayer.Size = new System.Drawing.Size(76, 13);
            this.lblSelectALayer.TabIndex = 6;
            this.lblSelectALayer.Text = "Select A Layer";
            // 
            // lblSelectAttribute
            // 
            this.lblSelectAttribute.AutoSize = true;
            this.lblSelectAttribute.Location = new System.Drawing.Point(40, 68);
            this.lblSelectAttribute.Name = "lblSelectAttribute";
            this.lblSelectAttribute.Size = new System.Drawing.Size(133, 13);
            this.lblSelectAttribute.TabIndex = 7;
            this.lblSelectAttribute.Text = "Select An Attribute Header";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Select The Attribute Value";
            // 
            // selectionDiag
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(439, 197);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblSelectAttribute);
            this.Controls.Add(this.lblSelectALayer);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.cboAttribute);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cboLayers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "selectionDiag";
            this.Text = "Select a Datafile";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboLayers;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.ComboBox cboAttribute;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Label lblSelectALayer;
        private System.Windows.Forms.Label lblSelectAttribute;
        private System.Windows.Forms.Label label1;
    }
}