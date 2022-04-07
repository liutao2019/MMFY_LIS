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
using dcl.common;
using dcl.client.wcf;
using dcl.entity;
using dcl.servececontract;
using dcl.client.statistical;
using dcl.client.cache;

namespace dcl.client.ananlyse
{
    public partial class FrmDateBasicAnalyse : FrmCommon
    {
        private DataTable dtNull = new DataTable();

        int strState = 0;
        int strAnalyse = 0;
        int strExamine = 0;
        int strData = 0;
        int strParData = 0;



        public FrmDateBasicAnalyse()
        {
            InitializeComponent();
        }

        private void FrmDateBasicAnalyse_Load(object sender, EventArgs e)
        {
            this.dateEditStart.EditValue = ServerDateTime.GetServerDateTime().AddDays(-ServerDateTime.GetServerDateTime().Day + 1).Date;
            this.dateEditEnd.EditValue = ServerDateTime.GetServerDateTime().AddMonths(1).AddDays(-ServerDateTime.GetServerDateTime().Day).Date;
            BindGridView();
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnStat.Name,
                sysToolBar1.btnCalculation.Name, sysToolBar1.BtnPrint.Name,
                sysToolBar1.BtnReset.Name,sysToolBar1.btnSaveTemplate.Name, sysToolBar1.BtnSelectTemplate.Name,
                sysToolBar1.BtnExport.Name,sysToolBar1.BtnClose.Name });
            sysToolBar1.btnSaveTemplate.Caption = "保存模板";
            sysToolBar1.btnCalculation.Caption = "高级查询";
            sysToolBar1.BtnCalculationClick += btnAdvanQuery_Click;

        }
        Dictionary<string, object> daAll = new Dictionary<string, object>();
        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindGridView()
        {
            ProxyStatistical proxy = new ProxyStatistical();
            EntityResponse response = proxy.Service.GetStatCache();
            daAll = response.GetResult() as Dictionary<string, object>;

        }
        #region
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnStatClicked(object sender, EventArgs e)
        {
            if (stat(true))
                strState += 1;
        }
        EntityDCLPrintData printer = new EntityDCLPrintData();
        EntityDCLPrintData printerAnl = new EntityDCLPrintData();//数据分析
        EntityDCLPrintData printerPos = new EntityDCLPrintData();//阳性检出
        EntityDCLPrintData printerSca = new EntityDCLPrintData();//数据浏览
        EntityDCLPrintData printerPat = new EntityDCLPrintData();//病人资料
        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="isTrue"></param>
        private bool stat(bool isTrue)
        {
            this.sysToolBar1.Focus();
            if (this.dateEditStart.EditValue == null || this.dateEditEnd.EditValue == null)
            {
                if (isTrue)
                    lis.client.control.MessageDialog.Show("请输入起始日期！", "提示");
                return false;
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
                    return false;
                }
                if (day > intConfigInterval)
                {
                    lis.client.control.MessageDialog.Show(string.Format("日期范围不能超过{0}天！", intConfigInterval), "提示");
                    return false;
                }
            }
            AdvanSql.TimeType = cbTimeType.Text;
            AdvanSql.EditYBStart = textEditYBStart.Text;
            AdvanSql.EditYBEnd = textEditYBEnd.Text;
            AdvanSql.EditAgeStart = textEditAgeStart.Text;
            AdvanSql.EditAgeEnd = textEditAgeEnd.Text;
            AdvanSql.DateEditStart = dateEditStart.EditValue.ToString();
            AdvanSql.DateEditEnd = Convert.ToDateTime(dateEditEnd.EditValue).AddDays(1).AddMilliseconds(-1).ToString();
            AdvanSql.Sex = lueSex.valueMember;
            AdvanSql.SelectedIndex = radioGroup1.SelectedIndex.ToString();
            string tbName = xtraTabControl1.SelectedTabPage.Name;
            AdvanSql.ReportCode = tbName;
            if (tbName == "tpAnalyse")
            {
                ProxyStatistical proxy = new ProxyStatistical();

                printer = printerAnl = proxy.Service.GetReportData(AdvanSql);
                if (printerAnl != null)
                {
                    if (printerAnl.ReportData.Tables["ErrorMessage"] != null)
                    {
                        //获取传过来的错误信息
                        string message = printerAnl.ReportData.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        MessageDialog.Show(message);
                        return false;
                    }
                    bsdataFenxi.DataSource = printerAnl.ReportData.Tables["可设计字段"];
                }
            }
            if (tbName == "tpExamine")
            {
                gridView2.Columns.Clear();
                ProxyStatistical proxy = new ProxyStatistical();
                printerPos = proxy.Service.GetReportData(AdvanSql);
                if (printerPos != null)
                {
                    if (printerPos.ReportData.Tables["ErrorMessage"] != null)
                    {
                        //获取传过来的错误信息
                        string message = printerPos.ReportData.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        MessageDialog.Show(message);
                        return false;
                    }
                    gridControl2.DataSource = printerPos.ReportData.Tables["可设计字段"];
                }

                foreach (DevExpress.XtraGrid.Columns.GridColumn dc in gridView2.Columns)
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
            }
            if (tbName == "tpData")
            {
                gridView3.Columns.Clear();
                ProxyStatistical proxy = new ProxyStatistical();
                printerSca = proxy.Service.GetReportData(AdvanSql);
                if (printerSca != null)
                {
                    if (printerSca.ReportData.Tables["ErrorMessage"] != null)
                    {
                        //获取传过来的错误信息
                        string message = printerSca.ReportData.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        MessageDialog.Show(message);
                        return false;
                    }
                    gridControl3.DataSource = printerSca.ReportData.Tables["可设计字段"];
                }

                foreach (DevExpress.XtraGrid.Columns.GridColumn dc in gridView3.Columns)
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
            }
            if (tbName == "tpParData")
            {
                gridView4.Columns.Clear();
                ProxyStatistical proxy = new ProxyStatistical();
                printerPat = proxy.Service.GetReportData(AdvanSql);
                if (printerPat != null)
                {
                    if (printerPat.ReportData.Tables["ErrorMessage"] != null)
                    {
                        //获取传过来的错误信息
                        string message = printerPat.ReportData.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        MessageDialog.Show(message);
                        return false;
                    }
                    gcAnalysis.DataSource = printerPat.ReportData.Tables["可设计字段"];
                }

                foreach (DevExpress.XtraGrid.Columns.GridColumn dc in gridView4.Columns)
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

            }
            return true;
        }


        private void sysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnPrintClicked(object sender, EventArgs e)
        {
            string tbName = xtraTabControl1.SelectedTabPage.Name;
            if (tbName == "tpParData")
            {
                gcAnalysis.Print();
                return;
            }

            try
            {
                DCLReportPrint.PrintByData(printer);
            }
            catch (ReportNotFoundException ex1)
            {
                lis.client.control.MessageDialog.Show(ex1.MSG);
            }
            catch (Exception ex2)
            {

            }

        }

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnPrintPreviewClicked(object sender, EventArgs e)
        {
            string tbName = xtraTabControl1.SelectedTabPage.Name;
            if (tbName == "tpParData")
            {
                lis.client.control.MessageDialog.Show("病人资料无需打印预览！", "提示");
                return;
            }
            try
            {
                DCLReportPrint.PrintPreviewByData(printer);
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
        private void sysToolBar1_BtnSaveTemplateClick(object sender, EventArgs e)
        {
            sysToolBar1.Focus();

            FrmStatisticsTemplate fst = new FrmStatisticsTemplate(dtStat);
            fst.ShowDialog();
        }
        private void sysToolBar1_BtnSelectTemplateClick(object sender, EventArgs e)
        {
            FrmSelectTemplate fst = new FrmSelectTemplate("DateBasicAnalyse");
            fst.clikcA += new FrmSelectTemplate.ClikeHander(fst_clikcA);
            fst.ShowDialog();
        }

        /// <summary>
        /// 读取模板
        /// </summary>
        /// <param name="e"></param>
        void fst_clikcA(ClickEventArgs e)
        {
            sysToolBar1_BtnResetClick(null, null);

            FrmAdvanBasicQuery frm = new FrmAdvanBasicQuery(this);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.dtStat = dtStat;
            frm.BindGridView(daAll);

            EntityTpTemplate dtTemp = new EntityTpTemplate();
            dtTemp.StName = e.name;
            dtTemp.StType = "DateBasicAnalyse";
            ProxyStatistical proxy = new ProxyStatistical();
            dtStat = proxy.Service.GetReportTemple(dtTemp);

            FrmGeneralStatistics FrmStat = new FrmGeneralStatistics();
            #region 模板赋值
            if (dtStat != null && dtStat.Count > 0)
            {
                foreach (EntityTpTemplate drTpr in dtStat)
                {
                    string tablename = drTpr.StTableName.ToString();

                    string key = FrmStat.ConvertFromTableName(drTpr.StTableName.ToString());
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

            //界面文本框显示条件
            //FrmAdvanBasicQuery frm = new FrmAdvanBasicQuery(this);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.dtStat = dtStat;
            frm.BindGridView(daAll);
            dtStat = frm.dtStat;
            AdvanSql = frm.getWhere();
            txtWhere.Text = AdvanSql.ItrNameString + AdvanSql.ComNameString + AdvanSql.DeptNameString + AdvanSql.DiagNameString
                + AdvanSql.OriNameString + AdvanSql.PhyNameString + AdvanSql.SampleNameString + AdvanSql.SepNameString;


        }
        /// <summary>
        /// 切换tab自动查询数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage.Name == "tpAnalyse")
            {
                if (strAnalyse != strState)
                {
                    stat(false);
                    strAnalyse = strState;
                }
                printer = printerAnl;
            }
            if (xtraTabControl1.SelectedTabPage.Name == "tpExamine")
            {
                if (strExamine != strState)
                {
                    stat(false);
                    strExamine = strState;
                }
                printer = printerPos;
            }
            if (xtraTabControl1.SelectedTabPage.Name == "tpData")
            {
                if (strData != strState)
                {
                    stat(false);
                    strData = strState;
                }
                printer = printerSca;
            }
            if (xtraTabControl1.SelectedTabPage.Name == "tpParData")
            {
                if (strParData != strState)
                {
                    stat(false);
                    strParData = strState;
                }
                printer = printerPat;
            }
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnExportClicked(object sender, EventArgs e)
        {
            string tbName = xtraTabControl1.SelectedTabPage.Name;
            switch (tbName)
            {
                case "tpAnalyse":
                    setExcel(gridControl1);
                    break;
                case "tpExamine":
                    setExcel(gridControl2);
                    break;
                case "tpData":
                    setExcel(gridControl3);
                    break;
                case "tpParData":
                    setExcel(gcAnalysis);
                    break;
                default:
                    break;
            }
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
                    if (ofd.FileName.Trim() == string.Empty)
                    {
                        lis.client.control.MessageDialog.Show("文件名不能为空！", "提示");
                        return;
                    }

                    try
                    {
                        gcExcel.ExportToXls(ofd.FileName.Trim());
                        lis.client.control.MessageDialog.Show("导出成功！", "提示");
                    }
                    catch (Exception)
                    {
                    }
                }

            }
        }

        private void textEditAgeStart_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void textEditAgeEnd_EditValueChanged(object sender, EventArgs e)
        {

        }
        EntityStatisticsQC AdvanSql = new EntityStatisticsQC();
        private void btnAdvanQuery_Click(object sender, EventArgs e)
        {
            FrmAdvanBasicQuery frm = new FrmAdvanBasicQuery(this);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.dtStat = dtStat;
            frm.BindGridView(daAll);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                dtStat = frm.dtStat;
                AdvanSql = frm.getWhere();
                txtWhere.Text = AdvanSql.ItrNameString + AdvanSql.ComNameString + AdvanSql.DeptNameString + AdvanSql.DiagNameString
                                     + AdvanSql.OriNameString + AdvanSql.PhyNameString + AdvanSql.SampleNameString + AdvanSql.SepNameString
                                     + AdvanSql.SampRemNameString+AdvanSql.SampStateNameString;
            }
        }
    }
}
