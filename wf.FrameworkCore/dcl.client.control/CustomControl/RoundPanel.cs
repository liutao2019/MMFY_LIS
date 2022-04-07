using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Lis.CustomControls
{
    public partial class RoundPanel : Panel
    {
        private RoundStyle _roundeStyle;
        private const int WM_PAINT = 0xF;

        public RoundPanel() : base()   
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.UpdateStyles();

        }

        private Color _borderColor = Color.FromArgb(23, 169, 254);

        [Browsable(true)]          //显示到属性栏
        [DefaultValue(typeof(Color), "23, 169, 254"), Description("控件边框颜色")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                base.Invalidate();
            }
        }

        private Color _beginColor = Color.FromArgb(0, 122, 204);

        [Browsable(true)]          //显示到属性栏
        [DefaultValue(typeof(Color), "0, 122, 204"), Description("背景初始颜色")]
        public Color BeginColor
        {
            get { return _beginColor; }
            set
            {
                _beginColor = value;
                base.Invalidate();
            }
        }

        private Color _endColor = Color.FromArgb(8, 39, 57);

        [Browsable(true)]          //显示到属性栏
        [DefaultValue(typeof(Color), "8, 39, 57"), Description("背景结束颜色")]
        public Color EndColor
        {
            get { return _endColor; }
            set
            {
                _endColor = value;
                base.Invalidate();
            }
        }


        private string _innerText = string.Empty;

        [Browsable(true)] //显示到属性栏
        [DefaultValue(typeof(string), ""), Description("文本内容")]
        public string InnerText
        {
            get { return _innerText; }
            set
            {
                _innerText = value;
                base.Invalidate();
            }
        }

        private Font _innerTextFont = new Font("微软雅黑", 24, FontStyle.Regular);

        [Browsable(true)] //显示到属性栏
        public Font InnerTextFont
        {
            get { return _innerTextFont; }
            set
            {
                _innerTextFont = value;
                base.Invalidate();
            }
        }

        private Color _innerTextColor = Color.FromArgb(255, 255, 255);

        [Browsable(true)]    //显示到属性栏
        [DefaultValue(typeof(Color), "255, 255, 255"), Description("内容字体颜色")]
        public Color InnerTextColor
        {
            get { return _innerTextColor; }
            set
            {
                _innerTextColor = value;
                base.Invalidate();
            }
        }

        private bool _autoSetFont = false;
        [Browsable(true)]   //显示到属性栏
        [DefaultValue(false), Description("根据大小自动设定字体大小")]
        public bool AutoSetFont
        {
            get { return _autoSetFont; }
            set
            {
                _autoSetFont = value;
                base.Invalidate();
            }
        }


        private int _radius = 10;
        [Browsable(true)]          //显示到属性栏
        [DefaultValue(typeof(int), "10"), Description("圆角弧度大小")]
        public int Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                base.Invalidate();
            }
        }

        protected override void WndProc(ref Message m)
        {
            try
            {
                base.WndProc(ref m);
                if (m.Msg == WM_PAINT)
                {
                    if (this.Radius > 0)
                    {
                        using (Graphics g = Graphics.FromHwnd(this.Handle))
                        {
                            Rectangle r = new Rectangle();
                            r.Width = this.Width;
                            r.Height = this.Height;
                            Draw(r, g, this.Radius, false, this._beginColor, this._endColor);

                            Brush fontBrush = new SolidBrush(this._innerTextColor);
                            StringFormat sf = new StringFormat();
                            sf.Alignment = StringAlignment.Center;
                            sf.LineAlignment = StringAlignment.Center;

                            if (_autoSetFont && !string.IsNullOrEmpty(this._innerText))
                            {
                                Font sd = GetFont(this.Size, this._innerText, "微软雅黑");
                                g.DrawString(this._innerText, sd, fontBrush, this.ClientRectangle, sf);
                            }
                            else
                            {
                                g.DrawString(this._innerText, this._innerTextFont, fontBrush, this.ClientRectangle, sf);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 获取字体
        /// </summary>
        /// <param name="s">字符串容器Size</param>
        /// <param name="P_String">字符串内容</param>
        /// <param name="sFontName">字体名称</param>
        /// <returns></returns>
        private Font GetFont(Size s, string P_String, string sFontName)
        {
            Bitmap _bitmap = new Bitmap(s.Width, s.Height);
            Graphics _graphics = Graphics.FromImage(_bitmap);
            float fontsize = 0.1f;
            for (Size _size = new Size(); _size.Width < s.Width && _size.Height < s.Height; fontsize += 0.1f)
            {
                Font font_1 = new Font(sFontName, fontsize);
                _size = _graphics.MeasureString(P_String, font_1).ToSize();
            }
            return new Font(sFontName, fontsize - 0.2f, FontStyle.Regular);

        }

        private void Draw(Rectangle rectangle, Graphics g, int _radius, bool cusp, Color begin_color, Color end_color)
        {
            int span = 2;
            //抗锯齿
            g.SmoothingMode = SmoothingMode.AntiAlias;
            //渐变填充
            LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush(rectangle, begin_color, end_color, LinearGradientMode.Vertical);
            //画尖角
            if (cusp)
            {
                span = 10;
                PointF p1 = new PointF(rectangle.Width - 12, rectangle.Y + 10);
                PointF p2 = new PointF(rectangle.Width - 12, rectangle.Y + 30);
                PointF p3 = new PointF(rectangle.Width, rectangle.Y + 20);
                PointF[] ptsArray = { p1, p2, p3 };
                g.FillPolygon(myLinearGradientBrush, ptsArray);
            }
            //填充
            g.FillPath(myLinearGradientBrush, DrawRoundRect(rectangle.X, rectangle.Y, rectangle.Width - span, rectangle.Height - 1, _radius));
        }

        public static GraphicsPath DrawRoundRect(int x, int y, int width, int height, int radius)
        {
            //四边圆角
            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(x, y, radius, radius, 180, 90);
            gp.AddArc(width - radius, y, radius, radius, 270, 90);
            gp.AddArc(width - radius, height - radius, radius, radius, 0, 90);
            gp.AddArc(x, height - radius, radius, radius, 90, 90);
            gp.CloseAllFigures();
            return gp;
        }

        [Browsable(false)]          //显示到属性栏
        public RoundStyle RoundeStyle
        {
            get { return _roundeStyle; }
            set
            {
                _roundeStyle = value;
                base.Invalidate();
            }
        }

        /// <summary>
        /// 建立带有圆角样式的路径。
        /// </summary>
        /// <param name="rect">用来建立路径的矩形。</param>
        /// <param name="_radius">圆角的大小。</param>
        /// <param name="style">圆角的样式。</param>
        /// <param name="correction">是否把矩形长宽减 1,以便画出边框。</param>
        /// <returns>建立的路径。</returns>
        GraphicsPath CreatePath(Rectangle rect, int radius, RoundStyle style, bool correction)
        {
            GraphicsPath path = new GraphicsPath();
            int radiusCorrection = correction ? 1 : 0;
            switch (style)
            {
                case RoundStyle.None:
                    path.AddRectangle(rect);
                    break;
                case RoundStyle.All:
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    path.AddArc(rect.Right - radius - radiusCorrection, rect.Y, radius, radius, 270, 90);
                    path.AddArc(rect.Right - radius - radiusCorrection, rect.Bottom - radius - radiusCorrection, radius, radius, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - radius - radiusCorrection, radius, radius, 90, 90);
                    break;
                case RoundStyle.Left:
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    path.AddLine(rect.Right - radiusCorrection, rect.Y, rect.Right - radiusCorrection, rect.Bottom - radiusCorrection);
                    path.AddArc(rect.X, rect.Bottom - radius - radiusCorrection, radius, radius, 90, 90);
                    break;
                case RoundStyle.Right:
                    path.AddArc(rect.Right - radius - radiusCorrection, rect.Y, radius, radius, 270, 90);
                    path.AddArc(rect.Right - radius - radiusCorrection, rect.Bottom - radius - radiusCorrection, radius, radius, 0, 90);
                    path.AddLine(rect.X, rect.Bottom - radiusCorrection, rect.X, rect.Y);
                    break;
                case RoundStyle.Top:
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    path.AddArc(rect.Right - radius - radiusCorrection, rect.Y, radius, radius, 270, 90);
                    path.AddLine(rect.Right - radiusCorrection, rect.Bottom - radiusCorrection, rect.X, rect.Bottom - radiusCorrection);
                    break;
                case RoundStyle.Bottom:
                    path.AddArc(rect.Right - radius - radiusCorrection, rect.Bottom - radius - radiusCorrection, radius, radius, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - radius - radiusCorrection, radius, radius, 90, 90);
                    path.AddLine(rect.X, rect.Y, rect.Right - radiusCorrection, rect.Y);
                    break;
            }
            path.CloseFigure(); //这句很关键，缺少会没有左边线。
            return path;
        }

        private void DrawBorder(Graphics g, Rectangle rect, RoundStyle roundStyle, int radius)
        {
            rect.Width -= 1;
            rect.Height -= 1;
            using (GraphicsPath path = CreatePath(rect, radius, roundStyle, false))
            {
                using (Pen pen = new Pen(this.BorderColor))
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        public enum RoundStyle
        {
            None = 0,
            All = 1,
            Left = 2,
            Right = 3,
            Top = 4,
            Bottom = 5,
            BottomLeft = 6,
            BottomRight = 7
        }
    }
}
