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

namespace dcl.client.resultquery
{
    public partial class FrmTwoWaySelect : FrmCommon
    {
        public FrmTwoWaySelect()
        {
            InitializeComponent();
        }
        ProxyTwoWaySelect proxy = new ProxyTwoWaySelect();


        private void txtBarCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (lueInstrmt.valueMember == null || lueInstrmt.valueMember.Trim() == "")
                {
                    lis.client.control.MessageDialog.Show("请选择仪器！", "提示");
                    return;
                }
                if (txtBarCode.EditValue == null || txtBarCode.EditValue.ToString().Trim() == "")
                {
                    lis.client.control.MessageDialog.Show("请输入条码号！", "提示");
                    return;
                }
                string flag = string.Empty;

                if (ckClear.Checked)
                {
                    flag = "0";
                }

                if (chkSet.Checked)
                {
                    flag = "1";
                }

                if (ckClear.Checked || chkSet.Checked)
                {
                    proxy.Service.UpdateSampFlag(flag, txtBarCode.EditValue.ToString());
                }
                EntityResponse respone = proxy.Service.GetBarcodeData(txtBarCode.EditValue.ToString(), lueInstrmt.valueMember.Trim());
                Dictionary<string, object> dict = respone.GetResult() as Dictionary<string, object>;

                bsTwoWay.DataSource = dict["TwoWays"] as List<EntitySampDetailMachineCode>;

                bsSampDetail.DataSource = dict["SampDetail"] as List<EntitySampDetail>;

                this.txtBarCode.EditValue = null;
            }
        }

        private void FrmTwoWaySelect_Load(object sender, EventArgs e)
        {
            this.txtBarCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarCode_KeyDown);

            base.ShowSucessMessage = false;
            sysItem.SetToolButtonStyle(new string[] { sysItem.BtnAdd.Name,sysItem.BtnDelete.Name
            ,sysItem.BtnClose.Name});
            sysItem.AutoEnableButtons = false;
            sysItem.BtnAdd.Caption = "置为上机";
            sysItem.BtnDelete.Caption = "清除上机";
            sysItem.OnBtnAddClicked += btnSet_Click;
            sysItem.OnBtnDeleteClicked += btnClear_Click;

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (gvBar.GetFocusedRow() == null)
            {
                lis.client.control.MessageDialog.Show("请选择要清除的上机标志的项目！", "提示");
                return;
            }
            bool result = false;
            EntitySampDetail samp = bsSampDetail.Current as EntitySampDetail;
            List<EntitySampDetail> list = new List<EntitySampDetail>();
            result = proxy.Service.UpdateSampFlagByCode("0", samp.SampBarCode, samp.OrderCode);

            if (result)
            {
                samp.CommFlag = 0;
                list.Add(samp);
                lis.client.control.MessageDialog.Show("操作成功！", "提示");
            }
            bsSampDetail.DataSource = list;
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (gvBar.GetFocusedRow() == null)
            {
                lis.client.control.MessageDialog.Show("请选择要设置的上机标志的项目！", "提示");
                return;
            }

            bool result = false;
            EntitySampDetail samp = bsSampDetail.Current as EntitySampDetail;
            List<EntitySampDetail> list = new List<EntitySampDetail>();
            result = proxy.Service.UpdateSampFlagByCode("1", samp.SampBarCode, samp.OrderCode);

            if (result)
            {
                samp.CommFlag = 1;
                list.Add(samp);
                lis.client.control.MessageDialog.Show("操作成功！", "提示");
            }
            bsSampDetail.DataSource = list;
        }

        private void ckClear_CheckedChanged(object sender, EventArgs e)
        {
            if (ckClear.Checked)
            {
                chkSet.Checked = false;
            }
        }

        private void chkSet_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSet.Checked)
            {
                ckClear.Checked = false;
            }
        }
    }
}
