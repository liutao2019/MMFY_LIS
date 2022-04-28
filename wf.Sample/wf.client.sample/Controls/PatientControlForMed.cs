using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using dcl.client.frame;
using dcl.client.common;
using dcl.common.extensions;
using DevExpress.XtraGrid.Columns;
using dcl.client.wcf;
using dcl.common;
using DevExpress.XtraEditors;
using dcl.client.report;
using System.IO;
using lis.client.control;
using dcl.root.logon;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using dcl.entity;
using System.Linq;
using System.Text.RegularExpressions;
using dcl.client.cache;
using DevExpress.XtraEditors.Controls;

namespace dcl.client.sample
{
    public partial class PatientControlForMed : XtraUserControl, IStepable
    {
        public event EventHandler ClearAllClick;
        public event EventHandler DoShowBaseInfo;

        string xmlFile = PathManager.SettingLisPath + @"\printXml\barcodePrintConfig.xml";
        string xmlLisFile = PathManager.SettingLisPath + @"\printXml\printConfig.xml";

        public List<EntitySampMain> ListSampMain = new List<EntitySampMain>();

        public List<EntityDicSample> listDicSample = new List<EntityDicSample>(); //装载标本数据

        private List<EntitySampDetail> ListSampDetail = new List<EntitySampDetail>();
        private StepType stepType;

        private SelectType selectType;

        string Barcode_ZYShouldSamplingConfirm;
        string Barcode_TJShouldSamplingConfirm;
        string Barcode_MZShouldSamplingConfirm;
        //条码信息颜色显示方式
        string BarcodeColorStyle;
        ProxySampMain proxy = null;
        private bool preBarCodeCheckCuvType = false;
        public bool CheckPower = true;
        internal void SetInfoDisVisable()
        {
            spccMain.CollapsePanel = DevExpress.XtraEditors.SplitCollapsePanel.Panel2;
            this.spccMain.Collapsed = true;
        }

        internal void SetInfoDisVisableForSearch(string lbHeaderText = "已送达:")
        {
            lbHeader.Text = lbHeaderText;
            spccMain.CollapsePanel = DevExpress.XtraEditors.SplitCollapsePanel.Panel2;
            this.spccMain.Collapsed = false;
            gridColumn32.VisibleIndex = gridColumn52.VisibleIndex + 1;
        }

        internal void SetInfoDisVisableForConfirm(string lbHeaderText = "条码数:")
        {
            lbHeader.Text = lbHeaderText;
            this.spccMain.Collapsed = true;
            gcBaseInfo.Visible = true;
            //标本流转是否显示时间轴 没有配置，默认显示
            if (ConfigHelper.GetSysConfigValueWithoutLogin("BCConfrm_ShowTimeLine") != "否")
            {
                //pnlTimeLine.Visible = true;
                //ucTimeLine1 = new ucTimeLine();
                //pnlTimeLine.Controls.Add(ucTimeLine1);
                //ucTimeLine1.Dock = DockStyle.Fill;
            }
            else
            {

            }
            gridColumn12.VisibleIndex = gridColumn10.VisibleIndex + 1;
        }

        string Barcode_ZYShouldCollectConfirm;
        string Barcode_TJShouldCollectConfirm;
        private string BC_MZFilterCTypeForPrintReturn;
        public bool isNotUpdateFlag = false;

        string Barcode_ZYShouldArriveConfirm;
        string Barcode_TJShouldArriveConfirm;

        //是否启用住院可重复打印已打印条码权限判断
        bool Barcode_EnablePrintAuthoryJudge = true;
        //是否启用删除体检条码权限判断
        bool Barcode_EnableDeleteTJBarcodeJudge = true;
        private bool Barcode_RecordSignPlace = false;


        private bool Enable_CombineUrgentFlag = false;

        //条码送达与签收界面强制限制目的地
        private bool BC_ForceSendDestFlag = false;

        /// <summary>
        /// 是否打印条码回执
        /// </summary>
        public bool m_blnCheckManualReturn = false;

        private bool BC_CanEditSex = false;
        /// <summary>
        /// 外部传入的病人来源ID
        /// </summary>
        public string m_strOriId = "";

        public bool IsAlone = false;

        public bool isResetBarcode = false; //是否重置条码

        /// <summary>
        /// 急查颜色
        /// </summary>
        Color corUrgent = Color.White;
        Color corBD = Color.White;

        private List<EntitySampProcessDetail> ListSampProcess = new List<EntitySampProcessDetail>();
        /// <summary>
        /// 条码Grid控件
        /// </summary>
        public GridControl MainGrid { get { return this.gcBarcode; } }
        /// <summary>
        /// 条码GridView
        /// </summary>
        public GridView MainGridView { get { return this.gvBarcode; } }
        /// <summary>
        /// 条码表
        /// </summary>
        public List<EntitySampMain> MainTable { get { return ListSampMain; } }
        public bool ShouldMultiSelect { get; set; }
        public bool SelectWhenNotPrint { get; set; }
        string Interface_HospitalInterfaceMode;
        /// <summary>
        /// 是否手工条码调用
        /// </summary>
        public bool IsUseBCManual = false;
        public IStep Step { get; set; }
        public bool ShowCollectNotice { get; set; }
        internal GridCheckMarksSelection Selection { get; set; }

        /// <summary>
        /// 院感条码
        /// </summary>
        public bool IsYG = false;
        //打印时对未设置对应HIS代码的组合的条码进行提示
        bool Bar_NoHisCodeNotPrint = false;
        /// <summary>
        /// 签收时是否显示组合提示状态颜色列
        /// </summary>
        bool Barcode_ShowComLineStatus = false;

        private bool Barcode_SignEnableBitchModifySamp;

        [System.Runtime.InteropServices.DllImport("winmm.dll")]
        private static extern bool PlaySound(string pszSound, int hmod, int fdwSound);

        private const int SND_FILENAME = 0x00020000; public const int SND_ASYNC = 0x0001;

