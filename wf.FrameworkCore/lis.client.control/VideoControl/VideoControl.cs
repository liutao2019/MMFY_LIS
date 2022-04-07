using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;


namespace lis.client.control
{
    public class VideoControl
    {
        private IntPtr lwndC;       //保存无符号句柄
        private IntPtr mControlPtr; //保存管理指示器
        private int mWidth;
        private int mHeight;
        public VideoControl(IntPtr handle, int width, int height)
        {
            mControlPtr = handle;
            mWidth = width;
            mHeight = height;
        }
        /// <summary>
        /// 打开摄像头
        /// </summary>
        public void StartWebCam()
        {
            byte[] lpszName = new byte[100];
            byte[] lpszVer = new byte[100];
            VideoAPI.capGetDriverDescriptionA(0, lpszName, 100, lpszVer, 100);
            lwndC = VideoAPI.capCreateCaptureWindowA(lpszName, VideoAPI.WS_CHILD | VideoAPI.WS_VISIBLE, 0, 0, mWidth, mHeight, mControlPtr, 0);
            if (VideoAPI.SendMessage(lwndC, VideoAPI.WM_CAP_DRIVER_CONNECT, 0, 0))//驱动程序连接
            {
                VideoAPI.SendMessage(lwndC, VideoAPI.WM_CAP_SET_PREVIEWRATE, 100, 0);//设置预览比例 52
                VideoAPI.SendMessage(lwndC, VideoAPI.WM_CAP_SET_PREVIEW, true, 0);//设置预览 50
            }
        }
        /// <summary>
        /// 关闭摄像头
        /// </summary>
        public void CloseWebCam()
        {
            VideoAPI.SendMessage(lwndC, VideoAPI.WM_CAP_DRIVER_DISCONNECT, 0, 0);//断开启动程序连接
            VideoAPI.DestroyWindow(lwndC);
        }
        public void Fors()
        {
            VideoAPI.DestroyWindow(lwndC);
        }

        /// <summary>
        /// 抓图
        /// </summary>
        /// <param name="path">要保存hBmp文件的路径</param>
        public void GrabImage(string path)
        {
            try
            {
                IntPtr hBmp = Marshal.StringToHGlobalAnsi(path);
                VideoAPI.SendMessage(lwndC, VideoAPI.WM_CAP_SAVEDIB, 0, hBmp.ToInt32());//保存文件
            }
            catch
            {
            }
        }

        /// <summary>
        /// 抓图
        /// </summary>
        /// <returns></returns>
        public Image GrabImage()
        {
            VideoAPI.SendMessage(lwndC, 0x41e, 0, 0);
            IDataObject obj1 = Clipboard.GetDataObject();
              //  判断数据是否为BITMAP
            if (obj1.GetDataPresent(typeof(Bitmap)))
            {
                //保存图像
                Image image1 = (Image)obj1.GetData(typeof(Bitmap));
                
                return image1;
            }

            return null;
         
        }
        
        /// <summary>
        /// 开始录像
        /// </summary>
        /// <param name="path">要保存bBmp文件的路径</param>
        public void Kinescope(string path)
        {
            IntPtr bBmp = Marshal.StringToHGlobalAnsi(path);
            VideoAPI.SendMessage(lwndC, VideoAPI.WM_CAP_FILE_SET_CAPTURE_FILEA, 0, bBmp.ToInt32());//设置捕获文件
            VideoAPI.SendMessage(lwndC, VideoAPI.WM_CAP_SEQUENCE, 0, 0);//次序
        }
        /// <summary>
        /// 结束录像
        /// </summary>
        public void StopKinescope()
        {
            VideoAPI.SendMessage(lwndC, VideoAPI.WM_CAP_STOP, 0, 0);//停止
        }
        /// <summary>
        /// 打开属性设置对话框，设置对比度、亮度等
        /// </summary>
        public void OpenProperty()
        {
            VideoAPI.SendMessage(lwndC, VideoAPI.WM_CAP_DLG_VIDEOSOURCE, true, 0);
        }
        /// <summary>
        /// 打开视频格式
        /// </summary>
        public void OpenImgSize()
        {
            VideoAPI.SendMessage(lwndC, VideoAPI.WM_CAP_DLG_VIDEOFORMAT, 0, 0);
        }
        public void d60()
        {
            VideoAPI.SendMessage(lwndC, VideoAPI.WM_CAP_GRAB_FRAME, true, 0);
        }
        public void d61()
        {
            VideoAPI.SendMessage(lwndC, VideoAPI.WM_CAP_START + 35, true, 0);
        }
        ///// <summary>
        ///// 录像
        ///// </summary>
        ///// <param name="path">要保存avi文件的路径</param>
        //public void Kinescope(string path)
        //{
        //    IntPtr hBmp = Marshal.StringToHGlobalAnsi(path);
        //    SendMessage(hWndC, WM_CAP_FILE_SET_CAPTURE_FILEA, 0, hBmp.ToInt64());
        //    SendMessage(hWndC, WM_CAP_SEQUENCE, 0, 0);
        //}
        ///// <summary>
        ///// 停止录像
        ///// </summary>
        //public void StopKinescope()
        //{
        //    SendMessage(hWndC, WM_CAP_STOP, 0, 0);
        //}
        //public void GrabImage(IntPtr hWndC, string path)
        //{
        //    IntPtr hBmp = Marshal.StringToHGlobalAnsi(path);
        //    VideoAPI.SendMessage(lwndC, VideoAPI.WM_CAP_SAVEDIB, 0, hBmp.ToInt32());
        //}

    }

}
