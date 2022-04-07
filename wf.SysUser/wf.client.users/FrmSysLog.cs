using System;
using System.Collections.Generic;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.entity;
using dcl.client.wcf;

namespace dcl.client.users
{
    public partial class FrmSysLog : FrmCommon
    {
        public FrmSysLog()
        {
            InitializeComponent();
        }
        ProxySysLog proxy = new ProxySysLog();
        /// <summary>
        /// 窗体载入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSysLog_Load(object sender, EventArgs e)
        {
            //初始化工具条
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnSearch.Name, sysToolBar1.BtnDelete.Name, sysToolBar1.BtnExport.Name, sysToolBar1.BtnClose.Name });

            //初始化时间段
            timeFrom.EditValue = DateTime.Now.AddDays(-3);
            timeTo.EditValue = DateTime.Now;

            //初始化模块和操作人员下拉框
            List<EntitySysFunction> listFunc = proxy.Service.GetFuncName();
            List<EntitySysUser> listUser = proxy.Service.GetLoginId();

            cboModule.Properties.Items.Add("用户登录");
            foreach (EntitySysFunction func in listFunc)
            {
                string funcName = func.FuncName;

                if (funcName.IndexOf(".") != -1)
                {
                    funcName = funcName.Substring(funcName.IndexOf(".") + 1);
                }

                if (funcName.IndexOf("(") != -1)
                {
                    funcName = funcName.Substring(0, funcName.IndexOf("("));
                }

                cboModule.Properties.Items.Add(funcName);
            }

            foreach (EntitySysUser user in listUser)
            {
                cboLoginId.Properties.Items.Add(user.UserLoginid);
            }
        }

        /// <summary>
        /// 查询日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnSearchClicked(object sender, EventArgs e)
        {
            LoadData();

            if (this.isActionSuccess)
            {
                sysToolBar1.LogMessage = String.Format("从{0}到{1}", timeFrom.Text, timeTo.Text);
            }
        }

        private void LoadData()
        {
            gdSysLog.DataSource = proxy.Service.GetSysLog(cboLoginId.Text.Trim(), cboModule.Text.Trim(), timeFrom.Text,timeTo.Text);
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 删除日志_只有按时间段批量删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            string timespan = String.Format("从{0}到{1}", timeFrom.Text, timeTo.Text);

            DialogResult dresult = MessageBox.Show("确认要清除" + timespan + "的所有操作日志吗?", PowerMessage.BASE_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            switch (dresult)
            {
                case DialogResult.OK:
                    proxy.Service.DeleteSysLog(timeFrom.Text, timeTo.Text);
                    break;
                case DialogResult.Cancel:
                    return;
            }

            if (this.isActionSuccess)
            {
                sysToolBar1.LogMessage = timespan;
            }

            LoadData();
        }

        private void sysToolBar1_OnBtnExportClicked(object sender, EventArgs e)
        {
            if (gdSysLog.DataSource != null)
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.DefaultExt = "xls";
                ofd.Filter = "Excel文件(*.xls)|*.xls";
                ofd.Title = "导出到Excel";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.Trim() == string.Empty)
                    {
                        lis.client.control.MessageDialog.Show("文件名不能为空！", "消息");
                        return;
                    }

                    try
                    {
                        gdSysLog.ExportToXls(ofd.FileName);

                        lis.client.control.MessageDialog.Show("导出成功！", "消息");
                    }
                    catch (Exception)
                    {
                    }
                }

            }
        }
    }
}
