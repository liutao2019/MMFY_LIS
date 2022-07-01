using DevExpress.XtraGrid;
using Lis.CustomControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;

using System.Threading;
using dcl.client.frame;
using dcl.client.result.Interface;
using dcl.entity;
using dcl.client.frame.runsetting;
using dcl.client.cache;
using lis.client.control;
using dcl.client.report;
using dcl.root.logon;
using dcl.client.common;
using dcl.client.wcf;
using dcl.common;
using System.IO;
using Lib.DAC;

namespace wf.client.reagent
{
    public partial class FrmReagentStorage : FrmCommon, IPatPanelConfig
    {
        List<EntityDCLPrintParameter> listPrintData_multithread;
        bool useMultiThread = false;
        /// <summary>
        /// 当前入库信息
        /// </summary>
        EntityReaStorage CurrentStorageInfo = new EntityReaStorage();//病人消息
        /// <summary>
        /// 面板配置类
        /// </summary>
        PatInputRuntimeSetting UserCustomSetting = null;
        /// <summary>
        /// 新增时获得焦点的控件
        /// </summary>
        Control FocusOnAddNewControl = null;
        List<EntityReaStorage> AllReaList = new List<EntityReaStorage>();
        List<EntityReaPurchase> AllPurList = new List<EntityReaPurchase>();
        private String fpat_id = "";//主键
        private String pur_id = "";//主键
        private string CurrentReaID;
        bool isLoadData = false;
        bool isDataChaged = false;
        string xmlFile = PathManager.SettingLisPath + @"\printXml\barcodePrintConfig.xml";

        internal GridCheckSelection Selection { get; set; }

        private string ReagentStoreValidDays = "";

        private string ReagentStoreBarcodeMode = "";

        private bool ReagentInPriceGreat0;

        int barcodefootlength = 6;

        string barcodeCustomFoot = "000001";

        private DataTable tableStoreDetail;

        /// <summary>
        /// 试剂入库表
        /// </summary>
        List<EntityReaStorageDetail> dtReaStorageDetail = new List<EntityReaStorageDetail>();
        public FrmReagentStorage()
        {
            InitializeComponent();
            this.roundPanelGroup1.RoundStorageGroupClick += this.roundPanelGroup1_Click;
            this.sysToolBar1.OnCloseClicked += SysToolBar1_OnCloseClicked;
            this.sysToolBar1.OnBtnSaveClicked += SysToolBar1_OnBtnSaveClicked;
            this.sysToolBar1.OnBtnAddClicked += SysToolBar1_OnBtnAddClicked;
            this.sysToolBar1.OnBtnRefreshClicked += SysToolBar1_OnBtnRefreshClicked;
            this.sysToolBar1.OnBtnDeleteClicked += SysToolBar1_OnBtnDeleteClicked;
            this.sysToolBar1.OnAuditClicked += SysToolBar1_OnAuditClicked;
            //this.sysToolBar1.BtnUndoClick += SysToolBar1_BtnUndoClick;
            this.sysToolBar1.BtnAnswerClick += SysToolBar1_BtnAnswerClick;
            this.sysToolBar1.OnBtnPrintClicked += SysToolBar1_OnBtnPrintClicked;
            this.sysToolBar1.OnPrintPreviewClicked += SysToolBar1_OnPrintPreviewClicked;
            this.sysToolBar1.OnBtnImportClicked += SysToolBar1_OnBtnImportClicked;
            sysToolBar1.BtnPrintSetClick += new System.EventHandler(this.sysToolBar1_BtnPrintSetClick);
            sysToolBar1.OnBtnBCPrintClicked += new System.EventHandler(this.sysToolBar1_OnBtnBCPrintClicked);

            ReagentStoreValidDays =
                ConfigHelper.GetSysConfigValueWithoutLogin("ReagentStoreValidDays");

            ReagentStoreBarcodeMode =
                ConfigHelper.GetSysConfigValueWithoutLogin("ReagentStoreBarcodeMode");

            barcodefootlength = Convert.ToInt32(ConfigHelper.GetSysConfigValueWithoutLogin("ReagentBarcodeCustomFootLength"));

            //条码自定义后缀
            barcodeCustomFoot = ConfigHelper.GetSysConfigValueWithoutLogin("ReagentBarcodeCustomFoot");

            radioGroup1.Text = ReagentStoreBarcodeMode;

            ReagentInPriceGreat0 = ConfigHelper.GetSysConfigValueWithoutLogin("ReagentInPriceGreat0") == "是";
        }

