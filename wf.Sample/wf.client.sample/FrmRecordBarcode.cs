using System;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.wcf;
using lis.client.control;
using System.Drawing;
using dcl.entity;

namespace dcl.client.sample
{
    public partial class FrmRecordBarcode : FrmCommon
    {
        public FrmRecordBarcode()
        {
            InitializeComponent();
            sysToolBar1.CheckPower = false;

            this.gvCname.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(gvCname_CustomDrawCell);

        }

        /// <summary>
        /// 设置单元格颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gvCname_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            object obj = this.gvCname.GetRow(e.RowHandle);
            EntitySampDetail rowview = obj as EntitySampDetail;
            if (rowview != null)
            {
                int bc_flag = rowview.SampFlag;
                if (bc_flag == 1)
                {

                    e.Appearance.ForeColor = Color.Blue;
                }
                else
                {
                    e.Appearance.ForeColor = Color.Black;

                }
            }
        }

        private EntitySampMain SampMain = null;

        private void FrmSendMessage_Load(object sender, EventArgs e)
        {
            //需要显示的按钮和顺序
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnSave.Name });
            sysToolBar1.BtnSave.Caption = "记录";
            sysToolBar1.BtnSave.Enabled = true;

            this.ActiveControl = this.txtBarcodeInput;
            this.txtBarcodeInput.Focus();
        }

        private void sysToolBar1_OnBtnSaveClicked(object sender, EventArgs e)
        {            /******peng:存储最后一次操作签名的姓名和时间*********/
            //string time = string.Empty;
            //string name = string.Empty;
            /****************************/
            if (this.SampMain == null)
            {
                MessageDialog.Show("无条码信息", "提示");
                return;
            }

            if (txtMessageContent.Text.Trim() == string.Empty)
            {
                MessageDialog.Show("请填写异常信息", "提示");
                return;
            }

            FrmCheckPassword frm = new FrmCheckPassword();
            if (frm.ShowDialog() == DialogResult.OK)//身份验证
            {

                //生成确认(签名)信息
                EntitySampOperation operationInfo = new EntitySampOperation(frm.OperatorID, frm.OperatorName);
                operationInfo.OperationStatus = "650";
                operationInfo.OperationStatusName = "记录";
                operationInfo.Remark = "记录条码：" + txtMessageContent.Text;
                operationInfo.OperationTime = IStep.GetServerTime();

                ProxySampProcessDetail proxy = new ProxySampProcessDetail();
                bool result = proxy.Service.SaveSampProcessDetail(operationInfo, SampMain);



                if (result)
                {
                    MessageDialog.Show("记录成功！");
                    ClearBarcodeInfo();
                    this.SampMain = null;
                    this.txtBarcodeInput.Focus();
                    this.ActiveControl = this.txtBarcode;
                }
                else
                {
                    MessageDialog.Show("记录失败！");
                }
            }
        }

        /// <summary>
        /// 条码输入框按下回车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBarcodeInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string barcode = this.txtBarcodeInput.Text;
                RefreshBarcodeInfo(barcode);
            }
        }

        /// <summary>
        /// 刷新条码信息
        /// </summary>
        /// <param name="barcode"></param>
        private void RefreshBarcodeInfo(string barcode)
        {
            if (!string.IsNullOrEmpty(barcode))
            {
                ProxySampMain proxy = new ProxySampMain();
                EntitySampMain samp = proxy.Service.SampMainQueryByBarId(barcode);

                if (!string.IsNullOrEmpty(samp.SampBarId))
                {
                    BindBarcodeInfo(samp);
                    SampMain = samp;
                }
                else
                {
                    MessageDialog.ShowAutoCloseDialog("无此条形码信息！");
                    SampMain = null;
                    ClearBarcodeInfo();
                }
            }
            else
            {
                ClearBarcodeInfo();
                SampMain = null;
            }
            this.txtBarcodeInput.Text = string.Empty;
            this.txtBarcodeInput.Focus();

        }

        /// <summary>
        /// 绑定条码信息到界面
        /// </summary>
        /// <param name="baseinfo"></param>
        private void BindBarcodeInfo(EntitySampMain sampMain)
        {
            this.txtBarcode.Text = sampMain.SampBarCode;
            selectDict_Sample1.SelectByID(sampMain.SampSamId);
            selectDict_Sample_Remarks1.SelectByID(sampMain.SampRemark);
            txtShowName.Text = sampMain.PidName;
            selectDict_Cuv1.SelectByID(sampMain.SampTubCode);//试管
            txtDepartment.Text = sampMain.PidDeptName;
            txtDoctor.Text = sampMain.PidDoctorName;
            txtDiagnose.Text = sampMain.PidDiag;
            this.txtBedNumber.Text = sampMain.PidBedNo;//床号
            txtPatNoID.Text = sampMain.PidInNo; //住院号

            this.gcItem.DataSource = sampMain.ListSampDetail;
            this.gcAction.DataSource = sampMain.ListSampProcessDetail;
        }

        /// <summary>
        /// 清空界面信息
        /// </summary>
        private void ClearBarcodeInfo()
        {
            BindBarcodeInfo(new EntitySampMain());
            this.txtMessageContent.Text = string.Empty;
        }


    }
}