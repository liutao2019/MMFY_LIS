using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.wcf;
using dcl.entity;


namespace dcl.client.control
{
    /// <summary>
    /// 报告解读信息
    /// </summary>
    public partial class FrmReportSumInfo : FrmCommon
    {
        string PatID;
        public FrmReportSumInfo(string pat_id)
        {
            InitializeComponent();
            PatID = pat_id;

            this.Shown += FrmReportSumInfo_Shown;
        }

        private void FrmReportSumInfo_Shown(object sender, EventArgs e)
        {
            splitContainerControl1.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
            this.Height = 300;
            if (string.IsNullOrEmpty(PatID)) return;
            EntityPidReportMain patInfo= new ProxyPatEnterNew().Service.GetReportInterpretation(PatID);

            if(patInfo!=null)
            {
                txtSumInfo.Text = patInfo.RepSumInfo;
                //让文本框获取焦点 
                this.txtSumInfo.Focus();
                //设置光标的位置到文本尾 
                this.txtSumInfo.Select(this.txtSumInfo.Text.Length, 0);
                //滚动到控件光标处 
                this.txtSumInfo.ScrollToCaret();
            }

            if (!string.IsNullOrEmpty(PatID))
            {
                try
                {
                    gcComment.DataSource = new ProxyPatEnterNew().Service.GetReportComment(PatID);
                }
                catch(Exception EX)
                {
                    Lib.LogManager.Logger.LogException(EX);
                }
            }
        }

        private void btnComment_Click(object sender, EventArgs e)
        {
            if (splitContainerControl1.PanelVisibility == DevExpress.XtraEditors.SplitPanelVisibility.Both)
            {
                splitContainerControl1.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
                this.Height = 300;
            }
            else
            {
                splitContainerControl1.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both;
                this.Height = 580;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCommentInfo.Text))
            {
                MessageBox.Show("评价内容不能为空.");
                return;
            }
            if (string.IsNullOrEmpty(PatID))
            {
                MessageBox.Show("无对应报告单，无法保存!");
                return;
            }

            EntityReportComment model = new EntityReportComment();
            model.RcComment = txtCommentInfo.Text;
            model.RcOpname = UserInfo.userName;
            model.RcOpcode = UserInfo.loginID;
            model.RcRepId = PatID;
            new ProxyPatEnterNew().Service.SaveReportComment(model);
            gcComment.DataSource = new ProxyPatEnterNew().Service.GetReportComment(PatID);
            txtCommentInfo.Text = string.Empty;
            txtCommentInfo.Focus();
        }

        private void btnLisKnowleage_Click(object sender, EventArgs e)
        {
            FrmLisKnowage frm = new FrmLisKnowage();
            frm.ShowDialog();
        }
    }
}
