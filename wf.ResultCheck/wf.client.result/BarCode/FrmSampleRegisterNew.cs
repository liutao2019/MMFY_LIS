using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.result.CommonPatientInput;

using dcl.client.sample;
using dcl.common;
using dcl.client.wcf;
using lis.client.control;
using dcl.client.common;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using dcl.client.report;

using DevExpress.XtraEditors.Controls;
using dcl.entity;
using System.Linq;
using dcl.client.result.DictToolkit;
using System.Reflection;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using dcl.client.cache;
using Lib.LogManager;

namespace dcl.client.result
{
    /// <summary>
    /// 条码录入
    /// 自动分配原则
    /// 
    /// </summary>
    public partial class FrmSampleRegisterNew : FrmCommon
    {
        /// <summary>
        /// 统计当前界面标本登记数
        /// </summary>
        private int statBarcodeCount = 0;

        //含多组合的条码登记次数
        private int executeTimes = 0;

        private bool CombineNotInItrNeedWaring = false;
        private bool BcReg_AfterInputNeedCheek = true;
        private bool CheckReceiveTimeAndPatdate = false;
        private bool SwitchOrder = false;

        string Lab_NewSidCheckNullItrIDs = string.Empty;
        //标本登记界面增加一个录入者选项
        bool Lab_SignInShowInspetor = false;
        /// <summary>
        /// 标本登记允许打印工作单
        /// 
        /// (对应了系统配置：SampleRegister_printWorkbill)
        /// </summary>
        private bool allowPrintWorkbill = false;

        List<EntityPidReportMain> listPatient = new List<EntityPidReportMain>();


        /// <summary>
        /// 急查颜色
        /// </summary>
        Color corUrgent = Color.White;
        Color corBD = Color.White;

        public FrmSampleRegisterNew()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 标记是否单独弹窗
        /// </summary>
        bool isDialog = false;
        string dialog_insId;//单独弹窗传参-仪器ID
        public FrmSampleRegisterNew(bool isDialog, string instructmentId)
        {
            InitializeComponent();
            this.isDialog = isDialog;
            dialog_insId = instructmentId;
        }


        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmPatRapidEnter_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            //*****************************************************
            gridControlSingle.MouseDown += gridControl1_MouseDown;
            gridViewSingle.CustomDrawColumnHeader += gridViewPatientList_CustomDrawColumnHeader;
            gridViewSingle.RowCellStyle += gridViewSingle_RowCellStyle;
            gridViewSingle.RowStyle += gridViewSingle_RowStyle;
            gridViewSingle.RowCountChanged += GridViewSingle_RowCountChanged;
            repositoryItemCheckEdit1.Click += new System.EventHandler(this.repositoryItemCheckEdit1_Click);

            lblItr.Text = lblName.Text = lblPatcName.Text = lblSid2.Text = lblHostOrder.Text = string.Empty;
            gridheadercheckbox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();

            //隐藏非报告仪器
            Lab_HideNotReportInstrmt = UserInfo.GetSysConfigValue("Lab_HideNotReportInstrmt") == "是";
            Lab_CheckInstrmtAudit = UserInfo.GetSysConfigValue("Lab_CheckInstrmtAudit") == "是";
            CombineNotInItrNeedWaring = UserInfo.GetSysConfigValue("CombineNotInItrNeedWaring") == "是";
            BcReg_AfterInputNeedCheek = UserInfo.GetSysConfigValue("BcReg_AfterInputNeedCheek") != "否";
            CheckReceiveTimeAndPatdate = UserInfo.GetSysConfigValue("Other_CheckReceiveTimeAndPatdate") == "是";
            SwitchOrder = UserInfo.GetSysConfigValue("SampleRegister_order") == "是";
            if (Lab_HideNotReportInstrmt)
            {
                this.txtInstructment.SelectFilter = "isnull(itr_rep_ins,'1')<>'0'";
            }
            Lab_BarRegRemoveZero = ConfigHelper.GetSysConfigValueWithoutLogin("Lab_BarRegRemoveZero") == "是";
            chkBarcodeCanRepeat.Enabled = ConfigHelper.GetSysConfigValueWithoutLogin("BC_EnableCanRepeatInput") != "否";
            if (UserInfo.GetSysConfigValue("SampleRegister_DateEditInput") == "是")
            {
                txtPatDate.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
                dateCheck.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            }
            //*****************************************************


            if (UserInfo.GetSysConfigValue("checkPrintQD") == "否")
            {
                barSave.BtnPrintList.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                col_selected.Visible = false;
                barSave.BtnPrintListGerm.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            corUrgent = ConfigHelper.GetBarcodeConfigColor("New_Barcode_Color_Urgent");
            corBD = ConfigHelper.GetBarcodeConfigColor("New_Barcode_Color_BD");

            bsPatList.DataSource = new List<EntityPidReportMain>();

            this.txtPatDate.EditValue = ServerDateTime.GetServerDateTime();
            ChangeColor();

            ckCom.Checked = UserInfo.GetSysConfigValue("Barcode_CombineType") == "是";
            statBarcodeCount = 0;//初始化标本登记数


            //开启配置显示病人临床路径
            if (ConfigHelper.GetSysConfigValueWithoutLogin("ShowIdentityName") == "否")
            {
                gridColumn12.Visible = false;
            }

            if (UserInfo.GetSysConfigValue("BidirectionalRegisterSidEqualSeqID") == "是")
            {
                rgType.Properties.Items.Remove(rgType.Properties.Items.GetItemByValue("序号"));
                rgTypeFiter.Properties.Items.Remove(rgTypeFiter.Properties.Items.GetItemByValue("序号"));
            }

            //系统配置：标本登记允许打印工作单
            if (ConfigHelper.GetSysConfigValueWithoutLogin("SampleRegister_printWorkbill") == "是")
            {
                allowPrintWorkbill = true;
                barSave.BtnPrintListGerm.Caption = "工作单";//原：细菌报告
            }
            else
            {
                allowPrintWorkbill = false;
                barSave.BtnPrintListGerm.Caption = "细菌报告";//原：细菌报告
            }

            //默认勾上改标记
            ckOrder.Checked = ConfigHelper.GetSysConfigValueWithoutLogin("BC_ShowPrintSettingBtn") == "是";

            Lab_NewSidCheckNullItrIDs = ConfigHelper.GetSysConfigValueWithoutLogin("Lab_NewSidCheckNullItrIDs");
            Lab_SignInShowInspetor = UserInfo.GetSysConfigValue("Lab_SignInShowInspetor") == "是";

            if (Lab_SignInShowInspetor)
            {
                gcAdditionalInput.Visible = true;
                lblInput.Visible = true;
                txtPatInspetor.Visible = true;

                //默认检验人
                txtPatInspetor.SelectByID(UserInfo.loginID);
            }

            barSave.BtnAnswer.Caption = "跳空";
            string PrintBarCode = "BtnBCPrint";

            if (UserInfo.HaveFunction("FrmBCSearch_Print", "FrmBCSearch_Print") &&
                ConfigHelper.GetSysConfigValueWithoutLogin("SampleRegister_showPrintBCBnt") == "否")
                PrintBarCode = string.Empty;

            barSave.BtnPrintList.Caption = "打印清单";
            barSave.BtnPrintListGerm.Caption = "细菌报告";
            barSave.BtnPrintPreview2.Caption = "预览清单";
            barSave.BtnExport.Caption = "导出Excel";
            barSave.SetToolButtonStyle(new string[] {
                                        barSave.BtnRefresh.Name,
                                        barSave.BtnDelete.Name,
                                        barSave.BtnAnswer.Name,
                                        barSave.BtnPrintListGerm.Name,
                                        barSave.BtnPrintList.Name,
                                        barSave.BtnPrintPreview2.Name,
                                        barSave.BtnClose.Name,
                                        barSave.BtnExport.Name,
                                        PrintBarCode});


            if (LocalSetting.Current.Setting.CType_id != null
                && LocalSetting.Current.Setting.CType_id.Trim() != ""
                && !isDialog)
            {
                txtType.valueMember = LocalSetting.Current.Setting.CType_id;
                //利用selectById代替displayMember保证在有ID传过来时可选择仪器，在没有ID
                //传过来时不显示物理组，改善用户体验
                txtType.SelectByID(txtType.valueMember);
            }


            if (!UserInfo.isAdmin)
            {
                List<EntityDicPubProfession> listType = txtType.dtSource;

                if (UserInfo.sqlUserTypesFilter != string.Empty)
                {
                    listType = listType.FindAll(w => UserInfo.listUserLab.FindIndex(f => f.LabId == w.ProId) > -1);
                }
                else
                {
                    listType = new List<EntityDicPubProfession>();
                }
                txtType.SetFilter(listType);
            }

            if (isDialog)
            {
                txtInstructment.SelectByID(dialog_insId);
                this.ActiveControl = this.txtBarCode;
                this.txtBarCode.Focus();
            }

        }


        private void GridViewSingle_RowCountChanged(object sender, EventArgs e)
        {
            lblPrintCount.Text = gridViewSingle.RowCount.ToString();
        }

        bool Lab_BarRegRemoveZero = false;
        bool Lab_HideNotReportInstrmt = false;
        bool Lab_CheckInstrmtAudit = false;
        public string NoticeName
        {
            get
            {
                return rgType.Properties.Items[rgType.SelectedIndex].Value.ToString();
            }
        }

        #region CheckBoxOnGridHeader 全选按钮

