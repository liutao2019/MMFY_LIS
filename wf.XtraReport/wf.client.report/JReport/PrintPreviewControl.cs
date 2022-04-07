using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;

namespace dcl.client.report
{
    /// <summary>
    /// 
    /// </summary>
    [ComVisible(true)]
    public sealed class PrintPreviewControl : Control
    {
        #region 私有对象

        private PreviewPageInfo[] pageInfo;
        private bool _havewheel;
        private Point position;
        private bool antiAlias;
        private bool autoZoom;
        //private const int border = 10;
        private int columns;
        //private const double DefaultZoom = 1;
        private PrintDocument document;
        private bool exceptionPrinting;
        private Size imageSize;
        //private Point lastOffset;
        private bool layoutOk;
        private bool pageInfoCalcPending;
        private int rows;
        private Point screendpi;

        public Point Screendpi
        {
            get { return screendpi; }
        }

        public int FirstLeft = 0;
        public int FirstTop = 0;
        //private const int SCROLL_LINE = 5;
        //private const int SCROLL_PAGE = 100;
        private int startPage;
        private Size virtualSize;
        private double zoom;
        private static readonly ContentAlignment anyRight, anyCenter, anyBottom, anyMiddle;

        #endregion

        #region 构造器

        static PrintPreviewControl()
        {
            anyRight = ContentAlignment.BottomRight | ContentAlignment.MiddleRight | ContentAlignment.TopRight;
            anyCenter = ContentAlignment.BottomCenter | ContentAlignment.MiddleCenter | ContentAlignment.TopCenter;
            anyBottom = ContentAlignment.BottomRight | ContentAlignment.BottomCenter | ContentAlignment.BottomLeft;
            anyMiddle = ContentAlignment.MiddleRight | ContentAlignment.MiddleCenter | ContentAlignment.MiddleLeft;
        }

        /// <summary>
        /// 初始化 <b>PrintPreviewControl</b> 类的新实例。
        /// </summary>
        public PrintPreviewControl()
        {
            // 检查用户是否安装了带滚轮的鼠标。
            _havewheel = SystemInformation.MouseWheelPresent;
            position = new Point(0, 0);
            virtualSize = new Size(1, 1);
            rows = 1;
            columns = 1;
            autoZoom = true;
            imageSize = Size.Empty;
            screendpi = Point.Empty;
            zoom = 0.3;
            ResetBackColor();
            ResetForeColor();
            base.Size = new Size(100, 100);
            base.SetStyle(ControlStyles.ResizeRedraw, false);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.Opaque, true);
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取打印预览时页面的数目。
        /// </summary>
        [Browsable(false)]
        public int PageCount
        {
            get { return pageInfo.Length; }
        }

        /// <summary>
        /// 
        /// </summary>
        private Point Position
        {
            get { return position; }
            set { SetPositionNoInvalidate(value); }
        }

        private Size VirtualSize
        {
            get { return virtualSize; }
            set
            {
                SetVirtualSizeNoInvalidate(value);
                base.Invalidate();
            }
        }

        public bool AutoZoom
        {
            get { return autoZoom; }
            set
            {
                if (autoZoom != value)
                {
                    autoZoom = value;
                    InvalidateLayout();
                }
            }
        }

        public int Columns
        {
            get { return columns; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("Columns", "预览页面列数不能小于1。");
                columns = value;
                InvalidateLayout();
            }
        }

        protected override CreateParams CreateParams
        {
            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            get
            {
                CreateParams params1 = base.CreateParams;
                params1.Style |= 0x100000;
                params1.Style |= 0x200000;
                return params1;
            }
        }

        public PrintDocument Document
        {
            get { return document; }
            set { document = value; }
        }

        public override RightToLeft RightToLeft
        {
            get { return base.RightToLeft; }
            set
            {
                base.RightToLeft = value;
                InvalidatePreview();
            }
        }

        public int Rows
        {
            get { return rows; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("Rows", "预览的纵向页数不能小于1。");
                rows = value;
                InvalidateLayout();
            }
        }

        public int StartPage
        {
            get
            {
                int num1 = startPage;
                if (pageInfo != null)
                    num1 = Math.Min(num1, pageInfo.Length - (rows * columns));
                return Math.Max(num1, 0);
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("StartPage", "起始页数不能小于零。");
                int s = StartPage;
                startPage = value;
                if (s != startPage)
                    InvalidateLayout();
            }
        }

