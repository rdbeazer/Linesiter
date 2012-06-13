namespace nx09SitingTool
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.spltMapLegend = new System.Windows.Forms.SplitContainer();
            this.spltLegTool = new System.Windows.Forms.SplitContainer();
            this.tabViews = new System.Windows.Forms.TabControl();
            this.tabLegend = new System.Windows.Forms.TabPage();
            this.lgMapMain = new DotSpatial.Controls.Legend();
            this.tabLCPDataStream = new System.Windows.Forms.TabPage();
            this.lbxLCPDataStream = new System.Windows.Forms.ListBox();
            this.tBMapMain = new DotSpatial.Controls.ToolManager();
            this.spltMapGraph = new System.Windows.Forms.SplitContainer();
            this.tabMapQuest = new System.Windows.Forms.TabControl();
            this.tbMap = new System.Windows.Forms.TabPage();
            this.mpMain = new DotSpatial.Controls.Map();
            this.tbQuestSets = new System.Windows.Forms.TabPage();
            this.dgQuesSets = new System.Windows.Forms.DataGridView();
            this.tbMainGraph = new System.Windows.Forms.TabPage();
            this.chtLeft = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.spltCharts = new System.Windows.Forms.SplitContainer();
            this.dgAttributes = new System.Windows.Forms.DataGridView();
            this.chtRight = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.iLMain = new System.Windows.Forms.ImageList(this.components);
            this.spatialStatusStrip1 = new DotSpatial.Controls.SpatialStatusStrip();
            this.tslStatus1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslProgBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.tslStatus2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssLCPA = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssStart = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssEnd = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslCoordX = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslCoordY = new System.Windows.Forms.ToolStripStatusLabel();
            this.apManMain = new DotSpatial.Controls.AppManager();
            ((System.ComponentModel.ISupportInitialize)(this.spltMapLegend)).BeginInit();
            this.spltMapLegend.Panel1.SuspendLayout();
            this.spltMapLegend.Panel2.SuspendLayout();
            this.spltMapLegend.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spltLegTool)).BeginInit();
            this.spltLegTool.Panel1.SuspendLayout();
            this.spltLegTool.Panel2.SuspendLayout();
            this.spltLegTool.SuspendLayout();
            this.tabViews.SuspendLayout();
            this.tabLegend.SuspendLayout();
            this.tabLCPDataStream.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spltMapGraph)).BeginInit();
            this.spltMapGraph.Panel1.SuspendLayout();
            this.spltMapGraph.Panel2.SuspendLayout();
            this.spltMapGraph.SuspendLayout();
            this.tabMapQuest.SuspendLayout();
            this.tbMap.SuspendLayout();
            this.tbQuestSets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgQuesSets)).BeginInit();
            this.tbMainGraph.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chtLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spltCharts)).BeginInit();
            this.spltCharts.Panel1.SuspendLayout();
            this.spltCharts.Panel2.SuspendLayout();
            this.spltCharts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAttributes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chtRight)).BeginInit();
            this.spatialStatusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // spltMapLegend
            // 
            this.spltMapLegend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spltMapLegend.Location = new System.Drawing.Point(0, 0);
            this.spltMapLegend.Name = "spltMapLegend";
            // 
            // spltMapLegend.Panel1
            // 
            this.spltMapLegend.Panel1.Controls.Add(this.spltLegTool);
            // 
            // spltMapLegend.Panel2
            // 
            this.spltMapLegend.Panel2.Controls.Add(this.spltMapGraph);
            this.spltMapLegend.Size = new System.Drawing.Size(953, 444);
            this.spltMapLegend.SplitterDistance = 203;
            this.spltMapLegend.TabIndex = 2;
            // 
            // spltLegTool
            // 
            this.spltLegTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spltLegTool.Location = new System.Drawing.Point(0, 0);
            this.spltLegTool.Name = "spltLegTool";
            this.spltLegTool.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spltLegTool.Panel1
            // 
            this.spltLegTool.Panel1.Controls.Add(this.tabViews);
            // 
            // spltLegTool.Panel2
            // 
            this.spltLegTool.Panel2.Controls.Add(this.tBMapMain);
            this.spltLegTool.Panel2Collapsed = true;
            this.spltLegTool.Size = new System.Drawing.Size(203, 444);
            this.spltLegTool.SplitterDistance = 190;
            this.spltLegTool.TabIndex = 0;
            // 
            // tabViews
            // 
            this.tabViews.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabViews.Controls.Add(this.tabLegend);
            this.tabViews.Controls.Add(this.tabLCPDataStream);
            this.tabViews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabViews.Location = new System.Drawing.Point(0, 0);
            this.tabViews.Name = "tabViews";
            this.tabViews.SelectedIndex = 0;
            this.tabViews.Size = new System.Drawing.Size(203, 444);
            this.tabViews.TabIndex = 0;
            // 
            // tabLegend
            // 
            this.tabLegend.Controls.Add(this.lgMapMain);
            this.tabLegend.Location = new System.Drawing.Point(4, 4);
            this.tabLegend.Name = "tabLegend";
            this.tabLegend.Padding = new System.Windows.Forms.Padding(3);
            this.tabLegend.Size = new System.Drawing.Size(195, 418);
            this.tabLegend.TabIndex = 0;
            this.tabLegend.Text = "Legend";
            this.tabLegend.UseVisualStyleBackColor = true;
            // 
            // lgMapMain
            // 
            this.lgMapMain.BackColor = System.Drawing.Color.White;
            this.lgMapMain.ControlRectangle = new System.Drawing.Rectangle(0, 0, 189, 412);
            this.lgMapMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lgMapMain.DocumentRectangle = new System.Drawing.Rectangle(0, 0, 187, 428);
            this.lgMapMain.HorizontalScrollEnabled = true;
            this.lgMapMain.Indentation = 30;
            this.lgMapMain.IsInitialized = false;
            this.lgMapMain.Location = new System.Drawing.Point(3, 3);
            this.lgMapMain.MinimumSize = new System.Drawing.Size(5, 5);
            this.lgMapMain.Name = "lgMapMain";
            this.lgMapMain.ProgressHandler = null;
            this.lgMapMain.ResetOnResize = false;
            this.lgMapMain.SelectionFontColor = System.Drawing.Color.Black;
            this.lgMapMain.SelectionHighlight = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(238)))), ((int)(((byte)(252)))));
            this.lgMapMain.Size = new System.Drawing.Size(189, 412);
            this.lgMapMain.TabIndex = 0;
            this.lgMapMain.Text = "legend1";
            this.lgMapMain.VerticalScrollEnabled = true;
            // 
            // tabLCPDataStream
            // 
            this.tabLCPDataStream.Controls.Add(this.lbxLCPDataStream);
            this.tabLCPDataStream.Location = new System.Drawing.Point(4, 4);
            this.tabLCPDataStream.Name = "tabLCPDataStream";
            this.tabLCPDataStream.Padding = new System.Windows.Forms.Padding(3);
            this.tabLCPDataStream.Size = new System.Drawing.Size(195, 293);
            this.tabLCPDataStream.TabIndex = 1;
            this.tabLCPDataStream.Text = "LCP Datastream";
            this.tabLCPDataStream.UseVisualStyleBackColor = true;
            // 
            // lbxLCPDataStream
            // 
            this.lbxLCPDataStream.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxLCPDataStream.FormattingEnabled = true;
            this.lbxLCPDataStream.Location = new System.Drawing.Point(3, 3);
            this.lbxLCPDataStream.Name = "lbxLCPDataStream";
            this.lbxLCPDataStream.Size = new System.Drawing.Size(189, 287);
            this.lbxLCPDataStream.TabIndex = 1;
            this.lbxLCPDataStream.Visible = false;
            // 
            // tBMapMain
            // 
            this.tBMapMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tBMapMain.ImageIndex = 0;
            this.tBMapMain.Legend = this.lgMapMain;
            this.tBMapMain.Location = new System.Drawing.Point(0, 0);
            this.tBMapMain.Name = "tBMapMain";
            this.tBMapMain.SelectedImageIndex = 0;
            this.tBMapMain.Size = new System.Drawing.Size(150, 46);
            this.tBMapMain.TabIndex = 3;
            // 
            // spltMapGraph
            // 
            this.spltMapGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spltMapGraph.Location = new System.Drawing.Point(0, 0);
            this.spltMapGraph.Name = "spltMapGraph";
            this.spltMapGraph.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spltMapGraph.Panel1
            // 
            this.spltMapGraph.Panel1.Controls.Add(this.tabMapQuest);
            // 
            // spltMapGraph.Panel2
            // 
            this.spltMapGraph.Panel2.Controls.Add(this.spltCharts);
            this.spltMapGraph.Size = new System.Drawing.Size(746, 444);
            this.spltMapGraph.SplitterDistance = 293;
            this.spltMapGraph.TabIndex = 0;
            // 
            // tabMapQuest
            // 
            this.tabMapQuest.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabMapQuest.Controls.Add(this.tbMap);
            this.tabMapQuest.Controls.Add(this.tbQuestSets);
            this.tabMapQuest.Controls.Add(this.tbMainGraph);
            this.tabMapQuest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMapQuest.HotTrack = true;
            this.tabMapQuest.Location = new System.Drawing.Point(0, 0);
            this.tabMapQuest.Name = "tabMapQuest";
            this.tabMapQuest.SelectedIndex = 0;
            this.tabMapQuest.Size = new System.Drawing.Size(746, 293);
            this.tabMapQuest.TabIndex = 0;
            this.tabMapQuest.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabMapQuest_Selected);
            // 
            // tbMap
            // 
            this.tbMap.Controls.Add(this.mpMain);
            this.tbMap.Location = new System.Drawing.Point(4, 4);
            this.tbMap.Name = "tbMap";
            this.tbMap.Padding = new System.Windows.Forms.Padding(3);
            this.tbMap.Size = new System.Drawing.Size(738, 267);
            this.tbMap.TabIndex = 0;
            this.tbMap.Text = "Map";
            this.tbMap.UseVisualStyleBackColor = true;
            // 
            // mpMain
            // 
            this.mpMain.AllowDrop = true;
            this.mpMain.BackColor = System.Drawing.Color.White;
            this.mpMain.CollectAfterDraw = false;
            this.mpMain.CollisionDetection = false;
            this.mpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mpMain.ExtendBuffer = false;
            this.mpMain.FunctionMode = DotSpatial.Controls.FunctionMode.None;
            this.mpMain.IsBusy = false;
            this.mpMain.Legend = this.lgMapMain;
            this.mpMain.Location = new System.Drawing.Point(3, 3);
            this.mpMain.Name = "mpMain";
            this.mpMain.ProgressHandler = null;
            this.mpMain.ProjectionModeDefine = DotSpatial.Controls.ActionMode.Prompt;
            this.mpMain.ProjectionModeReproject = DotSpatial.Controls.ActionMode.Prompt;
            this.mpMain.RedrawLayersWhileResizing = false;
            this.mpMain.SelectionEnabled = true;
            this.mpMain.Size = new System.Drawing.Size(732, 261);
            this.mpMain.TabIndex = 0;
            this.mpMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mpMain_MouseUp);
            // 
            // tbQuestSets
            // 
            this.tbQuestSets.Controls.Add(this.dgQuesSets);
            this.tbQuestSets.Location = new System.Drawing.Point(4, 4);
            this.tbQuestSets.Name = "tbQuestSets";
            this.tbQuestSets.Padding = new System.Windows.Forms.Padding(3);
            this.tbQuestSets.Size = new System.Drawing.Size(738, 185);
            this.tbQuestSets.TabIndex = 1;
            this.tbQuestSets.Text = "Question Set";
            this.tbQuestSets.UseVisualStyleBackColor = true;
            // 
            // dgQuesSets
            // 
            this.dgQuesSets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgQuesSets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgQuesSets.Location = new System.Drawing.Point(3, 3);
            this.dgQuesSets.Name = "dgQuesSets";
            this.dgQuesSets.Size = new System.Drawing.Size(732, 179);
            this.dgQuesSets.TabIndex = 0;
            // 
            // tbMainGraph
            // 
            this.tbMainGraph.Controls.Add(this.chtLeft);
            this.tbMainGraph.Location = new System.Drawing.Point(4, 4);
            this.tbMainGraph.Name = "tbMainGraph";
            this.tbMainGraph.Size = new System.Drawing.Size(738, 185);
            this.tbMainGraph.TabIndex = 2;
            this.tbMainGraph.Text = "Graph";
            this.tbMainGraph.UseVisualStyleBackColor = true;
            // 
            // chtLeft
            // 
            chartArea1.Name = "ChartArea1";
            this.chtLeft.ChartAreas.Add(chartArea1);
            this.chtLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chtLeft.Legends.Add(legend1);
            this.chtLeft.Location = new System.Drawing.Point(0, 0);
            this.chtLeft.Name = "chtLeft";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chtLeft.Series.Add(series1);
            this.chtLeft.Size = new System.Drawing.Size(738, 185);
            this.chtLeft.TabIndex = 0;
            this.chtLeft.Text = "chart1";
            // 
            // spltCharts
            // 
            this.spltCharts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spltCharts.Location = new System.Drawing.Point(0, 0);
            this.spltCharts.Name = "spltCharts";
            // 
            // spltCharts.Panel1
            // 
            this.spltCharts.Panel1.Controls.Add(this.dgAttributes);
            // 
            // spltCharts.Panel2
            // 
            this.spltCharts.Panel2.Controls.Add(this.chtRight);
            this.spltCharts.Panel2Collapsed = true;
            this.spltCharts.Size = new System.Drawing.Size(746, 147);
            this.spltCharts.SplitterDistance = 375;
            this.spltCharts.TabIndex = 0;
            // 
            // dgAttributes
            // 
            this.dgAttributes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgAttributes.Location = new System.Drawing.Point(0, 0);
            this.dgAttributes.Name = "dgAttributes";
            this.dgAttributes.Size = new System.Drawing.Size(746, 147);
            this.dgAttributes.TabIndex = 1;
            // 
            // chtRight
            // 
            chartArea2.Name = "ChartArea1";
            this.chtRight.ChartAreas.Add(chartArea2);
            this.chtRight.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chtRight.Legends.Add(legend2);
            this.chtRight.Location = new System.Drawing.Point(0, 0);
            this.chtRight.Name = "chtRight";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chtRight.Series.Add(series2);
            this.chtRight.Size = new System.Drawing.Size(96, 100);
            this.chtRight.TabIndex = 0;
            this.chtRight.Text = "chart1";
            // 
            // iLMain
            // 
            this.iLMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iLMain.ImageStream")));
            this.iLMain.TransparentColor = System.Drawing.Color.Transparent;
            this.iLMain.Images.SetKeyName(0, "AnswerIc.png");
            this.iLMain.Images.SetKeyName(1, "QuestionIc.png");
            this.iLMain.Images.SetKeyName(2, "WeightIc.png");
            // 
            // spatialStatusStrip1
            // 
            this.spatialStatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslStatus1,
            this.tslProgBar1,
            this.tslStatus2,
            this.tssLCPA,
            this.tssStart,
            this.tssEnd,
            this.toolStripStatusLabel1,
            this.tsslCoordX,
            this.tsslCoordY});
            this.spatialStatusStrip1.Location = new System.Drawing.Point(0, 444);
            this.spatialStatusStrip1.Name = "spatialStatusStrip1";
            this.spatialStatusStrip1.ProgressBar = null;
            this.spatialStatusStrip1.ProgressLabel = this.tslStatus1;
            this.spatialStatusStrip1.Size = new System.Drawing.Size(953, 24);
            this.spatialStatusStrip1.TabIndex = 1;
            this.spatialStatusStrip1.Text = "spatialStatusStrip1";
            // 
            // tslStatus1
            // 
            this.tslStatus1.Name = "tslStatus1";
            this.tslStatus1.Size = new System.Drawing.Size(0, 19);
            // 
            // tslProgBar1
            // 
            this.tslProgBar1.Name = "tslProgBar1";
            this.tslProgBar1.Size = new System.Drawing.Size(100, 18);
            this.tslProgBar1.Visible = false;
            // 
            // tslStatus2
            // 
            this.tslStatus2.Name = "tslStatus2";
            this.tslStatus2.Size = new System.Drawing.Size(0, 19);
            // 
            // tssLCPA
            // 
            this.tssLCPA.Name = "tssLCPA";
            this.tssLCPA.Size = new System.Drawing.Size(0, 19);
            // 
            // tssStart
            // 
            this.tssStart.Name = "tssStart";
            this.tssStart.Size = new System.Drawing.Size(0, 19);
            // 
            // tssEnd
            // 
            this.tssEnd.Name = "tssEnd";
            this.tssEnd.Size = new System.Drawing.Size(0, 19);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(896, 19);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // tsslCoordX
            // 
            this.tsslCoordX.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.tsslCoordX.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tsslCoordX.Name = "tsslCoordX";
            this.tsslCoordX.Size = new System.Drawing.Size(21, 19);
            this.tsslCoordX.Text = "--";
            // 
            // tsslCoordY
            // 
            this.tsslCoordY.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.tsslCoordY.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.tsslCoordY.Name = "tsslCoordY";
            this.tsslCoordY.Size = new System.Drawing.Size(21, 19);
            this.tsslCoordY.Text = "--";
            // 
            // apManMain
            // 
            //this.apManMain.CompositionContainer = null;
            this.apManMain.Directories = ((System.Collections.Generic.List<string>)(resources.GetObject("apManMain.Directories")));
            this.apManMain.DockManager = null;
            this.apManMain.HeaderControl = null;
            this.apManMain.Legend = this.lgMapMain;
            this.apManMain.Map = this.mpMain;
            this.apManMain.ProgressHandler = null;
            this.apManMain.ShowExtensionsDialog = DotSpatial.Controls.ShowExtensionsDialog.Default;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(953, 468);
            this.Controls.Add(this.spltMapLegend);
            this.Controls.Add(this.spatialStatusStrip1);
            this.Name = "frmMain";
            this.Text = "Line Siter";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.spltMapLegend.Panel1.ResumeLayout(false);
            this.spltMapLegend.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spltMapLegend)).EndInit();
            this.spltMapLegend.ResumeLayout(false);
            this.spltLegTool.Panel1.ResumeLayout(false);
            this.spltLegTool.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spltLegTool)).EndInit();
            this.spltLegTool.ResumeLayout(false);
            this.tabViews.ResumeLayout(false);
            this.tabLegend.ResumeLayout(false);
            this.tabLCPDataStream.ResumeLayout(false);
            this.spltMapGraph.Panel1.ResumeLayout(false);
            this.spltMapGraph.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spltMapGraph)).EndInit();
            this.spltMapGraph.ResumeLayout(false);
            this.tabMapQuest.ResumeLayout(false);
            this.tbMap.ResumeLayout(false);
            this.tbQuestSets.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgQuesSets)).EndInit();
            this.tbMainGraph.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chtLeft)).EndInit();
            this.spltCharts.Panel1.ResumeLayout(false);
            this.spltCharts.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spltCharts)).EndInit();
            this.spltCharts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgAttributes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chtRight)).EndInit();
            this.spatialStatusStrip1.ResumeLayout(false);
            this.spatialStatusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //private DotSpatial.Controls.RibbonControls.Ribbon ribMain;
        private DotSpatial.Controls.SpatialStatusStrip spatialStatusStrip1;
        private System.Windows.Forms.SplitContainer spltMapLegend;
        //private DotSpatial.Controls.RibbonControls.RibbonTab ribMapControls;
        //private DotSpatial.Controls.RibbonControls.RibbonPanel ribbonPanel1;
        //private DotSpatial.Controls.RibbonControls.RibbonPanel ribbonPanel2;
        //private DotSpatial.Controls.RibbonControls.RibbonButton btnZoomIn;
        //private DotSpatial.Controls.RibbonControls.RibbonButton btnAddLayer;
        //private DotSpatial.Controls.RibbonControls.RibbonButton btnZoomOut;
        //private DotSpatial.Controls.RibbonControls.RibbonButton btnFixedZoomIn;
        //private DotSpatial.Controls.RibbonControls.RibbonButton btnFixedZoomOut;
        //private DotSpatial.Controls.RibbonControls.RibbonButton btnPan;
        //private DotSpatial.Controls.RibbonControls.RibbonButton btnMaxExtents;
        //private DotSpatial.Controls.RibbonControls.RibbonPanel ribbonPanel3;
        //private DotSpatial.Controls.RibbonControls.RibbonButton btnInfo;
        //private DotSpatial.Controls.RibbonControls.RibbonTab ribOperations;
        //private DotSpatial.Controls.RibbonControls.RibbonOrbMenuItem ribOrbOpen;
        //private DotSpatial.Controls.RibbonControls.RibbonOrbMenuItem ribOrbNew;
        //private DotSpatial.Controls.RibbonControls.RibbonOrbMenuItem ribOrbSave;
        //private DotSpatial.Controls.RibbonControls.RibbonSeparator ribbonSeparator1;
        private System.Windows.Forms.ToolStripStatusLabel tslStatus1;
        private System.Windows.Forms.ToolStripProgressBar tslProgBar1;
        private DotSpatial.Controls.Legend lgMapMain;
        //private DotSpatial.Controls.RibbonControls.RibbonOrbMenuItem ribOExit;
        private System.Windows.Forms.SplitContainer spltMapGraph;
        //private DotSpatial.Controls.RibbonControls.RibbonButton btnHelp;
        private System.Windows.Forms.SplitContainer spltCharts;
        private System.Windows.Forms.DataVisualization.Charting.Chart chtRight;
        private System.Windows.Forms.ImageList iLMain;
        //private DotSpatial.Controls.RibbonControls.RibbonButton btnAbout;
        //private DotSpatial.Controls.RibbonControls.RibbonPanel ribPTestProcesses;
        //private DotSpatial.Controls.RibbonControls.RibbonButton btnMonteCarlo;
        private System.Windows.Forms.ToolStripStatusLabel tslStatus2;
        //private DotSpatial.Controls.RibbonControls.RibbonButton rbtnSubmitFrequencies;
        private System.Windows.Forms.SplitContainer spltLegTool;
        private DotSpatial.Controls.ToolManager tBMapMain;
        private System.Windows.Forms.ToolStripStatusLabel tssLCPA;
        private System.Windows.Forms.ToolStripStatusLabel tssStart;
        private System.Windows.Forms.ToolStripStatusLabel tssEnd;
        //private DotSpatial.Controls.RibbonControls.RibbonButton rbtnMCProcessTest;
        //private DotSpatial.Controls.RibbonControls.RibbonSeparator ribbonSeparator3;
        //private DotSpatial.Controls.RibbonControls.RibbonLabel ribbonLabel1;
        //private DotSpatial.Controls.RibbonControls.RibbonButton rbtnSelect;
        //private DotSpatial.Controls.RibbonControls.RibbonButton rbtnLoadAttributes;
        private System.Windows.Forms.DataGridView dgAttributes;
        //private DotSpatial.Controls.RibbonControls.RibbonSeparator ribbonSeparator2;
        //private DotSpatial.Controls.RibbonControls.RibbonButton rbtnPrintLayout;
        private System.Windows.Forms.ToolStripStatusLabel tsslCoordX;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        //private DotSpatial.Controls.RibbonControls.RibbonButton btnPresenceAbsence;
        //private DotSpatial.Controls.RibbonControls.RibbonButton btnVectorToRaster;
        //private DotSpatial.Controls.RibbonControls.RibbonButton btnStartEnd;
        private System.Windows.Forms.ToolStripStatusLabel tsslCoordY;
        //private DotSpatial.Controls.RibbonControls.RibbonButton rbtnSaveAttChange;
        //private DotSpatial.Controls.RibbonControls.RibbonButton rbtnMultiplyRasters;
        //private DotSpatial.Controls.RibbonControls.RibbonButton ribLCP;
        private System.Windows.Forms.ListBox lbxLCPDataStream;
        //private DotSpatial.Controls.RibbonControls.RibbonTab ribTool;
        //private DotSpatial.Controls.RibbonControls.RibbonPanel ribbonPanel5;
        //private DotSpatial.Controls.RibbonControls.RibbonButton rbtnStart;
        private DotSpatial.Controls.AppManager apManMain;
        private System.Windows.Forms.TabControl tabViews;
        private System.Windows.Forms.TabPage tabLegend;
        private System.Windows.Forms.TabPage tabLCPDataStream;
        private System.Windows.Forms.TabControl tabMapQuest;
        private System.Windows.Forms.TabPage tbMap;
        private DotSpatial.Controls.Map mpMain;
        private System.Windows.Forms.TabPage tbQuestSets;
        private System.Windows.Forms.DataGridView dgQuesSets;
        private System.Windows.Forms.TabPage tbMainGraph;
        private System.Windows.Forms.DataVisualization.Charting.Chart chtLeft;

    }
}