        public void gridViewPatientList_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column != null && e.Column.Name == "col_selected")
            {
                e.Info.InnerElements.Clear();
                e.Painter.DrawObject(e.Info);
                DrawCheckBoxOnHeader(e.Graphics, e.Bounds, bGridHeaderCheckBoxState);

                e.Handled = true;
            }
        }

        DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit gridheadercheckbox;// = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
        private void DrawCheckBoxOnHeader(Graphics g, Rectangle r, bool check)
        {
            DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo info;
            DevExpress.XtraEditors.Drawing.CheckEditPainter painter;
            DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs args;

            info = gridheadercheckbox.CreateViewInfo() as DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo;
            painter = gridheadercheckbox.CreatePainter() as DevExpress.XtraEditors.Drawing.CheckEditPainter;
            info.EditValue = check;
            info.Bounds = r;
            info.CalcViewInfo(g);
            args = new DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs(info, new DevExpress.Utils.Drawing.GraphicsCache(g), r);
            painter.Draw(args);
            args.Cache.Dispose();

        }

        private bool bGridHeaderCheckBoxState = false;
        protected virtual void gridControl1_MouseDown(object sender, MouseEventArgs e)
        {
            Point pt = this.gridViewSingle.GridControl.PointToClient(Control.MousePosition);
            GridHitInfo info = this.gridViewSingle.CalcHitInfo(pt);
            if (info.InColumn && info.Column.Name == "col_selected")
            {
                bGridHeaderCheckBoxState = !bGridHeaderCheckBoxState;
                this.gridViewSingle.InvalidateColumnHeader(this.gridViewSingle.Columns["col_selected"]);
                SelectAllPatientInGrid(bGridHeaderCheckBoxState);
            }
        }

        private void SelectAllPatientInGrid(bool selectAll)
        {
            for (int i = 0; i < this.gridViewSingle.RowCount; i++)
            {
                ((EntityPidReportMain)this.gridViewSingle.GetRow(i)).PatSelect = bGridHeaderCheckBoxState;
            }
            gridControlSingle.RefreshDataSource();
        }

        void repositoryItemCheckEdit1_Click(object sender, System.EventArgs e)
        {
            Point pt = this.gridViewSingle.GridControl.PointToClient(Control.MousePosition);
            GridHitInfo info = this.gridViewSingle.CalcHitInfo(pt);
            if (info.Column != null && info.Column.Name == "col_selected" && info.InRow && info.InRowCell)
            {
                ((EntityPidReportMain)this.gridViewSingle.GetFocusedRow()).PatSelect = true;
            }
            gridControlSingle.RefreshDataSource();
        }

        #endregion


        /// <summary>
        /// 条码文本框按下回车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBarCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue != 13 || string.IsNullOrEmpty(this.txtBarCode.Text))
                return;

            executeTimes++; //新增

            if (2 * txtBarCode.Text.Trim().Length == Encoding.Default.GetByteCount(txtBarCode.Text.Trim()))
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("请勿使用全角输入！");
                return;
            }

            if ((this.txtType.valueMember == null || this.txtType.valueMember.Trim() == string.Empty))
            {
                lis.client.control.MessageDialog.Show("请选择物理组", "提示");
                this.ActiveControl = this.txtType;
                this.txtType.Focus();
                return;
            }

            if ((this.txtInstructment.valueMember == null || this.txtInstructment.valueMember.Trim() == string.Empty) && rgType.SelectedIndex == 0 && !ckIns.Checked)//
            {
                lis.client.control.MessageDialog.Show("请选择仪器", "提示");
                this.ActiveControl = this.txtInstructment;
                this.txtInstructment.Focus();
                return;
            }

            if (this.txtSid.Text.Trim() == string.Empty
                && rgType.SelectedIndex == 0 && !ckIns.Checked)//录入的条码号为空
            {
                lis.client.control.MessageDialog.Show(string.Format("{0}为空,不能正确读取条形码信息", NoticeName), "提示");
                this.txtSid.Focus();
                this.ActiveControl = this.txtSid;
                return;
            }

            if (!chkYhsBarCode.Checked)
            {
                //判断医嘱状态（收费状态）
                EntitySampQC sampQC = new EntitySampQC();
                sampQC.ListSampBarId = new List<string> { txtBarCode.Text.Trim() };
                List<EntitySampMain> list = new ProxySampMain().Service.SampMainQuery(sampQC);
                if (list == null || list.Count <= 0)
                {
                    lis.client.control.MessageDialog.Show("无此条形码的信息,请与临床科联系", "提示");
                    return;
                }
                else
                {
                    EntitySampOperation sign = new EntitySampOperation();
                    sign.OperationStatus = "20";
                    sign.OperationTime = IStep.GetServerTime();
                    ProxySampMain proxy = new ProxySampMain();
                    string strResultMsg = proxy.Service.ConfirmBeforeCheck(sign, list[0]);
                    if (!string.IsNullOrEmpty(strResultMsg))
                    {
                        lis.client.control.MessageDialog.Show(strResultMsg, "提示");
                        return;
                    }
                }

            }


            ProxyPidReportMain proxyPatient = new ProxyPidReportMain();

            if (chkYhsBarCode.Checked)
            {
                EntitySampQC sampQC1 = new EntitySampQC();
                sampQC1.SampYhsBarCode = txtBarCode.Text.Trim();

                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();//开始计时

                ProxySampMain proxySampMain = new ProxySampMain();
                //通过总码先获取各明细条码信息，再逐一转换成患者信息进行登记
                //签收总码再登记
                List<EntitySampMain> sampleList = proxySampMain.Service.SampMainQuery(sampQC1);

                if (sampleList == null || sampleList.Count == 0)
                {
                    //不进行签收，直接登记上机
                    List<string> BarCodeList = new List<string>();
                    BarCodeList.Add(txtBarCode.Text.Trim());             
                    sampleList = proxySampMain.Service.GetSampleInfo(BarCodeList);
                }
                            
                MessageDialog.ShowAutoCloseDialog("患者数量:" + sampleList.Count.ToString(), 2m);

                sampleList = sampleList.OrderBy(m => m.SampBarCode).ToList();
                #region
                if (sampleList != null && sampleList.Count > 0)
                {
                    string defaultSampleState = UserInfo.GetSysConfigValue("DefaultSampleState");

                    foreach (var pat in sampleList)
                    {
                        EntityPidReportMain patient = proxyPatient.Service.GetPatientsByBarCode(pat.SampBarCode);

                        patient.PidRemark = defaultSampleState;
                        if (patient == null || string.IsNullOrEmpty(patient.RepBarCode))
                        {
                            lis.client.control.MessageDialog.Show("无此条形码的信息,请与临床科联系", "提示");

                            this.txtBarCode.Text = string.Empty;
                            this.ActiveControl = this.txtBarCode;
                            this.txtBarCode.Focus();
                        }
                        else
                        {
                            #region 判断该条码是否可以使用当前仪器
                            //List<EntityDicInstrument> Instruments = proxyPatient.Service.GetAllItrForBarCode(pat.SampBarCode);
                            //if (Instruments.Count == 0)
                            //{
                            //    lis.client.control.MessageDialog.Show("此条形码暂无仪器可供录入，请检查！", "提示");
                            //    return;
                            //}
                            //var a = from x in Instruments where x.ItrId == this.txtInstructment.valueMember select x;
                            //if (a.Count() == 0)
                            //{
                            //    string tip = "此条形码无法在该仪器录入，此条形码可用的仪器为：\n";
                            //    foreach (EntityDicInstrument Instrument in Instruments)
                            //    {
                            //        tip += Instrument.ItrName + "、";
                            //    }
                            //    lis.client.control.MessageDialog.Show(tip.TrimEnd('、'), "提示");
                            //    return;
                            //}
                            #endregion

                            this.bsPatInfo.DataSource = patient;

                            if (!chkBarcodeCanRepeat.Checked)
                            {
                                patient.ListPidReportDetail = patient.ListPidReportDetail.FindAll(w => w.SampFlag == 0);
                            }

                            if (patient.ListPidReportDetail.Count == 0)
                            {
                                EntityPatientQC qc = new EntityPatientQC();
                                qc.RepBarCode = this.txtBarCode.Text;
                                //List<EntityPidReportMain> listPidReport = new ProxyPidReportMain().Service.PatientQuery(qc);
                                string mes = "此条码已登记！";
                                //if (listPidReport != null && listPidReport.Count > 0)
                                //{
                                //    mes = string.Format("该条码在 {1} 时 在仪器 {0} 录入,样本号:{2},序号:{3},操作人:{4} \r\n",
                                //         listPidReport[0].ItrName, listPidReport[0].RepInDate, listPidReport[0].RepSid, listPidReport[0].RepSerialNum,
                                //         string.IsNullOrEmpty(listPidReport[0].LrName) ? listPidReport[0].RepCheckUserId : listPidReport[0].LrName);
                                //}
                                MessageDialog.Show(mes, "提示");
                                ClearAndSetBarcodeInputFocus();
                            }
                            else
                            {
                                string strSave = Save(patient);
                                if (strSave != string.Empty)
                                {
                                    statBarcodeCount++;//登记一条数据
                                    string pat_id = strSave;
                                    if (rgType.SelectedIndex == 1 && !ckOrder.Checked)
                                    {
                                        this.txtSid.Text = string.Empty;
                                    }
                                }
                            }
                        }
                    }

                }

                #endregion

                #region 
                //List<EntityPidReportMain> patientLIst = proxyPatient.Service.GetPatientsBySampleMain(sampleList);
                //if (patientLIst != null && patientLIst.Count > 0)
                //{
                //    foreach (var patient in patientLIst)
                //    {
                //        string defaultSampleState = UserInfo.GetSysConfigValue("DefaultSampleState");
                //        patient.PidRemark = defaultSampleState;
                //        if (patient == null || string.IsNullOrEmpty(patient.RepBarCode))
                //        {
                //            lis.client.control.MessageDialog.Show("无此条形码的信息,请与临床科联系", "提示");

                //            this.txtBarCode.Text = string.Empty;
                //            this.ActiveControl = this.txtBarCode;
                //            this.txtBarCode.Focus();
                //        }
                //        else
                //        {
                //            this.bsPatInfo.DataSource = patient;

                //            if (!chkBarcodeCanRepeat.Checked)
                //            {
                //                patient.ListPidReportDetail = patient.ListPidReportDetail.FindAll(w => w.SampFlag == 0);
                //            }

                //            if (patient.ListPidReportDetail.Count == 0)
                //            {
                //                EntityPatientQC qc = new EntityPatientQC();
                //                qc.RepBarCode = patient.RepBarCode;
                //                List<EntityPidReportMain> listPidReport = new ProxyPidReportMain().Service.PatientQuery(qc);
                //                string mes = "此条码已登记！";
                //                if (listPidReport != null && listPidReport.Count > 0)
                //                {
                //                    mes = string.Format("该条码在 {1} 时 在仪器 {0} 录入,样本号:{2},序号:{3},操作人:{4} \r\n",
                //                         listPidReport[0].ItrName, listPidReport[0].RepInDate, listPidReport[0].RepSid, listPidReport[0].RepSerialNum,
                //                         string.IsNullOrEmpty(listPidReport[0].LrName) ? listPidReport[0].RepCheckUserId : listPidReport[0].LrName);
                //                }
                //                MessageDialog.Show(mes, "提示");
                //                ClearAndSetBarcodeInputFocus();
                //            }
                //            else
                //            {
                //                string strSave = Save(patient);
                //                if (strSave != string.Empty)
                //                {
                //                    statBarcodeCount++;//登记一条数据
                //                    string pat_id = strSave;
                //                    if (rgType.SelectedIndex == 1 && !ckOrder.Checked)
                //                    {
                //                        this.txtSid.Text = string.Empty;
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                #endregion
                sw.Stop();
                Logger.WriteLine("登记查询","核酸总码保存患者信息时间：" + sw.Elapsed.ToString(@"hh\:mm\:ss"));

            }

            else
            {

                System.Diagnostics.Stopwatch sw1 = new System.Diagnostics.Stopwatch();
                sw1.Start();//开始计时

                EntityPidReportMain patient = proxyPatient.Service.GetPatientsByBarCode(txtBarCode.Text);

                string defaultSampleState = UserInfo.GetSysConfigValue("DefaultSampleState");
                patient.PidRemark = defaultSampleState;
                if (patient == null || string.IsNullOrEmpty(patient.RepBarCode))
                {
                    lis.client.control.MessageDialog.Show("无此条形码的信息,请与临床科联系", "提示");

                    this.txtBarCode.Text = string.Empty;
                    this.ActiveControl = this.txtBarCode;
                    this.txtBarCode.Focus();
                }
                else
                {
                    #region 判断该条码是否可以使用当前仪器
                    List<EntityDicInstrument> Instruments = proxyPatient.Service.GetAllItrForBarCode(txtBarCode.Text);
                    if (Instruments.Count == 0)
                    {
                        lis.client.control.MessageDialog.Show("此条形码暂无仪器可供录入，请检查！", "提示");
                        return;
                    }
                    var a = from x in Instruments where x.ItrId == this.txtInstructment.valueMember select x;
                    if (a.Count() == 0)
                    {
                        string tip = "此条形码无法在该仪器录入，此条形码可用的仪器为：\n";
                        foreach (EntityDicInstrument Instrument in Instruments)
                        {
                            tip += Instrument.ItrName + "、";
                        }
                        lis.client.control.MessageDialog.Show(tip.TrimEnd('、'), "提示");
                        return;
                    }
                    #endregion

                    string status = patient.BcStatus;
                    if (status == "9")
                    {
                        /**********提示信息增加操作者和时间*******************************************/
                        string name = string.Empty;
                        string time = string.Empty;
                        string remark = string.Empty;

                        ProxySampProcessDetail proxyProcess = new ProxySampProcessDetail();
                        EntitySampProcessDetail process = proxyProcess.Service.GetLastSampProcessDetail(patient.RepBarCode);

                        if (process != null && !string.IsNullOrEmpty(process.ProcBarno))
                        {
                            name = process.ProcUsername;
                            time = process.ProcDate.ToString();
                            remark = process.ProcContent;
                        }

                        MessageDialog.Show(string.Format("此条码已回退，不允许录入！\r\n{2}\r\n 操作者：{0},时间：{1}", name, time, remark), "提示");
                        ClearAndSetBarcodeInputFocus();
                        return;
                    }


                    this.bsPatInfo.DataSource = patient;
                    if (patient.BcStatus == "0"
                      || patient.BcStatus == "1"
                      || patient.BcStatus == "2"
                      || patient.BcStatus == "3"
                      || patient.BcStatus == "4"
                      || patient.BcStatus == "8"
                      || patient.BcStatus == "9"
                      || patient.BcStatus == "500"
                      || patient.BcStatus == "510"
                        )
                    {
                        if (patient.PidSrcId == "108" && UserInfo.GetSysConfigValue("Barcode_ZYShouldReceiveConfirm") == "是")//住院
                        {
                            MessageDialog.Show("当前条码未签收！", MessageBoxButtons.OK, MessageBoxDefaultButton.Button3);
                            ClearAndSetBarcodeInputFocus();
                            return;
                        }

                        if (patient.PidSrcId == "109" && UserInfo.GetSysConfigValue("Barcode_TJShouldReceiveConfirm") == "是")//住院
                        {
                            MessageDialog.Show("当前条码未签收！", MessageBoxButtons.OK, MessageBoxDefaultButton.Button3);
                            ClearAndSetBarcodeInputFocus();
                            return;
                        }
                        if (patient.PidSrcId == "107" && UserInfo.GetSysConfigValue("Barcode_MZShouldReceiveConfirm") == "是")//住院
                        {
                            MessageDialog.Show("当前条码未签收！", MessageBoxButtons.OK, MessageBoxDefaultButton.Button3);
                            ClearAndSetBarcodeInputFocus();
                            return;
                        }
                    }

                    if (CheckReceiveTimeAndPatdate)
                    {
                        DateTime recvTime;
                        DateTime jyTime = ServerDateTime.GetServerDateTime();

                        if (patient.SampReceiveDate != null)
                        {
                            recvTime = patient.SampReceiveDate.Value;
                            if ((recvTime > jyTime)
                                ||
                                (DateTime.TryParse(dateCheck.EditValue.ToString(), out jyTime) &&
                                 recvTime > jyTime))
                            {
                                MessageDialog.Show("签收时间大于检验时间，请检查！", "提示");
                                ClearAndSetBarcodeInputFocus();
                                return;
                            }
                        }

                    }

                    if (!chkBarcodeCanRepeat.Checked)
                    {
                        patient.ListPidReportDetail = patient.ListPidReportDetail.FindAll(w => w.SampFlag == 0);
                    }

                    if (patient.ListPidReportDetail.Count == 0)
                    {
                        EntityPatientQC qc = new EntityPatientQC();
                        qc.RepBarCode = this.txtBarCode.Text;
                        List<EntityPidReportMain> listPidReport = new ProxyPidReportMain().Service.PatientQuery(qc);
                        string mes = "此条码已登记！";
                        if (listPidReport != null && listPidReport.Count > 0)
                        {
                            mes = string.Format("该条码在 {1} 时 在仪器 {0} 录入,样本号:{2},序号:{3},操作人:{4} \r\n",
                                 listPidReport[0].ItrName, listPidReport[0].RepInDate, listPidReport[0].RepSid, listPidReport[0].RepSerialNum,
                                 string.IsNullOrEmpty(listPidReport[0].LrName) ? listPidReport[0].RepCheckUserId : listPidReport[0].LrName);
                        }
                        MessageDialog.Show(mes, "提示");
                        ClearAndSetBarcodeInputFocus();
                    }
                    else
                    {

                        string strSave = Save(patient);
                        if (strSave != string.Empty)
                        {
                            statBarcodeCount++;//登记一条数据
                            string pat_id = strSave;
                            if (rgType.SelectedIndex == 1 && !ckOrder.Checked)
                            {
                                this.txtSid.Text = string.Empty;
                            }

                            ////自动打印细菌申请单
                            //if (chkAutoPrint.Checked)
                            //{
                            //    List<string> listRepId = new List<string>();
                            //    listRepId.Add(strSave);
                            //    this.PrintBacApply(listRepId);
                            //}
                        }

                    }
                }


                sw1.Stop();
                Logger.WriteLine("登记查询", "普通总码保存患者信息时间：" + sw1.Elapsed.ToString(@"hh\:mm\:ss"));

            }

        }

        private bool LocatePatientByBarcode(string pat_id)
        {
            this.gridViewSingle.SelectAll();

            bool founded = false;
            for (int i = 0; i < this.gridViewSingle.RowCount; i++)
            {
                EntityPidReportMain dr = (EntityPidReportMain)this.gridViewSingle.GetRow(i);

                if (!string.IsNullOrEmpty(dr.RepId))
                {
                    if (pat_id == dr.RepId)
                    {
                        this.gridViewSingle.FocusedRowHandle = i;
                        founded = true;
                    }
                    else
                    {
                        this.gridViewSingle.UnselectRow(i);
                    }
                }
            }

            return founded;
        }

        /// <summary>
        /// 保存
        /// </summary>
        private string Save(EntityPidReportMain patInfo)
        {
            string pat_id = string.Empty;

            //获取界面控件值
            string pat_itr_id = this.txtInstructment.valueMember;
            DateTime pat_date = (DateTime)txtPatDate.EditValue;
            string pat_sid = this.txtSid.Text;
            if (pat_date < Convert.ToDateTime("1900-01-01"))
            {
                MessageDialog.Show(string.Format("登记时间{0}有误！", pat_date), "提示");
                return "";
            }
            //填充到DataTable
            patInfo.RepInDate = pat_date;
            patInfo.RepItrId = pat_itr_id;
            patInfo.ItrName = this.txtInstructment.displayMember;

            //检验者
            if (Lab_SignInShowInspetor && !string.IsNullOrEmpty(txtPatInspetor.valueMember))
            {
                patInfo.RepCheckUserId = txtPatInspetor.valueMember;
            }
            else
            {
                patInfo.RepCheckUserId = UserInfo.loginID;
            }

            //按样本号或序号
            if (rgType.SelectedIndex == 0) //按样本号
            {
                patInfo.RepSid = pat_sid;
            }
            else//如果是序号,条码号当成样本号保存
            {
                if (this.txtSid.Text.Trim() == string.Empty
                    && rgType.SelectedIndex == 1)
                {
                    patInfo.RepSerialNum = new ProxyPidReportMain().Service.GetItrHostOrder_MaxPlusOne((DateTime)txtPatDate.EditValue, this.txtInstructment.valueMember);
                }
                else
                {
                    patInfo.RepSerialNum = pat_sid;
                }
                //************************************************************************
                //增加配置，假若样本号等于序号
                if (ConfigHelper.GetSysConfigValueWithoutLogin("BidirectionalRegisterSidEqualSeqID") == "否")
                {
                    //条码录入按序号录入时样本号左边去零 
                    if (Lab_BarRegRemoveZero)
                    {
                        patInfo.RepSid = txtBarCode.Text.Trim().TrimStart('0'); //条码号当成样本号保存

                    }
                    else
                    {
                        patInfo.RepSid = txtBarCode.Text.Trim(); //条码号当成样本号保存

                    }
                }
                else
                {
                    patInfo.RepSid = patInfo.RepSerialNum;
                }
                //************************************************************************
            }


            #region 条码录入时时间计算方式--清远人医
            //系统配置：条码录入时时间计算方式
            string Lab_BarcodeTimeCal = ConfigHelper.GetSysConfigValueWithoutLogin("Lab_BarcodeTimeCal");

            if (!string.IsNullOrEmpty(Lab_BarcodeTimeCal) && Lab_BarcodeTimeCal == "清远人医")
            {
                //如果送检时间（收取确认时间pat_sdate）为空，则取接收时间作为送检时间（收取确认时间）;
                //如果接收时间(bc_receiver_date)也为空，则取检验登记时间(pat_date)作为送检时间（收取确认时间pat_sdate）
                if (string.IsNullOrEmpty(patInfo.SampSendDate.ToString()))
                {
                    if (patInfo.SampCheckDate != null)
                    {
                        patInfo.SampSendDate = patInfo.SampCheckDate;
                    }
                    else
                    {
                        patInfo.SampSendDate = patInfo.RepInDate;
                    }
                }
                else
                {
                    DateTime temp_pat_sample_receive_date = DateTime.MinValue;//申请时间
                    DateTime temp_pat_sample_date = DateTime.MinValue;//采样时间
                    DateTime temp_pat_reach_date = DateTime.MinValue;//送达时间
                    DateTime temp_pat_sdate = Convert.ToDateTime(patInfo.SampSendDate.ToString());//送检时间
                    DateTime temp_pat_apply_date = DateTime.MinValue;//接收时间(签收)
                    DateTime temp_pat_jy_date = DateTime.MinValue;//检验时间

                    if (patInfo.SampReceiveDate != null)
                    {
                        temp_pat_sample_receive_date = patInfo.SampReceiveDate.Value;
                    }
                    if (patInfo.SampCollectionDate != null)
                    {
                        temp_pat_sample_date = patInfo.SampCollectionDate.Value;
                    }
                    if (patInfo.SampReachDate != null)
                    {
                        temp_pat_reach_date = patInfo.SampReachDate.Value;
                    }
                    if (patInfo.SampApplyDate != null)
                    {
                        temp_pat_apply_date = patInfo.SampApplyDate.Value;
                    }
                    if (patInfo.SampCheckDate != null)
                    {
                        temp_pat_jy_date = patInfo.SampCheckDate.Value;
                    }

                    //如果送检时间,小于等于（申请时间，采样时间）
                    if (temp_pat_sdate <= temp_pat_sample_receive_date
                        || temp_pat_sdate <= temp_pat_sample_date)
                    {
                        if (temp_pat_reach_date > DateTime.MinValue)
                        {
                            //如果送达时间不为空，则送检时间等于送达时间
                            patInfo.SampSendDate = patInfo.SampReachDate;
                        }
                        else if (temp_pat_apply_date > DateTime.MinValue)
                        {
                            //如果签收时间不为空，则送检时间等于签收时间
                            patInfo.SampSendDate = patInfo.SampApplyDate;
                        }
                        else if (temp_pat_jy_date > DateTime.MinValue)
                        {
                            //如果检验时间不为空，则送检时间等于检验时间
                            patInfo.SampSendDate = patInfo.SampCheckDate;
                        }
                        else
                        {
                            patInfo.SampSendDate = patInfo.RepInDate;
                        }
                    }
                    else if (temp_pat_reach_date > DateTime.MinValue && temp_pat_sdate > temp_pat_reach_date)
                    {
                        //如果送检时间大等于送达时间,则送检时间等于送达时间
                        patInfo.SampSendDate = patInfo.SampReachDate;
                    }
                    else if (temp_pat_apply_date > DateTime.MinValue && temp_pat_sdate > temp_pat_apply_date)
                    {
                        //如果送检时间大等于签收时间,则送检时间等于签收时间
                        patInfo.SampSendDate = patInfo.SampApplyDate;
                    }
                    else if (temp_pat_jy_date > DateTime.MinValue && temp_pat_sdate > temp_pat_jy_date)
                    {
                        //如果送检时间大等于检验时间,则送检时间等于检验时间
                        patInfo.SampSendDate = patInfo.SampCheckDate;
                    }
                }

            }
            #endregion


            if (cbManualIntr.Checked)
            {
                patInfo.RepItrId = txtInstructment.valueMember;
                patInfo.ItrName = txtInstructment.displayMember;
            }
            else
            {
                #region 根据组合所在的仪器自动分配仪器
                //根据组合所在的仪器自动分配仪器
                IList<string> combineIDs = new List<string>();
                foreach (EntityPidReportDetail row in patInfo.ListPidReportDetail)
                {
                    if (!combineIDs.Contains(row.ComId))
                        combineIDs.Add(row.ComId);
                }

                if (combineIDs.Count == 0)
                {
                    lis.client.control.MessageDialog.Show("组合找不到仪器！", "提示");
                    return string.Empty;
                }

                List<EntityDicItrCombine> listInsCom = CacheClient.GetCache<EntityDicItrCombine>();

                List<EntityDicInstrument> listIns = CacheClient.GetCache<EntityDicInstrument>();

                List<EntityDicInstrument> dsInstrments = listIns.FindAll(z => listInsCom.FindIndex(
                                                                         w => combineIDs.Contains(w.ComId) &&
                                                                         w.ItrId == z.ItrId) >= 0);

                if (dsInstrments != null && dsInstrments.Count > 0)
                {
                    List<EntityDicInstrument> dtInstrumentType = new List<EntityDicInstrument>();//用于存放过滤物理组的仪器
                    string typeId = txtType.valueMember;//物理组别
                    foreach (EntityDicInstrument drIns in dsInstrments)
                    {
                        if (drIns.ItrLabId == typeId)//判断仪器是否属于该物理组
                        {
                            if (ckIns.Checked)
                            {
                                dtInstrumentType.Add((EntityDicInstrument)drIns.Clone());
                            }
                            else//按序号判断为双向仪器
                            {
                                if (drIns.ItrCommType == 2)
                                {
                                    dtInstrumentType.Add((EntityDicInstrument)drIns.Clone());
                                }
                            }
                        }
                    }
                    if (dtInstrumentType.Count == 0)//没有此物理组的仪器
                    {
                        lis.client.control.MessageDialog.Show("当前条码组合不能在此物理组登记!");
                        return string.Empty;
                    }
                    else if (dtInstrumentType.Count == 1)//属于此物理组的仪器只有一台
                    {
                        patInfo.RepItrId = dtInstrumentType[0].ItrId;
                        patInfo.ItrName = dtInstrumentType[0].ItrEname;
                        //txtItr2.valueMember = dtInstrumentType.Rows[0]["Id"].ToString();
                        lblItr.Text = dtInstrumentType[0].ItrEname;
                    }
                    else//列出列表给予用户选择
                    {
                        int itrCount = 0;
                        bool isItrAll = false;
                        string itrComId = "";
                        string itrComName = "";

                        #region 处理组合是否全部在某个仪器当中，这样的仪器有几台
                        StringBuilder comId = new StringBuilder();
                        bool isFrist = false;
                        for (int i = 0; i < combineIDs.Count; i++)
                        {
                            if (isFrist)
                                comId.Append(",");
                            comId.Append(string.Format("'{0}'", combineIDs[i]));
                            isFrist = true;
                        }

                        foreach (EntityDicInstrument drInsType in dtInstrumentType)
                        {
                            string insId = drInsType.ItrId;//["Id"].ToString();

                            int insComLength = listInsCom.FindAll(w => w.ItrId == insId && combineIDs.Contains(w.ComId)).Count;
                            if (insComLength >= combineIDs.Count)
                            {
                                itrComId = insId;
                                itrComName = drInsType.ItrEname;// drInsType["Code"].ToString();
                                itrCount++;
                                isItrAll = true;
                            }

                        }
                        #endregion

                        if (!isItrAll || (isItrAll == true && itrCount > 1))
                        {
                            FrmSelectInstrument frmSelectInstrument = new FrmSelectInstrument();
                            frmSelectInstrument.Instrments = dtInstrumentType;
                            frmSelectInstrument.ShowDialogByData();
                            if (string.IsNullOrEmpty(frmSelectInstrument.SelectInstrumentID))
                            {
                                lis.client.control.MessageDialog.Show("无效仪器信息!");
                                return string.Empty;
                            }
                            patInfo.RepItrId = frmSelectInstrument.SelectInstrumentID;
                            patInfo.ItrName = frmSelectInstrument.SelectInstrumentName;
                        }
                        else
                        {
                            patInfo.RepItrId = itrComId;
                            patInfo.ItrName = itrComName;
                        }
                    }

                    if (Lab_CheckInstrmtAudit && patInfo.RepItrId != null && !UserInfo.isAdmin &&
                        !string.IsNullOrEmpty(patInfo.RepItrId.ToString()))
                    {
                        List<EntityUserInstrmt> listInstrmt = new ProxyUserManage().Service.GetUserCanMgrIInstrmt(
                                patInfo.RepItrId.ToString());
                        if (listInstrmt.Count > 0)
                        {
                            if (listInstrmt.Where(w => w.UserLoginid == UserInfo.loginID).Count() == 0)
                            {
                                lis.client.control.MessageDialog.Show(string.Format("您无操作仪器[{0}]的权限!", patInfo.RepItrId));
                                return string.Empty;
                            }
                        }
                    }

                }
                else
                {
                    lis.client.control.MessageDialog.Show("无法获取仪器信息!");
                    return string.Empty;
                }

                #endregion
            }

            if (txtSid.Text.Trim() == "" && txtInstructment.valueMember.Trim() == "")//如果序号未填并且未选仪器，默认取该组合所在仪器的最大序号
            {
                DateTime date = new DateTime();
                date = DateTime.Now;

                ProxyPidReportMain proxyMain = new ProxyPidReportMain();

                if (!ckIns.Checked)
                    patInfo.RepSerialNum = proxyMain.Service.GetItrHostOrder_MaxPlusOne(date, patInfo.RepItrId.ToString());
                else
                    patInfo.RepSid = proxyMain.Service.GetItrSID_MaxPlusOne(date, patInfo.RepItrId.ToString(), true);

            }


            List<EntityPidReportDetail> dtBCCombineCopy = new List<EntityPidReportDetail>();
            StringBuilder sbComIn = new StringBuilder();
            StringBuilder sbComNotIn = new StringBuilder();
            if (rgType.SelectedIndex == 0 && !ckCom.Checked && !ckIns.Checked)//此代码不判断仪器是否能在此仪器做，按样本号且判断组合不选中时不验证
            {
                dtBCCombineCopy = EntityManager<EntityPidReportDetail>.ListClone(patInfo.ListPidReportDetail);
            }
            else
            {
                #region 判断组合是否能在此仪器内做
                bool isFist = false;
                bool isNotInFist = false;
                List<string> comid = DictInstrmt.Instance.GetItrCombineID(patInfo.RepItrId.ToString(), true);

                foreach (EntityPidReportDetail patCom in patInfo.ListPidReportDetail)
                {
                    if (comid.Exists(i => i == patCom.ComId))
                    {
                        dtBCCombineCopy.Add(patCom);
                        if (isFist)
                            sbComIn.Append(",");

                        sbComIn.Append(patCom.PatComName);
                        isFist = true;
                    }
                    else
                    {
                        if (isNotInFist)
                            sbComNotIn.Append(",");

                        sbComNotIn.Append(patCom.PatComName);
                        isNotInFist = true;
                    }
                }
                #endregion
            }

            if (dtBCCombineCopy.Count == 0)
            {
                MessageDialog.Show("当前条码组合不能在此仪器登记！", "提示");
                ClearAndSetBarcodeInputFocus();
            }
            else
            {
                if (dtBCCombineCopy.Count != patInfo.ListPidReportDetail.Count && !CombineNotInItrNeedWaring)
                {
                    string strmes = sbComIn.ToString() + "可以在此仪器登记！\r\n";
                    if (!string.IsNullOrEmpty(sbComNotIn.ToString()))
                        strmes += sbComNotIn.ToString() + "组合不能在此仪器登记！";
                    if (lis.client.control.MessageDialog.Show(strmes, MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1) == DialogResult.No)
                        return string.Empty;

                    #region 多组合实现多次登记 2018-05-03
                    if (ckIns.Checked) //自选仪器(是否勾选)
                    {
                        if (executeTimes > 0 && executeTimes <= patInfo.ListPidReportDetail.Count)
                        {
                            for (; executeTimes + 1 <= patInfo.ListPidReportDetail.Count;)
                            {
                                int k = 0;
                                if (!chkBarcodeCanRepeat.Checked)
                                {
                                    k++;
                                    chkBarcodeCanRepeat.Checked = true;
                                }
                                KeyEventArgs e = new KeyEventArgs(Keys.Enter);
                                txtBarCode_KeyDown(null, e); //调用回车事件

                                if (k > 0)
                                    chkBarcodeCanRepeat.Checked = false;
                            }
                        }
                    }
                    #endregion

                }

                patInfo.ListPidReportDetail = dtBCCombineCopy;
                string patItrId = patInfo.RepItrId.ToString();
                string pat_com_id = dtBCCombineCopy[0].ComId;
                if (ckSID.Checked)
                {
                    int patSid = -1;
                    patInfo.RepSid = patSid != -1 ? patSid.ToString() : patInfo.RepSid;
                    txtSid.Text = (patSid != -1 ? patSid : 0 + 1).ToString();
                }

                EntityRemoteCallClientInfo caller = dcl.client.common.Util.ToCallerInfo();
                caller.Remarks = LocalSetting.Current.Setting.Description;

                ProxyPidReportMain proxyPatient = new ProxyPidReportMain();
                EntityOperateResult opResult = proxyPatient.Service.SavePatient(caller, patInfo);

                this.txtBarCode.Focus();
                this.ActiveControl = this.txtBarCode;

                if (opResult.Success)
                {
                    string barcode = this.txtBarCode.Text;
                    pat_id = opResult.Data.Patient.RepId;
                    this.txtBarCode.EditValue = string.Empty;
                    if (!ckIns.Checked)
                        SetSid(true);

                    EntityPidReportMain pat = proxyPatient.Service.GetPatientByPatId(pat_id, false);
                    bool result = new ProxyObrResult().Service.SaveOrignObrResult(pat);
                    //if (!result)
                    //{
                    //    string message;
                    //    if (pat.PidSrcId == "107")
                    //    {
                    //        message = string.Format("   门诊登记标本时，匹配结果源数据失败!,录入日期为： {0}, 仪器代码为：{1},   样本号为：{2}", pat.RepInDate, pat.RepItrId, pat.RepSid);
                    //        Lib.LogManager.Logger.LogInfo("报告单号：" + pat.RepId + message);
                    //    }
                    //    else if (pat.PidSrcId == "108")
                    //    {
                    //        message = string.Format("   住院登记标本时，匹配结果源数据失败!,录入日期为： {0}, 仪器代码为：{1},   样本号为：{2}", pat.RepInDate, pat.RepItrId, pat.RepSid);
                    //        Lib.LogManager.Logger.LogInfo("报告单号：" + pat.RepId + message);
                    //    }
                    //    else
                    //    {
                    //        message = string.Format("   登记标本时，病人类型不存在!,录入日期为： {0}, 仪器代码为：{1},   样本号为：{2}", pat.RepInDate, pat.RepItrId, pat.RepSid);
                    //        Lib.LogManager.Logger.LogInfo("报告单号：" + pat.RepId + message);
                    //    }
                    //}


                    if (BcReg_AfterInputNeedCheek)
                    {
                        pat.PatSelect = true;
                    }
                    listPatient.Add(pat);
                    ((List<EntityPidReportMain>)bsPatList.DataSource).Add(pat);
                    gridControlSingle.RefreshDataSource();
                    LocatePatientByBarcode(pat_id);
                }
                else
                {
                    if (opResult.Message.Count == 1
                        && opResult.Message.Find(a => a.ErrorCode == EnumOperateErrorCode.ChargeFall) != null)
                    {

                        this.txtBarCode.EditValue = string.Empty;
                        if (!ckIns.Checked)
                            SetSid(true);

                        EntityPidReportMain pat = proxyPatient.Service.GetPatientByPatId(pat_id, false);

                        if (BcReg_AfterInputNeedCheek)
                        {
                            pat.PatSelect = true;
                        }
                        listPatient.Add(pat);
                        ((List<EntityPidReportMain>)bsPatList.DataSource).Add(pat);
                        gridControlSingle.RefreshDataSource();
                        LocatePatientByBarcode(pat_id);
                        lis.client.control.MessageDialog.ShowAutoCloseDialog(OperationMessageHelper.GetErrorMessage(opResult.Message), 2);
                    }
                    else
                    {
                        if (opResult.HasExcaptionError)
                        {
                            lis.client.control.MessageDialog.Show("保存失败", "提示");
                        }
                        else
                        {
                            lis.client.control.MessageDialog.Show(
                                OperationMessageHelper.GetErrorMessage(opResult.Message), "提示");
                        }
                    }
                }

            }
            return pat_id;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void RefreshPatList()
        {
            if (this.txtInstructment.valueMember != null
                    && this.txtInstructment.valueMember.ToString() != string.Empty
                    && txtPatDate.EditValue != null
                    && txtPatDate.EditValue != DBNull.Value
                    )
            {
                DateTime date = (DateTime)txtPatDate.EditValue;

                EntityPatientQC patientQc = new EntityPatientQC();
                patientQc.ListItrId.Add(this.txtInstructment.valueMember);
                patientQc.DateStart = date.Date;
                patientQc.DateEnd = date.Date.AddDays(1).AddMilliseconds(-1);

                ProxyPidReportMain proxy = new ProxyPidReportMain();

                listPatient = proxy.Service.PatientQuery(patientQc);

                //如果流水号不为空则按照流水号来排序
                //listPatient = listPatient.OrderBy(w => !string.IsNullOrEmpty(w.RepSerialNum) ? Convert.ToInt64(w.RepSerialNum) : -1).ToList();

                this.bsPatList.DataSource = EntityManager<EntityPidReportMain>.ListClone(listPatient);// dtPats;
                this.gridViewSingle.MoveLast();
            }
        }
        private string ProId = string.Empty;
        /// <summary>
        /// 仪器改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void txtInstructment_ValueChanged(object sender, dcl.client.control.ValueChangeEventArgs args)
        {
            cbManualIntr.Checked = !string.IsNullOrEmpty(txtInstructment.valueMember);
            RefreshPatList();
            string ctype_id = DictInstrmt.Instance.GetItrCTypeID(this.txtInstructment.valueMember);

            if (!string.IsNullOrEmpty(ctype_id))
            {
                EntityDicPubProfession rowCType = DictType.Instance.GetCType(ctype_id);
                ProId = ctype_id;
                if (rowCType != null)
                {
                    this.txtType.valueMember = ctype_id;
                    this.txtType.displayMember = rowCType.ProName;
                    txtType_ValueChanged(null, null);
                }
            }
            if (SwitchOrder)
            {
                EntityDicInstrument drIns = txtInstructment.selectRow;
                if (drIns != null)
                {
                    rgType.SelectedIndexChanged -= new EventHandler(rgType_SelectedIndexChanged);
                    if (drIns.ItrCommType == 2)
                        rgType.SelectedIndex = 1;
                    else
                        rgType.SelectedIndex = 0;
                    ChangeColor();
                    SetSid(false);
                    rgType.SelectedIndexChanged += new EventHandler(rgType_SelectedIndexChanged);
                }

            }
            else if (!ckIns.Checked)
                SetSid(false);
        }

        /// <summary>
        /// 设置样本号
        /// </summary>
        /// <param name="isAutomatic">样本号是否自动增长</param>
        private void SetSid(bool isAutomatic)
        {
            lbSid.Text = NoticeName;
            DateTime date = (DateTime)txtPatDate.EditValue;

            if (rgType.SelectedIndex == 0 && this.txtInstructment.valueMember != null
                && this.txtInstructment.valueMember.ToString() != string.Empty
                && txtPatDate.EditValue != null
                && txtPatDate.EditValue != DBNull.Value
                )
            {
                setCheck(false);
                if (!isAutomatic)
                {
                    ProxyPidReportMain proxyMain = new ProxyPidReportMain();

                    string sid = proxyMain.Service.GetItrSID_MaxPlusOne(date, this.txtInstructment.valueMember, true);

                    this.txtSid.EditValue = sid;
                }
                else
                {
                    try
                    {
                        long sid = Convert.ToInt64(txtSid.EditValue);

                        this.txtSid.EditValue = sid + 1;
                    }
                    catch (Exception)
                    {
                        this.txtSid.EditValue = null;
                        MessageDialog.ShowAutoCloseDialog("样本号输入错误，无法自增！");
                    }
                }
            }
            else if (rgType.SelectedIndex == 1)
            {
                setCheck(true);
                if (ckOrder.Checked && txtSid.EditValue != null && txtSid.EditValue.ToString().Trim() != "")
                {
                    try
                    {
                        if (isAutomatic)
                        {
                            long sid = Convert.ToInt64(txtSid.EditValue);

                            this.txtSid.EditValue = sid + 1;
                        }
                        else
                            this.txtSid.EditValue = string.Empty;
                    }
                    catch (Exception)
                    {
                        this.txtSid.EditValue = null;
                        MessageDialog.ShowAutoCloseDialog("样本号输入错误，无法自增！");
                    }
                }
                else
                    txtSid.EditValue = null;
                //int result = 0;
                //if (int.TryParse(this.txtSid.Text, out result))
                //{
                //    this.txtSid.EditValue = PatientEnterClient.NewInstance.GetItrHostOrder_MaxPlusOne(date, this.txtInstructment.valueMember);// (result + 1).ToString();
                //}
            }
            else
                setCheck(false);

            #region 单向仪器不允许改变序号
            EntityDicInstrument drIns = txtInstructment.selectRow;
            if (drIns != null && txtInstructment.valueMember.Trim() != ""
                && drIns.ItrCommType == 1
                && rgType.SelectedIndex == 1)
            {
                MessageDialog.ShowAutoCloseDialog("此仪器为单向仪器，不能用序号扫条码！");
                rgType.SelectedIndex = 0;
            }
            #endregion
        }


        private void setCheck(bool isCheck)
        {
            ckCom.Enabled = !isCheck;
            ckOrder.Enabled = isCheck;
            ckIns.Checked = false;
            ckIns.Enabled = !isCheck;
            //ckOrder.Checked = isCheck;
        }

        /// <summary>
        /// 日期改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPatDate_EditValueChanged(object sender, EventArgs e)
        {
            RefreshPatList();
            SetSid(false);
        }

        private void btnPrintBarCode_Click(object sender, EventArgs e)
        {
            try
            {
                if (bsPatList.DataSource != null && ((List<EntityPidReportMain>)bsPatList.DataSource).Count > 0)
                {
                    string filter = bsPatList.Filter;

                    List<EntityPidReportMain> listPatient = ((List<EntityPidReportMain>)bsPatList.DataSource);
                    listPatient = listPatient.FindAll(w => w.PatSelect);

                    List<string> strInPatIds = new List<string>();
                    List<string> strOutPatIds = new List<string>();
                    List<string> strTjPatIds = new List<string>();
                    string username = "";
                    string userid = "";
                    if (listPatient.Count > 0)
                    {
                        FrmCheckPassword frm = new FrmCheckPassword();
                        if (frm.ShowDialog() != DialogResult.OK) return;
                        userid = frm.OperatorID;
                        username = frm.OperatorName;
                    }
                    else
                    {
                        lis.client.control.MessageDialog.Show("请选择要打印的数据！", "提示");
                        return;
                    }
                    List<EntityPidReportMain> dataRow = new List<EntityPidReportMain>();
                    foreach (EntityPidReportMain drPat in listPatient)
                    {
                        if (!string.IsNullOrEmpty(drPat.RepBarCode))
                        {
                            if (drPat.PidSrcId == "107")
                                strOutPatIds.Add(drPat.RepBarCode);
                            else if (drPat.PidSrcId == "108")
                                strInPatIds.Add(drPat.RepBarCode);
                            else
                                strTjPatIds.Add(drPat.RepBarCode);

                            dataRow.Add(drPat);
                        }
                    }

                    if (strInPatIds.Count == 0 && strOutPatIds.Count == 0 && strTjPatIds.Count == 0)
                    {
                        lis.client.control.MessageDialog.Show("选择的数据无条码号！", "提示");
                        return;
                    }

                    Dictionary<string, List<string>> idDic = new Dictionary<string, List<string>>();
                    if (strInPatIds.Count > 0)
                    {
                        idDic.Add("108", strInPatIds);
                    }
                    if (strOutPatIds.Count > 0)
                    {
                        idDic.Add("107", strOutPatIds);
                    }
                    if (strTjPatIds.Count > 0)
                    {
                        idDic.Add("109", strTjPatIds);
                    }

                    foreach (var item in idDic)
                    {
                        string strReportCode = string.Empty;

                        if (item.Key == "107")
                            strReportCode = ConfigHelper.GetSysConfigValueWithoutLogin("MZBarcodeReport");
                        else if (item.Key == "108")
                            strReportCode = ConfigHelper.GetSysConfigValueWithoutLogin("ZYBarcodeReport");
                        else
                            strReportCode = ConfigHelper.GetSysConfigValueWithoutLogin("TJBarcodeReport");

                        EntityDCLPrintParameter printParameter = new EntityDCLPrintParameter();
                        printParameter.ListBarId = item.Value;
                        printParameter.ReportCode = strReportCode;

                        DCLReportPrint.Print(printParameter);
                    }

                    ProxySampProcessDetail proxyProcess = new ProxySampProcessDetail();
                    DateTime currentServerTime = ServerDateTime.GetServerDateTime();
                    //获取需要更新的条码
                    foreach (EntityPidReportMain row in dataRow)
                    {
                        EntitySampProcessDetail sampProcess = new EntitySampProcessDetail();
                        sampProcess.ProcStatus = "20";
                        sampProcess.ProcStatusName = "资料登记";
                        sampProcess.ProcUsercode = userid;
                        sampProcess.ProcUsername = username;
                        sampProcess.ProcBarcode = row.RepBarCode;
                        sampProcess.ProcBarno = row.RepBarCode;
                        sampProcess.ProcContent = "标本登记条码打印,不更改标本最后状态。";
                        sampProcess.ProcDate = currentServerTime;

                        proxyProcess.Service.SaveSampProcessDetailWithoutInterface(sampProcess);
                    }

                    for (int i = 0; i < this.gridViewSingle.RowCount; i++)
                    {
                        ((EntityPidReportMain)this.gridViewSingle.GetRow(i)).PatSelect = false;
                    }
                }
                else
                {
                    lis.client.control.MessageDialog.Show("请选择要打印的数据！", "提示");
                    return;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("打印条码失败", ex);
            }
        }
        private void BtnExport_Click(object sender, EventArgs e)
        {
            this.gridViewSingle.CloseEditor();
            List<EntityPidReportMain> listPat = this.bsPatList.DataSource as List<EntityPidReportMain>;
            if (gridControlSingle.DataSource != null && listPat != null && listPat.Count > 0)
            {
                List<EntityPidReportMain> listSel = listPat.FindAll(w => w.PatSelect == true);
                if (listSel.Count <= 0)
                {
                    DialogResult dresult = MessageBox.Show("未勾选数据，是否导出当前全部数据? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dresult == DialogResult.Cancel)
                        return;
                    else
                    {
                        listSel = EntityManager<EntityPidReportMain>.ListClone(listPat);
                    }
                }

                SaveFileDialog ofd = new SaveFileDialog();
                ofd.DefaultExt = "xls";
                ofd.Filter = "Excel文件(*.xls)|*.xls";
                ofd.Title = "导出到Excel";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.Trim() == string.Empty)
                    {
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("文件名不能为空！");
                        return;
                    }

                    try
                    {
                        if (listSel.Count == listPat.Count)  //导出全部
                        {
                            gridControlSingle.ExportToXls(ofd.FileName.Trim()); //控件导出excel
                        }
                        else  //导出勾选
                        {
                            //暂默认转换成新库字段(如想实现新旧库根据web.config切换,参照客户端获取web.config中SystemType的值来实现)
                            DataTable dtExcelInfo = EntityManager<EntityPidReportMain>.ConvertToDataTable(listSel, "wf");
                            InsertDataToExcel2(ofd.FileName.Trim(), dtExcelInfo); //导出excel
                        }

                        lis.client.control.MessageDialog.ShowAutoCloseDialog("导出成功！");
                    }
                    catch (Exception ex)
                    {
                        Lib.LogManager.Logger.LogException("导出", ex);
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("导出失败！");
                    }
                }
            }
            else
            {

                if (string.IsNullOrEmpty(txtType.valueMember))
                {
                    lis.client.control.MessageDialog.Show("请选择实验组！", "提示");
                }
                else if (string.IsNullOrEmpty(txtInstructment.valueMember))
                {
                    lis.client.control.MessageDialog.Show("请选择仪器！", "提示");
                }
                else
                {
                    lis.client.control.MessageDialog.Show("列表没有数据！", "提示");
                }
            }
        }

        /// <summary>
        /// 导出excel(生成新文件)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="dtExcelInfo"></param>
        public void InsertDataToExcel2(string path, DataTable dtExcelInfo)
        {
            if (!string.IsNullOrEmpty(path) && dtExcelInfo != null)
            {
                try
                {
                    HSSFWorkbook book = new HSSFWorkbook();
                    ISheet sheet1 = book.CreateSheet("Sheet1");
                    sheet1.DefaultColumnWidth = 10;

                    List<string> listFieldNms = new List<string>(); //字段名称
                    List<string> listCaptions = new List<string>(); //显示名称

                    #region  获取字段名(默认用新库)及其对应的实体成员名
                    Dictionary<string, object> values = new Dictionary<string, object>();// Dictionary<"实体成员名称"，"新库字段名">

                    EntityPidReportMain entity = new EntityPidReportMain();
                    PropertyInfo[] propertys = entity.GetType().GetProperties();

                    foreach (PropertyInfo item in propertys)
                    {
                        var attribute = item.GetCustomAttributes(typeof(FieldMapAttribute), false).FirstOrDefault();
                        if (attribute != null)
                        {
                            FieldMapAttribute map = (FieldMapAttribute)attribute;
                            values.Add(item.Name, map.MedName); //values.Add(item.Name, map.ClabName);
                        }
                    }
                    #endregion

                    int noteTempStartIndex = 0; //记录开始添加行

                    for (int i = 1; i < gridViewSingle.Columns.Count; i++) //去除PatSelect
                    {
                        if (gridViewSingle.Columns[i].Visible)
                        {
                            if (!string.IsNullOrEmpty(gridViewSingle.Columns[i].Caption))
                            {
                                //添加显示名称
                                listCaptions.Add(gridViewSingle.Columns[i].Caption);
                                //添加字段名称
                                if (!string.IsNullOrEmpty(gridViewSingle.Columns[i].FieldName))
                                {
                                    if (gridViewSingle.Columns[i].FieldName == "PidSexName")
                                        listFieldNms.Add("pid_sex");
                                    foreach (var info in values)
                                    {
                                        if (info.Key == gridViewSingle.Columns[i].FieldName)
                                            listFieldNms.Add(info.Value.ToString());
                                    }
                                }
                            }
                        }
                    }

                    //添加显示列
                    if (listCaptions.Count > 0)
                    {
                        int tempRowNum = 0; //从第几行开始添加
                        if (noteTempStartIndex > 0)
                        {
                            tempRowNum = noteTempStartIndex + 2; //如果不是第一行,则间隔两行再添加
                            noteTempStartIndex = tempRowNum;
                        }
                        IRow row = sheet1.CreateRow(tempRowNum);
                        for (int j = 0; j < listCaptions.Count; j++)
                        {
                            row.CreateCell(j).SetCellValue(listCaptions[j]);
                        }
                    }

                    //给对应的列名赋值
                    for (int i = 0; i < dtExcelInfo.Rows.Count; i++)
                    {
                        noteTempStartIndex++;
                        IRow row = sheet1.CreateRow(noteTempStartIndex);
                        for (int j = 0; j < listFieldNms.Count; j++)
                        {
                            if (listFieldNms[j] == "pid_sex")
                            {
                                string patSex = dtExcelInfo.Rows[i][listFieldNms[j]].ToString();
                                string patSexName = string.Empty;
                                if (patSex == "1")
                                    patSexName = "男";
                                else if (patSex == "2")
                                    patSexName = "女";
                                else
                                    patSexName = "未知";

                                row.CreateCell(j).SetCellValue(patSexName);
                            }
                            else
                                row.CreateCell(j).SetCellValue(dtExcelInfo.Rows[i][listFieldNms[j]].ToString());
                        }
                    }

                    //写入文件
                    FileStream file = new FileStream(path, FileMode.Create);
                    book.Write(file);
                    file.Close();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    throw ex;
                }
            }
        }

        public string GetInsertValues(string stepCode, string loginID, string userName, string barCodeID, string place, string barcodeDisplayID, string remark, string signTime)
        {
            return string.Format(" '{0}', '{1}' , '{2}' ,'{3}', '{4}', '{5}' ,'{6}' ,{7} ,'{8}'",
                          loginID,
                          userName,
                          signTime,//GetServerTime().ToString(Constant.DateTimeLongFormat),
                          stepCode,
                          barCodeID,
                          place,
                          barCodeID, //TO-DO:需要分开预置条码
                          1,
                          remark
                          );
        }

        private void txtBarCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == 13)
            {
                this.txtBarCode.Focus();
                this.ActiveControl = this.txtBarCode;
            }
        }

        /// <summary>
        /// 点击"刷新"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            RefreshPatList();
            this.SetSid(false);
            this.txtBarCode.Text = string.Empty;
            this.ActiveControl = this.txtBarCode;
            this.txtBarCode.Focus();
        }

        private void SetBarcodeInputFocus()
        {
            this.ActiveControl = this.txtBarCode;
            this.txtBarCode.Focus();
        }

        private void ClearAndSetBarcodeInputFocus()
        {
            this.txtBarCode.Text = string.Empty;
            SetBarcodeInputFocus();
        }

        /// <summary>
        /// 病人索引改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bsPatList_PositionChanged(object sender, EventArgs e)
        {
            if (this.bsPatList.Current != null)
            {
                EntityPidReportMain drv = this.bsPatList.Current as EntityPidReportMain;

                this.bsPatInfo.DataSource = drv;
            }
        }

        /// <summary>
        /// 组别改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void txtType_ValueChanged(object sender, dcl.client.control.ValueChangeEventArgs args)
        {
            //清空仪器
            //this.txtInstructment.valueMember = string.Empty;
            //this.txtInstructment.displayMember = string.Empty;

            List<EntityDicInstrument> listIns = txtInstructment.getDataSource();

            string currentSelectType = this.txtType.valueMember;

            //是否有物理组
            if (currentSelectType != null && currentSelectType.Trim(null) != string.Empty)
            {
                //根据仪器数据类型过滤出当前物理组的仪器
                listIns = listIns.FindAll(w => w.ItrLabId == currentSelectType);
            }

            if ((Lab_HideNotReportInstrmt || Lab_CheckInstrmtAudit) && !UserInfo.isAdmin)
            {
                //非管理员：列出有权限的仪器
                if (UserInfo.sqlUserTypesFilter != string.Empty)
                {
                    listIns = listIns.FindAll(w => w.ItrLabId == currentSelectType && UserInfo.sqlUserItrs.Contains(w.ItrId));
                }
                else
                {
                    listIns = new List<EntityDicInstrument>();
                }
            }
            //当切换和不是当前仪器的实验组时 将仪器也置为空  防止出现仪器丢失问题
            if (currentSelectType != ProId)
            {
                this.txtInstructment.displayMember = "";
                this.txtInstructment.valueMember = "";
            }
            txtInstructment.SetFilter(listIns);
        }


        private void rgType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeColor();
            if (rgType.SelectedIndex == 1 && !cbBindingIns.Checked)
            {
                txtInstructment.valueMember = "";
                txtInstructment.displayMember = "";
            }
            SetSid(false);
        }

        private void ChangeColor()
        {
            if (rgType.SelectedIndex == 1) //样本号
            {
                barSave.BtnAnswer.Enabled = false;
            }
            else
            {
                barSave.BtnAnswer.Enabled = true;
            }

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            //系统配置：标本登记允许批量删除
            if (col_selected.Visible && ConfigHelper.GetSysConfigValueWithoutLogin("SampleRegister_Allow_batchDel") == "是")
            {
                #region 批量删除

                if (bsPatList.DataSource != null && ((List<EntityPidReportMain>)bsPatList.DataSource).Count > 0)
                {
                    txtPatDate.Focus();

                    List<EntityPidReportMain> listPatient = ((List<EntityPidReportMain>)bsPatList.DataSource);
                    listPatient = listPatient.FindAll(w => w.PatSelect);

                    if (listPatient.Count > 0)
                    {
                        List<string> Str_AllowDelPatID = new List<string>();//能删除的
                        List<string> Str_CheckDelPatID = new List<string>();//有结果确认要删除的
                        List<string> Str_CannotDelPatID = new List<string>();//不能删除的

                        Dictionary<string, string> patItrDic = new Dictionary<string, string>();

                        ProxyObrResult proxyResult = new ProxyObrResult();

                        foreach (EntityPidReportMain drPat in listPatient)
                        {
                            string pat_id = drPat.RepId;
                            int patFlag = drPat.RepStatus.Value;
                            if (!patItrDic.ContainsKey(pat_id))
                            {
                                patItrDic.Add(pat_id, drPat.RepItrId);
                            }
                            if (patFlag == 1)
                            {
                                Str_CannotDelPatID.Add(pat_id);
                                continue;
                            }
                            if (patFlag == 2)
                            {
                                Str_CannotDelPatID.Add(pat_id);
                                continue;
                            }
                            if (patFlag == 4)
                            {
                                Str_CannotDelPatID.Add(pat_id);
                                continue;
                            }

                            EntityResultQC resultQc = new EntityResultQC();
                            resultQc.ListObrId.Add(pat_id);

                            List<EntityObrResult> listResult = proxyResult.Service.ObrResultQuery(resultQc);

                            if (listResult != null && listResult.Count > 0)
                            {
                                Str_CheckDelPatID.Add(pat_id);
                                continue;
                            }

                            Str_AllowDelPatID.Add(pat_id);
                        }

                        if (Str_CannotDelPatID.Count > 0)
                        {
                            MessageDialog.ShowAutoCloseDialog("【" + Str_CannotDelPatID.Count.ToString() + "条】记录已一审/已二审/已打印，无法删除！");
                        }

                        if (Str_CheckDelPatID.Count > 0)
                        {
                            if (MessageBox.Show("【" + Str_CheckDelPatID.Count.ToString() + "条】记录已有结果，是否继续删除？否则自动排除", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                foreach (string str in Str_CheckDelPatID)
                                {
                                    Str_AllowDelPatID.Add(str);
                                }
                            }
                        }

                        if (Str_AllowDelPatID.Count > 0)
                        {
                            FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 删除", LIS_Const.BillPopedomCode.Delete, "", "");
                            DialogResult dig = frmCheck.ShowDialog();
                            if (dig == DialogResult.OK)
                            {
                                DialogResult delDiaResult = lis.client.control.MessageDialog.Show("是否连同病人结果一起删除？", "确认", MessageBoxButtons.YesNoCancel);
                                if (delDiaResult != DialogResult.Cancel)
                                {
                                    bool bDelResult = (delDiaResult == DialogResult.Yes);

                                    EntityRemoteCallClientInfo caller = dcl.client.common.Util.ToCallerInfo(frmCheck.OperatorID, string.Empty, frmCheck.OperatorName);

                                    int p_noteTrue = 0;
                                    int p_noteFalse = 0;

                                    foreach (string str_tempPatID in Str_AllowDelPatID)
                                    {
                                        EntityOperationResult opResult = proxyResult.Service.DelPatCommonResult(caller, str_tempPatID, bDelResult, false);

                                        if (opResult.Message.Count == 0)
                                        {
                                            p_noteTrue++;
                                        }
                                        else
                                        {
                                            p_noteFalse++;
                                        }
                                    }

                                    MessageDialog.ShowAutoCloseDialog("删除成功【" + p_noteTrue.ToString() + "条】 删除失败【" + p_noteFalse.ToString() + "条】");
                                    simpleButton1_Click(sender, e);//删除完刷新
                                }
                            }
                        }
                    }
                }

                #endregion
            }
            else
            {
                #region 单条删除
                if (bsPatList.Current != null)
                {
                    EntityPidReportMain drPat = (EntityPidReportMain)bsPatList.Current;
                    string pat_id = drPat.RepId;
                    string patFlag = drPat.RepStatus.ToString();
                    if (patFlag == "1")
                    {
                        MessageDialog.ShowAutoCloseDialog("该记录已" + LocalSetting.Current.Setting.AuditWord + "，无法删除！");
                        return;
                    }
                    if (patFlag == "2")
                    {
                        MessageDialog.ShowAutoCloseDialog("该记录已" + LocalSetting.Current.Setting.ReportWord + "，无法删除！");
                        return;
                    }
                    if (patFlag == "4")
                    {
                        MessageDialog.ShowAutoCloseDialog("该记录已打印，无法删除！");
                        return;
                    }

                    ProxyObrResult proxyResult = new ProxyObrResult();

                    EntityResultQC resultQc = new EntityResultQC();
                    resultQc.ListObrId.Add(pat_id);

                    List<EntityObrResult> listResult = proxyResult.Service.ObrResultQuery(resultQc);

                    if (listResult != null && listResult.Count > 0)
                    {
                        if (MessageBox.Show("该记录已有结果，是否删除？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                            return;
                    }

                    FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 删除", LIS_Const.BillPopedomCode.Delete, "", "");

                    DialogResult dig = frmCheck.ShowDialog();
                    if (dig == DialogResult.OK)
                    {
                        DialogResult delDiaResult = lis.client.control.MessageDialog.Show("是否连同病人结果一起删除？", "确认", MessageBoxButtons.YesNoCancel);
                        if (delDiaResult != DialogResult.Cancel)
                        {
                            bool bDelResult = (delDiaResult == DialogResult.Yes);

                            EntityRemoteCallClientInfo caller = dcl.client.common.Util.ToCallerInfo(frmCheck.OperatorID, string.Empty, frmCheck.OperatorName);

                            EntityOperationResult opResult = proxyResult.Service.DelPatCommonResult(caller, pat_id, bDelResult, false);

                            if (opResult.Message.Count == 0)
                            {
                                lis.client.control.MessageDialog.Show("删除成功", "提示");
                                ((List<EntityPidReportMain>)bsPatList.DataSource).Remove(drPat);
                                gridControlSingle.RefreshDataSource();
                            }
                            else
                            {
                                lis.client.control.MessageDialog.Show("删除失败", "提示");
                            }
                        }
                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// 跳空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGaps_Click(object sender, EventArgs e)
        {
            if ((this.txtType.valueMember == null || this.txtType.valueMember.Trim() == string.Empty))
            {
                lis.client.control.MessageDialog.Show("请选择物理组", "提示");
                this.ActiveControl = this.txtType;
                this.txtType.Focus();
                return;
            }

            if ((this.txtInstructment.valueMember == null || this.txtInstructment.valueMember.Trim() == string.Empty) && !ckIns.Checked)
            {
                lis.client.control.MessageDialog.Show("请选择仪器", "提示");
                this.ActiveControl = this.txtInstructment;
                this.txtInstructment.Focus();
                return;
            }

            if ((this.txtSid.EditValue == null || this.txtSid.EditValue.ToString().Trim() == string.Empty))
            {
                lis.client.control.MessageDialog.Show("请选择样本号", "提示");
                this.ActiveControl = this.txtSid;
                this.txtSid.Focus();
                return;
            }

            EntityPidReportMain drPat = new EntityPidReportMain(); ;
            drPat.RepItrId = txtInstructment.valueMember;
            drPat.ItrName = txtInstructment.displayMember;
            drPat.RepSid = txtSid.Text;
            drPat.RepInDate = dateCheck.DateTime;
            drPat.RepCheckUserId = UserInfo.loginID;
            drPat.SampCheckDate = dateCheck.DateTime;
            drPat.SampReceiveDate = DateTime.Now.AddSeconds(-4);
            drPat.SampSendDate = DateTime.Now.AddSeconds(-2);
            drPat.SampApplyDate = DateTime.Now.AddSeconds(-1);

            ProxyPidReportMain proxyPatient = new ProxyPidReportMain();
            EntityOperateResult opResult = proxyPatient.Service.SavePatient(dcl.client.common.Util.ToCallerInfo(), drPat);

            this.txtBarCode.Focus();
            this.ActiveControl = this.txtBarCode;

            if (opResult.Success)
            {
                this.txtBarCode.EditValue = string.Empty;
                SetSid(true);
                RefreshPatList();
            }
            else
            {
                if (opResult.HasExcaptionError)
                {
                    lis.client.control.MessageDialog.Show("保存失败", "提示");
                }
                else
                {
                    lis.client.control.MessageDialog.Show(OperationMessageHelper.GetErrorMessage(opResult.Message), "提示");
                }
            }
        }

        private void btnPrintQD_Click(object sender, EventArgs e)
        {
            if (bsPatList.DataSource != null && ((List<EntityPidReportMain>)bsPatList.DataSource).Count > 0)
            {
                txtPatDate.Focus();

                List<EntityPidReportMain> listPatient = ((List<EntityPidReportMain>)bsPatList.DataSource);
                listPatient = listPatient.FindAll(w => w.PatSelect);

                List<string> listRepId = new List<string>();
                foreach (EntityPidReportMain drPat in listPatient)
                {
                    listRepId.Add(drPat.RepId);
                }

                if (listRepId.Count == 0)
                {
                    lis.client.control.MessageDialog.Show("请选择要打印的数据！", "提示");
                    return;
                }

                EntityDCLPrintParameter printParameter = new EntityDCLPrintParameter();
                printParameter.ListRepId = listRepId;
                printParameter.ReportCode = "QDPrint";

                DCLReportPrint.Print(printParameter);

                for (int i = 0; i < this.gridViewSingle.RowCount; i++)
                {
                    ((EntityPidReportMain)this.gridViewSingle.GetRow(i)).PatSelect = false;
                }
                gridViewSingle.RefreshData();
            }
            else
            {
                lis.client.control.MessageDialog.Show("请选择要打印的数据！", "提示");
                return;
            }
        }
        private void btnPrintPreviewQD_Click(object sender, EventArgs e)
        {
            if (bsPatList.DataSource != null && ((List<EntityPidReportMain>)bsPatList.DataSource).Count > 0)
            {
                txtPatDate.Focus();

                List<EntityPidReportMain> listPatient = ((List<EntityPidReportMain>)bsPatList.DataSource);
                listPatient = listPatient.FindAll(w => w.PatSelect);

                List<string> listRepId = new List<string>();
                foreach (EntityPidReportMain drPat in listPatient)
                {
                    listRepId.Add(drPat.RepId);
                }

                if (listRepId.Count == 0)
                {
                    lis.client.control.MessageDialog.Show("请选择要预览的数据！", "提示");
                    return;
                }

                EntityDCLPrintParameter printParameter = new EntityDCLPrintParameter();
                printParameter.ListRepId = listRepId;
                printParameter.ReportCode = "QDPrint";

                DCLReportPrint.PrintPreview(printParameter);

                for (int i = 0; i < this.gridViewSingle.RowCount; i++)
                {
                    ((EntityPidReportMain)this.gridViewSingle.GetRow(i)).PatSelect = false;
                }
                gridViewSingle.RefreshData();
            }
            else
            {
                lis.client.control.MessageDialog.Show("请选择要预览的数据！", "提示");
                return;
            }
        }
        private void ckIns_CheckedChanged(object sender, EventArgs e)
        {
            ckCom.Checked = false;
            ckCom.Enabled = !ckIns.Checked;
            cbBindingIns.Checked = false;
            cbBindingIns.Enabled = !ckIns.Checked;
            txtInstructment.Readonly = ckIns.Checked;
            txtSid.Properties.ReadOnly = ckIns.Checked;
            if (ckIns.Checked)
            {
                txtInstructment.valueMember = "";
                txtInstructment.displayMember = "";
                txtSid.Text = "";
            }
        }

        private void txtSid_Leave(object sender, EventArgs e)
        {
            if (txtSid.Text.Trim() == string.Empty)
                return;

            long testSampleID;
            if (!long.TryParse(this.txtSid.Text, out testSampleID))
            {
                MessageDialog.Show("输入的样本号/序号不正确，请确保为半角数字");
                txtSid.Text = string.Empty;
                return;
            }

            char[] c = txtSid.Text.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (IsCharDBC(c[i]))
                {
                    MessageDialog.Show("输入的样本号/序号不正确，请确保为半角数字");
                    txtSid.Text = string.Empty;
                    txtSid.Focus();
                    return;
                }
            }

        }
        public bool IsCharDBC(char c)
        {
            if (c > 65280 && c < 65375)
                return true;
            else
                return false;
        }


        private void btnPrintBali_Click(object sender, EventArgs e)
        {
            if (bsPatList.DataSource != null && ((List<EntityPidReportMain>)bsPatList.DataSource).Count > 0)
            {
                txtPatDate.Focus();

                List<EntityPidReportMain> listPatient = ((List<EntityPidReportMain>)bsPatList.DataSource);
                listPatient = listPatient.FindAll(w => w.PatSelect);

                List<string> listRepId = new List<string>();
                foreach (EntityPidReportMain drPat in listPatient)
                {
                    listRepId.Add(drPat.RepId);
                }

                if (listRepId.Count == 0)
                {
                    lis.client.control.MessageDialog.Show("请选择要打印的数据！", "提示");
                    return;
                }

                this.PrintBacApply(listRepId);
            }
            else
            {
                lis.client.control.MessageDialog.Show("请选择要打印的数据！", "提示");
                return;
            }
        }
        private void radioGroupPatFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        private void FilterData()
        {
            List<EntityPidReportMain> listPat = new List<EntityPidReportMain>();
            List<long> lisID = new List<long>();

            if (txtIDFiter.Text.Trim() != string.Empty)
            {
                lisID = SampleIDRangeUtil.ToList(txtIDFiter.Text.Trim());
            }

            if (radioGroupPatFlag.EditValue.ToString() == "未检验")
            {
                listPat = listPatient.Where(w => w.RepStatus == 0).ToList();
            }
            else if (radioGroupPatFlag.EditValue.ToString() == "需复查")
            {
                listPat = listPatient.Where(w => w.RepRecheckFlag == 1).ToList();
            }
            else
            {
                if (lisID.Count == 0)
                    listPat = listPatient;
                else
                {
                    if (rgTypeFiter.SelectedIndex == 0)
                        listPat = listPatient.Where(w => lisID.Contains(Convert.ToInt64(w.RepSid))).ToList();
                    else
                        listPat = listPatient.Where(w => lisID.Contains((!string.IsNullOrEmpty(w.RepSerialNum) ? Convert.ToInt32(w.RepSerialNum) : -1))).ToList();
                }
            }

            bsPatList.DataSource = listPat;
        }

        private void txtIDFiter_EditValueChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        private void ckSID_CheckedChanged(object sender, EventArgs e)
        {
            txtSid.Properties.ReadOnly = ckSID.Checked;

        }

        /// <summary>
        /// 打印细菌或打印工作单
        /// </summary>
        /// <param name="p_strPatId"></param>
        private void PrintBacApply(List<string> listPatId)
        {

            string strReportCode = string.Empty;

            if (allowPrintWorkbill)//标本登记允许打印工作单
            {
                //工作单默认的报表代码为：workbill
                //若仪器编码不为空时,则工作单的报表代码为：workbill+仪器编码
                if (this.txtInstructment.valueMember != null && !string.IsNullOrEmpty(this.txtInstructment.valueMember))
                {
                    strReportCode = "workbill" + this.txtInstructment.valueMember;
                }
            }
            else
            {
                if (listPatId.Count > 1)
                {
                    strReportCode = "barcodeRegisterBacilliPrint2";
                }
                else
                {
                    strReportCode = "barcodeRegisterBacilliPrint";
                }
            }


            EntityDCLPrintParameter printParameter = new EntityDCLPrintParameter();
            printParameter.ListRepId = listPatId;
            printParameter.ReportCode = strReportCode;

            DCLReportPrint.Print(printParameter);
        }


        private void gridViewSingle_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            //GridView grid = sender as GridView;
            //EntityPatients dr = (EntityPatients)grid.GetRow(e.RowHandle);
            //if (dr != null)
            //{
            //if (e.Column.FieldName == "PidName" && dr.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Audited)
            //{
            //    e.Appearance.ForeColor = Color.Green;
            //}
            //else if (e.Column.FieldName == "PidName" && dr.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Reported)
            //{
            //    e.Appearance.ForeColor = Color.Blue;
            //}
            //else if (e.Column.FieldName == "PidName" && dr.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Printed)
            //{
            //    e.Appearance.ForeColor = Color.Red;
            //}

            //if (e.Column.FieldName == "PidName" && dr.RepCtype == "2")
            //{
            //    e.Appearance.BackColor = Color.FromArgb(64, 224, 208);
            //}
            //if (e.Column.FieldName == "PidIdentityName" && dr.PidIdentityName == "临床路径")
            //{
            //    e.Appearance.BackColor = Color.FromArgb(0, 255, 0);
            //}
            //}
        }

        private void txtType_DisplayTextChanged(object sender, control.ValueChangeEventArgs args)
        {
            txtType_ValueChanged(null, null);
        }

        Color Barcode_Color_FirstReported = IStep.GetBarcodeConfigColor("Barcode_Color_FirstReported");//报告
        Color Barcode_Color_Reported = IStep.GetBarcodeConfigColor("Barcode_Color_Reported");//二审
        Color Barcode_Color_Printed = IStep.GetBarcodeConfigColor("Barcode_Color_Printed");//打印
        private void gridViewSingle_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView grid = sender as GridView;
            EntityPidReportMain dr = (EntityPidReportMain)grid.GetRow(e.RowHandle);
            if (e.RowHandle == this.gridViewSingle.FocusedRowHandle)
            {
                e.Appearance.BackColor = System.Drawing.Color.LightBlue;
                e.Appearance.Options.UseBackColor = true;
            }
            if (dr != null)
            {
                if (dr.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Audited)
                {
                    e.Appearance.ForeColor = Barcode_Color_Reported;
                }

                if (dr.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Reported)
                {
                    e.Appearance.ForeColor = Barcode_Color_FirstReported;
                }

                if (dr.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Printed)
                {
                    e.Appearance.ForeColor = Barcode_Color_Printed;
                }
            }
        }

    }
}
