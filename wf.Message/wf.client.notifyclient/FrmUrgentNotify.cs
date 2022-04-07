using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using dcl.client.wcf;

using System.Net;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.notifyclient
{
    /// <summary>
    /// 危急值内部提醒(默认只提取最近一周的信息)
    /// </summary>
    public partial class FrmUrgentNotify : Form
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
        /// 危急值信息多少分钟后提示小窗口
        /// </summary>
        public int LittleWinTime = 10;

        /// <summary>
        /// 危急值信息多少分钟后提示大窗口
        /// </summary>
        public int BigWinTime = 20;

        /// <summary>
        /// 急查信息显示时间
        /// </summary>
        public int JCShowTime = 60;


        /// <summary>
        /// 信息条数
        /// </summary>
        private int msgCount = 0;

        private FrmShowClew frmShowClewText = null;

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


        public FrmUrgentNotify()
        {
            InitializeComponent();
            this.groupBox1.MouseMove += new MouseEventHandler(groupBox1_MouseMove);
            this.groupBox1.MouseDown += new MouseEventHandler(groupBox1_MouseDown);
            this.Hide();
            IsFrmParentHide = true;
            IsStrongClose = false;
            gridrowColor = this.gvLookData.Appearance.Empty.BackColor;
            this.gcLookData.MouseClick += new MouseEventHandler(gcLookData_MouseClick);
            this.Deactivate += new EventHandler(FrmUrgentNotify_Deactivate);
            string PlaySoundMode = PlaySoundMgr.Instance.PlaySoundMode;
            if (string.IsNullOrEmpty(PlaySoundMode) || PlaySoundMode == "1")
            {
                this.toolStripComboBox1.SelectedIndex = 1;
            }
            else if (PlaySoundMode == "0")
            {
                this.toolStripComboBox1.SelectedIndex = 0;

            }
            else
            {
                this.toolStripComboBox1.SelectedIndex = 2;

            }

            this.MaximizeBox = System.Configuration.ConfigurationManager.AppSettings["DebugMode"] == "1";
            if (this.MaximizeBox)
            {
                this.SizeGripStyle = SizeGripStyle.Auto;
            }
            else
            {
                this.SizeGripStyle = SizeGripStyle.Hide;

            }
            this.toolStripComboBox1.SelectedIndexChanged += new EventHandler(toolStripComboBox1_SelectedIndexChanged);

        }

        void groupBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {


                this.gvLookData.Appearance.Empty.BackColor = gridrowColor;

                this.timer1.Stop();
                spanPoint = new Point(e.X, e.Y);
            }
        }

        Point spanPoint = new Point();


        void groupBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                this.Location = new Point(this.Location.X + (e.X - spanPoint.X), this.Location.Y + (e.Y - spanPoint.Y));
            }
        }

        void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PlaySoundMgr.Instance.SaveConfig(this.toolStripComboBox1.SelectedIndex.ToString());
        }


        void FrmUrgentNotify_Deactivate(object sender, EventArgs e)
        {
            this.gvLookData.Appearance.Empty.BackColor = gridrowColor;

            this.timer1.Stop();
        }

        void gcLookData_MouseClick(object sender, MouseEventArgs e)
        {
            this.gvLookData.Appearance.Empty.BackColor = gridrowColor;

            this.timer1.Stop();
        }
        /// <summary>
        /// 开始运行
        /// </summary>
        public void startShowFrm()
        {
            frmShowClewText = new FrmShowClew();
            frmShowClewText.scrollingText1.Click += new EventHandler(scrollingText1_Click);
            frmShowClewText.TopMost = true;
            //设置frmShowClewText的位置,默认右下角
            int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width - frmShowClewText.Width;
            int y = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - frmShowClewText.Height;
            frmShowClewText.SetDesktopLocation(x, y);

            FrmUrgentNotify_Load(null, null);
        }

        private void FrmUrgentNotify_Load(object sender, EventArgs e)
        {
            this.Hide();
            IsFrmParentHide = true;
            IsStrongClose = false;
            msgCount = 0;

            //readTimer_Tick(sender, e);

            chkSendMsg.Visible = dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("SendMsgTriggerType") == "内部确认危机值时";

            IsBatchAudit(false);//初始化非批量审核是控件状态

            this.readTimer = new Timer();
            readTimer.Interval = 12000;
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
            List<EntityPidReportMain> listMessages = GetDtUrgentMsg();
            if (listMessages != null)
            {
                //物理组ID
                string CType_id = dcl.client.common.LocalSetting.Current.Setting.CType_id;
                //如果物理组ID 为null或空 则不过滤物理组
                if (!string.IsNullOrEmpty(CType_id))
                {
                    if (listMessages != null && listMessages.Count > 0)
                    {
                        //listMessages = filterDtForNew(listMessages, string.Format("itr_type='{0}'", CType_id));
                        listMessages = listMessages.Where(w => w.ItrLabId == CType_id).ToList();
                    }
                }

                //物理组id集合
                string phytypeIDs = dcl.client.common.LocalSetting.Current.Setting.TypeIDList;
                if (!string.IsNullOrEmpty(phytypeIDs))
                {
                    if (listMessages != null && listMessages.Count > 0)
                    {
                        //dtMessages = filterDtForNew(dtMessages, string.Format("itr_type in ({0})", phytypeIDs));
                        listMessages = listMessages.Where(w => phytypeIDs.Contains(w.ItrLabId)).ToList();
                    }
                }

                //仪器ID集合
                string t_itr_id_list = dcl.client.common.LocalSetting.Current.Setting.ItrIDList;

                if ((!string.IsNullOrEmpty(t_itr_id_list)) && t_itr_id_list.Contains("'"))
                {
                    if (listMessages != null && listMessages.Count > 0)
                    {
                        //dtMessages = filterDtForNew(dtMessages, string.Format("pat_itr_id in ({0})", t_itr_id_list));
                        listMessages = listMessages.Where(w => t_itr_id_list.Contains(w.RepItrId)).ToList();
                    }
                }

                //危急值内部提醒多少小时内危急
                string InNotifyHourRangStr = dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Urgent_InNotify_HourRang");
                double InNotifyHourRangDou = 0;
                if (!string.IsNullOrEmpty(InNotifyHourRangStr) && double.TryParse(InNotifyHourRangStr, out InNotifyHourRangDou))
                {
                    //大于0才有效
                    if (InNotifyHourRangDou > 0)
                    {
                        if (listMessages != null && listMessages.Count > 0)
                        {
                            listMessages = listMessages.Where(w => w.ObrCreateTime >= DateTime.Now.AddHours(-InNotifyHourRangDou)).ToList();
                        }
                    }
                }

                ShowMessages(listMessages);
            }
        }

        /// <summary>
        /// 读取危急信息(DataTable)
        /// </summary>
        /// <param name="dep_code"></param>
        /// <returns></returns>
        private List<EntityPidReportMain> GetDtUrgentMsg()
        {
            List<EntityPidReportMain> listUrgentMsg = new List<EntityPidReportMain>();

            EntityUrgentHistoryUseParame eyWhere = new EntityUrgentHistoryUseParame();
            eyWhere.ReceiveID = "-1";
            eyWhere.DepIDs = System.Configuration.ConfigurationManager.AppSettings["DepIDs"] ?? "";
            eyWhere.FilterTime = LittleWinTime <= 0 ? "0" : LittleWinTime.ToString();//获取推迟多少分钟的危急值信息
            eyWhere.FilterTime2 = JCShowTime <= 0 ? "0" : JCShowTime.ToString();//获取推迟多少分钟的急查信息

            ProxyUrgentObrMessage proxyUrgentObrMsg = new ProxyUrgentObrMessage();
            listUrgentMsg = proxyUrgentObrMsg.Service.GetUrgentHistoryMsgGetAll(eyWhere);//获取危急值信息

            return listUrgentMsg;
        }
        
        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="dtShow"></param>
        private void ShowMessages(List<EntityPidReportMain> listShow)
        {
            //情况数据
            this.gcLookData.DataSource = null;

            if (listShow == null || listShow.Count == 0)
            {
                msgCount = 0;
                showCount();
                frmShowClewText.Hide();
                //暂无信息
                frmShowClewText.showText("暂无信息");
                frmShowClewText.timerStop();
                this.Hide();
                IsFrmParentHide = true;
                IsBatchAudit(false);
                return;
            }
            else
            {
                msgCount = listShow.Count;
                frmShowClewText.showText(string.Format("有未确认信息:{0}条", msgCount.ToString()));
            }

            this.gcLookData.DataSource = listShow;

            gvLookData.MoveLast();

            showCount();

            if (IsFrmParentHide)
            {
                //危急值内部提醒是否喇叭鸣叫
                if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Urgent_InNotify_PlaySound") != "否")
                {
                    PlaySoundMgr.Instance.PlaySound();
                }
                if (checkOverTime(BigWinTime, listShow))//超出特定时间弹大窗口
                {
                    frmShowClewText.Hide();

                    if (this.WindowState != FormWindowState.Maximized)
                        this.WindowState = FormWindowState.Maximized;

                    this.Show();
                    this.timer1.Start();
                    IsFrmParentHide = false;
                }
                else
                {
                    //frmShowClewText.TopLevel = true;
                    frmShowClewText.timerStart();
                    //AnimateWindow(frmShowClewText.Handle, 1000, AW_ACTIVATE | AW_BLEND);

                    if (!frmShowClewText.Visible)
                    {
                        IntPtr activeForm = GetActiveWindow();
                        frmShowClewText.Show();
                        SetActiveWindow(activeForm);
                    }

                    //frmShowClewText.Focus();
                }
            }
            else
            {
                //this.Show();
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

        private void FrmUrgentNotify_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (IsStrongClose || !this.Visible)//是否强关闭窗口
            {
                this.readTimer.Enabled = false;
                this.readTimer.Dispose();
                this.readTimer = null;

                this.frmShowClewText.Dispose();

                this.gcLookData.DataSource = null;
                this.gcLookData.Dispose();

                this.Dispose();
                //IsStrongClose = false;
            }
            else
            {
                e.Cancel = true;
                this.Hide();
                IsFrmParentHide = true;
                IsBatchAudit(false);
            }
        }

        /// <summary>
        /// 关闭前最后一次显示
        /// </summary>
        public void ClosePreLastShow()
        {
            try
            {
                if (this.readTimer != null)
                {
                    this.readTimer.Enabled = false;

                    List<EntityPidReportMain> listMessages = GetDtUrgentMsg();
                    if (listMessages != null)
                    {
                        //物理组ID
                        string CType_id = dcl.client.common.LocalSetting.Current.Setting.CType_id;
                        //如果物理组ID 为null或空 则不过滤物理组
                        if (!string.IsNullOrEmpty(CType_id))
                        {
                            if (listMessages != null && listMessages.Count > 0)
                            {
                                //dtMessages = filterDtForNew(dtMessages, string.Format("itr_type='{0}'", CType_id));
                                listMessages = listMessages.Where(w => w.ItrLabId == CType_id).ToList();
                            }
                        }

                        //物理组id集合
                        string phytypeIDs = dcl.client.common.LocalSetting.Current.Setting.TypeIDList;
                        if (!string.IsNullOrEmpty(phytypeIDs))
                        {
                            if (listMessages != null && listMessages.Count > 0)
                            {
                                //dtMessages = filterDtForNew(dtMessages, string.Format("itr_type in ({0})", phytypeIDs));
                                listMessages = listMessages.Where(w => phytypeIDs.Contains(w.ItrLabId)).ToList();
                            }
                        }

                        //情况数据
                        this.gcLookData.DataSource = null;

                        if (listMessages == null || listMessages.Count == 0)
                        {
                            msgCount = 0;
                            showCount();
                            frmShowClewText.Hide();
                            //暂无信息
                            frmShowClewText.showText("暂无信息");
                            frmShowClewText.timerStop();
                            this.Hide();
                            IsFrmParentHide = true;
                            return;
                        }
                        else
                        {
                            msgCount = listMessages.Count;
                            frmShowClewText.showText(string.Format("有未确认信息:{0}条", msgCount.ToString()));
                        }

                        this.gcLookData.DataSource = listMessages;

                        showCount();
                        
                        if (IsFrmParentHide)
                        {
                            PlaySoundMgr.Instance.PlaySound();
                            if (true)//超出特定时间弹大窗口
                            {
                                frmShowClewText.Hide();

                                if (this.WindowState != FormWindowState.Maximized)
                                    this.WindowState = FormWindowState.Maximized;

                                IsFrmParentHide = false;
                                this.ShowDialog();
                            }
                        }
                        else
                        {
                            IsFrmParentHide = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 显示消息数
        /// </summary>
        private void showCount()
        {
            lblShowCount.Text = string.Format("消息：{0}条", msgCount.ToString());
        }

        /// <summary>
        /// 急查报告为红色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvLookData_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            //DataRow dr = this.gvLookData.GetDataRow(e.RowHandle);
            EntityPidReportMain eyPats = this.gvLookData.GetRow(e.RowHandle) as EntityPidReportMain;
            if (eyPats != null)
            {
                if (eyPats.ObrType.ToString() == "4096")//急查报告为红色
                {
                    e.Appearance.ForeColor = Color.Red;

                }
            }
        }

        /// <summary>
        /// 双击审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvLookData_DoubleClick(object sender, EventArgs e)
        {
            if (gvLookData.GetFocusedRow() == null) return;

            int rowIndex = gvLookData.FocusedRowHandle;
            //DataRow drPatData = gvLookData.GetFocusedDataRow();
            EntityPidReportMain eyPatData = this.gvLookData.GetFocusedRow() as EntityPidReportMain;

            if (eyPatData != null)
            {
                #region 处理危急值消息
                string pat_id = eyPatData.RepId.ToString();
                #region 用户验证

                EntityAuditInfo CheckerInfo = null;

                //危急值内部提醒验证录入临床信息
                if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Urgent_AuditType_withdoc") == "是")
                {
                    //验证用户
                    FrmReadAffirm2 frmRA2 = null;

                    frmRA2 = new FrmReadAffirm2();

                    if (frmRA2.ShowDialog() != DialogResult.Yes)
                    {
                        return;
                    }
                    CheckerInfo = frmRA2.m_userInfo;
                }
                else
                {
                    //验证用户
                    FrmReadAffirm frmRA = null;

                    frmRA = new FrmReadAffirm();

                    if (frmRA.ShowDialog() != DialogResult.Yes)
                    {
                        return;
                    }
                    CheckerInfo = frmRA.m_userInfo;
                }

                CheckerInfo.Place = string.Format("{0}({1})", Environment.MachineName, GetClientIPv4());

                #endregion
                #region 再次查询病人信息 防止出现危急值已被审核 还会再次审核问题
                ProxyPidReportMain proxy = new ProxyPidReportMain();
                EntityPatientQC patientQc = new EntityPatientQC();
                patientQc.RepId = pat_id;
                List< EntityPidReportMain> listPatient = proxy.Service.PatientQuery(patientQc);
                if (listPatient.Count>0 && listPatient[0].RepUrgentFlag == 2)
                {
                    MessageBox.Show("该条数据已被审核");
                    this.readTimer.Start();
                    return;
                }
                #endregion
                if (!string.IsNullOrEmpty(pat_id))
                {
                    try
                    {
                        EntityDicPidReportMainExt ext = new EntityDicPidReportMainExt();
                        ext.RepId = pat_id;
                        string strmsg_date = ServerDateTime.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                        //危急值内部提醒验证录入临床信息
                        if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Urgent_AuditType_withdoc") == "是")
                        {
                            #region 更新临床信息
                            //这个需要更换成哪个呢？
           
                            string[] patExtColName = new string[6];//列名
                            string[] patExtColValue = new string[6];//列值
                          //  EntityDicPidReportMainExt ext = new EntityDicPidReportMainExt();
                            ext.MsgDocNum = CheckerInfo.MsgDocNum;
                            ext.MsgDocName = CheckerInfo.MsgDocName;
                            ext.MsgDepTel = CheckerInfo.MsgDepTel;
                            ext.MsgDate =Convert.ToDateTime(strmsg_date);
                            //ext.MsgInsginId = CheckerInfo.UserId;
                            //ext.MsgInsginDate =Convert.ToDateTime(strmsg_date);
                            //保存病人扩展资料
                            //EntityOperationResult eoPatExt = PatientCRUDClient.NewInstance.AddOrUpdatePatientExt(patExtColName, patExtColValue, pat_id);
                            //ProxyPidReportMainExt proxyPidMainExt = new ProxyPidReportMainExt();
                            //proxyPidMainExt.Service.AddOrUpdatePatientExt(ext);
                            #endregion
                        }
                        ext.MsgInsginId = CheckerInfo.UserId;
                        ext.MsgInsginDate = Convert.ToDateTime(strmsg_date);
                        ProxyPidReportMainExt proxyPidMainExt = new ProxyPidReportMainExt();
                        proxyPidMainExt.Service.AddOrUpdatePatientExt(ext);
                        string msg_id = eyPatData.ObrIdMsg;

                        //ProxyMessage proxy = new ProxyMessage();
                        //proxy.Service.DeleteMessageByIDAndUpdateCriticalChecker(CheckerInfo, msg_id, pat_id);
                        //proxy.Service.RefreshUrgentMessage();
                        ProxyObrMessage proxyObrMsg = new ProxyObrMessage();
                        proxyObrMsg.Service.DeleteMessageByIDAndUpdateCriticalChecker(CheckerInfo, msg_id, pat_id);
                        ProxyUrgentObrMessage proxyUrgObrMsg = new ProxyUrgentObrMessage();
                        proxyUrgObrMsg.Service.RefreshUrgentMessage();

                        readTimer_Tick(null, null);
                    }
                    catch (Exception)
                    {
                        //throw;
                    }

                }
                #endregion
            }
        }
        public string GetClientIPv4()
        {

            string ipv4 = String.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4 = IPA.ToString();
                    break;
                }
            }
            return ipv4;
        }
        /// <summary>
        /// 点击小窗口,弹出大窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scrollingText1_Click(object sender, EventArgs e)
        {
            frmShowClewText.Hide();
            this.Show();
            IsFrmParentHide = false;
        }
        /// <summary>
        /// 过滤数据表信息(不通过方法过滤，在相对应位置直接过滤，废除此方法)
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        private List<EntityPidReportMain> filterDtForNew(List<EntityPidReportMain> listSource, string sqlWhere)
        {
            //try
            //{
            //    if (listSource != null && listSource.Count > 0)
            //    {
            //        if (!string.IsNullOrEmpty(sqlWhere))//为空不过滤
            //        {
            //            DataTable dtCope = dtSource.Clone();
            //            DataRow[] drArray = dtSource.Select(sqlWhere);

            //            List<EntityPatients> listArray=listSource.Where(w=>w.)

            //            foreach (DataRow drItem in drArray)
            //            {
            //                dtCope.Rows.Add(drItem.ItemArray);
            //            }
            //            dtCope.TableName = "UrgentMsg";
            //            return dtCope;
            //        }
            //        else
            //        {
            //            return dtSource.Copy();//为空不过滤
            //        }
            //    }
            //}
            //catch
            //{

            //}
            //return dtSource.Copy();//错误或空，不过滤
            return null;
        }

        /// <summary>
        /// 批量审核(全选)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (gcLookData.DataSource != null)
            {
                //DataTable dsource = gcLookData.DataSource as DataTable;
                List<EntityPidReportMain> listSource = gcLookData.DataSource as List<EntityPidReportMain>;

                if (listSource != null && listSource.Count > 0)
                {
                    bool IsAllCheck = checkEdit1.Checked;//是否全选
                    foreach (var infoCheck in listSource)
                    {
                        infoCheck.PatSelect = IsAllCheck;
                    }
                    gcLookData.DataSource = listSource;//重新给控件赋值
                    gcLookData.RefreshDataSource();//刷新控件数据
                }
            }
        }

        /// <summary>
        /// 是否批量审核,并改变控件状态
        /// </summary>
        /// <param name="bln"></param>
        private void IsBatchAudit(bool bln)
        {
            if (!bln)
            {
                if (this.readTimer != null)
                {
                    this.readTimer.Start();//批量审核完毕后，启动刷新
                }
            }
            else
            {
                if (this.readTimer != null)
                {
                    this.readTimer.Stop();//启动批量审核时，暂停刷新
                }
            }

            if (this.BtnSAudit != null)
                this.BtnSAudit.Enabled = !bln;//批量审核-按钮

            if (this.BtnSAudit != null)
                this.BtnAudit.Visible = bln;//审核-按钮

            if (this.BtnSAudit != null)
                this.BtnUndo.Visible = bln;//放弃批量审核-按钮

            if (this.BtnSAudit != null)
            {
                if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("GetPatientsInfoType") != "outlink")
                {
                    this.checkEdit1.Visible = bln;//全选
                }
                this.checkEdit1.Checked = bln ? false : this.checkEdit1.Checked;

            }

            if (this.BtnSAudit != null)
            {
                this.colpat_select.Width = 30;
                this.colpat_select.Visible = bln;//复选列
            }
        }

        /// <summary>
        /// 启动批量审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSAudit_Click(object sender, EventArgs e)
        {
            if (this.gvLookData.RowCount > 0)
            {
                IsBatchAudit(true);//启动批量审核时，改变某些控件状态
            }
            else
            {
                MessageBox.Show("没有可以审核的数据!");
            }
        }

        /// <summary>
        /// 放弃批量审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUndo_Click(object sender, EventArgs e)
        {
            IsBatchAudit(false);
        }
        /// <summary>
        /// 批量审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAudit_Click(object sender, EventArgs e)
        {
            if (this.gvLookData.RowCount > 0)
            {
                //DataTable dtBatAudit = gcLookData.DataSource as DataTable;
                List<EntityPidReportMain> listBatAudit = gcLookData.DataSource as List<EntityPidReportMain>;

                //DataTable dtCeBatAudit = filterDtForNew(dtBatAudit, "pat_select=1");
                List<EntityPidReportMain> listCeBatAudit = new List<EntityPidReportMain>();
                if(listBatAudit!=null&& listBatAudit.Count>0)
                {
                    listCeBatAudit = listBatAudit.Where(w => w.PatSelect == true).ToList();
                }

                if (listCeBatAudit != null && listCeBatAudit.Count > 0)
                {
                    #region 处理危急值消息
                    
                    #region 用户验证

                    //AuditInfo CheckerInfo = null;
                    EntityAuditInfo CheckerInfo = null;

                    //危急值内部提醒验证录入临床信息
                    if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Urgent_AuditType_withdoc") == "是")
                    {
                        //验证用户
                        FrmReadAffirm2 frmRA2 = null;

                        frmRA2 = new FrmReadAffirm2();

                        if (frmRA2.ShowDialog() != DialogResult.Yes)
                        {
                            return;
                        }
                        CheckerInfo = frmRA2.m_userInfo;
                    }
                    else
                    {
                        //验证用户
                        FrmReadAffirm frmRA = null;


                        frmRA = new FrmReadAffirm();

                        if (frmRA.ShowDialog() != DialogResult.Yes)
                        {
                            return;
                        }
                        CheckerInfo = frmRA.m_userInfo;
                    }

                    CheckerInfo.Place = string.Format("{0}({1})", Environment.MachineName, GetClientIPv4());

                    if (chkSendMsg.Visible)
                    {
                        CheckerInfo.SendMsgFlag = chkSendMsg.Checked;
                    }

                    #endregion

                    try
                    {
                        //记录时间
                        string strmsg_date = ServerDateTime.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss");

                        //ProxyMessage proxy = new ProxyMessage();

                        foreach (var infoBatAudit in listCeBatAudit)
                        {
                            string msg_id = infoBatAudit.ObrIdMsg;
                            string pat_id = infoBatAudit.RepId;
                            if (!string.IsNullOrEmpty(pat_id))
                            {
                                EntityDicPidReportMainExt ext = new EntityDicPidReportMainExt();
                                ext.RepId = pat_id;
                                //危急值内部提醒验证录入临床信息
                                if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Urgent_AuditType_withdoc") == "是")
                                {
                                    #region 更新临床信息
                                    ext.MsgDocNum = CheckerInfo.MsgDocNum;
                                    ext.MsgDocName = CheckerInfo.MsgDocName;
                                    ext.MsgDepTel = CheckerInfo.MsgDepTel;
                                    ext.MsgDate = Convert.ToDateTime(strmsg_date);
                                    #endregion
                                }
                                ext.MsgInsginId = CheckerInfo.UserId;
                                ext.MsgInsginDate = Convert.ToDateTime(strmsg_date);
                                ProxyPidReportMainExt proxyPidReportMainExt = new ProxyPidReportMainExt();
                                proxyPidReportMainExt.Service.AddOrUpdatePatientExt(ext);
                                //proxy.Service.DeleteMessageByIDAndUpdateCriticalChecker(CheckerInfo, msg_id, pat_id);
                                ProxyObrMessage proxyObrMsg = new ProxyObrMessage();
                                proxyObrMsg.Service.DeleteMessageByIDAndUpdateCriticalChecker(CheckerInfo, msg_id, pat_id);
                            }
                        }
                        //ProxyMessage proxy = new ProxyMessage();
                        //proxy.Service.RefreshUrgentMessage();
                        ProxyUrgentObrMessage proxyUrgentMsg = new ProxyUrgentObrMessage();
                        proxyUrgentMsg.Service.RefreshUrgentMessage();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("批量审核失败：\r\n" + ex.Message);
                    }

                    readTimer_Tick(null, null);
                    IsBatchAudit(false);

                    #endregion
                }
                else
                {
                    MessageBox.Show("请选择要审核的数据!");
                }
            }
            else
            {
                MessageBox.Show("没有可以审核的数据!");
                IsBatchAudit(false);
            }

        }

        /// <summary>
        /// 检查信息是否有超出特定时间
        /// </summary>
        /// <param name="overTimeInt">超出分钟</param>
        /// <param name="dtTemp">数据表</param>
        /// <returns></returns>
        private bool checkOverTime(int overTimeInt, List<EntityPidReportMain> listTemp)
        {
            bool blnOverTime = false;

            if (overTimeInt == 0) return blnOverTime;

            if (listTemp != null && listTemp.Count > 0)
            {
                try
                {
                    foreach (var infoTemp in listTemp)
                    {
                        DateTime server_date = DateTime.Now;//服务器时间
                        if (DateTime.TryParse(infoTemp.ServerDate.ToString(), out server_date))
                        {
                            DateTime msg_create_time = DateTime.Now;//信息生成时间
                            if (DateTime.TryParse(infoTemp.ObrCreateTime.ToString(), out msg_create_time))
                            {
                                server_date = server_date.AddMinutes(-overTimeInt);

                                if (server_date >= msg_create_time)
                                {
                                    blnOverTime = true;
                                    break;
                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return false;
                }
            }

            return blnOverTime;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.gvLookData.Appearance.Empty.BackColor == gridrowColor)
            {
                this.gvLookData.Appearance.Empty.BackColor = Color.Red;
            }
            else
            {
                this.gvLookData.Appearance.Empty.BackColor = gridrowColor;
            }
        }

        private void FrmUrgentNotify_DoubleClick(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.readTimer.Start();
            this.Close();
        }

    }
}
