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
using dcl.client.wcf;
using dcl.entity;
using dcl.client.statistical;
using dcl.client.cache;

namespace dcl.client.ananlyse
{
    public partial class FrmTimeAnalyse : FrmCommon
    {
        public FrmTimeAnalyse()
        {
            InitializeComponent();
        }

        private void FrmTimeAnalyse_Load(object sender, EventArgs e)
        {
            //系统配置：时间统计分析允许过滤回退条码
            if (ConfigHelper.GetSysConfigValueWithoutLogin("TimeAnalyse_AllowCbReturnBC") == "是")
            {
                cbWithoutReturn.Visible = true;
                cbWithoutReturn.Checked = false;
            }

            this.dateEditStart.EditValue = ServerDateTime.GetServerDateTime().AddDays(-ServerDateTime.GetServerDateTime().Day + 1).Date;
            this.dateEditEnd.EditValue = ServerDateTime.GetServerDateTime().AddMonths(1).AddDays(-ServerDateTime.GetServerDateTime().Day).Date.AddDays(1).AddSeconds(-1);
            BindGridView();

            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnStat.Name, sysToolBar1.btnCalculation.Name,
                sysToolBar1.BtnSinglePrint.Name, sysToolBar1.BtnReset.Name,sysToolBar1.btnSaveTemplate.Name,
                sysToolBar1.BtnSelectTemplate.Name, sysToolBar1.BtnExport.Name,sysToolBar1.BtnClose.Name });
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
            EntityResponse ds = proxy.Service.GetStatCache();
            daAll = ds.GetResult() as Dictionary<string, object>;
        }


        DataSet ds = new DataSet();

        /// <summary>
        /// 事件统计
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
            TimeSpan ts = Convert.ToDateTime(dateEditEnd.EditValue).Subtract(Convert.ToDateTime(dateEditStart.EditValue));
            int day = ts.Days;
            if (day < 0 || day > 61)
            {
                lis.client.control.MessageDialog.Show("日期范围不能超过两个月且结束日期大于开始日期！", "提示");
                return;
            }
            if (this.chkTypes.CheckedItems.Count == 0)
            {
                lis.client.control.MessageDialog.Show("请选择要分组的标识！", "提示");
                return;
            }
            List<string> typeList = new List<string>();
            #region 查找数据
            for (int i = 0; i < chkTypes.CheckedItems.Count; i++)
            {
                string type = chkTypes.CheckedItems[i].ToString();
                typeList.Add(type);
            }
            AdvanSql.TimeType = cbTimeType.Text;
            AdvanSql.EditYBStart = textEditYBStart.Text;
            AdvanSql.EditYBEnd = textEditYBEnd.Text;
            AdvanSql.DateEditStart = dateEditStart.EditValue.ToString();
            AdvanSql.DateEditEnd = dateEditEnd.EditValue.ToString();
            AdvanSql.ReportCode = "timeAnalysis";
            AdvanSql.typeList = typeList;
            ProxyAnalyse proxy = new ProxyAnalyse();
            ds = proxy.Service.GetReportData(AdvanSql);
            #endregion
            if (ds != null)
            {
                try
                {
                    this.gvTime.Columns.Clear();
                    if (ds.Tables[0].Columns.Contains("ErrorMessage"))
                    {
                        //获取传过来的错误信息
                        string message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        MessageDialog.Show(message);
                    }
                    else
                    {
                        this.gcTime.DataSource = ds.Tables["可设计字段"];
                        foreach (DevExpress.XtraGrid.Columns.GridColumn dc in gvTime.Columns)
                        {
                            dc.OptionsColumn.AllowEdit = false;
                        }
                    }
                }
                catch (Exception)
                {
                    string message = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    MessageDialog.Show(message);
                }
            }
        }

        private void sysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            this.Close();
        }


        private void sysToolBar1_OnBtnSinglePrintClicked(object sender, EventArgs e)
        {
            DataTable dt = gcTime.DataSource as DataTable;
            if (dt != null && ((DataTable)gcTime.DataSource).Rows.Count > 0)
            {
                this.gcTime.Print();
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
            dtStat = new List<EntityTpTemplate>();
            txtWhere.Text = "";
            AdvanSql = new EntityStatisticsQC();
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


        private void sysToolBar1_BtnSelectTemplateClick(object sender, EventArgs e)
        {
            FrmSelectTemplate fst = new FrmSelectTemplate("TimeAnalyse");
            fst.clikcA += new FrmSelectTemplate.ClikeHander(fst_clikcA);
            fst.ShowDialog();
        }

        void fst_clikcA(ClickEventArgs e)
        {
            sysToolBar1_BtnResetClick(null, null);

            FrmAdvanTimeQuery frm = new FrmAdvanTimeQuery();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.dtStat = dtStat;
            frm.BindGridView(daAll);

            EntityTpTemplate dtTemp = new EntityTpTemplate();
            dtTemp.StName = e.name;
            dtTemp.StType = "TimeAnalyse";
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
            //FrmAdvanTimeQuery frm = new FrmAdvanTimeQuery();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.dtStat = dtStat;
            frm.BindGridView(daAll);
            dtStat = frm.dtStat;
            AdvanSql = frm.getWhere();
            txtWhere.Text = AdvanSql.ItrNameString + AdvanSql.ChkDocNameString
                + AdvanSql.ComNameString + AdvanSql.DeptNameString + AdvanSql.MarkNameString
                + AdvanSql.PhyNameString + AdvanSql.SampleNameString + AdvanSql.SendDocNameString
                + AdvanSql.SepNameString;
        }

        private void sysToolBar1_OnBtnExportClicked(object sender, EventArgs e)
        {
            if (gcTime.DataSource != null)
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
                        gcTime.ExportToXls(ofd.FileName);
                        lis.client.control.MessageDialog.Show("导出成功！", "提示");
                    }
                    catch (Exception)
                    {
                    }
                }

            }
        }



        EntityStatisticsQC AdvanSql = new EntityStatisticsQC();
        private void btnAdvanQuery_Click(object sender, EventArgs e)
        {
            FrmAdvanTimeQuery frm = new FrmAdvanTimeQuery();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.dtStat = dtStat;
            frm.BindGridView(daAll);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                dtStat = frm.dtStat;
                AdvanSql = frm.getWhere();
                txtWhere.Text = AdvanSql.ItrNameString + AdvanSql.ChkDocNameString
                    + AdvanSql.ComNameString + AdvanSql.DeptNameString + AdvanSql.MarkNameString
                    + AdvanSql.PhyNameString + AdvanSql.SampleNameString + AdvanSql.SendDocNameString
                    + AdvanSql.SepNameString;
            }
        }
    }
}
