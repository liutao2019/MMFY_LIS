using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.interfaces;

using dcl.client.sample;
using lis.client.control;
using dcl.client.wcf;
using dcl.entity;

namespace dcl.client.result
{
    public partial class FrmAdditionalBarcode : FrmCommon
    {
        public FrmAdditionalBarcode()
        {
            InitializeComponent();
        }

        public List<EntityPidReportDetail> PatientMi { get; set; }

        /// <summary>
        /// 所选的目标病人id
        /// </summary>
        public string  DestPatID { get; set; }

        private void txtBarCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtBarCode.Text.Trim() != string.Empty)
                {
                    EntityPidReportMain PatInfo = null;
                    EntityInterfaceExtParameter parameter = new EntityInterfaceExtParameter();
                    parameter.PatientID = txtBarCode.Text.Trim();
                    parameter.DownloadType = InterfaceType.BarcodePatient;

                    ProxyPidReportMainInterface proxy = new ProxyPidReportMainInterface();

                    PatInfo = proxy.Service.GetPatientFromInterface(parameter);

                    if (PatInfo == null)
                    {
                        MessageDialog.Show(string.Format("条码信息不存在"), "提示");
                        this.txtBarCode.SelectAll();
                        this.txtBarCode.Focus();
                        return;
                    }
                    if (!string.IsNullOrEmpty(DestPatID))
                    {
                        if (PatInfo.PidInNo != DestPatID)
                        {
                            MessageDialog.Show(string.Format("与选中的病人信息不符！\r\n当前条码对应的病人信息：姓名:{0},病人ID:{1}", PatInfo.PidName, PatInfo.PidInNo), "提示");
                            this.txtBarCode.SelectAll();
                            this.txtBarCode.Focus();
                            return;
                        }
                    }
                    if (PatInfo.BcStatus == "9")
                    {
                        string name = string.Empty;
                        string time = string.Empty;
                        string remark = string.Empty;
                        //PatientControl.GetLastBarcodeAction(PatInfo.BarCode, out name, out time, out remark);
                        MessageDialog.Show(string.Format("此条码已回退，不允许录入！\r\n{2}\r\n 操作者：{0},时间：{1}", name, time, remark), "提示");
                        return;
                    }
                    if (
                         PatInfo.BcStatus == "0"
                         || PatInfo.BcStatus == "1"
                         || PatInfo.BcStatus == "2"
                         || PatInfo.BcStatus == "3"
                         || PatInfo.BcStatus == "4"
                         || PatInfo.BcStatus == "8"
                         || PatInfo.BcStatus == "9"
                         || PatInfo.BcStatus == "500"
                         || PatInfo.BcStatus == "510")//条码未签收
                    {
                        if (PatInfo.PidSrcId == "108" && UserInfo.GetSysConfigValue("Barcode_ZYShouldReceiveConfirm") == "是")//住院
                        {
                            MessageDialog.Show("当前条码未签收！\r\n当前设置为：[住院条码]必须进行[签收]确认", "提示");
                            return;
                        }
                        if (PatInfo.PidSrcId == "109" && UserInfo.GetSysConfigValue("Barcode_TJShouldReceiveConfirm") == "是")//体检
                        {
                            MessageDialog.Show("当前条码未签收！\r\n当前设置为：[体检条码]必须进行[签收]确认", "提示");
                            return;
                        }
                    }
                    PatientMi = PatInfo.ListPidReportDetail;

                    this.DialogResult = DialogResult.OK;
                }
                else
                    this.DialogResult = DialogResult.No;
            }
        }
    }
}