        public bool UseAntiAlias
        {
            get { return antiAlias; }
            set { antiAlias = value; }
        }

        public double Zoom
        {
            get { return zoom; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("打印预览的显示大小不能小于0");
                autoZoom = false;
                zoom = value;
                InvalidateLayout();
            }
        }

        #endregion

        #region 公共/继承 方法

        protected override void OnPaint(PaintEventArgs pevent)
        {
            FirstLeft = 0; 
            FirstTop = 0;

            Brush brush1 = new SolidBrush(BackColor);
            try
            {
                if ((pageInfo == null) || (pageInfo.Length == 0))
                {
                    pevent.Graphics.FillRectangle(brush1, base.ClientRectangle);
                    if ((pageInfo != null) || exceptionPrinting)
                    {
                        StringFormat format1 = new StringFormat();
                        format1.Alignment = TranslateAlignment(ContentAlignment.MiddleCenter);
                        format1.LineAlignment = TranslateLineAlignment(ContentAlignment.MiddleCenter);
                        SolidBrush brush2 = new SolidBrush(ForeColor);
                        try
                        {
                            if (exceptionPrinting)
                            {
                                pevent.Graphics.DrawString("建立预览页时发生未处理的异常。", Font, brush2, base.ClientRectangle, format1);
                                base.OnPaint(pevent);
                                return;
                            }
                            pevent.Graphics.DrawString("文档不包含任何页。", Font, brush2, base.ClientRectangle, format1);
                            base.OnPaint(pevent);
                            return;
                        }
                        finally
                        {
                            brush2.Dispose();
                            format1.Dispose();
                        }
                    }
                    base.BeginInvoke(new MethodInvoker(CalculatePageInfo));
                }
                else
                {
                    if (!layoutOk)
                        ComputeLayout();
                    Size size1 = new Size(PixelsToPhysical(new Point(base.Size), screendpi));
                    Point point1 = new Point(VirtualSize);
                    Point point2 =
                        new Point(Math.Max(0, (base.Size.Width - point1.X) / 2),
                                  Math.Max(0, (base.Size.Height - point1.Y) / 2));
                    point2.X -= Position.X;
                    point2.Y -= Position.Y;
                    //lastOffset = point2;
                    int num1 = PhysicalToPixels(10, screendpi.X);
                    int num2 = PhysicalToPixels(10, screendpi.Y);
                    Region region1 = pevent.Graphics.Clip;
                    Rectangle[] rectangleArray1 = new Rectangle[rows * columns];
                    Point point3 = Point.Empty;
                    int num3 = 0;
                    try
                    {
                        for (int num4 = 0; num4 < rows; num4++)
                        {
                            point3.X = 0;
                            point3.Y = num3 * num4;
                            for (int num5 = 0; num5 < columns; num5++)
                            {
                                int num6 = (StartPage + num5) + (num4 * columns);
                                if (num6 < pageInfo.Length)
                                {
                                    Size size2 = pageInfo[num6].PhysicalSize;
                                    if (autoZoom)
                                    {
                                        double num7 = (size1.Width - (10 * (columns + 1))) /
                                                      ((double)(columns * size2.Width));
                                        double num8 = (size1.Height - (10 * (rows + 1))) /
                                                      ((double)(rows * size2.Height));
                                        zoom = Math.Min(num7, num8);
                                    }
                                    imageSize = new Size((int)(zoom * size2.Width), (int)(zoom * size2.Height));
                                    Point point4 = PhysicalToPixels(new Point(imageSize), screendpi);
                                    int num9 = (point2.X + (num1 * (num5 + 1))) + point3.X;
                                    int num10 = (point2.Y + (num2 * (num4 + 1))) + point3.Y;
                                    point3.X += point4.X;
                                    num3 = Math.Max(num3, point4.Y);
                                    rectangleArray1[num6 - StartPage] = new Rectangle(num9, num10, point4.X, point4.Y);

                                    if (FirstLeft == 0)
                                    {
                                        FirstLeft = num9;
                                    }

                                    if (FirstTop == 0)
                                    {
                                        FirstTop = num10;
                                    }

                                    pevent.Graphics.ExcludeClip(rectangleArray1[num6 - StartPage]);
                                }
                            }
                        }
                        pevent.Graphics.FillRectangle(brush1, base.ClientRectangle);
                    }
                    finally
                    {
                        pevent.Graphics.Clip = region1;
                    }
                    for (int num11 = 0; num11 < rectangleArray1.Length; num11++)
                    {
                        if ((num11 + StartPage) < pageInfo.Length)
                        {
                            Rectangle rectangle1 = rectangleArray1[num11];
                            pevent.Graphics.DrawRectangle(Pens.Black, rectangle1);
                            pevent.Graphics.FillRectangle(new SolidBrush(ForeColor), rectangle1);
                            rectangle1.Inflate(-1, -1);
                            if (pageInfo[num11 + StartPage].Image != null)
                                pevent.Graphics.DrawImage(pageInfo[num11 + StartPage].Image, rectangle1);
                            rectangle1.Width--;
                            rectangle1.Height--;
                            pevent.Graphics.DrawRectangle(Pens.Black, rectangle1);
                        }
                    }
                }
            }
            finally
            {
                brush1.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            InvalidateLayout();
            base.OnResize(e);
        }

        /// <summary>
        /// 
        /// </summary>
        public void InvalidatePreview()
        {
            pageInfo = null;
            InvalidateLayout();
        }

        public event EventHandler OnScroll;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e); // 向基类注册事件。
            if (pageInfo.Length == 0 || !_havewheel) // 无内容或客户没有滚轮鼠标，退出
                return;

