using System;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.wcf;
using lis.client.control;
using dcl.client.common;
using System.Drawing;
using DevExpress.XtraEditors;
using dcl.entity;
using System.Collections.Generic;
using dcl.client.cache;

namespace dcl.client.sample
{
    public partial class FrmReturnBarcodeV2 : FrmCommon, IStepable
    {
        public FrmReturnBarcodeV2()
        {
            InitializeComponent();
            sysToolBar1.CheckPower = false;

            this.gvCname.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(gvCname_CustomDrawCell);
            this.Shown += new EventHandler(FrmReturnBarcodeV2_Shown);

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
        public FrmReturnBarcodeV2(string barocde)
            : this()
        {
            this.txtBarcodeInput.Text = barocde;

        }
        //条码签收后不允许在采集确认、收取确认界面回退标本，
        //只能在签收界面回退；采集界面只能回退已采集状态的条码
        //，收取界面只能回退收取状态的条码
        private StepType stepType = StepType.Select;
        public FrmReturnBarcodeV2(StepType aStepType)
            : this()
        {
            stepType = aStepType;
        }

        private EntitySampMain SampMain = null;

        public IStep Step
        {
            get { return StepFactory.CreateStep(stepType); }
            set { Step = StepFactory.CreateStep(stepType); }
        }

        public StepType StepType
        {
            get { return stepType; }
            set
            {
                stepType = value;
                Step = StepFactory.CreateStep(value);

            }
        }

        void FrmReturnBarcodeV2_Shown(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtBarcodeInput.Text))
            {
                string barcode = this.txtBarcodeInput.Text;
                RefreshBarcodeInfo(barcode);
            }
        }

