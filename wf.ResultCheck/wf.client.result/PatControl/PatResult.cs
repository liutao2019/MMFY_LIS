using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using dcl.client.common;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System.Collections;
using dcl.root.logon;
using dcl.client.frame.runsetting;
using dcl.client.result.CommonPatientInput;
using dcl.client.result.Interface;
using dcl.client.wcf;
using dcl.common;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors;
using lis.client.control;
using dcl.client.result.DictToolkit;
using dcl.client.frame;
using System.Reflection;
using System.Threading;
using dcl.entity;
using System.Linq;

using Microsoft.CSharp;
using System.CodeDom.Compiler;
using dcl.client.cache;

namespace dcl.client.result.PatControl
{
    public partial class PatResult : UserControl
    {
        public FrmPatEnterNew parentFormNew = null;

        /// <summary>
        /// 结果表
        /// </summary>
        List<EntityObrResult> dtPatientResulto
        {
            get;
            set;
        }

        /// <summary>
        /// 是否修改过结果信息
        /// </summary>
        public static bool IsPatResultVChange { get; private set; }

        /// <summary>
        /// 是否计算小数点
        /// </summary>
        public bool ShouldCalcDigit = true;

        /// <summary>
        /// 项目特征选择窗体
        /// </summary>
        FrmItmPropLst prop = null;

        /// <summary>
        /// 查看复查原结果
        /// </summary>
        FrmOldResultView frmORV = null;//

        /// <summary>
        /// 固定单列显示
        /// </summary>
        private Boolean FixSingleColumn = false;

        /// <summary>
        /// 项目信息窗体
        /// </summary>
        FrmItemInfo info = null;

        /// <summary>
        /// 偏高偏低(阳性)标志表
        /// </summary>
        List<EntityDicResultTips> dict_res_ref_flag;

        public bool bAllowCalItemRef = false;
        bool bEnableCellValueChange = true;
        private bool Lab_FilterHasResultItem = false;
        private bool Lab_MarkModifyResultWithColor = false;
        private bool Lab_EiasaCheckItmResUseOdValue = false;
        private List<EntityObrResult> ResultDatatableBak = null;
        List<EntityPidReportDetail> patients_mi;

        List<EntitySysOperationLog> listSysOprLog = new List<EntitySysOperationLog>();

        private string patExp;

        public string PatExp
        {
            get { return patExp; }
            set { patExp = value; }
        }

        #region 字典
        /// <summary>
        /// 项目特征
        /// </summary>
        private List<EntityDefItmProperty> DictItemProp;

        #endregion
        public ICombineEditor CombineEditor { get; set; }
        #region props & fields
        /// <summary>
        /// 标识ID
        /// </summary>
        public string PatID { get; set; }

        /// <summary>
        /// 病人ID
        /// </summary>
        public string PatInNo { get; set; }

        /// <summary>
        /// 仪器
        /// </summary>
        public string Pat_itr_id { get; set; }


        private string pat_dep_id;
        /// <summary>
        /// 科室
        /// </summary>
        public string Pat_dep_id
        {
            get { return pat_dep_id; }
            set
            {
                pat_dep_id = value;
                gridControlDouble.Pat_dep_id = value;
            }
        }

        private string _itr_rep_flag;

        /// <summary>
        /// 仪器数据类型
        /// </summary>
        public string Itr_rep_flag
        {
            get
            {
                return this._itr_rep_flag;
            }
            set
            {
                string oldValue = _itr_rep_flag;
                if (oldValue != value)
                {
                    _itr_rep_flag = value;
                    ChangeRepFlag(_itr_rep_flag);
                }
            }
        }



        private int patage;
        /// <summary>
        /// 病人年龄(分钟)
        /// </summary>
        public int PatAge
        {
            get
            {
                return patage;
            }
            set
            {
                if (patage != value)
                {
                    patage = value;
                }
            }
        }

        private string patdiag;
        /// <summary>
        /// 临床诊断
        /// </summary>
        public string PatDiag
        {
            get
            {
                return patdiag;
            }
            set
            {
                if (patdiag != value)
                {
                    patdiag = value;
                }
            }
        }


        private string patsex;

        private string patFlag;
        /// <summary>
        /// 病人性别
        /// </summary>
        public string PatSex
        {
            get
            {
                return patsex;
            }
            set
            {
                if (patsex != value)
                {
                    patsex = value;
                }
            }
        }


        private string samtypeid;
        /// <summary>
        /// 样本类别ID
        /// </summary>
        public string SamType_id
        {
            get
            {
                return samtypeid;
            }
            set
            {
                if (samtypeid != value)
                {
                    samtypeid = value;
                }
            }
        }


        private string samrem;
        /// <summary>
        /// 标本备注
        /// </summary>
        public string Samrem
        {
            get { return samrem; }
            set
            {
                if (samrem != value)
                {
                    samrem = value;
                    gridControlDouble.Samrem = value;
                }
            }
        }

        private string itr_ptype;
        /// <summary>
        /// 仪器专业组
        /// </summary>
        public string Itr_ptype
        {
            get { return itr_ptype; }
            set
            {
                itr_ptype = value;
                gridControlDouble.Itr_ptype = value;
            }
        }
        #endregion

        /// <summary>
        /// 配置类
        /// </summary>
        PatInputPatResultSettingItem Config;

        /// <summary>
        /// 加载设置
        /// </summary>
        /// <param name="config"></param>
        public void ApplyConfig(PatInputPatResultSettingItem conf)
        {
            Config = conf;
            gridControlDouble.ApplyConfig(Config);
            this.VisibleStyle = Config.VisibleStyle;
            this.EachViewOnlyTwoCombine = Config.EachViewOnlyTwoCombine;
            this.LeftGridMaxRow = Config.LeftGridMaxRow;



            this.BindGrid();
        }

        #region ctor & load
        /// <summary>
        /// 构造函数
        /// </summary>
        public PatResult()
        {
            InitializeComponent();
            PatID = string.Empty;
            //Config = new PatInputPatResultSettingItem();

            //隐藏已删除项目
            gridViewSingle.Columns["IsNew"].FilterMode = DevExpress.XtraGrid.ColumnFilterMode.Value;
            gridViewSingle.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            gridViewSingle.ActiveFilterString = "[IsNew] != 2";

            //注册事件
            this.gridViewSingle.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridViewSingle_FocusedRowChanged);

            this.lblItemsCount.Text = string.Empty;
            this.lblLostItems.Visible = false;
            this.lblOverFlag.Visible = false;

            gridControlDouble.ColumnChange += gridControlDouble_ColumnChange;
            //*********************************************************************
            //根据功能权限判断是否拥有保存列顺序的功能
            if (!UserInfo.HaveFunctionByCode("savePatResultColumn"))
            {
                this.SaveColumnSortToolStrip.Visible = false;
            }

            //*********************************************************************
        }

        private void gridControlDouble_ColumnChange()
        {
            gridControlDouble.Visible = false;
            gridControlSingle.Visible = true;
            gridControlSingle.Dock = DockStyle.Fill;
            FixSingleColumn = true;
            //双列切换单列再次加载历史结果
            Thread t = new Thread(ThreadLoadHistory);
            t.Start();
            BindSingleGrid();
        }
        bool showItrWarningMsg = false;


        bool showPicForm = false;