            // 计算应该卷动的行数，鼠标滚轮滚动一格后，内容移动的行数
            // 这个行数用 WHEEL_DELTA 常量表示，定义在另一个文件
            // 在MSDN中，WHEEL_DELTA 常量推荐选择 120 ，在Windows的头文件中也是这样设置的
            // 但我发现如果选择120的话，滚动速度很慢，所以我选择了20这个数。
            // 数字可以自行设定，数字越小，则滚轮每滚动一格，
            // 内容页滚动的行数越多
            float scrollRatio = e.Delta * SystemInformation.MouseWheelScrollLines / WHEEL_DELTA; // WHEEL_DELTA 常量，这里选择了20
            Point p = position; // 当前页面所在的位置。
            // 因为鼠标滚轮仅负责上下滚动，所以
            // 获取当时预览页面的纵轴位置 482
            int y = p.Y;
            // 获取打印预览窗口的高度
            //height = Math.Max(base.Height, virtualSize.Height);
            int height = Math.Min(base.Height, virtualSize.Height); // 659

            y -= (int)scrollRatio;
            p.Y = y;
            Position = p;

            if (OnScroll != null)
            {
                this.OnScroll(null, null);
            }

            /* 
             * 实际上算法很简单，y 变量表示垂直滚动条标记方块的位置
             * height 变量表示显示区域的高度，因为有滚动条，所以
             * 使用Math.Max() 方法获取的就是 virtualSize.Height
             * virtualSize.Height表示预览窗口里面的内容页的大小
             * base.Height 表示显示打印预览控件的大小
             * 而内容页有可能比控件要大得多，
             * 所以我就选择了窗口大小为height的值
             * 当滚轮向下滚动，则计算后得出的 scrollRatio 值为负值
             * 此时就必须检查 y 的值，看当前页面处于什么位置
             * 如果在页面底部，就看有没有下页，如果有，就转到下页
             * 同样，向上滚动也这样处理。
             * 
             * 另外，必须注意，当滚动到下页或上页时，页面的纵轴y还处于原来的位置
             * 这表示如果上页处于页面最底部状态，下翻到下一页，还处于页面最底部。
             * 所以必须设定，如果下翻到下一页，那么y值必须设定到页面顶部。
             * 上翻同样，上翻到上一页，则y值设置到页面底部。
             * 
             * 设定好以后，就给Position 属性赋值，赋值的操作实际上就是调用
             * SetPositionNoInvalidate()  方法
            */
            //if (scrollRatio < 0) // 负数，向下滚动
            //{
            //    // 检查当前页面的y位置，是否处于底部。
            //    if (y >= (height - 177))
            //    {
            //        if (StartPage < (pageInfo.Length - 1)) // 如果有下页，就翻页，Length不是从零开始，必须减1
            //        {
            //            StartPage++;
            //            y = 0; // 到下一页后，显示的应该是页面顶部。
            //            p.Y = y;
            //            Position = p;
            //            return;
            //        }
            //        y = height; // 没有下页，就不翻页，直接到页面底部。
            //        p.Y = y;
            //        Position = p;
            //        return;
            //    }
            //    // 不是处于底部
            //    y += (int)Math.Abs(scrollRatio); // 纵轴加上滚动距离数
            //    p.Y = y; // 指定位置。
            //    Position = p; // 开始移动。
            //}
            //else // 正数，向上滚动
            //{
            //    y = p.Y;
            //    if (y <= 5) // 如果页面处于顶部状态
            //    {
            //        // 检查是否有上页
            //        if (StartPage > 0) // 有就翻到上页
            //        {
            //            StartPage--;
            //            // 到上页后，位置应该是上页的底部
            //            y = height;
            //            p.Y = y;
            //            Position = p;
            //            return;
            //        }
            //        // 如果没有上页，则把页面调整到顶部
            //        y = 0;
            //        p.Y = y;
            //        Position = p;
            //        return;
            //    }
            //    y -= (int)scrollRatio;
            //    p.Y = y;
            //    Position = p;
            //}
        }

