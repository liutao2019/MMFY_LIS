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
    /// 报告解读评价
    /// </summary>
    public partial class FrmReportComment : FrmCommon
    {
        /// <summary>
        /// 报告ID
        /// </summary>
        public string RepID { get; set; }
        public FrmReportComment()
        {
            InitializeComponent();
            this.Shown += FrmReportComment_Shown;
        }

        private void FrmReportComment_Shown(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(RepID))
            {
                gcComment.DataSource= new ProxyPatEnterNew().Service.GetReportComment(RepID);
            }
        }

        private void btnComment_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtCommentInfo.Text))
            {
                MessageBox.Show("评价内容不能为空.");
                return;
            }
            if (string.IsNullOrEmpty(RepID))
            {
                MessageBox.Show("无对应报告单，无法保存!");
                return;
            }

            EntityReportComment model = new EntityReportComment();
            model.RcComment = txtCommentInfo.Text;
            model.RcOpname = UserInfo.userName;
            model.RcOpcode = UserInfo.loginID;
            model.RcRepId = RepID;
            new ProxyPatEnterNew().Service.SaveReportComment(model);
            gcComment.DataSource = new ProxyPatEnterNew().Service.GetReportComment(RepID);
        }
    }
}