        /// <summary>
        /// 当前条码行
        /// </summary>
        public EntitySampMain CurrentSampMain
        {
            get
            {
                if (bsPatient.Current != null)
                    return (EntitySampMain)gvBarcode.GetFocusedRow();//.Current;
                else
                    return null;
            }
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

        public SelectType SelectType
        {
            get { return selectType; }
            set
            {
                selectType = value;
            }
        }

        public EntitySampMain BaseSampMain
        {
            get
            {
                if (Step.BaseSampMain == null)
                    Step.BaseSampMain = GetBaseInfo();

                return Step.BaseSampMain;
            }
        }

        /// <summary>
        /// 显示送达者与签收者列名
        /// </summary>
        public void showColReachAndReceiverForName(bool IsVisible)
        {
            gridColumn42.Visible = IsVisible;
            gridColumn33.Visible = IsVisible;
            gridColumnSendname.Visible = IsVisible;
            gridColumnBloodname.Visible = IsVisible;
            gridColumnCommFlag.Visible = IsVisible;
            gridColumnCommFlag.VisibleIndex = 1;
            gridColumn57.Visible = IsVisible;
        }

        private void OnClearAll(object sender, EventArgs e)
        {
            if (ClearAllClick != null)
            {
                this.ClearAllClick(sender, e);
            }
        }

        private void OnShowBaseInfo(object sender, EventArgs e)
        {
            if (DoShowBaseInfo != null)
            {
                this.DoShowBaseInfo(sender, e);
            }
        }

        private string Barcode_ReuturnPrinter = "条码";


        public static Color GetBarcodeConfigColor(string configCode)
        {
            string cfgValue = ConfigHelper.GetSysConfigValueWithoutLogin(configCode);

            Color col;
            //黑色,红色,灰色,蓝色,绿色,紫色
            switch (cfgValue)
            {
                case "黑色":
                    col = Color.Black;
                    break;

                case "红色":
                    col = Color.Red;
                    break;

                case "灰色":
                    col = Color.DarkGray;
                    break;
                case "粉红色":
                    col = Color.Pink;
                    break;
                case "黄色":
                    col = Color.Yellow;
                    break;
                case "蓝色":
                    col = Color.Blue;
                    break;

                case "绿色":
                    col = Color.Green;
                    break;

                case "紫色":
                    col = Color.Purple;
                    break;
                case "白色":
                    col = Color.White;
                    break;
                case "棕色":
                    col = Color.Brown;
                    break;

                default:
                    col = Color.Black;
                    break;
            }
            return col;
        }

        public void SetContextMenuStrip()
        {
            gcBarcode.ContextMenuStrip = contextMenuStrip1;
        }

        public bool ShouldShowIndicator = false;

        void gvBarcode_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void InitSelection()
        {
            if (ShouldMultiSelect)
            {
                MainGridView.ExpandAllGroups();
                Selection = new GridCheckMarksSelection(MainGridView);
                Selection.CheckMarkColumn.Width = 20;
                Selection.CheckMarkColumn.VisibleIndex = 0;
            }
        }

        public string PrintMachineName = "";

        /// <summary>
        /// 显示注意事项
        /// </summary>
        /// <param name="showCollectNotice">True:显示打印采血注意,False:显示保存注意</param>
        public void ShowNotice(bool showCollectNotice)
        {
            if (showCollectNotice)
            {
                GridColumn column = gvCname.Columns.ColumnByName("gridColumnSaveNotice");
                if (column != null)
                    column.Visible = false;
            }

            else
            {
                GridColumn column = gvCname.Columns.ColumnByName("gridColumnCollectNotice");
                if (column != null)
                    column.Visible = false;
            }
        }

        public PatientControlForMed()
        {
            InitializeComponent();
            this.Load += PatientControl_Load;

            gcBarcode.DoubleClick += gcBarcode_DoubleClick;
            gvBarcode.RowCountChanged += GvBarcode_RowCountChanged;
            gvBarcode.RowStyle += gvBarcode_RowStyle;
            gvBarcode.Click += gvBarcode_Click;
            gvBarcode.FocusedRowChanged += gvBarcode_FocusedRowChanged;

            this.selectDict_Sample1.onAfterSelected += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicSample>.afterSelected(this.selectDict_Sample1_onAfterSelected);
            this.selectDict_Sample_Remarks1.onAfterSelected += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicSampRemark>.afterSelected(this.selectDict_Sample_Remarks1_onAfterSelected);
            this.checkMan.Click += new System.EventHandler(this.checkMan_Click);
            this.txtAge.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAge_KeyDown);
            this.txtSex.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSex_KeyDown);
            this.gvCname.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvCname_RowCellStyle);
        }

        private void GvBarcode_RowCountChanged(object sender, EventArgs e)
        {
            UpdateCount();
        }

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


        private void PatientControl_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            proxy = new ProxySampMain();

            List<EntityDicSample> listSample = CacheClient.GetCache<EntityDicSample>();
            List<EntityDicTestTube> listTube = CacheClient.GetCache<EntityDicTestTube>();

            this.bsSampleType.DataSource = listSample;
            this.bsCuv.DataSource = listTube;

            Step.FormatRow(gvBarcode);

            InitSelection();
            Barcode_ReuturnPrinter = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_ReuturnPrinter");
            Enable_CombineUrgentFlag = ConfigHelper.GetSysConfigValueWithoutLogin("Enable_CombineUrgentFlag") == "是";
            Barcode_SignEnableBitchModifySamp = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_SignEnableBitchModifySamp") == "是";
            BC_CanEditSex = ConfigHelper.GetSysConfigValueWithoutLogin("BC_CanEditSex") == "是";
            preBarCodeCheckCuvType = ConfigHelper.GetSysConfigValueWithoutLogin("BarCode_PreBarCodeCheckCuvType") == "是";
            Interface_HospitalInterfaceMode = ConfigHelper.GetSysConfigValueWithoutLogin("Interface_HospitalInterfaceMode");
            if (BC_CanEditSex)
            {
                txtSex.Properties.ReadOnly = false;
            }

            BC_ForceSendDestFlag = ConfigHelper.GetSysConfigValueWithoutLogin("BC_ForceSendDestFlag") == "是";

            bsCType.DataSource = CacheClient.GetCache<EntityDicPubProfession>();

            Barcode_RecordSignPlace = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_RecordSignPlace") == "是";

            //条码打印可更改标本类别
            if (StepType != StepType.Print && stepType != StepType.Select)
            {
                selectDict_Sample1.Readonly = true;
                selectDict_Sample_Remarks1.Readonly = true;
                checkMan.Visible = false;
                cbMan.Visible = false;

                txtCurrentAddress.ReadOnly = true;
                txtIdentifierID.ReadOnly = true;
                selectPatType1.Readonly = true;
                selectIdentifierType1.Readonly = true;
            }

            if (stepType == StepType.Sampling || stepType == StepType.Confirm)
                selectDict_Sample_Remarks1.Readonly = false;


            if (ShouldShowIndicator)
            {
                gvBarcode.OptionsView.ShowIndicator = true;

                gvBarcode.IndicatorWidth = 40;
                gvBarcode.CustomDrawRowIndicator += new RowIndicatorCustomDrawEventHandler(gvBarcode_CustomDrawRowIndicator);

            }

            Barcode_EnablePrintAuthoryJudge = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_EnablePrintAuthoryJudge") == "是";
            Barcode_EnableDeleteTJBarcodeJudge = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_EnableDeleteTJBarcodeJudge") == "是";
            Barcode_ZYShouldSamplingConfirm = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_ZYShouldSamplingConfirm");
            Barcode_TJShouldSamplingConfirm = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_TJShouldSamplingConfirm");
            Barcode_MZShouldSamplingConfirm = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_MZShouldSamplingConfirm");

            Barcode_ZYShouldCollectConfirm = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_ZYShouldCollectConfirm");
            Barcode_TJShouldCollectConfirm = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_TJShouldCollectConfirm");
            BC_MZFilterCTypeForPrintReturn = ConfigHelper.GetSysConfigValueWithoutLogin("BC_MZFilterCTypeForPrintReturn");
            Barcode_ZYShouldArriveConfirm = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_ZYShouldArriveConfirm");
            Barcode_TJShouldArriveConfirm = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_TJShouldArriveConfirm");
            Bar_NoHisCodeNotPrint = ConfigHelper.GetSysConfigValueWithoutLogin("Bar_NoHisCodeNotPrint") == "是";

            Barcode_ShowComLineStatus = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_ShowComLineStatus") == "是";

            //条码信息颜色显示方式
            BarcodeColorStyle = ConfigHelper.GetSysConfigValueWithoutLogin("BarcodeInfo_ColorStyle");
            corUrgent = GetBarcodeConfigColor("New_Barcode_Color_Urgent");
            corBD = GetBarcodeConfigColor("New_Barcode_Color_BD");

            if (ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_TJShouldArriveConfirm") == "否" &&
                (stepType == StepType.Sampling || stepType == StepType.Confirm ||
                stepType == StepType.Send || stepType == StepType.Reach || stepType == StepType.SecondSend))
            {
                foreach (GridColumn colums in this.gvBarcode.Columns)
                {
                    colums.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                }
            }

            DataTable dtSex = CommonValue.GetSex();
            dtSex.Rows.Add("男", "男");
            dtSex.Rows.Add("女", "女");
            lueSex.DataSource = dtSex;

            // 启用标本打包功能
            if (ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_EablePackBarcode") == "是")
            {
                colbc_upid.Visible = true;
                colbc_upid.VisibleIndex = 2;
            }

            if (stepType == StepType.Confirm && Barcode_ShowComLineStatus)
            {
                gridColumn72.VisibleIndex = 2;

            }

            if (stepType == StepType.Select)
                gridColumn15.OptionsColumn.AllowEdit = false;

            if (ConfigHelper.GetSysConfigValueWithoutLogin("BarCode_SortYZID") == "是")
            {
                gridColumn1.SortOrder = DevExpress.Data.ColumnSortOrder.None;
                gridColumn70.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            }
            //开启配置显示病人临床路径
            if (ConfigHelper.GetSysConfigValueWithoutLogin("ShowIdentityName") == "否")
            {
                gridColumn52.Visible = false;
            }
            repositoryItemCheckEdit1.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(repositoryItemCheckEdit1_EditValueChanging);

            listDicSample = selectDict_Sample1.getDataSource();//赋值
        }

        /// <summary>
        /// 预置条码打印
        /// </summary>
        /// <param name="preBarcode"></param>
        /// <param name="print"></param>
        /// <returns></returns>
        public bool PrintPrePlaceBarcode(string preBarcode, IPrint print)
        {
            long testPrePlaceBarcode;
            bool isNum = Regex.IsMatch(preBarcode, @"^[0-9]*$");
            if (!long.TryParse(preBarcode, out testPrePlaceBarcode)
                || (preBarcode.ToString().Trim().Length != 10 && preBarcode.ToString().Trim().Length != 8)
                || !isNum)
            {
                MessageDialog.Show("输入的预置条码号不正确，请确保预置条码为10位或者8位数字");
                return false;
            }

            string barCode = preBarcode.ToString().Trim();
            EntitySampMain dr = CurrentSampMain;
            if (dr == null)
            {
                MessageDialog.Show("条码为空");
                return false;
            }
            List<EntityDicTestTube> tubeCache = CacheClient.GetCache<EntityDicTestTube>();
            
            //住院条码诊断为空时无法预制
            if (print is Inpatient
                && ConfigHelper.GetSysConfigValueWithoutLogin("InpatBarcode_PrintAndPrecut") == "是"
                && string.IsNullOrEmpty(dr.PidDiag))
            {
                MessageDialog.Show("诊断为空,无法预置!");
                return false;
            }
            long tubBarcode = Convert.ToInt64(barCode);

            //判断试管上的条码类型是否与检验条码的类型一致 
            if (!string.IsNullOrEmpty(dr.SampTubCode))
            {
                bool judge = true;

                if (judge)
                {
                    EntityDicTestTube tube = tubeCache.Find(w => w.TubCode == dr.SampTubCode);

                    if ((tube.TubBarcodeMin > 0 && tube.TubBarcodeMax > 0))
                    {
                        if (tubBarcode < tube.TubBarcodeMin || tubBarcode > tube.TubBarcodeMax)
                        {
                            MessageDialog.Show("试管类型不符，请重新扫描");
                            return false;
                        }
                    }
                }

            }
            if (dr == null)
                return false;
            string strStatus = dr.SampStatusId;
            if (strStatus == EnumBarcodeOperationCode.BarcodePrint.ToString()
                || strStatus == EnumBarcodeOperationCode.BarcodeGenerate.ToString()
                || strStatus == EnumBarcodeOperationCode.ResetPrePlaceBarcode.ToString()
                || strStatus == EnumBarcodeOperationCode.SampleReturn.ToString()
                )
            {

                if (!string.IsNullOrEmpty(dr.SampBarCode))
                {
                    //已经登记了预置条码
                    lis.client.control.MessageDialog.Show("该条记录已经扫描条码，要替换新的条码号请先重置该记录");
                    return false;
                }
                else
                {
                    if (!new ProxySampMain().Service.ExistSampMain(preBarcode.Trim()))
                    {
                        if (preBarCodeCheckCuvType)
                        {
                            string preBarCode = preBarcode.Trim();
                            string cuvType = dr.TubChargeCode;
                            string cuvName = dr.TubName;
                            if (preBarCode.Length >= 2 && cuvType.Length >= 2 && preBarCode.Substring(0, 2) != cuvType.Substring(0, 2))
                            {
                                if (MessageDialog.Show(string.Format("当前预置条码与[{1}]试管类型[{0}]不一致，是否继续", cuvType, cuvName),
                                                       MessageBoxButtons.YesNo) == DialogResult.No)
                                {
                                    return false;
                                }
                            }
                        }
                        int focusedRowHandle = MainGridView.FocusedRowHandle;
                        PrintPrePlaceBarcode(preBarcode.Trim());

                        //已打印的预置条码,在关联时不打印（[住院]打印条码预置条码允许为空）
                        if (ConfigHelper.GetSysConfigValueWithoutLogin("InPrintBarcodeBarcodeIsNull_ZY") == "是"
                            && strStatus == EnumBarcodeOperationCode.BarcodePrint.ToString())
                        {
                            return true;
                        }
                        else
                        {
                            if (ConfigHelper.GetSysConfigValueWithoutLogin("PreBarcode_NotAutoPrint") == "是")
                            {
                                //配置了预置条码滴管后不自动打条码 不打印 只更新打印状态
                                EntitySampMain samp = new EntitySampMain();
                                samp.SampBarCode = preBarcode.Trim();
                                samp.SampBarId = preBarcode.Trim();
                                List<EntitySampMain> list = new List<EntitySampMain>();
                                list.Add(samp);
                                string username = "";
                                string userid = "";
                                if (!CheckPower)
                                {
                                    if (print.SignInfo != null)
                                    {
                                        username = print.SignInfo.UserName;
                                        userid = print.SignInfo.LoginID;
                                    }
                                    else
                                    {
                                        username = "";
                                        userid = "";
                                    }
                                }
                                else
                                {
                                    username = UserInfo.userName;
                                    userid = UserInfo.loginID;
                                }
                                EntitySampOperation sampOp = new EntitySampOperation(userid, username);
                                sampOp.OperationStatus = "1";
                                sampOp.OperationStatusName = "打印条码";
                                sampOp.OperationPlace = LocalSetting.Current.Setting.Description;
                                sampOp.Remark = "IP：" + IPUtility.GetIP();
                                sampOp.OperationTime = IStep.GetServerTime();
                                bool result = new ProxySampMain().Service.UpdateSampMainStatus(sampOp, list);
                                if (result)
                                {
                                    BaseSampMain.SampStatusId = "1";
                                    BaseSampMain.SampPrintFlag = 1;
                                }
                                return true;
                            }
                            else
                            {
                                PrintBarcode(print);
                                return true;
                            }
                        }
                    }
                    else
                    {
                        lis.client.control.MessageDialog.Show("此试管已被登记!");
                        return false;
                    }

                }
            }
            else
            {
                lis.client.control.MessageDialog.Show("此试管已采集,不能再打印.");
                return false;
            }
        }
        private void repositoryItemCheckEdit1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (bsPatient.Current != null)
            {
                EntitySampMain sampMain = (EntitySampMain)bsPatient.Current;

                if (Enable_CombineUrgentFlag && (bool)e.NewValue && ListSampDetail != null && ListSampDetail.Count > 0)
                {
                    if (ListSampDetail.FindIndex(w => w.ComUrgentFlag == 1) < 0)
                    {
                        ShowAndClose("当前项目组合不允许加急！");
                        e.Cancel = true;
                        return;
                    }
                }
                //增加判断已打印条码不允许更新加急标志
                if (sampMain.SampPrintFlag == 1)
                {
                    ShowAndClose("当前条码已打印，无法修改！");
                    e.Cancel = true;
                    return;
                }
                bool result = proxy.Service.UpdateSampMainUrgentFlag((bool)e.NewValue, sampMain.SampBarId);

                if (result)
                    ShowAndClose("修改成功!");
                else
                {
                    ShowAndClose("修改失败!");
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="sql">where语句(不包含where关键字)</param>
        public void LoadData(EntitySampQC sql)
        {
            List<EntitySampMain> listSamp = proxy.Service.SampMainQuery(sql);

            if (stepType == StepType.Confirm || stepType == StepType.Reach || stepType == StepType.Sampling || stepType == StepType.SecondSend || stepType == StepType.Send)
            {
                listSamp = listSamp.OrderByDescending(o => o.SampLastactionDate).ToList();
            }
            else
            {
                string strSort = string.Empty;

                switch (selectType)
                {
                    case SelectType.Create:
                        listSamp = listSamp.OrderByDescending(o => o.SampDate).ToList();
                        break;
                    case SelectType.Sampling:
                        listSamp = listSamp.OrderByDescending(o => o.CollectionDate).ToList();
                        break;
                    case SelectType.Confirm:
                        listSamp = listSamp.OrderByDescending(o => o.SendDate).ToList();
                        break;
                    case SelectType.Send:
                        listSamp = listSamp.OrderByDescending(o => o.ReachFlag).ToList();
                        break;
                    case SelectType.Reach:
                        listSamp = listSamp.OrderByDescending(o => o.ReceiverFlag).ToList();
                        break;
                    case SelectType.SecondSend:
                        listSamp = listSamp.OrderByDescending(o => o.SecondSendDate).ToList();
                        break;
                    default:
                        listSamp = listSamp.OrderByDescending(o => o.ReceiverDate).ToList();
                        break;
                }
            }

            if(sql.PidSrcId == "109")
            {
                //体检按病人ID排序
                listSamp = listSamp.OrderBy(o => o.PidInNo).ToList();
            }


            if (listSamp != null && listSamp.Count > 0)
            {
                if (Selection != null)
                    Selection.ClearSelection();

                BindingPatientsSource(listSamp);

                SelectNotPrintForOutPatients();
            }//没有条码信息 需要清空上一个病人的条码信息
            else
            {
                BindingPatientsSource(listSamp);
            }
        }

        /// <summary>
        /// 打包条码加载数据
        /// </summary>
        public void LoadDataForPack(EntitySampQC sql)
        {
            List<EntitySampMain> listSampMain = proxy.Service.SampMainQuery(sql);

            if (listSampMain != null && listSampMain.Count > 0)
            {
                if (Selection != null)
                    Selection.ClearSelection();

                List<EntitySampMain> listSampSour = new List<EntitySampMain>();

                StringBuilder SB = new StringBuilder();
                string errstr = string.Empty;
                string warnstr = string.Empty;

                foreach (EntitySampMain row in listSampMain)
                {
                    string stepErrorMsg = string.Empty;
                    int setpErrorState = 0;
                    int tempRet;

                    //**************************************************************************
                    //增加了门诊采集确认判断标志
                    setpErrorState = QueryCollectState(row,
                                     Barcode_ZYShouldSamplingConfirm,
                                     Barcode_TJShouldSamplingConfirm,
                                     Barcode_MZShouldSamplingConfirm,
                                     ref stepErrorMsg);

                    //**************************************************************************
                    tempRet = QuerySendState(row,
                                     Barcode_ZYShouldCollectConfirm,
                                     Barcode_TJShouldCollectConfirm,
                                     ref stepErrorMsg);

                    setpErrorState = setpErrorState | tempRet;

                    tempRet = QueryReachState(row,
                                     Barcode_ZYShouldArriveConfirm,
                                     Barcode_TJShouldArriveConfirm,
                                     ref stepErrorMsg);

                    setpErrorState = setpErrorState | tempRet;

                    if (setpErrorState == 0)
                    {
                    }
                    else if ((setpErrorState & 2) == 2)
                    {
                        SB.Append(row.SampBarCode + ";");
                        errstr = stepErrorMsg;
                        continue;
                    }
                    else if ((setpErrorState & 1) == 1)
                    {
                        if (Step.Printer != null && Step.Printer.Name == new Manual().Name)
                        { }
                        //当系统配置里面，住院条码必须采集确认配置为：未执行时提示时，当某个条码没有采集直接签收时，弹出的提示框，默认光标停留在“否”那里
                        else
                        {
                            SB.Append(row.SampBarCode + ";");
                            warnstr = stepErrorMsg;
                        }
                    }

                    listSampSour.Add(row);
                }

                if (!string.IsNullOrEmpty(errstr))
                {
                    MessageDialog.Show(SB + "\r\n" + errstr + ",已自动过滤条码");
                    BindingPatientsSource(listSampSour);
                    return;
                }


                if (!string.IsNullOrEmpty(warnstr))
                {
                    if (MessageDialog.Show(SB + "\r\n" + warnstr + "\r\n是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        BindingPatientsSource(listSampSour);
                    }
                    else
                    {
                        BindingPatientsSource(listSampMain);
                    }
                    return;
                }

                BindingPatientsSource(listSampMain);
            }
        }

        public void BindingPatientsSource(List<EntitySampMain> listSampSour)
        {
            ListSampMain.Clear();
            ListSampMain = listSampSour;
            PatientBindingSource.DataSource = ListSampMain;
            gcBarcode.RefreshDataSource();
        }

        public void BindingSampMain(List<EntitySampMain> listSampSour, bool isOut = false)
        {
            //外院登记不需要清空
            if (!isOut)
            {
                ListSampMain.Clear();
            }
            ListSampMain = listSampSour;
            PatientBindingSource.DataSource = ListSampMain;
            gcBarcode.RefreshDataSource();
            if (!isOut)
            {         //清除勾选状态
                Selection.ClearSelection();
            }
            if (listSampSour.Count > 0)
            {
                gvBarcode.FocusedRowHandle = 0;
            }
        }

        public void SelectNotPrintForOutPatients()
        {
            //门诊条码的下载如果没有打印的默认打勾
            if (SelectWhenNotPrint)
                for (int i = 0; i < gvBarcode.RowCount; i++)
                {
                    EntitySampMain row = (EntitySampMain)gvBarcode.GetRow(i);

                    if (row.SampStatusId == "0" &&row.PidSrcId=="107")
                        Selection.SelectRow(i, true);
                }
        }

        public bool SelectRow(string barcode, bool focuedRow)
        {
            for (int i = 0; i < gvBarcode.RowCount; i++)
            {
                EntitySampMain row = gvBarcode.GetRow(i) as EntitySampMain;
                if (!Compare.IsNullOrDBNull(row.SampBarCode))
                {
                    if (row.SampBarCode.ToString() == barcode)
                    {
                        Selection.SelectRow(i, true);
                        if (focuedRow)
                            gvBarcode.FocusedRowHandle = i;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 添加条码
        /// </summary>
        /// <param name="barCode">条码号</param>
        /// <returns>是否成功</returns>
        public bool AddBarcode(string barCode)
        {
            return AddBarcode(barCode, null);
        }
        /// <summary>
        /// 如果是回退条码,则提示加急处理
        /// </summary>
        /// <param name="barCode"></param>
        public void ReturnBarCodeClew(string barCode)
        {
            try
            {
                EntitySampMain sampMain = proxy.Service.SampMainQueryByBarId(barCode);

                if (sampMain != null && !string.IsNullOrEmpty(sampMain.SampBarId))
                {
                    //回退条码提醒
                    if (sampMain.ListSampProcessDetail.FindIndex(i => i.ProcStatus == "9") >= 0)
                    {
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("该标本是回退标本，需加急处理");
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        internal void AutoRemoveRow()
        {
            try
            {
                if (!HasData() || gvBarcode.RowCount < 99)
                    return;
                for (int i = 79; i >= 0; i--)
                {
                    ListSampMain.RemoveAt(i);
                }

                PatientBindingSource.DataSource = ListSampMain;
                gcBarcode.RefreshDataSource();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
        }

        public void RemoveRow(string barcode)
        {

            for (int i = 0; i < gvBarcode.RowCount; i++)
            {
                EntitySampMain rowData = (EntitySampMain)this.gvBarcode.GetRow(i);
                if (!Compare.IsNullOrDBNull(rowData.SampBarCode))
                {
                    if (rowData.SampBarCode == barcode)
                    {
                        ListSampMain.Remove(rowData);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 条码查询时,连查条码号,查询后添加条码信息
        /// </summary>
        /// <param name="barCode">条码号</param>
        /// <returns></returns>
        public bool AddBarcode_Search(string barCode)
        {
            if (IncludeBarcode(barCode))  //表中已有条码号,提示并不增加
            {
                PointOut(barCode);

                lis.client.control.MessageDialog.Show(string.Format("该条码 {0} 已经在当前表中!", barCode), "条码重复");
                return false;
            }

            EntitySampMain sampMain = proxy.Service.SampMainQueryByBarId(barCode);

            if (sampMain != null && !string.IsNullOrEmpty(sampMain.SampBarId))
            {
                ListSampMain.Add(sampMain);
                PatientBindingSource.DataSource = ListSampMain;
                gcBarcode.RefreshDataSource();
                PointOut(barCode);//焦点定位到当前条码信息
                return true;
            }
            else
            {

                EntitySampProcessDetail processDetail = new ProxySampProcessDetail().Service.GetLastSampProcessDetail(barCode);

                if (!string.IsNullOrEmpty(processDetail.ProcStatus))
                {
                    MessageDialog.Show(string.Format("该条码已删除!\r\n原因：{0}\r\n操作者：{1}，时间：{2}，",
                                                      processDetail.ProcContent,
                                                      processDetail.ProcUsername,
                                                      processDetail.ProcDate.ToString()));
                    return false;
                }
                else
                {
                    MessageDialog.Show("该条码不存在!");
                    return false;
                }
            }

        }

        public bool NeedPrint = true;
        /// <summary>
        /// 添加条码
        /// </summary>
        /// <param name="barCode">条码号</param>
        /// <returns>是否成功</returns>
        public bool AddBarcode(string barCode, ArrayList cTypes)
        {

            if (IncludeBarcode(barCode))  //表中已有条码号,提示并不增加
            {
                PointOut(barCode);

                lis.client.control.MessageDialog.Show(string.Format("该条码 {0} 已经在当前表中!", barCode), "条码重复");
                return false;
            }


            EntitySampMain sampMain = proxy.Service.SampMainQueryByBarId(barCode);

            if (sampMain != null && !string.IsNullOrEmpty(sampMain.SampBarId))
            {
                if (cTypes != null)
                {
                    string ctype = sampMain.SampType;
                    bool isTrue = false;
                    for (int i = 0; i < cTypes.Count; i++)
                    {
                        if (cTypes[i].ToString().Trim() == ctype)
                            isTrue = true;
                    }

                    if (!isTrue)
                    {

                        if (BC_ForceSendDestFlag)
                        {
                            lis.client.control.MessageDialog.Show("该条码不在你所选物理组列表内,不能继续签入!");
                            return false;
                        }
                        else
                        {
                            if (lis.client.control.MessageDialog.Show("该条码不在你所选物理组列表内,是否继续签入？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.No)
                                return false;
                        }
                    }
                }

                if (UserInfo.GetSysConfigValue("Barcode_Sign_ShowExt") == "是")
                {
                    if (sampMain.ListSampDetail != null && sampMain.ListSampDetail.Count > 0)
                    {
                        StringBuilder sbRem = new StringBuilder();
                        foreach (EntitySampDetail drComRem in sampMain.ListSampDetail)
                        {
                            if (string.IsNullOrEmpty(drComRem.ComRemark))
                            {
                                sbRem.Append(string.Format("{0}:{1}\r\n", drComRem.ComName, drComRem.ComRemark));
                            }
                        }

                        if (sbRem.Length > 0)
                            lis.client.control.MessageDialog.ShowAutoCloseDialog(sbRem.ToString(), 2);
                    }
                }

                //判断条码批号是否与输入的批号一致
                if (Step.StepName == ConfigHelper.GetSysConfigValueWithoutLogin("barcode_confirmBatchNumber"))
                {

                    if (!string.IsNullOrEmpty(Step.Bcfrequency))
                    {
                        if (Step.Bcfrequency != sampMain.SampBarBatchNo.ToString())
                        {
                            lis.client.control.MessageDialog.Show("签收条码批号与输入条码批号不匹配!");
                            return false;
                        }

                    }
                    else
                    {
                        lis.client.control.MessageDialog.Show("请输入条码批号再进行签收确认!");

                        return false;
                    }
                }

                Step.BaseSampMain = sampMain;

                Step.StepController = GetStepController();

                if (Step.NeedCheckSentToMachine() && Step.HasAllSentToMachine()) //条码项目检查是否全部上机
                {
                    ShowMessage(Step.WarnForAllSentToMachine);
                    return false;
                }

                if (Step.NeedSigned() && !Step.HasSigned() && Convert.ToInt32(BaseSampMain.SampStatusId) < EnumBarcodeOperationCode.SampleReceive) //二次送检时需要签收
                {
                    ShowMessage(Step.WarnForNeedSign);
                    return false;
                }

                if (Step.ShouldDoAction && Step.HasDoneAction())
                {
                    ShowMessage(Step.WarnForHasDone);   //不返回，继续可以撤消          
                    return false;
                }

                //没有完成前一流程工作,提示但不添加到列表
                if (Step.MustFinishPreviousAction && Step.HasNotDoPreAction())
                {
                    ShowMessage(Step.WarnForNotPrepare);
                    return false;
                }

                if (Step is RenStep && Convert.ToInt32(Step.BaseSampMain.SampStatusId) >= 2)
                {
                    int processIndex = sampMain.ListSampProcessDetail.FindIndex(x => x.ProcStatus == Step.BaseSampMain.SampStatusId);

                    string msg;
                    if (processIndex >= 0)
                    {
                        EntitySampProcessDetail sampProcess = sampMain.ListSampProcessDetail[processIndex];

                        msg = string.Format(@"当前条码已执行：[{0}] 不能再执行：[{1}] 操作人:[{2}] 操作时间:[{3}]",
                            EnumBarcodeOperationCode.GetNameByCode(Step.BaseSampMain.SampStatusId), EnumBarcodeOperationCode.GetNameByCode(Step.StepCode),
                           sampProcess.ProcUsername, sampProcess.ProcDate.ToString());
                    }
                    else
                    {
                        msg = string.Format(@"当前条码已执行：[{0}] 不能再执行：[{1}]",
                           EnumBarcodeOperationCode.GetNameByCode(Step.BaseSampMain.SampStatusId), EnumBarcodeOperationCode.GetNameByCode(Step.StepCode));
                    }
                    ShowMessage(msg);
                    return false;
                }

                #region 核酸提取
                if (Step is HSTQStep || Step is HSDLKZStep || Step is HandOverStep)
                {
                    int processIndex = sampMain.ListSampProcessDetail.FindIndex(x => x.ProcStatus == Step.StepCode);//Step.BaseSampMain.SampStatusId);
                    string msg;
                    if (processIndex > 0)
                    {
                        EntitySampProcessDetail sampProcess = sampMain.ListSampProcessDetail[processIndex];
                        msg = string.Format(@"当前条码已执行：[{0}] 不能再执行：[{1}] 操作人:[{2}] 操作时间:[{3}]",
                            Step.StepName, Step.StepName,
                           sampProcess.ProcUsername, sampProcess.ProcDate.ToString());
                        ShowMessage(msg);
                        return false;
                    }
                }
                #endregion

                if (
                        (Convert.ToInt32(Step.BaseSampMain.SampStatusId) >= Convert.ToInt32(Step.StepCode)
                        && Step.BaseSampMain.SampStatusId != EnumBarcodeOperationCode.SampleReturn.ToString()
                        && Step.BaseSampMain.SampStatusId != EnumBarcodeOperationCode.SampleSecondSend.ToString()
                        && Step.StepCode != EnumBarcodeOperationCode.SampleSecondSend.ToString()
                        && (Step.StepCode != EnumBarcodeOperationCode.BarcodePrint.ToString() && Step.BaseSampMain.SampStatusId != EnumBarcodeOperationCode.BarcodePrint.ToString())
                        )
                    ||
                        (Step.BaseSampMain.SampStatusId == EnumBarcodeOperationCode.SampleSecondSend.ToString() && Step.StepCode == EnumBarcodeOperationCode.SampleCollect.ToString())
                    )
                {
                    int processIndex = sampMain.ListSampProcessDetail.FindIndex(x => x.ProcStatus == Step.BaseSampMain.SampStatusId);

                    string msg;
                    if (processIndex > 0)
                    {
                        EntitySampProcessDetail sampProcess = sampMain.ListSampProcessDetail[processIndex];
                        msg = string.Format(@"当前条码已执行：[{0}] 不能再执行：[{1}] 操作人:[{2}] 操作时间:[{3}]",
                            EnumBarcodeOperationCode.GetNameByCode(Step.BaseSampMain.SampStatusId), EnumBarcodeOperationCode.GetNameByCode(Step.StepCode),
                            sampProcess.ProcUsername, sampProcess.ProcDate.ToString());
                    }
                    else
                    {
                        msg = string.Format(@"当前条码已执行：[{0}] 不能再执行：[{1}]",
                           EnumBarcodeOperationCode.GetNameByCode(Step.BaseSampMain.SampStatusId), EnumBarcodeOperationCode.GetNameByCode(Step.StepCode));
                    }
                    ShowMessage(msg);
                    return false;
                }
                int timeout = 0;
                if (Convert.ToInt32(Step.StepCode) == EnumBarcodeOperationCode.SampleSecondSend)
                {
                    if (ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_SampleSecondSend") == "不可以" &&
                        sampMain.ListSampProcessDetail.FindIndex(x => x.ProcStatus == EnumBarcodeOperationCode.SampleSecondSend.ToString()) >= 0 &&
                        sampMain.ListSampProcessDetail.FindIndex(x => x.ProcStatus == EnumBarcodeOperationCode.Report.ToString()) >= 0)
                    {
                        ShowMessage("此报告已经二次送检！");
                        return false;
                    }
                }
                timeout = 0;
                if ((Convert.ToInt32(Step.StepCode) == EnumBarcodeOperationCode.SampleReceive ||
                     Convert.ToInt32(Step.StepCode) == EnumBarcodeOperationCode.SampleSecondSend)
                    && ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_blood_receive_timeout_warning") != null
                    && int.TryParse(ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_blood_receive_timeout_warning").ToString(), out timeout)
                    && timeout > 0)
                {
                    TimeSpan ts = DateTime.Now.Subtract(sampMain.CollectionDate.Value);
                    int day = ts.Hours;
                    if (day > timeout)
                    {
                        if (lis.client.control.MessageDialog.Show("此标本里采集已超过" + timeout.ToString() + "个小时，是否继续操作？！", "提示", MessageBoxButtons.YesNo) != DialogResult.Yes)
                            return false;
                    }
                }

                #region 流程判断

                string stepErrorMsg = string.Empty;
                int setpErrorState = 0;
                int tempRet;

                //**************************************************************************
                //增加了门诊采集确认判断标志
                setpErrorState = QueryCollectState(sampMain,
                                 Barcode_ZYShouldSamplingConfirm,
                                 Barcode_TJShouldSamplingConfirm,
                                 Barcode_MZShouldSamplingConfirm,
                                 ref stepErrorMsg);

                //**************************************************************************
                tempRet = QuerySendState(sampMain,
                                 Barcode_ZYShouldCollectConfirm,
                                 Barcode_TJShouldCollectConfirm,
                                 ref stepErrorMsg);

                setpErrorState = setpErrorState | tempRet;

                tempRet = QueryReachState(sampMain,
                                 Barcode_ZYShouldArriveConfirm,
                                 Barcode_TJShouldArriveConfirm,
                                 ref stepErrorMsg);

                setpErrorState = setpErrorState | tempRet;

                if (setpErrorState == 0)
                {
                }
                else if ((setpErrorState & 2) == 2)
                {
                    MessageDialog.Show(stepErrorMsg);
                    return false;
                }
                else if ((setpErrorState & 1) == 1)
                {
                    if (Step.Printer != null && Step.Printer.Name == new Manual().Name)
                    { }
                    //当系统配置里面，住院条码必须采集确认配置为：未执行时提示时，当某个条码没有采集直接签收时，弹出的提示框，默认光标停留在“否”那里
                    else if (sampMain.PidSrcId == "108" && Step.StepName == "签收" && lis.client.control.MessageDialog.Show(stepErrorMsg + "\r\n是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return false;
                    }
                    else if (MessageDialog.Show(stepErrorMsg + "\r\n是否继续？", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return false;
                    }
                }

                #endregion

                #region 签收时如果急查条码,则声音提示

                //系统配置：签收条码时急查有声音提示
                if (Step.StepName == "签收" && sampMain.SampUrgentFlag
                    && ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_Sign_JCPlaySound") == "是")
                {
                    string wavPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "WindowsNotify.wav";
                    if (File.Exists(wavPath))
                    {
                        PlaySound("WindowsNotify.wav", 0, SND_ASYNC | SND_FILENAME);//音响发声
                    }
                }

                #endregion
                sampMain.SampLastactionDate = ServerDateTime.GetServerDateTime(); //使签收的条码倒序显示
                ListSampMain.Add(sampMain);


                ListSampMain = ListSampMain.OrderByDescending(w => w.SampLastactionDate).ToList();
                PatientBindingSource.DataSource = ListSampMain;
                RefreshCurrentBarcodeInfo();
                gcBarcode.RefreshDataSource();
            }
            else
            {
                if (sampMain.ListSampProcessDetail.Count > 0)
                {
                    EntitySampProcessDetail sampProcess = sampMain.ListSampProcessDetail.OrderByDescending(f => Convert.ToInt32(f.ProcStatus)).ToList()[0];
                    MessageDialog.Show(string.Format("该条码已删除!\r\n原因：{0}\r\n操作者：{1}，时间：{2}，", sampProcess.ProcContent, sampProcess.ProcUsername, sampProcess.ProcDate));
                    return false;
                }
                else
                {
                    MessageDialog.Show("该条码不存在!");
                    return false;
                }

            }

            return true;
        }

        #region 条码状态查询
        /// <summary>
        /// 查询采集状态
        /// </summary>
        /// <param name="msg">返回错误信息</param>
        /// <returns>0=采集确认何忽略 1=未采集但需要提示是否继续 2=未采集并且不能继续下一步</returns>
        //*************************************************************************************
        //增加了门诊采集确认
        private int QueryCollectState(EntitySampMain barcodeInfo,
                                    string Barcode_ZYShouldSamplingConfirm,
                                    string Barcode_TJShouldSamplingConfirm,
                                    string Barcode_MZShouldSamplingConfirm,
                                    ref string msg)
        {
            string ori_id = barcodeInfo.PidSrcId;

            if (Step.StepCode == EnumBarcodeOperationCode.SampleCollect.ToString()
                || Step.StepCode == EnumBarcodeOperationCode.BarcodePrint.ToString()
                || IsUseBCManual)//如果为手工条码则不判断
            {
                return 0;
            }
            //修改判断方式，不在从sign里做判断。
            bool skip = barcodeInfo.CollectionFlag == 1 ||
                        barcodeInfo.SecondSendFlag == "1" ||
                        barcodeInfo.SampReturnFlag;

            if (skip)
            {
                return 0;
            }

            if (ori_id == "108")//住院
            {
                if (Barcode_ZYShouldSamplingConfirm == "是")
                {
                    msg += "\r\n当前[住院]条码未做[采集]确认";
                    return 2;
                }
                else if (Barcode_ZYShouldSamplingConfirm == "未执行时提示")
                {
                    msg += "\r\n当前[住院]条码未做[采集]确认";
                    return 1;
                }
            }
            else if (ori_id == "109")//体检
            {
                if (Barcode_TJShouldSamplingConfirm == "是")
                {
                    msg += "\r\n当前[体检]条码未做[采集]确认";
                    return 2;
                }
                else if (Barcode_TJShouldSamplingConfirm == "未执行时提示")
                {
                    msg += "\r\n当前[体检]条码未做[采集]确认";
                    return 1;
                }
            }
            //**********************************************************************************
            //增加门诊采集确认判断
            else if (ori_id == "107")//门诊
            {
                if (Barcode_MZShouldSamplingConfirm == "是")
                {
                    msg += "\r\n当前[门诊]条码未做[采集]确认";
                    return 2;
                }
                else if (Barcode_MZShouldSamplingConfirm == "未执行时提示")
                {
                    msg += "\r\n当前[门诊]条码未做[采集]确认";
                    return 1;
                }
            }

            //***********************************************************************************

            return 0;
        }

        /// <summary>
        /// 查询收取状态
        /// </summary>
        /// <param name="msg">返回错误信息</param>
        /// <returns>0=收取确认何忽略 1=未收取但需要提示是否继续 2=未收取并且不能继续下一步</returns>
        private int QuerySendState(EntitySampMain barcodeInfo,
                                    string Barcode_ZYShouldCollectConfirm,
                                    string Barcode_TJShouldCollectConfirm,
                                    ref string msg)
        {
            string ori_id = barcodeInfo.PidSrcId;
            if (Step.StepCode == EnumBarcodeOperationCode.SampleCollect.ToString()
                || Step.StepCode == EnumBarcodeOperationCode.SampleSend.ToString()
                || IsUseBCManual)//如果为手工条码则不判断)
            {
                return 0;
            }
            //修改判断方式，不在从sign里做判断。
            bool skip = barcodeInfo.SendFlag == 1 ||
                        barcodeInfo.SecondSendFlag == "1" ||
                        barcodeInfo.SampReturnFlag;

            if (skip)
            {
                return 0;
            }

            if (ori_id == "108")//住院
            {
                if (Barcode_ZYShouldCollectConfirm == "是")
                {
                    msg += "\r\n当前[住院]条码未做[收取]确认";
                    return 2;
                }
                else if (Barcode_ZYShouldCollectConfirm == "未执行时提示")
                {
                    msg += "\r\n当前[住院]条码未做[收取]确认";
                    return 1;
                }
            }
            else if (ori_id == "109")//体检
            {
                if (Barcode_TJShouldCollectConfirm == "是")
                {
                    msg += "\r\n当前[体检]条码未做[收取]确认";
                    return 2;
                }
                else if (Barcode_TJShouldCollectConfirm == "未执行时提示")
                {
                    msg += "\r\n当前[体检]条码未做[收取]确认";
                    return 1;
                }
            }
            return 0;
        }

        /// <summary>
        /// 查询送达状态
        /// </summary>
        /// <param name="msg">返回错误信息</param>
        /// <returns>0=送达确认可忽略 1=未送达但需要提示是否继续 2=未送达并且不能继续下一步</returns>
        private int QueryReachState(EntitySampMain barcodeInfo,
                                    string Barcode_ZYShouldArriveConfirm,
                                    string Barcode_TJShouldArriveConfirm,
                                    ref string msg)
        {
            string ori_id = barcodeInfo.PidSrcId;

            if (Step.StepCode == EnumBarcodeOperationCode.SampleCollect.ToString()
                || Step.StepCode == EnumBarcodeOperationCode.SampleSend.ToString()
                || Step.StepCode == EnumBarcodeOperationCode.SampleReach.ToString()
                || IsUseBCManual//如果为手工条码则不判断
                )
            {
                return 0;
            }

            //修改判断方式，不在从sign里做判断。
            bool skip = barcodeInfo.ReachFlag == 1 ||//已做送检确认
                        barcodeInfo.SecondSendFlag == "1" ||//已做二次送检
                        barcodeInfo.SampReturnFlag;//已做回退

            if (skip)
            {
                return 0;
            }

            if (ori_id == "108")//住院
            {
                if (Barcode_ZYShouldArriveConfirm == "是")
                {
                    msg += "\r\n当前[住院]条码未做[送达]确认";
                    return 2;
                }
                else if (Barcode_ZYShouldArriveConfirm == "未执行时提示")
                {
                    msg += "\r\n当前[住院]条码未做[送达]确认";
                    return 1;
                }
            }
            else if (ori_id == "109")//体检
            {
                if (Barcode_TJShouldArriveConfirm == "是")
                {
                    msg += "\r\n当前[体检]条码未做[送达]确认";
                    return 2;
                }
                else if (Barcode_TJShouldArriveConfirm == "未执行时提示")
                {
                    msg += "\r\n当前[体检]条码未做[送达]确认";
                    return 1;
                }
            }
            return 0;
        }
        #endregion

        private static IStepController GetStepController()
        {
            IStepController stepController = new BaseStepController();
            string checkType = ConfigHelper.GetSysConfigValueWithoutLogin("BarcodeCheck");
            if (checkType == "严格控制")
                stepController = new CoolStepController();

            return stepController;
        }

        /// <summary>
        /// 在列表中指出条码号
        /// </summary>
        /// <param name="barCode">条码号</param>
        private void PointOut(string barCode)
        {
            int index = ListSampDetail.FindIndex(w => w.SampBarId == barCode);
            if (index >= 0)
                gvBarcode.FocusedRowHandle = index;
        }

        /// <summary>
        /// 列表是否包含条码号
        /// </summary>
        private bool IncludeBarcode(string barCode)
        {
            return ListSampMain.FindIndex(w => w.SampBarCode == barCode) > -1;
        }

        /// <summary>
        /// 当前选中条码改变
        /// </summary>
        private void gvBarcode_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (DesignMode)
                return;

            if (!HasData())
            {
                ClearAll();
                return;
            }

            RefreshCurrentBarcodeInfo();

            //标本过滤事件
            selectDict_Sample1_onBeforeFilter();
        }

        bool IsNotEmpty(string source)
        {
            return Extensions.IsNotEmpty(source);
        }

        string DateMins(string dtstr1, string dtstr2)
        {
            DateTime dt1 = Convert.ToDateTime(dtstr1);
            DateTime dt2 = Convert.ToDateTime(dtstr2);

            var ts = dt1 - dt2;

            if (ts.Hours == 0 && ts.Days == 0)
            {
                return Math.Round(ts.TotalMinutes, 1).ToString() + "分";
            }
            else
            {
                if (ts.TotalDays > 2)
                {
                    return Math.Round(ts.TotalDays, 1).ToString() + "天";
                }
                if (ts.TotalHours > 24)
                {
                    return Math.Round(ts.TotalHours, 1).ToString() + "时";
                }

                return ts.Hours + "时 " + ts.Minutes + "分";

            }
        }

        private void FillTimeLine(EntitySampMain sampMain)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            string LASTDATE = "";

            if (stepType == StepType.Sampling && sampMain.CollectionDate == null)
            {
                sampMain.CollectionDate = ServerDateTime.GetServerDateTime();
            }

            if (stepType == StepType.Reach && sampMain.ReachDate == null)
            {
                sampMain.ReachDate = ServerDateTime.GetServerDateTime();
            }

            if (stepType == StepType.Send && sampMain.SendDate == null)
            {
                sampMain.SendDate = ServerDateTime.GetServerDateTime();
            }

            if (stepType == StepType.Confirm && sampMain.ReceiverDate == null)
            {
                sampMain.ReceiverDate = ServerDateTime.GetServerDateTime();
            }

            if (!dic.ContainsKey(sampMain.SampDate.ToString()))
            {
                LASTDATE = sampMain.SampDate.ToString();
                dic.Add(sampMain.SampDate.ToString(), "生成时间");
            }

            if (sampMain.CollectionDate != null
                 && !dic.ContainsKey(sampMain.CollectionDate.ToString()))
            {
                if (!string.IsNullOrEmpty(LASTDATE))
                {
                    string mins = DateMins(sampMain.CollectionDate.ToString(), LASTDATE);

                    dic.Add(sampMain.CollectionDate.ToString(), "采集时间" + "|" + mins);
                }
                else
                {
                    dic.Add(sampMain.CollectionDate.ToString(), "采集时间");
                }
                LASTDATE = sampMain.CollectionDate.ToString();
            }

            if (sampMain.SendDate != null
                 && !dic.ContainsKey(sampMain.SendDate.ToString()))
            {
                if (!string.IsNullOrEmpty(LASTDATE))
                {
                    string mins = DateMins(sampMain.SendDate.ToString(),
                        LASTDATE);

                    dic.Add(sampMain.SendDate.ToString(), "收取时间" + "|" + mins);
                }
                else
                {
                    dic.Add(sampMain.SendDate.ToString(), "收取时间");
                }
                LASTDATE = sampMain.SendDate.ToString();
            }

            if (sampMain.ReachDate != null
               && !dic.ContainsKey(sampMain.ReachDate.ToString()))
            {
                if (!string.IsNullOrEmpty(LASTDATE))
                {
                    string mins = DateMins(sampMain.ReachDate.ToString(), LASTDATE);

                    dic.Add(sampMain.ReachDate.ToString(), "送达时间" + "|" + mins);
                }
                else
                {
                    dic.Add(sampMain.ReachDate.ToString(), "送达时间");
                }
                LASTDATE = sampMain.ReachDate.ToString();
            }


            if (sampMain.ReceiverDate != null
               && !dic.ContainsKey(sampMain.ReceiverDate.ToString()))
            {
                if (!string.IsNullOrEmpty(LASTDATE))
                {
                    string mins = DateMins(sampMain.ReceiverDate.ToString(),
                        LASTDATE);

                    dic.Add(sampMain.ReceiverDate.ToString(), "签收时间" + "|" + mins);
                }
                else
                {
                    dic.Add(sampMain.ReceiverDate.ToString(), "签收时间");
                }
                LASTDATE = sampMain.ReceiverDate.ToString();
            }

            if (sampMain.SecondSendDate != null
                && !dic.ContainsKey(sampMain.SecondSendDate.ToString()))
            {
                if (!string.IsNullOrEmpty(LASTDATE))
                {
                    string mins = DateMins(sampMain.SecondSendDate.ToString(),
                        LASTDATE);

                    dic.Add(sampMain.SecondSendDate.ToString(), "二次送检" + "|" + mins);
                }
                else
                {
                    dic.Add(sampMain.SecondSendDate.ToString(), "二次送检");
                }
                LASTDATE = sampMain.SecondSendDate.ToString();
            }
        }


        /// <summary>
        /// 刷新当前条码的详细信息,包括项目信息
        /// </summary>
        public void RefreshCurrentBarcodeInfo()
        {
            if (bsPatient.Current == null)
                return;

            try
            {

                if (pnlTimeLine.Visible)
                {
                    try
                    {
                        FillTimeLine(CurrentSampMain);
                    }
                    catch
                    { }
                }
            }
            catch
            { }

            string barcodeNumber = CurrentSampMain.SampBarId;

            ShowBaseInfo();
            BindCName(barcodeNumber);
            ShowSignInfo(barcodeNumber);

            //已做采集确认以后步骤的条码不允许在条码打印界面更改病人资料
            SetControlEditStatus(barcodeNumber);
        }

        private void SetControlEditStatus(string barcodeNumber)
        {
            List<string> listBarCode = new List<string>();

            listBarCode.Add(barcodeNumber);

            if (BaseSampMain.SampStatusId == "0" || BaseSampMain.SampStatusId == "1" || BaseSampMain.SampStatusId == "2" || BaseSampMain.SampStatusId == "9")
            {
                cbMan.Properties.ReadOnly = false;
                selectDict_Sample1.Readonly = false;
                //系统配置：[条码打印]没有标本类型的才允许修改
                if (UserInfo.GetSysConfigValue("Barcode_AllowEditNullSamId") == "是"
                    && !string.IsNullOrEmpty(selectDict_Sample1.valueMember))
                {
                    selectDict_Sample1.Readonly = true;
                }

                selectDict_Sample_Remarks1.Readonly = false;

                if (UserInfo.GetSysConfigValue("Barcode_AllowEditAge") == "是" &&
                    (StepType == StepType.Print || StepType == StepType.Sampling))
                {
                    txtAge.Properties.ReadOnly = false;
                }
                else
                {
                    if (string.IsNullOrEmpty(txtAge.Text.Trim()))
                    {
                        txtAge.Properties.ReadOnly = false;
                    }
                    else
                    {
                        txtAge.Properties.ReadOnly = true;
                    }
                }
                //如果没有性别传过来，则开放修改
                if (string.IsNullOrEmpty(txtSex.Text.Trim()) || BC_CanEditSex)
                {
                    txtSex.Properties.ReadOnly = false;
                }
                else
                {
                    txtSex.Properties.ReadOnly = true;
                }

            }
            else
            {
                cbMan.Properties.ReadOnly = true;
                selectDict_Sample1.Readonly = true;
                selectDict_Sample_Remarks1.Readonly = true;
                txtAge.Properties.ReadOnly = true;
                txtSex.Properties.ReadOnly = true;

            }

            if(BaseSampMain.SampStatusId == "60")
            {
                txtCurrentAddress.ReadOnly = true;
                txtIdentifierID.ReadOnly = true;
                selectPatType1.Readonly = true;
                selectIdentifierType1.Readonly = true;
            }
            else
            {
                txtCurrentAddress.ReadOnly = false;
                txtIdentifierID.ReadOnly = false;
                selectPatType1.Readonly = false;
                selectIdentifierType1.Readonly = false;
            }

            if (Barcode_SignEnableBitchModifySamp && stepType == StepType.Confirm)
            {
                selectDict_Sample1.Readonly = false;
            }
        }

        private void ShowSignInfo(string barcode)
        {
            ListSampProcess = new ProxySampProcessDetail().Service.GetSampProcessDetail(barcode);
            gcAction.DataSource = ListSampProcess;
            this.gvAction.MoveLastVisible();
        }

        private void ShowBaseInfo()
        {
            Step.BaseSampMain = CurrentSampMain;
            selectDict_Sample1.SelectByID(Step.BaseSampMain.SampSamId);
            selectDict_Sample_Remarks1.valueMember = Step.BaseSampMain.RemId; //TO-DO:需要改成SelectByName
            selectDict_Sample_Remarks1.displayMember = Step.BaseSampMain.SampRemark;
            txtShowName.Text = Step.BaseSampMain.PidName;
            txtBarcodeNumber.Text = Step.BaseSampMain.SampBarCode;
            txtAge.Text = Step.BaseSampMain.SampAge;
            txtDepartment.Text = Step.BaseSampMain.PidDeptName;//科室号
            txtSex.Text = Step.BaseSampMain.PidSex;
            txtDiag.Text = Step.BaseSampMain.PidDiag;
            txtTimes.Text = Step.BaseSampMain.PidAdmissTimes.ToString();
            txtAplyTime.Text = Step.BaseSampMain.SampOccDate.ToString("yyyy-MM-dd HH:mm:ss"); //新增 执行日期

            txtCurrentAddress.Text = Step.BaseSampMain.PidAddress; //现住址
            txtIdentifierID.Text = Step.BaseSampMain.PidIdentityCard; //证件号
            selectIdentifierType1.SelectByDispaly(Step.BaseSampMain.PidIdentityName); //证件类型
            selectPatType1.SelectByDispaly(Step.BaseSampMain.SampPatType); //人员身份

            //条码查询开单医生显示名称与工号
            if (ConfigHelper.GetSysConfigValueWithoutLogin("BarcodeSearch_showDoctorIDName") == "是"
                && stepType == StepType.Select
                && !string.IsNullOrEmpty(Step.BaseSampMain.PidDoctorName))
            {
                txtDoctor.Text = Step.BaseSampMain.PidDoctorName + "|" + Step.BaseSampMain.PidDoctorCode;
            }
            else
            {
                txtDoctor.Text = Step.BaseSampMain.PidDoctorName;
            }
            this.txtBedNumber.Text = Step.BaseSampMain.PidBedNo;//床号
            txtPatNoID.Text = Step.BaseSampMain.PidInNo; //住院号 病人ID

            //如果没有年龄传过来，则开放修改
            if (string.IsNullOrEmpty(txtAge.Text.Trim()))
            {
                txtAge.Properties.ReadOnly = false;
            }

            //如果没有性别传过来，则开放修改
            if (string.IsNullOrEmpty(txtSex.Text.Trim()))
            {
                txtSex.Properties.ReadOnly = false;
            }


            checkMan.Checked = (Step.BaseSampMain.PidName.IndexOf("之夫") >= 0 || Step.BaseSampMain.PidName.IndexOf("之妻") >= 0 ||
                                Step.BaseSampMain.PidName.IndexOf("之女") >= 0 || Step.BaseSampMain.PidName.IndexOf("之子") >= 0);


            if (Step == null || Step.BaseSampMain == null || string.IsNullOrEmpty(Step.BaseSampMain.SampBarId))
                return;
            OnShowBaseInfo(Step.BaseSampMain, EventArgs.Empty);
        }

        public void ClearBaseInfo()
        {
            foreach (Control control in gcBaseInfo.Controls)
            {
                if (control is TextEdit || control is MemoEdit)
                    control.Text = "";
            }
            selectDict_Sample1.ClearSelect();
            selectDict_Sample1.displayMember = "";
            selectDict_Sample_Remarks1.ClearSelect();
            selectDict_Sample_Remarks1.displayMember = "";

            selectIdentifierType1.ClearSelect();
            selectIdentifierType1.displayMember = "";
            selectPatType1.ClearSelect();
            selectPatType1.displayMember = "";
            txtCurrentAddress.Text = "";
            txtIdentifierID.Text = "";
        }

        /// <summary>
        /// 获取项目明细表
        /// </summary>
        /// <param name="barcodeNumber">条码号</param>
        private void BindCName(string barcodeNumber)
        {
            ListSampDetail = new ProxySampDetail().Service.GetSampDetail(barcodeNumber);
            bsCName.DataSource = ListSampDetail;
            gvCname.BestFitColumns();
        }

        /// <summary>
        /// 获取当前条码信息
        /// </summary>
        /// <returns></returns>
        private EntitySampMain GetBaseInfo()
        {
            if (bsPatient.Current != null)
            {
                return (EntitySampMain)bsPatient.Current;
            }
            else
                return new EntitySampMain();
        }

        /// <summary>
        /// 更新打印状态
        /// </summary>
        public void UpdatePrintFlag()
        {
            List<EntitySampMain> rows = Selection.GetAllSelectSamp();
            if (rows != null && rows.Count > 0)
            {
                foreach (EntitySampMain row in rows)
                {
                    for (int i = 0; i < gvBarcode.RowCount; i++)
                    {
                        EntitySampMain samp = (EntitySampMain)gvBarcode.GetRow(i);
                        if (samp.SampBarId == row.SampBarId)
                        {
                            samp.SampPrintFlag = 1;
                            samp.SampStatusId = "1";
                        }
                    }
                
                }
            }
            else
            {
                EntitySampMain row = (EntitySampMain)MainGridView.GetFocusedRow();
                row.SampPrintFlag = 1;
                row.SampStatusId = "1";
            }
        }

        private void ShowMessage(string word)
        {
            lis.client.control.MessageDialog.Show(word, "提示");
        }

        public bool alone = false;

        internal void PrintBarcode(IPrint iPrint, bool alone)
        {
            this.alone = alone;
            PrintBarcode(iPrint);
        }

        /// <summary>
        /// 预置条码打印
        /// </summary>
        /// <param name="strPrePlaceBarcode"></param>
        public bool PrintPrePlaceBarcode(string strPrePlaceBarcode)
        {
            bool result = false;
            if (gvBarcode.RowCount > 0)
            {
                Selection.ClearSelection();
                Selection.SelectRow(gvBarcode.FocusedRowHandle, true);
                EntitySampMain dr = CurrentSampMain; 
                if (!string.IsNullOrEmpty(dr.SampBarId))
                {
                    result = new ProxySampMain().Service.UpdateSampMainBarCode(dr.SampBarId, strPrePlaceBarcode);
                    if (result)
                    {
                        dr.SampBarCode = strPrePlaceBarcode;
                        dr.SampBarId = strPrePlaceBarcode;
                    }
                }

            }

            return result;
        }
        //打印时对未设置对应HIS代码的组合的条码进行提示
        //
        bool CheckBarHisCode()
        {
            return true;
        }

        /// <summary>
        /// 选中所有未打印的条码
        /// </summary>
        /// <returns></returns>
        public bool SelectAllNoPrintRow()
        {
            for (int i = 0; i < gvBarcode.RowCount; i++)
            {
                EntitySampMain row = (EntitySampMain)gvBarcode.GetRow(i);
                //非预置条码勾上
                if (row.SampBarType == 0 && row.SampPrintFlag == 0)
                {
                    row.CheckMarkSelection = true;
                    Selection.SelectRow(i, true);
                }
            }
            return true;
        }
        bool AutoPrintReturn = false;

        /// <summary>
        /// 打印条码
        /// </summary>
        public void PrintBarcode(IPrint iPrint)
        {
            if (HasNotChoose())
            {
                ShowAndClose("请选择一条记录!");
                return;
            }

            string sampleMessage = string.Empty;
            if (SelectBarcodeNotSampleID(iPrint, ref sampleMessage))//没有标本的条码提示
            {
                ShowMessage("此条码未选择标本类型,不能打印,请在右边资料修改标本!" + "\r\n" + sampleMessage);
                return;
            }


            if (SelectBarcodeNotAge(iPrint, ref sampleMessage))//没有年龄的条码提示
            {
                ShowMessage("以下条码没有年龄,不能打印!" + "\r\n" + sampleMessage);
                return;
            }


            if (SelectBarcodeNotSex(iPrint, ref sampleMessage))//没有性别的条码提示
            {
                ShowMessage("以下条码没有性别,不能打印!" + "\r\n" + sampleMessage);
                return;
            }

            if (SelectBarcodeNotTreatReturmMsg(iPrint, ref sampleMessage))
            {
                ShowMessage("以下回退条码未处理,不能打印!" + "\r\n" + sampleMessage);
                return;
            }

            string barcodeMessage = string.Empty;
            if (SelectBarcodeNotBarCode(iPrint, ref barcodeMessage))//没有标本的条码提示
            {
                ShowMessage("以下条码没有填入条码,不能打印,请在上方扫入条码!" + "\r\n" + barcodeMessage);
                return;
            }

            //打印时对未设置对应HIS代码的组合的条码进行提示
            if (Bar_NoHisCodeNotPrint
                && !CheckBarHisCode())
            {
                return;
            }

            List<EntitySampMain> barcodesNotPrinted;//未打印条码
            List<EntitySampMain> barcodesPrinted;//已打印(未采集)/已回退条码
            List<EntitySampMain> barcodesCanNotPrint;//不能再打印(已采集未回退)的条码
            List<EntitySampMain> barcodesNotPower;//没有权限打印的条码

            //******第二次打印,验收时的操作者信息记录
            bool IsSecondPritBarCode = false;//是否第二次打印了条码
            string SecondPritOperatorName = "-1";//记录二次打印操作者名称
            string SecondPritOperatorID = "-1";//记录二次打印操作者ID
            //******************************************

            SelectBarcodeRePrint(iPrint, out barcodesNotPrinted, out barcodesPrinted, out barcodesCanNotPrint, out barcodesNotPower);

            string message = string.Empty;

            string username = "";
            string userid = "";
            if (IsSecondPritBarCode)
            {
                username = SecondPritOperatorName;
                userid = SecondPritOperatorID;
            }
            else if (alone)
            {
                username = iPrint.SignInfo.UserName;
                userid = iPrint.SignInfo.LoginID;
            }
            else
            {
                username = UserInfo.userName;
                userid = UserInfo.loginID;
            }

            //操作前确认费用，确认失败去不允许继续操作。
            message += SampMainCheck(ref barcodesNotPrinted, userid, username);
            message += SampMainCheck(ref barcodesPrinted, userid, username);
          
            string msgPrinted = string.Empty;
            string msgCantPrint = string.Empty;
            string msgNotPower = string.Empty;
            string msgNotPrint = string.Empty;
            foreach (EntitySampMain drv in barcodesPrinted)
            {
                msgPrinted += string.Format("条码号 [{0}]  姓名 [{1}]\r\n", drv.SampBarCode, drv.PidName);
            }

            foreach (EntitySampMain drv in barcodesCanNotPrint)
            {
                msgCantPrint += string.Format("条码号 [{0}]  姓名 [{1}]\r\n", drv.SampBarCode, drv.PidName);
            }

            foreach (EntitySampMain drv in barcodesNotPower)
            {
                msgNotPower += string.Format("条码号 [{0}]  姓名 [{1}]\r\n", drv.SampBarCode, drv.PidName);
            }

            if (barcodesCanNotPrint.Count > 0)
            {
                message += "以下条码已采集，不能再打印\r\n" + msgCantPrint + "\r\n";
            }

            if (barcodesPrinted.Count > 0)
            {
                message += "以下条码已打印，是否再次打印？\r\n" + msgPrinted + "\r\n";
            }

            if (barcodesNotPower.Count > 0)
            {
                message += "以下条码无重打的权限:\r\n" + msgNotPower + "\r\n";
            }


            //系统配置：住院条码诊断为空时不能打印和预制
            bool PrintAndPrecut = ConfigHelper.GetSysConfigValueWithoutLogin("InpatBarcode_PrintAndPrecut") == "是";
            //住院条码诊断为空时不能打印（三院）
            if (PrintAndPrecut && iPrint is Inpatient)
            {
                List<EntitySampMain> listNotPrint = barcodesNotPrinted.FindAll(w => string.IsNullOrEmpty(w.PidDiag));
                barcodesNotPrinted = barcodesNotPrinted.FindAll(w => !string.IsNullOrEmpty(w.PidDiag));
                if (listNotPrint.Count > 0)
                {
                    foreach (EntitySampMain drv in listNotPrint)
                    {
                        msgNotPrint += string.Format("条码号 [{0}]  姓名 [{1}]\r\n"
                                                        , drv.SampBarCode
                                                        , drv.PidName
                          );
                    }
                    message += "以下条码无诊断无法打印:\r\n" + msgNotPrint + "\r\n";
                }
            }

            //系统配置：条码打印是否提示采样注意事项
            string ShowExp = ConfigHelper.GetSysConfigValueWithoutLogin("BarcodePrint_ShowExp");
            //条码采样注意事项不为空时提示
            if ((ShowExp.Contains("门诊") && iPrint is OutPaitent) 
                || (ShowExp.Contains("住院") && iPrint is Inpatient) 
                || (ShowExp.Contains("体检") && iPrint is TJPaitent))
            {
                string messageExp = string.Empty;
                ProxySampDetail proxyDetail = new ProxySampDetail();
                List<string> listBarId = new List<string>();
                foreach (EntitySampMain sampMain in barcodesNotPrinted)
                {
                    listBarId.Add(sampMain.SampBarId);
                }
                foreach (EntitySampMain sampMain in barcodesPrinted)
                {
                    listBarId.Add(sampMain.SampBarId);
                }
                if (listBarId.Count > 0)
                {
                    List<EntitySampDetail> listSampDetail = proxyDetail.Service.GetSampDetailByListBarId(listBarId);
                    if (listSampDetail.Count > 0)
                    {
                        foreach (EntitySampDetail detail in listSampDetail)
                        {
                            if (!string.IsNullOrEmpty(detail.BloodNotice))
                            {
                                messageExp += string.Format("条码号 [{0}]  注意事项[{1}]\r\n"
                                      , detail.SampBarCode,
                                       detail.BloodNotice);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(messageExp))
                    {
                        message += "以下条码有采样注意事项:\r\n" + messageExp + "\r\n";
                    }
                }
            }


            if (message != string.Empty)
            {
                if (barcodesPrinted.Count > 0)
                {
                    message += "【是】：打印【未打印】和【已打印】的条码\r\n【否】：只打印【未打印】的条码\r\n【取消】：取消本次打印";

                    DialogResult diaResult = new DialogResult();
                    if (isResetBarcode)  //重置条码不弹窗，直接打印
                    {
                        diaResult = DialogResult.Yes;
                        isResetBarcode = false;
                    }
                    else
                        diaResult = MessageDialog.Show(message, "提示", MessageBoxButtons.YesNoCancel);

                    if (diaResult == DialogResult.Cancel)
                    {
                        return;
                    }
                    else if (diaResult == DialogResult.Yes)
                    {
                        //打印已打印条码时要权限控制[门诊][体检]
                        if (ConfigHelper.GetSysConfigValueWithoutLogin("InPrintBarcode_Confirm") == "是")
                        {
                            if (iPrint is TJPaitent || iPrint is OutPaitent)
                            {
                                //对已打印条码进行权限验证
                                //目前只针对“门诊”和“体检”
                                bool ConfirmOK = powerConfirm(out SecondPritOperatorName, out SecondPritOperatorID);
                                if (ConfirmOK == false) return;

                                IsSecondPritBarCode = true;//通过权限--二次打印
                            }
                        }
                        //打印已打印条码时要权限控制[住院]
                        if (ConfigHelper.GetSysConfigValueWithoutLogin("InPrintBarcode_Confirm_ZY") == "是")
                        {
                            if (iPrint is Inpatient)
                            {
                                //对已打印条码进行权限验证
                                bool ConfirmOK = powerConfirm(out SecondPritOperatorName, out SecondPritOperatorID);
                                if (ConfirmOK == false) return;

                                IsSecondPritBarCode = true;//通过权限--二次打印
                            }
                        }

                        barcodesNotPrinted.AddRange(barcodesPrinted);
                    }
                }
                else
                {
                    message += "是否继续？\r\n【是】：继续打印未打印的条码\r\n【否】：取消本次打印";
                    if (lis.client.control.MessageDialog.Show(message, "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                }
            }

            if (barcodesNotPrinted.Count == 0)
            {
                return;
            }

            Step.BaseSampMain = BaseSampMain;
            Step.Printer = iPrint;

            EntitySampOperation sampOp = new EntitySampOperation(userid, username);
            sampOp.OperationStatus = "1";
            sampOp.OperationStatusName = "打印条码";

            if (Barcode_RecordSignPlace)
            {
                sampOp.OperationPlace = LocalSetting.Current.Setting.Description;
            }
            else
            {
                sampOp.OperationPlace = LocalSetting.Current.Setting.DeptName;
            }

            sampOp.Remark = "IP：" + IPUtility.GetIP();
            iPrint.SignInfo = new SignInfo(userid, username);
            //门诊 体检 打印列表最后一个条码 判断是否勾上自动打印回执  勾上则打印回执
            if (iPrint is OutPaitent || iPrint is TJPaitent)
            {
                if (iPrint.AutoPrintReturnBarcode) 
                {
                    this.AutoPrintReturn = true;
                }
                else
                {
                    this.AutoPrintReturn = false;
                }
            }
            if (iPrint.ShowSpecialComfirmWhenPrint())
            {
                bool printBarcodeSuccess = false;
                //打印机打印
                try
                {
                    if (iPrint is TJPaitent)
                    {
                        //系统配置：[体检]按病人ID与标本排序打印条码
                        if (ConfigHelper.GetSysConfigValueWithoutLogin("TJBarcode_PrintByPNoAndSam") != "否")
                        {
                            barcodesNotPrinted = barcodesNotPrinted.OrderBy(o => o.PidInNo).ToList();
                        }
                    }


                    if (HQ.BPPrint.PrinterController.UseCustomPrintMode ||
                        HQ.BPPrint.BPPrintSetting.Current.EnableBPPrint)
                    {
                        EntityDCLPrintParameter paramter = new EntityDCLPrintParameter();
                        paramter.ReportCode = GetReportTemplate(iPrint);

                        foreach (EntitySampMain sm in barcodesNotPrinted)
                        {
                            paramter.listSampSn.Add(sm.SampSn.ToString());
                        }

                        ProxyReportPrint proxyPrint = new ProxyReportPrint();
                        EntityDCLPrintData printData = proxyPrint.Service.GetReportSource(paramter);

                        if (printData.ReportData == null || printData.ReportData.Tables == null || printData.ReportData.Tables.Count == 0)
                        {
                            return;
                        }

                        DataTable dtReportData = printData.ReportData.Tables[0];

                        List<DataRow> listReportData = new List<DataRow>();
                        foreach (DataRow item in dtReportData.Rows)
                        {
                            listReportData.Add(item);
                        }

                        if (iPrint is Manual)
                        {
                            printBarcodeSuccess = new HQ.BPPrint.OtherBPPrinter().Print(listReportData.ToArray()) > 0;
                        }
                        else
                        {
                            printBarcodeSuccess = HQ.BPPrint.LisBarcodePrinter.Print(listReportData.ToArray());

                            if (this.AutoPrintReturn &&
                                (iPrint is OutPaitent || iPrint is TJPaitent))
                            {
                                string reutrnTemplate = ConfigHelper.GetSysConfigValueWithoutLogin("MZBarcodeReturnReport");
                                if (iPrint is TJPaitent)
                                {
                                    reutrnTemplate = ConfigHelper.GetSysConfigValueWithoutLogin("TJBarcodeReturnReport");
                                }
                                if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["BPPrintReturn"]) &&
                                    System.Configuration.ConfigurationManager.AppSettings["BPPrintReturn"].ToUpper() == "Y")
                                {
                                    if (reutrnTemplate != null && reutrnTemplate.Trim() != string.Empty)
                                    {
                                        EntityDCLPrintParameter printPara = new EntityDCLPrintParameter();
                                        printPara.ReportCode = reutrnTemplate;

                                        foreach (EntitySampMain sm in barcodesNotPrinted)
                                        {
                                            printPara.ListBarId.Add(sm.SampBarId);
                                        }

                                        EntityDCLPrintData printReturnData = proxyPrint.Service.GetReportSource(printPara);

                                        if (printReturnData != null &&
                                            printReturnData.ReportData != null &&
                                            printReturnData.ReportData.Tables.Count > 0)
                                        {
                                            DataTable dt = printReturnData.ReportData.Tables["可设计字段"];
                                            HQ.BPPrint.MZReturnPrinter mzReturn = new HQ.BPPrint.MZReturnPrinter();
                                            mzReturn.Print(dt);
                                        }

                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        printBarcodeSuccess = PrintBarcodeWithMachine(iPrint, barcodesNotPrinted);
                    }

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

                //检查是否有不需要更新状态的条码信息，因为有些是外部条码打印，如体检检查条码打印
                for (int i2 = 0; i2 < barcodesNotPrinted.Count; i2++)
                {
                    EntitySampMain drv = barcodesNotPrinted[i2];

                    if (drv.PrintType == "TJPacs")
                    {
                        barcodesNotPrinted.Remove(drv);
                        i2--;
                    }
                }
                //如果检查完还有检验条码，则需要更新打印条码状态
                if (barcodesNotPrinted.Count > 0)
                {
                    #region 检验条码打印成功后更新状态
                    //打印成功后更新状态
                    sampOp.OperationTime = IStep.GetServerTime();// Convert.ToDateTime(iPrint.SignInfo.SignTime);
                    bool updateResult = false;
                    if (ShouldMultiSelect)
                    {
                        updateResult = proxy.Service.UpdateSampMainStatus(sampOp, barcodesNotPrinted);
                    }
                    else
                    {
                        if (Step.StepName != "条码查询")
                        {
                            List<EntitySampMain> list = new List<EntitySampMain>();
                            list.Add(CurrentSampMain);
                            updateResult = proxy.Service.UpdateSampMainStatus(sampOp, list);
                        }
                        else
                            updateResult = true;
                    }

                    if (!updateResult)
                    {
                        ShowAndClose("打印条码失败!", 1.5m);
                        return;
                    }
                    else
                    {
                        UpdatePrintFlag();
                        UpdateCount();

                        if (1 != 2)
                        {
                            if (ShouldMultiSelect)
                            {
                                List<EntitySampMain> rows = Selection.GetAllSelectSamp();
                            }
                            else
                            {
                                RefreshCurrentBarcode();
                            }
                        }
                    }
                    #endregion
                }

                if (printBarcodeSuccess)
                    Selection.ClearSelection();
            }

        }

        /// <summary>
        /// 医嘱状态检查
        /// </summary>
        /// <param name="barcodesNotPrinted"></param>
        /// <param name="userid"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        private string SampMainCheck(ref List<EntitySampMain> barcodesNotPrinted, string userid, string username)
        {
            List<EntitySampMain> listConfirmPrint = new List<EntitySampMain>();

            string strMsg = string.Empty;

            foreach (EntitySampMain sm in barcodesNotPrinted)
            {
                EntitySampOperation sign = new EntitySampOperation(userid, username);
                sign.OperationStatus = "1";
                sign.OperationStatusName = "打印条码";
                sign.OperationPlace = LocalSetting.Current.Setting.CType_name;
                sign.Remark = string.Format("IP地址:{0}", IPUtility.GetIP());
                sign.OperationTime = IStep.GetServerTime();
                sign.OperationWorkId = userid;

                ProxySampMain proxy = new ProxySampMain();
                string strResultMsg = proxy.Service.ConfirmBeforeCheck(sign, sm);

                if (strResultMsg.Trim() != string.Empty)
                {
                    strMsg += string.Format("条码号 [{0}]  姓名 [{1}]\r\n", sm.SampBarCode, sm.PidName);
                }
                else
                    listConfirmPrint.Add(sm);
            }

            barcodesNotPrinted = listConfirmPrint;

            if (strMsg != string.Empty)
            {
                strMsg = "以下条码已停止医嘱,请删除:\r\n" + strMsg + "\r\n";
            }

            return strMsg;
        }


        private void InvokEXE(string op, string FileName)
        {
            Process p = new Process();

            p.StartInfo.UseShellExecute = false;

            p.StartInfo.RedirectStandardOutput = true;

            p.StartInfo.FileName = FileName;

            p.StartInfo.CreateNoWindow = true;

            p.StartInfo.Arguments = op;

            p.Start();

            p.WaitForExit();

            string output = p.StandardOutput.ReadToEnd();
        }

        /// <summary>
        /// 打印回退条码
        /// </summary>
        /// <param name="iPrint"></param>
        public void PrintBarcodeReturn(IPrint iPrint)
        {
            if (HasNotChoose())
            {
                ShowAndClose("请选择一条记录!");
                return;
            }
            string sampleMessage = string.Empty;
            if (SelectBarcodeNotSampleID(iPrint, ref sampleMessage))//没有标本的条码提示
            {
                ShowMessage("以下条码没有标本,不能打印,请在右边资料修改标本!" + "\r\n" + sampleMessage);
                return;
            }

            string barcodeMessage = string.Empty;
            if (SelectBarcodeNotBarCode(iPrint, ref barcodeMessage))//没有标本的条码提示
            {
                ShowMessage("以下条码没有扫入试管号,不能打印,请在上方扫入条码!" + "\r\n" + barcodeMessage);
                return;
            }
            List<EntitySampMain> barcodesNotPrinted;//未打印条码
            List<EntitySampMain> barcodesPrinted;//已打印(未采集)/已回退条码
            List<EntitySampMain> barcodesCanNotPrint;//不能再打印(已采集未回退)的条码
            List<EntitySampMain> barcodesNotPower;//没有权限打印已打印的条码
            List<EntitySampMain> barcodesNeedPrint;//需要打印的条码

            SelectBarcodeRePrint(iPrint, out barcodesNotPrinted, out barcodesPrinted, out barcodesCanNotPrint, out barcodesNotPower);
            barcodesNeedPrint = new List<EntitySampMain>();
            //所有条码状态都可打印回执
            barcodesNeedPrint.AddRange(barcodesNotPrinted);
            barcodesNeedPrint.AddRange(barcodesPrinted);
            barcodesNeedPrint.AddRange(barcodesCanNotPrint);
            if (iPrint.ShowSpecialComfirmWhenPrint() && barcodesNeedPrint.Count > 0)
            {

                //打印机打印
                try
                {
                    if (HQ.BPPrint.BPPrintSetting.Current.EnableBPPrint && Barcode_ReuturnPrinter != "检验报告")
                    {
                        if (iPrint is OutPaitent || iPrint is TJPaitent)
                        {
                            string reutrnTemplate = ConfigHelper.GetSysConfigValueWithoutLogin("MZBarcodeReturnReport");
                            if (iPrint is TJPaitent)
                            {
                                reutrnTemplate = ConfigHelper.GetSysConfigValueWithoutLogin("TJBarcodeReturnReport");
                            }
                            if (reutrnTemplate != null && reutrnTemplate.Trim() != string.Empty)
                            {
                                EntityDCLPrintParameter printPara = new EntityDCLPrintParameter();
                                printPara.ReportCode = reutrnTemplate;

                                foreach (EntitySampMain drBarCode in barcodesNeedPrint)
                                {
                                    printPara.ListBarId.Add(drBarCode.SampBarCode);
                                }
                                ProxyReportPrint proxyPrint = new ProxyReportPrint();
                                EntityDCLPrintData printData = proxyPrint.Service.GetReportSource(printPara);

                                if (printData != null &&
                                    printData.ReportData != null &&
                                    printData.ReportData.Tables.Count > 0)
                                {
                                    DataTable dt = printData.ReportData.Tables["可设计字段"];
                                    HQ.BPPrint.MZReturnPrinter mzReturn = new HQ.BPPrint.MZReturnPrinter();
                                    mzReturn.Print(dt);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (PrintBarcodeReturnWithMaching(iPrint, barcodesNeedPrint) == false)
                        {
                            ShowAndClose("打印条码失败");
                            return;
                        }
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
            }
            else
            {
                if (barcodesNeedPrint.Count > 0)
                {
                    ShowMessage("该条码不能打印回执!");
                    return;
                }

            }

        }

        /// <summary>
        /// 条码查询界面打印条码回执
        /// </summary>
        /// <param name="iPrint"></param>
        public void PrintBarcodeReturnBySearch(IPrint iPrint)
        {
            if (HasNotSelect() && (ShouldMultiSelect && HasNotChoose()))
            {
                ShowAndClose("请勾选需要打印回执的条码!");
                return;
            }

            List<EntitySampMain> barcodesNeedPrint = new List<EntitySampMain>();//需要打印的条码
            List<EntitySampMain> barcodesCanNotPrint = new List<EntitySampMain>();//不能打印的条码
         
            if (gcBarcode.IsPrintingAvailable)
            {
                List<string> listBarId = new List<string>();

                List<EntitySampMain> dtSour = new List<EntitySampMain>();

                if (Selection == null)
                    dtSour = ListSampMain;
                else
                    dtSour = Selection.GetAllSelectSamp();

                foreach (EntitySampMain drSour in dtSour)
                {
                    if (drSour.SampStatusId == "0")
                        barcodesNeedPrint.Add(drSour);
                    else
                        barcodesCanNotPrint.Add(drSour);
                }

                if(barcodesCanNotPrint.Count>0)
                {
                    MessageBox.Show(string.Format("存在不能打印回执的条码:{0}", barcodesCanNotPrint[0].SampBarId));
                    return;
                }
                //打印机打印
                try
                {
                    //查询是否配置了报表打印回执，否则用条码机打印标签回执
                    if (!string.IsNullOrEmpty(ConfigHelper.GetSysConfigValueWithoutLogin("BarcodeSearchReturnReport")))
                    {
                        if (PrintBarcodeReturnWithDefaultPrinter(barcodesNeedPrint) == false)
                        {
                            ShowAndClose("打印条码回执失败");
                            return;
                        }
                    }
                    else
                    {
                        if (PrintBarcodeReturnWithMaching(iPrint, barcodesNeedPrint) == false)
                        {
                            ShowAndClose("打印条码回执失败");
                            return;
                        }
                    }

                }
                catch (ReportNotFoundException ex)
                {
                    ShowMessage("打印条码回执失败:" + ex.MSG);
                    return;
                }

            }
            else
                ShowAndClose("打印出错!请检查打印机是否安装正确...");
        }

        private bool HasNotChoose()
        {
            return Selection.SelectedCount <= 0;
        }

        ////<summary>
        ////打印条码
        ////</summary>
        public void PrintBarcodeAuto(int printtimes)
        {
            bool selectCheckedBar = ShouldMultiSelect && !HasNotChoose();

            if (HasNotSelect() && (ShouldMultiSelect && HasNotChoose()))
            {
                ShowAndClose("请选择一条记录!");
                return;
            }

            string sampleMessage = "";
            if (SelectBarcodeNotSampleID(new EmptyPrinter(), ref sampleMessage))
            {
                ShowMessage("以下条码没有标本,不能打印!" + "\r\n" + sampleMessage);
                return;
            }

            EntitySampMain sampMain = (EntitySampMain)bsPatient.Current;

            if (sampMain.SampBarCode == string.Empty)
            {
                lis.client.control.MessageDialog.Show("此条码没有扫入试管号,不能打印!");
                return;
            }

            string username = "";
            string userid = "";
            bool isAudit = false;
            bool isNeedAudit = false;

            //系统配置：[处理回退条码]身份验证方式
            if (isNeedAudit || ConfigHelper.GetSysConfigValueWithoutLogin("BC_BCSearchPrintBcNeedAudit") == "是")
            {
                isAudit = true;
                FrmCheckPassword frm = new FrmCheckPassword();
                if (frm.ShowDialog() != DialogResult.OK) return;
                userid = frm.OperatorID;
                username = frm.OperatorName;
            }
            Step.BaseSampMain = BaseSampMain;
            Step.Printer = new EmptyPrinter();

            #region 生成二维码
            //系统配置：条码打印时生成二维码
            if (ConfigHelper.GetSysConfigValueWithoutLogin("PrintBarcode_CreateQcCode") == "是")
            {
                QcCodeHelper qccode = new QcCodeHelper();
                qccode.DelOldFile();

                if (Selection != null)
                {
                    foreach (DataRowView item in Selection.GetAllSelectRow())
                    {
                        if (!string.IsNullOrEmpty(item.Row["bc_bar_code"].ToString()))
                        {
                            qccode.CreateQcCodePic(item.Row["bc_bar_code"].ToString());
                        }
                    }
                }
                else if (CurrentSampMain != null)
                {
                    if (!string.IsNullOrEmpty(CurrentSampMain.SampBarId))
                    {
                        qccode.CreateQcCodePic(CurrentSampMain.SampBarId);
                    }

                }

            }
            #endregion

            if (string.IsNullOrEmpty(userid))
            {
                username = UserInfo.userName;
                userid = UserInfo.loginID;
            }
            IPrint iPrint = new EmptyPrinter();

            EntitySampOperation oper = new EntitySampOperation(userid, username);

            if (iPrint.ShowSpecialComfirmWhenPrint())
            {
                //打印机打印
                try
                {
                    bool blTest = false;
                    for (int i = 0; i < printtimes; i++)
                    {
                        if (UserInfo.GetSysConfigValue("BarCode_EnableLabellerPrint") == "是"
                            && GetPrintConfig().Columns.Contains("PrintMode")
                            && GetPrintConfig().Rows[0]["PrintMode"].ToString() == "False")
                        {
                            if (Selection != null)
                            {
                                if (GetPrintConfig().Rows[0]["PrintWay"].ToString() == "DLL")
                                    blTest = HQ.LabellerPrint.PrintHelper.Instance.print("t01.txt", Selection.GetAllSelectRow());
                                else
                                {
                                    string str = "";
                                    foreach (DataRowView item in Selection.GetAllSelectRow())
                                    {
                                        str += "," + item["bc_bar_code"].ToString();
                                    }
                                    str = str.Remove(0, 1);
                                    InvokEXE(str, "HQ.LabellerPrint.Client.exe");
                                    blTest = true;
                                }
                            }
                            else if (CurrentSampMain != null)
                            {
                                List<DataRow> list = new List<DataRow>();
                                DataRow[] rowList = list.ToArray();
                                if (GetPrintConfig().Rows[0]["PrintWay"].ToString() == "DLL")
                                    blTest = HQ.LabellerPrint.PrintHelper.Instance.print("t01.txt", rowList);
                                else
                                {
                                    string str = "";
                                    str = CurrentSampMain.SampBarId;
                                    InvokEXE(str, "HQ.LabellerPrint.Client.exe");
                                    blTest = true;
                                }
                            }
                        }
                        else if (HQ.BPPrint.PrinterController.UseCustomPrintMode)
                        {
                            if (Selection != null)
                            {
                                blTest = HQ.BPPrint.PrinterController.PrintBarcode(Selection.GetAllSelectRow());
                            }
                            else if (CurrentSampMain != null)
                            {
                            }
                        }
                        else if (HQ.BPPrint.BPPrintSetting.Current.EnableBPPrint)
                        {
                            if (Selection != null)
                            {
                                blTest = HQ.BPPrint.LisBarcodePrinter.Print(Selection.GetAllSelectRow());
                            }
                            else if (CurrentSampMain != null)
                            {
                            }
                        }
                        else
                        {
                            if (Selection != null)
                                blTest = PrintBarcodeWithMachineAuto(Selection.GetAllSelectSamp());
                            else
                                blTest = PrintBarcodeWithMachineAuto(null);
                        }
                    }

                    if (blTest == false)
                    {
                        ShowAndClose("打印条码失败");
                        return;
                    }
                    else if (stepType == StepType.Select)
                    {
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

                if (!isNotUpdateFlag)
                //打印成功后更新状态
                {
                    oper.Remark = "条码查询打印,不更改标本最后状态。";

                    bool updateResult = false;
                    if (selectCheckedBar || isAudit)
                    {
                        List<EntitySampMain> rowViews = Selection.GetAllSelectSamp();

                        if (!selectCheckedBar && rowViews.Count == 0)
                        {
                            EntitySampMain dvr = (EntitySampMain)(MainGridView.GetFocusedRow());
                            rowViews = new List<EntitySampMain>();
                            rowViews.Add(dvr);
                        }
                        updateResult = Step.UpdateStatus(oper, rowViews);
                    }
                    else
                    {
                        updateResult = Step.UpdateStatus(oper);
                    }

                    if (!updateResult)
                    {
                        ShowAndClose("打印条码失败!", 1.5m);
                        return;
                    }
                    else
                    {
                        if (stepType != StepType.Select)
                        {
                            UpdatePrintFlag();
                            UpdateCount();
                        }


                        if (selectCheckedBar)
                        {
                            List<EntitySampMain> rows = Selection.GetAllSelectSamp();
                            RefreshRow(rows);
                        }
                        else
                            RefreshCurrentBarcode();

                        MoveNext();
                    }
                }
            }
        }


        /// <summary>
        /// 打印条码信息
        /// </summary>
        internal void PrintBarCodeInfo()
        {
            if (!HasData() || (Selection != null && Selection.GetAllSelectRow().Length <= 0))
            {
                ShowAndClose("没打印数据!");
                return;
            }

            if (gcBarcode.IsPrintingAvailable)
            {
                List<string> listBarId = new List<string>();

                List<EntitySampMain> dtSour = new List<EntitySampMain>();

                if (Selection == null)
                    dtSour = ListSampMain;
                else
                    dtSour = Selection.GetAllSelectSamp();

                foreach (EntitySampMain drSour in dtSour)
                {
                    listBarId.Add(drSour.SampBarId);
                }

                if (listBarId.Count > 0)
                {
                    EntityDCLPrintParameter printPara = new EntityDCLPrintParameter();
                    printPara.ReportCode = "printBarCodeInfo";
                    printPara.ListBarId = listBarId;

                    DCLReportPrint.Print(printPara);
                }
                else
                {
                    ShowAndClose("没打印数据!");
                    return;

                }
            }
            else
                ShowAndClose("打印出错!请检查打印机是否安装正确.");
        }

        /// <summary>
        /// 获取打印条码信息
        /// </summary>
        /// <param name="iPrint"></param>
        /// <param name="barcodesNotPrinted"></param>
        /// <param name="barcodesPrinted"></param>
        /// <param name="barcodesCanNotPrint"></param>
        private void SelectBarcodeRePrint(IPrint iPrint
                                                    , out List<EntitySampMain> barcodesNotPrinted
                                                    , out List<EntitySampMain> barcodesPrinted
                                                    , out List<EntitySampMain> barcodesCanNotPrint
                                                    , out List<EntitySampMain> barcodesNotPower
                                                    )
        {
            bool printFunction = true;
            if (!alone)
            {
                UserInfo.SkipPower = false;
                printFunction = UserInfo.HaveFunction("FrmBCPrint_InpatientPrint", "FrmBCPrint_InpatientPrint");
            }

            barcodesNotPrinted = new List<EntitySampMain>();//未打印条码
            barcodesPrinted = new List<EntitySampMain>();//已打印(未采集)/已回退条码
            barcodesCanNotPrint = new List<EntitySampMain>();//不能再打印(已采集未回退)的条码
            barcodesNotPower = new List<EntitySampMain>();//没有权限打印已打印的条码

            List<EntitySampMain> rows = Selection.GetAllSelectSamp();
            ProxySampProcessDetail proxyProcess = new ProxySampProcessDetail();
            foreach (EntitySampMain item in rows)
            {
                EntitySampProcessDetail process = proxyProcess.Service.GetLastSampProcessDetail(item.SampBarId);

                if (process != null && !string.IsNullOrEmpty(process.ProcBarno))
                {
                    if (Compare.IsEmpty(process.ProcStatus) //状态代码为空
                        || process.ProcStatus == "0"//未打印 
                        || process.ProcStatus == "170" //标本资料修改k
                        || process.ProcStatus == "570" //重置预置条码
                        )
                    {
                            barcodesNotPrinted.Add(item);
                    }

                    else if (
                        process.ProcStatus == EnumBarcodeOperationCode.BarcodePrint.ToString()
                             || process.ProcStatus == EnumBarcodeOperationCode.SampleReturn.ToString()
                        || (iPrint is Manual && process.ProcStatus == EnumBarcodeOperationCode.SampleReceive.ToString())

                        )
                    {
                        if (Barcode_EnablePrintAuthoryJudge)
                        {
                            if (iPrint is Inpatient && !printFunction)
                                barcodesNotPower.Add(item);
                            else
                                barcodesPrinted.Add(item);
                        }
                        else
                        {
                            if (process.ProcStatus == EnumBarcodeOperationCode.SampleCollect.ToString())
                            {
                                barcodesCanNotPrint.Add(item);
                            }
                            else
                            {
                                barcodesPrinted.Add(item);
                            }
                        }
                    }



                    else if (Convert.ToInt32(process.ProcStatus) >= EnumBarcodeOperationCode.SampleCollect
                        &&
                        Convert.ToInt32(process.ProcStatus) != EnumBarcodeOperationCode.DeleteBarcode //510
                        && Convert.ToInt32(process.ProcStatus) != EnumBarcodeOperationCode.DeleteDetail //500
                        && Convert.ToInt32(process.ProcStatus) != EnumBarcodeOperationCode.SampleReturn
                         && Convert.ToInt32(process.ProcStatus) != 170//标本资料修改
                        )
                    {
                        barcodesCanNotPrint.Add(item);
                    }

                }
            }
        }

        /// <summary>
        /// 选择的条码是否标本为空
        /// </summary>
        /// <param name="samepleMessage"></param>
        /// <returns></returns>
        private bool SelectBarcodeNotSampleID(IPrint iPrint, ref string sampleMessage)
        {
            bool result = false;

            if (iPrint is Manual)//手工
            {
                if (ConfigHelper.GetSysConfigValueWithoutLogin("InPrintBarcodeSampleIDIsNull_SG") != "否")//打印条码标本允许为空
                    return result;
            }
            if (iPrint is TJPaitent)//体检
            {
                if (ConfigHelper.GetSysConfigValueWithoutLogin("InPrintBarcodeSampleIDIsNull_TJ") == "是")//打印条码标本允许为空
                    return result;
            }
            if (iPrint is Inpatient)//住院
            {
                if (ConfigHelper.GetSysConfigValueWithoutLogin("InPrintBarcodeSampleIDIsNull_ZY") == "是")//打印条码标本允许为空
                    return result;
            }
            if (iPrint is OutPaitent)//门诊
            {
                if (ConfigHelper.GetSysConfigValueWithoutLogin("InPrintBarcodeSampleIDIsNull_MZ") == "是")//打印条码标本允许为空
                    return result;
            }

            if (Selection != null)
            {
                List<EntitySampMain> rows = Selection.GetAllSelectSamp();
                foreach (EntitySampMain row in rows)
                {
                    //如果是非检验条码则不判断标本是否为空
                    if (row.PrintType == "TJPacs")
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(row.SampSamId))
                    {
                        sampleMessage += string.Format("条码号 [{0}] ", row.SampBarCode) + "\r\n";
                        result = true;
                    }
                }

                return result;
            }
            else
                return string.IsNullOrEmpty(BaseSampMain.SampSamId);
        }

        /// <summary>
        /// 选择的条码是否填入预置条码
        /// </summary>
        /// <param name="samepleMessage"></param>
        /// <returns></returns>
        private bool SelectBarcodeNotBarCode(IPrint iPrint, ref string sampleMessage)
        {
            bool result = false;
            //住院
            {
                if (ConfigHelper.GetSysConfigValueWithoutLogin("InPrintBarcodeBarcodeIsNull_ZY") == "是")//[住院]打印条码预置条码允许为空
                    return result;
            }

            List<EntitySampMain> rows = Selection.GetAllSelectSamp();
            foreach (EntitySampMain row in rows)
            {

                if (string.IsNullOrEmpty(row.SampBarCode))
                {
                    sampleMessage += string.Format("条码号 [{0}] ", row.SampBarCode) + "\r\n";
                    result = true;
                }

            }

            return result;
        }

        /// <summary>
        /// 选择的条码是否年龄为空
        /// </summary>
        /// <param name="iPrint"></param>
        /// <param name="sampleMessage"></param>
        /// <returns></returns>
        private bool SelectBarcodeNotAge(IPrint iPrint, ref string sampleMessage)
        {

            bool result = false;

            if (iPrint is Manual)//手工
            {
                return result;
            }
            if (iPrint is TJPaitent)//体检
            {
                if (ConfigHelper.GetSysConfigValueWithoutLogin("InPrintBarcodeAgeIsNull_TJ") == "是")//打印条码年龄允许为空
                    return result;
            }
            if (iPrint is OutPaitent)//门诊
            {
                if (ConfigHelper.GetSysConfigValueWithoutLogin("InPrintBarcodeAgeIsNull_MZ") == "是")//打印条码年龄允许为空
                    return result;
            }
            if (iPrint is Inpatient)//住院
            {
                if (ConfigHelper.GetSysConfigValueWithoutLogin("InPrintBarcodeAgeIsNull_ZY") == "是")//打印条码年龄允许为空
                    return result;
            }

            if (iPrint.ShouldMergeCollect)
            {
                // Selection.View = MainGridView;
                List<EntitySampMain> rows = Selection.GetAllSelectSamp();
                foreach (EntitySampMain row in rows)
                {
                    //如果是非检验条码则不判断年龄是否为空
                    if (row.PrintType == "TJPacs")
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(row.PidAge))
                    {
                        sampleMessage += string.Format("条码号 [{0}] ", row.SampBarCode) + "\r\n";
                        result = true;
                    }
                }

                return result;
            }
            else
                return string.IsNullOrEmpty(BaseSampMain.PidAge);
        }

        /// <summary>
        /// 选择的条码是否性别为空
        /// </summary>
        /// <param name="iPrint"></param>
        /// <param name="sampleMessage"></param>
        /// <returns></returns>
        private bool SelectBarcodeNotSex(IPrint iPrint, ref string sampleMessage)
        {
            bool result = false;

            if (iPrint is Manual)
            {
                return result;
            }
            if (iPrint is TJPaitent)//体检
            {
                if (ConfigHelper.GetSysConfigValueWithoutLogin("InPrintBarcodeSexIsNull_TJ") == "是")//打印条码性别允许为空
                    return result;
            }
            if (iPrint is OutPaitent)//门诊
            {
                if (ConfigHelper.GetSysConfigValueWithoutLogin("InPrintBarcodeSexIsNull_MZ") == "是")//打印条码性别允许为空
                    return result;
            }
            if (iPrint is Inpatient)//住院
            {
                if (ConfigHelper.GetSysConfigValueWithoutLogin("InPrintBarcodeSexIsNull_ZY") == "是")//打印条码性别允许为空
                    return result;
            }

            if (iPrint.ShouldMergeCollect)
            {
                List<EntitySampMain> rows = Selection.GetAllSelectSamp();
                foreach (EntitySampMain row in rows)
                {
                    //如果是非检验条码则不判断年龄是否为空
                    if (row.PrintType == "TJPacs")
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(row.PidSex))
                    {
                        sampleMessage += string.Format("条码号 [{0}] ", row.SampBarCode) + "\r\n";
                        result = true;
                    }
                }
                return result;
            }
            else
                return string.IsNullOrEmpty(BaseSampMain.PidSex);
        }

        /// <summary>
        /// 选择的条码是否未处理回退条码
        /// </summary>
        /// <param name="iPrint"></param>
        /// <param name="sampleMessage"></param>
        /// <returns></returns>
        private bool SelectBarcodeNotTreatReturmMsg(IPrint iPrint, ref string sampleMessage)
        {
            bool result = false;

            if (iPrint is Manual)
            {
                return result;
            }
            if (iPrint is TJPaitent)//体检
            {
                return result;
            }
            if (iPrint is OutPaitent)//门诊
            {
                if (ConfigHelper.GetSysConfigValueWithoutLogin("InPrintNotConfirmRmsg_MZ") != "是")//未处理的回退条码不可打印
                    return result;
            }
            if (iPrint is Inpatient)//住院
            {
                if (ConfigHelper.GetSysConfigValueWithoutLogin("InPrintNotConfirmRmsg_ZY") != "是")//未处理的回退条码不可打印
                    return result;
            }

            //记录选中的条码号
            List<string> temp_list_SelectRowbc = new List<string>();

            List<EntitySampMain> rows = Selection.GetAllSelectSamp();

            #region 更新选择的条码状态

            foreach (EntitySampMain row in rows)
            {
                temp_list_SelectRowbc.Add(row.SampBarCode);
            }

            if (temp_list_SelectRowbc.Count > 0)
            {
                RefreshRow(rows);//更新选择的条码状态
                for (int j = 0; j < temp_list_SelectRowbc.Count; j++)
                {
                    SelectRow(temp_list_SelectRowbc[j], false);
                }

                rows = Selection.GetAllSelectSamp();
            }

            #endregion

            foreach (EntitySampMain row in rows)
            {
                //如果是非检验条码则不判断
                if (row.PrintType == "TJPacs")
                {
                    continue;
                }

                if (row.SampStatusId == "9" && row.SampReturnFlag)
                {
                    sampleMessage += string.Format("条码号 [{0}] ", row.SampBarCode) + "\r\n";
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取条码打印机名称
        /// </summary>
        /// <returns></returns>
        private DataTable GetPrintConfig()
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
                        return dt;
                    }
                }
            }

            return null;
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
        /// 获取检验报告打印机名称
        /// </summary>
        /// <returns></returns>
        private string GetLisPrintMachineName()
        {
            string printname = string.Empty;
            if (File.Exists(xmlFile))
            {
                DataSet dsPrint = new DataSet();
                dsPrint.ReadXml(xmlFile);
                if (dsPrint.Tables.Count > 0)
                {
                    DataTable dt = dsPrint.Tables[0];
                    if (dt != null && dt.Columns.Contains("ReturnPrintName"))
                    {
                        printname = dt.Rows[0]["ReturnPrintName"].ToString();
                    }
                }
            }

            if (string.IsNullOrEmpty(printname) && File.Exists(xmlLisFile))
            {
                DataSet dsPrint = new DataSet();
                dsPrint.ReadXml(xmlLisFile);
                if (dsPrint.Tables.Count > 0)
                {
                    DataTable dt = dsPrint.Tables[0];
                    if (dt != null)
                    {
                        return dt.Rows[0]["printName"].ToString();
                    }
                }
            }

            return printname;
        }

       

        /// <summary>
        /// 调用打印机打印条码
        /// </summary>
        private bool PrintBarcodeWithMachine(IPrint iPrint, List<EntitySampMain> listSampMain)
        {
            List<string> lisBC = new List<string>();
            List<EntitySampMain> lisPrint = new List<EntitySampMain>();
            if (ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_BCSampleID").Trim() != string.Empty)
            {
                string samId = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_BCSampleID").Trim();
                foreach (EntitySampMain item in listSampMain)
                {
                    if (item.SampSamId == samId)
                        lisBC.Add(item.SampBarId.ToString());
                    else
                        lisPrint.Add(item);
                }
                listSampMain = lisPrint;
            }

            string printMachineName = GetPrintMachineName();
            if (string.IsNullOrEmpty(printMachineName))
            {
                throw new BarcodePrinterNotFoundException();

            }

            //22
            ZeBraPrinter machine = new ZeBraPrinter();

            List<EntitySampMain> idList = new List<EntitySampMain>();
            List<EntitySampMain> idsMoreThanOne = new List<EntitySampMain>();//打印次数超过1的条码ID
            IList<int> idsMoreThanOneCount = new List<int>();//打印次数

            Dictionary<string, int> dictMoreThanOne = new Dictionary<string, int>();
            IList<String> printIDForMZReturn = new List<String>(); //门诊打印回执用的bc_in_no
            List<string> combinesName = new List<string>();

            Dictionary<string, string> dictCtype = new Dictionary<string, string>();
            Dictionary<string, string> dictCombinesName = new Dictionary<string, string>();
            //添加手工条码也打印回执
            bool blnManualReturn = false;
            if (iPrint is Manual && m_strOriId == "107" && m_blnCheckManualReturn)
            {

                blnManualReturn = true;
            }


            Boolean shouldPrintReturn =
                        iPrint != null &&
                        (
                        ((iPrint is Manual) && !String.IsNullOrEmpty(ConfigHelper.GetSysConfigValueWithoutLogin("MZBarcodeReturnReport")) && blnManualReturn)
                        ||
                        ((iPrint is OutPaitent) && !String.IsNullOrEmpty(ConfigHelper.GetSysConfigValueWithoutLogin("MZBarcodeReturnReport")) && this.AutoPrintReturn)
                        ||
                        ((iPrint is TJPaitent) && !String.IsNullOrEmpty(ConfigHelper.GetSysConfigValueWithoutLogin("TJBarcodeReturnReport")) && this.AutoPrintReturn)
                        );


            //记录手工条码打印条码病人信息，每个病人为一个list
            Dictionary<string, List<string>> dtnManalPatientInfo = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> dtnManalPatientCombine = new Dictionary<string, List<string>>();


            if (iPrint != null)
            {
                foreach (EntitySampMain row in listSampMain)
                {
                    //正常检验条码打印记录
                    if (BarcodePrintOnce(row))
                    {
                        idList.Add(row);
                        row.SampPrintTime = 1;
                    }
                    else
                    {
                        idList.Add(row);

                        dictMoreThanOne.Add(row.SampSn.ToString(), row.SampPrintTime);

                        idsMoreThanOne.Add(row);
                        idsMoreThanOneCount.Add(row.SampPrintTime);
                    }

                    if (shouldPrintReturn && !printIDForMZReturn.Contains(row.SampBarCode))//需要门诊打印回执
                    {
                        printIDForMZReturn.Add(row.SampBarCode);
                        combinesName.Add(row.SampComName);
                        dictCtype.Add(row.SampBarCode, row.SampType);
                        dictCombinesName.Add(row.SampBarCode, row.SampComName);
                        //如果是要打印手工条码进入数据处理
                        if (this.m_blnCheckManualReturn)
                        {
                            //这样处理是为了区别多个病人的条码回执数据
                            if (dtnManalPatientInfo.ContainsKey(row.PidName))
                            {
                                dtnManalPatientInfo[row.PidName].Add(row.SampBarCode);
                                dtnManalPatientCombine[row.PidName].Add(row.SampComName);
                            }
                            else
                            {
                                List<string> lstBarcode = new List<string>();
                                lstBarcode.Add(row.SampBarCode);
                                dtnManalPatientInfo.Add(row.PidName, lstBarcode);

                                List<string> lstCombine = new List<string>();
                                lstCombine.Add(row.SampComName);
                                dtnManalPatientCombine.Add(row.PidName, lstCombine);
                            }
                        }
                    }

                }
            }

            string template = "";
            string preTemplate = string.Empty;
            if (alone)
            {
                if (iPrint is TJPaitent)
                {
                    template = "TJBarcodeReport";
                    preTemplate = "TJPreBarcodeReport";
                }
                else if (iPrint is Inpatient)
                {
                    template = "ZYBarcodeReport";
                    preTemplate = "ZYPreBarcodeReport";
                }
                else if (iPrint is OutPaitent)
                {
                    template = "MZBarcodeReport";
                    preTemplate = "MZPreBarcodeReport";
                }
                else if (iPrint is Manual)
                {
                    if (IsYG)
                    {
                        //院感
                        template = ConfigHelper.GetSysConfigValueWithoutLogin("YGBarcodeReport");
                    }
                    else
                    {
                        template = "ManualBarcodeReport";
                    }
                }
            }
            else
            {
                template = GetReportTemplate(iPrint);
                preTemplate = GetReportPreTemplate(iPrint);
            }
            bool result = false;
            try
            {
                if (HasNotChoose())
                {
                    lis.client.control.MessageDialog.Show("没有选择条码", "提示");
                    return false;
                }

                if (idList != null && idList.Count > 0)
                {
                    if (idsMoreThanOne.Count == 0)
                    {
                        List<string> listBarId = new List<string>();
                        List<string> listPreBarId = new List<string>();

                        foreach (EntitySampMain item in idList)
                        {
                            if (item.SampBarType == 1)
                                listPreBarId.Add(item.SampSn.ToString());
                            else
                                listBarId.Add(item.SampSn.ToString());
                        }

                        if (listBarId.Count > 0)
                        {
                            machine.PrintInfo = new PrintInfo(listBarId);
                            result = machine.Print(printMachineName, template);
                        }
                        if (listPreBarId.Count > 0 && !string.IsNullOrEmpty(preTemplate))
                        {
                            machine.PrintInfo = new PrintInfo(listPreBarId);
                            result = machine.Print(printMachineName, preTemplate);
                        }
                    }
                    else
                    {
                        IList<EntitySampMain> tmpList = new List<EntitySampMain>();
                        foreach (EntitySampMain row in idList)
                        {
                            string bcid = row.SampSn.ToString();
                            if (dictMoreThanOne.ContainsKey(bcid))
                            {
                                if (tmpList.Count > 0)
                                {
                                    List<string> listBarId = new List<string>();
                                    List<string> listPreBarId = new List<string>();

                                    foreach (EntitySampMain item in tmpList)
                                    {
                                        if (item.SampBarType == 1)
                                            listPreBarId.Add(item.SampSn.ToString());
                                        else
                                            listBarId.Add(item.SampSn.ToString());
                                    }

                                    if (listBarId.Count > 0)
                                    {
                                        machine.PrintInfo = new PrintInfo(listBarId);
                                        result = machine.Print(printMachineName, template);
                                    }
                                    if (listPreBarId.Count > 0 && !string.IsNullOrEmpty(preTemplate))
                                    {
                                        machine.PrintInfo = new PrintInfo(listPreBarId);
                                        result = machine.Print(printMachineName, preTemplate);
                                    }
                                }
                                tmpList.Clear();

                                for (int j = 0; j < dictMoreThanOne[bcid]; j++)
                                {

                                    machine.PrintInfo = new PrintInfo(new List<string>() { row.SampSn.ToString() });

                                    if (row.SampBarType == 1 && !string.IsNullOrEmpty(preTemplate))
                                        result = machine.Print(printMachineName, preTemplate);
                                    else
                                        result = machine.Print(printMachineName, template);
                                }
                            }
                            else
                            {
                                tmpList.Add(row);
                            }
                        }
                        if (tmpList.Count > 0)
                        {
                            List<string> listBarId = new List<string>();

                            foreach (var item in tmpList)
                            {
                                listBarId.Add(item.SampSn.ToString());
                            }

                            machine.PrintInfo = new PrintInfo(listBarId);
                            result = machine.Print(printMachineName, template);
                        }
                    }
                }

                if (lisBC.Count > 0)
                {
                    machine.PrintInfo = new PrintInfo(lisBC);
                    result = machine.Print(printMachineName, "BCBarcodeReport");
                }

                if (shouldPrintReturn)
                {
                    //33
                    string reutrnTemplate = ConfigHelper.GetSysConfigValueWithoutLogin("MZBarcodeReturnReport");
                    //1
                    string printerNamew = printMachineName;
                    if (Barcode_ReuturnPrinter == "检验报告")
                    {
                        printerNamew = GetLisPrintMachineName();
                    }
                    if (this.m_blnCheckManualReturn)
                    {
                        foreach (string strKey in dtnManalPatientInfo.Keys)
                        {
                            machine.PrintInfo = new PrintInfo(dtnManalPatientInfo[strKey]);
                            result = machine.Print(printerNamew, reutrnTemplate, BarcodeTable.Patient.BarcodeDisplayNumber, string.Join("+", dtnManalPatientCombine[strKey].ToArray()));
                        }
                    }
                    else
                    {
                        if (iPrint is OutPaitent)
                        {
                            if (!string.IsNullOrEmpty(BC_MZFilterCTypeForPrintReturn))
                            {
                                string[] ctypes = BC_MZFilterCTypeForPrintReturn.Split(',');

                                for (int i = 0; i < ctypes.Length; i++)
                                {
                                    string typeid = ctypes[i];
                                    foreach (string bcid in dictCtype.Keys)
                                    {
                                        if (dictCtype[bcid] == typeid)
                                        {
                                            printIDForMZReturn.Remove(bcid);
                                            combinesName.Remove(dictCombinesName[bcid]);
                                        }
                                    }
                                }
                            }
                            if (printIDForMZReturn.Count > 0)
                            {
                                machine.PrintInfo = new PrintInfo(printIDForMZReturn);
                                result = machine.Print(printerNamew, reutrnTemplate,
                                                       BarcodeTable.Patient.BarcodeDisplayNumber,
                                                       string.Join("+", combinesName.ToArray()));
                            }
                        }
                        else if (iPrint is TJPaitent)
                        {

                            //体检传入标识到bc_address字段，个检为1，默认打印回执，团体为2，不打印回执。
                            if (ConfigHelper.GetSysConfigValueWithoutLogin("HospitalName").Contains("肿瘤") &&
                                listSampMain[0].PidAddress == "2")
                            {
                                shouldPrintReturn = false;
                            }

                            if (shouldPrintReturn)
                            {
                                reutrnTemplate = ConfigHelper.GetSysConfigValueWithoutLogin("TJBarcodeReturnReport");

                                List<string> listBarId = new List<string>();

                                foreach (var item in idList)
                                {
                                    listBarId.Add(item.SampBarId.ToString());
                                }

                                machine.PrintInfo = new PrintInfo(listBarId);
                                result = machine.Print(printMachineName, reutrnTemplate, BarcodeTable.Patient.ID, string.Join("+", combinesName.ToArray()));
                            }
                        }
                    }


                }

            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("打印出错:" + ex.Message);
                Logger.WriteException("条码", "条码调用打印器打印", ex.Message + "\r\n" + ex.StackTrace);
                return false;
            }


            return result;
        }

        /// <summary>
        /// 调用打印机打印条码回执
        /// </summary>
        /// <param name="iPrint"></param>
        /// <param name="dataToPrint"></param>
        /// <returns></returns>
        private bool PrintBarcodeReturnWithMaching(IPrint iPrint, List<EntitySampMain> listPrintSampMain)
        {
            string printMachineName = GetPrintMachineName();
            if (Barcode_ReuturnPrinter == "检验报告")
            {
                printMachineName = GetLisPrintMachineName();
            }
            if (string.IsNullOrEmpty(printMachineName))
            {
                throw new BarcodePrinterNotFoundException();
            }


            IList<string> idList = new List<string>();
            IList<string> idsMoreThanOne = new List<string>();//打印次数超过1的条码ID
            IList<int> idsMoreThanOneCount = new List<int>();//打印次数
            IList<String> printIDForReturn = new List<String>(); //打印回执用的bc_bar_code
            List<string> combinesName = new List<string>();


            Boolean shouldPrintReturn =
                iPrint != null &&
                (
                ((iPrint is OutPaitent) && !String.IsNullOrEmpty(ConfigHelper.GetSysConfigValueWithoutLogin("MZBarcodeReturnReport")))
                ||
                ((iPrint is TJPaitent) && !String.IsNullOrEmpty(ConfigHelper.GetSysConfigValueWithoutLogin("TJBarcodeReturnReport")))
                );

            IPrintMachine machine = new ZeBraPrinter();
            bool result = false;

            foreach (EntitySampMain row in listPrintSampMain)
            {
                printIDForReturn.Add(row.SampBarCode);

                combinesName.Add(row.SampPrintTime.ToString());
            }
            if (iPrint is OutPaitent && iPrint.ShouldMergeCollect)//打印门诊回执
            {
                if (shouldPrintReturn)
                {
                    if(listPrintSampMain.Count>1 && ConfigHelper.GetSysConfigValueWithoutLogin("mzreturnreportusesum")=="是")
                    {
                        //若系统配置打印回执时合并取报告时间，合并打印,处理数据
                        List<List<string>> printexp = new List<List<string>>();
                        combinesName.Clear();

                        Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
                        List<EntityDicCombine> cachedata = (new ProxyCacheData().Service.GetCacheData("EntityDicCombine")).GetResult() as List<EntityDicCombine>;
                        foreach (EntitySampMain row in listPrintSampMain)
                        {
                            string key = GetComKey(row, cachedata);
                            if(string.IsNullOrEmpty(key))
                            {
                                printexp.Add(new List<string> { row.SampBarCode } );
                                combinesName.Add(row.SampPrintTime.ToString());
                            }
                            else
                            {
                                if (dic.Keys.Contains(key))
                                    dic[key].Add(row.SampBarCode);
                                else
                                {
                                    dic.Add(key, new List<string> { row.SampBarCode });
                                    combinesName.Add(row.SampPrintTime.ToString());
                                }
                                    
                            }
                        }
                        foreach(KeyValuePair<string, List<string>> kv in dic)
                        {
                            printexp.Add(kv.Value);
                        }

                        printMachineName = GetPrintMachineName("ReturnPrintName");//调用回执条码的配置
                        string reutrnTemplate = ConfigHelper.GetSysConfigValueWithoutLogin("MZBarcodeReturnReport2");
                        
                        result = machine.PrintExReturnReport(printexp,printMachineName, reutrnTemplate, BarcodeTable.Patient.BarcodeDisplayNumber, string.Join("+", combinesName.ToArray()));

                    }
                    else
                    {
                        printMachineName = GetPrintMachineName("ReturnPrintName");//调用回执条码的配置
                        string reutrnTemplate = ConfigHelper.GetSysConfigValueWithoutLogin("MZBarcodeReturnReport");
                        machine.PrintInfo = new PrintInfo(printIDForReturn);
                        result = machine.Print(printMachineName, reutrnTemplate, BarcodeTable.Patient.BarcodeDisplayNumber, string.Join("+", combinesName.ToArray()));
                    }

                    
                }
            }
            else if (iPrint is TJPaitent)//打印体检回执
            {
                string reutrnTemplate = ConfigHelper.GetSysConfigValueWithoutLogin("TJBarcodeReturnReport");
                machine.PrintInfo = new PrintInfo(printIDForReturn);
                result = machine.Print(printMachineName, reutrnTemplate, BarcodeTable.Patient.BarcodeDisplayNumber, string.Join("+", combinesName.ToArray()));
            }
            return result;

        }

        private string GetComKey(EntitySampMain row, List<EntityDicCombine> cachedata)
        {
            string Key = "";
            List<EntitySampDetail>  lst = new ProxySampDetail().Service.GetSampDetail(row.SampBarCode);
            foreach (EntitySampDetail detail in lst)
            {
                var en = cachedata.FindAll(w => w.ComId == detail.ComId);
                if (en.Count() == 0)
                    return "";
                if (string.IsNullOrEmpty(Key))
                {
                    Key = en[0].ComGetReportTime;//取报告时间
                }
                else
                {
                    if (Key != en[0].ComGetReportTime)
                        return "";
                }
            }
            return Key;
        }

        /// <summary>
        /// 调用打印机打印条码回执
        /// </summary>
        /// <param name="iPrint"></param>
        /// <param name="dataToPrint"></param>
        /// <returns></returns>
        private bool PrintBarcodeReturnWithDefaultPrinter(List<EntitySampMain> listPrintSamp)
        {
            bool result = false;

            List<String> listBarCode = new List<string>();

            foreach (EntitySampMain row in listPrintSamp)
            {
                listBarCode.Add(row.SampBarCode);
            }

            EntityDCLPrintParameter printPara = new EntityDCLPrintParameter();
            printPara.ReportCode = ConfigHelper.GetSysConfigValueWithoutLogin("BarcodeSearchReturnReport");
            printPara.ListBarId = listBarCode;

            DCLReportPrint.Print(printPara);

            return result;

        }

        /// <summary>
        /// 获取当前界面中所有的条码
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllBarcodesByMZ()
        {
            List<string> ret = new List<string>();

            if (this.ListSampMain != null)
            {
                foreach (EntitySampMain row in ListSampMain)
                {
                    if (row.PidSrcId == "107")
                    {
                        ret.Add(row.SampBarId);
                    }

                }
            }
            return ret;
        }

        /// <summary>
        /// 重新打印的条码只打印一次
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private bool BarcodePrintOnce(EntitySampMain row)
        {
            return row.SampPrintTime == 0 || row.SampPrintTime == 1
                || stepType == StepType.Select || row.SampPrintFlag == 1; //重新打印的条码只打印一次
        }

        /// <summary>
        /// 输出到条码打印机打印，自动判断来源
        /// </summary>
        /// <returns></returns>
        private bool PrintBarcodeWithMachineAuto(List<EntitySampMain> listPrint)
        {
            string printMachineName = GetPrintMachineName();
            if (string.IsNullOrEmpty(printMachineName))
            {
                throw new BarcodePrinterNotFoundException();

            }
            IPrintMachine machine = new ZeBraPrinter();

            IList<string> idList = new List<string>();

            if (ConfigHelper.GetSysConfigValueWithoutLogin("BarcodeCheckSelect") == "是" && listPrint.Count > 0)
            {
                foreach (EntitySampMain row in listPrint)
                {
                    idList.Add(row.SampSn.ToString());
                }
            }
            else
                idList.Add(BaseSampMain.SampSn.ToString());
            machine.PrintInfo = new PrintInfo(idList);


            string template = "";
            if (HasNotSelect())
                return false;
            template = GetReportTemplate(CurrentSampMain.PidSrcId);

            //系统配置：[条码查询]使用院感条码报表的物理组IDs
            string cf_BCSearch_UseYGBarcodeReportCtypeIDs = ConfigHelper.GetSysConfigValueWithoutLogin("BCSearch_UseYGBarcodeReportCtypeIDs");
            if (CurrentSampMain.SampType.Length > 0
                && !string.IsNullOrEmpty(cf_BCSearch_UseYGBarcodeReportCtypeIDs)
                && cf_BCSearch_UseYGBarcodeReportCtypeIDs.Contains(CurrentSampMain.SampType)
                && !string.IsNullOrEmpty(ConfigHelper.GetSysConfigValueWithoutLogin("YGBarcodeReport")))
            {
                //院感条码
                template = ConfigHelper.GetSysConfigValueWithoutLogin("YGBarcodeReport");
            }

            bool result = false;
            try
            {
                result = machine.Print(printMachineName, template);
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("打印出错:" + ex.Message);
                Logger.WriteException("条码", "条码调用打印器打印", ex.Message + "\r\n" + ex.StackTrace);
                return false;
            }


            return result;
        }

        private string GetReportTemplate(IPrint printer)
        {
            string template = "";
            //叫号的操作状态是采集
            if (this.StepType != StepType.Print && this.StepType != StepType.Sampling)
                return "";

            if (printer is Inpatient)
                template = ConfigHelper.GetSysConfigValueWithoutLogin("ZYBarcodeReport");

            else if (printer is OutPaitent)
                template = ConfigHelper.GetSysConfigValueWithoutLogin("MZBarcodeReport");

            else if (printer is TJPaitent)
                template = ConfigHelper.GetSysConfigValueWithoutLogin("TJBarcodeReport");

            else if (printer is Manual)
            {
                if (!IsYG)
                    template = ConfigHelper.GetSysConfigValueWithoutLogin("ManualBarcodeReport");
                else
                    template = ConfigHelper.GetSysConfigValueWithoutLogin("YGBarcodeReport");
            }

            return template;

        }

        private string GetReportPreTemplate(IPrint printer)
        {
            string template = "";
            if (this.StepType != StepType.Print && this.StepType != StepType.Sampling)
                return "";

            if (printer is Inpatient)
                template = ConfigHelper.GetSysConfigValueWithoutLogin("ZYPreBarcodeReport");

            else if (printer is OutPaitent)
                template = ConfigHelper.GetSysConfigValueWithoutLogin("MZPreBarcodeReport");

            else if (printer is TJPaitent)
                template = ConfigHelper.GetSysConfigValueWithoutLogin("TJPreBarcodeReport");

            else if (printer is Manual)
            {
                if (!IsYG)
                    template = ConfigHelper.GetSysConfigValueWithoutLogin("ManualBarcodeReport");
                else
                    template = ConfigHelper.GetSysConfigValueWithoutLogin("YGBarcodeReport");
            }

            return template;

        }

        private string GetReportTemplate(string oriId)
        {
            string template = "";
            if (oriId == "107")
                template = ConfigHelper.GetSysConfigValueWithoutLogin("MZBarcodeReport");

            else if (oriId == "108")
                template = ConfigHelper.GetSysConfigValueWithoutLogin("ZYBarcodeReport");

            else
                template = ConfigHelper.GetSysConfigValueWithoutLogin("ZYBarcodeReport");
            //else if (printer is TJPaitent)
            //    template = ConfigHelper.GetSysConfigValueWithoutLogin("TJBarcodeReport");

            return template;

        }


        /// <summary>
        /// 跳到下一条
        /// </summary>
        public void MoveNext()
        {
            if (this.HasData() && ListSampMain.Count > 1)
                gvBarcode.MoveNext();
        }


        /// <summary>
        /// 跳到下一条
        /// </summary>
        public void MoveNext(int focusedRowHandle)
        {
            //光标移动到未打印条码
            if (gvBarcode.FocusedRowHandle != gvBarcode.DataRowCount - 1)
            {
                for (int i = focusedRowHandle + 1; i < gvBarcode.RowCount; i++)
                {
                    EntitySampMain dr = (EntitySampMain)this.gvBarcode.GetRow(i);

                    if (dr.SampPrintFlag == 1)
                    { }
                    else
                    {
                        gvBarcode.FocusedRowHandle = i;
                        break;
                    }
                }
            }
            else
            {
                gvBarcode.FocusedRowHandle = focusedRowHandle;
                gvBarcode.RefreshData();
            }
        }

        public void MovePreBarcode()
        {
            for (int i = 0; i < gvBarcode.RowCount; i++)
            {
                EntitySampMain dr = (EntitySampMain)this.gvBarcode.GetRow(i);

                if (dr.SampBarCode != string.Empty)
                { }
                else
                {
                    gvBarcode.FocusedRowHandle = i;
                    break;
                }
            }
        }


        public void MoveLast()
        {
            gvBarcode.MoveLast();
        }


        /// <summary>
        /// 清除当前条码
        /// </summary>    
        internal bool ClearSelected()
        {
            if (HasNotSelect())
                return false;

            EntitySampMain baseInfo = GetBaseInfo();
            if (baseInfo == null)
                return false;

            if (lis.client.control.MessageDialog.Show(string.Format("是否清除 [ {0} {1} ] 此条码信息?", baseInfo.SampBarId, baseInfo.PidName), "提示", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return false;

            ListSampMain.Remove(CurrentSampMain);

            gcBarcode.RefreshDataSource();
            return true;
        }

        private BindingSource PatientBindingSource
        {
            get { return bsPatient; }
            set
            {
                this.bsPatient = value;
            }
        }

        /// <summary>
        /// 更新条码统计数
        /// </summary>
        public void UpdateCount()
        {
            try
            {
                if (ListSampMain != null && ListSampMain.Count > 0)
                    lblPrintCount.Text = gvBarcode.RowCount.ToString();

                if (gvBarcode.RowCount == 0)
                    lblPrintCount.Text = "0";
            }
            catch
            { }
        }

        /// <summary>
        /// 有条码
        /// </summary>
        internal bool HasData()
        {
            return MainTable != null && MainTable.Count > 0;
        }

        /// <summary>
        /// 没选择条码
        /// </summary>
        internal bool HasNotSelect()
        {
            return MainGridView.FocusedRowHandle < 0;
        }

        /// <summary>
        /// 默认焦点行打上勾
        /// </summary>
        internal void DefaultSelectFocusedRow()
        {
            if (MainTable != null && MainTable.Count > 0)
            {
                Selection.SelectRow(MainGridView.FocusedRowHandle, true);
            }
        }

        private bool IsClearAll = false;

        /// <summary>
        /// 确认全部条码
        /// </summary>
        /// <param name="loginID">登陆ID</param>
        /// <param name="userName">用户姓名</param>
        internal bool ComfirmAll(string loginID, string userName, bool isClearAll, string workId)
        {
            if (HasData())
            {
                IsClearAll = isClearAll;

                List<EntitySampMain> barcodeNumbers = GetAllUsedBarcodesInfo();

                #region 送达与签收步骤取本地配置的"物理组"  modified by lin : 2010/05/31
                EntitySampOperation sign = new EntitySampOperation(loginID, userName);

                if (Step is ReceiveStep || Step is ReachStep)
                {
                    sign.OperationPlace = LocalSetting.Current.Setting.CType_name;
                }
                else if (Step is PrintStep || Step is SamplingStep || Step is SendStep)
                {
                    sign.OperationPlace = LocalSetting.Current.Setting.DeptName;
                }

                if (Step is ReceiveStep || Step is ReachStep || Step is SendStep)
                {
                    sign.Remark = string.Format("IP地址:{0}", IPUtility.GetIP());
                }
                //条码所有确认流程记录本地配置[描述]设置的地点
                if (Barcode_RecordSignPlace)
                {
                    sign.OperationPlace = LocalSetting.Current.Setting.Description;
                }
                sign.OperationTime = IStep.GetServerTime();
                sign.OperationWorkId = workId;
                List<EntitySysUser> listUser = CacheClient.GetCache<EntitySysUser>();
                int index = listUser.FindIndex(w => w.UserLoginid == workId);
                if (index > -1)
                {
                    sign.OperationUserInCode = listUser[index].UserIncode;
                }

                #endregion

                bool result = Step.ComfirmAll(sign, barcodeNumbers);

                if (result)
                {
                    foreach (EntitySampMain item in barcodeNumbers)
                    {
                        item.SampStatusId = Step.StepCode;
                        item.SampLastactionDate = sign.OperationTime;
                    }

                    if (isClearAll)
                    {
                        ClearAll();
                    }
                }

                return result;
            }
            return false;
        }

        public int GetAllUsedBarcodesCount()
        {
            int count = 0;
            foreach (EntitySampMain row in ListSampMain)
            {
                if (IsNotEmpty(row.SampBarId) && row.SampStatusId != Step.StepCode)
                    count++;
            }

            return count;
        }

        private List<EntitySampMain> GetAllUsedBarcodesInfo()
        {
            List<EntitySampMain> result = new List<EntitySampMain>();

            if (IsClearAll && ShouldMultiSelect && stepType != StepType.Select && stepType != StepType.Print)
            {
                List<EntitySampMain> lisSamp = Selection.GetAllSelectSamp();
                if (lisSamp.Count <= 0)
                {
                    Selection.SelectAll();
                    lisSamp = Selection.GetAllSelectSamp();
                }

                foreach (EntitySampMain sam in lisSamp)
                {
                    if (Convert.ToInt32(sam.SampStatusId) < Convert.ToInt32(Step.StepCode) ||
                        sam.SampStatusId == "9" ||//条码回退
                        sam.SampStatusId == "8")//二次送检
                    {
                        if (IsNotEmpty(sam.SampBarCode))
                            result.Add(sam);
                    }
                }
            }
            else
            {
                foreach (EntitySampMain sam in ListSampMain)
                {
                    if (Convert.ToInt32(sam.SampStatusId) < Convert.ToInt32(Step.StepCode) ||
                        sam.SampStatusId == "9" ||//条码回退
                        sam.SampStatusId == "8")//二次送检
                    {
                        if (IsNotEmpty(sam.SampBarCode))
                            result.Add(sam);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 重置
        /// </summary>
        internal void Reset()
        {
            ClearAll();
            //barcodeStates1.Reset();
        }

        /// <summary>
        /// 刷新当前条码
        /// </summary>
        public void RefreshCurrentBarcode()
        {
            RefreshFocusRow();
            RefreshCurrentBarcodeInfo();
        }

        /// <summary>
        /// 更新当前行的状态
        /// </summary>
        private void RefreshFocusRow()
        {
            if (HasNotSelect() || bsPatient.Current == null)
                return;

            EntitySampMain row = (EntitySampMain)bsPatient.Current;

            EntitySampMain sampMain = proxy.Service.SampMainQueryByBarId(row.SampBarId);

            if (!string.IsNullOrEmpty(sampMain.SampBarId))
            {
                row = (EntitySampMain)sampMain.Clone();
            }
        }

        /// <summary>
        /// 刷新当前病人资料
        /// </summary>
        public void RefreshCurrentPatient()
        {
            if (HasNotSelect())
                return;

            if (CurrentSampMain != null)
            {
                List<EntitySampMain> list = new List<EntitySampMain>();
                list.Add(CurrentSampMain);

                RefreshRow(list);
            }
            RefreshCurrentBarcodeInfo();
        }

        private void RefreshRow(List<EntitySampMain> rows)
        {
            if (rows == null || rows.Count == 0)
                return;

            List<string> listBarId = new List<string>();

            foreach (EntitySampMain row in rows)
            {
                listBarId.Add(row.SampBarCode);
            }

            EntitySampQC sampQc = new EntitySampQC();
            sampQc.ListSampBarId = listBarId;
            List<EntitySampMain> list = proxy.Service.SampMainQuery(sampQc);

            if (list.Count > 0)
            {
                int index = ListSampMain.FindIndex(w => w.SampSn == list[0].SampSn);

                ListSampMain[index] = list[0];
            }
        }

        /// <summary>
        /// 打印清单
        /// </summary>
        internal void PrintList()
        {
            try
            {
                if (!HasData())
                {
                    ShowAndClose("没打印数据!");
                    return;
                }

                if (gcBarcode.IsPrintingAvailable)
                {
                    List<string> listBarCode = new List<string>();

                    if (Selection == null)
                    {
                        foreach (EntitySampMain drSour in ListSampMain)
                        {
                            listBarCode.Add(drSour.SampBarId);
                        }

                    }
                    else
                    {
                        List<EntitySampMain> dtSour = Selection.GetAllSelectSamp();
                        foreach (EntitySampMain drSour in dtSour)
                        {
                            listBarCode.Add(drSour.SampBarId);
                        }
                    }
                    if (listBarCode.Count > 0)
                    {
                        EntityDCLPrintParameter printPara = new EntityDCLPrintParameter();
                        printPara.ListBarId = listBarCode;
                        printPara.ReportCode = "printBarCodeQD";
                        DCLReportPrint.Print(printPara);
                    }
                    else
                    {
                        ShowAndClose("没打印数据!");
                        return;
                    }
                }
                else
                    ShowAndClose("打印出错!请检查打印机是否安装正确.");
            }
            catch (Exception ex)
            {
                MessageDialog.ShowAutoCloseDialog(ex.Message, 3);
            }

        }

        /// <summary>
        /// 打印细菌清单
        /// </summary>
        internal void PrintListGerm()
        {
            try
            {
                if (!HasData())
                {
                    ShowAndClose("没打印数据!");
                    return;
                }

                if (gcBarcode.IsPrintingAvailable)
                {
                    List<string> listBarCode = new List<string>();

                    if (Selection == null)
                    {
                        foreach (EntitySampMain drSour in ListSampMain)
                        {
                            listBarCode.Add(drSour.SampBarId);
                        }

                    }
                    else
                    {
                        List<EntitySampMain> dtSour = Selection.GetAllSelectSamp();
                        foreach (EntitySampMain drSour in dtSour)
                        {
                            listBarCode.Add(drSour.SampBarId);
                        }
                    }

                    if (listBarCode.Count > 0)
                    {
                        EntityDCLPrintParameter printPara = new EntityDCLPrintParameter();
                        printPara.ListBarId = listBarCode;
                        printPara.ReportCode = "printBarCodeQDGerm";
                        DCLReportPrint.Print(printPara);
                    }
                    else
                    {
                        ShowAndClose("没打印数据!");
                        return;
                    }
                }
                else
                    ShowAndClose("打印出错!请检查打印机是否安装正确.");
            }
            catch (Exception ex)
            {
                MessageDialog.ShowAutoCloseDialog(ex.Message, 3);
            }

        }


        /// <summary>
        /// 撤消项目明细
        /// </summary>
        /// <param name="DoctorCode"></param>
        /// <param name="DoctorName"></param>
        internal bool DeleteCname(string DoctorCode, string DoctorName)
        {
            if (ListSampDetail.Count <= 0)
            {
                ShowAndClose("没有明细信息");
                return false;
            }
            if (ListSampDetail.Count == 1)
            {
                ShowAndClose("该条码只有一条明细，请直接执行删除条码操作");
                return false;
            }

            ProxySampProcessDetail proxy = new ProxySampProcessDetail();
            EntitySampProcessDetail process = proxy.Service.GetLastSampProcessDetail(BaseSampMain.SampBarId);

            if (BaseSampMain.SampStatusId == "1")
            {
                ShowAndClose("病人" + BaseSampMain.PidName + "条码已打印,不能删除!");
                return false;
            }
            if (process == null || string.IsNullOrEmpty(process.ProcBarno))
                ShowAndClose("无此条形码信息!");

            string deleteConfig = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_CanDeleteBarcode");

            if (deleteConfig == "下载后"
                && process.ProcStatus != EnumBarcodeOperationCode.SampleReturn.ToString()
                )
            {
                ShowAndClose("病人" + BaseSampMain.PidName + "条码已生成,不能删除!");
                return false;
            }
            else if (deleteConfig == "打印后" && (Convert.ToInt32(process.ProcStatus) >= 1)
                        && process.ProcStatus != EnumBarcodeOperationCode.SampleReturn.ToString())
            {
                ShowAndClose("病人" + BaseSampMain.PidName + "条码已打印,不能删除!");
                return false;
            }
            else if ((deleteConfig == "采集后" &&
                        (Convert.ToInt32(process.ProcStatus) >= 2))
                        && process.ProcStatus != EnumBarcodeOperationCode.SampleReturn.ToString()
                )
            {
                ShowAndClose("病人" + BaseSampMain.PidName + "条码已采集,不能删除!");
                return false;
            }
            else if (
                    (deleteConfig == "签收后" && (Convert.ToInt32(process.ProcStatus) >= 5))
                    && process.ProcStatus != EnumBarcodeOperationCode.SampleReturn.ToString()
                    )
            {
                ShowAndClose("病人" + BaseSampMain.PidName + "条码检验科已经接收,不能删除!");
                return false;
            }

            if (ListSampDetail.Count == 1) //如果只有一条明细，则提示删除条码
            {
                return DeleteBarcode(DoctorCode, DoctorName);
            }

            EntitySampDetail row = (EntitySampDetail)gvCname.GetFocusedRow();
            string combineHisName = row.ComName;


            if (lis.client.control.MessageDialog.Show(string.Format("是否删除 [ {0} {1} ] 此明细信息?", row.SampBarCode, combineHisName), "提示", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return false;
            }

            List<EntitySampDetail> listDeleteSampDetail = new List<EntitySampDetail>();
            listDeleteSampDetail.Add(row);

            ProxySampDetail proxyDetail = new ProxySampDetail();
            bool result = proxyDetail.Service.DeleteSampDetail(listDeleteSampDetail);

            if (result)
            {
                RefreshCurrentBarcode();
                //删除条码明细后 更新界面上的组合名称
               List<EntitySampDetail> listSampDetail = new ProxySampDetail().Service.GetSampDetail(BaseSampMain.SampBarId);
                if (listSampDetail.Count > 0)
                {
                    EntitySampMain samp = (EntitySampMain)bsPatient.Current;
                    string comName = string.Empty;
                    foreach (EntitySampDetail detail in listSampDetail)
                    {
                        comName += string.Format("+{0}", detail.ComName);
                    }
                    if (comName.Length > 0)
                    {
                        comName = comName.Remove(0, 1);
                        samp.SampComName = comName;
                        int handle = gvBarcode.FocusedRowHandle;
                        gvBarcode.RefreshRow(handle);
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// 删除选中单个条码
        /// </summary>
        /// <param name="DoctorCode"></param>
        /// <param name="DoctorName"></param>
        internal bool DeleteBarcode(string DoctorCode, string DoctorName)
        {
            return DeleteSampMain(DoctorCode, DoctorName, true);
        }

        /// <summary>
        /// 批量删除条码
        /// </summary>
        internal bool DeleteBarcodeForAll(string DoctorId, string DoctorName)
        {
            return DeleteSampMain(DoctorId, DoctorName, false);
        }

        /// <summary>
        /// 删除条码方法
        /// </summary>
        /// <param name="OperationID"></param>
        /// <param name="OperationName"></param>
        /// <param name="IsSingle"></param>
        /// <returns></returns>
        private bool DeleteSampMain(string OperationID, string OperationName, bool IsSingle)
        {
            if (ListSampMain == null || ListSampMain.Count == 0)
            {
                ShowAndClose("没有条码数据");
                return false;
            }

            List<EntitySampMain> listDeleteSampMain = new List<EntitySampMain>();
            bool needReset = (ConfigHelper.GetSysConfigValueWithoutLogin("PreBarCodeDel_NeedReset") == "是");
            if (IsSingle)
            {
                if (bsPatient.Current != null)
                {
                    EntitySampMain sampMain = (EntitySampMain)bsPatient.Current;
                    listDeleteSampMain.Add(sampMain);
                }
            }
            else
            {
                listDeleteSampMain = Selection.GetAllSelectSamp();
            }
            string msg = string.Empty;
            foreach (EntitySampMain sampMain in listDeleteSampMain)
            {
                if (needReset && sampMain.SampBarType == 1 && !string.IsNullOrEmpty(sampMain.SampBarCode))
                {
                    msg += string.Format("条码号[{0}] \r\n", sampMain.SampBarCode);
                }
                if (!string.IsNullOrEmpty(msg))
                {
                    ShowMessage("以下预置条码未重置,不能删除!" + "\r\n" + msg);
                    return false;
                }
            }
            if (listDeleteSampMain.Count == 0)
            {
                ShowAndClose("请选择条码数据");
                return false;
            }

            if (MessageDialog.Show("是否删除条码信息?", "提示", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return false;

            //广医二院：如果HIS那边传入了病人工号和名字，删除、采集的时候不需要弹出验证。直接过
            bool skipPower = IsAlone && !string.IsNullOrEmpty(OperationID) && !string.IsNullOrEmpty(OperationName);

            FrmHISCheckPassword frm = new FrmHISCheckPassword(Step.Audit);
            if (!(ConfigHelper.GetSysConfigValueWithoutLogin("DeleteBCAfterConfirm") == "否") && !skipPower)
            {
                if (frm.ShowDialog() != DialogResult.OK)//身份验证
                    return false;
            }

            if (skipPower)
            {
                frm.OperatorID = OperationID;
                frm.OperatorName = OperationName;
            }
            string deleteConfig = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_CanDeleteBarcode");

            //启用删除已采集的条码时自动弹出回退窗口功能
            bool needReturnFlag = ConfigHelper.GetSysConfigValueWithoutLogin("DeleteCollectBarCodeNeedReturn") == "是";

            int i = 0;
            foreach (EntitySampMain rowView in listDeleteSampMain)
            {
                EntitySampQC samp = new EntitySampQC();
                samp.ListSampBarId.Add(rowView.SampBarId);

                List<EntitySampMain> listBarcode = proxy.Service.SampMainQuery(samp);

                if (listBarcode == null || listBarcode.Count == 0)
                {
                    ShowAndClose("无此条形码信息!");
                    continue;
                }

                EntitySampMain rowBarcode = listBarcode[0];

                if (needReturnFlag && rowBarcode.SampStatusId == EnumBarcodeOperationCode.SampleCollect.ToString())
                {
                    FrmReturnBarcodeV2 returnBarcodeV2 = new FrmReturnBarcodeV2(rowBarcode.SampBarId);
                    returnBarcodeV2.ShowDialog();

                    rowBarcode = proxy.Service.SampMainQueryByBarId(rowView.SampBarId);
                }


                if (deleteConfig == "下载后"
                    && rowBarcode.SampStatusId != EnumBarcodeOperationCode.SampleReturn.ToString()
                    )
                {
                    ShowAndClose("病人" + rowBarcode.PidName + "条码已生成,不能删除!");
                    continue;
                }
                else if (
                        (deleteConfig == "采集后" &&
                        (rowBarcode.CollectionFlag == 1
                        || rowBarcode.SendFlag == 1
                        || rowBarcode.ReachFlag == 1
                        || rowBarcode.ReceiverFlag == 1
                        ))
                    && rowBarcode.SampStatusId != EnumBarcodeOperationCode.SampleReturn.ToString()
                    )
                {
                    ShowAndClose("病人" + rowBarcode.PidName + "条码已采集,不能删除!");
                    continue;
                }
                else if ((deleteConfig == "签收后" &&
                            rowBarcode.ReachFlag == 1)
                    && rowBarcode.SampStatusId != EnumBarcodeOperationCode.SampleReturn.ToString()
                        )
                {
                    ShowAndClose("病人" + rowBarcode.PidName + "条码检验科已经接收,不能删除!");
                    continue;
                }

                bool printFunction = true;
                if (!alone)
                {
                    UserInfo.SkipPower = false;
                    printFunction = UserInfo.HaveFunction("FrmBCPrint_TJ_DeleteBarcode", "FrmBCPrint_TJ_DeleteBarcode");
                }
                if (Barcode_EnableDeleteTJBarcodeJudge)
                {
                    if (rowBarcode.PidSrcId == "109" && !printFunction)
                    {
                        ShowAndClose("无此条码[" + rowBarcode.SampBarId + "]删除权限,不能删除!");
                        continue;
                    }
                }

                EntitySampOperation oper = new EntitySampOperation();
                oper.OperationID = frm.OperatorID;
                oper.OperationName = frm.OperatorName;
                oper.OperationStatus = "510";
                oper.OperationStatusName = "删除条码";
                oper.OperationTime = ServerDateTime.GetServerDateTime();
                oper.Remark = string.Format("删除条码：" + rowBarcode.SampBarId);
                oper.OperationPlace = UserInfo.ip;
                oper.OperationWorkId = frm.OperatorSftId;
                proxy.Service.DeleteSampMain(oper, rowBarcode);

                i++;
                ListSampMain.Remove(rowView);

                if (rowView.PidSrcId == "107")
                {
                    string strStepProc = ConfigHelper.GetSysConfigValueWithoutLogin("BC_MZAdivceCancelConfirmType");

                    //outlink接口只能客户端调用。其他接口统一由服务端调用
                    if (strStepProc == "outlink")
                    {
                        Outlink outlink = new Outlink();


                        new ProxySampDetail().Service.GetSampDetail(rowView.SampBarId);
                        //查找此条码的医嘱信息(bc_cname)
                        IList<EntitySampDetail> list = new ProxySampDetail().Service.GetSampDetail(rowView.SampBarId);

                        foreach (EntitySampDetail entity in list)
                        {
                            try
                            {
                                if (string.IsNullOrEmpty(entity.OrderSn))
                                    continue;
                                //调用outlink进行取消费用
                                string TWCId = string.Empty;
                                string Rpid = string.Empty;
                                string SeqId = "0";
                                if (entity.OrderSn.Split('|').Length >= 3)
                                {
                                    TWCId = entity.OrderSn.Split('|')[0];
                                    Rpid = entity.OrderSn.Split('|')[1];
                                    SeqId = entity.OrderSn.Split('|')[2];
                                }
                                if (entity.OrderSn.Split('|').Length == 2)
                                {
                                    TWCId = entity.OrderSn.Split('|')[0];
                                    Rpid = entity.OrderSn.Split('|')[1];
                                }
                                if (entity.OrderSn.Split('|').Length == 1)
                                {
                                    TWCId = entity.OrderSn.Split('|')[0];
                                }
                                string strResult = outlink.CancelLisMzOrder(TWCId, Rpid, SeqId, frm.OperatorID);
                                if (strResult != null && strResult.Trim() != string.Empty)
                                {
                                    Lib.LogManager.Logger.LogException(new Exception(entity.OrderSn + ":" + strResult));
                                }
                            }
                            catch (Exception ex)
                            {
                                Lib.LogManager.Logger.LogException(ex);
                            }
                        }
                    }
                }

            }

            int focusedRowIndex = i;

            Selection.ClearSelection();

            if (!HasData())
                ClearAll();

            //RefreshCurrentBarcode();

            gcBarcode.RefreshDataSource();

            return i > 0;


        }

        /// <summary>BaseInfo.SampPrintFlag
        /// 拆分条码
        /// </summary>
        internal bool SeparateBarcode(bool isPreplaceBarcode)
        {
            if (!HasData())
            {
                ShowAndClose("没有条码数据");
                return false;
            }

            if (ListSampDetail.Count <= 1)
            {
                ShowAndClose("条码项目数低于2条,无法拆分", 1.5m);
                return false;
            }

            if (BaseSampMain.SampPrintFlag == 1 && BaseSampMain.SampStatusId != "9")//回退可以拆分
            {
                ShowAndClose("条码已经打印,不能拆分!", 1.5m);
                return false;
            }

            if (BaseSampMain.ReceiverFlag == 1)
            {
                ShowAndClose("检验科已经接收,不能拆分!", 1.5m);
                return false;
            }

            List<EntitySampDetail> list = new List<EntitySampDetail>();
            if (gvCname.FocusedRowHandle >= 0)
            {
                int[] rowHandle = gvCname.GetSelectedRows();

                if (rowHandle.Count() == 0 ||
                    rowHandle.Count() == ListSampDetail.Count)
                {
                    ShowAndClose("请选择需要拆分的项目!", 1.5m);
                    return false;
                }

                foreach (int i in rowHandle)
                {
                    list.Add((EntitySampDetail)gvCname.GetRow(i));
                }
            }
            else
            {
                ShowAndClose("请选择需要拆分的项目!", 1.5m);
                return false;
            }

            string itemLisNameArry = string.Empty;
            for (int i = 0; i < list.Count; i++)
            {
                EntitySampDetail itemInfo = list[i];
                itemInfo.SampBarId = CurrentSampMain.SampSn.ToString();
                if (i == list.Count - 1)
                {
                    itemLisNameArry += (itemInfo.ComName);
                }
                else
                {
                    itemLisNameArry += (itemInfo.ComName + ",");
                }
            }

            if (lis.client.control.MessageDialog.Show(string.Format("是否拆分 [ {0} {1}，项目：{2} ] 此条码信息?", BaseSampMain.SampBarCode, BaseSampMain.PidName, itemLisNameArry), "提示", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return false;

            BaseSampMain.ListSampDetail = ListSampDetail;
            BaseSampMain.ListSampProcessDetail = ListSampProcess;

            List<EntitySampMain> listResult = proxy.Service.SeperateSampMain(BaseSampMain, list);

            bool result = listResult != null && listResult.Count > 0;

            int handle = gvBarcode.FocusedRowHandle;

            if (result)
            {

                ListSampMain.Remove(BaseSampMain);

                foreach (EntitySampMain item in listResult)
                {
                    item.CheckMarkSelection = false;
                }

                ListSampMain.AddRange(listResult);

                gcBarcode.RefreshDataSource();

                gvBarcode.FocusedRowHandle = handle;
                RefreshCurrentBarcode();
                ShowWarn(result);
            }

            return result;
        }


        internal void MergeSampMain()
        {
            if (!HasData())
            {
                ShowAndClose("没有条码数据");
                return;
            }

            List<EntitySampMain> listSampMain = Selection.GetAllSelectSamp();
            List<EntitySampMain> listSampTemp = EntityManager<EntitySampMain>.ListClone(listSampMain);
            if (listSampMain.Count <= 1)
            {
                ShowAndClose("条码数低于2条,无法合并", 1.5m);
                return;
            }

            string strPidInNo = listSampMain[0].PidInNo;
            List<string> listSampBarId = new List<string>();
            string strCNameNew = string.Empty;
            foreach (EntitySampMain sampMain in listSampMain)
            {
                if (sampMain.SampPrintFlag == 1 && sampMain.SampStatusId != "9")//回退可以拆分
                {
                    ShowAndClose("条码已经打印,不能合并!", 1.5m);
                    return;
                }

                if (sampMain.ReceiverFlag == 1)
                {
                    ShowAndClose("检验科已经接收,不能合并!", 1.5m);
                    return;
                }

                if (sampMain.PidInNo != strPidInNo)
                {
                    ShowAndClose("不同人的标本,不能合并!", 1.5m);
                    return;
                }
                strCNameNew += "+" + sampMain.SampComName;
                listSampBarId.Add(sampMain.SampBarId);
            }
            strCNameNew = strCNameNew.Remove(0, 1);
            if (proxy.Service.MergeSampMain(listSampBarId))
            {
                for (int i = 1; i < listSampMain.Count; i++)
                {
                    ListSampMain.Remove(listSampMain[i]);
                    listSampTemp.Remove(listSampMain[i]);
                }
                foreach (EntitySampMain samp in ListSampMain)
                {
                    if (samp.SampBarId == listSampTemp[0].SampBarId)
                    {
                        samp.SampComName = strCNameNew;
                    }
                }
                Selection.ClearSelection();
                PatientBindingSource.DataSource = ListSampMain;
                gcBarcode.RefreshDataSource();

            }
            else
                ShowWarn(false);

        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="result"></param>
        private void ShowWarn(bool result)
        {
            string word = result ? "操作成功" : "操作失败";
            lis.client.control.MessageDialog.Show(word);
        }

        public bool blReset = false;

        /// <summary>
        /// 清空全部
        /// </summary>
        internal void ClearAll()
        {
            ClearPatients();
            ClearBaseInfo();
            ClearCombines();
            ClearSign();
            OnClearAll(this, EventArgs.Empty);


        }

        private void ClearSign()
        {
            gcAction.DataSource = new List<EntitySampProcessDetail>();
            if (ListSampProcess != null)
                ListSampProcess.Clear();
        }

        private void ClearPatients()
        {
            bsPatient.DataSource = new List<EntitySampMain>();
            this.ListSampMain.Clear();
        }

        private void ClearCombines()
        {
            bsCName.DataSource = new List<EntitySampDetail>();
            ListSampDetail.Clear();
        }

        /// <summary>
        /// 更改条码的标本类别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectDict_Sample1_onAfterSelected(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectDict_Sample1.valueMember) && string.IsNullOrEmpty(selectDict_Sample1.displayMember)) //标本为空不提示          
                return;
            List<EntitySampMain> barcode = new List<EntitySampMain>();
            string opID = UserInfo.loginID;
            string opName = UserInfo.userName;
            if (Barcode_SignEnableBitchModifySamp && stepType == StepType.Confirm)
            {
                if (!HasData())
                {
                    ShowAndClose("没有条码数据");
                    return;
                }

                barcode = Selection.GetAllSelectSamp();

                if (barcode.Count <= 0)
                {
                    ShowAndClose("请勾选条码数据");
                    return;
                }
            }
            else
            {
                if (bsPatient.Current == null)
                {
                    ShowAndClose("请选择要修改的数据！");
                    return;
                }

                barcode.Add(GetBaseInfo());
            }

            if (!Barcode_SignEnableBitchModifySamp && BaseSampMain.SampStatusId != "0" && BaseSampMain.SampStatusId != "1" && BaseSampMain.SampStatusId != "2" && BaseSampMain.SampStatusId != "9")
            {
                selectDict_Sample1.valueMember = BaseSampMain.SampSamId;
                selectDict_Sample1.SelectByID(BaseSampMain.SampSamId);
                ShowAndClose("已做采集确认的病人资料信息不能再修改");
                return;
            }

            DialogResult verification = new DialogResult();

            if (Step.StepName == "条码查询")
            {
                FrmCheckPassword frmCheck = new FrmCheckPassword();
                verification = frmCheck.ShowDialog();
                opID = frmCheck.OperatorID;
                opName = frmCheck.OperatorName;
            }
            else
            {
                if (Barcode_SignEnableBitchModifySamp && stepType == StepType.Confirm)
                {
                    FrmCheckPassword frmCheck = new FrmCheckPassword("FrmBarcode_EnableBitchModifySamp");
                    verification = frmCheck.ShowDialog();
                    opID = frmCheck.OperatorID;
                    opName = frmCheck.OperatorName;
                }
                else
                {
                    if (Step.Printer is Inpatient 
                        || Step.Printer is OutPaitent 
                        || Step.Printer is TJPaitent 
                        || Step.Printer is Manual 
                        || ConfigHelper.GetSysConfigValueWithoutLogin("MZpatientBarcodeChangeConfirm") == "否")//住院条码、手工条码直接过、沙井不需要用户验证
                        verification = DialogResult.OK;
                    else
                    {
                        FrmHISCheckPassword checkPassword = new FrmHISCheckPassword(Step.Printer.Audit);
                        verification = checkPassword.ShowDialog();
                        opID = checkPassword.OperatorID;
                        opName = checkPassword.OperatorName;
                    }
                }

            }

            if (verification == DialogResult.OK)
            {
                EntitySampOperation operation = new EntitySampOperation();
                operation.OperationStatus = "170";
                operation.OperationStatusName = "标本修改";
                operation.OperationID = opID;
                operation.OperationName = opName;
                operation.OperationIP = UserInfo.ip;
                operation.OperationTime = ServerDateTime.GetServerDateTime();

                bool result = proxy.Service.UpdateSampMainSampleInfo(selectDict_Sample1.valueMember, selectDict_Sample1.displayMember, barcode, operation);

                if (result)
                {
                    CurrentSampMain.SampSamId = selectDict_Sample1.valueMember;
                    CurrentSampMain.SampSamName = selectDict_Sample1.displayMember;
                    ShowAndClose("修改成功!");
                }
                else
                    ShowAndClose("修改失败!");
            }
            else
            {
                ShowAndClose("验证权限失败!");
            }
            if (Barcode_SignEnableBitchModifySamp && stepType == StepType.Confirm)
            {
                RefreshRow(Selection.GetAllSelectSamp());
            }
            else
            {
                RefreshCurrentBarcode(); //更新当前条码
            }

            if (OnResetFocus != null)
                OnResetFocus(this, EventArgs.Empty);
        }


        public void ShowAndClose(string msg)
        {
            lis.client.control.MessageDialog.ShowAutoCloseDialog(msg, 1m);
        }

        public void ShowAndClose(string msg, decimal second)
        {
            lis.client.control.MessageDialog.ShowAutoCloseDialog(msg, second);
        }

        private void selectDict_Sample_Remarks1_onAfterSelected(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(selectDict_Sample1.valueMember)) //标本为空不提示          
            //    return;

            string barcode = GetBaseInfo().SampBarId;
            if (!string.IsNullOrEmpty(barcode))
            {
                SetControlEditStatus(barcode);
                if (BaseSampMain.SampStatusId != "0" && BaseSampMain.SampStatusId != "1" && BaseSampMain.SampStatusId != "2" && BaseSampMain.SampStatusId != "9")
                {
                    selectDict_Sample_Remarks1.valueMember = BaseSampMain.SampRemId;
                    selectDict_Sample_Remarks1.displayMember = BaseSampMain.SampRemContent;
                    ShowAndClose("已做采集确认的病人资料信息不能再修改");
                    return;
                }
            }

            if (lis.client.control.MessageDialog.Show("是否修改标本备注", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                bool result = proxy.Service.UpdateSampMainRemark(selectDict_Sample_Remarks1.valueMember, selectDict_Sample_Remarks1.displayMember, barcode);
                BaseSampMain.SampRemId = selectDict_Sample_Remarks1.valueMember;
                BaseSampMain.SampRemark = selectDict_Sample_Remarks1.displayMember;
                if (result)
                {
                    ShowAndClose("修改成功！");
                }
                else
                    ShowAndClose("修改失败！");
            }

            RefreshCurrentBarcode();  //更新当前条码
            ResetFocus(this, EventArgs.Empty);
        }

        public event EventHandler OnResetFocus;

        private void ResetFocus(Object sender, EventArgs e)
        {
            if (OnResetFocus != null)
                OnResetFocus(sender, e);
        }

        internal void SetBaseInfoStyle(PrintType printType)
        {
            if (printType == PrintType.Inpatient)
            {
                checkMan.Visible = true;
                cbMan.Visible = true;

                this.gvBarcode.BeginSort();

                try
                {
                    this.gvBarcode.ClearSorting();

                    this.gvBarcode.Columns["PidPatno"].SortIndex = 0;
                    this.gvBarcode.Columns["PidPatno"].SortMode = ColumnSortMode.DisplayText;
                    this.gvBarcode.Columns["PidPatno"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;

                    this.gvBarcode.Columns["PidInNo"].SortIndex = 1;
                    this.gvBarcode.Columns["PidInNo"].SortMode = ColumnSortMode.DisplayText;
                    this.gvBarcode.Columns["PidInNo"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                }
                finally
                {
                    this.gvBarcode.EndSort();
                }
            }
            else if (printType == PrintType.Outpatient)
            {
                checkMan.Visible = true;
                cbMan.Visible = true;
                gvBarcode.Columns["PidBedNo"].Visible = false;
                gridColumn14.VisibleIndex = 4;
                gvBarcode.Columns["PidInNo"].Caption = "病人ID";
            }
            else if (printType == PrintType.TJ)
            {
                this.gvBarcode.BeginSort();
                try
                {
                    this.gvBarcode.ClearSorting();
                    //this.gvBarcode.Columns["bc_pid"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                    this.gvBarcode.Columns["PidPatno"].SortIndex = 0;
                    this.gvBarcode.Columns["PidPatno"].SortMode = ColumnSortMode.DisplayText;
                    this.gvBarcode.Columns["PidPatno"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;

                    this.gvBarcode.Columns["PidInNo"].SortIndex = 1;
                    this.gvBarcode.Columns["PidInNo"].SortMode = ColumnSortMode.DisplayText;
                    this.gvBarcode.Columns["PidInNo"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                }
                finally
                {
                    this.gvBarcode.EndSort();
                }
            }
            else
            {

            }
        }

        /// <summary>
        /// 清除打勾
        /// </summary>
        internal void ClearChecked()
        {
            if (Selection != null)
                Selection.ClearSelection();
        }

        private void checkMan_Click(object sender, EventArgs e)
        {
            if (!HasData() || HasNotSelect())
            {

                return;
            }
            if (BaseSampMain.SampStatusId == new ReceiveStep().StepCode)
            {
                /**********增加最后一次操作签名的操作者和时间**********************************/

                EntitySampProcessDetail processDetail = new ProxySampProcessDetail().Service.GetLastSampProcessDetail(BaseSampMain.SampBarId);

                lis.client.control.MessageDialog.ShowAutoCloseDialog(string.Format("标本已经签收,不能修改!操作者：{0}，时间：{1}",
                                                                                    processDetail.ProcUsername,
                                                                                    processDetail.ProcDate.ToString()), 1);
                /***********************************************************/

                return;
            }

            bool man = this.checkMan.Checked;
            BarcodePatients bc = new BarcodePatients();
            Int64 id = 0;
            if (Int64.TryParse(BaseSampMain.SampSn.ToString(), out id))
            {
                bc.Id = id;
            }

            if (bc.Id == null || bc.Id <= 0)
            {
                ShowAndClose("修改失败");
                return;
            }

            if (this.checkMan.Checked)
            {
                string strInfo = cbMan.Text;
                bc.Name = BaseSampMain.PidName + strInfo;
                if (strInfo == "之夫" || strInfo == "之子")
                    bc.Sex = "男";
                else
                    bc.Sex = "女";
            }

            if (!this.checkMan.Checked)
            {
                string strInfo = cbMan.Text;
                bc.Name = BaseSampMain.PidName.Replace(strInfo, "");
                if (strInfo == "之夫" || strInfo == "之子")
                    bc.Sex = "女";
                else
                    bc.Sex = "男";
            }

            bool result = proxy.Service.UpdateSampMainNameAndSex(bc.Name, bc.Sex, BaseSampMain.SampBarId.ToString());

            if (result)
            {
                ShowAndClose("成功", 1m);
                BaseSampMain.PidName = bc.Name;
                BaseSampMain.PidSex = bc.Sex;
                RefreshCurrentBarcode();
                return;

            }
            else
            {
                ShowAndClose("失败", 1m);
                return;
            }
        }

        private void txtAge_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            if (bsPatient.Current == null)
            {
                ShowAndClose("请选择要修改的数据！");
                return;
            }
            string barcode = GetBaseInfo().SampBarId;
            if (!string.IsNullOrEmpty(barcode))
            {
                SetControlEditStatus(barcode);
            }
            if (BaseSampMain.SampStatusId != "0" && BaseSampMain.SampStatusId != "1" && BaseSampMain.SampStatusId != "2")
            {
                txtAge.EditValue = BaseSampMain.SampAge;
                ShowAndClose("已做采集确认的病人资料信息不能再修改");
                return;
            }


            DialogResult verification = new DialogResult();

            if (Step.StepName == "条码查询")
            {
                FrmCheckPassword frmCheck = new FrmCheckPassword();
                verification = frmCheck.ShowDialog();
            }
            else
            {
                if (Step.Printer is Inpatient 
                    || Step.Printer is Manual 
                    || ConfigHelper.GetSysConfigValueWithoutLogin("MZpatientBarcodeChangeConfirm") == "否")//住院条码、手工条码直接过
                    verification = DialogResult.OK;
                else
                {
                    FrmHISCheckPassword checkPassword = new FrmHISCheckPassword(Step.Printer.Audit);
                    verification = checkPassword.ShowDialog();
                }
            }

            if (verification == DialogResult.OK)
            {
                //BarcodeAgeInfo ChangeAgeInfo = new BarcodeAgeInfo(GetBaseInfo().PidInNo, GetBaseInfo().SampBarId, new Age_xYxMxDxHxI_YMDHI_IntType().ConvertRule(this.txtAge.Text.Trim()));
                bool result = false;// BarcodeClient.NewInstance.ChangeAge(ChangeAgeInfo);
                if (result)
                {
                    ShowAndClose("修改成功!");
                }
                else
                    ShowAndClose("修改失败!");
            }
            else
            {
                ShowAndClose("验证权限失败!");
            }

            RefreshCurrentPatient();  //更新当前条码

            if (OnResetFocus != null)
                OnResetFocus(this, EventArgs.Empty);
        }

        private void txtSex_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            if (bsPatient.Current == null)
            {
                ShowAndClose("请选择要修改的数据！");
                return;
            }

            string barcode = GetBaseInfo().SampBarId;
            if (!string.IsNullOrEmpty(barcode))
            {
                SetControlEditStatus(barcode);
            }
            if (BaseSampMain.SampStatusId != "0" && BaseSampMain.SampStatusId != "1" && BaseSampMain.SampStatusId != "2")
            {
                txtSex.EditValue = BaseSampMain.PidSex;
                ShowAndClose("已做采集确认的病人资料信息不能再修改");
                return;
            }


            if (BaseSampMain.SampStatusId == new ReceiveStep().StepCode)
            {
                ShowAndClose("标本已经签收,不能修改");
                return;
            }

            DialogResult verification = new DialogResult();

            if (Step.StepName == "条码查询")
            {
                FrmCheckPassword frmCheck = new FrmCheckPassword();
                verification = frmCheck.ShowDialog();
            }
            else
            {
                if (Step.Printer is Inpatient || Step.Printer is Manual || ConfigHelper.GetSysConfigValueWithoutLogin("MZpatientBarcodeChangeConfirm") == "否")//住院条码、手工条码直接过
                    verification = DialogResult.OK;
                else
                {
                    FrmHISCheckPassword checkPassword = new FrmHISCheckPassword(Step.Printer.Audit);
                    verification = checkPassword.ShowDialog();
                }
            }

            if (verification == DialogResult.OK)
            {
                //BarcodeSexInfo ChangeSexInfo = new BarcodeSexInfo(GetBaseInfo().PidInNo, GetBaseInfo().SampBarId, this.txtSex.Text.Trim());
                bool result = false;//BarcodeClient.NewInstance.ChangeSex(ChangeSexInfo);
                if (result)
                {
                    ShowAndClose("修改成功!");
                }
                else
                    ShowAndClose("修改失败!");
            }
            else
            {
                ShowAndClose("验证权限失败!");
            }

            RefreshCurrentPatient();  //更新当前条码

            if (OnResetFocus != null)
                OnResetFocus(this, EventArgs.Empty);
        }

        private void gvBarcode_Click(object sender, EventArgs e)
        {
            if (clikcA != null)
            {
                clikcA();
            }
        }

        public delegate void ClikeHander();
        public event ClikeHander clikcA;

        private void gvBarcode_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle == this.gvBarcode.FocusedRowHandle)
            {
                e.Appearance.BackColor = System.Drawing.Color.LightBlue;
                e.Appearance.Options.UseBackColor = true;
            }
            EntitySampMain sampMain = (EntitySampMain)gvBarcode.GetRow(e.RowHandle);
            if (BarcodeColorStyle != "默认")//默认是设置单元格颜色
            {
                if (sampMain != null && !string.IsNullOrEmpty(sampMain.SampStatusId))
                {
                    Color color = GetColorFormat(sampMain.SampStatusId.Trim());
                    if (BarcodeColorStyle == "整行字体")
                    {
                        e.Appearance.ForeColor = color; //整行赋相应状态的字体颜色
                    }
                    else
                    {
                        e.Appearance.BackColor = color; //整行赋相应状态的字体颜色
                    }
                }
            }
        }

        public Color GetColorFormat(string bc_status)
        {
            Color color = new Color();
            if (bc_status == EnumBarcodeOperationCode.BarcodeGenerate.ToString() || bc_status == EnumBarcodeOperationCode.DeleteDetail.ToString())
            {
                color = IStep.GetBarcodeConfigColor( "Barcode_Color_Downloaded");
            }
            else if (bc_status == EnumBarcodeOperationCode.BarcodePrint.ToString() || bc_status == EnumBarcodeOperationCode.SampleReturn.ToString())
            {
                color = IStep.GetBarcodeConfigColor("Barcode_Color_Printed");
            }
            else if (bc_status == EnumBarcodeOperationCode.SampleCollect.ToString())
            {
                color = IStep.GetBarcodeConfigColor("Barcode_Color_Blooded");
            }
            else if (bc_status == EnumBarcodeOperationCode.SampleSend.ToString())
            {
                color = IStep.GetBarcodeConfigColor("Barcode_Color_Sended");
            }
            else if (bc_status == EnumBarcodeOperationCode.SampleReach.ToString())
            {
                color = IStep.GetBarcodeConfigColor("Barcode_Color_Reach");
            }
            else if (bc_status == EnumBarcodeOperationCode.SampleReceive.ToString() || bc_status == EnumBarcodeOperationCode.SampleRegister.ToString() ||
                    bc_status == EnumBarcodeOperationCode.Audit.ToString() || bc_status == EnumBarcodeOperationCode.UndoAudit.ToString() ||
                    bc_status == EnumBarcodeOperationCode.UndoReport.ToString() || bc_status == EnumBarcodeOperationCode.SampleSecondSend.ToString() ||
                    bc_status == EnumBarcodeOperationCode.AppendBarcode.ToString() || bc_status == EnumBarcodeOperationCode.DeletePatient.ToString() ||
                    bc_status == "30")
            {
                color = IStep.GetBarcodeConfigColor("Barcode_Color_Received");
            }
            else if (bc_status == EnumBarcodeOperationCode.Report.ToString())
            {
                color = IStep.GetBarcodeConfigColor("Barcode_Color_Reported");
            }

            return color;
        }

        /// <summary>
        /// 对'已打印条码'进行权限验证
        /// </summary>
        /// <returns>通过true,否则false</returns>
        private bool powerConfirm(out string OperatorName, out string OperatorID)
        {
            OperatorName = "-1";
            OperatorID = "-1";
            FrmCheckPassword checkMPower = new FrmCheckPassword();//身份验证
            if (checkMPower.ShowDialog() != DialogResult.OK) return false;

            List<dcl.entity.EntityUserRole> listUserRole = new List<entity.EntityUserRole>();

            listUserRole = listUserRole.FindAll(w => w.UserLoginId == checkMPower.OperatorID && w.RoleRemark.IndexOf("护长组") >= 0);

            if (listUserRole.Count <= 0 && checkMPower.OperatorID != "admin")
            {
                lis.client.control.MessageDialog.Show("【已打印】的条码\r\n非护长组的无权限打印!");
                return false;
            }
            OperatorName = checkMPower.OperatorName;
            OperatorID = checkMPower.OperatorID;

            return true;
        }

        List<GridColumn> GetInitColumnSort()
        {
            List<GridColumn> list = new List<GridColumn>();

            list.Add(gridColumn9);
            list.Add(gridColumn10);
            list.Add(gridColumn4);
            list.Add(gridColumn14);
            list.Add(gridColumn5);
            list.Add(gridColumn6);
            list.Add(gridColumn51);
            list.Add(gridColumn17);
            list.Add(gridColumn12);
            list.Add(gridColumn7);
            list.Add(gridColumn59);
            list.Add(gridColumn48);
            list.Add(gridColumn63);
            list.Add(gridColumn11);
            list.Add(gridColumn8);
            list.Add(gridColumn13);
            list.Add(gridColumn45);
            list.Add(gridColumn16);
            list.Add(gridColumn3);
            list.Add(gridColumn61);
            list.Add(gridColumn54);
            list.Add(gridColumn55);
            list.Add(col_pid);
            list.Add(gridColumn60);

            return list;
        }

        public void SetColumnSort(string colMzSortValue)
        {
            List<GridColumn> list = GetInitColumnSort();
            GridColumn[] cols = list.ToArray();
            string[] sortStrings = colMzSortValue.Split(',');

            int num;
            for (int i = 0; i < sortStrings.Length; i++)
            {
                if (string.IsNullOrEmpty(sortStrings[i]) || !int.TryParse(sortStrings[i], out num))
                    continue;
                GridColumn newcol = cols[num - 1];
                for (int j = 1; j < gvBarcode.VisibleColumns.Count; j++)
                {
                    GridColumn column = gvBarcode.VisibleColumns[j];
                    if (newcol.FieldName == column.FieldName)
                    {
                        column.VisibleIndex = i + 1;
                        break;
                    }
                }
            }
            gridColumn15.VisibleIndex = 1;
        }

        public void ExportToExcel()
        {
            if (gcBarcode != null)
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.DefaultExt = "xls";
                ofd.Filter = "Excel文件(*.xls)|*.xls";
                ofd.Title = "导出到Excel";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.Trim() == string.Empty)
                    {
                        lis.client.control.MessageDialog.Show("文件名不能为空！", "提示");
                        return;
                    }

                    try
                    {
                        gcBarcode.ExportToXls(ofd.FileName.Trim());
                        lis.client.control.MessageDialog.Show("导出成功！", "提示");
                    }
                    catch (Exception)
                    {
                    }
                }

            }
        }


        public void SetTjCompanyVisable()
        {
            gridColumn15.Visible = false;
        }

        /// <summary>
        /// 条码打包
        /// </summary>
        internal void PackBarcode()
        {
            try
            {
                if (!HasData())
                {
                    ShowAndClose("没条码数据!");
                    return;
                }
                string packRule = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_PackRule");
                List<EntitySampMain> listSour = (List<EntitySampMain>)PatientBindingSource.DataSource;
                string prtTemplate = "Pack_Barcode_Report";
                var gp = listSour.GroupBy(w => w.SampInfo).Select(group => new { group.Key });
                if (packRule == "根据目的地字段打包")
                {
                    gp = listSour.GroupBy(w => w.SampSendDest).Select(group => new { group.Key });
                }
                string machineName = GetPrintMachineName();
                FrmReportPrint pForm = new FrmReportPrint();
                string typeid = string.Empty;
                string remark = string.Empty;
                string typename = string.Empty;
                bool printBarcodeSuccess = false;
                string strCTypeId = string.Empty;
                string bcDepName = string.Empty;
                List<EntitySampMain> listS = new List<EntitySampMain>();
                //if (gp.Count() > 0)
                //{
                foreach (var item in gp)
                {
                    List<string> listBarCode = new List<string>();
                    string strTypeExp = item.Key.ToString();

                    if (strTypeExp != string.Empty)
                    {
                        typeid = item.Key.ToString();
                        listS = listSour.FindAll(w => w.SampInfo == strTypeExp);
                        if (packRule == "根据目的地字段打包")
                        {
                            listS = listSour.FindAll(w => w.SampSendDest == strTypeExp);
                        }
                        if (ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_PackWay") == "过滤已打包条码")
                        {
                            string msgPacked = "";
                            foreach (EntitySampMain dr in listS)
                            {
                                if (!string.IsNullOrEmpty(dr.PidUniqueId.ToString()))
                                {
                                    msgPacked += string.Format("条码号 [{0}]  姓名 [{1}]\r\n"
                                    , dr.SampBarCode.ToString()
                                    , dr.PidName.ToString()
                                    );
                                }
                                listBarCode.Add(dr.SampBarCode);
                            }
                            string message = string.Empty;

                            if (!string.IsNullOrEmpty(msgPacked))
                            {
                                message += "以下条码已打包，不能重新打包，此次打包将不包含以下条码\r\n" + msgPacked + "\r\n";
                                lis.client.control.MessageDialog.Show(message);
                            }
                            listS = listSour.FindAll(w => w.SampInfo == strTypeExp && string.IsNullOrEmpty(w.PidUniqueId));
                            if (listS.Count == 0)
                            {
                                lis.client.control.MessageDialog.Show("包号[" + strTypeExp + "]中无可打包的条码");
                                continue;
                            }
                        }
                        typename = listS[0].ProName;

                        strCTypeId = typeid + ServerDateTime.GetServerDateTime().ToString("yyMMddHHmmss");

                        if (strCTypeId.Length % 2 != 0)
                            strCTypeId = "0" + strCTypeId;
                    }
                    else
                    {
                        if (packRule == "根据目的地字段打包")
                        {
                            listS = listSour.FindAll(w => string.IsNullOrEmpty(w.SampSendDest));
                        }
                        else
                        {
                            listS = listSour.FindAll(w => w.SampInfo == null || w.SampInfo == "");
                        }
                        typename = listSour[0].ProName;
                        if (listSour != null && listSour.Count > 0)
                        {
                            bcDepName = listSour[0].PidDeptName;
                        }
                        DateTime dtiTodayNow = DateTime.Now;

                        //获取数据库时间
                        dtiTodayNow = ServerDateTime.GetServerDateTime();

                        strCTypeId = dtiTodayNow.ToString("yyMMddHHmmssffff");
                    }

                    // }

                    EntityDCLPrintParameter paramter = new EntityDCLPrintParameter();
                    paramter.ReportCode = prtTemplate;
                    paramter.CustomParameter.Add("ctypeid", strCTypeId);
                    paramter.CustomParameter.Add("ctypename", typename);
                    paramter.CustomParameter.Add("remark", remark);
                    paramter.CustomParameter.Add("packcount", listS.Count);
                    paramter.CustomParameter.Add("opname", (UserInfo.userName != null ? UserInfo.userName : ""));
                    paramter.CustomParameter.Add("optime", ServerDateTime.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss"));
                    ProxyReportPrint proxyPrint = new ProxyReportPrint();
                    EntityDCLPrintData printData = proxyPrint.Service.GetReportSource(paramter);

                    //串口打印
                    if (HQ.BPPrint.BPPrintSetting.Current.EnableBPPrint)
                    {

                        if (printData.ReportData.Tables != null && printData.ReportData.Tables.Count > 0)
                        {
                            DataTable dt = printData.ReportData.Tables["可设计字段"];
                            DataRow[] dvNotPrinted = dt.Select();
                            printBarcodeSuccess = HQ.BPPrint.LisBarcodePrinter.Print(dvNotPrinted);
                        }
                        else
                        {
                            throw new Exception("串口打印错误!找不到报表sql,代码为Pack_Barcode_Report.");
                        }
                    }
                    else
                    {
                        DCLReportPrint.PrintByData(printData);
                    }
                    foreach (EntitySampMain samp in listS)
                    {
                        listBarCode.Add(samp.SampBarCode);
                    }
                    if (listBarCode.Count > 0)
                    {
                        ProxySampMain proxy = new ProxySampMain();
                        proxy.Service.UpdateBarcodeBale(strCTypeId, listBarCode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowAutoCloseDialog(ex.Message, 3);
            }

        }


        /// <summary>
        /// 列表-目的地显示大些
        /// </summary>
        public void showBiggridColumn51()
        {
            gvBarcode.RowHeight = 28;
            this.gridColumn51.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn51.AppearanceCell.ForeColor = System.Drawing.Color.Red;
            this.gridColumn51.AppearanceCell.Options.UseFont = true;
            this.gridColumn51.AppearanceCell.Options.UseForeColor = true;
            repositoryItemLookUpEdit1.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        /// <summary>
        /// 采血确认操作时间
        /// </summary>
        private DateTime BloodConfirmRunTime = DateTime.MinValue;

        private string BloodConfirm_loginID { get; set; }

        private string userName_loginID { get; set; }

        /// <summary>
        /// 采集确认
        /// </summary>
        public void BloodConfirm()
        {
            List<EntitySampMain> rows = Selection.GetAllSelectSamp();
            if (rows != null && rows.Count > 0)
            {
                int BC_MZsetBloodConfirmWaitTime_Int = 200;//默认200秒
                //系统配置：门诊条码界面[采集确认]验证时长(秒)
                string BC_MZsetBloodConfirmWaitTime = ConfigHelper.GetSysConfigValueWithoutLogin("BC_MZsetBloodConfirmWaitTime");
                if (!string.IsNullOrEmpty(BC_MZsetBloodConfirmWaitTime)
                    && int.TryParse(BC_MZsetBloodConfirmWaitTime, out BC_MZsetBloodConfirmWaitTime_Int)
                    && BC_MZsetBloodConfirmWaitTime_Int > 0)
                {

                }
                else
                {
                    BC_MZsetBloodConfirmWaitTime_Int = 200;
                }

                List<EntitySampMain> listSampMain = new List<EntitySampMain>();

                foreach (EntitySampMain row in rows)
                {
                    if (IsNotEmpty(row.SampBarId))
                    {

                        #region 检查条码状态

                        EntitySampMain sampMain = proxy.Service.SampMainQueryByBarId(row.SampBarId);

                        if (sampMain != null && !string.IsNullOrEmpty(sampMain.SampBarId))
                        {
                            string status = sampMain.SampStatusId;
                            string temp_bc_blood_flag = sampMain.SampBloodFlag.ToString();
                            string temp_bc_status_cname = sampMain.SampStatusName;
                            if (status == "0" || status == "9")
                            {
                                lis.client.control.MessageDialog.Show("条码【" + sampMain.SampBarId + "】未打印,不能采集确认！");
                                return;
                            }
                            else if (temp_bc_blood_flag == "1")
                            {
                                lis.client.control.MessageDialog.Show("条码【" + sampMain.SampBarId + "】已采集！");
                                return;
                            }
                            else if (status != "1" && status != "2" && !string.IsNullOrEmpty(temp_bc_status_cname)
                                && temp_bc_status_cname.Contains("已"))
                            {
                                lis.client.control.MessageDialog.Show("条码【" + sampMain.SampBarId + "】," + temp_bc_status_cname + ",不能采集确认！");
                                return;
                            }
                            else if (status != "1" && status != "2" && !string.IsNullOrEmpty(temp_bc_status_cname)
                                && !temp_bc_status_cname.Contains("已"))
                            {
                                lis.client.control.MessageDialog.Show("条码【" + sampMain.SampBarId + "】,已" + temp_bc_status_cname + ",不能采集确认！");
                                return;
                            }
                            else if (status != "1" && status != "2")
                            {
                                lis.client.control.MessageDialog.Show("条码【" + sampMain.SampBarId + "】,不能采集确认！");
                                return;
                            }
                            else if (status == "1" && temp_bc_blood_flag != "1")
                            {

                            }
                            else
                            {
                                lis.client.control.MessageDialog.Show("只有已打印并且未采集的条码才能采集确认！");
                                return;
                            }
                        }
                        #endregion

                        listSampMain.Add(sampMain);
                    }
                }

                if (listSampMain.Count > 0)
                {
                    EntitySampMain temp_BaseInfo = listSampMain[0];

                    if (temp_BaseInfo != null)
                    {
                        IStep temp_boolConfirm = StepFactory.CreateStep(StepType.Sampling);
                        temp_boolConfirm.BaseSampMain = temp_BaseInfo;

                        #region 身份验证

                        if (string.IsNullOrEmpty(BloodConfirm_loginID) || string.IsNullOrEmpty(userName_loginID)
                                        || BloodConfirmRunTime == DateTime.MinValue
                                        || BloodConfirmRunTime <= DateTime.Now.AddSeconds(-BC_MZsetBloodConfirmWaitTime_Int))
                        {
                            FrmHISCheckPassword frm2 = new FrmHISCheckPassword(temp_boolConfirm.Audit);
                            frm2.Text = "采集待确认条码有" + listSampMain.Count.ToString() + "条";

                            if (frm2.ShowDialog() != DialogResult.OK)
                            {
                                return;
                            }
                            else
                            {
                                BloodConfirm_loginID = frm2.OperatorID;
                                userName_loginID = frm2.OperatorName;
                            }
                        }
                        #endregion

                        BloodConfirmRunTime = DateTime.Now;

                        if (true)
                        {
                            #region 送达与签收步骤取本地配置的"物理组"

                            EntitySampOperation sampOpe = new EntitySampOperation(BloodConfirm_loginID, userName_loginID);


                            if (temp_boolConfirm is SamplingStep)
                            {
                                sampOpe.OperationPlace = LocalSetting.Current.Setting.DeptName;
                            }

                            if (temp_boolConfirm is ReceiveStep || temp_boolConfirm is ReachStep || temp_boolConfirm is SendStep)
                            {
                                sampOpe.Remark = string.Format("IP地址:{0}", IPUtility.GetIP());
                            }
                            //条码所有确认流程记录本地配置[描述]设置的地点
                            if (Barcode_RecordSignPlace)
                            {
                                sampOpe.OperationPlace = LocalSetting.Current.Setting.Description;
                            }
                            sampOpe.OperationTime = IStep.GetServerTime();
                            sampOpe.OperationWorkId = "";
                            #endregion

                            temp_boolConfirm.ComfirmAll(sampOpe, listSampMain);

                            //RefreshCurrentBarcode();
                            //RefreshRow(listSampMain);
                        }
                    }
                }
            }
            else
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("请勾选需要采集确认的条码！");
            }
        }

        private void gcBarcode_DoubleClick(object sender, EventArgs e)
        {
            EntitySampMain row = (EntitySampMain)gvBarcode.GetFocusedRow();
            if (row != null && stepType == StepType.Confirm)
            {
                string text = string.Format("条码号：{0} 姓名：{1} 住院号：{2} 检查项目：标本：{3} 标本已签收！", row.SampBarCode, row.PidName, row.PidInNo, row.SampSamName);
            }
        }

        /// <summary>
        /// 复制标本信息
        /// </summary>
        /// <returns></returns>
        public string CopyBarcodeInfo()
        {
            string text = string.Empty;
            string[] strList = null;
            EntitySampMain row = (EntitySampMain)gvBarcode.GetFocusedRow();
            if (row != null)
            {
                strList = row.SampComName.Split('+');
                string age = row.PidAge.ToUpper().Replace('Y', '岁').Replace("M", "月").Replace('D', '天').Replace('H', '时').Replace('I', '分');
                foreach (string comName in strList)
                {
                    text += string.Format("条码号：{0} 姓名：{1} 住院号：{2} 检查项目：{3}标本：{4} 年龄：{5} 性别：{6} 科室：{7} 医生：{8} 诊断：{9} 标本签收！\r\n",
                    row.SampBarCode, row.PidName, row.PidInNo, comName, row.SampSamName, age, row.PidSex, row.PidDeptName, row.PidDoctorName, row.PidDiag);
                }
            }
            return text;
        }
        bool isTypeIdExist = false;
        public void selectDict_Sample1_onBeforeFilter()
        {
            #region 过滤微生物室标本
            EntitySampMain currentEntity = GetBaseInfo();
            string bacteriaTypeId = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_BacteriaTypeId");
            isTypeIdExist = false;
            if (currentEntity != null && !string.IsNullOrEmpty(bacteriaTypeId))
            {
                var sampType = this.listDicSample.Find(a => a.SamId == currentEntity.SampSamId);
                if (sampType != null)
                {
                    this.selectDict_Sample1.selectRow = sampType;
                    this.selectDict_Sample1.displayMember = sampType.SamName;
                    this.selectDict_Sample1.valueMember = sampType.SamId;
                }

                string[] wordsArray = bacteriaTypeId.Split(',');
                foreach (string word in wordsArray)
                {
                    if (!string.IsNullOrEmpty(currentEntity.SampType) && currentEntity.SampType == word)
                        isTypeIdExist = true;
                }

                if (isTypeIdExist)
                {
                    List<EntityDicSample> listFilter = new List<EntityDicSample>();
                    listFilter = listDicSample.FindAll(w => !string.IsNullOrEmpty(w.SamTransType)).ToList();
                    this.selectDict_Sample1.SetFilter(listFilter);
                }
                else
                {
                    List<EntityDicSample> listALL = new List<EntityDicSample>();
                    listALL = EntityManager<EntityDicSample>.ListClone(listDicSample);
                    this.selectDict_Sample1.SetFilter(listALL);
                }
            }
            #endregion
        }

        private void gvCname_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView grid = sender as GridView;
            EntitySampDetail dr = (EntitySampDetail)grid.GetRow(e.RowHandle);
            if (dr != null)
            {
                if (e.Column.FieldName == "CommFlag" && dr.CommFlag.ToString() == "1")
                {
                    e.Appearance.BackColor = Color.Green;
                }
            }
        }

        /// <summary>
        /// 是否显示病人信息
        /// </summary>
        /// <param name="show"></param>
        public void ShowPatInfoColumns(bool show)
        {
            //if (show)
            //{
            //    this.gcColItrName.Visible = show;
            //    this.gcColItrName.VisibleIndex = 5;

            //    this.gcColPatSid.Visible = show;
            //    this.gcColPatSid.VisibleIndex = 6;
            //}
            //else
            //{
            //    this.gcColPatSid.Visible = false;
            //    this.gcColPatSid.VisibleIndex = -1;

            //    this.gcColItrName.Visible = false;
            //    this.gcColItrName.VisibleIndex = -1;
            //}
        }

        public void ShowInfoYG()
        {
            gridColumn15.Visible = false;
            gridColumn4.Visible = false;
            colbc_upid.Visible = false;
            gridColumn10.Caption = "监测对象";
            label1.Text = "监测内容:";
            label1.Location = new System.Drawing.Point(-3, 34);
            this.txtShowName.Size = new System.Drawing.Size(340, 24);
            checkMan.Visible = cbMan.Visible = label6.Visible = txtAge.Visible = label11.Visible = txtSex.Visible
               = false;
        }

        /// <summary>
        /// 保存条码信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave_Click(object sender, EventArgs e)
        {
            if(CurrentSampMain == null)
            {
                return;
            }
            string barcode = GetBaseInfo().SampBarId;
            if (!string.IsNullOrEmpty(barcode))
            {
                SetControlEditStatus(barcode);
                if (CurrentSampMain.SampStatusId == "60" )
                {
                    ShowAndClose("该条码已出报告，不允许修改！");
                    return;
                }
            }


            CurrentSampMain.PidIdentityName = selectIdentifierType1.displayMember;
            CurrentSampMain.PidIdentityCard = txtIdentifierID.Text;
            CurrentSampMain.SampPatType = selectPatType1.displayMember;
            CurrentSampMain.PidAddress = txtCurrentAddress.Text;

            bool result = proxy.Service.UpdateSampMainOtherInfo(CurrentSampMain, barcode);

            if (result)
            {
                ShowAndClose("修改成功！");
            }
            else
                ShowAndClose("修改失败！");

            RefreshCurrentBarcode();  //更新当前条码
            ResetFocus(this, EventArgs.Empty);
        }

        private void gvBarcode_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {

            if (e.Column.Caption == "序号")
                e.DisplayText = (e.ListSourceRowIndex + 1).ToString();

        }

        private void gvBarcode_CustomDrawRowIndicator_1(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if(e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString() ;
            }
        }
    }

}
