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
    public partial class FrmBcComTATNotifyNew : Form
    {
        /// <summary>
        /// 定时器
        /// </summary>
        private Timer readTimer = null;

        private Timer readTimerCombine = null;

        /// <summary>
        /// 是否强关闭窗口
        /// </summary>
        private bool IsStrongClose = false;

        public bool blBcTAT { get; set; }

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

        public FrmBcComTATNotifyNew()
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

            FrmBcComTATNotifyNew_Load(null, null);
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
            //    WhereCType_id = " bc_ctype='" + CType_id + "' ";
            //}

            List<EntityDicMsgTAT> cache = null;

            try
            {
                //ProxyMessage proxy = new ProxyMessage();
                //cache = proxy.Service.GetBcComTATMessage(WhereCType_id);
                ProxyCombineTATMsg proxyComTATMsg = new ProxyCombineTATMsg();
                //采集
                cache = proxyComTATMsg.Service.GetBcComTATMessage();

                //如果物理组ID 为null或空 则不过滤物理组
                if (!string.IsNullOrEmpty(CType_id))
                {
                    cache = cache.Where(w => w.SampType == CType_id).ToList();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            ShowMessages(cache);
            this.pnlSend.Text = "条码监控信息";

        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="dtShow"></param>
        private void ShowMessages(List<EntityDicMsgTAT> cache)
        {
            //情况数据
            this.gcblood.DataSource = null;

            msgCount = 0;
            //采集
            if (cache != null && cache.Count > 0)
            {
                msgCount += cache.Count;
                this.gcblood.DataSource = cache;
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

        private void FrmBcComTATNotifyNew_Load(object sender, EventArgs e)
        {
            this.Hide();
            IsFrmParentHide = true;
            IsStrongClose = false;
            msgCount = 0;

            //readTimer_Tick(sender, e);
            if (blBcTAT)
            {
                this.readTimer = new Timer();
                readTimer.Interval = 60000;
                readTimer.Tick += new EventHandler(readTimer_Tick);
                readTimer.Start();
            }
            else
            {
                this.readTimerCombine = new Timer();
                readTimerCombine.Interval = 60000;
                readTimerCombine.Tick += new EventHandler(readTimerCombine_Tick);
                readTimerCombine.Start();
            }
        }

        void readTimerCombine_Tick(object sender, EventArgs e)
        {
            List<EntityDicMsgTAT> list_dtComTATMsg = new List<EntityDicMsgTAT>();
            try
            {
                //ProxyMessage proxy = new ProxyMessage();
                //DataTable t_dtComTATMsg = proxy.Service.GetComTATMessage(strWhere);//获取组合TAT信息
                ProxyCombineTATMsg proxyComTATMsg = new ProxyCombineTATMsg();
                list_dtComTATMsg = proxyComTATMsg.Service.GetComTATMessage();//获取组合TAT信息

                #region 查询条件

                string t_itr_type = dcl.client.common.LocalSetting.Current.Setting.CType_id;

                string t_itr_id_list = dcl.client.common.LocalSetting.Current.Setting.ItrIDList;//仪器ID集合

                if (!string.IsNullOrEmpty(t_itr_type))
                {
                    //strWhere += string.Format("itr_type='{0}' ", t_itr_type);
                    if ((!string.IsNullOrEmpty(t_itr_id_list)) && t_itr_id_list.Contains("'"))
                    {
                        //strWhere += string.Format(" or pat_itr_id in({0}) ", t_itr_id_list);
                        list_dtComTATMsg = list_dtComTATMsg.Where(w => w.ItrType == t_itr_type || t_itr_id_list.Contains(w.RepItrId)).ToList();
                    }
                    else
                    {
                        list_dtComTATMsg = list_dtComTATMsg.Where(w => w.ItrType == t_itr_type).ToList();
                    }

                }
                else
                {
                    if ((!string.IsNullOrEmpty(t_itr_id_list)) && t_itr_id_list.Contains("'"))
                    {
                        //strWhere += string.Format("pat_itr_id in({0}) ", t_itr_id_list);
                        list_dtComTATMsg = list_dtComTATMsg.Where(w => t_itr_id_list.Contains(w.RepItrId)).ToList();
                    }
                }

                #endregion

                if (list_dtComTATMsg != null && list_dtComTATMsg.Count > 0)
                {
                    //dvtempsort.Sort = "time_mi_over desc";
                    //dtComTATMsg.TableName = "ComTATMsgCache";

                    //排序
                    list_dtComTATMsg = list_dtComTATMsg.OrderByDescending(i => i.TimeMiOver).ToList();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            ShowMessages(list_dtComTATMsg);
            this.pnlSend.Text = "组合监控信息";
        }
    }
}
