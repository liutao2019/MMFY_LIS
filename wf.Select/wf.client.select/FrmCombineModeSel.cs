using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using DevExpress.XtraReports.UI;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Linq;
using dcl.client.common;
using dcl.client.report;
using System.Diagnostics;
using lis.client.control;
using System.IO;
using dcl.client.wcf;
using Lib.LogManager;
using System.Configuration;
using dcl.entity;
using dcl.common;
using dcl.client.control;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using dcl.client.cache;

namespace dcl.client.resultquery
{
    public partial class FrmCombineModeSel : FrmCommon
    {
        #region 字段属性

        bool CanSearchOuterReport = false;
        protected selectParameter selPar = new selectParameter();
        bool EnableCardReader = false;
        string CanQueryOtherDeptReport = string.Empty;
        bool SearchOuterReportInterfaceMode = true;//使用旧金域接口进行查询报告单
        bool SearchCDR = false;
        bool SerialNumber = false;//流水号查询
        int intDefaultQueryInterval = 3;//默认的查询间隔
        string labelPatIDText = string.Empty;

        DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit gridheadercheckbox;// = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();

        List<EntityDicInstrument> dtItr;

        ProxyCombModelSel proxy = new ProxyCombModelSel();
        List<entity.EntityPidReportMain> listEntPatients = new List<entity.EntityPidReportMain>();//

        string strCardReaderDriver;
        //用于判断病人ID是否进行了统一设置
        string PatientIDNameConfirm;

        /// <summary>
        /// 是否输出打印者与时间信息
        /// </summary>
        bool IsOutputPrintPersonAndTime { get; set; }

        /// <summary>
        /// 是否打印报告时需要设置打印人
        /// </summary>
        bool IsOutputPrintPersonAndTimeByPrint { get; set; }

        /// <summary>
        /// 输出打印者
        /// </summary>
        string outPrintPerson { get; set; }
        string outPrintPersonID { get; set; }

        private DateTime? LastPrintTime;

        private string labQuery_PatName_SearchModel = "全模糊";

        string patientsSelectViewSortConfigcode = "PatientsSelectViewSort";
        /// <summary>
        /// 输入打印时间
        /// </summary>
        string outPrintTime { get; set; }

        /// <summary>
        /// 病历号查询前面自动补零位数
        /// </summary>
        private string patInNoAutoAddZeroNum;

        /// <summary>
        /// 是否过滤特殊病人
        /// </summary>
        private bool Query_FilterSpecPatients = false;

        /// <summary>
        /// 流水号查询条件对应查询字段
        /// </summary>
        private string SelColNameForplPid = "pat_pid";

        /// <summary>
        /// 流水号查询条件对应第三方接口(命令ID)
        /// </summary>
        private string SelCmdIDForplPid = "";

        /// <summary>
        /// 修改登录用户信息(供住院登录模式使用)
        /// </summary>
        [System.ComponentModel.Description("修改登录用户信息(供住院登录模式使用)")]
        public string setLoginID
        {
            get
            {
                return UserInfo.loginID;
            }
            set
            {
                UserInfo.loginID = value;
            }
        }

        private bool Lab_EnableNoBarCodeCheck = false;

        /// <summary>
        /// 住院报告查询支持多病人同时查询过滤
        /// </summary>
        bool Select_EnableMutliPatQuery = false;

        private string Lab_NoBarCodeCheckItrExpectList = "";
        bool Lab_ReportCodeIsNullNotAllowPrint = false;

        /// <summary>
        /// 是否要预览打印(目前只有批量打印使用)
        /// </summary>
        bool HasShowPreview = false;

        #endregion

        #region 初始化加载等

        /// <summary>
        /// 非独立加载
        /// </summary>
        public FrmCombineModeSel()
        {
            InitializeComponent();

            selPar.EnbState = false;

            this.Shown += new EventHandler(FrmCombineModeSel_Shown);

            //? t
            SearchOuterReportInterfaceMode = ConfigHelper.GetSysConfigValueWithoutLogin("SearchOuterReportInterfaceMode") != "新接口";

            //系统配置：批量打印时需要设置打印人 f
            IsOutputPrintPersonAndTime = ConfigHelper.GetSysConfigValueWithoutLogin("Repselect_setPrintPerson") == "是";

            //系统配置：打印报告时需要设置打印人 n
            IsOutputPrintPersonAndTimeByPrint = ConfigHelper.GetSysConfigValueWithoutLogin("Repselect_setPrintPersonByPrint") == "是";

            CanSearchOuterReport = ConfigHelper.GetSysConfigValueWithoutLogin("CanSearchOuterReport") == "是";

            labQuery_PatName_SearchModel = ConfigHelper.GetSysConfigValueWithoutLogin("LabQuery_PatName_SearchModel");
            patInNoAutoAddZeroNum = ConfigHelper.GetSysConfigValueWithoutLogin("LabQuery_PatInNoAutoAddZeroNum");
            Query_FilterSpecPatients = ConfigHelper.GetSysConfigValueWithoutLogin("Query_FilterSpecPatients") == "是";
            Lab_EnableNoBarCodeCheck = ConfigHelper.GetSysConfigValueWithoutLogin("Lab_EnableNoBarCodeCheck") == "是";
            Lab_NoBarCodeCheckItrExpectList = ConfigHelper.GetSysConfigValueWithoutLogin("Lab_NoBarCodeCheckItrExpectList");
            Lab_ReportCodeIsNullNotAllowPrint = ConfigHelper.GetSysConfigValueWithoutLogin("Lab_ReportCodeIsNullNotAllowPrint") == "是";

            //自定义审核显示语 检验
            string AuditWord = ConfigHelper.GetSysConfigValueWithoutLogin("AuditWord");
            //自定义报告显示语 审核
            this.chkReported.Text = "已" + ConfigHelper.GetSysConfigValueWithoutLogin("ReportWord");

            this.chkUnaudited.Text = "未" + AuditWord;
            this.lbAuditOper.Text = AuditWord + "者";
            this.chkAudited.Text = "已" + AuditWord;
           

            //是否启用读卡器
            string cardconvertConfig = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_EnableCardReader");
            strCardReaderDriver = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_CardReaderDriver");
            if (!string.IsNullOrEmpty(cardconvertConfig))
            {
                this.EnableCardReader = cardconvertConfig.Contains("住院") || cardconvertConfig.Contains("门诊") || cardconvertConfig.Contains("体检");
            }

            sysToolBar1.BtnImport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(BtnImport_Click);

            //病人列表列排序
            this.gridColumn2.VisibleIndex = -1;//样本号数字
            this.col_selected.VisibleIndex = 0;//选择
            this.gridColumn12.VisibleIndex = 1;//排序号
            this.gridColumn1.VisibleIndex = 2;//姓名
            this.gridColumn7.VisibleIndex = 3;//组合
            this.gridColumn15.VisibleIndex = 4;//样本号
            this.gridColumn10.VisibleIndex = 5;//科别
            this.gridColumn8.VisibleIndex = 6;//床号
            this.gridColumn9.VisibleIndex = 7;//病人ID        
            this.gridColumn6.VisibleIndex = 8;//日期           
            this.gridColumn11.VisibleIndex = 9;//序号
            this.gridColumn3.VisibleIndex = 10;//性别
            this.gridColumn4.VisibleIndex = 11;//年龄
            this.gridColumn18.VisibleIndex = 12;//标本名称
            this.gridColumn5.VisibleIndex = 13;//仪器
            this.gridColumn13.VisibleIndex = 14;//检查类型
            this.gridColumn14.VisibleIndex = 15;//申请医生
            this.gridColumn16.VisibleIndex = 16;//申请医生
            this.gridColumn17.VisibleIndex = 17;//申请医生
            this.col_selected.VisibleIndex = 0;//选择
        }

        /// <summary>
        /// 传参单独加载
        /// </summary>
        /// <param name="par"></param>
        /// <param name="depart"></param>
        public FrmCombineModeSel(string par, string depart)
        {
            InitializeComponent();

            selPar.EnbState = true;

            this.Shown += new EventHandler(FrmCombineModeSel_Shown);
            CanSearchOuterReport = ConfigHelper.GetSysConfigValueWithoutLogin("CanSearchOuterReport") == "是";

            //系统配置：批量打印时需要设置打印人
            IsOutputPrintPersonAndTime = ConfigHelper.GetSysConfigValueWithoutLogin("Repselect_setPrintPerson") == "是";

            //系统配置：打印报告时需要设置打印人
            IsOutputPrintPersonAndTimeByPrint = ConfigHelper.GetSysConfigValueWithoutLogin("Repselect_setPrintPersonByPrint") == "是";
            if (ConfigurationManager.AppSettings["EnableMutliPatQuery"] != null
                 && ConfigurationManager.AppSettings["EnableMutliPatQuery"].ToString().Trim() == "Y")
            {
                Select_EnableMutliPatQuery = true;
            }
            labQuery_PatName_SearchModel = ConfigHelper.GetSysConfigValueWithoutLogin("LabQuery_PatName_SearchModel_Clinical");
            CanQueryOtherDeptReport = ConfigHelper.GetSysConfigValueWithoutLogin("CanQueryOtherDeptReport");
            SearchOuterReportInterfaceMode = ConfigHelper.GetSysConfigValueWithoutLogin("SearchOuterReportInterfaceMode") != "新接口";
            Query_FilterSpecPatients = ConfigHelper.GetSysConfigValueWithoutLogin("Query_FilterSpecPatients") == "是";



            patientsSelectViewSortConfigcode = "PatientsSelectViewSortForAlone";
            chkUnaudited.Visible = true;
            chkAudited.Visible = true;
            chkReported.Text = "未打印";
            UserInfo.SkipPower = true;
            DownLoadZYBarcode();
            sysToolBar1.ShowItemToolTips = false;
            if (selPar.Iselect.DepartValidate)
                selPar.Iselect.readDepartInfo();
            labelPatIDText = ConfigHelper.GetSysConfigValueWithoutLogin("InpatientsReportSelectColumnName");// 2010-6-24
            labelPatID.Text = labelPatIDText;
            //住院检验报告查询tab默认[显示报告数据]
            if (ConfigHelper.GetSysConfigValueWithoutLogin("InPatsRepSelectTabPage_xtcShow") != "否")
                this.xtcReport.SelectedTabPage = xtcShow;
        }
        private void FrmCombineModeSel_Shown(object sender, EventArgs e)
        {

            //解决 报告查询，统计分析等具有时间选择需要单击进行编辑； 
            prpPar.Visible = true;
            if (this.MdiParent != null)
            {
                this.MdiParent.Activate();
            }
            this.Activate();
            this.deStart.Focus();
            prpPar.printPreviewStaticItem3.Caption = "100%";
            prpPar.zoomBarEditItem1.EditValue = "100%";
        }
        private void FrmCombineModeSel_Load(object sender, EventArgs e)
        {
            //录入者检验者过滤检验组显示
            selectInOper.SetFilter(selectInOper.getDataSource().FindAll(i => i.UserType == "检验组"));
            selectAuditOper.SetFilter(selectAuditOper.getDataSource().FindAll(i => i.UserType == "检验组"));

            //设置显示导出按钮
            prpPar.SetCanExportFile(UserInfo.GetSysConfigValue("LabQuery_EnableExportReport") == "是");
            prpPar.printControl1.Zoom = 1.2f;

            gridheadercheckbox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();

            setPanlVisible();

            //创建 /lis 目录
            if (!Directory.Exists(PathManager.SettingLisPath))
                Directory.CreateDirectory(PathManager.SettingLisPath);

            base.ShowSucessMessage = false;

            sysToolBar1.btnAntibiotics.Caption = "科室验证";
            sysToolBar1.BtnPrint.Caption = "合并打印";
            sysToolBar1.BtnSinglePrint.Caption = "打印(F2)";
            sysToolBar1.BtnSinglePrint.Tag = "F2";
            sysToolBar1.btnReturn.Caption = "取消(Esc)";
            sysToolBar1.btnReturn.Tag = "Escape";
            sysToolBar1.BtnPrintBatch.Caption = "批量打印";
            sysToolBar1.BtnQualityOut.Caption = "集中打印";
            sysToolBar1.BtnQuickEntry.Caption = "打印完成";
            sysToolBar1.BtnResultView.Caption = "查看电子病历";
            sysToolBar1.BtnUndo.Caption = "取消已阅读";
            sysToolBar1.BtnQualityAudit.Caption = "读卡并查询";
            sysToolBar1.BtnQuickEntry.Enabled = false;

            if (UserInfo.isAdmin || selPar.EnbState)
                this.gcPar.ContextMenuStrip = this.contextMenuLeft;

            setColumnSort();

            string stringDefaultQueryInterval = ConfigHelper.GetSysConfigValueWithoutLogin("Select_DefaultQueryInterval");

            int.TryParse(stringDefaultQueryInterval, out intDefaultQueryInterval);

            this.deStart.EditValue = DateTime.Now.AddDays(1 - intDefaultQueryInterval).Date;//默认时间
            this.deEnd.EditValue = DateTime.Now.Date.AddDays(1).AddSeconds(-1);

            //启用流水号查询功能
            SerialNumber = ConfigHelper.GetSysConfigValueWithoutLogin("Select_SerialNumber") == "是";
            if (SerialNumber)
            {
                plDate.Height = 81;
                plGrid.Height = 100;
            }

            dtItr = CacheClient.GetCache<EntityDicInstrument>();
            bindingSourceItr.DataSource = dtItr;

            if (selPar.Dep_incode != null && selPar.Dep_incode != "" && selPar.Dep_incode != "-1")
            {
                List<string> listDeptCode = new List<string>(selPar.Dep_incode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                if (listDeptCode.Count > 0)
                {
                    lueDepId.SetFilter(lueDepId.getDataSource().FindAll(i => listDeptCode.Contains(i.DeptCode)));
                }
            }

            #region Obsolete
            SearchCDR = ConfigHelper.GetSysConfigValueWithoutLogin("InpatientsReportCDR") == "是";
            //报告查询流水号别名
            string strPidName = ConfigHelper.GetSysConfigValueWithoutLogin("Repselect_PidName");
            //开启报告解读功能
            ReportSumToolStripMenuItem.Visible = ConfigHelper.GetSysConfigValueWithoutLogin("Interpretation_Report") == "是";


            PatientIDNameConfirm = ConfigHelper.GetSysConfigValueWithoutLogin("PatientIDNameConfirm");

            //若病人ID配置不是为"不统一设置"，则修改为配置的名称
            if (!string.IsNullOrEmpty(PatientIDNameConfirm) && PatientIDNameConfirm != "不统一设置")
            {
                this.labelPatID.Text = PatientIDNameConfirm;
            }
            #endregion
        }

        #endregion

        

        private void DownLoadZYBarcode()
        {
            string result = "";
            if (File.Exists(selPar.Path))
                result = File.ReadAllText(selPar.Path);

            if (string.IsNullOrEmpty(result))
            {
                MessageDialog.Show("参数传递出错");
                return;
            }
            string[] parm = result.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            //读取参数
            if (parm == null || parm.Length < 3)
            {
                MessageDialog.Show("参数传递出错");
                return;
            }
            if (parm.Length >= 7)
            {
                string oriID = parm[6];
                if (oriID == "-1")
                {
                    this.lue_ori.valueMember = null;
                }
                else
                {
                    this.lue_ori.valueMember = oriID;
                }

            }

            if (selPar.Dep_incode != parm[1] || selPar.Pat_in_no != parm[2])
            {
                selPar.Dep_incode = parm[1].Replace('&', ',');
                if (parm[2].Trim() != "")
                {
                    selPar.Pat_in_no = parm[2];
                    if (parm[2].Trim() != "-1")
                        txt_in_no.EditValue = selPar.Pat_in_no;
                    else
                        txt_in_no.EditValue = null;

                }
            }
            if (!string.IsNullOrEmpty(selPar.Dep_incode))
            {
                List<string> listDeptCode = new List<string>(selPar.Dep_incode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                List<EntityDicPubDept> listDep = CacheClient.GetCache<EntityDicPubDept>();
                List<EntityDicPubDept> dtDep = CacheClient.GetCache<EntityDicPubDept>();
                List<EntityDicPubDept> drDepId = listDep.FindAll(w => listDeptCode.Contains(w.DeptCode));
                if (drDepId.Count > 0)
                {
                    switch (CanQueryOtherDeptReport)
                    {
                        case "无限制":
                            break;
                        case "当前科室":
                            lueDepId.Readonly = true;
                            break;
                        case "当前病区":
                            lueDepId.dtSource = lueDepId.dtSource.Where(i => i.DeptParentId == selPar.Dep_incode && i.DeptDelFlag != "1").ToList();
                            lueDepId.LoadDataSource(lueDepId.dtSource);
                            break;
                        default:
                            break;
                    }
                    lueDepId.valueMember = drDepId[0].DeptCode;
                    lueDepId.displayMember = drDepId[0].DeptName.ToString();
                    lueDepId.SetFilter(drDepId);
                }
                else
                {
                    lueDepId.SetFilter(new List<EntityDicPubDept>());
                }

                //中山三院允许参数为-1时加载全部科室
                if (parm[1] == "-1" && ConfigHelper.GetSysConfigValueWithoutLogin("HospitalName") == "中山三院")
                {
                    lueDepId.LoadDataSource(dtDep);
                }

                //默认的查询间隔
                int intDefaultQueryInterval = 3;

                try
                {
                    string stringDefaultQueryInterval = ConfigHelper.GetSysConfigValueWithoutLogin("Select_DefaultQueryInterval");

                    if (selPar.EnbState)
                    {
                        //查询：住院查询客户端默认间隔(天)
                        string strHospital = ConfigHelper.GetSysConfigValueWithoutLogin("Select_HospitalDefaultQueryInterval");
                        if (strHospital.Trim() != string.Empty)
                        {
                            stringDefaultQueryInterval = strHospital;
                        }
                    }

                    int.TryParse(stringDefaultQueryInterval, out intDefaultQueryInterval);
                    if (intDefaultQueryInterval == 0)
                    {
                        intDefaultQueryInterval = 1;
                    }


                }
                catch (Exception ex)
                {
                    Logger.LogException("获取默认查询间隔", ex);
                }

                this.deStart.EditValue = DateTime.Now.AddDays(1 - intDefaultQueryInterval).Date;//默认时间

                this.deEnd.EditValue = DateTime.Now.Date.AddDays(1).AddSeconds(-1);

                string strPatId = string.Empty;

                if (parm.Length >= 4 && parm[2] != "-1")
                    strPatId = parm[2];
                txtName.EditValue = null;//清空姓名
                if (!string.IsNullOrEmpty(strPatId))
                {
                    SelectPatients(strPatId, true);
                }
            }
        }

        #region CheckBoxOnGridHeader 全选按钮

        void gridViewPatientList_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column != null && e.Column.Name == "col_selected")
            {
                e.Info.InnerElements.Clear();
                e.Painter.DrawObject(e.Info);
                DrawCheckBoxOnHeader(e.Graphics, e.Bounds, bGridHeaderCheckBoxState);

                e.Handled = true;
            }
        }

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

