using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using dcl.common.extensions;
using dcl.client.common;
using DevExpress.XtraGrid.Views.Grid;
using dcl.client.frame;
using dcl.client.wcf;
using lis.client.control;
using System.Diagnostics;
using dcl.common;
using System.CodeDom.Compiler;
using System.Reflection;
using dcl.root.logon;
using dcl.client.users;
using dcl.entity;
using System.Linq;
using System.IO;
using dcl.client.cache;


namespace dcl.client.sample
{
    public partial class BCPrintControl : ClientBaseControl, IStepable, IPrintable
    {

        private string DepartCode = string.Empty;

        public BCPrintControl()
        {
            InitializeComponent();
            this.Load += BCPrintControl_Load;
        }

        private void BCPrintControl_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            sysToolBar1.BtnImport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(BtnImport_Click);
            sysToolBar1.OnBtnQualityDataClicked += SysToolBar1_OnBtnQualityDataClicked;
            cbTypeID.SelectedIndexChanged += new EventHandler(cbTypeID_SelectedIndexChanged);
            this.Disposed += new EventHandler(BCPrintControl_Disposed);
            scrollingText1.Click += scrollingText1_Click;

            //是否启用读卡器
            string cardconvertConfig = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_EnableCardReader");

            //开启读取银行卡
            card_ReadBankCar = ConfigHelper.GetSysConfigValueWithoutLogin("Card_ReadBankCar") == "是";

            preBarCodeCheckCuvType = ConfigHelper.GetSysConfigValueWithoutLogin("BarCode_PreBarCodeCheckCuvType") == "是";
            //读卡器驱动类型
            strCardReaderDriver = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_CardReaderDriver");
            //门诊、住院、体检默认是否下载后自动打印条码
            string Barcode_MZBarcodeAutoPrint = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_MZBarcodeAutoPrint");
            string Barcode_TJBarcodeAutoPrint = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_TJBarcodeAutoPrint");

            if (!string.IsNullOrEmpty(cardconvertConfig))
            {
                if (this.PrintType == PrintType.Inpatient)
                {
                    this.EnableCardReader = cardconvertConfig.Contains("住院");
                }
                else if (this.PrintType == PrintType.Outpatient)
                {
                    this.EnableCardReader = cardconvertConfig.Contains("门诊");
                }
                else if (this.PrintType == PrintType.TJ)
                {
                    this.EnableCardReader = cardconvertConfig.Contains("体检");
                }
            }

            //住院条码不用检查权限
            sysToolBar1.CheckPower = this.CheckPower;
            sysToolBar1.BtnDelete.Caption = "删除条码";
            sysToolBar1.BtnDeleteSub.Caption = "删除明细";
            sysToolBar1.BtnUndo.Caption = "重置条码";
            sysToolBar1.BtnSinglePrint.Caption = "打印标本备注";
            sysToolBar1.BtnResultView.Caption = "回退查询";
            sysToolBar1.BtnAdd.Caption = "生成条码"; //新增
            sysToolBar1.BtnAdd.Tag = "";

            //判断是否有门诊回执报表
            bool blnHaveMZBCReturn = false;

            if (!string.IsNullOrEmpty(ConfigHelper.GetSysConfigValueWithoutLogin("MZBarcodeReturnReport")))
            {
                blnHaveMZBCReturn = true;
            }
            string[] strBtnName;
            string[] strBtntool;

            sysToolBar1.BtnImport.Caption = "读卡并下载";
            sysToolBar1.BtnConfirm.Caption = "采集确认";
            //使用的条码类型：自动条码、预置条码
            string barcode_type = ConfigHelper.GetSysConfigValueWithoutLogin("7");
            //string strUndoButrn = (ConfigHelper.GetSysConfigValueWithoutLogin("PrePlaceBarcode") == "是") ? sysToolBar1.BtnUndo.Name : string.Empty;
            string strUndoButrn = (barcode_type == "预置条码" ? sysToolBar1.BtnUndo.Name : string.Empty);
            string btnReadCarName = this.EnableCardReader ? sysToolBar1.BtnImport.Name : string.Empty;

            bool blnPrintBcExp = ConfigHelper.GetSysConfigValueWithoutLogin("PrintBcExp") == "是";


            string strBtnSampling = string.Empty;
            string strBtnGet = string.Empty;//收取
            string strBtnChangePwd = string.Empty;
            string strBtnBcSearch = string.Empty;
            string strBtnYgBarcode = string.Empty;//院感条码

            string strBtnVerifOrder = string.Empty;//医嘱查询
            strBtnVerifOrder = sysToolBar1.BtnQualityData.Name;
            sysToolBar1.BtnQualityData.Caption = "医嘱查询";

            if (IsAlone)
            {
                strBtnSampling = sysToolBar1.btnAmendment.Name;
                strBtnGet = sysToolBar1.BtnPageDown.Name;
                //strBtnVerifOrder = sysToolBar1.BtnQualityData.Name;
                strBtnChangePwd = sysToolBar1.BtnQualityTest.Name;
                strBtnBcSearch = sysToolBar1.btnBrowse.Name;
                strBtnYgBarcode = sysToolBar1.BtnDeSpe.Name;
                sysToolBar1.btnAmendment.Caption = "采集确认";
                sysToolBar1.BtnPageDown.Caption = "收取确认";
                sysToolBar1.BtnQualityData.Caption = "医嘱查询";
                sysToolBar1.BtnQualityTest.Caption = "密码修改";
                sysToolBar1.btnBrowse.Caption = "条码查询";
                sysToolBar1.BtnDeSpe.Caption = "院感条码";
                sysToolBar1.BtnAmendmentClick += btnBCSignin_Click;
                sysToolBar1.OnBtnPageDownClicked += sysToolBar_GetConfirmClick;
                sysToolBar1.OnBtnQualityTestClicked += sysToolBar_ChangePasswordClick;
                sysToolBar1.BtnBrowseClick += sysToolBar1_BtnBrowseClick;
                sysToolBar1.BtnDeSpeClick += sysToolBar_ManuBarcodeClick;
            }

