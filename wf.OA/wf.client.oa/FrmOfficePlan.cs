using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using System.Collections;
using dcl.client.report;
using dcl.client.wcf;
using System.Reflection;
using dcl.entity;
using System.Linq;

namespace dcl.client.oa
{
    public partial class FrmOfficePlan : FrmCommon
    {

        #region 全局变量
        /// <summary>
        /// 字段命名分隔符
        /// </summary>
        private char strSplit = '┏';
        /// <summary>
        /// 第一次加载窗体
        /// </summary>
        bool firstLoad = true;
        /// <summary>
        /// 从合成表的多少列开始分离数据
        /// </summary>
        private int intStart = 0;

        /// <summary>
        /// 班次临时表
        /// </summary>
        DataTable dtBC = new DataTable();
        /// <summary>
        /// 班次临时表 实体
        /// </summary>
        List<EntityOaDicShift> listBC = new List<EntityOaDicShift>();
        /// <summary>
        /// 排班临时表
        /// </summary>
        List<EntityOaDicShiftDetail> listPersonPlan = new List<EntityOaDicShiftDetail>();
        /// <summary>
        /// 人员临时表
        /// </summary>
        List<EntitySysUser> UserList = new List<EntitySysUser>();
        /// <summary>
        /// 排班人员临时表
        /// </summary>
        DataTable dtUserTemp = new DataTable();

        /// <summary>
        /// 模板临时表
        /// </summary>
        List<EntityOaShiftTemplate> listTemp = new List<EntityOaShiftTemplate>();

        /// <summary>
        /// 保存这个科室班次的信息(key为名字+起始日期,value为ID）
        /// </summary>
        Dictionary<string, string> dtnPlanName_id = new Dictionary<string, string>();
        /// <summary>
        /// 保存这个科室班次的信息(Key为ID,value为名字+起始日期）
        /// </summary>
        Dictionary<string, string> dtnPlanId_name = new Dictionary<string, string>();

        Dictionary<string, string> dtnPlanAll = new Dictionary<string, string>();

        //星期
        string[] dayOfWeek = new string[] { "日", "一", "二", "三", "四", "五", "六" };

        string dtemplate_id = string.Empty;
        DateTime date = DateTime.Now;
        bool isTemplate = false;
        int startCol = 0;

        #endregion

        public FrmOfficePlan()
        {
            InitializeComponent();
        }
        List<EntitySysUser> listUser = new List<EntitySysUser>();
        #region 窗体载入事件

