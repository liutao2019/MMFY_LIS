using dcl.client.frame;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace dcl.client.result
{
    public partial class FrmImagView : FrmCommon
    {
        List<string> filesName = new List<string>();
        public List<Image> imagelist = new List<Image>();
        public Image currentImage;
        int filesNum = 0;
        int currentIndex = 0;
        public FrmImagView()
        {
            InitializeComponent();
        }

        int zoom = 20;
        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            pictureEdit1.Properties.ZoomPercent += zoom;            
        }

        private void FrmImagView_Load(object sender, EventArgs e)
        {
            currentIndex = imagelist.IndexOf(currentImage);
            pictureEdit1.Image = imagelist[currentIndex];
            filesNum = imagelist.Count;
            //DirectoryInfo folder = new DirectoryInfo(Application.StartupPath + @"\MarrowImag\");
            //foreach (FileInfo file in folder.GetFiles())
            //{
            //    filesName.Add(file.FullName);
            //}
            //if (filesName != null && filesName.Count > 0)
            //{
            //    filesNum = filesName.Count;
            //    pictureEdit1.Image = Image.FromFile(filesName[0]);
            //}
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            pictureEdit1.Properties.ZoomPercent -= zoom;
        }

        private void nextPic_Click(object sender, EventArgs e)
        {
            try
            {
                currentIndex++;
                if (currentIndex > filesNum - 1)
                    currentIndex = 0;
                //pictureEdit1.Image = Image.FromFile(filesName[currentIndex]);
                pictureEdit1.Image = imagelist[currentIndex];
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }

        }

        private void lastPic_Click(object sender, EventArgs e)
        {
            currentIndex--;
            if (currentIndex < 0)
                currentIndex = filesNum - 1;
            //pictureEdit1.Image = Image.FromFile(filesName[currentIndex]);
            pictureEdit1.Image = imagelist[currentIndex];
        }

        private void btnRotation_Click(object sender, EventArgs e)
        {
            Image picClone = (Image)pictureEdit1.Image.Clone();
            pictureEdit1.Image = RotateImg(picClone, 90);
            picClone.Dispose();
        }

        /// <summary>
        /// 图像旋转
        /// </summary>
        /// <param name="b"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public Image RotateImg(Image b, int angle)
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