        private void SysToolBar1_OnBtnImportClicked(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = string.Format("*.{0}|*.{0}", "xls");
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ReadDictToMemory(openFileDialog.FileName);
            }
        }
        /// <summary>
        /// 读取字典到内存
        /// </summary>
        private void ReadDictToMemory(string path)
        {
            string connStr1 = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + path+"';Persist Security Info=False;Extended Properties='Excel 8.0;HDR=YES;IMEX=1'";

            SqlHelper helper = new SqlHelper(connStr1, EnumDbDriver.Oledb);
            tableStoreDetail = helper.GetTable("select * from [Sheet1$]", "StoreDetail");
        }
        private void ReadPurReaID()
        {
            DataRow[] drs = tableStoreDetail.Select();

            //获取试剂库信息
            List<EntityReaSetting> listReagentLibrary = CacheClient.GetCache<EntityReaSetting>().FindAll(w => !string.IsNullOrEmpty(w.Drea_tender));

            List<EntityReaPurchaseDetail> listPurDetail = new List<EntityReaPurchaseDetail>();

            foreach (EntityReaSetting row in listReagentLibrary)
            {
                string provincialNum = row.Drea_tender;
                if (provincialNum.Trim() != string.Empty)
                {
                    if (drs.Length > 0)
                    {
                        foreach (var item in drs)
                        {
                            if (provincialNum == item["省标标号"].ToString().Trim())
                            {
                                EntityReaPurchaseDetail detail = new EntityReaPurchaseDetail();
                                detail.Rpcd_reaid = row.Drea_id;
                                detail.Rpcd_reacount = Convert.ToInt32(item["数量"]);
                                detail.Rpcd_price = Convert.ToDecimal(item["单价"]);
                                detail.Report = "合格";
                                detail.OutPackage = "合格";
                                detail.Temparate = "合格";
                                listPurDetail.Add(detail);
                            }
                        }
                    }
                }
            }
            if (listPurDetail!=null&& listPurDetail.Count > 0)
            {
                setDetail(listPurDetail);

            }
            MessageBox.Show(string.Format("本次更新入库记录{0}个，还有{1}不能入库，请手工入库", listPurDetail.Count, drs.Count() - listPurDetail.Count));


        }
        private void InitSelection()
        {
            gvReadetail.ExpandAllGroups();
            Selection = new GridCheckSelection(gvReadetail);
            Selection.CheckMarkColumn.Width = 20;
            Selection.CheckMarkColumn.VisibleIndex = 0;
        }
        /// <summary>
        /// 打印条码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnBCPrintClicked(object sender, EventArgs e)
        {
            bool printBarcodeSuccess;
            try
            {
                printBarcodeSuccess = PrintBarcodeWithMachine();
                if (!printBarcodeSuccess)
                {
                    ShowAndClose("打印条码失败");
                    return;
                }
            }
            catch (ReportNotFoundException ex)
            {
                ShowMessage("打印条码失败:" + ex.MSG);
                return;
            }
            catch (BarcodePrinterNotFoundException ex1)
            {
                ShowMessage("打印条码失败:" + ex1.Message);
                return;
            }

            int focusedRowHandle = gvReadetail.FocusedRowHandle;
            if (sender != null)
            {
                MoveNext(focusedRowHandle);
            }
            else
            {
                MovePreBarcode();
            }
            if (printBarcodeSuccess)
                Selection.ClearSelection();
        }
        public void ShowAndClose(string msg)
        {
            lis.client.control.MessageDialog.ShowAutoCloseDialog(msg, 1m);
        }
        private void ShowMessage(string word)
        {
            lis.client.control.MessageDialog.Show(word, "提示");
        }
        public void MovePreBarcode()
        {
            for (int i = 0; i < gvReadetail.RowCount; i++)
            {
                EntityReaStorageDetail dr = (EntityReaStorageDetail)this.gvReadetail.GetRow(i);

                if (dr.Rsd_barcode != string.Empty)
                { }
                else
                {
                    gvReadetail.FocusedRowHandle = i;
                    break;
                }
            }
        }
        /// <summary>
        /// 跳到下一条
        /// </summary>
        public void MoveNext(int focusedRowHandle)
        {
            //光标移动到未打印条码
            if (gvReadetail.FocusedRowHandle != gvReadetail.DataRowCount - 1)
            {
                for (int i = focusedRowHandle + 1; i < gvReadetail.RowCount; i++)
                {
                    EntityReaStorageDetail dr = (EntityReaStorageDetail)this.gvReadetail.GetRow(i);

                    gvReadetail.FocusedRowHandle = i;
                    break;
                }
            }
            else
            {
                gvReadetail.FocusedRowHandle = focusedRowHandle;
                gvReadetail.RefreshData();
            }
        }
        /// <summary>
        /// 条码机设置
        /// </summary>
        private void sysToolBar1_BtnPrintSetClick(object sender, EventArgs e)
        {
            FrmPrintConfigurationV2 configer = new FrmPrintConfigurationV2();
            configer.ShowDialog();
        }
        /// <summary>
        /// 调用打印机打印条码
        /// </summary>
        private bool PrintBarcodeWithMachine()
        {
            string printMachineName = GetPrintMachineName();
            if (string.IsNullOrEmpty(printMachineName))
            {
                throw new BarcodePrinterNotFoundException();

            }
            ZeBraPrinter machine = new ZeBraPrinter();
            bool result = false;
            try
            {
                string template = "ReagentBarcode";
                if (HasNotChoose())
                {
                    lis.client.control.MessageDialog.Show("没有选择条码", "提示");
                    return false;
                }
                List<EntityReaStorageDetail> rows = Selection.GetAllSelectT<EntityReaStorageDetail>();

                List<string> listBarId = new List<string>();

                foreach (EntityReaStorageDetail item in rows)
                {
                    listBarId.Add(item.Rsd_barcode.ToString());
                }

                machine.PrintInfo = new PrintInfo(listBarId);
                result = machine.PrintReaBarcode(printMachineName, template);
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("打印出错:" + ex.Message);
                Logger.WriteException("条码", "条码调用打印器打印", ex.Message + "\r\n" + ex.StackTrace);
                return false;
            }
            return result;
        }
        private bool HasNotChoose()
        {
            return Selection.SelectedCount <= 0;
        }
        /// <summary>
        /// 获取条码打印机名称
        /// </summary>
        /// <returns></returns>
        private string GetPrintMachineName(string printField = "printName")
        {
            if (File.Exists(xmlFile))
            {
                DataSet dsPrint = new DataSet();
                dsPrint.ReadXml(xmlFile);
                if (dsPrint.Tables.Count > 0)
                {
                    DataTable dt = dsPrint.Tables[0];
                    if (dt != null)
                    {
                        return dt.Rows[0][printField].ToString();
                    }
                }
            }

            return "";
        }
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="isPreview"></param>
        private void Print(bool isPreview)
        {
            OnPatPrint(null, isPreview);
        }
        /// <summary>
        /// 打印/预览
        /// <param name="pat_id">病人ID，如果传null值则打印当前勾选中的病人</param>
        /// <param name="isPreview">当前打印是否为打印预览</param>
        /// </summary>
        private void OnPatPrint(string pat_id, bool isPreview)
        {
            string prtTemplate = "ReagentStorage";

            this.BeginLoading();
            List<string> listPatID = new List<string>();
            StringBuilder sbPatSidWhere2 = new StringBuilder();
            if (pat_id == null)
            {
                List<EntityReaStorage> listPatient = GetCheckedReaStorage();
                if (listPatient.Count > 0)
                {
                    foreach (EntityReaStorage dr in listPatient)
                    {
                        if (isPreview)
                        {
                            listPatID.Add(dr.Rsr_no.ToString());
                        }
                        else
                        {
                            if (dr.Rsr_status.ToString() == LIS_Const.REAGENT_FLAG.Audited)
                            {
                                if (string.IsNullOrEmpty(dr.AuditorName))
                                {
                                    sbPatSidWhere2.Append(string.Format(",[{0}]", dr.Rsr_no));
                                    continue;
                                }
                                listPatID.Add(dr.Rsr_no.ToString());

                            }
                        }
                    }

                    if (sbPatSidWhere2.Length > 0)
                    {
                        MessageDialog.Show("单号为:" + sbPatSidWhere2.Remove(0, 1) + " 的入库单无审核者信息，需重新审核", "提示");
                        this.CloseLoading();
                        return;
                    }
                    if (listPatID.Count == 0)
                    {
                        MessageDialog.Show("没有符合打印/预览要求的记录，请检查选中的记录是否已审核", "提示");
                        this.CloseLoading();
                        return;
                    }
                }
                else
                {
                    MessageDialog.Show("在请左侧勾选需要预览/打印的记录", "提示");
                    this.CloseLoading();
                    return;
                }
            }
            else//存打
            {
                listPatID.Add(pat_id);
            }

            #region 新打印方式，传多个打印指令打印多个病人，每条指令一个病人

            List<EntityDCLPrintParameter> listPara = new List<EntityDCLPrintParameter>();
            int sequence = 0;

            //ShowDialogForm tip = new ShowDialogForm("提示", "正在打开......", "请耐心等候！", listPatID.Count);
            foreach (string patient_id in listPatID)
            {
                EntityDCLPrintParameter printPara = new EntityDCLPrintParameter();
                //printPara.RepId = patient_id;
                printPara.CustomParameter.Add("StorageId", patient_id);
                printPara.ReportCode = prtTemplate;
                printPara.Sequence = sequence;
                listPara.Add(printPara);
                sequence++;

                //tip.SetProgress();
            }
            listPrintData_multithread = EntityManager<EntityDCLPrintParameter>.ListClone(listPara);

            if (isPreview)//是否为打印预览
            {
                if (useMultiThread)
                {
                    Thread thread = new Thread(new ThreadStart(StartPreviewReports));
                    thread.IsBackground = true;
                    thread.Start();
                }
                else
                {
                    try
                    {
                        this.CloseLoading();
                        StartPreviewReports();
                        
                    }
                    catch (ReportNotFoundException ex1)
                    {
                        MessageDialog.Show(ex1.MSG);
                    }
                    catch (Exception ex2)
                    {
                        Logger.WriteException(this.GetType().ToString(), "print", ex2.Message);
                    }
                }
            }
            else
            {
                try
                {
                    //不是打印预览才需要更新打印标志
                    pForm_PrintStart2(listPara);
                    DCLReportPrint.BatchPrint(listPara, ReportSetting.PrintName);
                    this.RefreshPatients();
                    this.SelectAllStorageInGrid(false);
                }
                catch (ReportNotFoundException ex1)
                {
                    MessageDialog.Show(ex1.MSG);
                }
                catch (Exception ex2)
                {
                    Logger.WriteException(this.GetType().ToString(), "print", ex2.Message);
                }
            }

            //tip.Close();

            #endregion
            this.CloseLoading();
        }
        public void SelectAllStorageInGrid(bool selectAll)
        {
            //在全部勾选前把焦点行设置成-1解决：全选的时候焦点行会显示不了勾
            if (selectAll)
            {
                gvReaStorage.SelectAll();
            }
            else
                gvReaStorage.ClearSelection();
            this.gcReaStorage.RefreshDataSource();
        }
        public void RefreshPatients()
        {
            GetReaList(false);
            this.addNew();
        }
        void pForm_PrintStart2(List<EntityDCLPrintParameter> listPara)
        {
            List<string> listPatID = new List<string>();

            foreach (EntityDCLPrintParameter item in listPara)
            {
                listPatID.Add(item.RepId);
            }

            if (listPatID != null && listPatID.Count > 0)
            {
                UpdatePrintState(listPatID);
            }
        }
        /// <summary>
        /// 更新打印状态
        /// </summary>
        /// <param name="listPatID"></param>
        public void UpdatePrintState(IEnumerable<string> listPatID)
        {
            if (listPatID != null)
            {
                string strPlace = null;
                try
                {
                    strPlace = LocalSetting.Current.Setting.Description;//描述信息
                }
                catch (Exception)
                {
                }
                if (!ConfigHelper.IsNotOutlink())
                {
                    strPlace = IPUtility.GetIP();
                }
                new ProxyReaStorage().Service.UpdatePrintState_whitOperator(listPatID, UserInfo.loginID, UserInfo.userName, strPlace);
            }
        }

        //**************************
        //另一线程的方法
        void StartPreviewReports()
        {
            try
            {
                DCLReportPrint.BatchPrintPreview(listPrintData_multithread);
            }
            catch (ReportNotFoundException ex1)
            {
                MessageDialog.Show(ex1.MSG);
            }
            catch (Exception ex2)
            {

            }
        }
        private void SysToolBar1_OnPrintPreviewClicked(object sender, EventArgs e)
        {
            if (GetCheckedReaStorage().Count <= 0)
            {
                return;
            }
            Print(true);
        }

        private void SysToolBar1_OnBtnPrintClicked(object sender, EventArgs e)
        {
            if (GetCheckedReaStorage().Count <= 0)
            {
                return;
            }
            Print(false);
            RefreshPatients();
        }

        private void SysToolBar1_BtnAnswerClick(object sender, EventArgs e)
        {
            if (CurrentStorageInfo == null || string.IsNullOrEmpty(CurrentStorageInfo.Rsr_no))
            {
                MessageDialog.Show("请勾选你要撤销数据！", "提示");
                return;
            }
            if (CurrentStorageInfo.out_flag == 1)
            {
                MessageDialog.Show("该记录已出库！", "提示");
                return;
            }
            if (CurrentStorageInfo.Rsr_status != "4")
            {
                MessageDialog.Show("该记录未审核！", "提示");
                return;
            }
            EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
            Caller.IPAddress = UserInfo.ip;
            Caller.LoginID = UserInfo.loginID;
            Caller.LoginName = UserInfo.userName;
            FrmReaCheckPassword frmCheck = new FrmReaCheckPassword("身份验证 - 撤销", LIS_Const.ReagentPopedomCode.StorageReturn, "", "");
            DialogResult dig = frmCheck.ShowDialog();

            if (dig == DialogResult.OK)
            {
                Caller.IPAddress = UserInfo.ip;
                Caller.LoginID = frmCheck.OperatorID;
                Caller.LoginName = frmCheck.OperatorName;
                Caller.UserID = frmCheck.OperatorID;
                CurrentStorageInfo.Rsr_returnreason = frmCheck.ReturnReason;
                new ProxyReaStorage().Service.ReturnReaData(Caller, CurrentStorageInfo);

            }
            else if (dig == DialogResult.No)
            {
                MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
            }
            RefreshPatients();
        }

