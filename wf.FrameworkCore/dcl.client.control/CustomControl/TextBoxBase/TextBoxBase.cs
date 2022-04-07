using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
namespace dcl.client.control
{
    public class TextBoxBase : TextBox
    {
        #region Field

        private EMouseState _state = EMouseState.Normal;
        private Font _defaultFont = new Font("微软雅黑", 9);

        //当Text属性为空时编辑框内出现的提示文本
        private string _waterText;
        private Color _waterTextColor = Color.DarkGray;

        #endregion

        #region Constructor

        public TextBoxBase()
        {
            SetStyles();
            this.Font = _defaultFont;
            this.BorderStyle = BorderStyle.FixedSingle;
        }

        #endregion

        #region Properites

        [Description("当Text属性为空时编辑框内出现的提示文本")]
        public String WaterText
        {
            get { return _waterText; }
            set
            {
                if (_waterText != value)
                {
                    _waterText = value;
                    base.Invalidate();
                }
            }
        }

        [Description("获取或设置EmptyTextTip的颜色")]
        public Color WaterTextColor
        {
            get { return _waterTextColor; }
            set
            {
                if (_waterTextColor != value)
                {
                    _waterTextColor = value;
                    base.Invalidate();
                }
            }
        }

        #endregion

        #region Override

        protected override void OnMouseEnter(EventArgs e)
        {
            _state = EMouseState.Move;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (_state == EMouseState.Move && Focused)
            {
                _state = EMouseState.Up;
            }
            else if (_state == EMouseState.Up)
            {
                _state = EMouseState.Up;
            }
            else
            {
                _state = EMouseState.Normal;
            }
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
            {
                _state = EMouseState.Move;
            }
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
            {
                if (ClientRectangle.Contains(mevent.Location))
                {
                    _state = EMouseState.Move;
                }
                else
                {
                    _state = EMouseState.Up;
                }
            }
            base.OnMouseUp(mevent);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            _state = EMouseState.Normal;
            base.OnLostFocus(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (Enabled)
            {
                _state = EMouseState.Normal;
            }
            else
            {
                _state = EMouseState.Leave;
            }
            base.OnEnabledChanged(e);
        }


        public const int WM_PAINT = 0xF;
        public const int WM_CTLCOLOREDIT = 0x133;


        protected override void WndProc(ref Message m)
        {
            //TextBox是由系统进程绘制，重载OnPaint方法将不起作用
            base.WndProc(ref m);
            if (m.Msg == WM_PAINT || m.Msg == WM_CTLCOLOREDIT)
            {
                WmPaint(ref m);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_defaultFont != null)
                {
                    _defaultFont.Dispose();
                }
            }

            _defaultFont = null;
            base.Dispose(disposing);
        }

        #endregion

        #region Private

        private void SetStyles()
        {
            // TextBox由系统绘制，不能设置 ControlStyles.UserPaint样式
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
        }

        private void WmPaint(ref Message m)
        {
            Graphics g = Graphics.FromHwnd(base.Handle);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (!Enabled)
            {
                _state = EMouseState.Leave;
            }

            switch (_state)
            {
                case EMouseState.Normal:
                    DrawNormalTextBox(g);
                    break;
                case EMouseState.Move:
                    DrawHighLightTextBox(g);
                    break;
                case EMouseState.Up:
                    DrawFocusTextBox(g);
                    break;
                case EMouseState.Leave:
                    DrawDisabledTextBox(g);
                    break;
                default:
                    break;
            }

            if (Text.Length == 0 && !string.IsNullOrEmpty(WaterText) && !Focused)
            {
                TextRenderer.DrawText(g, WaterText, Font, ClientRectangle, WaterTextColor, GetTextFormatFlags(TextAlign, RightToLeft == RightToLeft.Yes));
            }
        }

        private void DrawNormalTextBox(Graphics g)
        {
            using (Pen borderPen = new Pen(Color.LightGray))
            {
                g.DrawRectangle(
                    borderPen,
                    new Rectangle(
                        ClientRectangle.X,
                        ClientRectangle.Y,
                        ClientRectangle.Width - 1,
                        ClientRectangle.Height - 1));
            }
        }

        private void DrawHighLightTextBox(Graphics g)
        {
            using (Pen highLightPen = new Pen(ColorTable.HighLightColor))
            {
                Rectangle drawRect = new Rectangle(
                        ClientRectangle.X,
                        ClientRectangle.Y,
                        ClientRectangle.Width - 1,
                        ClientRectangle.Height - 1);

                g.DrawRectangle(highLightPen, drawRect);

                //InnerRect
                drawRect.Inflate(-1,-1);
                highLightPen.Color = ColorTable.HighLightInnerColor;
                g.DrawRectangle(highLightPen, drawRect);
            }
        }

        private void DrawFocusTextBox(Graphics g)
        {
            using (Pen focusedBorderPen = new Pen(ColorTable.HighLightInnerColor))
            {
                g.DrawRectangle(
                    focusedBorderPen,
                    new Rectangle(
                        ClientRectangle.X,
                        ClientRectangle.Y,
                        ClientRectangle.Width - 1,
                        ClientRectangle.Height - 1));
            }
        }

        private void DrawDisabledTextBox(Graphics g)
        {
            using (Pen disabledPen = new Pen(SystemColors.ControlDark))
            {
                g.DrawRectangle(disabledPen,
                    new Rectangle(
                        ClientRectangle.X,
                        ClientRectangle.Y,
                        ClientRectangle.Width - 1,
                        ClientRectangle.Height - 1));
            }
        }

        private static TextFormatFlags GetTextFormatFlags
            (HorizontalAlignment alignment, bool rightToleft)
        {
            TextFormatFlags flags = TextFormatFlags.WordBreak |
                TextFormatFlags.SingleLine;
            if (rightToleft)
            {
                flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
            }

            switch (alignment)
            {
                case HorizontalAlignment.Center:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                    break;
                case HorizontalAlignment.Left:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                    break;
                case HorizontalAlignment.Right:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                    break;
            }
            return flags;
        }

        #endregion

        /// <summary>
        /// 鼠标状态
        /// </summary>
        public enum EMouseState
        {
            /// <summary>
            /// 默认,正常状态
            /// </summary>
            Normal,

            /// <summary>
            /// 鼠标划过
            /// </summary>
            Move,

            /// <summary>
            /// 鼠标按下
            /// </summary>
            /// 
            Down,

            /// <summary>
            /// 鼠标释放
            /// </summary>    
            Up,

            /// <summary>
            /// 鼠标离开
            /// </summary>
            Leave,
        }
    }
}
