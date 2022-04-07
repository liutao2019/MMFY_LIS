using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using dcl.client.common;
using dcl.client.frame;
using dcl.common;
using dcl.client.wcf;
using dcl.entity;
using DevExpress.XtraEditors;
using dcl.client.cache;

namespace dcl.client.sample
{
    public partial class BCManual : XtraUserControl
    {
        #region 全局变量
        private BCPrintControl m_objBCPrintcotrl = null;
        private bool isIDTypeVisible = true;

        /// <summary>
        /// 是否保存UPID唯一号(目前滨海使用)
        /// </summary>
        private bool isSaveBCPatUPID = false;

        private string appendID = string.Empty;
        #endregion

        /// <summary>
        /// 是否独立界面
        /// </summary>
        public bool IsAlone { get; set; }

        /// <summary>
        /// 科室代码
        /// </summary>
        public string DeptCode
        {
            get;
            set;
        }


        public BCManual()
        {
            InitializeComponent();
        }

        DataTable combine = new DataTable();
        List<EntityDicCombine> listComb = new List<EntityDicCombine>();
        FrmCombineManager f;
        private void BCManual_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            CreateControl();
            this.patientControl1.Step.Printer = PrintFactory.Create(PrintType.Manual);

            //不显示明细信息·
            patientControl1.SetInfoDisVisable();
            //使用的条码类型：自动条码、预置条码
            string barcode_type = ConfigHelper.GetSysConfigValueWithoutLogin("7");
            if (barcode_type == "预置条码")
            {
                chkPreBarcode.Visible = true;
                panelControl2.Visible = true;
                txtPrePlaceBarcode.Visible = true;
                lblPrePlaceBarcode.Visible = true;
            }
            string strUndoButrn = string.Empty;
            string[] strBtnName;
            string[] strBtntool;

            strBtnName = new string[] {
                sysToolBar1.BtnBCPrint.Name,
                sysToolBar1.BtnPrintList.Name,
                sysToolBar1.BtnReset.Name,
                sysToolBar1.BtnSave.Name,
                sysToolBar1.BtnDelete.Name,
                sysToolBar1.BtnPrintSet.Name,
                strUndoButrn,
                sysToolBar1.BtnSearch.Name,
                sysToolBar1.BtnClose.Name
                };
            sysToolBar1.OnBtnSearchClicked += SysToolBar1_OnBtnSearchClicked;
            strBtntool = new string[] { "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F11", "F12" };
            sysToolBar1.BtnReset.Caption = "清空";

            UserInfo.SkipPower = true;
            sysToolBar1.CheckPower = false;
            sysToolBar1.SetToolButtonStyle(strBtnName, strBtntool);
            sysToolBar1.OnBtnPrintListClicked += new EventHandler(sysToolBar1_OnBtnPrintListClicked);

            sysToolBar1.OnBtnBCPrintClicked += new System.EventHandler(this.sysToolBar1_OnBtnBCPrintClicked);
            sysToolBar1.OnBtnSaveClicked += new System.EventHandler(this.sysToolBar1_OnBtnSaveClicked);
            sysToolBar1.OnBtnDeleteClicked += new System.EventHandler(this.sysToolBar1_OnBtnDeleteClicked);
            sysToolBar1.BtnResetClick += new System.EventHandler(this.sysToolBar1_BtnResetClick);
            sysToolBar1.BtnPrintSetClick += new System.EventHandler(this.sysToolBar1_BtnPrintSetClick);
            sysToolBar1.OnCloseClicked += new System.EventHandler(this.sysToolBar1_OnCloseClicked);



            dateTimeControl1.DateTime = ServerDateTime.GetServerDateTime();
            txtBirthday.DateTime = ServerDateTime.GetServerDateTime();
            combine.Columns.Add("com_id");
            combine.Columns.Add("com_name");
            combine.Columns.Add("urgent", typeof(bool));

