using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

using dcl.client.common;
using lis.client.control;
using System.Collections;
using dcl.client.report;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using dcl.common;
using DevExpress.XtraGrid;
using dcl.entity;
using dcl.client.wcf;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.statistical
{
    public partial class FrmGeneralStatistics : FrmCommon
    {
        private DataTable dtNull = new DataTable();
        //项目
        //组合
        int genType = 0;

        private String strReportCode = String.Empty;

        public FrmGeneralStatistics()
        {
            InitializeComponent();
            this.Shown += new EventHandler(FrmGeneralStatistics_Shown);
        }

        void FrmGeneralStatistics_Shown(object sender, EventArgs e)
        {
            //解决 报告查询，统计分析等具有时间选择需要单击进行编辑； 
            AdvanSql.Where += " ";
            AdvanSql.SubWhere += "  ";
            AdvanSql.ItemWhere += "   ";
            AdvanSql.QcWhere += "   ";

            prpGene.Visible = true;
            if (this.MdiParent != null)
            {
                this.MdiParent.Activate();
            }
            this.Activate();
            this.dateEditStart.Focus();

        }

        public FrmGeneralStatistics(string reportCode)
        {
            InitializeComponent();
            this.Shown += new EventHandler(FrmGeneralStatistics_Shown);
            strReportCode = reportCode;
        }
        string localPath = PathManager.ReportPath;

        private void FrmDateBasicAnalyse_Load(object sender, EventArgs e)
        {
            //根据系统配置获取'默认的统计间隔'，单位：天
            string strDefaultQueryInterval = UserInfo.GetSysConfigValue("Stat_DefaultQueryInterval");



            int intDefaultQueryInterval = 30;//默认30天

            if (int.TryParse(strDefaultQueryInterval, out intDefaultQueryInterval) && intDefaultQueryInterval >= 0)
            {
                this.dateEditStart.EditValue = ServerDateTime.GetServerDateTime().AddDays(-intDefaultQueryInterval).Date;
                this.dateEditEnd.EditValue = ServerDateTime.GetServerDateTime().AddDays(1).Date.AddSeconds(-1);//取当天最大时间23:59:59
            }
            else
            {
                this.dateEditStart.EditValue = ServerDateTime.GetServerDateTime().AddDays(-ServerDateTime.GetServerDateTime().Day + 1).Date;
                this.dateEditEnd.EditValue = ServerDateTime.GetServerDateTime().AddMonths(1).Date.AddSeconds(-1);//.AddDays(-ServerDateTime.GetServerDateTime().Day).Date;
            }

            BindGridView();
            string exportBtnName = sysToolBar1.BtnExport.Name;
            if (UserInfo.HaveFunctionByCode("fun_stat_Export"))
            {
                prpGene.SetCanExportFile(true);
                //系统配置：综合统计[导出RTF]的导出格式
                if (UserInfo.GetSysConfigValue("Stat_PpbItem23Command") == "ExportRtf")
                {
                    prpGene.SetPrintPreviewBarItem23Command("ExportRtf");//导出Rtf
                }
            }
            else
            {
                prpGene.SetCanExportFile(false);
                exportBtnName = string.Empty;
            }

            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnStat.Name, sysToolBar1.btnCalculation.Name, sysToolBar1.BtnSinglePrint.Name,
                sysToolBar1.BtnReset.Name,sysToolBar1.btnSaveTemplate.Name, sysToolBar1.BtnSelectTemplate.Name,exportBtnName,sysToolBar1.BtnClose.Name });

            sysToolBar1.btnSaveTemplate.Caption = "保存模板";
            sysToolBar1.btnCalculation.Caption = "高级查询";
            sysToolBar1.BtnCalculationClick += btnAdvanQuery_Click;
            cmbType_onBeforeFilter();
            if (strReportCode != string.Empty)
            {
                List<EntityReportStat> drCmbTypes = cmbType.dtSource.Where(i => i.RepCode == strReportCode.Trim()).ToList();
                if (drCmbTypes.Count > 0)
                {
                    cmbType.selectRow = drCmbTypes[0];
                    cmbType.displayMember = drCmbTypes[0].RepName.ToString();
                    cmbType.valueMember = drCmbTypes[0].RepCode.ToString();
                }
            }
        }
        XtraReport xr;
        Dictionary<string, object> daAll = new Dictionary<string, object>();
        /// <summary>
        /// 绑定数据源
        /// </summary>
        private void BindGridView()
        {
            ProxyStatistical proxy = new ProxyStatistical();
            EntityResponse ds = proxy.Service.GetStatCache();
            daAll = ds.GetResult() as Dictionary<string, object>;
        }



        private void btnAddItem_Click(object sender, EventArgs e)
        {
            bsNull.AddNew();
        }

        EntityDCLPrintData printer = new EntityDCLPrintData();

        /// <summary>
        /// 统计方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnStatClicked(object sender, EventArgs e)
        {
            this.sysToolBar1.Focus();
            if (this.dateEditStart.EditValue == null || this.dateEditEnd.EditValue == null)
            {
                lis.client.control.MessageDialog.Show("请输入起始日期！", "提示");
                return;
            }

            if (cmbType.valueMember == null || cmbType.valueMember.Trim() == "")
            {
                lis.client.control.MessageDialog.Show(string.Format("请选择统计类型！"), "提示");
                return;
            }

            System.TimeSpan ts = Convert.ToDateTime(dateEditEnd.EditValue).Subtract(Convert.ToDateTime(dateEditStart.EditValue));
            int day = ts.Days;

            //根据系统配置获能查询的最大区间，单位：天
            string strConfigInterval = UserInfo.GetSysConfigValue("Stat_MaxQueryInterval");

            int intConfigInterval = 60;//默认60天

            int.TryParse(strConfigInterval, out intConfigInterval);

            if (intConfigInterval != 0)
            {
                if (day < 0)
                {
                    lis.client.control.MessageDialog.Show(string.Format("结束日期不能小于开始日期！", intConfigInterval), "提示");
                    return;
                }
                if (day > intConfigInterval)
                {
                    lis.client.control.MessageDialog.Show(string.Format("日期范围不能超过{0}天！", intConfigInterval), "提示");
                    return;
                }
            }
            genType = 1;
            #region 查找数据
            string tbName = xtraTabControl1.SelectedTabPage.Name;
            //List<string> stWhere = getWhere();
            AdvanSql.ReportCode = cmbType.valueMember;
            string sDate = "";
            string eDate = "";
            if (this.dateEditStart.EditValue != null)
            {
                sDate = (Convert.ToDateTime(dateEditStart.EditValue)).ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (this.dateEditEnd.EditValue != null)
            {
                eDate = (Convert.ToDateTime(dateEditEnd.EditValue)).ToString("yyyy-MM-dd HH:mm:ss");
            }
            string nDate = ServerDateTime.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss");
            AdvanSql.TimeType = cbTimeType.Text;
            AdvanSql.EditYBStart = textEditYBStart.Text;
            AdvanSql.EditYBEnd = textEditYBEnd.Text;
            AdvanSql.EditAgeStart = textEditAgeStart.Text;
            AdvanSql.EditAgeEnd = textEditAgeEnd.Text;
            AdvanSql.DateEditStart = sDate;
            AdvanSql.DateEditEnd = eDate;
            AdvanSql.SelectedIndex = radioGroup1.SelectedIndex.ToString();
            AdvanSql.OrgId = selectDicPubOrganize1.valueMember;
            ProxyStatistical proxy = new ProxyStatistical();
            printer = proxy.Service.GetReportData(AdvanSql);


            gvData.Columns.Clear();

            //判断返回的DataSet是否有数据或者错误信息
            if (printer.ReportData.Tables.Count > 0)
            {
                if (printer.ReportData.Tables["ErrorMessage"] != null)
                {
                    //获取传过来的错误信息
                    string message = printer.ReportData.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    MessageDialog.Show(message);
                    return;
                }
                DataTable dt = printer.ReportData.Tables[0];
                gcData.DataSource = dt;
            }
            else
                return;

            foreach (DevExpress.XtraGrid.Columns.GridColumn dc in gvData.Columns)
            {
                dc.OptionsColumn.AllowEdit = false;
                dc.Width = 75;
                if (dc.ColumnType.Name == "DateTime")//如果为时间列,自动转换
                {
                    dc.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    dc.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
                    dc.Width = 110;
                }
            }

            #endregion
            if (tbName == "xtReport")
            {
                if (printer.ReportData != null)
                {
                    try
                    {
                        string pathStr = "";
                        xr = new XtraReport();
                        pathStr = localPath + printer.ReportName + printer.ReportSuffix;
                        xr.LoadLayout(pathStr);
                        SetHospitalName.setName(xr.Bands);
                        DataTable dt = new DataTable();
                        xr.DataSource = printer.ReportData;
                        prpGene.printControl1.PrintingSystem = xr.PrintingSystem;
                        xr.CreateDocument();
                        prpGene.printControl1.ExecCommand(DevExpress.XtraPrinting.PrintingSystemCommand.PageLayoutFacing);
                        prpGene.printControl1.ExecCommand(DevExpress.XtraPrinting.PrintingSystemCommand.PageLayoutContinuous);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            else
            {
                gcData.DataSource = printer.ReportData.Tables[0];
            }
        }

        private void sysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sysToolBar1_OnBtnSinglePrintClicked(object sender, EventArgs e)
        {
            string xtName = xtraTabControl1.SelectedTabPage.Name;
            if (xtName == "xtReport")
            {
                if (xr != null)
                {
                    xr.Print();
                }
            }
            else
            {
                if (chartControl1 != null)
                {
                    chartControl1.Print();
                }
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_BtnResetClick(object sender, EventArgs e)
        {
            sysToolBar1.Focus();
            txtWhere.Text = "";
            AdvanSql.Where = " ";
            AdvanSql.SubWhere = "  ";
            AdvanSql.ItemWhere = "   ";
            dtStat = new List<EntityTpTemplate>();
        }

        List<EntityTpTemplate> dtStat = new List<EntityTpTemplate>();
        /// <summary>
        /// 保存模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_BtnSaveTemplateClick(object sender, EventArgs e)
        {
            sysToolBar1.Focus();

            FrmStatisticsTemplate fst = new FrmStatisticsTemplate(dtStat);
            fst.ShowDialog();
        }



        private void rgChar_SelectedIndexChanged(object sender, EventArgs e)
        {
            //sysToolBar1_OnBtnStatClicked(sender, e);
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (genType == 1)
            {
                sysToolBar1_OnBtnStatClicked(sender, e);
                genType = 0;
            }
        }

        private void sysToolBar1_BtnSelectTemplateClick(object sender, EventArgs e)
        {
            FrmSelectTemplate fst = new FrmSelectTemplate("GeneralStatistics");
            fst.clikcA += new FrmSelectTemplate.ClikeHander(fst_clikcA);
            fst.ShowDialog();
        }

        void fst_clikcA(ClickEventArgs e)
        {
            sysToolBar1_BtnResetClick(null, null);

            FrmAdvanGeneralQuery frm = new FrmAdvanGeneralQuery();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.dtStat = dtStat;
            frm.BindGridView(daAll);

            EntityTpTemplate dtTemp = new EntityTpTemplate();
            dtTemp.StName = e.name;
            dtTemp.StType = "GeneralStatistics";
            ProxyStatistical proxy = new ProxyStatistical();
            dtStat = proxy.Service.GetReportTemple(dtTemp);

            #region 模板赋值
            if (dtStat != null && dtStat.Count > 0)
            {
                foreach (EntityTpTemplate drTpr in dtStat)
                {
                    string tablename = drTpr.StTableName.ToString();

                    string key = ConvertFromTableName(drTpr.StTableName.ToString());
                    if (key == "dtInst")
                    {
                        List<EntityDicInstrument> dtIns = daAll[key] as List<EntityDicInstrument>;
                        foreach (var item in dtIns)
                        {
                            if (item.SpId.ToString() == drTpr.StTableId.ToString())
                            {
                                item.Checked = true;
                            }
                        }
                    }
                    if (key == "dtDep")
                    {
                        List<EntityDicPubDept> dtIns = daAll[key] as List<EntityDicPubDept>;
                        foreach (var item in dtIns)
                        {
                            if (item.SpId.ToString() == drTpr.StTableId.ToString())
                            {
                                item.Checked = true;
                            }
                        }
                    }
                    if (key == "dtDiag")
                    {
                        List<EntityDicPubIcd> dtIns = daAll[key] as List<EntityDicPubIcd>;
                        foreach (var item in dtIns)
                        {
                            if (item.SpId.ToString() == drTpr.StTableId.ToString())
                            {
                                item.Checked = true;
                            }
                        }
                    }
                    if (key == "dtSam")
                    {
                        List<EntityDicSample> dtIns = daAll[key] as List<EntityDicSample>;
                        foreach (var item in dtIns)
                        {
                            if (item.SpId.ToString() == drTpr.StTableId.ToString())
                            {
                                item.Checked = true;
                            }
                        }
                    }
                    if (key == "dtPhyType")
                    {
                        List<EntityDicPubProfession> dtIns = daAll[key] as List<EntityDicPubProfession>;
                        foreach (var item in dtIns)
                        {
                            if (item.SpId.ToString() == drTpr.StTableId.ToString())
                            {
                                item.Checked = true;
                            }
                        }
                    }
                    if (key == "dtSpeType")
                    {
                        List<EntityDicPubProfession> dtIns = daAll[key] as List<EntityDicPubProfession>;
                        foreach (var item in dtIns)
                        {
                            if (item.SpId.ToString() == drTpr.StTableId.ToString())
                            {
                                item.Checked = true;
                            }
                        }
                    }
                    if (key == "dtCombine")
                    {
                        List<EntityDicCombine> dtIns = daAll[key] as List<EntityDicCombine>;
                        foreach (var item in dtIns)
                        {
                            if (item.SpId.ToString() == drTpr.StTableId.ToString())
                            {
                                item.Checked = true;
                            }
                        }
                    }
                    if (key == "dtUs")
                    {
                        List<EntitySysUser> dtIns = daAll[key] as List<EntitySysUser>;
                        foreach (var item in dtIns)
                        {
                            if (item.SpId.ToString() == drTpr.StTableId.ToString())
                            {
                                item.Checked = true;
                            }
                        }
                    }
                    if (key == "dtAndit")
                    {
                        List<EntitySysUser> dtIns = daAll[key] as List<EntitySysUser>;
                        foreach (var item in dtIns)
                        {
                            if (item.SpId.ToString() == drTpr.StTableId.ToString())
                            {
                                item.Checked = true;
                            }
                        }
                    }
                    if (key == "dtDoc")
                    {
                        List<EntityDicDoctor> dtIns = daAll[key] as List<EntityDicDoctor>;
                        foreach (var item in dtIns)
                        {
                            if (item.SpId.ToString() == drTpr.StTableId.ToString())
                            {
                                item.Checked = true;
                            }
                        }
                    }
                    if (key == "dictResRefFlag")
                    {
                        List<EntityDicResultTips> dtIns = daAll[key] as List<EntityDicResultTips>;
                        foreach (var item in dtIns)
                        {
                            if (item.SpId.ToString() == drTpr.StTableId.ToString())
                            {
                                item.Checked = true;
                            }
                        }
                    }
                    if (key == "dtOri")
                    {
                        List<EntityDicOrigin> dtIns = daAll[key] as List<EntityDicOrigin>;
                        foreach (var item in dtIns)
                        {
                            if (item.SpId.ToString() == drTpr.StTableId.ToString())
                            {
                                item.Checked = true;
                            }
                        }
                    }
                    if (key == "dictUrgent")
                    {
                        List<EntityDicPubEvaluate> dtIns = daAll[key] as List<EntityDicPubEvaluate>;
                        foreach (var item in dtIns)
                        {
                            if (item.SpId.ToString() == drTpr.StTableId.ToString())
                            {
                                item.Checked = true;
                            }
                        }
                    }
                    if (key == "dictSampStatus")
                    {
                        List<EntityDicSState> dtIns = daAll[key] as List<EntityDicSState>;
                        foreach (var item in dtIns)
                        {
                            if (item.SpId.ToString() == drTpr.StTableId.ToString())
                            {
                                item.Checked = true;
                            }
                        }
                    }
                    if (key == "dictSampRemark")
                    {
                        List<EntityDicSampRemark> dtIns = daAll[key] as List<EntityDicSampRemark>;
                        foreach (var item in dtIns)
                        {
                            if (item.SpId.ToString() == drTpr.StTableId.ToString())
                            {
                                item.Checked = true;
                            }
                        }
                    }
                    if (key == "null")
                    {
                        List<EntityObrResult> dtIns = daAll["dtNull"] as List<EntityObrResult>;
                        foreach (var item in dtIns)
                        {
                            item.ItmEname = drTpr.ResItmEcd;
                            item.ObrValue = drTpr.ResChr;
                            item.ObrValue2 = drTpr.ResOdChr;
                            item.ObrUnit = drTpr.ResUnit;
                            dtIns.Add(item);
                        }
                    }
                }
            }
            #endregion

            if (cmbType.valueMember == null || cmbType.valueMember.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("请选择统计类型！", "提示");
                return;
            }

            //FrmAdvanGeneralQuery frm = new FrmAdvanGeneralQuery();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.dtStat = dtStat;
            frm.BindGridView(daAll);
            dtStat = frm.dtStat;
            AdvanSql = frm.getWhere();
            txtWhere.Text = AdvanSql.ItrNameString + AdvanSql.AuditNameString + AdvanSql.ChkDocNameString
                + AdvanSql.ComNameString + AdvanSql.DeptNameString + AdvanSql.DiagNameString + AdvanSql.MarkNameString
                + AdvanSql.OriNameString + AdvanSql.PhyNameString + AdvanSql.SampleNameString + AdvanSql.SendDocNameString
                + AdvanSql.SepNameString + AdvanSql.SexNameString + AdvanSql.TipNameString+AdvanSql.SampStateNameString+AdvanSql.SampRemNameString;

        }

        public string ConvertFromTableName(string tablename)
        {
            if (string.IsNullOrEmpty(tablename)) return string.Empty;
            string StTableName = string.Empty;
            #region 表名转换
            switch (tablename)
            {
                case "dict_instrmt":
                    StTableName = "dtInst";
                    break;
                case "dict_depart":
                    StTableName = "dtDep";
                    break;
                case "dict_diagnos":
                    StTableName = "dtDiag";
                    break;
                case "dict_sample":
                    StTableName = "dtSam";
                    break;
                case "dict_PhyType":
                    StTableName = "dtPhyType";
                    break;
                case "dict_SepType":
                    StTableName = "dtSpeType";
                    break;
                case "dict_combine":
                    StTableName = "dtCombine";
                    break;
                case "poweruserinfo":
                    StTableName = "dtUs";
                    break;
                case "poweruserinfo1":
                    StTableName = "dtAndit";
                    break;
                case "dict_doctor":
                    StTableName = "dtDoc";
                    break;
                case "dict_res_ref_flag":
                    StTableName = "dictResRefFlag";
                    break;
                case "Dict_origin":
                    StTableName = "dtOri";
                    break;
                case "Dict_bscripe":
                    StTableName = "dictUrgent";
                    break;
                case "Dict_s_state":
                    StTableName = "dictSampStatus";
                    break;
                case "Dict_sample_remarks":
                    StTableName = "dictSampRemark";
                    break;
            }
            #endregion
            return StTableName;
        }

        private void sysToolBar1_OnBtnExportClicked(object sender, EventArgs e)
        {
            string tbName = xtraTabControl1.SelectedTabPage.Name;
            if (tbName == "xtImg")
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.DefaultExt = "jpg";
                ofd.Filter = "图片文件(*.jpg)|*.jpg";
                ofd.Title = "导出到图片";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.Trim() == string.Empty)
                    {
                        lis.client.control.MessageDialog.Show("文件名不能为空！", "提示");
                        return;
                    }

                    try
                    {
                        chartControl1.ExportToImage(ofd.FileName.Trim(), System.Drawing.Imaging.ImageFormat.Jpeg);
                        lis.client.control.MessageDialog.Show("导出成功！", "提示");
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            else
            {
                if (gcData.DataSource != null)
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
                            gcData.ExportToXls(ofd.FileName.Trim());
                            lis.client.control.MessageDialog.Show("导出成功！", "提示");
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }

        }

        private void cmbType_onBeforeFilter()
        {
            this.cmbType.dtSource = this.cmbType.dtSource.Where(i => i.RepType == "GeneralStatistics").OrderBy(i=>i.RepSeq.Length).ThenBy(i=>i.RepSeq).ToList();
            if (UserInfo.isAdmin == false)
            {
                //string fileter = "funcCode = 'GeneralStatistics'";
                List<EntitySysFunction> listUserFunc = UserInfo.entityUserInfo.Func;
                if (listUserFunc != null && listUserFunc.Count > 0)
                {
                    List<EntitySysFunction> listUser = listUserFunc.Where(w => w.FuncCode == "GeneralStatistics").ToList();
                    if (listUser.Count > 0)
                    {
                        string str = string.Empty;

                        foreach (EntitySysFunction item in listUser)
                        {
                            if (item.FuncChildName.Trim() != string.Empty)
                            {
                                str += ",'" + item.FuncChildName.Trim() + "'";
                            }
                        }

                        if (str.Length > 0)
                        {
                            str = str.Remove(0, 1);
                            this.cmbType.dtSource = this.cmbType.dtSource.Where(i => str.Contains(i.RepCode)).ToList();
                        }
                    }
                }
            }
            if (strReportCode != string.Empty)
            {
                this.cmbType.dtSource = this.cmbType.dtSource.Where(i => i.RepCode == strReportCode.Trim()).ToList();
            }
        }
        EntityStatisticsQC AdvanSql = new EntityStatisticsQC();
        private void btnAdvanQuery_Click(object sender, EventArgs e)
        {
            if (cmbType.valueMember == null || cmbType.valueMember.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("请选择统计类型！", "提示");
                return;
            }

            FrmAdvanGeneralQuery frm = new FrmAdvanGeneralQuery();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.dtStat = dtStat;
            frm.BindGridView(daAll);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                dtStat = frm.dtStat;
                AdvanSql = frm.getWhere();
                txtWhere.Text = AdvanSql.ItrNameString + AdvanSql.AuditNameString + AdvanSql.ChkDocNameString
                    + AdvanSql.ComNameString + AdvanSql.DeptNameString + AdvanSql.DiagNameString + AdvanSql.MarkNameString
                    + AdvanSql.OriNameString + AdvanSql.PhyNameString + AdvanSql.SampleNameString + AdvanSql.SendDocNameString
                    + AdvanSql.SepNameString + AdvanSql.SexNameString + AdvanSql.TipNameString+AdvanSql.SampStateNameString+AdvanSql.SampRemNameString;
            }
        }
    }
}
