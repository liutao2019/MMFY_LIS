using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace lis.client.control
{
    /// <summary>
    /// 视频输出控件
    /// </summary>
    public partial class VideoOutputControl : DevExpress.XtraEditors.XtraUserControl
    {
        #region 全局变量

        VideoControl videoCon = null;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public VideoOutputControl()
        {
            InitializeComponent();
            
        }

        private void VideoOutputControl_Load(object sender, EventArgs e)
        {
            videoCon = new VideoControl(pictureEdit1.Handle,pictureEdit1.Width,pictureEdit1.Height);
          

        }

        private void btnSetCameraImage_Click(object sender, EventArgs e)
        {
            videoCon.OpenProperty();
        }

        private void btnSetFormat_Click(object sender, EventArgs e)
        {
            videoCon.OpenImgSize();
        }

        private void btnTruncate_Click(object sender, EventArgs e)
        {
            try
            {
                
                Image ShowImage = null;
                ShowImage = videoCon.GrabImage();
                videoCon.CloseWebCam();
                if (ShowImage != null)
                {
                    pictureEdit1.Image = ShowImage;
                }
            }
            catch (Exception objEx)
            {
                MessageBox.Show(objEx.Message);
            }
        }

        private void btnOpenCamera_Click(object sender, EventArgs e)
        {
            videoCon.StartWebCam();
        }

        private void btnCloseCamera_Click(object sender, EventArgs e)
        {
            videoCon.CloseWebCam();
        }
    }
}
