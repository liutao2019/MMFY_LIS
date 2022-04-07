namespace dcl.client.tools
{
    partial class ConGauge
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
            DevExpress.XtraGauges.Core.Model.ArcScaleRange arcScaleRange5 = new DevExpress.XtraGauges.Core.Model.ArcScaleRange();
            DevExpress.XtraGauges.Core.Model.ArcScaleRange arcScaleRange6 = new DevExpress.XtraGauges.Core.Model.ArcScaleRange();
            this.gaugeControl1 = new DevExpress.XtraGauges.Win.GaugeControl();
            this.circularGauge1 = new DevExpress.XtraGauges.Win.Gauges.Circular.CircularGauge();
            this.lblInfo = new DevExpress.XtraGauges.Win.Base.LabelComponent();
            this.lblName = new DevExpress.XtraGauges.Win.Base.LabelComponent();
            this.rangeBar1 = new DevExpress.XtraGauges.Win.Gauges.Circular.ArcScaleRangeBarComponent();
            this.scHumidity = new DevExpress.XtraGauges.Win.Gauges.Circular.ArcScaleComponent();
            this.rangeBar2 = new DevExpress.XtraGauges.Win.Gauges.Circular.ArcScaleRangeBarComponent();
            this.scTemp = new DevExpress.XtraGauges.Win.Gauges.Circular.ArcScaleComponent();
            ((System.ComponentModel.ISupportInitialize)(this.circularGauge1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scHumidity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scTemp)).BeginInit();
            this.SuspendLayout();
            // 
            // gaugeControl1
            // 
            this.gaugeControl1.AutoLayout = false;
            this.gaugeControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gaugeControl1.Gauges.AddRange(new DevExpress.XtraGauges.Base.IGauge[] {
            this.circularGauge1});
            this.gaugeControl1.Location = new System.Drawing.Point(0, 0);
            this.gaugeControl1.Name = "gaugeControl1";
            this.gaugeControl1.Size = new System.Drawing.Size(129, 122);
            this.gaugeControl1.TabIndex = 0;
            this.gaugeControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gaugeControl1_MouseClick);
            this.gaugeControl1.MouseEnter += new System.EventHandler(this.gaugeControl1_MouseEnter);
            // 
            // circularGauge1
            // 
            this.circularGauge1.AutoSize = DevExpress.Utils.DefaultBoolean.True;
            this.circularGauge1.Bounds = new System.Drawing.Rectangle(4, 4, 120, 110);
            this.circularGauge1.Labels.AddRange(new DevExpress.XtraGauges.Win.Base.LabelComponent[] {
            this.lblInfo,
            this.lblName});
            this.circularGauge1.Name = "circularGauge1";
            this.circularGauge1.RangeBars.AddRange(new DevExpress.XtraGauges.Win.Gauges.Circular.ArcScaleRangeBarComponent[] {
            this.rangeBar1,
            this.rangeBar2});
            this.circularGauge1.Scales.AddRange(new DevExpress.XtraGauges.Win.Gauges.Circular.ArcScaleComponent[] {
            this.scHumidity,
            this.scTemp});
            // 
            // lblInfo
            // 
            this.lblInfo.AllowHTMLString = true;
            this.lblInfo.AppearanceText.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lblInfo.Name = "circularGauge1_Label1";
            this.lblInfo.Size = new System.Drawing.SizeF(110F, 70F);
            this.lblInfo.Text = "";
            this.lblInfo.ZOrder = -1001;
            // 
            // lblName
            // 
            this.lblName.AppearanceText.Font = new System.Drawing.Font("Segoe UI Semibold", 15F, System.Drawing.FontStyle.Bold);
            this.lblName.Name = "lblName";
            this.lblName.Position = new DevExpress.XtraGauges.Core.Base.PointF2D(125F, 240F);
            this.lblName.Size = new System.Drawing.SizeF(250F, 25F);
            // 
            // rangeBar1
            // 
            this.rangeBar1.ArcScale = this.scHumidity;
            this.rangeBar1.Name = "circularGauge1_RangeBar2";
            this.rangeBar1.ShowBackground = true;
            this.rangeBar1.StartOffset = 80F;
            this.rangeBar1.ZOrder = -10;
            // 
            // scHumidity
            // 
            this.scHumidity.AppearanceMajorTickmark.BorderBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:White");
            this.scHumidity.AppearanceMajorTickmark.ContentBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:White");
            this.scHumidity.AppearanceMinorTickmark.BorderBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:White");
            this.scHumidity.AppearanceMinorTickmark.ContentBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:White");
            this.scHumidity.AppearanceTickmarkText.Font = new System.Drawing.Font("Tahoma", 8.5F);
            this.scHumidity.AppearanceTickmarkText.TextBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:#484E5A");
            this.scHumidity.Center = new DevExpress.XtraGauges.Core.Base.PointF2D(125F, 125F);
            this.scHumidity.EndAngle = 90F;
            this.scHumidity.MajorTickCount = 0;
            this.scHumidity.MajorTickmark.FormatString = "{0:F0}";
            this.scHumidity.MajorTickmark.ShapeOffset = -14F;
            this.scHumidity.MajorTickmark.TextOrientation = DevExpress.XtraGauges.Core.Model.LabelOrientation.LeftToRight;
            this.scHumidity.MaxValue = 100F;
            this.scHumidity.MinorTickCount = 0;
            this.scHumidity.MinorTickmark.ShapeOffset = -7F;
            this.scHumidity.MinorTickmark.ShapeType = DevExpress.XtraGauges.Core.Model.TickmarkShapeType.Circular_Style16_2;
            this.scHumidity.MinorTickmark.ShowTick = false;
            this.scHumidity.Name = "scale1";
            this.scHumidity.StartAngle = -270F;
            this.scHumidity.Value = 20F;
            // 
            // rangeBar2
            // 
            this.rangeBar2.ArcScale = this.scTemp;
            this.rangeBar2.EndOffset = 35F;
            this.rangeBar2.Name = "circularGauge1_RangeBar2Copy0";
            this.rangeBar2.ShowBackground = true;
            this.rangeBar2.StartOffset = 75F;
            this.rangeBar2.ZOrder = -10;
            // 
            // scTemp
            // 
            this.scTemp.AppearanceMajorTickmark.BorderBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:White");
            this.scTemp.AppearanceMajorTickmark.ContentBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:White");
            this.scTemp.AppearanceMinorTickmark.BorderBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:White");
            this.scTemp.AppearanceMinorTickmark.ContentBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:White");
            this.scTemp.AppearanceTickmarkText.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Bold);
            this.scTemp.AppearanceTickmarkText.TextBrush = new DevExpress.XtraGauges.Core.Drawing.SolidBrushObject("Color:#484E5A");
            this.scTemp.Center = new DevExpress.XtraGauges.Core.Base.PointF2D(125F, 125F);
            this.scTemp.EndAngle = -45F;
            this.scTemp.MajorTickCount = 0;
            this.scTemp.MajorTickmark.FormatString = "{0:F0}";
            this.scTemp.MajorTickmark.ShowTick = false;
            this.scTemp.MajorTickmark.TextOrientation = DevExpress.XtraGauges.Core.Model.LabelOrientation.LeftToRight;
            this.scTemp.MaxValue = 30F;
            this.scTemp.MinorTickCount = 0;
            this.scTemp.MinorTickmark.ShapeOffset = -7F;
            this.scTemp.MinorTickmark.ShapeType = DevExpress.XtraGauges.Core.Model.TickmarkShapeType.Circular_Style16_2;
            this.scTemp.MinValue = -30F;
            this.scTemp.Name = "scale1Copy0";
            arcScaleRange5.EndValue = 0F;
            arcScaleRange5.Name = "Range0";
            arcScaleRange5.StartValue = -30F;
            arcScaleRange6.EndValue = 50F;
            arcScaleRange6.Name = "Range1";
            this.scTemp.Ranges.AddRange(new DevExpress.XtraGauges.Core.Model.IRange[] {
            arcScaleRange5,
            arcScaleRange6});
            this.scTemp.StartAngle = -270F;
            // 
            // ConGauge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.gaugeControl1);
            this.Name = "ConGauge";
            this.Size = new System.Drawing.Size(129, 122);
            this.Resize += new System.EventHandler(this.ConGauge_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.circularGauge1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scHumidity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rangeBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scTemp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGauges.Win.GaugeControl gaugeControl1;
        private DevExpress.XtraGauges.Win.Gauges.Circular.CircularGauge circularGauge1;
        private DevExpress.XtraGauges.Win.Base.LabelComponent lblInfo;
        private DevExpress.XtraGauges.Win.Gauges.Circular.ArcScaleRangeBarComponent rangeBar1;
        private DevExpress.XtraGauges.Win.Gauges.Circular.ArcScaleComponent scHumidity;
        private DevExpress.XtraGauges.Win.Gauges.Circular.ArcScaleRangeBarComponent rangeBar2;
        private DevExpress.XtraGauges.Win.Gauges.Circular.ArcScaleComponent scTemp;
        private DevExpress.XtraGauges.Win.Base.LabelComponent lblName;
    }
}