            Point pt = this.gridViewPatientList.GridControl.PointToClient(Control.MousePosition);
            GridHitInfo info = this.gridViewPatientList.CalcHitInfo(pt);
            if (info.InColumn && info.Column.Name == "col_selected")
            {
                bGridHeaderCheckBoxState = !bGridHeaderCheckBoxState;
                this.gridViewPatientList.InvalidateColumnHeader(this.gridViewPatientList.Columns["col_selected"]);
                SelectAllPatientInGrid(bGridHeaderCheckBoxState);
            }
        }

        private void SelectAllPatientInGrid(bool selectAll)
        {
            for (int i = 0; i < this.gridViewPatientList.RowCount; i++)
            {
                ((entity.EntityPidReportMain)this.gridViewPatientList.GetRow(i)).PatSelect = bGridHeaderCheckBoxState;
            }
            gcPar.RefreshDataSource();
        }
        #endregion



        private void SelectPatients(string strPatId, bool isAuto)
        {
            try
            {
                prpPar.Focus();

                bool isSortByCombine =
                 ConfigHelper.GetSysConfigValueWithoutLogin("Repselect_OrderByPatInNoAndCombineGroup") == "是";
                if (string.IsNullOrEmpty(lueDepId.displayMember) && labelPatID.Text == labelPatIDText && string.IsNullOrEmpty(txt_in_no.Text) && string.IsNullOrEmpty(txtName.Text))
                {
                    lis.client.control.MessageDialog.Show(string.Format("请选择科室或者输入{0}或者输入姓名", labelPatIDText), "提示");
                    return;
                }
                EntityAnanlyseQC query = GetEntity(strPatId, isAuto);
                if (query == null)
                {
                    return;
                }
                query.CombineFlag = isSortByCombine ? "1" : "0";
                query.SearchType = "Lis";
                query.CanSearchOuterReport = CanSearchOuterReport;
                //反序列化
                listEntPatients = GetPatientAllBuff(query, this.deEnd.DateTime.ToString(), this.deEnd.DateTime.ToString());
                selPar.State = 0;

                if (selPar.EnbState)//如果是独立调用时,查询前清空报表
                {
                    if (selPar.Xr != null && selPar.Xr.DataSource != null)
                    {
                        try
                        {
                            ((DataSet)selPar.Xr.DataSource).Tables[0].Clear();
                            selPar.Xr.CreateDocument();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                gridViewPatientList.FocusedRowChanged -= new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
                DataTable par = new DataTable();

                if (Select_EnableMutliPatQuery && par.Rows.Count > 0 && !isAuto)
                {
                    FrmPatSelector selector = new FrmPatSelector(par);

                    DialogResult dr = selector.ShowDialog();

                    if (dr == DialogResult.Yes)
                    {

                        StringBuilder txt_in_no_Sql = new StringBuilder();

                        foreach (string patInNo in selector.m_Retlist)
                        {
                            if (txt_in_no_Sql.Length > 0)
                            {
                                txt_in_no_Sql.Append(",");
                            }

                            if (!string.IsNullOrEmpty(patInNo))
                            {
                                txt_in_no_Sql.AppendFormat("'{0}'", patInNo);
                            }
                        }

                        if (txt_in_no_Sql.Length == 0) return;

                        DataTable newPar = par.Clone();

                        DataRow[] rows = par.Select(string.Format("pat_in_no in ({0}) and pat_flag in (2,4)", txt_in_no_Sql));

                        foreach (DataRow row in rows)
                        {
                            DataRow newRow = newPar.NewRow();
                            newRow.ItemArray = row.ItemArray;
                            newPar.Rows.Add(newRow);
                        }
                        bsPar.DataSource = newPar;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    bsPar.DataSource = listEntPatients;
                }
                //添加系统配置：报告查询信息按照科室和住院号排序
                if (ConfigHelper.GetSysConfigValueWithoutLogin("Repselect_OrderByDepInNo") == "是")
                {
                    bsPar.DataSource = listEntPatients.OrderBy(i => i.PidDeptName).ThenBy(i => i.PidInNo).ToList();

                }
                else if (ConfigHelper.GetSysConfigValueWithoutLogin("Repselect_OrderByNameRepData") == "是")
                {
                    //系统配置：报告查询信息按病人姓名和报告时间倒序排序
                    bsPar.DataSource = (from x in listEntPatients
                                        orderby x.PidName, x.RepReportDate descending
                                        select x).ToList();
                }
                else if (isSortByCombine)
                {
                    bsPar.DataSource = (from x in listEntPatients
                                        orderby x.PidInNo, x.ComPriId, x.RepReportDate descending
                                        select x).ToList();
                }
                else
                {
                    bsPar.DataSource = (from x in listEntPatients orderby x.PidInNo, x.RepReportDate descending select x).ToList();
                }
                setCountTest();
                if (!selPar.EnbState)
                {
                    sysToolBar1.btnReturn.Enabled = true;
                    sysToolBar1.BtnSearch.Enabled = false;

                    plBottom.Visible = true;

                    panPar.Visible = false;
                    plBottom.Dock = DockStyle.Fill;
                    if (selPar.Iselect.DepIdFilter.Length > 0)
                        plGrid.Visible = false;
                }
                gridViewPatientList.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
                showReport();
            }
            finally
            {
                try
                {
                    if (!selPar.EnbState) //主程序调用时按流水号排序
                    {
                        List<EntityPidReportMain> listFilter = new List<EntityPidReportMain>();
                        listFilter = bsPar.DataSource as List<EntityPidReportMain>;
                        if (listFilter != null && listFilter.Count > 0)
                            bsPar.DataSource = listFilter.OrderBy(i => i.RepSerialNum.Length).ThenBy(i => i.RepSerialNum).ToList();
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
        }

        /// <summary>
        /// 获取压缩后的病人信息
        /// </summary>
        /// <param name="query"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        private List<EntityPidReportMain> GetPatientAllBuff(EntityAnanlyseQC query, string dateStart, string dateEnd)
        {
            MemoryStream stream = new MemoryStream();
            byte[] buffer = proxy.Service.GetPatientsListAllBuffer(query, dateStart, dateEnd);
            //解压压缩流
            byte[] bytes = Compression.InflateData(buffer);
            stream = new MemoryStream(bytes);
            IFormatter formatter = new BinaryFormatter();
            //反序列化
            List<entity.EntityPidReportMain> listEntPatients = (List<EntityPidReportMain>)formatter.Deserialize(stream);
            return listEntPatients;
        }

        /// <summary>
        /// 设置查询界面
        /// </summary>
        private void setPanlVisible()
        {
            plSid.Visible = selPar.Iselect.PlSidVisible;

            plOperer.Visible = selPar.Iselect.plOpererVisible;

            plOrder.Visible = selPar.Iselect.PlOrderVisible;

            plDepart.Visible = selPar.Iselect.PlDepartVisible;

            plIdType.Visible = selPar.Iselect.PlIdTypeVisible;

            plId.Visible = selPar.Iselect.PlIdVisible;

            plName.Visible = selPar.Iselect.PlNameVisible;

            plCtype.Visible = selPar.Iselect.PlCtypeVisible;

            plOrigin.Visible = selPar.Iselect.PlOriginVisible;

            plBarCode.Visible = selPar.Iselect.PlBarCodeVisible;

            //是否启用外部报告单查询
            if (CanSearchOuterReport)
            {
                plOutRep.Visible = true;
            }


            //系统配置：报告查询增加[床号]查询条件
            if (ConfigHelper.GetSysConfigValueWithoutLogin("Repselect_AddWhereForBed") == "是")
            {
                plBed.Visible = selPar.Iselect.plBedVisible;
            }

            //系统配置：报告查询增加[诊断]查询条件
            if (ConfigHelper.GetSysConfigValueWithoutLogin("Repselect_AddWhereForDiag") == "是")
            {
                panelPatDiag.Visible = selPar.Iselect.plDiagVisible;
            }

            //系统配置：报告查询[流水号]条件(格式:字段,命令id)
            if (!SerialNumber && !string.IsNullOrEmpty(ConfigHelper.GetSysConfigValueWithoutLogin("Repselect_AddWhereForPid")))
            {

                string strRepselect_AddWhereForPid = ConfigHelper.GetSysConfigValueWithoutLogin("Repselect_AddWhereForPid");
                string[] tempSpl = strRepselect_AddWhereForPid.Split(new char[] { ',' }, StringSplitOptions.None);
                if (tempSpl.Length >= 1)//流水号条件对应查询的字段名称设置,默认pat_pid
                {
                    if (tempSpl[0] == "pat_pid" || tempSpl[0] == "pat_social_no" || tempSpl[0] == "pat_in_no"
                        || tempSpl[0] == "pat_emp_id")
                    {
                        SelColNameForplPid = tempSpl[0];
                    }
                }
                if (tempSpl.Length >= 2)//第三方接口,命令ID设置,如果为空,则不调用
                {
                    int tempCmdId = 0;
                    if (int.TryParse(tempSpl[1], out tempCmdId))
                    {
                        SelCmdIDForplPid = tempSpl[1];
                    }
                }
            }

            plInpatientMessage.Visible = selPar.Iselect.PlInpatientMessageVisible;
            plYQ.Visible = true;

            if (selPar.EnbState)
            {
                panPar.Dock = selPar.Iselect.PanParDock;

                panPar.Height = 260; //原450
            }

            plBottom.Dock = selPar.Iselect.PlBottomDock;

            plBottom.Visible = selPar.Iselect.PlBottomVisible;

            gridColumn12.Visible = selPar.Iselect.SelectColumnsVisible;
            if (this.EnableCardReader)
            {
                sysToolBar1.BtnImport.Caption = "读卡并查询";
                sysToolBar1.BtnAdd.Caption = "旧报告查询";
                List<string> btnList = new List<string>();
                for (int i = 0; i < selPar.Iselect.ToolButton.Length; i++)
                {
                    if (i == 1)
                    {
                        btnList.Add(sysToolBar1.BtnImport.Name);
                    }
                    btnList.Add(selPar.Iselect.ToolButton[i]);

                    if (UserInfo.GetSysConfigValue("Lab_ReportBrowse") == "是")
                    {
                        btnList.Add(sysToolBar1.btnBrowse.Name);
                        sysToolBar1.btnBrowse.Caption = "病历浏览";
                    }
                    if (UserInfo.GetSysConfigValue("Repselect_CombineClassPrint") == "是"
                        && (i == selPar.Iselect.ToolButton.Length - 2))
                    {
                        btnList.Add(sysToolBar1.BtnBCPrint.Name);
                        sysToolBar1.BtnBCPrint.Caption = "按项目分类打印";
                    }
                }

                
               
                    sysToolBar1.SetToolButtonStyle(btnList.ToArray(), new string[] { "F1", "F11", "", "", "F12" });

                
            }
            else
            {
                sysToolBar1.BtnAdd.Caption = "旧报告查询";
                List<string> btnList = new List<string>();
                for (int i = 0; i < selPar.Iselect.ToolButton.Length; i++)
                {

                    btnList.Add(selPar.Iselect.ToolButton[i]);
                    if (UserInfo.GetSysConfigValue("Lab_ReportBrowse") == "是")
                    {
                        btnList.Add(sysToolBar1.btnBrowse.Name);
                        sysToolBar1.btnBrowse.Caption = "病历浏览";
                    }
                    if (UserInfo.GetSysConfigValue("Repselect_CombineClassPrint") == "是"
                        && (i == selPar.Iselect.ToolButton.Length - 2))
                    {
                        btnList.Add(sysToolBar1.BtnBCPrint.Name);
                        sysToolBar1.BtnBCPrint.Caption = "按项目分类打印";
                    }
                }

                    sysToolBar1.SetToolButtonStyle(btnList.ToArray(), new string[] { "F1", "F11", "", "F12" });

                
            }

            for (int j = 0; j < panPar.Controls.Count; j++)
            {
                if (panPar.Controls[j] is Panel)
                {
                    Panel pltempItem = panPar.Controls[j] as Panel;
                    if (pltempItem.Visible)
                    {
                        pltempItem.Margin = new System.Windows.Forms.Padding(1);
                        pltempItem.Refresh();
                    }
                }
            }

        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnSearchClicked(object sender, EventArgs e)
        {
            SelectPatients(string.Empty, false);
        }

        private void resetCondition()
        {
            deStart.EditValue = DateTime.Now.AddDays(1 - intDefaultQueryInterval).Date;//默认时间
            deEnd.EditValue = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            txtYBStart.Text = string.Empty;
            txtYBEnd.Text = string.Empty;
            txtXHStart.Text = string.Empty;
            txtXHEnd.Text = string.Empty;
            lueDepId.displayMember = string.Empty;
            lueDepId.valueMember = string.Empty;
            lue_no_id.displayMember = string.Empty; ;
            lue_no_id.valueMember = string.Empty;
            txt_in_no.Text = string.Empty;
            txtName.Text = string.Empty;
            lue_Wtype.displayMember = string.Empty;
            lue_Wtype.valueMember = string.Empty;
            lue_ori.displayMember = string.Empty;
            lue_ori.valueMember = string.Empty;
            txtBarcodeId.Text = string.Empty;
            txtPatTel.Text = string.Empty;
            txtPatBed.Text = string.Empty;
        }


        private void setCountTest()
        {
            if (bsPar.DataSource == null) return;
            List<entity.EntityPidReportMain> listPat = bsPar.DataSource as List<entity.EntityPidReportMain>;

            if (listPat != null)
            {
                lblAll.Text = listPat.Count.ToString();
                lblPrint.Text = listPat.FindAll(i => i.RepStatus.ToString() == "4").Count.ToString();
                lblReport.Text = listPat.FindAll(i => i.RepStatus.ToString() == "2").Count.ToString();
            }
        }
        List<string> listItrId;

        /// <summary>
        /// 得到查询实体
        /// </summary>
        /// <param name="strPatId"></param>
        /// <param name="isAuto"></param>
        /// <returns></returns>
        private EntityAnanlyseQC GetEntity(string strPatId, bool isAuto)
        {
            //if (selPar.EnbState)
            EntityAnanlyseQC query = new EntityAnanlyseQC();

            query.SearchOuterInterfaceMode = SearchOuterReportInterfaceMode;
            query.SearchCDR = SearchCDR;

            if (this.deStart.EditValue == null || this.deEnd.EditValue == null)
            {
                lis.client.control.MessageDialog.Show("请输入起始日期和结束日期！", "提示");
                return null;
            }

            System.TimeSpan ts = Convert.ToDateTime(deEnd.EditValue).Subtract(Convert.ToDateTime(deStart.EditValue));
            int day = ts.Days;

            //根据系统配置获能查询的最大区间，单位：天
            string strConfigInterval = UserInfo.GetSysConfigValue("Select_MaxQueryInterval");

            int intConfigInterval = 60;//默认60天

            int.TryParse(strConfigInterval, out intConfigInterval);

            if (intConfigInterval != 0 && (day < 0 || day > intConfigInterval))
            {
                if (string.IsNullOrEmpty(this.txt_in_no.Text.Trim()) && string.IsNullOrEmpty(this.txtName.Text.Trim()))
                {
                    lis.client.control.MessageDialog.Show(string.Format("日期范围超过{0}天需要输入[病人姓名]或[病人ID]且结束日期大于开始日期！", intConfigInterval), "提示");
                    return null;
                }
            }

            query.EnbState = selPar.EnbState;
            query.IsAuto = isAuto;
            if (selPar.EnbState)//如果是住院调用
            {
                if (txt_in_no.Text.Trim() == "" && txtName.Text.Trim() == "" && (lueDepId.valueMember == null
                    || lueDepId.valueMember.Trim() == "") && (lueDepId.displayMember == null || lueDepId.displayMember.Trim() == "") && strPatId == string.Empty)
                {
                    //lis.client.control.MessageDialog.Show("请输入查询条件！", "提示");
                    return null;
                }
                else if (Select_EnableMutliPatQuery && !isAuto)
                {
                }
                else
                {
                    query.EnableMutliPatQuery = Select_EnableMutliPatQuery;
                }

                if (Query_FilterSpecPatients)
                {
                    query.IsFilterSpecPat = true;
                }
            }
            //************************************************************************************//
            //如果不是住院调用的话，判断当前用户是否有权限操作此窗体
            else
            {
                if (UserInfo.HaveFunctionByCode("dcl.client.resultquery.canLookNotAuditReport"))
                {
                    query.IsCanLookNotAuditReport = true;
                }
                else
                {
                    query.IsCanLookNotAuditReport = false;
                }

                query.IsNotOutlink = ConfigHelper.IsNotOutlink();
                if (Query_FilterSpecPatients && !ConfigHelper.IsNotOutlink())
                {
                    query.IsFilterSpecPat = true;
                }
                listItrId = new List<string>();
                if (!string.IsNullOrEmpty(selectDict_Instrmt1.valueMember))
                {
                    listItrId.Add(selectDict_Instrmt1.valueMember);
                }
                if (!string.IsNullOrEmpty(selectDict_Instrmt2.valueMember))
                {
                    listItrId.Add(selectDict_Instrmt2.valueMember);
                }
                query.ListItrId = listItrId;

                if (Query_FilterSpecPatients && ConfigHelper.IsNotOutlink())
                {
                    if (!UserInfo.HaveFunctionByCode("FrmCombineModeSel_GGALL"))
                    {
                        query.IsGGALL = false;
                        string con = string.Empty;
                        if (UserInfo.HaveFunctionByCode("FrmCombineModeSel_GGBJDX"))
                        {
                            query.GGBJDX = true;
                        }
                        if (UserInfo.HaveFunctionByCode("FrmCombineModeSel_GGGWU"))
                        {
                            query.GGGWU = true;
                        }
                        if (UserInfo.HaveFunctionByCode("FrmCombineModeSel_GGTD"))
                        {
                            query.GGTD = true;
                        }
                        if (UserInfo.HaveFunctionByCode("FrmCombineModeSel_GGGR"))
                        {
                            query.GGGR = true;
                        }
                        if (UserInfo.HaveFunctionByCode("FrmCombineModeSel_GGGB"))
                        {
                            query.GGGB = true;
                        }

                    }

                }
            }

            String strSelectTime = ConfigHelper.GetSysConfigValueWithoutLogin("SelectTime");
            query.StrSelectTime = strSelectTime;

            query.DateType = cbDateType.Text;
            

            if (selPar.EnbState)
            {
                string strSelectType = ConfigHelper.GetSysConfigValueWithoutLogin("Repselect_SelectTimeType");
                if (strSelectType.Trim() != string.Empty)
                    query.StrSelectTime = strSelectType;
            }

                if (this.deStart.EditValue != null)
                {
                    query.DateStart = Convert.ToDateTime(deStart.EditValue);
                }
                if (this.deEnd.EditValue != null)
                {
                    query.DateEnd = Convert.ToDateTime(deEnd.EditValue);
                }

            query.listSid = new List<EntitySid>();
            EntitySid sid = new EntitySid();
            if (this.txtYBStart.EditValue.ToString() != "")
            {
                if (Convert.ToInt32(txtYBStart.EditValue) > 0)
                {
                    sid.StartSid = Convert.ToInt32(txtYBStart.EditValue);
                }
            }
            //申请医生
            if (lueDoctor.valueMember != null && lueDoctor.valueMember.Trim() != "")
            {
                query.PatDocName = lueDoctor.popupContainerEdit1.Text.Trim();
            }

            //体检单位
            if (txtEmpCompany.EditValue != null && txtEmpCompany.EditValue.ToString() != "")
            {
                if (txtEmpCompany.EditValue.ToString() != "")
                {
                    query.PatEmpCompanyName = txtEmpCompany.EditValue.ToString();
                }
            }

            if (lueItem.displayMember != null && lueItem.displayMember.Trim() != "")
            {
                query.PatCName = lueItem.displayMember;
                query.PatComId = lueItem.valueMember;
            }
            if (this.txtYBEnd.EditValue.ToString() != "")
            {
                if (Convert.ToInt32(txtYBEnd.EditValue) > 0)
                {
                    sid.EndSid = Convert.ToInt32(txtYBEnd.EditValue);
                }
            }
            query.listSid.Add(sid);

            query.listSort = new List<EntitySortNo>();
            EntitySortNo sortNo = new EntitySortNo();
            if (this.txtXHStart.EditValue.ToString() != "")
            {
                if (Convert.ToInt32(txtXHStart.EditValue) > 0)
                {
                    sortNo.StartNo = Convert.ToInt32(txtXHStart.EditValue);

                }
            }
            if (this.txtXHEnd.EditValue.ToString() != "")
            {
                if (Convert.ToInt32(txtXHEnd.EditValue) > 0)
                {
                    sortNo.EndNo = Convert.ToInt32(txtXHEnd.EditValue);
                }
            }
            query.listSort.Add(sortNo);

            #region 急查常规
            //if (this.txtCheckType.Text.Trim() == string.Empty || this.txtCheckType.Text == "全部")
            //{

            //}
            //else if (this.txtCheckType.Text == "常规")
            //{
            //    where += " and patients.pat_ctype = '1'";
            //}
            //else if (this.txtCheckType.Text == "急查")
            //{
            //    where += " and patients.pat_ctype = '2'";
            //}
            //else if (this.txtCheckType.Text == "溶栓")
            //{
            //    where += " and patients.pat_ctype = '4'";
            //}
            #endregion

            ///姓名
            if (txtName.EditValue != null && txtName.EditValue.ToString() != "")
            {
                if (txtName.EditValue.ToString() != "")
                {
                    if (string.IsNullOrEmpty(labQuery_PatName_SearchModel) || labQuery_PatName_SearchModel == "全模糊")
                    {
                        query.matchType = MatchType.QUANMOHU;
                    }
                    if (labQuery_PatName_SearchModel == "半模糊")
                    {
                        query.matchType = MatchType.BANMOHU;
                    }
                    if (labQuery_PatName_SearchModel == "全匹配")
                    {
                        query.matchType = MatchType.QUANPIPEI;
                    }
                    query.PatName = txtName.EditValue.ToString();
                }
            }

            ///科别
            query.DepIdFilter = selPar.Iselect.DepIdFilter;
            query.TxtInNo = txt_in_no.Text.Trim();
            if (selPar.Iselect.DepIdFilter.Length > 0)
            {
                //住院客户端，只要填写了住院号，就忽略科室
                if (txt_in_no.Text.Trim() == string.Empty)
                {
                    if (!string.IsNullOrEmpty(lueDepId.valueMember)
                        && !string.IsNullOrEmpty(lueDepId.displayMember)
                        )
                    {
                        query.DepId = lueDepId.valueMember;
                        query.PatWardId = lueDepId.valueMember;
                        query.PatDepName = lueDepId.displayMember;
                    }
                    else if (lueDepId.popupContainerEdit1.Text.ToString().Length > 0)
                    {
                        query.PatDepName = lueDepId.popupContainerEdit1.Text.ToString();
                    }

                }
            }
            else
            {
                if (!string.IsNullOrEmpty(lueDepId.valueMember)
                   && !string.IsNullOrEmpty(lueDepId.displayMember)
                   )
                {
                    query.DepId = lueDepId.valueMember;
                    query.PatWardId = lueDepId.valueMember;
                    query.PatDepName = lueDepId.displayMember;
                }
                else if (lueDepId.popupContainerEdit1.Text.ToString().Length > 0)
                {
                    query.PatDepName = lueDepId.popupContainerEdit1.Text.ToString();
                }
            }
            ///ID类型
            if (lue_no_id.valueMember != null && lue_no_id.valueMember != "")
            {
                query.PatNoId = lue_no_id.valueMember.ToString();
            }
            //检验者(录入人)
            if (selectInOper.valueMember != null && selectInOper.valueMember != "")
            {
                query.PidCheckUserId = selectInOper.valueMember.ToString();
            }

            //一审人
            if (selectAuditOper.valueMember != null && selectAuditOper.valueMember != "")
            {
                query.PidAuditUserId = selectAuditOper.valueMember.ToString();
            }

            ///病人ID
            if (txt_in_no.EditValue != null && txt_in_no.EditValue.ToString() != "")
            {
                if (txt_in_no.Text != null && txt_in_no.Text.StartsWith("#"))
                    txt_in_no.Text = txt_in_no.Text.Replace("#", "");
                this.txt_in_no.Text = ConverterCreator.Instance.Converter.Convert(this.txt_in_no.Text.Trim());

                string txt_in_noStr = this.txt_in_no.Text;
                int AddZeroNum = 0;
                int.TryParse(patInNoAutoAddZeroNum, out AddZeroNum);

                string strCis_yz_id = "";//记录-要查询的申请单号
                StringBuilder txt_in_no_Sql = new StringBuilder();
                string[] in_no_list;
                if (Select_EnableMutliPatQuery)
                {
                    in_no_list = txt_in_noStr.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                }
                else
                {
                    in_no_list = txt_in_noStr.Split(new char[] { '|', ',' }, StringSplitOptions.RemoveEmptyEntries);
                }
                foreach (string in_no in in_no_list)
                {
                    string patInNo = "";
                    if (in_no.Contains("&") && !Select_EnableMutliPatQuery)
                    {
                        if (!in_no.StartsWith("&"))
                        {
                            patInNo = in_no.Substring(0, in_no.IndexOf("&"));//取&前内容为病历号信息
                        }

                        if (!in_no.EndsWith("&"))
                        {
                            //取&后内容为申请单号信息,默认仅获取最后一个申请单号
                            strCis_yz_id = in_no.Substring(in_no.LastIndexOf("&") + 1, in_no.Length - (in_no.LastIndexOf("&") + 1));
                        }
                    }
                    else
                    {
                        patInNo = in_no;
                    }

                    if (AddZeroNum > 0 && lue_no_id.valueMember != "110" && lue_no_id.valueMember != "111")
                    {
                        patInNo = in_no.PadLeft(AddZeroNum, '0');
                    }
                    if (txt_in_no_Sql.Length > 0)
                    {
                        txt_in_no_Sql.Append(",");
                    }

                    if (!string.IsNullOrEmpty(patInNo))
                    {
                        txt_in_no_Sql.AppendFormat("'{0}'", patInNo);
                    }
                }

                if (!string.IsNullOrEmpty(strCis_yz_id))//如果申请单号内容不为空,则用申请单号查询
                {
                    query.StrCisYzId = strCis_yz_id;
                }

                query.TxtInNoSql = txt_in_no_Sql.ToString();
                if (txt_in_no_Sql != null && !string.IsNullOrEmpty(txt_in_no_Sql.ToString()))
                {
                    #region 用病历号查询

                    if (selPar.EnbState)//流水号保存在pat_pid
                    {
                        string column = ConfigHelper.GetSysConfigValueWithoutLogin("InpatientsReportSelectColumn");
                        //系统配置：住院检验报告查询接口只用一个字段
                        string isOnlyoneSelColumn = ConfigHelper.GetSysConfigValueWithoutLogin("InpatientsReportOnlyoneSelColumn");
                        query.isOnlyOneSelColumn = isOnlyoneSelColumn == "是";
                        query.SelectColumn = column;
                        if (!string.IsNullOrEmpty(column))
                        {
                            if (isOnlyoneSelColumn == "是")
                            {
                                query.SelectColumn = column;

                            }
                            else
                            {
                            }
                        }
                        else
                        {
                            query.PatInNo = txt_in_no_Sql.ToString();
                        }
                    }
                    else
                    {
                        query.HospitalInterfaceMode = ConfigHelper.GetSysConfigValueWithoutLogin("Interface_HospitalInterfaceMode");

                        //病历号-默认-查询所用字段
                        //报告查询[病历号]所用字段

                        query.UseColumns = ConfigHelper.GetSysConfigValueWithoutLogin("Repselect_useColumns");

                        if (lue_no_id.valueMember != null && lue_no_id.valueMember != "" && lue_no_id.displayMember.IndexOf("体检") >= 0)
                            query.TempSelNoStr1 = txt_in_no_Sql.ToString();
                        else
                            query.TempSelNoStr2 = txt_in_no_Sql.ToString();
                    }
                    #endregion
                }

                #region 中山六院流水号
                //if (selPar.EnbState)//中大六院传流水号，而流水号保存在pat_pid  2010-6-17 
                //{
                //    string column = ConfigHelper.GetSysConfigValueWithoutLogin("InpatientsReportSelectColumn");

                //    if (!string.IsNullOrEmpty(column))
                //    {
                //        where += string.Format(" and (patients.{0}='{1}' or pat_in_no = '{1}')", column, txt_in_no.EditValue);
                //    }
                //    else
                //    {
                //        where += string.Format(" and (patients.pat_pid='{0}' or pat_in_no = '{0}')", txt_in_no.EditValue);
                //    }
                //}
                //else
                //{

                //    if (lue_no_id.valueMember != null && lue_no_id.valueMember != "" && lue_no_id.displayMember.IndexOf("体检") >= 0)
                //        where += string.Format(" and (patients.pat_in_no='{0}' or patients.pat_emp_id='{0}')", txt_in_no.EditValue);
                //    else
                //        where += string.Format(" and patients.pat_in_no='{0}'", txt_in_no.EditValue);
                //}
                #endregion

            }
            ///物理组别
            if (lue_Wtype.valueMember != null && lue_Wtype.valueMember != "")
            {
                query.TypeId = lue_Wtype.valueMember;
            }

            ///来源类型
            if (lue_ori.valueMember != null && lue_ori.valueMember.ToString() != "")
            {
                query.OriId = lue_ori.valueMember;
            }
            //是否有权限查看保密数据
            if (!UserInfo.HaveFunction(214))
            {
                query.EnableLookSecretData = true;
            }
            //条码号
            if (txtBarcodeId.EditValue != null && txtBarcodeId.EditValue.ToString() != "")
            {
                query.PatBarCode = txtBarcodeId.EditValue.ToString();
                query.IsKMReportDIYCon = dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("select_IsKMReportDIYCon") == "是";
            }

            //判断是否有权限查看未审核数据
            query.IsLookNaturalReport = !UserInfo.HaveFunction("dcl.client.resultquery.FrmCombineModeSel", "LookNaturalReport");

            //床号
            if (plBed.Visible && txtPatBed.EditValue != null && txtPatBed.EditValue.ToString() != "")
            {
                if (txtPatBed.EditValue.ToString() != "")
                {
                    query.PatBedNo = txtPatBed.EditValue.ToString();
                }
            }

            //诊断
            if (panelPatDiag.Visible && txtPatDiag.EditValue != null && txtPatDiag.EditValue.ToString() != "")
            {
                if (txtPatDiag.EditValue.ToString() != "")
                {
                    query.PatDiag = txtPatDiag.EditValue.ToString();
                }
            }


            //电话号码  目前滨海用作 身份证号码
            if (plTel.Visible && txtPatTel.EditValue != null && txtPatTel.EditValue.ToString() != "")
            {

                //如果用 身份证号码 查询,则排除其他条件
                query.PatTel = txtPatTel.EditValue.ToString();

                ///物理组别
                if (lue_Wtype.valueMember != null && lue_Wtype.valueMember != "")
                {
                    query.TypeId = lue_Wtype.valueMember;
                }

            }

            return query;
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnReturnClicked(object sender, EventArgs e)
        {
            sysToolBar1.BtnSearch.Enabled = true;
            sysToolBar1.btnReturn.Enabled = false;
            plBottom.Visible = false;
            panPar.Visible = true;

                if (selPar.Xr != null && selPar.Xr.DataSource != null)
                {
                    try
                    {
                        ((DataSet)selPar.Xr.DataSource).Tables[0].Clear();
                        selPar.Xr.CreateDocument();
                    }
                    catch
                    {
                    }
                }
                if (patHistory1 != null)
                    patHistory1.Reset();
                if (controlReportResultShow1 != null)
                    controlReportResultShow1.Reset();

            prpPar.SetCanExportFile(false);
        }

        /// <summary>
        /// 病人改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            showReport();
        }


        private void getReport()
        {
            lblState.Visible = true;
            mpState.Visible = true;
            lblState.Text = "正在加载数据....";
            lblState.Refresh();
            mpState.Text = "25";
            mpState.Refresh();
            entity.EntityPidReportMain patient = gridViewPatientList.GetFocusedRow() as entity.EntityPidReportMain;
            //string localPath1 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\hope\lis\xtraReport\";
            //AppDomain.CurrentDomain.BaseDirectory;
            //MessageBox.Show();
            string localPath = PathManager.ReportPath;

            EntityAnanlyseQC ananlyseQc = new EntityAnanlyseQC();
            ananlyseQc.PatId = patient.RepId;
            if (patient != null)
            {
                bool isOuterReport = patient.IsOuterReport == "1";

                string code = patient.ItrReportId;

                if (code != "")
                {
                    string where = string.Empty;
                    ananlyseQc.IsOuterReport = isOuterReport;
                    if (isOuterReport)//外部报告单
                    {
                        ananlyseQc.SearchOuterInterfaceMode = SearchOuterReportInterfaceMode;
                        ananlyseQc.PatBarCode = patient.RepBarCode;
                        ananlyseQc.IsKMReportDIYCon = ConfigHelper.GetSysConfigValueWithoutLogin("select_IsKMReportDIYCon") == "是";
                    }
                    else
                    {
                        ananlyseQc.PatCType = patient.RepCtype;

                        //特殊情况_因为现在系统一种仪器只对应一个报表类型,而细菌报表药敏/无菌涂片的格式和数据表均不同,所以根据cs_rlts表有无数据来做区分
                        //以bacilli开头。
                        if (code.IndexOf("bacilli") == 0)
                        {
                            EntityAnanlyseQC query = new EntityAnanlyseQC();
                            query.PatId = patient.RepId;

                            string strPostfix = code.Replace("bacilli", "");
                            EntityQcResultList result = proxy.Service.GetCsAndAnResult(patient.RepId);


                            if (result.listDesc.Count > 0)
                            {
                                //系统配置：细菌管理同时有药敏与无菌结果时优先药敏
                                if (UserInfo.GetSysConfigValue("BacLab_ExistsAnAndCs_SelAn") == "是"
                                    && result.listAnti.Count > 0)
                                {
                                }
                                else
                                {
                                    code = "smear" + strPostfix;
                                }
                            }
                            if (result.listAnti.Count == 0 && result.listDesc.Count == 0)
                                code = "asepsis" + strPostfix;
                        }
                    }
                    lblState.Text = "正在加载报表....";
                    lblState.Refresh();
                    mpState.Text = "50";
                    mpState.Refresh();
                    try
                    {
                        ananlyseQc._repCode = code;
                        //EntityDCLPrintData printer = proxy.Service.GetPatientReportInfo(ananlyseQc);

                        if (selPar.EnbState)//是否单独调用界面,是就提示危急值
                        {
                            if (ConfigHelper.GetSysConfigValueWithoutLogin("InPatsRepSelectUrgentNotice") == "是")
                                if (patient.RepUrgentFlag != null)
                                {
                                    if (patient.RepUrgentFlag.Value.ToString() == "1" || patient.RepUrgentFlag.Value.ToString() == "2")
                                        lis.client.control.MessageDialog.ShowAutoCloseDialog("此报告出现危急值", 2);
                                }
                        }
                        EntityDCLPrintParameter par = new EntityDCLPrintParameter();
                        par.RepId = patient.RepId;
                        par.ReportCode = code;
                        selPar.Xr = DCLReportPrint.GetXtraReportByPrintData(par);
                        prpPar.printControl1.PrintingSystem = selPar.Xr.PrintingSystem;
                        prpPar.printPreviewStaticItem3.Caption = "100%";
                        prpPar.zoomBarEditItem1.EditValue = "100%";
                        lblState.Text = "加载完成！";
                        lblState.Refresh();
                        mpState.Text = "100";
                        mpState.Refresh();
                        prpPar.printControl1.ExecCommand(DevExpress.XtraPrinting.PrintingSystemCommand.PageLayoutFacing);
                        prpPar.printControl1.ExecCommand(DevExpress.XtraPrinting.PrintingSystemCommand.PageLayoutContinuous);
                    }
                    catch (Exception ex)
                    {
                        Lib.LogManager.Logger.LogException(ex);
                    }
                }
                else
                {
                    lis.client.control.MessageDialog.Show("该仪器在字典中未设置需要使用的报表类型！", "提示");
                    return;
                }

                #region 门诊报告单调用第三方Web

                //系统配置：[门诊]报告单调用第三方Web
                if (ConfigHelper.GetSysConfigValueWithoutLogin("Repselect_MZ_OpenOtherWeb") == "是"
                    && patient.SrcName.ToString() == "门诊")
                {
                    if (File.Exists("MZReportOpenOtherWebIP.txt"))
                    {
                        //例子：http://10.1.200.72:8080/emr-query/query/showMzPatientInfo.jsp?patientId=0000575971&hisVisitNo=113477002
                        //病人id：patientId=0000575971
                        //门诊就诊id：hisVisitNo=113477002

                        //如上,那么txt内容就为如下
                        //http://10.1.200.72:8080/emr-query/query/showMzPatientInfo.jsp?patientId=#字段名称1#&hisVisitNo=#字段名称2#;字段名称1;字段名称2


                        string MZReportOpenOtherWebIP_con = File.ReadAllText("MZReportOpenOtherWebIP.txt");
                        if (MZReportOpenOtherWebIP_con.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Length >= 2)
                        {
                            string[] tempSpl = MZReportOpenOtherWebIP_con.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                            //0 为 web地址
                            //1或以上为参数字段名称
                            string otherwebip = tempSpl[0];

                            //检查是否所有参数字段都存在
                            for (int i = 1; i < tempSpl.Length; i++)
                            {
                                if (patient.RepId != null)
                                {
                                    otherwebip = otherwebip.Replace("#" + tempSpl[i] + "#", patient.RepId);
                                }
                                else
                                {
                                    Lib.LogManager.Logger.LogException("门诊报告单调用第三方Web", new Exception("表【" + "patients" + "】不存在字段【" + tempSpl[i] + "】,请修改MZReportOpenOtherWebIP.txt内容！"));
                                    break;
                                }

                                //都存在则调用第三方web
                                if (i == (tempSpl.Length - 1))
                                {
                                    try
                                    {
                                        //调用系统默认的浏览器  
                                        System.Diagnostics.Process.Start(otherwebip);
                                    }
                                    catch (Exception ex_otherwebip)
                                    {
                                        Lib.LogManager.Logger.LogException("门诊报告单调用第三方Web", ex_otherwebip);
                                        lis.client.control.MessageDialog.ShowAutoCloseDialog(ex_otherwebip.Message);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Lib.LogManager.Logger.LogException("门诊报告单调用第三方Web", new Exception("找不到文件:MZReportOpenOtherWebIP.txt"));
                    }
                }

                #endregion


            }
            lblState.Visible = false;
            mpState.Visible = false;
            gcPar.Focus();
        }

        private void getAnalysis()
        {
            patHistory1.Reset();
            entity.EntityPidReportMain patient = gridViewPatientList.GetFocusedRow() as entity.EntityPidReportMain;
            patHistory1.LoadPatHistoryFromSelect(patient.RepId, patient.RepInDate.Value);
        }

        /// <summary>
        /// 显示报表数据
        /// </summary>
        private void getPatAndResData()
        {
            //DataRow drPar = gridViewPatientList.GetFocusedDataRow();
            entity.EntityPidReportMain patient = gridViewPatientList.GetFocusedRow() as entity.EntityPidReportMain;

            if (patient.RepCtype == "旧检验数据")
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("旧检验请直接查看报告");
                xtcReport.SelectedTabPageIndex = 0;
                return;
            }

            string str_pat_id = patient.RepId;//获取ID
            DateTime patData = patient.RepInDate.Value;
            try
            {
                if (!string.IsNullOrEmpty(str_pat_id))
                {

                    DataTable dtPatientsShow = new DataTable();
                    EntityAnanlyseQC query = new EntityAnanlyseQC();
                    query.PatId = str_pat_id;//获取ID
                    query.SearchType = "Lis";
                    query.IsSingleSearch = true;
                    List<entity.EntityPidReportMain> patList = GetPatientAllBuff(query, patData.ToString(), patData.ToString());

                    if (patList != null && patList.Count > 0)
                    {

                        EntityQcResultList listQcResult = proxy.Service.GetPatientResultData(query, patData);

                        this.controlReportResultShow1.show_load_info(patList[0], listQcResult);
                    }
                    else
                    {
                        this.controlReportResultShow1.show_load_info(null, null);
                    }
                }

            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("获取数据时，遇到错误！");
            }
        }

        private void showReport()
        {
            if (gridViewPatientList.GetFocusedRow() == null) return;
            if (xtcReport.SelectedTabPageIndex == 0) getReport();
            if (xtcReport.SelectedTabPageIndex == 1) getAnalysis();
            if (xtcReport.SelectedTabPageIndex == 2) getPatAndResData();
            prpPar.SetCanExportFile(UserInfo.GetSysConfigValue("LabQuery_EnableExportReport") == "是");

            UpdateRepotLookcode();
        }

        /// <summary>
        /// 记录报告查阅者
        /// </summary>
        private void UpdateRepotLookcode()
        {
            //系统配置：取消二审时检查是否已阅读
            if ((ConfigHelper.GetSysConfigValueWithoutLogin("UndoAudit_Second_CheckLookcode") == "是" || ConfigHelper.GetSysConfigValueWithoutLogin("Query_RecordLookDate") == "是") && selPar.EnbState)
            {
                if (gridViewPatientList.GetFocusedRow() == null) return;

                //Lib.LogManager.Logger.LogInfo("se//oginID" + setLoginID);
                entity.EntityPidReportMain patient = gridViewPatientList.GetFocusedRow() as entity.EntityPidReportMain;
                string str_pat_id = patient.RepId;//获取ID
                List<string> temp_patlist = new List<string>();
                if (!string.IsNullOrEmpty(str_pat_id))
                {
                    temp_patlist.Add(str_pat_id);
                }
                else
                {
                    return;
                }
                if (string.IsNullOrEmpty(UserInfo.loginID) && string.IsNullOrEmpty(setLoginID))
                {
                    new ProxyPidReportMain().Service.UpdateRepReadUserId("nw", "科室查看", temp_patlist);
                    patient.RepReadUserId = "科室查看";

                }
                new ProxyPidReportMain().Service.UpdateRepReadUserId("nw", UserInfo.loginID ?? setLoginID, temp_patlist);
                patient.RepReadUserId = (UserInfo.loginID ?? setLoginID);

            }
        }


        /// <summary>
        /// 多打
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnPrintClicked(object sender, EventArgs e)
        {
            isClassPrint = false;
            print(false);
            //如果独立报告查询,则打印完,焦点回到病历号
            if (selPar.EnbState)
            {
                txt_in_no.Focus();
                txt_in_no.SelectAll();
            }
        }

        //删除危急值信息patId集合
        private List<string> listDeleteMsgPatId = null;
        private void print(bool BatchPrint)
        {
            sysToolBar1.Focus();
            if (bsPar.DataSource == null) return;
            List<entity.EntityPidReportMain> dtPatients = bsPar.DataSource as List<entity.EntityPidReportMain>;
            if (dtPatients == null || dtPatients.FindAll(i => i.PatSelect == true).Count <= 0)
            {
                if (isClearSelect)
                    lis.client.control.MessageDialog.Show("请选择要打印的数据！", "提示");
                return;
            }
            #region 住院打印前设置打印机

            if (selPar.EnbState)
            {
                //系统配置：住院检验报告查询打印前设置打印机
                if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("InpatientsRepSel_PrintingSet") == "是")
                {
                    sysToolBar1_BtnPrintSetClick(null, null);
                }
            }
            else
            {
                //系统配置：非住院检验报告查询打印前设置打印机
                if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("NoInpatientsRepSel_PrintingSet") == "是")
                {
                    sysToolBar1_BtnPrintSetClick(null, null);
                }
            }

            #endregion


            #region 打印前更新一下报告审核状态
            if (dtPatients != null && dtPatients.Count > 0)
            {
                List<entity.EntityPidReportMain> dtSelectPatients = dtPatients.FindAll(i => i.PatSelect == true);
                if (isClassPrint && dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Repselect_CombineClassPrint") == "是")
                    dtSelectPatients = listClass.FindAll(i => i.PatSelect == true);

                if (true)
                {
                    #region 方式一更新报告审核状态(批量in)
                    string inPatIDs = "";
                    foreach (entity.EntityPidReportMain drtemp in dtSelectPatients)
                    {
                        if (drtemp.RepStatus == null)
                        {
                            drtemp.RepStatus = 0;
                        }
                    }
                    #endregion
                }
                else
                {
                    //方式二更新报告审核状态(遍列每条)
                    foreach (entity.EntityPidReportMain drtemp in dtSelectPatients)
                    {
                        if (!string.IsNullOrEmpty(drtemp.RepId.ToString()))
                        {
                            string temp_pat_flag = new ProxyPidReportMain().Service.GetPatientState(drtemp.RepId);
                            if (string.IsNullOrEmpty(temp_pat_flag) || temp_pat_flag == "1" || temp_pat_flag == "0")
                            {
                                drtemp.RepStatus = string.IsNullOrEmpty(temp_pat_flag) ? 0 : Convert.ToInt32(temp_pat_flag);
                            }
                        }
                    }
                }
            }
            #endregion

            isClearSelect = true;

            List<entity.EntityPidReportMain> listReportedOrPrinted = null;

            listReportedOrPrinted = dtPatients.FindAll(i => i.PatSelect == true && (i.RepStatus.Value.ToString() == LIS_Const.PATIENT_FLAG.Reported || i.RepStatus.Value.ToString() == LIS_Const.PATIENT_FLAG.Printed) || i.RepTempFlag == "1").OrderByDescending(i => i.RepReportDate).ToList();

            if (isClassPrint && dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Repselect_CombineClassPrint") == "是")
                listReportedOrPrinted = listClass.FindAll(i => i.PatSelect == true && (i.RepStatus.Value.ToString() == LIS_Const.PATIENT_FLAG.Reported || i.RepStatus.Value.ToString() == LIS_Const.PATIENT_FLAG.Printed) || i.RepTempFlag == "1").OrderByDescending(i => i.RepReportDate).ToList();

            if (ConfigHelper.GetSysConfigValueWithoutLogin("Repselect_TwoPrintShowMessage") == "是")
            {
                List<entity.EntityPidReportMain> listPrinter = dtPatients.FindAll(i => i.PatSelect == true && (i.RepStatus.Value.ToString() == LIS_Const.PATIENT_FLAG.Printed)).OrderByDescending(i => i.RepReportDate).ToList();
                if (listPrinter != null && listPrinter.Count > 0)
                {
                    string strMes = string.Empty;
                    foreach (entity.EntityPidReportMain item in listPrinter)
                    {
                        strMes += string.Format("姓名：{0} 组合：{1}\r\n", item.PidName.ToString(), item.PidComName.ToString());
                    }

                    if (MessageDialog.Show("以下标本已打印,是否需要再次打印?\r\n {0}" + strMes, MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;
                }

            }

            #region 杏坛医院特殊需求

            //杏坛医院,特殊的报告需要到检验科打印，自助和临床只能查询预览，不能打印。 
            //针对某些仪器的报告只能预览，不能打印。 
            //系统配置：住院检验报告查询不能打印的仪器ID(id1,id2)
            string s_InpatientsReportSelect_notprintItrIDs = dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("InpatientsReportSelect_notprintItrIDs");
            if (selPar.EnbState && !string.IsNullOrEmpty(s_InpatientsReportSelect_notprintItrIDs))
            {
                string cannotprintitrIDHint = "";

                for (int tempindex = 0; tempindex < listReportedOrPrinted.Count; tempindex++)
                {
                    //遍历检查是否有不能打印的仪器id
                    if (s_InpatientsReportSelect_notprintItrIDs.Contains(listReportedOrPrinted[tempindex].RepItrId.ToString()))
                    {
                        if (cannotprintitrIDHint == "")
                        {
                            cannotprintitrIDHint = "以下报告不能打印\r\n请到检验科一楼咨询台打印\r\n\r\n";
                            cannotprintitrIDHint += string.Format("【{0}】 [{1}]", listReportedOrPrinted[tempindex].PidName, listReportedOrPrinted[tempindex].PidComName);
                        }
                        else
                        {
                            cannotprintitrIDHint += string.Format("\r\n【{0}】 [{1}]", listReportedOrPrinted[tempindex].PidName, listReportedOrPrinted[tempindex].PidComName);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(cannotprintitrIDHint))
                {
                    lis.client.control.MessageDialog.Show(cannotprintitrIDHint);
                    return;
                }
            }
            #endregion

            if (listReportedOrPrinted.Count > 0)
            {
                #region 检查设置打印人

                if ((IsOutputPrintPersonAndTime && BatchPrint) || (IsOutputPrintPersonAndTimeByPrint && !BatchPrint))
                {
                    if (string.IsNullOrEmpty(outPrintPerson) || LastPrintTime == null || DateTime.Now.AddMinutes(-2) > LastPrintTime)
                    {
                        FrmCheckPassword frm = new FrmCheckPassword();
                        if (frm.ShowDialog() == DialogResult.OK)//身份验证
                        {
                            sysToolBar1.BtnQuickEntry.Enabled = true;
                            outPrintPerson = frm.OperatorName;
                            outPrintPersonID = frm.OperatorID;
                            LastPrintTime = DateTime.Now;
                        }
                        else
                        {
                            sysToolBar1.BtnQuickEntry.Enabled = false;
                            return;
                        }
                    }
                    else
                    {
                        LastPrintTime = DateTime.Now;
                    }
                    //获取服务端中间层时间
                    outPrintTime = ServerDateTime.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                }

                #endregion

                #region 检查无条码打印

                if (Lab_EnableNoBarCodeCheck)
                {
                    StringBuilder sbPatSidWhere = new StringBuilder();

                    for (int i = 0; i < listReportedOrPrinted.Count; i++)
                    {
                        //DataRow dr2 = dr[i];
                        entity.EntityPidReportMain dr2 = listReportedOrPrinted[i];
                        if ((dr2.RepPrintFlag == null)
                        && (dr2.RepBarCode == null || string.IsNullOrEmpty(dr2.RepBarCode.ToString())))
                        {
                            string currentitrID = dr2.RepItrId.ToString();

                            if (!Lab_NoBarCodeCheckItrExpectList.Contains(currentitrID))
                            {
                                sbPatSidWhere.Append(string.Format(",姓名为:{1}样本号为:{0}", dr2.RepSid, dr2.PidName));
                            }
                        }
                    }
                    if (sbPatSidWhere.Length > 0)
                    {
                        MessageDialog.Show(sbPatSidWhere.Remove(0, 1) + " 的报告单无条码信息，需要在检验报告管理界面进行打印确认", "提示");
                        return;
                    }
                }

                #endregion


                if (Lab_ReportCodeIsNullNotAllowPrint)
                {
                    StringBuilder sbPatSidWhere = new StringBuilder();

                    for (int i = 0; i < listReportedOrPrinted.Count; i++)
                    {
                        entity.EntityPidReportMain dr2 = listReportedOrPrinted[i];
                        bool isOuterReport = dr2.IsOuterReport.ToString() == "1";
                        if (isOuterReport) continue;

                        if (dr2.RepTempFlag.ToString() == "1") continue;

                        if (string.IsNullOrEmpty(dr2.ShName) || string.IsNullOrEmpty(dr2.BgName))
                        {
                            sbPatSidWhere.Append(string.Format(",姓名为:{1}样本号为:{0}", dr2.RepSid, dr2.PidName));
                        }

                    }
                    if (sbPatSidWhere.Length > 0)
                    {
                        MessageDialog.Show(sbPatSidWhere.Remove(0, 1) + " 的报告单无审核者信息，请重新审核", "提示");
                        return;
                    }
                }

                //顺序
                int sequence = 0;

                List<EntityDCLPrintParameter> listPrtPara = new List<EntityDCLPrintParameter>();

                StringBuilder showNotPrintMsg = new StringBuilder();
                bool Query_NotPrintInterimMicReport = ConfigHelper.GetSysConfigValueWithoutLogin("Query_NotPrintInterimMicReport") == "是"; //不允许打印临时报告
                string strMicInterimReport = ConfigHelper.GetSysConfigValueWithoutLogin("Mic_InterimReport"); //临时报告仪器

                selPar.StPatId.Length = 0;
                if (isClassPrint && dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Repselect_CombineClassPrint") == "是")
                {
                    #region 组合分类打印
                    foreach (entity.EntityPidReportMain drPat in listClass)
                    {
                        bool isOuterReport = drPat.IsOuterReport.ToString() == "1";

                        if (drPat.PatSelect == true
                            && (drPat.RepStatus.Value.ToString() == LIS_Const.PATIENT_FLAG.Reported || drPat.RepStatus.Value.ToString() == LIS_Const.PATIENT_FLAG.Printed
                            || (drPat.RepTempFlag.ToString() == "1")
                                || isOuterReport))
                        {

                            string code = drPat.ItrReportId.ToString();
                            string pat_id = drPat.RepId.ToString();
                            string pat_name = drPat.PidName.ToString();
                            string pat_dep_name = drPat.PidDeptName.ToString();
                            string temp_in_no = drPat.PidInNo.ToString();
                            selPar.StPatId.Append(string.Format(",'{0}'", drPat.RepId));

                            if (code != "")
                            {
                                string where = string.Empty;
                                EntityDCLPrintParameter printPara = new EntityDCLPrintParameter();
                                printPara.RepId = pat_id;
                                if (isOuterReport)
                                {
                                    if (SearchOuterReportInterfaceMode)
                                    {
                                        string bar_code = drPat.RepBarCode.ToString();
                                        //系统配置：是否金域报表自定义连接地址
                                        if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("select_IsKMReportDIYCon") == "是")
                                        {
                                            printPara.CustomParameter.Add("F_HospSampleID", bar_code);
                                        }
                                        else
                                        {
                                            printPara.CustomParameter.Add("KM_LIS_ExtReport", bar_code);
                                        }
                                        pat_id = bar_code;
                                        printPara.ListBarId.Add(bar_code);
                                        printPara.RepId = null;
                                    }
                                    else
                                    {
                                        printPara.CustomParameter.Add("ReportInfoId", pat_id);

                                    }

                                }
                                else
                                {
                                    if (drPat.RepCtype.ToString() == "旧检验数据")
                                        printPara.CustomParameter.Add("LismainRepno", pat_id);
                                    else
                                        printPara.RepId = pat_id;
                                }
                                if (code.IndexOf("bacilli") == 0)
                                {
                                    string strPostfix = code.Replace("bacilli", "");
                                    EntityQcResultList result = proxy.Service.GetCsAndAnResult(drPat.RepId.ToString());
                                    if (result.listDesc.Count > 0)
                                    {
                                        //系统配置：细菌管理同时有药敏与无菌结果时优先药敏
                                        if (UserInfo.GetSysConfigValue("BacLab_ExistsAnAndCs_SelAn") == "是"
                                            && result.listAnti.Count > 0)
                                        {
                                        }
                                        else
                                        {
                                            code = "smear" + strPostfix;
                                        }
                                    }

                                    if (result.listAnti.Count == 0 && result.listDesc.Count == 0)
                                    {
                                        code = "asepsis" + strPostfix;
                                    }
                                }

                                printPara.ReportCode = code;
                                printPara.Sequence = sequence;
                                printPara.PatName = pat_name;
                                printPara.PatDepName = pat_dep_name;
                                listPrtPara.Add(printPara);
                                sequence++;
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    //遍历当前选中的病人(支持排序)
                    for (int i = 0; i < gridViewPatientList.RowCount; i++)
                    {
                        entity.EntityPidReportMain drPat = gridViewPatientList.GetRow(i) as entity.EntityPidReportMain;
                        bool isOuterReport = drPat.IsOuterReport.ToString() == "1";

                        if (drPat.PatSelect == true
                            && (drPat.RepStatus.Value.ToString() == LIS_Const.PATIENT_FLAG.Reported || drPat.RepStatus.Value.ToString() == LIS_Const.PATIENT_FLAG.Printed
                            || (drPat.RepTempFlag.ToString() == "1")
                                || isOuterReport))
                        {

                            string code = drPat.ItrReportId.ToString();
                            string pat_id = drPat.RepId.ToString();
                            string pat_name = drPat.PidName.ToString();
                            string pat_dep_name = drPat.PidDeptName.ToString();
                            string temp_in_no = drPat.PidInNo.ToString();
                            selPar.StPatId.Append(string.Format(",'{0}'", drPat.RepId));

                            if (Query_NotPrintInterimMicReport &&
                                !string.IsNullOrEmpty(strMicInterimReport) &&
                                drPat.RepItrId == strMicInterimReport)
                            {
                                showNotPrintMsg.Append(string.Format(",姓名为:{1},样本号为:{0}", drPat.RepSid, drPat.PidName));
                                continue;
                            }

                            if (code != "")
                            {
                                string where = string.Empty;
                                EntityDCLPrintParameter printPara = new EntityDCLPrintParameter();
                                printPara.RepId = pat_id;
                                if (isOuterReport)
                                {
                                    if (SearchOuterReportInterfaceMode)
                                    {
                                        string bar_code = drPat.RepBarCode.ToString();
                                        //系统配置：是否金域报表自定义连接地址
                                        if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("select_IsKMReportDIYCon") == "是")
                                        {
                                            printPara.CustomParameter.Add("F_HospSampleID", bar_code);
                                        }
                                        else
                                        {
                                            printPara.CustomParameter.Add("KM_LIS_ExtReport", bar_code);
                                        }
                                        pat_id = bar_code;
                                        printPara.ListBarId.Add(bar_code);
                                        printPara.RepId = null;
                                    }
                                    else
                                    {
                                        printPara.CustomParameter.Add("ReportInfoId", pat_id);

                                    }

                                }
                                else
                                {
                                    if (drPat.RepCtype.ToString() == "旧检验数据")
                                        printPara.CustomParameter.Add("LismainRepno", pat_id);
                                    else
                                        printPara.RepId = pat_id;
                                }
                                if (code.IndexOf("bacilli") == 0)
                                {
                                    string strPostfix = code.Replace("bacilli", "");
                                    EntityQcResultList result = proxy.Service.GetCsAndAnResult(drPat.RepId.ToString());
                                    if (result.listDesc.Count > 0)
                                    {
                                        //系统配置：细菌管理同时有药敏与无菌结果时优先药敏
                                        if (UserInfo.GetSysConfigValue("BacLab_ExistsAnAndCs_SelAn") == "是"
                                            && result.listAnti.Count > 0)
                                        {
                                        }
                                        else
                                        {
                                            code = "smear" + strPostfix;
                                        }
                                    }

                                    if (result.listAnti.Count == 0 && result.listDesc.Count == 0)
                                    {
                                        code = "asepsis" + strPostfix;
                                    }
                                }

                                printPara.ReportCode = code;
                                printPara.Sequence = sequence;
                                printPara.PatName = pat_name;
                                printPara.PatDepName = pat_dep_name;
                                listPrtPara.Add(printPara);

                                sequence++;

                                if (ConfigHelper.GetSysConfigValueWithoutLogin("Lab_PrintReportConfirmMsg") == "是"
                                    && drPat.RepUrgentFlag == 1
                                    && drPat.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Reported)
                                {
                                    if (listDeleteMsgPatId == null)
                                        listDeleteMsgPatId = new List<string>();
                                    listDeleteMsgPatId.Add(drPat.RepId);
                                }
                            }
                        }
                    }

                    if (showNotPrintMsg.Length > 0)
                    {
                        MessageDialog.Show(showNotPrintMsg.Remove(0, 1) + " 为临时报告，不允许打印\r\n【确定】：打印【可打印】的报告", "提示");
                    }
                }
                if (listPrtPara.Count > 0)
                {
                    if (ReportSetting.ContinuousPrinting)
                    {
                        try
                        {

                            List<EntityDCLPrintParameter> listPara1 = new List<EntityDCLPrintParameter>();
                            List<EntityDCLPrintParameter> listPara2 = new List<EntityDCLPrintParameter>();
                            List<EntityDCLPrintParameter> listBatchPara = new List<EntityDCLPrintParameter>();

                            if (ConfigHelper.GetSysConfigValueWithoutLogin("checkBatchPrintReqseq") == "是")
                            {
                                if (listPrtPara != null && listPrtPara.Count > 0)
                                {
                                    string strBatchSamID = ConfigHelper.GetSysConfigValueWithoutLogin("Repselect_BatchPrintSamID");
                                    //获取每份报告单有代表性的组合的序号
                                    foreach (EntityDCLPrintParameter p_eypd in listPrtPara)
                                    {
                                        string str_p_id = p_eypd.RepId;
                                        int p_value = 99999;//默认序号99999
                                        if (!string.IsNullOrEmpty(str_p_id))
                                        {
                                            ProxyCombModelSel proxy = new ProxyCombModelSel();
                                            List<EntityPidReportDetail> listDetail = proxy.Service.GetCombineSeqForPatID(str_p_id);
                                            if (listDetail.Count > 0)
                                            {
                                                if (!int.TryParse(listDetail[0].ComSeq, out p_value))
                                                {
                                                    p_value = 99999;
                                                }
                                            }
                                            p_eypd.Sequence = p_value;
                                            if (strBatchSamID != string.Empty)
                                            {
                                                int checkType = 0;

                                                string[] strBatchSamIDs = strBatchSamID.Split(new char[] { ';' });

                                                foreach (EntityPidReportDetail item in listDetail)
                                                {
                                                    if (strBatchSamIDs[0].Contains(item.ComId))
                                                    {
                                                        checkType = 1;
                                                        break;
                                                    }
                                                    else if (strBatchSamID.Length > 1 && strBatchSamIDs[1].Contains(item.ComId))
                                                    {
                                                        checkType = 2;
                                                        break;
                                                    }
                                                }
                                                if (checkType == 1)
                                                    listPara1.Add(p_eypd);
                                                else if (checkType == 2)
                                                    listPara2.Add(p_eypd);
                                                else
                                                    listBatchPara.Add(p_eypd);
                                            }
                                        }
                                        p_eypd.Sequence = p_value;//批量打印报告顺序号
                                    }

                                    if (listPara1.Count > 0 || listPara2.Count > 0)
                                        listPrtPara = listBatchPara;

                                    //根据序号升序排序报告打印顺序
                                    listPrtPara.Sort(delegate (EntityDCLPrintParameter pdX, EntityDCLPrintParameter pdY)
                                    {
                                        if (pdX.Sequence != pdY.Sequence)
                                            return pdX.Sequence.CompareTo(pdY.Sequence);
                                        else
                                            return pdY.Sequence.CompareTo(pdX.Sequence);
                                    });
                                }
                            }
                            if (HasShowPreview)
                            {
                                if (listPara1.Count > 0)
                                {
                                    foreach (var item in listPara1)
                                    {
                                        DCLReportPrint.PrintPreview(item);
                                    }
                                }

                                if (listPara2.Count > 0)
                                {
                                    foreach (var item in listPara2)
                                    {
                                        DCLReportPrint.PrintPreview(item);
                                    }
                                }
                                foreach (var item in listBatchPara)
                                {
                                    DCLReportPrint.PrintPreview(item);
                                }
                            }
                            else
                            {
                                if (listPara1.Count > 0)
                                    DCLReportPrint.ContinuousPrint(listPara1);
                                if (listPara2.Count > 0)
                                    DCLReportPrint.ContinuousPrint(listPara2);
                                DCLReportPrint.ContinuousPrint(listPrtPara);
                                frp_PrintStart2(null, null);
                            }
                            HasShowPreview = false;
                        }
                        catch (ReportNotFoundException ex1)
                        {
                            lis.client.control.MessageDialog.Show(ex1.MSG);
                        }
                        catch (Exception ex2)
                        {

                        }
                    }
                    else
                    {
                        try
                        {
                            List<EntityDCLPrintParameter> listPara1 = new List<EntityDCLPrintParameter>();
                            List<EntityDCLPrintParameter> listPara2 = new List<EntityDCLPrintParameter>();
                            List<EntityDCLPrintParameter> listBatchPara = new List<EntityDCLPrintParameter>();

                            //如果为套打则按照报告组合序号打印
                            //系统配置:A4批量打印按报告组合序号排序
                            if (BatchPrint
                                && ConfigHelper.GetSysConfigValueWithoutLogin("checkBatchPrintReqseq") == "是")
                            {
                                if (listPrtPara != null && listPrtPara.Count > 0)
                                {
                                    string strBatchSamID = ConfigHelper.GetSysConfigValueWithoutLogin("Repselect_BatchPrintSamID");

                                    //获取每份报告单有代表性的组合的序号
                                    foreach (EntityDCLPrintParameter p_eypd in listPrtPara)
                                    {
                                        string str_p_id = p_eypd.RepId;
                                        int p_value = 99999;//默认序号99999
                                        if (!string.IsNullOrEmpty(str_p_id))
                                        {
                                            ProxyCombModelSel proxy = new ProxyCombModelSel();
                                            List<EntityPidReportDetail> listDetail = proxy.Service.GetCombineSeqForPatID(str_p_id);
                                            if (listDetail.Count > 0)
                                            {
                                                if (!int.TryParse(listDetail[0].ComSeq, out p_value))
                                                {
                                                    p_value = 99999;
                                                }
                                            }
                                            p_eypd.Sequence = p_value;
                                            if (strBatchSamID != string.Empty)
                                            {
                                                int checkType = 0;

                                                string[] strBatchSamIDs = strBatchSamID.Split(new char[] { ';' });

                                                foreach (EntityPidReportDetail item in listDetail)
                                                {
                                                    if (strBatchSamIDs[0].Contains(item.ComId))
                                                    {
                                                        checkType = 1;
                                                        break;
                                                    }
                                                    else if (strBatchSamID.Length > 1 && strBatchSamIDs[1].Contains(item.ComId))
                                                    {
                                                        checkType = 2;
                                                        break;
                                                    }
                                                }
                                                if (checkType == 1)
                                                    listPara1.Add(p_eypd);
                                                else if (checkType == 2)
                                                    listPara2.Add(p_eypd);
                                                else
                                                    listBatchPara.Add(p_eypd);
                                            }
                                        }
                                        p_eypd.Sequence = p_value;//批量打印报告顺序号
                                    }

                                    if (listPara1.Count > 0 || listPara2.Count > 0)
                                        listPrtPara = listBatchPara;

                                    //根据序号升序排序报告打印顺序
                                    listPrtPara.Sort(delegate (EntityDCLPrintParameter pdX, EntityDCLPrintParameter pdY)
                                    {
                                        if (pdX.Sequence != pdY.Sequence)
                                            return pdX.Sequence.CompareTo(pdY.Sequence);
                                        else
                                            return pdY.Sequence.CompareTo(pdX.Sequence);
                                    });
                                }
                            }
                            if (HasShowPreview)
                            {
                                if (listPara1.Count > 0)
                                {
                                    foreach (var item in listPara1)
                                    {
                                        DCLReportPrint.PrintPreview(item);
                                    }
                                }

                                if (listPara2.Count > 0)
                                {
                                    foreach (var item in listPara2)
                                    {
                                        DCLReportPrint.PrintPreview(item);
                                    }
                                }
                                foreach (var item in listBatchPara)
                                {
                                    DCLReportPrint.PrintPreview(item);
                                }
                            }
                            else
                            {
                                if (listPara1.Count > 0)
                                    DCLReportPrint.BatchPrint(listPara1);
                                if (listPara2.Count > 0)
                                    DCLReportPrint.BatchPrint(listPara2);
                                DCLReportPrint.BatchPrint(listPrtPara);
                                frp_PrintStart2(null, null);
                            }
                            HasShowPreview = false;
                        }
                        catch (ReportNotFoundException ex1)
                        {
                            lis.client.control.MessageDialog.Show(ex1.MSG);
                        }
                        catch (Exception ex2)
                        {
                            Logger.LogException("FrmCombineModeSel_Print", ex2);
                        }
                    }
                }
            }
            else
                lis.client.control.MessageDialog.Show("所选数据未报告！", "提示");
        }


        void frp_PrintStart2(object sender, FrmReportPrint.PrintEventArgs arg)
        {
            if (selPar.StPatId.Length > 0)
            {
                if (selPar.StPatId[0] == ',')
                {
                    selPar.StPatId.Remove(0, 1);
                }

                EntityAnanlyseQC annanlyseQc = new EntityAnanlyseQC();
                string OperatorID = "";//操作者id
                string OperatorName = "";//操作者名称
                string strPlace = "";//描述信息
                string strRemark = "";////备注信息--记录ip

                //如果有登陆用户,则用登陆用户作为操作者记录报告打印流程
                if (!string.IsNullOrEmpty(outPrintPerson) && (IsOutputPrintPersonAndTime || IsOutputPrintPersonAndTimeByPrint))
                {
                    annanlyseQc.OperatorID = outPrintPersonID;
                    annanlyseQc.OperatorName = outPrintPerson;
                }
                else if (UserInfo.userName != null && UserInfo.loginID != null && UserInfo.userName.Length > 0 &&
                     UserInfo.loginID.Length > 0)
                {
                    annanlyseQc.OperatorID = UserInfo.loginID;
                    annanlyseQc.OperatorName = UserInfo.userName;

                    if (!string.IsNullOrEmpty(LocalSetting.Current.Setting.Description))
                    {
                        annanlyseQc.StrPlace = LocalSetting.Current.Setting.Description;//描述信息
                    }
                }
                annanlyseQc.StrRemark = dcl.common.IPUtility.GetIP();//获取本机ip

                if (!ConfigHelper.IsNotOutlink())
                {
                    if (selPar.EnbState)
                    {
                        annanlyseQc.OperatorID = "病区打印";
                        annanlyseQc.OperatorName = "病区打印";
                    }

                    annanlyseQc.StrRemark = "检验报告查询模块打印";
                }
                annanlyseQc.PatId = selPar.StPatId.ToString();

                proxy.Service.UpdatePatFlagToPrinted(annanlyseQc);


                if (bsPar.DataSource == null) return;
                List<entity.EntityPidReportMain> patList = bsPar.DataSource as List<entity.EntityPidReportMain>;
                List<entity.EntityPidReportMain> listSelectedPat = new List<entity.EntityPidReportMain>();

                if (SerialNumber)
                    listSelectedPat = patList.FindAll(i => i.PatSelect == true);
                else
                    listSelectedPat = patList.FindAll(i => i.PatSelect == true && i.RepStatus == 2);

                foreach (entity.EntityPidReportMain dr in listSelectedPat)
                {
                    dr.RepStatus = int.Parse(LIS_Const.PATIENT_FLAG.Printed);
                    dr.PatSelect = false;
                }
                if (ConfigHelper.GetSysConfigValueWithoutLogin("Lab_PrintReportConfirmMsg") == "是"
                    && listDeleteMsgPatId != null && listDeleteMsgPatId.Count > 0)
                {
                    EntityAuditInfo audit = new EntityAuditInfo();
                    audit.UserId = UserInfo.loginID;
                    audit.UserName = UserInfo.userName;
                    audit.MsgAffirmType = "3";//1-自动确认 2-手工确认 2-报告单打印确认
                    audit.IsOnlyInsgin = true;
                    ProxyUserMessage proxyUserMsg = new ProxyUserMessage();
                    List<EntityDicObrMessageContent> messageList = new List<EntityDicObrMessageContent>();
                    ProxyObrMessageContent proxyMsg = new ProxyObrMessageContent();
                    messageList = proxyMsg.Service.GetAllMessage(false, false, true);
                    foreach (string id in listDeleteMsgPatId)
                    {
                        ProxyObrMessage proxyDeptMsg = new ProxyObrMessage();
                        EntityDicObrMessageContent message = messageList.Find(i => i.ObrValueA == id);
                        if (message != null)
                            proxyDeptMsg.Service.DeleteMessageByIDAndUpdateCriticalChecker(audit, message.ObrId, id);
                    }
                }
            }
        }


        /// <summary>
        /// 改变行颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewPatientList_RowStyle(object sender, RowStyleEventArgs e)
        {
            entity.EntityPidReportMain dr = (entity.EntityPidReportMain)this.gridViewPatientList.GetRow(e.RowHandle);
            if (dr != null && dr.RepStatus != null)
            {
                if (selPar.EnbState && ConfigHelper.GetSysConfigValueWithoutLogin("Select_ListColour") == "是")
                {
                    if (dr.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Printed)
                    {
                        e.Appearance.ForeColor = Color.Blue;
                    }
                    else if (dr.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Natural && dr.MicReportFlag == 1)
                    {
                        e.Appearance.ForeColor = Color.Orange;
                    }
                    else
                    {
                        e.Appearance.ForeColor = Color.Black;
                    }
                }
                else
                {
                    if (dr.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Audited)
                    {
                        e.Appearance.ForeColor = Color.Green;
                    }
                    else if (dr.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Reported)
                    {
                        e.Appearance.ForeColor = Color.Blue;
                    }
                    else if (dr.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Printed)
                    {
                        e.Appearance.ForeColor = Color.Red;
                    }
                    else if (dr.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Natural && dr.MicReportFlag == 1)
                    {
                        e.Appearance.ForeColor = Color.Orange;
                    }

                    if (dr.RepCtype.ToString() == "急查" || dr.RepCtype.ToString() == "溶栓")
                    {
                        e.Appearance.BackColor = Color.FromArgb(255, 224, 192);
                    }
                }
            }
        }

        /// <summary>
        /// 控制仪器权限
        /// </summary>
        /// <param name="strFilter"></param>
        private void lue_itr_id_onBeforeFilter(ref string strFilter)
        {
            string strIns = "(" + UserInfo.sqlUserItrs + ")";

            if (UserInfo.sqlUserItrs != "" && UserInfo.sqlUserItrs != "-1")
            {
                strFilter += "and itr_id in " + strIns + "";
            }
            else
            {
                if (UserInfo.isAdmin == false)
                    strFilter += "and 1<>1";
            }

        }

        List<entity.EntityPidReportMain> checkList = new List<entity.EntityPidReportMain>();
        /// <summary>
        /// 状态过滤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkChange_CheckedChanged(object sender, EventArgs e)
        {
            gridViewPatientList.FocusedRowChanged -= new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            checkList = new List<entity.EntityPidReportMain>();
            foreach (entity.EntityPidReportMain pat in listEntPatients)
            {
                pat.PatSelect = false;
            }
            bGridHeaderCheckBoxState = false;
            if (chkAudited.Checked)
            {
                List<entity.EntityPidReportMain> listPat = new List<entity.EntityPidReportMain>();
                listPat = listEntPatients.FindAll(i => i.RepStatus.Value.ToString() == LIS_Const.PATIENT_FLAG.Audited);
                checkList.AddRange(listPat);
            }
            if (chkPrinted.Checked)
            {
                List<entity.EntityPidReportMain> listPat = new List<entity.EntityPidReportMain>();
                listPat = listEntPatients.FindAll(i => i.RepStatus.Value.ToString() == LIS_Const.PATIENT_FLAG.Printed);
                checkList.AddRange(listPat);
            }
            if (chkReported.Checked)
            {
                List<entity.EntityPidReportMain> listPat = new List<entity.EntityPidReportMain>();
                listPat = listEntPatients.FindAll(i => i.RepStatus.Value.ToString() == LIS_Const.PATIENT_FLAG.Reported);
                checkList.AddRange(listPat);
            }
            if (chkUnaudited.Checked)
            {
                List<entity.EntityPidReportMain> listPat = new List<entity.EntityPidReportMain>();
                listPat = listEntPatients.FindAll(i => i.RepStatus.Value.ToString() == LIS_Const.PATIENT_FLAG.Natural);
                checkList.AddRange(listPat);
            }
            if (chkPositive.Checked)
            {
                List<entity.EntityPidReportMain> listPat = new List<entity.EntityPidReportMain>();
                if(!chkUnaudited.Checked && !chkAudited.Checked && !chkPrinted.Checked && !chkReported.Checked)
                {
                    checkList = listEntPatients.FindAll(i => proxy.Service.IsContainOutlier(i.RepId));
                }
                else
                {
                    checkList = checkList.FindAll(i => proxy.Service.IsContainOutlier(i.RepId));
                }
            }

            bsPar.DataSource = checkList;
            if (!chkUnaudited.Checked && !chkAudited.Checked && !chkPrinted.Checked && !chkReported.Checked && !chkPositive.Checked)
            {
                checkList = listEntPatients;
                bsPar.DataSource = checkList;
            }

            selPar.State = 0;
            gridViewPatientList.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            for (int i = 0; i < checkList.Count; i++)
            {
                checkList[i].PatSort = (i + 1).ToString();
            }



            lblAll.Text = checkList.Count.ToString();

            lblReport.Text = checkList.FindAll(i => i.RepStatus.Value.ToString() == LIS_Const.PATIENT_FLAG.Reported).Count.ToString();

            lblPrint.Text = checkList.FindAll(i => i.RepStatus.Value.ToString() == LIS_Const.PATIENT_FLAG.Printed).Count.ToString();

        }

        private void gridViewPatientList_Click(object sender, EventArgs e)
        {
            if (selPar.State == 0)
            {
                gridView1_FocusedRowChanged(sender, null);
                selPar.State = 1;
            }
        }

        private void FrmCombineModeSel_Paint(object sender, PaintEventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                if (selPar.EnbState && ConfigHelper.GetSysConfigValueWithoutLogin("Allow_Multi_BarCodeClient") != "是")
                {
                    DownLoadZYBarcode();
                }
            }
        }


        private void sysToolBar1_BtnPrintSetClick(object sender, EventArgs e)
        {
            FrmPrintConfiguration frc = new FrmPrintConfiguration();
            frc.ShowDialog();
        }


        private void xtcReport_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            showReport();
        }

        private void FrmCombineModeSel_FormClosing(object sender, FormClosingEventArgs e)
        {
#if !DEBUG
            //if (selPar.EnbState)
            //{
            //    try
            //    {
            //        string autoupdatePath = Application.StartupPath + "\\autoupdate.exe";

            //        if (File.Exists(autoupdatePath))
            //        {
            //            Process.Start(autoupdatePath);
            //        }

            //        //模拟点击桌面的程序图标进行程序更新
            //        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + "\\程序\\广州慧扬健康科技有限公司\\慧扬医学检验信息系统-条码检验查询.appref-ms");
            //    }
            //    catch (Exception ex)
            //    {
            //        Logger.WriteException(this.GetType().Name, "FrmCombineModeSel_FormClosing", ex.ToString());
            //    }
            //}
#endif
        }

        private void sysToolBar1_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            sysToolBar1.Focus();
            if (bsPar.DataSource == null) return;
            List<entity.EntityPidReportMain> listPat = bsPar.DataSource as List<entity.EntityPidReportMain>;
            List<entity.EntityPidReportMain> pats = new List<entity.EntityPidReportMain>();

            bool canDeleteAuditedResult = ConfigHelper.GetSysConfigValueWithoutLogin("RepQuery_CanDeleteAuditedData") == "是";

            if (canDeleteAuditedResult)
            {
                pats = listPat.FindAll(i => i.PatSelect == true);
            }
            else
            {
                pats = listPat.FindAll(i => i.PatSelect == true && i.RepStatus == 0);
            }

            if (pats.Count > 0)
            {
                if (lis.client.control.MessageDialog.Show("是否要删除这" + pats.Count.ToString() + "条数据？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
                FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 删除", LIS_Const.BillPopedomCode.Delete, "", "");

                DialogResult dig = frmCheck.ShowDialog();
                if (dig == DialogResult.OK)
                {
                    bool bDelResult = lis.client.control.MessageDialog.Show("是否连同病人结果一起删除？", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes ? true : false;
                    bool result = false;
                    foreach (entity.EntityPidReportMain drPat in pats)
                    {
                        if (drPat.ItrReportType != null && drPat.ItrReportType.Trim() != "")
                        {
                            string pat_id = drPat.RepId.Trim();

                            if (drPat.ItrReportType.ToString().Trim() == "3")
                            {
                                result = proxy.Service.DeletePatientInfo(drPat, bDelResult == true ? "1" : "0");
                            }
                            else
                            {
                                EntityLogLogin logLogin = new EntityLogLogin();
                                logLogin.LogLoginID = frmCheck.OperatorID;
                                logLogin.LogIP = IPUtility.GetIP();

                                result = new ProxyCombModelSel().Service.DelPatCommonResult(logLogin, drPat, bDelResult, canDeleteAuditedResult);
                            }
                        }
                    }
                    if (result)
                        lis.client.control.MessageDialog.Show("删除成功！", "提示");
                    sysToolBar1_OnBtnSearchClicked(null, null);
                }
            }
            else
            {
                lis.client.control.MessageDialog.Show("请选择你要删除的数据或所选病人已审核/报告！", "提示");
            }
        }

        private void sysToolBar1_OnBtnPrintListClicked(object sender, EventArgs e)
        {
            isClassPrint = false;
            sysToolBar1.Focus();
            if (bsPar.DataSource == null) return;
            List<entity.EntityPidReportMain> listPat = (bsPar.DataSource as List<entity.EntityPidReportMain>).FindAll(i => i.PatSelect == true);
            if (listPat.Count <= 0)
            {
                lis.client.control.MessageDialog.Show("请选择你要打印的数据", "提示");
                return;
            }

            List<EntityDCLPrintParameter> listPara = new List<EntityDCLPrintParameter>();
            foreach (entity.EntityPidReportMain item in listPat)
            {
                EntityDCLPrintParameter parameter = new EntityDCLPrintParameter();
                parameter.RepId = item.RepId;
                parameter.ReportCode = "printPatientsList";

                listPara.Add(parameter);
            }
            DCLReportPrint.BatchPrint(listPara);
        }

        private void sysToolBar1_OnBtnStatClicked(object sender, EventArgs e)
        {
            //FrmGeneralStatistics frmStat = new FrmGeneralStatistics();
            //frmStat.ShowDialog();
        }

        private void sysToolBar1_OnBtnBCPrintClicked(object sender, EventArgs e)
        {
            isClassPrint = false;
            print(true);
        }

        private void txt_in_no_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                sysToolBar1_OnBtnSearchClicked(null, null);
                txt_in_no.Focus();
                txt_in_no.SelectAll();
            }

        }

        private void SaveColumnSortToolStrip_Click(object sender, EventArgs e)
        {
            int count = 0;
            for (int m = 0; m < gridViewPatientList.Columns.Count; m++)
            {
                if (gridViewPatientList.Columns[m].Visible)
                {
                    count++;
                }
            }
            int[] sort = new int[count];
            int i = 0;
            foreach (DevExpress.XtraGrid.Columns.GridColumn column in gridViewPatientList.Columns)
            {
                if (column.Visible)
                {
                    sort[i] = column.VisibleIndex;
                    i++;
                }
            }

            string sortStr = string.Empty;
            for (int m = 0; m < sort.Length; m++)
            {
                sortStr += "," + sort[m].ToString();
            }
            sortStr = sortStr.Remove(0, 1);

            bool result = proxy.Service.SaveSysPara(sortStr, patientsSelectViewSortConfigcode);

            if (result)
                lis.client.control.MessageDialog.ShowAutoCloseDialog("保存成功");
        }


        /// <summary>
        /// 设置列顺序
        /// </summary>
        public void setColumnSort()
        {
            string sort = ConfigHelper.GetSysConfigValueWithoutLogin(patientsSelectViewSortConfigcode);
            if (string.IsNullOrEmpty(sort))
            {
                return;
            }
            try
            {
                string strtempindex = "";//记录排序号
                string strtempCaption = "";//记录列名
                int fVisibleIndex = 0;//递增排序号
                foreach (DevExpress.XtraGrid.Columns.GridColumn column in gridViewPatientList.Columns)
                {
                    if (column.Visible)
                    {
                        column.VisibleIndex = fVisibleIndex;
                        fVisibleIndex++;
                    }
                }
                foreach (DevExpress.XtraGrid.Columns.GridColumn column in gridViewPatientList.Columns)
                {
                    if (column.Visible)
                    {
                        strtempCaption += column.Caption + ",";
                        strtempindex += column.VisibleIndex.ToString() + ",";
                    }
                }


                foreach (DevExpress.XtraGrid.Columns.GridColumn column in gridViewPatientList.Columns)
                {
                    if (column.Visible)
                    {
                        int length = sort.IndexOf(',');
                        if (length > 0)
                        {
                            column.VisibleIndex = Convert.ToInt32(sort.Substring(0, sort.IndexOf(',')));
                        }
                        else
                        {
                            column.VisibleIndex = Convert.ToInt32(sort.Substring(0));
                        }
                        if (length > 0)
                        {
                            sort = sort.Substring(sort.IndexOf(',') + 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("加载结果浏览页列顺序出错", ex);
            }

        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                sysToolBar1_OnBtnSearchClicked(null, null);
                txtName.Focus();
                txtName.SelectAll();
            }
        }

        bool isClearSelect = true;


        string strDefaultPrinter = string.Empty;

        private void sysToolBar1_OnBtnQualityOutClicked(object sender, EventArgs e)
        {
            sysToolBar1.Focus();
            System.Drawing.Printing.PrintDocument prtdoc = new System.Drawing.Printing.PrintDocument();
            strDefaultPrinter = prtdoc.PrinterSettings.PrinterName;

            if (bsPar.DataSource == null) return;
            List<entity.EntityPidReportMain> listPat = bsPar.DataSource as List<entity.EntityPidReportMain>;
            if (listPat == null || listPat.FindAll(i => i.PatSelect == true).Count <= 0)
            {
                lis.client.control.MessageDialog.Show("请选择要打印的数据！", "提示");
                return;
            }
            List<entity.EntityPidReportMain> drPat = new List<entity.EntityPidReportMain>();
            drPat = listPat.FindAll(i => i.PatSelect == true && (i.RepStatus.Value.ToString() == LIS_Const.PATIENT_FLAG.Reported || i.RepStatus.Value.ToString() == LIS_Const.PATIENT_FLAG.Printed)).OrderByDescending(i => i.RepReportDate).ToList();

            if (drPat.Count > 0)
            {

                StringBuilder strPatId = new StringBuilder();
                List<EntityDCLPrintParameter> listPara = new List<EntityDCLPrintParameter>();
                for (int i = 0; i < drPat.Count; i++)
                {
                    entity.EntityPidReportMain patient = drPat[i];


                    string code = patient.ItrReportId.ToString();
                    string itrFlag = patient.ItrReportType.ToString();
                    if (code.IndexOf("bacilli") < 0 && itrFlag != "4")
                    {
                        strPatId.Append(string.Format(",'{0}'", patient.RepId.ToString()));
                        patient.PatSelect = false;
                    }

                    //加入打印实体
                    EntityDCLPrintParameter printPara = new EntityDCLPrintParameter();
                    printPara.RepId = patient.RepId;
                    printPara.ReportCode = "summaryPrint";
                    printPara.Sequence = i;
                    listPara.Add(printPara);

                }
                isClearSelect = false;
                if (strPatId.Length > 0)
                {
                    strPatId = strPatId.Remove(0, 1);

                    selPar.StPatId = strPatId;

                    try
                    {
                        DCLReportPrint.BatchPrint(listPara, strDefaultPrinter);
                        frp_PrintStart2(null, null);
                    }
                    catch (ReportNotFoundException ex1)
                    {
                        lis.client.control.MessageDialog.Show(ex1.MSG);
                    }
                    catch (Exception ex2)
                    {
                        Logger.LogException("FrmCombineModeSel_Print", ex2);
                    }

                }
                print(true);
                isClearSelect = true;
                strDefaultPrinter = string.Empty;
            }
        }

        /// <summary>
        /// 设置打印人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_BtnQuickEntryClick(object sender, EventArgs e)
        {
            outPrintPerson = string.Empty;
            sysToolBar1.BtnQuickEntry.Enabled = false;
        }

        /// <summary>
        /// 为打印者信息赋值
        /// </summary>
        /// <param name="strOutPtPn"></param>
        public void setoutPrintPerson(string strOutPtPn)
        {
            outPrintPerson = strOutPtPn;
        }

        /// <summary>
        /// 方正电子病历接口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnResultViewClicked(object sender, EventArgs e)
        {
            entity.EntityPidReportMain drPar = gridViewPatientList.GetFocusedRow() as entity.EntityPidReportMain;
            if (drPar != null)
            {
                string temp_pat_in_no = "";//患者ID
                string temp_pat_pid = "";//His系统就诊号
                string temp_pat_dep_id = "";//科室编码
                string temp_userCd = "000000";//用户编码

                //方正电子病历接口地址
                string StrFounderCisAddress = ConfigHelper.GetSysConfigValueWithoutLogin("RepSel_FounderCisAddress");

                if (!string.IsNullOrEmpty(StrFounderCisAddress))
                {
                    if (!string.IsNullOrEmpty(drPar.PidInNo))
                    {
                        temp_pat_in_no = drPar.PidInNo.ToString();
                    }

                    if (!string.IsNullOrEmpty(drPar.RepInputId))
                    {
                        temp_pat_pid = drPar.RepInputId.ToString();
                    }

                    if (!string.IsNullOrEmpty(drPar.PidDeptId))
                    {
                        temp_pat_dep_id = drPar.PidDeptId.ToString();
                    }

                    string p_cisaddress = string.Format("{0}patientId={1}&hisVisitNo={2}&deptCd={3}&userCd={4}", StrFounderCisAddress, temp_pat_in_no, temp_pat_pid, temp_pat_dep_id, temp_userCd);
                    //调用系统默认的浏览器  
                    System.Diagnostics.Process.Start(p_cisaddress);
                }
                else
                {
                    lis.client.control.MessageDialog.Show("查看失败，请设置电子病历访问地址！", "提示");
                }
            }
            else
            {
                lis.client.control.MessageDialog.Show("请选中需要查看电子病历的信息！", "提示");
            }
        }

        /// <summary>
        /// 取消阅读
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_BtnUndoClick(object sender, EventArgs e)
        {
            sysToolBar1.Focus();
            if (bsPar.DataSource == null) return;
            List<entity.EntityPidReportMain> listPat = bsPar.DataSource as List<entity.EntityPidReportMain>;
            if (listPat == null || listPat.FindAll(i => i.PatSelect == true).Count <= 0)
            {
                lis.client.control.MessageDialog.Show("请勾选要取消阅读的数据！", "提示");
                return;
            }

            FrmCheckPassword frm = new FrmCheckPassword();
            if (frm.ShowDialog() == DialogResult.OK)//身份验证
            {
                //DataRow[] drtempSelArray = dt.Select("pat_select='true'");
                List<entity.EntityPidReportMain> listSelPat = listPat.FindAll(i => i.PatSelect == true);
                List<string> temp_patlist = new List<string>();
                bool isotherLooker = false;
                foreach (entity.EntityPidReportMain drtemp in listSelPat)
                {
                    string str_pat_id = drtemp.RepId.ToString();

                    if (!isotherLooker && drtemp.RepReadUserId.ToString() != frm.OperatorID)
                    {
                        isotherLooker = true;
                    }
                    if (!string.IsNullOrEmpty(str_pat_id))
                    {
                        temp_patlist.Add(str_pat_id);
                    }
                }

                string op = "unForOwnLooker";//自己取消自己阅读
                //用户是否有权限取消所有阅读信息
                if (isotherLooker)
                {
                    op = "un";
                    EntityUserQc userQc = new EntityUserQc();
                    userQc.LoginId = frm.OperatorID;
                    userQc.FuncCode = "FrmCombineModeSel_UnAllLookcode";
                    if (new ProxySysUserInfo().Service.SysUserQuery(userQc).Count == 0)
                    {
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("提示:你只能取消自己阅读过的报告");
                    }
                    else
                    {
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("提示:您只能取消自己[所属科室]的阅读过的报告");
                    }
                }
                if (temp_patlist.Count > 0)
                {
                    new ProxyPidReportMain().Service.UpdateRepReadUserId(op, frm.OperatorID, temp_patlist);
                }
            }
        }

        /// <summary>
        /// 批量打印预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnPrintBatchPviewClicked(object sender, EventArgs e)
        {
            HasShowPreview = true;
            print(true);
            HasShowPreview = false;
        }

        /// <summary>
        /// 读卡并查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnQualityAuditClicked(object sender, System.EventArgs e)
        {
            try
            {
                int al_reader = HuaDaIDReader.ICC_Reader_Open("USB1");//打开端口

                if (al_reader > 0)
                {
                    if (HuaDaIDReader.ICC_PosBeep(al_reader, 0x05) >= 0)//蜂鸣
                    {

                    }

                    string temp_strRv = "";//记录卡信息

                    //请求卡片
                    if (HuaDaIDReader.PICC_Reader_Request(al_reader) >= 0)
                    {
                        ulong temp_sb = new ulong();
                        //防碰撞卡片
                        if (HuaDaIDReader.PICC_Reader_anticoll(al_reader, ref temp_sb) >= 0)
                        {
                            string temp_hexkey = "ACF9FF8DFE57";
                            //下载认证密钥
                            if (HuaDaIDReader.PICC_Reader_Authentication_PassHEX(al_reader, 96, 0, temp_hexkey) >= 0)
                            {
                                StringBuilder temp_DataHex = new StringBuilder();
                                //读取卡片信息
                                if (HuaDaIDReader.PICC_Reader_ReadHEX(al_reader, 2, temp_DataHex) >= 0)
                                {
                                    if (temp_DataHex.ToString().Length > 0)
                                    {
                                        temp_strRv = temp_DataHex.ToString();
                                        if (temp_strRv.ToUpper().IndexOf("F") > 0)
                                        {
                                            int TEMPIX = temp_strRv.ToUpper().IndexOf("F");
                                            temp_strRv = temp_strRv.Substring(0, TEMPIX);
                                            if (!string.IsNullOrEmpty(temp_strRv))
                                            {
                                                this.txt_in_no.Text = temp_strRv;
                                                SelectPatients(string.Empty, false);
                                            }
                                        }
                                        else
                                        {
                                            MessageDialog.ShowAutoCloseDialog("诊疗卡信息空白,请检查卡是否有效", 2M);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(temp_strRv))//没有读取不到信息,则提示
                    {
                        MessageDialog.ShowAutoCloseDialog("读取诊疗卡信息失败,请检查卡是否放好", 2M);
                    }

                    if (HuaDaIDReader.ICC_Reader_Close(al_reader) == 0)//关闭
                    {
                    }
                }
                else
                {
                    MessageDialog.ShowAutoCloseDialog("打开读卡端口失败,请检查读卡器是否连接好", 2M);
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("读取华大诊疗卡接口", ex);
            }
        }

        private void sysToolBar1_OnBtnExportClicked(object sender, EventArgs e)
        {
            if (gcPar.DataSource != null)
            {
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
                        gcPar.ExportToXls(ofd.FileName.Trim());
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("导出成功！");
                    }
                    catch (Exception ex)
                    {
                        Lib.LogManager.Logger.LogException("导出", ex);
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("导出失败！");
                    }
                }

            }
        }

        private void gridViewPatientList_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle == this.gridViewPatientList.FocusedRowHandle)
            {
                //e.Appearance.BackColor = System.Drawing.Color.LightBlue;
                e.Appearance.Options.UseBackColor = true;
            }
            GridView grid = sender as GridView;
            entity.EntityPidReportMain row = grid.GetRow(e.RowHandle) as entity.EntityPidReportMain;

            if (e.Column.FieldName == "PidName")
            {
                if (row != null)
                {
                    if (row.RepUrgentFlag.ToString() == "1" || row.RepUrgentFlag.ToString() == "2")//未查看危急值
                    {
                        e.Appearance.BackColor = Color.FromArgb(255, 128, 255);
                    }
                }
            }

        }

        private void sysToolBar1_BtnBrowseClick(object sender, EventArgs e)
        {
            if (bsPar.DataSource != null
                && gridViewPatientList.GetFocusedDataRow() != null)
            {
                entity.EntityPidReportMain pat = gridViewPatientList.GetFocusedRow() as entity.EntityPidReportMain;

                if (pat.SrcName == "住院")
                {
                    if (pat.PidInNo.ToString().Length <= 0)
                    {
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("当前没有住院号信息不能病历浏览");
                        return;
                    }

                    string temp_url = @"http://172.17.250.10:82/show.aspx?EncryptImf=strPatientCode&ZYHM=strPatientCode";

                    temp_url = temp_url.Replace("strPatientCode", pat.PidInNo.ToString());

                    //调用系统默认的浏览器   
                    //System.Diagnostics.Process.Start("explorer.exe", temp_url);
                    System.Diagnostics.Process.Start(temp_url);
                }
                else
                {
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("当前不是住院病人不能病历浏览");
                }
            }
            else
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("请选中一行需要病历浏览的病人信息");
            }
        }

        List<entity.EntityPidReportMain> listClass = null;
        bool isClassPrint = false;

        private void sysToolBar1_OnBtnClassPrintClicked(object sender, EventArgs e)
        {
            if ((DataTable)bsPar.DataSource == null)
            {
                lis.client.control.MessageDialog.Show("无可打印数据");
                return;
            }
            FrmCombineClassPrint frmCCP = new FrmCombineClassPrint();
            frmCCP.ShowDialog();
            if (frmCCP.DialogResult == DialogResult.OK)
            {
                EntityAnanlyseQC query = new EntityAnanlyseQC();
                query.StrClassWhere = frmCCP.strClass;
                if (bsPar.DataSource == null) return;
                List<entity.EntityPidReportMain> listPat = (bsPar.DataSource as List<entity.EntityPidReportMain>).FindAll(i => i.PatSelect == true);
                DataTable dtPatId = new DataTable();
                foreach (entity.EntityPidReportMain dr in listPat)
                {
                    if (!string.IsNullOrEmpty(dr.RepId.ToString()))
                    {
                        query.PatId = dr.RepId.ToString();
                        dtPatId = proxy.Service.GetPatId(query);
                    }
                    if (dtPatId != null && dtPatId.Rows.Count > 0)
                    {
                        listClass.Add(dr);
                    }
                }
                if (listClass != null && listClass.Count > 0)
                {
                    isClassPrint = true;
                    print(false);
                }
                isClassPrint = false;
            }
        }
        string list_itr_id = "";
        private void txtBingdingItrID_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            gvApparatus.CloseEditor();
            bindingSourceItr.EndEdit();
            if (bindingSourceItr.DataSource != null && bindingSourceItr.DataSource is DataView)
            {
                DataView dvItr = bindingSourceItr.DataSource as DataView;
                if (dvItr != null && dvItr.Table.Rows.Count > 0)
                {
                    DataRow[] drpaItrID = dvItr.Table.Select(" sp_select=1 ");
                    int kcount = drpaItrID.Length;
                    if (kcount > 0)
                    {
                        txtBingdingItrID.Text = "已设置" + kcount.ToString() + "台";
                        list_itr_id = "";
                        foreach (DataRow dr in drpaItrID)
                        {
                            if (string.IsNullOrEmpty(list_itr_id))
                            {
                                list_itr_id = string.Format("'{0}'", dr["itr_id"]);
                            }
                            else
                            {
                                list_itr_id += string.Format(",'{0}'", dr["itr_id"]);
                            }
                        }
                    }
                    else
                    {
                        txtBingdingItrID.Text = "未设置";
                        list_itr_id = "";
                    }
                }
            }
        }

        private void lue_Wtype_onAfterSelected_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lue_Wtype.valueMember))
            {
                List<EntityDicInstrument> drs = dtItr.Where(i => i.ItrLabId == lue_Wtype.valueMember).ToList();

                bindingSourceItr.DataSource = drs;
            }
        }

        /// <summary>
        /// 报告解读
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportSumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EntityPidReportMain drPar = gridViewPatientList.GetFocusedRow() as entity.EntityPidReportMain;

            if (drPar != null)
            {
                new FrmReportSumInfo(drPar.RepId).ShowDialog();
            }
        }

        #region Obsolete

        #region 读取卡数据并下载医嘱

        void BtnImport_Click(object sender, EventArgs e)
        {
            ReadCardDataAndDownLoad();
        }

        void ReadCardDataAndDownLoad()
        {
            CardReader.ICardReader reader = CardReader.CardReaderFactory.CreateCardReader(strCardReaderDriver);
            if (reader.ReadCardData())
            {
                string data = reader.CardData;
                if (string.IsNullOrEmpty(data))
                {
                    MessageDialog.ShowAutoCloseDialog("无法读取卡数据，请拿起卡再重新放到读卡器上！");

                }
                else
                {
                    if (reader.RequireConvert)
                    {
                        ProxySysItfInterface proxy = new ProxySysItfInterface();
                        string interfaceKey = "门诊卡号转换";
                        EntityOperationResult result = proxy.Service.CardDataConvert(data, interfaceKey);
                        if (result.HasError)
                        {
                            MessageDialog.Show(result.Message[0].Param.ToString());
                        }
                        else if (result.OperationResultData != null
                            && !string.IsNullOrEmpty(result.OperationResultData.ToString()))
                        {
                            this.txt_in_no.Text = result.OperationResultData.ToString();
                            sysToolBar1_OnBtnSearchClicked(null, null);

                        }
                        else
                        {
                            MessageDialog.Show(string.Format("未找到卡 {0} 对应的数据", data), "卡号转换提示");
                        }
                    }
                    else
                    {
                        this.txt_in_no.Text = data;
                        sysToolBar1_OnBtnSearchClicked(null, null);

                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(reader.Msg))
                {
                    MessageDialog.Show(reader.Msg);
                }
                else
                {
                    MessageDialog.ShowAutoCloseDialog("无法读取卡数据，请把卡放到读卡器上！");
                }
            }

        }


        #endregion

        #endregion
    }


    public class selectParameter
    {
        public selectParameter()
        {
        }

        private IReportSelect iselect;

        public IReportSelect Iselect
        {
            get
            {
                if (iselect == null)
                {
                    if (enbState)
                        iselect = new HospitalSelect();
                    else
                        iselect = new NormalSelect();
                }
                return iselect;
            }
        }

        XtraReport xr;

        /// <summary>
        /// 报表对象
        /// </summary>
        public XtraReport Xr
        {
            get { return xr; }
            set { xr = value; }
        }

        StringBuilder stPatId = new StringBuilder();

        /// <summary>
        /// 打印ID集合
        /// </summary>
        public StringBuilder StPatId
        {
            get { return stPatId; }
            set { stPatId = value; }
        }

        int state = 0;

        /// <summary>
        /// 查询状态
        /// </summary>
        public int State
        {
            get { return state; }
            set { state = value; }
        }

        bool enbState = true;

        /// <summary>
        /// 状态(是否是单独调用界面)
        /// </summary>
        public bool EnbState
        {
            get { return enbState; }
            set { enbState = value; }
        }

        string pat_in_no = string.Empty;
        /// <summary>
        /// 病人ID
        /// </summary>
        public string Pat_in_no
        {
            get { return pat_in_no; }
            set { pat_in_no = value; }
        }
        string dep_incode = string.Empty;

        /// <summary>
        /// 科室His编码
        /// </summary>
        public string Dep_incode
        {
            get { return dep_incode; }
            set { dep_incode = value; }
        }

        string path = PathManager.SettingPath + @"reportparam.ini";
        /// <summary>
        /// 外部调用报表参数位置
        /// </summary>
        public string Path
        {
            get { return path; }
        }


        

    }
}
