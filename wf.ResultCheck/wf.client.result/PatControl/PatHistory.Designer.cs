namespace dcl.client.result.PatControl
{
    partial class PatHistory
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView1 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.toolTipController1 = new DevExpress.Utils.ToolTipController(this.components);
            this.btnExportImg = new DevExpress.XtraEditors.SimpleButton();
            this.btnExportResult = new DevExpress.XtraEditors.SimpleButton();
            this.chkShowThreshold = new DevExpress.XtraEditors.CheckEdit();
            this.chkShowCritical = new DevExpress.XtraEditors.CheckEdit();
            this.chkShowRef = new DevExpress.XtraEditors.CheckEdit();
            this.btnRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtResultCount = new DevExpress.XtraEditors.SpinEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.chartControl1 = new DevExpress.XtraCharts.ChartControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbAge = new System.Windows.Forms.Label();
            this.lbItmEcd = new System.Windows.Forms.Label();
            this.lbDateInterval = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkShowThreshold.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkShowCritical.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkShowRef.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtResultCount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(742, 210);
            this.gridControl1.TabIndex = 6;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gridView1.Appearance.EvenRow.Options.UseBackColor = true;
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsCustomization.AllowGroup = false;
            this.gridView1.OptionsCustomization.AllowSort = false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            // 
            // btnExportImg
            // 
            this.btnExportImg.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnExportImg.Location = new System.Drawing.Point(504, 4);
            this.btnExportImg.Name = "btnExportImg";
            this.btnExportImg.Size = new System.Drawing.Size(75, 23);
            this.btnExportImg.TabIndex = 8;
            this.btnExportImg.Text = "导出分析图";
            this.btnExportImg.Click += new System.EventHandler(this.btnExportImg_Click);
            // 
            // btnExportResult
            // 
            this.btnExportResult.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnExportResult.Location = new System.Drawing.Point(584, 4);
            this.btnExportResult.Name = "btnExportResult";
            this.btnExportResult.Size = new System.Drawing.Size(71, 23);
            this.btnExportResult.TabIndex = 7;
            this.btnExportResult.Text = "导出结果";
            this.btnExportResult.Click += new System.EventHandler(this.btnExportResult_Click);
            // 
            // chkShowThreshold
            // 
            this.chkShowThreshold.Location = new System.Drawing.Point(428, 6);
            this.chkShowThreshold.Name = "chkShowThreshold";
            this.chkShowThreshold.Properties.Caption = "显示阈值";
            this.chkShowThreshold.Size = new System.Drawing.Size(70, 19);
            this.chkShowThreshold.TabIndex = 6;
            // 
            // chkShowCritical
            // 
            this.chkShowCritical.Location = new System.Drawing.Point(338, 6);
            this.chkShowCritical.Name = "chkShowCritical";
            this.chkShowCritical.Properties.Caption = "显示危急值";
            this.chkShowCritical.Size = new System.Drawing.Size(84, 19);
            this.chkShowCritical.TabIndex = 5;
            // 
            // chkShowRef
            // 
            this.chkShowRef.EditValue = true;
            this.chkShowRef.Location = new System.Drawing.Point(243, 6);
            this.chkShowRef.Name = "chkShowRef";
            this.chkShowRef.Properties.Caption = "显示参考值";
            this.chkShowRef.Size = new System.Drawing.Size(79, 19);
            this.chkShowRef.TabIndex = 4;
            // 
            // btnRefresh
            // 
            this.btnRefresh.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnRefresh.Location = new System.Drawing.Point(164, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(120, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "次结果";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "列出最近";
            // 
            // txtResultCount
            // 
            this.txtResultCount.EditValue = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.txtResultCount.Location = new System.Drawing.Point(61, 5);
            this.txtResultCount.Name = "txtResultCount";
            this.txtResultCount.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtResultCount.Properties.IsFloatValue = false;
            this.txtResultCount.Properties.Mask.EditMask = "N00";
            this.txtResultCount.Properties.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.txtResultCount.Size = new System.Drawing.Size(53, 20);
            this.txtResultCount.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnExportImg);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.btnExportResult);
            this.panelControl1.Controls.Add(this.txtResultCount);
            this.panelControl1.Controls.Add(this.chkShowThreshold);
            this.panelControl1.Controls.Add(this.label2);
            this.panelControl1.Controls.Add(this.chkShowCritical);
            this.panelControl1.Controls.Add(this.btnRefresh);
            this.panelControl1.Controls.Add(this.chkShowRef);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(742, 30);
            this.panelControl1.TabIndex = 10;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.chartControl1);
            this.splitContainerControl1.Panel1.Controls.Add(this.panelControl2);
            this.splitContainerControl1.Panel1.Controls.Add(this.panelControl1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.gridControl1);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(742, 452);
            this.splitContainerControl1.SplitterPosition = 237;
            this.splitContainerControl1.TabIndex = 12;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // chartControl1
            // 
            this.chartControl1.BorderOptions.Color = System.Drawing.Color.Transparent;
            this.chartControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartControl1.Legend.AlignmentHorizontal = DevExpress.XtraCharts.LegendAlignmentHorizontal.Right;
            this.chartControl1.Legend.AlignmentVertical = DevExpress.XtraCharts.LegendAlignmentVertical.TopOutside;
            this.chartControl1.Legend.Direction = DevExpress.XtraCharts.LegendDirection.LeftToRight;
            this.chartControl1.Location = new System.Drawing.Point(0, 55);
            this.chartControl1.Name = "chartControl1";
            this.chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.chartControl1.SeriesTemplate.View = lineSeriesView1;
            this.chartControl1.Size = new System.Drawing.Size(742, 182);
            this.chartControl1.TabIndex = 13;
            chartTitle1.Text = "";
            this.chartControl1.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.label6);
            this.panelControl2.Controls.Add(this.label3);
            this.panelControl2.Controls.Add(this.label5);
            this.panelControl2.Controls.Add(this.lbName);
            this.panelControl2.Controls.Add(this.label4);
            this.panelControl2.Controls.Add(this.lbAge);
            this.panelControl2.Controls.Add(this.lbItmEcd);
            this.panelControl2.Controls.Add(this.lbDateInterval);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 30);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(2);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(742, 25);
            this.panelControl2.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(285, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 14);
            this.label6.TabIndex = 7;
            this.label6.Text = "项目";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "姓名";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(424, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 14);
            this.label5.TabIndex = 6;
            this.label5.Text = "时间";
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Location = new System.Drawing.Point(58, 5);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(11, 14);
            this.lbName.TabIndex = 0;
            this.lbName.Text = ".";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(131, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 14);
            this.label4.TabIndex = 5;
            this.label4.Text = "年龄";
            // 
            // lbAge
            // 
            this.lbAge.AutoSize = true;
            this.lbAge.Location = new System.Drawing.Point(166, 5);
            this.lbAge.Name = "lbAge";
            this.lbAge.Size = new System.Drawing.Size(11, 14);
            this.lbAge.TabIndex = 1;
            this.lbAge.Text = ".";
            // 
            // lbItmEcd
            // 
            this.lbItmEcd.AutoSize = true;
            this.lbItmEcd.Location = new System.Drawing.Point(320, 5);
            this.lbItmEcd.Name = "lbItmEcd";
            this.lbItmEcd.Size = new System.Drawing.Size(11, 14);
            this.lbItmEcd.TabIndex = 2;
            this.lbItmEcd.Text = ".";
            // 
            // lbDateInterval
            // 
            this.lbDateInterval.AutoSize = true;
            this.lbDateInterval.Location = new System.Drawing.Point(458, 5);
            this.lbDateInterval.Name = "lbDateInterval";
            this.lbDateInterval.Size = new System.Drawing.Size(11, 14);
            this.lbDateInterval.TabIndex = 3;
            this.lbDateInterval.Text = ".";
            // 
            // PatHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.splitContainerControl1);
            this.Name = "PatHistory";
            this.Size = new System.Drawing.Size(742, 452);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkShowThreshold.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkShowCritical.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkShowRef.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtResultCount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.Utils.ToolTipController toolTipController1;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SpinEdit txtResultCount;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.SimpleButton btnRefresh;
        private DevExpress.XtraEditors.CheckEdit chkShowThreshold;
        private DevExpress.XtraEditors.CheckEdit chkShowCritical;
        private DevExpress.XtraEditors.CheckEdit chkShowRef;
        private DevExpress.XtraEditors.SimpleButton btnExportImg;
        private DevExpress.XtraEditors.SimpleButton btnExportResult;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private System.Windows.Forms.Label lbDateInterval;
        private System.Windows.Forms.Label lbItmEcd;
        private System.Windows.Forms.Label lbAge;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraCharts.ChartControl chartControl1;
    }
}
