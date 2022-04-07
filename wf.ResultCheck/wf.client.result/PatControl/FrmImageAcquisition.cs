using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using lis.client.control;
using dcl.client.wcf;
using dcl.client.frame;
using dcl.client.common;

using System.Drawing.Imaging;
using dcl.client.sample;
using Camera_NET;
using System.Runtime.InteropServices;
using AForge.Video.DirectShow;

namespace dcl.client.result.PatControl
{
    public partial class FrmImageAcquisition : FrmCommon
    {
        Bitmap btmRotate = null;  //旋转图像
        Bitmap btmContrast = null;  //调整对比度图像
        Bitmap btmLight = null;  //调整亮度原始图像
        bool boolConstrast = false;
        bool boolLight = false;
        string PatID = string.Empty;
        bool save = false;  //判断是否已保存

        private FilterInfoCollection videoDevices;
        public bool show = false;

        public FrmImageAcquisition(string Pid)
        {
            InitializeComponent();

            this.Load += new EventHandler(FrmImageAcquisition_Load);
            this.FormClosing += new FormClosingEventHandler(FrmImageAcquisition_FormClosing);

            this.tb_contrast.ValueChanged += new EventHandler(tb_contrast_ValueChanged);
            this.tb_light.ValueChanged += new EventHandler(tb_light_ValueChanged);
            this.tb_blue.ValueChanged += new EventHandler(tb_blue_ValueChanged);
            this.tb_green.ValueChanged += new EventHandler(tb_green_ValueChanged);
            this.tb_red.ValueChanged += new EventHandler(tb_red_ValueChanged);
            PatID = Pid;
            if (Devices() < 1)
            {
                return;
            }
            show = true;
            CameraConn();
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FrmImageAcquisition_Load(object sender, EventArgs e)
        {
            this.sysToolBar1.SetToolButtonStyle(new string[] {
                                                        sysToolBar1.btnCalculation.Name,
                                                        sysToolBar1.BtnUndo.Name,
                                                        sysToolBar1.BtnResultView.Name,
                                                        sysToolBar1.btnReturn.Name,
                                                        sysToolBar1.BtnClose.Name
                                                            });

            sysToolBar1.btnCalculation.Caption = "保存图像";
            sysToolBar1.btnReturn.Caption = "取消";
            sysToolBar1.BtnUndo.Caption = "图像翻转";
            sysToolBar1.BtnResultView.Caption = "截图";

            sysToolBar1.OnResultViewClicked += BtnResultView_Click;
            sysToolBar1.OnBtnReturnClicked += btnReturn_Click;
            sysToolBar1.BtnUndoClick += BtnUndo_Click;


            pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            ExitModit();
        }

        /// <summary>
        /// 点击图像翻转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BtnUndo_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.pictureEdit1.Image != null)
                {
                    EnterModit();
                    Image tmp = this.pictureEdit1.Image;
                    tmp = ImageSetting.RotateImg(tmp, 90);
                    //tmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    this.pictureEdit1.Image = tmp;
                    btmRotate = (Bitmap)this.pictureEdit1.Image;
                }
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show("捕捉失败：" + ex.Message, "提示");
            }
        }

        #region MyRegion
        ///// <summary>
        ///// 导入图像
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void BtnImport_Click(object sender, EventArgs e)
        //{
        //    OpenFileDialog ofd = new OpenFileDialog();
        //    ofd.Filter = "JPG Files(*.jpg;*.jpeg)|*.jpg;*.jpeg|BMP Files(*.bmp)|*.bmp|All Files(*.*)|*.*";
        //    ofd.CheckFileExists = true;
        //    ofd.CheckPathExists = true;
        //    if (ofd.ShowDialog() == DialogResult.OK)
        //    {
        //        Bitmap bmp = new Bitmap(ofd.FileName);
        //        if (bmp == null)
        //        {
        //            MessageBox.Show("加载图片失败!", "错误");
        //            return;
        //        }
        //        ofd.Dispose();
        //        pk.Stop();
        //        ExitModit();
        //        this.pictureEdit1.Image = bmp;
        //        btmOriginal = (Bitmap)pictureEdit1.Image;
        //    }
        //} 