        //private void SysToolBar1_BtnUndoClick(object sender, EventArgs e)
        //{
        //    if (CurrentStorageInfo == null || string.IsNullOrEmpty(CurrentStorageInfo.Rsr_no))
        //    {
        //        MessageDialog.Show("请勾选你要取消审核数据！", "提示");
        //        return;
        //    }
        //    if (CurrentStorageInfo.Rsr_status != "4")
        //    {
        //        MessageDialog.Show("该记录未审核！", "提示");
        //        return;
        //    }
        //    EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
        //    Caller.IPAddress = UserInfo.ip;
        //    Caller.LoginID = UserInfo.loginID;
        //    Caller.LoginName = UserInfo.userName;
        //    FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 取消审核", LIS_Const.ReagentPopedomCode.Audit, "", "");
        //    frmCheck.operationCode = EnumOperationCode.Audit;//保存用以取消时统一身份验证
        //                                                     //验证窗口
        //    DialogResult dig = frmCheck.ShowDialog();
        //    if (dig == DialogResult.OK)
        //    {
        //        Caller.IPAddress = UserInfo.ip;
        //        Caller.LoginID = frmCheck.OperatorID;
        //        Caller.LoginName = frmCheck.OperatorName;
        //        Caller.UserID = frmCheck.OperatorID;

        //        CurrentStorageInfo.Rsr_auditor = string.Empty;
        //        CurrentStorageInfo.Rsr_auditdate = null;
        //        CurrentStorageInfo.ListReaStorageDetail = dtReaStorageDetail;
        //        CurrentStorageInfo.Rsr_status = "1";

        //        var ret = new ProxyReaStorage().Service.UpdateReaData(Caller, CurrentStorageInfo);

        //    }
        //    else if (dig == DialogResult.No)
        //    {
        //        MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
        //    }
        //    RefreshPatients();
        //}

        private void SysToolBar1_OnAuditClicked(object sender, EventArgs e)
        {
            if (CurrentStorageInfo == null || string.IsNullOrEmpty(CurrentStorageInfo.Rsr_no))
            {
                MessageDialog.Show("请勾选你要审核数据！", "提示");
                return;
            }
            if (CurrentStorageInfo.out_flag == 1)
            {
                MessageDialog.Show("该记录已出库！", "提示");
                return;
            }
            if (CurrentStorageInfo.Rsr_status == "4")
            {
                MessageDialog.Show("该记录已审核！", "提示");
                return;
            }
            if (dtReaStorageDetail.Count <= 0)
            {
                MessageDialog.Show($"没有入库明细！", "提示");
                return;
            }
            bool check = CheckItems(dtReaStorageDetail);
            if (check)
            {
                EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
                Caller.IPAddress = UserInfo.ip;
                Caller.LoginID = UserInfo.loginID;
                Caller.LoginName = UserInfo.userName;
                FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 审核", LIS_Const.ReagentPopedomCode.StorageAudit, "", "");
                frmCheck.operationCode = EnumOperationCode.Audit;//保存用以取消时统一身份验证
                                                                 //验证窗口
                DialogResult dig = frmCheck.ShowDialog();
                if (dig == DialogResult.OK)
                {
                    Caller.IPAddress = UserInfo.ip;
                    Caller.LoginID = frmCheck.OperatorID;
                    Caller.LoginName = frmCheck.OperatorName;
                    Caller.UserID = frmCheck.OperatorID;

                    CurrentStorageInfo.Rsr_auditor = Caller.UserID;
                    CurrentStorageInfo.Rsr_auditdate = ServerDateTime.GetServerDateTime();
                    CurrentStorageInfo.ListReaStorageDetail = dtReaStorageDetail;
                    CurrentStorageInfo.Rsr_status = "4";
                    CurrentStorageInfo.StorageMode = radioGroup2.Text;
                    var ret = new ProxyReaStorage().Service.AuditReaData(Caller, CurrentStorageInfo);
                }
                else if (dig == DialogResult.No)
                {
                    MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
                }
            }
            RefreshPatients();
        }