            if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("BCManualMode") == "外院")
            {
                selectDict_Origin1.SelectByID("110");
            }
            else if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("BCManualMode") == "门诊")
            {
                selectDict_Origin1.SelectByID("107");
                txtPatIdType.SelectByID("107");
            }
            else if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("BCManualMode") == "住院")
            {
                selectDict_Origin1.SelectByID("108");
                txtPatIdType.SelectByID("106");
            }
            else if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("BCManualMode") == "体检")
            {
                selectDict_Origin1.SelectByID("109");
                txtPatIdType.SelectByID("110");
            }
            else
            {
                selectDict_Origin1.SelectByID("107");
                txtPatIdType.SelectByID("107");
            }

            this.ActiveControl = txtPatientID;

            appendID = UserInfo.GetSysConfigValue("BC_CustomFilterNoType");

            if (ConfigHelper.GetSysConfigValueWithoutLogin("Lab_ShowNewSid") == "是")
            {
                patientControl1.SetTjCompanyVisable();
            }

            if (UserInfo.GetSysConfigValue("Barcode_SaveNotClearInfo") == "是")
            {
                chkSaveNotClearInfo.Visible = true;
                chkSaveNotClearInfo.Checked = true;
            }

        }

        private void SysToolBar1_OnBtnSearchClicked(object sender, EventArgs e)
        {
            FrmManualQuery frm = new FrmManualQuery();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                patientControl1.LoadData(frm.SampQc);
                if (!patientControl1.HasData())
                {
                    lis.client.control.MessageDialog.Show("无返回数据!");
                    return;
                }
            }
        }

        private new bool DesignMode
        {
            get { return System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToLower() == "devenv"; }
        }

        private void txtCombineEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Plus)
            {
                f.Visible = true;
            }
        }

        void f_RefreshCombineTextDemanded(object sender, EventArgs args)
        {
            if (f.dtCombine == null)
                return;

            txtCombineEdit.Text = "";

            foreach (EntityDicCombine row in f.dtCombine)
            {
                txtCombineEdit.Text += "+" + row.ComName.ToString();
            }

            txtCombineEdit.Text = txtCombineEdit.Text.TrimStart('+');
        }

        private void sysToolBar1_OnBtnSaveClicked(object sender, EventArgs e)
        {
            if (CheckInput())
                return;

            if (selectDict_Origin1.valueMember == null || selectDict_Origin1.valueMember.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("请输入来源！", "提示");
                selectDict_Origin1.Focus();
                return;
            }

            if (f == null || f.dtCombine.Count == 0)
            {
                lis.client.control.MessageDialog.Show("请输入检查项目！", "提示");
                txtCombineEdit.Focus();
                return;
            }

            //手工条码病人编号是否必须录入
            if (UserInfo.GetSysConfigValue("bar_PatientID_MustInput") == "是")
            {
                if (string.IsNullOrEmpty(this.txtPatientID.Text.Trim()))
                {
                    lis.client.control.MessageDialog.Show("请输入" + this.lblPatInNo.Text + "！", "提示");
                    txtPatientID.Focus();
                    return;
                }
            }

            string prevPatIdType = null;//ID类型
            prevPatIdType = this.txtPatIdType.valueMember;
            if (isIDTypeVisible && string.IsNullOrEmpty(prevPatIdType))
            {
                lis.client.control.MessageDialog.Show("请输入ID类型！", "提示");
                txtPatIdType.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtPatientName.Text.Trim()))
            {
                lis.client.control.MessageDialog.Show("请输入姓名！", "提示");
                txtPatientName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(cbSex.Text))
            {
                lis.client.control.MessageDialog.Show("请选择性别！", "提示");
                cbSex.Focus();
                return;
            }
            //手工条码年龄是否可以为空
            if (UserInfo.GetSysConfigValue("bar_AgeValue_MustInput") != "是")
            {
                if (string.IsNullOrEmpty(txtAge.AgeValueText))
                {
                    lis.client.control.MessageDialog.Show("请输入年龄！", "提示");
                    txtAge.Focus();
                    return;
                }
            }

            if (UserInfo.GetSysConfigValue("bar_Doctor_MustInput") == "是")
            {
                if (txtDoctor.selectRow == null)
                {
                    lis.client.control.MessageDialog.Show("请选择开单医生！", "提示");
                    txtDoctor.Focus();
                    return;
                }
            }

            if (UserInfo.GetSysConfigValue("bar_Dept_MustInput") == "是")
            {
                if (txtDeptId.valueMember == null || txtDeptId.valueMember.Trim() == string.Empty)
                {
                    lis.client.control.MessageDialog.Show("请选择科室！", "提示");
                    txtDeptId.Focus();
                    return;
                }

            }

            //如果病人编号为空则自动生成十位数
            if (string.IsNullOrEmpty(this.txtPatientID.Text.Trim()))
            {
                //生成九位随机数
                long lngRandom = (long)new Random(Environment.TickCount).Next(200000000, 999999999);
                //九位数再乘以随机一位数为十位数
                this.txtPatientID.Text = (lngRandom).ToString() + ((long)new Random(Environment.TickCount).Next(2, 9)).ToString();
            }

            EntitySampMain sampMain = new EntitySampMain();

            sampMain.PidName = txtPatientName.Text;
            sampMain.SampBarId = txtPrePlaceBarcode.Text;
            sampMain.SampBarCode = txtPrePlaceBarcode.Text;
            sampMain.PidSex = cbSex.Text;
            sampMain.PidAge = txtAge.AgeValueText;
            sampMain.PidAddress = txtAddress.Text;
            sampMain.PidTel = txtTel.Text;
            sampMain.PidExamNo = txtEmpID.Text;
            sampMain.PidDeptName = txtDeptId.displayMember;
            sampMain.PidDeptCode = txtDeptId.valueMember;
            sampMain.PidDoctorName = txtDoctor.displayMember;
            sampMain.PidDoctorCode = txtDoctor.selectRow != null ? txtDoctor.selectRow.DoctorCode : string.Empty;
            sampMain.SampComName = txtCombineEdit.Text;
            sampMain.SampStatusId = ((int)EnumBarcodeOperationCode.BarcodeGenerate).ToString();
            sampMain.SampStatusName = "生成条码";
            sampMain.SampDate = dateTimeControl1.DateTime;
            sampMain.SampBarId = chkPreBarcode.Checked ? txtPrePlaceBarcode.Text : string.Empty;
            sampMain.SampBarCode = chkPreBarcode.Checked ? txtPrePlaceBarcode.Text : string.Empty;
            sampMain.PidDiag = (string.IsNullOrEmpty(selectDict_diagnos1.displayMember) ? selectDict_diagnos1.popupContainerEdit1.Text : selectDict_diagnos1.displayMember);
            sampMain.PidIdtId = prevPatIdType;//ID类型
            sampMain.SampInfo = "122";//标识手工条码
            sampMain.PidUniqueId = isSaveBCPatUPID ? txtPatientID.Text : null;//UPID唯一号;目前滨海使用
            sampMain.PidBirthday = txtBirthday.DateTime;
            sampMain.PidSrcId = selectDict_Origin1.valueMember;
            sampMain.PidSrcName = selectDict_Origin1.displayMember;
            sampMain.PidInNo = txtPatientID.Text;
            sampMain.SampSamId = selectDict_Sample1.valueMember;
            sampMain.SampSamName = selectDict_Sample1.displayMember;
            sampMain.PidBedNo = this.txtBedNumber.Text;
            if (chkPreBarcode.Checked)
            {
                sampMain.SampBarType = 1;
            }
            else
            {
                sampMain.SampBarType = 0;
            }
            sampMain.SampOccDate = dateTimeControl1.DateTime;
            sampMain.PidInsuId = txtPatFeeType.valueMember;


            //生成bc_cname表
            if (f.dtCombine != null)
            {
                foreach (EntityDicCombine row in f.dtCombine)
                {
                    EntitySampDetail sampDetail = new EntitySampDetail();

                    sampDetail.ComId = row.ComId.ToString();
                    sampDetail.ComName = row.ComName.ToString();
                    sampDetail.OrderName = row.ComName.ToString();
                    sampDetail.SampType = txtType.displayMember;
                    sampDetail.OrderDate = dateTimeControl1.DateTime;
                    sampDetail.OrderOccDate = dateTimeControl1.DateTime;
                    sampMain.ListSampDetail.Add(sampDetail);
                }
            }

            EntitySampProcessDetail sampProcess = new EntitySampProcessDetail();

            sampProcess.ProcStatus = sampMain.SampStatusId;
            sampProcess.ProcStatusName = sampMain.SampStatusName;
            sampProcess.ProcTimes = 0;
            sampProcess.ProcDate = sampMain.SampDate;
            sampProcess.ProcUsercode = UserInfo.loginID;
            sampProcess.ProcUsername = UserInfo.userName;

            sampMain.ListSampProcessDetail.Add(sampProcess);

            ProxySampMain proxy = new ProxySampMain();
            List<string> listBarCode = proxy.Service.ManualCreateSampMain(sampMain);

            if (listBarCode == null || listBarCode.Count == 0)
                lis.client.control.MessageDialog.Show("保存失败.");
            else
            {
                foreach (string strBarCode in listBarCode)
                {
                    patientControl1.AddBarcode(strBarCode);

                    //传手工新增的条码号到传过来的条码打印界面
                    if (m_objBCPrintcotrl != null)
                    {
                        //如果是门认界面才转移
                        if (m_objBCPrintcotrl.Printer is OutPaitent)
                        {
                            //系统配置参数打开才转移
                            if (this.checkBox_ToOutPatientPrint.Checked && this.checkBox_PrintReturnBarcode.Checked)
                            {
                                //手工条码录入门诊来源才转移
                                if (this.selectDict_Origin1.valueMember == "107")
                                {
                                    m_objBCPrintcotrl.patientControl.AddBarcode(strBarCode);
                                    m_objBCPrintcotrl.patientControl.Selection.SelectAll();
                                    sysToolBar1_BtnResetClick(null, null);
                                }


                            }
                        }
                    }
                }
                patientControl1.SelectWhenNotPrint = true;
                patientControl1.SelectNotPrintForOutPatients();
                patientControl1.NeedPrint = false;

                Clear();
                lis.client.control.MessageDialog.ShowAutoCloseDialog("保存成功.");
                this.txtPatientID.Focus();
                patientControl1.MainGridView.MoveLast();
            }
        }

        private bool CheckInput()
        {
            return false;
        }

        private void sysToolBar1_OnBtnPrintListClicked(object sender, EventArgs e)
        {
            patientControl1.PrintList();
        }

        private void sysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            if (this.ParentForm != null)
                this.ParentForm.Close();
        }

        /// <summary>
        /// 打印条码事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnBCPrintClicked(object sender, EventArgs e)
        {
            //传入病人来源ID区分是否打印条码回执
            patientControl1.m_strOriId = this.selectDict_Origin1.valueMember;
            patientControl1.m_blnCheckManualReturn = this.checkBox_PrintReturnBarcode.Checked;
            patientControl1.IsYG = IsYG;
            //打印条码事件
            patientControl1.PrintBarcode(new Manual());
            // lis.client.control.MessageDialog.Show("操作成功！");
        }

        private void sysToolBar1_BtnResetClick(object sender, EventArgs e)
        {
            patientControl1.Reset();
            Clear();
        }

        private void Clear()
        {
            //如果钩选记忆功能，则不清除
            if (chkSaveNotClearInfo.Checked)
            {
            }
            else
            {
                if (f.dtCombine != null)
                    f.dtCombine.Clear();
                txtCombineEdit.Text = "";
            }

            if (!chkSaveNotClearInfo.Checked)
            {
                txtBedNumber.Text = "";
            }

            txtAge.AgeValueText = txtPrePlaceBarcode.Text
                 = selectDict_diagnos1.displayMember =
                 txtPatientName.Text = txtAddress.Text =
            txtTel.Text = txtEmpID.Text = "";
            selectDict_Sample1.displayMember = "";
            selectDict_Sample1.valueMember = "";
            txtType.displayMember = txtType.valueMember = "";

            //如果钩选记忆功能，则不清除
            if (!chkSaveNotClearInfo.Checked)
            {
                txtDoctor.valueMember = string.Empty;
                txtDoctor.displayMember = string.Empty;
                txtDoctor.selectRow = null;
            }


            //如果钩选记忆功能，则不清除
            if (chkSaveNotClearInfo.Checked)
            {

            }
            else
            {
                txtDeptId.displayMember = string.Empty;
                txtDeptId.valueMember = string.Empty;
            }
            cbSex.SelectedIndex = 0;

            if (selectDict_Origin1.valueMember != null && selectDict_Origin1.valueMember.ToString() == "110")
            {
                txtPatientID.Text = UserInfo.GetSysConfigValue("Barcode_ManualPatientDefaultPrefix");

                txtPatientID.SelectionStart = txtPatientID.Text.Length + 1;
                txtPatientID.ScrollToCaret();
            }
            else if (!chkSaveNotClearInfo.Checked)
            {
                txtPatientID.Text = string.Empty;
            }
        }

        public void setControlValue(EntitySampMain drValue)
        {
            Clear();
            txtPatientName.EditValue = drValue.PidName;
            cbSex.EditValue = drValue.PidSex;
            txtAge.AgeValueText = drValue.PidAge;
            txtPatientID.EditValue = drValue.PidInNo;
            txtDeptId.SelectByID(drValue.PidDeptCode);
            txtDoctor.SelectByDispaly(drValue.PidDoctorName);
            selectDict_Origin1.SelectByID("107");
            dateTimeControl1.EditValue = drValue.SampOccDate;
            selectDict_diagnos1.displayMember = drValue.PidDiag;
            selectDict_Sample1.SelectByID(drValue.SampSamId);
            txtCombineEdit.Focus();
        }

        private new void CreateControl()
        {
            string ctype = string.Empty;
            if (this.txtType.valueMember != null)
            {
                ctype = this.txtType.valueMember;
            }

            if (f == null)
            {
                f = new FrmCombineManager(listComb, ctype, this.txtType.displayMember);
                f.IsFilter = true;
                f.ShowUrgentSelect = true;
                f.RefreshCombineTextDemanded += new FrmCombineManager.ResreshCombineTextDemandedEventhandler(f_RefreshCombineTextDemanded);
                f.SaveBarCode += new FrmCombineManager.ResreshCombineTextDemandedEventhandler(f_SaveBarCode);
            }

            f.StrFilter = txtCombineEdit.Text;
            txtCombineEdit.Text = string.Empty;
            f.Visible = false;
        }

        void f_SaveBarCode(object sender, EventArgs args)
        {
            f.Visible = false;
            sysToolBar1_OnBtnSaveClicked(sender, null);
        }

        private void txtCombineEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                f.Activate();
                f.Focus();
                if (!f.Visible)
                {
                    f.Visible = true;
                    f.StrFilter = txtCombineEdit.Text;
                }
            }
        }

        private void sysToolBar1_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            patientControl1.DeleteBarcode(string.Empty, string.Empty);
        }

        private void txtPatientID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtPatientID.Text != null && txtPatientID.Text.Trim() != string.Empty)
                {
                    ProxySampMain proxy = new ProxySampMain();
                    List<EntitySampMain> listSampMian = proxy.Service.GetPatientsInfoByBcInNo(txtPatientID.Text);

                    if (listSampMian.Count > 0)
                    {
                        txtPatientName.Text = listSampMian[0].PidName;
                        cbSex.Text = listSampMian[0].PidSex;
                        txtAge.AgeValueText = listSampMian[0].PidAge;
                        txtAge_Leave(null, null);

                        if (!string.IsNullOrEmpty(listSampMian[0].PidSrcId))
                        {
                            //绑定来源
                            string strpOriId = listSampMian[0].PidSrcId;
                            if (!string.IsNullOrEmpty(strpOriId)
                                && (strpOriId == "110" || strpOriId == "108" || strpOriId == "107" || strpOriId == "109"))
                            {
                                selectDict_Origin1.SelectByID(strpOriId);
                            }
                            else
                            {
                                selectDict_Origin1.SelectByID("-1");
                            }
                        }

                        if (!string.IsNullOrEmpty(listSampMian[0].PidIdtId) && txtPatIdType.Visible)
                        {
                            //绑定ID类型
                            string strpNoId = listSampMian[0].PidIdtId;
                            if (!string.IsNullOrEmpty(strpNoId)
                                && (strpNoId == "110" || strpNoId == "106" || strpNoId == "107"))
                            {
                                txtPatIdType.SelectByID(strpNoId);
                            }
                            else
                            {
                                txtPatIdType.SelectByID("-1");//如果不符合,则置空
                            }
                        }

                    }
                }
            }
        }


        /// <summary>
        /// 接收其它控件传值控制，门诊条码、住院条码控制
        /// </summary>
        /// <param name="p_objControl"></param>
        public void m_mthSetSoureControl(BCPrintControl p_objControl)
        {
            m_objBCPrintcotrl = p_objControl;
        }

        /// <summary>
        /// 清除传入的条码打印控制控件
        /// </summary>
        public void m_mthClearSoureControl()
        {
            m_objBCPrintcotrl = null;
        }

        private void checkBox_ToOutPatientPrint_CheckedChanged(object sender, EventArgs e)
        {
            //如果钩选为转移门诊条码打印，那么会自动选择打印条码回执
            if (checkBox_ToOutPatientPrint.Checked)
            {
                checkBox_PrintReturnBarcode.Checked = true;
            }
        }

        private void cbSex_KeyDown(object sender, KeyEventArgs e)
        {
            if (cbSex.Text == "1")
            {
                cbSex.EditValue = "男";
            }
            else if (cbSex.Text == "2")
            {
                cbSex.EditValue = "女";
            }
        }

        private void selectDict_Origin1_ValueChanged(object sender, dcl.client.control.ValueChangeEventArgs args)
        {
            string prefix = UserInfo.GetSysConfigValue("Barcode_ManualPatientDefaultPrefix");
            if (args.Value != null && args.Value.ToString() == "110")
            {
                if (txtPatientID.Text.Trim() == string.Empty)
                {
                    txtPatientID.Text = prefix;
                    txtPatientID.SelectionStart = txtPatientID.Text.Length + 1;
                    txtPatientID.ScrollToCaret();
                }
            }
            else
            {
                if (txtPatientID.Text.Trim() == prefix)
                {
                    txtPatientID.Text = string.Empty;
                }
            }
        }

        private void txtPatientID_Enter(object sender, EventArgs e)
        {
            txtPatientID.SelectionStart = txtPatientID.Text.Length + 1;
            txtPatientID.ScrollToCaret();
        }

        private void sysToolBar1_BtnPrintSetClick(object sender, EventArgs e)
        {
            FrmPrintConfigurationV2 configer = new FrmPrintConfigurationV2();
            configer.ShowDialog();
        }

        /// <summary>
        /// 院感条码
        /// </summary>
        bool IsYG = false;
        internal void SetMaunaText()
        {
            IsYG = true;
            //lblTitle.Text = "院感条码打印";
        }

        private string GetBirthday(string age, DateTime dtNow)
        {
            string strBirthday = string.Empty;                         // 年龄的字符串表示
            int intYear = 0;                                    // 岁
            int intMonth = 0;                                    // 月
            int intDay = 0;                                    // 天

            // 如果没有设定出生日期, 返回空
            if (string.IsNullOrEmpty(age))
            {
                return string.Empty;
            }
            string[] strAge = age.Split('Y', 'M', 'D', 'H', 'I');
            // 计算天数
            intDay = dtNow.Day - Convert.ToInt16(strAge[2]);
            if (intDay < 0)
            {
                dtNow = dtNow.AddMonths(-1);
                intDay += DateTime.DaysInMonth(dtNow.Year, dtNow.Month);
            }
            if (intDay == 0)
            {
                intDay = 1;
            }

            // 计算月数
            intMonth = dtNow.Month - Convert.ToInt16(strAge[1]);
            if (intMonth <= 0)
            {
                intMonth += 12;
                dtNow = dtNow.AddYears(-1);
            }

            // 计算年数
            intYear = dtNow.Year - Convert.ToInt16(strAge[0]);

            strBirthday = intYear.ToString() + "-" + intMonth.ToString() + "-" + intDay.ToString();
            //strBirthday += intMonth.ToString() + "M";
            //strBirthday += intDay.ToString() + "D" + intHour;

            return strBirthday;
        }

        private string GetAge(DateTime dtBirthday, DateTime dtNow)
        {
            string strAge = string.Empty;                         // 年龄的字符串表示
            int intYear = 0;                                    // 岁
            int intMonth = 0;                                    // 月
            int intDay = 0;                                    // 天

            // 如果没有设定出生日期, 返回空
            if (string.IsNullOrEmpty(dtBirthday.ToString(CommonValue.DateTimeFormat)))
            {
                return string.Empty;
            }

            // 计算天数
            intDay = dtNow.Day - dtBirthday.Day;
            if (intDay < 0)
            {
                dtNow = dtNow.AddMonths(-1);
                intDay += DateTime.DaysInMonth(dtNow.Year, dtNow.Month);
            }

            // 计算月数
            intMonth = dtNow.Month - dtBirthday.Month;
            if (intMonth < 0)
            {
                intMonth += 12;
                dtNow = dtNow.AddYears(-1);
            }

            // 计算年数
            intYear = dtNow.Year - dtBirthday.Year;
            if (intYear < 0)
            {
                return string.Empty;
            }
            string intHour = "0H0I";

            strAge = intYear.ToString() + "Y";
            strAge += intMonth.ToString() + "M";
            strAge += intDay.ToString() + "D" + intHour;

            return strAge;
        }

        private void txtBirthday_Leave(object sender, EventArgs e)
        {
            txtAge.AgeValueText = GetAge(txtBirthday.DateTime, DateTime.Now);
        }

        private void txtAge_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAge.AgeValueText))
            {
                txtBirthday.DateTime = DateTime.Now;
                return;
            }
            txtBirthday.DateTime = Convert.ToDateTime(GetBirthday(txtAge.AgeValueText, DateTime.Now));
        }

        private void txtPrePlaceBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && txtPrePlaceBarcode.Text.Trim() != string.Empty)
            {
                bool result = patientControl1.PrintPrePlaceBarcode(txtPrePlaceBarcode.Text.Trim(), new Manual());
                if (result)
                {
                    int focusedRowHandle = patientControl1.MainGridView.FocusedRowHandle;
                    patientControl1.MoveNext(focusedRowHandle);
                }
                txtPrePlaceBarcode.Text = string.Empty;
                txtPrePlaceBarcode.Focus();
            }
        }
    }
}
