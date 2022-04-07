using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using dcl.client.frame;
using dcl.client.common;
using dcl.root.logon;
using dcl.entity;
using System.Linq;
using System.Drawing.Drawing2D;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using AForge.Video.DirectShow;
using System.IO;
using System.Drawing.Imaging;
using dcl.client.cache;

namespace dcl.client.result
{
    public partial class FrmMarrowMark : FrmCommon
    {
        //public List<CellMark> cellMarkList { get; set; }
        /// <summary>
        /// 记录标记信息
        /// </summary>
        public List<EntityObrCellsMark> cellMarkList { get; set; }
        /// <summary>
        /// 截图信息
        /// </summary>
        public List<EntityObrResultImage> resultImageList = new List<EntityObrResultImage>();
        /// <summary>
        /// 从上一界面获取的标记信息
        /// </summary>
        public List<EntityObrResult> dtPatientResulto { get; set; }
        /// <summary>
        /// 从上一界面获取图像信息
        /// </summary>
        public List<EntityObrResultImage> result_images { get; set; }
        /// <summary>
        /// 当前选中的细胞名称 控件名称
        /// </summary>
        string CurrentCellName = "";
        /// <summary>
        /// 当前选中的细胞名称 实际细胞名称
        /// </summary>
        string CurrentActualCellName = "";
        /// <summary>
        /// 按钮名称
        /// </summary>
        string CurrentButtonName = "";
        /// <summary>
        /// 标本类型
        /// </summary>
        public string SampleType = "";
        /// <summary>
        /// 样本号
        /// </summary>
        public string txtPatSid = "";
        /// <summary>
        /// 报告ID
        /// </summary>
        public string RepId = "";
        /// <summary>
        /// 标记的矩形
        /// </summary>
        List<Rectangle> rectangleList = new List<Rectangle>();

        /// <summary>
        /// 矩形标记坐标
        /// </summary>
        List<RectangleCoordinate> RectangleCoordinateList = new List<RectangleCoordinate>();

        /// <summary>
        /// 原始图像的矩形标记坐标
        /// </summary>
        List<RectangleCoordinate> RectangleCoordinateList_Original = new List<RectangleCoordinate>();

        /// <summary>
        /// 导入图片后的缩放值
        /// </summary>
        double offset;

        /// <summary>
        /// 图像载入控件后 X轴的偏移值
        /// </summary>
        int xOffSet;
        /// <summary>
        /// 图像载入控件后 Y轴的偏移值
        /// </summary>
        int yOffSet;
        /// <summary>
        /// 是否是标记状态
        /// </summary>
        bool IsMarkModel;
        /// <summary>
        /// 是否手动额外标记不在矩形框中的细胞
        /// </summary>
        bool IsByUser;
        /// <summary>
        /// 自动标记时判断是标记还是清空
        /// </summary>
        bool clearAutoMark;
        private FilterInfoCollection videoDevices;
        public FrmMarrowMark()
        {
            InitializeComponent();
            cellMarkList = new List<EntityObrCellsMark>();
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            int decivesNum = CameraDevices();
            if (decivesNum > 0)
            {
                CameraConn();
                panelVideo.Visible = true;
                panelPicture.Visible = false;
                labelSwitch.Text = "切换图片";
            }
            else
            {
                panelVideo.Visible = false;
                panelPicture.Visible = true;
                labelSwitch.Text = "切换视频";
            }            

            markCellName.Text = "";
            BindClickEvent();
            //this.panel20
            this.pictureEdit1.SendToBack();//将背景图片放到最下面
            this.pictureBox1.BackColor = Color.Transparent;
            this.panelVideo.BackColor = Color.Transparent;
            //this.panel20.BackColor = Color.Transparent;//将Panel设为透明
            this.pictureBox1.Parent = this.pictureEdit1;//将panel父控件设为背景图片控件
            this.pictureBox1.BringToFront();//将panel放在前面
        }