        private void FrmSendMessage_Load(object sender, EventArgs e)
        {
            //需要显示的按钮和顺序
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnSave.Name });
            sysToolBar1.BtnSave.Caption = "回退";
            sysToolBar1.BtnSave.Enabled = true;

            if (!DesignMode)
            {
                List<EntityDicSampReturn> listSampReturn = CacheClient.GetCacheCopy<EntityDicSampReturn>();
                gcBCReturn.DataSource = listSampReturn;
            }

            if (UserInfo.GetSysConfigValue("Barcode_Retroversion") == "是")
                txtMessageContent.Properties.ReadOnly = true;

            this.ActiveControl = this.txtBarcodeInput;
            this.txtBarcodeInput.Focus();
        }

        private void sysToolBar1_OnBtnSaveClicked(object sender, EventArgs e)
        {
            /****************************/
            if (this.SampMain == null)
            {
                MessageDialog.Show("无条码信息", "提示");
                return;
            }
            EntitySampProcessDetail processDetail = new EntitySampProcessDetail();
            if (this.SampMain.SampStatusId == EnumBarcodeOperationCode.SampleReturn.ToString())
            {
                /********增加操作人和时间******************************/
                processDetail = new ProxySampProcessDetail().Service.GetLastSampProcessDetail(SampMain.SampBarId);
                MessageDialog.Show(string.Format("无法进行此操作，原因：当前条码已被回退\n操作人：" + processDetail.ProcUsername + ",时间：" + processDetail.ProcDate + ""), "提示");
                /***************************************************************/

                return;
            }

            List<EntitySampDetail> listSampDetail = new ProxySampDetail().Service.GetSampDetail(SampMain.SampBarId);

            //条码回退前必须删除病人资料
            if (ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_ReturnBarcodeMustBeletePatInfo") != "否")
            {
                if (SampMain.ListSampDetail.Count > 0 && SampMain.ListSampDetail.FindIndex(w => w.SampFlag == 1) >= 0)
                {
                    MessageDialog.Show(string.Format("无法进行此操作，原因：条码已录入，请先删除病人信息再回退\n操作人：" + processDetail.ProcUsername + ",时间：" + processDetail.ProcDate + ""), "提示");
                    return;
                }
            }

            if (stepType != StepType.Select)
            {
                if (SampMain.SampStatusId != EnumBarcodeOperationCode.SampleCollect.ToString() &&
                    stepType == StepType.Sampling)
                {
                    MessageDialog.Show(string.Format("采集界面只能回退[标本采集]状态的条码,该条码当前状态为[{0}]", EnumBarcodeOperationCode.GetNameByCode(SampMain.SampStatusId)), "提示");
                    return;
                }

                if (SampMain.SampStatusId != EnumBarcodeOperationCode.SampleSend.ToString() &&
                    stepType == StepType.Send)
                {
                    MessageDialog.Show(string.Format("收取界面只能回退[标本收取]状态的条码,该条码当前状态为[{0}]", EnumBarcodeOperationCode.GetNameByCode(SampMain.SampStatusId)), "提示");
                    return;
                }

                if (SampMain.SampStatusId != EnumBarcodeOperationCode.SampleReach.ToString() &&
                 stepType == StepType.Reach)
                {
                    MessageDialog.Show(string.Format("送达界面只能回退[标本送达]状态的条码,该条码当前状态为[{0}]", EnumBarcodeOperationCode.GetNameByCode(SampMain.SampStatusId)), "提示");
                    return;
                }

                if (SampMain.SampStatusId != EnumBarcodeOperationCode.SampleReceive.ToString() && SampMain.SampStatusId != EnumBarcodeOperationCode.DeletePatient.ToString() &&
                stepType == StepType.Confirm)
                {
                    MessageDialog.Show(string.Format("签收界面只能回退[标本签收]或者[删除病人资料]状态的条码,该条码当前状态为[{0}]", EnumBarcodeOperationCode.GetNameByCode(SampMain.SampStatusId)), "提示");
                    return;
                }
                if (SampMain.SampStatusId != EnumBarcodeOperationCode.SampleSecondSend.ToString() &&
                stepType == StepType.SecondSend)
                {
                    MessageDialog.Show(string.Format("二次送检界面只能回退[二次送检]状态的条码,该条码当前状态为[{0}]", EnumBarcodeOperationCode.GetNameByCode(SampMain.SampStatusId)), "提示");
                    return;
                }
            }


            if (txtMessageContent.Text.Trim() == string.Empty || string.IsNullOrEmpty(messageType))
            {
                MessageDialog.Show("请选择回退原因", "提示");
                return;
            }

            List<EntitySampMain> listSampMain = new List<EntitySampMain>();

            ReturnStep returnStep = new ReturnStep();

            DateTime dtServer = IStep.GetServerTime();

            returnStep.BaseSampMain = SampMain;

            listSampMain.Add(SampMain);
            Step.Audit.ShouldAuditWhenPrint = false;
            FrmHISCheckPassword frm = new FrmHISCheckPassword(Step.Audit);

            //系统配置：[处理回退条码]身份验证方式
            if (ConfigHelper.GetSysConfigValueWithoutLogin("CheckTypeOpt_DisposeReturnBC") == "佛山南海")
            {
                frm.Text = "佛山南海";
            }
            if (frm.ShowDialog() == DialogResult.OK)//身份验证
            {
                //生成确认(签名)信息
                EntitySampOperation sign = new EntitySampOperation { OperationID = frm.OperatorID, OperationName = frm.OperatorName, OperationWorkId = frm.OperatorSftId } ;

                //生成回退信息
                EntitySampReturn returnMessage = new EntitySampReturn();
                returnMessage.ReturnSampSn = SampMain.SampSn;
                returnMessage.SampBarId = SampMain.SampBarId;
                returnMessage.SampBarCode = SampMain.SampBarCode;
                returnMessage.ReturnDate = dtServer;
                returnMessage.MessageType = Convert.ToInt32(messageType);

                sign.Remark = this.txtMessageContent.Text;
                returnMessage.ReturnReasons = this.txtMessageContent.Text;

                returnMessage.ReturnReceiver = txtReceiver.displayMember;
                returnMessage.ReturnUserId = frm.OperatorID;
                if (!string.IsNullOrEmpty(frm.OperatorName))
                {
                    returnMessage.ReturnUserName = frm.OperatorName;
                }
                else
                {
                    List<EntitySysUser> listUser = (CacheClient.GetCache("EntitySysUser") as List<EntitySysUser>).FindAll(w => w.UserLoginid == frm.OperatorID);
                    if (listUser.Count > 0)
                    {
                        returnMessage.ReturnUserName = listUser[0].UserName;
                    }
                }

                returnMessage.ReturnProcFlag = false;
                returnMessage.DelFlag = false;
                returnMessage.ReturnDeptCode = SampMain.PidDeptCode;
                returnMessage.ReturnDeptName = SampMain.PidDeptName;
                returnMessage.PidSrcId = SampMain.PidSrcId;

                /************************************************/

                sign.Remark += "回退项目：";

                if (listSampDetail != null && listSampDetail.Count > 0)
                {
                    foreach (EntitySampDetail item in listSampDetail)
                    {
                        if (item.SampFlag != 1)
                        {
                            sign.Remark += item.ComName + ",";
                        }
                    }
                    sign.Remark.Remove(sign.Remark.Length - 2, 1);
                }

                if (!string.IsNullOrEmpty(returnMessage.ReturnReceiver))
                {
                    sign.Remark += string.Format("接受者：{0}", returnMessage.ReturnReceiver);
                }

                /************************************************/

                sign.OperationTime = dtServer;
                //bc.ReturnFlag = 1; //暂不更新回退标志

                //插入回退消息
                bool result = new ProxySampReturn().Service.SaveSampReturn(returnMessage);

                if (result && returnStep.ComfirmAll(sign, listSampMain))//回退操作，更新条码信息，然后异步调用回退接口
                {
                    string op_code = "";
                    string op_name = "";
                    if (frm != null)
                    {
                        op_code = frm.OperatorID;
                        op_name = frm.OperatorName;
                    }

                    MessageDialog.ShowAutoCloseDialog("回退成功！");
                    ClearBarcodeInfo();
                    this.SampMain = null;
                    txtReceiver.displayMember = string.Empty;
                    txtReceiver.valueMember = string.Empty;
                    this.txtBarcodeInput.Focus();
                    this.ActiveControl = this.txtBarcodeInput;
                }
                else
                {
                    MessageDialog.Show("回退失败！");
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
                //常规检验回退标本有条码号时隐藏回退界面关闭按钮
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
        /// <param name="sampMain"></param>
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

            foreach (var cl in panelControl3.Controls)
            {
                if (cl is DevExpress.XtraEditors.CheckEdit)
                    (cl as CheckEdit).Checked = false;
            }
        }


        private void repositoryItemCheckEdit1_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit ck = sender as DevExpress.XtraEditors.CheckEdit;
            EntityDicSampReturn drMes = (EntityDicSampReturn)gvBCReturn.GetFocusedRow();
            if (ck.Checked)
            {
                txtMessageContent.Text += drMes.ReturnContent + "\r\n";
            }
            else
            {
                txtMessageContent.Text = txtMessageContent.Text.Replace(drMes.ReturnContent + "\r\n", string.Empty);
            }
        }
        string messageType = string.Empty;
        private void chkChange_CheckedChanged(object sender, EventArgs e)
        {
            if (ckOther.Checked)
            {
                txtMessageContent.Text = "";
                messageType = "14";
                gcBCReturn.Visible = true;
            }
            else
            {
                gcBCReturn.Visible = false;
                if (ckFirst.Checked)
                {
                    txtMessageContent.Text = ckFirst.Text;
                    messageType = "1";
                }
                else if (ckSecond.Checked)
                {
                    txtMessageContent.Text = ckSecond.Text;
                    messageType = "2";
                }
                else if (ckThree.Checked)
                {
                    txtMessageContent.Text = ckThree.Text;
                    messageType = "3";
                }
                else if (ckFour.Checked)
                {
                    txtMessageContent.Text = ckFour.Text;
                    messageType = "4";
                }
                else if (ckFive.Checked)
                {
                    txtMessageContent.Text = ckFive.Text;
                    messageType = "5";
                }
                else if (ckSix.Checked)
                {
                    txtMessageContent.Text = ckSix.Text;
                    messageType = "6";
                }
                else if (ckSeven.Checked)
                {
                    txtMessageContent.Text = ckSeven.Text;
                    messageType = "7";
                }
                else if (ckEight.Checked)
                {
                    txtMessageContent.Text = ckEight.Text;
                    messageType = "8";
                }
                else if (ckNine.Checked)
                {
                    txtMessageContent.Text = ckNine.Text;
                    messageType = "9";
                }
                else if (ckTen.Checked)
                {
                    txtMessageContent.Text = ckTen.Text;

                    messageType = "10";
                }
                else if (ckEleven.Checked)
                {
                    txtMessageContent.Text = ckEleven.Text;
                    messageType = "11";
                }
                else if (ckTwelve.Checked)
                {
                    txtMessageContent.Text = ckTwelve.Text;
                    messageType = "12";
                }
                else if (ckThirteen.Checked)
                {
                    txtMessageContent.Text = ckThirteen.Text;
                    messageType = "13";
                }
                else if (ckFifteen.Checked)
                {
                    txtMessageContent.Text = ckFifteen.Text;
                    messageType = "15";
                }
            }

        }
    }
}