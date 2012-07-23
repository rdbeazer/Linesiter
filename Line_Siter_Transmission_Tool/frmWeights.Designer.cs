namespace LineSiterSitingTool
{
    partial class frmWeights
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
            this.trkHigh = new System.Windows.Forms.TrackBar();
            this.txtHigh = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMedHigh = new System.Windows.Forms.TextBox();
            this.trkMedHigh = new System.Windows.Forms.TrackBar();
            this.lblMediumLow = new System.Windows.Forms.Label();
            this.txtMedLow = new System.Windows.Forms.TextBox();
            this.trkMedLow = new System.Windows.Forms.TrackBar();
            this.lblLSMedium = new System.Windows.Forms.Label();
            this.txtMedium = new System.Windows.Forms.TextBox();
            this.trkMedium = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLow = new System.Windows.Forms.TextBox();
            this.trkLow = new System.Windows.Forms.TrackBar();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trkHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkMedHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkMedLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkMedium)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkLow)).BeginInit();
            this.SuspendLayout();
            // 
            // trkHigh
            // 
            this.trkHigh.Location = new System.Drawing.Point(56, 57);
            this.trkHigh.Name = "trkHigh";
            this.trkHigh.Size = new System.Drawing.Size(104, 45);
            this.trkHigh.TabIndex = 0;
            this.trkHigh.TickFrequency = 5;
            this.trkHigh.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trkHigh.Value = 1;
            this.trkHigh.Scroll += new System.EventHandler(this.trkHigh_Scroll);
            // 
            // txtHigh
            // 
            this.txtHigh.Location = new System.Drawing.Point(160, 67);
            this.txtHigh.Name = "txtHigh";
            this.txtHigh.Size = new System.Drawing.Size(46, 20);
            this.txtHigh.TabIndex = 1;
            this.txtHigh.Text = "0.1";
            this.txtHigh.TextChanged += new System.EventHandler(this.txtHigh_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Weight for LSHigh";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Weight for LSMedHigh";
            // 
            // txtMedHigh
            // 
            this.txtMedHigh.Location = new System.Drawing.Point(160, 135);
            this.txtMedHigh.Name = "txtMedHigh";
            this.txtMedHigh.Size = new System.Drawing.Size(46, 20);
            this.txtMedHigh.TabIndex = 4;
            this.txtMedHigh.Text = "0.3";
            this.txtMedHigh.TextChanged += new System.EventHandler(this.txtMedHigh_TextChanged);
            // 
            // trkMedHigh
            // 
            this.trkMedHigh.Location = new System.Drawing.Point(56, 125);
            this.trkMedHigh.Name = "trkMedHigh";
            this.trkMedHigh.Size = new System.Drawing.Size(104, 45);
            this.trkMedHigh.TabIndex = 3;
            this.trkMedHigh.TickFrequency = 5;
            this.trkMedHigh.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trkMedHigh.Value = 3;
            this.trkMedHigh.Scroll += new System.EventHandler(this.trkMedHigh_Scroll);
            // 
            // lblMediumLow
            // 
            this.lblMediumLow.AutoSize = true;
            this.lblMediumLow.Location = new System.Drawing.Point(53, 257);
            this.lblMediumLow.Name = "lblMediumLow";
            this.lblMediumLow.Size = new System.Drawing.Size(113, 13);
            this.lblMediumLow.TabIndex = 11;
            this.lblMediumLow.Text = "Weight for LSMedLow";
            // 
            // txtMedLow
            // 
            this.txtMedLow.Location = new System.Drawing.Point(160, 283);
            this.txtMedLow.Name = "txtMedLow";
            this.txtMedLow.Size = new System.Drawing.Size(46, 20);
            this.txtMedLow.TabIndex = 10;
            this.txtMedLow.Text = "0.7";
            this.txtMedLow.TextChanged += new System.EventHandler(this.txtMedLow_TextChanged);
            // 
            // trkMedLow
            // 
            this.trkMedLow.Location = new System.Drawing.Point(56, 273);
            this.trkMedLow.Name = "trkMedLow";
            this.trkMedLow.Size = new System.Drawing.Size(104, 45);
            this.trkMedLow.TabIndex = 9;
            this.trkMedLow.TickFrequency = 5;
            this.trkMedLow.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trkMedLow.Value = 7;
            this.trkMedLow.Scroll += new System.EventHandler(this.trkMedLow_Scroll);
            // 
            // lblLSMedium
            // 
            this.lblLSMedium.AutoSize = true;
            this.lblLSMedium.Location = new System.Drawing.Point(53, 183);
            this.lblLSMedium.Name = "lblLSMedium";
            this.lblLSMedium.Size = new System.Drawing.Size(109, 13);
            this.lblLSMedium.TabIndex = 8;
            this.lblLSMedium.Text = "Weight for LSMedium";
            // 
            // txtMedium
            // 
            this.txtMedium.Location = new System.Drawing.Point(160, 209);
            this.txtMedium.Name = "txtMedium";
            this.txtMedium.Size = new System.Drawing.Size(46, 20);
            this.txtMedium.TabIndex = 7;
            this.txtMedium.Text = "0.5";
            this.txtMedium.TextChanged += new System.EventHandler(this.txtMedium_TextChanged);
            // 
            // trkMedium
            // 
            this.trkMedium.Location = new System.Drawing.Point(56, 199);
            this.trkMedium.Name = "trkMedium";
            this.trkMedium.Size = new System.Drawing.Size(104, 45);
            this.trkMedium.TabIndex = 6;
            this.trkMedium.TickFrequency = 5;
            this.trkMedium.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trkMedium.Value = 5;
            this.trkMedium.Scroll += new System.EventHandler(this.trkMedium_Scroll);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 331);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Weight for LSLow";
            // 
            // txtLow
            // 
            this.txtLow.Location = new System.Drawing.Point(160, 357);
            this.txtLow.Name = "txtLow";
            this.txtLow.Size = new System.Drawing.Size(46, 20);
            this.txtLow.TabIndex = 13;
            this.txtLow.Text = "0.9";
            this.txtLow.TextChanged += new System.EventHandler(this.txtLow_TextChanged);
            // 
            // trkLow
            // 
            this.trkLow.Location = new System.Drawing.Point(56, 347);
            this.trkLow.Name = "trkLow";
            this.trkLow.Size = new System.Drawing.Size(104, 45);
            this.trkLow.TabIndex = 12;
            this.trkLow.TickFrequency = 5;
            this.trkLow.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trkLow.Value = 9;
            this.trkLow.Scroll += new System.EventHandler(this.trkLow_Scroll);
            // 
            // btnSubmit
            // 
            this.btnSubmit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSubmit.Location = new System.Drawing.Point(223, 407);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 15;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(142, 407);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // frmWeights
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(321, 452);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtLow);
            this.Controls.Add(this.trkLow);
            this.Controls.Add(this.lblMediumLow);
            this.Controls.Add(this.txtMedLow);
            this.Controls.Add(this.trkMedLow);
            this.Controls.Add(this.lblLSMedium);
            this.Controls.Add(this.txtMedium);
            this.Controls.Add(this.trkMedium);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMedHigh);
            this.Controls.Add(this.trkMedHigh);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtHigh);
            this.Controls.Add(this.trkHigh);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmWeights";
            this.Text = "Select Weights";
            ((System.ComponentModel.ISupportInitialize)(this.trkHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkMedHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkMedLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkMedium)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkLow)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trkHigh;
        private System.Windows.Forms.TextBox txtHigh;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMedHigh;
        private System.Windows.Forms.TrackBar trkMedHigh;
        private System.Windows.Forms.Label lblMediumLow;
        private System.Windows.Forms.TextBox txtMedLow;
        private System.Windows.Forms.TrackBar trkMedLow;
        private System.Windows.Forms.Label lblLSMedium;
        private System.Windows.Forms.TextBox txtMedium;
        private System.Windows.Forms.TrackBar trkMedium;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLow;
        private System.Windows.Forms.TrackBar trkLow;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnCancel;
    }
}