        /// <summary>
        /// 绑定所有的点击事件
        /// </summary>
        private void BindClickEvent()
        {
            BL_LX_YSXXB_button.Click += new EventHandler(CellChooseClick);
            BL_LX_YSLXB_button.Click += new EventHandler(CellChooseClick);
            BL_LX_ZYLXB_button.Click += new EventHandler(CellChooseClick);

            BL_LX_SJFY_button.Click += new EventHandler(CellChooseClick);
            BL_LX_SJGZ_button.Click += new EventHandler(CellChooseClick);
            BL_LX_SJWY_button.Click += new EventHandler(CellChooseClick); 
            BL_LX_SJZY_button.Click += new EventHandler(CellChooseClick); 
            BL_LX_SSFY_button.Click += new EventHandler(CellChooseClick);
            BL_LX_SSGZ_button.Click += new EventHandler(CellChooseClick);
            BL_LX_SSWY_button.Click += new EventHandler(CellChooseClick);
            BL_LX_SSZY_button.Click += new EventHandler(CellChooseClick);
            BL_LX_ZXFY_button.Click += new EventHandler(CellChooseClick);
            BL_LX_ZXGZ_button.Click += new EventHandler(CellChooseClick);
            BL_LX_ZXWY_button.Click += new EventHandler(CellChooseClick);
            BL_LX_ZXZY_button.Click += new EventHandler(CellChooseClick);

            BL_HX_JWY_button.Click += new EventHandler(CellChooseClick);
            BL_HX_JYS_button.Click += new EventHandler(CellChooseClick);
            BL_HX_JZAOY_button.Click += new EventHandler(CellChooseClick);
            BL_HX_JZY_button.Click += new EventHandler(CellChooseClick);
            BL_HX_WY_button.Click += new EventHandler(CellChooseClick);
            BL_HX_YS_button.Click += new EventHandler(CellChooseClick);
            BL_HX_ZAOY_button.Click += new EventHandler(CellChooseClick);
            BL_HX_ZY_button.Click += new EventHandler(CellChooseClick);

            BL_LBX_CS_button.Click += new EventHandler(CellChooseClick);
            BL_LBX_YS_button.Click += new EventHandler(CellChooseClick);
            BL_LBX_YX_button.Click += new EventHandler(CellChooseClick);
            BL_LBX_YZ_button.Click += new EventHandler(CellChooseClick);

            BL_DHX_CS_button.Click += new EventHandler(CellChooseClick);
            BL_DHX_YS_button.Click += new EventHandler(CellChooseClick);
            BL_DHX_YZ_button.Click += new EventHandler(CellChooseClick);

            BL_JX_CS_button.Click += new EventHandler(CellChooseClick);
            BL_JX_YS_button.Click += new EventHandler(CellChooseClick);
            BL_JX_YZ_button.Click += new EventHandler(CellChooseClick);

            MR_YSXXB_button.Click += new EventHandler(CellChooseClick);

            MR_LX_YSLXB_button.Click += new EventHandler(CellChooseClick);
            MR_LX_ZYLIB_button.Click += new EventHandler(CellChooseClick);

            MR_JX_CS_button.Click += new EventHandler(CellChooseClick);
            MR_JX_YS_button.Click += new EventHandler(CellChooseClick);
            MR_JX_YZ_button.Click += new EventHandler(CellChooseClick);

            MR_LX_SJFY_button.Click += new EventHandler(CellChooseClick);
            MR_LX_SJGZ_button.Click += new EventHandler(CellChooseClick);
            MR_LX_SJWY_button.Click += new EventHandler(CellChooseClick);
            MR_LX_SJZY_button.Click += new EventHandler(CellChooseClick);
            MR_LX_SSFY_button.Click += new EventHandler(CellChooseClick);
            MR_LX_SSGZ_button.Click += new EventHandler(CellChooseClick);
            MR_LX_SSWY_button.Click += new EventHandler(CellChooseClick);
            MR_LX_SSZY_button.Click += new EventHandler(CellChooseClick);
            MR_LX_ZXFY_button.Click += new EventHandler(CellChooseClick);
            MR_LX_ZXGZ_button.Click += new EventHandler(CellChooseClick);
            MR_LX_ZXWY_button.Click += new EventHandler(CellChooseClick);
            MR_LX_ZXZY_button.Click += new EventHandler(CellChooseClick);

            MR_HX_JWY_button.Click += new EventHandler(CellChooseClick);
            MR_HX_JYS_button.Click += new EventHandler(CellChooseClick);
            MR_HX_JZAOY_button.Click += new EventHandler(CellChooseClick);
            MR_HX_JZY_button.Click += new EventHandler(CellChooseClick);
            MR_HX_WY_button.Click += new EventHandler(CellChooseClick);
            MR_HX_YS_button.Click += new EventHandler(CellChooseClick);
            MR_HX_ZAOY_button.Click += new EventHandler(CellChooseClick);
            MR_HX_ZY_button.Click += new EventHandler(CellChooseClick);

            MR_LBX_CS_button.Click += new EventHandler(CellChooseClick);
            MR_LBX_YS_button.Click += new EventHandler(CellChooseClick);
            MR_LBX_YX_button.Click += new EventHandler(CellChooseClick);
            MR_LBX_YZ_button.Click += new EventHandler(CellChooseClick);

            MR_DHX_CS_button.Click += new EventHandler(CellChooseClick);
            MR_DHX_YS_button.Click += new EventHandler(CellChooseClick);
            MR_DHX_YZ_button.Click += new EventHandler(CellChooseClick);

            MR_JX_CS_button.Click += new EventHandler(CellChooseClick);
            MR_JX_YS_button.Click += new EventHandler(CellChooseClick);
            MR_JX_YZ_button.Click += new EventHandler(CellChooseClick);

            MR_QT_BM_button.Click += new EventHandler(CellChooseClick);
            MR_QT_CX_button.Click += new EventHandler(CellChooseClick);
            MR_QT_FL_button.Click += new EventHandler(CellChooseClick);
            MR_QT_NP_button.Click += new EventHandler(CellChooseClick);
            MR_QT_TH_button.Click += new EventHandler(CellChooseClick);
            MR_QT_TS_button.Click += new EventHandler(CellChooseClick);
            MR_QT_WZ_button.Click += new EventHandler(CellChooseClick);
            MR_QT_ZF_button.Click += new EventHandler(CellChooseClick);
            MR_QT_ZJ_button.Click += new EventHandler(CellChooseClick);
            MR_QT_ZS_button.Click += new EventHandler(CellChooseClick);
            MR_QT_ZZ_button.Click += new EventHandler(CellChooseClick);

            MR_JHX_CB_button.Click += new EventHandler(CellChooseClick);
            MR_JHX_KL_button.Click += new EventHandler(CellChooseClick);
            MR_JHX_LH_button.Click += new EventHandler(CellChooseClick);
            MR_JHX_YS_button.Click += new EventHandler(CellChooseClick);
            MR_JHX_YZ_button.Click += new EventHandler(CellChooseClick);
        }

        /// <summary>
        /// 预处理图像，画出图像轮廓存储轮廓坐标
        /// </summary>
        private void PreImagMark()
        {
            if (pictureEdit1.Image != null)
            {
                RectangleCoordinateList.Clear();
                Bitmap map = new Bitmap(pictureEdit1.Image);
                map = Calculate(map, 25);
                Image<Gray, byte> I = new Image<Gray, byte>(map);
                Image<Bgr, byte> DrawI = I.Convert<Bgr, byte>();
                Image<Gray, byte> CannyImage = I.Clone();
                CvInvoke.Canny(I, CannyImage, 150, 255, 5, true);
                EntityObrCellsMark cellMark = new EntityObrCellsMark();
                RectangleCoordinateList = BoundingBox(CannyImage, DrawI);
                //把标记的坐标再扩大几个像素，便于后面截下标记的图像
                foreach (RectangleCoordinate cordinate in RectangleCoordinateList)
                {
                    if (cordinate.MinX > 0 && cordinate.MinY > 0 && cordinate.MaxX > 0 && cordinate.MaxY > 0)
                    {
                        cordinate.MinX -= 5;
                        cordinate.MinY -= 5;
                        cordinate.MaxX += 5;
                        cordinate.MaxY += 5;
                    }
                }
            }
        }

