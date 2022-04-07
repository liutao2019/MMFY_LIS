using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGauges.Core.Model;
using DevExpress.XtraGauges.Core.Drawing;
using DevExpress.XtraGauges.Win.Gauges.Circular;
using dcl.entity;
using lis.client.control;

namespace dcl.client.tools
{
    public partial class ConGauge : UserControl
    {
        public event EventHandler AfterClicked;
        public event EventHandler MouseEntered;
        public EntityTemHarvester tempHarvester;
        public ConGauge()
        {
            InitializeComponent();
            this.Load += ConGauge_Load;
        }

        private void ConGauge_Load(object sender, EventArgs e)
        {
            scTemp.EnableAnimation = true;
            scTemp.EasingMode = EasingMode.EaseInOut;
            scTemp.EasingFunction = new CubicEase();
            scTemp.AutoRescaling = false;
            scHumidity.EnableAnimation = true;
            scHumidity.EasingMode = EasingMode.EaseInOut;
            scHumidity.EasingFunction = new CubicEase();

            rangeBar1.Appearance.ContentBrush = new SolidBrushObject(Color.Gray);
        }

        public void LoadData()
        {
            float humi;
            float temp;

            //温度
            if (float.TryParse(tempHarvester.ThTemperature, out temp))
            {
                scTemp.Value = temp;
            }
            else
            {
                //写入日志 转换失败
            }

            //湿度
            if (float.TryParse(tempHarvester.ThHumidity, out humi))
            {
                scTemp.Value = humi;
            }
            else
            {
                //写入日志 转换失败
            }

            //判断是否在字典的温度范围
            if (temp != 0F && humi != 0F)
            {
                Color textColor;
                string tempColor = string.Empty;
                string humiColor = string.Empty;

                #region 根据温度选择颜色
                //float hLimit = Convert.ToSingle(tempHarvester.DtHLimit);
                //float lLimit = Convert.ToSingle(tempHarvester.DtLLimit);

                //if (temp > lLimit && temp < hLimit)
                //{
                //    textColor = PrimaryColor;
                //}
                //else if (temp > hLimit)
                //{
                //    textColor = HotColor;
                //}
                //else
                //{
                //    textColor = ColdColor;
                //}
                if (temp > 0)
                {
                    textColor = Color.Red;

                }
                else
                {
                    textColor = Color.Blue;
                }
                #endregion

                tempColor = string.Format("{0},{1},{2}", textColor.R, textColor.G, textColor.B);
                humiColor = string.Format("{0},{1},{2}", Color.Gray.R, Color.Gray.G, Color.Gray.B);
                rangeBar2.Appearance.ContentBrush = new SolidBrushObject(textColor);
                #region 显示温度湿度信息

                #region 设置最小温度，最大温度

                #endregion
                scTemp.Value = temp;
                scHumidity.Value = humi;
                lblName.Text = tempHarvester.DhName + " " + tempHarvester.ProName;
                lblInfo.Text = string.Format("<color={0}>温度:{1}°C<br><color={2}>湿度:{3}%", tempColor, temp, humiColor, humi);

                #endregion

            }
        }

        private void ConGauge_Resize(object sender, EventArgs e)
        {
            Rectangle rectangle = new Rectangle(circularGauge1.Bounds.X, circularGauge1.Bounds.Y, this.Width, this.Height - 10);
            circularGauge1.Bounds = rectangle;
        }

        private void gaugeControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (AfterClicked != null)
                AfterClicked(sender, e);
        }

        private void gaugeControl1_MouseEnter(object sender, EventArgs e)
        {
            if (MouseEntered != null)
                MouseEntered(sender, e);
        }
    }
}