        /// <summary>
        /// Windows 内部消息处理。
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x114:
                    WmHScroll(ref m);
                    return;

                case 0x115:
                    WmVScroll(ref m);
                    return;

                case 0x100:
                    WmKeyDown(ref m);
                    return;
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// 设定打印预览的背景色，打印预览背景一般设定为深灰色。
        /// </summary>
        public override void ResetBackColor()
        {
            BackColor = SystemColors.AppWorkspace;
        }

        /// <summary>
        /// 前景色，就是白色，就是没有打印页显示时，显示文字颜色。
        /// </summary>
        public override void ResetForeColor()
        {
            ForeColor = Color.White;
        }

        #endregion

        #region 私有方法

        private static StringAlignment TranslateAlignment(ContentAlignment align)
        {
            if ((align & anyRight) != ((ContentAlignment)0))
                return StringAlignment.Far;
            if ((align & anyCenter) != ((ContentAlignment)0))
                return StringAlignment.Center;
            return StringAlignment.Near;
        }

        private static StringAlignment TranslateLineAlignment(ContentAlignment align)
        {
            if ((align & anyBottom) != ((ContentAlignment)0))
                return StringAlignment.Far;
            if ((align & anyMiddle) != ((ContentAlignment)0))
                return StringAlignment.Center;
            return StringAlignment.Near;
        }

        private void WmVScroll(ref Message m)
        {
            if (m.LParam != IntPtr.Zero)
                base.WndProc(ref m);
            else
            {
                Point point = Position;
                int y = point.Y;
                int height = Math.Max(base.Height, virtualSize.Height);
                point.Y = AdjustScroll(m, y, height);
                Position = point;
            }
        }

        /// <summary>
        /// 当鼠标滚动的时候移动垂直滚动条的位置。
        /// </summary>
        /// <param name="m"></param>
        /// <param name="pos"></param>
        /// <param name="maxPos"></param>
        /// <returns></returns>
        private int AdjustScroll(Message m, int pos, int maxPos)
        {
            switch (LOWORD(m.WParam))
            {
                case (0):
                    if (pos > 5)
                        pos -= 5;
                    else
                        pos = 0;
                    return pos;
                case (1):
                    if (pos < (maxPos - 5))
                        pos += 5;
                    else
                        pos = maxPos;
                    return pos;
                case (2):
                    if (pos > 100)
                    {
                        pos -= 100;
                        return pos;
                    }
                    pos = 0;
                    return pos;
                case (3):
                    if (pos < (maxPos - 100))
                    {
                        pos += 100;
                        return pos;
                    }
                    pos = maxPos;
                    return pos;
                case (4):
                case (5):
                    pos = HIWORD(m.WParam);
                    return pos;
            }

            if (OnScroll != null)
            {
                this.OnScroll(null, null);
            }

            return pos;
        }