            //如果没有报表的话就不显示此打印条码回执按钮
            if (blnHaveMZBCReturn && (this.Printer is OutPaitent))
            {
                if (Barcode_MZBarcodeAutoPrint == "是")
                {
                    chkAutoPrintBarcode.Checked = true;
                }
                else
                {
                    chkAutoPrintBarcode.Checked = false;
                }


                if (ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_AutoCheckPrintReturnBarcode") != "否")
                {
                    chkAutoPrintReturnBarcode.Checked = true;
                    this.Printer.AutoPrintReturnBarcode = true;
                }


                strBtnName = new string[] {
                sysToolBar1.BtnBCPrint.Name,
                sysToolBar1.BtnBCPrintReturn.Name,
                sysToolBar1.BtnDelete.Name,
                sysToolBar1.BtnDeleteSub.Name,
                sysToolBar1.BtnPrintList.Name,
                sysToolBar1.BtnSperateBarcode.Name,
                sysToolBar1.BtnQualityOut.Name,
                sysToolBar1.BtnResultView.Name,
                sysToolBar1.BtnPrintSet.Name,
                strUndoButrn,
                btnReadCarName,
                blnPrintBcExp?sysToolBar1.BtnSinglePrint.Name:string.Empty,
                sysToolBar1.BtnConfirm.Name,
                sysToolBar1.BtnImport.Name,
                strBtnSampling,
                strBtnGet,
                strBtnVerifOrder,
                strBtnChangePwd,
                strBtnBcSearch
                ,sysToolBar1.BtnAdd.Name,  //生成条码按钮
                sysToolBar1.BtnClose.Name
                };

                strBtntool = new string[]{"F2", "F3","F4",
                };
            }
            else
            {
                if (this.Printer.Name == "住院")
                {
                    string YgBtnName = string.Empty;
                    //住院条码打印界面增加院感条码按钮
                    if (ConfigHelper.GetSysConfigValueWithoutLogin("BC_ShowManuaBarcodeInZY") == "是")
                        YgBtnName = strBtnYgBarcode;

                    strBtnName = new string[] {
                    sysToolBar1.BtnGetVersion.Name,
                    sysToolBar1.BtnBCPrint.Name,
                    sysToolBar1.BtnBCPrintReturn.Name,
                    sysToolBar1.BtnDelete.Name,
                    sysToolBar1.BtnDeleteSub.Name,
                    sysToolBar1.BtnPrintList.Name,
                    sysToolBar1.BtnSperateBarcode.Name,
                    sysToolBar1.BtnQualityOut.Name,
                    sysToolBar1.BtnResultView.Name,
                    sysToolBar1.BtnPrintSet.Name,
                    strUndoButrn,

                    btnReadCarName,
                    blnPrintBcExp ? sysToolBar1.BtnSinglePrint.Name : string.Empty,
                    sysToolBar1.BtnConfirm.Name,
                    sysToolBar1.BtnSearch.Name,
                    sysToolBar1.BtnClose.Name,
                    strBtnSampling,
                    strBtnGet,
                    strBtnVerifOrder,
                    strBtnChangePwd,
                    strBtnBcSearch
                    ,sysToolBar1.BtnAdd.Name //生成条码按钮
                    ,YgBtnName  //院感条码
                };

                    strBtntool = new string[] { "F1", "F2", "F3", "F4" };
                }
                else
                {
                    strBtnName = new string[] {
                    sysToolBar1.BtnBCPrint.Name,
                    sysToolBar1.BtnBCPrintReturn.Name,
                    sysToolBar1.BtnDelete.Name,
                    sysToolBar1.BtnDeleteSub.Name,
                    sysToolBar1.BtnPrintList.Name,
                    sysToolBar1.BtnSperateBarcode.Name,
                    sysToolBar1.BtnQualityOut.Name,
                    sysToolBar1.BtnResultView.Name,
                    sysToolBar1.BtnPrintSet.Name,
                    strUndoButrn,
                    btnReadCarName,
                    blnPrintBcExp?sysToolBar1.BtnSinglePrint.Name:string.Empty,
                    sysToolBar1.BtnConfirm.Name,
                    sysToolBar1.BtnClose.Name,
                    strBtnSampling,
                    strBtnGet,
                    strBtnVerifOrder,
                    strBtnChangePwd,
                    strBtnBcSearch
                    ,sysToolBar1.BtnAdd.Name //生成条码按钮
                    };

                    strBtntool = new string[] { "F2", "F3", "F4" };
                }
            }
            sysToolBar1.BtnGetVersion.Caption = "下载";
            sysToolBar1.BtnQualityOut.Caption = "合并条码";
            sysToolBar1.BtnGetVersionClick += SysToolBar1_BtnGetVersionClick;

            if (this.Printer is Inpatient)
            {
                chkAutoPrintBarcode.Visible = false;
                chkAutoPrintBarcode.Checked = false;
                chkAutoPrintReturnBarcode.Visible = false;
                chkAutoPrintReturnBarcode.Checked = false;
                sysToolBar1.BtnBCPrintReturn.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                if (IsAlone)
                {
                    dcl.client.notifyclient.BcSamplToReceiveTATNotifyHelper.Current.start();
                }
            }
            else if (this.Printer is TJPaitent)
            {
                patientControl.SetTjCompanyVisable();
                if (ConfigHelper.GetSysConfigValueWithoutLogin("TJPacsDownLoad") == "是")
                {
                    //panel_FilterType.Visible = true;
                    //checkEdit_autoPrint.Visible = true;
                }

                if (Barcode_TJBarcodeAutoPrint == "是")
                {
                    chkAutoPrintBarcode.Checked = true;
                }
                else
                {
                    chkAutoPrintBarcode.Checked = false;
                }
                if (ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_AutoCheckPrintReturnBarcodeForTJ") == "是")
                {
                    chkAutoPrintReturnBarcode.Checked = true;
                    this.Printer.AutoPrintReturnBarcode = true;
                }

                this.lblOutPatients.Visible = true;
                this.txtOutPatientsEnd.Visible = true;
                this.txtEmpCompanyDept.Visible = true;
                this.lblEmpCompanyName.Visible = true;
            }

            if (barcode_type == "预置条码")
            {
                lblPrePlaceBarcode.Visible = txtPrePlaceBarcode.Visible = ckPrePlaceBarcode.Visible = true;
            }
            sysToolBar1.SetToolButtonStyle(strBtnName, strBtntool);
            sysToolBar1.Visible = true;

            if (ConfigHelper.GetSysConfigValueWithoutLogin("Enable_CreateBarCodeButton") == "是")
            {
                sysToolBar1.BtnAdd.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }


            //系统配置：门诊条码界面显示[采集确认]按钮
            if ((this.Printer is OutPaitent) && ConfigHelper.GetSysConfigValueWithoutLogin("BC_MZShowbtnBloodConfirm") == "是")
            {
                //只有门诊条码才现在‘采集确认’按钮
                sysToolBar1.BtnConfirm.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            else
            {
                sysToolBar1.BtnConfirm.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }

            //是否不显示"删除条码明细"按钮
            if (ConfigHelper.GetSysConfigValueWithoutLogin("BCPrint_hide_BtnDeleteSub") == "是")
            {
                sysToolBar1.BtnDeleteSub.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }

            Printer.Printablor = this;
            Printer.IsAlone = this.IsAlone;
            Printer.Init();
            Init();


            if (Step != null && Printer != null)
            {
                Step.Printer = Printer;
                patientControl.Step = Step;
            }

            sysToolBar1.BtnResultView.Visibility = DevExpress.XtraBars.BarItemVisibility.Never; ;//默认隐藏-查询回退条码_按钮
            //住院显示回退标本消息
            if (this.PrintType == PrintType.Inpatient)
            {
                ShowReturnMessage = true;
                //住院条码打印显示[查询回退条码]按钮
                if (ConfigHelper.GetSysConfigValueWithoutLogin("IsShowReturnMsgBtn") == "是")
                {
                    sysToolBar1.BtnResultView.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//住院界面显示-查询回退条码_按钮
                }
            }

            //门诊条码打印显示回退条码信息
            if (this.PrintType == PrintType.Outpatient
                && ConfigHelper.GetSysConfigValueWithoutLogin("IsShowReturnMsg_Mz") == "是")
            {
                ShowReturnMessageMz = true;
            }

            patientControl.SetBaseInfoStyle(this.PrintType);
            if (ShowReturnMessage || ShowReturnMessageMz)
            {
                this.scrollingText1.Visible = true;
                timer1.Tick += new EventHandler(timer1_Tick);
                timer1.Start();
                timer1_Tick(timer1, EventArgs.Empty);
            }

            this.patientControl.ShowNotice(true);
            //门诊条码的下载如果没有打印的默认打勾
            if (this.PrintType == PrintType.Outpatient || PrintType == PrintType.Inpatient || printType == PrintType.TJ)
                this.patientControl.SelectWhenNotPrint = true;

            patientControl.OnResetFocus += new EventHandler(patientControl_OnResetFocus);
            //加载自定义读取类型,比如门诊下载有：门诊ID、姓名等等
            if (!IsAlone)//如果不是独立界面
            {
                LoadCustomerReadType();

                //住院显示回退标本消息
                if (this.PrintType == PrintType.Inpatient)
                {
                    lblDept.Visible = selectDict_Depart1.Visible = true;
                    lblBedNum.Visible = txtBedNum.Visible = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_SingleClientAllowInputBed") != "否";
                }
            }
            else
            {
                patientControl.IsAlone = IsAlone;
                LoadCustomerReadType();

                lblDept.Visible = selectDict_Depart1.Visible = (ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_SingleClientAllowSelectDept") == "是");
                if (this.PrintType == PrintType.Inpatient)
                {
                    lblBedNum.Visible = txtBedNum.Visible = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_SingleClientAllowInputBed") != "否";
                }
            }

            patientControl.clikcA += new PatientControlForMed.ClikeHander(patientControl_setTxtPrePlaceBarcodeFocus);

            if (Printer is OutPaitent)
            {
                string colMzSortValue = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_MzColumnVisiableIndex");
                //门诊条码打印模块将删除条码加入到角色权限控制中
                if (ConfigHelper.GetSysConfigValueWithoutLogin("Enable_MZBCPrint_DeleteBarcodeFunc") == "是" && !UserInfo.HaveFunction("FrmBCPrint_MZ_DeleteBarcode", "FrmBCPrint_MZ_DeleteBarcode"))
                {
                    sysToolBar1.BtnDelete.Enabled = false;
                }
                //门诊条码将合并条码加入到角色权限控制中
                if (!UserInfo.HaveFunction("FrmBCPrint_MZ_MergeBarcode", "FrmBCPrint_MZ_MergeBarcode"))
                {
                    sysToolBar1.BtnQualityOut.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                //门诊条码将拆分条码加入到角色权限控制中
                if (!UserInfo.HaveFunction("FrmBCPrint_MZ_SpiltBarcode", "FrmBCPrint_MZ_SpiltBarcode"))
                {
                    sysToolBar1.BtnSperateBarcode.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                patientControl.SetContextMenuStrip();

                if (!string.IsNullOrEmpty(colMzSortValue))
                {
                    patientControl.SetColumnSort(colMzSortValue);
                }
            }

            //条码下载默认状态
            cmStatus.Text = ConfigHelper.GetSysConfigValueWithoutLogin("BarcodeDownLoad_DefaultState");
            FilterDepart();

            //条码客户端合并条码权限问题  儿科和新生儿科才有合并条码功能
            if (IsAlone)
            {
                sysToolBar1.BtnQualityOut.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                if (!string.IsNullOrEmpty(selectDict_Depart1.displayMember) && selectDict_Depart1.displayMember.Contains("儿"))
                {
                    sysToolBar1.BtnQualityOut.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }
            }
        }

        void cbTypeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtOutPatients.Focus();
        }

        List<EntitySysItfContrast> readTypeDict = new List<EntitySysItfContrast>();
        bool showReturnMessage = false;
        public bool CheckPower = true;

        //回退消息
        List<EntitySampReturn> listReturn = new List<EntitySampReturn>();
        bool EnableCardReader = false;
        /// <summary>
        /// 打印状态条件过滤
        /// </summary>
        string strFilterPrint = string.Empty;

        /// <summary>
        /// 打印数据类型条件过滤（如功能科或者检验科）
        /// </summary>
        string strFilterPrintType = string.Empty;


        /// <summary>
        /// 记录转换前的下拉索引号(下载完,还原回去)
        /// </summary>
        int Note_cbTypeID_SelectedIndex = -1;

        public override void InitParamters()
        {
            this.subTable = BarcodeTable.Patient.TableName;
            this.primaryKeyOfSubTable = BarcodeTable.Patient.ID;
        }

        /// <summary>
        /// 是否独立界面
        /// </summary>
        public bool IsAlone { get; set; }

        string dep_code = null;

        /// <summary>
        /// 科室代码
        /// </summary>
        public string DeptCode
        {
            get
            {
                return dep_code;
            }
            set
            {
                dep_code = value;

                if (!string.IsNullOrEmpty(value) && this.selectDict_Depart1.Visible == true)
                {
                    string[] strDeptCode = value.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

                    if (strDeptCode.Length > 1)
                        this.selectDict_Depart1.SelectByValue(strDeptCode[0]);
                    else
                        this.selectDict_Depart1.SelectByValue(value);
                }
            }
        }

        /// <summary>
        /// 病人ID
        /// </summary>
        public string PatID { get; set; }

        /// <summary>
        /// 是否显示回退标本消息
        /// </summary>
        public bool ShowReturnMessage { get { return showReturnMessage; } set { showReturnMessage = value; } }

        /// <summary>
        /// 是否显示回退标本消息(门诊)
        /// </summary>
        public bool ShowReturnMessageMz { get; set; }

        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }

        /// <summary>
        /// 医生工号ID
        /// </summary>
        public string DoctorId { get; set; }

        /// <summary>
        /// 医生工号
        /// </summary>
        public string DoctorCode { get; set; }

        public bool IsFormatTJCode { get; set; }

        /// <summary>
        /// 读卡器驱动类型
        /// </summary>
        public string strCardReaderDriver = "";

        private bool preBarCodeCheckCuvType = false;
        bool card_ReadBankCar = false;


        /// <summary>
        /// 条码表主视图
        /// </summary>
        public GridView MainGridView
        {
            get { return patientControl.MainGridView; }
        }


        /// <summary>
        /// 科室过滤
        /// </summary>
        void FilterDepart()
        {
            if (IsAlone)
            {
                string path = PathManager.SettingPath + @"barcodeparam.ini";
                string result = "";
                if (File.Exists(path))
                    result = File.ReadAllText(path);
                string[] parm = result.Split(' ');
                //住院条码
                if (parm == null || parm.Length < 3)
                {
                    MessageDialog.Show("参数传递出错");
                    return;
                }

                if (parm[1].Trim() == "-1" && parm[2].Trim() == "-1")//住院号与部门代码不可同时为-1
                {
                    MessageDialog.Show("参数传递出错");
                    return;
                }
                if (parm[1].Trim() == "-1")
                    parm[1] = "";

                if (parm[2].Trim() == "-1")
                    parm[2] = "";

                string docID = string.Empty;

                if (parm.Length > 4)
                {
                    docID = parm[4].Trim();
                }
                if (DeptCode != parm[1] || PatID != parm[2] || DoctorName != parm[3] || docID != DoctorCode)
                {

                    this.DeptCode = parm[1].Trim();
                    this.PatID = parm[2].Trim();
                    this.DoctorName = parm[3].Trim();
                    DoctorCode = docID;
                }
                if (!string.IsNullOrEmpty(PatID))
                {
                    txtInpatientID.Text = this.PatID;
                }
                if (!string.IsNullOrEmpty(this.DeptCode))
                {
                    List<string> listDeptCode = new List<string>(DeptCode.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries));

                    if (!string.IsNullOrEmpty(DepartCode))
                        listDeptCode.Add(DepartCode);

                    List<EntityDicPubDept> listDep = CacheClient.GetCache<EntityDicPubDept>();

                    List<EntityDicPubDept> drs = listDep.FindAll(w => listDeptCode.Contains(w.DeptCode));// tb.Select(string.Format("dep_code in ({0})", deptWhere));

                    if (drs.Count > 0)
                    {
                        List<string> listWardCode = new List<string>();
                        foreach (EntityDicPubDept ward in drs)
                        {
                            if (!string.IsNullOrEmpty(ward.DeptParentId))
                            {
                                listWardCode.Add(ward.DeptParentId);
                            }
                        }

                        if (listWardCode.Count > 0)
                        {
                            selectDict_Depart1.SetFilter(selectDict_Depart1.getDataSource().FindAll(w => listWardCode.Contains(w.DeptParentId)));
                        }
                        else
                            selectDict_Depart1.SetFilter(new List<EntityDicPubDept>());
                    }

                }
                else
                {
                    return;
                }
                this.DeptCode = "";
            }
            else
            {
                if (!string.IsNullOrEmpty(this.DeptCode))
                {
                    List<string> listDeptCode = new List<string>(DeptCode.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries));

                    if (!string.IsNullOrEmpty(DepartCode))
                        listDeptCode.Add(DepartCode);

                    List<EntityDicPubDept> listDep = CacheClient.GetCache<EntityDicPubDept>();

                    List<EntityDicPubDept> drs = listDep.FindAll(w => listDeptCode.Contains(w.DeptCode));

                    if (drs.Count > 0)
                    {
                        List<string> listWardCode = new List<string>();
                        foreach (EntityDicPubDept ward in drs)
                        {
                            if (!string.IsNullOrEmpty(ward.DeptParentId))
                            {
                                listWardCode.Add(ward.DeptParentId);
                            }
                        }

                        if (listWardCode.Count > 0)
                        {
                            selectDict_Depart1.SetFilter(selectDict_Depart1.getDataSource().FindAll(w => listWardCode.Contains(w.DeptParentId)));
                        }
                        else
                            selectDict_Depart1.SetFilter(new List<EntityDicPubDept>());
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(UserInfo.userInfoId) || UserInfo.userInfoId == "-1")
                        return;


                    List<EntityDicPubDept> listDep = selectDict_Depart1.GetSource();

                    listDep = listDep.FindAll(w => UserInfo.listUserDepart.FindIndex(u => u.DeptId == w.DeptId) > -1);

                    //如果按病区下载
                    string dep_download_type = UserInfo.GetSysConfigValue("Barcode_InPatientDeptDownloadType");
                    if (dep_download_type == "按病区下载" || dep_download_type == "按病区递归下载")
                    {
                        listDep = listDep.FindAll(w => string.IsNullOrEmpty(w.DeptParentId));
                    }

                    selectDict_Depart1.SetFilter(listDep);
                }
            }
        }

        #region 读取卡数据并下载遗嘱

        void ReadCardDataAndDownLoad()
        {
            if (card_ReadBankCar && cbTypeID.Text.Contains("银行"))
            {
                LKEBankCarReadAndDown();
            }
            else if (!string.IsNullOrEmpty(strCardReaderDriver) && strCardReaderDriver == "华大读写器")
            {
                HuaDaIDReadAndDown();
            }
            else
            {
                CardReader.ICardReader reader = null;
                try
                {
                    reader = CardReader.CardReaderFactory.CreateCardReader(strCardReaderDriver);
                }
                catch(Exception ex)
                {
                    MessageDialog.ShowAutoCloseDialog(ex.Message);
                }
                if(reader != null)
                {
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
                                string interfaceKey = "住院卡号转换";
                                EntityOperationResult result = proxy.Service.CardDataConvert(data, interfaceKey);
                                if (result.HasError)
                                {
                                    MessageDialog.Show(result.Message[0].Param.ToString());
                                }
                                else if (result.OperationResultData != null
                                    && !string.IsNullOrEmpty(result.OperationResultData.ToString()))
                                {
                                    this.txtOutPatients.Text = result.OperationResultData.ToString();

                                    LoadData();
                                    ClearPatientID();
                                    if (txtPrePlaceBarcode.Visible)
                                    {
                                        txtPrePlaceBarcode.Focus();
                                    }
                                }
                                else
                                {
                                    MessageDialog.Show(string.Format("未找到卡 {0} 对应的数据", data), "卡号转换提示");
                                }
                            }
                            else
                            {
                                this.txtOutPatients.Text = data;
                                LoadData();
                                ClearPatientID();
                                if (txtPrePlaceBarcode.Visible)
                                {
                                    txtPrePlaceBarcode.Focus();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(reader.Msg))
                        {
                            MessageDialog.ShowAutoCloseDialog(reader.Msg, 2.5m);
                        }
                        else
                        {
                            MessageDialog.ShowAutoCloseDialog("无法读取卡数据，请把卡放到读卡器上！");
                        }
                    }
                }
            }

            this.txtOutPatients.Focus();
        }


