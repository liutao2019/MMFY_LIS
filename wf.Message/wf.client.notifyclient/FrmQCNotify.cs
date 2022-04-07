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
    public partial class FrmQCNotify : Form
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();//获得当前活动窗体
        [DllImport("user32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr hwnd);//设置活动窗体
        Color gridrowColor = Color.Empty;

        /// <summary>
        /// 是否强关闭窗口
        /// </summary>
        private bool IsStrongClose = false;

        public FrmQCNotify()
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
            //设置frmShowClewText的位置,默认右下角
            int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width - this.Width;
            int y = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - this.Height;
            this.SetDesktopLocation(x, y);

            this.Hide();
            IsStrongClose = false;
            this.timer1.Start();//启动计时器
        }

        /// <summary>
        /// 显示信息数
        /// </summary>
        /// <param name="cou"></param>
        private void showCountToLbl(int cou)
        {
            if (cou == null || cou <= 0)
            {
                lblShowCount.Text = "消息：0条";
            }
            else
            {
                lblShowCount.Text = "消息：" + cou.ToString() + "条";
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

        /// <summary>
        /// 读取仪器质控信息(List<EntityDicQcRuleMes>)
        /// </summary>
        /// <returns></returns>
        private List<EntityDicQcRuleMes> GetDtItrQcMsg()
        {
            //DataTable dtItrQcMsg = new DataTable("dtItrQcMsg");
            List<EntityDicQcRuleMes> listItrQcMsg = new List<EntityDicQcRuleMes>();
            try
            {
                string t_itr_type = dcl.client.common.LocalSetting.Current.Setting.CType_id;
                string typeIdList = dcl.client.common.LocalSetting.Current.Setting.TypeIDList;//物理组id集合
                string itrIdList = dcl.client.common.LocalSetting.Current.Setting.ItrIDList;//仪器id集合
                #region 查询条件（暂时没有）

                string strWhere = "";

                if (!string.IsNullOrEmpty(t_itr_type))
                {
                    //strWhere += string.Format("itr_type='{0}' ", t_itr_type);
                }

                //条件是否为不空,
                if (!string.IsNullOrEmpty(strWhere))
                {
                    //if ((!string.IsNullOrEmpty(t_itr_id_list)) && t_itr_id_list.Contains("'"))
                    //{
                    //    strWhere += string.Format(" or pat_itr_id in({0}) ", t_itr_id_list);
                    //}
                }
                else
                {
                    //if ((!string.IsNullOrEmpty(t_itr_id_list)) && t_itr_id_list.Contains("'"))
                    //{
                    //    strWhere += string.Format("pat_itr_id in({0}) ", t_itr_id_list);
                    //}
                }

                #endregion

                //ProxyMessage proxy = new ProxyMessage();
                //DataTable t_dtItrQcMsg = proxy.Service.GetItrQcMessage(strWhere);//获取仪器质控信息
                ProxyObrMessage proxyObrMsg = new ProxyObrMessage();
                listItrQcMsg = proxyObrMsg.Service.GetItrQcMessage(strWhere);//获取仪器质控信息
                if (!string.IsNullOrEmpty(typeIdList))
                {
                    if (!string.IsNullOrEmpty(itrIdList))
                    {
                        listItrQcMsg = listItrQcMsg.FindAll(w => typeIdList.Contains(w.ItrLabId) || itrIdList.Contains(w.QcmItrId));
                    }
                    else {
                        listItrQcMsg = listItrQcMsg.FindAll(w => typeIdList.Contains(w.ItrLabId));
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(itrIdList))
                    {
                        listItrQcMsg = listItrQcMsg.FindAll(w => typeIdList.Contains(w.ItrLabId));
                    }
                }
                if (listItrQcMsg != null && listItrQcMsg.Count > 0)
                {
                    foreach (var infoTemp in listItrQcMsg)
                    {
                        if (infoTemp.QcmType == "新增")
                        {
                            infoTemp.QcmType = "未审核";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return listItrQcMsg;
        }

        private void FrmQCNotify_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsStrongClose)//是否强关闭窗口
            {
                this.timer1.Enabled = false;
                this.timer1.Dispose();

                this.gcQcData.DataSource = null;
                this.gcQcData.Dispose();

                this.Dispose();
                //IsStrongClose = false;
            }
            else
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            List<EntityDicQcRuleMes> listMessages = GetDtItrQcMsg();
            ShowMessages(listMessages);//显示窗口
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="dtShow"></param>
        private void ShowMessages(List<EntityDicQcRuleMes> listShow)
        {
            if (listShow == null || listShow.Count == 0)
            {
                //清空数据
                this.gcQcData.DataSource = null;
                showCountToLbl(0);
                this.Hide();
                return;
            }
            else
            {
                showCountToLbl(listShow.Count);
            }
            //dtShow.DefaultView.Sort = "qrm_start_time,qcm_itr_id,qrm_item_id asc";
            listShow = listShow.OrderBy(i => i.QrmStartTime).ThenBy(i => i.QcmItrId).ThenBy(i => i.QrmItemId).ToList();

            this.gcQcData.DataSource = listShow;


            PlaySoundMgr.Instance.PlaySound();

            if (!this.Visible)
            {
                IntPtr activeForm = GetActiveWindow();
                this.Show();
                SetActiveWindow(activeForm);
            }
        }
    }
}
