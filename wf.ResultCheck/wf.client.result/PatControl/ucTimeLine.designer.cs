namespace dcl.client.result
{
    partial class ucTimeLine
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraCharts.TextAnnotation textAnnotation1 = new DevExpress.XtraCharts.TextAnnotation();
            DevExpress.XtraCharts.SeriesPointAnchorPoint seriesPointAnchorPoint1 = new DevExpress.XtraCharts.SeriesPointAnchorPoint();
            DevExpress.XtraCharts.RelativePosition relativePosition1 = new DevExpress.XtraCharts.RelativePosition();
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel1 = new DevExpress.XtraCharts.PointSeriesLabel();
            DevExpress.XtraCharts.SeriesPoint seriesPoint1 = new DevExpress.XtraCharts.SeriesPoint(new System.DateTime(2017, 5, 12, 0, 0, 0, 0), new object[] {
            ((object)(0D))}, 0);
            DevExpress.XtraCharts.SeriesPoint seriesPoint2 = new DevExpress.XtraCharts.SeriesPoint(new System.DateTime(2017, 5, 13, 0, 0, 0, 0), new object[] {
            ((object)(0D))});
            DevExpress.XtraCharts.SeriesPoint seriesPoint3 = new DevExpress.XtraCharts.SeriesPoint(new System.DateTime(2017, 5, 14, 0, 0, 0, 0), new object[] {
            ((object)(0D))});
            DevExpress.XtraCharts.SeriesPoint seriesPoint4 = new DevExpress.XtraCharts.SeriesPoint(new System.DateTime(2017, 5, 15, 0, 0, 0, 0), new object[] {
            ((object)(0D))});
            DevExpress.XtraCharts.StepLineSeriesView stepLineSeriesView1 = new DevExpress.XtraCharts.StepLineSeriesView();
            this.chartControl2 = new DevExpress.XtraCharts.ChartControl();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(textAnnotation1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(seriesPointAnchorPoint1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(stepLineSeriesView1)).BeginInit();
            this.SuspendLayout();
            // 
            // chartControl2
            // 
            seriesPointAnchorPoint1.SeriesID = 0;
            seriesPointAnchorPoint1.SeriesPointID = 0;
            textAnnotation1.AnchorPoint = seriesPointAnchorPoint1;
            textAnnotation1.AutoSize = false;
            textAnnotation1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            textAnnotation1.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;
            textAnnotation1.Height = 37;
            textAnnotation1.Name = "Text Annotation 1";
            textAnnotation1.RuntimeMoving = true;
            textAnnotation1.RuntimeResizing = true;
            relativePosition1.Angle = 90D;
            relativePosition1.ConnectorLength = 30D;
            textAnnotation1.ShapePosition = relativePosition1;
            textAnnotation1.Text = "标本采集\r\n2017-11-11 ";
            textAnnotation1.TextColor = System.Drawing.Color.Yellow;
            textAnnotation1.Width = 78;
            textAnnotation1.ZOrder = 1;
            this.chartControl2.AnnotationRepository.AddRange(new DevExpress.XtraCharts.Annotation[] {
            textAnnotation1});
            this.chartControl2.BorderOptions.Visibility = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.Default;
            xyDiagram1.AxisX.Visibility = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisX.VisualRange.Auto = false;
            xyDiagram1.AxisX.VisualRange.MaxValueSerializable = "05/15/2017 00:00:00.000";
            xyDiagram1.AxisX.VisualRange.MinValueSerializable = "05/12/2017 00:00:00.000";
            xyDiagram1.AxisY.Visibility = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram1.DefaultPane.BorderVisible = false;
            xyDiagram1.PaneDistance = 100;
            this.chartControl2.Diagram = xyDiagram1;
            this.chartControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartControl2.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            this.chartControl2.Location = new System.Drawing.Point(0, 0);
            this.chartControl2.Name = "chartControl2";
            pointSeriesLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            pointSeriesLabel1.Border.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            pointSeriesLabel1.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            pointSeriesLabel1.LineLength = 20;
            pointSeriesLabel1.LineStyle.LineJoin = System.Drawing.Drawing2D.LineJoin.MiterClipped;
            pointSeriesLabel1.LineVisibility = DevExpress.Utils.DefaultBoolean.True;
            pointSeriesLabel1.TextPattern = "{A}";
            series1.Label = pointSeriesLabel1;
            series1.Name = "Series 1";
            series1.Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] {
            seriesPoint1,
            seriesPoint2,
            seriesPoint3,
            seriesPoint4});
            series1.SeriesID = 0;
            series1.ShowInLegend = false;
            series1.ToolTipEnabled = DevExpress.Utils.DefaultBoolean.True;
            stepLineSeriesView1.MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            series1.View = stepLineSeriesView1;
            this.chartControl2.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.chartControl2.Size = new System.Drawing.Size(325, 150);
            this.chartControl2.TabIndex = 36;
            // 
            // ucTimeLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chartControl2);
            this.Name = "ucTimeLine";
            this.Size = new System.Drawing.Size(325, 150);
            this.Load += new System.EventHandler(this.ucTimeLine_Load);
            ((System.ComponentModel.ISupportInitialize)(seriesPointAnchorPoint1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(textAnnotation1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(stepLineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraCharts.ChartControl chartControl2;
    }
}