        private bool isComOpen = false;

        private void LKEBankCarReadAndDown()
        {
            try
            {
                if (!isComOpen)
                {
                    OpenLKE();
                }

                if (isComOpen)
                {
                    StringBuilder secTrack = new StringBuilder(10240);
                    StringBuilder triTrack = new StringBuilder(10240);
                    int rec = BankCarReader.LKE_MSR_Read(0, secTrack, triTrack, 10);
                    if (rec != 0)
                    {
                        //MessageDialog.ShowAutoCloseDialog("读取卡数据出错：" + rec,2m);
                        Logger.WriteException(this.GetType().ToString(), "读取银行卡接口", "读取卡数据出错返回错误编码：" + rec);
                        return;
                    }
                    if (!string.IsNullOrEmpty(secTrack.ToString()) && secTrack.ToString().Contains("="))
                    {
                        this.txtOutPatients.Text = secTrack.ToString().Split('=')[0];
                        LoadData();
                        ClearPatientID();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().ToString(), "读取银行卡接口", ex.ToString());
            }


        }

        /// <summary>
        /// 读取华大诊疗卡信息
        /// </summary>
        private void HuaDaIDReadAndDown()
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
                                                this.txtOutPatients.Text = temp_strRv;
                                                LoadData();
                                                ClearPatientID();
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
                Logger.WriteException(this.GetType().ToString(), "读取华大诊疗卡接口", ex.ToString());
            }


        }