        private void CalculatePageInfo()
        {
            if (!pageInfoCalcPending)
            {
                pageInfoCalcPending = true;
                try
                {
                    if (pageInfo == null)
                    {
                        try
                        {
                            ComputePreview();
                        }
                        catch
                        {
                            exceptionPrinting = true;
                            throw;
                        }
                        finally
                        {
                            base.Invalidate();
                        }
                    }
                }
                finally
                {
                    pageInfoCalcPending = false;
                }
            }
        }


        [DllImport("user32.dll")]
        static extern int GetScrollPos(IntPtr hWnd, int nBar);

        public int GetScrollPosY()
        {
            return GetScrollPos(this.Handle, 1);
        }

        public int GetScrollPosX()
        {
            return GetScrollPos(this.Handle, 0);
        }

        public void SetPositionNoInvalidate(Point value)
        {
            Point point = position;
            position = value;
            position.X = Math.Min(position.X, virtualSize.Width - base.Width);
            position.Y = Math.Min(position.Y, virtualSize.Height - base.Height);
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            Rectangle r = base.ClientRectangle;
            RECT rect = RECT.FromXYWH(r.X, r.Y, r.Width, r.Height);
            ScrollWindow(new HandleRef(this, base.Handle), point.X - position.X, point.Y - position.Y, ref rect,
                               ref rect);
            SetScrollPos(new HandleRef(this, base.Handle), 0, position.X, true);
            SetScrollPos(new HandleRef(this, base.Handle), 1, position.Y, true);
        }


        private void SetVirtualSizeNoInvalidate(Size value)
        {
            virtualSize = value;
            SetPositionNoInvalidate(position);
            SCROLLINFO scrInfo = new SCROLLINFO();
            scrInfo.fMask = 3;
            scrInfo.nMin = 0;
            scrInfo.nMax = Math.Max(base.Height, virtualSize.Height) - 1;
            scrInfo.nPage = base.Height;
            SetScrollInfo(new HandleRef(this, base.Handle), 1, scrInfo, true);
            scrInfo.fMask = 3;
            scrInfo.nMin = 0;
            scrInfo.nMax = Math.Max(base.Width, virtualSize.Width) - 1;
            scrInfo.nPage = base.Width;
            SetScrollInfo(new HandleRef(this, base.Handle), 0, scrInfo, true);
        }

        private void ComputeLayout()
        {
            layoutOk = true;
            if (pageInfo.Length == 0)
                base.ClientSize = base.Size;
            else
            {
                Graphics graphics1 = base.CreateGraphics();
                IntPtr ptr1 = graphics1.GetHdc();
                screendpi =
                    new Point(GetDeviceCaps(new HandleRef(graphics1, ptr1), 0x58),
                              GetDeviceCaps(new HandleRef(graphics1, ptr1), 90));
                
                //强制设置为100与XtraReport一致,便于坐标处理
                //screendpi =
                //    new Point(100, 100);
                graphics1.ReleaseHdcInternal(ptr1);
                graphics1.Dispose();
                Size size1 = pageInfo[StartPage].PhysicalSize;
                Size size2 = new Size(PixelsToPhysical(new Point(base.Size), screendpi));
                if (autoZoom)
                {
                    double num1 = (size2.Width - (10 * (columns + 1))) / ((double)(columns * size1.Width));
                    double num2 = (size2.Height - (10 * (rows + 1))) / ((double)(rows * size1.Height));
                    zoom = Math.Min(num1, num2);
                }
                imageSize = new Size((int)(zoom * size1.Width), (int)(zoom * size1.Height));
                int num3 = (imageSize.Width * columns) + (10 * (columns + 1));
                int num4 = (imageSize.Height * rows) + (10 * (rows + 1));
                SetVirtualSizeNoInvalidate(new Size(PhysicalToPixels(new Point(num3, num4), screendpi)));
            }
        }

        private void ComputePreview()
        {
            if (document == null)
                pageInfo = new PreviewPageInfo[0];
            else
            {
                System.Drawing.Printing.PrintController printControl = document.PrintController;
                PreviewPrintController previewControl = new PreviewPrintController();
                previewControl.UseAntiAlias = UseAntiAlias;
                document.PrintController = new PrintControllerWithStatusDialog(previewControl, "正在处理打印预览");
                document.Print();
                pageInfo = previewControl.GetPreviewPageInfo();
                document.PrintController = printControl;
            }
        }

