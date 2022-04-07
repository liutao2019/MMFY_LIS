using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.entity;
using dcl.client.wcf;

namespace dcl.client.result
{
    public partial class FrmResultView : FrmCommon
    {
        public FrmResultView()
        {
            InitializeComponent();
            this.Shown += new EventHandler(FrmResultView_Shown);
            autoSave = UserInfo.GetSysConfigValue("Lab_ResultTemplateShowResultView") == "否";
            if (autoSave)
            {
                this.Hide();
                this.Text = "";
                this.Size = new Size(0, 0);
            }
            barSave.QuickOption = true; 
        }
        bool autoSave = false;
        void FrmResultView_Shown(object sender, EventArgs e)
        {
            if (autoSave)
            {
                barSave_OnBtnSaveClicked(sender, e);
            }
        }

        public List<EntityObrResult> DataSource;

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmResultView_Load(object sender, EventArgs e)
        {
            barSave.SetToolButtonStyle(new string[] { barSave.BtnSave.Name }, new string[] { "F3" });
            if (autoSave)
            {
                this.Hide();
                this.Size = new Size(0, 0);
            }
            if (DataSource != null)
            {
                //gdSysLog.DataSource = DataSource;
                bsResult.DataSource = DataSource;
            }
        }
        ProxyResultTemp proxy = new ProxyResultTemp();
        /// <summary>
        /// 保存批量录入的结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void barSave_OnBtnSaveClicked(object sender, EventArgs e)
        {
            if (DataSource == null || DataSource.Count == 0)
            {
                lis.client.control.MessageDialog.Show("没有需要保存的数据", "信息");
                return;
            }

            try
            {

                string message = string.Empty;// "保存成功";
                ProxyElisaAnalyse elisaProxy = new ProxyElisaAnalyse();
                string notsave = elisaProxy.Service.SaveBatchObrResult(DataSource);
                if (notsave != "")
                {
                    message += "\r\n" + notsave;
                    lis.client.control.MessageDialog.Show(message, "信息");
                }
                else
                {
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("保存成功！",2m);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch
            {
                lis.client.control.MessageDialog.Show("保存失败,请检查数据的有效性", "信息");
                return;
            }
        }
    }
}
