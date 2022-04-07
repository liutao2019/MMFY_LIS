using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using DevExpress.LookAndFeel;

namespace dcl.client.msgsend
{
    static class Program
    {
        /// <summary> 
        /// 该函数设置由不同线程产生的窗口的显示状态。 
        /// </summary> 
        /// <param name="hWnd">窗口句柄</param> 
        /// <param name="cmdShow">指定窗口如何显示。查看允许值列表，请查阅ShowWlndow函数的说明部分。</param> 
        /// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零。</returns> 
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        /// <summary> 
        /// 该函数将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。系统给创建前台窗口的线程分配的权限稍高于其他线程。 
        /// </summary> 
        /// <param name="hWnd">将被激活并被调入前台的窗口句柄。</param> 
        /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零。</returns> 
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private const int WS_SHOWNORMAL = 1;

        /// <summary>
        /// 窗口显示最前端
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="dwTime"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        const int AW_HOR_POSITIVE = 0x0001;
        const int AW_HOR_NEGATIVE = 0x0002;
        const int AW_VER_POSITIVE = 0x0004;
        const int AW_VER_NEGATIVE = 0x0008;
        const int AW_CENTER = 0x0010;
        const int AW_HIDE = 0x10000;
        const int AW_ACTIVATE = 0x20000;
        const int AW_SLIDE = 0x40000;
        const int AW_BLEND = 0x80000;

        [STAThread]
        static void Main(params string[] args)
        {

            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.Skins.SkinManager.EnableFormSkins();
            Application.SetCompatibleTextRenderingDefault(false);
            UserLookAndFeel.Default.UseDefaultLookAndFeel = true;
            UserLookAndFeel.Default.SetSkinStyle("Visual Studio 2013 Blue");

            Process instance = RunningInstance();
            if (instance == null)
            {
                Application.Run(new FrmDIYCritical());
            }
            else
            {
                HandleRunningInstance(instance);
            }
        }

        /// <summary> 
        /// 获取正在运行的实例，没有运行的实例返回null; 
        /// </summary> 
        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            foreach (Process process in processes)
            {
                if (process.Id != current.Id)
                {
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                    {
                        return process;
                    }
                }
            }
            return null;
        }

        /// <summary> 
        /// 显示已运行的程序。 
        /// </summary> 
        public static void HandleRunningInstance(Process instance)
        {
            ShowWindowAsync(instance.MainWindowHandle, WS_SHOWNORMAL); //显示，可以注释掉 
            SetForegroundWindow(instance.MainWindowHandle);            //放到前端 
            ShowWindow(instance.MainWindowHandle, (int)ShowWindowOption.SW_MAXIMIZE);

            //AnimateWindow(instance.MainWindowHandle, 1000, AW_ACTIVATE | AW_BLEND);//从下到上且不占其它程序焦点 
        }

        ///<summary>向指定窗体句柄发送显示状态</summary>
        /// <param name="hwnd">窗体句柄</param>
        /// <param name="nCmdShow">窗体显示参数(如下参数例表)</param>
        /// <returns>是否成功</returns>
        ///<remarks>
        ///<para> nCmdShow:为以下其参数例表</para>
        ///</remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "ShowWindow")]
        public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

        /// <summary>
        /// ShowWindow函数枚举值
        /// </summary>
        public enum ShowWindowOption
        {
            /// <summary>
            /// 隐藏窗口，活动状态给令一个窗口
            /// </summary>
            SW_HIDE = 0,
            /// <summary>
            /// 用原来的大小和位置显示一个窗口，同时令其进入活动状态
            /// </summary>
            SW_SHOWNORMAL = 1,
            /// <summary>
            /// 最小化窗口，并将其激活
            /// </summary>
            SW_SHOWMINIMIZED = 2,
            /// <summary>
            /// 最大化窗口，并将其激活
            /// </summary>
            SW_SHOWMAXIMIZED = 3,
            /// <summary>
            /// 最大化窗口，并将其激活
            /// </summary>
            SW_MAXIMIZE = 3,
            /// <summary>
            /// 用最近的大小和位置显示一个窗口，同时不改变活动窗口
            /// </summary>
            SW_SHOWNOACTIVATE = 4,
            /// <summary>
            /// 用当前的大小和位置显示一个窗口，同时令其进入活动状态
            /// </summary>
            SW_SHOW = 5,
            /// <summary>
            /// 最小化窗口，活动状态给令一个窗口
            /// </summary>
            SW_MINIMIZE = 6,
            /// <summary>
            /// 最小化一个窗口，同时不改变活动窗口
            /// </summary>
            SW_SHOWMINNOACTIVE = 7,
            /// <summary>
            /// 用当前的大小和位置显示一个窗口，不改变活动窗口
            /// </summary>
            SW_SHOWNA = 8,
            /// <summary>
            /// 用原来的大小和位置显示一个窗口，同时令其进入活动状态
            /// </summary>
            SW_RESTORE = 9
        }
    }
}