        /// <summary>
        /// 标记图像
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        private void ImageMark(int X,int Y)
        {
            try
            {
                if (!IsMarkModel)
                    return;
                double zoompercent = pictureEdit1.Properties.ZoomPercent;
                //Image img = DrawI.Bitmap;
                //string path = @"d:\image\Emgu_save1.jpg";
                //img.Save(path);
                //System.Diagnostics.Process.Start(path);
                EntityObrCellsMark cellMark = new EntityObrCellsMark();
                Dictionary<int, int> rectangles = new Dictionary<int, int>();
                int position = 0;
                //在鼠标点击点中可能存在多个矩形，把所有记录下来取面积最小的
                foreach (RectangleCoordinate coordinate in RectangleCoordinateList)
                {
                    if (X > coordinate.MinX && X < coordinate.MaxX && Y > coordinate.MinY && Y < coordinate.MaxY)
                    {
                        int area = (coordinate.MaxX - coordinate.MinX) * (coordinate.MaxY - coordinate.MinY);
                        rectangles.Add(position,area);
                    }
                    position++;
                    if (position == RectangleCoordinateList.Count - 1)
                        break;
                }

                //选择点不在标记范围内 直接标记数字
                if (rectangles.Count < 1)
                {

                    cellMark.startPoint = new Point(X-10, Y-10);
                    cellMark.endPoint = new Point(X + 20,Y + 20);
                    cellMark.IsUserDefine = true;
                    cellMark.Area = 900;
                    cellMark.CellName = CurrentCellName;
                    cellMark.CellActualName = CurrentActualCellName;
                    //return;
                }
                else
                {
                    int minAreaKey = rectangles.Keys.Select(x => new { x, y = rectangles[x] }).OrderBy(x => x.y).First().x;
                    //矩形的长宽
                    int width = RectangleCoordinateList[minAreaKey].MaxX - RectangleCoordinateList[minAreaKey].MinX;
                    int heigth = RectangleCoordinateList[minAreaKey].MaxY - RectangleCoordinateList[minAreaKey].MinY;

                    Image im = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
                    Graphics g = Graphics.FromImage(im);
                    //GraphicsPath gp = new GraphicsPath();
                    Rectangle rec = new Rectangle(new Point(RectangleCoordinateList[minAreaKey].MinX, RectangleCoordinateList[minAreaKey].MinY), new Size(width, heigth));
                    cellMark.CellName = CurrentCellName;
                    cellMark.CellActualName = CurrentActualCellName;
                    cellMark.startPoint = new Point(RectangleCoordinateList[minAreaKey].MinX, RectangleCoordinateList[minAreaKey].MinY);
                    cellMark.endPoint = new Point(RectangleCoordinateList[minAreaKey].MaxX, RectangleCoordinateList[minAreaKey].MaxY);
                    cellMark.Area = width * heigth;
                    //如果在已标记的矩形框内进行标记
                    if (rectangleList.Contains(rec))
                    {
                        if (lis.client.control.MessageDialog.Show("您确定要在此范围内进行标记吗？", "确认", MessageBoxButtons.YesNo) ==
                        DialogResult.Yes)
                        {
                            cellMark.startPoint = new Point(X, Y);
                            cellMark.IsUserDefine = true;
                            cellMark.Area = 9;
                        }
                        else
                            return;
                        //SelectTheSameCell = true;
                    }
                    cellMark.rectangle = rec;
                    rectangleList.Add(rec);
                }

                cellMark.markPoint = new Point(X, Y);
                //多次点击表示需要标记未识别出的区域                

                int cellCounts =  NumSetting(CurrentCellName,false);
                cellMark.Counts = cellCounts;

                cellMarkList.Add(cellMark);

                DrawCellMark(cellMarkList);
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().ToString(), "ImageMark", ex.Message);
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// 处理图像为黑白
        /// </summary>
        /// <param name="_bitmap"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public Bitmap Calculate(Bitmap _bitmap, int threshold)
        {
            Bitmap grayBitmap = new Bitmap(_bitmap.Width, _bitmap.Height);
            List<int> piex = new List<int>();
            for (int i = 0; i < _bitmap.Width; i++)
            {
                for (int j = 0; j < _bitmap.Height; j++)
                {
                    //得到像素的原始色彩        
                    var oColor = _bitmap.GetPixel(i, j);
                    //得到该色彩的亮度
                    var brightness = oColor.GetBrightness();
                    //用该亮度计算灰度
                    var gRGB = (int)(brightness * 255);
                    piex.Add(gRGB);
                    //组成灰度色彩
                    var gColor = Color.FromArgb(gRGB, gRGB, gRGB);
                    //直接分黑白取轮廓
                    if (gRGB < 200)
                    {
                        gColor = Color.FromArgb(0, 0, 0);
                    }
                    else
                        gColor = Color.FromArgb(255, 255, 255);

                    //最后将该灰度色彩赋予该像素
                    grayBitmap.SetPixel(i, j, gColor);
                }
            }
            var ss = piex;
            Image img = grayBitmap;

            return grayBitmap;
        }
        /// <summary>
        /// 返回所有矩形像素点位置
        /// X 最小最大值 Y最小最大值
        /// </summary>
        /// <param name="src"></param>
        /// <param name="draw"></param>
        /// <returns></returns>
        private List<RectangleCoordinate> BoundingBox(Image<Gray, byte> src, Image<Bgr, byte> draw)
        {
            List<RectangleCoordinate> coordinateList = new List<RectangleCoordinate>();
            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.FindContours(src, contours, null, RetrType.External,
                                      ChainApproxMethod.ChainApproxSimple);
                int count = contours.Size;
                Point[][] con1 = contours.ToArrayOfArray();

                int recNum = 0;
                foreach (Point[] con in con1)
                {
                    RectangleCoordinate coordinate = new RectangleCoordinate();
                    if (con.Length > 4)
                    {
                        List<int> arrayX = new List<int>();
                        List<int> arrayY = new List<int>();
                        foreach (var pos in con)
                        {
                            arrayX.Add(pos.X);
                            arrayY.Add(pos.Y);
                        }
                        arrayX.Sort();
                        arrayY.Sort();
                        coordinate.MinX = arrayX[0];
                        coordinate.MaxX = arrayX[arrayX.Count - 1];
                        coordinate.MinY = arrayY[0];
                        coordinate.MaxY = arrayY[arrayX.Count - 1];
                        coordinateList.Add(coordinate);
                    }
                    recNum++;
                }
                //画出所有矩形 (这里不需要画出只需要记录坐标信息)
                //for (int i = 0; i < count; i++)
                //{
                //    using (VectorOfPoint contour = contours[i])
                //    {
                //        Rectangle BoundingBox = CvInvoke.BoundingRectangle(contour);
                //        CvInvoke.Rectangle(draw, BoundingBox, new MCvScalar(255, 0, 255, 255), 3);
                //    }
                //}
                return coordinateList;
            }
        }
        /// <summary>
        /// 设置选中细胞的数量
        /// </summary>
        /// <param name="controlName">控件名称</param>
        /// <param name="delete"></param>
        /// <returns></returns>
        private int NumSetting(string controlName,bool delete)
        {
            try
            {
                object obj = this.GetType().GetField(controlName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
                NumericUpDown numeric = (NumericUpDown)obj;
                if (!delete)
                    numeric.Value += 1;
                else if (numeric.Value > 0 && delete)
                    numeric.Value -= 1;
                return (int)numeric.Value;
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().ToString(), "NumSetting", ex.Message);
                return 0;
            }

        }

        /// <summary>
        /// 设置按钮颜色
        /// </summary>
        /// <param name="controlName">控件名称</param>
        /// <param name="isLastCell">是否是上一个点击的按钮</param>
        private void ColorSetting(string controlName,bool isLastCell)
        {
            try
            {
                object obj = this.GetType().GetField(controlName,
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
                    | System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
                SimpleButton numeric = (SimpleButton)obj;
                if (isLastCell)
                    numeric.Appearance.BackColor = Color.Maroon;
                else
                    numeric.Appearance.BackColor = Color.DarkGray;
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().ToString(), "ColorSetting", ex.Message);
            }

        }

        /// <summary>
        /// 获取当前标记的细胞名称
        /// </summary>
        /// <param name="sender"></param>
        private void SetCurrrentCellName(object sender)
        {
            if (!string.IsNullOrEmpty(CurrentCellName))
            {
                ColorSetting(CurrentButtonName, true);
            }
            SimpleButton btn = (SimpleButton)sender;
            markCellName.Text = btn.Text;
            CurrentCellName = btn.Tag.ToString();
            CurrentButtonName = btn.Name.ToString();
            CurrentActualCellName = btn.Text;
            ColorSetting(CurrentButtonName, false);
        }

