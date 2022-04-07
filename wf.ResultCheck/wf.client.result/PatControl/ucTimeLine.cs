using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraCharts;
using dcl.entity;

namespace dcl.client.result
{
    public partial class ucTimeLine : UserControl
    {
        public ucTimeLine()
        {
            InitializeComponent();
        }

        private void ucTimeLine_Load(object sender, EventArgs e)
        {
            chartControl2.Series.Clear();
        }

        public void Reset()
        {
            chartControl2.Series.Clear();
        }

        public void LoadData(Dictionary<string,string> dic)
        {
            StepLineSeriesView stepLineSeriesView2 = new DevExpress.XtraCharts.StepLineSeriesView();
            chartControl2.Series.Clear();
            Series serie = new Series();
            stepLineSeriesView2.MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            serie.View = stepLineSeriesView2;

            foreach (var key in dic.Keys)
            {
                SeriesPoint sp = new SeriesPoint(Convert.ToDateTime(key).ToString("MM-dd HH:mm:ss"), new object[] {
                ((object)(0D))});
                sp.Tag = dic[key];
                serie.Points.Add(sp);
            }
            chartControl2.Series.Add(serie);
          
            foreach(SeriesPoint point in serie.Points)
            {
                string tag = point.Tag.ToString();

                TextAnnotation textAnnotation2 = new TextAnnotation();
                RelativePosition freePosition2 = new RelativePosition();
                SeriesPointAnchorPoint seriesPointAnchorPoint2 = new SeriesPointAnchorPoint();
                seriesPointAnchorPoint2.SeriesPoint = point;
                textAnnotation2.AnchorPoint = seriesPointAnchorPoint2;
                textAnnotation2.BackColor = System.Drawing.SystemColors.Info;
                textAnnotation2.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;
                textAnnotation2.Name = tag.Split('|')[0];
                textAnnotation2.RuntimeMoving = true;
                textAnnotation2.RuntimeResizing = true;
                freePosition2.Angle = 270D;
                freePosition2.ConnectorLength = 28;
                textAnnotation2.ShapePosition = freePosition2;
                textAnnotation2.Text = tag.Split('|')[0]+"\r\n"+ point.Argument;
                //textAnnotation2.TextColor = System.Drawing.Color.Yellow;
                textAnnotation2.ZOrder = 1;

                if (tag.Split('|').Length == 2)
                {
                    TextAnnotation textAnnotation21 = new TextAnnotation();
                    RelativePosition freePosition21 = new RelativePosition();
                    SeriesPointAnchorPoint seriesPointAnchorPoint21 = new SeriesPointAnchorPoint();
                    seriesPointAnchorPoint21.SeriesPoint = point;
                    textAnnotation21.AnchorPoint = seriesPointAnchorPoint21;
                    textAnnotation21.BackColor = System.Drawing.SystemColors.Info;
                    textAnnotation21.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;
                    textAnnotation21.Name = tag.Split('|')[1];
                    textAnnotation21.RuntimeMoving = true;
                    textAnnotation21.RuntimeResizing = true;
                    freePosition21.Angle = 150D;
                    freePosition21.ConnectorLength = 45;
                    textAnnotation21.ShapePosition = freePosition21;
                    textAnnotation21.Text = tag.Split('|')[1] ;
                    //textAnnotation2.TextColor = System.Drawing.Color.Yellow;
                    textAnnotation21.ZOrder = 1;

                    this.chartControl2.AnnotationRepository.AddRange(new DevExpress.XtraCharts.Annotation[] {
            textAnnotation2,textAnnotation21});
                }
                else
                {
                    this.chartControl2.AnnotationRepository.AddRange(new DevExpress.XtraCharts.Annotation[] {
            textAnnotation2});
                }


            }
        }

        public void LoadData(List<EntityTimeLineParameters> listPar)
        {
            StepLineSeriesView stepLineSeriesView2 = new DevExpress.XtraCharts.StepLineSeriesView();
            chartControl2.Series.Clear();
            Series serie = new Series();
            stepLineSeriesView2.MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            serie.View = stepLineSeriesView2;

            foreach (EntityTimeLineParameters par in listPar)
            {
                SeriesPoint sp = new SeriesPoint(Convert.ToDateTime(par.DateTime).ToString("MM-dd HH:mm:ss"), new object[] {
                ((object)(0D))});
                sp.Tag = par.DateName;
                serie.Points.Add(sp);
            }
            chartControl2.Series.Add(serie);

            foreach (SeriesPoint point in serie.Points)
            {
                string tag = point.Tag.ToString();
                List<EntityTimeLineParameters> list = listPar.FindAll(w=>w.DateName==tag);
                bool overTime = false;
                if (list.Count > 0)
                {
                    overTime =Convert.ToBoolean(list[0].OverTime);
                }
                TextAnnotation textAnnotation2 = new TextAnnotation();
                RelativePosition freePosition2 = new RelativePosition();
                SeriesPointAnchorPoint seriesPointAnchorPoint2 = new SeriesPointAnchorPoint();
                seriesPointAnchorPoint2.SeriesPoint = point;
                textAnnotation2.AnchorPoint = seriesPointAnchorPoint2;
                if (overTime)
                {
                    textAnnotation2.BackColor = Color.Red;
                }
                else
                {
                    textAnnotation2.BackColor = System.Drawing.SystemColors.Info;
                }
                textAnnotation2.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;
                textAnnotation2.Name = tag.Split('|')[0];
                textAnnotation2.RuntimeMoving = true;
                textAnnotation2.RuntimeResizing = true;
                freePosition2.Angle = 270D;
                freePosition2.ConnectorLength = 28;
                textAnnotation2.ShapePosition = freePosition2;
                textAnnotation2.Text = tag.Split('|')[0] + "\r\n" + point.Argument;
                //textAnnotation2.TextColor = System.Drawing.Color.Yellow;
                textAnnotation2.ZOrder = 1;

                if (tag.Split('|').Length == 2)
                {
                    TextAnnotation textAnnotation21 = new TextAnnotation();
                    RelativePosition freePosition21 = new RelativePosition();
                    SeriesPointAnchorPoint seriesPointAnchorPoint21 = new SeriesPointAnchorPoint();
                    seriesPointAnchorPoint21.SeriesPoint = point;
                    textAnnotation21.AnchorPoint = seriesPointAnchorPoint21;
                    if (overTime)
                    {
                        textAnnotation21.BackColor = Color.Red;
                    }
                    else
                    {
                        textAnnotation21.BackColor = System.Drawing.SystemColors.Info;
                    }
                    textAnnotation21.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;
                    textAnnotation21.Name = tag.Split('|')[1];
                    textAnnotation21.RuntimeMoving = true;
                    textAnnotation21.RuntimeResizing = true;
                    freePosition21.Angle = 150D;
                    freePosition21.ConnectorLength = 45;
                    textAnnotation21.ShapePosition = freePosition21;
                    textAnnotation21.Text = tag.Split('|')[1];
                    //textAnnotation2.TextColor = System.Drawing.Color.Yellow;
                    textAnnotation21.ZOrder = 1;

                    this.chartControl2.AnnotationRepository.AddRange(new DevExpress.XtraCharts.Annotation[] {
            textAnnotation2,textAnnotation21});
                }
                else
                {
                    this.chartControl2.AnnotationRepository.AddRange(new DevExpress.XtraCharts.Annotation[] {
            textAnnotation2});
                }


            }
        }
    }
}
