using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors;
using dcl.client.frame.runsetting;
using dcl.client.frame;
using dcl.root.logon;
using DevExpress.XtraGrid;
using dcl.client.result.CommonPatientInput;
using dcl.client.result.Interface;
using lis.client.control;
using dcl.client.common;

namespace dcl.client.result
{
    public partial class FrmPatPanelConfig : FrmCommon
    {
        /// <summary>
        /// 式样窗体对象
        /// </summary>
        IPatPanelConfig pForm = null;

        /// <summary>
        /// 父窗体名称
        /// </summary>
        string pFormName = string.Empty;


        DataTable dtPatFunction = null;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="frmPatient"></param>
        public FrmPatPanelConfig(IPatPanelConfig frmPatient)
        {
            InitializeComponent();

            pForm = frmPatient;
            if (pForm != null)
            {
                pFormName = pForm.GetType().Name;
                this.Text = "面板设置(" + pForm.Text + ")";
            }
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FromPatListGridConfig_Load(object sender, EventArgs e)
        {
            //if (pForm is FrmPatEnter)//窗体为普通病人录入时显示病人结果配置页
            //{
            //    tabPatResult.PageVisible = true;
            //}
            //else
            //{
            //    tabPatResult.PageVisible = false;
            //}
            tabPatResult.PageVisible = true;
            //加载式样
            LoadSetting();

            //当前用户为管理员时显示加载程序默认/保存系统默认
            if (!UserInfo.isAdmin)
            {
                this.btnLoadProgrmDefault.Visible = false;
                this.btnSaveAsSystem.Visible = false;
            }

            if (UserInfo.GetSysConfigValue("Lab_EableCustomeShortCut") == "是")
            {
                tabShortCut.PageVisible = true;
            }
            else
            {
                tabShortCut.PageVisible = false;
            }
        }

        /// <summary>
        /// 加载设置
        /// </summary>
        /// <param name="setting"></param>
        public void LoadSetting(PatInputRuntimeSetting setting)
        {
            #region 病人列表
            this.colorAuditedBack.Color = setting.PatListPanel.BackColorAudited;
            this.colorNormalBack.Color = setting.PatListPanel.BackColorNormal;
            this.colorPrintedBack.Color = setting.PatListPanel.BackColorPrinted;
            this.colorReportedBack.Color = setting.PatListPanel.BackColorReported;
            this.preReportBack.Color = setting.PatListPanel.BackColorPreReported;

            if (setting.PatListPanel.BackColorUrgent == Color.Empty)//已查看危急值背景色
                this.colorUrgentBack.Color = Color.FromArgb(192, 255, 255);
            else
                this.colorUrgentBack.Color = setting.PatListPanel.BackColorUrgent;

            if (setting.PatListPanel.BackColorNUrgent == Color.Empty)//未查看危急值背景色
                this.colorNUrgentBack.Color = Color.FromArgb(192, 255, 255);
            else
                this.colorNUrgentBack.Color = setting.PatListPanel.BackColorNUrgent;

            if (setting.PatListPanel.BackColorUrgentRecord == Color.Empty)//已登记危急值记录背景色
                this.colorUrgentRecord.Color = Color.Transparent;
            else
                this.colorUrgentRecord.Color = setting.PatListPanel.BackColorUrgentRecord;

            if (setting.PatListPanel.BackColorNUrgentRecord == Color.Empty)//未登记危急值记录背景色
                this.colorNUrgentRecord.Color = Color.Transparent;
            else
                this.colorNUrgentRecord.Color = setting.PatListPanel.BackColorNUrgentRecord;

            if (setting.PatListPanel.BackColorPreReported == Color.Empty)//未登记危急值记录背景色
                this.preReportBack.Color = Color.Transparent;
            else
                this.preReportBack.Color = setting.PatListPanel.BackColorPreReported;


            this.colorAuditedFore.Color = setting.PatListPanel.ForeColorAudited;
            this.colorNormalFore.Color = setting.PatListPanel.ForeColorNormal;
            this.colorPrintedFore.Color = setting.PatListPanel.ForeColorPrinted;
            this.colorReportedFore.Color = setting.PatListPanel.ForeColorReported;
            this.preReportFore.Color = setting.PatListPanel.ForeColorPreReported;


            this.gridControlPatList.DataSource = setting.PatListPanel.GridColSetting;

            #endregion

            #region 病人结果
            this.gridControlResultList.DataSource = setting.PatResultPanel.GridColSetting;

            this.colorBackGreaterRefValue.Color = setting.PatResultPanel.BackColorGreaterThanRef;
            this.colorBackGreaterThanMax.Color = setting.PatResultPanel.BackColorGreaterThanMax;
            this.colorBackGreaterThanPan.Color = setting.PatResultPanel.BackColorGreaterThanPan;

            this.colorBackLowerRefValue.Color = setting.PatResultPanel.BackColorLowerThanRef;
            this.colorBackLowerThanMin.Color = setting.PatResultPanel.BackColorLowerThanMin;
            this.colorBackLowerThanPan.Color = setting.PatResultPanel.BackColorLowerThanPan;

            this.colorForeGreaterRefValue.Color = setting.PatResultPanel.ForeColorGreaterThanRef;
            this.colorForeGreaterThanMax.Color = setting.PatResultPanel.ForeColorGreaterThanMax;
            this.colorForeGreaterThanPan.Color = setting.PatResultPanel.ForeColorGreaterThanPan;

            this.colorForeLowerRefValue.Color = setting.PatResultPanel.ForeColorLowerThanRef;
            this.colorForeLowerThanMin.Color = setting.PatResultPanel.ForeColorLowerThanMin;
            this.colorForeLowerThanPan.Color = setting.PatResultPanel.ForeColorLowerThanPan;

            this.chkOrderItmSeq.Checked = setting.PatResultPanel.OrderByItmSeq;

            this.radioGroupPatResultVisibleStyle.EditValue = setting.PatResultPanel.VisibleStyle;
            this.chkEachViewOnlyTwoCombine.Checked = setting.PatResultPanel.EachViewOnlyTwoCombine;

            this.chkSavePatInfoNoNext.Checked = setting.PatResultPanel.SavePatInfoNoNext;
            this.chkSaveInNoNextResult.Checked = setting.PatResultPanel.SaveFocusResultColumn;
            radioGroupPatResultVisibleStyle_SelectedIndexChanged(null, EventArgs.Empty);
            #endregion

            //病人信息
            this.gridControlPatInfo.DataSource = setting.PatInfoPanel.Items;

            gcShortCut.DataSource = setting.PatShortCut.ShortCutSetting;

            dtPatFunction = GetPatFunctionSour();

            string strFunctionSort = ConfigHelper.GetSysConfigValueWithoutLogin("PatFunctionSort");

            if (strFunctionSort.Trim() != string.Empty)
            {
                string[] strFuncSorts = strFunctionSort.Split(',');
                for (int i = 0; i < strFuncSorts.Length; i++)
                {
                    dtPatFunction.Rows[i]["VisibleIndex"] = strFuncSorts[i];
                }
            }

            string strFunctionVisible = ConfigHelper.GetSysConfigValueWithoutLogin("PatFunctionVisible");

            if (strFunctionVisible.Trim() != string.Empty)
            {
                string[] strFuncVisibles = strFunctionVisible.Split(',');
                for (int i = 0; i < strFuncVisibles.Length; i++)
                {
                    dtPatFunction.Rows[i]["Visible"] = strFuncVisibles[i];
                }
            }

            DataView dvPatFunction = new DataView(dtPatFunction);
            dvPatFunction.Sort = "VisibleIndex";

            dtPatFunction = dvPatFunction.ToTable("dtPatFunction");

            gcPatFunction.DataSource = dtPatFunction;
        }

        /// <summary>
        /// 加载设置
        /// </summary>
        public void LoadSetting()
        {
            string userID = UserInfo.loginID;
            PatInputRuntimeSetting setting = PatInputRuntimeSetting.Load(this.pFormName, string.Empty, userID);

            LoadSetting(setting);

        }

        /// <summary>
        /// 获取当前最新设置
        /// </summary>
        /// <returns></returns>
        public PatInputRuntimeSetting GetCurrentSetting()
        {
            PatInputRuntimeSetting setting = new PatInputRuntimeSetting();

            #region 病人列表
            setting.PatListPanel.BackColorAudited = colorAuditedBack.Color;
            setting.PatListPanel.BackColorNormal = colorNormalBack.Color;
            setting.PatListPanel.BackColorPrinted = colorPrintedBack.Color;
            setting.PatListPanel.BackColorReported = colorReportedBack.Color;
            setting.PatListPanel.BackColorPreReported = preReportBack.Color;

            setting.PatListPanel.BackColorUrgent = colorUrgentBack.Color;//已查看危急值背景色
            setting.PatListPanel.BackColorNUrgent = colorNUrgentBack.Color;//未查看危急值背景色

            setting.PatListPanel.BackColorUrgentRecord = colorUrgentRecord.Color;//已登记危急值记录背景色
            setting.PatListPanel.BackColorNUrgentRecord = colorNUrgentRecord.Color;//未登记危急值记录背景色

            setting.PatListPanel.ForeColorAudited = colorAuditedFore.Color;
            setting.PatListPanel.ForeColorNormal = colorNormalFore.Color;
            setting.PatListPanel.ForeColorPrinted = colorPrintedFore.Color;
            setting.PatListPanel.ForeColorReported = colorReportedFore.Color;
            setting.PatListPanel.ForeColorPreReported = preReportFore.Color;



            setting.PatListPanel.GridColSetting = this.gridControlPatList.DataSource as DataTable;
            #endregion

            #region 病人结果
            //病人结果列的自定义
            setting.PatResultPanel.GridColSetting = this.gridControlResultList.DataSource as DataTable;

            setting.PatResultPanel.BackColorGreaterThanRef = this.colorBackGreaterRefValue.Color;
            setting.PatResultPanel.BackColorGreaterThanMax = this.colorBackGreaterThanMax.Color;
            setting.PatResultPanel.BackColorGreaterThanPan = this.colorBackGreaterThanPan.Color;

            setting.PatResultPanel.BackColorLowerThanRef = this.colorBackLowerRefValue.Color;
            setting.PatResultPanel.BackColorLowerThanMin = this.colorBackLowerThanMin.Color;
            setting.PatResultPanel.BackColorLowerThanPan = this.colorBackLowerThanPan.Color;

            setting.PatResultPanel.ForeColorGreaterThanRef = this.colorForeGreaterRefValue.Color;
            setting.PatResultPanel.ForeColorGreaterThanMax = this.colorForeGreaterThanMax.Color;
            setting.PatResultPanel.ForeColorGreaterThanPan = this.colorForeGreaterThanPan.Color;

            setting.PatResultPanel.ForeColorLowerThanRef = this.colorForeLowerRefValue.Color;
            setting.PatResultPanel.ForeColorLowerThanMin = this.colorForeLowerThanMin.Color;
            setting.PatResultPanel.ForeColorLowerThanPan = this.colorForeLowerThanPan.Color;

            setting.PatResultPanel.OrderByItmSeq = this.chkOrderItmSeq.Checked;

            setting.PatResultPanel.VisibleStyle = (int)this.radioGroupPatResultVisibleStyle.EditValue;
            setting.PatResultPanel.EachViewOnlyTwoCombine = chkEachViewOnlyTwoCombine.Checked;

            setting.PatResultPanel.SavePatInfoNoNext = this.chkSavePatInfoNoNext.Checked;
            setting.PatResultPanel.SaveFocusResultColumn = this.chkSaveInNoNextResult.Checked;
            #endregion

            //病人信息
            setting.PatInfoPanel.Items = gridControlPatInfo.DataSource as DataTable;


            setting.PatShortCut.ShortCutSetting = gcShortCut.DataSource as DataTable;
            return setting;
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                PatInputRuntimeSetting setting = GetCurrentSetting();
                PatInputRuntimeSetting.SaveUser(this.pFormName, UserInfo.loginID, setting);
                SavePatFunction();

                ApplySetting(setting);
                lis.client.control.MessageDialog.Show("保存成功", "提示");
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, string.Format("保存用户配置失败，用户ID={0}", UserInfo.loginID), ex.ToString());
                lis.client.control.MessageDialog.Show("保存失败", "提示", MessageBoxButtons.OK);
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 加载系统默认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadDefault_Click(object sender, EventArgs e)
        {
            if (lis.client.control.MessageDialog.Show("加载系统默认将会把现有的覆盖，是否继续？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                PatInputRuntimeSetting setting = PatInputRuntimeSetting.LoadSystem(this.pFormName);

                if (setting == null)
                {
                    setting = new PatInputRuntimeSetting();
                }

                LoadSetting(setting);
            }
        }

        /// <summary>
        /// 点击应用设置(不保存)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApply_Click(object sender, EventArgs e)
        {
            PatInputRuntimeSetting setting = GetCurrentSetting();
            ApplySetting(setting);

        }

        /// <summary>
        /// 应用设置
        /// </summary>
        /// <param name="setting"></param>
        private void ApplySetting(PatInputRuntimeSetting setting)
        {
            if (pForm != null)
            {
                pForm.ApplySetting(setting);
                this.Focus();
            }
        }

        /// <summary>
        /// 保存为系统默认(管理员)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveAsSystem_Click(object sender, EventArgs e)
        {
            if (lis.client.control.MessageDialog.Show("此操作将会把现有的系统默认覆盖，是否继续？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    PatInputRuntimeSetting setting = GetCurrentSetting();
                    PatInputRuntimeSetting.SaveSystem(this.pFormName, setting);

                    lis.client.control.MessageDialog.Show("保存成功", "提示");
                }
                catch (Exception ex)
                {
                    Logger.WriteException(this.GetType().Name, string.Format("保存系统默认配置失败，用户ID={0}", UserInfo.loginID), ex.ToString());
                    lis.client.control.MessageDialog.Show("保存失败", "提示", MessageBoxButtons.OK);
                }
            }
        }

        /// <summary>
        /// 加载程序默认(管理员)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadProgrmDefault_Click(object sender, EventArgs e)
        {
            if (lis.client.control.MessageDialog.Show("此操作将会把现有的覆盖，是否继续？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                LoadSetting(new PatInputRuntimeSetting());
            }
        }

        #region 病人列表网格配置

        /// <summary>
        /// 上移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPatListUp_Click(object sender, EventArgs e)
        {
            DataRow current = this.gridViewPatList.GetFocusedDataRow();
            DataTable dt = this.gridControlPatList.DataSource as DataTable;

            int currentRowIndex = this.gridViewPatList.FocusedRowHandle;
            if (current != null && currentRowIndex > 0)
            {
                DataRow drPrev = dt.Rows[currentRowIndex - 1];
                current["VisibleIndex"] = (int)current["VisibleIndex"] - 1;
                drPrev["VisibleIndex"] = (int)drPrev["VisibleIndex"] + 1;

                DataRow drtemp = dt.NewRow();

                drtemp.ItemArray = drPrev.ItemArray;

                drPrev.ItemArray = current.ItemArray;

                current.ItemArray = drtemp.ItemArray;

                this.gridViewPatList.MovePrev();
            }
        }

        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPatListDown_Click(object sender, EventArgs e)
        {
            DataRow current = this.gridViewPatList.GetFocusedDataRow();
            DataTable dt = this.gridControlPatList.DataSource as DataTable;

            int currentRowIndex = this.gridViewPatList.FocusedRowHandle;
            if (current != null && currentRowIndex < (dt.Rows.Count - 1))
            {
                DataRow drNext = dt.Rows[currentRowIndex + 1];
                current["VisibleIndex"] = (int)current["VisibleIndex"] + 1;
                drNext["VisibleIndex"] = (int)drNext["VisibleIndex"] - 1;

                DataRow drtemp = dt.NewRow();

                drtemp.ItemArray = drNext.ItemArray;

                drNext.ItemArray = current.ItemArray;

                current.ItemArray = drtemp.ItemArray;

                this.gridViewPatList.MoveNext();
            }
        }
        #endregion

        /// <summary>
        /// RadioGroup病人结果显示方式改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioGroupPatResultVisibleStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((int)this.radioGroupPatResultVisibleStyle.EditValue == 1)
            {
                this.chkEachViewOnlyTwoCombine.Enabled = true;
            }
            else
            {
                this.chkEachViewOnlyTwoCombine.Enabled = false;
            }
        }

        private void gridViewPatInfo_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "NextToSave")
            {
                if (Convert.ToBoolean(e.Value) == true)
                {
                    int rowIndex = e.RowHandle;

                    DataRow drCurrent = this.gridViewPatInfo.GetDataRow(e.RowHandle);
                    string currentControlName = drCurrent["ControlName"].ToString();

                    DataTable dt = this.gridControlPatInfo.DataSource as DataTable;
                    foreach (DataRow item in dt.Rows)
                    {
                        if (item["ControlName"].ToString() != currentControlName)
                        {
                            item["NextToSave"] = false;
                        }
                    }
                }
            }
            else if (e.Column.FieldName == "FocusOnAddNew")
            {
                if (Convert.ToBoolean(e.Value) == true)
                {
                    int rowIndex = e.RowHandle;

                    DataRow drCurrent = this.gridViewPatInfo.GetDataRow(e.RowHandle);
                    string currentControlName = drCurrent["ControlName"].ToString();

                    DataTable dt = this.gridControlPatInfo.DataSource as DataTable;
                    foreach (DataRow item in dt.Rows)
                    {
                        if (item["ControlName"].ToString() != currentControlName)
                        {
                            item["FocusOnAddNew"] = false;
                        }
                    }
                }
            }
        }

