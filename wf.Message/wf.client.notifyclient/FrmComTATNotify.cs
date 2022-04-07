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
    public partial class FrmComTATNotify : Form
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

        public FrmComTATNotify()
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
        /// 读取组合TAT信息(DataTable)缓存
        /// </summary>
        /// <returns></returns>
        private List<EntityDicMsgTAT> GetDtComTATMsg()
        {
            //DataTable dtComTATMsg = new DataTable("ComTATMsgCache");
            List<EntityDicMsgTAT> list_dtComTATMsg = new List<EntityDicMsgTAT>();
            try
            {
                //ProxyMessage proxy = new ProxyMessage();
                //DataTable t_dtComTATMsg = proxy.Service.GetComTATMessage(strWhere);//获取组合TAT信息
                ProxyCombineTATMsg proxy = new ProxyCombineTATMsg();
                list_dtComTATMsg = proxy.Service.GetComTATMessage();//获取组合TAT信息

                #region 查询条件

                //string strWhere = "";
                string t_itr_type = dcl.client.common.LocalSetting.Current.Setting.CType_id;

                string t_itr_id_list = dcl.client.common.LocalSetting.Current.Setting.ItrIDList;//仪器ID集合

                //条件进行过整合
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
                        list_dtComTATMsg = list_dtComTATMsg.Where(w => w.RepItrId == t_itr_id_list).ToList();
                    }
                }
                #endregion

                if (list_dtComTATMsg != null && list_dtComTATMsg.Count > 0)
                {
                    //排序
                    //dvtempsort.Sort = "time_mi_over desc";

                    list_dtComTATMsg = list_dtComTATMsg.OrderByDescending(i => i.TimeMiOver).ToList();

                    #region 过滤-合管的项目按照TAT时间最长显示(已注释)
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
                    //dtComTATMsg.TableName = "ComTATMsgCache";
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return list_dtComTATMsg;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            List<EntityDicMsgTAT> listMessages = GetDtComTATMsg();
            ShowMessages(listMessages);//显示窗口
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
            //dtShow.DefaultView.Sort = "time_mi_over desc";
            listShow = listShow.OrderByDescending(i => i.TimeMiOver).ToList();

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
            //检验组合TAT提醒允许关闭
            if (IsStrongClose == false && dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Combine_TAT_NotifyAllowClose") == "是")
            {
                lis.client.control.FrmCheckPassword frmpw = new lis.client.control.FrmCheckPassword();
                if (frmpw.ShowDialog() == DialogResult.OK)
                {
                    IsStrongClose = true;
                }
            }

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
            //DataRow dr = this.gvLookData.GetDataRow(e.RowHandle);
            EntityDicMsgTAT eyMsgTAT = this.gvLookData.GetRow(e.RowHandle) as EntityDicMsgTAT;
            if (eyMsgTAT != null)
            {
                //if (dr["over_tat"].ToString() == "1")//超出TAT为红色
                if (eyMsgTAT.OverTat.ToString() == "1")//超出TAT为红色
                {
                    e.Appearance.BackColor = Color.Red;
                }
            }
        }
    }
}
