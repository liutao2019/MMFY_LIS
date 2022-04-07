using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraCharts;
using dcl.entity;
using dcl.client.wcf;

namespace dcl.client.tools
{
    public partial class ConTempLine : UserControl
    {

        #region 全局变量
        ChartTitle title = new ChartTitle();
        List<EntityTemHarvester> listTem;
        public String DhId;
        #endregion

        public void LoadData(List<EntityTemHarvester> listTem)
        {
            if (listTem != null && listTem.Count > 0)
            {
                chartControl1.Series.Clear();
                chartControl1.Titles.Clear();
                Series series = new Series();
                title.Text = string.Format("下图为设备{0}的温度曲线", listTem[0].DhName);
                chartControl1.Titles.Add(title);

                series.View = new LineSeriesView();
                series.View.Color = Color.Green;

                foreach (EntityTemHarvester temHar in listTem)
                {
                    SeriesPoint point = new SeriesPoint(temHar.ThDate.ToString("yyMMdd HH:mm"), new double[] { Convert.ToDouble(temHar.ThTemperature) });

                    series.Points.Add(new SeriesPoint(temHar.ThDate.ToString("yyMMdd HH:mm"), new double[] { Convert.ToDouble(temHar.ThTemperature) }));
                }
                chartControl1.Series.Add(series);

                #region 画出温度上下限的直线
                DevExpress.XtraCharts.XYDiagram gg = this.chartControl1.Diagram as DevExpress.XtraCharts.XYDiagram;

                gg.AxisY.ConstantLines.Clear();//清空Y轴数值

                DevExpress.XtraCharts.ConstantLine cl = new DevExpress.XtraCharts.ConstantLine();
                cl.ShowInLegend = false;
                cl.Title.Alignment = ConstantLineTitleAlignment.Far;
                cl.Color = Color.Red;
                cl.Name = string.Format("温度上限{0}°C", listTem[0].DtHLimit);
                cl.AxisValue = listTem[0].DtHLimit;

                DevExpress.XtraCharts.ConstantLine c2 = new DevExpress.XtraCharts.ConstantLine();
                c2.ShowInLegend = false;
                c2.Title.Alignment = ConstantLineTitleAlignment.Far;
                c2.Color = Color.Blue;
                c2.Name = string.Format("温度下限{0}°C", listTem[0].DtLLimit);
                c2.AxisValue = listTem[0].DtLLimit;

                gg.AxisY.ConstantLines.AddRange(
                        new ConstantLine[] { cl, c2 });
                #endregion

                chartControl1.Show();
            }
        }

        public ConTempLine(string dh_id)
        {
            this.DhId = dh_id;

        }
        public ConTempLine()
        {
            InitializeComponent();
        }

        private void ConTempLine_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            this.listTem = new ProxyTempHandle().Service.GetHarvesterByDhId(this.DhId);
            LoadData(this.listTem);
        }
    }
}
