using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using dcl.client.wcf;
using dcl.entity;
using System.Linq;

namespace dcl.client.notifyclient
{
    public partial class FrmBcSamplToReceiveTATNotify : Form
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

        public FrmBcSamplToReceiveTATNotify()
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
        /// 读取TAT信息(DataTable)缓存
        /// </summary>
        /// <returns></returns>
        private List<EntityDicMsgTAT> GetDtComTATMsg()
        {
            //DataTable dtComTATMsg = new DataTable("SamplToReceiveTATMsgCache");
            List<EntityDicMsgTAT> listComTATMsg = new List<EntityDicMsgTAT>();
            try
            {
                #region 查询条件(先前就已经注释了的代码)

                string strWhere = "";
                //string t_itr_type = dcl.client.common.LocalSetting.Current.Setting.CType_id;

                //string t_itr_id_list = dcl.client.common.LocalSetting.Current.Setting.ItrIDList;//仪器ID集合

                //if (!string.IsNullOrEmpty(t_itr_type))
                //{
                //    strWhere += string.Format("itr_type='{0}' ", t_itr_type);
                //}

                ////条件是否为不空,
                //if (!string.IsNullOrEmpty(strWhere))
                //{
                //    if ((!string.IsNullOrEmpty(t_itr_id_list)) && t_itr_id_list.Contains("'"))
                //    {
                //        strWhere += string.Format(" or pat_itr_id in({0}) ", t_itr_id_list);
                //    }
                //}
                //else
                //{
                //    if ((!string.IsNullOrEmpty(t_itr_id_list)) && t_itr_id_list.Contains("'"))
                //    {
                //        strWhere += string.Format("pat_itr_id in({0}) ", t_itr_id_list);
                //    }
                //}

                #endregion
                
                //ProxyMessage proxy = new ProxyMessage();
                //DataTable t_dtComTATMsg = proxy.Service.GetBcBcSamplToReceiveTATMessage(strWhere);//获取条码(采集到签收)TAT数据
                ProxyCombineTATMsg proxyComTATMsg = new ProxyCombineTATMsg();
                listComTATMsg = proxyComTATMsg.Service.GetBcBcSamplToReceiveTATMessage();//获取条码(采集到签收)TAT数据

                if (listComTATMsg != null && listComTATMsg.Count > 0)
                {
                    #region 过滤-合管的项目按照TAT时间最长显示（先前就已经注释掉了的代码）
                    /**
                    if (t_dtComTATMsg.Columns.Contains("time_tat"))
                    {
                        //按照最大TAT时间排序-降序
                        DataView dvtemp = t_dtComTATMsg.DefaultView.ToTable().DefaultView;
                        dvtemp.Sort = "time_mi desc";
                        t_dtComTATMsg = dvtemp.ToTable();

                        DataTable dtclonetemp = t_dtComTATMsg.Clone();
                        for (int j = 0; j < t_dtComTATMsg.Rows.Count; j++)
                        {
                            //每个pat_id，只选取TAT时间最大的
                            if (dtclonetemp.Select(string.Format("pat_id='{0}'", t_dtComTATMsg.Rows[j]["pat_id"])).Length<=0)
                            {
                                dtclonetemp.Rows.Add(t_dtComTATMsg.Rows[j].ItemArray);
                            }
                        }
                        
                        if (dtclonetemp != null)
                        {
                            //按照最大TAT时间排序-降序
                            dvtemp = dtclonetemp.DefaultView.ToTable().DefaultView;
                            dvtemp.Sort = "time_mi desc";
                            t_dtComTATMsg = dvtemp.ToTable();
                        }
                    }
                    **/
                    #endregion
                    //dtComTATMsg.TableName = "SamplToReceiveTATMsgCache";
                }
            }
            catch (Exception ex)
            {
            }

            return listComTATMsg;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            List<EntityDicMsgTAT> listMessage= GetDtComTATMsg();
            ShowMessages(listMessage);//显示窗口
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
            //dtShow.DefaultView.Sort = "over_tat desc";
            listShow = listShow.OrderByDescending(i => i.OverTat).ToList();

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

        private void FrmComTATNotify_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsStrongClose)//是否强关闭窗口
            {
                this.timer1.Enabled = false;
                this.timer1.Dispose();

                this.gcLookData.DataSource = null;
                this.gcLookData.Dispose();

                this.Dispose();
                //IsStrongClose = false;
            }
            else
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void gvLookData_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            DataRow dr = this.gvLookData.GetDataRow(e.RowHandle);
            if (dr != null)
            {
                //if (dr["over_tat"].ToString() == "1")//超出TAT为红色
                //{
                //    e.Appearance.BackColor = Color.Red;
                //}
            }
        }
    }
}