        private void InvalidateLayout()
        {
            layoutOk = false;
            base.Invalidate();
        }

        private void WmHScroll(ref Message m)
        {
            if (m.LParam != IntPtr.Zero)
                base.WndProc(ref m);
            else
            {
                Point point = position;
                int y = point.X;
                int height = Math.Max(base.Width, virtualSize.Width);
                point.X = AdjustScroll(m, y, height);
                Position = point;
            }
        }

        private void WmKeyDown(ref Message msg)
        {
            Keys key = ((Keys)((int)msg.WParam)) | ModifierKeys;
            Point point = Position;
            int offSet = 0;
            int nLong = 0;
            switch ((key & Keys.KeyCode))
            {
                case Keys.Prior:
                    if ((key & ~Keys.KeyCode) != Keys.Control)
                    {
                        if (StartPage > 0)
                            StartPage--;
                        return;
                    }
                    offSet = point.X;
                    if (offSet <= 100)
                    {
                        offSet = 0;
                        break;
                    }
                    offSet -= 100;
                    break;

                case Keys.Next:
                    if ((key & ~Keys.KeyCode) != Keys.Control)
                    {
                        if (StartPage < pageInfo.Length)
                            StartPage++;
                        return;
                    }
                    offSet = point.X;
                    nLong = Math.Max(base.Width, virtualSize.Width);
                    if (offSet >= (nLong - 100))
                    {
                        offSet = nLong;
                        goto Label_00DD;
                    }
                    offSet += 100;
                    goto Label_00DD;

                case Keys.End:
                    if ((key & ~Keys.KeyCode) == Keys.Control)
                    {
                        StartPage = pageInfo.Length;
                        return;
                    }
                    return;

                case Keys.Home:
                    if ((key & ~Keys.KeyCode) == Keys.Control)
                    {
                        StartPage = 0;
                        return;
                    }
                    return;

                case Keys.Left:
                    offSet = point.X;
                    if (offSet <= 5)
                    {
                        offSet = 0;
                        goto Label_01BD;
                    }
                    offSet -= 5;
                    goto Label_01BD;

                case Keys.Up:
                    offSet = point.Y;
                    if (offSet <= 5)
                    {
                        offSet = 0;
                        goto Label_015C;
                    }
                    offSet -= 5;
                    goto Label_015C;

                case Keys.Right:
                    offSet = point.X;
                    nLong = Math.Max(base.Width, virtualSize.Width);
                    if (offSet >= (nLong - 5))
                    {
                        offSet = nLong;
                        goto Label_01FA;
                    }
                    offSet += 5;
                    goto Label_01FA;

                case Keys.Down:
                    offSet = point.Y;
                    nLong = Math.Max(base.Height, virtualSize.Height);
                    if (offSet >= (nLong - 5))
                    {
                        offSet = nLong;
                        goto Label_0199;
                    }
                    offSet += 5;
                    goto Label_0199;

                default:
                    return;
            }
            point.X = offSet;
            Position = point;
            return;
        Label_00DD:
            point.X = offSet;
            Position = point;
            return;
        Label_015C:
            point.Y = offSet;
            Position = point;
            return;
        Label_0199:
            point.Y = offSet;
            Position = point;
            return;
        Label_01BD:
            point.X = offSet;
            Position = point;
            return;
        Label_01FA:
            point.X = offSet;
            Position = point;
        }

        private static Point PhysicalToPixels(Point physical, Point dpi)
        {
            return new Point(PhysicalToPixels(physical.X, dpi.X), PhysicalToPixels(physical.Y, dpi.Y));
        }

        private static Size PhysicalToPixels(Size physicalSize, Point dpi)
        {
            return new Size(PhysicalToPixels(physicalSize.Width, dpi.X), PhysicalToPixels(physicalSize.Height, dpi.Y));
        }

        private static int PhysicalToPixels(int physicalSize, int dpi)
        {
            return (int)(((double)(physicalSize * dpi)) / 100);
        }

