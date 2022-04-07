using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraTreeList.Nodes;
using dcl.client.frame;
using dcl.client.wcf;
using dcl.client.report;
using DevExpress.XtraCharts;
using dcl.entity;
using System.Linq;


namespace dcl.client.qc
{
    public partial class FrmQcDataAnalyse : FrmCommon
    {
        List<EntityDicInstrument> listLueApparatus = new List<EntityDicInstrument>();//用于存放测定仪器下拉控件过滤后的值
        List<EntityDicPubProfession> listLueType = new List<EntityDicPubProfession>();//存放实验组下拉控件过滤后的值

        public FrmQcDataAnalyse()
        {
            InitializeComponent();
        }

        private void FrmQcDataAnalyse_Load(object sender, EventArgs e)
        {
            dtBegin.DateTime = DateTime.Now.Date.AddMonths(-1);
            dtEnd.DateTime = DateTime.Now.Date;

            if (!UserInfo.isAdmin)
            {
                if (UserInfo.entityUserInfo.UserItrsQc.Count > 0)
                {
                    listLueApparatus = lue_Apparatus.getDataSource().FindAll(i => UserInfo.entityUserInfo.UserItrsQc.FindIndex(w => w.ItrId == i.ItrId) > -1);
                    //(测定仪器)加载时过滤
                    lue_Apparatus.SetFilter(listLueApparatus);
                }
                else
                    lue_Apparatus.SetFilter(new List<EntityDicInstrument>());
            }
            else
            {
                listLueApparatus = lue_Apparatus.getDataSource();
            }

            if (!UserInfo.isAdmin)
            {
                if (UserInfo.entityUserInfo.UserQcLab.Count > 0)
                {
                    //（实验组）加载时过滤
                    listLueType = lueType.getDataSource().FindAll(i => UserInfo.entityUserInfo.UserQcLab.FindIndex(w => w.LabId == i.ProId) > -1);
                    lueType.SetFilter(listLueType);
                }
                else
                    lueType.SetFilter(new List<EntityDicPubProfession>());
            }
            else
            {
                listLueType = lueType.getDataSource();
            }


            this.chartQCAnalyse.Series.Clear();

            sysToolBar.SetToolButtonStyle(new string[] { sysToolBar.BtnStat.Name, sysToolBar.BtnExport.Name, sysToolBar.BtnPrint.Name });
        }

        string userTypes = string.Empty;
        string userQcTypes = string.Empty;

        private void lue_Apparatus_ValueChanged(object sender, dcl.client.control.ValueChangeEventArgs args)
        {
            if (lue_Apparatus.valueMember != null && lue_Apparatus.valueMember != "" && dtBegin.EditValue != null && dtEnd.EditValue != null)
            {
                DateTime sDate = Convert.ToDateTime(this.dtBegin.EditValue);
                DateTime eDate = Convert.ToDateTime(this.dtEnd.EditValue).AddMonths(1).AddDays(1);

                QcTreeViewType showType = QcTreeViewType.多水平;  //默认 多水平
                if (rgType.EditValue?.ToString()=="1")  //选中 多项目
                    showType = QcTreeViewType.多项目;

                ProxyObrQcResult proxyQc = new ProxyObrQcResult();
                List<EntityQcTreeView> listQcTreeView = new List<EntityQcTreeView>();
                listQcTreeView = proxyQc.Service.GetQcTreeView(lue_Apparatus.valueMember, sDate, eDate, showType, false);

                this.tlQcItem.DataSource = listQcTreeView;

                tlQcItem.ExpandAll();
            }
        }

        private void lueType_ValueChanged(object sender, dcl.client.control.ValueChangeEventArgs args)
        {
            lue_Apparatus.displayMember = "";
            lue_Apparatus.valueMember = "";

            if (!string.IsNullOrEmpty(lueType.valueMember))
            {
                if (listLueApparatus.Count > 0)
                {
                    lue_Apparatus.SetFilter(listLueApparatus.FindAll(w => w.ItrLabId == lueType.valueMember));
                }
                else
                {
                    lue_Apparatus.SetFilter(new List<EntityDicInstrument>());
                }
            }
        }

