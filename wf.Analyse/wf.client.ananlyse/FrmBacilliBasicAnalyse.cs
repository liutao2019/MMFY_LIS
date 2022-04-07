using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using lis.client.control;
using dcl.client.report;
using DevExpress.XtraReports.UI;
using DevExpress.XtraCharts;
using System.IO;
using dcl.entity;
using dcl.client.wcf;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.ananlyse
{
    public partial class FrmBacilliBasicAnalyse : FrmCommon
    {
        /// <summary>
        /// 统计查询条件
        /// </summary>
        EntityStatisticsQC AdvanSql = new EntityStatisticsQC();

        /// <summary>
        /// 
        /// </summary>
        string txtAdvanCondition = "";

        /// <summary>
        /// 
        /// </summary>
        List<EntityTpTemplate> dtStat = new List<EntityTpTemplate>();

        /// <summary>
        /// 
        /// </summary>
        DataRow drPrintConfig = null;

        /// <summary>
        /// 
        /// </summary>
        Dictionary<string, object> daAll = new Dictionary<string, object>();

        /// <summary>
        /// 
        /// </summary>
        string localPath = PathManager.ReportPath;

        /// <summary>
        /// 
        /// </summary>
        XtraReport xr;

        public FrmBacilliBasicAnalyse()
        {
            InitializeComponent();
            this.Shown += new EventHandler(FrmBacilliBasicAnalyse_Shown);
        }

        void FrmBacilliBasicAnalyse_Shown(object sender, EventArgs e)
        {
            //解决 报告查询，统计分析等具有时间选择需要单击进行编辑； 
            prpBacilli.Visible = true;
            if (this.MdiParent != null)
            {
                this.MdiParent.Activate();
            }
            this.Activate();
            this.dateEditStart.Focus();
            var reportStat = cmbType.dtSource;
            reportStat = reportStat.Where(i => i.RepType == "BacilliAnalyse").ToList();
            cmbType.dtSource = reportStat;
        }

        private void FrmDateBasicAnalyse_Load(object sender, EventArgs e)
        {
            txtWhere.Properties.ReadOnly = true;
            this.dateEditStart.EditValue = ServerDateTime.GetServerDateTime().AddDays(-ServerDateTime.GetServerDateTime().Day + 1).Date;
            this.dateEditEnd.EditValue = ServerDateTime.GetServerDateTime().AddMonths(1).AddDays(-ServerDateTime.GetServerDateTime().Day).Date;
            BindGridView();
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnStat.Name,
                sysToolBar1.btnCalculation.Name, sysToolBar1.BtnSinglePrint.Name,
                sysToolBar1.BtnReset.Name,sysToolBar1.btnSaveTemplate.Name, sysToolBar1.BtnSelectTemplate.Name,
                sysToolBar1.BtnExport.Name,sysToolBar1.BtnClose.Name });
            sysToolBar1.btnSaveTemplate.Caption = "保存模板";
            sysToolBar1.btnCalculation.Caption = "高级查询";
            sysToolBar1.BtnCalculationClick += SysToolBar1_BtnCalculationClick;

            #region 设置打印机
            //   string xmlFile = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\hope\lis\printXml\printConfig.xml";
            string xmlFile = PathManager.SettingLisPath + @"\printXml\printConfig.xml";

            if (File.Exists(xmlFile))
            {
                DataSet dsPrint = new DataSet();
                dsPrint.ReadXml(xmlFile);
                if (dsPrint.Tables.Count > 0)
                {
                    DataTable dt = dsPrint.Tables[0];
                    if (dt != null)
                    {
                        drPrintConfig = dt.Rows[0];
                    }
                }
            }
            #endregion

            base.ShowSucessMessage = false;
        }


        /// <summary>
        /// 高级查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SysToolBar1_BtnCalculationClick(object sender, EventArgs e)
        {
            if (cmbType.valueMember == null || cmbType.valueMember.Trim() == "")
            {
                lis.client.control.MessageDialog.Show("请选择统计类型！", "提示");
                return;
            }

            FrmAdvanBacQuery frm = new FrmAdvanBacQuery();
            frm.dtStat = dtStat;
            frm.BindGridView(daAll);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                dtStat = frm.dtStat;
                AdvanSql = frm.getWhere(cmbType.displayMember.Split('.')[0]);
                txtAdvanCondition = AdvanSql.BactypeNameString + AdvanSql.BacNameString + AdvanSql.AntiNameString +
                                                   AdvanSql.DeptNameString + AdvanSql.SampleNameString + AdvanSql.OriNameString +
                                                   AdvanSql.SampStateNameString + AdvanSql.SampRemNameString;
                txtWhere.Text = txtAdvanCondition;
            }
        }

        /// <summary>
        /// 查询事件
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
                lis.client.control.MessageDialog.Show("请选择统计类型！", "提示");
                return;
            }
            if (!string.IsNullOrEmpty(this.textEditYBStart.Text))
            {
                AdvanSql.EditYBStart = this.textEditYBStart.Text;
            }
            if (!string.IsNullOrEmpty(this.textEditYBEnd.Text))
            {
                AdvanSql.EditYBEnd = this.textEditYBEnd.Text;
            }
            if (!string.IsNullOrEmpty(this.textEditAgeStart.Text))
            {
                AdvanSql.EditAgeStart = this.textEditAgeStart.Text;
            }
            if (!string.IsNullOrEmpty(this.textEditAgeEnd.Text))
            {
                AdvanSql.EditAgeEnd = this.textEditAgeEnd.Text;
            }
            string tbName = xtraTabControl1.SelectedTabPage.Name;
            AdvanSql.ReportCode = cmbType.valueMember;
            if (tbName == "j_Table")
            {
                #region 统计数据
                string sDate = "";
                string eDate = "";
                if (this.dateEditStart.EditValue != null)
                {
                    sDate = (Convert.ToDateTime(dateEditStart.EditValue)).ToString("yyyy-MM-dd");
                }
                if (this.dateEditEnd.EditValue != null)
                {
                    eDate = (Convert.ToDateTime(dateEditEnd.EditValue)).ToString("yyyy-MM-dd");
                }
                AdvanSql.DateEditStart = sDate;
                AdvanSql.DateEditEnd = eDate;
                AdvanSql.SelectedIndex = "0";
                AdvanSql.CmbResults = cmbResults.EditValue.ToString();
                string type = cmbType.displayMember.Split('.')[0];
                AdvanSql.BacilliType = type;
                AdvanSql.TimeType = cbTimeType.Text;
                ProxyStatistical proxyStat = new ProxyStatistical();
                EntityDCLPrintData ds = proxyStat.Service.GetReportData(AdvanSql);
                string localPath = PathManager.ReportPath;
                if (ds != null)
                {
                    if (ds.ReportData.Tables["ErrorMessage"] != null)
                    {
                        //获取传过来的错误信息
                        string message = ds.ReportData.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        MessageDialog.Show(message);
                        return;
                    }

                    try
                    {
                        string pathStr = "";
                        xr = new XtraReport();
                        pathStr = localPath + ds.ReportName+ds.ReportSuffix;
                        xr.LoadLayout(pathStr);
                        SetHospitalName.setName(xr.Bands);
                        DataTable dt = new DataTable();
                        xr.DataSource = ds.ReportData;
                        prpBacilli.printControl1.PrintingSystem = xr.PrintingSystem;
                        //xr.PrintingSystem = this.printingSystem1;
                        #region 设置打印机
                        if (drPrintConfig != null)
                        {
                            xr.PrintingSystem.PageSettings.PrinterName = drPrintConfig["printName"].ToString();
                            xr.PaperName = drPrintConfig["paper"].ToString();
                            xr.PaperKind = System.Drawing.Printing.PaperKind.Custom;
                            xr.ReportUnit = ReportUnit.HundredthsOfAnInch;
                            xr.PageHeight = Convert.ToInt32(drPrintConfig["heigth"]);
                            xr.PageWidth = Convert.ToInt32(drPrintConfig["width"]);
                        }
                        #endregion
                        xr.CreateDocument();
                        prpBacilli.printControl1.ExecCommand(DevExpress.XtraPrinting.PrintingSystemCommand.PageLayoutFacing);
                        prpBacilli.printControl1.ExecCommand(DevExpress.XtraPrinting.PrintingSystemCommand.PageLayoutContinuous);
                    }
                    catch (Exception ex)
                    {
                        lis.client.control.MessageDialog.Show(ex.ToString());
                    }
                }
                #endregion
                return;
            }
            if (tbName == "j_Data")
            {
                string sDate = string.Empty;
                string eDate = string.Empty;
                if (this.dateEditStart.EditValue != null)
                {
                    sDate = (Convert.ToDateTime(dateEditStart.EditValue)).ToString("yyyy-MM-dd");
                }
                if (this.dateEditEnd.EditValue != null)
                {
                    eDate = (Convert.ToDateTime(dateEditEnd.EditValue)).ToString("yyyy-MM-dd");
                }
                AdvanSql.DateEditStart = sDate;
                AdvanSql.DateEditEnd = eDate;
                ProxyAnalyse proxy = new ProxyAnalyse();
                DataSet ds = proxy.Service.GetAnalyseData(AdvanSql);
                setColumnSort(ds.Tables[0]);
                DataTable dtPatients = ds.Tables[0];
                gvData.Columns.Clear();
                gvData.OptionsView.ColumnAutoWidth = false;
                dtPatients.Columns.Remove("pat_id");
                //排序
                if (dtPatients != null && dtPatients.Rows.Count > 0
                    && dtPatients.Columns.Contains("日期")
                    && dtPatients.Columns.Contains("时间"))
                {
                    DataView dvtempcopy = dtPatients.DefaultView.ToTable().DefaultView;
                    dvtempcopy.Sort = "日期 asc,时间 asc";
                    dtPatients = dvtempcopy.ToTable();
                }
                gcData.DataSource = dtPatients;
                for (int j = 0; j < gvData.Columns.Count; j++)
                {
                    gvData.Columns[j].OptionsColumn.AllowEdit = false;
                    gvData.Columns[j].Width = 75;
                }

                return;
            }
        }

        /// <summary>
        /// 选择模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_BtnSelectTemplateClick(object sender, EventArgs e)
        {
            FrmSelectTemplate fst = new FrmSelectTemplate("BacilliBasicAnalyse");
            fst.clikcA += new FrmSelectTemplate.ClikeHander(fst_clikcA);
            fst.ShowDialog();
        }

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

        /// <summary>
        /// 导出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            string tbName = xtraTabControl1.SelectedTabPage.Name;
            if (tbName == "j_3D")
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
            if (tbName == "j_Img")
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
                        charBac.ExportToImage(ofd.FileName.Trim(), System.Drawing.Imaging.ImageFormat.Jpeg);
                        lis.client.control.MessageDialog.Show("导出成功！", "提示");
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            if (tbName == "j_Data")
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
                            gcData.ExportToXls(ofd.FileName);
                            lis.client.control.MessageDialog.Show("导出成功！", "提示");
                        }
                        catch (Exception)
                        {
                        }
                    }

                }
            }
            if (tbName == "j_Table")
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.DefaultExt = "pdf";
                ofd.Filter = "pdf文件(*.pdf)|*.pdf";
                ofd.Title = "导出到pdf";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.Trim() == string.Empty)
                    {
                        lis.client.control.MessageDialog.Show("文件名不能为空！", "提示");
                        return;
                    }
                    try
                    {
                        prpBacilli.printControl1.PrintingSystem.ExportToPdf(ofd.FileName);
                        lis.client.control.MessageDialog.Show("导出成功！", "提示");
                    }
                    catch (Exception)
                    {
                    }
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
            dtStat = new List<EntityTpTemplate>();
            AdvanSql = new EntityStatisticsQC();
            txtAdvanCondition = "";
            txtWhere.Text = "";
        }

        /// <summary>
        /// 提取模板
        /// </summary>
        /// <param name="e"></param>
        private void fst_clikcA(ClickEventArgs e)
        {
            sysToolBar1_BtnResetClick(null, null);

            FrmAdvanBacQuery frm = new FrmAdvanBacQuery();
            frm.dtStat = dtStat;
            frm.BindGridView(daAll);

            EntityTpTemplate dtTemp = new EntityTpTemplate();
            dtTemp.StName = e.name;
            dtTemp.StType = "BacilliBasicAnalyse";
            ProxyStatistical proxy = new ProxyStatistical();
            dtStat = proxy.Service.GetReportTemple(dtTemp);

            #region 模板赋值
            if (dtStat != null && dtStat.Count > 0)
            {
                foreach (EntityTpTemplate drTpr in dtStat)
                {
                    string tablename = drTpr.StTableName.ToString();

                    string key = ConvertFromTableName(drTpr.StTableName.ToString());
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
                    if (key == "dictBacteri")
                    {
                        List<EntityDicMicBacteria> dtIns = daAll[key] as List<EntityDicMicBacteria>;
                        foreach (var item in dtIns)
                        {
                            if (item.SpId.ToString() == drTpr.StTableId.ToString())
                            {
                                item.Checked = true;
                            }
                        }
                    }
                    if (key == "dictAntibio")
                    {
                        List<EntityDicMicAntibio> dtIns = daAll[key] as List<EntityDicMicAntibio>;
                        foreach (var item in dtIns)
                        {
                            if (item.SpId.ToString() == drTpr.StTableId.ToString())
                            {
                                item.Checked = true;
                            }
                        }
                    }
                    if (key == "dictBtype")
                    {
                        List<EntityDicMicBacttype> dtIns = daAll[key] as List<EntityDicMicBacttype>;
                        foreach (var item in dtIns)
                        {
                            if (item.SpId.ToString() == drTpr.StTableId.ToString())
                            {
                                item.Checked = true;
                            }
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

            //在界面文本框显示条件
            frm.dtStat = dtStat;
            frm.BindGridView(daAll);
            dtStat = frm.dtStat;
            AdvanSql = frm.getWhere(cmbType.displayMember.Split('.')[0]);
            txtAdvanCondition = AdvanSql.BactypeNameString + AdvanSql.BacNameString + AdvanSql.AuditNameString +
                                               AdvanSql.DeptNameString + AdvanSql.SampleNameString + AdvanSql.OriNameString +
                                               AdvanSql.SampRemNameString + AdvanSql.SampStateNameString;
            txtWhere.Text = txtAdvanCondition;
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnPrintClicked(object sender, EventArgs e)
        {
            string tbName = xtraTabControl1.SelectedTabPage.Name;
            if (tbName == "j_Table")
            {
                if (xr != null)
                {
                    xr.Print();
                }
                return;
            }
            if (tbName == "j_Img")
            {
                charBac.Print();
                return;
            }
            if (tbName == "j_3D")
            {
                chartControl1.Print();
                return;
            }

        }

        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindGridView()
        {
            ProxyStatistical proxy = new ProxyStatistical();
            EntityResponse ds = proxy.Service.GetStatCache();
            daAll = ds.GetResult() as Dictionary<string, object>;
        }

        /// <summary>
        /// 设置列顺序
        /// </summary>
        private void setColumnSort(DataTable dt)
        {
            string sort = ConfigHelper.GetSysConfigValueWithoutLogin("BacilliBasicAnalyseColumnSort");
            if (string.IsNullOrEmpty(sort))
            {
                return;
            }
            int i = 0;
            try
            {
                string[] strList = sort.Split(',');
                if (strList.Length > 0)
                {
                    foreach (string s in strList)
                    {
                        if (dt.Columns.Contains(s))
                        {
                            dt.Columns[s].SetOrdinal(i);
                            i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.WriteException("加载结果浏览页列顺序出错", "lis.client.lab.PatControl.PatResult.setColumnSort", ex.Message);
            }

        }

        /// <summary>
        /// 细菌统计图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void charBac_ObjectHotTracked(object sender, HotTrackEventArgs e)
        {
            SeriesPoint point = e.AdditionalObject as SeriesPoint;

            if (point != null)
            {
                string pt = point.Tag.ToString();
                toolTipController1.ShowHint(pt);
            }
            else
            {
                toolTipController1.HideHint();
            }
        }

        /// <summary>
        /// 表明转换
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public string ConvertFromTableName(string tablename)
        {
            if (string.IsNullOrEmpty(tablename)) return string.Empty;
            string StTableName = string.Empty;
            #region 表名转换
            switch (tablename)
            {
                case "dict_depart":
                    StTableName = "dtDep";
                    break;
                case "dict_sample":
                    StTableName = "dtSam";
                    break;
                case "Dict_origin":
                    StTableName = "dtOri";
                    break;
                case "dict_bacteri":
                    StTableName = "dictBacteri";
                    break;
                case "dict_antibio":
                    StTableName = "dictAntibio";
                    break;
                case "dict_btype":
                    StTableName = "dictBtype";
                    break;
            }
            #endregion
            return StTableName;
        }

        #region Obsolete
        
        private void charImag(DataTable repImg, DataTable btName, ViewType vt, ChartControl cr, double inter, int spa, string[] str, bool isTrue)
        {
            ChartTitle ct = cr.Titles[0];
            DevExpress.XtraCharts.XYDiagram gg = new XYDiagram();
            //btName = repImg.DefaultView.ToTable(true, new string[] { "细菌名称", "细菌编码" });
            foreach (DataRow drBac in btName.Rows)
            {
                Series serie1 = new Series(drBac[str[0]].ToString(), vt);
                foreach (DataRow drImg in repImg.Rows)
                {
                    if (drImg[str[1]].ToString() == drBac[str[1]].ToString())
                    {
                        if (isTrue)
                        {
                            double av = 0;
                            if (drImg[str[3]] != null && drImg[str[3]].ToString() != "" && Convert.ToDouble(drImg[str[3]]) != 0)
                            {
                                av = (Convert.ToDouble(drImg[str[2]]) / Convert.ToDouble(drImg[str[3]])) * 100;
                            }


                            SeriesPoint points = new SeriesPoint(drImg[str[4]], new object[] { av.ToString("0") });
                            string x = drImg[str[7]].ToString() + "\r\n耐药菌株:" + drImg[str[8]] + " 总数:" + Convert.ToDouble(drImg[str[9]]).ToString("0");
                            points.Tag = x;
                            serie1.Points.Add(points);
                        }
                        else
                        {
                            SeriesPoint points = new SeriesPoint(drImg[str[2]], new object[] { drImg[str[3]] });
                            points.Tag = drImg[str[4]].ToString();
                            serie1.Points.Add(points);
                        }
                    }
                }
                cr.Series.Add(serie1);
            }
            ct.Text = str[5];
            if (vt == ViewType.Bar)
            {
                gg = cr.Diagram as DevExpress.XtraCharts.XYDiagram;
                gg.AxisX.Range.MaxValueInternal = Convert.ToDouble(inter);
                gg.AxisY.GridSpacing = spa;
                gg.AxisY.Title.Text = str[6];
            }
            else
            {
                XYDiagram3D gc = chartControl1.Diagram as DevExpress.XtraCharts.XYDiagram3D;
                gc.AxisY.GridSpacing = spa;
            }
        }

        #endregion

    }
}
