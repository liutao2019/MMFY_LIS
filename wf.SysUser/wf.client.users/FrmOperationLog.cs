using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using dcl.client.frame;
using dcl.client.wcf;
using dcl.entity;

namespace dcl.client.users
{
    public partial class FrmOperationLog : FrmCommon
    {
        List<EntitySysOperationLog> listLog = new List<EntitySysOperationLog>();
        ProxySysOperationLog proxy = new ProxySysOperationLog();
        public FrmOperationLog()
        {
            InitializeComponent();
        }

        string userTypes = string.Empty;

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmOperationLog_Load(object sender, EventArgs e)
        {
            //初始化按钮
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnSearch.Name, sysToolBar1.BtnReset.Name });

            //重置搜索条件
            sysToolBar1_BtnResetClick(null, null);

            //设置根据pat_id号来查看记录的pat_id列表中的pat_id的可见性
            this.reportStateMonitor1.setPatIdVisible(false);

            foreach (EntityUserLab lab in UserInfo.listUserLab)
            {
                userTypes += "'" + lab.LabId+ "',";
            }
            if (userTypes != "")
            {
                userTypes = userTypes.TrimEnd(',');
                userTypes = " (" + userTypes + ") ";
            }
            txtType_onBeforeFilter();
            txtInstrmt_onBeforeFilter();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnSearchClicked(object sender, EventArgs e)
        {
  

            List<EntityPidReportMain> listPatients = proxy.Service.GetPatients(timeFrom.Text, timeTo.Text, txtIdType.valueMember.Trim(), txtType.valueMember, txtPatID.Text.Trim(), txtPatName.Text.Trim(), txtInstrmt.valueMember.Trim(), txtPat_chk_code.Text.Trim());

            //*************************************************************************************//
            //有pat_id的也要加进右边pat_id的操作记录
            if (listPatients != null && listPatients.Count > 0)
            {
                this.reportStateMonitor1.LoadData(listPatients[0].RepId.ToString());
            }
            else
            {
                if ((txtPatID.Text.Trim() != "") || (txtPatName.Text.Trim() != ""))
                {
                    this.reportStateMonitor1.LoadData(GetPatId());
                }
            }
            gdPatients.DataSource = null;
            gdPatients.DataSource = listPatients;
            gdPatients.RefreshDataSource();
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_BtnResetClick(object sender, EventArgs e)
        {
            //初始化时间段
            timeFrom.EditValue = DateTime.Now.AddDays(-3);
            timeTo.EditValue = DateTime.Now;

            txtIdType.displayMember = "";
            txtIdType.valueMember = "";

            txtInstrmt.displayMember = "";
            txtInstrmt.valueMember = "";

            txtPatID.Text = "";
            txtPatName.Text = "";
        }

        /// <summary>
        /// 选择行数据时显示修改历史
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            string patId = "";
            if (gridView1.FocusedRowHandle > -1)
            {
                //EntityPatients patients = (EntityPatients)bsPatients.Current;
                EntityPidReportMain patients = gridView1.GetFocusedRow() as EntityPidReportMain;
                patId = patients.RepId;
                this.reportStateMonitor1.LoadData(patId);
            }
            //显示修改历史
            LoadOperationLog(patId, true);
        }

