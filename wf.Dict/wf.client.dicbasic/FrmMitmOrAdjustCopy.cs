using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using dcl.entity;
using dcl.client.wcf;

namespace dcl.client.dicbasic
{
    public partial class FrmMitmOrAdjustCopy : FrmCommon
    {
        public FrmMitmOrAdjustCopy()
        {
            InitializeComponent();
        }

        public string TypeName { get; set; }

        public string OriItrId { get; set; }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (lue_Instrmt.valueMember == null || lue_Instrmt.valueMember.Trim() == string.Empty)
            {
                lis.client.control.MessageDialog.Show("请选择要目标仪器！");
                return;
            }
            EntityDicInstrument dtInstrmt = new EntityDicInstrument();

            dtInstrmt.ItrOrgId = OriItrId;
            dtInstrmt.ItrId = lue_Instrmt.valueMember;

            EntityRequest request = new EntityRequest();
            request.SetRequestValue(dtInstrmt);
            EntityResponse result = new EntityResponse();
            //ProxyItrResCopy proxy = new ProxyItrResCopy();
            ProxyCommonDic proxy = new ProxyCommonDic("svc.FrmMitmOrAdjustCopy");
            if (TypeName == "结果调整")
            {
                result = proxy.Service.Update(request);
            }
            else
            {
                result = proxy.Service.Other(request);
            }

            if (base.isActionSuccess)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("复制成功");
                this.DialogResult = DialogResult.Yes;

            }
            else
                lis.client.control.MessageDialog.ShowAutoCloseDialog("复制失败！");
        }

        private void FrmMitmOrAdjustCopy_Load(object sender, EventArgs e)
        {
            if (TypeName != string.Empty)
            {
                if (TypeName == "结果调整")
                {
                    this.Text = "仪器结果调整复制";
                }
            }
        }
    }
}