        private void sysToolBar_OnBtnStatClicked(object sender, EventArgs e)
        {
            if (lue_Apparatus.valueMember == null || lue_Apparatus.valueMember.ToString().Trim() == "")
            {
                lis.client.control.MessageDialog.Show("请选择仪器！");
                return;
            }

            //List<QCItemAudit> listQCItem = new List<QCItemAudit>();
            List<EntityObrQcResultQC> listQCItem = new List<EntityObrQcResultQC>();
            for (int j = 0; j < tlQcItem.AllNodesCount; j++)
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tlQcItem.FindNodeByID(j);

                if (tn.Checked && tn["TvType"].ToString() == "1")//判断选中并且选中的为项目
                {
                    //QCItemAudit qcItem = new QCItemAudit();
                    EntityObrQcResultQC qcItem = new EntityObrQcResultQC();
                    qcItem.StateTime = dtBegin.DateTime;
                    qcItem.EndTime = dtEnd.DateTime;
                    qcItem.ItemId = tn["TvMatItmId"].ToString();
                    qcItem.QcParDetailId = tn["TvId"].ToString().Split('&')[0];
                    listQCItem.Add(qcItem);
                }
            }

            if (listQCItem.Count <= 0)
            {
                lis.client.control.MessageDialog.Show("请选择要统计的项目批号！");
                return;
            }
            ProxyObrQcResult proxyObrResult = new ProxyObrQcResult();
            List<EntityQcStatistic> listQcData = new List<EntityQcStatistic>();
            listQcData = proxyObrResult.Service.GetAnalyseData(listQCItem);

            gcData.DataSource = listQcData;

            this.chartQCAnalyse.Series.Clear();
            if (listQcData.Count > 0)
            {
                string item = listQcData[0].ITEM == null ? string.Empty : listQcData[0].ITEM;

                Series series = new Series(item, ViewType.Stock);
                Series series2 = new Series(item, ViewType.Line);
                series.Label.Visible = false;
                series2.Label.Visible = false;
                //必须设置ArgumentScaleType的类型，否则显示会转换为日期格式，导致不是希望的格式显示
                //也就是说，显示字符串的参数，必须设置类型为ScaleType.Qualitative
                series.ArgumentScaleType = ScaleType.Qualitative;//x轴类型
                series2.ArgumentScaleType = ScaleType.Qualitative;//x轴类型
                StockSeriesView lsv = ((StockSeriesView)series.View);
                lsv.LevelLineLength = 0;

                LineSeriesView lsv2 = ((LineSeriesView)series2.View);

                lsv2.LineMarkerOptions.Size = 5;
                lsv2.LineStyle.Thickness = 1;

                foreach (var infoQcData in listQcData)
                {
                    double dbAVG = infoQcData.ActualAVG;
                    double dbSD = infoQcData.ActualSD;

                    if (dbAVG != 0)
                    {
                        string month = infoQcData.MONTH == null ? string.Empty : infoQcData.MONTH;
                        SeriesPoint sp = new SeriesPoint(month, new double[] { dbAVG + dbSD, dbAVG - dbSD, 0, 0 });
                        series.Points.Add(sp);

                        SeriesPoint sp2 = new SeriesPoint(month, dbAVG);
                        series2.Points.Add(sp2);
                    }
                }
                this.chartQCAnalyse.Series.Add(series);
                this.chartQCAnalyse.Series.Add(series2);

                object maxValue = listQcData.Max(w => w.ActualAVG);//dtTable.Compute("Max(ActualAVG)", string.Empty);
                object minValue = listQcData.Min(w => w.ActualAVG); //("Min(ActualAVG)", string.Empty);

                XYDiagram gg = this.chartQCAnalyse.Diagram as XYDiagram;
                gg.AxisY.ConstantLines.Clear();//清空Y轴数值
                gg.AxisY.Range.MaxValue = 10000;
                gg.AxisY.Range.MinValue = 0;
                gg.AxisY.Range.MaxValue = Convert.ToDouble(maxValue) + 2;
                gg.AxisY.Range.MinValue = Convert.ToDouble(minValue) - 2;

            }
        }

        private void dtEnd_EditValueChanged(object sender, EventArgs e)
        {
            lue_Apparatus_ValueChanged(sender, null);
        }

        private void sysToolBar_OnBtnExportClicked(object sender, EventArgs e)
        {
            if (gcData.DataSource != null && ((List<EntityQcStatistic>)gcData.DataSource).Count > 0)
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
                        DevExpress.XtraPrinting.XlsxExportOptionsEx op = new DevExpress.XtraPrinting.XlsxExportOptionsEx();

                        op.ExportType = DevExpress.Export.ExportType.WYSIWYG;
                        bandedGridView1.OptionsPrint.PrintHeader = true;
                        bandedGridView1.OptionsPrint.AutoWidth = false;
                        bandedGridView1.ExportToXlsx(ofd.FileName.Trim(), op);


                        //gcData.ExportToXls(ofd.FileName.Trim());
                        lis.client.control.MessageDialog.Show("导出成功！", "提示");
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            else
            {
                lis.client.control.MessageDialog.Show("无数据导出！", "提示");
            }
        }

