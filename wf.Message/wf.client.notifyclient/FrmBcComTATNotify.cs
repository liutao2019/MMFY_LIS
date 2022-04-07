using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using dcl.client.wcf;
using dcl.entity;
using System.Linq;

namespace dcl.client.notifyclient
{
    public partial class FrmBcComTATNotify : Form
    {
        /// <summary>
        /// 定时器
        /// </summary>
        private Timer readTimer = null;

        /// <summary>
        /// 是否强关闭窗口
        /// </summary>
        private bool IsStrongClose = false;


        /// <summary>
        /// 信息条数
        /// </summary>
        private int msgCount = 0;

        private FrmShowBcTATClew frmShowBcTATClewText = null;

        /// <summary>
        /// 大窗口是否被隐藏
        /// </summary>
        private bool IsFrmParentHide { get; set; }

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
        Color gridrowColor = Color.Empty;

        public FrmBcComTATNotify()
        {
            InitializeComponent();
            this.Hide();
            IsFrmParentHide = true;
            IsStrongClose = false;
        }

        /// <summary>
        /// 开始运行
        /// </summary>
        public void startShowFrm()
        {
            frmShowBcTATClewText = new FrmShowBcTATClew();
            frmShowBcTATClewText.scrollingText1.Click += new EventHandler(scrollingText1_Click);
            frmShowBcTATClewText.TopMost = true;
            //设置frmShowBcTATClewText的位置,默认右下角
            int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width - frmShowBcTATClewText.Width;
            int y = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - frmShowBcTATClewText.Height;
            frmShowBcTATClewText.SetDesktopLocation(x, y);

            FrmBcComTATNotify_Load(null, null);
        }

        /// <summary>
        /// 点击小窗口,弹出大窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scrollingText1_Click(object sender, EventArgs e)
        {
            frmShowBcTATClewText.Hide();
            this.Show();
            IsFrmParentHide = false;
        }

        private void FrmBcComTATNotify_Load(object sender, EventArgs e)
        {
            this.Hide();
            IsFrmParentHide = true;
            IsStrongClose = false;
            msgCount = 0;

            //readTimer_Tick(sender, e);

            this.readTimer = new Timer();
            readTimer.Interval = 60000;
            readTimer.Tick += new EventHandler(readTimer_Tick);
            readTimer.Start();
        }

