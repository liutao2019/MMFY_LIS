using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using dcl.entity;
using dcl.client.frame;
using dcl.client.wcf;

namespace dcl.client.notifyclient
{
    public partial class FrmReagentMsg : Form
    {
        public FrmReagentMsg()
        {
            InitializeComponent();
        }
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
        /// <summary>
        /// 开始运行
        /// </summary>
        public void startShowFrm()
        {
            //设置frmShowClewText的位置,默认右下角
            int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width - this.Width;
            int y = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Height - this.Height;
            this.SetDesktopLocation(x, y);
            this.TopLevel = true;
            this.Hide();
            string IsMaximizedWindowState = GetAppValueByKey("IsMaximizedWindowState", "N");//如果为null添加默认值N

            if (IsMaximizedWindowState.ToUpper() == "Y" || IsMaximizedWindowState.ToUpper() == "YES")
            {
                //窗口最大化
                if (this.WindowState == FormWindowState.Normal || this.WindowState == FormWindowState.Minimized)
                {
                    this.WindowState = FormWindowState.Maximized;
                }
            }
            RefreshData(true);
        }
        /// <summary>
        /// 强制关闭窗体
        /// </summary>
        public void StriogClose()
        {

            this.Close();

            GC.Collect();
        }

        private void FrmReagentMsg_Load(object sender, EventArgs e)
        {
            //提示窗口是否最大化
            

        }
        /// <summary>
        /// 获取应用程序默认配置
        /// </summary>
        /// <param name="appKey">key值</param>
        /// <param name="defValue">默认value值(当为null)</param>
        /// <returns>value值</returns>
        private string GetAppValueByKey(string appKey, string defValue)
        {
            string kValue = null;

            try
            {
                kValue = ConfigurationManager.AppSettings[appKey];
            }
            catch
            {

            }
            //当默认值为空时,赋值-默认值
            if (string.IsNullOrEmpty(defValue))
            {
                defValue = "nullnull";
            }
            //当value为null时,赋值-默认值
            if (string.IsNullOrEmpty(kValue))
            {
                kValue = defValue;
            }

            return kValue;
        }

        void RefreshData(bool changeStatus)
        {
            if (!UserInfo.isAdmin && UserInfo.entityUserInfo.Func.FindIndex(m=>m.FuncId == 458) < 0)
            {
                return;
            }
            List<EntityReaSetting> highlist = new List<EntityReaSetting>();
            List<EntityReaSetting> lowlist = new List<EntityReaSetting>();
            List<EntityReaStorageDetail> duesoonlist = new List<EntityReaStorageDetail>();
            List<EntityReaStorageDetail> expiredlist = new List<EntityReaStorageDetail>();

            ProxyReaSetting proxysetting = new ProxyReaSetting();
            ProxyReaStorageDetail proxydetail = new ProxyReaStorageDetail();
            
            List<EntityReaSetting> listsetting = proxysetting.Service.SearchReaSettingAll();

            List<EntityReaStorageDetail> listdetail = proxydetail.Service.getNotdelivered();

            foreach (var item in listsetting)
            {
                if (item.Drea_upper_limit !=0 && item.Rri_Count >= item.Drea_upper_limit)
                {
                    highlist.Add(item);
                }
                if (item.Drea_lower_limit != 0 && item.Rri_Count <= item.Drea_lower_limit)
                {
                    lowlist.Add(item);
                }
                int day = 0;
                int.TryParse(item.Drea_alarmdays, out day);
                if (day != 0)
                {
                    duesoonlist = proxydetail.Service.QueryListByDate(DateTime.Now.Date.AddDays(day), item.Drea_id);
                }
                expiredlist = proxydetail.Service.QueryListByDate(DateTime.Now.Date, item.Drea_id);
            }
            gcStoreHigh.DataSource = highlist;
            gcStoreLow.DataSource = lowlist;
            gcExpired.DataSource = expiredlist;
            gcDueSoon.DataSource = duesoonlist;

            if (highlist.Count > 0 || lowlist.Count > 0|| expiredlist.Count > 0|| duesoonlist.Count > 0)
            {
                IntPtr activeForm = GetActiveWindow();
                this.Show();
                SetActiveWindow(activeForm);
                #region 声音提示
                PlaySoundMgr.Instance.PlaySound();
                #endregion

                gcStoreHigh.RefreshDataSource();
                gcStoreLow.RefreshDataSource();
                gcExpired.RefreshDataSource();
                gcDueSoon.RefreshDataSource();
            }

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                if (gcDueSoon != null)
                {
                    SaveFileDialog ofd = new SaveFileDialog();
                    ofd.DefaultExt = "xls";
                    ofd.Filter = "Excel文件(*.xls)|*.xls";
                    ofd.Title = "导出到Excel";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        if (ofd.FileName.Trim() == string.Empty)
                        {
                            lis.client.control.MessageDialog.Show("文件名不能为空！", "提示");
                            return;
                        }

                        try
                        {
                            gcDueSoon.ExportToXls(ofd.FileName.Trim());
                            lis.client.control.MessageDialog.Show("导出成功！", "提示");
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                
            }
            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                if (gcExpired != null)
                {
                    SaveFileDialog ofd = new SaveFileDialog();
                    ofd.DefaultExt = "xls";
                    ofd.Filter = "Excel文件(*.xls)|*.xls";
                    ofd.Title = "导出到Excel";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        if (ofd.FileName.Trim() == string.Empty)
                        {
                            lis.client.control.MessageDialog.Show("文件名不能为空！", "提示");
                            return;
                        }

                        try
                        {
                            gcExpired.ExportToXls(ofd.FileName.Trim());
                            lis.client.control.MessageDialog.Show("导出成功！", "提示");
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            if (xtraTabControl1.SelectedTabPageIndex == 2)
            {
                if (gcStoreHigh != null)
                {
                    SaveFileDialog ofd = new SaveFileDialog();
                    ofd.DefaultExt = "xls";
                    ofd.Filter = "Excel文件(*.xls)|*.xls";
                    ofd.Title = "导出到Excel";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        if (ofd.FileName.Trim() == string.Empty)
                        {
                            lis.client.control.MessageDialog.Show("文件名不能为空！", "提示");
                            return;
                        }

                        try
                        {
                            gcStoreHigh.ExportToXls(ofd.FileName.Trim());
                            lis.client.control.MessageDialog.Show("导出成功！", "提示");
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            if (xtraTabControl1.SelectedTabPageIndex == 3)
            {
                if (gcStoreLow != null)
                {
                    SaveFileDialog ofd = new SaveFileDialog();
                    ofd.DefaultExt = "xls";
                    ofd.Filter = "Excel文件(*.xls)|*.xls";
                    ofd.Title = "导出到Excel";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        if (ofd.FileName.Trim() == string.Empty)
                        {
                            lis.client.control.MessageDialog.Show("文件名不能为空！", "提示");
                            return;
                        }

                        try
                        {
                            gcStoreLow.ExportToXls(ofd.FileName.Trim());
                            lis.client.control.MessageDialog.Show("导出成功！", "提示");
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            StriogClose();
        }
    }
}