        public List<EntityReaStorage> GetCheckedReaStorage()
        {
            gvReaStorage.CloseEditor();
            this.bsReaStorage.EndEdit();

            List<EntityReaStorage> checkList = new List<EntityReaStorage>();
            var selectIndex = gvReaStorage.GetSelectedRows();
            foreach (int index in selectIndex)
            {
                checkList.Add(gvReaStorage.GetRow(index) as EntityReaStorage);
            }

            if (checkList.Count <= 0
                && CurrentStorageInfo != null
                && !string.IsNullOrEmpty(CurrentStorageInfo.Rsr_no))
            {
                checkList.Add(gvReaStorage.GetFocusedRow() as EntityReaStorage);
            }

            return checkList;
        }
        bool DeleteStorage()
        {
            gvReaStorage.CloseEditor();
            this.bsReaStorage.EndEdit();

            StringBuilder logMsg = new StringBuilder();
            List<EntityReaStorage> delReaStorageList = new List<EntityReaStorage>();

            int patCount = 0;
            bool delflag = false;

            if (UserInfo.GetSysConfigValue("BathOrSingleDelFlag").Equals("是"))
            {
                List<EntityReaStorage> dtReaStorage = GetCheckedReaStorage();
                if (dtReaStorage == null) return false;
                delflag = true;

                foreach (var dr in dtReaStorage)
                {
                    patCount++;

                    if (dr.Rsr_status.ToString() == "1"
                        || dr.Rsr_status.ToString() == "2")
                    {
                        delReaStorageList.Add(dr);
                    }
                }

            }
            else
            {
                delflag = false;
                EntityReaStorage dr = this.gvReaStorage.GetFocusedRow() as EntityReaStorage;
                if (dr != null)
                {
                    patCount++;
                    if (dr.Rsr_status.ToString() == "1"
                        || dr.Rsr_status.ToString() == "2")
                    {
                        delReaStorageList.Add(dr);
                    }
                }
            }

            if (patCount == 0)
            {
                if (delflag == false)
                    MessageDialog.Show("请选中你要删除数据！", "提示");
                else
                    MessageDialog.Show("请勾选你要删除数据！", "提示");

                return false;
            }

            if (delReaStorageList.Count < 1)
            {
                lis.client.control.MessageDialog.Show(string.Format("所选数据已审核！"), "提示");
                return false;
            }
            if (delReaStorageList.Count > 1)
            {
                if (MessageDialog.Show(string.Format("您将要删除 {0} 条病人记录，是否继续？", delReaStorageList.Count), "确认", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return false;
                }
            }
            else
            {
                object name = delReaStorageList[0].Rsr_no;
                if (MessageDialog.Show(string.Format("您将要删除 入库单号:{0} 的记录，是否继续？", name != null ? name.ToString() : string.Empty), "确认", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return false;
                }
            }

            FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 删除", LIS_Const.ReagentPopedomCode.StorageDelete, "", "");

            DialogResult dig = frmCheck.ShowDialog();
            if (dig == DialogResult.OK)
            {
                EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
                Caller.IPAddress = UserInfo.ip;
                Caller.LoginID = frmCheck.OperatorID;
                Caller.LoginName = frmCheck.OperatorName;

                var ret = new ProxyReaStorage().Service.DeleteReaData(Caller, delReaStorageList);

                addNew();
                GetReaList(false);
                return true;
            }
            else if (dig == DialogResult.No)
            {
                MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
            }
            return false;
        }
        private void SysToolBar1_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            DeleteStorage();
        }

        private void SysToolBar1_OnBtnRefreshClicked(object sender, EventArgs e)
        {
            GetReaList(false);
            addNew();
        }

        private void SysToolBar1_OnBtnAddClicked(object sender, EventArgs e)
        {
            addNew();
        }
        private String getMaxSID()
        {
            DateTime dtPatDate = this.txtReaDate.DateTime;
            string sid = new ProxyReaStorage().Service.GetReaSID_MaxPlusOne(dtPatDate, new StorageStep().StepCode);
            return sid;
        }
        private String getMaxBarcode()
        {
            DateTime dtPatDate = this.txtReaDate.DateTime;
            string sid = new ProxyReaStorage().Service.GetReaBarcode_MaxPlusOne(dtPatDate, new StorageStep().StepCode);
            return sid;
        }
        private void gvReaPurchase_Click(object sender, EventArgs e)
        {

        }
        private void addNew()
        {
            if (Selection != null)
                Selection.ClearSelection();
            this.txtReaDate.Properties.ReadOnly = false;
            CurrentReaID = "";
            CurrentStorageInfo = new EntityReaStorage();

            DateTime dtToday = ServerDateTime.GetServerDateTime();

            txtReaDate.EditValue = dtToday;
            //*********************************************
            txtReaSid.Text = getMaxSID();

            fpat_id = "";
            pur_id = "";
            txtPurno.Text = "";
            txtCount.Text = "";
            txtPrice.Text = "0";
            txtInvoice.Text = "";
            txtBatchNo.Text = "";
            txtBarcode.Text = "";

            dtReaStorageDetail = new List<EntityReaStorageDetail>();

            this.gvReadetail.ClearSelection();

            gcReaDetail.DataSource = dtReaStorageDetail;
            gcReaDetail.RefreshDataSource();
            gvReadetail.RefreshData();
        }

        private void SysToolBar1_OnBtnSaveClicked(object sender, EventArgs e)
        {
            sysToolBar1.Focus();
            SaveOrUpdate();

        }
        public void SaveOrUpdate()
        {
            if (dtReaStorageDetail.Count <= 0)
            {
                MessageDialog.Show($"没有入库明细！", "提示");
                return;
            }
            EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
            Caller.IPAddress = UserInfo.ip;
            Caller.LoginID = UserInfo.loginID;
            Caller.LoginName = UserInfo.userName;
            if (string.IsNullOrEmpty(fpat_id))
            {
                bool check = CheckItems(dtReaStorageDetail);
                if (check)
                {
                    FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 保存", LIS_Const.ReagentPopedomCode.StorageSave, "", "");
                    frmCheck.operationCode = EnumOperationCode.Audit;//保存用以取消时统一身份验证
                                                                     //验证窗口
                    DialogResult dig = frmCheck.ShowDialog();
                    if (dig == DialogResult.OK)
                    {
                        Caller.IPAddress = UserInfo.ip;
                        Caller.LoginID = frmCheck.OperatorID;
                        Caller.LoginName = frmCheck.OperatorName;
                        Caller.UserID = frmCheck.OperatorID;

                        EntityReaStorage apply = new EntityReaStorage();
                        apply.Rsr_no = txtReaSid.Text;
                        apply.Rsr_date = txtReaDate.DateTime;
                        apply.ListReaStorageDetail = dtReaStorageDetail;
                        apply.Rsr_status = "1";
                        apply.Rsr_purno = txtPurno.Text;
                        apply.StorageMode = radioGroup2.Text;
                        var ret = new ProxyReaStorage().Service.SaveReaData(Caller, apply);
                    }
                    else if (dig == DialogResult.No)
                    {
                        MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
                        return;
                    }
                }
                else
                {
                    return;
                }

            }
            else
            {
                if (CurrentStorageInfo.Rsr_status == "1" || CurrentStorageInfo.Rsr_status == "2")
                {
                    bool check = CheckItems(dtReaStorageDetail);
                    if (check)
                    {
                        FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 保存", LIS_Const.ReagentPopedomCode.StorageSave, "", "");
                        frmCheck.operationCode = EnumOperationCode.Audit;//保存用以取消时统一身份验证
                                                                         //验证窗口
                        DialogResult dig = frmCheck.ShowDialog();
                        if (dig == DialogResult.OK)
                        {
                            Caller.IPAddress = UserInfo.ip;
                            Caller.LoginID = frmCheck.OperatorID;
                            Caller.LoginName = frmCheck.OperatorName;
                            Caller.UserID = frmCheck.OperatorID;

                            CurrentStorageInfo.Rsr_date = txtReaDate.DateTime;
                            CurrentStorageInfo.ListReaStorageDetail = dtReaStorageDetail;
                            CurrentStorageInfo.Rsr_status = "1";

                            var ret = new ProxyReaStorage().Service.UpdateReaData(Caller, CurrentStorageInfo);
                        }
                        else if (dig == DialogResult.No)
                        {
                            MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
                            return;
                        }
                    }

                }
                else
                {
                    MessageDialog.Show("当前记录已审核，不能进行当前操作！", "提示");
                }
            }
            RefreshPatients();
        }

        public bool CheckItems(List<EntityReaStorageDetail> list)
        {
            bool check = false;
            int days;
            bool res = int.TryParse(ReagentStoreValidDays, out days);
            foreach (var item in dtReaStorageDetail)
            {
                if (string.IsNullOrEmpty(item.Rsd_no))
                {
                    MessageDialog.Show("入库单号不能为空！", "提示");
                    check = false;
                    break;
                }
                if (string.IsNullOrEmpty(item.Rsd_reaid))
                {
                    MessageDialog.Show("试剂信息不能为空！", "提示");
                    check = false;
                    break;
                }
                if (string.IsNullOrEmpty(item.Rsd_groupid))
                {
                    MessageDialog.Show("试剂组别不能为空！", "提示");
                    check = false;
                    break;
                }
                //if (string.IsNullOrEmpty(item.Rsd_package))
                //{
                //    MessageDialog.Show("试剂规格不能为空！", "提示");
                //    check = false;
                //    break;
                //}
                if (string.IsNullOrEmpty(item.Rsd_batchno))
                {
                    MessageDialog.Show("批号不能为空！", "提示");
                    check = false;
                    break;
                }
                if (string.IsNullOrEmpty(item.Rsd_supid))
                {
                    MessageDialog.Show("供应商不能为空！", "提示");
                    check = false;
                    break;
                }
                if (string.IsNullOrEmpty(item.Rsd_unitid))
                {
                    MessageDialog.Show("单位不能为空！", "提示");
                    check = false;
                    break;
                }
                if (string.IsNullOrEmpty(item.Rsd_barcode))
                {
                    MessageDialog.Show("条码号不能为空！", "提示");
                    check = false;
                    break;
                }
                if (string.IsNullOrEmpty(item.Rsd_outerpacking))
                {
                    MessageDialog.Show("外包装不能为空！", "提示");
                    check = false;
                    break;
                }
                if (string.IsNullOrEmpty(item.Rsd_temperature))
                {
                    MessageDialog.Show("到达温度不能为空！", "提示");
                    check = false;
                    break;
                }
                if (string.IsNullOrEmpty(item.Rsd_report))
                {
                    MessageDialog.Show("检验报告不能为空！", "提示");
                    check = false;
                    break;
                }
                if (item.Rsd_validdate < DateTime.Now)
                {
                    MessageDialog.Show("有效日期不能小于今天！", "提示");
                    check = false;
                    break;
                }
                if (item.Rsd_reacount <= 0)
                {
                    MessageDialog.Show("入库数量不能小于0！", "提示");
                    check = false;
                    break;
                }
                if (res)
                {
                    if (item.Rsd_validdate < DateTime.Now.AddDays(days))
                    {
                        MessageDialog.Show($"入库有效日期不能少于{days}天！", "提示");
                        check = false;
                        break;
                    }
                }
                check = true;
            }
            return check;
        }

        private void SysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 关闭编辑单元格并更新数据
        /// </summary>
        private void CloseEditor()
        {
            this.gvReadetail.CloseEditor();
        }
        /// <summary>
        /// 添加试剂信息
        /// </summary>
        /// <param name="com_id">试剂ID</param>
        private void AddReaItems(string com_id, int com_seq)
        {
            CloseEditor();

            //根据组合ID获取组合信息
            EntityReaSetting drRea = DictReagent.Instance.GetReaByID(com_id);

            if (drRea != null)
            {
                EntityReaStorageDetail drReaStorageDetail = new EntityReaStorageDetail();
                drReaStorageDetail.Rsd_no = txtReaSid.Text;
                drReaStorageDetail.ReagentName = drRea.Drea_name;
                drReaStorageDetail.Rsd_package = drRea.Drea_package;
                drReaStorageDetail.Rsd_reaid = drRea.Drea_id;
                drReaStorageDetail.SupName = drRea.Rsupplier_name;
                drReaStorageDetail.Rsd_supid = drRea.Drea_supplier;
                drReaStorageDetail.IsNew = 1;
                dtReaStorageDetail.Add(drReaStorageDetail);
            }
        }
        /// <summary>
        /// 加载用户式样配置
        /// </summary>
        private void LoadUserSetting()
        {
            PatInputRuntimeSetting setting = PatInputRuntimeSetting.Load("FrmReagenStorage", string.Empty, UserInfo.loginID);
            if (setting != null)
            {
                ApplySetting(setting);

            }

        }
        public void ApplySetting(PatInputRuntimeSetting setting)
        {
            this.UserCustomSetting = setting;
            ApplySetting();
        }
        public void ApplySetting()
        {
            PatInputRuntimeSetting setting = this.UserCustomSetting;

            #region 左侧式样

            DataTable dtSearchBarDDL = new DataTable();
            dtSearchBarDDL.Columns.Add("name");
            dtSearchBarDDL.Columns.Add("value");

            DataRow drSearchBarDDL = dtSearchBarDDL.NewRow();

            drSearchBarDDL["name"] = string.Empty;
            drSearchBarDDL["value"] = string.Empty;
            dtSearchBarDDL.Rows.Add(drSearchBarDDL);

            drSearchBarDDL = dtSearchBarDDL.NewRow();
            drSearchBarDDL["name"] = "入库单号";
            drSearchBarDDL["value"] = "rea_sid";
            dtSearchBarDDL.Rows.Add(drSearchBarDDL);

            this.cmbBarSearchPatType.Properties.DropDownRows = dtSearchBarDDL.Rows.Count;
            this.cmbBarSearchPatType.Properties.DataSource = dtSearchBarDDL;
            this.cmbBarSearchPatType.Properties.DisplayMember = "name";
            this.cmbBarSearchPatType.Properties.ValueMember = "value";



            #endregion

            #region 整行显示字体颜色 2018-05-11
            for (int i = 0; i < gvReaStorage.Columns.Count; i++)
            {
                string columnName = gvReaStorage.Columns[i].FieldName;
                if (columnName != "PatSelect" && columnName != "pat_icon")
                {
                    //未审核
                    FormatRowStorageTo(gvReaStorage, columnName, LIS_Const.REAGENT_FLAG.Natural, setting.PatListPanel.BackColorNormal, setting.PatListPanel.BackColorNormal, setting.PatListPanel.ForeColorNormal);
                    //已审核
                    FormatRowStorageTo(gvReaStorage, columnName, LIS_Const.REAGENT_FLAG.Audited, setting.PatListPanel.BackColorAudited, setting.PatListPanel.BackColorAudited, setting.PatListPanel.ForeColorAudited);
                    //未通过
                    FormatRowStorageTo(gvReaStorage, columnName, LIS_Const.REAGENT_FLAG.Returned, setting.PatListPanel.BackColorReturn, setting.PatListPanel.BackColorReturn, setting.PatListPanel.ForeColorReturn);
                    //已完成
                    FormatRowStorageTo(gvReaStorage, columnName, LIS_Const.REAGENT_FLAG.Done, setting.PatListPanel.BackColorDone, setting.PatListPanel.BackColorDone, setting.PatListPanel.ForeColorDone);

                }
            }
            #endregion

            #region 中间式样

            //设置新增时获得焦点的控件
            if (FocusOnAddNewControl == null)
            {
                FocusOnAddNewControl = this.txtReaSid;
            }

            if (FocusOnAddNewControl != null)
            {
                this.ActiveControl = FocusOnAddNewControl;
                FocusOnAddNewControl.Focus();
            }
            #endregion
        }

        private void FormatRowStorageTo(GridView gridView, string fieldName, string auditedValue, Color backColorAudited, Color backColor, Color foreColor)
        {
            GridFormatRule rule = new GridFormatRule();

            rule.Column = gridView.Columns["Rsr_status"];
            rule.ColumnApplyTo = gridView.Columns[fieldName];
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue1 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            formatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.Equal;
            formatConditionRuleValue1.Value1 = auditedValue;
            if (backColorAudited != Color.Transparent)
            {
                formatConditionRuleValue1.Appearance.BackColor = backColor;//已审核
            }
            formatConditionRuleValue1.Appearance.ForeColor = foreColor;
            formatConditionRuleValue1.Appearance.Options.UseForeColor = true;
            rule.Rule = formatConditionRuleValue1;
            gridView.FormatRules.Add(rule);
        }

        /// <summary>
        /// 绑定
        /// </summary>
        private void BindGrid()
        {
            if (this.dtReaStorageDetail != null)
            {
                this.dtReaStorageDetail = this.dtReaStorageDetail.OrderBy(i => i.ReagentName).ToList();

                CloseEditor();

                this.gvReadetail.ClearGrouping();
                this.gvReadetail.ClearSorting();
                this.gcReaDetail.DataSource = this.dtReaStorageDetail;

                gvReadetail.RefreshData();
                gcReaDetail.RefreshDataSource();
            }
        }
        private void BindLookupData()
        {
            if (Selection != null)
                Selection.ClearSelection();
            this.gcReaDetail.DataSource = this.dtReaStorageDetail;
            gvReadetail.RefreshData();
            gcReaDetail.RefreshDataSource();
        }
        private void SearchPatientsAndAddNew()
        {
            GetReaList(false);

            addNew();

            this.txtReaSid.Text = getMaxSID();
        }

        private void SelectedPageChanged(int index)
        {
            switch (index)
            {
                case 0:
                    cbeFlag.EditValue = "-1";//全部
                    break;
                case 1:
                    cbeFlag.EditValue = "1";//未审核
                    break;
                case 2:
                    cbeFlag.EditValue = "2";//审核未通过
                    break;
                case 4:
                    cbeFlag.EditValue = "4";//已审核
                    break;
                case 8:
                    cbeFlag.EditValue = "8";//未打印
                    break;
                case 9:
                    cbeFlag.EditValue = "9";//已完成
                    break;
                default:
                    break;
            }
        }
        private void cbeAge_EditValueChanged(object sender, EventArgs e)
        {
            GetReaList(false);
            addNew();
        }
        /// <summary>
        /// 根据时间获得列表
        /// </summary>
        /// <param name="isTrue"></param>
        private void GetReaList(bool isTrue)
        {
            EntityReaQC qc = new EntityReaQC();
            qc.DateStart = dtBegin.DateTime.Date;
            qc.DateEnd = dtEnd.DateTime.Date.AddDays(1).AddSeconds(-1);

            string strPatFlag = cbeFlag.EditValue.ToString();
            if (strPatFlag == "1")//未审核
            {
                qc.ReaStatus = "1";
            }
            else if (strPatFlag == "2")//审核不通过
            {
                qc.ReaStatus = "2";
            }
            else if (strPatFlag == "4")//已审核
            {
                qc.ReaStatus = "4";
            }
            else if (strPatFlag == "8")//未打印
            {
                qc.ReaPrintFlag = "0";
            }
            var list = new ProxyReaStorage().Service.ReaQuery(qc);

            EntityReaQC reaQc = new EntityReaQC();
            qc.DateStart = dtBegin.DateTime.Date;
            qc.DateEnd = dtEnd.DateTime.Date.AddDays(1).AddSeconds(-1);
            qc.ReaStatus = "4";
            var purchaseList = new ProxyReaPurchae().Service.ReaQuery(qc);

            AllReaList = list;
            AllPurList = purchaseList;
            List<EntityReaStorage> dtOldRea = bsReaStorage.DataSource as List<EntityReaStorage>;

            bsReaStorage.ResetBindings(false);

            bsReaStorage.DataSource = list;

            gcReaStorage.RefreshDataSource();
            gvReaStorage.RefreshData();

            bsReaPurchase.ResetBindings(false);

            bsReaPurchase.DataSource = purchaseList;

            gcReaPurchase.RefreshDataSource();
            gvReaPurchase.RefreshData();

            RefreshItemsCount();//更新记录
            this.fpat_id = "";
            this.pur_id = "";
            txtPurno.Text = "";
            txtCount.Text = "";
            txtPrice.Text = "0";
            txtInvoice.Text = "";
            txtBatchNo.Text = "";

            txtBarcode.Text = "";

            RapitSearch();
            isLoadData = true;
        }
        private void RapitSearch()
        {
            List<EntityReaStorage> ReaList = AllReaList;
            List<EntityReaPurchase> PurList = AllPurList;
            if (AllReaList.Count == 0) return;

            if (this.cmbBarSearchPatType.EditValue != null
                && this.cmbBarSearchPatType.EditValue.ToString().Trim(null) != string.Empty
                && this.txtBarSearchCondition.Text.Trim(null) != string.Empty)
            {
                string searchField = this.cmbBarSearchPatType.EditValue.ToString();
                string searchValue = this.txtBarSearchCondition.Text;


                //条码号
                if (searchField == "rea_sid")
                {
                    ReaList = ReaList.Where(i => i.Rsr_no.Contains(searchValue)).ToList();
                }
                //采购单号
                if (searchField == "rpc_no")
                {
                    if (PurList != null && PurList.Count > 0)
                    {
                        PurList = PurList.Where(i => i.Rpc_no.Contains(searchValue)).ToList();

                    }
                }
            }
            else
            {
                ReaList = AllReaList;

            }
            bsReaStorage.DataSource = ReaList;
            bsReaPurchase.DataSource = PurList;
            RefreshItemsCount();//更新记录数量信息
            bsReaLst_PositionChanged(null, null);
        }
        /// <summary>
        /// 样本状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bsReaLst_PositionChanged(object sender, EventArgs e)
        {
            if (bsReaStorage.Current != null)
            {
                EntityReaStorage drLst = (EntityReaStorage)bsReaStorage.Current;

                if (drLst != null && !string.IsNullOrEmpty(drLst.Rsr_no))
                {
                    string pid = drLst.Rsr_no;
                    if (pid != CurrentReaID)
                    {
                        if (AnPatientChanging(CurrentReaID, pid, drLst))
                        {
                            CurrentReaID = pid;
                            this.GetReaDetailData(pid);
                            this.FillUiFromEntity();

                        }
                    }
                }
            }
        }
        public void FillUiFromEntity()
        {
            isDataChaged = false;

            if (CurrentStorageInfo == null || string.IsNullOrEmpty(CurrentStorageInfo.Rsr_no))
                return;

            fpat_id = CurrentStorageInfo.Rsr_no;
            var dr = CurrentStorageInfo;
            txtReaSid.Text = CurrentStorageInfo.Rsr_no;
            txtReaDate.DateTime = CurrentStorageInfo.Rsr_date ?? DateTime.MinValue;

            if (CurrentStorageInfo.Rsr_status == "2" && !string.IsNullOrEmpty(CurrentStorageInfo.Rsr_returnreason))
            {
                txtReason.Visible = true;
                txtReason.Text = CurrentStorageInfo.Rsr_returnreason;
            }
        }

        public void setDetail(List<EntityReaPurchaseDetail> resList)
        {
            List<EntityReaSetting> setList = bsReaSetting.DataSource as List<EntityReaSetting>;
            if (resList != null && resList.Count > 0)
            {
                if (string.Equals(radioGroup1.Text.Trim(), "0"))
                {
                    foreach (var item in resList)
                    {
                        for (int i = 0; i < item.StoreCount; i++)
                        {
                            EntityReaStorageDetail detail = new EntityReaStorageDetail();
                            EntityReaSetting setting = setList.Find(m => m.Drea_id == item.Rpcd_reaid);
                            detail.del_flag = 0;
                            detail.IsNew = 1;
                            detail.Rsd_status = "0";
                            detail.Rsd_conid = setting.Drea_condition;
                            detail.Rsd_groupid = setting.Drea_group;
                            detail.Rsd_no = txtReaSid.Text;
                            detail.Rsd_package = setting.Drea_package;
                            detail.Rsd_pdtid = setting.Drea_product;
                            detail.Rsd_posid = setting.Drea_position;
                            detail.Rsd_supid = setting.Drea_supplier;
                            detail.Rsd_unitid = setting.Drea_unit;
                            detail.Rsd_reaid = item.Rpcd_reaid;
                            detail.Rsd_reacount = 1;
                            detail.Rsd_purno = item.Rpcd_no;

                            detail.Rsd_batchno = item.BatchNo;
                            detail.Rsd_invoiceno = item.InvoiceNo;
                            detail.Rsd_outerpacking = item.OutPackage;
                            detail.Rsd_price = item.Rpcd_price;
                            detail.Rsd_report = item.Report;
                            detail.Rsd_temperature = item.Temparate;
                            detail.Rsd_validdate = item.ValidDate;
                            detail.ReagentName = item.ReagentName;
                            detail.Rsd_barcoderule = "0";
                            ProxyReaSetting proxy = new ProxyReaSetting();
                            string oldBarcode = proxy.Service.GetReaBarcodeByID(item.Rpcd_reaid);
                            string strbarcode = (string.IsNullOrEmpty(oldBarcode) ? "000001" : oldBarcode);
                            int barcode = 0;
                            if (int.TryParse(strbarcode, out barcode))
                            {
                                EntityReaSetting entity = new EntityReaSetting();
                                entity.Drea_id = item.Rpcd_reaid;
                                entity.Barcode = (barcode + 1).ToString().PadLeft(6, '0');
                                bool res = proxy.Service.UpdateBarcode(entity);
                                if (!res)
                                {
                                    lis.client.control.MessageDialog.Show("更新条码号失败！", "提示");
                                    txtBarcode.Focus();
                                    return;
                                }
                            }
                            else
                            {
                                lis.client.control.MessageDialog.Show("条码号不为整数！", "提示");
                                txtBarcode.Focus();
                                return;
                            }
                            detail.Rsd_barcode = item.Rpcd_reaid + strbarcode;
                            this.dtReaStorageDetail.Add(detail);
                        }

                    }
                }
                else
                {
                    foreach (var item in resList)
                    {
                        EntityReaStorageDetail detail = new EntityReaStorageDetail();
                        EntityReaSetting setting = setList.Find(m => m.Drea_id == item.Rpcd_reaid);
                        detail.del_flag = 0;
                        detail.IsNew = 1;
                        detail.Rsd_status = "0";
                        detail.Rsd_conid = setting.Drea_condition;
                        detail.Rsd_groupid = setting.Drea_group;
                        detail.Rsd_no = txtReaSid.Text;
                        detail.Rsd_package = setting.Drea_package;
                        detail.Rsd_pdtid = setting.Drea_product;
                        detail.Rsd_posid = setting.Drea_position;
                        detail.Rsd_supid = setting.Drea_supplier;
                        detail.Rsd_unitid = setting.Drea_unit;
                        detail.Rsd_reaid = item.Rpcd_reaid;
                        detail.Rsd_reacount = item.Rpcd_reacount;
                        detail.Rsd_count = item.Rpcd_reacount;
                        detail.Rsd_purno = item.Rpcd_no;

                        detail.Rsd_batchno = item.BatchNo;
                        detail.Rsd_invoiceno = item.InvoiceNo;
                        detail.Rsd_outerpacking = item.OutPackage;
                        detail.Rsd_price = item.Rpcd_price;
                        detail.Rsd_report = item.Report;
                        detail.Rsd_temperature = item.Temparate;
                        detail.Rsd_validdate = item.ValidDate;
                        detail.Rsd_barcoderule = "1";
                        ProxyReaSetting proxy = new ProxyReaSetting();
                        string oldBarcode = proxy.Service.GetReaBarcodeByID("100000");
                        DateTime dt = ServerDateTime.GetServerDateTime();
                        bool res = false;
                        string barcode;
                        if (string.Equals(dt.ToString("yyMMdd"), oldBarcode.Substring(0, 6)))
                        {
                            int num = Convert.ToInt32(oldBarcode.Substring(6, barcodefootlength)) + 1;
                            barcode = oldBarcode.Substring(0, 6) + num.ToString().PadLeft(barcodefootlength, '0');
                        }
                        else
                        {
                            barcode = dt.ToString("yyMMdd") + barcodeCustomFoot;
                            
                        }
                        EntityReaSetting entity = new EntityReaSetting();
                        entity.Drea_id = "1";
                        entity.Barcode = barcode;
                        detail.Rsd_barcode = barcode;
                        res = proxy.Service.UpdateBarcode(entity);
                        if (!res)
                        {
                            lis.client.control.MessageDialog.Show("更新条码号失败！", "提示");
                            txtBarcode.Focus();
                            return;
                        }
                        //string strbarcode = (string.IsNullOrEmpty(oldBarcode) ? "000001" : oldBarcode);
                        //int barcode = 0;
                        //if (int.TryParse(strbarcode, out barcode))
                        //{
                        //    EntityReaSetting entity = new EntityReaSetting();
                        //    entity.Drea_id = item.Rpcd_reaid;
                        //    entity.Barcode = (barcode + 1).ToString().PadLeft(6, '0');
                        //    bool res = proxy.Service.UpdateBarcode(entity);
                        //    if (!res)
                        //    {
                        //        lis.client.control.MessageDialog.Show("更新条码号失败！", "提示");
                        //        txtBarcode.Focus();
                        //        return;
                        //    }
                        //}
                        //else
                        //{
                        //    lis.client.control.MessageDialog.Show("条码号不为整数！", "提示");
                        //    txtBarcode.Focus();
                        //    return;
                        //}

                        this.dtReaStorageDetail.Add(detail);
                    }
                }
            }
            BindLookupData();
        }


        //根据ID得到数据
        private void GetReaDetailData(string _pat_id)
        {
            isDataChaged = false;

            EntityReaQC qc = new EntityReaQC();
            qc.ReaNo = _pat_id;
            var resList = new ProxyReaStorageDetail().Service.GetDetail(qc);
            dtReaStorageDetail = resList;
            CurrentStorageInfo.ListReaStorageDetail = dtReaStorageDetail;
            BindGrid();
        }
        bool AnPatientChanging(string prev_patid, string pat_id, EntityReaStorage drPat)
        {
            if (isDataChaged && isLoadData && (!string.IsNullOrEmpty(txtReaSid.Text)))
            {
                if (MessageDialog.Show("当前资料或结果未保存，是否保存？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SysToolBar1_OnBtnSaveClicked(null, null);
                }
            }
            return true;
        }
        /// <summary>
        /// 更新记录数量信息
        /// </summary>
        public void RefreshItemsCount()
        {
            int countUnAudited = 0;
            int countAudited = 0;
            int countPrinted = 0;
            int countReturned = 0;
            int countTotal = 0;

            if (this.bsReaStorage.DataSource != null)
            {
                List<EntityReaStorage> dtpat = this.bsReaStorage.DataSource as List<EntityReaStorage>;
                if (dtpat != null)
                {
                    countTotal = dtpat.Count;

                    foreach (var item in dtpat)
                    {
                        if (item.Rsr_status != null)
                        {
                            if (item.Rsr_status.ToString() == "4")
                            {
                                countAudited++;
                            }
                            else if (item.Rsr_status.ToString() == "0")
                            {
                                countUnAudited++;
                            }
                            else if (item.Rsr_status.ToString() == "2")
                            {
                                countReturned++;
                            }
                        }
                        if (item.Rsr_printflag.ToString() == "1")
                        {
                            countPrinted++;
                        }
                    }
                }
            }
            this.lbRecordCount.Text = string.Format("总数：{0} 已审核：{1} 未审核：{2} 已打印：{3} 未通过：{4}", countTotal, countAudited, countUnAudited, countPrinted, countReturned);
        }
        private void FrmReagenStorage_Load(object sender, EventArgs e)
        {
            radioGroup1.SelectedIndex = Convert.ToInt32(ReagentStoreBarcodeMode);
            List<EntityDicPubEvaluate> listEvaluate = CacheClient.GetCache<EntityDicPubEvaluate>();
            ctlRepositoryItemLookUpEdit7.DataSource = lookUpEdit1.Properties.DataSource = listEvaluate.
                FindAll(i => i.EvaFlag == "26").OrderBy(i => i.EvaSortNo).ToList();
            ctlRepositoryItemLookUpEdit9.DataSource = lookUpEdit3.Properties.DataSource = listEvaluate.
                FindAll(i => i.EvaFlag == "27").OrderBy(i => i.EvaSortNo).ToList();
            ctlRepositoryItemLookUpEdit8.DataSource = lookUpEdit2.Properties.DataSource = listEvaluate.
                FindAll(i => i.EvaFlag == "25").OrderBy(i => i.EvaSortNo).ToList();
            lookUpEdit1.Text = lookUpEdit2.Text = lookUpEdit3.Text = "合格";
            LoadUserSetting();
            ApplySetting();
            dtBegin.EditValue = DateTime.Now;
            dtEnd.EditValue = DateTime.Now;
            string range = UserInfo.GetSysConfigValue("Rea_DateRange");
            InitSelection();
            List<EntityDicReaSupplier> supList = CacheClient.GetCache<EntityDicReaSupplier>();
            List<EntityReaSetting> setList = CacheClient.GetCache<EntityReaSetting>();
            List<EntityDicReaGroup> grpList = CacheClient.GetCache<EntityDicReaGroup>();
            List<EntityDicReaProduct> pdtList = CacheClient.GetCache<EntityDicReaProduct>();
            List<EntityDicReaUnit> unitList = CacheClient.GetCache<EntityDicReaUnit>();
            List<EntityDicReaStorePos> posList = CacheClient.GetCache<EntityDicReaStorePos>();
            List<EntityDicReaStoreCondition> conList = CacheClient.GetCache<EntityDicReaStoreCondition>();

            bsSup.DataSource = supList;
            bsReaSetting.DataSource = setList;
            bsGroup.DataSource = grpList;
            bsPdt.DataSource = pdtList;
            bsUnit.DataSource = unitList;
            bsPos.DataSource = posList;
            bsCon.DataSource = conList;
            DateTime dtServer = ServerDateTime.GetServerDateTime();
            txtReaDate.DateTime = dtServer;
            if (!string.IsNullOrEmpty(range))
            {
                dtBegin.EditValue = dtServer.AddDays(-(Convert.ToInt32(range) - 1));
            }
            sysToolBar1.OrderCustomer = true;
            sysToolBar1.BtnDelete.Caption = "删除";
            sysToolBar1.BtnUndo.Caption = "取消审核";
            sysToolBar1.BtnAnswer.Caption = "撤销入库";
            sysToolBar1.SetToolButtonStyle(new string[] {sysToolBar1.BtnAdd.Name,
                                                sysToolBar1.BtnSave.Name,
                                                sysToolBar1.BtnBCPrint.Name,            //打印条码
                                                sysToolBar1.BtnDelete.Name,
                                                sysToolBar1.BtnAnswer.Name,
                                                sysToolBar1.BtnRefresh.Name,
                                                sysToolBar1.BtnAudit.Name,
                                                sysToolBar1.BtnPrint.Name,
                                                sysToolBar1.BtnPrintPreview2.Name,
                                                sysToolBar1.BtnPrintSet.Name,           //设置
                                                sysToolBar1.BtnImport.Name,
                                                sysToolBar1.BtnClose.Name});
            BindLookupData();
        }



        private void roundPanelGroup1_Click(object sender, EventArgs e)
        {
            RoundPanel curRp = roundPanelGroup1.GetCurRoundPanel();
            if (curRp.Tag == null)
                return;
            int pageIndex = ConvertHelper.IntParse(curRp.Tag?.ToString(), 0);
            SelectedPageChanged(pageIndex);
        }

        private void dtBegin_EditValueChanged(object sender, EventArgs e)
        {
            if (dtEnd.EditValue == null)
                return;
            SearchPatientsAndAddNew();
        }

        private void dtEnd_EditValueChanged(object sender, EventArgs e)
        {
            if (dtBegin.EditValue == null)
                return;
            SearchPatientsAndAddNew();
        }

        private void gvReaStorage_Click(object sender, EventArgs e)
        {
            if (bsReaStorage.Current != null)
            {
                gvReaStorage.CloseEditor();
                EntityReaStorage drLst = (EntityReaStorage)bsReaStorage.Current;

                txtPackage.Visible = false;
                CurrentStorageInfo = drLst;
                CurrentReaID = drLst.Rsr_no;
                this.GetReaDetailData(CurrentReaID);
                this.FillUiFromEntity();
                isDataChaged = false;
            }
        }

        private void 面板设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmReaPanelConfig frm = new FrmReaPanelConfig(this);
            frm.Show();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (!checkUI()) { return; }
            int count = 0;
            if (int.TryParse(txtCount.Text, out count))
            {
                if (count <= 0)
                {
                    lis.client.control.MessageDialog.Show("入库数量不能为0！", "提示");
                    txtCount.Focus();
                    return;
                }
            }
            else
            {
                lis.client.control.MessageDialog.Show("入库数量填写不规范！", "提示");
                txtCount.Focus();
                return;
            }
            decimal price = 0;
            if (decimal.TryParse(txtPrice.Text, out price))
            {
                if (ReagentInPriceGreat0 && price <= 0)
                {
                    lis.client.control.MessageDialog.Show("价格不能为0！", "提示");
                    txtPrice.Focus();
                    return;
                }
            }
            else
            {
                lis.client.control.MessageDialog.Show("价格填写不规范！", "提示");
                txtPrice.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtBarcode.Text))
            {
                if (string.Equals(radioGroup1.Text.Trim(), "0"))
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (!GetBarcode()) { return; }
                        EntityReaStorageDetail detail = FillDetail(1);
                        detail.Rsd_price = price;

                        this.dtReaStorageDetail.Add(detail);
                    }
                }
                else
                {
                    if (!GetBarcode()) { return; }
                    EntityReaStorageDetail detail = FillDetail(count);

                    detail.Rsd_price = price;
                    this.dtReaStorageDetail.Add(detail);
                }
            }
            else
            {
                EntityReaStorageDetail detail = FillDetail(1);
                detail.Rsd_price = price;

                this.dtReaStorageDetail.Add(detail);
            }
            txtBarcode.Text = "";
            BindLookupData();
        }

        public bool GetBarcode()
        {
            if (string.Equals(radioGroup1.Text.Trim(), "0"))
            {
                ProxyReaSetting proxy = new ProxyReaSetting();
                string oldBarcode = proxy.Service.GetReaBarcodeByID(selectDicReaSetting1.valueMember);
                string strbarcode = (string.IsNullOrEmpty(oldBarcode) ? "000001" : oldBarcode);
                int barcode = 0;
                if (int.TryParse(strbarcode, out barcode))
                {
                    EntityReaSetting entity = new EntityReaSetting();
                    entity.Drea_id = selectDicReaSetting1.valueMember;
                    entity.Barcode = (barcode + 1).ToString().PadLeft(6, '0');
                    bool res = proxy.Service.UpdateBarcode(entity);
                    if (!res)
                    {
                        lis.client.control.MessageDialog.Show("更新条码号失败！", "提示");
                        txtBarcode.Focus();
                        return false;
                    }
                }
                else
                {
                    lis.client.control.MessageDialog.Show("条码号不为整数！", "提示");
                    txtBarcode.Focus();
                    return false;
                }
                txtBarcode.Text = selectDicReaSetting1.valueMember + strbarcode;

                if (string.IsNullOrEmpty(txtBarcode.Text))
                {
                    lis.client.control.MessageDialog.Show("条码号不能为空！", "提示");
                    txtBarcode.Focus();
                    return false;
                }
                return true;
            }
            else
            {
                ProxyReaSetting proxy = new ProxyReaSetting();
                string oldBarcode = proxy.Service.GetReaBarcodeByID("100000");
                DateTime dt = ServerDateTime.GetServerDateTime();
                bool res = false;
                string barcode;
                if (string.Equals(dt.ToString("yyMMdd"), oldBarcode.Substring(0, 6)))
                {
                    int num = Convert.ToInt32(oldBarcode.Substring(6, barcodefootlength)) + 1;
                    barcode = oldBarcode.Substring(0, 6) + num.ToString().PadLeft(barcodefootlength, '0');
                }
                else
                {
                    barcode = dt.ToString("yyMMdd") + barcodeCustomFoot;

                }
                EntityReaSetting entity = new EntityReaSetting();
                entity.Drea_id = "1";
                entity.Barcode = barcode;
                txtBarcode.Text = barcode;
                res = proxy.Service.UpdateBarcode(entity);
                if (!res)
                {
                    lis.client.control.MessageDialog.Show("更新条码号失败！", "提示");
                    txtBarcode.Focus();
                    return false;
                }
                
                return true;
            }
                
        }

        public EntityReaStorageDetail FillDetail(int count)
        {
            EntityReaStorageDetail detail = new EntityReaStorageDetail();
            detail.del_flag = 0;
            detail.IsNew = 1;
            detail.Rsd_reacount = count;
            detail.Rsd_count = count;
            detail.Rsd_barcode = txtBarcode.Text;
            detail.Rsd_batchno = txtBatchNo.Text;
            detail.Rsd_conid = selectDicReaStoreCondition1.valueMember;
            detail.Rsd_groupid = selectDicReaGroup2.valueMember;
            detail.Rsd_invoiceno = txtInvoice.Text;
            detail.Rsd_no = txtReaSid.Text;
            detail.Rsd_outerpacking = lookUpEdit1.Text;
            detail.Rsd_package = txtPackage.Text;
            detail.Rsd_pdtid = selectDicReaProduct1.valueMember;
            detail.Rsd_posid = selectDicReaStorePosition1.valueMember;
            detail.Rsd_barcoderule = radioGroup1.Text.Trim();
            detail.Rsd_report = lookUpEdit3.Text;
            detail.Rsd_supid = selectDicReaSupplier2.valueMember;
            detail.Rsd_temperature = lookUpEdit2.Text;
            detail.Rsd_unitid = selectDicReaUnit1.valueMember;
            detail.Rsd_validdate = dateEdit2.DateTime;
            detail.Rsd_reaid = selectDicReaSetting1.valueMember;
            detail.Rsd_status = "0";
            detail.Rsd_purno = txtPurno.Text;
            detail.ConName = selectDicReaStoreCondition1.displayMember;
            detail.GroupName = selectDicReaGroup2.displayMember;
            detail.PdtName = selectDicReaProduct1.displayMember;
            detail.PosName = selectDicReaStorePosition1.displayMember;
            detail.ReagentName = selectDicReaSetting1.displayMember;
            detail.SupName = selectDicReaSupplier2.displayMember;
            detail.UnitName = selectDicReaUnit1.displayMember;
            return detail;
        }

        public bool checkUI()
        {
            bool check = false;
            if (string.IsNullOrEmpty(txtReaSid.Text))
            {
                MessageDialog.Show("入库单号不能为空！", "提示");
                check = false;
                return check;
            }
            if (string.IsNullOrEmpty(selectDicReaSetting1.valueMember))
            {
                MessageDialog.Show("试剂信息不能为空！", "提示");
                check = false;
                return check;
            }
            if (string.IsNullOrEmpty(selectDicReaGroup2.valueMember))
            {
                MessageDialog.Show("试剂组别不能为空！", "提示");
                check = false;
                return check;
            }
            //if (string.IsNullOrEmpty(txtPackage.Text))
            //{
            //    MessageDialog.Show("试剂规格不能为空！", "提示");
            //    check = false;
            //    return check;
            //}
            if (string.IsNullOrEmpty(txtBatchNo.Text))
            {
                MessageDialog.Show("批号不能为空！", "提示");
                check = false;
                return check;
            }
            if (string.IsNullOrEmpty(selectDicReaSupplier2.valueMember))
            {
                MessageDialog.Show("供应商不能为空！", "提示");
                check = false;
                return check;
            }
            if (string.IsNullOrEmpty(selectDicReaUnit1.valueMember))
            {
                MessageDialog.Show("单位不能为空！", "提示");
                check = false;
                return check;
            }
            if (string.IsNullOrEmpty(lookUpEdit1.Text))
            {
                MessageDialog.Show("外包装不能为空！", "提示");
                check = false;
                return check;
            }
            if (string.IsNullOrEmpty(lookUpEdit2.Text))
            {
                MessageDialog.Show("到达温度不能为空！", "提示");
                check = false;
                return check;
            }
            if (string.IsNullOrEmpty(lookUpEdit3.Text))
            {
                MessageDialog.Show("检验报告不能为空！", "提示");
                check = false;
                return check;
            }
            if (dateEdit2.DateTime < DateTime.Now)
            {
                MessageDialog.Show("有效日期不能小于今天！", "提示");
                check = false;
                return check;
            }

            check = true;
            return check;
        }

        private void selectDicReaSetting1_ValueChanged(object sender, dcl.client.control.ValueChangeEventArgs args)
        {
            List<EntityReaSetting> setList = bsReaSetting.DataSource as List<EntityReaSetting>;
            EntityReaSetting item = setList.Find(m => m.Drea_id == selectDicReaSetting1.valueMember);
            selectDicReaGroup2.valueMember = item.Drea_group;
            selectDicReaProduct1.valueMember = item.Drea_product;
            selectDicReaStoreCondition1.valueMember = item.Drea_condition;
            selectDicReaStorePosition1.valueMember = item.Drea_position;
            selectDicReaSupplier2.valueMember = item.Drea_supplier;
            selectDicReaUnit1.valueMember = item.Drea_unit;
            txtPackage.Text = item.Drea_package;
            txtCount.Text = "";
            txtPrice.Text = "0";
            txtInvoice.Text = "";
            txtBatchNo.Text = "";
            txtBarcode.Text = "";
        }

        private void contextMenuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            this.CloseEditor();

            ContextMenuStrip menu = (sender as ContextMenuStrip);

            menu.Visible = false;
            switch (e.ClickedItem.Name)
            {
                case "MenuDelItem":
                    MenuDelItemClick(gvReadetail, true);
                    break;
            }
        }
        #region 删除项目
        /// <summary>
        /// 菜单删除项目
        /// </summary>
        /// <param name="sourceGrid"></param>
        /// <param name="bNeedConfirm">是否需要提示确认</param>
        private void MenuDelItemClick(GridView sourceGrid, bool bNeedConfirm)
        {
            EntityReaQC qc = new EntityReaQC();
            qc.ReaNo = txtReaSid.Text;
            //获取当前记录的审核状态
            ProxyReaStorage mainProxy = new ProxyReaStorage();
            List<EntityReaStorage> list = mainProxy.Service.ReaQuery(qc);

            if (list != null && list.Count > 0 && list[0].Rsr_status != "1" && list[0].Rsr_status != "2")
            {
                lis.client.control.MessageDialog.Show("当前记录已审核，不能删除", "提示");
            }
            else
            {
                int[] selectedRowHandler = sourceGrid.GetSelectedRows();

                if (selectedRowHandler.Length == 0)
                {
                    return;
                }
                List<EntityReaStorageDetail> selectDataRows = new List<EntityReaStorageDetail>();
                bool needComma = false;
                string tipItemsText = string.Empty;
                foreach (int rowHandler in selectedRowHandler)
                {
                    EntityReaStorageDetail row = sourceGrid.GetRow(rowHandler) as EntityReaStorageDetail;
                    if (row != null)
                    {
                        selectDataRows.Add(row);
                    }

                    if (needComma)
                    {
                        tipItemsText += "，";
                    }

                    tipItemsText += string.Format("[{0}]", row.ReagentName.TrimEnd().ToString());

                    needComma = true;
                }

                string messagetips = "是否要移除项目{0}？\r\n{1}";

                if (UserInfo.GetSysConfigValue("PatEnterItemDeleteMode") == "立刻从数据库删除") //是否立刻从数据库删除
                {
                    messagetips = string.Format(messagetips, tipItemsText, "当前的配置为：[确定]后立刻从数据库删除");
                }
                else
                {
                    messagetips = string.Format(messagetips, tipItemsText, "当前的配置为：[确定]后需要点击[保存]才从数据库中删除");
                }

                if (!bNeedConfirm
                    || lis.client.control.MessageDialog.Show(messagetips, "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    RemoveItem(selectDataRows, true);
                }
            }
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        public void RemoveItem(List<EntityReaStorageDetail> rowsPatResultItem, bool removeHasResult)
        {
            bool deleteInDatabase = false;
            if (UserInfo.GetSysConfigValue("PatEnterItemDeleteMode") == "立刻从数据库删除")
            {
                deleteInDatabase = true;
            }

            for (int i = 0; i < rowsPatResultItem.Count; i++)
            {
                EntityReaStorageDetail drPatResultItem = rowsPatResultItem[i];
                //是否已录入结果
                bool hasResult = false;
                if (drPatResultItem.Rsd_reacount > 0)
                {
                    hasResult = true;
                }
                //数据库是否存在结果
                bool recordInDataBase = false;
                if (drPatResultItem.ObrSn != 0)
                {
                    recordInDataBase = true;
                }
                if (hasResult)
                {
                    if (removeHasResult && recordInDataBase)
                    {
                        if (!Compare.IsEmpty(drPatResultItem.Rsd_no) && !Compare.IsEmpty(drPatResultItem.ObrSn))
                        {
                            if (deleteInDatabase)
                            {

                                EntityLogLogin logLogin = new EntityLogLogin();
                                logLogin.LogIP = UserInfo.ip;
                                logLogin.LogLoginID = UserInfo.loginID;

                                string rsdno = drPatResultItem.Rsd_no.ToString();
                                string rea_name = drPatResultItem.ReagentName.TrimEnd().ToString();
                                string rea_itm_id = string.Empty;
                                if (!Compare.IsEmpty(drPatResultItem.Rsd_reaid))
                                {
                                    rea_itm_id = drPatResultItem.Rsd_reaid.ToString();
                                }

                                long reskey = -1;

                                bool opResult = false;
                                if (!Compare.IsEmpty(drPatResultItem.ObrSn))
                                {
                                    reskey = Convert.ToInt64(drPatResultItem.ObrSn);
                                }

                                if (rea_itm_id == string.Empty)
                                {

                                }
                                else if (reskey != -1)
                                {
                                    opResult = new ProxyReaStorageDetail().Service.DeleteCommonResultItemByObrSn(logLogin, reskey.ToString(), rsdno);
                                }
                                else
                                {
                                    rowsPatResultItem.Remove(drPatResultItem);
                                    i--;
                                    this.dtReaStorageDetail.Remove(drPatResultItem);
                                }


                                if (!opResult)
                                {
                                    MessageDialog.Show(string.Format("删除[{0}]失败！", rea_name), "错误");
                                }
                                else
                                {
                                    rowsPatResultItem.Remove(drPatResultItem);
                                    i--;
                                    int deleteIndex = dtReaStorageDetail.FindIndex(w => w.ObrSn == drPatResultItem.ObrSn);
                                    if (deleteIndex > -1)
                                        this.dtReaStorageDetail.RemoveAt(deleteIndex);
                                }
                            }
                            else
                            {
                                drPatResultItem.IsNew = 2;
                            }
                        }
                        else
                        {
                            rowsPatResultItem.Remove(drPatResultItem);
                            this.dtReaStorageDetail.Remove(drPatResultItem);
                            i--;
                        }
                    }
                    else
                    {
                        if (!recordInDataBase)
                        {
                            rowsPatResultItem.Remove(drPatResultItem);
                            this.dtReaStorageDetail.Remove(drPatResultItem);
                        }

                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(drPatResultItem.Rsd_no) && drPatResultItem.ObrSn != 0)
                    {
                        if (deleteInDatabase)
                        {
                            EntityLogLogin logLogin = new EntityLogLogin();
                            logLogin.LogIP = UserInfo.ip;
                            logLogin.LogLoginID = UserInfo.loginID;

                            string resid = drPatResultItem.Rsd_no.ToString();
                            string res_itm_ecd = drPatResultItem.ReagentName.TrimEnd().ToString();
                            string res_itm_id = string.Empty;
                            if (!Compare.IsEmpty(drPatResultItem.Rsd_reaid))
                            {
                                res_itm_id = drPatResultItem.Rsd_reaid.ToString();
                            }

                            long reskey = -1;

                            bool opResult = false;
                            if (!Compare.IsEmpty(drPatResultItem.ObrSn))
                            {
                                reskey = Convert.ToInt64(drPatResultItem.ObrSn);
                            }

                            if (res_itm_id == string.Empty)
                            {

                            }
                            else if (reskey != 0)
                            {
                                opResult = new ProxyReaStorageDetail().Service.DeleteCommonResultItemByObrSn(logLogin, reskey.ToString(), resid);
                            }
                            else
                            {
                                rowsPatResultItem.Remove(drPatResultItem);
                            }


                            if (!opResult)
                            {
                                MessageDialog.Show(string.Format("删除[{0}]失败！", res_itm_ecd), "错误");
                            }
                            else
                            {
                                rowsPatResultItem.Remove(drPatResultItem);

                                int deleteIndex = dtReaStorageDetail.FindIndex(w => w.ObrSn == drPatResultItem.ObrSn);
                                if (deleteIndex > -1)
                                    this.dtReaStorageDetail.RemoveAt(deleteIndex);
                            }

                        }
                        else
                        {
                            drPatResultItem.IsNew = 2;
                        }
                    }
                    else
                    {
                        rowsPatResultItem.Remove(drPatResultItem);
                        i--;
                        this.dtReaStorageDetail.Remove(drPatResultItem);
                    }
                }
            }
            this.gcReaDetail.RefreshDataSource();
            this.BindGrid();

        }
        #endregion

        private void gvReaPurchase_DoubleClick(object sender, EventArgs e)
        {
            if (bsReaPurchase.Current != null)
            {
                if (dtReaStorageDetail.Count > 0)
                {
                    lis.client.control.MessageDialog.Show("已有采购订单被添加！", "提示");
                    return;
                }
                EntityReaPurchase entity = bsReaPurchase.Current as EntityReaPurchase;
                EntityReaQC qc = new EntityReaQC();
                qc.ReaNo = entity.Rpc_no;
                var resList = new ProxyReaPurchaseDetail().Service.GetDetail(qc);
                FrmPurDetail frm = new FrmPurDetail();
                frm.detailList = resList;
                DialogResult dig = frm.ShowDialog();
                if (dig != DialogResult.OK)
                {
                    return;
                }
                List<EntityReaPurchaseDetail> list = new List<EntityReaPurchaseDetail>();
                list = frm.selectDataRows;

                setDetail(list);
            }
        }
    }
}