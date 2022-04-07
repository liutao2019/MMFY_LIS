using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using dcl.client.wcf;
using dcl.entity;

namespace dcl.client.notifyclient
{
    /// <summary>
    /// LED内部通知
    /// </summary>
    public partial class FrmShowLEDMsg : Form
    {
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

        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();//获得当前活动窗体
        [DllImport("user32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr hwnd);//设置活动窗体

        /// <summary>
        /// 是否强关闭窗口
        /// </summary>
        private bool IsStrongClose = false;

        public FrmShowLEDMsg()
        {
            InitializeComponent();
            this.Hide();
            IsStrongClose = false;
        }

        /// <summary>
        /// 开始运行
        /// </summary>
        public void startShowFrm()
        {
            //设置窗口位置的位置,默认置顶居中
            int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width/2 - this.Width/2;
            //int y = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - this.Height;
            this.SetDesktopLocation(x, 0);
            this.Hide();
            IsStrongClose = false;
            this.timer1.Start();//启动计时器
        }

        public void showText(string str)
        {
            scrollingText1.ScrollText = string.IsNullOrEmpty(str)?"当前无消息":str;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timer1 != null) timer1.Stop();

            try
            {
                //ProxyMessage proxy = new ProxyMessage();
                //List<dcl.pub.entities.Message.EntityMessageContent> result = proxy.Service.GetLEDMessage(false, false);
                ProxyObrMessageContent proxyMsgContent = new ProxyObrMessageContent();
                List<EntityDicObrMessageContent> result = proxyMsgContent.Service.GetLEDMessage(false, false);//获取LED消息

                if (result != null && result.Count > 0)
                {
                    string strlist = "";
                    foreach (var msgcontent in result)
                    {
                        if (!string.IsNullOrEmpty(msgcontent.ObrContent))
                        {
                            if (string.IsNullOrEmpty(strlist))
                            {
                                strlist = msgcontent.ObrContent;
                            }
                            else
                            {
                                strlist += "   " + msgcontent.ObrContent;
                            }
                        }
                    }

                    showText(strlist);

                    if (!this.Visible)
                    {
                        IntPtr activeForm = GetActiveWindow();
                        this.Show();
                        if (this.Size.Height != 25) this.Size = new Size(354, 25);
                        SetActiveWindow(activeForm);
                    }
                }
                else
                {
                    showText("");
                }
            }
            catch (Exception ex)
            {
                showText("LED显示遇到错误：" + ex.Message);
            }

            if (timer1 != null && !IsStrongClose) timer1.Start();
        }

        /// <summary>
        /// 强制关闭窗体
        /// </summary>
        public void StriogClose()
        {
            IsStrongClose = true;

            this.Close();

            GC.Collect();
        }

        private void scrollingText1_DoubleClick(object sender, EventArgs e)
        {
            if (scrollingText1!=null&&!string.IsNullOrEmpty(scrollingText1.ScrollText))
            {
                MessageBox.Show(scrollingText1.ScrollText);
            }
        }
    }
}
