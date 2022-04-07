using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using dcl.client.common;
using dcl.client.frame;
using dcl.common;
using dcl.client.wcf;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.sample
{
    public partial class BCManualYG : UserControl
    {
        #region 全局变量
        private BCPrintControl m_objBCPrintcotrl = null;

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


        public BCManualYG()
        {
            InitializeComponent();
        }

        List<EntityDicCombine> listComb = new List<EntityDicCombine>();
        FrmCombineManager f;

        private void BCManualYG_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            CreateControl();

            DateTime now = ServerDateTime.GetServerDateTime();
            this.dateTimeControl1.DateTime = now; //申请时间默认为当前服务器

            this.patientControl1.Step.Printer = PrintFactory.Create(PrintType.Manual);

            UserInfo.SkipPower = true;
            sysToolBar1.CheckPower = false;

            sysToolBar1.SetToolButtonStyle(new string[] {
                sysToolBar1.BtnBCPrint.Name,
                sysToolBar1.BtnReset.Name,
                sysToolBar1.BtnSave.Name,
                sysToolBar1.BtnDelete.Name,
                sysToolBar1.BtnPrintSet.Name,
                //sysToolBar1.BtnSearch.Name,
                sysToolBar1.BtnClose.Name
                });

            //strBtntool = new string[] { "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10","F11", "F12" };
            //系统配置：院感条码[忽略]复选框默认打勾
            if (ConfigHelper.GetSysConfigValueWithoutLogin("BC_YGBarcodeCbNot_Checked") == "否")
            {
                ckbNotSex.Checked = false;
                ckbNotAge.Checked = false;
            }
            sysToolBar1.BtnReset.Caption = "清空";
            //sysToolBar1.BtnSearch.Caption = "报告查询";
            //sysToolBar1.Enabled = true;

            selectDicPubEvalute_onBeforeFilter();
            string strOri = ConfigHelper.GetSysConfigValueWithoutLogin("BC_YGBarcodeDefaultOrigin");
            if (!string.IsNullOrEmpty(strOri))
                selectDict_Origin1.SelectByID(strOri);
            chkAutoPrint.Checked = false;
            patientControl1.ShowInfoYG();

            lbEvaluteMore.Click += LbEvaluteMore_Click;

            this.txtCombineEdit.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtCombineEdit_ButtonClick);
            this.txtCombineEdit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCombineEdit_KeyDown);
        }


        private void setDisinfectDefault()
        {
            if (!checkDepart.Checked)
                txtDeptId.SelectByID(DeptCode);
            if (!checkPatName.Checked)
            {
                selectDic_Pub_Evalute.displayMember = string.Empty;
                selectDic_Pub_Evalute.valueMember = string.Empty;
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
                f.Location = new Point(94, 155);
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
        /// <summary>
        /// 监测对象过滤
        /// </summary>
        private void selectDicPubEvalute_onBeforeFilter()
        {
            List<EntityDicPubEvaluate> List = CacheClient.GetCache<EntityDicPubEvaluate>();
            List = List.FindAll(w => w.EvaFlag == "19");
            selectDic_Pub_Evalute.SetFilter(List);
        }
        private new void CreateControl()
        {
            if (f == null)
            {
                f = new FrmCombineManager(listComb, string.Empty, string.Empty);
                f.IsFilter = true;
                f.IsYgManual = true;
                bool isShowUrgentSelect = ConfigHelper.GetSysConfigValueWithoutLogin("BCManualYG_EnableHideUrgentColumns") == "是";//是否隐藏加急列
                f.ShowUrgentSelect = !isShowUrgentSelect;

                f.RefreshCombineTextDemanded += new FrmCombineManager.ResreshCombineTextDemandedEventhandler(f_RefreshCombineTextDemanded);
                f.SaveBarCode += new FrmCombineManager.ResreshCombineTextDemandedEventhandler(f_SaveBarCode);
            }

            f.StrFilter = txtCombineEdit.Text;

            f.Location = this.txtCombineEdit.Location;
            f.Left = this.txtCombineEdit.PointToScreen(new Point(0, 0)).X + this.txtCombineEdit.Width - f.Width;
            f.Top = this.txtCombineEdit.PointToScreen(new Point(0, this.txtCombineEdit.Height)).Y;
            txtCombineEdit.Text = string.Empty;

            //f.Show();
            f.Visible = false;
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
            if (!ckbNotSex.Checked)
            {
                if (string.IsNullOrEmpty(cbSex.Text))
                {
                    cbSex.SelectedIndex = 2;
                    cbSex.Text = "未知";
                }
            }

            if (!ckbNotAge.Checked)
            {
                if (string.IsNullOrEmpty(txtAge.AgeValueText))
                {
                    txtAge.AgeValueText = "0Y0M0D0H0I";
                }
            }
            if (f == null || f.dtCombine.Count == 0)
            {
                lis.client.control.MessageDialog.Show("请输入监测项目！", "提示");
                txtCombineEdit.Focus();
                return;
            }

            if (string.IsNullOrEmpty(selectDic_Pub_Evalute.displayMember))
            {
                lis.client.control.MessageDialog.Show("请输入监测对象！", "提示");
                selectDic_Pub_Evalute.Focus();
                return;
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
            string createTime = dateTimeControl1.DateTime.ToString(CommonValue.DateTimeFormat);
            EntitySampMain sampMain = new EntitySampMain();

            sampMain.PidName = selectDic_Pub_Evalute.displayMember;
            sampMain.PidSex = ckbNotSex.Checked ? null : cbSex.Text;
            sampMain.PidAge = ckbNotAge.Checked ? null : txtAge.AgeValueText;
            sampMain.PidDeptName = txtDeptId.displayMember;
            sampMain.PidDeptCode = txtDeptId.valueMember;
            sampMain.PidDoctorName = txtDoctor.displayMember;
            sampMain.PidDoctorCode = txtDoctor.valueMember;
            sampMain.SampComName = txtCombineEdit.Text;
            sampMain.SampStatusId = ((int)EnumBarcodeOperationCode.BarcodeGenerate).ToString();
            sampMain.SampStatusName = "生成条码";
            sampMain.SampDate = dateTimeControl1.DateTime;
            sampMain.PidSrcId = selectDict_Origin1.valueMember;
            sampMain.PidSrcName = selectDict_Origin1.displayMember;
            sampMain.SampBarType = 0;
            sampMain.SampOccDate = dateTimeControl1.DateTime;
            sampMain.SampUrgentFlag = false;


            //生成bc_cname表
            if (f.dtCombine != null)
            {
                foreach (EntityDicCombine row in f.dtCombine)
                {
                    EntitySampDetail sampDetail = new EntitySampDetail();

                    sampDetail.ComId = row.ComId.ToString();
                    sampDetail.ComName = row.ComName.ToString();
                    sampDetail.OrderName = row.ComName.ToString();
                    sampDetail.OrderDate = dateTimeControl1.DateTime;
                    sampDetail.OrderOccDate = dateTimeControl1.DateTime;
                    sampDetail.ComUrgentFlag =Convert.ToInt32(row.Urgent);
                    sampMain.ListSampDetail.Add(sampDetail);
                }
            }
            EntitySampProcessDetail sampProcess = new EntitySampProcessDetail();
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
                }
                patientControl1.SelectWhenNotPrint = true;
                patientControl1.SelectNotPrintForOutPatients();
                patientControl1.NeedPrint = false;
                Clear();
                lis.client.control.MessageDialog.ShowAutoCloseDialog("保存成功.");
                if (chkAutoPrint.Checked == true)
                {
                    sysToolBar1_OnBtnBCPrintClicked(null, null);
                }
                this.selectDic_Pub_Evalute.Focus();
                patientControl1.MainGridView.MoveLast();
            }
        }

        //监测对象多选
        private void LbEvaluteMore_Click(object sender, EventArgs e)
        {
            FrmPubEvaluateManager man = new FrmPubEvaluateManager("19", selectDic_Pub_Evalute.displayMember);
            if (man.ShowDialog() == DialogResult.OK)
            {
                selectDic_Pub_Evalute.displayMember = man.result;
            }
        }

        private bool CheckInput()
        {
            return false;
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
            patientControl1.IsYG = true;
            //打印条码事件
            patientControl1.PrintBarcode(new Manual());
        }

        private void sysToolBar1_BtnResetClick(object sender, EventArgs e)
        {
            patientControl1.Reset();
            Clear();
        }

        private void Clear()
        {
            if (!checkItem.Checked)
            {
                if (f.dtCombine != null && f.dtCombine.Count > 0)
                    f.dtCombine.Clear();
                txtCombineEdit.Text = "";
            }

            //txtPatientName.Text = "";
            if (!checkDoctor.Checked)
            {
                txtDoctor.valueMember = string.Empty;
                txtDoctor.displayMember = string.Empty;
                txtDoctor.selectRow = null;
            }
            if (!checkDepart.Checked)
            {
                txtDeptId.displayMember = string.Empty;
                txtDeptId.valueMember = string.Empty;
            }
            cbSex.SelectedIndex = 2;//默认未知
            txtAge.AgeValueText = "0Y0M0D0H0I";//默认0岁

            //if (IsAlone && ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_DisinfectBarCode") == "开")
            setDisinfectDefault();
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



        private void sysToolBar1_BtnPrintSetClick(object sender, EventArgs e)
        {
            FrmPrintConfigurationV2 configer = new FrmPrintConfigurationV2();
            configer.ShowDialog();
        }

        private DataTable dtImportBC = null;
        void frmexportmsg_ImportBCDataEvent(DataTable dt)
        {
            dtImportBC = null;
            if (dt != null)
            {
                dtImportBC = dt;
            }
        }

        private void selectDict_Origin1_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            if (selectDict_Origin1.valueMember.Trim() == "110")
            {
                txtDeptId.SetFilter(txtDeptId.getDataSource().FindAll(w => w.DeptSource == "110"));
            }
        }
    }
}
