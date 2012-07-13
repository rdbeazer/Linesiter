namespace LineSiterSitingTool
{
    partial class frmSelectLayers
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
            this.btnFinish = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.btnAddAll = new System.Windows.Forms.Button();
            this.btnAddOne = new System.Windows.Forms.Button();
            this.lbxToList = new System.Windows.Forms.ListBox();
            this.lbxFromList = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rtbHelpBox);
            this.panel1.Location = new System.Drawing.Point(637, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(214, 358);
            this.panel1.TabIndex = 0;
            // 
            // rtbHelpBox
            // 
            this.rtbHelpBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbHelpBox.Location = new System.Drawing.Point(0, 0);
            this.rtbHelpBox.Name = "rtbHelpBox";
            this.rtbHelpBox.Size = new System.Drawing.Size(214, 358);
            this.rtbHelpBox.TabIndex = 0;
            this.rtbHelpBox.Text = "";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnFinish);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnRemove);
            this.panel2.Controls.Add(this.btnRemoveAll);
            this.panel2.Controls.Add(this.btnAddAll);
            this.panel2.Controls.Add(this.btnAddOne);
            this.panel2.Controls.Add(this.lbxToList);
            this.panel2.Controls.Add(this.lbxFromList);
            this.panel2.Location = new System.Drawing.Point(2, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(629, 379);
            this.panel2.TabIndex = 1;
            // 
            // btnFinish
            // 
            this.btnFinish.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnFinish.Location = new System.Drawing.Point(538, 336);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(75, 23);
            this.btnFinish.TabIndex = 15;
            this.btnFinish.Text = "Finish";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(457, 336);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(270, 243);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(88, 23);
            this.btnRemove.TabIndex = 13;
            this.btnRemove.Text = "< Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.Location = new System.Drawing.Point(270, 195);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(88, 23);
            this.btnRemoveAll.TabIndex = 12;
            this.btnRemoveAll.Text = "<< Remove All";
            this.btnRemoveAll.UseVisualStyleBackColor = true;
            // 
            // btnAddAll
            // 
            this.btnAddAll.Location = new System.Drawing.Point(270, 147);
            this.btnAddAll.Name = "btnAddAll";
            this.btnAddAll.Size = new System.Drawing.Size(88, 23);
            this.btnAddAll.TabIndex = 11;
            this.btnAddAll.Text = "Add All >>";
            this.btnAddAll.UseVisualStyleBackColor = true;
            // 
            // btnAddOne
            // 
            this.btnAddOne.Location = new System.Drawing.Point(270, 99);
            this.btnAddOne.Name = "btnAddOne";
            this.btnAddOne.Size = new System.Drawing.Size(88, 23);
            this.btnAddOne.TabIndex = 10;
            this.btnAddOne.Text = "Add >";
            this.btnAddOne.UseVisualStyleBackColor = true;
            this.btnAddOne.Click += new System.EventHandler(this.btnAddOne_Click);
            // 
            // lbxToList
            // 
            this.lbxToList.FormattingEnabled = true;
            this.lbxToList.Location = new System.Drawing.Point(372, 33);
            this.lbxToList.Name = "lbxToList";
            this.lbxToList.Size = new System.Drawing.Size(246, 290);
            this.lbxToList.TabIndex = 9;
            // 
            // lbxFromList
            // 
            this.lbxFromList.FormattingEnabled = true;
            this.lbxFromList.Location = new System.Drawing.Point(10, 33);
            this.lbxFromList.Name = "lbxFromList";
            this.lbxFromList.Size = new System.Drawing.Size(246, 290);
            this.lbxFromList.TabIndex = 8;
            // 
            // frmSelectLayers
            // 
            this.AcceptButton = this.btnFinish;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(863, 382);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmSelectLayers";
            this.Text = "frmSelectLayers";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox rtbHelpBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnRemoveAll;
        private System.Windows.Forms.Button btnAddAll;
        private System.Windows.Forms.Button btnAddOne;
        private System.Windows.Forms.ListBox lbxToList;
        private System.Windows.Forms.ListBox lbxFromList;


    }
}