        private void OpenLKE()
        {
            string Com = ConfigHelper.ReadXml("Com",
                                                 System.AppDomain.CurrentDomain.SetupInformation
                                                       .ApplicationBase + "BankCarReader.XML");
            string Baud = ConfigHelper.ReadXml("Baud",
                                                  System.AppDomain.CurrentDomain.SetupInformation
                                                        .ApplicationBase + "BankCarReader.XML");
            int comPort = 1;
            int baudLv = 9600;
            int.TryParse(Baud, out baudLv);
            if (string.IsNullOrEmpty(Com) || !int.TryParse(Com, out comPort))
            {
                MessageDialog.ShowAutoCloseDialog("无法找到BankCarReader.XML配置文件或者配置值出错！", 2M);
                return;
            }
            int rec = BankCarReader.LKE_OpenCOM(comPort, baudLv);
            if (rec != 0)
            {
                MessageDialog.ShowAutoCloseDialog("打开端口出错：" + rec, 2M);
                return;
            }
            isComOpen = true;
        }

        void BCPrintControl_Disposed(object sender, EventArgs e)
        {
            if (isComOpen)
            {
                isComOpen = false;
                BankCarReader.LKE_CloseCOM();
            }
        }


        #endregion
        void patientControl_setTxtPrePlaceBarcodeFocus()
        {
            if (txtPrePlaceBarcode.Visible)
            {
                if (ckPrePlaceBarcode.Checked)
                    txtPrePlaceBarcode.Focus();
            }
        }

        /// <summary>
        /// 加载自定义读取类型,比如门诊下载有：门诊ID、姓名等等
        /// </summary>
        private void LoadCustomerReadType()
        {
            List<EntitySysItfInterface> listInterface = CacheClient.GetCache<EntitySysItfInterface>();
            List<EntitySysItfContrast> listContrast = CacheClient.GetCache<EntitySysItfContrast>();

            if (listInterface == null || listInterface.Count == 0 ||
                listContrast == null || listContrast.Count == 0)
            {
                return;
            }

            int interfaceIndex = listInterface.FindIndex(w => w.ItfaceInterfaceType == (Printer.Name + "条码"));

            if (interfaceIndex < 0)
                return;

            listContrast = listContrast.FindAll(w => w.ContItfaceId == listInterface[interfaceIndex].ItfaceId && w.ContSearchSeq > 0).OrderBy(o => o.ContSearchSeq).ToList();

            if (listContrast.Count > 0)
            {
                cbTypeID.Text = "";
                cbTypeID.Properties.Items.Clear();

                foreach (EntitySysItfContrast row in listContrast)
                {
                    cbTypeID.Properties.Items.Add(row.ContRemark);
                    readTypeDict.Add(row);
                }

                if (cbTypeID.Properties.Items.Count > 0)
                    cbTypeID.SelectedIndex = 0;
            }
        }

        void patientControl_OnResetFocus(object sender, EventArgs e)
        {
            ClearAndFocusBarcode();
        }


        void timer1_Tick(object sender, EventArgs e)
        {
            //获取回退条码的信息
            GetReturnBarcodeMessage();
        }

        /// <summary>
        /// 刷新回退条码信息
        /// </summary>
        public void RefreshReturnBarcodeMessage()
        {
            GetReturnBarcodeMessage();
        }