        string PrevResult = string.Empty; //用于记录选中那一行的结果值，方便保存失败后进行回显
        /// <summary>
        /// 选中某行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gridViewSingle_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            EntityObrResult dr = gridViewSingle.GetFocusedRow() as EntityObrResult;
            if (dr != null)
            {
                PrevResult = dr.ObrValue;
            }
        }
        //判断结果偏高偏低的时候是否包含设定值在内
        bool Ref_CheckValueInclude = false;
        bool Pan_CheckValueInclude = false;
        //比值结果按照分数判断参考值
        /// <summary>
        /// 比值结果按照分数判断参考值
        /// </summary>
        bool Ref_CheckValueIsGradeScore = true;
        //参考值忽略结果的大于小于号直接判断
        /// <summary>
        /// 参考值忽略结果的大于小于号直接判断
        /// </summary>
        bool Ref_CheckValueIsNegbigOrLittleSymbol = false;
        string showItrWarningMsgConfig = string.Empty;

        bool AutoDouble = false;

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatResult_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                //判断结果偏高偏低的时候是否包含设定值在内
                Ref_CheckValueInclude = UserInfo.GetSysConfigValue("Ref_CheckValueInclude") == "是";
                Pan_CheckValueInclude = UserInfo.GetSysConfigValue("Pan_CheckValueInclude") == "是";
                AutoDouble = UserInfo.GetSysConfigValue("Lab_AutoResultDouble") == "是";
                //menuDoubleView.Visible = AutoDouble; 
                //系统配置：比值结果按照分数判断参考值
                Ref_CheckValueIsGradeScore = UserInfo.GetSysConfigValue("Ref_CheckValueIsGradeScore") != "否";
                //系统配置：参考值忽略结果的大于小于号直接判断
                Ref_CheckValueIsNegbigOrLittleSymbol = UserInfo.GetSysConfigValue("Ref_CheckValueIsNegbigOrLittleSymbol") == "是";
                bool allowCPC = UserInfo.GetSysConfigValue("Lab_Allow_CopyPasteCut") != "否";
                bool allowCut = UserInfo.GetSysConfigValue("Lab_Allow_Cut") != "否";
                Lab_EiasaCheckItmResUseOdValue = UserInfo.GetSysConfigValue("Lab_EiasaCheckItmResUseOdValue") == "是";
                //判断是否开启系统配置病人历史结果查询过滤时只用姓名与性名条件过滤
                if (UserInfo.GetSysConfigValue("Lab_ResultHistoryContrainName") == "是")
                {
                    this.lblHistoryResultOlnyName.Visible = true;
                }
                else
                {
                    this.lblHistoryResultOlnyName.Visible = false;
                    this.panel2.Height = this.panel2.Height - this.lblHistoryResultOlnyName.Height + 10;
                }
                this.gridColumn6.Visible = UserInfo.GetSysConfigValue("Lab_ResultViewShowOriItr") == "是";
                string Res_WarningMsgForwardItrid = UserInfo.GetSysConfigValue("Res_WarningMsgForwardItrid");
                if (UserInfo.GetSysConfigValue("Lab_ShowAlarmColumn") == "是")
                {

                    if (!string.IsNullOrEmpty(Pat_itr_id) && !string.IsNullOrEmpty(Res_WarningMsgForwardItrid) && Res_WarningMsgForwardItrid.Contains(Pat_itr_id))
                    {
                        gridColumn11.VisibleIndex = colRefRange.VisibleIndex + 1;
                        gridColumn7.VisibleIndex = colRefRange.VisibleIndex + 2;
                        gridColumn8.VisibleIndex = colRefRange.VisibleIndex + 3;
                        gridColumn9.VisibleIndex = colRefRange.VisibleIndex + 4;
                        gridColumn12.VisibleIndex = colRefRange.VisibleIndex + 5;
                        gridColumn13.VisibleIndex = colRefRange.VisibleIndex + 6;
                        gridColumn14.VisibleIndex = colRefRange.VisibleIndex + 7;
                        gridColumn10.VisibleIndex = colRefRange.VisibleIndex + 8;
                    }
                    else
                    {
                        gridColumn11.Visible = true;
                        gridColumn11.VisibleIndex = colResType.VisibleIndex + 1;
                        gridColumn7.VisibleIndex = gridColumn11.VisibleIndex + 1;
                        gridColumn8.VisibleIndex = gridColumn11.VisibleIndex + 2;
                        gridColumn9.VisibleIndex = gridColumn11.VisibleIndex + 3;
                        gridColumn12.VisibleIndex = gridColumn11.VisibleIndex + 4;
                        gridColumn13.VisibleIndex = gridColumn11.VisibleIndex + 5;
                        gridColumn14.VisibleIndex = gridColumn11.VisibleIndex + 6;
                        gridColumn10.VisibleIndex = gridColumn11.VisibleIndex + 7;
                    }
                }

                this.menuCopyItem.Visible = allowCPC;
                this.menuCutItem.Visible = allowCPC & allowCut;
                this.menuPasteItem.Visible = allowCPC;
                this.menuViewClipboard.Visible = allowCPC;
                Lab_FilterHasResultItem = UserInfo.GetSysConfigValue("Lab_FilterHasResultItem") == "是";
                Lab_MarkModifyResultWithColor = UserInfo.GetSysConfigValue("Lab_MarkModifyResultWithColor") == "是";
                ApplyConfig(new PatInputPatResultSettingItem());


                List<EntityDefItmProperty> listItmProp = CacheClient.GetCache<EntityDefItmProperty>();
                List<EntityDicResultTips> listTips = CacheClient.GetCache<EntityDicResultTips>();

                DataTable dtResRepType = new DataTable();
                dtResRepType.Columns.Add("value");
                dtResRepType.Columns.Add("name");
                dtResRepType.Rows.Add(new string[] { LIS_Const.PatResultType.Cal, "计算" });
                dtResRepType.Rows.Add(new string[] { LIS_Const.PatResultType.Itr, "仪器" });
                dtResRepType.Rows.Add(new string[] { LIS_Const.PatResultType.Normal, "手工" });
                this.leditResType.DataSource = dtResRepType;

                //计算
                //仪器
                //手工
                this.dict_res_ref_flag = listTips;
                this.DictItemProp = listItmProp;

                this.repositoryItemLookUpEdit2.DataSource = this.dict_res_ref_flag;

                this.patage = -1;

                this.patsex = "1";
                this.samtypeid = string.Empty;
                this.itr_ptype = string.Empty;

                CombineEditor.CombineAdded += new CombineAddedEventHandler(CombineEditor_CombineAdded);
                CombineEditor.CombineRemoved += new CombineRemovedEventHandler(CombineEditor_CombineRemoved);
                CombineEditor.Reseted += new EventHandler(CombineEditor_Reseted);
                CombineEditor.ButtonClicked += new CombineEditBoxButtonClick(CombineEditor_ButtonClicked);
                this.dockPanel2.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                //项目显示默认类型 DefaultItemsShowType

                if (UserInfo.GetSysConfigValue("DefaultItemsShowType") == "全部")
                    radioGroup1.SelectedIndex = 1;

                try
                {
                    int colres_width = 70;
                    string conf_colres_width = ConfigHelper.GetSysConfigValueWithoutLogin("PatResult_colres_width");
                    if (!string.IsNullOrEmpty(conf_colres_width))
                    {
                        colres_width = Convert.ToInt32(conf_colres_width);
                    }

                    //int colres_width =ConfigHelper.GetSysConfigValueWithoutLogin("PatResult_colres_width").ToString());
                    if (colres_width != colres_chr.Width)
                    {
                        colres_chr.Width = colres_width;
                    }
                }
                catch
                {
                    throw;
                }
                showItrWarningMsgConfig = UserInfo.GetSysConfigValue("Lab_ShowItrWarningMsg");
                string Lab_ShowItrWarningMsgWinSizeconfig = UserInfo.GetSysConfigValue("Lab_ShowItrWarningMsgWinSize");

                int width = 0;
                int height = 0;
                if (!string.IsNullOrEmpty(Lab_ShowItrWarningMsgWinSizeconfig))
                {
                    string[] array = Lab_ShowItrWarningMsgWinSizeconfig.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (array.Length == 2)
                    {
                        if (!int.TryParse(array[0], out width))
                        {
                            width = 0;
                        }
                        if (!int.TryParse(array[1], out height))
                        {
                            height = 0;
                        }
                    }
                }

                if (string.IsNullOrEmpty(showItrWarningMsgConfig) || showItrWarningMsgConfig == "不显示")
                {
                    this.dockPanel1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                }
                else
                {
                    this.dockPanel1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
                }
                if (showItrWarningMsgConfig.Contains("底部显示"))
                {
                    if (height > 0)
                    {
                        this.dockPanel1.Height = height;
                    }
                    this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
                    showItrWarningMsg = true;

                }
                else if (showItrWarningMsgConfig.Contains("右边显示"))
                {
                    if (width > 0)
                    {
                        this.dockPanel1.Width = width;
                    }
                    this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
                    showItrWarningMsg = true;

                }
                else if (showItrWarningMsgConfig.Contains("浮动显示"))
                {

                    this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Float;

                    this.dockPanel1.FloatSize = new Size(width, height);
                    System.Drawing.Rectangle rect = System.Windows.Forms.Screen.GetWorkingArea(this);//实例化一个当前窗口的对象
                    this.dockPanel1.FloatLocation = new Point(rect.Width - width - 3, rect.Height - height - 3);
                    showItrWarningMsg = true;
                }
                if (showItrWarningMsgConfig.Contains("自动隐藏"))
                {
                    this.dockPanel1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
                }
                PatResultDoubleColumn.showDoubleItrWarningMsg = showItrWarningMsg;
            }
        }

        /// <summary>
        /// 组合编辑框按键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="button_type"></param>
        void CombineEditor_ButtonClicked(object sender, string button_type)
        {
            switch (button_type.ToLower())
            {
                case "up":
                    RefreshCurrentCombineItems();
                    BindGrid();
                    break;

                case "down":
                    ClearNoneResultItems(false);
                    break;
            }
        }

        /// <summary>
        /// 组合编辑框重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CombineEditor_Reseted(object sender, EventArgs e)
        {
            this.Reset();
        }

        /// <summary>
        /// 组合编辑框移除组合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="com_id"></param>
        void CombineEditor_CombineRemoved(object sender, string com_id)
        {
            bool deleteRes = false;

            if (UserInfo.GetSysConfigValue("Lab_ShowDeleteCombineItem") == "是")
            {
                deleteRes = MessageDialog.Show(string.Format("您确定要删除该组合的所有项目结果？"), "提示",
                                                MessageBoxButtons.YesNo) == DialogResult.Yes;
            }
            RemoveCombineItems(com_id, deleteRes);
            BindGrid();
            RefreshItemsCountText();
            ItemsCountCheck();
            this.NotNullItemCheck();
            radioGroup1_SelectedIndexChanged(this.radioGroup1, EventArgs.Empty);
            this.gridControlSingle.Focus();
        }

        /// <summary>
        /// 组合编辑框添加组合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="com_id"></param>
        void CombineEditor_CombineAdded(object sender, string com_id, int com_seq)
        {
            AddCombineItems(com_id, com_seq);
            BindGrid();

            MoveFirst();
            RefreshItemsCountText();
            ItemsCountCheck();
            this.NotNullItemCheck();
            radioGroup1_SelectedIndexChanged(this.radioGroup1, EventArgs.Empty);
        }
        #endregion

        #region 显示方式配置
        int visiblestyle;

        /// <summary>
        /// 显示方式
        /// 0 = 单列
        /// 1 = 双列
        /// 2 = 树型
        /// </summary>
        public int VisibleStyle
        {
            get
            {
                return visiblestyle;
            }
            set
            {
                if (this.visiblestyle != value)
                {
                    visiblestyle = value;
                    if (this.dtPatientResulto != null)
                    {
                        this.BindGrid();
                    }
                }
            }
        }

        /// <summary>
        /// 双列显示时,只有2个组合的时候是否在左右各显示一个组合内容
        /// </summary>
        public bool EachViewOnlyTwoCombine { get; set; }

        /// <summary>
        /// 双列显示时左侧网格最大显示列数
        /// </summary>
        public int LeftGridMaxRow { get; set; }
        #endregion

        /// <summary>
        /// 病人组合明细
        /// </summary>
        public List<EntityPidReportDetail> PatientsMi
        {
            get
            {
                return patients_mi;
            }
            set
            {
                patients_mi = value;
            }
        }

        public void MergeResult(string pat_id)
        {
            if (string.IsNullOrEmpty(pat_id)) return;
            ProxyPatResult resultProxy = new ProxyPatResult();
            EntityQcResultList qcResult = resultProxy.Service.GetPatientCommonResult(pat_id, true);
            List<EntityObrResult> dtPatRes = qcResult.listResulto;
            EntityPidReportMain dtPatInfo = qcResult.patient;
            List<EntityPidReportDetail> dtComMi = new List<EntityPidReportDetail>();
            if (dtPatInfo != null && !string.IsNullOrEmpty(dtPatInfo.RepId))
            {
                dtComMi = dtPatInfo.ListPidReportDetail;
                this.PatID = dtPatInfo.RepId;
                this.PatInNo = dtPatInfo.PidInNo;
                this.patsex = dtPatInfo.PidSexName;

                if (!Compare.IsNullOrDBNull(dtPatInfo.PidAge))
                {
                    this.patage = Convert.ToInt32(dtPatInfo.PidAge);
                }
                else
                {
                    this.patage = -1;
                }

                this.samtypeid = dtPatInfo.PidSamId;

                if (dtPatInfo.RepStatus != null)
                    this.patFlag = dtPatInfo.RepStatus.Value.ToString().TrimEnd() == string.Empty ? "0" : dtPatInfo.RepStatus.Value.ToString();
            }
            else
                this.patFlag = "0";
            //获取病人结果
            Merge(this.dtPatientResulto, dtPatRes);
            this.patients_mi = dtComMi;

            this.MoveFirst();

            this.SetAutoCalParam();

            ItemsCountCheck();
            this.NotNullItemCheck();

            CalAllRowsItemRef(false, false, false);

            //绑定
            BindGrid();


            //刷新显示项目数
            RefreshItemsCountText();
        }

        private void Merge(List<EntityObrResult> dataTable, List<EntityObrResult> dtPatRes)
        {
            if (dataTable == null || dtPatRes == null)
            {
                return;
            }
            try
            {
                foreach (EntityObrResult result in dataTable)
                {
                    try
                    {
                        EntityObrResult obrResult = dtPatRes.Find(i => i.ItmId == result.ItmId);
                        if (obrResult != null)
                        {
                            dataTable.Add(obrResult);
                        }
                        else
                        {
                            if (result.ObrType != 3)
                            {
                                result.ObrValue = string.Empty;
                                result.ObrValue2 = string.Empty;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Lib.LogManager.Logger.LogException("mergerdate", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException(this.GetType().ToString(), "mergerdate", ex.Message);
            }

        }

        /// <summary>
        /// 加载病人资料
        /// </summary>
        /// <param name="pat_id"></param>
        public void LoadResult(string pat_id, List<EntityPidReportDetail> dtCombine)
        {
            //return;
            ProxyPatResult resultProxy = new ProxyPatResult();
            EntityQcResultList qcResult = resultProxy.Service.GetPatientCommonResult(pat_id, true);
            List<EntityObrResult> dtPatRes = qcResult.listResulto;

            EntityPidReportMain dtPatInfo = qcResult.patient;
            List<EntityPidReportDetail> dtComMi = null;
            if (dtCombine == null || dtCombine.Count < 1)
            {
                if (dtPatInfo != null)
                {
                    dtComMi = dtPatInfo.ListPidReportDetail;
                }
            }
            else
            {
                dtComMi = dtCombine;
            }
            //暂时注释
            LoadResult(dtPatInfo, dtPatRes, dtComMi);
        }

        private EntityPidReportMain dtPatInfoCopy = null;
        /// <summary>
        /// 数据加载入口
        /// </summary>
        /// <param name="dtPatInfo"></param>
        /// <param name="dtPatResult"></param>
        /// <param name="dtPatCombine"></param>
        public void LoadResult(EntityPidReportMain dtPatInfo, List<EntityObrResult> dtPatResult, List<EntityPidReportDetail> dtPatCombine)
        {
            IsPatResultVChange = false;
            listSysOprLog = null;
            this.Reset();
            listSysOprLog = null;
            if (dtPatInfo != null)
            {
                dtPatInfoCopy = dtPatInfo;
                this.PatID = dtPatInfo.RepId;
                this.PatInNo = dtPatInfo.PidInNo;
                this.patsex = dtPatInfo.PidSex;

                if (!Compare.IsNullOrDBNull(dtPatInfo.PidAge))
                {
                    this.patage = Convert.ToInt32(dtPatInfo.PidAge);
                }
                else
                {
                    this.patage = -1;
                }

                this.samtypeid = dtPatInfo.PidSamId;

                if (dtPatInfo.RepRecheckFlag == 1)
                {
                    复查ToolStripMenuItem.Enabled = true;
                }
                else
                {
                    复查ToolStripMenuItem.Enabled = false;
                }

                this.patExp = dtPatInfo.RepRemark;
                if (dtPatInfo.RepStatus != null)
                    this.patFlag = dtPatInfo.RepStatus.Value.ToString() == string.Empty ? "0" : dtPatInfo.RepStatus.Value.ToString();
                else
                    this.patFlag = "0";
            }
            if (Lab_MarkModifyResultWithColor)
            {
                SetModifyFlag(dtPatResult);
            }
            ////获取病人结果
            this.dtPatientResulto = dtPatResult;


            if (Lab_FilterHasResultItem)
            {
                ResultDatatableBak = null;
                ResultDatatableBak = new List<EntityObrResult>();
            }
            if (dtPatCombine != null)
            {
                this.patients_mi = EntityManager<EntityPidReportDetail>.ListClone(dtPatCombine);
            }

            this.SetAutoCalParam();

            ItemsCountCheck();
            this.NotNullItemCheck();

            CalAllRowsItemRef(false, false, false);

            radioGroup1_SelectedIndexChanged(this.radioGroup1, EventArgs.Empty);

            //绑定;
            BindGrid();

            //刷新显示项目数
            RefreshItemsCountText();


            this.MoveFirst();

            if (showPicForm)
            {
                patPhotoList1.LoadPhotos(this.PatID);
                patPhotoList1.SetPanelNoVis();
            }
            if (showItrWarningMsg)
            {
                bool hasMsg = this.itrWarningMsg1.LoadItrWarningMsg(this.PatID, false);
                if (showItrWarningMsgConfig.Contains("自动隐藏"))
                {
                    if (hasMsg)
                    {
                        this.dockPanel1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
                    }
                    else
                    {
                        this.dockPanel1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;

                    }
                }
            }
            gridControlSingle.RefreshDataSource();
            gridViewSingle.RefreshData();
            Thread t = new Thread(ThreadLoadHistory);
            t.Start();
            //ThreadLoadHistory();
        }
        List<EntityObrResult> listHisResult = new List<EntityObrResult>();
        private void ThreadLoadHistory()
        {
            try
            {
                if (!string.IsNullOrEmpty(PatID))
                {
                    listHisResult = new ProxyPatResult().Service.GetPatCommonResultHistoryWithRef(PatID, 10, Convert.ToDateTime("2100-01-01"));

                    if (listHisResult == null || listHisResult.Count == 0 || listHisResult[0].PidInNo != PatInNo || this.gridControlSingle == null) return;
                    SetHistoryRes(this.gridControlSingle, listHisResult);
                    this.gridControlSingle.RefreshDataSource();
                }
            }
            catch (Exception EX)
            {
                Lib.LogManager.Logger.LogException(EX);
            }
        }
        private delegate void DelegateSetPatStatus(GridControl grid, List<EntityObrResult> data);
        private void SetHistoryRes(GridControl grid, List<EntityObrResult> data)
        {
            if (grid.InvokeRequired)
            {
                DelegateSetPatStatus del = new DelegateSetPatStatus(SetHistoryRes);
                this.Invoke(del, new object[] { grid, data });
            }
            else
            {
                try
                {
                    if (grid.DataSource != null && grid.DataSource is List<EntityObrResult>)
                    {

                        List<EntityObrResult> source = grid.DataSource as List<EntityObrResult>;
                        if (source.Count == 0) return;
                        if (data != null && data.Count > 0 && data[0].PidInNo != source[0].PidInNo) return;//多线程的缘故会导致审核完成后切换病人时不一致

                        StringBuilder sbResultId = new StringBuilder();
                        foreach (EntityObrResult drResult in source)
                        {
                            sbResultId.Append(string.Format(",'{0}'", drResult.ItmId));
                        }
                        sbResultId.Remove(0, 1);
                        string itmIds = sbResultId.ToString();

                        //选出包含于项目列表的病人结果并根据病人ID去重
                        List<EntityObrResult> dtTimes = (from x in data where itmIds.Contains(x.ItmId) select x)
                            .GroupBy(i => i.ObrId).Select(i => i.First()).OrderByDescending(i => i.ObrDate).ToList();


                        try
                        {
                            if (dtTimes.Count > 0)
                            {
                                string columnNAME = string.Empty;
                                for (int j = 0; j < dtTimes.Count; j++)
                                {
                                    EntityObrResult drTime = dtTimes[j];
                                    foreach (EntityObrResult drResult in source)
                                    {
                                        if (!string.IsNullOrEmpty(drResult.ItmId))
                                        {
                                            string itm_id = drResult.ItmId.ToString();

                                            int resultIndex = data.FindIndex(i => i.ObrId == drTime.ObrId && i.ItmId == itm_id);
                                            if (resultIndex != -1)
                                            {
                                                EntityObrResult result = data[resultIndex];
                                                string strTime = result.ObrDate.ToString("yy/MM/dd");
                                                if (j == 0)
                                                {
                                                    drResult.HistoryResult1 = result.ObrValue;
                                                    drResult.HistoryDate1 = result.ObrDate.ToString("yyyy-MM-dd HH:mm:ss");
                                                }
                                                else if (j == 1)
                                                {
                                                    drResult.HistoryResult2 = result.ObrValue;
                                                    drResult.HistoryDate2 = result.ObrDate.ToString("yyyy-MM-dd HH:mm:ss");
                                                }
                                                else
                                                {
                                                    drResult.HistoryResult3 = result.ObrValue;
                                                    drResult.HistoryDate3 = result.ObrDate.ToString("yyyy-MM-dd HH:mm:ss");
                                                }
                                                columnNAME = "HistoryResult" + (j + 1).ToString();
                                                if (this.gridViewSingle.Columns.FirstOrDefault(i => i.FieldName == columnNAME) != null)
                                                    this.gridViewSingle.Columns[columnNAME].Caption = "历史结果" + "\r\n" + strTime;
                                            }
                                            CalcPatResultRow(drResult, true);
                                        }
                                    }
                                }
                                #region 过滤"必录+历史结果1
                                if (UserInfo.GetSysConfigValue("Lab_PopedomFilter") == "必录+历史结果" && (patFlag != "2" && patFlag != "4"))
                                {
                                    List<EntityObrResult> dataFilter = PatientHistoryViewFilter(data);
                                    if (dtPatientResulto == null || dtPatientResulto.Count == 0) return;
                                    List<string> listItmId = new List<string>();
                                    if (dataFilter != null && dataFilter.Count > 0)
                                    {
                                        foreach (EntityObrResult item in dataFilter)
                                        {
                                            //找出没有显示历史结果1的项目
                                            if (dtPatientResulto.FindAll(w => w.ItmId == item.ItmId).Count <= 0)
                                                listItmId.Add(item.ItmId);
                                        }
                                    }
                                    int count = 0;
                                    int seq = dtPatientResulto.Count;
                                    //防止重复添加
                                    if (listItmId.Count > 0)
                                    {
                                        for (int i = 0; i < listItmId.Count; i++)
                                        {
                                            EntityObrResult insertEntity = new EntityObrResult();
                                            //存在则不添加
                                            if (source.FindAll(w => w.ItmId == listItmId[count]).Count > 0) continue;

                                            List<EntityObrResult> dtPatResult = dataFilter.FindAll(w => w.ItmId == listItmId[count]);
                                            if (dtPatResult.Count > 0)
                                            {
                                                insertEntity.ObrId = PatID;
                                                insertEntity.ComMiSort = seq + 1;
                                                insertEntity.ResComSeq = 999;
                                                insertEntity.ItmName = dtPatResult[0].ItmName;
                                                insertEntity.ItmEname = dtPatResult[0].ItmEname.TrimEnd();
                                                insertEntity.ItmComId = "-1";
                                                if (!string.IsNullOrEmpty(dtPatResult[0].RefLowerLimit) && !string.IsNullOrEmpty(dtPatResult[0].RefUpperLimit))
                                                {
                                                    insertEntity.ResRefRange = dtPatResult[0].RefLowerLimit + "-" + dtPatResult[0].RefUpperLimit;
                                                }
                                                else if (!string.IsNullOrEmpty(dtPatResult[0].RefLowerLimit) && string.IsNullOrEmpty(dtPatResult[0].RefUpperLimit))
                                                {
                                                    insertEntity.ResRefRange = dtPatResult[0].RefLowerLimit;
                                                }
                                                else if (string.IsNullOrEmpty(dtPatResult[0].RefLowerLimit) && !string.IsNullOrEmpty(dtPatResult[0].RefUpperLimit))
                                                {
                                                    insertEntity.ResRefRange = dtPatResult[0].RefUpperLimit;

                                                }
                                                insertEntity.RefLowerLimit = dtPatResult[0].RefLowerLimit;
                                                insertEntity.RefUpperLimit = dtPatResult[0].RefUpperLimit;
                                                insertEntity.ResRefLCal = dtPatResult[0].RefLowerLimit;
                                                insertEntity.ResRefHCal = dtPatResult[0].RefUpperLimit;
                                                insertEntity.HistoryResult1 = dtPatResult[0].ObrValue;
                                                insertEntity.ObrType = dtPatResult[0].ObrType;
                                                insertEntity.ObrRecheckFlag = dtPatResult[0].ObrRecheckFlag;
                                                insertEntity.IsNew = 1;
                                                insertEntity.ObrDate = ServerDateTime.GetServerDateTime();
                                                source.Add(insertEntity);
                                                count++;
                                                seq++;
                                            }

                                        }
                                        foreach (EntityObrResult dr in source)
                                        {
                                            CalcPatResultRow(dr, true);
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteException(this.GetType().Name, "reshistroyfill", ex.ToString());
                        }

                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
                finally
                {

                }

            }
        }
        /// <summary>
        /// 历史结果过滤不包含此组合的项目
        /// </summary>
        /// <param name="listPatientHistory"></param>
        private List<EntityObrResult> PatientHistoryViewFilter(List<EntityObrResult> listPatientHistory)
        {
            List<EntityObrResult> listHistroyResult = new List<EntityObrResult>();
            string comIds = string.Empty;
            if (patients_mi != null && patients_mi.Count > 0)
            {
                foreach (EntityPidReportDetail detail in this.patients_mi)
                {
                    string com_id = detail.ComId;
                    if (!comIds.Contains(com_id))
                    {
                        comIds += string.Format(",'{0}'", com_id);
                    }
                }
            }
            if (string.IsNullOrEmpty(comIds))
                return listHistroyResult;
            comIds = comIds.Remove(0, 1);
            string sbItmId = string.Empty;
            List<EntityDicCombineDetail> listComDetailCache = CacheClient.GetCache("EntityDicCombineDetail") as List<EntityDicCombineDetail>;
            List<EntityDicCombineDetail> listComDetail = (from x in listComDetailCache where comIds.Contains(x.ComId) select x).ToList();
            if (listComDetail != null && listComDetail.Count > 0)
            {
                foreach (EntityDicCombineDetail detail in listComDetail)
                {
                    sbItmId += string.Format(",'{0}'", detail.ComItmId);
                }
            }
            if (string.IsNullOrEmpty(sbItmId))
                return listHistroyResult;
            sbItmId = sbItmId.Remove(0, 1);

            listHistroyResult = (from x in listPatientHistory where sbItmId.Contains(x.ItmId) select x).OrderByDescending(w => w.ObrDate).ToList();
            return listHistroyResult;
        }
        private void SetModifyFlag(List<EntityObrResult> dtPatResult)
        {
            if (dtPatResult != null && dtPatResult.Count > 0)
            {
                GetSysoperationlog();
                if (listSysOprLog != null && (listSysOprLog.FindAll(i => i.OperatAction == "修改").Count > 0))
                {
                    foreach (EntityObrResult result in dtPatResult)
                    {
                        string ecd = !string.IsNullOrEmpty(result.ItmEname.TrimEnd()) ? result.ItmEname.TrimEnd() : "";
                        if (!string.IsNullOrEmpty(result.ItmEname.TrimEnd()) &&
                            listSysOprLog.FindAll(i => i.OperatAction == "修改" && i.OperatObject == ecd).Count > 0)
                        {
                            result.IsModify = 1;
                        }
                    }
                }
            }
        }


        private void GetSysoperationlog()
        {
            if (!string.IsNullOrEmpty(this.PatID) && listSysOprLog == null)
            {
                ProxySysOperationLog proxy = new ProxySysOperationLog();
                EntityLogQc qc = new EntityLogQc();
                qc.Operatkey = PatID;
                qc.OperatModule = "病人资料";
                listSysOprLog = proxy.Service.GetOperationLog(qc);
            }
        }


        private void SetAutoCalParam()
        {
            if (this.dtPatientResulto != null && this.dtPatientResulto.Count > 0)
            {
                //填充自动计算项目信息
                foreach (EntityObrResult result in this.dtPatientResulto)
                {
                    int restype = result.ObrTypeShow;

                    //系统配置：关闭自动关联计算项目的功能
                    bool IsStopCalItem = (ConfigHelper.GetSysConfigValueWithoutLogin("Lab_stopCalItem") == "是");

                    if (restype == 2 && (!IsStopCalItem))
                    {
                        result.CalculateDestItmId = string.Empty;

                        string itm_id = result.ItmId;
                        string itm_ecd = result.ItmEname.TrimEnd();
                        string calitems = DictClItem.Instance.GetCalItems(itm_id);

                        result.CalculateSourceItmId = calitems;

                        foreach (string cal_item in calitems.Split(','))
                        {
                            List<EntityObrResult> results = this.dtPatientResulto.FindAll(i => i.ItmEname.TrimEnd().TrimEnd() == cal_item);

                            foreach (EntityObrResult obrResult in results)
                            {
                                if (!string.IsNullOrEmpty(obrResult.CalculateDestItmId) && obrResult.CalculateDestItmId.TrimEnd() != string.Empty)
                                {
                                    obrResult.CalculateDestItmId = obrResult.CalculateDestItmId + "," + itm_id;
                                }
                                else
                                {
                                    obrResult.CalculateDestItmId = itm_id;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void NotNullItemCheck()
        {
            if (this.patients_mi != null && this.patients_mi.Count > 0)
            {
                //必录项目
                List<string> listNotNullItem = new List<string>();

                List<string> listNotNullItemHasResult = new List<string>();

                //遍历当前病人检验组合
                foreach (EntityPidReportDetail drPatComMi in this.patients_mi)
                {
                    string com_id = drPatComMi.ComId.ToString();

                    //查找组合所有检验项目
                    List<EntityDicCombineDetail> dtComItems = DictCombineMi.Instance.GetCombineMi(com_id, this.patsex);

                    foreach (EntityDicCombineDetail drComItem in dtComItems)
                    {
                        string com_itm_id = drComItem.ComItmId;
                        string com_itm_ecd = SQLFormater.Format(drComItem.ComItmEname);

                        if (!listNotNullItem.Exists(i => i == com_itm_id))
                        {
                            if (!string.IsNullOrEmpty(drComItem.ComMustItem))
                            {
                                if (Convert.ToInt32(drComItem.ComMustItem) == 1)
                                {
                                    listNotNullItem.Add(com_itm_id);

                                    if (this.dtPatientResulto != null && this.dtPatientResulto.FindAll(i => i.ItmId == com_itm_id && !string.IsNullOrEmpty(i.ObrValue)).Count > 0)
                                    {
                                        listNotNullItemHasResult.Add(com_itm_id);
                                    }
                                }
                            }
                        }
                    }
                }


                this.lblNotEmptyItem.Text = string.Format("项目数:{0}/{1}", listNotNullItemHasResult.Count, listNotNullItem.Count);
            }
            else
            {
                this.lblNotEmptyItem.Text = string.Format("项目数:{0}/{0}", 0);
            }

            if (NullItemInfo != null)
            {
                CalInfoEventArgs arg = new CalInfoEventArgs();

                arg.TipText = this.lblNotEmptyItem.Text;
                NullItemInfo("", arg);
            }
        }


        /// <summary>
        /// 漏项检查
        /// </summary>
        public void ItemsCountCheck()
        {
            if (this.patients_mi != null && this.patients_mi.Count > 0)
            {
                if (this.dtPatientResulto == null)
                {
                    this.lblLostItems.Visible = true;
                }
                else
                {
                    //缺少项目数
                    int lostItemCount = 0;

                    //遍历当前病人检验组合
                    foreach (EntityPidReportDetail PatComMi in this.patients_mi)
                    {
                        string com_id = PatComMi.ComId.ToString();

                        //查找组合所有检验项目
                        List<EntityDicCombineDetail> ComItems = DictCombineMi.Instance.GetCombineMi(com_id, this.patsex);

                        foreach (EntityDicCombineDetail drComItem in ComItems)
                        {
                            string com_itm_id = drComItem.ComItmId;
                            string com_itm_ecd = SQLFormater.Format(drComItem.ComItmEname);

                            //对比当前项目
                            List<EntityObrResult> drsPatResult = this.dtPatientResulto.FindAll(i => (i.ItmId == com_itm_id || i.ItmEname.TrimEnd() == com_itm_ecd)
                                                                                                && (i.IsNew != 2));
                            if (drsPatResult.Count == 0)
                            {
                                lostItemCount++;
                            }
                        }
                    }

                    if (lostItemCount > 0)
                    {
                        this.lblLostItems.Visible = true;
                    }
                    else
                    {
                        this.lblLostItems.Visible = false;
                    }
                }
            }
            else
            {
                this.lblLostItems.Visible = false;
            }
        }

        /// <summary>
        /// 添加组合对应的项目
        /// </summary>
        /// <param name="com_id">组合ID</param>
        private void AddCombineItems(string com_id, int com_seq)
        {
            CloseEditor();

            //根据组合ID获取组合信息
            EntityDicCombine drCombine = DictCombine.Instance.GetCombinebyID(com_id);

            if (drCombine != null)
            {
                string com_name = drCombine.ComName;

                //获取组合包含的项目
                List<EntityDicCombineDetail> dtComItems = DictCombineMi.Instance.GetCombineMi(com_id, this.patsex);


                if (dtComItems.Count > 0)
                {
                    List<string> itemsID = new List<string>();

                    //遍历
                    foreach (EntityDicCombineDetail drComItem in dtComItems)
                    {
                        if ((this.radioGroup1.EditValue.ToString() == "0" && drComItem.ComMustItem == "1")
                            || this.radioGroup1.EditValue.ToString() == "1"
                            )
                        {
                            if (!string.IsNullOrEmpty(drComItem.ComItmId))
                            {
                                string itm_id = drComItem.ComItmId;
                                if (dtPatientResulto != null && dtPatientResulto.FindAll(i => i.ItmId == itm_id && i.IsNew != 2).Count == 0)
                                    itemsID.Add(itm_id);//添加到ID集合
                            }
                        }
                    }
                    if (itemsID.Count == 0) return;
                    ////获取参考值：年龄/性别为空时的计算规则
                    //string configCalAge = UserInfo.GetSysConfigValue("GetRefOnNullAge");
                    //string configCalSex = UserInfo.GetSysConfigValue("GetRefOnNullSex");

                    //int calage = -1;

                    //if (!string.IsNullOrEmpty(configCalAge)
                    //    && configCalAge != "不计算参考值")
                    //{
                    //    calage = AgeConverter.YearToMinute(20);
                    //    if (this.patage >= 0)
                    //    {
                    //        calage = this.patage;
                    //    }
                    //}

                    //获取项目：项目信息、项目样本信息、参考值
                    List<EntityItmRefInfo> listItemRefInfo = new ProxyPatResult().Service.GetItemRefInfo(itemsID,
                                                                this.samtypeid,
                                                                GetConfigOnNullAge(this.patage),
                                                                GetConfigOnNullSex(this.patsex),
                                                                this.samrem, this.Pat_itr_id, this.Pat_dep_id, "");
                    //GetSysoperationlog();

                    foreach (string itm_id in itemsID)
                    {
                        List<EntityItmRefInfo> drItems = listItemRefInfo.FindAll(i => i.ItmId == itm_id);

                        if (drItems.Count > 0)
                        {
                            EntityItmRefInfo drItem = drItems[0];

                            string defValue = null;

                            List<EntityDicCombineDetail> drsCombineMi = dtComItems.FindAll(i => i.ComItmId == itm_id);

                            if (!Compare.IsNullOrDBNull(drItem.ItmDefault))
                            {
                                defValue = drItem.ItmDefault;
                            }

                            EntityDicCombineDetail drCombineMi = null;
                            if (drsCombineMi.Count > 0)
                            {
                                drCombineMi = drsCombineMi[0];
                            }
                            AddItem(drItem, drCombineMi, com_id, com_name, com_seq, defValue, null);
                        }
                    }

                }
            }

            SetAutoCalParam();
        }



        /// <summary>
        /// 添加缺失项目
        /// </summary>
        public void RefreshCurrentCombineItems()
        {
            if (patients_mi == null)
                return;
            foreach (EntityPidReportDetail item in this.patients_mi)
            {
                int com_seq = 0;

                if (!Compare.IsNullOrDBNull(item.ComSeq)
                    && item.ComSeq.TrimEnd(null) != string.Empty)
                {
                    //如果有2个以上组合序号为0时,则按照patients_mi的pat_seq排序
                    if (item.SortNo != null && !string.IsNullOrEmpty(item.SortNo.ToString()) && patients_mi.FindAll(i => i.ComSeq == "0").Count > 1)
                    {
                        com_seq = item.SortNo.Value;
                    }
                    else
                    {
                        com_seq = Convert.ToInt32(item.ComSeq);
                    }
                }
                else if (!Compare.IsNullOrDBNull(item.SortNo) && !string.IsNullOrEmpty(item.SortNo.Value.ToString()))//2012-12-21
                {
                    com_seq = item.SortNo.Value;
                }
                else
                {
                    com_seq = 99999;
                }
                AddCombineItems(item.ComId, com_seq);
            }

            this.RefreshItemsCountText();
        }

        /// <summary>
        /// 清除无结果项目
        /// <param name="keepNullItem">是否保留必录项目</param>
        /// </summary>
        public void ClearNoneResultItems(bool keepNullItem)
        {
            if (dtPatientResulto == null)
                return;
            for (int i = this.dtPatientResulto.Count - 1; i >= 0; i--)
            {
                EntityObrResult dr = dtPatientResulto[i];
                if (!string.IsNullOrEmpty(dr.ObrValue))
                {

                }
                else
                {
                    if (!keepNullItem)
                    {
                        dtPatientResulto.Remove(dr);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(dr.IsNotEmpty) && dr.IsNotEmpty != "1")
                        {
                            dtPatientResulto.Remove(dr);
                        }
                    }
                }
                this.gridControlSingle.RefreshDataSource();
            }
            RefreshItemsCountText();
            this.ItemsCountCheck();
            this.NotNullItemCheck();
        }


        #region 绑定
        /// <summary>
        /// 绑定
        /// </summary>
        private void BindGrid()
        {
            if (this.dtPatientResulto != null)
            {
                CloseEditor();

                if (AutoDouble && !FixSingleColumn)
                {
                    if (dtPatientResulto.Count < 20)
                    {
                        if (!gridControlSingle.Visible)
                        {
                            gridControlDouble.Visible = false;
                            gridControlSingle.Visible = true;
                            gridControlSingle.Dock = DockStyle.Fill;
                        }
                        BindSingleGrid();
                    }
                    else
                    {
                        if (!gridControlDouble.Visible)
                        {
                            gridControlSingle.Visible = false;
                            gridControlDouble.Visible = true;
                            gridControlDouble.Dock = DockStyle.Fill;
                        }
                        gridControlDouble.PatientsMi = PatientsMi;
                        if (PatID == string.Empty && dtPatInfoCopy != null)
                            gridControlDouble.LoadResult(EntityManager<EntityPidReportMain>.EntityClone(dtPatInfoCopy), dtPatientResulto, true);
                        else
                            gridControlDouble.LoadResult(dtPatInfoCopy, dtPatientResulto, true);
                    }
                }
                else
                {
                    BindSingleGrid();
                }
            }
        }

        /// <summary>
        /// 绑定数据到单网格
        /// </summary>
        private void BindSingleGrid()
        {
            this.gridViewSingle.ClearGrouping();
            this.gridViewSingle.ClearSorting();
            this.gridViewSingle.Columns["ResComName"].Visible = false;
            if (Config != null && Config.OrderByItmSeq)
            {
                this.dtPatientResulto = this.dtPatientResulto.OrderBy(i => i.ItmSeq).ThenBy(i => i.ItmEname.TrimEnd()).ToList();
            }
            else
            {
                this.dtPatientResulto = this.dtPatientResulto.OrderBy(i => i.ResComSeq).ThenBy(i => i.ComMiSort).ThenBy(i => i.ItmSeq).ThenBy(i => i.ItmEname.TrimEnd()).ToList();
            }
            this.gridControlSingle.DataSource = this.dtPatientResulto;
            gridViewSingle.RefreshData();
            gridControlSingle.RefreshDataSource();
            if (dtPatientResulto != null && dtPatientResulto.Count > 0)
            {
                SetHistoryDate("1");
                SetHistoryDate("2");
                SetHistoryDate("3");
            }
        }

        /// <summary>
        /// 显示历史结果
        /// </summary>
        /// <param name="index"></param>
        private void SetHistoryDate(string index)
        {
            List<EntityObrResult> listResult = new List<EntityObrResult>();
            try
            {
                if (index == "1")
                    listResult = dtPatientResulto.FindAll(i => !string.IsNullOrEmpty(i.HistoryDate1));
                else if (index == "2")
                    listResult = dtPatientResulto.FindAll(i => !string.IsNullOrEmpty(i.HistoryDate2));
                else
                    listResult = dtPatientResulto.FindAll(i => !string.IsNullOrEmpty(i.HistoryDate3));

                if (listResult.Count > 0)
                {
                    DateTime dt = new DateTime();
                    if (index == "1")
                        dt = Convert.ToDateTime(listResult[0].HistoryDate1);
                    else if (index == "2")
                        dt = Convert.ToDateTime(listResult[0].HistoryDate2);
                    else
                        dt = Convert.ToDateTime(listResult[0].HistoryDate3);

                    gridViewSingle.Columns["HistoryResult" + index].Caption = "历史" + index + "\r\n" + dt.ToString(CommonValue.DateTimeFormat);
                }
                else
                {
                    gridViewSingle.Columns["HistoryResult" + index].Caption = "历史" + index;
                }
            }
            catch (Exception)
            {
            }
        }


        #endregion

        /// <summary>
        /// 移除组合
        /// </summary>
        /// <param name="com_id"></param>
        public void RemoveCombineItems(string com_id, bool removeHasResult)
        {
            CloseEditor();

            List<EntityObrResult> drs = new List<EntityObrResult>();
            if (this.dtPatientResulto != null)
                drs = this.dtPatientResulto.FindAll(i => i.ItmComId == com_id);

            this.RemoveItem(drs, removeHasResult);

            if (Lab_FilterHasResultItem && ResultDatatableBak != null && ResultDatatableBak.Count > 0)
            {
                for (int i = ResultDatatableBak.Count - 1; i >= 0; i--)
                {
                    EntityObrResult dr = ResultDatatableBak[i];
                    if (!string.IsNullOrEmpty(dr.ItmComId) && dr.ItmComId == com_id)
                    {
                        ResultDatatableBak.Remove(dr);
                    }
                }
            }

            //重新绑定
            BindGrid();

            ItemsCountCheck();
            this.NotNullItemCheck();
            RefreshItemsCountText();
        }


        #region 添加项目

        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="itm_id"></param>
        /// <returns></returns>
        public EntityObrResult AddItem(string itm_id)
        {
            List<EntityItmRefInfo> listItmRefInfo = new ProxyPatResult().Service.GetItemRefInfo(new List<string> { itm_id }, this.samtypeid, GetConfigOnNullAge(this.patage), GetConfigOnNullSex(this.patsex), this.samrem, this.Pat_itr_id, Pat_dep_id, "");
            if (listItmRefInfo.Count > 0)
            {
                EntityItmRefInfo drItem = listItmRefInfo[0];

                string defValue = string.Empty;
                if (!Compare.IsNullOrDBNull(drItem.ItmDefault))
                {
                    defValue = drItem.ItmDefault;
                }

                //在病人组合中是否有包含次项目
                bool bItemInCombine = false;
                EntityObrResult drAddedItem = null;
                foreach (EntityPidReportDetail drCom in this.PatientsMi)
                {
                    string com_id = drCom.ComId;
                    string com_name = drCom.PatComName;
                    int com_seq = 0;
                    if (drCom.SortNo != null)
                        com_seq = Convert.ToInt32(drCom.SortNo);

                    List<EntityDicCombineDetail> dtComItems = DictCombineMi.Instance.GetCombineMi(com_id, this.patsex);

                    foreach (EntityDicCombineDetail drComItem in dtComItems)
                    {
                        if (!string.IsNullOrEmpty(drComItem.ComItmId) && drComItem.ComItmId == itm_id)
                        {
                            bItemInCombine = true;
                            drAddedItem = AddItem(drItem, drComItem, com_id, com_name, com_seq, defValue, null);
                            break;
                        }
                    }

                    if (bItemInCombine == true)
                    {
                        break;
                    }
                }

                if (bItemInCombine == true)
                {

                }
                else
                {
                    drAddedItem = AddItem(drItem, null, string.Empty, string.Empty, 999, defValue, null);
                }
                return drAddedItem;
            }
            return null;
        }

        /// <summary>
        /// 添加项目，带结果
        /// </summary>
        /// <param name="itm_id"></param>
        /// <param name="res_chr"></param>
        /// <param name="res_od_chr"></param>
        /// <returns></returns>
        public EntityObrResult AddItem(string itm_id, string res_chr, string res_od_chr)
        {
            ////获取参考值：年龄/性别为空时的计算规则
            List<EntityItmRefInfo> listItmRefInfo = new ProxyPatResult().Service.GetItemRefInfo(new List<string> { itm_id }, this.samtypeid, GetConfigOnNullAge(this.patage), GetConfigOnNullSex(this.patsex), this.samrem, this.Pat_itr_id, Pat_dep_id, "");
            if (listItmRefInfo.Count > 0)
            {
                EntityItmRefInfo drItem = listItmRefInfo[0];

                string defValue = string.Empty;
                if (!Compare.IsNullOrDBNull(drItem.ItmDefault))
                {
                    defValue = drItem.ItmDefault;
                }

                return AddItem(drItem, null, string.Empty, string.Empty, 9999, res_chr, res_od_chr);
            }
            return null;
        }

        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="drItem"></param>
        public EntityObrResult AddItem(EntityItmRefInfo drItem, EntityDicCombineDetail drComMi, string com_id, string com_name, int com_seq, string res_chr, string res_od_chr)
        {
            if (drItem != null)
            {
                string itm_id = drItem.ItmId;

                //项目编号
                string itm_ecd = string.Empty;
                if (!string.IsNullOrEmpty(drItem.ItmEcode))
                {
                    itm_ecd = drItem.ItmEcode;
                }

                string strEcd = SQLFormater.Format(itm_ecd.TrimEnd());

                //查找当前病人结果表中的项目是否已存在
                List<EntityObrResult> drsResultItems = dtPatientResulto.FindAll(i => i.ItmId == itm_id || i.ItmEname.TrimEnd() == strEcd);

                EntityObrResult drResultItem = null;
                if (drsResultItems.Count == 0)
                {
                    drResultItem = new EntityObrResult();
                    FillItemToResult(drResultItem, drItem, drComMi, com_id, com_name, itm_id, itm_ecd, res_chr, res_od_chr);
                    drResultItem.IsNew = 1;
                    drResultItem.ResComSeq = com_seq;
                    this.dtPatientResulto.Add(drResultItem);
                }
                else
                {
                    EntityObrResult drResultExistItem = drsResultItems[0];
                    int row_state = drResultExistItem.IsNew;


                    if (row_state == 2)//需要添加的项目为已被删除的项目
                    {
                        this.dtPatientResulto.Remove(drResultExistItem);//先把被删除(隐藏)的项目移除，再添加

                        drResultItem = new EntityObrResult();
                        FillItemToResult(drResultItem, drItem, drComMi, com_id, com_name, itm_id, itm_ecd, res_chr, res_od_chr);
                        drResultItem.ResComSeq = com_seq;
                        drResultItem.IsNew = 0;
                        this.dtPatientResulto.Add(drResultItem);
                    }
                    else
                    {
                        drResultItem = drsResultItems[0];
                        if (drResultItem.ObrSn == 0)
                        {
                            drResultItem.IsNew = 1;
                        }
                        else
                        {
                            drResultItem.IsNew = 0;
                        }
                    }
                }

                this.gridControlSingle.RefreshDataSource();
                return drResultItem;
            }
            return null;
        }
        #endregion

        private void FillItemToResult(EntityObrResult drResultItem, EntityItmRefInfo drItem, EntityDicCombineDetail drComMi, string com_id, string com_name, string itm_id, string itm_ecd, string res_chr, string res_od_chr)
        {
            drResultItem.ObrFlag = 1;

            //项目名称
            string itm_name = string.Empty;
            if (!string.IsNullOrEmpty(drItem.ItmName))
            {
                itm_name = drItem.ItmName;
            }

            //单位
            string itm_unit = string.Empty;
            if (!string.IsNullOrEmpty(drItem.ItmUnit))
            {
                itm_unit = drItem.ItmUnit;
            }

            string itm_rep_ecd = string.Empty;
            if (!Compare.IsNullOrDBNull(drItem.ItmRepCode) && drItem.ItmRepCode.TrimEnd() != string.Empty)
            {
                itm_rep_ecd = drItem.ItmRepCode;
            }
            else
            {
                itm_rep_ecd = itm_ecd;
            }
            drResultItem.ObrDate = ServerDateTime.GetServerDateTime();
            drResultItem.ObrId = this.PatID;
            drResultItem.ItmId = itm_id;
            drResultItem.ObrItmMethod = drItem.ItmMethod;
            drResultItem.ItmEname = itm_ecd.TrimEnd();
            drResultItem.ItmReportCode = itm_rep_ecd;
            drResultItem.ItmName = itm_name;
            drResultItem.ObrUnit = itm_unit;
            drResultItem.ResComName = com_name;
            drResultItem.ItmDtype = drItem.ItmResType;
            drResultItem.ItmMaxDigit = drItem.ItmMaxDigit;

            if (drItem.ItmCaluFlag == 1)
            {
                drResultItem.ObrType = Convert.ToInt32(LIS_Const.PatResultType.Cal);
            }
            else if (!string.IsNullOrEmpty(res_chr))
            {
                drResultItem.ObrType = 3;
            }
            else
            {
                drResultItem.ObrType = Convert.ToInt32(LIS_Const.PatResultType.Normal);
            }

            drResultItem.ItmSeq = drItem.ItmSortNo;
            drResultItem.ItmPrice = drItem.ItmPrice;
            drResultItem.ItmComId = com_id;

            if (!string.IsNullOrEmpty(drItem.ItmPositiveRes))
                drResultItem.ResPositiveResult = drItem.ItmPositiveRes;

            if (!string.IsNullOrEmpty(drItem.ItmUrgentRes))
                drResultItem.ResCustomCriticalResult = drItem.ItmUrgentRes;

            if (!string.IsNullOrEmpty(drItem.ItmResultAllow))
                drResultItem.ResAllowValues = drItem.ItmResultAllow;

            drResultItem.ResMax = drItem.ItmMaxValue;
            drResultItem.ResMin = drItem.ItmMinValue;

            drResultItem.ResPanH = drItem.ItmDangerUpperLimit;
            drResultItem.ResPanL = drItem.ItmDangerLowerLimit;

            drResultItem.RefUpperLimit = drItem.ItmUpperLimitValue;
            drResultItem.RefLowerLimit = drItem.ItmLowerLimitValue;



            drResultItem.ResMaxCal = drItem.ItmMaxValueCal;
            drResultItem.ResMinCal = drItem.ItmMinValueCal;

            drResultItem.ResPanHCal = drItem.ItmDangerUpperLimitCal;
            drResultItem.ResPanLCal = drItem.ItmDangerLowerLimitCal;

            drResultItem.ResRefHCal = drItem.ItmUpperLimitValueCal;
            if (drResultItem.ResRefHCal == null) drResultItem.ResRefHCal = string.Empty;

            drResultItem.ResRefLCal = drItem.ItmLowerLimitValueCal;
            if (drResultItem.ResRefLCal == null) drResultItem.ResRefLCal = string.Empty;

            if (drResultItem.ResRefLCal.TrimEnd() != string.Empty
                && drResultItem.ResRefHCal.TrimEnd() != string.Empty)
            {
                drResultItem.ResRefRange = drResultItem.ResRefLCal.ToString().TrimEnd() + " - " + drResultItem.ResRefHCal.ToString().TrimEnd();
            }
            else
            {
                drResultItem.ResRefRange = drResultItem.ResRefLCal.ToString().TrimEnd() + drResultItem.ResRefHCal.ToString().TrimEnd();
            }

            if (drComMi != null)
            {
                drResultItem.IsNotEmpty = drComMi.ComMustItem;
                drResultItem.ComMiSort = drComMi.ComSortNo;
            }
            //2012年7月6日9:53:53  录入之后删除，然后再新增此样本号时会没有默认值的问题

            if (listSysOprLog != null && listSysOprLog.Count > 0)
            {
                if (listSysOprLog.FindAll(i => i.OperatAction != "新增" && i.OperatObject == itm_ecd.Replace("'", "''")).Count < 1)
                    drResultItem.ObrValue = res_chr;
            }
            else
                drResultItem.ObrValue = res_chr;


            if (this.Itr_rep_flag == LIS_Const.InstmtDataType.Eiasa)
            {
                drResultItem.ObrValue2 = res_od_chr;
            }
        }


        /// <summary>
        /// 传入当前病人结果行,填充参考值阈值危机值
        /// </summary>
        /// <param name="drResultItem"></param>
        public void GetSingleItemRef(EntityObrResult drResultItem)
        {
            string itm_ecd = drResultItem.ItmEname.TrimEnd().ToString();
            string itm_id = drResultItem.ItmId.ToString();
            string res_itr_id = drResultItem.ObrItrId?.ToString();

            ProxyPatResult patResProxy = new ProxyPatResult();
            List<string> items = new List<string>() { itm_id };
            List<EntityItmRefInfo> listRefInfo = patResProxy.Service.GetItemRefInfo(items, samtypeid, GetConfigOnNullAge(this.patage), GetConfigOnNullSex(this.patsex), "", res_itr_id, this.Pat_dep_id, this.PatDiag);
            EntityItmRefInfo itemRefinfo = listRefInfo.FirstOrDefault();
            if (itemRefinfo != null)
            {
                drResultItem.RefLowerLimit = itemRefinfo.ItmLowerLimitValue;
                drResultItem.RefUpperLimit = itemRefinfo.ItmUpperLimitValue;

                drResultItem.ResMax = itemRefinfo.ItmMaxValue;
                drResultItem.ResMin = itemRefinfo.ItmMinValue;

                drResultItem.ResPanH = itemRefinfo.ItmDangerUpperLimit;
                drResultItem.ResPanL = itemRefinfo.ItmDangerLowerLimit;


                drResultItem.ResRefLCal = itemRefinfo.ItmLowerLimitValueCal;
                drResultItem.ResRefHCal = itemRefinfo.ItmUpperLimitValueCal;

                drResultItem.ResMaxCal = itemRefinfo.ItmMaxValueCal;
                drResultItem.ResMinCal = itemRefinfo.ItmMinValueCal;

                drResultItem.ResPanHCal = itemRefinfo.ItmDangerUpperLimitCal;
                drResultItem.ResPanLCal = itemRefinfo.ItmDangerLowerLimitCal;

                CalcPatResultRow(drResultItem, false);
            }

        }

        /// <summary>
        /// 计算所有行的参考值
        /// </summary>
        /// <param name="bGetItemsRef">是否重新获取参考值</param>
        /// <param name="bShowHint">是否显示提示</param>
        /// <param name="bGetItemsSam">是否获取项目样本信息</param>
        public void CalAllRowsItemRef(bool bGetItemsRef, bool bGetItemsSam, bool bShowHint)
        {
            if (this.dtPatientResulto != null && this.dtPatientResulto.Count > 0 && bAllowCalItemRef)
            {
                if (bGetItemsRef || bGetItemsSam)//是否重新获取参考值和样本信息
                {
                    List<string> listItemID = new List<string>();
                    foreach (EntityObrResult dr in this.dtPatientResulto)
                    {
                        listItemID.Add(dr.ItmId);
                    }

                    //从缓存中查找参考值
                    List<EntityDicItmRefdetail> dtItemsRef;

                    if (bGetItemsRef)
                    {
                        dtItemsRef = DictItem.Instance.GetItemsRef(listItemID, this.samtypeid, GetConfigOnNullAge(this.patage), GetConfigOnNullSex(this.patsex), this.samrem, this.Pat_itr_id, this.Pat_dep_id, this.PatDiag);
                    }
                    else
                    {
                        dtItemsRef = DictItem.Instance.DictItemRef_schema;
                    }

                    List<EntityDicItemSample> dtItemsSam;

                    if (bGetItemsSam)
                    {
                        dtItemsSam = DictItem.Instance.GetItemSam(listItemID, this.samtypeid);
                    }
                    else
                    {
                        dtItemsSam = DictItem.Instance.DictItemSam_schema;
                    }

                    //遍历结果表
                    foreach (EntityObrResult drResultItem in this.dtPatientResulto)
                    {
                        string itm_id = drResultItem.ItmId;

                        if (bGetItemsRef)
                        {
                            List<EntityDicItmRefdetail> drsItemRef = dtItemsRef.Where(w => w.ItmId == itm_id).ToList();

                            //填充参考值信息
                            if (drsItemRef.Count > 0)
                            {
                                //把参考值、危急值、阈值
                                EntityDicItmRefdetail drItemRef = drsItemRef[0];

                                drResultItem.RefLowerLimit = drItemRef.ItmLowerLimitValue;
                                drResultItem.RefUpperLimit = drItemRef.ItmUpperLimitValue;

                                drResultItem.ResMax = drItemRef.ItmMaxValue;
                                drResultItem.ResMin = drItemRef.ItmMinValue;

                                drResultItem.ResPanH = drItemRef.ItmDangerUpperLimit;
                                drResultItem.ResPanL = drItemRef.ItmDangerLowerLimit;

                                drResultItem.ResRefLCal = drItemRef.ItmLowerLimitValue;
                                if (drResultItem.ResRefLCal == null) drResultItem.ResRefLCal = string.Empty;

                                drResultItem.ResRefHCal = drItemRef.ItmUpperLimitValue;
                                if (drResultItem.ResRefHCal == null) drResultItem.ResRefHCal = string.Empty;

                                if (drResultItem.ResRefLCal.TrimEnd() != string.Empty
                                    && drResultItem.ResRefHCal.TrimEnd() != string.Empty)
                                {
                                    drResultItem.ResRefRange = drResultItem.ResRefLCal.TrimEnd() + " - " + drResultItem.ResRefHCal.TrimEnd();
                                }
                                else
                                {
                                    drResultItem.ResRefRange = drResultItem.ResRefLCal + drResultItem.ResRefHCal;
                                }

                                drResultItem.ResMaxCal = drItemRef.ItmMaxValueCal;
                                drResultItem.ResMinCal = drItemRef.ItmMinValueCal;

                                drResultItem.ResPanHCal = drItemRef.ItmDangerUpperLimitCal;
                                drResultItem.ResPanLCal = drItemRef.ItmDangerLowerLimitCal;
                            }
                            else
                            {
                                drResultItem.RefLowerLimit = null;
                                drResultItem.RefUpperLimit = null;

                                drResultItem.ResMax = null;
                                drResultItem.ResMin = null;

                                drResultItem.ResPanH = null;
                                drResultItem.ResPanL = null;

                                drResultItem.ResRefLCal = null;
                                drResultItem.ResRefHCal = null;

                                drResultItem.ResMaxCal = null;
                                drResultItem.ResMinCal = null;

                                drResultItem.ResPanHCal = null;
                                drResultItem.ResPanLCal = null;

                                drResultItem.ResRefRange = null;
                            }
                        }

                        if (bGetItemsSam)
                        {
                            List<EntityDicItemSample> drsItemSam = dtItemsSam.Where(w => w.ItmId == itm_id).ToList();
                            //填充样本信息
                            if (drsItemSam.Count > 0)
                            {
                                EntityDicItemSample drItemSam = drsItemSam[0];
                                drResultItem.ObrUnit = drItemSam.ItmUnit;
                                drResultItem.ItmDtype = drItemSam.ItmResType;
                                if (!string.IsNullOrEmpty(drItemSam.ItmPrice.ToString()))
                                {
                                    drResultItem.ItmPrice = Convert.ToDecimal(drItemSam.ItmPrice.ToString());
                                }
                                drResultItem.ItmMaxDigit = Convert.ToInt16(drItemSam.ItmMaxDigit);
                            }
                            else
                            {
                                drResultItem.ObrUnit = null;
                                drResultItem.ItmDtype = null;
                                drResultItem.ItmPrice = 0;
                                drResultItem.ItmMaxDigit = 0;
                            }
                        }

                        CalcPatResultRow(drResultItem, bShowHint);
                    }
                    BindGrid();
                }
                else
                {
                    foreach (EntityObrResult drResult in this.dtPatientResulto)
                    {
                        CalcPatResultRow(drResult, bShowHint);
                    }
                }
            }
        }

        /// <summary>
        /// 获取当前结果
        /// </summary>
        /// <returns></returns>
        public List<EntityObrResult> GetResultTable()
        {
            try
            {
                CloseEditor();

                if (gridControlSingle.Visible)
                {
                    return this.dtPatientResulto;
                }
                else
                {
                    return gridControlDouble.GetResultTable();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "GetResultTable", ex.ToString());
                throw;
            }
        }


        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            if (this.dtPatientResulto != null)
            {
                this.dtPatientResulto.Clear();
            }

            if (this.patients_mi != null)
            {
                this.patients_mi.Clear();
            }
            RefreshItemsCountText();
            ItemsCountCheck();
            this.NotNullItemCheck();

            this.gridViewSingle.Columns["HistoryResult1"].Caption = "历史1";
            this.gridViewSingle.Columns["HistoryResult2"].Caption = "历史2";
            this.gridViewSingle.Columns["HistoryResult3"].Caption = "历史3";
            string Res_WarningMsgForwardItrid = UserInfo.GetSysConfigValue("Res_WarningMsgForwardItrid");
            if (UserInfo.GetSysConfigValue("Lab_ShowAlarmColumn") == "是")
            {

                if (!string.IsNullOrEmpty(Pat_itr_id) && !string.IsNullOrEmpty(Res_WarningMsgForwardItrid) && Res_WarningMsgForwardItrid.Contains(Pat_itr_id))
                {
                    gridColumn11.VisibleIndex = colRefRange.VisibleIndex + 1;
                    gridColumn7.VisibleIndex = colRefRange.VisibleIndex + 2;
                    gridColumn8.VisibleIndex = colRefRange.VisibleIndex + 3;
                    gridColumn9.VisibleIndex = colRefRange.VisibleIndex + 4;
                    gridColumn12.VisibleIndex = colRefRange.VisibleIndex + 5;
                    gridColumn13.VisibleIndex = colRefRange.VisibleIndex + 6;
                    gridColumn14.VisibleIndex = colRefRange.VisibleIndex + 7;
                    gridColumn10.VisibleIndex = colRefRange.VisibleIndex + 8;
                }
                else
                {
                    gridColumn11.Visible = true;
                    gridColumn11.VisibleIndex = colResType.VisibleIndex + 1;
                    gridColumn7.VisibleIndex = gridColumn11.VisibleIndex + 1;
                    gridColumn8.VisibleIndex = gridColumn11.VisibleIndex + 2;
                    gridColumn9.VisibleIndex = gridColumn11.VisibleIndex + 3;
                    gridColumn12.VisibleIndex = gridColumn11.VisibleIndex + 4;
                    gridColumn13.VisibleIndex = gridColumn11.VisibleIndex + 5;
                    gridColumn14.VisibleIndex = gridColumn11.VisibleIndex + 6;
                    gridColumn10.VisibleIndex = gridColumn11.VisibleIndex + 7;
                }
            }

            if (gridControlDouble.Visible)
                gridControlDouble.Reset();
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void ResetForBarcode()
        {
            if (this.dtPatientResulto != null)
            {
                this.dtPatientResulto.Clear();
            }
        }

        /// <summary>
        /// 关闭编辑单元格并更新数据
        /// </summary>
        private void CloseEditor()
        {
            this.gridViewSingle.CloseEditor();
        }

        /// <summary>
        /// 菜单点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuLeft_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            this.CloseEditor();

            ContextMenuStrip menu = (sender as ContextMenuStrip);

            GridControl grid = menu.SourceControl as GridControl;
            GridView sourceGrid = null;

            if (grid == this.gridControlSingle)
            {
                sourceGrid = this.gridViewSingle;
            }

            menu.Visible = false;
            if (sourceGrid != null)
            {
                switch (e.ClickedItem.Name)
                {
                    case "menuDelItem":
                        MenuDelItem(sourceGrid, true);
                        break;
                    case "menuBakItm":
                        MenuBakItm(sourceGrid);
                        break;
                    case "menuReBakItm":
                        MenuReBakItm();
                        break;
                    case "menuAddItem":
                        MenuAddItem(sourceGrid);
                        break;
                    case "menuCalc":
                        ItemCalc(true);
                        break;
                    case "menuValidate":
                        ItemValidate();
                        break;
                    case "menuItemInfo":
                        ItemInfo(sourceGrid);
                        break;
                    case "menuItemProp":
                        ItemProp(sourceGrid);
                        break;
                    case "menuViewItemModifyHistory":
                        ViewItemModifyHistory();
                        break;
                    case "menuViewClipboard":
                        ViewClipBoardData();
                        break;
                    case "menuCopyItem":
                        CopyResult();
                        break;
                    case "menuPasteItem":
                        PasteResult();
                        break;
                    case "menuCutItem":
                        CutResult();
                        break;
                    case "menuLookOldResult"://查看复查原结果
                        LookOldResultView();
                        break;
                    case "menuMedicalRecord"://查看复查原结果
                        BrowseMedicalRecord(dtPatInfoCopy != null ? dtPatInfoCopy.PidInNo : "");
                        break;
                }
            }
            RefreshItemsCountText();
            ItemsCountCheck();
            this.NotNullItemCheck();
        }

        private void ViewItemModifyHistory()
        {
            EntityObrResult dr = this.gridViewSingle.GetFocusedRow() as EntityObrResult;

            if (dr == null || dr.ItmEname.TrimEnd() == null)
            {
                return;
            }
            GetSysoperationlog();

            if (listSysOprLog != null)
            {
                List<EntitySysOperationLog> drs =
                    listSysOprLog.FindAll(i => i.OperatGroup == "病人结果" && i.OperatObject == dr.ItmEname.TrimEnd());

                if (drs.Count == 0)
                {
                    lis.client.control.MessageDialog.ShowAutoCloseDialog(dr.ItmEname.TrimEnd() + "暂无修改记录");
                    return;
                }

                frmItemModifyHistory frm = new frmItemModifyHistory(drs);
                frm.Show();
            }
        }

        #region 剪贴板相关
        /// <summary>
        /// 查看当前剪贴板数据
        /// </summary>
        public void ViewClipBoardData()
        {
            frmViewResultClipboard frm = new frmViewResultClipboard();
            frm.ShowDialog();
        }

        /// <summary>
        /// 把当前选中的结果复制到剪贴板
        /// </summary>
        public void CopyResult()
        {
            int[] selectedRowHandler = this.gridViewSingle.GetSelectedRows();

            if (selectedRowHandler.Length == 0)
            {
                return;
            }

            DataTable tableClipboard = ResultClipboard.DataStruct;

            foreach (int rowHandler in selectedRowHandler)
            {
                EntityObrResult rowResult = this.gridViewSingle.GetRow(rowHandler) as EntityObrResult;

                DataRow rowClipboard = tableClipboard.NewRow();

                rowClipboard["res_itm_id"] = rowResult.ItmId;
                rowClipboard["res_itm_ecd"] = rowResult.ItmEname.TrimEnd();
                rowClipboard["res_chr"] = rowResult.ObrValue;
                rowClipboard["res_od_chr"] = rowResult.ObrValue2;


                tableClipboard.Rows.Add(rowClipboard);
            }

            ResultClipboard.Current.Clear();
            ResultClipboard.Current.resulto = tableClipboard;
        }

        /// <summary>
        /// 把当前剪贴板内容放入到当前样本号中
        /// </summary>
        public void PasteResult()
        {
            if (ResultClipboard.Current.resulto == null || ResultClipboard.Current.resulto.Rows.Count == 0)
            {
                return;
            }

            foreach (DataRow row in ResultClipboard.Current.resulto.Rows)
            {
                EntityObrResult rowPastedItem = this.AddItem(row["res_itm_id"].ToString(), row["res_chr"].ToString(), row["res_od_chr"].ToString());
                rowPastedItem.ObrValue = row["res_chr"].ToString();
                rowPastedItem.ObrValue2 = row["res_od_chr"].ToString();
            }

            CalAllRowsItemRef(false, false, false);
        }

        /// <summary>
        /// 剪切当前选中结果
        /// </summary>
        public void CutResult()
        {
            //获取当前记录的审核状态
            ProxyPidReportMain mainProxy = new ProxyPidReportMain();
            string strPatState = mainProxy.Service.GetPatientState(this.PatID);

            if (strPatState == LIS_Const.PATIENT_FLAG.Natural || strPatState == string.Empty)//未审核
            {
                this.CopyResult();
                this.MenuDelItem(this.gridViewSingle, false);
            }
            else
            {
                lis.client.control.MessageDialog.Show("当前记录已" + LocalSetting.Current.Setting.AuditWord + "，不能剪切", "提示");
            }
        }
        #endregion

        #region 删除项目
        /// <summary>
        /// 菜单删除项目
        /// </summary>
        /// <param name="sourceGrid"></param>
        /// <param name="bNeedConfirm">是否需要提示确认</param>
        private void MenuDelItem(GridView sourceGrid, bool bNeedConfirm)
        {
            //获取当前记录的审核状态
            ProxyPidReportMain mainProxy = new ProxyPidReportMain();
            string strPatState = mainProxy.Service.GetPatientState(this.PatID);

            if (strPatState == LIS_Const.PATIENT_FLAG.Natural || strPatState == string.Empty)//未审核
            {
                int[] selectedRowHandler = sourceGrid.GetSelectedRows();

                if (selectedRowHandler.Length == 0)
                {
                    return;
                }


                List<EntityObrResult> selectDataRows = new List<EntityObrResult>();

                bool needComma = false;
                string tipItemsText = string.Empty;
                foreach (int rowHandler in selectedRowHandler)
                {
                    EntityObrResult row = sourceGrid.GetRow(rowHandler) as EntityObrResult;
                    if (row != null)
                    {
                        selectDataRows.Add(row);
                    }

                    if (needComma)
                    {
                        tipItemsText += "，";
                    }

                    tipItemsText += string.Format("[{0}]", row.ItmEname.TrimEnd().ToString());

                    needComma = true;
                }


                string messagetips = "是否要移除项目{0}？\r\n{1}";

                if (dcl.client.frame.UserInfo.GetSysConfigValue("PatEnterItemDeleteMode") == "立刻从数据库删除") //是否立刻从数据库删除
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
                    ItemsCountCheck();
                    this.NotNullItemCheck();
                    RefreshItemsCountText();
                }

            }
            else//已审核、报告、打印
            {
                lis.client.control.MessageDialog.Show("当前记录已" + LocalSetting.Current.Setting.AuditWord + "，不能删除", "提示");
            }
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        public void RemoveItem(List<EntityObrResult> rowsPatResultItem, bool removeHasResult)
        {
            bool deleteInDatabase = false;
            if (dcl.client.frame.UserInfo.GetSysConfigValue("PatEnterItemDeleteMode") == "立刻从数据库删除")
            {
                deleteInDatabase = true;
            }

            bool statusCheck = false;
            //List<EntityObrResult> listDeleteResult = EntityManager<EntityObrResult>.ListClone(rowsPatResultItem);

            for (int i = 0; i < rowsPatResultItem.Count; i++)
            {
                EntityObrResult drPatResultItem = rowsPatResultItem[i];
                //是否已录入结果
                bool hasResult = false;
                if (!string.IsNullOrEmpty(drPatResultItem.ObrValue)
                    && drPatResultItem.ObrValue.TrimEnd(null) != string.Empty)
                {
                    hasResult = true;
                }

                //数据库是否存在结果
                bool recordInDataBase = false;
                if (drPatResultItem.ObrSn != 0)
                {
                    recordInDataBase = true;
                }
                if (!string.IsNullOrEmpty(drPatResultItem.ObrId) && !statusCheck)
                {
                    EntityPidReportMain patient = new ProxyPidReportMain().Service.GetPatientByPatId(drPatResultItem.ObrId, false);
                    string currPatFlag = string.Empty;
                    if (patient.RepInitialFlag != 0 && (patient.RepStatus == null || (patient.RepStatus != null && patient.RepStatus.Value == 0)))
                    {
                        currPatFlag = "1";
                    }
                    else
                    {
                        currPatFlag = patient.RepStatus.Value.ToString();
                    }

                    if (currPatFlag != LIS_Const.PATIENT_FLAG.Natural && currPatFlag != string.Empty)
                    {
                        MessageDialog.Show(string.Format("当前记录已{1}或已{0}，不能删除组合", "审核", LocalSetting.Current.Setting.ReportWord), "提示");
                        break;
                    }
                    statusCheck = true;
                }

                if (hasResult)
                {
                    if (removeHasResult && recordInDataBase)
                    {
                        if (!Compare.IsEmpty(drPatResultItem.ObrId) && !Compare.IsEmpty(drPatResultItem.ObrSn))
                        {
                            if (deleteInDatabase)
                            {

                                EntityLogLogin logLogin = new EntityLogLogin();
                                logLogin.LogIP = UserInfo.ip;
                                logLogin.LogLoginID = UserInfo.loginID;

                                string resid = drPatResultItem.ObrId.ToString();
                                string res_itm_ecd = drPatResultItem.ItmEname.TrimEnd().ToString();
                                string res_itm_id = string.Empty;
                                if (!Compare.IsEmpty(drPatResultItem.ItmId))
                                {
                                    res_itm_id = drPatResultItem.ItmId.ToString();
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
                                else if (reskey != -1)
                                {
                                    opResult = new ProxyPatResult().Service.DeleteCommonResultItemByObrSn(logLogin, reskey.ToString(), resid);
                                }
                                else
                                {
                                    rowsPatResultItem.Remove(drPatResultItem);
                                    i--;
                                }


                                if (!opResult)
                                {
                                    MessageDialog.Show(string.Format("删除[{0}]失败！", res_itm_ecd), "错误");
                                }
                                else
                                {
                                    rowsPatResultItem.Remove(drPatResultItem);
                                    i--;
                                    int deleteIndex = dtPatientResulto.FindIndex(w => w.ObrSn == drPatResultItem.ObrSn);
                                    if (deleteIndex > -1)
                                        this.dtPatientResulto.RemoveAt(deleteIndex);
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
                        }
                    }
                    else
                    {
                        if (!recordInDataBase)
                        {
                            rowsPatResultItem.Remove(drPatResultItem);
                        }

                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(drPatResultItem.ObrId) && drPatResultItem.ObrSn != 0)
                    // != null && drPatResultItem.ObrId != DBNull.Value && drPatResultItem.ObrId.ToString().TrimEnd(null) != string.Empty)
                    {
                        if (deleteInDatabase)
                        {
                            EntityLogLogin logLogin = new EntityLogLogin();
                            logLogin.LogIP = UserInfo.ip;
                            logLogin.LogLoginID = UserInfo.loginID;

                            string resid = drPatResultItem.ObrId.ToString();
                            string res_itm_ecd = drPatResultItem.ItmEname.TrimEnd().ToString();
                            string res_itm_id = string.Empty;
                            if (!Compare.IsEmpty(drPatResultItem.ItmId))
                            {
                                res_itm_id = drPatResultItem.ItmId.ToString();
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
                                opResult = new ProxyPatResult().Service.DeleteCommonResultItemByObrSn(logLogin, reskey.ToString(), resid);
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

                                int deleteIndex = dtPatientResulto.FindIndex(w => w.ObrSn == drPatResultItem.ObrSn);
                                if (deleteIndex > -1)
                                    this.dtPatientResulto.RemoveAt(deleteIndex);
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
                        this.dtPatientResulto.Remove(drPatResultItem);
                    }
                }
            }
            this.gridControlSingle.RefreshDataSource();
            this.BindGrid();

            ////是否已录入结果
            //bool hasResult = false;
            //if (drPatResultItem.ObrValue != null && drPatResultItem.ObrValue != DBNull.Value && drPatResultItem.ObrValue.ToString().TrimEnd(null) != string.Empty)
            //{
            //    hasResult = true;
            //}

            //if (hasResult)
            //{
            //    if (removeHasResult)
            //    {
            //        if (!Compare.IsEmpty(drPatResultItem.ObrId) && !Compare.IsEmpty(drPatResultItem.ObrSn))// != null && drPatResultItem.ObrId != DBNull.Value && drPatResultItem.ObrId.ToString().TrimEnd(null) != string.Empty)
            //        {
            //            if (dcl.client.frame.UserInfo.GetSysConfigValue("PatEnterItemDeleteMode") == "立刻从数据库删除")
            //            {
            //                EntityRemoteCallClientInfo remoteCaller = new EntityRemoteCallClientInfo();
            //                remoteCaller.IPAddress = UserInfo.ip;
            //                remoteCaller.LoginID = UserInfo.loginID;

            //                string resid = drPatResultItem.ObrId.ToString();

            //                string res_itm_id = string.Empty;
            //                if (!Compare.IsEmpty(drPatResultItem.ItmId))
            //                {
            //                    res_itm_id = drPatResultItem.ItmId.ToString();
            //                }

            //                long reskey = -1;

            //                EntityOperationResult opResult = null;
            //                if (!Compare.IsEmpty(drPatResultItem.ObrSn))
            //                {
            //                    reskey = Convert.ToInt64(drPatResultItem.ObrSn);
            //                }

            //                if (res_itm_id == string.Empty)
            //                {

            //                }
            //                else if (reskey != -1)
            //                {
            //                    opResult = PatientCRUDClient.NewInstance.DeleteCommonResultItem_byKey(remoteCaller, reskey, resid);
            //                }
            //                else
            //                {
            //                    drPatResultItem.Delete();
            //                }


            //                if (opResult != null)
            //                {
            //                    if (!opResult.Success)
            //                    {
            //                        MessageDialog.Show("删除失败！", "错误");
            //                    }
            //                    else
            //                    {
            //                        drPatResultItem.Delete();
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                drPatResultItem.IsNew = 2;
            //            }
            //        }
            //        else
            //        {
            //            drPatResultItem.Delete();
            //        }
            //    }
            //}
            //else
            //{
            //    drPatResultItem.Delete();
            //}


        }
        #endregion

        #region 备份项目结果
        /// <summary>
        /// 备份项目结果
        /// </summary>
        /// <param name="sourceGrid"></param>
        private void MenuBakItm(GridView sourceGrid)
        {
            try
            {
                if (string.IsNullOrEmpty(this.PatID)) return;

                ProxyPidReportMain mainProxy = new ProxyPidReportMain();
                string strPatState = mainProxy.Service.GetPatientState(this.PatID);
                if (strPatState == LIS_Const.PATIENT_FLAG.Natural || strPatState == string.Empty)//未审核
                {
                    int[] selectedRowHandler = sourceGrid.GetSelectedRows();

                    if (selectedRowHandler.Length == 0)
                    {
                        return;
                    }


                    //List<DataRow> selectDataRows = new List<DataRow>();
                    List<EntityObrResult> selectDataRows = new List<EntityObrResult>();

                    bool needComma = false;
                    string tipItemsText = string.Empty;
                    foreach (int rowHandler in selectedRowHandler)
                    {
                        EntityObrResult row = sourceGrid.GetRow(rowHandler) as EntityObrResult;
                        if (row != null)
                        {
                            selectDataRows.Add(row);
                        }

                        if (needComma)
                        {
                            tipItemsText += "，";
                        }

                        tipItemsText += string.Format("[{0}]", row.ItmEname.TrimEnd().ToString());

                        needComma = true;
                    }


                    string messagetips = string.Format("是否要备份项目{0}？", tipItemsText);

                    if (lis.client.control.MessageDialog.Show(messagetips, "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        string temp_res_id = this.PatID;
                        List<string> temp_res_itm_ids = new List<string>();
                        List<string> temp_res_keys = new List<string>();

                        foreach (EntityObrResult drbak in selectDataRows)
                        {
                            temp_res_keys.Add(drbak.ObrSn.ToString());
                        }

                        FrmBakItmForResulto frmBakItmForResulto = new FrmBakItmForResulto();
                        frmBakItmForResulto.startBakResulto(temp_res_id, temp_res_itm_ids, temp_res_keys);
                    }
                }
                else//已审核、报告、打印
                {
                    lis.client.control.MessageDialog.Show("当前记录已" + LocalSetting.Current.Setting.AuditWord + "，不能备份", "提示");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "MenuBakItm", ex.ToString());
                lis.client.control.MessageDialog.Show("备份失败", "提示");
            }
        }

        #endregion

        #region 还原项目结果
        /// <summary>
        /// 还原项目结果
        /// </summary>
        private void MenuReBakItm()
        {
            try
            {
                if (string.IsNullOrEmpty(this.PatID)) return;

                ProxyPidReportMain mainProxy = new ProxyPidReportMain();
                string strPatState = mainProxy.Service.GetPatientState(this.PatID);
                if (strPatState == LIS_Const.PATIENT_FLAG.Natural || strPatState == string.Empty)//未审核
                {
                    FrmBakItmForResulto frmBakItmForResulto = new FrmBakItmForResulto();
                    frmBakItmForResulto.bak_res_id = this.PatID;
                    frmBakItmForResulto.ShowDialog();
                }
                else//已审核、报告、打印
                {
                    lis.client.control.MessageDialog.Show("当前记录已" + LocalSetting.Current.Setting.AuditWord + "，不能还原", "提示");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "MenuBakItm", ex.ToString());
                lis.client.control.MessageDialog.Show("备份失败", "提示");
            }
        }
        #endregion

        #region 添加项目
        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="sourceGrid"></param>
        private void MenuAddItem(GridView sourceGrid)
        {
            //获取病人记录状态
            ProxyPidReportMain mainProxy = new ProxyPidReportMain();
            string strPatState = mainProxy.Service.GetPatientState(this.PatID);

            if (strPatState == LIS_Const.PATIENT_FLAG.Natural || strPatState == string.Empty)//未审核
            {
                FrmItemSelect frm = new FrmItemSelect();
                frm.itm_ptype = itr_ptype;
                frm.itr_id = Pat_itr_id;
                frm.ResetFilter();

                Control comEditor = this.CombineEditor as Control;
                frm.Location = this.Location;
                frm.Left = comEditor.PointToScreen(new Point(0, 0)).X + comEditor.Width - frm.Width;
                frm.Top = comEditor.PointToScreen(new Point(0, comEditor.Height)).Y;

                frm.ShowDialog();

                if (frm.DialogResult == DialogResult.OK)
                {
                    string itm_id = frm.ReturnItemID;
                    string itm_ecd = frm.ReturnItemECD;

                    //添加项目
                    AddItem(itm_id);

                    SetAutoCalParam();
                }
            }
            else//已审核、报告、打印
            {
                lis.client.control.MessageDialog.Show("当前记录已" + LocalSetting.Current.Setting.AuditWord + "，不能再添加项目", "提示");
            }
        }
        #endregion

        #region 项目关联计算
        //公用效验方法
        private DataSet Variable(Hashtable ht)
        {
            List<EntityDicItmCalu> dci = CacheClient.GetCache<EntityDicItmCalu>();
            string[] parm = new string[ht.Count];
            string[] value = new string[ht.Count];
            ht.Keys.CopyTo(parm, 0);
            ht.Values.CopyTo(value, 0);
            ArrayList list = new ArrayList();
            DataTable pb = new DataTable();
            pb.TableName = "result";
            pb.Columns.Add("cal_fmla");
            pb.Columns.Add("cal_flag");
            pb.Columns.Add("cal_item_ecd");//存ID
            pb.Columns.Add("itm_ecd");//存ECD
            pb.Columns.Add("cal_sp_formula");//存ECD
            pb.Columns.Add("retu");

            if (ht.ContainsKey("CysC") && ht.ContainsKey("CRE(酶法)") &&
                ConfigHelper.GetSysConfigValueWithoutLogin("LAB_EnableCalcCKD_EPI") == "是")
            {
                return CalcCKD_EPI(ht);
            }
            List<string> fmla = new List<string>();
            foreach (EntityDicItmCalu dr in dci)
            {
                string cal_item_ecd = string.Empty;
                if (!string.IsNullOrEmpty(dr.ItmId))
                {
                    cal_item_ecd = dr.ItmId;
                }
                if (!string.IsNullOrEmpty(dr.CalItrId) && !string.IsNullOrEmpty(Pat_itr_id)
                    && dr.CalItrId != Pat_itr_id)
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(dr.CalExpression) &&
                   fmla.Contains(dr.CalExpression + cal_item_ecd))
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(dr.CalExpression))
                {
                    fmla.Add(dr.CalExpression + cal_item_ecd);
                }
                if (!string.IsNullOrEmpty(dr.CalVariable))
                {
                    string[] varpr = dr.CalVariable.Split(',');
                    int count = 0;
                    for (int i = 0; i < parm.Length; i++)
                    {
                        for (int j = 0; j < varpr.Length; j++)
                        {
                            if (varpr[j].ToString() == parm[i].ToString())
                                count++;
                        }
                    }
                    if (count == varpr.Length && count > 0)
                    {
                        pb.Rows.Add(dr.CalExpression, dr.CalFlag, dr.ItmId, dr.ItmEcode, dr.CalSpFormula);

                    }
                }
            }
            CalInfoEventArgs eArgs = null;
            for (int i = 0; i < pb.Rows.Count; i++)
            {
                string methAll = pb.Rows[i]["cal_fmla"].ToString();
                string itmID = pb.Rows[i]["cal_item_ecd"].ToString();

                if (pb.Rows[i]["cal_sp_formula"] != null &&
                    !string.IsNullOrEmpty(pb.Rows[i]["cal_sp_formula"].ToString()))
                {
                    if (eArgs == null)
                    {
                        eArgs = new CalInfoEventArgs();
                        if (ClaItemInfo != null)
                        {
                            ClaItemInfo(this, eArgs);
                        }
                    }
                    pb.Rows[i]["retu"] = new CalcItemResHelper().GetCalcRes(pb.Rows[i]["cal_sp_formula"].ToString(), ht, eArgs);
                    continue;
                }
                //检验系统中的计算项目：如果参与计算项目结果有一项不为数值的，不用计算
                //tom 2012年9月11日11:13:27
                bool can_cal = true;

                for (int j = 0; j < ht.Count; j++)
                {
                    string fam = "[" + parm[j] + "]";
                    if (methAll.Contains(fam))
                    {
                        double dValue = 0;

                        //value[j] = value[j].Replace(">=", "").Replace("<=", "").Replace("＞＝", "").Replace("＜＝", "").Replace("<", "").Replace(">", "").Replace("＞", "").Replace("＜", "").Replace(" ", "");
                        //*******************************************************************************//
                        //在计算项目中若有大于小于符号则去掉
                        try
                        {
                            if (!double.TryParse(value[j], out dValue))
                            {
                                for (int n = 0; n < value[j].Length; n++)
                                {

                                    if (double.TryParse(value[j].Substring(n, 1), out dValue))
                                    {
                                        value[j] = value[j].Substring(n);
                                        break;

                                    }

                                }

                            }
                        }
                        catch
                        { }

                        //******************************************************************************
                        can_cal = can_cal && double.TryParse(value[j], out dValue);
                        if (can_cal)
                        {

                            string va = dValue.ToString("0.0000");


                            methAll = methAll.Replace(fam, va);
                        }
                    }
                }
                if (can_cal)
                {
                    DataTable dt = new DataTable();
                    try
                    {
                        object objValue = dt.Compute(methAll, string.Empty);

                        decimal decVal = 0;

                        if (decimal.TryParse(objValue.ToString(), out decVal))
                        {
                            CalInfoEventArgs args = new CalInfoEventArgs();
                            if (ClaItemInfo != null)
                            {
                                ClaItemInfo(this, args);
                            }
                            int? itm_max_digit = null;
                            EntityDicItemSample itemSam = dcl.client.cache.CacheItemSam.Current.SelectAll().Find(k => k.ItmId == itmID && k.ItmSamId == args.SampID);
                            if (itemSam != null)
                            {
                                itm_max_digit = itemSam.ItmMaxDigit;
                            }
                            if (itm_max_digit == null || itm_max_digit < 0)
                            {
                                decVal = decimal.Round(decVal, 4);
                                pb.Rows[i]["retu"] = decVal.ToString("0.00");
                            }
                            else
                            {
                                decVal = decimal.Round(decVal, itm_max_digit.Value);
                                if (itm_max_digit == 0)
                                {
                                    pb.Rows[i]["retu"] = decVal.ToString();
                                }
                                else
                                {

                                    pb.Rows[i]["retu"] = decVal.ToString(string.Format("0.{0}", new string('0', itm_max_digit.Value)));
                                }

                            }
                        }

                        //pb.Rows[i]["retu"] = .ToString();
                    }
                    catch
                    {
                        //2013年2月28日16:45:32 叶
                        //当使用DataTable.Compute无法计算表达式的值时,比如带Math.Log()的表达式
                        //用动态编译后进行计算
                        try
                        {
                            //2013年5月14日14:20:41 叶
                            CalInfoEventArgs args = new CalInfoEventArgs();
                            if (ClaItemInfo != null)
                            {
                                ClaItemInfo(this, args);
                            }
                            if (methAll.Contains("[标本]") || methAll.Contains("[标本备注]"))
                            {
                                methAll = methAll.Replace("[标本]", string.Format("\"{0}\"", args.SampName));
                                methAll = methAll.Replace("[标本备注]", string.Format("\"{0}\"", args.SampRem));
                            }


                            object objValue = ExpressionCompute.CalExpression(methAll);
                            if (objValue != null)
                            {
                                decimal decVal = 0;

                                if (decimal.TryParse(objValue.ToString(), out decVal))
                                {
                                    int? itm_max_digit = null;
                                    EntityDicItemSample itemSam = dcl.client.cache.CacheItemSam.Current.SelectAll().Find(k => k.ItmId == itmID && k.ItmSamId == args.SampID);
                                    if (itemSam != null)
                                    {
                                        itm_max_digit = itemSam.ItmMaxDigit;
                                    }
                                    if (itm_max_digit == null || itm_max_digit < 0)
                                    {
                                        decVal = decimal.Round(decVal, 4);
                                        pb.Rows[i]["retu"] = decVal.ToString("0.00");
                                    }
                                    else
                                    {
                                        decVal = decimal.Round(decVal, itm_max_digit.Value);
                                        if (itm_max_digit == 0)
                                        {
                                            pb.Rows[i]["retu"] = decVal.ToString();
                                        }
                                        else
                                        {

                                            pb.Rows[i]["retu"] = decVal.ToString(string.Format("0.{0}", new string('0', itm_max_digit.Value)));
                                        }

                                    }
                                }
                            }
                            else
                            {

                                pb.Rows[i]["retu"] = string.Empty;
                            }
                        }
                        catch (Exception ex)
                        {

                            pb.Rows[i]["retu"] = string.Empty;
                        }
                    }
                }
            }

            DataSet dss = new DataSet();
            dss.Tables.Add(pb);
            return dss;
        }

        private DataSet VariableItmCalu(Hashtable ht, int cot)
        {
            List<EntityDicItmCalu> listCalu = CacheClient.GetCache<EntityDicItmCalu>();
            string[] parm = new string[ht.Count];
            string[] value = new string[ht.Count];
            ht.Keys.CopyTo(parm, 0);
            ht.Values.CopyTo(value, 0);
            ArrayList list = new ArrayList();
            DataTable pb = new DataTable();
            pb.TableName = "result";
            pb.Columns.Add("cal_fmla");
            pb.Columns.Add("cal_flag");
            pb.Columns.Add("cal_item_ecd");//存ID
            pb.Columns.Add("itm_ecd");//存ECD
            pb.Columns.Add("retu");
            foreach (EntityDicItmCalu dr in listCalu)
            {
                if (!string.IsNullOrEmpty(dr.CalName))
                {
                    string[] varpr = dr.CalName.Split(',');
                    int count = 0;
                    for (int i = 0; i < parm.Length; i++)
                    {
                        for (int j = 0; j < varpr.Length; j++)
                        {
                            if (varpr[j].ToString() == parm[i].ToString())
                                count++;
                        }
                    }
                    if (count == varpr.Length && count > 0)
                    {
                        pb.Rows.Add(dr.CalExpression, dr.CalFlag, dr.ItmId, dr.ItmEcode);

                    }
                }
            }

            for (int i = 0; i < pb.Rows.Count; i++)
            {
                string methAll = pb.Rows[i]["cal_fmla"].ToString();

                for (int j = 0; j < ht.Count; j++)
                {
                    string fam = "[" + parm[j] + "]";
                    double dValue = 0;
                    string va;
                    bool isSucc = double.TryParse(value[j], out dValue);

                    if (isSucc)
                    {
                        va = dValue.ToString("0.0000");
                    }
                    else
                    {
                        va = "\"" + value[j] + "\"";
                    }


                    methAll = methAll.Replace(fam, va);
                }


                if (cot == 1)
                {

                    CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();

                    ICodeCompiler objICodeCompiler = objCSharpCodePrivoder.CreateCompiler();

                    CompilerParameters objCompilerParameters = new CompilerParameters();
                    objCompilerParameters.ReferencedAssemblies.Add("System.dll");
                    objCompilerParameters.GenerateExecutable = false;
                    objCompilerParameters.GenerateInMemory = true;
                    CompilerResults cr;

                    cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, Efficacy(methAll));

                    if (cr.Errors.HasErrors)
                    {
                        throw new ArgumentException();
                    }
                    else
                    {
                        Assembly objAssembly = cr.CompiledAssembly;
                        object objHelloWorld = objAssembly.CreateInstance("DynamicCodeGenerate.HelloWorld");
                        MethodInfo objMI = objHelloWorld.GetType().GetMethod("OutPut");
                        pb.Rows[i]["retu"] = objMI.Invoke(objHelloWorld, null).ToString();
                    }
                }
                else
                {
                    DataTable dt = new DataTable();
                    try
                    {
                        object objValue = dt.Compute(methAll, string.Empty);

                        decimal decVal = 0;

                        if (decimal.TryParse(objValue.ToString(), out decVal))
                        {
                            decVal = decimal.Round(decVal, 4);
                            pb.Rows[i]["retu"] = decVal.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }

            DataSet dss = new DataSet();
            dss.Tables.Add(pb);
            return dss;
        }

        private static DataSet Variable(Hashtable ht, List<EntityDicItmCheck> dtGroup, List<EntityDicItmCheckDetail> dtEfficacy)
        {
            string[] parm = new string[ht.Count];
            string[] value = new string[ht.Count];
            ht.Keys.CopyTo(parm, 0);
            ht.Values.CopyTo(value, 0);
            DataTable pb = new DataTable();
            pb.TableName = "result";
            pb.Columns.Add("ei_fmla");
            pb.Columns.Add("ei_name");
            pb.Columns.Add("ei_type");
            pb.Columns.Add("ei_variable");
            pb.Columns.Add("retu");

            bool isGroupPass = false;
            foreach (EntityDicItmCheck row in dtGroup)
            {
                bool isFirstSet = true;
                dtEfficacy = dtEfficacy.FindAll(i => i.CheckIdDetial == row.CheckId);
                foreach (EntityDicItmCheckDetail dr in dtEfficacy)
                {
                    if (!string.IsNullOrEmpty(dr.CheckVariable))
                    {
                        if (isFirstSet)
                        {
                            isGroupPass = true;
                            isFirstSet = false;
                        }
                        string[] varpr = dr.CheckVariable.Split(',');
                        if (!string.IsNullOrEmpty(dr.CheckTypeDetail) && dr.CheckTypeDetail == "1")
                        {
                            int count = 0;
                            for (int i = 0; i < parm.Length; i++)
                            {
                                for (int j = 0; j < varpr.Length; j++)
                                {
                                    if (varpr[j].ToString() == parm[i].ToString())
                                        count++;
                                }
                            }
                            if (count == varpr.Length && count > 0)
                            {
                                SetReturnValue(ht, pb, dr, parm, value, ref isGroupPass);
                            }
                        }
                        if (!string.IsNullOrEmpty(dr.CheckTypeDetail) && dr.CheckTypeDetail == "2")
                        {
                            SetReturnValue(ht, pb, dr, parm, value, ref isGroupPass);
                        }
                    }
                }

                //仪器下不同组的关系为：或 有一个组通过，则不再效验其它组规则
                if (isGroupPass)
                {
                    return new DataSet();
                }
            }

            DataSet dss = new DataSet();
            dss.Tables.Add(pb);
            return dss;
        }
        private static bool SetReturnValue(Hashtable ht, DataTable pb, EntityDicItmCheckDetail dr, string[] parm,
                                           string[] value, ref bool isGroupPass)
        {
            pb.Rows.Add(dr.CheckExpression, dr.CheckNameDetail, dr.CheckTypeDetail, dr.CheckVariable, "");
            string retValue = GetEfficacyValue(ht, pb.Rows[pb.Rows.Count - 1], parm, value);
            if (retValue == "True")
            {
                isGroupPass = false;
                pb.Rows[pb.Rows.Count - 1]["retu"] = "True";
                return true;
            }
            return false;
        }

        private static string GetEfficacyValue(Hashtable ht, DataRow pbRow, string[] parm,
                                               string[] value)
        {
            string methAll = pbRow["ei_fmla"].ToString();
            string eiType = pbRow["ei_type"].ToString();
            if (eiType == "1")
            {
                for (int j = 0; j < ht.Count; j++)
                {
                    string fam = "[" + parm[j] + "]";

                    double dValue = 0;
                    string va;
                    bool isSucc = double.TryParse(value[j], out dValue);

                    if (isSucc)
                    {
                        va = dValue.ToString("0.0000");
                    }
                    else
                    {
                        va = "\"" + value[j] + "\"";
                    }

                    methAll = methAll.Replace(fam, va);
                }
            }
            if (eiType == "2")
            {
                //string[] varpr = pbRow["ei_variable"].ToString().Split(',');
                //Type t = typeof(EntityPatients4Audit);
                //for (int j = 0; j < varpr.Length; j++)
                //{
                //    DataRow[] rows = dtPatColumn.Select(string.Format("column_id='{0}'", varpr[j]));
                //    if (rows.Length == 0) continue;
                //    object obj = t.GetProperty(varpr[j]).GetValue(patInfo, null);
                //    string va = obj == null ? "" : obj.ToString();

                //    double dValue = 0;
                //    bool isSucc = double.TryParse(va, out dValue);

                //    if (isSucc)
                //    {
                //        va = dValue.ToString("0.0000");
                //    }
                //    else
                //    {
                //        va = "\"" + va + "\"";
                //    }

                //    string fam = "[" + rows[0]["column_name"] + "]";
                //    methAll = methAll.Replace(fam, va);
                //}
            }

            CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();

            ICodeCompiler objICodeCompiler = objCSharpCodePrivoder.CreateCompiler();

            CompilerParameters objCompilerParameters = new CompilerParameters();
            objCompilerParameters.ReferencedAssemblies.Add("System.dll");
            objCompilerParameters.GenerateExecutable = false;
            objCompilerParameters.GenerateInMemory = true;
            CompilerResults cr;

            cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, Efficacy(methAll));

            if (cr.Errors.HasErrors)
            {
                throw new ArgumentException();
            }
            Assembly objAssembly = cr.CompiledAssembly;
            object objHelloWorld = objAssembly.CreateInstance("DynamicCodeGenerate.HelloWorld");
            MethodInfo objMI = objHelloWorld.GetType().GetMethod("OutPut");
            return objMI.Invoke(objHelloWorld, null).ToString();
        }
        private static string Efficacy(string meth)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("using System;");
            sb.Append(Environment.NewLine);
            sb.Append("namespace DynamicCodeGenerate");
            sb.Append(Environment.NewLine);
            sb.Append("{");
            sb.Append(Environment.NewLine);
            sb.Append("    public class HelloWorld");
            sb.Append(Environment.NewLine);
            sb.Append("    {");
            sb.Append(Environment.NewLine);
            sb.Append("        public bool OutPut()");
            sb.Append(Environment.NewLine);
            sb.Append("        {");
            sb.Append(Environment.NewLine);
            sb.Append("             if(");
            sb.Append(meth);
            sb.Append(")");
            sb.Append(Environment.NewLine);
            sb.Append("     return true;");
            sb.Append(Environment.NewLine);
            sb.Append("   else");
            sb.Append(Environment.NewLine);
            sb.Append("     return false;");
            sb.Append(Environment.NewLine);
            sb.Append("        }");
            sb.Append(Environment.NewLine);
            sb.Append("    }");
            sb.Append(Environment.NewLine);
            sb.Append("}");
            string code = sb.ToString();
            return code;
        }
        private DataSet CalcCKD_EPI(Hashtable ht)
        {
            DataTable pb = new DataTable();
            pb.TableName = "result";
            pb.Columns.Add("cal_fmla");
            pb.Columns.Add("cal_flag");
            pb.Columns.Add("cal_item_ecd"); //存ID
            pb.Columns.Add("itm_ecd"); //存ECD
            pb.Columns.Add("retu");
            try
            {
                CalInfoEventArgs args = new CalInfoEventArgs();
                if (ClaItemInfo != null)
                {
                    ClaItemInfo(this, args);
                }
                if (args.Age.HasValue)
                {
                    string sex = args.Sex;

                    double age = args.Age.Value;

                    string mathall = string.Empty;

                    string cre = ht["CRE(酶法)"].ToString();
                    string csys = ht["CysC"].ToString();
                    double crevalue;
                    double csysvalue;
                    if (double.TryParse(cre, out crevalue) && double.TryParse(csys, out csysvalue))
                    {

                        if (sex == "1")
                        {
                            if ((crevalue / 88.4) < 0.9)
                            {
                                if (csysvalue <= 0.8)
                                {
                                    mathall =
                                        string.Format(
                                            "135* (Math.Pow(({0}/0.9),-{1}))*(Math.Pow(({2}/0.9),-{3}))*(Math.Pow(0.995,{4}))",
                                            Math.Round(crevalue / 88.4, 2), "0.207", csysvalue, "0.375", age);
                                }
                                else
                                {
                                    mathall =
                                        string.Format(
                                            "135* (Math.Pow(({0}/0.9),-{1}))*(Math.Pow(({2}/0.9),-{3}))*(Math.Pow(0.995,{4}))",
                                            Math.Round(crevalue / 88.4, 2), "0.207", csysvalue, "0.711", age);
                                }
                            }
                            else
                            {
                                if (csysvalue <= 0.8)
                                {
                                    mathall =
                                        string.Format(
                                            "135* (Math.Pow(({0}/0.9),-{1}))*(Math.Pow(({2}/0.8),-{3}))*(Math.Pow(0.995,{4}))",
                                             Math.Round(crevalue / 88.4, 2), "0.601", csysvalue, "0.375", age);
                                }
                                else
                                {
                                    mathall =
                                        string.Format(
                                            "135* (Math.Pow(({0}/0.9),-{1}))*(Math.Pow(({2}/0.8),-{3}))*(Math.Pow(0.995,{4}))",
                                             Math.Round(crevalue / 88.4, 2), "0.601", csysvalue, "0.711", age);
                                }
                            }
                        }
                        else
                        {
                            if ((crevalue / 88.4) < 0.7)
                            {
                                if (csysvalue <= 0.8)
                                {
                                    mathall =
                                        string.Format(
                                            "130* (Math.Pow(({0}/0.7),-{1}))*(Math.Pow(({2}/0.8),-{3}))*(Math.Pow(0.995,{4}))",
                                             Math.Round(crevalue / 88.4, 2), "0.248", csysvalue, "0.375", age);
                                }
                                else
                                {
                                    mathall =
                                        string.Format(
                                            "130* (Math.Pow(({0}/0.7),-{1}))*(Math.Pow(({2}/0.8),-{3}))*(Math.Pow(0.995,{4}))",
                                             Math.Round(crevalue / 88.4, 2), "0.248", csysvalue, "0.711", age);
                                }
                            }
                            else
                            {
                                if (csysvalue <= 0.8)
                                {
                                    mathall =
                                        string.Format(
                                            "130* (Math.Pow(({0}/0.7),-{1}))*(Math.Pow(({2}/0.8),-{3}))*(Math.Pow(0.995,{4}))",
                                             Math.Round(crevalue / 88.4, 2), "0.601", csysvalue, "0.375", age);
                                }
                                else
                                {
                                    mathall =
                                        string.Format(
                                            "130* (Math.Pow(({0}/0.7),-{1}))*(Math.Pow(({2}/0.8),-{3}))*(Math.Pow(0.995,{4}))",
                                             Math.Round(crevalue / 88.4, 2), "0.601", csysvalue, "0.711", age);
                                }
                            }
                        }
                    }


                    if (!string.IsNullOrEmpty(mathall))
                    {
                        pb.Rows.Add("1", "1", "50753", "肌酐与scys");

                        object objValue = ExpressionCompute.CalExpression(mathall);
                        if (objValue != null)
                        {
                            decimal decVal = 0;

                            if (decimal.TryParse(objValue.ToString(), out decVal))
                            {

                                decVal = decimal.Round(decVal, 4);
                                pb.Rows[0]["retu"] = decVal.ToString("0.00");
                            }
                            else
                            {

                                pb.Rows[0]["retu"] = string.Empty;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                pb.Rows.Add("1", "1", "50753", "肌酐与scys");
                dcl.root.logon.Logger.WriteException("", "", ex.ToString());
            }

            DataSet dss = new DataSet();
            dss.Tables.Add(pb);
            return dss;
        }

        /// <summary>
        /// 项目关联计算，并保存到数据库
        /// </summary>
        public void ItemCalc(bool showMsg)
        {
            ProxyPidReportMain mainProxy = new ProxyPidReportMain();
            string pat_flag = mainProxy.Service.GetPatientState(this.PatID);
            if (pat_flag == LIS_Const.PATIENT_FLAG.Natural || pat_flag == string.Empty)
            {
                this.CloseEditor();

                //生成关联计算参数表
                Hashtable ht = new Hashtable();
                foreach (EntityObrResult drSource in dtPatientResulto)
                {
                    if (!string.IsNullOrEmpty(drSource.ObrValue) && drSource.ObrValue.ToString().TrimEnd(null) != string.Empty)
                    {
                        string item_ecd = drSource.ItmEname.TrimEnd().ToString();

                        if (!ht.Contains(item_ecd))
                        {
                            ht.Add(item_ecd, drSource.ObrValue);
                        }
                    }
                }

                try
                {
                    //关联计算
                    DataSet dsResult = Variable(ht);
                    DataTable dtCalResult = dsResult.Tables[0];
                    if (dtCalResult.Rows.Count > 0)
                    {
                        List<EntityDicCombineDetail> dtComItem = CacheClient.GetCache<EntityDicCombineDetail>();
                        StringBuilder sbComId = new StringBuilder();
                        foreach (EntityPidReportDetail dr in this.patients_mi)
                        {
                            sbComId.Append(string.Format(",{0}", dr.ComId));
                        }
                        if (sbComId.Length > 0)
                            sbComId.Remove(0, 1);

                        foreach (DataRow drResult in dtCalResult.Rows)
                        {

                            string itm_id = drResult["cal_item_ecd"].ToString();
                            string itm_ecd = drResult["itm_ecd"].ToString();
                            string value = drResult["retu"].ToString();

                            string valueINItmProp = value;
                            if (!string.IsNullOrEmpty(value))
                            {
                                decimal dec = 0;

                                if (decimal.TryParse(value, out dec))
                                {
                                    dec = decimal.Round(dec, 2);

                                    valueINItmProp = dec.ToString();
                                }

                                string itmprop = DictItem.Instance.GetItmProp(itm_id, valueINItmProp);
                                if (!string.IsNullOrEmpty(itmprop))
                                    value = itmprop;
                            }

                            EntityObrResult existItem = dtPatientResulto.Find(i => i.ItmId == itm_id || i.ItmEname.TrimEnd() == itm_ecd);

                            int ComItemCount = 0;
                            if (sbComId.Length > 0)
                                ComItemCount = dtComItem.FindAll(i => sbComId.ToString().Contains(i.ComId) && i.ComItmId == itm_id).Count;

                            if (ComItemCount > 0 || this.patients_mi.Count == 0)
                            {

                                if (existItem == null)//项目不存在：添加
                                {
                                    EntityObrResult drItem = this.AddItem(itm_id, value, null);
                                    if (drItem != null)
                                    {
                                        drItem.ObrType = Convert.ToInt16(LIS_Const.PatResultType.Cal);
                                    };
                                }
                                else//存在：更新结果
                                {
                                    existItem.ObrValue = value;
                                    existItem.ObrType = Convert.ToInt16(LIS_Const.PatResultType.Cal);
                                    GetSingleItemRef(existItem);
                                }
                                ProxyObrResult resultProxy = new ProxyObrResult();
                                bool result = resultProxy.Service.UpdateObrResult(existItem);
                                this.gridControlSingle.RefreshDataSource();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteException(this.GetType().Name, "ItemCalc", ex.ToString());
                }
            }
            else
            {
                if (showMsg)
                {
                    lis.client.control.MessageDialog.Show("当前记录已" + LocalSetting.Current.Setting.AuditWord + "，不能再关联计算", "提示");
                }
            }
        }

        /// <summary>
        /// 项目关联计算，先保存到实体，不保存到数据库
        /// </summary>
        public void ItemCalcNotSave()
        {
            this.CloseEditor();

            //生成关联计算参数表
            Hashtable ht = new Hashtable();
            foreach (EntityObrResult drSource in dtPatientResulto)
            {
                if (!string.IsNullOrEmpty(drSource.ObrValue) && drSource.ObrValue.ToString().TrimEnd(null) != string.Empty)
                {
                    string item_ecd = drSource.ItmEname.TrimEnd().ToString();

                    if (!ht.Contains(item_ecd))
                    {
                        ht.Add(item_ecd, drSource.ObrValue);
                    }
                }
            }

            try
            {
                //关联计算
                DataSet dsResult = Variable(ht);
                DataTable dtCalResult = dsResult.Tables[0];
                if (dtCalResult.Rows.Count > 0)
                {
                    List<EntityDicCombineDetail> dtComItem = CacheClient.GetCache<EntityDicCombineDetail>();
                    StringBuilder sbComId = new StringBuilder();
                    foreach (EntityPidReportDetail dr in this.patients_mi)
                    {
                        sbComId.Append(string.Format(",{0}", dr.ComId));
                    }
                    if (sbComId.Length > 0)
                        sbComId.Remove(0, 1);

                    foreach (DataRow drResult in dtCalResult.Rows)
                    {
                        string itm_id = drResult["cal_item_ecd"].ToString();
                        string itm_ecd = drResult["itm_ecd"].ToString();
                        string value = drResult["retu"].ToString();

                        string valueINItmProp = value;
                        if (!string.IsNullOrEmpty(value))
                        {
                            decimal dec = 0;

                            if (decimal.TryParse(value, out dec))
                            {
                                dec = decimal.Round(dec, 2);

                                valueINItmProp = dec.ToString();
                            }

                            string itmprop = DictItem.Instance.GetItmProp(itm_id, valueINItmProp);
                            if (!string.IsNullOrEmpty(itmprop))
                                value = itmprop;
                        }

                        EntityObrResult existItem = dtPatientResulto.Find(i => i.ItmId == itm_id || i.ItmEname.TrimEnd() == itm_ecd);

                        int ComItemCount = 0;
                        if (sbComId.Length > 0)
                            ComItemCount = dtComItem.FindAll(i => sbComId.ToString().Contains(i.ComId) && i.ComItmId == itm_id).Count;

                        if (ComItemCount > 0 || this.patients_mi.Count == 0)
                        {

                            if (existItem == null)//项目不存在：添加
                            {
                                EntityObrResult drItem = this.AddItem(itm_id, value, null);
                                if (drItem != null)
                                {
                                    drItem.ObrType = Convert.ToInt16(LIS_Const.PatResultType.Cal);
                                };
                            }
                            else//存在：更新结果
                            {
                                existItem.ObrValue = value;
                                existItem.ObrType = Convert.ToInt16(LIS_Const.PatResultType.Cal);
                                GetSingleItemRef(existItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "ItemCalcNotSave", ex.ToString());
            }

        }



        #endregion

        #region 项目校验
        /// <summary>
        /// 项目校验
        /// </summary>
        protected void ItemValidate()
        {
            this.CloseEditor();

            Hashtable ht = new Hashtable();
            foreach (EntityObrResult dr in dtPatientResulto)
            {
                ht.Add(dr.ItmEname.TrimEnd().ToString(), dr.ObrValue.ToString());
            }
            DataSet result = VariableItmCalu(ht, 1);
            String error = "";
            foreach (DataTable dt in result.Tables)
            {
                foreach (DataRow drVali in dt.Rows)
                {
                    if (drVali["retu"].ToString() == "False")
                    {
                        error += "经公式校验结果有异常：" + drVali["cal_fmla"].ToString() + "\n\r";

                    }
                }
            }
            EntityResponse response = new ProxyPatResult().Service.SearchItmCheckAndDetail(Pat_itr_id);
            List<object> listObj = response.GetResult() as List<object>;
            DataSet result2 = new DataSet();
            List<EntityDicItmCheck> listCheck = listObj[0] as List<EntityDicItmCheck>;
            List<EntityDicItmCheckDetail> listCheckDetail = listObj[1] as List<EntityDicItmCheckDetail>;

            if (listCheck.Count > 0 && listCheckDetail.Count > 0)
            {
                result2 = Variable(ht, listCheck, listCheckDetail);
            }
            foreach (DataTable dt in result2.Tables)
            {
                foreach (DataRow drVali in dt.Rows)
                {
                    if (drVali["retu"].ToString() == "True")
                    {
                        if (drVali["ei_name"] != null && !string.IsNullOrEmpty(drVali["ei_name"].ToString()))
                        {

                            error += "经公式效验结果有异常,内容为:" + drVali["ei_name"].ToString() + "\n\r";
                        }
                        else
                        {

                            error += "经公式效验结果有异常：公式为:" + drVali["ei_fmla"].ToString() + "\n\r";
                        }

                    }
                }
            }
            if (error != "") { lis.client.control.MessageDialog.Show(error, "结果存在异常"); }
        }
        #endregion

        /// <summary>
        /// 项目信息
        /// </summary>
        /// <param name="sourceGrid"></param>
        public void ItemInfo(GridView sourceGrid)
        {
            EntityObrResult dr = sourceGrid.GetFocusedRow() as EntityObrResult;
            if (dr != null && !Compare.IsNullOrDBNull(dr.ItmId.ToString()))
            {
                string itm_id = dr.ItmId.ToString();
                if (info == null)
                {
                    info = new FrmItemInfo();
                }
                info.LoadPropInfo(itm_id, this.samtypeid);
                info.Show();
            }
        }

        public void BrowseMedicalRecord(string pidInNo)
        {
            string url = ConfigHelper.GetSysConfigValueWithoutLogin("Lab_MedicalRecordUrl");
            url = url.Replace("@regno", pidInNo);
            System.Diagnostics.Process.Start(url);
        }

        /// <summary>
        /// 查看复查原结果
        /// </summary>
        public void LookOldResultView()
        {
            EntityObrResult dr = this.gridViewSingle.GetFocusedRow() as EntityObrResult;

            if (dr == null)
            {
                return;
            }

            DateTime p_dtime = DateTime.Now;
            string sqlSelItr = "";
            string p_sid = "";

            if (!DateTime.TryParse(dr.ObrDate.ToString(), out p_dtime))
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("没找到复查原结果(code:010)");
                return;//如果日期为空,则跳出
            }


            if (string.IsNullOrEmpty(dr.ObrItrId))
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("没找到复查原结果(code:020)");
                return;
            }
            else
            {
                sqlSelItr = dr.ObrItrId;
            }

            if (string.IsNullOrEmpty(dr.ObrSid))
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("没找到复查原结果(code:030)");
                return;
            }
            else
            {
                p_sid = dr.ObrSid;
            }

            string itm_id = dr.ItmId.ToString();
            PoxyMitmNoResultView proxy = new PoxyMitmNoResultView();
            List<EntityDicObrResultOriginal> dtResult = proxy.Service.GetInstructmentResult2(p_dtime, sqlSelItr, 0, p_sid);
            dtResult = dtResult.FindAll(i => i.ItmID == itm_id);
            try
            {
                if (dtResult != null && dtResult.Count > 0)
                {
                    #region 排序

                    for (int i = 0; i < dtPatientResulto.Count; i++)
                    {
                        List<EntityDicObrResultOriginal> listOriginal = dtResult.FindAll(w => w.ItmID == dtPatientResulto[i].ItmId);
                        if (listOriginal != null && listOriginal.Count > 0)
                        {
                            foreach (EntityDicObrResultOriginal p_dr in listOriginal)
                            {
                                p_dr.RowSer = i;
                            }
                        }
                    }

                    foreach (EntityDicObrResultOriginal p_dr in dtResult)
                    {
                        if (p_dr.RowSer == null)
                        {
                            p_dr.RowSer = 999;
                        }
                    }

                    dtResult = dtResult.OrderBy(i => i.RowSer).ToList();

                    #endregion

                    if (frmORV == null)
                    {
                        frmORV = new FrmOldResultView();
                    }
                    frmORV.dtSource = dtResult;
                    frmORV.Show();
                }
                else
                {
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("没找到复查原结果");
                }
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("遇到错误");
                Logger.WriteException(this.GetType().Name, "查看复查原结果出错", ex.ToString());
            }
        }

        /// <summary>
        /// 获取当前显示的项目结果
        /// </summary>
        /// <returns></returns>
        public List<EntityObrResult> GetResultShowData()
        {
            if (this.gridViewSingle.DataSource != null)
            {
                if (this.gridViewSingle.DataSource is List<EntityObrResult>)
                {
                    List<EntityObrResult> dttempResShowData = ((List<EntityObrResult>)this.gridViewSingle.DataSource);
                    return dttempResShowData;
                }
            }
            return null;
        }

        /// <summary>
        /// 项目特征
        /// </summary>
        /// <param name="sourceGrid"></param>
        public void ItemProp(GridView sourceGrid)
        {
            EntityObrResult dr = sourceGrid.GetFocusedRow() as EntityObrResult;

            if (dr == null) return;

            string itm_id = dr.ItmId.ToString();
            string itm_ecd = dr.ItmEname.TrimEnd().ToString();

            List<EntityDefItmProperty> listProp = DictItem.Instance.GetDclItmProp(itm_id);
            if (listProp.Count == 0)
            {
                if (UserInfo.GetSysConfigValue("Lab_ResultShowItemPropMode") == "是")//启用单击弹出项目特征，没有项目特征则不弹
                    return;
                EntityDefItmProperty entityProp = new EntityDefItmProperty();
                entityProp.PtyItmEname = itm_ecd;
                entityProp.PtyItmProperty = string.Empty;
                listProp.Add(entityProp);
            }

            if (prop == null)
            {
                prop = new FrmItmPropLst();
                prop.Left = this.PointToScreen(new Point(0, 0)).X + 300;
                prop.Top = this.PointToScreen(new Point(0, 0)).Y + 100;
            }

            prop.Visible = true;
            prop.parentControl = this;
            prop.parent_grid = sourceGrid;



            prop.listSource = listProp;
            prop.Text = "当前项目：" + itm_ecd;

            try
            {
                prop.TopMost = true;
                prop.Show();
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "ShowProp", ex.ToString());
            }
            finally
            {

            }
        }

        public bool isPo = false;

        /// <summary>
        /// 单元格值改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (bEnableCellValueChange)
            {
                bEnableCellValueChange = false;
                GridView grid = sender as GridView;

                if (e.Column.FieldName == "ObrValue2" && e.Value != null && e.Value != DBNull.Value)//计算判定结果
                {
                    EntityObrResult drResulto = grid.GetFocusedRow() as EntityObrResult;
                    if (!Compare.IsEmpty(drResulto.ItmId))
                    {
                        string judgeValue = DictImmJudge.Instance.GetJudge(drResulto.ItmId.ToString(), e.Value.ToString());

                        if (!string.IsNullOrEmpty(judgeValue))
                        {
                            drResulto.ObrValue = judgeValue;
                        }
                    }
                }

                if (e.RowHandle >= 0 && e.Column.FieldName == "ObrValue")
                {
                    EntityObrResult drResulto = grid.GetFocusedRow() as EntityObrResult;

                    if (drResulto != null)
                    {
                        drResulto.IsModify = 2;

                        if (!Compare.IsEmpty(drResulto.ItmId))
                        {
                            string itemprops = DictItem.Instance.GetItmProp(drResulto.ItmId.ToString(), e.Value.ToString());

                            if (itemprops != string.Empty)
                            {
                                string[] props = itemprops.Split(';');
                                int rowIndex = grid.FocusedRowHandle;

                                for (int i = 0; i < props.Length; i++)
                                {
                                    if (grid.RowCount >= (rowIndex + i) + 1)
                                    {
                                        if (i > 0)
                                        {
                                            isPo = true;
                                            isEnt = false;
                                        }
                                        grid.SetRowCellValue(rowIndex + i, "ObrValue", props[i]);
                                    }
                                }
                                isPo = false;
                            }
                        }

                        if (!string.IsNullOrEmpty(drResulto.CalculateDestItmId))
                        {
                            ItemCalc(true);
                        }
                    }
                }

                if (e.Column.FieldName == "ObrValue" && e.Value != null && e.Value != DBNull.Value)
                {
                    PointOutHint(e.Value.ToString(), e.RowHandle, grid);
                }

                if ((e.Column.FieldName == "ObrValue" || e.Column.FieldName == "ObrValue2")
                    && (UserInfo.GetSysConfigValue("PromptSaveAfterResValueChanged") == "是"
                    || UserInfo.GetSysConfigValue("Lab_CheckSaveBeforeLeave") == "是"))
                {

                    if (!string.IsNullOrEmpty(this.PatID))
                    {
                        ProxyPidReportMain mainProxy = new ProxyPidReportMain();
                        string pat_flag = mainProxy.Service.GetPatientState(this.PatID);
                        if (string.IsNullOrEmpty(pat_flag) || pat_flag == LIS_Const.PATIENT_FLAG.Natural)
                        {
                            if (UserInfo.GetSysConfigValue("PromptSaveAfterResValueChanged") == "是")
                            {
                                if (prop == null)
                                {
                                    prop = new FrmItmPropLst();
                                }
                                prop.TopMost = false;
                                if (isPo || MessageDialog.Show("数据已改变，是否保存？", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    prop.TopMost = true;
                                    EntityObrResult drResulto = grid.GetFocusedRow() as EntityObrResult;

                                    SaveChangedValue(drResulto);
                                    SaveChangedCalItemValue(drResulto);

                                    this.gridViewSingle.Focus();
                                    if (!isEnt)
                                        gridViewSingle.MoveNext();
                                }
                                else
                                {
                                    //*******************************************************************
                                    //若点击否则显示回原来的值
                                    if (PrevResult != string.Empty)
                                    {
                                        EntityObrResult drResulto = grid.GetFocusedRow() as EntityObrResult;
                                        drResulto.ObrValue = PrevResult;
                                    }
                                    //*******************************************************************
                                }
                            }
                            else if (UserInfo.GetSysConfigValue("Lab_CheckSaveBeforeLeave") == "是")
                            {
                                IsPatResultVChange = true;
                            }
                        }
                        else
                        {
                            MessageDialog.Show("当前记录已" + LocalSetting.Current.Setting.AuditWord + "，保存失败！", "提示");

                            //*******************************************************************
                            //若保存失败则显示回原来的值
                            if (PrevResult != string.Empty)
                            {
                                EntityObrResult drResulto = grid.GetFocusedRow() as EntityObrResult;
                                drResulto.ObrValue = PrevResult;
                            }
                            //*******************************************************************
                        }
                    }
                }

                this.NotNullItemCheck();
            }
            bEnableCellValueChange = true;
        }

        private bool SaveChangedValue(EntityObrResult drResulto)
        {
            bool success = true;

            if (drResulto.ObrSn != 0)
            {

                EntityLogLogin logLogin = new EntityLogLogin();
                logLogin.LogLoginID = UserInfo.loginID;
                logLogin.LogIP = UserInfo.ip;

                bool result = new ProxyPatResult().Service.UpdatePatientResultByResKey(logLogin, drResulto);

                if (!result)
                {
                    success = false;
                }
                else
                {
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(this.PatID))
                {
                    string calculate_dest_itm_id = drResulto.CalculateDestItmId;

                    EntityPidReportMain patient = new ProxyPidReportMain().Service.GetPatientByPatId(this.PatID, false);
                    if (patient != null)
                    {
                        drResulto.ObrId = patient.RepId;
                        drResulto.ObrItrId = patient.RepItrId;
                        drResulto.ObrSid = patient.RepSid;
                        drResulto.ObrFlag = 1;
                    }

                    ProxyObrResult resultProxy = new ProxyObrResult();
                    bool result = resultProxy.Service.InsertObrResult(drResulto);

                    if (!result)
                    {
                        success = false;
                        //MessageDialog.Show("保存失败！", "错误");
                    }
                    else
                    {
                        //查出插入的检验信息，更新显示的病人信息
                        EntityResultQC resultQc = new EntityResultQC();
                        resultQc.ListObrId.Add(drResulto.ObrId);
                        resultQc.ItmId = drResulto.ItmId;
                        EntityObrResult newResult = resultProxy.Service.ObrResultQuery(resultQc, false).First();
                        if (!Compare.IsEmpty(newResult))
                        {
                            string res_itm_id = newResult.ItmId.ToString();

                            EntityObrResult drsResulto = dtPatientResulto.Find(i => i.ItmId == res_itm_id);
                            if (drsResulto != null)
                            {
                                drsResulto.ObrSn = newResult.ObrSn;
                                drsResulto.IsNew = 0;
                                drsResulto.ObrDate = newResult.ObrDate;
                                drsResulto.ObrItrId = newResult.ObrItrId;
                                drsResulto.ObrSid = newResult.ObrSid;
                            }
                        }
                        //this.dtPatientResulto.AcceptChanges();
                    }
                    //}
                }
            }
            return success;
        }

        private bool SaveChangedCalItemValue(EntityObrResult drResulto)
        {
            bool success = true;

            //查找是否有关联计算项目
            if (!Compare.IsEmpty(drResulto.CalculateDestItmId))
            {

                string calculate_dest_itm_id = drResulto.CalculateDestItmId;
                string[] destItemsID = calculate_dest_itm_id.Split(',');

                foreach (string strDestItemID in destItemsID)
                {
                    EntityObrResult drsDestItemID = this.dtPatientResulto.Find(i => i.ItmId == strDestItemID);

                    if (drsDestItemID != null && !Compare.IsEmpty(drsDestItemID.ObrValue))
                    {
                        success = SaveChangedValue(drsDestItemID);
                    }
                }
            }

            return success;
        }

        public void PointOutHint(string inputResult, int rowHandle, GridView grid)
        {
            //根据结果填充提示信息
            EntityObrResult dr = grid.GetRow(rowHandle) as EntityObrResult;

            string itm_ecd = dr.ItmEname.TrimEnd().ToString();
            itm_ecd = SQLFormater.FormatSQL(itm_ecd);
            //计算单表
            EntityObrResult drsSingle = this.dtPatientResulto.Find(i => i.ItmEname.TrimEnd() == itm_ecd);
            if (drsSingle != null)
            {
                CalcPatResultRow(drsSingle, true);
            }

            //项目数据类型
            if (!string.IsNullOrEmpty(dr.ItmDtype)) //&& e.Value != null && e.Value != DBNull.Value)
            {
                string item_datatype = dr.ItmDtype;

                if (item_datatype == "数值")//数值型结果
                {
                    if (!Compare.IsNullOrDBNull(inputResult) && inputResult.TrimEnd(null) != string.Empty)
                    {
                        decimal dec;

                        #region 需求所致去掉大于号小于号来判断是否为数值 edit by zheng.yibin
                        inputResult = inputResult.TrimStart('>');
                        inputResult = inputResult.TrimStart('<');

                        #endregion

                        if (!decimal.TryParse(inputResult, out dec))//不为数值
                        {
                            //toolTipController1.HideHint();
                            //toolTipController1.ShowHint(string.Format("项目：{0} 只能输入数字", itm_ecd));
                        }
                        else
                        {
                            //toolTipController1.HideHint();
                            //if (dr["itm_max_digit"] != null && dr["itm_dtype"] != DBNull.Value)
                            //{
                            //    int max_digit = Convert.ToInt32(dr["itm_max_digit"]);

                            //dr.ObrValue = Math.Round((double)dec, max_digit).ToString(
                            //string result = String.Format("{0:N" + max_digit + "}", dec);
                            //dr.ObrValue = result;
                            //}
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 对病人结果表中的某一行的参考值和是否超出;结果数据类型是否正确
        /// </summary>
        /// <param name="drResult"></param>
        public void CalcPatResultRow(EntityObrResult drResult, bool bShowHint)
        {
            /*
             * 金域接口添加后lis的改造。
             * 先保存res_ref_flag中的原始值，因为第三方传过来的值就保存此字段中。
             */
            string res_ref_flag = drResult.RefFlag;
            if (drResult != null && !string.IsNullOrEmpty(drResult.ObrValue))
            {
                #region 判断有没有大小于号结果 edit by zheng
                //结果
                string strValue = drResult.ObrValue;

                //判断酶标数值结果
                if (Lab_EiasaCheckItmResUseOdValue && Itr_rep_flag == LIS_Const.InstmtDataType.Eiasa
               && !string.IsNullOrEmpty(drResult.ObrValue2))
                {
                    strValue = drResult.ObrValue2;
                }

                //结果符号，为>号或<号
                string strSymbol = "";
                if (strValue.Contains(">"))
                {
                    strSymbol = ">";
                    strValue = strValue.TrimStart('>');
                }
                else if (strValue.Contains("<"))
                {
                    strSymbol = "<";
                    strValue = strValue.TrimStart('<');
                }

                #endregion

                #region 判断历史结果有没有大小于号 edit by zheng

                string strValue1 = "";
                string strValue2 = "";
                string strValue3 = "";
                //结果符号，为>号或<号
                string strSymbol1 = "";
                string strSymbol2 = "";
                string strSymbol3 = "";



                if (!string.IsNullOrEmpty(drResult.HistoryResult1))
                {
                    //结果
                    strValue1 = drResult.HistoryResult1;


                    if (strValue1.Contains(">"))
                    {
                        strSymbol1 = ">";
                        strValue1 = strValue1.TrimStart('>');
                    }
                    else if (strValue1.Contains("<"))
                    {
                        strSymbol1 = "<";
                        strValue1 = strValue1.TrimStart('<');
                    }
                }
                if (!string.IsNullOrEmpty(drResult.HistoryResult2))
                {
                    //结果
                    strValue2 = drResult.HistoryResult2;

                    if (strValue2.Contains(">"))
                    {
                        strSymbol2 = ">";
                        strValue2 = strValue2.TrimStart('>');
                    }
                    else if (strValue2.Contains("<"))
                    {
                        strSymbol2 = "<";
                        strValue2 = strValue2.TrimStart('<');
                    }
                }
                if (!string.IsNullOrEmpty(drResult.HistoryResult3))
                {
                    //结果
                    strValue3 = drResult.HistoryResult3;

                    if (strValue1.Contains(">"))
                    {
                        strSymbol3 = ">";
                        strValue3 = strValue3.TrimStart('>');
                    }
                    else if (strValue3.Contains("<"))
                    {
                        strSymbol3 = "<";
                        strValue3 = strValue3.TrimStart('<');
                    }
                }

                strValue1 = ResultRemoveSymbol(strValue1);

                #endregion

                decimal decValue;

                #region 历史结果decValue
                decimal decValue1;
                decimal decValue2;
                decimal decValue3;
                #endregion


                //去掉指定的符号来计算参考值
                strValue = ResultRemoveSymbol(strValue);
                strValue1 = ResultRemoveSymbol(strValue1);
                strValue2 = ResultRemoveSymbol(strValue2);
                strValue3 = ResultRemoveSymbol(strValue3);

                if (
                    !strValue.Contains("+")
                    &&
                     decimal.TryParse(strValue, out decValue))//是否数值型结果
                {
                    string strItmRef_l = string.Empty;//参考值下限
                    string strItmRef_h = string.Empty;//参考值上限
                    string strItmPan_l = string.Empty;//危急值下限
                    string strItmPan_h = string.Empty;//危急值上限
                    string strItm_min = string.Empty;//阈值下限
                    string strItm_max = string.Empty;//阈值上限

                    string strItmRef_lSymbol = string.Empty;//参考值下限
                    string strItmRef_hSymbol = string.Empty;//参考值上限
                    string strItmPan_lSymbol = string.Empty;//危急值下限
                    string strItmPan_hSymbol = string.Empty;//危急值上限
                    string strItm_minSymbol = string.Empty;//阈值下限
                    string strItm_maxSymbol = string.Empty;//阈值上限

                    //是否存在参考值
                    bool hasRef = false;

                    if (!string.IsNullOrEmpty(drResult.ResRefLCal))
                    {
                        strItmRef_l = drResult.ResRefLCal;
                        if (drResult.RefLowerLimit.Contains(">") || drResult.RefLowerLimit.Contains("≥"))
                        {
                            strItmRef_lSymbol = ">";
                        }
                        strItmRef_l = ResultRemoveSymbol(strItmRef_l);
                        hasRef = true;
                    }

                    if (!string.IsNullOrEmpty(drResult.ResRefHCal))
                    {
                        strItmRef_h = drResult.ResRefHCal;
                        if (drResult.RefUpperLimit.Contains("<") || drResult.RefUpperLimit.Contains("≤"))
                        {
                            strItmRef_hSymbol = "<";
                        }
                        strItmRef_h = ResultRemoveSymbol(strItmRef_h);
                        hasRef = true;
                    }

                    if (!string.IsNullOrEmpty(drResult.ResPanLCal))
                    {
                        strItmPan_l = drResult.ResPanLCal;
                        if (drResult.ResPanL.Contains(">"))
                        {
                            strItmPan_lSymbol = ">";
                        }
                        strItmPan_l = ResultRemoveSymbol(strItmPan_l);
                    }

                    if (!string.IsNullOrEmpty(drResult.ResPanHCal))
                    {
                        strItmPan_h = drResult.ResPanHCal;
                        if (drResult.ResPanH.Contains("<"))
                        {
                            strItmPan_hSymbol = "<";
                        }
                        strItmPan_h = ResultRemoveSymbol(strItmPan_h);
                    }

                    if (!string.IsNullOrEmpty(drResult.ResMinCal))
                    {
                        strItm_min = drResult.ResMinCal.ToString();
                        if (drResult.ResMin.ToString().Contains(">"))
                        {
                            strItm_minSymbol = ">";
                        }
                        strItm_min = ResultRemoveSymbol(strItm_min);
                    }

                    if (!string.IsNullOrEmpty(drResult.ResMaxCal))
                    {
                        strItm_max = drResult.ResMaxCal.ToString();
                        if (drResult.ResMax.ToString().Contains("<"))
                        {
                            strItm_maxSymbol = "<";
                        }
                        strItm_max = ResultRemoveSymbol(strItm_max);
                    }

                    decimal decItmRef_l;
                    decimal decItmRef_h;
                    decimal decItmMax;
                    decimal decItmMin;
                    decimal decItmPan_l;
                    decimal decItmpan_h;
                    bool refLowAndHighIsString = false; //参考值下限是否是字符串  2010-7-1

                    EnumResRefFlag enumResRefFlag = EnumResRefFlag.Normal;

                    //历史结果的
                    EnumResRefFlag enumResRefFlag1 = EnumResRefFlag.Normal;
                    EnumResRefFlag enumResRefFlag2 = EnumResRefFlag.Normal;
                    EnumResRefFlag enumResRefFlag3 = EnumResRefFlag.Normal;
                    string message = string.Empty;

                    //低于参考值下限
                    if (decimal.TryParse(strItmRef_l, out decItmRef_l))
                    {
                        //冒号判断
                        if (ResultSymbolCheck(drResult.ObrValue.ToString(), drResult.ResRefLCal.ToString()) && Ref_CheckValueIsGradeScore)
                        {
                            if (decValue > decItmRef_l)
                            {
                                enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower1;

                                message += string.Format("低于参考值下限[{0}];", decItmRef_l);
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(strSymbol) && !Ref_CheckValueIsNegbigOrLittleSymbol) //edit by zheng
                            {
                                if (strSymbol == "<" && decValue <= decItmRef_l)
                                {
                                    //drResult.RefFlag = "2";

                                    enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower1;

                                    message += string.Format("低于参考值下限[{0}];", decItmRef_l);
                                }
                            }
                            else
                            {
                                //drResult.RefFlag = "2";
                                if (strItmRef_lSymbol.Contains(">") || Ref_CheckValueInclude)
                                {
                                    if (drResult.RefLowerLimit.ToString().Length > 0
                                        && drResult.RefLowerLimit.ToString().StartsWith(">="))
                                    {
                                        if (decValue < decItmRef_l)
                                        {
                                            enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower1;

                                            message += string.Format("低于参考值下限[{0}];", decItmRef_l);
                                        }
                                    }
                                    else if (decValue < decItmRef_l)
                                    {
                                        enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower1;

                                        message += string.Format("低于参考值下限[{0}];", decItmRef_l);
                                    }
                                }
                                else
                                {
                                    if (decValue < decItmRef_l)
                                    {
                                        enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower1;

                                        message += string.Format("低于参考值下限[{0}];", decItmRef_l);
                                    }
                                }
                            }
                        }

                        #region 历史结果1的判断，为了颜色赋值
                        if (decimal.TryParse(strValue1, out decValue1))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1) && !Ref_CheckValueIsNegbigOrLittleSymbol)
                            {
                                if (strSymbol1 == "<" && decValue1 <= decItmRef_l)
                                {
                                    enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Lower1;
                                }
                            }
                            else
                            {
                                if (strItmRef_lSymbol.Contains(">") || Ref_CheckValueInclude)
                                {
                                    if (decValue1 <= decItmRef_l)
                                    {
                                        enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Lower1;

                                    }
                                }
                                else
                                {
                                    if (decValue1 < decItmRef_l)
                                    {
                                        enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Lower1;

                                    }
                                }

                            }
                        }
                        #endregion

                        #region 历史结果2的判断，为了颜色赋值
                        if (decimal.TryParse(strValue2, out decValue2))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol2) && !Ref_CheckValueIsNegbigOrLittleSymbol)
                            {
                                if (strSymbol2 == "<" && decValue2 <= decItmRef_l)
                                {
                                    enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Lower1;
                                }
                            }
                            else
                            {
                                if (strItmRef_lSymbol.Contains(">") || Ref_CheckValueInclude)
                                {
                                    if (decValue2 <= decItmRef_l)
                                    {
                                        enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Lower1;
                                    }
                                }
                                else
                                {
                                    if (decValue2 < decItmRef_l)
                                    {
                                        enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Lower1;
                                    }
                                }

                            }
                        }
                        #endregion

                        #region 历史结果3的判断，为了颜色赋值
                        if (decimal.TryParse(strValue3, out decValue3))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol3) && !Ref_CheckValueIsNegbigOrLittleSymbol)
                            {
                                if (strSymbol3 == "<" && decValue3 <= decItmRef_l)
                                {
                                    enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Lower1;
                                }
                            }
                            else
                            {
                                if (strItmRef_lSymbol.Contains(">") || Ref_CheckValueInclude)
                                {
                                    if (decValue3 <= decItmRef_l)
                                    {
                                        enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Lower1;
                                    }
                                }
                                else
                                {
                                    if (decValue3 < decItmRef_l)
                                    {
                                        enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Lower1;
                                    }
                                }

                            }
                        }
                        #endregion
                    }


                    //高于参考值上限
                    if (decimal.TryParse(strItmRef_h, out decItmRef_h))
                    {
                        //冒号判断
                        if (ResultSymbolCheck(drResult.ObrValue.ToString(), drResult.ResRefHCal.ToString()) && Ref_CheckValueIsGradeScore)
                        {
                            if (decValue < decItmRef_h)
                            {
                                enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater1;

                                message += string.Format("高于参考值上限[{0}];", decItmRef_h);
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(strSymbol) && !Ref_CheckValueIsNegbigOrLittleSymbol)
                            {
                                if (strSymbol == ">" && decValue >= decItmRef_h) // && zheng
                                {
                                    enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater1;

                                    message += string.Format("高于参考值上限[{0}];", decItmRef_h);
                                }
                            }
                            else
                            {
                                if (strItmRef_hSymbol.Contains("<") || Ref_CheckValueInclude)
                                {
                                    if (drResult.RefUpperLimit.ToString().Length > 0
                                        && drResult.RefUpperLimit.ToString().StartsWith("<="))
                                    {
                                        if (decValue > decItmRef_h)
                                        {
                                            enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater1;

                                            message += string.Format("高于参考值上限[{0}];", decItmRef_h);
                                        }
                                    }
                                    else if (decValue > decItmRef_h)
                                    {
                                        enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater1;

                                        message += string.Format("高于参考值上限[{0}];", decItmRef_h);
                                    }
                                }
                                else
                                {
                                    if (decValue > decItmRef_h)
                                    {
                                        enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater1;

                                        message += string.Format("高于参考值上限[{0}];", decItmRef_h);
                                    }
                                }

                            }
                        }

                        #region 历史结果1的判断，为了颜色赋值
                        if (decimal.TryParse(strValue1, out decValue1))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1) && !Ref_CheckValueIsNegbigOrLittleSymbol)
                            {
                                if (strSymbol1 == ">" && decValue1 >= decItmRef_h)
                                {
                                    enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Greater1;
                                }
                            }
                            else
                            {
                                if (strItmRef_hSymbol.Contains("<") || Ref_CheckValueInclude)
                                {
                                    if (decValue1 >= decItmRef_h)
                                    {

                                        enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Greater1;

                                    }
                                }
                                else
                                {
                                    if (decValue1 > decItmRef_h)
                                    {

                                        enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Greater1;

                                    }
                                }

                            }
                        }
                        #endregion

                        #region 历史结果2的判断，为了颜色赋值
                        if (decimal.TryParse(strValue2, out decValue2))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol2) && !Ref_CheckValueIsNegbigOrLittleSymbol)
                            {
                                if (strSymbol2 == ">" && decValue2 >= decItmRef_h)
                                {
                                    enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Greater1;
                                }
                            }
                            else
                            {
                                if (strItmRef_hSymbol.Contains("<") || Ref_CheckValueInclude)
                                {
                                    if (decValue2 >= decItmRef_h)
                                    {
                                        enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Greater1;
                                    }
                                }
                                else
                                {
                                    if (decValue2 > decItmRef_h)
                                    {
                                        enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Greater1;
                                    }
                                }

                            }
                        }
                        #endregion

                        #region 历史结果3的判断，为了颜色赋值
                        if (decimal.TryParse(strValue3, out decValue3))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol3) && !Ref_CheckValueIsNegbigOrLittleSymbol)
                            {
                                if (strSymbol3 == ">" && decValue3 >= decItmRef_h)
                                {
                                    enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Greater1;
                                }
                            }
                            else
                            {
                                if (strItmRef_hSymbol.Contains("<") || Ref_CheckValueInclude)
                                {
                                    if (decValue3 >= decItmRef_h)
                                    {
                                        enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Greater1;
                                    }
                                }
                                else
                                {
                                    if (decValue3 > decItmRef_h)
                                    {
                                        enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Greater1;
                                    }
                                }

                            }
                        }
                        #endregion
                    }

                    if (decimal.TryParse(strItmRef_h, out decItmRef_h) == false && decimal.TryParse(strItmRef_l, out decItmRef_l) == false)
                    //参考值为字符  2010-7-22
                    {
                        refLowAndHighIsString = true;
                    }

                    //低于极小阈值
                    #region 低于极小阈值


                    if (decimal.TryParse(strItm_min, out decItmMin))
                    {
                        if (!string.IsNullOrEmpty(strSymbol) && !Ref_CheckValueIsNegbigOrLittleSymbol)
                        {
                            if (strSymbol == "<" && decValue <= decItmMin) //edit by zheng && sink
                            {
                                enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower3;
                                //FrmPatInputMessage.Instance.AddMessage(string.Format("{0} 小于极小阈值",drResult.ItmEname.TrimEnd()));
                                message += string.Format("低于极小阈值[{0}];", decItmMin);
                            }
                        }
                        else
                        {
                            if (strItm_minSymbol.Contains(">") || Pan_CheckValueInclude)
                            {
                                if (decValue <= decItmMin)
                                {
                                    enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower3;
                                    message += string.Format("低于极小阈值[{0}];", decItmMin);
                                }
                            }
                            else
                            {
                                if (decValue < decItmMin)
                                {
                                    enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower3;
                                    //FrmPatInputMessage.Instance.AddMessage(string.Format("{0} 小于极小阈值",drResult.ItmEname.TrimEnd()));
                                    message += string.Format("低于极小阈值[{0}];", decItmMin);
                                }
                            }

                        }

                        #region 历史结果1的判断，为了颜色赋值
                        if (decimal.TryParse(strValue1, out decValue1))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1) && !Ref_CheckValueIsNegbigOrLittleSymbol)
                            {
                                if (strSymbol1 == "<" && decValue1 <= decItmMin)
                                {
                                    enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Lower3;
                                }
                            }
                            else
                            {
                                if (strItm_minSymbol.Contains(">") || Pan_CheckValueInclude)
                                {

                                    if (decValue1 <= decItmMin)
                                    {
                                        enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Lower3;
                                    }
                                }
                                else
                                {

                                    if (decValue1 < decItmMin)
                                    {
                                        enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Lower3;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 历史结果2的判断，为了颜色赋值
                        if (decimal.TryParse(strValue2, out decValue2))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1) && !Ref_CheckValueIsNegbigOrLittleSymbol)
                            {
                                if (strSymbol2 == "<" && decValue2 <= decItmMin)
                                {
                                    enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Lower3;
                                }
                            }
                            else
                            {
                                if (strItm_minSymbol.Contains(">") || Pan_CheckValueInclude)
                                {


                                    if (decValue2 <= decItmMin)
                                    {
                                        enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Lower3;
                                    }
                                }
                                else
                                {


                                    if (decValue2 < decItmMin)
                                    {
                                        enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Lower3;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 历史结果3的判断，为了颜色赋值
                        if (decimal.TryParse(strValue3, out decValue3))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol3) && !Ref_CheckValueIsNegbigOrLittleSymbol)
                            {
                                if (strSymbol3 == "<" && decValue3 <= decItmMin)
                                {
                                    enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Lower3;
                                }
                            }
                            else
                            {
                                if (strItm_minSymbol.Contains(">") || Pan_CheckValueInclude)
                                {



                                    if (decValue3 <= decItmMin)
                                    {
                                        enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Lower3;
                                    }
                                }
                                else
                                {



                                    if (decValue3 < decItmMin)
                                    {
                                        enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Lower3;
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion

                    //高于极大阈值
                    #region 高于极大阈值


                    if (decimal.TryParse(strItm_max, out decItmMax))
                    {
                        if (!string.IsNullOrEmpty(strSymbol) && !Ref_CheckValueIsNegbigOrLittleSymbol)
                        {
                            if (strSymbol == ">" && decValue >= decItmMax) //edit by zheng && sink 2010-8-5
                            {
                                enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater3;
                                //FrmPatInputMessage.Instance.AddMessage(string.Format("{0} 大于极大阈值",drResult.ItmEname.TrimEnd()));
                                message += string.Format("高于极大阈值[{0}];", decItmMax);
                            }
                        }
                        else
                        {
                            if (strItm_maxSymbol.Contains("<") || Pan_CheckValueInclude)
                            {
                                if (decValue >= decItmMax)
                                {
                                    enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater3;
                                    //FrmPatInputMessage.Instance.AddMessage(string.Format("{0} 大于极大阈值",drResult.ItmEname.TrimEnd()));
                                    message += string.Format("高于极大阈值[{0}];", decItmMax);
                                }
                            }
                            else
                            {
                                if (decValue > decItmMax)
                                {
                                    enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater3;
                                    //FrmPatInputMessage.Instance.AddMessage(string.Format("{0} 大于极大阈值",drResult.ItmEname.TrimEnd()));
                                    message += string.Format("高于极大阈值[{0}];", decItmMax);
                                }
                            }

                        }

                        #region 历史结果1的判断，为了颜色赋值
                        if (decimal.TryParse(strValue1, out decValue1))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1) && !Ref_CheckValueIsNegbigOrLittleSymbol)
                            {
                                if (strSymbol1 == ">" && decValue1 >= decItmMax)
                                {
                                    enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Greater3;
                                }
                            }
                            else
                            {
                                if (strItm_maxSymbol.Contains("<") || Pan_CheckValueInclude)
                                {

                                    if (decValue1 >= decItmMax)
                                    {
                                        enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Greater3;
                                    }
                                }
                                else
                                {

                                    if (decValue1 > decItmMax)
                                    {
                                        enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Greater3;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 历史结果2的判断，为了颜色赋值
                        if (decimal.TryParse(strValue2, out decValue2))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1) && !Ref_CheckValueIsNegbigOrLittleSymbol)
                            {
                                if (strSymbol2 == ">" && decValue2 >= decItmMax)
                                {
                                    enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Greater3;
                                }
                            }
                            else
                            {

                                if (strItm_maxSymbol.Contains("<") || Pan_CheckValueInclude)
                                {


                                    if (decValue2 >= decItmMax)
                                    {
                                        enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Greater3;
                                    }
                                }
                                else
                                {


                                    if (decValue2 > decItmMax)
                                    {
                                        enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Greater3;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 历史结果3的判断，为了颜色赋值
                        if (decimal.TryParse(strValue3, out decValue3))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol3) && !Ref_CheckValueIsNegbigOrLittleSymbol)
                            {
                                if (strSymbol3 == ">" && decValue3 >= decItmMax)
                                {

                                    enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Greater3;
                                }
                            }
                            else
                            {
                                if (strItm_maxSymbol.Contains("<") || Pan_CheckValueInclude)
                                {



                                    if (decValue3 >= decItmMax)
                                    {
                                        enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Greater3;
                                    }
                                }
                                else
                                {



                                    if (decValue3 > decItmMax)
                                    {
                                        enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Greater3;
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion

                    //低于危机值下限
                    if (decimal.TryParse(strItmPan_l, out decItmPan_l))
                    {
                        if (!string.IsNullOrEmpty(strSymbol) && !Ref_CheckValueIsNegbigOrLittleSymbol)
                        {
                            if (strSymbol == "<" && decValue <= decItmPan_l) //edit by zheng && sink
                            {
                                enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower2;
                                message += string.Format("低于危急值下限[{0}];", decItmPan_l);
                            }
                        }
                        else
                        {

                            if (strItmPan_lSymbol.Contains(">") || Pan_CheckValueInclude)
                            {
                                if (decValue <= decItmPan_l)
                                {
                                    enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower2;
                                    message += string.Format("低于危急值下限[{0}];", decItmPan_l);
                                }
                            }
                            else
                            {

                                if (decValue < decItmPan_l)
                                {
                                    enumResRefFlag = enumResRefFlag | EnumResRefFlag.Lower2;
                                    message += string.Format("低于危急值下限[{0}];", decItmPan_l);
                                }
                            }

                        }
                        #region 历史结果1的判断，为了颜色赋值
                        if (decimal.TryParse(strValue1, out decValue1))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1) && !Ref_CheckValueIsNegbigOrLittleSymbol)
                            {
                                if (strSymbol1 == "<" && decValue1 <= decItmPan_l)
                                {


                                    enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Lower2;


                                }
                            }
                            else
                            {
                                if (strItmPan_lSymbol.Contains(">") || Pan_CheckValueInclude)
                                {
                                    if (decValue1 <= decItmPan_l)
                                    {

                                        enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Lower2;
                                    }
                                }
                                else
                                {

                                    if (decValue1 < decItmPan_l)
                                    {

                                        enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Lower2;
                                    }
                                }


                            }
                        }
                        #endregion

                        #region 历史结果2的判断，为了颜色赋值
                        if (decimal.TryParse(strValue2, out decValue2))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1) && !Ref_CheckValueIsNegbigOrLittleSymbol)
                            {
                                if (strSymbol2 == "<" && decValue2 <= decItmPan_l)
                                {


                                    enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Lower2;


                                }
                            }
                            else
                            {
                                if (strItmPan_lSymbol.Contains(">") || Pan_CheckValueInclude)
                                {

                                    if (decValue2 <= decItmPan_l)
                                    {
                                        enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Lower2;
                                    }
                                }
                                else
                                {


                                    if (decValue2 < decItmPan_l)
                                    {
                                        enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Lower2;
                                    }
                                }


                            }
                        }
                        #endregion

                        #region 历史结果3的判断，为了颜色赋值
                        if (decimal.TryParse(strValue3, out decValue3))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol3) && !Ref_CheckValueIsNegbigOrLittleSymbol)
                            {
                                if (strSymbol3 == "<" && decValue3 <= decItmPan_l)
                                {


                                    enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Lower2;


                                }
                            }
                            else
                            {
                                if (strItmPan_lSymbol.Contains(">") || Pan_CheckValueInclude)
                                {


                                    if (decValue3 <= decItmPan_l)
                                    {
                                        enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Lower2;
                                    }
                                }
                                else
                                {



                                    if (decValue3 < decItmPan_l)
                                    {
                                        enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Lower2;
                                    }
                                }


                            }
                        }
                        #endregion
                    }

                    //高于危机值上限
                    if (decimal.TryParse(strItmPan_h, out decItmpan_h))
                    {
                        if (!string.IsNullOrEmpty(strSymbol) && !Ref_CheckValueIsNegbigOrLittleSymbol)
                        {
                            if (strSymbol == ">" && decValue >= decItmpan_h) //edit by zheng && sink
                            {
                                enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater2;
                                message += string.Format("高于危急值上限[{0}];", decItmpan_h);
                            }
                        }
                        else
                        {
                            if (strItmPan_hSymbol.Contains("<") || Pan_CheckValueInclude)
                            {
                                if (decValue >= decItmpan_h)
                                {
                                    enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater2;
                                    message += string.Format("高于危急值上限[{0}];", decItmpan_h);
                                }
                            }
                            else
                            {
                                if (decValue > decItmpan_h)
                                {
                                    enumResRefFlag = enumResRefFlag | EnumResRefFlag.Greater2;
                                    message += string.Format("高于危急值上限[{0}];", decItmpan_h);
                                }
                            }

                        }

                        #region 历史结果1的判断，为了颜色赋值
                        if (decimal.TryParse(strValue1, out decValue1))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1) && !Ref_CheckValueIsNegbigOrLittleSymbol)
                            {
                                if (strSymbol1 == ">" && decValue1 >= decItmpan_h)
                                {


                                    enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Greater2;


                                }
                            }
                            else
                            {
                                if (strItmPan_hSymbol.Contains("<") || Pan_CheckValueInclude)
                                {

                                    if (decValue1 >= decItmpan_h)
                                    {
                                        enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Greater2;
                                    }
                                }
                                else
                                {

                                    if (decValue1 > decItmpan_h)
                                    {
                                        enumResRefFlag1 = enumResRefFlag1 | EnumResRefFlag.Greater2;
                                    }
                                }


                            }
                        }
                        #endregion

                        #region 历史结果2的判断，为了颜色赋值
                        if (decimal.TryParse(strValue2, out decValue2))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol1) && !Ref_CheckValueIsNegbigOrLittleSymbol)
                            {
                                if (strSymbol2 == ">" && decValue2 >= decItmpan_h)
                                {


                                    enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Greater2;


                                }
                            }
                            else
                            {
                                if (strItmPan_hSymbol.Contains("<") || Pan_CheckValueInclude)
                                {


                                    if (decValue2 >= decItmpan_h)
                                    {
                                        enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Greater2;
                                    }
                                }
                                else
                                {


                                    if (decValue2 > decItmpan_h)
                                    {
                                        enumResRefFlag2 = enumResRefFlag2 | EnumResRefFlag.Greater2;
                                    }
                                }


                            }
                        }
                        #endregion

                        #region 历史结果3的判断，为了颜色赋值
                        if (decimal.TryParse(strValue3, out decValue3))//是否数值型结果
                        {
                            if (!string.IsNullOrEmpty(strSymbol3) && !Ref_CheckValueIsNegbigOrLittleSymbol)
                            {
                                if (strSymbol3 == ">" && decValue3 >= decItmpan_h)
                                {


                                    enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Greater2;


                                }
                            }
                            else
                            {
                                if (strItmPan_hSymbol.Contains("<") || Pan_CheckValueInclude)
                                {



                                    if (decValue3 >= decItmpan_h)
                                    {
                                        enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Greater2;
                                    }
                                }
                                else
                                {



                                    if (decValue3 > decItmpan_h)
                                    {
                                        enumResRefFlag3 = enumResRefFlag3 | EnumResRefFlag.Greater2;
                                    }
                                }


                            }
                        }
                        #endregion
                    }

                    if (message != string.Empty && bShowHint)
                    {
                        message = string.Format("项目：{0} ", drResult.ItmEname.TrimEnd().ToString()) + message;
                        //隐藏前一个提示
                        if (toolTipController1.Active)
                            toolTipController1.HideHint();
                    }
                    else
                    {//没有参考值时不显示
                        if (toolTipController1.Active)
                            toolTipController1.HideHint();
                    }

                    if (!hasRef)//如果没有参考值
                    {
                        drResult.RefFlag = ((int)EnumResRefFlag.Unknow).ToString();
                    }
                    else
                    {
                        if (refLowAndHighIsString)//参考值上下限是否是字符串  2010-7-22
                        {
                            drResult.RefFlag = ((int)EnumResRefFlag.Unknow).ToString();
                        }
                        else// if (enumResRefFlag == EnumResRefFlag.Normal)
                        {
                            drResult.RefFlag = ((int)enumResRefFlag).ToString();
                            //drResult.RefFlag = ((int)EnumResRefFlag.Normal).ToString();
                            //drResult["res_ref_flag"] = ((int)EnumResRefFlag.Normal).ToString();

                            drResult.RefFlagHistory1 = ((int)enumResRefFlag1);

                            drResult.RefFlagHistory2 = ((int)enumResRefFlag2);

                            drResult.RefFlagHistory3 = ((int)enumResRefFlag3);
                        }
                    }

                    //未审核可以计算小数点，已审核不可计算
                    if (ShouldCalcDigit && drResult.ItmMaxDigit != -1)
                    {
                        int max_digit = drResult.ItmMaxDigit;
                        if (max_digit != 0)
                        {
                            // decimal decVal = Math.Round(decValue, max_digit); // 2010-6-8
                            //string result = String.Format("{0:N" + max_digit + "}", decValue);
                            //drResult.ObrValue = strSymbol + decValue.ToString("F" + max_digit.ToString()); // 2010-6-8 项目小数位补零 //edit by zheng
                        }
                    }
                }
                else
                {
                    string res_positive_result = string.Empty;
                    if (!string.IsNullOrEmpty(drResult.ResPositiveResult))
                        res_positive_result = drResult.ResPositiveResult.ToString().TrimEnd();

                    string res_custom_critical_result = string.Empty;
                    if (!string.IsNullOrEmpty(drResult.ResCustomCriticalResult))
                        res_custom_critical_result = drResult.ResCustomCriticalResult.ToString().TrimEnd();

                    string res_allow_values = string.Empty;
                    if (!string.IsNullOrEmpty(drResult.ResAllowValues))
                        res_allow_values = drResult.ResAllowValues.ToString().TrimEnd();

                    bool isCustomCritical = false;
                    if (res_custom_critical_result.TrimEnd() != string.Empty && strValue != null && strValue.TrimEnd() != string.Empty)
                    {
                        foreach (string pos_res in res_custom_critical_result.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            if (strValue == pos_res)
                            {
                                drResult.RefFlag = ((int)EnumResRefFlag.CustomCriticalValue).ToString();
                                isCustomCritical = true;
                                break;
                            }
                        }
                    }

                    bool hasNotAllowValue = true;
                    if (res_allow_values.TrimEnd() != string.Empty && strValue != null && strValue.TrimEnd() != string.Empty)
                    {
                        foreach (string res_allow in res_allow_values.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            if (strValue == res_allow)
                            {
                                hasNotAllowValue = false;
                                break;

                            }
                        }

                        if (hasNotAllowValue)
                            drResult.RefFlag = ((int)EnumResRefFlag.ExistedNotAllowValues).ToString();
                    }
                    else
                    {
                        hasNotAllowValue = false;
                    }

                    if (res_positive_result != string.Empty && !isCustomCritical && !hasNotAllowValue)
                    {
                        if (strValue != null)
                        {
                            if (strValue.TrimEnd().Contains("弱阳性") || strValue.TrimEnd() == "±")
                            {
                                drResult.RefFlag = ((int)EnumResRefFlag.WeaklyPositive).ToString();
                            }
                            else
                            {
                                bool is_pos = false;
                                foreach (string pos_res in res_positive_result.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                                {
                                    if (strValue == pos_res)
                                    {
                                        drResult.RefFlag = ((int)EnumResRefFlag.Positive).ToString();
                                        is_pos = true;
                                        break;
                                    }
                                }

                                if (!is_pos)
                                {
                                    drResult.RefFlag = ((int)EnumResRefFlag.Unknow).ToString();
                                }
                            }
                        }
                        else if (!hasNotAllowValue)
                        {
                            drResult.RefFlag = ((int)EnumResRefFlag.Normal).ToString();
                        }
                        else
                        {
                            drResult.RefFlag = ((int)EnumResRefFlag.Unknow).ToString();
                        }
                    }
                    else if (!isCustomCritical && !hasNotAllowValue)
                    {
                        if (strValue != null
                            &&
                                (
                                    strValue.TrimEnd() == "+"
                                    || strValue.StartsWith("+")
                                    || strValue.EndsWith("+")
                                    || strValue.IndexOf("阳性") >= 0
                                    || strValue.ToLower() == "pos"
                                )
                            && !strValue.TrimEnd().Contains("弱阳性")
                            && !(strValue.TrimEnd() == "±")
                            && !(strValue.Length > 1 && strValue.Replace("+", "").TrimEnd() == string.Empty)
                            )
                        {
                            drResult.RefFlag = ((int)EnumResRefFlag.Positive).ToString();
                        }
                        else if ((strValue.TrimEnd().Contains("弱阳性") || strValue.TrimEnd() == "±") && hasNotAllowValue)
                        {
                            drResult.RefFlag = ((int)EnumResRefFlag.WeaklyPositive).ToString();
                        }
                        else if (!hasNotAllowValue)
                        {
                            drResult.RefFlag = ((int)EnumResRefFlag.Normal).ToString();
                        }
                        else
                        {
                            drResult.RefFlag = ((int)EnumResRefFlag.Unknow).ToString();
                        }
                    }
                }
            }

            if(drResult.ItmId == "2082")
            {
                if (Convert.ToDouble(drResult.ObrValue) < 0.1)
                {
                    drResult.RefFlag = "0";
                }
                else if (Convert.ToDouble(drResult.ObrValue) > 0.1 && Convert.ToDouble(drResult.ObrValue) < 50)
                {
                    drResult.RefFlag = "8";
                }

                drResult.RefUpperLimit = "";
                drResult.RefLowerLimit = "";
                drResult.RefType = 1;
            }

            /*2013-09-09  金域接口对lis的改造。如果发现当前的记录是从第三方传过来的，
             * 则对res_ref_flag,res_ref_range的值进行重新赋值。
            */
            if (!string.IsNullOrEmpty(drResult.PatReportId) &&
                String.Equals("outreport", drResult.PatReportId.TrimEnd(), StringComparison.CurrentCultureIgnoreCase))
            {
                drResult.RefFlag = res_ref_flag;
            }
        }


        /// <summary>
        /// 去掉指定的符号
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        private string ResultRemoveSymbol(string strValue)
        {
            double douTemp = 0;
            strValue = strValue.TrimStart(new char[] { '=', '>', '<', '≥', '≤', (char)30 });
            if (strValue.Contains(":"))
            {
                string[] splited = strValue.Split(':');
                if (splited.Length == 2)
                {
                    decimal decLeft;
                    decimal decRight;
                    if (decimal.TryParse((double.TryParse(splited[0], out douTemp) ? douTemp.ToString() : splited[0]), out decLeft)
                        && decimal.TryParse((double.TryParse(splited[1], out douTemp) ? douTemp.ToString() : splited[1]), out decRight))
                    {

                        return Convert.ToDecimal(Convert.ToDouble(splited[1])).ToString();
                    }
                }
            }
            else if (strValue.Contains("："))
            {
                string[] splited = strValue.Split('：');
                if (splited.Length == 2)
                {
                    decimal decLeft;
                    decimal decRight;
                    if (decimal.TryParse((double.TryParse(splited[0], out douTemp) ? douTemp.ToString() : splited[0]), out decLeft)
                        && decimal.TryParse((double.TryParse(splited[1], out douTemp) ? douTemp.ToString() : splited[1]), out decRight))
                    {

                        return Convert.ToDecimal(Convert.ToDouble(splited[1])).ToString();
                    }
                }
            }

            if (!string.IsNullOrEmpty(strValue) && double.TryParse(strValue, out douTemp))
            {
                strValue = Convert.ToDecimal(douTemp).ToString();
            }

            return strValue;
        }

        /// <summary>
        /// 冒号判断
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        private bool ResultSymbolCheck(string strValue, string refvalue)
        {
            try
            {
                if (string.IsNullOrEmpty(strValue) || string.IsNullOrEmpty(refvalue)) return false;

                strValue = strValue.TrimStart(new char[] { '=', '>', '<', (char)30 });
                refvalue = refvalue.TrimStart(new char[] { '=', '>', '<', (char)30 });
                if (strValue.Contains(":") && refvalue.Contains(":"))
                {
                    string[] splited = strValue.Split(':');
                    string[] splitedRef = strValue.Split(':');
                    if (splited.Length == 2)
                    {
                        decimal decLeft;
                        decimal decRight;
                        decimal decLeftRef;
                        decimal decRightRef;
                        if (decimal.TryParse(splited[0], out decLeft)
                            && decimal.TryParse(splited[1], out decRight)
                            && decimal.TryParse(splitedRef[0], out decLeftRef)
                            && decimal.TryParse(splitedRef[1], out decRightRef))
                        {

                            return true;
                        }
                    }
                }
            }
            catch
            { }

            return false;

        }


        /// <summary>
        /// 双击网格弹出项目特征
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView_DoubleClick(object sender, EventArgs e)
        {
            GridView sourceGrid = sender as GridView;
            //点击结果时显示参考值
            GridHitInfo info;
            Point pt = sourceGrid.GridControl.PointToClient(Control.MousePosition);
            if (pt == null)
                return;
            info = sourceGrid.CalcHitInfo(pt);
            if (info == null || info.Column == null)
                return;

            if (info.InRow && info.Column.FieldName == "ObrRemark")
            {
                EntityObrResult row = gridViewSingle.GetFocusedRow() as EntityObrResult;

                if (row != null && !string.IsNullOrEmpty(row.ObrRemark))
                {
                    MessageDialog.Show(row.ObrRemark, "报警说明");
                }
            }

            //******************************************************
            //若当前为readonly属性的话，则不允许弹出
            if (info.Column.ReadOnly == true)
            {
                return;
            }
            //*****************************************************

            if (info.InRow && info.Column.FieldName == "ObrValue")
            {

                this.ItemProp(sourceGrid);
            }

        }

        /// <summary>
        /// FocusedRowChanged
        /// 更新显示当前选中的项目的特征和信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

            GridView sourceGrid = sender as GridView;

            if (this.prop != null && this.prop.Visible)
            {
                this.ItemProp(sourceGrid);
            }

            if (this.info != null && this.info.Visible)
            {
                this.ItemInfo(sourceGrid);
            }

            //点击结果时显示参考值
            GridHitInfo info;
            Point pt = sourceGrid.GridControl.PointToClient(Control.MousePosition);
            if (pt == null)
                return;
            info = sourceGrid.CalcHitInfo(pt);
            if (info == null || info.Column == null)
                return;

            if (info.InRow && info.Column.FieldName == "ObrValue")
            {
                EntityObrResult dr = sourceGrid.GetFocusedRow() as EntityObrResult;
                if (dr == null || Compare.IsNullOrDBNull(dr.ObrValue) || string.IsNullOrEmpty(dr.ObrValue.ToString()))
                    return;

                PointOutHint(dr.ObrValue.ToString(), sourceGrid.FocusedRowHandle, sourceGrid);
            }
        }

        /// <summary>
        /// 行式样
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            e.Appearance.BackColor = Color.Transparent;
            GridView grid = sender as GridView;

            EntityObrResult dr = grid.GetRow(e.RowHandle) as EntityObrResult;
            if (dr == null)
                return;

            if (!string.IsNullOrEmpty(dr.RefFlag) && dr.RefFlag.ToString().TrimEnd(null) != string.Empty)
            {
                EnumResRefFlag enumResRefFlag = (EnumResRefFlag)Convert.ToInt32(dr.RefFlag);
                string strRes = dr.ObrValue.ToString();


                if (enumResRefFlag != EnumResRefFlag.Unknow)
                {
                    if ((enumResRefFlag & EnumResRefFlag.Greater3) == EnumResRefFlag.Greater3)
                    {
                        if (e.Column.FieldName == "ItmEname.TrimEnd()")
                        {
                            e.Appearance.BackColor = Config.BackColorGreaterThanMax;
                            e.Appearance.ForeColor = Config.ForeColorGreaterThanMax;
                        }
                        else if (e.Column.FieldName == "RefFlag")
                        {
                            e.Appearance.ForeColor = Config.ForeColorGreaterThanMax;
                        }
                    }

                    if ((enumResRefFlag & EnumResRefFlag.Greater2) == EnumResRefFlag.Greater2)
                    {
                        if (e.Column.FieldName == "ItmName")
                        {
                            e.Appearance.BackColor = Config.BackColorGreaterThanPan;
                            e.Appearance.ForeColor = Config.ForeColorGreaterThanPan;
                        }
                        if (e.Column.FieldName == "ItmEname.TrimEnd()")
                        {
                            e.Appearance.BackColor = Config.BackColorGreaterThanPan;
                            e.Appearance.ForeColor = Config.ForeColorGreaterThanPan;
                        }
                        else if (e.Column.FieldName == "RefFlag")
                        {
                            e.Appearance.ForeColor = Config.ForeColorGreaterThanPan;
                        }
                    }

                    if (FlagEquals(enumResRefFlag, EnumResRefFlag.Positive) || FlagEquals(enumResRefFlag, EnumResRefFlag.Greater1))
                    {
                        //检验结果数值的颜色显示（添加包含了历史结果也进行颜色控制）
                        if (e.Column.FieldName == "ObrValue")//|| e.Column.FieldName == "history_result1" || e.Column.FieldName == "history_result2" || e.Column.FieldName == "history_result3")
                        {
                            e.Appearance.BackColor = Config.BackColorGreaterThanRef;
                            e.Appearance.ForeColor = Config.ForeColorGreaterThanRef;
                        }
                        else if (e.Column.FieldName == "RefFlag")
                        {
                            e.Appearance.ForeColor = Config.ForeColorGreaterThanRef;
                        }
                    }

                    if ((enumResRefFlag & EnumResRefFlag.Lower3) == EnumResRefFlag.Lower3)
                    {
                        if (e.Column.FieldName == "ItmEname.TrimEnd()")
                        {
                            e.Appearance.BackColor = Config.BackColorLowerThanMin;
                            e.Appearance.ForeColor = Config.ForeColorLowerThanMin;
                        }
                        else if (e.Column.FieldName == "RefFlag")
                        {
                            e.Appearance.ForeColor = Config.ForeColorLowerThanMin;
                        }
                    }

                    if ((enumResRefFlag & EnumResRefFlag.Lower2) == EnumResRefFlag.Lower2)
                    {
                        if (e.Column.FieldName == "ItmName")
                        {
                            e.Appearance.BackColor = Config.BackColorLowerThanPan;
                            e.Appearance.ForeColor = Config.ForeColorLowerThanPan;
                        }
                        if (e.Column.FieldName == "ItmEname.TrimEnd()")
                        {
                            e.Appearance.BackColor = Config.BackColorLowerThanPan;
                            e.Appearance.ForeColor = Config.ForeColorLowerThanPan;
                        }
                        else if (e.Column.FieldName == "RefFlag")
                        {
                            e.Appearance.ForeColor = Config.ForeColorLowerThanPan;
                        }
                    }

                    if ((enumResRefFlag & EnumResRefFlag.Lower1) == EnumResRefFlag.Lower1)
                    {
                        //检验结果数值的颜色显示（添加包含了历史结果也进行颜色控制）
                        if (e.Column.FieldName == "ObrValue")
                        {
                            e.Appearance.BackColor = Config.BackColorLowerThanRef;
                            e.Appearance.ForeColor = Config.ForeColorLowerThanRef;
                        }
                        else if (e.Column.FieldName == "RefFlag")
                        {
                            e.Appearance.ForeColor = Config.ForeColorLowerThanRef;
                        }
                    }
                }

                if (strRes != null && strRes != "")
                {
                    string res_positive_result = string.Empty;
                    if (!string.IsNullOrEmpty(dr.ResPositiveResult))
                        res_positive_result = dr.ResPositiveResult.ToString().TrimEnd();

                    string res_custom_critical_result = string.Empty;
                    if (!string.IsNullOrEmpty(dr.ResCustomCriticalResult))
                        res_custom_critical_result = dr.ResCustomCriticalResult.ToString().TrimEnd();

                    string res_allow_values = string.Empty;
                    if (!string.IsNullOrEmpty(dr.ResAllowValues))
                        res_allow_values = dr.ResAllowValues.ToString().TrimEnd();

                    bool isCustomCritical = false;
                    if (res_custom_critical_result.TrimEnd() != string.Empty)
                    {
                        foreach (string pos_res in res_custom_critical_result.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            if (strRes == pos_res)
                            {
                                e.Appearance.ForeColor = Color.Red;
                                isCustomCritical = true;
                                break;
                            }
                        }
                    }

                    bool hasNotAllowValue2 = true;
                    bool hasNotAllowValue = true;
                    if (res_allow_values.TrimEnd() != string.Empty && strRes != null && strRes.TrimEnd() != string.Empty)
                    {
                        foreach (string res_allow in res_allow_values.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            if (strRes == res_allow)
                            {
                                hasNotAllowValue = false;
                                hasNotAllowValue2 = false;
                                break;

                            }
                        }

                        if (hasNotAllowValue)
                            e.Appearance.ForeColor = Color.Red;
                    }
                    else
                    {
                        hasNotAllowValue = false;
                    }

                    string words = UserInfo.GetSysConfigValue("ResultNoticeWhenWordsAre");
                    if (!string.IsNullOrEmpty(words))
                    {
                        string[] wordsArray = words.Split(',');
                        foreach (string word in wordsArray)
                        {
                            if (strRes.Replace(" ", "").Contains(word) && hasNotAllowValue2)
                            {
                                e.Appearance.ForeColor = Color.Red;
                            }
                        }
                    }

                    if (res_positive_result != string.Empty && !isCustomCritical && !hasNotAllowValue)
                    {
                        if (strRes != null)
                        {
                            if (strRes.Contains("弱阳性") || strRes.TrimEnd() == "±")
                            {
                                e.Appearance.ForeColor = Color.Red;
                            }
                            else
                            {
                                foreach (string pos_res in res_positive_result.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                                {
                                    if (strRes == pos_res)
                                    {
                                        e.Appearance.ForeColor = Color.Red;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (!isCustomCritical && !hasNotAllowValue)
                    {
                        if (strRes != null
                        &&
                            (
                                strRes.TrimEnd() == "+"
                                || strRes.StartsWith("+")
                                || strRes.EndsWith("+")
                                || strRes.IndexOf("阳性") >= 0
                                || strRes.ToLower() == "pos"
                            )
                        && !strRes.TrimEnd().Contains("弱阳性")
                        && !strRes.TrimEnd().Equals("±")
                        && !(strRes.Length > 1 && strRes.Replace("+", "").TrimEnd() == string.Empty)
                        )
                        {
                            e.Appearance.ForeColor = Color.Red;
                        }
                        else if ((strRes.TrimEnd().Contains("弱阳性") || strRes.TrimEnd().Equals("±")) && hasNotAllowValue)
                        {
                            e.Appearance.ForeColor = Color.Red;
                        }
                    }
                }
            }


            if (dr.IsNew.ToString() == "1" && dr.ObrValue != null && dr.ObrValue.ToString().TrimEnd() != string.Empty)
            {
                if (e.Column.FieldName == "ItmEname.TrimEnd()")
                {
                    e.Appearance.BackColor = Color.Gold;
                }
            }
            //修改标识颜色提醒
            if (dr.IsModify == 1 && dr.ObrValue != null && dr.ObrValue.ToString().TrimEnd() != string.Empty)
            {
                if (e.Column.FieldName == "ItmEname.TrimEnd()")
                {
                    e.Appearance.BackColor = Color.DeepSkyBlue;
                }
            }

            #region 历史结果颜色控制

            //历史结果1
            EnumResRefFlag enumResRefFlag1 = (EnumResRefFlag)dr.RefFlagHistory1;
            if (FlagEquals(enumResRefFlag1, EnumResRefFlag.Positive) || FlagEquals(enumResRefFlag1, EnumResRefFlag.Greater1))
            {
                //检验结果数值的颜色显示（添加包含了历史结果也进行颜色控制）
                if (e.Column.FieldName == "HistoryResult1")
                {
                    e.Appearance.BackColor = Config.BackColorGreaterThanRef;
                    e.Appearance.ForeColor = Config.ForeColorGreaterThanRef;
                }

            }
            if ((enumResRefFlag1 & EnumResRefFlag.Lower1) == EnumResRefFlag.Lower1)
            {
                if (e.Column.FieldName == "HistoryResult1")
                {
                    e.Appearance.BackColor = Config.BackColorLowerThanRef;
                    e.Appearance.ForeColor = Config.ForeColorLowerThanRef;
                }
            }
            //历史结果2
            EnumResRefFlag enumResRefFlag2 = (EnumResRefFlag)dr.RefFlagHistory2;
            if (FlagEquals(enumResRefFlag2, EnumResRefFlag.Positive) || FlagEquals(enumResRefFlag2, EnumResRefFlag.Greater1))
            {
                //检验结果数值的颜色显示（添加包含了历史结果也进行颜色控制）
                if (e.Column.FieldName == "HistoryResult2")
                {
                    e.Appearance.BackColor = Config.BackColorGreaterThanRef;
                    e.Appearance.ForeColor = Config.ForeColorGreaterThanRef;
                }

            }
            if ((enumResRefFlag2 & EnumResRefFlag.Lower1) == EnumResRefFlag.Lower1)
            {
                if (e.Column.FieldName == "HistoryResult2")
                {
                    e.Appearance.BackColor = Config.BackColorLowerThanRef;
                    e.Appearance.ForeColor = Config.ForeColorLowerThanRef;
                }
            }


            //历史结果3
            EnumResRefFlag enumResRefFlag3 = (EnumResRefFlag)dr.RefFlagHistory3;
            if (FlagEquals(enumResRefFlag3, EnumResRefFlag.Positive) || FlagEquals(enumResRefFlag3, EnumResRefFlag.Greater1))
            {
                //检验结果数值的颜色显示（添加包含了历史结果也进行颜色控制）
                if (e.Column.FieldName == "HistoryResult3")
                {
                    e.Appearance.BackColor = Config.BackColorGreaterThanRef;
                    e.Appearance.ForeColor = Config.ForeColorGreaterThanRef;
                }
            }

            if ((enumResRefFlag3 & EnumResRefFlag.Lower1) == EnumResRefFlag.Lower1)
            {
                if (e.Column.FieldName == "HistoryResult3")
                {
                    e.Appearance.BackColor = Config.BackColorLowerThanRef;
                    e.Appearance.ForeColor = Config.ForeColorLowerThanRef;
                }
            }

            //报警信息
            if (!string.IsNullOrEmpty(dr.ObrWarn1) && dr.ObrWarn1.TrimEnd(null) != string.Empty)
            {
                double dValue;
                if (double.TryParse(dr.ObrWarn1, out dValue))
                {
                    if (Math.Abs(dValue) >= 5 && e.Column.FieldName == "ObrWarn1")
                    {
                        e.Appearance.ForeColor = Color.Red;
                    }
                }
            }
            //报警信息
            if (!string.IsNullOrEmpty(dr.ObrWarn2) && dr.ObrWarn2.TrimEnd(null) != string.Empty)
            {
                double dValue;
                if (double.TryParse(dr.ObrWarn2, out dValue))
                {
                    if (Math.Abs(dValue) >= 5 && e.Column.FieldName == "ObrWarn2")
                    {
                        e.Appearance.ForeColor = Color.Red;
                    }
                }
            }
            //报警信息
            if (!string.IsNullOrEmpty(dr.ObrWarn3) && dr.ObrWarn3.TrimEnd(null) != string.Empty)
            {
                double dValue;
                if (double.TryParse(dr.ObrWarn3, out dValue))
                {
                    if (Math.Abs(dValue) >= 5 && e.Column.FieldName == "ObrWarn3")
                    {
                        e.Appearance.ForeColor = Color.Red;
                    }
                }
            }

            //报警信息
            if (!string.IsNullOrEmpty(dr.ObrWarn4) && dr.ObrWarn4.TrimEnd(null) != string.Empty)
            {
                double dValue;
                if (double.TryParse(dr.ObrWarn4, out dValue))
                {
                    if (Math.Abs(dValue) >= 5 && e.Column.FieldName == "ObrWarn4")
                    {
                        e.Appearance.ForeColor = Color.Red;
                    }
                }
            }
            #endregion

            if (dr.ObrRecheckFlag != -1 && e.Column.FieldName == "ItmEname.TrimEnd()")
            {
                if (dr.ObrRecheckFlag == 1)
                    e.Appearance.BackColor = Color.FromArgb(255, 224, 192);
                if (dr.ObrRecheckFlag == 2)
                    e.Appearance.BackColor = Color.FromArgb(227, 239, 255);
            }
        }

        /// <summary>
        /// 包含Flag状态
        /// </summary>
        /// <returns></returns>
        private bool FlagEquals(EnumResRefFlag flag1, EnumResRefFlag flag2)
        {
            return (flag1 & flag2) == flag2;
        }

        /// <summary>
        /// 更新项目的样本号
        /// </summary>
        /// <param name="new_sid"></param>
        public void UpdateSID(string res_id, string new_sid)
        {
            foreach (EntityObrResult dr in this.dtPatientResulto)
            {
                dr.ObrSid = new_sid;
                dr.ObrId = res_id;
            }
        }

        /// <summary>
        /// 更改结果类型
        /// </summary>
        /// <param name="rep_flag"></param>
        public void ChangeRepFlag(string rep_flag)
        {
            if (rep_flag == LIS_Const.InstmtDataType.Eiasa)
            {
                this.gridViewSingle.Columns["ObrValue2"].Visible = true;
                this.gridViewSingle.Columns["ObrValue2"].VisibleIndex = 3;

                this.gridViewSingle.Columns["ObrValue3"].Visible = false;
                this.gridViewSingle.Columns["ObrValue4"].Visible = false;
            }
            else if (rep_flag == LIS_Const.InstmtDataType.Rapid)
            {
                this.gridViewSingle.Columns["ObrValue2"].Visible = false;
                this.gridViewSingle.Columns["ObrValue3"].Visible = true;
                this.gridViewSingle.Columns["ObrValue3"].VisibleIndex = 3;

                this.gridViewSingle.Columns["ObrValue4"].Visible = true;
                this.gridViewSingle.Columns["ObrValue4"].VisibleIndex = 4;
            }
            else
            {
                this.gridViewSingle.Columns["ObrValue2"].Visible = false;
                this.gridViewSingle.Columns["ObrValue3"].Visible = false;
                this.gridViewSingle.Columns["ObrValue4"].Visible = false;
            }
        }

        /// <summary>
        /// 刷新提示"当前项目数"
        /// </summary>
        /// <returns></returns>
        public void RefreshItemsCountText()
        {
            int count = 0;
            if (this.VisibleStyle == LIS_Const.ResultGridVisibleStyle.Single)
            {
                if (this.dtPatientResulto != null)
                {
                    count = this.dtPatientResulto.Count;
                }

            }
            this.lblItemsCount.Text = "项目数：" + count;
        }


        /// <summary>
        /// 保存成功后，更新项目isnew的值
        /// </summary>
        public void SaveSuccess()
        {
            if (this.dtPatientResulto != null)
            {
                for (int i = this.dtPatientResulto.Count - 1; i >= 0; i--)
                {
                    if (this.dtPatientResulto[i].IsNew == 2)
                    {
                        this.dtPatientResulto.RemoveAt(i);
                    }
                    else
                    {
                        this.dtPatientResulto[i].IsNew = 0;
                    }
                }
            }
        }

        /// <summary>
        /// 展示编辑器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView_ShowingEditor(object sender, CancelEventArgs e)
        {
            GridView grid = sender as GridView;
            if (grid.FocusedColumn.FieldName == "ObrValue" || grid.FocusedColumn.FieldName == "ObrValue2")
            {
                EntityObrResult dr = grid.GetRow(grid.FocusedRowHandle) as EntityObrResult;
                if (UserInfo.GetSysConfigValue("Lab_AllowEditCalItem") != "是" && dr.ObrType.ToString() == LIS_Const.PatResultType.Cal)
                {
                    toolTipController1.ShowHint("当前为关联计算结果，不能编辑");
                    e.Cancel = true;
                }
            }
        }

        private void gridViewSingle_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            //此处用作当用户更改结果单元的值时提示用户是否继续更改，此功能实现麻烦
            //可参考 http://devexpress.com/Support/Center/p/Q133351.aspx
        }


        /// <summary>
        /// 点击提示时查看临床意义
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewSingle_Click(object sender, EventArgs e)
        {
            //系统配置设置单击显示项目特征，就不显示临床意义。
            if (UserInfo.GetSysConfigValue("Lab_ResultShowItemPropMode") == "是")
            {
                gridView_DoubleClick(sender, e);
            }
            else
            {
                GridView gridView = sender as GridView;
                if (gridView == null)
                    return;

                GridHitInfo info;
                Point pt = gridView.GridControl.PointToClient(Control.MousePosition);
                if (pt == null)
                    return;
                info = gridView.CalcHitInfo(pt);
                if (info == null)
                    return;

                if (info.InRow && info.Column != null && info.Column.FieldName == "RefFlag")
                {
                    EntityObrResult dr = gridView.GetFocusedRow() as EntityObrResult;
                    if (dr != null && !Compare.IsNullOrDBNull(dr.ItmId.ToString()))
                    {
                        string itm_id = dr.ItmId.ToString();
                        string sam_id = this.samtypeid;
                        //DataRow[] drsItemSam = DictItem.Instance.Dict_item_sam.Select(string.Format("itm_id ='{0}' and itm_sam_id='{1}'", itm_id, sam_id));
                        List<EntityDicItemSample> listItmSam = CacheClient.GetCache<EntityDicItemSample>();
                        int iemSamIndex = listItmSam.FindIndex(i => i.ItmId == itm_id && i.ItmSamId == sam_id);
                        if (iemSamIndex > -1)
                        {
                            EntityDicItemSample itmSam = listItmSam[iemSamIndex];
                            //临床意义
                            if (dr != null && !string.IsNullOrEmpty(itmSam.ItmMeaning))
                            {
                                //隐藏前一个提示
                                if (toolTipController1.Active)
                                    toolTipController1.HideHint();

                                if (itmSam.ItmMeaning != "")
                                    toolTipController1.ShowHint(string.Format("临床意义：{0}", itmSam.ItmMeaning));
                            }
                        }
                    }
                }
            }
        }


        void repositoryItemTextEdit3_DoubleClick(object sender, System.EventArgs e)
        {

            //显示项目特征
            if (sender == null)
                return;

            TextEdit textEdit = (sender as TextEdit);
            if (textEdit == null || textEdit.Parent == null)
                return;

            GridControl mainControl = (textEdit.Parent as GridControl);
            if (mainControl == null)
                return;

            if (mainControl.MainView is GridView)
                this.ItemProp(mainControl.MainView as GridView);
        }

        private void gridViewSingle_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {

            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.Bounds = new Rectangle(e.Info.Bounds.X, e.Info.Bounds.Y, 35, e.Info.Bounds.Height);
                EntityObrResult dr = (sender as GridView).GetRow(e.RowHandle) as EntityObrResult;
                if (dr != null)
                {
                    e.Info.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                    e.Info.Appearance.ForeColor = Color.Black;
                    e.Info.DisplayText = (e.RowHandle + 1).ToString();
                    if (!string.IsNullOrEmpty(dr.IsNotEmpty))
                    {
                        if (Convert.ToInt32(dr.IsNotEmpty) == 1)
                        {
                            e.Info.Appearance.ForeColor = Color.Red;
                            e.Info.DisplayText += "*";
                        }
                        else
                        {
                            e.Info.DisplayText += "";
                        }
                    }
                }
            }
        }

        public void MoveFirst()
        {
            this.gridViewSingle.MoveFirst();
        }


        public bool isEnt = true;
        private void gridControlSingle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                GridView gv = (sender as GridControl).MainView as GridView;

                if (gv != null)
                {
                    EntityObrResult dr = gv.GetFocusedRow() as EntityObrResult;

                    if (dr != null)
                    {
                        if (!Compare.IsEmpty(dr.ObrType) && Convert.ToInt32(dr.ObrType) == 2)
                        {

                            isEnt = true;
                            //e.Handled = true;
                            gv.MoveNext();


                        }
                    }
                }
            }
        }

        private void gridViewSingle_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle == this.gridViewSingle.FocusedRowHandle)
            {
                e.Appearance.BackColor = System.Drawing.Color.LightBlue;
                e.Appearance.Options.UseBackColor = true;

            }

            EntityObrResult dr = this.gridViewSingle.GetRow(e.RowHandle) as EntityObrResult;
            if (dr != null)
            {
                if (dr.ObrRecheckFlag.ToString().TrimEnd() == "1")
                    e.Appearance.BackColor = Color.FromArgb(255, 224, 192);
                if (dr.ObrRecheckFlag.ToString().TrimEnd() == "2")
                    e.Appearance.BackColor = Color.FromArgb(227, 239, 255);
            }
        }

        /// <summary>
        /// 显示必录/全部 切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.radioGroup1.EditValue.ToString() == "0")//必录
            {
                if (this.patFlag == "0")
                {
                    RefreshCurrentCombineItems();

                    if (Lab_FilterHasResultItem)
                    {
                        FilterResultItems();
                    }
                    else
                    {
                        ClearNoneResultItems(true);

                    }
                    if (UserInfo.GetSysConfigValue("Lab_PopedomFilter") == "必录+历史结果" && patFlag != "2" && patFlag != "4")
                    {
                        SetMustHistoryRes(listHisResult);
                    }
                }
            }
            else if (this.radioGroup1.EditValue.ToString() == "1")//全部
            {
                if (this.patFlag == "0")
                {
                    if (new DictCombineMi().GetCombineMibyResult(patients_mi, this.dtPatientResulto).Count > 0)
                    {
                        if (Lab_FilterHasResultItem && ResultDatatableBak != null && ResultDatatableBak.Count > 0)
                        {
                            foreach (EntityObrResult dr in ResultDatatableBak)
                            {
                                if (dtPatientResulto.FindAll(i => i.ItmId == dr.ItmId).Count == 0)
                                {
                                    dtPatientResulto.Add(dr);
                                }
                            }
                            ResultDatatableBak = null;
                        }
                        RefreshCurrentCombineItems();
                    }
                }
            }
            BindGrid();
        }
        private void SetMustHistoryRes(List<EntityObrResult> data)
        {
            #region 过滤"必录+历史结果1
            if (data.Count > 0)
            {
                List<EntityObrResult> dataFilter = PatientHistoryViewFilter(data);
                if (dtPatientResulto == null || dtPatientResulto.Count == 0) return;
                List<string> listItmId = new List<string>();
                if (dataFilter != null && dataFilter.Count > 0)
                {
                    foreach (EntityObrResult item in dataFilter)
                    {
                        //找出没有显示历史结果1的项目
                        if (dtPatientResulto.FindAll(w => w.ItmId == item.ItmId).Count <= 0)
                            listItmId.Add(item.ItmId);
                    }
                }
                int count = 0;
                int seq = dtPatientResulto.Count;
                //防止重复添加
                if (listItmId.Count > 0)
                {
                    for (int i = 0; i < listItmId.Count; i++)
                    {
                        EntityObrResult insertEntity = new EntityObrResult();
                        //存在则不添加
                        if (dtPatientResulto.FindAll(w => w.ItmId == listItmId[count]).Count > 0) continue;

                        List<EntityObrResult> dtPatResult = dataFilter.FindAll(w => w.ItmId == listItmId[count]);
                        if (dtPatResult.Count > 0)
                        {
                            insertEntity.ObrId = PatID;
                            insertEntity.ItmId = dtPatResult[0].ItmId;
                            insertEntity.ComMiSort = seq + 1;
                            insertEntity.ResComSeq = 999;
                            insertEntity.ItmName = dtPatResult[0].ItmName;
                            insertEntity.ItmEname = dtPatResult[0].ItmEname.TrimEnd();
                            insertEntity.ItmComId = "-1";
                            if (!string.IsNullOrEmpty(dtPatResult[0].RefLowerLimit) && !string.IsNullOrEmpty(dtPatResult[0].RefUpperLimit))
                            {
                                insertEntity.ResRefRange = dtPatResult[0].RefLowerLimit + "-" + dtPatResult[0].RefUpperLimit;
                            }
                            else if (!string.IsNullOrEmpty(dtPatResult[0].RefLowerLimit) && string.IsNullOrEmpty(dtPatResult[0].RefUpperLimit))
                            {
                                insertEntity.ResRefRange = dtPatResult[0].RefLowerLimit;
                            }
                            else if (string.IsNullOrEmpty(dtPatResult[0].RefLowerLimit) && !string.IsNullOrEmpty(dtPatResult[0].RefUpperLimit))
                            {
                                insertEntity.ResRefRange = dtPatResult[0].RefUpperLimit;

                            }
                            insertEntity.RefLowerLimit = dtPatResult[0].RefLowerLimit;
                            insertEntity.RefUpperLimit = dtPatResult[0].RefUpperLimit;
                            insertEntity.ResRefLCal = dtPatResult[0].RefLowerLimit;
                            insertEntity.ResRefHCal = dtPatResult[0].RefUpperLimit;
                            insertEntity.HistoryResult1 = dtPatResult[0].ObrValue;
                            insertEntity.ObrType = dtPatResult[0].ObrType;
                            insertEntity.ObrRecheckFlag = dtPatResult[0].ObrRecheckFlag;
                            insertEntity.IsNew = 1;
                            insertEntity.ObrDate = ServerDateTime.GetServerDateTime();
                            dtPatientResulto.Add(insertEntity);
                            count++;
                            seq++;
                        }

                    }
                    foreach (EntityObrResult dr in dtPatientResulto)
                    {
                        CalcPatResultRow(dr, true);
                    }

                }
            }
            #endregion
        }
        private void FilterResultItems()
        {

            ResultDatatableBak = new List<EntityObrResult>();

            for (int i = dtPatientResulto.Count - 1; i >= 0; i--)
            {
                EntityObrResult dr = dtPatientResulto[i];
                if (!string.IsNullOrEmpty(dr.IsNotEmpty) && dr.IsNotEmpty.ToString() != "1" && string.IsNullOrEmpty(dr.ObrValue))
                {

                    if (ResultDatatableBak.FindAll(w => w.ItmId == dr.ItmId).Count == 0)
                    {
                        ResultDatatableBak.Add(dr);
                    }
                    dtPatientResulto.Remove(dr);
                }
            }
        }


        private int GetConfigOnNullAge(int age)
        {
            if (age < 0)
            {
                //获取参考值：年龄/性别为空时的计算规则
                string configCalAge = UserInfo.GetSysConfigValue("GetRefOnNullAge");

                int calage = -1;

                if (!string.IsNullOrEmpty(configCalAge)
                    && configCalAge != "不计算参考值")
                {
                    calage = AgeConverter.YearToMinute(Convert.ToInt32(configCalAge));
                    if (age >= 0)
                    {
                        calage = age;
                    }
                }
                return calage;
            }
            else
            {
                return age;
            }
        }

        private string GetConfigOnNullSex(string sex)
        {
            if (string.IsNullOrEmpty(sex)

                || (sex != "1"
                && sex != "2"
                && sex != "0"))
            {
                //获取参考值：年龄/性别为空时的计算规则
                string configCalSex = UserInfo.GetSysConfigValue("GetRefOnNullSex");

                if (configCalSex.Contains("男"))
                {
                    return "1";
                }
                else if (configCalSex.Contains("女"))
                {
                    return "2";
                }

                return "0";
            }
            else
            {
                return sex;
            }
        }


        private void 报告复查ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PatID != null && PatID.TrimEnd() != string.Empty)
            {
                if (patFlag == "0")
                {
                    int[] selectedRowHandler = this.gridViewSingle.GetSelectedRows();

                    if (selectedRowHandler.Length == 0)
                    {
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("请选择要复查的项目！");
                        return;
                    }

                    List<EntityObrResult> recheckResult = new List<EntityObrResult>();

                    foreach (int rowHandler in selectedRowHandler)
                    {
                        EntityObrResult rowResult = this.gridViewSingle.GetRow(rowHandler) as EntityObrResult;
                        if (!string.IsNullOrEmpty(rowResult.ObrValue))
                            recheckResult.Add(rowResult);
                        else
                        {
                            MessageBox.Show("该项目无结果,不进行复查");
                            return;
                        }
                    }

                    ProxyPatientRecheck recheckProxy = new ProxyPatientRecheck();
                    if (recheckProxy.Service.RecheckResultItem(PatID, recheckResult))
                    {
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("已标记复查！");
                        复查ToolStripMenuItem.Enabled = true;
                        foreach (int rowHandler in selectedRowHandler)
                        {
                            EntityObrResult rowResult = this.gridViewSingle.GetRow(rowHandler) as EntityObrResult;
                            rowResult.ObrRecheckFlag = 1;
                        }
                        if (dtPatInfoCopy != null)
                        {
                            dtPatInfoCopy.RepRecheckFlag = 1;
                        }
                    }
                }
                else
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("该记录已审核/报告！");
            }

        }

        /// <summary>
        /// 设置结果是否可修改
        /// </summary>
        /// <param name="flag"></param>
        public void setResultCouldModify(bool flag)
        {
            if (gridViewSingle != null)
            {
                if (gridViewSingle.Columns[2] != null)
                {
                    this.gridViewSingle.Columns[2].OptionsColumn.ReadOnly = flag;
                    this.gridViewSingle.Columns[2].OptionsColumn.AllowEdit = !flag;
                    this.gridViewSingle.Columns[2].OptionsColumn.AllowEdit = !flag;

                    this.menuAddItem.Enabled = !flag;
                    this.menuDelItem.Enabled = !flag;
                    this.menuItemProp.Enabled = !flag;
                }
            }
        }

        /// <summary>
        /// 保存列顺序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveColumnSortToolStrip_Click(object sender, EventArgs e)
        {
            int count = 0;
            for (int m = 0; m < gridViewSingle.Columns.Count; m++)
            {
                if (gridViewSingle.Columns[m].Visible)
                {
                    count++;
                }
            }
            int[] sort = new int[count];
            for (int m = 0; m < sort.Length; m++)
            {
                sort[m] = -1;
            }
            int i = 0;
            foreach (DevExpress.XtraGrid.Columns.GridColumn column in gridViewSingle.Columns)
            {
                if (column.Visible)
                {
                    sort[i] = column.VisibleIndex;
                    i++;
                }
            }

            string sortStr = string.Empty;
            for (int m = 0; m < sort.Length - 1; m++)
            {
                sortStr += sort[m].ToString() + ",";
            }
            sortStr += sort[sort.Length - 1];

            bool result = new ProxyPatResult().Service.SaveColumnSort(sortStr);
            if (result)
            {
                MessageDialog.ShowAutoCloseDialog("保存成功！", 1);
            }
            else
                MessageDialog.ShowAutoCloseDialog("保存失败！", 1);

        }

        public void SetColumnFocus()
        {
            gridViewSingle.Focus();
            if (gridViewSingle.RowCount > 0)
            {
                gridViewSingle.SelectCell(0, colres_chr);
                gridViewSingle.ShowEditor();
            }
        }

        /// <summary>
        /// 设置列顺序
        /// </summary>
        public void setColumnSort()
        {
            string sort = ConfigHelper.GetSysConfigValueWithoutLogin("PatResultColumnSort");
            if (string.IsNullOrEmpty(sort))
            {
                return;
            }
            //sort = sort.Replace(",","");
            int i = 0;
            try
            {
                foreach (DevExpress.XtraGrid.Columns.GridColumn column in gridViewSingle.Columns)
                {
                    if (column.Visible)
                    {
                        //if (sort.Substring(0,sort.IndexOf(',')))
                        ///{
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
                Logger.WriteException("加载结果浏览页列顺序出错", "dcl.client.result.PatControl.PatResult.setColumnSort", ex.Message);
            }

        }
        public event ClaItemInfoEventHandler ClaItemInfo = null;

        private int note_history_col = 0;//记录历史结果-列标识
        private string note_history_rowValue = "";//记录历史结-果行内容


        public event ClaItemInfoEventHandler NullItemInfo = null;

        public event ClaItemInfoEventHandler LostItemInfo = null;

        /// <summary>
        /// 焦点移动到历史结果上,显示日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewSingle_MouseMove(object sender, MouseEventArgs e)
        {
            GridView gridView = sender as GridView;
            if (gridView == null)
                return;

            GridHitInfo info;
            Point pt = gridView.GridControl.PointToClient(Control.MousePosition);
            if (pt == null)
                return;
            info = gridView.CalcHitInfo(pt);
            if (info == null)
                return;


            if (info.InRow && info.Column != null
                && (info.Column.FieldName == "HistoryResult1"
                || info.Column.FieldName == "HistoryResult2"
                || info.Column.FieldName == "HistoryResult3"))
            {

                if (info.InRow && info.Column != null && info.Column.FieldName == "HistoryResult1")
                {
                    EntityObrResult dr = gridView.GetRow(info.RowHandle) as EntityObrResult;
                    if (dr != null && !Compare.IsNullOrDBNull(dr.HistoryDate1))
                    {
                        if (note_history_col == 1 && note_history_rowValue == dr.ItmEname.TrimEnd().ToString())
                        {
                            return;
                        }
                        else
                        {
                            note_history_col = 1;
                            note_history_rowValue = dr.ItmEname.TrimEnd().ToString();
                        }

                        //隐藏前一个提示
                        if (toolTipController1.Active)
                            toolTipController1.HideHint();

                        toolTipController1.ShowHint("1 项目:" + dr.ItmEname.TrimEnd().ToString() + "\r\n" + dr.HistoryDate1);
                    }
                }
                else if (info.InRow && info.Column != null && info.Column.FieldName == "HistoryResult2")
                {
                    EntityObrResult dr = gridView.GetRow(info.RowHandle) as EntityObrResult;
                    if (dr != null && !Compare.IsNullOrDBNull(dr.HistoryDate2))
                    {
                        if (note_history_col == 2 && note_history_rowValue == dr.ItmEname.TrimEnd().ToString())
                        {
                            return;
                        }
                        else
                        {
                            note_history_col = 2;
                            note_history_rowValue = dr.ItmEname.TrimEnd().ToString();
                        }

                        //隐藏前一个提示
                        if (toolTipController1.Active)
                            toolTipController1.HideHint();

                        toolTipController1.ShowHint("2 项目:" + dr.ItmEname.TrimEnd().ToString() + "\r\n" + dr.HistoryDate2);
                    }
                }
                else if (info.InRow && info.Column != null && info.Column.FieldName == "HistoryResult3")
                {
                    EntityObrResult dr = gridView.GetRow(info.RowHandle) as EntityObrResult;
                    if (dr != null && !Compare.IsNullOrDBNull(dr.HistoryDate3))
                    {
                        if (note_history_col == 3 && note_history_rowValue == dr.ItmEname.TrimEnd().ToString())
                        {
                            return;
                        }
                        else
                        {
                            note_history_col = 3;
                            note_history_rowValue = dr.ItmEname.TrimEnd().ToString();
                        }

                        //隐藏前一个提示
                        if (toolTipController1.Active)
                            toolTipController1.HideHint();

                        toolTipController1.ShowHint("3 项目:" + dr.ItmEname.TrimEnd().ToString() + "\r\n" + dr.HistoryDate3);
                    }
                }
            }
            else if (info.InRow && info.Column != null && info.Column.FieldName == "RefFlag")
            {
                //如果为特定列，不消除，防止跟点击事件冲突
                note_history_col = 0;//非历史结果列,标识为0
            }
            else
            {
                note_history_col = 0;//非历史结果列,标识为0

                if (toolTipController1.Active)
                    toolTipController1.HideHint();
            }
        }

        /// <summary>
        /// 导出excel模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuExportXlsMu_Click(object sender, EventArgs e)
        {
            #region 生成GridControl和GridView

            //GridView
            DevExpress.XtraGrid.Views.Grid.GridView gvExport_Phy = new DevExpress.XtraGrid.Views.Grid.GridView();

            //GridControl
            DevExpress.XtraGrid.GridControl gcExport_Physical = new DevExpress.XtraGrid.GridControl();

            //GridColumn
            DevExpress.XtraGrid.Columns.GridColumn colExport1 = new DevExpress.XtraGrid.Columns.GridColumn();
            DevExpress.XtraGrid.Columns.GridColumn colExport2 = new DevExpress.XtraGrid.Columns.GridColumn();
            DevExpress.XtraGrid.Columns.GridColumn colExport3 = new DevExpress.XtraGrid.Columns.GridColumn();
            DevExpress.XtraGrid.Columns.GridColumn colExport4 = new DevExpress.XtraGrid.Columns.GridColumn();
            //DevExpress.XtraGrid.Columns.GridColumn colExport5 = new DevExpress.XtraGrid.Columns.GridColumn();

            //gcExport_Physical
            gcExport_Physical.Name = "gcExport_Physical";
            //gcExport_Physical.Visible = true;
            gcExport_Physical.MainView = gvExport_Phy;
            gcExport_Physical.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
                gvExport_Phy});

            //gvExport_Phy
            gvExport_Phy.Name = "gvExport_Phy";
            gvExport_Phy.GridControl = gcExport_Physical;
            gvExport_Phy.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
                colExport1,colExport2,colExport3,colExport4});


            // 
            // colExport1
            // 
            colExport1.Caption = "样本编码";
            colExport1.FieldName = "res_sid";
            colExport1.Name = "colExport1";
            colExport1.Visible = true;
            colExport1.VisibleIndex = 0;
            colExport1.Width = 131;
            // 
            // colExport2
            // 
            colExport2.Caption = "项目代码";
            colExport2.FieldName = "res_itm_ecd";
            colExport2.Name = "colExport2";
            colExport2.Visible = true;
            colExport2.VisibleIndex = 1;
            colExport2.Width = 122;
            // 
            // colExport3
            // 
            colExport3.Caption = "结果";
            colExport3.FieldName = "ObrValue";
            colExport3.Name = "colExport3";
            colExport3.Visible = true;
            colExport3.VisibleIndex = 2;
            colExport3.Width = 90;
            // 
            // colExport4
            // 
            colExport4.Caption = "单位";
            colExport4.FieldName = "res_unit";
            colExport4.Name = "colExport4";
            colExport4.Visible = true;
            colExport4.VisibleIndex = 3;
            colExport4.Width = 86;
            // 
            // colExport5
            // 
            //colExport5.Caption = "病人ID";
            //colExport5.FieldName = "pat_in_no";
            //colExport5.Name = "colExport5";
            //colExport5.Visible = true;
            //colExport5.VisibleIndex = 4;
            //colExport5.Width = 101;

            #endregion

            DataTable dtExportPhy = new DataTable("templet");

            dtExportPhy.Columns.Add("res_sid", System.Type.GetType("System.String"));//样本编码
            dtExportPhy.Columns.Add("res_itm_ecd", System.Type.GetType("System.String"));//项目代码
            dtExportPhy.Columns.Add("ObrValue", System.Type.GetType("System.String"));//结果
            dtExportPhy.Columns.Add("res_unit", System.Type.GetType("System.String"));//单位


            dtExportPhy.Rows.Add(new object[] { "10001", "WBC", "7.33", "10^9/L" });

            gcExport_Physical.DataSource = dtExportPhy;//

            setExcel(gcExport_Physical);//导出模板
        }

        /// <summary>
        /// 导出Excel方法
        /// </summary>
        /// <param name="gcExcel"></param>
        private void setExcel(DevExpress.XtraGrid.GridControl gcExcel)
        {
            if (gcExcel.DataSource != null)
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.DefaultExt = "xls";
                ofd.Filter = "Excel文件(*.xls)|*.xls";
                ofd.Title = "导出到Excel";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.TrimEnd() == string.Empty)
                    {
                        lis.client.control.MessageDialog.Show("文件名不能为空！", "提示");
                        return;
                    }

                    try
                    {
                        gcExcel.ExportToXls(ofd.FileName.TrimEnd());
                        lis.client.control.MessageDialog.Show("导出模板成功！", "提示");
                    }
                    catch (Exception ex)
                    {
                        dcl.root.logon.Logger.WriteException(this.GetType().Name, "导出模板", ex.ToString());
                        lis.client.control.MessageDialog.Show("导出模板失败！", "提示");
                    }
                }

            }
        }

        private void menuItemResCompare_Click(object sender, EventArgs e)
        {
            FrmItemCompare cp = new FrmItemCompare();
            cp.ResData = dtPatientResulto;
            cp.ShowDialog();
        }

        public void SetPicVisable(string itr_id)
        {
            showPicForm = false;
            if (DictInstrmt.Instance.ShowPicInResList(itr_id))
            {
                showPicForm = true;
                if (dockPanel2.Visibility == DevExpress.XtraBars.Docking.DockVisibility.Hidden)
                {
                    string Lab_ShowItrPicWinSize = UserInfo.GetSysConfigValue("Lab_ShowItrPicWinSize");

                    int width = 0;
                    int height = 0;
                    if (!string.IsNullOrEmpty(Lab_ShowItrPicWinSize))
                    {
                        string[] array = Lab_ShowItrPicWinSize.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (array.Length == 2)
                        {
                            if (!int.TryParse(array[0], out width))
                            {
                                width = 0;
                            }
                            if (!int.TryParse(array[1], out height))
                            {
                                height = 0;
                            }
                        }
                    }
                    this.dockPanel2.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
                    if (height > 0)
                    {
                        this.dockPanel2.Height = height;
                    }
                    this.dockPanel2.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
                }

            }
            else
            {
                this.dockPanel2.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
            }
        }

        private void 复查ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (PatID != null && PatID.TrimEnd() != string.Empty)
                {
                    patExp = parentFormNew.fpat_exp2.Text;
                    if (patFlag == "0")
                    {
                        bool ok = false;
                        if (showItrWarningMsg || !ConfigHelper.IsNotOutlink())
                        {
                            FrmRecheckConfirm frm = new FrmRecheckConfirm();
                            frm.Init(PatID, Pat_itr_id);
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                if (patExp == null || patExp.TrimEnd() == string.Empty)
                                    patExp = frm.RemarkMsg;
                                else
                                    patExp += "," + frm.RemarkMsg;
                                ok = true;
                            }
                        }
                        else
                        {
                            ok = true;
                            List<EntityDicInstrument> listInst = CacheClient.GetCache<EntityDicInstrument>();
                            int index = listInst.FindIndex(i => i.ItrId == Pat_itr_id);

                            string strUrt = string.Empty;

                            if (index > -1 && listInst[index].ItrMicroFlag.ToString() == "1")
                                strUrt = "镜检";

                            if (patExp == null || patExp.TrimEnd() == string.Empty ||
                                patExp.TrimEnd() == ("标本已" + strUrt + "复查"))
                                patExp = "标本已" + strUrt + "复查";
                            else
                                patExp += ",标本已" + strUrt + "复查";

                        }
                        if (ok)
                        {
                            EntityPidReportMain updatePat = new EntityPidReportMain();
                            updatePat.RepId = PatID;
                            updatePat.RepRemark = patExp;
                            updatePat.RepRecheckFlag = 2;

                            parentFormNew.fpat_exp2.Text = patExp;
                            if (new ProxyPidReportMain().Service.UpdateRepRecheckFlag(updatePat))
                            {
                                lis.client.control.MessageDialog.ShowAutoCloseDialog("复查完毕！");
                                if (dtPatInfoCopy != null)
                                {
                                    dtPatInfoCopy.RepRecheckFlag = 2;
                                }
                                复查ToolStripMenuItem.Enabled = false;
                            }
                        }
                    }
                    else
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("该记录已审核/报告！");
                }
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog(ex.Message);
            }
        }

        private void menuDoubleView_Click(object sender, EventArgs e)
        {
            FixSingleColumn = false;
            AutoDouble = true;
            BindGrid();
        }
    }




    /// <summary>
    /// 计算公式辅助信息
    /// </summary>
    public class CalInfoEventArgs : EventArgs
    {
        public CalInfoEventArgs()
        {
            SampName = string.Empty;
            SampRem = string.Empty;
        }
        /// <summary>
        /// 标本
        /// </summary>
        public string SampName { get; set; }
        /// <summary>
        /// 标本备注
        /// </summary>
        public string SampRem { get; set; }
        /// <summary>
        /// 标本ID
        /// </summary>
        public string SampID { get; set; }

        /// <summary>
        /// 仪器ID
        /// </summary>
        public string itm_itr_id { get; set; }


        /// <summary>
        /// sex
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// sex
        /// </summary>
        public string pat_weight { get; set; }

        /// <summary>
        /// age
        /// </summary>
        public double? Age { get; set; }

        public string TipText { get; set; }

        public bool lostitem { get; set; }
    }
    public delegate void ClaItemInfoEventHandler(object obj, CalInfoEventArgs args);

}