        private void btnPatFunctionUp_Click(object sender, EventArgs e)
        {
            DataRow current = this.gvPatFunction.GetFocusedDataRow();
            DataTable dt = this.gcPatFunction.DataSource as DataTable;

            int currentRowIndex = this.gvPatFunction.FocusedRowHandle;
            if (current != null && currentRowIndex > 0)
            {
                DataRow drPrev = dt.Rows[currentRowIndex - 1];

                //current["VisibleIndex"] = Convert.ToInt32(current["VisibleIndex"]) - 1;
                //drPrev["VisibleIndex"] = Convert.ToInt32(drPrev["VisibleIndex"]) + 1;


                object sort = current["VisibleIndex"];
                current["VisibleIndex"] = drPrev["VisibleIndex"];
                drPrev["VisibleIndex"] = sort;


                DataRow drtemp = dt.NewRow();

                drtemp.ItemArray = drPrev.ItemArray;

                drPrev.ItemArray = current.ItemArray;

                current.ItemArray = drtemp.ItemArray;

                this.gvPatFunction.MovePrev();
            }
        }

        private void btnPatFunctionDown_Click(object sender, EventArgs e)
        {
            DataRow current = this.gvPatFunction.GetFocusedDataRow();
            DataTable dt = this.gcPatFunction.DataSource as DataTable;

            int currentRowIndex = this.gvPatFunction.FocusedRowHandle;
            if (current != null && currentRowIndex < (dt.Rows.Count - 1))
            {
                DataRow drNext = dt.Rows[currentRowIndex + 1];

                object sort = current["VisibleIndex"];
                current["VisibleIndex"] = drNext["VisibleIndex"];
                drNext["VisibleIndex"] = sort;

                DataRow drtemp = dt.NewRow();

                drtemp.ItemArray = drNext.ItemArray;

                drNext.ItemArray = current.ItemArray;

                current.ItemArray = drtemp.ItemArray;

                this.gvPatFunction.MoveNext();
            }

        }

