using System;
using System.Collections.Generic;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.wcf;
using System.Net;
using System.Runtime.InteropServices;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.msgsend
{
    public partial class FrmDIYCritical : FrmCommon
    {
        /// <summary>
        /// 信息条数
        /// </summary>
        private int msgCount = 0;

        //  private string strMainSqlwhere { get; set; }

        EntityDicObrMessageContent entityCodition = new EntityDicObrMessageContent();

        /// <summary>
        /// 本地设置科室ID
        /// </summary>
        internal static string strLocaDepID { get; set; }

        /// <summary>
        /// 本地设置科室ID
        /// </summary>
        internal static string strLocaDepName { get; set; }

        /// <summary>
        /// 是否强关闭窗口
        /// </summary>
        private bool IsStrongClose = false;

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

        public FrmDIYCritical()
        {
            InitializeComponent();
        }

        //private FrmShowMsg frmShowMsg = null;

        private FrmShowInMsg frmShowInMsg = null;

        private void FrmDIYCritical_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;




        }

        void frmShowMsg_ReturnOKEvent(object sender)
        {
            if (this.WindowState != FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Maximized;
            }

            this.TopMost = true;

            if (tabControl1.SelectedTab != tabPage2)
            {
                tabControl1.SelectedTab = tabPage2;
            }

            this.TopMost = false;
        }

        private void sbtExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sbtClear_Click(object sender, EventArgs e)
        {
            ClearSelControls();
        }

        /// <summary>
        /// 清除查询条件内容
        /// </summary>
        private void ClearSelControls()
        {
            entityCodition = new EntityDicObrMessageContent();

            //发送日期
            datePatDateS.EditValue = null;
            datePatDateE.EditValue = null;

            //病人号
            txtPatInNo.Text = "";

            //名称
            txtPatName.Text = "";

            //床号
            txtBedNo.Text = "";

            //病人来源
            selectDicPubSource.ClearSelect();

            //科室
            selectDicPubDept1.ClearSelect();

            rbtAll.Checked = true;
        }

        private void sbtSelect_Click(object sender, EventArgs e)
        {
            try
            {
                string sqlwhere = "";

                //发送日期
                #region 发送日期
                if (datePatDateS.EditValue != null && datePatDateE.EditValue != null)
                {
                    if (datePatDateS.DateTime.Date >= datePatDateE.DateTime.Date.AddDays(1))
                    {
                        lis.client.control.MessageDialog.Show("请输入正确的发送日期范围");
                        datePatDateS.Focus();
                        return;
                    }
                    entityCodition.StartDate = datePatDateS.DateTime.Date;
                    entityCodition.EndDate = datePatDateE.DateTime.Date.AddDays(1);

                }
                else if (datePatDateS.EditValue != null)
                {
                    entityCodition.StartDate = datePatDateS.DateTime.Date;
                    entityCodition.EndDate = datePatDateE.DateTime.Date.AddDays(1);
                }
                else if (datePatDateE.EditValue != null)
                {
                    entityCodition.StartDate = datePatDateE.DateTime.Date;
                    entityCodition.EndDate = datePatDateE.DateTime.Date.AddDays(1);
                }
                #endregion


                //病人号
                #region 病人号
                if (!string.IsNullOrEmpty(txtPatInNo.Text.Trim()))
                {
                    entityCodition.PidInNo = txtPatInNo.Text.Trim();
                }

                #endregion

                //病人名称
                #region 病人名称
                if (!string.IsNullOrEmpty(txtPatName.Text.Trim()))
                {
                    entityCodition.PidName = txtPatName.Text.Trim();
                }
                #endregion

                //床号
                #region 床号
                if (!string.IsNullOrEmpty(txtBedNo.Text.Trim()))
                {
                    entityCodition.PidBedNo = txtBedNo.Text.Trim();
                }
                #endregion

                //病人来源
                #region 病人来源
                if (!string.IsNullOrEmpty(selectDicPubSource.valueMember))
                {
                    entityCodition.PidSrcId = selectDicPubSource.valueMember;
                }
                #endregion

                //科室
                #region 科室
                if (!string.IsNullOrEmpty(selectDicPubDept1.valueMember))
                {
                    entityCodition.ObrUserName = selectDicPubDept1.valueMember;
                }
                #endregion

                //状态
                #region 状态
                if (rbtN.Checked)//未查看
                {
                    entityCodition.MsgStatus = "0";
                }
                else if (rbtY.Checked)//已查看
                {
                    entityCodition.MsgStatus = "1";
                }

                #endregion

                //填写人科室 selectDicPubDeptID
                #region 填写人科室
                if (!string.IsNullOrEmpty(selectDicPubDeptID.valueMember))
                {
                    entityCodition.ObrSendDeptCode = selectDicPubDeptID.valueMember;
                }
                #endregion


                SearcheData();

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                lis.client.control.MessageDialog.Show(ex.Message);
            }
        }

        /// <summary>
        /// 查询信息
        /// </summary>
        private void SearcheData()
        {
            try
            {
                if (string.IsNullOrEmpty(entityCodition.StartDate.ToString()) && string.IsNullOrEmpty(entityCodition.EndDate.ToString()))
                {
                    entityCodition.StartDate = DateTime.Now.Date;
                    entityCodition.EndDate = DateTime.Now.Date.AddDays(1);
                }
                ProxyObrMessageContent proxyMsg = new ProxyObrMessageContent();
                List<EntityDicObrMessageContent> listMsgInfo = proxyMsg.Service.GetDIYCriticalMsg(entityCodition);
                bsMsg.DataSource = listMsgInfo;

                tabControl1.SelectedTab = tabPage1;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                lis.client.control.MessageDialog.Show(ex.Message);
            }
        }

        private void sbtAdd_Click(object sender, EventArgs e)
        {
            FrmAddDIYMsg frm = new FrmAddDIYMsg();
            frm.ShowDialog();
            SearcheData();
        }

        private void sbtAddSel_Click(object sender, EventArgs e)
        {
            if (bsMsg.DataSource != null && bsMsg.Position >= 0)
            {
                // DataRow drCurr = ((DataRowView)bsMsg.Current).Row;
                EntityDicObrMessageContent listCurr = (EntityDicObrMessageContent)bsMsg.Current;
                if (listCurr != null)
                {
                    FrmAddDIYMsg frm = new FrmAddDIYMsg(listCurr);
                    frm.ShowDialog();
                    SearcheData();
                }
            }
            else
            {
                lis.client.control.MessageDialog.Show("请选中需要新增危急信息的病人");
            }
        }

        /// <summary>
        /// 读取危急值信息
        /// </summary>
        /// <returns></returns>
        private List<EntityPidReportMain> GetDtUrgentMsg()
        {
            List<EntityPidReportMain> listUrgentMsg = new List<EntityPidReportMain>();

            EntityUrgentHistoryUseParame eyWhere = new EntityUrgentHistoryUseParame();
            eyWhere.ReceiveID = "-1";
            eyWhere.DepIDs = strLocaDepID;
            eyWhere.FilterTime = "15";//获取推迟多少分钟的危急值信息
            eyWhere.FilterTime2 = "0";//获取推迟多少分钟的急查信息
            eyWhere.IsOnlyDIY = "1";
            ProxyUrgentObrMessage proxyUrgentObrMsg = new ProxyUrgentObrMessage();
            listUrgentMsg = proxyUrgentObrMsg.Service.GetUrgentHistoryMsgGetAll(eyWhere);//获取危急值信息

            return listUrgentMsg;
        }
        /// <summary>
        /// 获取未确认消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            List<EntityPidReportMain> dtMessages = GetDtUrgentMsg();
            if (dtMessages != null)
            {
                //物理组ID
                //string CType_id = dcl.client.common.LocalSetting.Current.Setting.CType_id;
                //如果物理组ID 为null或空 则不过滤物理组
                //if (!string.IsNullOrEmpty(CType_id))
                //{
                //    if (dtMessages != null && dtMessages.Columns.Contains("itr_type"))
                //    {
                //        dtMessages = filterDtForNew(dtMessages, string.Format("itr_type='{0}'", CType_id));
                //    }
                //}

                //if (dtMessages != null && dtMessages.Rows.Count > 0)
                //{
                ShowMessages(dtMessages);
            }
            else
            {
                msgCount = 0;
                showCount();
            }
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="dtShow"></param>
        private void ShowMessages(List<EntityPidReportMain> listShow)
        {
            //情况数据
            this.gcLookData.DataSource = null;
            frmShowInMsg.FillgvLookData(null);

            if (listShow == null || listShow.Count == 0)
            {
                msgCount = 0;
                showCount();
                //frmShowMsg.HideThisMsg();//隐藏
                frmShowInMsg.HideThisMsg();//
                return;
            }
            else
            {
                msgCount = listShow.Count;
            }

            this.gcLookData.DataSource = listShow;



            showCount();

            //if (this.WindowState != FormWindowState.Maximized || tabControl1.SelectedTab != tabPage2)
            {
                //frmShowMsg.ShowThisMsg();

                frmShowInMsg.FillgvLookData(listShow);
                frmShowInMsg.ShowThisMsg();
            }
        }

        /// <summary>
        /// 显示消息数
        /// </summary>
        private void showCount()
        {
            if (msgCount <= 0)
            {
                lblShowCount.Text = "";
            }
            else
            {
                lblShowCount.Text = string.Format("有危急消息超时未确认：{0}条", msgCount.ToString());
            }
        }

        private void gvLookData_DoubleClick(object sender, EventArgs e)
        {
            if (gvLookData.GetFocusedDataRow() == null) return;

            int rowIndex = gvLookData.FocusedRowHandle;
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

                if (!string.IsNullOrEmpty(pat_id))
                {
                    try
                    {
                        //危急值内部提醒验证录入临床信息
                        if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Urgent_AuditType_withdoc") == "是")
                        {
                            #region 更新临床信息

                            string strmsg_date = ServerDateTime.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss");

                            string[] patExtColName = new string[6];//列名
                            string[] patExtColValue = new string[6];//列值

                            patExtColName[0] = "msg_doc_num";//临床工号
                            patExtColName[1] = "msg_doc_name";//临床名称
                            patExtColName[2] = "msg_dep_tel";//临床电话
                            patExtColName[3] = "msg_date";//记录时间
                            patExtColName[4] = "msg_insgin_id";//危急值内部提示处理人
                            patExtColName[5] = "msg_insgin_date";//危急值内部提示处理时间

                            //patExtColValue[0] = "'" + CheckerInfo.msg_doc_num + "'";//列值
                            //patExtColValue[1] = "'" + CheckerInfo.msg_doc_name + "'";//列值
                            //patExtColValue[2] = "'" + CheckerInfo.msg_dep_tel + "'";//列值
                            patExtColValue[0] = "'" + CheckerInfo.MsgDocNum + "'";//列值
                            patExtColValue[1] = "'" + CheckerInfo.MsgDocName + "'";//列值
                            patExtColValue[2] = "'" + CheckerInfo.MsgDepTel + "'";//列值

                            patExtColValue[3] = "'" + strmsg_date + "'";//列值
                            patExtColValue[4] = "'" + CheckerInfo.UserId + "'";//列值
                            patExtColValue[5] = "'" + strmsg_date + "'";//列值

                            //保存病人扩展资料
                            //EntityOperationResult eoPatExt = PatientCRUDClient.NewInstance.AddOrUpdatePatientExt(patExtColName, patExtColValue, pat_id);

                            #endregion
                        }

                        string msg_id = eyPatData.ObrIdMsg;

                        ProxyObrMessage proxyObrMsg = new ProxyObrMessage();
                        proxyObrMsg.Service.DeleteMessageByIDAndUpdateCriticalChecker(CheckerInfo, msg_id, pat_id);
                        ProxyUrgentObrMessage proxyUrgObrMsg = new ProxyUrgentObrMessage();
                        proxyUrgObrMsg.Service.RefreshUrgentMessage();

                        timer1_Tick(null, null);
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
        /// 批量审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAudit_Click(object sender, EventArgs e)
        {
            if (this.gvLookData.RowCount > 0)
            {
                List<EntityPidReportMain> listBatAudit = gcLookData.DataSource as List<EntityPidReportMain>;
                List<EntityPidReportMain> listCeBatAudit = new List<EntityPidReportMain>();
                if (listBatAudit != null && listBatAudit.Count > 0)
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
                    #endregion

                    try
                    {
                        //记录时间
                        string strmsg_date = ServerDateTime.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                        ProxyObrMessage proxyObrMsg = new ProxyObrMessage();

                        foreach (var infoBatAudit in listCeBatAudit)
                        {
                            string msg_id = infoBatAudit.ObrIdMsg;
                            string pat_id = infoBatAudit.RepId;
                            if (!string.IsNullOrEmpty(pat_id))
                            {
                                //危急值内部提醒验证录入临床信息
                                if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Urgent_AuditType_withdoc") == "是")
                                {
                                    #region 更新临床信息

                                    string[] patExtColName = new string[6];//列名
                                    string[] patExtColValue = new string[6];//列值

                                    patExtColName[0] = "msg_doc_num";//临床工号
                                    patExtColName[1] = "msg_doc_name";//临床名称
                                    patExtColName[2] = "msg_dep_tel";//临床电话
                                    patExtColName[3] = "msg_date";//记录时间
                                    patExtColName[4] = "msg_insgin_id";//危急值内部提示处理人
                                    patExtColName[5] = "msg_insgin_date";//危急值内部提示处理时间

                                    //patExtColValue[0] = "'" + CheckerInfo.msg_doc_num + "'";//列值
                                    //patExtColValue[1] = "'" + CheckerInfo.msg_doc_name + "'";//列值
                                    //patExtColValue[2] = "'" + CheckerInfo.msg_dep_tel + "'";//列值
                                    patExtColValue[0] = "'" + CheckerInfo.MsgDocNum + "'";//列值
                                    patExtColValue[1] = "'" + CheckerInfo.MsgDocName + "'";//列值
                                    patExtColValue[2] = "'" + CheckerInfo.MsgDepTel + "'";//列值

                                    patExtColValue[3] = "'" + strmsg_date + "'";//列值
                                    patExtColValue[4] = "'" + CheckerInfo.UserId + "'";//列值
                                    patExtColValue[5] = "'" + strmsg_date + "'";//列值
                                    #endregion
                                }

                                proxyObrMsg.Service.DeleteMessageByIDAndUpdateCriticalChecker(CheckerInfo, msg_id, pat_id);
                            }
                        }
                        ProxyUrgentObrMessage proxyUrgObrMsg = new ProxyUrgentObrMessage();
                        proxyUrgObrMsg.Service.RefreshUrgentMessage();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("批量审核失败：\r\n" + ex.Message);
                    }

                    timer1_Tick(null, null);

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
            }

        }

        /// <summary>
        /// 过滤数据表信息
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        //private DataTable filterDtForNew(DataTable dtSource, string sqlWhere)
        //{
        //    try
        //    {
        //        if (dtSource != null && dtSource.Rows.Count > 0)
        //        {
        //            if (!string.IsNullOrEmpty(sqlWhere))//为空不过滤
        //            {
        //                DataTable dtCope = dtSource.Clone();
        //                DataRow[] drArray = dtSource.Select(sqlWhere);

        //                foreach (DataRow drItem in drArray)
        //                {
        //                    dtCope.Rows.Add(drItem.ItemArray);
        //                }
        //                dtCope.TableName = "UrgentMsg";
        //                return dtCope;
        //            }
        //            else
        //            {
        //                return dtSource.Copy();//为空不过滤
        //            }
        //        }
        //    }
        //    catch
        //    {

        //    }
        //    return dtSource.Copy();//错误或空，不过滤
        //}

        /// <summary>
        /// 批量审核(全选)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (gcLookData.DataSource != null)
            {
                List<EntityPidReportMain> dsource = gcLookData.DataSource as List<EntityPidReportMain>;
                if (dsource != null && dsource.Count > 0)
                {
                    bool IsAllCheck = checkEdit1.Checked;//是否全选
                    foreach (EntityPidReportMain drcheck in dsource)
                    {
                        drcheck.PatSelect = IsAllCheck;
                    }
                }
            }
        }

        private void FrmDIYCritical_MouseLeave(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 保存本地设置--科室
        /// </summary>
        /// <param name="strDepContent"></param>
        private void saveIniFileForLocaDep(string strDepContent)
        {
            string filepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "LocaDepInfo.ini";
            try
            {
                if (strDepContent == null) strDepContent = "";
                System.IO.File.WriteAllText(filepath, strDepContent);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 读取本地设置--科室
        /// </summary>
        /// <returns></returns>
        private string readIniFileForLocaDep()
        {
            string strDepContent = "";
            string filepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "LocaDepInfo.ini";
            try
            {
                if (System.IO.File.Exists(filepath))
                {
                    strDepContent = System.IO.File.ReadAllText(filepath);
                }
                else
                {
                    saveIniFileForLocaDep(null);
                }
            }
            catch (Exception ex)
            {
            }
            return strDepContent;
        }

        /// <summary>
        /// 保存本地设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sbtSaveLocaset_Click(object sender, EventArgs e)
        {
            //本地设置--科室
            if (string.IsNullOrEmpty(selectDicLocaDepart.valueMember))
            {
                saveIniFileForLocaDep(null);
            }
            else
            {
                saveIniFileForLocaDep(selectDicLocaDepart.valueMember);
            }

            lis.client.control.MessageDialog.Show("保存成功,本地设置需要重新打开程序才生效");
        }

        private void FrmDIYCritical_Shown(object sender, EventArgs e)
        {
            //读取本地设置
            strLocaDepID = readIniFileForLocaDep();
            if (!string.IsNullOrEmpty(strLocaDepID))
            {
                //加载科室信息
                List<EntityDicPubDept> dttemp_deptinfo = CacheClient.GetCache<EntityDicPubDept>();

                if (dttemp_deptinfo != null && dttemp_deptinfo.Count > 0)
                {
                    List<EntityDicPubDept> drtempList = dttemp_deptinfo.FindAll(r => r.DeptCode == strLocaDepID);
                    if (drtempList.Count > 0)
                    {
                        selectDicLocaDepart.valueMember = strLocaDepID;
                        selectDicLocaDepart.displayMember = drtempList[0].DeptName.ToString();
                        strLocaDepName = drtempList[0].DeptName.ToString();

                        selectDicPubDeptID.valueMember = strLocaDepID;
                        selectDicPubDeptID.displayMember = drtempList[0].DeptName.ToString();
                    }
                    else
                    {
                        strLocaDepID = "";
                    }
                }
            }
            frmShowInMsg = new FrmShowInMsg();
            frmShowInMsg.gvLookData_DoubleClickOKEvent += new FrmShowInMsg.gvLookData_DoubleClickHandler(frmShowInMsg_gvLookData_DoubleClickOKEvent);
            frmShowInMsg.startShowFrm();

            showCount();
            timer1.Start();
        }

        void frmShowInMsg_gvLookData_DoubleClickOKEvent(object sender, EventArgs e)
        {
            timer1_Tick(null, null);
        }



        private void FrmDIYCritical_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (MessageBox.Show("关闭程序将同时关闭内部提醒,是否继续关闭？", "关闭提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            //{
            //}
            //else
            //{
            //    e.Cancel = true;
            //}

            if (IsStrongClose)
            {
            }
            else
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            AnimateWindow(this.Handle, 1000, AW_ACTIVATE | AW_BLEND);//从下到上且不占其它程序焦点 
            this.WindowState = FormWindowState.Maximized;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("关闭程序将同时关闭内部提醒,是否继续关闭？", "关闭提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                IsStrongClose = true;
                this.Close();
            }
        }
    }
}
