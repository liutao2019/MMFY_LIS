using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace lis.client.control
{
    public class VideoAPI
    {
       
            [DllImport("avicap32.dll")]
            public static extern IntPtr capCreateCaptureWindowA(byte[] lpszWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, int nID);
            [DllImport("avicap32.dll")]
            public static extern bool capGetDriverDescriptionA(short wDriver, byte[] lpszName, int cbName, byte[] lpszVer, int cbVer);
            [DllImport("User32.dll")]
            public static extern bool SendMessage(IntPtr hWnd, int wMsg, bool wParam, int lParam);
            [DllImport("User32.dll")]
            public static extern bool SendMessage(IntPtr hWnd, int wMsg, short wParam, int lParam);
            [DllImport("user32")]
            public static extern bool DestroyWindow(IntPtr hWnd);

            #region
            //        // 开始定义消息参数  整数型
            ////const  avicap32='avicap32.dll';
            //public const int WM_CAP_START= WM_USER=1024;
            //// start of unicode messages
            //public const int WM_CAP_UNICODE_START=            WM_USER+100; //开始
            //public const int WM_CAP_GET_CAPSTREAMPTR=         (WM_CAP_START+  1); //获得 CAPSTR EAMPTR
            //public const int WM_CAP_SET_CALLBACK_ERROR=       (WM_CAP_START+  2); //设置收回错误
            //public const int WM_CAP_SET_CALLBACK_STATUS=      (WM_CAP_START+  3); //设置收回状态
            public const int WM_CAP_SET_CALLBACK_YIELD = (WM_CAP_START + 4); //设置收回出产
            public const int WM_CAP_SET_CALLBACK_FRame = (WM_CAP_START + 5); //设置收回结构
            //public const int WM_CAP_SET_CALLBACK_VIDEOSTREAM = (WM_CAP_START + 6); //设置收回视频流
            public const int WM_CAP_SET_CALLBACK_WAVESTREAM = (WM_CAP_START + 7); //设置收回视频波流
            public const int WM_CAP_GET_USER_DATA = (WM_CAP_START + 8); //获得使用者数据
            public const int WM_CAP_SET_USER_DATA = (WM_CAP_START + 9); //设置使用者数据
            //public const int WM_CAP_DRIVER_CONNECT          =(WM_CAP_START+  10); //驱动程序连接
            //public const int WM_CAP_DRIVER_DISCONNECT       =(WM_CAP_START+  11); //断开启动程序连接
            //public const int WM_CAP_DRIVER_GET_NAME         =(WM_CAP_START+  12); //获得驱动程序名字
            //public const int WM_CAP_DRIVER_GET_VERSION      =(WM_CAP_START+  13); //获得驱动程序版本
            //public const int WM_CAP_DRIVER_GET_CAPS         =(WM_CAP_START+  14); //获得驱动程序帽子
            //public const int WM_CAP_FILE_SET_CAPTURE_FILE    =(WM_CAP_START+  20); //设置捕获文件
            //public const int WM_CAP_FILE_GET_CAPTURE_FILE    =(WM_CAP_START+  21); //获得捕获文件
            //public const int WM_CAP_FILE_SAVEAS              =(WM_CAP_START+  23); //另存文件为
            //public const int WM_CAP_FILE_SAVEDIB             =(WM_CAP_START+  25); //保存文件
            //// out of order to save on ifdefs
            //public const int WM_CAP_FILE_ALLOCATE           =(WM_CAP_START+  22); //分派文件
            //public const int WM_CAP_FILE_SET_INFOCHUNK      =(WM_CAP_START+  24); //设置开始文件
            //public const int WM_CAP_EDIT_COPY               =(WM_CAP_START+  30); //编辑复制
            //public const int WM_CAP_SET_AUDIOFORMAT         =(WM_CAP_START+  35); //设置音频格式
            //public const int WM_CAP_GET_AUDIOFORMAT         =(WM_CAP_START+  36); //捕获音频格式
            //public const int WM_CAP_DLG_VIDEOFORMAT         =(WM_CAP_START+  41); //1065 打开视频格式设置对话框
            //public const int WM_CAP_DLG_VIDEOSOURCE         =(WM_CAP_START+  42); //1066 打开属性设置对话框，设置对比度、亮度等。
            //public const int WM_CAP_DLG_VIDEODISPLAY        =(WM_CAP_START+  43); //1067 打开视频显示
            //public const int WM_CAP_GET_VIDEOFORMAT         =(WM_CAP_START+  44); //1068 获得视频格式
            //public const int WM_CAP_SET_VIDEOFORMAT         =(WM_CAP_START+  45); //1069 设置视频格式
            //public const int WM_CAP_DLG_VIDEOCOMPRESSION    =(WM_CAP_START+  46); //1070 打开压缩设置对话框
            //public const int WM_CAP_SET_PREVIEW             =(WM_CAP_START+  50); //设置预览
            //public const int WM_CAP_SET_OVERLAY             =(WM_CAP_START+  51); //设置覆盖
            //public const int WM_CAP_SET_PREVIEWRATE         =(WM_CAP_START+  52); //设置预览比例
            //public const int WM_CAP_SET_SCALE               =(WM_CAP_START+  53); //设置刻度
            //public const int WM_CAP_GET_STATUS = (WM_CAP_START + 54); //获得状态
            //public const int WM_CAP_SET_SCROLL              =(WM_CAP_START+  55); //设置卷
            //public const int WM_CAP_GRAB_FRame              =(WM_CAP_START+  60); //逮捕结构
            //public const int WM_CAP_GRAB_FRame_NOSTOP       =(WM_CAP_START+  61); //停止逮捕结构
            //public const int WM_CAP_SEQUENCE                =(WM_CAP_START+  62); //次序
            //public const int WM_CAP_SEQUENCE_NOFILE         =(WM_CAP_START+  63); //没有文件
            //public const int WM_CAP_SET_SEQUENCE_SETUP      =(WM_CAP_START+  64); //设置安装次序
            //public const int WM_CAP_GET_SEQUENCE_SETUP      =(WM_CAP_START+  65); //获得安装次序
            //public const int WM_CAP_SET_MCI_DEVICE          =(WM_CAP_START+  66); //设置媒体控制接口
            //public const int WM_CAP_GET_MCI_DEVICE          =(WM_CAP_START+  67); //获得媒体控制接口 
            //public const int WM_CAP_STOP                    =(WM_CAP_START+  68); //停止
            //public const int WM_CAP_ABORT                   =(WM_CAP_START+  69); //异常中断
            //public const int WM_CAP_SINGLE_FRame_OPEN       =(WM_CAP_START+  70); //打开单一的结构
            //public const int WM_CAP_SINGLE_FRame_CLOSE      =(WM_CAP_START+  71); //关闭单一的结构
            //public const int WM_CAP_SINGLE_FRame            =(WM_CAP_START+  72); //单一的结构
            //public const int WM_CAP_PAL_OPEN                =(WM_CAP_START+  80); //打开视频
            //public const int WM_CAP_PAL_SAVE                =(WM_CAP_START+  81); //保存视频
            //public const int WM_CAP_PAL_PASTE               =(WM_CAP_START+  82); //粘贴视频
            //public const int WM_CAP_PAL_AUTOCREATE          =(WM_CAP_START+  83); //自动创造
            //public const int WM_CAP_PAL_MANUALCREATE        =(WM_CAP_START+  84); //手动创造
            //// Following added post VFW 1.1
            //public const int WM_CAP_SET_CALLBACK_CAPCONTROL =(WM_CAP_START+  85); // 设置收回的错误
            //public const int WM_CAP_END                      =WM_CAP_SET_CALLBACK_CAPCONTROL; 
            #endregion
            //
            public const int WM_USER = 0x400;
            public const int WS_CHILD = 0x40000000;
            public const int WS_VISIBLE = 0x10000000;
            public const int WM_CAP_START = WM_USER;
            public const int WM_CAP_STOP = WM_CAP_START + 68;//停止
            public const int WM_CAP_DRIVER_CONNECT = WM_CAP_START + 10;//驱动程序连接
            public const int WM_CAP_DRIVER_DISCONNECT = WM_CAP_START + 11;//断开启动程序连接
            public const int WM_CAP_SAVEDIB = WM_CAP_START + 25;//保存文件
            public const int WM_CAP_GRAB_FRAME = WM_CAP_START + 60;//逮捕结构
            public const int WM_CAP_SEQUENCE = WM_CAP_START + 62;//次序
            public const int WM_CAP_FILE_SET_CAPTURE_FILEA = WM_CAP_START + 20;//设置捕获文件
            public const int WM_CAP_SEQUENCE_NOFILE = WM_CAP_START + 63;
            public const int WM_CAP_SET_OVERLAY = WM_CAP_START + 51;
            public const int WM_CAP_SET_PREVIEW = WM_CAP_START + 50;
            public const int WM_CAP_SET_CALLBACK_VIDEOSTREAM = WM_CAP_START + 6;
            public const int WM_CAP_SET_CALLBACK_ERROR = WM_CAP_START + 2;
            public const int WM_CAP_SET_CALLBACK_STATUSA = WM_CAP_START + 3;
            public const int WM_CAP_SET_CALLBACK_FRAME = WM_CAP_START + 5;
            public const int WM_CAP_SET_SCALE = WM_CAP_START + 53;
            public const int WM_CAP_SET_PREVIEWRATE = WM_CAP_START + 52;//设置预览比例
            public const int WM_CAP_DLG_VIDEOFORMAT = (WM_CAP_START + 41); //1065 打开视频格式设置对话框
            public const int WM_CAP_DLG_VIDEOSOURCE = WM_CAP_START + 42;//1066 打开属性设置对话框，设置对比度、亮度等
            public const int WM_CAP_GET_VIDEOFORMAT = (WM_CAP_START + 44); //1068 获得视频格式
            public const int WM_CAP_SET_VIDEOFORMAT = (WM_CAP_START + 45); //1069 设置视频格式
            //public const int WM_CAP_FILE_SET_CAPTURE_FILE    =(WM_CAP_START+  20); //设置捕获文件
            //public const int WM_CAP_FILE_GET_CAPTURE_FILE    =(WM_CAP_START+  21); //获得捕获文件
        

    }
}
