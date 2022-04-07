using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using dcl.client.wcf;
using System.Net;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.msgsend
{
    public partial class FrmShowInMsg :System.Windows.Forms.Form
    {
        public FrmShowInMsg()
        {
            InitializeComponent();
            //设置frmShowClewText的位置,默认右下角
            int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width - this.Width;
            int y = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - this.Height;
            this.SetDesktopLocation(x, y);
            this.Hide();
        }

        public void startShowFrm()
        {
            //设置frmShowClewText的位置,默认右下角
            int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width - this.Width;
            int y = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - this.Height;
            this.SetDesktopLocation(x, y);

            this.Hide();
        }

        public void HideThisMsg()
        {
            if (this.Visible)
            {
                this.Hide();
            }
        }

        public void ShowThisMsg()
        {
            if (!this.Visible)
            {
                this.TopMost = true;
                this.Show();
            }
        }

        public void FillgvLookData(List<EntityPidReportMain> listShow)
        {
            gcLookData.DataSource = listShow;
        }

        private void FrmShowMsg_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            if (ReturnOKEvent != null)
            {
                ReturnOKEvent(null);
            }
            this.Hide();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ReturnOKEvent(null);
            this.Hide();
        }

        /// <summary>
        /// 委托
        /// </summary>
        /// <param name="dt"></param>
        public delegate void ReturnOKHandler(object sender);

        public delegate void gvLookData_DoubleClickHandler(object sender, EventArgs e);

        /// <summary>
        /// 事件
        /// </summary>
        public event ReturnOKHandler ReturnOKEvent;

        public event gvLookData_DoubleClickHandler gvLookData_DoubleClickOKEvent;

        private void gvLookData_DoubleClick(object sender, EventArgs e)
        {
            if (gvLookData.GetFocusedDataRow() == null) return;

            int rowIndex = gvLookData.FocusedRowHandle;
            DataRow drPatData = gvLookData.GetFocusedDataRow();

            if (drPatData != null)
            {
                #region 处理危急值消息
                string pat_id = drPatData["pat_id"].ToString();

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

                        string msg_id = drPatData["msg_id"].ToString();

                        //ProxyMessage proxy = new ProxyMessage();
                        //proxy.Service.DeleteMessageByIDAndUpdateCriticalChecker(CheckerInfo, msg_id, pat_id);
                        ProxyObrMessage proxyObrMsg = new ProxyObrMessage();
                        proxyObrMsg.Service.DeleteMessageByIDAndUpdateCriticalChecker(CheckerInfo, msg_id, pat_id);
                        
                        //proxy.Service.RefreshUrgentMessage();
                        ProxyUrgentObrMessage proxyUrgObrMsg = new ProxyUrgentObrMessage();
                        proxyUrgObrMsg.Service.RefreshUrgentMessage();
                        
                        if (gvLookData_DoubleClickOKEvent != null)
                        {
                            gvLookData_DoubleClickOKEvent(sender, e);
                            //timer1_Tick(null, null);
                        }
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


    }
}