        /// <summary>
        /// 定时器事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void readTimer_Tick(object sender, EventArgs e)
        {
            string WhereCType_id = "";
            //物理组ID
            string CType_id = dcl.client.common.LocalSetting.Current.Setting.CType_id;
            //如果物理组ID 为null或空 则不过滤物理组
            //if (!string.IsNullOrEmpty(CType_id))
            //{
            //    WhereCType_id = " and bc_ctype='" + CType_id + "' ";
            //}

            List<EntityDicMsgTAT> listblood = null;//采集
            List<EntityDicMsgTAT> listSend = null;//收取
            List<EntityDicMsgTAT> listSign = null;//签收
            List<EntityDicMsgTAT> listReach = null;//送达
            List<EntityDicMsgTAT> listSecondSend = null;//二次送检
            List<EntityDicMsgTAT> listRegister = null;//检验中

            try
            {
                //ProxyMessage proxy = new ProxyMessage();
                ////采集
                //dtblood = proxy.Service.GetBcComTATMessage(string.Format("bc_status='2' {0}", WhereCType_id));
                ////收取
                //dtSend = proxy.Service.GetBcComTATMessage(string.Format("bc_status='3' {0}", WhereCType_id));
                ////签收
                //dtSign = proxy.Service.GetBcComTATMessage(string.Format("bc_status='5' {0}", WhereCType_id));
                ////送达
                //dtReach = proxy.Service.GetBcComTATMessage(string.Format("bc_status='4' {0}", WhereCType_id));
                ////二次送检
                //dtSecondSend = proxy.Service.GetBcComTATMessage(string.Format("bc_status='8' {0}", WhereCType_id));
                ////检验中
                //dtRegister = proxy.Service.GetBcComLabTATMessage(string.Format("1=1 {0}", WhereCType_id));

                ProxyCombineTATMsg proxyTATMsg = new ProxyCombineTATMsg();
                //采集
                listblood = proxyTATMsg.Service.GetBcComTATMessage();
                //收取
                listSend = proxyTATMsg.Service.GetBcComTATMessage();
                //签收
                listSign = proxyTATMsg.Service.GetBcComTATMessage();
                //送达
                listReach = proxyTATMsg.Service.GetBcComTATMessage();
                //二次送检
                listSecondSend = proxyTATMsg.Service.GetBcComTATMessage();
                //检验中
                listRegister = proxyTATMsg.Service.GetBcComLabTATMessage();

                if (!string.IsNullOrEmpty(CType_id))
                {
                    listblood = listblood.Where(w => w.SampStatusId == "2" && w.SampType == CType_id).ToList();//采集
                    listSend = listSend.Where(w => w.SampStatusId == "3" && w.SampType == CType_id).ToList();//收取
                    listSign = listSign.Where(w => w.SampStatusId == "5" && w.SampType == CType_id).ToList();//签收
                    listReach = listReach.Where(w => w.SampStatusId == "4" && w.SampType == CType_id).ToList();//送达
                    listSecondSend = listSecondSend.Where(w => w.SampStatusId == "8" && w.SampType == CType_id).ToList();//二次送检
                    listRegister = listRegister.Where(w =>w.SampType == CType_id).ToList();//检验中
                }
                else
                {
                    listblood = listblood.Where(w => w.SampStatusId == "2").ToList();//采集
                    listSend = listSend.Where(w => w.SampStatusId == "3").ToList();//收取
                    listSign = listSign.Where(w => w.SampStatusId == "5").ToList();//签收
                    listReach = listReach.Where(w => w.SampStatusId == "4").ToList();//送达
                    listSecondSend = listSecondSend.Where(w => w.SampStatusId == "8").ToList();//二次送检
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            ShowMessages(listblood, listSend, listSign, listReach, listSecondSend, listRegister);

        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="dtShow"></param>
        private void ShowMessages(List<EntityDicMsgTAT> listblood, List<EntityDicMsgTAT> listSend, List<EntityDicMsgTAT> listSign,
                 List<EntityDicMsgTAT> listReach, List<EntityDicMsgTAT> listSecondSend, List<EntityDicMsgTAT> listRegister)
        {
            //情况数据
            this.gcblood.DataSource = null;
            this.gcSend.DataSource = null;
            this.gcSign.DataSource = null;
            this.gcReach.DataSource = null;
            this.gcSecondSend.DataSource = null;
            this.gcRegister.DataSource = null;

            msgCount = 0;
            //采集
            if (listblood != null && listblood.Count > 0)
            {
                msgCount += listblood.Count;
                this.gcblood.DataSource = listblood;
            }

            //收取
            if (listSend != null && listSend.Count > 0)
            {
                msgCount += listSend.Count;
                this.gcSend.DataSource = listSend;
            }

            //签收
            if (listSign != null && listSign.Count > 0)
            {
                msgCount += listSign.Count;
                this.gcSign.DataSource = listSign;
            }

            //送达
            if (listReach != null && listReach.Count > 0)
            {
                msgCount += listReach.Count;
                this.gcReach.DataSource = listReach;
            }

            //二次送检
            if (listSecondSend != null && listSecondSend.Count > 0)
            {
                msgCount += listSecondSend.Count;
                this.gcSecondSend.DataSource = listSecondSend;
            }

            //检验中
            if (listRegister != null && listRegister.Count > 0)
            {
                msgCount += listRegister.Count;
                this.gcRegister.DataSource = listRegister;
            }

            if (msgCount <= 0)
            {
                frmShowBcTATClewText.Hide();
                //暂无信息
                frmShowBcTATClewText.showText("暂无信息");
                frmShowBcTATClewText.timerStop();
                this.Hide();
                IsFrmParentHide = true;
                return;
            }
            else
            {
                frmShowBcTATClewText.showText(string.Format("TAT信息:{0}条", msgCount.ToString()));
            }

            if (IsFrmParentHide)
            {
                PlaySoundMgr.Instance.PlaySound();
                if (false)//超出特定时间弹大窗口
                {
                    frmShowBcTATClewText.Hide();

                    if (this.WindowState != FormWindowState.Maximized)
                        this.WindowState = FormWindowState.Maximized;

                    this.Show();
                    IsFrmParentHide = false;
                }
                else
                {
                    frmShowBcTATClewText.timerStart();
                    if (!frmShowBcTATClewText.Visible)
                    {
                        IntPtr activeForm = GetActiveWindow();
                        frmShowBcTATClewText.Show();
                        SetActiveWindow(activeForm);
                    }
                }
            }
            else
            {
                IsFrmParentHide = false;
            }
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

        private void FrmBcComTATNotify_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsStrongClose || !this.Visible)//是否强关闭窗口
            {
                this.readTimer.Enabled = false;
                this.readTimer.Dispose();
                this.readTimer = null;

                this.frmShowBcTATClewText.Dispose();

                this.gcblood.DataSource = null;
                this.gcblood.Dispose();
                this.gcSend.DataSource = null;
                this.gcSend.Dispose();
                this.gcSign.DataSource = null;
                this.gcSign.Dispose();
                this.gcReach.DataSource = null;
                this.gcReach.Dispose();
                this.gcSecondSend.DataSource = null;
                this.gcSecondSend.Dispose();
                this.gcRegister.DataSource = null;
                this.gcRegister.Dispose();

                this.Dispose();
                //IsStrongClose = false;
            }
            else
            {
                e.Cancel = true;
                this.Hide();
                IsFrmParentHide = true;
            }
        }

    }
}