        private void SavePatFunction()
        {
            if (dtPatFunction != null)
            {
                string strPatFunctionSort = string.Empty;
                string strPatFunctionVisible = string.Empty;

                DataView dvPatFunction = new DataView(dtPatFunction.Copy());
                dvPatFunction.Sort = "ID";

                for (int i = 0; i < dvPatFunction.Count; i++)
                {
                    strPatFunctionSort += ("," + dvPatFunction[i].Row["VisibleIndex"].ToString());
                    strPatFunctionVisible += ("," + dvPatFunction[i].Row["Visible"].ToString());
                }

                strPatFunctionSort = strPatFunctionSort.Remove(0, 1);
                strPatFunctionVisible = strPatFunctionVisible.Remove(0, 1);

                //dcl.client.wcf.PatientCRUDClient.NewInstance.savePatFunctionSet(strPatFunctionSort, strPatFunctionVisible);
            }
        }


        private DataTable GetPatFunctionSour()
        {
            DataTable dtPatFunctionSour = new DataTable("dtPatFunctionSour");
            dtPatFunctionSour.Columns.Add("HeaderText");
            dtPatFunctionSour.Columns.Add("Visible");
            dtPatFunctionSour.Columns.Add("VisibleIndex");
            dtPatFunctionSour.Columns.Add("ID");

            dtPatFunctionSour.Rows.Add("结果浏览", "1", 1, 1);
            dtPatFunctionSour.Rows.Add("双列浏览", "1", 2, 2);
            dtPatFunctionSour.Rows.Add("图像浏览", "1", 3, 3);
            dtPatFunctionSour.Rows.Add("历史结果", "1", 4, 4);
            dtPatFunctionSour.Rows.Add("相关结果", "1", 5, 5);
            dtPatFunctionSour.Rows.Add("医嘱信息", "1", 6, 6);
            dtPatFunctionSour.Rows.Add("条码状态", "1", 7, 7);
            dtPatFunctionSour.Rows.Add("结果合并", "1", 8, 8);
            dtPatFunctionSour.Rows.Add("报告状态", "1", 9, 9);

            return dtPatFunctionSour;
        }
    }
}
