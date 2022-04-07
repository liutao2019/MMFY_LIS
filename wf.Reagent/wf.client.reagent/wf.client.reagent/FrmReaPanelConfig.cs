using dcl.client.frame;
using dcl.client.frame.runsetting;
using dcl.client.result.Interface;
using dcl.root.logon;
using lis.client.control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace wf.client.reagent
{
    public partial class FrmReaPanelConfig : Form
    {
        /// <summary>
        /// 式样窗体对象
        /// </summary>
        IPatPanelConfig pForm = null;
        /// <summary>
        /// 父窗体名称
        /// </summary>
        string pFormName = string.Empty;

        public FrmReaPanelConfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="frmPatient"></param>
        public FrmReaPanelConfig(IPatPanelConfig frmPatient)
        {
            InitializeComponent();

            pForm = frmPatient;
            if (pForm != null)
            {
                pFormName = pForm.GetType().Name;
                this.Text = "面板设置(" + pForm.Text + ")";
            }
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FromReaPanelConfig_Load(object sender, EventArgs e)
        {
            //if (pForm is FrmPatEnter)//窗体为普通病人录入时显示病人结果配置页
            //{
            //    tabPatResult.PageVisible = true;
            //}
            //else
            //{
            //    tabPatResult.PageVisible = false;
            //}
            //加载式样
            LoadSetting();

            //当前用户为管理员时显示加载程序默认/保存系统默认
            if (!UserInfo.isAdmin)
            {
                this.btnLoadProgrmDefault.Visible = false;
                this.btnSaveAsSystem.Visible = false;
            }

        }
        /// <summary>
        /// 加载设置
        /// </summary>
        public void LoadSetting()
        {
            string userID = UserInfo.loginID;
            PatInputRuntimeSetting setting = PatInputRuntimeSetting.Load(this.pFormName, string.Empty, userID);

            LoadSetting(setting);

        }
        /// <summary>
        /// 加载设置
        /// </summary>
        /// <param name="setting"></param>
        public void LoadSetting(PatInputRuntimeSetting setting)
        {
            #region 病人列表
            this.colorAuditedBack.Color = setting.PatListPanel.BackColorAudited;
            this.colorSavedBack.Color = setting.PatListPanel.BackColorNormal;
            this.colorPrintedBack.Color = setting.PatListPanel.BackColorPrinted;
            this.colorReturnBack.Color = setting.PatListPanel.BackColorReturn;
            this.colorDoneBack.Color = setting.PatListPanel.BackColorDone;

            

            this.colorAuditedFore.Color = setting.PatListPanel.ForeColorAudited;
            this.colorSavedFore.Color = setting.PatListPanel.ForeColorNormal;
            this.colorPrintedFore.Color = setting.PatListPanel.ForeColorPrinted;
            this.colorReturnFore.Color = setting.PatListPanel.ForeColorReturn;
            this.colorDoneFore.Color = setting.PatListPanel.ForeColorDone;

            #endregion

            

        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                PatInputRuntimeSetting setting = GetCurrentSetting();
                PatInputRuntimeSetting.SaveUser(this.pFormName, UserInfo.loginID, setting);

                ApplySetting(setting);
                MessageDialog.Show("保存成功", "提示");
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, string.Format("保存用户配置失败，用户ID={0}", UserInfo.loginID), ex.ToString());
                MessageDialog.Show("保存失败", "提示", MessageBoxButtons.OK);
            }
        }
        /// <summary>
        /// 保存为系统默认(管理员)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveAsSystem_Click(object sender, EventArgs e)
        {
            if (MessageDialog.Show("此操作将会把现有的系统默认覆盖，是否继续？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    PatInputRuntimeSetting setting = GetCurrentSetting();
                    PatInputRuntimeSetting.SaveSystem(this.pFormName, setting);

                    MessageDialog.Show("保存成功", "提示");
                }
                catch (Exception ex)
                {
                    Logger.WriteException(this.GetType().Name, string.Format("保存系统默认配置失败，用户ID={0}", UserInfo.loginID), ex.ToString());
                    MessageDialog.Show("保存失败", "提示", MessageBoxButtons.OK);
                }
            }
        }

        /// <summary>
        /// 加载程序默认(管理员)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadProgrmDefault_Click(object sender, EventArgs e)
        {
            if (MessageDialog.Show("此操作将会把现有的覆盖，是否继续？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                LoadSetting(new PatInputRuntimeSetting());
            }
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 加载系统默认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadDefault_Click(object sender, EventArgs e)
        {
            if (MessageDialog.Show("加载系统默认将会把现有的覆盖，是否继续？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                PatInputRuntimeSetting setting = PatInputRuntimeSetting.LoadSystem(this.pFormName);

                if (setting == null)
                {
                    setting = new PatInputRuntimeSetting();
                }

                LoadSetting(setting);
            }
        }
        /// <summary>
        /// 点击应用设置(不保存)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApply_Click(object sender, EventArgs e)
        {
            PatInputRuntimeSetting setting = GetCurrentSetting();
            ApplySetting(setting);

        }
        /// <summary>
        /// 应用设置
        /// </summary>
        /// <param name="setting"></param>
        private void ApplySetting(PatInputRuntimeSetting setting)
        {
            if (pForm != null)
            {
                pForm.ApplySetting(setting);
                this.Focus();
            }
        }
        /// <summary>
        /// 获取当前最新设置
        /// </summary>
        /// <returns></returns>
        public PatInputRuntimeSetting GetCurrentSetting()
        {
            PatInputRuntimeSetting setting = new PatInputRuntimeSetting();

            #region 病人列表
            setting.PatListPanel.BackColorAudited = colorAuditedBack.Color;
            setting.PatListPanel.BackColorNormal = colorSavedBack.Color;
            setting.PatListPanel.BackColorPrinted = colorPrintedBack.Color;
            setting.PatListPanel.BackColorReturn = colorReturnBack.Color;
            setting.PatListPanel.BackColorDone = colorDoneBack.Color;

            setting.PatListPanel.ForeColorAudited = colorAuditedFore.Color;
            setting.PatListPanel.ForeColorNormal = colorSavedFore.Color;
            setting.PatListPanel.ForeColorPrinted = colorPrintedFore.Color;
            setting.PatListPanel.ForeColorReturn = colorReturnFore.Color;
            setting.PatListPanel.ForeColorDone = colorDoneFore.Color;

            #endregion


            return setting;
        }
    }
}
