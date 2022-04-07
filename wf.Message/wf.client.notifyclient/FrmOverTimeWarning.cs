using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using dcl.client.frame;
using System.Runtime.InteropServices;

namespace dcl.client.notifyclient
{
    public partial class FrmOverTimeWarning : FrmCommon
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();//获得当前活动窗体
        [DllImport("user32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr hwnd);//设置活动窗体
        Color gridrowColor = Color.Empty;

        /// <summary>
        /// 日期
        /// </summary>
        private DateTime pa_date=DateTime.MinValue;
        /// <summary>
        /// 仪器IDs
        /// </summary>
        private string pa_pat_itr_ids { get; set; }

        /// <summary>
        /// 多少分钟刷新
        /// </summary>
        public int OvertimeQueryInter = 1;

        /// <summary>
        /// 是否强关闭窗口
        /// </summary>
        private bool IsStrongClose = false;

        //public FrmOverTimeWarning()
        //{
        //    InitializeComponent();
        //    this.Hide();
        //    IsStrongClose = false;
        //}

        public FrmOverTimeWarning(DateTime date, string pat_itr_ids)
        {
            InitializeComponent();
            this.Hide();
            IsStrongClose = false;

            pa_date = date;
            pa_pat_itr_ids = pat_itr_ids;
        }

        public void LoadData(DataTable table)
        {
            gdSysLog.DataSource = table;
            gdSysLog.RefreshDataSource();
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
        /// 强制关闭窗体
        /// </summary>
        public void StriogClose()
        {
            IsStrongClose = true;

            this.Close();

            GC.Collect();
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmResultView_Load(object sender, EventArgs e)
        {
            this.timer1.Interval = OvertimeQueryInter * 60 * 1000;

            barSave.SetToolButtonStyle(new string[] { barSave.BtnClose.Name }, new string[] { "F4" });
            //PatientCRUDClient.NewInstance.GetPatientStatusForOverTime(patdate, itrID);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DataTable dtOverTime=null;

            if (pa_date != DateTime.MinValue && !string.IsNullOrEmpty(pa_pat_itr_ids))
            {
                //dtOverTime = dcl.client.wcf.PatientCRUDClient.NewInstance.GetPatientStatusForOverTime(pa_date, pa_pat_itr_ids);
            }

            ShowMessages(dtOverTime);//显示窗口
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="dtShow"></param>
        private void ShowMessages(DataTable dtShow)
        {
            if (dtShow == null || dtShow.Rows.Count == 0)
            {
                //清空数据
                this.gdSysLog.DataSource = null;
                this.Hide();
                return;
            }

            this.gdSysLog.DataSource = dtShow;

            if (!this.Visible)
            {
                IntPtr activeForm = GetActiveWindow();
                this.Show();
                SetActiveWindow(activeForm);
            }
        }

        private void FrmOverTimeWarning_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsStrongClose)//是否强关闭窗口
            {
                this.timer1.Enabled = false;
                this.timer1.Dispose();

                this.gdSysLog.DataSource = null;
                this.gdSysLog.Dispose();

                this.Dispose();
                //IsStrongClose = false;
            }
            else
            {
                e.Cancel = true;
                this.Hide();
            }
        }
    }
}