        /// <summary>
        /// 查询历史修改记录并分组
        /// </summary>
        /// <param name="patId"></param>
        /// <param name="showSearchPanel"></param>
        public void LoadOperationLog(string patId, bool showSearchPanel)
        {
            //隐藏或显示搜索面板
            plCommand.Visible = showSearchPanel;

            //清空界面
            tabControl.TabPages.Clear();
            gdSysOperationLog.DataSource = null;
            gvLog.Columns.Clear();
            if (patId != "")
            {
                EntityLogQc qc = new EntityLogQc();
                qc.Operatkey = patId;
                qc.OperatModule = "病人资料";
                listLog = proxy.Service.GetOperationLog(qc);
                List<string> list = listLog.Select(w => w.OperatGroup).ToList();
                list = list.Distinct().ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    tabControl.TabPages.Add(list[i]);
                }
            }
        }

        /// <summary>
        /// 显示修改历史
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            gvLog.Bands.Clear();
            gvLog.Columns.Clear();
            gdSysOperationLog.DataSource = null;
            if (tabControl.SelectedTabPageIndex > -1)
            {
                DevExpress.XtraTab.XtraTabPage page = tabControl.SelectedTabPage;
                string group = page.Text;
                List<EntitySysOperationLog> list = listLog.Where(w => w.OperatGroup == group).ToList();
                //去重
                List<string> listResult = list.Select(w => w.OperatObject).Distinct().ToList();
                DataTable dtResult = new DataTable();
                dtResult.Columns.Add("OperationContent");
                for (int i = 0; i < listResult.Count; i++)
                {
                    dtResult.Rows.Add();
                    dtResult.Rows[i]["OperationContent"] = listResult[i];
                }
                DevExpress.XtraGrid.Views.BandedGrid.GridBand contentBand = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
                contentBand.AppearanceHeader.Options.UseBackColor = true;
                contentBand.AppearanceHeader.BackColor = this.BackColor;
                contentBand.OptionsBand.AllowMove = false;
                contentBand.MinWidth = 160;
                contentBand.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                contentBand.Caption = "字段\\批次";

                DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colContent = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
                colContent.AppearanceHeader.Options.UseBackColor = true;
                colContent.AppearanceHeader.BackColor = this.BackColor;
                colContent.Visible = true;
                colContent.OptionsColumn.AllowEdit = false;
                colContent.FieldName = "OperationContent";
                colContent.AppearanceCell.Options.UseBackColor = true;
                colContent.AppearanceCell.BackColor = this.BackColor;

                contentBand.Columns.Add(colContent);
                gvLog.Columns.Add(colContent);
                gvLog.Bands.Add(contentBand);
                //去重
                // List<EntitySysOperationLog> listDictLog = list.GroupBy(x => new { x.OperatObject, x.OperatUserId, x.OperatServername }).Select(x => x.First()).ToList();
                List<EntitySysOperationLog> listDictLog = (from c in list
                                                           group c by new
                                                           {
                                                               c.OperatDate,
                                                               c.OperatUserId,
                                                               c.OperatServername
                                                           } into grp
                                                           select grp.First()).ToList();
                for (int i = 0; i < listDictLog.Count; i++)
                {
                    string date = listDictLog[i].OperatDate.ToString("yyyy-MM-dd HH:mm:ss");

                    #region 生成控件列表

                    Color color = Color.White;
                    if (i % 2 == 1)
                    {
                        color = Color.LightYellow;
                    }

                    DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
                    gridBand.AppearanceHeader.Options.UseBackColor = true;
                    gridBand.AppearanceHeader.BackColor = color;
                    gridBand.OptionsBand.AllowMove = false;
                    gridBand.Caption = date;

                    DevExpress.XtraGrid.Views.BandedGrid.GridBand operationTypeBand = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
                    operationTypeBand.AppearanceHeader.Options.UseBackColor = true;
                    operationTypeBand.AppearanceHeader.BackColor = color;
                    operationTypeBand.Caption = "类型";

                    DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colOperationType = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
                    colOperationType.AppearanceHeader.Options.UseBackColor = true;
                    colOperationType.AppearanceHeader.BackColor = color;
                    colOperationType.Visible = true;
                    colOperationType.FieldName = date + "OperationType";
                    colOperationType.Width = 50;
                    colOperationType.OptionsColumn.FixedWidth = true;
                    colOperationType.OptionsColumn.AllowEdit = false;
                    colOperationType.AppearanceCell.Options.UseBackColor = true;
                    colOperationType.AppearanceCell.BackColor = color;

                    operationTypeBand.Columns.Add(colOperationType);
                    gridBand.Children.Add(operationTypeBand);
                    gvLog.Columns.Add(colOperationType);

                    DevExpress.XtraGrid.Views.BandedGrid.GridBand operatorIDBand = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
                    operatorIDBand.AppearanceHeader.Options.UseBackColor = true;
                    operatorIDBand.AppearanceHeader.BackColor = color;
                    operatorIDBand.Caption = "操作人";

                    DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colOperatorID = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
                    colOperatorID.AppearanceHeader.Options.UseBackColor = true;
                    colOperatorID.AppearanceHeader.BackColor = color;
                    colOperatorID.Visible = true;
                    colOperatorID.FieldName = date + "OperatorID";
                    colOperatorID.Width = 80;
                    colOperatorID.OptionsColumn.FixedWidth = true;
                    colOperatorID.OptionsColumn.AllowEdit = false;
                    colOperatorID.AppearanceCell.Options.UseBackColor = true;
                    colOperatorID.AppearanceCell.BackColor = color;

                    operatorIDBand.Columns.Add(colOperatorID);
                    gridBand.Children.Add(operatorIDBand);
                    gvLog.Columns.Add(colOperatorID);

                    DevExpress.XtraGrid.Views.BandedGrid.GridBand descriptionBand = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
                    descriptionBand.AppearanceHeader.Options.UseBackColor = true;
                    descriptionBand.AppearanceHeader.BackColor = color;
                    descriptionBand.Caption = "数据";

                    DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colDescription = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
                    colDescription.AppearanceHeader.Options.UseBackColor = true;
                    colDescription.AppearanceHeader.BackColor = color;
                    colDescription.Visible = true;
                    colDescription.FieldName = date + "Description";
                    colDescription.Width = 60;
                    //colDescription.OptionsColumn.FixedWidth = true;
                    colDescription.OptionsColumn.AllowEdit = false;
                    colDescription.AppearanceCell.Options.UseBackColor = true;
                    colDescription.AppearanceCell.BackColor = color;

                    descriptionBand.Columns.Add(colDescription);
                    gridBand.Children.Add(descriptionBand);
                    gvLog.Columns.Add(colDescription);

                    gvLog.Bands.Add(gridBand);
                    #endregion
                    if (!dtResult.Columns.Contains(date + "OperationType"))
                    {
                        //生成数据表
                        dtResult.Columns.Add(date + "OperationType");
                        dtResult.Columns.Add(date + "OperatorID");
                        dtResult.Columns.Add(date + "Description");

                        for (int j = 0; j < dtResult.Rows.Count; j++)
                        {
                            string content = dtResult.Rows[j]["OperationContent"].ToString();
                            //string filer = "Group ='" + group + "' and OperationTime = '" + Convert.ToDateTime(dtDate.Rows[i]["OperationTime"]).ToString("yyyy-MM-dd HH:mm:ss.fff") + "' and OperationContent='" + content.Replace("'", "''") + "'";
                            List<EntitySysOperationLog> listOper = listLog.Where(w => w.OperatGroup == group && w.OperatDate == listDictLog[i].OperatDate && w.OperatObject == content.Replace("'", "''")).ToList();
                            if (listOper.Count > 0)
                            {
                                dtResult.Rows[j][date + "OperationType"] = listOper[0].OperatAction;
                                dtResult.Rows[j][date + "OperatorID"] = listOper[0].OperatUserName;
                                dtResult.Rows[j][date + "Description"] = listOper[0].OperatContent;
                            }
                        }
                    }
              
                }
                gdSysOperationLog.DataSource = dtResult;
            }
        }
    

        public string GetPatId()
        {
            string PatId = string.Empty;
            string PatName = string.Empty;

            PatId = txtPatID.Text.Trim();
            PatName = txtPatName.Text.Trim();
            string Pat_id = string.Empty;

            //若还没删除，即在patients表中还能找到pat_id，则现在patients表中找到pat_id
            string findPatId = string.Empty;
            Pat_id = proxy.Service.GetPatId(timeFrom.Text.Trim(), timeTo.Text.Trim(), PatId, PatName);
            if (Pat_id != string.Empty)
            {
                return Pat_id;
            }
            Pat_id = proxy.Service.GetDeletePatId(timeFrom.Text.Trim(), timeTo.Text.Trim(), PatId, PatName);

            return Pat_id;

        }

        private void txtType_onBeforeFilter()
        {
            List<EntityDicPubProfession> TypeList =  this.txtType.getDataSource();
            //由权限控制物理组别
            if (this.userTypes != "" && UserInfo.isAdmin == false)
            {
                TypeList = TypeList.Where(i => userTypes.Contains(i.ProId)).ToList();
            }
            else
            {
                if (UserInfo.isAdmin == false)
                    TypeList = new List<EntityDicPubProfession>();
            }

            this.txtType.SetFilter(TypeList);
        }

        private void txtInstrmt_onBeforeFilter()
        {
            List<EntityDicInstrument> itrList = this.txtInstrmt.getDataSource();
            string strIns = "(" + UserInfo.sqlUserItrs + ")";
            if (this.txtType.valueMember == "" || this.txtType.valueMember == null)
            {
                if (UserInfo.sqlUserItrs != "" && UserInfo.sqlUserItrs != "-1")
                {
                    itrList = itrList.Where(i => strIns.Contains(i.ItrId)).ToList();
                    //strFilter += "and itr_id in " + strIns + "";
                }
                else
                {
                    if (UserInfo.isAdmin == false)
                        itrList = new List<EntityDicInstrument>();
                }
            }
            else
            {
                if (UserInfo.sqlUserItrs == "-1")
                    itrList = itrList.Where(i => i.ItrLabId == this.txtType.valueMember).ToList();
                else
                   itrList = itrList.Where(i => i.ItrLabId == this.txtType.valueMember && strIns.Contains(i.ItrId)).ToList();
            }
            this.txtInstrmt.SetFilter(itrList);
        }
    }
}
