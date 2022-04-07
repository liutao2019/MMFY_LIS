using System;
using System.Collections.Generic;
using System.Windows.Forms;
using dcl.client.wcf;
using dcl.entity;

namespace dcl.client.sample
{
    public partial class FrmOuterCourtRegister : DevExpress.XtraEditors.XtraForm
    {
        ProxySampMain proxy = new ProxySampMain();

        public FrmOuterCourtRegister()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.FrmOuterCourtRegister_Load);
            this.sysToolBar1.OnCloseClicked += new System.EventHandler(this.SysToolBar1_OnCloseClicked);
            this.sysToolBar1.OnBtnDeleteClicked += new System.EventHandler(this.sysToolBar1_OnBtnDeleteClicked);
            this.txtBarcode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBarcode_KeyPress);
        }

        private void FrmOuterCourtRegister_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[] {
                sysToolBar1.BtnDelete.Name,
                sysToolBar1.BtnClose.Name,
            });
        }

        private void SysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 删除登记资料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            patientControlForMed1.DeleteBarcode(string.Empty, string.Empty);
        }


        List<EntitySampMain> listSource = new List<EntitySampMain>();
        private void txtBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar != (char)Keys.Enter)
                    return;
                DownLoadFromInterface(txtBarcode.Text.Trim());
                EntitySampMain sampMain = proxy.Service.SampMainQueryByBarId(txtBarcode.Text.Trim());
                if (listSource.FindIndex(w => w.SampBarCode == txtBarcode.Text.Trim()) > -1)
                {
                    lis.client.control.MessageDialog.Show(string.Format("该条码 {0} 已经在当前列表中!", txtBarcode.Text.Trim()), "条码重复");
                    ClearAndFocusBarcode();
                    return;
                }
                if (!string.IsNullOrEmpty(sampMain.SampBarId))
                    listSource.Add(sampMain);
                patientControlForMed1.BindingSampMain(listSource, true);
                patientControlForMed1.RefreshCurrentBarcode();
                patientControlForMed1.RefreshCurrentBarcodeInfo();
                ClearAndFocusBarcode();
                //bool result = patientControlForMed1.AddBarcode(txtBarcode.Text.Trim());
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
        }
        public void ClearAndFocusBarcode()
        {
            txtBarcode.Text = "";
            txtBarcode.Focus();
            this.ActiveControl = txtBarcode;
        }
        /// <summary>
        /// 下载外送条码
        /// </summary>
        /// <param name="barcode"></param>
        private void DownLoadFromInterface(string barcode)
        {
            EntityInterfaceExtParameter downLoadInfo = new EntityInterfaceExtParameter();

            downLoadInfo.DownloadType = InterfaceType.OutsideDownload;
            downLoadInfo.PatientID = barcode;
            try
            {
                ProxySampMainDownload download = new ProxySampMainDownload();
                download.Service.DownloadOutsideBarcode(downLoadInfo);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
        }
    }
}