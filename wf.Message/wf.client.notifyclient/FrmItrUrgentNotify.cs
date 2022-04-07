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
    public partial class FrmItrUrgentNotify : Form
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

        public FrmItrUrgentNotify()
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            List<EntityDicMsgTAT> listMessages = GetDtItrUrgentMsg();
            ShowMessages(listMessages);//显示窗口
        }

        /// <summary>
        /// 读取仪器危急值信息(DataTable)缓存
        /// </summary>
        /// <returns></returns>
        private List<EntityDicMsgTAT> GetDtItrUrgentMsg()
        {
            //DataTable dtItrUrgentMsg = new DataTable("ItrUrgentMsgCache");
            List<EntityDicMsgTAT> listItrUrgentMsg = new List<EntityDicMsgTAT>();
            try
            {
                //ProxyMessage proxy = new ProxyMessage();
                //DataTable t_dtItrUrgentMsg = proxy.Service.GetItrUrgentMessage(strWhere);//获取仪器危急值
                ProxyInstrmtUrgentTATMsg proxyInstrmt = new ProxyInstrmtUrgentTATMsg();
                listItrUrgentMsg = proxyInstrmt.Service.GetItrUrgentMessage();//获取仪器危急值

                #region 查询条件(过滤)

                string t_itr_type = dcl.client.common.LocalSetting.Current.Setting.CType_id;

                string t_itr_id_list = dcl.client.common.LocalSetting.Current.Setting.ItrIDList;//仪器ID集合

                if (listItrUrgentMsg != null && listItrUrgentMsg.Count > 0)
                {
                    if (!string.IsNullOrEmpty(t_itr_type))
                    {
                        //strWhere += string.Format("itr_type='{0}' ", t_itr_type);
                        if ((!string.IsNullOrEmpty(t_itr_id_list)) && t_itr_id_list.Contains("'"))
                        {
                            //strWhere += string.Format(" or pat_itr_id in({0}) ", t_itr_id_list);
                            listItrUrgentMsg = listItrUrgentMsg.Where(w => w.ItrType == t_itr_type || t_itr_id_list.Contains(w.RepItrId)).ToList();
                        }
                        else
                        {
                            listItrUrgentMsg = listItrUrgentMsg.Where(w => w.ItrType == t_itr_type).ToList();
                        }
                    }
                    else
                    {
                        if ((!string.IsNullOrEmpty(t_itr_id_list)) && t_itr_id_list.Contains("'"))
                        {
                            //strWhere += string.Format("pat_itr_id in({0}) ", t_itr_id_list);
                            listItrUrgentMsg = listItrUrgentMsg.Where(w => w.RepItrId == t_itr_id_list).ToList();
                        }
                    }
                }
                #endregion

                #region 测试数据(先前就已注释)

                //dtItrUrgentMsg.Columns.Add("pat_id");
                //dtItrUrgentMsg.Columns.Add("pat_itr_id");
                //dtItrUrgentMsg.Columns.Add("pat_sid");
                //dtItrUrgentMsg.Columns.Add("pat_host_order");
                //dtItrUrgentMsg.Columns.Add("pat_name");

                //dtItrUrgentMsg.Columns.Add("pat_date",Type.GetType("System.DateTime"));
                //dtItrUrgentMsg.Columns.Add("pat_flag");
                //dtItrUrgentMsg.Columns.Add("itr_mid");
                //dtItrUrgentMsg.Columns.Add("itr_type");
                //dtItrUrgentMsg.Columns.Add("type_name");

                //DataRow drADD = dtItrUrgentMsg.NewRow();

                //drADD[0] = "10178201301152";
                //drADD[1] = "10178";
                //drADD[2] = "2";
                //drADD[3] = "";
                //drADD[4] = "统一奶茶";

                //drADD[5] = "2013-01-15 14:56:28.000";
                //drADD[6] = "0";
                //drADD[7] = "凝血STACompact";
                //drADD[8] = "10074";
                //drADD[9] = "血液";

                //dtItrUrgentMsg.Rows.Add(drADD);

                //drADD = dtItrUrgentMsg.NewRow();

                //drADD[0] = "10179201301081";
                //drADD[1] = "10179";
                //drADD[2] = "1";
                //drADD[3] = "";
                //drADD[4] = "鹅鹅鹅";

                //drADD[5] = "2013-01-08 14:49:50.000";
                //drADD[6] = "1";
                //drADD[7] = "BC-5180";
                //drADD[8] = "10074";
                //drADD[9] = "血液";

                //dtItrUrgentMsg.Rows.Add(drADD);

                //drADD = dtItrUrgentMsg.NewRow();

                //drADD[0] = "10184201303141";
                //drADD[1] = "10184";
                //drADD[2] = "1";
                //drADD[3] = "";
                //drADD[4] = "林型畅";

                //drADD[5] = "2013-03-14 17:13:18.000";
                //drADD[6] = "0";
                //drADD[7] = "7080";
                //drADD[8] = "10073";
                //drADD[9] = "生化";

                //dtItrUrgentMsg.Rows.Add(drADD);

                #endregion

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return listItrUrgentMsg;
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="dtShow"></param>
        private void ShowMessages(List<EntityDicMsgTAT> listShow)
        {
            if (listShow == null || listShow.Count == 0)
            {
                //清空数据
                this.gcLookData.DataSource = null;
                showCountToLbl(0);
                this.Hide();
                return;
            }
            else
            {
                showCountToLbl(listShow.Count);
            }

            //dtShow.DefaultView.Sort = "pat_itr_id,pat_date,pat_sid asc";
            listShow = listShow.OrderBy(i => i.RepItrId).ThenBy(i=>i.RepInDate).ThenBy(i=>i.RepSid).ToList();

            this.gcLookData.DataSource = listShow;
            
            PlaySoundMgr.Instance.PlaySound();

            if (!this.Visible)
            {
                IntPtr activeForm = GetActiveWindow();
                this.Show();
                SetActiveWindow(activeForm);
            }
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

        private void FrmItrUrgentNotify_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsStrongClose)//是否强关闭窗口
            {
                this.timer1.Enabled = false;
                this.timer1.Dispose();

                this.gcLookData.DataSource = null;
                this.gcLookData.Dispose();

                //this.Dispose();
                //IsStrongClose = false;
            }
            else
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void gvLookData_DoubleClick(object sender, EventArgs e)
        {
            if (gvLookData.GetFocusedRow() == null) return;

            int rowIndex = gvLookData.FocusedRowHandle;
            //DataRow drPatData = gvLookData.GetFocusedDataRow();
            EntityDicMsgTAT eyPatData = this.gvLookData.GetFocusedRow() as EntityDicMsgTAT;

            if (eyPatData != null)
            {
                #region 处理危急值消息

                string pat_id = eyPatData.RepId;

                string t_pat_name = eyPatData.PidNameNew;//病人名称

                string t_pat_itrName = eyPatData.ItrEname;//仪器名称

                string t_pat_date = "";//日期
                DateTime dtiNow = DateTime.Now;
                if (DateTime.TryParse(eyPatData.RepInDate.ToString(), out dtiNow))
                {
                    t_pat_date = dtiNow.ToString("yyyy-MM-dd");
                }

                string t_pat_sid = eyPatData.RepSid;//样本号

                string t_pat_host_order = eyPatData.RepSerialNum;//序号

                #region 用户验证(先前就已注释)

                //AuditInfo CheckerInfo = null;

                ////验证用户
                //FrmReadAffirm frmRA = null;


                //frmRA = new FrmReadAffirm();

                //if (frmRA.ShowDialog() != DialogResult.Yes)
                //{
                //    return;
                //}
                //CheckerInfo = frmRA.m_userInfo;
                //CheckerInfo.Place = string.Format("{0}({1})", Environment.MachineName, GetClientIPv4());

                #endregion

                string affirmText =
                    string.Format("日期：{0}\r\n仪器：{1}\r\n样本号：{2}\r\n序号：{3}\r\n名称：{4}\r\n\r\n确定消除此提示？"
                    , t_pat_date, t_pat_itrName
                    , t_pat_sid, t_pat_host_order, t_pat_name);

                if (MessageBox.Show(affirmText, "处理确认", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                {
                    return;
                }

                if (!string.IsNullOrEmpty(pat_id))
                {
                    try
                    {
                        //ProxyMessage proxy = new ProxyMessage();
                        ProxyInstrmtUrgentTATMsg proxyItrMsg = new ProxyInstrmtUrgentTATMsg();
                        if (proxyItrMsg.Service.DeleteItrUrgentMsgDataByID(pat_id))
                        {
                            timer1_Tick(null, null);//刷新
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
    }
}