        ///// <summary>
        ///// 使用原图
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void BtnResultView_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (btmOriginal != null)
        //        {
        //            if (this.pictureEdit1.Image != null)
        //            {
        //                EnterModit();
        //                this.pictureEdit1.Image.Dispose();
        //                this.pictureEdit1.Image = btmOriginal;
        //                //btmRotate = btmOriginal;
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        lis.client.control.MessageDialog.Show("调整图像失败：" + ex.Message, "提示");
        //    }
        //}

        ///// <summary>
        ///// 取消
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void BtnCancel_Click(object sender, EventArgs e)
        //{
        //    EnterModit();
        //} 
        #endregion

        /// <summary>
        /// 对比度调整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tb_contrast_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                int degree = tb_contrast.Value;

                //调整过亮度
                if (boolLight)
                {
                    //使用调整过亮度的图像
                    var copyBtm = btmLight.Clone() as Bitmap;
                    this.pictureEdit1.Image = ImageSetting.ContrastP(copyBtm, degree);
                }
                //未调整过亮度
                else
                {
                    //使用原始图像
                    var copyBtm = btmRotate.Clone() as Bitmap;
                    this.pictureEdit1.Image = ImageSetting.ContrastP(copyBtm, degree);
                }
                btmContrast = (Bitmap)this.pictureEdit1.Image;
                boolConstrast = true;
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show("调整图像失败：" + ex.Message, "提示");
            }

        }

        /// <summary>
        /// 亮度调整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tb_light_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                int degree = tb_light.Value;
                //调整过对比度
                if (boolConstrast)
                {
                    //使用调整过对比度的图像
                    var copyBtm = btmContrast.Clone() as Bitmap;
                    this.pictureEdit1.Image = ImageSetting.KiLighten(copyBtm, degree);
                }
                //未调整过对比度
                else
                {
                    //使用原始图像
                    var copyBtm = btmRotate.Clone() as Bitmap;
                    this.pictureEdit1.Image = ImageSetting.KiLighten(copyBtm, degree);

                }
                btmLight = (Bitmap)this.pictureEdit1.Image;
                boolLight = true;
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show("调整图像失败：" + ex.Message, "提示");
            }
        }

        /// <summary>
        /// 红色调整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tb_red_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Bitmap copyBtm = btmRotate.Clone() as Bitmap;
                //this.pictureEdit1.Image = ImageSetting.Tricolor(copyBtm, tb_red.Value, 2);
                this.pictureEdit1.Image = ImageSetting.KiColorBalance(copyBtm, tb_red.Value, tb_green.Value, tb_blue.Value);
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show("调整图像失败：" + ex.Message, "提示");
            }

        }

        /// <summary>
        /// 绿色调整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tb_green_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Bitmap copyBtm = btmRotate.Clone() as Bitmap;
                //this.pictureEdit1.Image = ImageSetting.Tricolor(copyBtm, tb_green.Value, 1);
                this.pictureEdit1.Image = ImageSetting.KiColorBalance(copyBtm, tb_red.Value, tb_green.Value, tb_blue.Value);
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show("调整图像失败：" + ex.Message, "提示");
            }
        }

        /// <summary>
        /// 蓝色调整
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tb_blue_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Bitmap copyBtm = btmRotate.Clone() as Bitmap;
                //this.pictureEdit1.Image = ImageSetting.Tricolor(copyBtm, tb_blue.Value, 0);
                this.pictureEdit1.Image = ImageSetting.KiColorBalance(copyBtm, tb_red.Value, tb_green.Value, tb_blue.Value);
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show("调整图像失败：" + ex.Message, "提示");
            }

        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FrmImageAcquisition_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (videPlayer!=null)
            //videPlayer.Dispose();
            if (this.pictureEdit1.Image != null && !save)
            {
                if (e.CloseReason != CloseReason.ApplicationExitCall && lis.client.control.MessageDialog.Show("图像已采集，尚未保存，您确定要退出图像采集吗？", "确认", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    videPlayer.SignalToStop();
                    videPlayer.WaitForStop();
                    videPlayer.Dispose();
                }
            }
            if (this.pictureEdit1.Image == null)
            {
                videPlayer.SignalToStop();
                videPlayer.WaitForStop();
                videPlayer.Dispose();
            }
        }

        /// <summary>
        /// 图像编辑模式
        /// </summary>
        private void EnterModit()
        {
            sysToolBar1.btnReturn.Enabled = true;
            sysToolBar1.BtnResultView.Enabled = false;
            this.tb_contrast.Enabled = true;
            this.tb_light.Enabled = true;
            this.tb_red.Enabled = true;
            this.tb_green.Enabled = true;
            this.tb_blue.Enabled = true;
            this.tb_contrast.Value = 0;
            this.tb_light.Value = 0;
            this.tb_red.Value = 0;
            this.tb_green.Value = 0;
            this.tb_blue.Value = 0;
            boolConstrast = false;
            boolLight = false;
        }

        /// <summary>
        /// 图像不可编辑模式
        /// </summary>
        private void ExitModit()
        {
            sysToolBar1.btnReturn.Enabled = false;
            sysToolBar1.BtnResultView.Enabled = true;
            this.tb_contrast.Enabled = false;
            this.tb_light.Enabled = false;
            this.tb_red.Enabled = false;
            this.tb_green.Enabled = false;
            this.tb_blue.Enabled = false;
        }


        /// <summary>
        /// 截图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BtnResultView_Click(object sender, EventArgs e)
        {
            if (videPlayer != null && videPlayer.IsRunning)
            {
                System.Drawing.Image img = new Bitmap(videPlayer.Width, videPlayer.Height);
                videPlayer.DrawToBitmap((Bitmap)img, new Rectangle(0, 0, videPlayer.Width, videPlayer.Height));
                EnterModit();
                this.pictureEdit1.Image = img;
                btmRotate = (Bitmap)this.pictureEdit1.Image;
            }
            else
            {
                lis.client.control.MessageDialog.Show("未找到图像", "提示");
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnReturn_Click(object sender, EventArgs e)
        {
            this.pictureEdit1.Image = null;
            ExitModit();
        }

        //连接摄像头
        private void CameraConn()
        {
            try
            {
                VideoCaptureDevice videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.DesiredFrameSize = new Size(320, 240);
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
        /// 获取视频设备
        /// </summary>
        private int Devices()
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

    }

    /// <summary>
    /// 图像设置类
    /// </summary>
    class ImageSetting
    {
        /// <summary>
        /// 亮度调整
        /// </summary>
        /// <param name="btm"></param>
        /// <param name="degree"></param>
        public static Bitmap KiLighten(Bitmap btm, int degree)
        {
            if (btm == null)
            {
                return null;
            }
            //确定最小值和最大值
            if (degree < -255) degree = -255;
            if (degree > 255) degree = 255;
            try
            {
                //确定图像的宽和高
                int width = btm.Width;
                int height = btm.Height;

                int pix = 0;
                //LockBits将Bitmap锁定到内存中
                BitmapData data = btm.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                unsafe
                {
                    //p指向地址
                    byte* p = (byte*)data.Scan0;//8位无符号整数
                    int offset = data.Stride - width * 3;
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            // 处理指定位置像素的亮度
                            for (int i = 0; i < 3; i++)
                            {
                                pix = p[i] + degree;
                                if (degree < 0) p[i] = (byte)Math.Max(0, pix);
                                if (degree > 0) p[i] = (byte)Math.Min(255, pix);
                            } // i
                            p += 3;
                        } // x
                        p += offset;
                    } // y
                }
                btm.UnlockBits(data);//从内存中解除锁定

                return btm;
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// 对比度调整
        /// </summary>
        /// <param name="btm"></param>
        /// <param name="v"></param>
        public static Bitmap ContrastP(Bitmap btm, int degree)
        {
            if (btm == null)
            {
                return null;
            }

            if (degree < -100) degree = -100;
            if (degree > 100) degree = 100;

            try
            {

                double pixel = 0;
                double contrast = (100.0 + degree) / 100.0;
                contrast *= contrast;
                int width = btm.Width;
                int height = btm.Height;
                BitmapData data = btm.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                unsafe
                {
                    byte* p = (byte*)data.Scan0;
                    int offset = data.Stride - width * 3;
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            // 处理指定位置像素的对比度
                            for (int i = 0; i < 3; i++)
                            {
                                pixel = ((p[i] / 255.0 - 0.5) * contrast + 0.5) * 255;
                                if (pixel < 0) pixel = 0;
                                if (pixel > 255) pixel = 255;
                                p[i] = (byte)pixel;
                            } // i
                            p += 3;
                        } // x
                        p += offset;
                    } // y
                }
                btm.UnlockBits(data);
                return btm;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 三原色调整
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="tem_Value"></param>
        /// <param name="tb"></param>
        public static Bitmap Tricolor(Bitmap bmp, int tem_Value, int i)
        {
            if (bmp == null)
            {
                return null;
            }

            try
            {
                int pix = 0;
                BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                unsafe
                {
                    byte* p = (byte*)(data.Scan0);
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        for (int x = 0; x < bmp.Width; x++)
                        {
                            // 处理指定位置像素的颜色
                            pix = p[i] + tem_Value;
                            if (tem_Value < 0) p[i] = (byte)Math.Max(0, pix);
                            if (tem_Value > 0) p[i] = (byte)Math.Min(255, pix);
                            p += 3;
                        }
                        p += data.Stride - data.Width * 3;
                    }
                }
                bmp.UnlockBits(data);
                return bmp;
            }
            catch
            {

                return null;
            }
        }

        /// <summary>
        /// 色彩调整
        /// </summary>
        /// <param name="bmp">原始图</param>
        /// <param name="rVal">r增量</param>
        /// <param name="gVal">g增量</param>
        /// <param name="bVal">b增量</param>
        /// <returns>处理后的图</returns>
        public static Bitmap KiColorBalance(Bitmap bmp, int rVal, int gVal, int bVal)
        {

            if (bmp == null)
            {
                return null;
            }


            int h = bmp.Height;
            int w = bmp.Width;

            try
            {
                if (rVal > 255 || rVal < -255 || gVal > 255 || gVal < -255 || bVal > 255 || bVal < -255)
                {
                    return null;
                }

                BitmapData srcData = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                unsafe
                {
                    byte* p = (byte*)srcData.Scan0.ToPointer();

                    int nOffset = srcData.Stride - w * 3;
                    int r, g, b;

                    for (int y = 0; y < h; y++)
                    {
                        for (int x = 0; x < w; x++)
                        {

                            b = p[0] + bVal;
                            if (bVal >= 0)
                            {
                                if (b > 255) b = 255;
                            }
                            else
                            {
                                if (b < 0) b = 0;
                            }

                            g = p[1] + gVal;
                            if (gVal >= 0)
                            {
                                if (g > 255) g = 255;
                            }
                            else
                            {
                                if (g < 0) g = 0;
                            }

                            r = p[2] + rVal;
                            if (rVal >= 0)
                            {
                                if (r > 255) r = 255;
                            }
                            else
                            {
                                if (r < 0) r = 0;
                            }

                            p[0] = (byte)b;
                            p[1] = (byte)g;
                            p[2] = (byte)r;

                            p += 3;
                        }

                        p += nOffset;


                    }
                } // end of unsafe

                bmp.UnlockBits(srcData);

                return bmp;
            }
            catch
            {
                return null;
            }

        } // end of color

        /// <summary>
        /// 图像旋转
        /// </summary>
        /// <param name="b"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Image RotateImg(Image b, int angle)
        {

            angle = angle % 360;


            //弧度转换  

            double radian = angle * Math.PI / 180.0;

            double cos = Math.Cos(radian);

            double sin = Math.Sin(radian);


            //原图的宽和高  

            int w = b.Width;

            int h = b.Height;

            int W = (int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));

            int H = (int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));


            //目标位图  

            Bitmap dsImage = new Bitmap(W, H);

            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(dsImage);

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;


            //计算偏移量  

            Point Offset = new Point((W - w) / 2, (H - h) / 2);


            //构造图像显示区域：让图像的中心与窗口的中心点一致  

            Rectangle rect = new Rectangle(Offset.X, Offset.Y, w, h);

            Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);


            g.TranslateTransform(center.X, center.Y);

            g.RotateTransform(360 - angle);


            //恢复图像在水平和垂直方向的平移  

            g.TranslateTransform(-center.X, -center.Y);

            g.DrawImage(b, rect);


            //重至绘图的所有变换  

            g.ResetTransform();


            g.Save();

            g.Dispose();

            //保存旋转后的图片  

            b.Dispose();

            //dsImage.Save("FocusPoint.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

            return dsImage;

        }
    }
}