        /// <summary>
        /// 改变button样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Paint(object sender, PaintEventArgs e)
        {
            //Draw(e.ClipRectangle, e.Graphics, 18, false, Color.FromArgb(0, 122, 204), Color.FromArgb(8, 39, 57));
            //base.OnPaint(e);
            ////Graphics g = e.Graphics;
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

        private void FrmMarrowMark_Load(object sender, EventArgs e)
        {
            panel_BL.HorizontalScroll.Visible = true;
            panel_MR.HorizontalScroll.Visible = true;

            if (SampleType != "骨髓")
            {
                xtraTabPage_MR.PageVisible = false;
            }

            if (this.dtPatientResulto != null && this.dtPatientResulto.Count > 0 )
            {
                try
                {
                    LoadCellsNum(this.dtPatientResulto);
                    LoadCellsImag(this.result_images);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            else
            {
                MR_HXBZS.Value = 200;
                BL_HXBZS.Value = 200;
            }
            //panel1.HorizontalScroll.Visible = true;//水平的显示
            //string fileName = "originalpic_test.jpg";// "originalpic2.jpg";
            //string path = Application.StartupPath + @"\MarrowImag\" + fileName;
            //if (File.Exists(path))
            //{
            //    pictureEdit1.Image = Image.FromFile(path);
            //    PreImagMark();
            //}

            //string path = @"d:\image\Emgu_save1.jpg";
            //pictureEdit1.Image = Image.FromFile(path);

        }

        /// <summary>
        /// 根据已保存的数据，在镜检界面载入细胞数
        /// </summary>
        /// <param name="dtPatientResulto"></param>
        private void LoadCellsNum(List<EntityObrResult> dtPatientResulto)
        {
            if (dtPatientResulto != null && dtPatientResulto.Count > 0)
            {
                EntityObrResult blresult = dtPatientResulto.Find(x => x.ItmEname == "核细胞总数");
                if (blresult == null || string.IsNullOrWhiteSpace(blresult.ObrBldValue))
                    return;
                double BL_HXBZS = Convert.ToDouble(blresult.ObrBldValue);
                if (BL_HXBZS > 0)
                {
                    foreach (EntityObrResult dtpatient in dtPatientResulto)
                    {
                        SetCellNum(panel_BL, dtpatient.ItmEname, dtpatient.ObrBldValue, BL_HXBZS);

                        //if (dtpatient.IsValueNeedCalculate)
                        //{
                        //    string bl_num = (Convert.ToDouble(dtpatient.ObrBldValue) * BL_HXBZS / 100).ToString();
                        //    SetCellNum(panel_BL,dtpatient.ItmName,bl_num,BL_HXBZS);
                        //}
                        //else
                        //{
                        //    SetCellNum(panel_BL, dtpatient.ItmName, dtpatient.ObrBldValue, BL_HXBZS);
                        //}
                    }
                }
                EntityObrResult boneresult = dtPatientResulto.Find(x => x.ItmEname == "核细胞总数");
                if (boneresult == null || string.IsNullOrWhiteSpace(boneresult.ObrBoneValue) )
                    return;
                double MR_HXBZS = Convert.ToDouble(boneresult.ObrBoneValue);
                if (MR_HXBZS > 0)
                {
                    foreach (EntityObrResult dtpatient in dtPatientResulto)
                    {
                        SetCellNum(panel_MR, dtpatient.ItmEname, dtpatient.ObrBoneValue, MR_HXBZS);

                        //if (dtpatient.IsValueNeedCalculate)
                        //{
                        //    string mr_num = (Convert.ToDouble(dtpatient.ObrBoneValue) * BL_HXBZS / 100).ToString();
                        //    SetCellNum(panel_MR, dtpatient.ItmName, mr_num, MR_HXBZS);
                        //}
                        //else
                        //{
                        //    SetCellNum(panel_MR, dtpatient.ItmName, dtpatient.ObrBoneValue, MR_HXBZS);
                        //}
                    }
                }

            }
        }
        /// <summary>
        /// 根据已保存的数据，在镜检界面载入图片
        /// </summary>
        /// <param name="result_images"></param>
        private void LoadCellsImag(List<EntityObrResultImage> result_images)
        {
            if (result_images != null && result_images.Count > 0)
            {
                UpdatePictures(result_images);
                videoSwitch_Click(null,null);
            }
        }

        /// <summary>
        /// 载入图片
        /// </summary>
        /// <param name="list_result_img"></param>
        private void UpdatePictures(List<EntityObrResultImage> list_result_img)
        {
            flowLayoutPanel1.Controls.Clear();
            if (list_result_img == null || list_result_img.Count == 0)
            {
                return;
            }
            foreach (EntityObrResultImage img in list_result_img)
            {
                MemoryStream ms = new MemoryStream(img.ObrImage);
                Image img_show = Image.FromStream(ms);
                PictureEdit pic = new PictureEdit();
                ContextMenu emptyMenu = new ContextMenu();
                pic.Properties.ContextMenu = emptyMenu;
                pic.MouseClick += new MouseEventHandler(DeleteShot);
                pic.Size = new Size(81, 62);
                pic.Image = img_show;
                pic.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
                flowLayoutPanel1.Controls.Add(pic);
            }
            foreach (Control con in this.flowLayoutPanel1.Controls)
            {
                if (con is PictureEdit)
                {
                    PictureEdit pic = (PictureEdit)con;
                    cellMarkList.Clear();
                    rectangleList.Clear();
                    DrawCellMark(cellMarkList);
                    pictureEdit1.Image = SetImageSize(pic.Image, new Size(this.panelPicture.Width, this.panelPicture.Height));
                    PreImagMark();
                    var xx = pictureEdit1.Image.Size;
                    int offsetY = (this.panelPicture.Height - xx.Height) / 2;
                    int offsetX = (this.panelPicture.Width - xx.Width) / 2;
                    xOffSet = offsetX;
                    yOffSet = offsetY;
                    foreach (RectangleCoordinate coordinate in RectangleCoordinateList)
                    {
                        coordinate.MinY += offsetY;
                        coordinate.MaxY += offsetY;

                        coordinate.MinX += offsetX;
                        coordinate.MaxX += offsetX;
                    }
                    break;
                }
            }
        }
        /// <summary>
        /// 通过已保存的值设置细胞数量
        /// </summary>
        /// <param name="control">控件名称</param>
        /// <param name="controlTagName">设置的控件标记名称（几位项目名称）</param>
        /// <param name="num">数值</param>
        /// <param name="HXBZS">相应的核细胞总数</param>
        private void SetCellNum(Control control,string controlTagName,string num ,double HXBZS)
        {
            if (string.IsNullOrEmpty(num))
            {
                return;
            }
            foreach (Control con in control.Controls)
            {
                if (con is Panel)
                {                       
                    foreach (Control num2 in con.Controls)
                    {
                        if (num2 is GroupBox)
                        {
                            foreach (Control numer in num2.Controls)
                            {
                                if (numer is NumericUpDown)//BaseEdit
                                {
                                    if (numer.Tag != null && numer.Tag.ToString() == controlTagName)
                                    {
                                        NumericUpDown numeri = (NumericUpDown)numer;
                                        numeri.Value = Convert.ToDecimal(Convert.ToDouble(num) * HXBZS / 100);
                                    }
                                }
                            }
                        }
                        else if (num2 is NumericUpDown)//BaseEdit
                        {
                            NumericUpDown numeric = (NumericUpDown)num2;
                            if (num2.Tag != null && num2.Tag.ToString() == controlTagName)
                            {
                                //骨髓片的NAP阳性率等参数、骨髓片的巨核系参数、血片的红系参数都直接显示设置的数值，不进行计算
                                if (con.Name == "panel_MR_Other" || con.Name == "panel_MR_JHX" || numeric.Name == "MR_HXBZS")
                                {
                                    numeric.Value = Convert.ToDecimal(num);
                                }
                                else if (con.Name == "panel_BL_HX" || numeric.Name == "BL_HXBZS")
                                {
                                    numeric.Value = Convert.ToDecimal(num);
                                }
                                else
                                {
                                    numeric.Value = Convert.ToDecimal(Convert.ToDouble(num) * HXBZS / 100);
                                }
                            }
                        }
                        else if (num2 is ComboBoxEdit)
                        {
                            ComboBoxEdit numeric = (ComboBoxEdit)num2;
                            if (num2.Tag != null && num2.Tag.ToString() == controlTagName)
                            {
                                numeric.EditValue = num;
                            }
                        }
                        else if (num2 is TextEdit)
                        {
                            TextEdit numeric = (TextEdit)num2;
                            if (num2.Tag != null && num2.Tag.ToString() == controlTagName)
                            {
                                numeric.EditValue = num;
                            }

                        }
                    }
                }
            }
            //foreach (Control con in control.Controls)
            //{
            //    if (con.Tag != null && con.Tag.ToString() == controlTagName && con is NumericUpDown)
            //    {
            //        NumericUpDown numeri = (NumericUpDown)con;
            //        numeri.Value = Convert.ToDecimal(num);
            //    }
            //    else if (con.Tag != null && con.Tag.ToString() == controlTagName && con is ComboBoxEdit)
            //    {
            //        ComboBoxEdit combo = (ComboBoxEdit)con;
            //        combo.EditValue = num;
            //    }
            //}
        }

        private void pictureEdit1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                //DrawRectangle(sender,e);
                //NumSetting(CurrentCellName);
            }
            catch (Exception ex)
            {

            }

        }


        private void simpleButton5_Click(object sender, EventArgs e)
        {
            //CurrentCellName = this.Tag.ToString();
        }

        private void simpleButton134_Click(object sender, EventArgs e)
        {
        }

        private void CellChooseClick(object sender, EventArgs e)
        {
            IsMarkModel = true;
            SetCurrrentCellName(sender);
            DrawCellMark(cellMarkList);
        }

        /// <summary>
        /// 获取标记的细胞数量
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        private int GetCellsNum(Control control)
        {
            int counts = 0;
            foreach (Control num in control.Controls)
            {
                if (num is GroupBox)
                {
                    foreach (Control numer in num.Controls)
                    {
                        if (numer is NumericUpDown)//BaseEdit
                        {
                            NumericUpDown numeric = (NumericUpDown)numer;
                            counts += (int)numeric.Value;
                        }
                    }
                }
                else if (num is NumericUpDown)//BaseEdit
                {
                    NumericUpDown numeric = (NumericUpDown)num;
                    counts += (int)numeric.Value;
                }
            }
            return counts;
        }

        /// <summary>
        /// 清空控件标记的值
        /// </summary>
        /// <param name="control"></param>
        private void ClearAllCellsNum(Control control)
        {
            List<string> output = new List<string>();
            foreach (Control con in control.Controls)
            {
                if (con is Panel)
                {
                    foreach (Control num in con.Controls)
                    {
                        if (num is GroupBox)
                        {
                            foreach (Control numer in num.Controls)
                            {
                                if (numer is NumericUpDown)//BaseEdit
                                {
                                    NumericUpDown numeric = (NumericUpDown)numer;
                                    numeric.Value = 0;
                                }
                            }
                        }
                        else if (num is NumericUpDown)//BaseEdit
                        {
                            NumericUpDown numeric = (NumericUpDown)num;
                            numeric.Value = 0;
                        }
                        else if (num is TextEdit)
                        {
                            TextEdit numeric = (TextEdit)num;
                            numeric.EditValue = "";
                        }
                        //num.Text = "" ;
                    }                    
                }
            }
        }
        /// <summary>
        /// 获取所有的截图图片
        /// </summary>
        /// <returns></returns>
        private List<EntityObrResultImage> GetAllShot()
        {
            resultImageList.Clear();
            foreach (Control pic in flowLayoutPanel1.Controls)
            {                
                if (pic is PictureEdit)
                {
                    EntityObrResultImage resultImage = new EntityObrResultImage();
                    var pict = pic as PictureEdit;
                    Bitmap map = new Bitmap(pict.Image);
                    resultImage.ObrImage =  Bitmap2Byte(map);
                    resultImageList.Add(resultImage);
                }
            }
            return resultImageList;
        }

        /// <summary>
        /// 保存所有标记的图像
        /// </summary>
        private void SaveMarkPicture()
        {            
            string date = DateTime.Now.ToString("yyyy-MM-dd");

            try
            {
                foreach (EntityObrCellsMark cellmark in cellMarkList)
                {
                    string dirpath = Application.StartupPath + "\\MarkCollection\\" + date + "\\报告ID-" + RepId + "\\" + cellmark.CellActualName + "\\";
                    if (!Directory.Exists(dirpath))
                        Directory.CreateDirectory(dirpath);
                    int width = cellmark.rectangle.Width;
                    int height = cellmark.rectangle.Height;
                    Bitmap bitmap = new Bitmap(width,height);
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        Rectangle rectSource = new Rectangle(cellmark.startPoint.X - xOffSet , cellmark.startPoint.Y - yOffSet , cellmark.rectangle.Width, cellmark.rectangle.Height);
                        Rectangle rectDest = new Rectangle(0, 0, width, height);
                        g.DrawImage(pictureEdit1.Image, rectDest, rectSource, GraphicsUnit.Pixel);
                        //g.DrawImage(pictureEdit1.Image, cellmark.rectangle);
                        bitmap.Save(dirpath + "\\" + cellmark.CellActualName  + cellmark.Counts + ".png", System.Drawing.Imaging.ImageFormat.Png);
                        bitmap.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

        }

        /// <summary>
        /// 获取控件标记的值
        /// </summary>
        /// <param name="control">控件名称</param>
        /// <param name="isBlood">是否是血片数据</param>
        private void GetAllCellsNum(Control control,bool isBlood)
        {
            try
            {
                List<EntityDicItmItem> listItem = new List<EntityDicItmItem>();
                listItem = CacheClient.GetCache<EntityDicItmItem>();
                List<string> output = new List<string>();
                foreach (Control con in control.Controls)
                {
                    if (con is Panel)
                    {
                        //if (con.Name == "panel5")
                        //{
                        //    foreach (Control num in con.Controls)
                        //    {
                        //        string name = num.Name;
                        //        string tag = num.Tag?.ToString();
                        //    }
                        //}
                        foreach (Control num in con.Controls)
                        {
                            if (num is GroupBox)
                            {
                                foreach (Control numer in num.Controls)
                                {
                                    if (numer is NumericUpDown)//BaseEdit
                                    {
                                        NumericUpDown numeric = (NumericUpDown)numer;
                                        if (numeric.Value > 0)
                                        {
                                            EntityObrCellsMark cellmark = new EntityObrCellsMark();
                                            cellmark.Counts = (int)numeric.Value;
                                            //保存的值需要进行计算
                                            if (isBlood)
                                                cellmark.ObrBldValue = Math.Round((double)(numeric.Value / BL_HXBZS.Value * 100), 1).ToString();
                                            else
                                                cellmark.ObrBoneValue = Math.Round((double)(numeric.Value / MR_HXBZS.Value * 100), 1).ToString();  //(int)numeric.Value;
                                            if (listItem.Find(x => x.ItmEcode == numeric.Tag.ToString()) != null)
                                            {
                                                cellmark.ItemId = listItem.Find(x => x.ItmEcode == numeric.Tag.ToString()).ItmId;
                                            }
                                            cellmark.CellName = numeric.Name;
                                            cellmark.CellActualName = numeric.Tag.ToString();
                                            //if (con.Name == "panel_MR_HX" || con.Name == "panel_BL_HX"
                                            //    || con.Name == "panel_MR_JHX" || con.Name == "panel_MR_Other"
                                            //    || numeric.Name == "MR_HXBZS" || numeric.Name == "BL_HXBZS")
                                            //{
                                            //    cellmark.IsValueNeedCalculate = false;
                                            //}
                                            //else
                                            //    cellmark.IsValueNeedCalculate = true;
                                            cellMarkList.Add(cellmark);
                                        }
                                    }
                                }
                            }
                            else if (num is NumericUpDown)//BaseEdit
                            {
                                NumericUpDown numeric = (NumericUpDown)num;
                                if (numeric.Value > 0)
                                {
                                    EntityObrCellsMark cellmark = new EntityObrCellsMark();
                                    //骨髓片的NAP阳性率等参数、骨髓片的巨核系参数、血片的红系参数都直接显示设置的数值，不进行计算
                                    if (con.Name == "panel_MR_Other" || con.Name == "panel_MR_JHX"|| numeric.Name == "MR_HXBZS")
                                    {
                                        cellmark.ObrBoneValue = numeric.Value.ToString();
                                    }
                                    else if (con.Name == "panel_BL_HX" || numeric.Name == "BL_HXBZS")
                                    {
                                        cellmark.ObrBldValue = numeric.Value.ToString();
                                    }
                                    else
                                    {
                                        cellmark.Counts = (int)numeric.Value;
                                        if (isBlood)
                                            cellmark.ObrBldValue = Math.Round((double)(numeric.Value / BL_HXBZS.Value * 100), 1).ToString(); //(int)numeric.Value;
                                        else
                                            cellmark.ObrBoneValue = Math.Round((double)(numeric.Value / MR_HXBZS.Value * 100), 1).ToString(); //(int)numeric.Value;
                                    }
                                    cellmark.CellName = numeric.Name;
                                    cellmark.CellActualName = numeric.Tag.ToString();

                                    if (listItem.Find(x => x.ItmEcode == numeric.Tag.ToString()) != null)
                                    {
                                        cellmark.ItemId = listItem.Find(x => x.ItmEcode == numeric.Tag.ToString()).ItmId;
                                    }
                                    //if (con.Name == "panel_MR_HX" || con.Name == "panel_BL_HX"
                                    //    || con.Name == "panel_MR_JHX" || con.Name == "panel_MR_Other"
                                    //    || numeric.Name == "MR_HXBZS" || numeric.Name == "BL_HXBZS")
                                    //{
                                    //    cellmark.IsValueNeedCalculate = false;
                                    //}
                                    //else
                                    //    cellmark.IsValueNeedCalculate = true;
                                    cellMarkList.Add(cellmark);
                                }
                            }
                            else if (num is ComboBoxEdit)
                            {
                                ComboBoxEdit numeric = (ComboBoxEdit)num;
                                EntityObrCellsMark cellmark = new EntityObrCellsMark();
                                cellmark.ObrBoneValue = numeric.EditValue.ToString();
                                if (listItem.Find(x => x.ItmEcode == numeric.Tag.ToString()) != null)
                                {
                                    cellmark.ItemId = listItem.Find(x => x.ItmEcode == numeric.Tag.ToString()).ItmId;
                                }
                                cellMarkList.Add(cellmark);
                            }
                            //else if (num is TextEdit)
                            //{
                            //    TextEdit numeric = (TextEdit)num;
                            //    EntityObrCellsMark cellmark = new EntityObrCellsMark();
                            //    cellmark.ObrBoneValue = numeric.EditValue.ToString();
                            //    if (listItem.Find(x => x.ItmEcode == numeric.Tag.ToString()) != null)
                            //    {
                            //        cellmark.ItemId = listItem.Find(x => x.ItmEcode == numeric.Tag.ToString()).ItmId;
                            //    }
                            //    cellMarkList.Add(cellmark);
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            //var ss = output;
        }
        /// <summary>
        /// 设置 粒细胞系统:红细胞系统 比值
        /// </summary>
        /// <param name="control"></param>
        /// <param name="isBlood"></param>
        private void GetLXHX(Control control, bool isBlood)
        {
            try
            {
                List<EntityDicItmItem> listItem = new List<EntityDicItmItem>();
                listItem = CacheClient.GetCache<EntityDicItmItem>();
                List<string> output = new List<string>();
                foreach (Control con in control.Controls)
                {
                    if (con is Panel)
                    {
                        foreach (Control num in con.Controls)
                        {
                            if (num is TextEdit)
                            {
                                TextEdit numeric = (TextEdit)num;
                                EntityObrCellsMark cellmark = new EntityObrCellsMark();
                                cellmark.ObrBoneValue = numeric.EditValue.ToString();
                                if (listItem.Find(x => x.ItmEcode == numeric.Tag.ToString()) != null)
                                {
                                    cellmark.ItemId = listItem.Find(x => x.ItmEcode == numeric.Tag.ToString()).ItmId;
                                }
                                cellMarkList.Add(cellmark);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
        }
        /// <summary>
        /// 是否保存
        /// </summary>
        public bool IsSave { get; set; }
        /// <summary>
        /// 是否关闭
        /// </summary>
        public bool IsClose { get; set; }
        /// <summary>
        /// 矩形轮廓的各个顶点
        /// </summary>
        class RectangleCoordinate
        {
            public int MinX { get; set; }

            public int MaxX { get; set; }

            public int MinY { get; set; }

            public int MaxY { get; set; }
        }

        /// <summary>
        /// 连接摄像头
        /// </summary>
        private void CameraConn()
        {
            try
            {
                VideoCaptureDevice videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.DesiredFrameSize = new Size(this.panelVideo.Width, this.panelVideo.Height);
                videoSource.DesiredFrameRate = 1;
                videPlayer.VideoSource = videoSource;
                videPlayer.Start();
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show("启用摄像头失败！请检查驱动程序是否正常，或者有别的应用程序在占用此设备。", "提示");
                Lib.LogManager.Logger.LogException(ex);
            }
        }

        /// <summary>
        /// 检测所有摄像头
        /// </summary>
        /// <returns></returns>
        private int CameraDevices()
        {
            int devCount = 0;
            try
            {
                // 枚举所有视频输入设备
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                devCount = videoDevices.Count;

                if (videoDevices.Count == 0)
                    throw new ApplicationException();
            }
            catch (ApplicationException ex)
            {
                videoDevices = null;
                lis.client.control.MessageDialog.Show("未检测到摄像头设备！请检查摄像头是否连接正常，或者驱动程序是否正确安装。", "提示");
                Lib.LogManager.Logger.LogException(ex);
            }
            return devCount;
        }

        /// <summary>
        /// 截图 获取镜下图片进行细胞标记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BtnResultView_Click(object sender, EventArgs e)
        {
            if (videPlayer != null && videPlayer.IsRunning)
            {
                System.Drawing.Image img = new Bitmap(videPlayer.Width, videPlayer.Height);
                videPlayer.DrawToBitmap((Bitmap)img, new Rectangle(0, 0, videPlayer.Width, videPlayer.Height));
                //EnterModit();
                this.pictureEdit1.Image = img;
                //btmRotate = (Bitmap)this.pictureEdit1.Image;
            }
            else
            {
                lis.client.control.MessageDialog.Show("未找到图像", "提示");
            }
        }


        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (flowLayoutPanel1.Controls.Count < 1)//清空图像后不标记
                {
                    return;
                }
                if (e.Button == MouseButtons.Right)
                {
                    RemoveImageMark(e.X, e.Y);
                        
                }
                else
                {
                    ImageMark(e.X,e.Y);
                }
                //NumSetting(CurrentCellName);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
        }

        /// <summary>
        /// 删除细胞标记
        /// </summary>
        /// <param name="e"></param>
        private void RemoveImageMark(int X,int Y)
        {
            string cellname = "";
            List<EntityObrCellsMark> filterCellMark = new List<EntityObrCellsMark>();
            foreach (EntityObrCellsMark cellmark in cellMarkList)
            {
                if (X > cellmark.startPoint.X && X < cellmark.endPoint.X && Y > cellmark.startPoint.Y && Y < cellmark.endPoint.Y)
                {
                    filterCellMark.Add(cellmark);
                }                                    
            }
            //删除选中点范围内面积最小的区域
            EntityObrCellsMark minCell = filterCellMark.OrderBy(x => x.Area).First();
            cellname = minCell.CellName;
            int cellCount = minCell.Counts;
            cellMarkList.Remove(minCell);
            rectangleList.Remove(minCell.rectangle);
            foreach (EntityObrCellsMark cellmark in cellMarkList)
            {
                if (cellmark.CellName == cellname && cellmark.Counts > cellCount)
                {
                    cellmark.Counts -= 1;
                }
            }
            NumSetting(cellname,true);
            DrawCellMark(cellMarkList);
        }
        /// <summary>
        /// 画出标记
        /// </summary>
        private void DrawCellMark(List<EntityObrCellsMark> cellMarkList)
        {
            Image im = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            Graphics g = Graphics.FromImage(im);
            foreach (EntityObrCellsMark cellmark in cellMarkList)
            {
                if (cellmark.CellName == CurrentCellName)
                {
                    g.DrawRectangle(new Pen(Color.Red), cellmark.rectangle);
                    g.DrawString(cellmark.Counts.ToString(), new Font("微软雅黑", 12, FontStyle.Bold), new SolidBrush(Color.Black), cellmark.markPoint);
                }
            }
            this.pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            this.pictureBox1.BackgroundImage = im;
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (markType.SelectedIndex == 0)
            //{
            //    IsByUser = false;
            //}
            //else
            //    IsByUser = true;
        }

        /// <summary>
        /// 自动标记所有
        /// </summary>
        private void AutoMartkAllCells()
        {
            cellMarkList = new List<EntityObrCellsMark>();
            foreach (RectangleCoordinate coordinate in RectangleCoordinateList)
            {
                int width = coordinate.MaxX - coordinate.MinX;
                int heigth = coordinate.MaxY - coordinate.MinY;
                EntityObrCellsMark cellMark = new EntityObrCellsMark();
                Rectangle rec = new Rectangle(new Point(coordinate.MinX, coordinate.MinY), new Size(width, heigth));
                cellMark.CellName = CurrentCellName;
                cellMark.startPoint = new Point(coordinate.MinX, coordinate.MinY);
                cellMark.endPoint = new Point(coordinate.MaxX, coordinate.MaxY);
                cellMark.Area = width * heigth;
                cellMark.rectangle = rec;
                //过滤过小的标记
                if (cellMark.Area < 600)
                    continue;
                cellMarkList.Add(cellMark);
            }
            DrawCellMark(cellMarkList);

        }

        /// <summary>
        /// 截图处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteShot(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (lis.client.control.MessageDialog.Show("确定删除该图像吗？", "确认", MessageBoxButtons.YesNo) ==
                            DialogResult.Yes)
                {
                    PictureEdit pic = (PictureEdit)sender;
                    this.flowLayoutPanel1.Controls.Remove(pic);
                    if (this.flowLayoutPanel1.Controls.Count < 1)
                    {
                        this.pictureEdit1.Image.Dispose();
                        cellMarkList.Clear();
                        rectangleList.Clear();
                        DrawCellMark(cellMarkList);
                    }                        
                }
                else
                    return;
                   
            }
            else if (e.Button == MouseButtons.Left)
            {
                PictureEdit pic = (PictureEdit)sender;
                if (panelVideo.Visible == true)
                {
                    FrmImagView imagview = new FrmImagView();
                    imagview.imagelist = GetAllImage(this.flowLayoutPanel1);
                    imagview.currentImage = pic.Image;
                    imagview.ShowDialog();
                }
                else
                {
                    cellMarkList.Clear();
                    rectangleList.Clear();
                    DrawCellMark(cellMarkList);
                    pictureEdit1.Image = SetImageSize(pic.Image, new Size(this.panelPicture.Width, this.panelPicture.Height));
                    PreImagMark();
                    var xx = pictureEdit1.Image.Size;
                    int offsetY = (this.panelPicture.Height - xx.Height) / 2;
                    int offsetX = (this.panelPicture.Width - xx.Width) / 2;
                    xOffSet = offsetX;
                    yOffSet = offsetY;
                    foreach (RectangleCoordinate coordinate in RectangleCoordinateList)
                    {
                        coordinate.MinY += offsetY;
                        coordinate.MaxY += offsetY;

                        coordinate.MinX += offsetX;
                        coordinate.MaxX += offsetX;
                    }
                }
            }
            //PictureEdit pic = (PictureEdit)sender;
            //pictureEdit1.Image = pic.Image;
            //cellMarkList.Clear();
            //rectangleList.Clear();
            //DrawCellMark(cellMarkList);
            //ClearAllCellsNum(this.panel6);
            //ClearAllCellsNum(this.panel9);

            //pictureBox1.BackgroundImage
        }

        private List<Image> GetAllImage(Control control)
        {
            List<Image> imaglist = new List<Image>();
            foreach (Control con in control.Controls)
            {
                if (con is PictureEdit)
                {
                    PictureEdit im = con as PictureEdit;
                    imaglist.Add(im.Image);
                }
            }
            return imaglist;
        }

        private void ClearAllMarkInfo()
        {
            cellMarkList.Clear();
            rectangleList.Clear();
            DrawCellMark(cellMarkList);
            ClearAllCellsNum(this.panel_BL);
            ClearAllCellsNum(this.panel_MR);
        }

        private void PictureShot(object sender, EventArgs e)
        {
            if (flowLayoutPanel1.Controls.Count > 0)
            {
                ContextMenu emptyMenu = new ContextMenu();
                PictureEdit pic = new PictureEdit();
                pic.Properties.ContextMenu = emptyMenu;
                pic.MouseClick += new MouseEventHandler(DeleteShot);
                pic.Size = new Size(81, 62);
                pic.Image = pictureEdit1.Image;
                pic.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
                flowLayoutPanel1.Controls.Add(pic);
            }
        }

        private void ScreenShot()
        {
            if (videPlayer != null && videPlayer.IsRunning)
            {
                System.Drawing.Image img = new Bitmap(videPlayer.Width, videPlayer.Height);
                videPlayer.DrawToBitmap((Bitmap)img, new Rectangle(0, 0, videPlayer.Width, videPlayer.Height));
                //EnterModit();
                ContextMenu emptyMenu = new ContextMenu();
                PictureEdit pic = new PictureEdit();
                pic.Properties.ContextMenu = emptyMenu;
                pic.Size = new Size(81, 62);
                pic.MouseClick += new MouseEventHandler(DeleteShot);
                pic.Image = img;
                pic.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
                flowLayoutPanel1.Controls.Add(pic);

                //this.pictureEdit1.Image = img;
                //btmRotate = (Bitmap)this.pictureEdit1.Image;
            }
            else
            {
                lis.client.control.MessageDialog.Show("未找到图像", "提示");
            }
        }
        /// <summary>
        /// bitmap转bit数组
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public byte[] Bitmap2Byte(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                byte[] data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return data;
            }
        }
        /// <summary>
        /// 截图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_screenShot_Click(object sender, EventArgs e)
        {
            if (panelVideo.Visible == true)
            {
                ScreenShot();
            }
            else
                PictureShot(sender,e);
            //PictureEdit pic = new PictureEdit();
            ////pic.Click += new EventHandler(PictureCheck);
            //pic.Size = new Size(81, 62);
            //pic.Image = pictureEdit1.Image;
            //pic.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            //flowLayoutPanel1.Controls.Add(pic);
            ////panel24.Controls.Add(pic);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_save_click(object sender, EventArgs e)
        {
            //cellMarkList.Clear();
            if (BL_HXBZS.Value < 1)
            {
                lis.client.control.MessageDialog.Show("请设置血片核细胞总数");
                return;
            }
            if (GetCellsNum(panel_BL_HX) < 1)
            {
                lis.client.control.MessageDialog.Show("血片红系细胞总数为零，请检查红系细胞数量");
                return;
            }
            if (SampleType == "骨髓")
            {
                if (MR_HXBZS.Value < 1)
                {
                    lis.client.control.MessageDialog.Show("请设置骨髓片核细胞总数");
                    return;
                }
                if (GetCellsNum(panel_MR_HX) < 1)
                {
                    lis.client.control.MessageDialog.Show("骨髓片红系细胞总数为零，请检查红系细胞数量");
                    return;
                }
                GetAllCellsNum(this.panel_MR, false);
                
                MR_LXBHXB.EditValue = (Convert.ToDouble(GetCellsNum(panel_MR_LX))  / Convert.ToDouble(GetCellsNum(panel_MR_HX)) ).ToString("f1") + ":1";
                GetLXHX(this.panel_MR, false);
                //MR_LXBHXB2.Value = GetCellsNum(panel_MR_LX) / GetCellsNum(panel_MR_HX);
                
            }
            GetAllCellsNum(this.panel_BL,true);


            GetAllShot();
            SaveMarkPicture();
            IsSave = true;
            var list = cellMarkList;
            this.Close();
        }

        /// <summary>
        /// 手动导入图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LoadFile_Click(object sender, EventArgs e)
        {
            panelVideo.Visible = false;
            panelPicture.Visible = true;
            videoSwitch.Text = "切换视频";
            OpenFileDialog ofd = new OpenFileDialog();
            string strFileName = "";
            //载入jpg格式图片
            ofd.Filter = "图片文件|*.jpg*";
            ofd.ValidateNames = true;
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                strFileName = ofd.FileName;
            }
            else
                return;

            cellMarkList.Clear();
            rectangleList.Clear();
            DrawCellMark(cellMarkList);

            //flowLayoutPanel1.Controls.Clear();
            Image im = Image.FromFile(strFileName);
            pictureEdit1.Image = SetImageSize(im, new Size(this.panelPicture.Width, this.panelPicture.Height));

            ContextMenu emptyMenu = new ContextMenu();
            PictureEdit pic = new PictureEdit();
            pic.Properties.ContextMenu = emptyMenu;
            pic.MouseClick += new MouseEventHandler(DeleteShot);
            pic.Size = new Size(81, 62);
            pic.Image = im;
            pic.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            flowLayoutPanel1.Controls.Add(pic);

            var xx = pictureEdit1.Image.Size;
            PreImagMark();
            int offsetY = (this.panelPicture.Height - xx.Height) / 2;
            int offsetX = (this.panelPicture.Width - xx.Width) / 2;
            foreach (RectangleCoordinate coordinate in RectangleCoordinateList)
            {
                coordinate.MinY += offsetY;
                coordinate.MaxY += offsetY;

                coordinate.MinX += offsetX;
                coordinate.MaxX += offsetX;
            }
            var s = RectangleCoordinateList;
        }

        /// <summary>
        /// 载入的图片适应控件大小
        /// </summary>
        /// <param name="imgToResize"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private Image SetImageSize(Image imgToResize,Size size)
        {
            //获取图片宽度
            int sourceWidth = imgToResize.Width;
            //获取图片高度
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //计算宽度的缩放比例
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //计算高度的缩放比例
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            offset = nPercent;
            //期望的宽度
            int destWidth = (int)(sourceWidth * nPercent);
            //期望的高度
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //绘制图像
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }

        private void panelVideo_MouseClick(object sender, MouseEventArgs e)
        {

        }
        /// <summary>
        /// 视频图像切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void videoSwitch_Click(object sender, EventArgs e)
        {
            if (labelSwitch.Text == "切换图片")
            {
                videPlayer.SignalToStop();
                videPlayer.WaitForStop();
                labelSwitch.Text = "切换视频";
                panelVideo.Visible = false;
                panelPicture.Visible = true;
            }
            else
            {
                labelSwitch.Text = "切换图片";
                panelVideo.Visible = true;
                panelPicture.Visible = false;
                CameraConn();
            }
        }
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMarrowMark_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsSave || IsClose)
            {
                return;
            }
            if (lis.client.control.MessageDialog.Show("您确定要关闭吗？", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                videPlayer.SignalToStop();
                videPlayer.WaitForStop();
            }
            else
                e.Cancel = true;
        }
        /// <summary>
        /// 清空所有标记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearMark_Click(object sender, EventArgs e)
        {
            if (lis.client.control.MessageDialog.Show("确定清空所有标记吗？", "确认", MessageBoxButtons.YesNo) ==
                            DialogResult.Yes)
            {
                ClearAllMarkInfo();
            }
            else
                return;
                
        }

        /// <summary>
        /// 视频标记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void videPlayer_MouseClick(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(CurrentCellName))
            {
                if (e.Button == MouseButtons.Right)
                {
                    NumSetting(CurrentCellName, true);
                }
                else
                {
                    NumSetting(CurrentCellName, false);
                }
            }
        }

        /// <summary>
        /// 自动标记所有区域 重复点击清空标记再次点击标记区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutoMark_Click(object sender, EventArgs e)
        {
            if (clearAutoMark)
            {
                cellMarkList.Clear();
                rectangleList.Clear();
                DrawCellMark(cellMarkList);
                btnAutoMark.Text = "显示标记";
            }
            else
            {
                AutoMartkAllCells();
                btnAutoMark.Text = "清空标记";
            }
            
            clearAutoMark = !clearAutoMark;
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (lis.client.control.MessageDialog.Show("您确定要关闭吗？", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                IsClose = true;
                videPlayer.SignalToStop();
                videPlayer.WaitForStop();
                this.Close();
            }
            else
                return;
        }

        //protected override void WndProc(ref Message m)
        //{
        //    int SC_MAXIMIZE = 0xF030;
        //    int WM_SYSCOMMAND = 0x112;
        //    if (m.Msg == WM_SYSCOMMAND)
        //    {
        //        if (m.WParam.ToInt32() == SC_MAXIMIZE)   //拦截窗体最大化按钮
        //        {
        //            var ss = panelPicture.Size;
        //        }
        //    }
        //}
    }
}
