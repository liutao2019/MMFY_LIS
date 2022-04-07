using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using dcl.common.extensions;
using dcl.client.common;

using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using dcl.client.frame;
using dcl.common;
using lis.client.control;
using DevExpress.XtraEditors;

using dcl.client.report;
using dcl.client.wcf;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.sample
{
    public partial class BCSearchControl : ClientBaseControl, IStepable
    {
        public BCSearchControl()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.BCPrintControl_Load);
        }
        //启用采血照相功能
        bool BarBloodPatImgAQ = false;

        bool Barcode_ShowPrintTimeText;

        private List<EntitySampMain> ListSampMain = new List<EntitySampMain>();

        private int BCSearch_InterDays = -1;
        public override void InitParamters()
        {
            this.subTable = BarcodeTable.Patient.TableName;
            this.primaryKeyOfSubTable = BarcodeTable.Patient.ID;
        }

        public bool IsForFee { get; set; }
        List<EntityDicInstrument> listIns = new List<EntityDicInstrument>();
        private string labQuery_PatName_SearchModel = "全模糊";
        /// <summary>
        /// 住院独立调用条码查询客户，需要控制允许他查询的科室。
        /// </summary>
        public string DeptCode { get; set; }

        private void BCPrintControl_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            this.cePrint.CheckedChanged += new System.EventHandler(this.cbBCQueryPrint_CheckedChanged);
            cepatmain.CheckedChanged += Cepatmain_CheckedChanged;

            if (UserInfo.GetSysConfigValue("BarcodeCheckSelect") == "是")
                patientControl.ShouldMultiSelect = true;
            BarBloodPatImgAQ = ConfigHelper.GetSysConfigValueWithoutLogin("BarBloodPatImgAQ") == "是";

            string days = ConfigHelper.GetSysConfigValueWithoutLogin("BCSearch_InterDays");

            int intDay;
            if (!string.IsNullOrEmpty(days) && int.TryParse(days, out intDay))
            {
                BCSearch_InterDays = intDay;
            }


            //显示送达者与签收者列名
            patientControl.showColReachAndReceiverForName(true);

            string btnBcPrint = UserInfo.HaveFunction("FrmBCSearch_Print", "FrmBCSearch_Print") ? sysToolBar1.BtnBCPrint.Name : string.Empty;
            string queryTime = ConfigHelper.GetSysConfigValueWithoutLogin("BC_SearchDefaultQueryTime");
            if (!string.IsNullOrEmpty(queryTime))
            {
                cmbTimeType.EditValue = queryTime;
            }

            Barcode_ShowPrintTimeText = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_ShowPrintTimeText") == "是";

            if (Barcode_ShowPrintTimeText)
            {
                lblPt.Visible = true;
                txtPrintTime.Visible = true;
            }
            //判断是否有门诊回执报表
            bool blnHaveMZBCReturn = false;
            if (!string.IsNullOrEmpty(ConfigHelper.GetSysConfigValueWithoutLogin("MZBarcodeReturnReport")))
            {
                blnHaveMZBCReturn = true;
            }
            string BarBloodPatImgAQName = BarBloodPatImgAQ ? sysToolBar1.btnBrowse.Name : "";
            sysToolBar1.btnBrowse.Caption = "查看抽血照片";

            sysToolBar1.BtnSinglePrint.Caption = "打印二维码";
            sysToolBar1.BtnDeSpe.Caption = "记录";

            sysToolBar1.BtnQualityOut.Caption = "复制标本信息";


            if (!string.IsNullOrEmpty(this.DeptCode))
            {
                sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnSearch.Name, btnBcPrint, sysToolBar1.BtnClose.Name });
                lueDepart.SelectByID(DeptCode);
            }
            else
            {
                if (blnHaveMZBCReturn)
                {
                    sysToolBar1.SetToolButtonStyle(new string[] {
                        sysToolBar1.BtnSearch.Name,
                btnBcPrint,
                sysToolBar1.BtnBCPrintReturn.Name,
                 sysToolBar1.BtnPrintList.Name,
                 sysToolBar1.BtnQualityTest.Name,
                sysToolBar1.BtnReset.Name,
                sysToolBar1.BtnCancel.Name,
                BarBloodPatImgAQName,
                "BtnExport",
                sysToolBar1.BtnDeSpe.Name,
                sysToolBar1.BtnClose.Name ,
                    "BtnQualityOut"});
                }
                else
                {
                    sysToolBar1.SetToolButtonStyle(new string[] {
                         sysToolBar1.BtnSearch.Name,
                btnBcPrint,
                sysToolBar1.BtnPrintList.Name,
                sysToolBar1.BtnQualityTest.Name,
                sysToolBar1.BtnReset.Name,
                sysToolBar1.BtnCancel.Name,
                BarBloodPatImgAQName,"BtnExport",
                sysToolBar1.BtnDeSpe.Name,
                sysToolBar1.BtnClose.Name,
                    "BtnQualityOut"});
                }
                sysToolBar1.BtnCancel.Caption = "撤销条码";
                sysToolBar1.BtnQualityTest.Caption = "打印条码信息";
            }

            if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("PrintBarcode_CreateQcCode") != "是")
            {
                sysToolBar1.BtnSinglePrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }

            Init();
            lblTitle.Text = "条码查询统计";

            sysToolBar1.OnBtnSearchClicked += SysToolBar1_OnBtnSearchClicked;
            sysToolBar1.OnBtnExportClicked += new EventHandler(sysToolBar1_OnBtnExportClicked);
            sysToolBar1.OnBtnQualityOutClicked += new EventHandler(sysToolBar1_BtnCopyClick);


            //条码查询初始化时间间隔设置
            int time = 0;
            try
            {
                time = Convert.ToInt32(ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_New_Search_DaySpan"));
            }
            catch
            {
                time = 7;
            }
            this.deStart.EditValue = ServerDateTime.GetServerDateTime().Date.AddDays(-time);

            deEnd.EditValue = ServerDateTime.GetServerDateTime().Date.AddDays(1).AddMilliseconds(-1);

            labQuery_PatName_SearchModel = ConfigHelper.GetSysConfigValueWithoutLogin("SampQuery_PatName_SearchModel");

            sysToolBar1.btnBrowse.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(btnBrowse_Click);

            patientControl.SetInfoDisVisableForSearch();

            listIns = CacheClient.GetCache<EntityDicInstrument>();

            //仪器数据初始化
            bindingSourceItr.DataSource = listIns;
        }

        private void sysToolBar1_BtnCopyClick(object sender, EventArgs e)
        {
            string text = patientControl.CopyBarcodeInfo();
            FrmCopyBarcode copy = new FrmCopyBarcode(text);
            copy.Show();
        }

        private void SysToolBar1_OnBtnSearchClicked(object sender, EventArgs e)
        {
            sysToolBar1.Focus();
            btnAdviceDownload_Click(null, null);
        }

        void sysToolBar1_OnBtnExportClicked(object sender, EventArgs e)
        {
            patientControl.ExportToExcel();
        }

        /// <summary>
        /// 查看抽血照片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnBrowse_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
        }

        public override void InitData()
        {
        }

        int focus = 0;  //条件框最后获得焦点标志
        private void btnAdviceDownload_Click(object sender, EventArgs e)
        {
            try
            {
                System.TimeSpan ts = Convert.ToDateTime(deEnd.EditValue).Subtract(Convert.ToDateTime(deStart.EditValue));

                if (BCSearch_InterDays != -1)
                {
                    if (ts.Days > BCSearch_InterDays)
                    {
                        lis.client.control.MessageDialog.Show("查询时间间隔不能超过" + BCSearch_InterDays + "天！", "提示");
                        return;
                    }
                }
                else
                {
                    if (!isTheTimeOver30Days())
                    {
                        if (ts.Days > 30)
                        {
                            lis.client.control.MessageDialog.Show("查询时间间隔不能超过30天！", "提示");
                            return;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(this.DeptCode) && string.IsNullOrEmpty(lueDepart.valueMember))
                {
                    lis.client.control.MessageDialog.Show("请选择查询科室！", "提示");
                    return;
                }

                #region 病人姓名、条码号、病人ID、科室、实验组必填一项
                if (string.IsNullOrEmpty(selectAuditOper.valueMember?.ToString())
                    && string.IsNullOrEmpty(txtPatName.Text.Trim())
                    && string.IsNullOrEmpty(this.txtBarcode.Text.Trim())
                    && string.IsNullOrEmpty(txtInNo.Text.Trim())
                    && string.IsNullOrEmpty(lueType.valueMember)
                    && (string.IsNullOrEmpty(this.DeptCode) && string.IsNullOrEmpty(lueDepart.valueMember))
                    )
                {
                    lis.client.control.MessageDialog.Show("科室、病人ID、姓名、实验组、条码号必填一项！", "提示");
                    return;
                }


                #endregion

                //splashScreenManager1.ShowWaitForm();
                //splashScreenManager1.SetWaitFormCaption("请等待");
                //splashScreenManager1.SetWaitFormDescription("正在加载中。。。");
                patientControl.Reset();

                LoadData();
                if (focus == 1)
                {
                    //txtBarcode.Text = string.Empty; //不清空查询条件
                    txtBarcode.Focus();
                }
                if (focus == 2)
                {
                    //txtInNo.Text = string.Empty;
                    txtInNo.Focus();
                }
                if (focus == 3)
                {
                    //txtPatName.Text = string.Empty;
                    txtPatName.Focus();
                }
                if (focus == 4)
                {
                    //txtSigner.Text = string.Empty;
                    txtBarcode.Focus();
                }
            }
            finally
            {
                try
                {
                    // splashScreenManager1.CloseWaitForm();
                }
                catch
                { }
            }
        }


        /// <summary>
        /// 查询数据
        /// </summary>
        private void LoadData()
        {
            if (string.IsNullOrEmpty(deStart.Text) || string.IsNullOrEmpty(deEnd.Text))
            {
                lis.client.control.MessageDialog.Show("请输入时间范围或编号!");
                return;
            }


            EntitySampQC sampQuery = new EntitySampQC();

            sampQuery.StartDate = Convert.ToDateTime(deStart.EditValue).ToString("yyyy-MM-dd HH:mm:ss");
            sampQuery.EndDate = Convert.ToDateTime(deEnd.EditValue).ToString("yyyy-MM-dd HH:mm:ss");

            SelectType sType = new SelectType();

            sampQuery.SearchDateType = SearchDataType.标本流程时间;
            switch (cmbTimeType.Text)
            {
                case "医嘱下载":
                    sampQuery.SearchDateType = SearchDataType.标本下载时间;
                    sType = SelectType.Create;
                    break;
                case "条码打印":
                    sampQuery.SerchDateStatus = "1";
                    break;
                case "条码采集":
                    sampQuery.SerchDateStatus = "2";
                    sType = SelectType.Sampling;
                    break;
                case "条码收取":
                    sampQuery.SerchDateStatus = "3";
                    sType = SelectType.Confirm;
                    break;
                case "条码送达":
                    sampQuery.SerchDateStatus = "4";
                    sType = SelectType.Send;
                    break;
                case "条码签收":
                    sampQuery.SerchDateStatus = "5";
                    sType = SelectType.Reach;
                    break;
                case "二次送检":
                    sampQuery.SerchDateStatus = "8";
                    sType = SelectType.SecondSend;
                    break;
                default:
                    break;
            }

            if (selectAuditOper.valueMember != null && selectAuditOper.valueMember != "")
            {
                string OperUserID = selectAuditOper.valueMember.ToString();
                switch (OperType.Text)
                {
                    case "采集者":
                        sampQuery.CollectionUserID = OperUserID;
                        break;
                    case "送达者":
                        sampQuery.ReachUserID = OperUserID;
                        break;
                    case "送检者":
                        sampQuery.SendUserID = OperUserID;
                        break;
                    case "签收者":
                        sampQuery.ReceivedUserID = OperUserID;
                        break;
                }
            }

            patientControl.SelectType = sType;

            sampQuery.LeftJoinSampleDetail = cepatmain.Checked;
            if(!string.IsNullOrEmpty(selectDict_Instrmt1.valueMember)&& cepatmain.Checked)
                sampQuery.ItrId = selectDict_Instrmt1.valueMember;

            sampQuery.PidDeptCode = lueDepart.valueMember;
            sampQuery.PidDeptName = lueDepart.displayMember;

            if (!string.IsNullOrEmpty(lueType.valueMember))
                sampQuery.ListType.Add(lueType.valueMember);

            if (!string.IsNullOrEmpty(txtBarcode.Text.Trim()))
                sampQuery.ListSampBarId.Add(txtBarcode.Text.Trim());

            if (!string.IsNullOrEmpty(txtInNo.Text))
            {
                sampQuery.SearchType = "病人ID";
                txtInNo.Text = ConverterCreator.Instance.Converter.Convert(txtInNo.Text);
                sampQuery.SearchValue = SQLFormater.FormatSQL(txtInNo.Text);
            }
            if (txtPatName.Text.Trim() != "")
            {
                if (string.IsNullOrEmpty(labQuery_PatName_SearchModel) || labQuery_PatName_SearchModel == "全模糊")
                {
                    sampQuery.matchType = MatchType.QUANMOHU;
                }
                if (labQuery_PatName_SearchModel == "半模糊")
                {
                    sampQuery.matchType = MatchType.BANMOHU;
                }
                if (labQuery_PatName_SearchModel == "全匹配")
                {
                    sampQuery.matchType = MatchType.QUANPIPEI;
                }
                sampQuery.PidName = txtPatName.Text.Trim();
            }
            sampQuery.RegRackNo = txtRackNo.Text.Trim();

            sampQuery.ComId = lueItem.valueMember;

            patientControl.ClearChecked();

            patientControl.LoadData(sampQuery);

            if (!patientControl.HasData())
            {
                //splashScreenManager1.CloseWaitForm();
                lis.client.control.MessageDialog.Show("无返回数据!");
                return;
            }

            ListSampMain = new List<EntitySampMain>(patientControl.ListSampMain.ToArray());

            if (radioGroup1.SelectedIndex > 0)
                Filter();
        }


        private void InitTitle()
        {
            if (Step != null)
                lblTitle.Text = "标本" + Step.StepName;
        }

        private void Init()
        {
            //adviceTime1.Value = GetDefaultAdviceTime();
        }

        public DateTimeRange GetDefaultAdviceTime()
        {
            return new DateTimeRange(DateTime.Now.Date.AddMonths(-1), DateTime.Now.Date.AddDays(1).AddSeconds(-1));
        }


        private void sysToolBar1_OnBtnBCPrintClicked(object sender, EventArgs e)
        {
            int i = 1;
            if (Barcode_ShowPrintTimeText && !string.IsNullOrEmpty(txtPrintTime.Text))
            {
                if (!int.TryParse(txtPrintTime.Text, out i))
                {
                    i = 1;
                }
            }
            patientControl.PrintBarcodeAuto(i);

        }

        private void ShowMessage(string word)
        {
            lis.client.control.MessageDialog.Show(word, "提示");
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Filter();
            patientControl.UpdateCount();
            patientControl.RefreshCurrentBarcodeInfo();
        }

        private void Filter()
        {
            if (radioGroup1.SelectedIndex == 0)
                patientControl.BindingSampMain(ListSampMain.ToList());
            else if (radioGroup1.SelectedIndex == 1)
                patientControl.BindingSampMain(ListSampMain.Where(w => w.SampStatusId == "1").ToList());
            else if (radioGroup1.SelectedIndex == 2)
                patientControl.BindingSampMain(ListSampMain.Where(w => w.SampStatusId == "2").ToList());
            else if (radioGroup1.SelectedIndex == 3)
                patientControl.BindingSampMain(ListSampMain.Where(w => w.SampStatusId == "3").ToList());
            else if (radioGroup1.SelectedIndex == 4)
                patientControl.BindingSampMain(ListSampMain.Where(w => w.SampStatusId == "4").ToList());
            else if (radioGroup1.SelectedIndex == 5)
                patientControl.BindingSampMain(ListSampMain.Where(w => w.SampStatusId == "5" && w.ReceiverDate != null &&
                w.ReceiverDate.Value > deStart.DateTime && w.ReceiverDate.Value < deEnd.DateTime).ToList());
            else if (radioGroup1.SelectedIndex == 6)
                patientControl.BindingSampMain(ListSampMain.Where(w => w.SampStatusId == "20" || w.SampStatusId == "50" || w.SampStatusId == "70").ToList());
            else if (radioGroup1.SelectedIndex == 7)
                patientControl.BindingSampMain(ListSampMain.Where(w => w.SampStatusId == "40").ToList());
            else if (radioGroup1.SelectedIndex == 8)
                patientControl.BindingSampMain(ListSampMain.Where(w => w.SampStatusId == "60").ToList());
            else if (radioGroup1.SelectedIndex == 9)
                patientControl.BindingSampMain(ListSampMain.Where(w => w.SampStatusId == "560").ToList());
            else if (radioGroup1.SelectedIndex == 10)
                patientControl.BindingSampMain(ListSampMain.Where(w => w.DelFlag == "1").ToList());
            else if (radioGroup1.SelectedIndex == 11)
                patientControl.BindingSampMain(ListSampMain.Where(w => w.SampStatusId == "9").ToList());
        }

        private void sysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            if (this.ParentForm != null)
                this.ParentForm.Close();
        }

        #region IStepable 成员

        public IStep Step
        {
            get;
            set;
        }
        private StepType stepType = StepType.Select;


        public StepType StepType
        {
            get { return stepType; }
            set
            {
                Step = StepFactory.CreateStep(StepType.Select);
                patientControl.StepType = this.StepType;
            }
        }

        public bool Barcode_ShowInfo = false;

        #endregion


        /// <summary>
        /// 打印清单
        /// </summary>
        private void sysToolBar1_OnBtnPrintListClicked(object sender, EventArgs e)
        {
            patientControl.PrintList();
        }


        /// <summary>
        /// 撤消明细
        /// </summary>
        private void sysToolBar1_BtnDeleteSubClick(object sender, EventArgs e)
        {
            patientControl.DeleteCname(string.Empty, string.Empty);
        }

        /// <summary>
        /// 撤消条码
        /// </summary>        
        private void sysToolBar1_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            patientControl.DeleteBarcode(string.Empty, string.Empty);

        }


        /// <summary>
        /// 拆分条码
        /// </summary>
        private void sysToolBar1_BtnSperateBarcodeClick(object sender, EventArgs e)
        {
            patientControl.SeparateBarcode(false);
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if (ceContinuitySearch.Checked)//如果勾选了,连查
            {
                //*********此功能暂时不受其他查询条件限制,只跟条码有关
                //*********
                if (txtBarcode.Text.Trim().Length <= 0)
                {
                    lis.client.control.MessageDialog.Show("请输入条码号信息");
                    txtBarcode.Focus();
                    txtBarcode.SelectAll();
                    return;
                }

                //查询并添加条码
                bool addOk = patientControl.AddBarcode_Search(txtBarcode.Text.Trim());
                if (addOk)
                {
                    txtBarcode.Text = "";
                    txtBarcode.Focus();
                }
            }
            else
            {
                btnAdviceDownload_Click(null, EventArgs.Empty);
                ClearAndFocusBarcode(txtBarcode);
                int i = 1;
                if (Barcode_ShowPrintTimeText && !string.IsNullOrEmpty(txtPrintTime.Text))
                {
                    if (!int.TryParse(txtPrintTime.Text, out i))
                    {
                        i = 1;
                    }
                }

                if (patientControl.HasData() && UserInfo.GetSysConfigValue("Barcode_QueryPrint") == "是" && cePrint.Checked)
                {
                    patientControl.DefaultSelectFocusedRow();//默认焦点行打上勾
                    patientControl.PrintBarcodeAuto(i);
                }
                else if (patientControl.HasData() && cePrint.Checked)
                {
                    patientControl.DefaultSelectFocusedRow();//默认焦点行打上勾
                    patientControl.PrintBarcodeAuto(i);
                }
            }
        }

        private void ClearAndFocusBarcode(TextEdit text)
        {
            text.Text = "";
            text.Focus();
        }

        private void txtInNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;
            btnAdviceDownload_Click(null, EventArgs.Empty);
            ClearAndFocusBarcode(txtInNo);
        }

        private void txtPatName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAdviceDownload_Click(null, EventArgs.Empty);
                ClearAndFocusBarcode(txtPatName);
            }
        }

        private void cmbTimeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTimeType.Text.Trim() == "医嘱下载")
            {
                deStart.EditValue = DateTime.Now.Date.AddDays(-3);
                txtPlace.Properties.ReadOnly = true;
            }
            else
            {
                deStart.EditValue = DateTime.Now.Date;
                txtPlace.Properties.ReadOnly = false;
            }

            deEnd.EditValue = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);
        }

        private void sysToolBar1_BtnResetClick(object sender, EventArgs e)
        {
            patientControl.Reset();

            resetAllControl();
        }

        private void resetAllControl()
        {
            cmbTimeType.SelectedIndex = 0;
            cmbTimeType_SelectedIndexChanged(null, null);

            lueDepart.valueMember = string.Empty;
            lueDepart.displayMember = string.Empty;

            lueItem.valueMember = string.Empty;
            lueItem.displayMember = string.Empty;

            lueType.valueMember = string.Empty;
            lueType.displayMember = string.Empty;

            //cbSignerSearchType.SelectedIndex = 0;

            txtBarcode.Text = string.Empty;
            txtInNo.Text = string.Empty;
            txtPatName.Text = string.Empty;
            //txtSigner.Text = string.Empty;
        }

        private void sysToolBar1_OnBtnCancelClicked(object sender, EventArgs e)
        {
            FrmReturnBarcodeV2 frm = new FrmReturnBarcodeV2();
            frm.ShowDialog();
        }

        private void patientControl_Load(object sender, EventArgs e)
        {
            if (UserInfo.GetSysConfigValue("Barcode_QueryPrint") == "是")
            {
                ceContinuitySearch.Checked = false;
                ceContinuitySearch.Enabled = false;
                cePrint.Checked = true;
            }
            else
            {
                cePrint.Checked = false;
                ceContinuitySearch.Enabled = true;
            }
        }


        /// <summary>
        /// 判断查询日期是否允许超过30天
        /// </summary>
        /// <returns></returns>
        private bool isTheTimeOver30Days()
        {
            if ((txtPatName.EditValue != null && txtPatName.EditValue.ToString().Trim() != "") ||
                (!string.IsNullOrEmpty(txtInNo.Text)) ||
                (!string.IsNullOrEmpty(txtBarcode.Text)))
                return true;
            else
                return false;
        }

        private void sysToolBar1_OnBtnBCPrintReturnClicked(object sender, EventArgs e)
        {
            patientControl.PrintBarcodeReturnBySearch(new OutPaitent());
        }

        /// <summary>
        /// 回退统计，暂时未发现有地方使用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {

        }

        private void sysToolBar1_OnBtnQualityTestClicked(object sender, EventArgs e)
        {
            patientControl.PrintBarCodeInfo();
        }

        /// <summary>
        /// 上机信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cepatmain_CheckedChanged(object sender, EventArgs e)
        {
            patientControl.ShowPatInfoColumns(cepatmain.Checked);
            selectDict_Instrmt1.Enabled = cepatmain.Checked;
        }

        /// <summary>
        /// 勾选打印时改变连打复选框状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbBCQueryPrint_CheckedChanged(object sender, EventArgs e)
        {
            if (cePrint.Checked)
            {
                ceContinuitySearch.Checked = false;
                ceContinuitySearch.Enabled = false;
            }
            else
            {
                ceContinuitySearch.Enabled = true;
            }
        }

        private void txtPrintTime_EditValueChanged(object sender, EventArgs e)
        {

        }


        private void txtBarcode_Enter(object sender, EventArgs e)
        {
            focus = 1;
        }

        private void txtInNo_Enter(object sender, EventArgs e)
        {
            focus = 2;
        }

        private void txtPatName_Enter(object sender, EventArgs e)
        {
            focus = 3;
        }

        private void txtSigner_Enter(object sender, EventArgs e)
        {
            focus = 4;
        }

        /// <summary>
        /// 打印二维码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnSinglePrintClicked(object sender, EventArgs e)
        {
            //patientControl1.m_blnCheckManualReturn = this.checkBox_PrintReturnBarcode.Checked;

            //打印条码事件
            //patientControl.PrintBarcodeForQcCode(new Manual());
        }

        private void sysToolBar1_BtnDeSpeClick(object sender, EventArgs e)
        {
            FrmRecordBarcode frmRecord = new FrmRecordBarcode();
            frmRecord.ShowDialog();
        }
        private void lueType_onAfterSelected(object sender, EventArgs e)
        {
            if (lueType.valueMember != "")
            {
                bindingSourceItr.DataSource = listIns.FindAll(w => w.ItrLabId == lueType.valueMember);
            }
        }

        #region DesignMode

        protected new bool DesignMode
        {
            get
            {
                bool returnFlag = false;
#if DEBUG
                if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                    returnFlag = true;
                else if (System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToUpper().Equals("DEVENV"))
                    returnFlag = true;
#endif
                return returnFlag;
            }
        }

        #endregion
    }
}