        private void tlQcItem_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            DevExpress.XtraTreeList.Nodes.TreeListNode tn = e.Node;
            SetCheckedChildNodes(tn, tn.Checked);
        }

        /// <summary>
        /// 递归选择子节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="check"></param>
        private void SetCheckedChildNodes(DevExpress.XtraTreeList.Nodes.TreeListNode node, bool check)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                node.Nodes[i].Checked = check;
                SetCheckedChildNodes(node.Nodes[i], check);
            }
        }

        private void bandedGridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            //DataRow dr = this.bandedGridView1.GetDataRow(e.RowHandle);
            EntityQcStatistic eyQcStatis = this.bandedGridView1.GetRow(e.RowHandle) as EntityQcStatistic;
            if (eyQcStatis != null && eyQcStatis.ActualFlag == 1)
            {
                e.Appearance.ForeColor = Color.Red;
            }
        }

        private void sysToolBar_OnBtnPrintClicked(object sender, EventArgs e)
        {
            if (gcData.DataSource != null)
            {
                //DataSet dsQcDataAnalyse = new DataSet();
                //DataTable dtQcdataAnalyse = (DataTable)gcData.DataSource;
                List<EntityQcStatistic> listQcdataAnalyse = this.gcData.DataSource as List<EntityQcStatistic>;

                DataTable dtQcdataAnalyse = EntityManager<EntityQcStatistic>.ConvertToDataTable(listQcdataAnalyse,"wf");
                dtQcdataAnalyse.TableName = "可设计字段";

                if (!dtQcdataAnalyse.Columns.Contains("图"))
                    dtQcdataAnalyse.Columns.Add("图");
                if (!dtQcdataAnalyse.Columns.Contains("物理组"))
                {
                    dtQcdataAnalyse.Columns.Add("物理组");
                }
                dtQcdataAnalyse.Columns.Add("偏倚");
                dtQcdataAnalyse.Columns.Add("失控率");
                dtQcdataAnalyse.Columns.Add("可接受范围");
                dtQcdataAnalyse.Columns.Add("是否符合");
                string file = Application.StartupPath + "\\Temp";
                addFile(file);
                string fileName = Application.StartupPath + "\\Temp\\qc_" + Guid.NewGuid().ToString() + ".jpg";
                //this.chartQCAnalyse.Dock = DockStyle.None; //如果不注释的话，在点击打印时会将图片隐藏掉（跟下DockStyle.Top是一起用的）
                this.chartQCAnalyse.Dock = DockStyle.Top;
                this.chartQCAnalyse.Width = 1033;
                this.chartQCAnalyse.Height = 167;
                this.chartQCAnalyse.ExportToImage(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                //this.chartQCAnalyse.Dock = DockStyle.Top;

                foreach (DataRow dr in dtQcdataAnalyse.Rows)
                {
                    dr["图"] = fileName;
                    dr["物理组"] = lueType.displayMember;
                    double AVG = Convert.ToDouble(dr["AVG"]);
                    dr["AVG"] = AVG.ToString("0.00");
                    dr["SD"] = Convert.ToDouble(dr["SD"]).ToString("0.00");
                    dr["CV"] = Convert.ToDouble(dr["CV"]).ToString("0.00");
                    double MatItmX = Convert.ToDouble(dr["Rmatdet_itm_x"]);
                    int OutControlNumber = Convert.ToInt32(dr["失控数"]);
                    int N = Convert.ToInt32(dr["N"]);
                    dr["偏倚"] = ((AVG - MatItmX) / MatItmX).ToString("P");
                    dr["失控率"] = (OutControlNumber / N).ToString("P");
                    
                    if (dr["Rmatdet_allow_cv"] != DBNull.Value)
                    {
                        dr["可接受范围"] = "靶值±" + Convert.ToDouble(dr["Rmatdet_allow_cv"])+"%";
                        dr["是否符合"] = Math.Abs((AVG - MatItmX) / MatItmX)*100 >
                        Math.Abs(Convert.ToDouble(dr["Rmatdet_allow_cv"])) ? "不符合" : "符合";
                    }
                }
                

               //dsQcDataAnalyse.Tables.Add(dtQcdataAnalyse.Copy());
               //FrmReportPrint.Print(dsQcDataAnalyse, "质控_数据统计分析.repx");

               DataSet ds = new DataSet();
                ds.Tables.Add(dtQcdataAnalyse);

                EntityDCLPrintData printData = new EntityDCLPrintData();
                printData.ReportName = "质控_数据统计分析";
                printData.ReportData = ds;

                DCLReportPrint.PrintByData(printData);

            }
        }
        /// <summary>
        /// 判断文件夹是否存在
        /// </summary>
        /// <param name="path">文件夹的路径</param>
        private void addFile(string path)
        {
            if (Directory.Exists(path) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(path);
            }
        }
        private void sysToolBar_OnPrintPreviewClicked(object sender, EventArgs e)
        {
            if (gcData.DataSource != null)
            {
                DataSet dsQcDataAnalyse = new DataSet();
                //DataTable dtQcdataAnalyse = (DataTable)gcData.DataSource;
                List<EntityQcStatistic> listQcdataAnalyse = this.gcData.DataSource as List<EntityQcStatistic>;
                DataTable dtQcdataAnalyse = EntityManager<EntityQcStatistic>.ConvertToDataTable(listQcdataAnalyse,"wf");
                dtQcdataAnalyse.TableName = "可设计字段";

                if (!dtQcdataAnalyse.Columns.Contains("图"))
                {
                    dtQcdataAnalyse.Columns.Add("图");
                }
                if (!dtQcdataAnalyse.Columns.Contains("物理组"))
                {
                    dtQcdataAnalyse.Columns.Add("物理组");
                }
                dtQcdataAnalyse.Columns.Add("偏倚");
                dtQcdataAnalyse.Columns.Add("失控率");
                dtQcdataAnalyse.Columns.Add("可接受范围");
                dtQcdataAnalyse.Columns.Add("是否符合");
                string file = Application.StartupPath + "\\Temp";
                addFile(file);
                string fileName = Application.StartupPath + "\\Temp\\qc_" + Guid.NewGuid().ToString() + ".jpg";
                //this.chartQCAnalyse.Dock = DockStyle.None;  //如果不注释的话，在点击打印预览时会将图片隐藏掉（跟下DockStyle.Top是一起用的）
                this.chartQCAnalyse.Dock = DockStyle.Top;
                this.chartQCAnalyse.Width = 1033;
                this.chartQCAnalyse.Height = 167;
                this.chartQCAnalyse.ExportToImage(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                //this.chartQCAnalyse.Dock = DockStyle.Top;

                foreach (DataRow row in dtQcdataAnalyse.Rows)
                {
                    row["图"] = fileName;
                    row["物理组"] = lueType.displayMember;
                    double AVG = Convert.ToDouble(row["AVG"]);
                    row["AVG"] = AVG.ToString("0.00");
                    row["SD"] = Convert.ToDouble(row["SD"]).ToString("0.00");
                    row["CV"] = Convert.ToDouble(row["CV"]).ToString("0.00");
                    double MatItmX = Convert.ToDouble(row["Rmatdet_itm_x"]);
                    int OutControlNumber = Convert.ToInt32(row["失控数"]);
                    int N = Convert.ToInt32(row["N"]);
                    row["偏倚"] = ((AVG - MatItmX) / MatItmX).ToString("P");
                    row["失控率"] = (OutControlNumber / N).ToString("P");
                    if (row["Rmatdet_allow_cv"] != DBNull.Value)
                    {
                        row["可接受范围"] = "靶值±" + Convert.ToDouble(row["Rmatdet_allow_cv"])+"%";
                        row["是否符合"] = Math.Abs((AVG - MatItmX) / MatItmX) * 100 >
                            Math.Abs(Convert.ToDouble(row["Rmatdet_allow_cv"])) ? "不符合" : "符合";
                    }
                    
                }

                //dsQcDataAnalyse.Tables.Add(dtQcdataAnalyse.Copy());
                //FrmReportPrint.PrintPreview(dsQcDataAnalyse, "质控_数据统计分析.repx");

                dsQcDataAnalyse.Tables.Add(dtQcdataAnalyse);

                EntityDCLPrintData printData = new EntityDCLPrintData();
                printData.ReportName = "质控_数据统计分析";
                printData.ReportData = dsQcDataAnalyse;

                //DCLReportPrint.PrintByData(printData);
                DCLReportPrint.PrintPreviewByData(printData);
            }
        }

        private void checkEditAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int j = 0; j < tlQcItem.AllNodesCount; j++)
            {
                TreeListNode tn = tlQcItem.FindNodeByID(j);
                tn.Checked = checkEditAll.Checked;
            }
        }

        private void rgType_EditValueChanged(object sender, EventArgs e)
        {
            string str = rgType.EditValue?.ToString();
            if(!string.IsNullOrEmpty(str))
                lue_Apparatus_ValueChanged(null, null);
        }
    }
}