        /// <summary>
        /// 获取回退条码的信息
        /// </summary>
        private void GetReturnBarcodeMessage()
        {
            if (PrintType == PrintType.Inpatient || (printType == PrintType.Outpatient && ShowReturnMessageMz)) //住院条码的回退标本过滤科室
            {
                EntitySampQC sampQc = new EntitySampQC();

                if (IsAlone)
                    sampQc.PidDeptCode = this.DeptCode;
                else
                {
                    if (this.DeptCode != null && this.DeptCode.Trim() != string.Empty)
                        sampQc.PidDeptCode = selectDict_Depart1.valueMember;
                    else
                        sampQc.PidDeptCode = this.DeptCode;
                }
                sampQc.StartDate = adviceTime1.Value.Start.ToString(CommonValue.DateTimeFormat);
                sampQc.EndDate = adviceTime1.Value.End.ToString(CommonValue.DateTimeFormat);
                sampQc.HandleProc = ReturnProc.未处理;
                sampQc.SearchHospital = false;
                //回退条码的信息
                ProxySampReturn proxy = new ProxySampReturn();
                listReturn = proxy.Service.GetSampReturn(sampQc);
                //过滤,只显示门诊条码回退消息
                if (ShowReturnMessageMz)
                {
                    List<EntitySampReturn> messages_temp = new List<EntitySampReturn>();
                    if (listReturn != null && listReturn.Count > 0)
                    {
                        for (int j = 0; j < listReturn.Count; j++)
                        {
                            if (listReturn[j].PidSrcId != null && listReturn[j].PidSrcId != "" && listReturn[j].PidSrcId == "107")
                            {
                                messages_temp.Add(listReturn[j]);
                            }
                        }

                        listReturn = messages_temp;
                    }
                }

                if (listReturn != null && listReturn.Count > 0)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    int count = 0;
                    foreach (EntitySampReturn m in listReturn)
                    {
                        if (count > 50)
                        {
                            break;
                        }
                        stringBuilder.AppendFormat("条码：{0}    回退人：{1}     回退原因：{2}      ", m.SampBarCode, m.ReturnUserName, m.ReturnReasons);
                        count++;

                    }
                    //滚动显示回退信息
                    scrollingText1.ScrollText = stringBuilder.ToString().Replace("\r\n"," ");
                }
                else
                {
                    //滚动显示回退信息
                    scrollingText1.ScrollText = "暂无回退条码";
                }
            }
        }

        #region sysToolBarEvent


        void sysToolBar1_BtnBrowseClick(object sender, EventArgs e)
        {
            FrmBCSearch frm = new FrmBCSearch();
            frm.DeptCode = this.DeptCode;
            frm.WindowState = FormWindowState.Maximized;
            frm.Show();
        }

        private void SysToolBar1_BtnGetVersionClick(object sender, EventArgs e)
        {
            btnAdviceDownload_Click(null, null);
        }

        private void btnAdviceDownload_Click(object sender, EventArgs e)
        {
            LoadData();//下载条码
            //不等于抽血或非抽血项目时

            ClearInPatientID(); //清除住院号

            if (txtPrePlaceBarcode.Visible)
            {
                txtPrePlaceBarcode.Focus();
            }
        }

        //读卡并下载
        void BtnImport_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ReadCardDataAndDownLoad();
        }


        /// <summary>
        /// 查询回退条码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnResultViewClicked(object sender, EventArgs e)
        {
            FrmReturnMessageV2 frmV2 = new FrmReturnMessageV2();
            frmV2.IsAlone = IsAlone;
            if (IsAlone)
            {
                frmV2.DeptCode = this.DeptCode;
            }
            else
            {
                if (this.DeptCode != null && this.DeptCode.Trim() != string.Empty)
                    frmV2.DeptCode = selectDict_Depart1.valueMember;
                else
                    frmV2.DeptCode = this.DeptCode;
            }
            frmV2.ShowDialog();
        }

        /// <summary>
        /// 打印条码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnBCPrintClicked(object sender, EventArgs e)
        {
            if (patientControl.MainGridView.RowCount <= 0)
            {
                lis.client.control.MessageDialog.Show("请勾选需要打印的记录！", "提示");
                return;
            }

            bool alone = !CheckPower;
            if (alone)
            {
                this.Printer.SignInfo = new SignInfo("", DoctorName);
            }
            patientControl.PrintBarcode(this.Printer, alone);
            int focusedRowHandle = patientControl.MainGridView.FocusedRowHandle;
            if (sender != null)
            {
                patientControl.MoveNext(focusedRowHandle);
            }
            else
            {
                patientControl.MovePreBarcode();
            }
            ClearAndFocusBarcode();
        }


        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            if (this.ParentForm != null)
                this.ParentForm.Close();
        }

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
            patientControl.DeleteCname(DoctorCode, DoctorName);
            ClearAndFocusBarcode();
        }

        /// <summary>
        /// 撤消条码
        /// </summary>        
        private void sysToolBar1_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            bool ret = patientControl.DeleteBarcodeForAll(DoctorCode, DoctorName);//批量条码
        }


        /// <summary>
        /// 拆分条码
        /// </summary>
        private void sysToolBar1_BtnSperateBarcodeClick(object sender, EventArgs e)
        {
            patientControl.SeparateBarcode(ckPrePlaceBarcode.Visible && ckPrePlaceBarcode.Checked);
            ClearAndFocusBarcode();
        }

        private void sysToolBar1_OnBtnSearchClicked(object sender, EventArgs e)
        {
            LoadData(true);
            if (txtPrePlaceBarcode.Visible)
            {
                txtPrePlaceBarcode.Focus();
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


        FrmBCSignIn frmSample = null;
        //采集确认
        private void btnBCSignin_Click(object sender, EventArgs e)
        {
            //系统配置：[采集]收取[送达]默认当前登录者
            if (ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_SamplingORSendORReachUseloginID") == "是")
            {
                lis.client.control.FrmCheckPassword frmpw = new lis.client.control.FrmCheckPassword();
                if (frmpw.ShowDialog() == DialogResult.OK)
                {
                    UserInfo.loginID = frmpw.OperatorID;
                    UserInfo.userName = frmpw.OperatorName;
                }
                else
                {
                    return;
                }
            }
            if (frmSample == null || frmSample.IsDisposed)
            {
                frmSample = new FrmBCSignIn(true, false, DoctorName, DoctorCode);
                frmSample.Show();
            }
            else
                frmSample.Activate();
        }

        /// <summary>
        /// 弹出标本收取确认窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar_GetConfirmClick(object sender, EventArgs e)
        {
            //系统配置：[采集]收取[送达]默认当前登录者
            if (ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_SamplingORSendORReachUseloginID") == "是")
            {
                lis.client.control.FrmCheckPassword frmpw = new lis.client.control.FrmCheckPassword();
                if (frmpw.ShowDialog() == DialogResult.OK)
                {
                    UserInfo.loginID = frmpw.OperatorID;
                    UserInfo.userName = frmpw.OperatorName;
                }
                else
                {
                    return;
                }
            }

            FrmBCSignIn fbc = new FrmBCSignIn(false, true);
            fbc.ShowDialog();
        }

        /// <summary>
        /// 打印条码回执
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnBCPrintReturnClicked(object sender, EventArgs e)
        {
            if (this.Printer is OutPaitent)
            {
                bool alone = !CheckPower;

                if (alone)
                {
                    this.Printer.SignInfo = new SignInfo("", DoctorName);
                }
                patientControl.PrintBarcodeReturn(this.Printer);

                ClearAndFocusBarcode(true);
            }
            else if (this.Printer is TJPaitent)
            {
                patientControl.PrintBarcodeReturn(this.Printer);

                ClearAndFocusBarcode(true);
            }
        }

        private void sysToolBar1_BtnUndoClick(object sender, EventArgs e)
        {
            EntitySampMain sampMain = patientControl.CurrentSampMain;
            if (sampMain == null)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("无重置条码！");
            }
            else
            {
                string bc_bar_no = sampMain.SampBarCode;

                if (string.IsNullOrEmpty(bc_bar_no))
                {
                    MessageDialog.ShowAutoCloseDialog("条码无需重置！");
                    return;
                }
                if (sampMain.SampBarType == 0)
                {
                    MessageDialog.ShowAutoCloseDialog("非预制条码无法重置！");
                    return;
                }

                EntitySampProcessDetail proDetail = new ProxySampProcessDetail().Service.GetLastSampProcessDetail(bc_bar_no);

                string strStatus = proDetail.ProcStatus;

                //只有：打印条码、回退标本、生成条码、重置条码的状态后才能再次重置条码
                if (
                    strStatus == EnumBarcodeOperationCode.BarcodePrint.ToString()
                    || strStatus == EnumBarcodeOperationCode.SampleReturn.ToString()
                    || strStatus == EnumBarcodeOperationCode.BarcodeGenerate.ToString()
                    || strStatus == EnumBarcodeOperationCode.ResetPrePlaceBarcode.ToString()
                    )
                {
                    string strBarCode = sampMain.SampBarCode;

                    if (strBarCode.Trim() == string.Empty)
                    {
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("此条码未预置条码，无需重置。");
                        return;
                    }

                    if (lis.client.control.MessageDialog.Show("是否要重置 " + strBarCode + " 的预置条码？", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        EntitySampOperation operation = new EntitySampOperation(UserInfo.loginID, UserInfo.userName);
                        operation.OperationStatus = "570";
                        operation.OperationStatusName = "重置预置条码";
                        operation.OperationPlace = LocalSetting.Current.Setting.CType_name;
                        operation.Remark = string.Format("IP地址:{0}", IPUtility.GetIP());
                        operation.OperationTime = IStep.GetServerTime();
                        operation.OperationWorkId = UserInfo.loginID;

                        string newBarCode = new ProxySampMain().Service.UndoSampMain(operation, sampMain);
                        if (!string.IsNullOrEmpty(newBarCode))
                        {
                            patientControl.BaseSampMain.SampBarCode = string.Empty;
                            patientControl.BaseSampMain.SampBarId = newBarCode;
                            patientControl.BaseSampMain.SampPrintFlag = 0;
                            patientControl.BaseSampMain.SampStatusId = "0";
                            patientControl.MainGrid.RefreshDataSource();
                            lis.client.control.MessageDialog.ShowAutoCloseDialog("重置成功！");
                            patientControl.isResetBarcode = true;
                        }
                        else
                            lis.client.control.MessageDialog.ShowAutoCloseDialog("重置错误！");
                    }
                }
                else
                    lis.client.control.MessageDialog.Show("此试管已采集,不能重置,如需重置请回退标本！");
            }
        }

        /// <summary>
        /// 采集确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnConfirmClicked(object sender, System.EventArgs e)
        {
            patientControl.BloodConfirm();
        }

        private void sysToolBar_ManuBarcodeClick(object sender, EventArgs e)
        {
            FrmBCPrint manuprint = new FrmBCPrint(IsAlone, "手工条码", "");
            manuprint.SetMaunaText();
            manuprint.ShowDialog();
        }

        /// <summary>
        /// 合并条码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnQualityOutClicked(object sender, EventArgs e)
        {
            patientControl.MergeSampMain();
        }


        /// <summary>
        /// 生成条码按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnAddClicked(object sender, EventArgs e)
        {
            EntitySampMain dr = patientControl.CurrentSampMain;
            if (dr == null)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("无数据！");
            }
            else
            {
                int bc_bar_type = dr.SampBarType;
                string strBarId = dr.SampBarId;

                if (bc_bar_type == 1 && string.IsNullOrEmpty(dr.SampBarCode)) //判断是否预制条码
                {
                    if (lis.client.control.MessageDialog.Show("是否要对病人:" + dr.PidName + "、组合为\"" + dr.SampComName + "\"生成条码？", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        String newBarCode = new ProxySampMain().Service.CancelUndoSampMain(strBarId);
                        if (!string.IsNullOrEmpty(newBarCode))
                        {
                            patientControl.BaseSampMain.SampBarCode = newBarCode;
                            patientControl.BaseSampMain.SampBarId = strBarId;
                            patientControl.BaseSampMain.SampPrintFlag = 0;
                            patientControl.BaseSampMain.SampStatusId = "0";
                            patientControl.BaseSampMain.SampBarType = 0; //条码类型(0-打印条码 1-预制条码)
                            patientControl.MainGrid.RefreshDataSource();
                            lis.client.control.MessageDialog.ShowAutoCloseDialog("生成成功！");
                            patientControl.isResetBarcode = true;
                        }
                        else
                            lis.client.control.MessageDialog.ShowAutoCloseDialog("生成错误！");
                    }
                }
                else
                    lis.client.control.MessageDialog.Show("此条码是非预制条码，不能生成条码！");
            }
        }


        private void sysToolBar_ChangePasswordClick(object sender, EventArgs e)
        {
            FrmChangePassword frm = new FrmChangePassword(true);
            frm.ShowDialog();
        }

        //医嘱查询
        private void SysToolBar1_OnBtnQualityDataClicked(object sender, EventArgs e)
        {
            EntitySampMain samp = patientControl.MainGridView.GetFocusedRow() as EntitySampMain;
            FrmVerifOrder frm = new FrmVerifOrder(samp);
            frm.Show();
        }

        /// <summary>
        /// 单击滚动条
        /// </summary>
        private void scrollingText1_Click(object sender, EventArgs e)
        {
            if (listReturn == null || listReturn.Count == 0)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("无回退条码");
                return;
            }
            ShowReturnMessageForm();
        }

        private void txtInpatientID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                //住院下载条码
                btnAdviceDownload_Click(sender, null);
            }
        }


        #endregion



        public override void InitData()
        {
        }


        /// <summary>
        /// 清除住院号
        /// </summary>
        private void ClearInPatientID()
        {
            txtInpatientID.Text = "";
            txtInpatientID.Focus();
        }


        /// <summary>
        /// 下载条码
        /// </summary>
        private void LoadData()
        {
            LoadData(false);
        }

        /// <summary>
        /// 下载条码
        /// </summary>
        /// <param name="onlySearchDataBase">只查询数据</param>
        private void LoadData(bool onlySearchDataBase)
        {
            LoadData(onlySearchDataBase, true);
        }

        /// <summary>
        /// 下载条码
        /// </summary>
        /// <param name="onlySearchDataBase">只查询数据</param>
        private void LoadData(bool onlySearchDataBase, bool isShowMes)
        {
            try
            {
                if (adviceTime1 != null && (adviceTime1.Value.End.Date - adviceTime1.Value.Start.Date).TotalDays >= 15)
                {
                    if (Printer is OutPaitent)
                    {
                        if (string.IsNullOrEmpty(txtOutPatients.Text))
                        {
                            lis.client.control.MessageDialog.ShowAutoCloseDialog("超过半个月的查询必须输入条件");
                            txtOutPatients.Focus();
                            return;
                        }
                    }

                    if (Printer is Inpatient)
                    {
                        if (string.IsNullOrEmpty(txtInpatientID.Text) &&
                            string.IsNullOrEmpty(selectDict_Depart1.valueMember) &&
                            string.IsNullOrEmpty(txtBedNum.Text))
                        {
                            lis.client.control.MessageDialog.ShowAutoCloseDialog("超过半个月的查询必须输入条件");
                            txtInpatientID.Focus();
                            return;
                        }
                    }

                    if (Printer is TJPaitent)
                    {
                        if (string.IsNullOrEmpty(txtOutPatients.Text) &&
                            string.IsNullOrEmpty(txtOutPatientsEnd.Text) &&
                            string.IsNullOrEmpty(txtEmpCompanyDept.Text))
                        {
                            lis.client.control.MessageDialog.ShowAutoCloseDialog("超过半个月的查询必须输入条件");
                            txtOutPatients.Focus();
                            return;
                        }
                    }
                }

                //显示进度条
                splashScreenManager1.ShowWaitForm();
                splashScreenManager1.SetWaitFormCaption("请等待");
                splashScreenManager1.SetWaitFormDescription("正在加载中。。。");

                //下载条码的条件
                string msg = "";
                if (onlySearchDataBase == false)//从接口下载条码
                    msg = DownLoadFromInterface();

                #region 检索数据

                EntitySampQC sampQC = new EntitySampQC();

                //先以医嘱时间范围组合查询Sql
                sampQC.StartDate = adviceTime1.Value.Start.Date.ToString(CommonValue.DateTimeFormat);
                sampQC.EndDate = adviceTime1.Value.End.Date.AddDays(1).AddMilliseconds(-1).ToString(CommonValue.DateTimeFormat);

                //门诊
                if (printType == PrintType.Outpatient)
                {
                    sampQC.PidSrcId = "107";

                    string serachValue = txtOutPatients.Text.Trim();

                    if (serachValue != string.Empty && cbTypeID.EditValue!=null)
                    {
                        if(cbTypeID.EditValue.ToString()=="门诊号")
                            sampQC.PidInNo = serachValue;
                        else if(cbTypeID.EditValue.ToString() == "诊疗卡号")
                            sampQC.PidSocialNo = serachValue;
                        else if (cbTypeID.EditValue.ToString() == "姓名")
                            sampQC.PidName = serachValue;
                        else
                            sampQC.PidInNo = serachValue;
                    }
                }

                //住院
                if (printType == PrintType.Inpatient)
                {
                    sampQC.PidSrcId = "108";

                    string strDepCode = selectDict_Depart1.valueMember;

                    //科室
                    if (string.IsNullOrEmpty(strDepCode))
                    {
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("请选择下载科室");
                        return;
                    }

                    sampQC.PidDeptCode = strDepCode;

                    string patientid = txtInpatientID.Text.Trim();

                    if (patientid != string.Empty)
                    {
                        //位数补零
                        string patInNoAutoAddZeroNum = ConfigHelper.GetSysConfigValueWithoutLogin("LabQuery_PatInNoAutoAddZeroNum");

                        int addZeroNum = 0;
                        int.TryParse(patInNoAutoAddZeroNum, out addZeroNum);

                        if (addZeroNum > 0)
                        {
                            patientid = patientid.PadLeft(addZeroNum, '0');
                        }

                        sampQC.PidInNo = patientid;
                    }

                    //床号
                    if (txtBedNum.Text.Trim() != string.Empty)
                        sampQC.PidBedNo = txtBedNum.Text.Trim();
                }

                //体检
                if (printType == PrintType.TJ)
                {
                    sampQC.PidSrcId = "109";

                    string serachValue = txtOutPatients.Text.Trim();

                    if (serachValue != string.Empty)
                    {
                        if (cbTypeID.SelectedIndex == 0)
                        {
                            sampQC.PidInNo = serachValue;
                            if (txtOutPatientsEnd.Text.Trim() != string.Empty)
                                sampQC.PidInNoEnd = txtOutPatientsEnd.Text.Trim();
                        }
                        else if (cbTypeID.SelectedIndex == 1)
                            sampQC.PidSocialNo = serachValue;
                        else if (cbTypeID.SelectedIndex == 2)
                            sampQC.PidName = serachValue;
                    }
                }

                sampQC.SearchDeleteSampMain = false;

                //下载完条码后，再从检验数据库查询出来打印
                DoLoadData(sampQC);

                #endregion

                if (this.Printer is TJPaitent)
                {
                    txtEmpCompanyDept.Properties.Items.Clear();
                    List<EntitySampMain> tbData = patientControl.ListSampMain;
                    if (tbData != null && tbData.Count > 0)
                    {
                        txtEmpCompanyDept.Properties.Items.Add(string.Empty);
                        foreach (EntitySampMain row in tbData)
                        {
                            if (!txtEmpCompanyDept.Properties.Items.Contains(row.PidExamCompanyDept))
                            {
                                txtEmpCompanyDept.Properties.Items.Add(row.PidExamCompanyDept);
                            }
                        }
                    }
                }
                bool isSelect = true;
                if (!patientControl.HasData())
                {
                    if (isShowMes)
                    {
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("该时间段无医嘱!");
                        return;
                    }
                }
                else
                {
                    ListSampMain = EntityManager<EntitySampMain>.ListClone(patientControl.ListSampMain);

                    if (ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_DifferentOCCDate") == "是")
                    {
                        List<string> listSampBarId = new List<string>();
                        foreach (EntitySampMain item in ListSampMain)
                        {
                            if (item.SampStatusId == "0" && item.PidSrcId == "107")
                                listSampBarId.Add(item.SampBarId);
                        }
                        if (listSampBarId.Count > 0)
                        {
                            ProxySampDetail proxyDetail = new ProxySampDetail();
                            if (proxyDetail.Service.ExistDifferentOCCDate(listSampBarId))
                            {
                                MessageDialog.Show("本次下载的条码中存在不同执行日期的医嘱，请注意！");
                                //存在不同执行日期的医嘱 本次只做一天的 取消非预制条码的勾选  让护士自行勾选
                                isSelect = false;
                            }
                        }

                        var sums = ListSampMain.FindAll(f => f.SampStatusId == "0" && f.PidSrcId == "107")
                                               .GroupBy(w => new { PidName = w.PidName, SampComName = w.SampComName })
                                               .Select(group => new { Patient = group.Key, Count = group.Count() });

                        foreach (var item in sums)
                        {
                            if (item.Count > 1)
                            {
                                MessageDialog.Show(string.Format("本次下载的条码中存在相同项目医嘱[{0}]，请注意！", item.Patient.SampComName));
                                break;
                            }
                        }
                    }

                    splashScreenManager1.SetWaitFormDescription("数据过滤。。。");

                    FilterMain();

                    //勾选未打印 
                    if (cmStatus.Text.ToString() == "未打印" && printType != PrintType.Inpatient && isSelect)
                    {
                        patientControl.SelectAllNoPrintRow();
                    }
                    else
                    {
                        patientControl.ClearChecked();
                    }
                }

                if (txtPrePlaceBarcode.Visible)
                {
                    txtPrePlaceBarcode.Focus();
                }
                //如果不是只查询数据库，则进入是否自动打印事件
                if (!onlySearchDataBase && patientControl.HasData())
                {
                    if (chkAutoPrintBarcode.Checked)
                    {
                        this.sysToolBar1_OnBtnBCPrintClicked(null, null);
                    }
                }
            }
            finally
            {
                try
                {
                    splashScreenManager1.CloseWaitForm();
                }
                catch
                { }
            }
        }

        /// <summary>
        /// 从接口下载条码
        /// </summary>
        private string DownLoadFromInterface()
        {
            EntityInterfaceExtParameter downLoadInfo = new EntityInterfaceExtParameter();

            if (DoctorName != null && DoctorName != string.Empty)
                downLoadInfo.OperationName = DoctorName;
            else
            {
                if (UserInfo.userName != null && UserInfo.userName != string.Empty)
                    downLoadInfo.OperationName = UserInfo.userName;
                else
                    downLoadInfo.OperationName = string.Empty;
            }

            if (PrintType == PrintType.Inpatient)
                downLoadInfo.DownloadType = InterfaceType.ZYDownload;
            else if (PrintType == PrintType.Outpatient)
                downLoadInfo.DownloadType = InterfaceType.MZDownload;
            else if (PrintType == PrintType.TJ)
                downLoadInfo.DownloadType = InterfaceType.TJDownload;


            if (PrintType == PrintType.Inpatient)//|| PrintType == PrintType.TJ)// 2010-8-25 //如果是住院与体检条码都用住院号的文本框,自定义条码查询字段后这段代码将不起作用
            {
                if (readTypeDict != null &&
                    readTypeDict.Count > 0 &&
                    !string.IsNullOrEmpty(readTypeDict[cbTypeID.SelectedIndex].ContScriptForSearch) &&
                    cbTypeID.Properties.Items.Count >= readTypeDict.Count)
                {
                    IFormater formater = new ScriptFormater(readTypeDict[cbTypeID.SelectedIndex].ContScriptForSearch);//根据C#脚本格式化号码
                    txtInpatientID.Text = formater.FormatPatientID(txtInpatientID.Text);
                }

                downLoadInfo.PatientID = txtInpatientID.Text;
            }
            //如果有自定义查询列
            else if (readTypeDict != null && readTypeDict.Count > 0 && cbTypeID.Properties.Items.Count >= readTypeDict.Count)
            {
                if (!string.IsNullOrEmpty(readTypeDict[cbTypeID.SelectedIndex].ContScriptForSearch))
                {
                    IFormater formater = new ScriptFormater(readTypeDict[cbTypeID.SelectedIndex].ContScriptForSearch);//根据C#脚本格式化号码
                    downLoadInfo.PatientID = formater.FormatPatientID(txtOutPatients.Text);
                    if (PrintType == PrintType.TJ && !string.IsNullOrEmpty(txtOutPatientsEnd.Text))
                    {
                        downLoadInfo.InvoiceID = formater.FormatPatientID(txtOutPatientsEnd.Text);
                    }
                }
                else
                {
                    downLoadInfo.PatientID = txtOutPatients.Text;
                    if (PrintType == PrintType.TJ && !string.IsNullOrEmpty(txtOutPatientsEnd.Text))
                    {
                        downLoadInfo.InvoiceID = txtOutPatientsEnd.Text;
                    }
                }

                //自定义查询列，从院网接口处读取
                if (!string.IsNullOrEmpty(readTypeDict[cbTypeID.SelectedIndex].ContSysColumn))
                {
                    string columnName = readTypeDict[cbTypeID.SelectedIndex].ContSysColumn;
                    downLoadInfo.LisSearchColumn = columnName;
                }
            }
            else//门诊条码的参数
            {
                if (cbTypeID.SelectedIndex == 0)//病人ID
                {
                    downLoadInfo.PatientID = txtOutPatients.Text;
                }
                else if (cbTypeID.SelectedIndex == 1)//发票号
                {
                    downLoadInfo.InvoiceID = txtOutPatients.Text;
                }
                else if (cbTypeID.SelectedIndex == 2)//病人姓名
                {
                    downLoadInfo.PatientName = txtOutPatients.Text;
                }
            }


            downLoadInfo.StartTime = adviceTime1.Value.Start; //医嘱开始与结束时间
            downLoadInfo.EndTime = adviceTime1.Value.End;

            if (IsAlone)
            {
                if (this.selectDict_Depart1.Visible && !string.IsNullOrEmpty(this.selectDict_Depart1.valueMember))
                {
                    downLoadInfo.DeptID = this.selectDict_Depart1.valueMember;
                }
                else
                {
                    downLoadInfo.DeptID = this.DeptCode;//科室代码
                }
            }
            else
                downLoadInfo.DeptID = selectDict_Depart1.valueMember;

            DataSet result = null;

            if (!(IsNormal() || this.Printer is TJPaitent))//outlink接口从客户端取数据
            {
                downLoadInfo.OutlinkInterface = true;
                downLoadInfo.OutlinkData = Printer.DownloadHisOrder(downLoadInfo);
            }

            downLoadInfo.MzFiterSams = LocalSetting.Current.Setting.MzDefaultSam;
            try
            {
                ProxySampMainDownload download = new ProxySampMainDownload();
                download.Service.DownloadBarcode(downLoadInfo);
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "下载条码", ex.ToString() + ex.StackTrace);
            }

            string msg = "";
            if (result == null || result.Tables.Count <= 0 || result.Tables[0].Rows.Count == 0)
            {
                msg = "该时间段无新医嘱!";
            }
            return msg;
        }

        /// <summary>
        /// 是否为通用的接口类型
        /// </summary>
        /// <returns></returns>
        private static bool IsNormal()
        {
            return BarcodeClientHelper.IsNormal();
        }


        /// <summary>
        /// 打印清单
        /// </summary>
        private void PrintList()
        {
            patientControl.PrintList();
        }

        /// <summary>
        /// 下载条码
        /// </summary>
        /// <param name="sql"></param>
        private void DoLoadData(EntitySampQC sampQC)
        {
            try
            {
                patientControl.LoadData(sampQC);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// 初始化医嘱时间
        /// </summary>
        private void Init()
        {
            adviceTime1.Value = Printer.GetDefaultAdviceTime();
        }


        /// <summary>
        /// 过滤
        /// </summary>
        private void Filter()
        {
            if (patientControl.HasData())
            {
                patientControl.UpdateCount();
                //重新指定当前条码
                patientControl.RefreshCurrentBarcodeInfo();
            }
        }


        #region IStepable 成员

        public IStep Step
        {
            get;
            set;
        }
        private StepType stepType = StepType.Print;


        public StepType StepType
        {
            get { return stepType; }
            set
            {
                stepType = value;
                Step = StepFactory.CreateStep(value);
                patientControl.StepType = this.StepType;
            }
        }

        #endregion

        #region IPrintable 成员

        public IPrint Printer { get; set; }

        private PrintType printType = PrintType.Inpatient;
        public PrintType PrintType
        {
            get { return printType; }
            set
            {
                printType = value;
                Printer = PrintFactory.Create(this.PrintType);
            }
        }

        #endregion

        /// <summary>
        /// 门诊下载医嘱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                bool hasInput = NotInputOutpatients();
                if (this.EnableCardReader && hasInput)
                {
                    ReadCardDataAndDownLoad();

                    if (Note_cbTypeID_SelectedIndex != -1)
                    {
                        this.cbTypeID.SelectedIndex = Note_cbTypeID_SelectedIndex;
                        Note_cbTypeID_SelectedIndex = -1;
                    }
                }
                else
                {
                    if (hasInput)
                    {
                        if (PrintType == PrintType.TJ)
                            lis.client.control.MessageDialog.Show("请输入体检资料！", "提示");
                        else
                            lis.client.control.MessageDialog.Show("请输入门诊资料！", "提示");
                        return;
                    }

                    LoadData();
                    ClearPatientID();

                    if (Note_cbTypeID_SelectedIndex != -1)
                    {
                        this.cbTypeID.SelectedIndex = Note_cbTypeID_SelectedIndex;
                        Note_cbTypeID_SelectedIndex = -1;
                    }

                    //默认选择参数配置。
                    string strCbTypeSelectIndex = ConfigHelper.GetSysConfigValueWithoutLogin("OpBarcodeTypeSelect");
                    if (!string.IsNullOrEmpty(strCbTypeSelectIndex) && strCbTypeSelectIndex != "无")
                    {
                        this.cbTypeID.SelectedIndex = Convert.ToInt16(strCbTypeSelectIndex);
                    }
                }
            }
        }

        /// <summary>
        /// 重置医嘱时间
        /// </summary>
        private void ResetTime()
        {
            adviceTime1.Value = Printer.GetDefaultAdviceTime();
        }

        /// <summary>
        /// 门诊号没输入
        /// </summary>
        /// <returns></returns>
        private bool NotInputOutpatients()
        {
            return string.IsNullOrEmpty(txtOutPatients.Text);
        }

        /// <summary>
        /// 清空病人ID
        /// </summary>
        private void ClearPatientID()
        {
            txtOutPatients.Text = "";
        }

        #region IPrintable 成员

        /// <summary>
        /// 门诊初始化
        /// </summary>
        public void OutpatientInit()
        {
            panelOutpatient.Visible = true;
        }

        /// <summary>
        /// 住院初始化
        /// </summary>
        public void InpatientInit()
        {
            lblID.Text = this.Printer.Name + "号:";
            if (this.Printer is TJPaitent)
                lblID.Text = "会员卡号:";
            panelInpatient.Visible = true;
        }

        #endregion

       
        private void FilterMain()
        {
            if (cmStatus.Text == "") return;

            if (ListSampMain.Count > 0)
            {
                FilterOnly();
                patientControl.UpdateCount();
                //重新指定当前条码
                patientControl.RefreshCurrentBarcodeInfo();

            }
        }


        /// <summary>
        /// 清除条码输入框并设置焦点
        /// </summary>
        private void ClearAndFocusBarcode(bool printReturn = false)
        {
            int focusedRowHandle = patientControl.MainGridView.FocusedRowHandle;
            EntitySampMain samp = patientControl.MainGridView.GetFocusedRow() as EntitySampMain;
            if (this.PrintType == PrintType.Inpatient)
            {
                txtInpatientID.Text = "";
                txtInpatientID.Focus();
            }
            //门诊和体检打印完最后一个条码 光标置为卡号输入框
            if (focusedRowHandle == patientControl.MainGridView.DataRowCount - 1 && samp.SampPrintFlag == 1)
            {
                if (PrintType == PrintType.Outpatient || PrintType == PrintType.TJ)
                {
                    txtOutPatients.Text = "";
                    txtPrePlaceBarcode.Text = string.Empty;
                    txtOutPatients.Focus();
                }
            }
            else if (samp.SampBarType == 1)
            {
                txtPrePlaceBarcode.Text = string.Empty;
                txtPrePlaceBarcode.Focus();
            }
            //门诊和体检只要点了打印回执  则将光标跳转到病人id输入框 （中山三院）
            if (printReturn && this.PrintType != PrintType.Inpatient)
            {
                txtOutPatients.Text = "";
                txtPrePlaceBarcode.Text = string.Empty;
                txtOutPatients.Focus();
            }

    
        }

        private List<EntitySampMain> ListSampMain = new List<EntitySampMain>();

        private void FilterOnly()
        {
            List<EntitySampMain> listFilter = new List<EntitySampMain>();
            if (cmStatus.Text.ToString() == "全部")
                listFilter = ListSampMain.ToList();
            else if (cmStatus.Text.ToString() == "未打印")
                listFilter = ListSampMain.Where(w => w.SampStatusId == "0" || w.SampStatusId == "9").ToList();
            else if (cmStatus.Text.ToString() == "未采集")
                listFilter = ListSampMain.Where(w => w.SampStatusId == "1").ToList();
            else if (cmStatus.Text.ToString() == "已打印")
                listFilter = ListSampMain.Where(w => w.SampStatusId == "1" || w.SampStatusId == "2").ToList();
            else if (cmStatus.Text.ToString() == "已收取")
                listFilter = ListSampMain.Where(w => w.SampStatusId == "3").ToList();
            else if (cmStatus.Text.ToString() == "已送达")
                listFilter = ListSampMain.Where(w => w.SampStatusId == "4").ToList();
            else if (cmStatus.Text.ToString() == "已签收")
                listFilter = ListSampMain.Where(w => w.SampStatusId == "5").ToList();
            else if (cmStatus.Text.ToString() == "已报告")
                listFilter = ListSampMain.Where(w => w.SampStatusId == "60").ToList();
            else if (cmStatus.Text.ToString() == "已采集")
                listFilter = ListSampMain.Where(w => w.SampBloodFlag).ToList();

            if (cmBloodStatus.Text.ToString() == "抽血项目")
                listFilter = listFilter.Where(w => w.SampSamName.Contains("血")).ToList();
            else if (cmBloodStatus.Text.ToString() == "非抽血项目")
                listFilter = listFilter.Where(w => !w.SampSamName.Contains("血")).ToList();
            // 门诊体检且有预置条码 非预置条码排在前面
            if ((this.Printer is OutPaitent || this.Printer is TJPaitent) && txtPrePlaceBarcode.Visible == true)
            {
                listFilter = listFilter.OrderBy(w => w.SampBarType).ToList();
            }
            patientControl.BindingSampMain(listFilter);
        }

        private void Filter_CheckedChanged(object sender, EventArgs e)
        {
            FilterMain();
        }

        /// <summary>
        /// 下载条码
        /// </summary>
        /// <param name="patID"></param>
        internal void DownloadAdvice(string patID)
        {

            this.PatID = txtInpatientID.Text = patID;
            btnAdviceDownload_Click(null, null);
        }


        internal void DownloadTjAdvice(string tjId)
        {
            string[] id = tjId.Split('&');

            if (id.Length > 0)
            {
                for (int i = 0; i < id.Length; i++)
                {
                    if (id[i] != string.Empty && id[i] != "-1")
                    {
                        if (IsFormatTJCode)
                            txtOutPatients.Text = id[i].Substring(0, id[i].Length - 8);
                        else
                            txtOutPatients.Text = id[i];
                        LoadData(false, false);
                        ClearPatientID();
                        ResetTime();
                    }
                }
            }

        }

        /// <summary>
        /// 显示回退条码信息窗口
        /// </summary>
        private void ShowReturnMessageForm()
        {
            FrmReturnMessageV2 frm = new FrmReturnMessageV2();
            GetReturnBarcodeMessage();
            frm.ReturnMessages = listReturn;
            frm.IsScrollingText = true;
            frm.ShowDialog();

            if (frm.NotData())
                return;
            if (frm.NeedHandle)
            {
                EntitySampReturn message = frm.GetCurrentReturnMessages();
                patientControl.AddBarcode(message.SampBarId);
                patientControl.SelectRow(message.SampBarId, true);
            }
        }

        public EntitySampMain GetFocusDataRow()
        {
            return patientControl.CurrentSampMain;
        }

        private void txtPrePlaceBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter)
                return;

            if (string.IsNullOrEmpty(txtPrePlaceBarcode.Text.Trim()))
                return;

            bool result = patientControl.PrintPrePlaceBarcode(txtPrePlaceBarcode.Text.Trim(), this.Printer);
            if (result)
            {
                int focusedRowHandle = patientControl.MainGridView.FocusedRowHandle;
                patientControl.MoveNext(focusedRowHandle);
            }
            if (cmStatus.Text != "全部")
                Filter();
            txtPrePlaceBarcode.Text = string.Empty;
            txtPrePlaceBarcode.Focus();
            ClearAndFocusBarcode();
        }

        private void ckPrePlaceBarcode_CheckedChanged(object sender, EventArgs e)
        {
            txtPrePlaceBarcode.Properties.ReadOnly = !ckPrePlaceBarcode.Checked;
        }

        private void txtEmpCompanyDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            string empdept = this.txtEmpCompanyDept.Text;
            if (string.IsNullOrEmpty(empdept))
            {
                //patientControl.PatientBindingSource.Filter = string.Empty;
            }
            else
            {
                //patientControl.PatientBindingSource.Filter = string.Format("bc_emp_company_dept = '{0}'", empdept);
            }
        }

        private void chkAutoPrintReturnBarcode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoPrintReturnBarcode.Checked)
            {
                this.Printer.AutoPrintReturnBarcode = true;
            }
            else
            {
                this.Printer.AutoPrintReturnBarcode = false;
            }
        }

        /// <summary>
        /// 提示有条码回退信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            MessageBox.Show("有回退条码信息,请处理", "回退信息提醒", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ShowReturnMessageForm();
        }

        private void cmStatus2_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterMain();
        }

        public new bool DesignMode
        {
            get { return System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToLower() == "devenv"; }
        }

    }

    /// <summary>
    /// ID格式化
    /// </summary>
    public abstract class IFormater
    {
        public virtual string FormatPatientID(string p)
        {
            return p;
        }
    }


    public class TodayFormater : IFormater
    {

        public override string FormatPatientID(string p)
        {
            if (p.Length > 0 && p.Length < 6)
            {
                return DateTime.Now.ToString("yyMMdd") + p;
            }
            else
                return p;
        }

    }

    /// <summary>
    /// 脚本格式化器
    /// </summary>
    public class ScriptFormater : IFormater
    {
        public string Script { get; set; }
        public ScriptFormater(string script)
        {
            this.Script = script;
        }

        public override string FormatPatientID(string input)
        {
            if (string.IsNullOrEmpty(this.Script) || this.Script.Trim() == "")
                return input;

            CodeDomProvider provider = new Microsoft.CSharp.CSharpCodeProvider();
            string script = "    public class class1    {       " + this.Script + "    }";
            CompilerResults results = provider.CompileAssemblyFromSource(new CompilerParameters(), script);
            if (results.Errors.HasErrors)
            {
                Logger.WriteInfo("lis.client.barcode", "条码下载号码格式化出错", results.Errors[0].ToString());
                return input;
            }
            else
            {
                Assembly assembly = results.CompiledAssembly;
                Type targetType = assembly.GetType("class1");
                object targetObject = Activator.CreateInstance(targetType);
                MethodInfo[] methods = targetType.GetMethods(BindingFlags.Public | BindingFlags.Static);
                MethodInfo method = null;
                if (methods != null && methods.Length == 1)
                {
                    method = methods[0];
                    object ret = method.Invoke(null, new object[] { input });
                    if (ret == null)
                        return input;

                    return ret.ToString();
                }
                else
                {
                    return input;
                }

            }
        }
    }

    /// <summary>
    /// 公司内部系统格式化器
    /// </summary>
    public class CompanyFormater : IFormater
    {
        public override string FormatPatientID(string patientID)
        {
            if (Extensions.IsEmpty(patientID))
                return "";
            string p = string.Format("000000000{0}00", patientID);
            p = p.Substring(p.Length - 12, 12);
            return p;
        }
    }

    /// <summary>
    /// 条码客户端助手
    /// </summary>
    public class BarcodeClientHelper
    {
        /// <summary>
        /// 接口类型是否为通用
        /// </summary>
        public static bool IsNormal()
        {
            return ConfigHelper.GetSysConfigValueWithoutLogin("GetPatientsInfoType") == "通用";
        }
    }
}