        private static Point PixelsToPhysical(Point pixels, Point dpi)
        {
            return new Point(PixelsToPhysical(pixels.X, dpi.X), PixelsToPhysical(pixels.Y, dpi.Y));
        }

        private static Size PixelsToPhysical(Size pixels, Point dpi)
        {
            return new Size(PixelsToPhysical(pixels.Width, dpi.X), PixelsToPhysical(pixels.Height, dpi.Y));
        }

        private static int PixelsToPhysical(int pixels, int dpi)
        {
            return (int)((pixels * 100) / ((double)dpi));
        }

        #endregion

        #region Win32 API
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nXAmount"></param>
        /// <param name="nYAmount"></param>
        /// <param name="rectScrollRegion"></param>
        /// <param name="rectClip"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool ScrollWindow(HandleRef hWnd, int nXAmount, int nYAmount, ref RECT rectScrollRegion, ref RECT rectClip);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nBar"></param>
        /// <param name="nPos"></param>
        /// <param name="bRedraw"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int SetScrollPos(HandleRef hWnd, int nBar, int nPos, bool bRedraw);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="fnBar"></param>
        /// <param name="si"></param>
        /// <param name="redraw"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int SetScrollInfo(HandleRef hWnd, int fnBar, SCROLLINFO si, bool redraw);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hDC"></param>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int GetDeviceCaps(HandleRef hDC, int nIndex);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int LOWORD(int n)
        {
            return (n & 0xffff);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int LOWORD(IntPtr n)
        {
            return LOWORD((int)((long)n));
        }

        public static int HIWORD(int n)
        {
            return ((n >> 0x10) & 0xffff);
        }

        public static int HIWORD(IntPtr n)
        {
            return HIWORD((int)(long)n);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="top"></param>
            /// <param name="right"></param>
            /// <param name="bottom"></param>
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="r"></param>
            public RECT(Rectangle r)
            {
                left = r.Left;
                top = r.Top;
                right = r.Right;
                bottom = r.Bottom;
            }
            /// <summary>
            /// 
            /// </summary>
            public int left;
            /// <summary>
            /// 
            /// </summary>
            public int top;
            /// <summary>
            /// 
            /// </summary>
            public int right;
            /// <summary>
            /// 
            /// </summary>
            public int bottom;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="width"></param>
            /// <param name="height"></param>
            /// <returns></returns>
            public static RECT FromXYWH(int x, int y, int width, int height)
            {
                return new RECT(x, y, x + width, y + height);
            }

            /// <summary>
            /// 
            /// </summary>
            public Size Size
            {
                get { return new Size(right - left, bottom - top); }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public sealed class SCROLLINFO
        {
            public SCROLLINFO()
            {
                this.cbSize = Marshal.SizeOf(typeof(SCROLLINFO));
            }

            public SCROLLINFO(int mask, int min, int max, int page, int pos)
            {
                this.cbSize = Marshal.SizeOf(typeof(SCROLLINFO));
                this.fMask = mask;
                this.nMin = min;
                this.nMax = max;
                this.nPage = page;
                this.nPos = pos;
            }

            public int cbSize;
            public int fMask;
            public int nMin;
            public int nMax;
            public int nPage;
            public int nPos;
            public int nTrackPos;
        }

        /// <summary>
        /// Win32 API 常数，指示在使用 <see cref="RemoveMenu"/> 函数时指定使用索引数而不是使用ID。
        /// </summary>
        private const int MF_BYPOSITION = 0x00000400;
        private const uint FILE_SHARE_READ = 0x00000001;
        private const uint FILE_SHARE_WRITE = 0x00000002;
        private const uint FILE_SHARE_DELETE = 0x00000004;
        private const uint SMART_GET_VERSION = 0x00074080; // SMART_GET_VERSION
        private const uint SMART_SEND_DRIVE_COMMAND = 0x0007c084; // SMART_SEND_DRIVE_COMMAND
        private const uint SMART_RCV_DRIVE_DATA = 0x0007c088; // SMART_RCV_DRIVE_DATA
        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;
        private const uint CREATE_NEW = 1;
        private const uint OPEN_EXISTING = 3;
        private const uint BUFFER_SIZE = 512;
        private const uint WHEEL_DELTA = 15;
        #endregion
    }
}