        /// <summary>
        /// 窗体载入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmOfficePlan_Load(object sender, EventArgs e)
        {
            firstLoad = false;
            sysToolBar1.SetToolButtonStyle(new string[] {
                sysToolBar1.BtnModify.Name,
                sysToolBar1.BtnSave.Name,
                sysToolBar1.BtnCancel.Name,
                sysToolBar1.BtnExport.Name,
                sysToolBar1.BtnSinglePrint.Name });

            sysToolBar1.BtnSinglePrint.Caption = "个人排班表";


            this.sysToolBar1.OnBtnModifyClicked += new System.EventHandler(this.sysToolBar_OnBtnModifyClicked);
            this.sysToolBar1.OnBtnSaveClicked += new System.EventHandler(this.sysToolBar_OnBtnSaveClicked);
            this.sysToolBar1.OnBtnCancelClicked += new System.EventHandler(this.sysToolBar_OnBtnCancelClicked);
            this.sysToolBar1.OnBtnExportClicked += new System.EventHandler(this.sysToolBar_OnBtnExportClicked);
            this.sysToolBar1.OnBtnSinglePrintClicked += new System.EventHandler(this.sysToolBar_OnBtnSinglePrintClicked);
            this.sysToolBar1.BtnCopyClick += new System.EventHandler(this.sysToolBar_BtnCopyClick);

            this.bandedGridView.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.bandedGridView_RowStyle);
            this.bandedGridView.CustomDrawEmptyForeground += new DevExpress.XtraGrid.Views.Base.CustomDrawEventHandler(this.bandedGridView_CustomDrawEmptyForeground);
            this.bandedGridView.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.bandedGridView_FocusedRowChanged);
            this.bandedGridView.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.bandedGridView_FocusedColumnChanged);

            this.popEdit.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.popEdit_ButtonClick);

            //默认时间段为本周
            timeFrom.EditValue = GetWeekFirstDayMon(DateTime.Now);
            timeTo.EditValue = GetWeekFirstDayMon(DateTime.Now).AddDays(6);

            List<EntityDicPubProfession> listType = cboDictType.dtSource;
            EntityDicPubProfession profession = new EntityDicPubProfession();
            profession.ProId = "-1";
            profession.ProName = "全科";
            profession.ProPyCode = "qk";
            profession.ProSortNo = -1;
            profession.ProType = 1;

            listType.Add(profession);

            if (UserInfo.defaultType != "")
            {
                cboDictType.valueMember = UserInfo.defaultType;
                cboDictType.displayMember = UserInfo.defaultTypeName;
            }

            SetCanEdit(UserInfo.HaveFunctionByCode("fun_office_Plan_Edit"), true);

            if (UserInfo.GetSysConfigValue("Office_dutyTemplateSelect") == "是")
            {
                panel3.Visible = false;
                panel4.Visible = false;
                sysToolBar1.BtnSinglePrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            }
            this.label1.Text = "";
        }

        #endregion
        public void EnableButton(bool enable)
        {
            sysToolBar1.BtnModify.Enabled = !enable;
            sysToolBar1.BtnCancel.Enabled = !enable;
            sysToolBar1.BtnSave.Enabled = enable;
        }
        #region sysToolBar1状态
        /// <summary>
        /// sysToolBar1状态
        /// </summary>
        /// <param name="canEdit"></param>
        /// <param name="canExport"></param>
        public void SetCanEdit(bool canEdit, bool canExport)
        {
            sysToolBar1.BtnModify.Visibility = canEdit ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
            sysToolBar1.BtnCopy.Visibility = canEdit ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
            sysToolBar1.BtnCancel.Visibility = canEdit ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
            sysToolBar1.BtnSave.Visibility = canEdit ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
            sysToolBar1.BtnExport.Visibility = canExport ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;

        }

        #endregion

        #region 选择物理组

        /// <summary>
        /// 选择物理组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void selectDict_Type1_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            #region
            if (cboDictType.valueMember == null && !firstLoad)
            {
                lis.client.control.MessageDialog.Show("请选择物理组别！！");
            }
            else if (cboDictType.valueMember == "")
            {
                lis.client.control.MessageDialog.Show("请选择物理组别！！");
            }
            else
            {
                string strType = cboDictType.valueMember;
                if (UserInfo.GetSysConfigValue("Office_dutyTemplateSelect") != "是")
                {
                    LoadDutyInfo(strType);
                }
                else
                {
                    ProxyOaShiftDict proxy = new ProxyOaShiftDict();
                    listTemp = proxy.Service.GetTemplateData();
                    foreach (EntityOaShiftTemplate entity in listTemp)
                    {
                        string[] strSpilt = entity.TempType.Split(',');
                        foreach (string s in strSpilt)
                        {
                            if (s == strType)
                            {
                                dtemplate_id = entity.TempId;
                                isTemplate = true;
                                break;
                            }
                            else
                            {
                                dtemplate_id = "";
                                isTemplate = false;
                            }
                        }
                        if (isTemplate)
                        {
                            break;
                        }
                    }
                    if (!isTemplate)
                    {
                        lis.client.control.MessageDialog.Show("所选择物理组别没有排班模板！！");
                        this.bandedGridView.Bands.Clear();
                        this.bandedGridView.Columns.Clear();
                        this.label1.Text = string.Empty;
                        return;
                    }
                    if (dtemplate_id == "10002")
                    {
                        startCol = 9;
                    }
                    else
                    {
                        startCol = 8;
                    }
                    LoadDutyTemplateInfo(strType, dtemplate_id, DateTime.Now);
                }
            }
            #endregion

        }

        #endregion

        #region 自动生成列
        private void LoadDutyInfo(string p)
        {
            try
            {
                ProxyOaShiftDict proxy = new ProxyOaShiftDict();

                sysToolBar1.EnableButton(false);
                sysToolBar1.BtnSinglePrint.Enabled = rdoName.Checked | rdoDate.Checked;// rdoName.Checked;
                bandedGridView.Bands.Clear();
                bandedGridView.Columns.Clear();

                #region 读取人员资料

                listUser = proxy.Service.GetUser(p);
                bindingSourceUser.DataSource = listUser;
                DataTable dtUser = new DataTable();
                dtUser.Columns.Add("ID");
                dtUser.Columns.Add("Name");
                dtUser.Columns.Add("loginId");
                dtUser.Columns.Add("userType");
                dtUser.Columns.Add("password");
                dtUser.Columns.Add("wb");
                dtUser.Columns.Add("py");
                dtUser.Columns.Add("typeSourceId");
                foreach (EntitySysUser user in listUser)
                {
                    DataRow drUser = dtUser.NewRow();
                    drUser["ID"] = user.UserId;
                    drUser["Name"] = user.UserName;
                    drUser["loginId"] = user.UserLoginid;
                    drUser["password"] = user.UserPassword;
                    drUser["wb"] = user.WbCode;
                    drUser["py"] = user.PyCode;
                    drUser["typeSourceId"] = user.TypeSourceId;
                    dtUser.Rows.Add(drUser);
                }
                intStart = dtUser.Columns.Count;
                UserList = listUser;
                if (listUser == null)//查询到的人员为空
                    return;

                ckeUser.Items.Clear();
                for (int i = 0; i < listUser.Count; i++)
                {
                    ckeUser.Items.Add(listUser[i].UserName, false);

                }

                #endregion

                #region 加载班次信息到控件里
                List<EntityOaDicShift> listShiftData = proxy.Service.GetDutyData();
                List<EntityOaDicShift> listShift = new List<EntityOaDicShift>();
                listShift = listShiftData.Where(w => w.ShiftDeptId == p).ToList();
                foreach (EntityOaDicShift shift in listShiftData)
                {
                    string strPlanName = shift.ShiftName;
                    if (!dtnPlanAll.ContainsKey(shift.ShiftId))
                    {
                        dtnPlanAll.Add(shift.ShiftId, strPlanName);
                    }
                }
                dtBC = new DataTable();
                if (!dtBC.Columns.Contains("duty_id"))
                {
                    dtBC.Columns.Add("duty_id");
                    dtBC.Columns.Add("duty_dept_id");
                    dtBC.Columns.Add("duty_name");
                    dtBC.Columns.Add("duty_sdate");
                    dtBC.Columns.Add("duty_edate");
                    dtBC.Columns.Add("duty_wb");
                    dtBC.Columns.Add("duty_py");
                    dtBC.Columns.Add("duty_exp");
                    dtBC.Columns.Add("duty_flag");
                    dtBC.Columns.Add("type_name");
                    dtBC.Columns.Add("type_wb");
                    dtBC.Columns.Add("type_py");
                    dtBC.Columns.Add("duty_del");
                }
                foreach (EntityOaDicShift shift in listShift)
                {
                    DataRow drBC = dtBC.NewRow();
                    drBC["duty_id"] = shift.ShiftId;
                    drBC["duty_dept_id"] = shift.ShiftDeptId;
                    drBC["duty_name"] = shift.ShiftName;
                    drBC["duty_sdate"] = shift.ShiftStartDate;
                    drBC["duty_edate"] = shift.ShiftEndDate;
                    drBC["duty_wb"] = shift.WbCode;
                    drBC["duty_py"] = shift.PyCode;
                    drBC["duty_exp"] = shift.ShiftRemark;
                    drBC["duty_flag"] = shift.ShiftFlag;
                    drBC["type_name"] = shift.TypeName;
                    drBC["type_wb"] = shift.TypeWb;
                    drBC["type_py"] = shift.TypePy;
                    drBC["duty_del"] = shift.DelFlag;
                    dtBC.Rows.Add(drBC);
                }
                List<EntityOaDicShift> list = new List<EntityOaDicShift>();
                foreach (EntityOaDicShift shift in listShift)
                {
                    list.Add(shift);
                }

                if (list.Count < 1)
                {
                    lis.client.control.MessageDialog.Show("你所在的物理组还没用创建班次内容\n请编辑班次！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                    dcl.client.frame.FrmCommon frm = this.MdiParent as FrmCommon;
                    MethodInfo mi = frm.GetType().GetMethod("LoadForm");
                    mi.Invoke(frm, new object[] { "排班设置", "dcl.client.oa.FrmDutyDict" });

                    return;
                }

                ckePlan.Items.Clear();
                dtnPlanName_id.Clear();
                dtnPlanId_name.Clear();

                foreach (EntityOaDicShift listPlanName in list)
                {

                    string strPlanName = listPlanName.ShiftName;
                    ckePlan.Items.Add(strPlanName, false);
                    if (dtnPlanName_id.ContainsKey(strPlanName))
                    {
                        lis.client.control.MessageDialog.Show("班次名字有重复，请先修改班次名字");
                        return;
                    }
                    dtnPlanName_id.Add(strPlanName, listPlanName.ShiftId);
                    dtnPlanId_name.Add(listPlanName.ShiftId, strPlanName);
                }
                #endregion

                #region 读取排班表资料_dtPersonPlan

                //读取排班表资料_dtPersonPlan
                DateTime from = Convert.ToDateTime(timeFrom.Text + " 00:00:00");
                DateTime to = Convert.ToDateTime(timeTo.Text + " 00:00:00");

                listPersonPlan = proxy.Service.GetDutyPlan(from, to, chkAll.Checked ? "All" : p);
                DevExpress.XtraGrid.Views.BandedGrid.GridBand userBand = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
                userBand.AppearanceHeader.Options.UseBackColor = true;
                userBand.AppearanceHeader.BackColor = this.BackColor;
                DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colName = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
                colName.AppearanceHeader.Options.UseBackColor = true;
                colName.AppearanceHeader.BackColor = this.BackColor;
                colName.AppearanceCell.Options.UseBackColor = true;
                colName.AppearanceCell.BackColor = this.BackColor;
                colName.Visible = true;
                colName.OptionsColumn.AllowEdit = false;
                userBand.OptionsBand.AllowMove = false;
                userBand.MinWidth = 160;

                userBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                if (rdoName.Checked)
                {
                    userBand.Caption = "姓名\\日期";
                    colName.FieldName = "Name";
                }
                else if (rdoDate.Checked)
                {
                    userBand.Caption = "姓名\\日期";
                    colName.FieldName = "Name";
                }
                else if (radioButton1.Checked)
                {
                    userBand.Caption = "班次\\日期";

                    colName.FieldName = "duty_name";
                }
                userBand.Columns.Add(colName);
                bandedGridView.Columns.Add(colName);
                bandedGridView.Bands.Add(userBand);
                DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand = null;
                for (DateTime tmp = from; tmp <= to; tmp = tmp.AddDays(1))
                {
                    //按日期排版的显示方式
                    if (rdoDate.Checked)
                    {
                        if (gridBand == null)
                        {
                            gridBand = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
                        }
                        //为了显示双行列，第一行列为月份，第二行具体到天数
                        if (gridBand.Caption != tmp.ToString("yyyy年-MM月"))
                        {
                            if (tmp != from)//判断第一次生成时不加进网格里
                            {
                                bandedGridView.Bands.Add(gridBand);
                            }

                            gridBand = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
                        }
                    }
                    else
                    {
                        gridBand = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
                    }


                    gridBand.AppearanceHeader.Options.UseBackColor = true;
                    gridBand.AppearanceHeader.BackColor = this.BackColor;
                    gridBand.OptionsBand.AllowMove = false;


                    if (rdoDate.Checked)
                    {
                        gridBand.Caption = tmp.ToString("yyyy年-MM月");
                    }
                    else
                    {
                        gridBand.Caption = tmp.ToString("yyyy-MM-dd") + " 星期" + dayOfWeek[Convert.ToInt32(tmp.DayOfWeek)];
                    }
                    gridBand.MinWidth = 120;



                    //DataRow[] drs = dtBC.Select("1=1");
                    for (int i = 0; i < list.Count; i++)
                    {
                        DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colDate = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
                        colDate.AppearanceHeader.Options.UseBackColor = true;
                        colDate.AppearanceHeader.BackColor = this.BackColor;
                        colDate.Visible = true;
                        //colDate.OptionsColumn.FixedWidth = true;

                        #region 加载按人员排版

                        if (rdoName.Checked)
                        {
                            DevExpress.XtraGrid.Views.BandedGrid.GridBand dateBand = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
                            dateBand.AppearanceHeader.Options.UseBackColor = true;
                            dateBand.AppearanceHeader.BackColor = this.BackColor;

                            dateBand.Caption = list[i].ShiftName;

                            bandedGridView.OptionsView.RowAutoHeight = false;
                            colDate.AppearanceCell.Options.UseBackColor = true;

                            colDate.ColumnEdit = checkEdit;
                            colDate.FieldName = tmp.ToString("yyyy-MM-dd") + this.strSplit.ToString() + list[i].ShiftId;

                            colDate.OptionsColumn.AllowEdit = false;

                            //dateBand.Width = 40;
                            if (list.Count < 3)
                            {
                                colDate.Width = 120 / list.Count;
                            }
                            else
                            {
                                colDate.Width = 40;
                            }


                            dateBand.Columns.Add(colDate);
                            bandedGridView.Columns.Add(colDate);
                            gridBand.Children.Add(dateBand);
                            DataColumn dataColumn = new DataColumn(colDate.FieldName, System.Type.GetType("System.Boolean"));
                            dataColumn.DefaultValue = false;
                            dtUser.Columns.Add(dataColumn);
                        }

                        #endregion

                        #region 加载按日期排版
                        else if (rdoDate.Checked)//加载按日期排版
                        {
                            DevExpress.XtraGrid.Views.BandedGrid.GridBand dateBand = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
                            dateBand.AppearanceHeader.Options.UseBackColor = true;
                            dateBand.AppearanceHeader.BackColor = this.BackColor;
                            dateBand.Caption = tmp.ToString("dd") + " " + dayOfWeek[Convert.ToInt32(tmp.DayOfWeek)];//日 星期

                            bandedGridView.OptionsView.RowAutoHeight = false;
                            this.bandedGridView.RowHeight = 20;



                            memoEdit.AutoHeight = true;
                            memoEdit.LinesCount = 8;

                            //设置列不可编辑无法触发子控件的点击事件
                            memoEdit.ReadOnly = true;
                            memoEdit.ScrollBars = ScrollBars.None;
                            memoEdit.Click += new EventHandler(memoEdit_Click);

                            colDate.AppearanceCell.Options.UseBackColor = true;
                            colDate.AppearanceCell.BackColor = Color.LightYellow;
                            colDate.AppearanceCell.Options.UseTextOptions = true;
                            colDate.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                            colDate.ColumnEdit = memoEdit;
                            colDate.FieldName = tmp.ToString("yyyy-MM-dd");
                            colDate.Caption = tmp.ToString("yyyy-MM-dd");
                            DataColumn dataColumn = new DataColumn(tmp.ToString("yyyy-MM-dd"), System.Type.GetType("System.String"));
                            dataColumn.DefaultValue = "";
                            dtUser.Columns.Add(dataColumn);

                            colDate.Width = 40;

                            dateBand.Columns.Add(colDate);
                            bandedGridView.Columns.Add(colDate);
                            gridBand.Children.Add(dateBand);

                            break;

                        }
                        #endregion

                        #region 加载按班次排版格式

                        else if (radioButton1.Checked)//加载按班次排版格式
                        {
                            bandedGridView.OptionsView.RowAutoHeight = true;
                            memoEdit.AutoHeight = true;
                            memoEdit.LinesCount = 8;

                            //设置列不可编辑无法触发子控件的点击事件
                            memoEdit.ReadOnly = true;
                            memoEdit.ScrollBars = ScrollBars.None;
                            memoEdit.Click += new EventHandler(memoEdit_Click);

                            colDate.AppearanceCell.Options.UseBackColor = true;
                            if (gridBand.Caption.Contains("星期六") || gridBand.Caption.Contains("星期日"))
                            {
                                colDate.AppearanceCell.BackColor = Color.LightPink;
                            }
                            else {
                                colDate.AppearanceCell.BackColor = Color.LightYellow;
                            }
                            colDate.AppearanceCell.Options.UseTextOptions = true;
                            colDate.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                            colDate.ColumnEdit = memoEdit;
                            colDate.FieldName = tmp.ToString("yyyy-MM-dd");
                            DataColumn dataColumn = new DataColumn(colDate.FieldName, System.Type.GetType("System.String"));
                            dataColumn.DefaultValue = "";
                            dtBC.Columns.Add(dataColumn);

                            colDate.Width = 125;


                            bandedGridView.Columns.Add(colDate);
                            gridBand.Columns.Add(colDate);


                            break;
                        }

                        #endregion
                    }
                    //按日期排班格式显示
                    if (rdoDate.Checked)
                    {
                        //为了显示双行列，第一行列为月份，第二行具体到天数
                        if (to.AddDays(-1) == tmp)
                        {
                            bandedGridView.Bands.Add(gridBand);
                        }
                    }
                    else
                    {
                        bandedGridView.Bands.Add(gridBand);
                    }


                }
                #endregion

                #region 显示信息
                if (rdoName.Checked)//按日期排班格式一
                {
                    #region 按人员
                    for (int i = 0; i < dtUser.Rows.Count; i++)
                    {
                        string userID = dtUser.Rows[i]["ID"].ToString();
                        string typeID = p;
                        for (int j = intStart; j < dtUser.Columns.Count; j++)
                        {
                            string[] fields = dtUser.Columns[j].ColumnName.Split(this.strSplit);
                            string planDate = fields[0];
                            string planID = fields[1];

                            List<EntityOaDicShiftDetail> listSelect = listPersonPlan.Where(w => w.ShiftDeptId == typeID && w.DetailUserId == userID && w.DetailShiftId == planID && w.DetailDate == Convert.ToDateTime(planDate)).ToList();

                            if (listSelect.Count > 0)
                            {
                                dtUser.Rows[i][j] = true;
                            }

                            if (chkAll.Checked)
                            {
                                listSelect = listPersonPlan.Where(w => w.ShiftDeptId != typeID && w.DetailUserId == userID && w.DetailShiftId != planID && w.ShiftName == dtnPlanAll[planID] && w.DetailDate == Convert.ToDateTime(planDate)).ToList();

                                if (listSelect.Count > 0)
                                {
                                    dtUser.Rows[i][j] = true;
                                }
                            }
                        }
                    }

                    gdPlan.DataSource = dtUser;
                    #endregion
                }
                else if (rdoDate.Checked)//按日期排班格式二
                {
                    #region 按日期
                    for (int i = 0; i < dtUser.Rows.Count; i++)
                    {
                        string userID = dtUser.Rows[i]["ID"].ToString();
                        string typeID = p;
                        for (int j = intStart; j < dtUser.Columns.Count; j++)
                        {
                            string planDate = dtUser.Columns[j].ColumnName;

                            List<EntityOaDicShiftDetail> listSelectArr = listPersonPlan.Where(w => w.ShiftDeptId == typeID && w.DetailUserId == userID && w.DetailDate == Convert.ToDateTime(planDate)).ToList();
                            string strPlanName = string.Empty;

                            if (listSelectArr.Count > 0)
                            {
                                foreach (EntityOaDicShiftDetail select in listSelectArr)
                                {
                                    if (dtnPlanId_name.ContainsKey(select.DetailShiftId))
                                    {
                                        strPlanName += "," + dtnPlanId_name[select.DetailShiftId];
                                    }
                                }
                            }
                            if (chkAll.Checked)
                            {
                                listSelectArr =
                                    listPersonPlan.Where(w => w.ShiftDeptId != typeID && w.DetailUserId == userID && w.DetailDate == Convert.ToDateTime(planDate)).ToList();
                                if (listSelectArr.Count > 0)
                                {
                                    foreach (EntityOaDicShiftDetail select in listSelectArr)
                                    {
                                        if (dtnPlanAll.ContainsKey(select.DetailShiftId) &&
                                            !strPlanName.Contains(dtnPlanAll[select.DetailShiftId]))
                                        {
                                            strPlanName += "," + dtnPlanAll[select.DetailShiftId];
                                        }

                                    }
                                }
                            }
                            if (strPlanName.Length > 0)
                            {
                                dtUser.Rows[i][j] = strPlanName.Substring(1);
                            }
                            else
                            {
                                dtUser.Rows[i][j] = strPlanName;
                            }
                        }
                    }

                    gdPlan.DataSource = dtUser;
                    #endregion
                }
                else if (radioButton1.Checked)
                {
                    #region 按班次
                    for (int i = 0; i < dtBC.Rows.Count; i++)
                    {
                        string planID = dtBC.Rows[i]["duty_id"].ToString();
                        string typeID = p;

                        for (DateTime tmp = from; tmp <= to; tmp = tmp.AddDays(1))
                        {
                            List<EntityOaDicShiftDetail> listSelects = listPersonPlan.Where(w => w.ShiftDeptId == typeID && w.DetailShiftId == planID && w.DetailDate == tmp).ToList();
                            string userList = "";
                            if (listSelects.Count > 0)
                            {
                                foreach (EntityOaDicShiftDetail Select in listSelects)
                                {
                                    userList += "," + Select.UserName;
                                }
                            }

                            if (chkAll.Checked)
                            {
                                listSelects = listPersonPlan.Where(w => w.ShiftDeptId != typeID && w.DetailShiftId != planID && w.ShiftName == dtnPlanAll[planID] && w.DetailDate == tmp).ToList();
                                if (listSelects.Count > 0)
                                {
                                    foreach (EntityOaDicShiftDetail Select in listSelects)
                                    {
                                        if (!userList.Contains(Select.UserName) && dtUser.Select("Name='" + Select.UserName + "'").Length > 0)
                                            userList += "," + Select.UserName;
                                    }
                                }
                            }

                            if (userList.Length > 0)
                            {
                                dtBC.Rows[i][tmp.ToString("yyyy-MM-dd")] = userList.Substring(1);
                            }

                        }
                    }

                    gdPlan.DataSource = dtBC;
                    #endregion
                }

                #endregion

            }
            catch (Exception exception)
            {
                Lib.LogManager.Logger.LogException(exception);
            }

        }
        #endregion

        #region 自动生成排班模板
        /// <summary>
        /// 自动生成排班模板
        /// </summary>
        /// <param name="p"></param>
        /// <param name="templateType"></param>
        private void LoadDutyTemplateInfo(string p, string templateType, DateTime now)
        {
            try
            {
                #region MyRegion
                ProxyOaShiftDict proxy = new ProxyOaShiftDict();
                bandedGridView.Bands.Clear();
                bandedGridView.Columns.Clear();

                #region 读取人员资料

                List<EntitySysUser> listUser = proxy.Service.GetUser(p);
                DataTable dtUser = new DataTable();
                dtUser.Columns.Add("ID");
                dtUser.Columns.Add("Name");
                dtUser.Columns.Add("loginId");
                dtUser.Columns.Add("userType");
                dtUser.Columns.Add("password");
                dtUser.Columns.Add("wb");
                dtUser.Columns.Add("py");
                dtUser.Columns.Add("typeSourceId");
                List<EntityOaShiftTemplate> listtemp = listTemp.Where(w => w.TempId == templateType).ToList();
                string[] strUser = listtemp[0].TempUserId.Split(' ');
                foreach (string strID in strUser)
                {
                    string strType = strID.Substring(0, strID.IndexOf(':'));
                    if (strType == p)
                    {
                        string[] strUserList = strID.Substring(strID.IndexOf(':') + 1).Split(',');
                        foreach (string strListID in strUserList)
                        {
                            List<EntitySysUser> drUser = listUser.Where(w => w.UserId == strListID).ToList();
                            if (drUser.Count > 0)
                            {
                                DataRow User = dtUser.NewRow();
                                User["ID"] = drUser[0].UserId;
                                User["Name"] = drUser[0].UserName;
                                User["loginId"] = drUser[0].UserLoginid;
                                User["password"] = drUser[0].UserPassword;
                                User["wb"] = drUser[0].WbCode;
                                User["py"] = drUser[0].PyCode;
                                User["typeSourceId"] = drUser[0].TypeSourceId;
                                dtUser.Rows.Add(User);
                            }
                        }
                    }
                }
                int intStart = dtUser.Columns.Count;

                if (dtUser == null)//查询到的人员为空
                    return;
                #endregion
                int days = DateTime.DaysInMonth(now.Year, now.Month);

                #region 加载班次信息到控件里
                List<EntityOaDicShift> listDutyData = proxy.Service.GetDutyData();
                foreach (EntityOaDicShift shift in listDutyData)
                {
                    string strPlanName = shift.ShiftName;
                    if (!dtnPlanAll.ContainsKey(shift.ShiftId))
                    {
                        dtnPlanAll.Add(shift.ShiftId, strPlanName);
                    }
                }
                List<EntityOaDicShift> lists = listDutyData.Where(w => w.ShiftDeptId == p).ToList();
                dtBC = dtBC.Clone();
                foreach (EntityOaDicShift shift in lists)
                {
                    DataRow drBC = dtBC.NewRow();
                    drBC["duty_id"] = shift.ShiftId;
                    drBC["duty_dept_id"] = shift.ShiftDeptId;
                    drBC["duty_name"] = shift.ShiftName;
                    drBC["duty_sdate"] = shift.ShiftStartDate;
                    drBC["duty_edate"] = shift.ShiftEndDate;
                    drBC["duty_wb"] = shift.WbCode;
                    drBC["duty_py"] = shift.PyCode;
                    drBC["duty_exp"] = shift.ShiftRemark;
                    drBC["duty_flag"] = shift.ShiftFlag;
                    drBC["type_name"] = shift.TypeName;
                    drBC["type_wb"] = shift.TypeWb;
                    drBC["type_py"] = shift.TypePy;
                    drBC["duty_del"] = shift.DelFlag;
                    dtBC.Rows.Add(drBC);
                }

                if (dtBC.Rows.Count < 1)
                {
                    lis.client.control.MessageDialog.Show("你所在的物理组还没用创建班次内容\n请编辑班次！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                    dcl.client.frame.FrmCommon frm = this.MdiParent as FrmCommon;
                    MethodInfo mi = frm.GetType().GetMethod("LoadForm");
                    mi.Invoke(frm, new object[] { "排班设置", "dcl.client.oa.FrmDutyDict" });

                    return;
                }

                ckePlan.Items.Clear();
                dtnPlanName_id.Clear();
                dtnPlanId_name.Clear();

                foreach (DataRow drPlanName in dtBC.Rows)
                {

                    string strPlanName = drPlanName["duty_name"].ToString();
                    ckePlan.Items.Add(strPlanName, false);
                    if (dtnPlanName_id.ContainsKey(strPlanName))
                    {
                        lis.client.control.MessageDialog.Show("班次名字有重复，请先修改班次名字");
                        return;
                    }
                    dtnPlanName_id.Add(strPlanName, drPlanName["duty_id"].ToString());
                    dtnPlanId_name.Add(drPlanName["duty_id"].ToString(), strPlanName);
                }
                #endregion

                #region 构造grideview
                DateTime from = new DateTime(now.Year, now.Month, 1);
                DateTime to = from.AddMonths(1).AddDays(-1);
                listPersonPlan = proxy.Service.GetDutyPlan(from, to, p);

                DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand = null;
                DevExpress.XtraGrid.Views.BandedGrid.GridBand titleBand = null;
                titleBand = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
                titleBand.AppearanceHeader.Options.UseBackColor = true;
                titleBand.AppearanceHeader.BackColor = this.BackColor;
                titleBand.OptionsBand.AllowMove = false;

                titleBand.Caption = cboDictType.displayMember + from.ToString("yyyy") + "年" + from.ToString("MM") + "月" + "值班表";
                titleBand.AppearanceHeader.Options.UseFont = true;
                titleBand.AppearanceHeader.Options.UseTextOptions = true;
                titleBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                titleBand.AppearanceHeader.Font = new Font(titleBand.AppearanceHeader.Font.FontFamily, 18);
                titleBand.AppearanceHeader.Font = new Font(titleBand.AppearanceHeader.Font, titleBand.AppearanceHeader.Font.Style | FontStyle.Bold);
                this.bandedGridView.BandPanelRowHeight = 35;

                //按日期排版的显示方式
                if (bandedGridView.Columns.Count == 0)
                {
                    gridBand = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
                    DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colName = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
                    colName.AppearanceHeader.Options.UseBackColor = true;
                    colName.AppearanceHeader.BackColor = this.BackColor;
                    colName.AppearanceCell.Options.UseBackColor = true;
                    colName.AppearanceCell.BackColor = this.BackColor;
                    colName.Visible = true;
                    colName.OptionsColumn.AllowEdit = false;
                    DevExpress.XtraGrid.Views.BandedGrid.GridBand userChildBand = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
                    userChildBand.AppearanceHeader.Options.UseBackColor = true;
                    userChildBand.AppearanceHeader.BackColor = this.BackColor;
                    userChildBand.Caption = "星期";
                    gridBand.Caption = "日期";
                    colName.FieldName = "Name";

                    userChildBand.Columns.Add(colName);
                    gridBand.Children.Add(userChildBand);

                    bandedGridView.Columns.Add(colName);
                    titleBand.Children.Add(gridBand);
                    if (templateType == "10002")
                    {
                        DevExpress.XtraGrid.Views.BandedGrid.GridBand postChildBand = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
                        postChildBand.AppearanceHeader.Options.UseBackColor = true;
                        postChildBand.AppearanceHeader.BackColor = this.BackColor;
                        postChildBand.Caption = "岗位";
                        DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colPost = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
                        colPost.AppearanceHeader.Options.UseBackColor = true;
                        colPost.AppearanceHeader.BackColor = this.BackColor;
                        colPost.Visible = true;
                        colPost.AppearanceCell.Options.UseBackColor = true;
                        colPost.AppearanceCell.BackColor = Color.LightYellow;
                        colPost.AppearanceCell.Options.UseTextOptions = true;
                        colPost.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;

                        colPost.FieldName = "post";
                        colPost.Caption = "post";
                        DataColumn dataColumn = new DataColumn("post", System.Type.GetType("System.String"));
                        dataColumn.DefaultValue = "";
                        dtUser.Columns.Add(dataColumn);

                        postChildBand.AppearanceHeader.Options.UseTextOptions = true;
                        postChildBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        postChildBand.Columns.Add(colPost);
                        bandedGridView.Columns.Add(colPost);
                        titleBand.Children.Add(postChildBand);
                    }
                }
                for (DateTime tmp = from; tmp <= to; tmp = tmp.AddDays(1))
                {
                    gridBand = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
                    gridBand.AppearanceHeader.Options.UseBackColor = true;
                    gridBand.AppearanceHeader.BackColor = this.BackColor;
                    gridBand.OptionsBand.AllowMove = false;

                    gridBand.Caption = tmp.ToString("dd");
                    gridBand.MinWidth = 120;

                    memoEdit.AutoHeight = true;
                    memoEdit.LinesCount = 30;

                    //设置列不可编辑无法触发子控件的点击事件
                    memoEdit.ReadOnly = true;
                    memoEdit.ScrollBars = ScrollBars.None;
                    memoEdit.Click += new EventHandler(memoEdit_Click);

                    DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colDate = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
                    colDate.AppearanceHeader.Options.UseBackColor = true;
                    colDate.AppearanceHeader.BackColor = this.BackColor;
                    colDate.Visible = true;
                    colDate.ColumnEdit = memoEdit;
                    //colDate.OptionsColumn.FixedWidth = true;
                    DevExpress.XtraGrid.Views.BandedGrid.GridBand dateBand = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
                    dateBand.AppearanceHeader.Options.UseBackColor = true;
                    dateBand.AppearanceHeader.BackColor = this.BackColor;
                    dateBand.Caption = dayOfWeek[Convert.ToInt32(tmp.DayOfWeek)];//日 星期

                    bandedGridView.OptionsView.RowAutoHeight = false;
                    this.bandedGridView.RowHeight = 20;

                    colDate.AppearanceCell.Options.UseBackColor = true;
                    colDate.AppearanceCell.BackColor = Color.LightYellow;
                    colDate.AppearanceCell.Options.UseTextOptions = true;
                    colDate.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;

                    colDate.FieldName = tmp.ToString("yyyy-MM-dd");
                    colDate.Caption = tmp.ToString("yyyy-MM-dd");
                    DataColumn dataColumn = new DataColumn(tmp.ToString("yyyy-MM-dd"), System.Type.GetType("System.String"));
                    dataColumn.DefaultValue = "";
                    dtUser.Columns.Add(dataColumn);

                    colDate.Width = 40;

                    dateBand.AppearanceHeader.Options.UseTextOptions = true;
                    dateBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gridBand.AppearanceHeader.Options.UseTextOptions = true;
                    gridBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                    dateBand.Columns.Add(colDate);
                    bandedGridView.Columns.Add(colDate);
                    gridBand.Children.Add(dateBand);
                    titleBand.Children.Add(gridBand);
                }

                bandedGridView.Bands.Add(titleBand);
                if (templateType == "10002")
                {
                    DevExpress.XtraGrid.Views.BandedGrid.GridBand holidayTitleBand = null;
                    holidayTitleBand = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
                    holidayTitleBand.AppearanceHeader.Options.UseBackColor = true;
                    holidayTitleBand.AppearanceHeader.BackColor = this.BackColor;
                    holidayTitleBand.OptionsBand.AllowMove = false;
                    holidayTitleBand.Caption = "存假情况";
                    gridBand = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
                    gridBand.AppearanceHeader.Options.UseBackColor = true;
                    gridBand.AppearanceHeader.BackColor = this.BackColor;
                    gridBand.OptionsBand.AllowMove = false;

                    gridBand.Caption = from.ToString("yyyy") + "公休假";
                    gridBand.AppearanceHeader.Options.UseTextOptions = true;
                    gridBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    gridBand.MinWidth = 120;
                    holidayTitleBand.AppearanceHeader.Options.UseFont = true;
                    holidayTitleBand.AppearanceHeader.Options.UseTextOptions = true;
                    holidayTitleBand.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    holidayTitleBand.AppearanceHeader.Font = new Font(holidayTitleBand.AppearanceHeader.Font.FontFamily, 11);
                    holidayTitleBand.AppearanceHeader.Font = new Font(holidayTitleBand.AppearanceHeader.Font, titleBand.AppearanceHeader.Font.Style | FontStyle.Bold);

                    string[] type = new string[] { "已休", "未休", "法定" };
                    for (int i = 0; i < 3; i++)
                    {
                        DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn holidayType = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
                        holidayType.AppearanceHeader.Options.UseBackColor = true;
                        holidayType.AppearanceHeader.BackColor = this.BackColor;
                        holidayType.Visible = true;
                        holidayType.AppearanceCell.Options.UseBackColor = true;
                        holidayType.AppearanceCell.BackColor = Color.LightYellow;
                        holidayType.AppearanceCell.Options.UseTextOptions = true;
                        holidayType.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;

                        DevExpress.XtraGrid.Views.BandedGrid.GridBand holidayBand = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
                        holidayBand.AppearanceHeader.Options.UseBackColor = true;
                        holidayBand.AppearanceHeader.BackColor = this.BackColor;
                        holidayBand.Caption = type[i];
                        string holiday_type = "holiday_type" + i;
                        holidayType.FieldName = holiday_type;
                        holidayType.Caption = holiday_type;
                        DataColumn dataColumn = new DataColumn(holiday_type, System.Type.GetType("System.String"));
                        dataColumn.DefaultValue = "";
                        dtUser.Columns.Add(dataColumn);

                        holidayType.Width = 40;

                        holidayBand.Columns.Add(holidayType);
                        bandedGridView.Columns.Add(holidayType);
                        gridBand.Children.Add(holidayBand);
                    }
                    holidayTitleBand.Children.Add(gridBand);
                    bandedGridView.Bands.Add(holidayTitleBand);
                }
                #endregion

                #region 显示信息
                for (int i = 0; i < dtUser.Rows.Count; i++)
                {
                    string userID = dtUser.Rows[i]["ID"].ToString();
                    string typeID = p;
                    for (int j = startCol; j < (days + startCol); j++)
                    {
                        string planDate = dtUser.Columns[j].ColumnName;
                        List<EntityOaDicShiftDetail> listSelectArr = listPersonPlan.Where(w => w.ShiftDeptId == typeID && w.DetailUserId == userID && w.DetailDate == Convert.ToDateTime(planDate)).ToList();
                        string strPlanName = string.Empty;

                        if (listSelectArr.Count > 0)
                        {
                            foreach (EntityOaDicShiftDetail select in listSelectArr)
                            {
                                if (dtnPlanId_name.ContainsKey(select.DetailShiftId))
                                {
                                    strPlanName += "," + dtnPlanId_name[select.DetailShiftId];
                                }
                                if (dtemplate_id == "10002")
                                {
                                    dtUser.Rows[i]["post"] = select.DetailWorkPost;
                                    dtUser.Rows[i]["holiday_type0"] = select.DetailHolidayA;
                                    dtUser.Rows[i]["holiday_type1"] = select.DetailHolidayB;
                                    dtUser.Rows[i]["holiday_type2"] = select.DetailHolidayC;
                                }
                            }
                        }
                        if (strPlanName.Length > 0)
                        {
                            dtUser.Rows[i][j] = strPlanName.Substring(1);
                        }
                        else
                        {
                            dtUser.Rows[i][j] = strPlanName;
                        }
                    }
                }

                dtUserTemp = dtUser.Copy();
                DataRow drRemark = dtUserTemp.NewRow();
                drRemark["Name"] = listtemp[0].TempRemark;
                dtUserTemp.Rows.Add(drRemark);
                this.label1.Text = listtemp[0].TempRemark;

                #endregion
                gdPlan.DataSource = dtUser;

                #endregion
            }
            catch (Exception exception)
            {
                Lib.LogManager.Logger.LogException(exception);
            }
        }

        #endregion

        DevExpress.XtraEditors.MemoEdit edit = new DevExpress.XtraEditors.MemoEdit();

        void memoEdit_Click(object sender, EventArgs e)
        {
            if (sysToolBar1.BtnModify.Enabled == true)
            {
                return;
            }

            edit = (DevExpress.XtraEditors.MemoEdit)sender;
            string[] user = edit.Text.Split(',');
            if (UserInfo.GetSysConfigValue("Office_dutyTemplateSelect") != "是")
            {
                if (rdoDate.Checked)//按日期选择排班时显示班次
                {
                    for (int i = 0; i < ckePlan.Items.Count; i++)
                    {
                        if (Array.IndexOf(user, ckePlan.Items[i].Value.ToString()) != -1)
                        {
                            ckePlan.Items[i].CheckState = CheckState.Checked;
                        }
                        else
                        {
                            ckePlan.Items[i].CheckState = CheckState.Unchecked;
                        }
                    }
                    popEidtPlan.Location = PointToClient(new Point(Cursor.Position.X - 5, Cursor.Position.Y - 50));
                    popEidtPlan.ShowPopup();
                }
                else if (radioButton1.Checked)//按班次选择排班时显示人名
                {
                    for (int i = 0; i < ckeUser.Items.Count; i++)
                    {
                        if (Array.IndexOf(user, ckeUser.Items[i].Value.ToString()) != -1)
                        {
                            ckeUser.Items[i].CheckState = CheckState.Checked;
                        }
                        else
                        {
                            ckeUser.Items[i].CheckState = CheckState.Unchecked;
                        }
                    }

                    popEdit.Location = PointToClient(new Point(Cursor.Position.X - 5, Cursor.Position.Y - 50));
                    popEdit.ShowPopup();
                }
            }
            else
            {
                for (int i = 0; i < ckePlan.Items.Count; i++)
                {
                    if (Array.IndexOf(user, ckePlan.Items[i].Value.ToString()) != -1)
                    {
                        ckePlan.Items[i].CheckState = CheckState.Checked;
                    }
                    else
                    {
                        ckePlan.Items[i].CheckState = CheckState.Unchecked;
                    }
                }
                popEidtPlan.Location = PointToClient(new Point(Cursor.Position.X - 5, Cursor.Position.Y - 50));
                popEidtPlan.ShowPopup();
            }

        }
        DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit memoEdit = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();

        #region 修改时间
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timeFrom_EditValueChanged(object sender, EventArgs e)
        {
            if (cboDictType.valueMember == null && !firstLoad)
            {
                //lis.client.control.MessageDialog.Show("请选择物理组别！！");
                //LoadDutyInfo("All");
            }
            else if (cboDictType.valueMember == "")
            {
                lis.client.control.MessageDialog.Show("显示全部人员!!");
                string strType = cboDictType.valueMember;
                LoadDutyInfo("All");
            }
            else
            {
                string strType = cboDictType.valueMember;
                LoadDutyInfo(strType);
            }
        }

        #endregion

        #region 选择分组

        /// <summary>
        /// 选择分组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoName_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (sysToolBar1.Enabled == true && sysToolBar1.BtnModify.Enabled == false && firstLoad == false)
                {
                    DialogResult dresult = MessageBox.Show("切换视图将丢失未保存的排班计划,是否确认?", OfficeMessage.BASE_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    if (dresult == DialogResult.Cancel)
                    {
                        firstLoad = true;
                        rdoName.Checked = !rdoName.Checked;
                        firstLoad = false;
                        return;
                    }
                }

                sysToolBar1.BtnSinglePrint.Enabled = rdoName.Checked;

                if (cboDictType.valueMember == "")
                {
                    lis.client.control.MessageDialog.Show("显示全部人员!!");
                    string strType = cboDictType.valueMember;
                    LoadDutyInfo("All");
                }
                else
                {
                    string strType = cboDictType.valueMember;
                    LoadDutyInfo(strType);
                }
            }
            catch (Exception exception)
            {
                Lib.LogManager.Logger.LogException(exception);
            }
        }

        #endregion

        #region 选择框关闭时写入数据

        /// <summary>
        /// 选择框关闭时写入数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popEdit_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {

            string user = "";
            bindingSourceUser.EndEdit();
            gridViewUser.CloseEditor();
            DataTable dt = (DataTable)gdPlan.DataSource;
            List<EntitySysUser> detailTb = bindingSourceUser.DataSource as List<EntitySysUser>;
            if (detailTb != null)
            {
                List<EntitySysUser> rows = detailTb.Where(w => w.Checked.ToString().Contains("True")).ToList();
                foreach (EntitySysUser row in rows)
                {
                    if (row.UserId != null && row.UserId.ToString() != "-1")
                    {
                        user += "," + row.UserName;
                    }
                }
                foreach (EntitySysUser sysuser in detailTb)
                {
                    sysuser.Checked = false;
                }
                bindingSourceUser.DataSource = detailTb;
            }
            if (user != "")
            {
                dt.Rows[bandedGridView.FocusedRowHandle][bandedGridView.FocusedColumn.FieldName] = user.Substring(1);
            }
            else
            {
                dt.Rows[bandedGridView.FocusedRowHandle][bandedGridView.FocusedColumn.FieldName] = "";
            }
        }

        #endregion

        private void timeFrom_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (sysToolBar1.Enabled == true && sysToolBar1.BtnModify.Enabled == false)
            {
                DialogResult dresult = MessageBox.Show("切换视图将丢失未保存的排班计划,是否确认?", OfficeMessage.BASE_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (dresult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }


        #region 操作控制
        /// <summary>
        /// 操作控制
        /// </summary>
        private void SetColAllowEdit(bool allowEdit)
        {
            if (rdoName.Checked)
            {
                for (int i = 1; i < bandedGridView.Columns.Count; i++)
                {
                    bandedGridView.Columns[i].OptionsColumn.AllowEdit = allowEdit;
                }
            }
        }

        #endregion

        #region 修改排班计划

        /// <summary>
        /// 修改排班计划
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar_OnBtnModifyClicked(object sender, EventArgs e)
        {
            SetColAllowEdit(true);
            sysToolBar1.BtnSinglePrint.Enabled = rdoName.Checked | rdoDate.Checked;
        }

        #endregion

        #region 保存按钮

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar_OnBtnSaveClicked(object sender, EventArgs e)
        {
            #region 张力
            if (gdPlan.DataSource == null)
            {
                return;
            }
            popEdit.Location = new Point(0, 0);
            popEdit.Focus();

            this.isActionSuccess = false;
            DataTable dt = ((DataTable)(gdPlan.DataSource)).Copy();
            ProxyOaShiftDictDetail proxy = new ProxyOaShiftDictDetail();
            #region 需要保存的数据

            List<EntityOaDicShiftDetail> dtSave = new List<EntityOaDicShiftDetail>();
            #endregion
            #region 要清除的数据
            DateTime dtNow = date;
            Dictionary<string, object> dict = new Dictionary<string, object>();
            string[] par = new string[4];
            string isshow = UserInfo.GetSysConfigValue("Office_dutyTemplateSelect");
            if (UserInfo.GetSysConfigValue("Office_dutyTemplateSelect") != "是")
            {
                par[0] = isshow;
                par[1] = timeFrom.Text;
                par[2] = timeTo.Text;
                par[3] = null;
                dict.Add("par", par);
            }
            else
            {
                DateTime from = new DateTime(dtNow.Year, dtNow.Month, 1);
                DateTime to = from.AddMonths(1);
                par[0] = isshow;
                par[1] = from.ToString();
                par[2] = to.ToString();
                par[3] = cboDictType.valueMember;
                dict.Add("par", par);
            }
            #endregion

            #region 无模板
            if (UserInfo.GetSysConfigValue("Office_dutyTemplateSelect") != "是")
            {
                if (rdoName.Checked == true)
                {
                    #region 从人员视图保存
                    string typeID = cboDictType.valueMember;
                    foreach (DataRow dr in dt.Rows)
                    {
                        string userId = dr["ID"].ToString();
                        string userName = dr["Name"].ToString();

                        for (int i = intStart; i < dt.Columns.Count; i++)
                        {
                            if (Convert.ToBoolean(dr[i]) == false)
                            {
                                continue;
                            }
                            string[] fieldName = dt.Columns[i].ColumnName.Split(this.strSplit);
                            string planID = fieldName[1];
                            DateTime planDate = Convert.ToDateTime(fieldName[0] + " 00:00:00");

                            DataRow[] drBC = dtBC.Select("duty_id = '" + planID + "'");

                            EntityOaDicShiftDetail drSave = new EntityOaDicShiftDetail();

                            string planName = drBC[0]["duty_name"].ToString();
                            string pTimeFrom = drBC[0]["duty_sdate"].ToString();
                            string pTimeTo = drBC[0]["duty_edate"].ToString();

                            drSave.DetailUserId = userId;
                            drSave.DetailShiftId = planID;
                            drSave.DetailDate = planDate;
                            dtSave.Add(drSave);
                        }
                    }
                    #endregion
                }
                else if (rdoDate.Checked)
                {
                    #region 日期
                    //分离每人
                    foreach (DataRow dr in dt.Rows)
                    {
                        string userId = dr["ID"].ToString();
                        string userName = dr["Name"].ToString();

                        //分离每人每一天
                        for (int i = intStart; i < dt.Columns.Count; i++)
                        {
                            if (dr[i] == DBNull.Value || string.IsNullOrEmpty(dr[i].ToString()))
                            {
                                continue;
                            }
                            string[] strPlanArr = dr[i].ToString().Split(',');
                            //分离每人每一天的每一个班次
                            foreach (string strPlanName in strPlanArr)
                            {
                                if (!dtnPlanName_id.ContainsKey(strPlanName))
                                {
                                    continue;
                                }

                                DateTime planDate = Convert.ToDateTime(dt.Columns[i].ColumnName + " 00:00:00");
                                string planID = dtnPlanName_id[strPlanName];
                                DataRow[] drBC = dtBC.Select("duty_id ='" + planID + "'");
                                EntityOaDicShiftDetail drSave = new EntityOaDicShiftDetail();

                                string planName = drBC[0]["duty_name"].ToString();
                                string pTimeFrom = drBC[0]["duty_sdate"].ToString();
                                string pTimeTo = drBC[0]["duty_edate"].ToString();

                                drSave.DetailUserId = userId;
                                drSave.DetailShiftId = planID;
                                drSave.DetailDate = planDate;
                                dtSave.Add(drSave);
                            }
                        }
                    }

                    #endregion
                }
                else if (radioButton1.Checked == true)
                {
                    #region 从班次视图保存

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string planID = dt.Rows[i]["duty_id"].ToString();
                        DataRow[] drBC = dtBC.Select("duty_id = '" + planID + "'");

                        string planName = drBC[0]["duty_name"].ToString();
                        string pTimeFrom = drBC[0]["duty_sdate"].ToString();
                        string pTimeTo = drBC[0]["duty_edate"].ToString();

                        DateTime from = Convert.ToDateTime(timeFrom.Text + " 00:00:00");
                        DateTime to = Convert.ToDateTime(timeTo.Text + " 00:00:00");
                        for (DateTime tmp = from; tmp <= to; tmp = tmp.AddDays(1))
                        {
                            string userString = dt.Rows[i][tmp.ToString("yyyy-MM-dd")].ToString();

                            if (userString.Length > 0)
                            {
                                string[] arrUser = userString.Split(',');
                                for (int j = 0; j < arrUser.Length; j++)
                                {
                                    EntityOaDicShiftDetail drSave = new EntityOaDicShiftDetail();
                                    List<EntitySysUser> user = UserList.Where(w => w.UserName == arrUser[j]).ToList();
                                    drSave.DetailUserId = user[0].UserId;
                                    drSave.DetailShiftId = planID;
                                    drSave.DetailDate = tmp;
                                    dtSave.Add(drSave);
                                }
                            }
                        }
                    }
                    #endregion
                }


                if (dtSave.Count > 0)
                {
                    string message = string.Empty;
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    foreach (EntityOaDicShiftDetail row in dtSave)
                    {
                        string planID = row.DetailShiftId;
                        DataRow[] drBC = dtBC.Select("duty_id = '" + planID + "'");
                        DateTime rowStart = Convert.ToDateTime(drBC[0]["duty_sdate"]);
                        DateTime rowEnd = Convert.ToDateTime(drBC[0]["duty_edate"]);
                        string planName = drBC[0]["duty_name"].ToString();
                        foreach (EntityOaDicShiftDetail rowCheck in dtSave)
                        {
                            if (row.DetailShiftId == rowCheck.DetailShiftId
                                || row.DetailUserId != rowCheck.DetailUserId
                                || row.DetailDate != rowCheck.DetailDate)
                                continue;

                            DataRow[] drBcCheck = dtBC.Select("duty_id = '" + rowCheck.DetailShiftId + "'");
                            DateTime compareStart = Convert.ToDateTime(drBcCheck[0]["duty_sdate"]);
                            DateTime compareEnd = Convert.ToDateTime(drBcCheck[0]["duty_edate"]);
                            string planNameCheck = drBcCheck[0]["duty_name"].ToString();

                            if (!dic.ContainsKey(rowCheck.DetailUserId + Convert.ToDateTime(rowCheck.DetailDate).ToString("yyyy-MM-dd")))
                            {
                                dic.Add(rowCheck.DetailUserId + Convert.ToDateTime(rowCheck.DetailDate).ToString("yyyy-MM-dd"), planNameCheck + planName);
                            }
                            else
                            {
                                if (dic[rowCheck.DetailUserId + Convert.ToDateTime(rowCheck.DetailDate).ToString("yyyy-MM-dd")].Contains(planNameCheck))
                                {
                                    continue;
                                }
                                else
                                {
                                    dic[rowCheck.DetailUserId + Convert.ToDateTime(rowCheck.DetailDate).ToString("yyyy-MM-dd")] += planNameCheck;
                                }
                            }

                            if ((rowStart == compareStart && rowEnd == compareEnd)
                                || (rowStart > compareStart && rowStart < compareEnd)
                                || (rowEnd > compareStart && rowEnd < compareEnd)
                                || (rowStart < compareStart && rowEnd >= compareEnd)
                                || (rowStart > compareStart && rowEnd < compareEnd))
                            {
                                List<EntitySysUser> listuser = UserList.Where(w => w.UserId == rowCheck.DetailUserId).ToList();
                                message += string.Format("[{3}]在[{0}]的[{1}]与[{2}]的排班时间存在冲突;\r\n",
                                    Convert.ToDateTime(rowCheck.DetailDate).ToString("yyyy-MM-dd"),
                                    planNameCheck, planName, listuser[0].UserName);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(message))
                    {
                        DialogResult dresult = MessageBox.Show(message + "是否继续保存？", OfficeMessage.BASE_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        if (dresult == DialogResult.Cancel)
                        {
                            return;
                        }
                    }
                }
            }
            #endregion

            #region 有模板

            else
            {
                int days = DateTime.DaysInMonth(dtNow.Year, dtNow.Month);
                //分离每人
                foreach (DataRow dr in dt.Rows)
                {
                    string userId = dr["ID"].ToString();
                    string userName = dr["Name"].ToString();

                    //分离每人每一天
                    for (int i = startCol; i < (days + startCol); i++)
                    {
                        if (dr[i] == DBNull.Value || string.IsNullOrEmpty(dr[i].ToString()))
                        {
                            continue;
                        }
                        string[] strPlanArr = dr[i].ToString().Split(',');
                        //分离每人每一天的每一个班次
                        foreach (string strPlanName in strPlanArr)
                        {
                            if (!dtnPlanName_id.ContainsKey(strPlanName))
                            {
                                continue;
                            }
                            DateTime planDate = Convert.ToDateTime(dt.Columns[i].ColumnName + " 00:00:00");
                            string planID = dtnPlanName_id[strPlanName];
                            DataRow[] drBC = dtBC.Select("duty_id ='" + planID + "'");
                            EntityOaDicShiftDetail drSave = new EntityOaDicShiftDetail();

                            string planName = drBC[0]["duty_name"].ToString();
                            string pTimeFrom = drBC[0]["duty_sdate"].ToString();
                            string pTimeTo = drBC[0]["duty_edate"].ToString();

                            drSave.DetailUserId = userId;
                            drSave.DetailShiftId = planID;
                            drSave.DetailDate = planDate;
                            drSave.DetailType = cboDictType.valueMember;
                            if (dt.Columns.Contains("post"))
                            {
                                drSave.DetailWorkPost = dr["post"].ToString();
                            }
                            if (dt.Columns.Contains("holiday_type0"))
                            {
                                drSave.DetailHolidayA = dr["holiday_type0"].ToString();
                            }
                            if (dt.Columns.Contains("holiday_type1"))
                            {
                                drSave.DetailHolidayB = dr["holiday_type1"].ToString();
                            }
                            if (dt.Columns.Contains("holiday_type2"))
                            {
                                drSave.DetailHolidayC = dr["holiday_type2"].ToString();
                            }

                            dtSave.Add(drSave);
                        }
                    }
                }
            }

            #endregion
            dict.Add("listShiftDetail", dtSave);
            EntityRequest request = new EntityRequest();
            request.SetRequestValue(dict);
            #region 保存操作
            bool isSuccess = proxy.Service.UpdateShiftPlan(request);
            if (isSuccess)
            {
                SetColAllowEdit(false);
                sysToolBar1.BtnSinglePrint.Enabled = rdoName.Checked | rdoDate.Checked;
                if (UserInfo.GetSysConfigValue("Office_dutyTemplateSelect") == "是")
                {
                    string strType = cboDictType.valueMember;
                    LoadDutyTemplateInfo(strType, dtemplate_id, dtNow);
                }
                EnableButton(false);
                lis.client.control.MessageDialog.Show("修改成功！");
            }
            #endregion


            #endregion
        }

        #endregion

        #region 放弃按钮

        /// <summary>
        /// 放弃按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar_OnBtnCancelClicked(object sender, EventArgs e)
        {
            //LoadCols();
            string strType = cboDictType.valueMember;
            if (UserInfo.GetSysConfigValue("Office_dutyTemplateSelect") != "是")
            {
                LoadDutyInfo(strType);
            }
            else
            {
                LoadDutyTemplateInfo(strType, dtemplate_id, DateTime.Now);
            }
            sysToolBar1.BtnSinglePrint.Enabled = rdoName.Checked | rdoDate.Checked;
        }

        #endregion

        #region 导出到Excel
        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar_OnBtnExportClicked(object sender, EventArgs e)
        {
            if (gdPlan.DataSource != null)
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.DefaultExt = "xls";
                ofd.Filter = "Excel文件(*.xls)|*.xls";
                ofd.Title = "导出到Excel";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.Trim() == string.Empty)
                    {
                        lis.client.control.MessageDialog.Show("文件名不能为空！", OfficeMessage.BASE_TITLE);
                        return;
                    }

                    try
                    {
                        if (UserInfo.GetSysConfigValue("Office_dutyTemplateSelect") == "是")
                        {
                            gdPlan.DataSource = dtUserTemp;
                            gdPlan.ExportToXls(ofd.FileName);
                            dtUserTemp.Rows.RemoveAt(dtUserTemp.Rows.Count - 1);
                            gdPlan.DataSource = dtUserTemp;
                        }
                        else
                        {
                            gdPlan.ExportToXls(ofd.FileName);
                        }
                        lis.client.control.MessageDialog.Show("导出成功！", OfficeMessage.BASE_TITLE);
                    }
                    catch (Exception)
                    {
                    }
                }

            }

        }
        #endregion

        #region 打印个人排班表
        /// <summary>
        /// 打印个人排班表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar_OnBtnSinglePrintClicked(object sender, EventArgs e)
        {
            if (bandedGridView.SelectedRowsCount < 1)
            {
                lis.client.control.MessageDialog.Show(OfficeMessage.BASE_SELECT_NULL, OfficeMessage.BASE_TITLE);
                return;
            }
            DataRow dr = bandedGridView.GetDataRow(bandedGridView.FocusedRowHandle);
            EntityDCLPrintParameter para = new EntityDCLPrintParameter();
            para.ReportCode = "dutyplan";
            if (UserInfo.GetSysConfigValue("Office_dutyTemplateSelect") != "是")
            {
                para.CustomParameter.Add("ShiftStartDate", timeFrom.Text);
                para.CustomParameter.Add("ShiftEndDate", timeTo.Text);
                para.CustomParameter.Add("ShiftUserId", dr["ID"].ToString());

            }
            else
            {
                DateTime now = DateTime.Now;
                DateTime from = new DateTime(now.Year, now.Month, 1);
                DateTime to = from.AddMonths(1).AddDays(-1);
                para.CustomParameter.Add("DetailStartDate", from);
                para.CustomParameter.Add("DatailEndDate", to);
                para.CustomParameter.Add("DetailUserId", dr["ID"].ToString());
            }
            try
            {
                DCLReportPrint.PrintPreview(para);
            }
            catch (ReportNotFoundException ex1)
            {
                lis.client.control.MessageDialog.Show(ex1.MSG);
            }
            catch (Exception ex2)
            {

            }
        }
        #endregion

        #region 复制排班计划
        /// <summary>
        /// 复制排班计划
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar_BtnCopyClick(object sender, EventArgs e)
        {
            FrmOfficePlanCopy frm = new FrmOfficePlanCopy();
            bool blTemplate = UserInfo.GetSysConfigValue("Office_dutyTemplateSelect") != "是";
            if (blTemplate)
            {
                frm.sFrom.Text = timeFrom.Text;
                frm.sTo.Text = timeTo.Text;

                frm.dFrom.Text = Convert.ToDateTime(timeTo.Text).AddDays(1).ToString("yyyy-MM-dd");
            }
            else
            {
                DateTime from = new DateTime(date.Year, date.Month, 1);
                DateTime to = from.AddMonths(1).AddDays(-1);
                frm.sFrom.Text = from.ToString("yyyy-MM-dd");
                frm.sTo.Text = to.ToString("yyyy-MM-dd");
                frm.dFrom.Text = to.AddDays(1).ToString("yyyy-MM-dd");
            }
            frm.TypeID = this.cboDictType.valueMember;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                firstLoad = true;
                if (blTemplate)
                {
                    timeFrom.Text = frm.dFrom.Text;
                    timeTo.Text = frm.dTo.Text;
                    LoadDutyInfo(this.cboDictType.valueMember);
                }
                else
                {
                    LoadDutyTemplateInfo(this.cboDictType.valueMember, dtemplate_id, Convert.ToDateTime(frm.dFrom.Text));
                }
                firstLoad = false;
            }
        }
        #endregion

        #region 关闭时写入数据（按日期排班界面）
        /// <summary>
        /// 关闭时写入数据（按日期排班界面）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popEidtPlan_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            string user = "";
            for (int i = 0; i < ckePlan.CheckedItems.Count; i++)
            {
                user += "," + ckePlan.CheckedItems[i].ToString();
            }
            DataTable dt = (DataTable)gdPlan.DataSource;
            if (user != "")
            {
                dt.Rows[bandedGridView.FocusedRowHandle][bandedGridView.FocusedColumn.FieldName] = user.Substring(1);
            }
            else
            {
                dt.Rows[bandedGridView.FocusedRowHandle][bandedGridView.FocusedColumn.FieldName] = "";
            }
        }
        #endregion

        #region 如果数据内容为空时

        /// <summary>
        /// 如果数据内容为空时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bandedGridView_CustomDrawEmptyForeground(object sender, DevExpress.XtraGrid.Views.Base.CustomDrawEventArgs e)
        {
            if (bandedGridView.RowCount == 0)
            {
                string str = "没有你所想要的数据！请查看你所在的部门的班次信息！或者重新选择日期刷新！";
                Font f = new Font("宋体", 10, FontStyle.Bold);

                Rectangle r = new Rectangle(e.Bounds.Left + 5, e.Bounds.Top + 5, e.Bounds.Width - 5, e.Bounds.Height - 5);
                e.Graphics.DrawString(str, f, Brushes.Black, r);
            }

        }

        #endregion

        #region 得到本周第一天
        private DateTime GetWeekFirstDayMon(DateTime datetime)
        {
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);
            weeknow = weeknow == 0 ? (7 - 1) : (weeknow - 1);
            int daydiff = (-1) * weeknow;
            string FirstDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(FirstDay);
        }

        #endregion

        #region 按周或者月来显示

        private void btnThisWeek_Click(object sender, EventArgs e)
        {
            timeFrom.EditValue = GetWeekFirstDayMon(DateTime.Now);
            timeTo.EditValue = GetWeekFirstDayMon(DateTime.Now).AddDays(6);
        }

        private void btnPreWeek_Click(object sender, EventArgs e)
        {
            timeFrom.EditValue = GetWeekFirstDayMon(Convert.ToDateTime(timeFrom.EditValue).AddDays(-7));
            timeTo.EditValue = GetWeekFirstDayMon(Convert.ToDateTime(timeFrom.EditValue)).AddDays(6);
        }

        private void btnNextWeek_Click(object sender, EventArgs e)
        {
            timeFrom.EditValue = GetWeekFirstDayMon(Convert.ToDateTime(timeFrom.EditValue).AddDays(7));
            timeTo.EditValue = GetWeekFirstDayMon(Convert.ToDateTime(timeFrom.EditValue)).AddDays(6);
        }

        private void btnPreMonth_Click(object sender, EventArgs e)
        {
            if (UserInfo.GetSysConfigValue("Office_dutyTemplateSelect") != "是")
            {
                timeFrom.EditValue = Convert.ToDateTime(timeFrom.EditValue).AddMonths(-1).ToString("yyyy-MM-01");
                timeTo.EditValue = Convert.ToDateTime(timeFrom.EditValue).AddMonths(1).AddDays(-1);
            }
            else if (isTemplate)
            {
                date = DateTime.Now.AddMonths(-1);
                LoadDutyTemplateInfo(this.cboDictType.valueMember, dtemplate_id, date);
            }
        }

        private void btnThisMonth_Click(object sender, EventArgs e)
        {
            if (UserInfo.GetSysConfigValue("Office_dutyTemplateSelect") != "是")
            {
                timeFrom.EditValue = DateTime.Now.ToString("yyyy-MM-01");
                timeTo.EditValue = Convert.ToDateTime(timeFrom.EditValue).AddMonths(1).AddDays(-1);
            }
            else if (isTemplate)
            {
                date = DateTime.Now;
                LoadDutyTemplateInfo(this.cboDictType.valueMember, dtemplate_id, date);
            }
        }

        private void btnNextMonth_Click(object sender, EventArgs e)
        {
            if (UserInfo.GetSysConfigValue("Office_dutyTemplateSelect") != "是")
            {
                timeFrom.EditValue = Convert.ToDateTime(timeFrom.EditValue).AddMonths(1).ToString("yyyy-MM-01");
                timeTo.EditValue = Convert.ToDateTime(timeFrom.EditValue).AddMonths(1).AddDays(-1);
            }
            else if (isTemplate)
            {
                date = DateTime.Now.AddMonths(1);
                LoadDutyTemplateInfo(this.cboDictType.valueMember, dtemplate_id, date);
            }
        }

        #endregion

        private void ckePlan_ItemChecking(object sender, DevExpress.XtraEditors.Controls.ItemCheckingEventArgs e)
        {
            if (dtBC == null && ckePlan.Items[e.Index].CheckState == CheckState.Checked) return;
            string name = ckePlan.Items[e.Index].Value.ToString();

            DataRow[] rows = dtBC.Select(string.Format("duty_name='{0}'", name));
            if (rows.Length > 0)
            {
                DateTime rowStart = Convert.ToDateTime(rows[0]["duty_sdate"]);
                DateTime rowEnd = Convert.ToDateTime(rows[0]["duty_edate"]);
                for (int i = 0; i < ckePlan.Items.Count; i++)
                {
                    if (i != e.Index && ckePlan.Items[i].CheckState == CheckState.Checked)
                    {
                        DataRow[] rowOthers = dtBC.Select(string.Format("duty_name='{0}'", ckePlan.Items[i].Value));

                        if (rowOthers.Length > 0)
                        {
                            DateTime compareStart = Convert.ToDateTime(rowOthers[0]["duty_sdate"]);
                            DateTime compareEnd = Convert.ToDateTime(rowOthers[0]["duty_edate"]);

                            if ((rowStart == compareStart && rowEnd == compareEnd)
                                || (rowStart > compareStart && rowStart < compareEnd)
                                || (rowEnd > compareStart && rowEnd < compareEnd)
                                || (rowStart < compareStart && rowEnd >= compareEnd)
                                || (rowStart > compareStart && rowEnd < compareEnd))
                            {
                                ckePlan.Items[i].CheckState = CheckState.Unchecked;
                            }

                        }
                    }
                }
            }

        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {

            if (cboDictType.valueMember == null && !firstLoad)
            {
                return;
            }
            if (cboDictType.valueMember == "")
            {
                return;
            }

            string strType = cboDictType.valueMember;
            LoadDutyInfo(strType);

        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (listUser == null) return;
            if (!string.IsNullOrEmpty(txtSearch.EditValue.ToString()))
            {
                string searchStr = txtSearch.EditValue.ToString();
                bindingSourceUser.DataSource = listUser.Where(w => w.Checked.ToString().Contains("true") ||
                    w.UserName != null && w.UserName.Contains(searchStr) ||
                    w.LoginId != null && w.LoginId.Contains(searchStr) ||
                    w.UserType != null && w.UserType.Contains(searchStr) ||
                    w.WbCode != null && w.WbCode.Contains(searchStr) ||
                    w.PyCode != null && w.PyCode.Contains(searchStr)).ToList();

            }
            else
            {
                bindingSourceUser.DataSource = listUser;
            }
        }

        private void bandedGridView_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle % 2 == 0)
            {
                e.Appearance.BackColor = Color.LightGray;
            }
        }

        bool rowChange = false;
        bool columnChange = false;
        int lastRowHandle = 0;
        string lastColumnFieldName = "";
        int rowHandle = 0;
        string columnFieldName = "";
        private void bandedGridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            rowChange = true;
            lastRowHandle = rowHandle;
            rowHandle = bandedGridView.FocusedRowHandle;
        }

        private void bandedGridView_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            columnChange = true;
            lastColumnFieldName = columnFieldName;
            columnFieldName = bandedGridView.FocusedColumn != null ? bandedGridView.FocusedColumn.FieldName : "";
        }

        private void popEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Close)
            {
                this.popEdit.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.popEdit_Closed);
            }
            else {
                this.popEdit.Closed -= new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.popEdit_Closed);
            }
        }
    }
}
