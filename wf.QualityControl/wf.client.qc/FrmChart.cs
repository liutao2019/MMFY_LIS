using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using DevExpress.XtraCharts;
using System.IO;
using dcl.client.report;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using dcl.client.common;
using lis.client.control;
using DevExpress.XtraTreeList.Nodes;
using dcl.client.wcf;
using DevExpress.XtraTreeList;
using Lib.LogManager;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.qc
{
    public partial class FrmChart : FrmCommon
    {
        public FrmChart()
        {
            InitializeComponent();
        }

        //string strType;
        public FrmChart(string itr_id, string itr_mid)
        {
            InitializeComponent();
            try
            {
                this.lue_Apparatus.valueMember = itr_id;
                this.lue_Apparatus.displayMember = itr_mid;
                this.lue_Apparatus.Readonly = true;
                this.lueType.Readonly = true;
            }
            catch (Exception ex)
            {
                Logger.LogException(string.Format("public FrmChart('{0}')", itr_mid), ex);
            }
        }

        List<EntityObrQcResult> dtMonica = new List<EntityObrQcResult>();
        List<EntityObrQcResult> listQcResultFilter = new List<EntityObrQcResult>();
        bool istrueSingle = true;//用于判断是单水平显示还是多水平显示
        bool isTwoAuditPower = false;//是否拥有二次审核权限
        bool blQcAuditMode = false;//是否启用审核模式
        bool isAllowMuchItemAuditBDL = false;//允许半定量质控多项目审核

        //存放实验组权限过滤后的数据
        private List<EntityDicPubProfession> listLueType = new List<EntityDicPubProfession>();

        private void FrmChart_Load(object sender, EventArgs e)
        {
            this.tlQcItem.AfterCheckNode += new DevExpress.XtraTreeList.NodeEventHandler(this.tlQcItem_AfterCheckNode);
            this.tlQcItem.FocusedNodeChanged += new FocusedNodeChangedEventHandler(this.tlQcItem_AfterFocusedNode);
            this.checkEditAll.CheckedChanged += new System.EventHandler(this.checkEditAll_CheckedChanged);
            this.gvLot.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gridView1_RowStyle);
            this.gvLot.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvLot_ShowingEditor);
            this.gvLot.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gvLot_MouseMove);
            this.gvLot.DoubleClick += new System.EventHandler(this.gvLot_DoubleClick);

            base.ShowSucessMessage = false;
            if (ConfigHelper.GetSysConfigValueWithoutLogin("HospitalName") == "中山三院")
            {
                dtBegin.EditValue = DateTime.Now.Date.ToString("yyyy/MM/1");
            }
            else
            {
                dtBegin.EditValue = DateTime.Now.Date.AddMonths(-1);
            }
            dtEnd.EditValue = DateTime.Now.Date;

            #region 实验组权限过滤
            if (!UserInfo.isAdmin)
            {
                if (UserInfo.entityUserInfo.UserQcLab.Count > 0)
                {
                    listLueType = this.lueType.getDataSource().FindAll(w => UserInfo.entityUserInfo.UserQcLab.FindIndex(i => i.LabId == w.ProId) > -1);
                    this.lueType.SetFilter(listLueType);
                }
                else
                    this.lueType.SetFilter(new List<EntityDicPubProfession>());
            }
            else
            {
                listLueType = this.lueType.getDataSource();
                this.lueType.SetFilter(listLueType);
            }
            #endregion

            lue_Apparatus.Focus();
            lue_Apparatus.SetFilter(new List<EntityDicInstrument>());

            sysToolBar1.BtnQualityData.Caption = "取消审核";
            sysToolBar1.BtnQualityRule.Caption = "二次审核";
            sysToolBar1.BtnExport.Caption = "导出统计";
            sysToolBar1.BtnSaveDefault.Caption = "导出数据";
            sysToolBar1.BtnResultView.Caption = "导出最后测定数据";
            sysToolBar1.btnReturn.Caption = "质控评价";
            sysToolBar1.BtnResultView.Glyph = sysToolBar1.BtnExport.Glyph;
            blQcAuditMode = UserInfo.GetSysConfigValue("QCAuditMode") == "启用";
            xtProcess.PageVisible = blQcAuditMode;

            string btnUndoAuti = UserInfo.HaveFunction("lis.client.qc.FrmChart", "QCUndoAuti") ? sysToolBar1.BtnQualityData.Name : "";

            updata_SimpleButton.Visible = UserInfo.HaveFunction("dcl.client.qc.FrmChart", "UpdateDict");


            isTwoAuditPower = UserInfo.HaveFunctionByCode("FrmChart_TwoAudit");
            string btnTwoAudit = isTwoAuditPower ? sysToolBar1.BtnQualityRule.Name : string.Empty;
            sysToolBar1.SetToolButtonStyle(new string[] { "BtnQualityImage",
                                                            "BtnPrint",
                                                            "BtnQualityTest",
                                                            "BtnQualityAudit",
                                                            btnUndoAuti,
                                                            btnTwoAudit,
                                                            sysToolBar1.BtnPageUp.Name,
                                                            sysToolBar1.BtnPageDown.Name,
                                                            sysToolBar1.BtnExport.Name,
                                                            sysToolBar1.BtnSaveDefault.Name,
                                                            "btnBrowse",
                                                            sysToolBar1.BtnClose.Name,
                                                            sysToolBar1.BtnRefresh.Name,    //新增刷新按钮
                                                            sysToolBar1.btnReturn.Name });//
            sysToolBar1.BtnQualityRule.Enabled = !blQcAuditMode;
            sysToolBar1.BtnQualityAudit.Enabled = !blQcAuditMode;
            qcm_next_time.Visible = isTwoAuditPower;
            sysToolBar1.BtnBrowseClick += new EventHandler(sysToolBar1_BtnBrowseClick);
            sysToolBar1.btnBrowse.Caption = "修改记录";
            if (UserInfo.GetSysConfigValue("QC_ShowQCLog") == "是")
            {
                sysToolBar1.btnBrowse.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            DataTable dt = CommonClient.CreateDT(new string[] { "dtId", "dtName" }, "DataType");
            dt.Rows.Add("0", "仪器");
            dt.Rows.Add("1", "手工");
            repositoryItemLookUpEdit1.DataSource = dt;

            if (UserInfo.GetSysConfigValue("QCVisibleQcmType") == "是")
            {
                gridColumn2.Visible = true;
            }

            if (UserInfo.GetSysConfigValue("QCImageModify") == "允许")
                this.chartControl1.ObjectSelected += new DevExpress.XtraCharts.HotTrackEventHandler(chartControl1_ObjectSelected);
        }

        void sysToolBar1_BtnBrowseClick(object sender, EventArgs e)
        {
            frmQcDataModifyHistory history = new frmQcDataModifyHistory();
            history.ShowDialog();
        }

        /// <summary>
        /// 显示质控图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                sysToolBar1.Focus();
                if (dtBegin.EditValue == null || dtEnd.EditValue == null)
                {
                    lis.client.control.MessageDialog.Show("起始日期不能为空！", "提示");
                    dtBegin.Focus();
                    return;
                }
                showItemsView();
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show(ex.Message, "提示");
            }
        }

        /// <summary>
        /// 质控图项目显示方法
        /// </summary>
        private void showItemsView()
        {
            showItemsView(tabSub.SelectedTabPage == xtProcess);
        }

        /// <summary>
        /// 质控图项目显示方法
        /// </summary>
        private void showItemsView(bool audit)
        {
            chartControl1.Visible = true;
            emptyQcDate();

            int type = 0;//用于统计所选项目的个数
            double[] sdb = new double[] { 0, 0, 0, 0, 0 };//用于存放ave、sd、ccv
            StringBuilder itemNames = new StringBuilder();
            List<EntityObrQcResultQC> listQCItem = new List<EntityObrQcResultQC>();
            List<String> listMatSnItmId = new List<String>();

            TreeList tlQc = new TreeList();
            if (audit)
                tlQc = tlQcAuditItem;
            else
                tlQc = tlQcItem;

            for (int j = 0; j < tlQc.AllNodesCount; j++)
            {
                TreeListNode tl = tlQc.FindNodeByID(j);

                EntityQcTreeView tn = (EntityQcTreeView)tlQc.GetDataRecordByNode(tl);

                bool tn_Checked = tl.Checked;  //true
                string tn_type = tn.TvType; // 为0
                bool cetype = ceType_Radar.Checked; //false

                if (tl.Checked && (tn.TvType == "1" || (ceType_Radar.Checked && tn.TvType == "2")))//判断选中并且选中的为项目
                {
                    if (ceType_MC.Checked && tn.TvMatItmCcv == null)
                    {//显示图像为Monica时，判断是否存在CCV标准
                        lis.client.control.MessageDialog.Show(tn.TvName + "项目无CCV标准,无法显示Monica图!", "提示");
                        return;
                    }
                    if (ceType_BDL.Checked && (tn.TvMatMaxValue == null || tn.TvMatMinValue == null))
                    {//显示图像为半定量时，判断是否存在最大、最小值
                        lis.client.control.MessageDialog.Show(tn.TvName + "项目无最大、最小值,无法显示半定量图!", "提示");
                        return;
                    }
                    itemNames.Append(string.Format(" or (qcm_itm_ecd='{0}' and qcm_id='{1}')", tn.TvMatItmId, tn.TvId.Split('&')[0]));
                    EntityObrQcResultQC qcItem = new EntityObrQcResultQC();
                    qcItem.StateTime = dtBegin.DateTime;
                    qcItem.EndTime = dtEnd.DateTime;
                    qcItem.ItemId = tn.TvMatItmId;
                    qcItem.QcParDetailId = tn.TvId.Split('&')[0];
                    qcItem.IsCheckGrubbs = tn.TvMatRuleId == "Grubbs法";
                    listQCItem.Add(qcItem);
                    if (type == 0)//得到选中的第一个项目的标准差等信息作为标准
                    {
                        if (tn.TvMatRuleId != "Grubbs法")
                        {
                            try
                            {
                                sdb[0] = Convert.ToDouble(tn.TvMatItmX);
                                sdb[1] = Convert.ToDouble(tn.TvMatItmSd);
                            }
                            catch (Exception) { }
                        }
                        if (tn.TvMatItmCcv != null)
                        {
                            try
                            {
                                sdb[2] = Convert.ToDouble(tn.TvMatItmCcv);
                            }
                            catch (Exception) { }
                        }
                        if (ceType_BDL.Checked)
                        {
                            try
                            {
                                sdb[3] = Convert.ToDouble(tn.TvMatMaxValue);
                                sdb[4] = Convert.ToDouble(tn.TvMatMinValue);
                            }
                            catch (Exception) { }
                        }
                    }
                    type++;
                    //string[] strParRule = tn.TvId.Split('&');strParRule[0], strParRule[1]
                    listMatSnItmId.Add(tn.TvId);
                }

            }

            if (type == 0)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("请选择质控项目！");
                this.chartControl1.Series.Clear();
                return;
            }


            if (itemNames.Length > 0)
                itemNames.Remove(0, 3);
            else
                return;

            if (ceType_BDL.Checked && type > 1 && !isAllowMuchItemAuditBDL)
            {
                lis.client.control.MessageDialog.Show("半定量质控图无法显示多条！", "提示");
                return;
            }
            isAllowMuchItemAuditBDL = false;

            ProxyObrQcResult proxyQC = new ProxyObrQcResult();
            List<EntityObrQcResult> dtOriginalItem = proxyQC.Service.GetQcResult(listQCItem);

            DataTable Items = new DataTable();//用于放入审核过的结果

            if (type > 1)
                istrueSingle = false;
            else
                istrueSingle = true;

            this.chartControl1.Series.Clear();//清空历史图

            if (listMatSnItmId.Count > 1)
                dtOriginalItem = QCAuditMoreRule(dtOriginalItem, listMatSnItmId);

            //用于放入统计数据
            DataTable dtStatistics = CommonClient.CreateDT(new string[] { "type_id", "stItem", "stAve", "stSD", "stCV", "stItemId", "stDetailId", "stNo", "qcr_reag_manu", "qcr_m_pro" }, "statistics");
            dtStatistics.Columns.Add("stActualN", typeof(int));
            dtStatistics.Columns.Add("stActualAve", typeof(double));
            dtStatistics.Columns.Add("stActualSD", typeof(double));
            dtStatistics.Columns.Add("stActualCV", typeof(double));
            dtStatistics.Columns.Add("qcr_reag_date", typeof(DateTime));
            dtStatistics.Columns.Add("stAllowCV", typeof(double));

            List<EntityObrQcResult> dtQcValue = new List<EntityObrQcResult>();// dtOriginalItem.Clone();

            int i = 0;
            Series xSeries = new Series();

            if (ceType_Radar.Checked)
            {
                for (int j = 0; j < tlQc.AllNodesCount; j++)
                {
                    TreeListNode tlCombine = tlQc.FindNodeByID(j);

                    EntityQcTreeView tnCombine = (EntityQcTreeView)tlQc.GetDataRecordByNode(tlCombine);

                    if (tlCombine.Checked && tnCombine.TvType == "2")
                    {
                        Series series = new Series(tnCombine.TvName, ViewType.RadarLine);
                        series.Label.Visible = false;

                        RadarLineSeriesView lsv = ((RadarLineSeriesView)series.View);

                        lsv.LineMarkerOptions.Size = 5;
                        lsv.LineStyle.Thickness = 1;

                        for (int z = 0; z < tlQc.AllNodesCount; z++)
                        {
                            TreeListNode tl = tlQc.FindNodeByID(z);

                            EntityQcTreeView tn = (EntityQcTreeView)tlQc.GetDataRecordByNode(tl);

                            if (tl.Checked && tn.TvType == "1" && tn.TvParentId == tnCombine.TvId) //
                            {
                                string strFilter = string.Format("qcm_itm_ecd='{0}' and qcm_id='{1}' ", tn.TvMatItmId, tn.TvId.Split('&')[0]);//and qcr_nsd is not null



                                List<EntityObrQcResult> drQcValues = dtOriginalItem.FindAll(w => w.QresItmId == tn.TvMatItmId && w.QresMatDetId == tn.TvId.Split('&')[0])
                                                                                   .OrderByDescending(o => o.QresDate).ToList();


                                if (drQcValues.Count > 0)
                                {
                                    List<EntityObrQcResult> dtCalculation = QcAudit(drQcValues, tn.TvId, null);//审核数据


                                    dtQcValue.AddRange(dtCalculation);

                                    if (dtCalculation.Count > 0)
                                    {

                                        series.Points.Add(getRadarPoint(dtCalculation[dtCalculation.Count - 1]));

                                        DataRow drSta = dtStatistics.NewRow();
                                        drSta["type_id"] = tn.TvId;
                                        if (tn.TvMatItmX != null)
                                            drSta["stAve"] = tn.TvMatItmX;
                                        if (tn.TvMatItmSd != null)
                                            drSta["stSD"] = tn.TvMatItmSd;
                                        if (tn.TvMatItmCv != null)
                                            drSta["stCV"] = tn.TvMatItmCv;
                                        if (tn.TvMatAllowCv != null)
                                            drSta["stAllowCV"] = tn.TvMatAllowCv;


                                        try
                                        {
                                            drSta["qcr_m_pro"] = tn.TvMatMPro;
                                            drSta["qcr_reag_date"] = tn.TvMatReadValidDate;
                                        }
                                        catch (Exception)
                                        {
                                        }

                                        Statistics(drSta, dtCalculation);
                                        dtStatistics.Rows.Add(drSta);
                                    }
                                }
                            }
                        }
                        this.chartControl1.Series.Add(series);
                    }
                }
            }
            else
            {
                List<DateTime> listTime = new List<DateTime>();

                dtOriginalItem = dtOriginalItem.OrderBy(w => w.QresDate).ToList();

                //时间排序
                foreach (EntityObrQcResult item in dtOriginalItem)
                {
                    DateTime qcTime = item.QresDate.Date;
                    if (!listTime.Contains(qcTime) && qcTime >= dtBegin.DateTime.Date)
                        listTime.Add(qcTime);
                }

                //按照时间添加一根默认时间线，用来解决多个项目时间错乱问题。
                foreach (DateTime time in listTime)
                {
                    xSeries.ArgumentScaleType = ScaleType.Qualitative;
                    SeriesPoint point = new SeriesPoint(time.ToString("MM-dd"), 0);
                    xSeries.Points.Add(point);
                }
                this.chartControl1.Series.Add(xSeries);


                for (int jk = 0; jk < tlQc.AllNodesCount; jk++)
                {
                    TreeListNode tl = tlQc.FindNodeByID(jk);

                    EntityQcTreeView tn = (EntityQcTreeView)tlQc.GetDataRecordByNode(tl);

                    if (tl.Checked && tn.TvType == "1")
                    {
                        DataRow drSta = dtStatistics.NewRow();
                        drSta["type_id"] = tn.TvId;
                        if (tn.TvMatItmX != null)
                            drSta["stAve"] = tn.TvMatItmX;
                        if (tn.TvMatItmSd != null)
                            drSta["stSD"] = tn.TvMatItmSd;
                        if (tn.TvMatItmCv != null)
                            drSta["stCV"] = tn.TvMatItmCv;
                        drSta["qcr_reag_manu"] = tn.TvMatReagManufacturer;
                        if (tn.TvMatAllowCv != null)
                            drSta["stAllowCV"] = tn.TvMatAllowCv;
                        try
                        {
                            drSta["qcr_m_pro"] = tn.TvMatMPro;
                            drSta["qcr_reag_date"] = tn.TvMatReadValidDate;
                        }
                        catch (Exception)
                        {
                        }

                        Series series = new Series(tn.TvName, ViewType.Line);
                        series.ArgumentScaleType = ScaleType.Qualitative;
                        LineSeriesView lsv = ((LineSeriesView)series.View);
                        lsv.MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
                        lsv.LineMarkerOptions.Size = 5;
                        lsv.LineStyle.Thickness = 1;
                        if (cePointType.Checked)
                        {
                            lsv.LineMarkerOptions.Kind = (DevExpress.XtraCharts.MarkerKind)i;
                            lsv.LineMarkerOptions.Size = 8;
                        }

                        List<EntityObrQcResult> drQcValues = dtOriginalItem.FindAll(w => w.QresItmId == tn.TvMatItmId && w.QresMatDetId == tn.TvId.Split('&')[0])
                                                                           .OrderBy(w => w.QresDate).ToList();

                        EntityObrQcResultQC qcItemTwo = null;
                        if (listQCItem.Count == 2)
                        {
                            foreach (EntityObrQcResultQC qcItem in listQCItem)
                            {
                                if (qcItem.ItemId.Trim() != tn.TvMatItmId
                                    || qcItem.QcParDetailId != tn.TvId.Split('&')[0])
                                    qcItemTwo = qcItem;
                            }
                        }

                        if (drQcValues.Count > 0)
                        {
                            if (tn.TvMatRuleId == "Grubbs法")
                            {
                                if (drQcValues[drQcValues.Count - 1].MatItmX == null)
                                {
                                    //靶值与SD精确几位小数
                                    if (ConfigHelper.GetSysConfigValueWithoutLogin("QC_AVGandSDtoDouble") == "3")
                                    {
                                        try
                                        {
                                            sdb[0] = Convert.ToDouble(Convert.ToDouble(drQcValues[drQcValues.Count - 1].QresValue).ToString("0.000"));
                                        }
                                        catch (Exception) { }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            sdb[0] = Convert.ToDouble(Convert.ToDouble(drQcValues[drQcValues.Count - 1].QresValue).ToString("0.00"));
                                        }
                                        catch (Exception) { }
                                    }
                                    sdb[1] = 1;
                                }
                                else
                                {
                                    //靶值与SD精确几位小数
                                    if (ConfigHelper.GetSysConfigValueWithoutLogin("QC_AVGandSDtoDouble") == "3")
                                    {
                                        try
                                        {
                                            sdb[0] = Convert.ToDouble(Convert.ToDouble(drQcValues[drQcValues.Count - 1].MatItmX).ToString("0.000"));
                                            sdb[1] = Convert.ToDouble(Convert.ToDouble(drQcValues[drQcValues.Count - 1].MatItmSd).ToString("0.000"));
                                        }
                                        catch (Exception) { }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            sdb[0] = Convert.ToDouble(Convert.ToDouble(drQcValues[drQcValues.Count - 1].MatItmX).ToString("0.00"));
                                            sdb[1] = Convert.ToDouble(Convert.ToDouble(drQcValues[drQcValues.Count - 1].MatItmSd).ToString("0.00"));
                                        }
                                        catch (Exception) { }
                                    }
                                }
                            }

                            List<EntityObrQcResult> dtCalculation = QcAudit(drQcValues, tn.TvId, qcItemTwo);//审核数据

                            //质控物水平按启用时间过滤已审核的数据
                            if (UserInfo.GetSysConfigValue("QC_RunTheStartTime") == "是")
                            {
                                if (tn.TvMatDateStart != null)
                                {
                                    dtCalculation = dtCalculation.FindAll(w => w.QresDate > tn.TvMatDateStart.Value || w.QresAuditFlag == 0);
                                }
                            }

                            dtQcValue.AddRange(dtCalculation);
                            if (dtCalculation.Count > 0)
                                Statistics(drSta, dtCalculation);

                            #region 固化靶值与SD(取最早时间已审核的)
                            if (tn.TvMatRuleId != "Grubbs法" && dtCalculation != null && dtCalculation.Count > 0)
                            {
                                //固化靶值与SD(取最早时间已审核的)
                                //系统配置：质控图JL固化靶值与标准差
                                if (ConfigHelper.GetSysConfigValueWithoutLogin("QC_ChartLJSolidCxSd") == "是")
                                {
                                    if (dtCalculation[0].QresAuditFlag.ToString() == "1")
                                    {
                                        //靶值与SD精确几位小数
                                        if (ConfigHelper.GetSysConfigValueWithoutLogin("QC_AVGandSDtoDouble") == "3")
                                        {
                                            try
                                            {
                                                sdb[0] = Convert.ToDouble(Convert.ToDouble(dtCalculation[0].QresItmX).ToString("0.000"));
                                                sdb[1] = Convert.ToDouble(Convert.ToDouble(dtCalculation[0].QresItmSd).ToString("0.000"));
                                            }
                                            catch (Exception) { }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                sdb[0] = Convert.ToDouble(Convert.ToDouble(dtCalculation[0].QresItmX).ToString("0.00"));
                                                sdb[1] = Convert.ToDouble(Convert.ToDouble(dtCalculation[0].QresItmSd).ToString("0.00"));
                                            }
                                            catch (Exception) { }
                                        }

                                        if (drSta != null && drSta.Table.Columns.Contains("stAve") && drSta.Table.Columns.Contains("stSD"))
                                        {
                                            drSta["stAve"] = sdb[0];
                                            drSta["stSD"] = sdb[1];
                                        }
                                    }
                                }
                            }
                            #endregion

                            List<EntityObrQcResult> listPointView = getDate(sdb, series, dtCalculation);//根据数据画线

                            Color c = series.Points[0].Color;

                            if (ceSerieType.Checked && !ceType_Radar.Checked)
                            {
                                List<EntityObrQcResult> drQcDatas = dtCalculation.FindAll(w => w.QresRunawayFlag == "2");
                                if (drQcDatas.Count > 0)
                                    getSK(drQcDatas, lsv.LineMarkerOptions.Kind, c);
                            }
                            if (cePointLast.Checked)
                            {
                                if (listPointView.Count > 0)
                                    getSK(listPointView, lsv.LineMarkerOptions.Kind, c);
                            }

                            series.Label.Visible = false;

                            lsv.Color = c;

                            this.chartControl1.Series.Add(series);

                            i++;
                            if (i > 9)
                                i = 0;
                            dtStatistics.Rows.Add(drSta);
                        }
                    }
                }
            }



            DataView dv = dtStatistics.DefaultView.Table.DefaultView;
            dv.Sort = " stItemId asc";
            DataTable dtStatisticsTemp = dv.ToTable();

            gcInfo.DataSource = dtStatisticsTemp; //gcInfo是右侧统计的数据源

            List<EntityDicQcConvert> dtQcRange = new List<EntityDicQcConvert>();

            if (ceType_BDL.Checked)
                dtQcRange = proxyQC.Service.GetQcConvert(lue_Apparatus.valueMember, listQCItem[0].ItemId);

            if (istrueSingle)
                getRange(sdb[0], sdb[1], sdb[2], sdb[3], sdb[4], dtQcRange);//画出质控图范围
            else
                getRange(0, 1, 0, 0, 0, dtQcRange);

            //bsGcData数据源是右侧"结果"的数据源
            bsGcData.DataSource = dtQcValue;//显示结果值
            listQcResultFilter = EntityManager<EntityObrQcResult>.ListClone(dtQcValue);

            if (ckShowType_S.Checked)
            {
                qcm_c_no.GroupIndex = 0;
                itm_ecd.GroupIndex = -1;
            }
            else
            {
                qcm_c_no.GroupIndex = -1;
                itm_ecd.GroupIndex = 0;
            }

            qcm_info_group.GroupIndex = 1;
        }


        private void Statistics(DataRow drSta, List<EntityObrQcResult> dtCalculation)
        {
            List<EntityObrQcResult> listMath = dtCalculation.FindAll(w => w.QresDisplay == 0);

            if (ceSet_con.Checked)
                listMath = listMath.FindAll(w => string.IsNullOrEmpty(w.QresRunawayFlag) || w.QresRunawayFlag != "2");
            if (ceSet_Sd.Checked)
            {
                if (txtSet != null && txtSet.ToString().Trim() != "")
                {
                    try
                    {
                        double sdCount = Math.Abs(Convert.ToDouble(txtSet.EditValue));
                        listMath = listMath.FindAll(w => w.NSD < sdCount && w.NSD > -sdCount);
                    }
                    catch (Exception)
                    { }
                }
            }

            if (listMath.Count > 0)
            {
                double stActualAve = listMath.Average(w => w.FinalValue);

                double stActualSum = listMath.Sum(w => Math.Pow(w.FinalValue - stActualAve, 2));

                double stActualSD = Math.Sqrt(stActualSum / listMath.Count()); //EntityFunctions.StandardDeviation(from o in listMath select o.FinalValue);

                stActualSD = stActualSD == null ? 0 : stActualSD;

                double stActualCV = 0;
                if (Convert.ToDouble(stActualAve) > 0)
                    stActualCV = (Convert.ToDouble(stActualSD) / Convert.ToDouble(stActualAve)) * 100;
                drSta["stActualAve"] = Convert.ToDouble(stActualAve).ToString("0.000");
                drSta["stActualSD"] = Convert.ToDouble(stActualSD).ToString("0.000");
                drSta["stActualCV"] = stActualCV.ToString("0.00");
                drSta["stActualN"] = listMath.Count;
                EntityObrQcResult drCalculation = dtCalculation[0];
                drSta["stItem"] = drCalculation.MatLevel.ToString() + " - " + drCalculation.MatBatchNo.ToString() + " - " + drCalculation.ItmEcode.ToString();
                drSta["stItemId"] = drCalculation.QresItmId.ToString();
                drSta["stDetailId"] = drCalculation.MatBatchNo.ToString();
                drSta["stNo"] = drCalculation.MatLevel.ToString();
            }
        }


        private DataTable GetStatistics(DataTable dtStatistics)
        {
            if (dtStatistics.Rows.Count > 1)
            {
                DataTable dtNo = dtStatistics.Clone();
                foreach (DataRow drStatistics in dtStatistics.Rows)
                {
                    string filter = string.Format("stItemId='{0}' and stDetailId='{1}' and stNo like '{2}%'",
                                    drStatistics["stItemId"].ToString(), drStatistics["stDetailId"].ToString(), drStatistics["stNo"].ToString().Substring(0, 1));

                    string[] stItem = drStatistics["stItem"].ToString().Split('-');

                    string strTypeId = stItem[0].Substring(0, 1) + " -" + stItem[1] + "-" + stItem[2]; ;

                    if (dtNo.Select("type_id='" + strTypeId + "'").Length <= 0 && dtStatistics.Select(filter).Length > 1)
                    {
                        object stActualAve = dtStatistics.Compute("avg(stActualAve)", filter);
                        object stActualSD = dtStatistics.Compute("avg(stActualSD)", filter);
                        object stActualCV = dtStatistics.Compute("avg(stActualCV)", filter);
                        object stActualN = dtStatistics.Compute("sum(stActualN)", filter);
                        dtNo.Rows.Add(drStatistics.ItemArray);
                        DataRow dr = dtNo.Rows[dtNo.Rows.Count - 1];
                        dr["type_id"] = strTypeId;
                        dr["stItem"] = strTypeId;
                        dr["stActualAve"] = stActualAve;
                        dr["stActualSD"] = stActualSD;
                        dr["stActualCV"] = stActualCV;
                        dr["stActualN"] = stActualN;
                    }
                }

                dtStatistics.Merge(dtNo);
                return dtStatistics;
            }
            return dtStatistics;
        }

        /// <summary>
        /// 独立画点
        /// </summary>
        /// <param name="dtQCData"></param>
        /// <param name="kind"></param>
        /// <param name="color"></param>
        private void getSK(List<EntityObrQcResult> dtQCData, DevExpress.XtraCharts.MarkerKind kind, Color color)
        {
            Series series = new Series("点状图", ViewType.Point);
            series.ArgumentScaleType = ScaleType.Qualitative;
            series.Label.Visible = false;
            PointSeriesView lsv = ((PointSeriesView)series.View);
            lsv.PointMarkerOptions.BorderVisible = false;
            lsv.PointMarkerOptions.Size = 8;
            lsv.PointMarkerOptions.Kind = kind;
            series.ShowInLegend = false;

            foreach (EntityObrQcResult drQcData in dtQCData)
            {
                if (drQcData.NSD != null)
                {
                    SeriesPoint point = getPoint(drQcData, dtQCData, color);
                    series.Points.Add(point);
                }
            }
            this.chartControl1.Series.Add(series);
        }

        /// <summary>
        /// 审核数据，判断是否失控
        /// </summary>
        /// <param name="drQcValue"></param>
        /// <param name="qcr_key"></param>
        private List<EntityObrQcResult> QcAudit(List<EntityObrQcResult> drQcValue, string qcr_key, EntityObrQcResultQC qcItemAudit)
        {
            List<String> listMatSnItmId = new List<string>();
            listMatSnItmId.Add(qcr_key);

            ProxyQcMateriaRule proxyMateriaRule = new ProxyQcMateriaRule();
            List<EntityDicQcRule> listQcRule = proxyMateriaRule.Service.GetQcRule(listMatSnItmId);

            listQcRule = listQcRule.FindAll(w => w.RulIsMoreLevel == 0 && w.RulName != "R-4s").OrderBy(o => o.SortNo).ToList();

            QCValueAudit qcValueAudit = new QCValueAudit();
            qcValueAudit.QCValue = drQcValue;
            qcValueAudit.QCRule = listQcRule;
            qcValueAudit.ImageType = QCImageType.LJ;
            qcValueAudit.WestgardType = listQcRule.Count == 8;
            qcValueAudit.DtBeginTime = ceType_Radar.Checked ? dtEnd.DateTime.Date : dtBegin.DateTime;
            qcValueAudit.DtEndTime = dtEnd.DateTime;
            qcValueAudit.DateFile = true;

            if (ceType_BDL.Checked)
                return qcValueAudit.SemiQuantitativeAudit();

            return qcValueAudit.QcAudit();
        }

        public List<EntityObrQcResult> QCAuditMoreRule(List<EntityObrQcResult> dtQcValue, List<String> listMatSnItmId)
        {
            ProxyQcMateriaRule proxyMateriaRule = new ProxyQcMateriaRule();
            List<EntityDicQcRule> listQcRule = proxyMateriaRule.Service.GetQcRule(listMatSnItmId);

            bool isCheckWestgard = false;

            listQcRule = listQcRule.FindAll(w => w.RulIsMoreLevel == 1 || w.RulName == "R-4s" || w.RulMAmount == 1);

            List<EntityObrQcResult> listQcValue = dtQcValue.OrderBy(w => w.QresItmId).OrderBy(w => w.QresDate).ToList();

            QCValueAudit qcValueAudit = new QCValueAudit();
            qcValueAudit.QCValue = listQcValue;
            qcValueAudit.QCRule = listQcRule;
            qcValueAudit.WestgardType = isCheckWestgard;
            qcValueAudit.DtBeginTime = ceType_Radar.Checked ? dtEnd.DateTime.Date : dtBegin.DateTime;
            qcValueAudit.DtEndTime = dtEnd.DateTime;
            qcValueAudit.DateFile = false;
            qcValueAudit.QCControl = chartControl1;

            return qcValueAudit.QcAudit();
        }


        public List<XYDiagramPane> diagramPane;


        #region 质控图显示封装

        /// <summary>
        /// 得到数据，并根据数据画线
        /// </summary>
        /// <param name="istrue">是否是多项目</param>
        /// <param name="sdb">ave、sd信息</param>
        /// <param name="series">线</param>
        /// <param name="drQcValue">结果数据</param>
        private List<EntityObrQcResult> getDate(double[] sdb, Series series, List<EntityObrQcResult> drQcValue)
        {
            List<EntityObrQcResult> listPointView = new List<EntityObrQcResult>();

            Random r = new Random();
            Color c = Color.FromArgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));

            dtMonica.Clear();

            List<EntityObrQcResult> dtQua = new List<EntityObrQcResult>();

            #region monica质控数据处理
            if (ceType_MC.Checked || ceType_UD.Checked || ceRes_Ave.Checked)
            {
                for (int k = 0; k < drQcValue.Count; k++)//筛选数据，把相同时间数据后面放入相同标识
                {
                    if (k == 0)
                        drQcValue[k].QcAve = "1";//第一个结果，标识设为1
                    else
                    {
                        DateTime dtA = Convert.ToDateTime(drQcValue[k].QresDate);//得到结果测定时间
                        DateTime dtB = Convert.ToDateTime(drQcValue[k - 1].QresDate);//得到上一个结果时间
                        if (dtA.ToString("yyyy-MM-dd") == dtB.ToString("yyyy-MM-dd"))
                            drQcValue[k].QcAve = drQcValue[k - 1].QcAve;//如果两个时间相等，两个数据标识相同
                        else
                            drQcValue[k].QcAve = (Convert.ToInt32(drQcValue[k - 1].QcAve) + 1).ToString();//否则标识+1，为新的标识
                    }

                    dtMonica.Add(drQcValue[k]);
                }
                for (int n = 1; n <= dtMonica.Count; n++)
                {
                    List<EntityObrQcResult> dtQave = new List<EntityObrQcResult>();
                    List<EntityObrQcResult> drAve = dtMonica.FindAll(w => w.QcAve == n.ToString()); //根据标识，查找相同的标识
                    int len = drAve.Count;
                    if (len > 0)//如果标识相同（时间相同），算出平均值，放入临时结果表，用于画图
                    {
                        double sum = 0;
                        if (n == 1)//如果为第一个结果，则直接画出改点
                        {
                            for (int m = 0; m < len; m++)
                            {
                                dtQua.Add(drAve[m]);
                                double qcm_value = 0;
                                try
                                {
                                    qcm_value = Convert.ToDouble(drAve[m].QresValue);
                                }
                                catch (Exception)
                                {
                                }
                                sum += qcm_value;
                            }
                            EntityObrQcResult drMave = drAve[len - 1];
                            drMave.QresValue = (sum / len).ToString();
                            try
                            {
                                drMave.NSD = (Convert.ToDouble(drMave.QresValue) - Convert.ToDouble(drMave.MatItmX)) / Convert.ToDouble(drMave.MatItmSd);
                            }
                            catch (Exception)
                            { }

                            dtQua.Add(drMave);
                        }
                        else//否则算出这两点的平均值，插入临时结果表，用于画图
                        {
                            for (int l = 0; l < len; l++)
                            {
                                dtQave.Add(drAve[l]);
                                double qMeas = 0;
                                try
                                {
                                    qMeas = Convert.ToDouble(drAve[l].QresValue);
                                }
                                catch (Exception)
                                {
                                    if (drAve[l].ItmConvertValue != null && drAve[l].ItmConvertValue.ToString() != "")//如果存在该半定量，读取
                                        qMeas = Convert.ToDouble(drAve[l].ItmConvertValue);
                                }
                                sum += qMeas;
                            }
                            drAve[len - 1].QresValue = (sum / len).ToString();

                            drAve[len - 1].NSD = (sum / len - Convert.ToDouble(drAve[len - 1].MatItmX)) / Convert.ToDouble(drAve[len - 1].MatItmSd);

                            dtQua.Add(drAve[len - 1]);//控制位置，控制线经过均值，并从均值出去

                            if (!ceRes_Ave.Checked || (ceRes_Ave.Checked && !ceData_Rep.Checked))//如果选择只画平均值，将不添加原始数据
                            {
                                foreach (EntityObrQcResult drve in dtQave)
                                {
                                    dtQua.Add(drve);
                                }
                                dtQua.Add(drAve[len - 1]);
                            }
                        }
                    }
                }
                drQcValue = EntityManager<EntityObrQcResult>.ListClone(dtQua);//.Copy();
            }
            #endregion

            int nextCount = 1;

            //根据结果画线
            foreach (EntityObrQcResult dr in drQcValue)
            {
                if (!string.IsNullOrEmpty(dr.QresValue))//判断不能为空结果
                {
                    bool add = true;

                    //失控点不连线
                    if (add && ceSerieType.Checked && dr.QresRunawayFlag == "2")
                        add = false; ;

                    //显示有效数据
                    if (add && ceShow_Fal.Checked && dr.QresDisplay != 0)
                        add = false;

                    //不显示失控数据
                    if (add &&
                        (ceRes_Con.Checked || (ceRes_Ave.Checked && ceData_con.Checked)) &&
                        !string.IsNullOrEmpty(dr.QresRunawayRule)
                        )
                    {
                        add = false;
                    }

                    //每天只连最后一点
                    if (add && cePointLast.Checked)
                    {
                        if (nextCount < drQcValue.Count &&
                         dr.QresDate.ToString("yyyyMMdd") == drQcValue[nextCount].QresDate.ToString("yyyyMMdd"))
                        {
                            listPointView.Add(dr);
                            add = false;
                        }
                    }

                    if (add)
                        series.Points.Add(getPoint(dr, drQcValue, c));
                }

                nextCount++;
            }

            return listPointView;
        }

        /// <summary>
        /// 根据结果画点
        /// </summary>
        /// <param name="dr">结果</param>
        /// <param name="sdb">ave等信息</param>
        /// <returns>点</returns>
        private SeriesPoint getPoint(EntityObrQcResult dr, List<EntityObrQcResult> dtOriginalItem, Color c)
        {
            DateTime date = new DateTime();
            DateTime qdate = Convert.ToDateTime(dr.QresDate);
            double chr = 0;
            try
            {
                if (ceType_BDL.Checked)
                    chr = Convert.ToDouble(dr.FinalValue);
                else
                {
                    chr = Convert.ToDouble(dr.NSD);
                    //值超过最大/最小限制，默认为最大/最小值
                    if (chr > 4)
                        chr = 4;
                    if (chr < -4)
                        chr = -4;
                }
            }
            catch (Exception)
            { }

            DateTime.TryParse(dr.QresDate.ToString(), out date);

            int index = dtOriginalItem.FindAll(w => w.QresDate >= date.Date && w.QresDate < date).Count;

            SeriesPoint point = new SeriesPoint();

            if (ceTransverse.Checked)
                point = new SeriesPoint(date.ToString("MM-dd") + "-" + (index + 1), chr);
            else
                point = new SeriesPoint(date.ToString("MM-dd"), chr);

            string c_x = dr.QresAuditFlag.ToString() == "1" ? dr.QresItmX.ToString() : dr.MatItmX.ToString();
            string sd = dr.QresAuditFlag.ToString() == "1" ? dr.QresItmSd.ToString() : dr.MatItmSd.ToString();

            point.Tag = "值:" + dr.QresValue.ToString() +
                        "\r\n项目:" + dr.ItmEcode.ToString() +
                        "\r\n时间:" + date.ToString("yyyy-MM-dd") +
                        "\r\nX:" + c_x +
                        "\r\nSD:" + sd +
                        "\r\n#" + dr.QresSn.ToString() + "#";

            point.Color = c;

            return point;
        }




        private SeriesPoint getRadarPoint(EntityObrQcResult dr)
        {
            DateTime date = new DateTime();
            DateTime qdate = Convert.ToDateTime(dr.QresDate);
            double chr = 0;
            try
            {
                if (ceType_BDL.Checked)
                    chr = dr.FinalValue;
                else
                {
                    chr = dr.NSD;
                    //值超过最大/最小限制，默认为最大/最小值
                    if (chr > 3)
                        chr = 3;
                    if (chr < -3)
                        chr = -3;
                }
            }
            catch (Exception)
            { }

            DateTime.TryParse(dr.QresDate.ToString(), out date);

            SeriesPoint point = new SeriesPoint();

            point = new SeriesPoint(dr.ItmEcode, Math.Abs(chr));
            string c_x = dr.QresAuditFlag.ToString() == "1" ? dr.QresItmX.ToString() : dr.MatItmX.ToString();
            string sd = dr.QresAuditFlag.ToString() == "1" ? dr.QresItmSd.Value.ToString() : dr.MatItmSd.ToString();

            point.Tag = "值:" + dr.QresValue +
                        "\r\n项目:" + dr.ItmEcode +
                        "\r\n时间:" + date.ToString("yyyy-MM-dd") +
                        "\r\n靶值:" + c_x +
                        "\r\nSD:" + sd +
                        "\r\n#" + dr.QresSn.ToString() + "#";
            return point;
        }

        /// <summary>
        /// 画出质控图的范围
        /// </summary>
        /// <param name="ave">平均值</param>
        /// <param name="sd">标准差</param>
        /// <param name="ccv">CCV</param>
        /// <param name="istrue">是否是多水平/项目</param>
        private void getRange(double ave, double sd, double ccv, double max, double min, List<EntityDicQcConvert> qcValueRange)
        {
            #region 设置质控图等参数、样式

            if (ceType_Radar.Checked)
            {
                DevExpress.XtraCharts.RadarDiagram rd = this.chartControl1.Diagram as DevExpress.XtraCharts.RadarDiagram;
                if (rd != null)
                {
                    rd.AxisY.Range.MinValue = 0;
                    rd.AxisY.Range.MaxValue = +3;
                    rd.AxisY.GridSpacing = 1;
                    rd.AxisY.Tickmarks.MinorVisible = false;
                    rd.AxisY.Visible = false;
                }
            }
            else
            {

                DevExpress.XtraCharts.XYDiagram gg = this.chartControl1.Diagram as DevExpress.XtraCharts.XYDiagram;
                gg.AxisX.Label.Angle = 30;
                gg.AxisY.ConstantLines.Clear();//清空Y轴数值
                //设置质控图显示范围
                gg.AxisY.Range.MaxValue = 10000;
                gg.AxisY.Range.MinValue = 0;

                if (ceType_BDL.Checked)
                {
                    gg.AxisY.Range.MinValue = min - 1;
                    gg.AxisY.Range.MaxValue = max + 1;
                }
                else
                {
                    gg.AxisY.Range.MinValue = -4;
                    gg.AxisY.Range.MaxValue = +4;
                }
                gg.AxisY.Visible = istrueSingle;

                //画警戒线
                if (ceType_MC.Checked)//判断是否是Monica质控
                {
                    if (ccv != 0)
                    {
                        DevExpress.XtraCharts.ConstantLine cl = new DevExpress.XtraCharts.ConstantLine();
                        cl.Title.Alignment = ConstantLineTitleAlignment.Far;
                        cl.Color = Color.Red;
                        cl.Name = "最大允许线值";
                        cl.AxisValue = ave + 1.5 * ccv * ave;
                        DevExpress.XtraCharts.ConstantLine c2 = new DevExpress.XtraCharts.ConstantLine();
                        c2.Title.Alignment = ConstantLineTitleAlignment.Far;
                        c2.Color = Color.Red;
                        c2.Name = "最小允许线值";
                        c2.AxisValue = ave - 1.5 * ccv * ave;
                        DevExpress.XtraCharts.ConstantLine c3 = new DevExpress.XtraCharts.ConstantLine();
                        c3.Title.Alignment = ConstantLineTitleAlignment.Far;
                        c3.Color = Color.Blue;
                        c3.Name = "警告值线";
                        c3.AxisValue = ave + 0.8 * ccv * ave;
                        DevExpress.XtraCharts.ConstantLine c4 = new DevExpress.XtraCharts.ConstantLine();
                        c4.Title.Alignment = ConstantLineTitleAlignment.Far;
                        c4.Color = Color.Blue;
                        c4.Name = "警告值线";
                        c4.AxisValue = ave - 0.8 * ccv * ave;
                        DevExpress.XtraCharts.ConstantLine c5 = new DevExpress.XtraCharts.ConstantLine();
                        c5.Title.Alignment = ConstantLineTitleAlignment.Far;
                        c5.Color = Color.Black;
                        c5.Name = "靶值";
                        c5.AxisValue = ave;
                        gg.AxisY.ConstantLines.Add(cl);
                        gg.AxisY.ConstantLines.Add(c2);
                        gg.AxisY.ConstantLines.Add(c3);
                        gg.AxisY.ConstantLines.Add(c4);
                        gg.AxisY.ConstantLines.Add(c5);
                    }
                }
                else if (ceType_LJ.Checked)
                {

                    DevExpress.XtraCharts.ConstantLine cl = new DevExpress.XtraCharts.ConstantLine();
                    cl.ShowInLegend = false;
                    cl.Title.Alignment = ConstantLineTitleAlignment.Far;
                    cl.Color = Color.Yellow;
                    cl.Name = ceLevel.Checked ? string.Empty : "+2sd";
                    cl.AxisValue = 2;
                    DevExpress.XtraCharts.ConstantLine c2 = new DevExpress.XtraCharts.ConstantLine();
                    c2.ShowInLegend = false;
                    c2.Title.Alignment = ConstantLineTitleAlignment.Far;
                    c2.Color = Color.Yellow;
                    c2.Name = ceLevel.Checked ? string.Empty : "-2sd";
                    c2.AxisValue = -2;
                    DevExpress.XtraCharts.ConstantLine c3 = new DevExpress.XtraCharts.ConstantLine();
                    c3.ShowInLegend = false;
                    c3.Title.Alignment = ConstantLineTitleAlignment.Far;
                    c3.Color = Color.Black;
                    c3.Name = ceLevel.Checked ? string.Empty : "靶值";
                    c3.AxisValue = 0;
                    DevExpress.XtraCharts.ConstantLine c4 = new DevExpress.XtraCharts.ConstantLine();
                    c4.ShowInLegend = false;
                    c4.Title.Alignment = ConstantLineTitleAlignment.Far;
                    c4.Color = Color.Red;
                    c4.Name = ceLevel.Checked ? string.Empty : "+3sd";
                    c4.AxisValue = 3;
                    DevExpress.XtraCharts.ConstantLine c5 = new DevExpress.XtraCharts.ConstantLine();
                    c5.ShowInLegend = false;
                    c5.Title.Alignment = ConstantLineTitleAlignment.Far;
                    c5.Color = Color.Red;
                    c5.Name = ceLevel.Checked ? string.Empty : "-3sd";
                    c5.AxisValue = -3;
                    DevExpress.XtraCharts.ConstantLine c6 = new DevExpress.XtraCharts.ConstantLine();
                    c6.ShowInLegend = false;
                    c6.Title.Alignment = ConstantLineTitleAlignment.Far;
                    c6.Color = Color.Green;
                    c6.Name = ceLevel.Checked ? string.Empty : "-sd";
                    c6.AxisValue = -1;
                    DevExpress.XtraCharts.ConstantLine c7 = new DevExpress.XtraCharts.ConstantLine();
                    c7.ShowInLegend = false;
                    c7.Title.Alignment = ConstantLineTitleAlignment.Far;
                    c7.Color = Color.Green;
                    c7.Name = ceLevel.Checked ? string.Empty : "+sd";
                    c7.AxisValue = 1;
                    gg.AxisY.ConstantLines.AddRange(
                        new ConstantLine[] { cl, c2, c3, c4, c5, c6, c7 });

                    gg.AxisY.CustomLabels.Clear();

                    gg.AxisY.CustomLabels.AddRange(
                            new DevExpress.XtraCharts.CustomAxisLabel[] {
                    new CustomAxisLabel((ave + sd * 3).ToString("0.000"),3),
                    new CustomAxisLabel((ave + sd * 2).ToString("0.000"),2),
                    new CustomAxisLabel((ave + sd * 1).ToString("0.000"),1),
                    new CustomAxisLabel((ave ).ToString(),0),
                    new CustomAxisLabel((ave - sd * 1).ToString("0.000"),-1),
                    new CustomAxisLabel((ave - sd * 2).ToString("0.000"),-2),
                    new CustomAxisLabel((ave - sd * 3).ToString("0.000"),-3)}
                            );
                }
                else
                {

                    DevExpress.XtraCharts.ConstantLine maxLine = new DevExpress.XtraCharts.ConstantLine();
                    maxLine.Title.Alignment = ConstantLineTitleAlignment.Far;
                    maxLine.Color = Color.Red;
                    maxLine.Name = "最大允许值";
                    maxLine.AxisValue = max;
                    DevExpress.XtraCharts.ConstantLine minLine = new DevExpress.XtraCharts.ConstantLine();
                    minLine.Title.Alignment = ConstantLineTitleAlignment.Far;
                    minLine.Color = Color.Red;
                    minLine.Name = "最小允许值";
                    minLine.AxisValue = min;

                    gg.AxisY.ConstantLines.AddRange(new ConstantLine[] { maxLine, minLine });

                    gg.AxisY.CustomLabels.Clear();


                    foreach (EntityDicQcConvert dr in qcValueRange)
                    {
                        gg.AxisY.CustomLabels.Add(new CustomAxisLabel(dr.ItmValue, Convert.ToDouble(dr.ItmConvertValue.ToString())));
                    }
                }
            }
            #endregion
        }

        #endregion

        bool isIncludeMes = ConfigHelper.GetSysConfigValueWithoutLogin("QC_ResultAnalyseAndDeal") == "是" ? true : false;

        #region 窗口弹出

        /// <summary>
        /// 报表打印预览方法
        /// </summary>
        /// <param name="isPrint"></param>
        private void reportPrintorPreview(bool isPrint)
        {
            if (bsGcData.DataSource == null)
            {
                lis.client.control.MessageDialog.Show("无打印数据", "提示");
                return;
            }
            DataTable dt = ((DataTable)gcInfo.DataSource).Copy(); //gcInfo.DataSource:项目-平均值-标准差-CV-N-平均值-标准差-CV
            DataSet dsQC = new DataSet();
            int runAwayNum = 0; //失控数
            if (dt.Rows.Count == 0)
            {
                lis.client.control.MessageDialog.Show("无打印数据", "提示");
                return;
            }

            //系统配置：质控结果分析与处理信息备注
            if (isIncludeMes)
            {
                FrmBscripeSelectV3 fb = new FrmBscripeSelectV3("21");
                fb.ShowDialog();
            }

            if ((isPrint && lis.client.control.MessageDialog.Show("是否打印？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes) || !isPrint)
            {
                string file = Application.StartupPath + "\\Temp";
                addFile(file);
                string fileName = Application.StartupPath + "\\Temp\\qc_" + Guid.NewGuid().ToString() + ".jpg";
                this.chartControl1.Dock = DockStyle.None;
                this.chartControl1.Width = 1000;
                this.chartControl1.Height = 300;
                this.chartControl1.ExportToImage(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                this.chartControl1.Dock = DockStyle.Fill;
                dt.Columns.Add("图");
                dt.Columns.Add("时间范围");
                dt.Columns.Add("仪器");
                dt.Columns.Add("仪器名称");
                dt.Columns.Add("项目");
                dt.Columns.Add("质控批号");
                dt.Columns.Add("质控水平");
                dt.Columns.Add("测定方法");
                dt.Columns.Add("temp1");
                dt.Columns.Add("有效日期", typeof(DateTime));
                dt.Columns.Add("signImage", typeof(byte[]));
                dt.Columns.Add("目标CV");
                dt.Columns.Add("允许范围CV%");
                dt.Columns.Add("CCV");
                dt.Columns.Add("单位");
                dt.Columns.Add("试剂批号");
                dt.Columns.Add("试剂厂家");
                dt.Columns.Add("统计方法");
                dt.Columns.Add("质控规则");
                dt.Columns.Add("质控厂家");
                dt.Columns.Add("有效期");
                //新增字段2018/04/03
                dt.Columns.Add("质控结果分析与处理");
                dt.Columns.Add("失控率");
                dt.Columns.Add("偏倚率");

                string path = "C:\\Program Files\\hope\\lis\\" + "QcAnalyseResult.txt";
                string strLine = string.Empty;
                string strMes = string.Empty;

                if (File.Exists(path) && isIncludeMes)
                {
                    StreamReader sR = File.OpenText(path);
                    while ((strLine = sR.ReadLine()) != null)
                    {
                        strMes = strLine;
                    }
                    sR.Close();
                }

                //dt表多水平行全部赋值
                for (int i2 = 0; i2 < dt.Rows.Count; i2++)
                {
                    dt.Rows[i2]["temp1"] = "数据";


                    dt.Rows[i2]["图"] = fileName;
                    dt.Rows[i2]["时间范围"] = dtBegin.Text + "~" + dtEnd.Text;
                    dt.Rows[dt.Rows.Count - 1]["时间范围"] = dtBegin.Text + "~" + dtEnd.Text;
                    dt.Rows[i2]["仪器"] = lue_Apparatus.displayMember;
                    if (!string.IsNullOrEmpty(lue_Apparatus.displayMember))
                    {
                        dt.Rows[i2]["仪器名称"] = lue_Apparatus.selectRow.ItrName;
                    }
                    else
                    {
                        dt.Rows[i2]["仪器名称"] = "";
                    }
                    string[] stItem = dt.Rows[i2]["stItem"].ToString().Split('-');
                    if (stItem.Length > 1)
                    {
                        dt.Rows[i2]["质控批号"] = dt.Rows[i2]["stItem"].ToString().Split('-')[1];
                        dt.Rows[i2]["质控水平"] = dt.Rows[i2]["stItem"].ToString().Split('-')[0];
                    }
                    dt.Rows[i2]["目标CV"] = dt.Rows[i2]["stAllowCV"].ToString();

                    string mid = dt.Rows[i2]["type_id"].ToString().Split('&')[0];
                    string itm_id = dt.Rows[i2]["stItemId"].ToString();
                    DataTable qc_sample = new DataTable();
                    DataSet dsSearch = new DataSet();
                    DataTable dtSearch = new DataTable();
                    dtSearch.TableName = "res";
                    dtSearch.Columns.Add("sql");
                    dsSearch.Tables.Add(dtSearch);
                    DataRow drNew = dtSearch.NewRow();

                    try
                    {
                        if (dt.Columns.Contains("qcr_m_pro") && dt.Rows[i2]["qcr_m_pro"] != null &&
                            dt.Rows[i2]["qcr_m_pro"] != DBNull.Value)
                        {
                            dt.Rows[i2]["测定方法"] = dt.Rows[i2]["qcr_m_pro"].ToString();
                        }
                        if (!string.IsNullOrEmpty(strMes))
                        {
                            dt.Rows[i2]["质控结果分析与处理"] = strMes;
                        }
                    }
                    catch (Exception)
                    {

                    }


                    string strItem = string.Empty;
                    if (ckShowType_S.Checked)
                        strItem = "项目：";
                    else
                        strItem = "水平：";

                    bool isTrue = false;

                    for (int j = 0; j < tlQcItem.AllNodesCount; j++)
                    {
                        TreeListNode tn = tlQcItem.FindNodeByID(j);

                        if (tn.Checked && tn["TvType"].ToString() == "0")
                        {
                            if (isTrue)
                                strItem += ",";
                            strItem += tn["TvName"].ToString();
                            isTrue = true;
                        }
                    }

                    dt.Rows[i2]["项目"] = strItem;

                }


                //增加结果与审核上去
                if (ConfigHelper.IsNotOutlink()) //如果不是outlink佛山市一
                {
                    List<EntityObrQcResult> dtLot = (List<EntityObrQcResult>)bsGcData.DataSource;
                    DataTable dtQCData = EntityManager<EntityObrQcResult>.ConvertToDataTable(dtLot);

                    if (dtQCData != null)
                    {
                        //生成qc_value表的列
                        foreach (DataColumn col in dtQCData.Columns)
                        {
                            dt.Columns.Add(col.ColumnName);
                        }

                        //用于显示报表数据的临时表,
                        DataTable dtReportTemp = dt.Clone();
                        //填充行数据到dt,用于报表的显示
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtQCData.Rows.Count; i++)
                            {
                                DataRow[] dtDataArr = dt.Select(" type_id='" + dtQCData.Rows[i]["qcm_id"].ToString() + "&" + dtQCData.Rows[i]["qcm_itm_ecd"].ToString() + "'");


                                DataRow newRow = dtReportTemp.NewRow();
                                foreach (DataColumn colInDt in dt.Columns)
                                {
                                    if (dtQCData.Columns.Contains(colInDt.ColumnName)) //
                                    {
                                        newRow[colInDt.ColumnName] = dtQCData.Rows[i][colInDt.ColumnName];
                                        if (colInDt.ColumnName == "qcr_yield_manu")
                                            newRow["质控厂家"] = dtQCData.Rows[i][colInDt.ColumnName];
                                        if (colInDt.ColumnName == "qcr_edate")
                                            newRow["有效期"] = dtQCData.Rows[i][colInDt.ColumnName];
                                    }
                                    else //不是质控结果表的字段取第一行的数据
                                    {
                                        newRow[colInDt.ColumnName] = dtDataArr[0][colInDt.ColumnName];
                                    }
                                }
                                //统计失控数
                                EntityObrQcResult dr = (EntityObrQcResult)this.gvLot.GetRow(i);
                                if(dr.QresRunawayFlag == "2")
                                {
                                    runAwayNum++;
                                }
                                dtReportTemp.Rows.Add(newRow.ItemArray);
                            }
                        }

                        dt = dtReportTemp.Copy();
                    }

                    //加上结果日期
                    dt.Columns.Add("结果日期");
                    if (dt.Columns.Contains("qcm_date"))
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if (!string.IsNullOrEmpty(row["qcm_date"].ToString()))
                                row["结果日期"] = Convert.ToDateTime(row["qcm_date"].ToString()).Month.ToString() + "/" + Convert.ToDateTime(row["qcm_date"].ToString()).Day.ToString();
                        }
                    }

                    //添加上qcm_meas的数值列
                    if (!dt.Columns.Contains("qcm_meas_dec"))
                    {
                        dt.Columns.Add("qcm_meas_dec", typeof(decimal));
                    }

                    List<EntitySysUser> listUserRole = CacheClient.GetCache<EntitySysUser>();
                    string runRate = ((runAwayNum * 100.0) / dt.Rows.Count).ToString("0.00") + "%";
                    foreach (DataRow row in dt.Rows)
                    {
                        string qcmeas = row["qcm_meas"].ToString();

                        decimal decOut;
                        if (decimal.TryParse(qcmeas, out decOut))
                        {
                            row["qcm_meas_dec"] = decOut;
                        }

                        if (listUserRole != null && listUserRole.Count > 0 &&
                            dt.Columns.Contains("qcr_i_name")
                            && row["qcr_i_name"] != null
                            && row["qcr_i_name"] != DBNull.Value && !string.IsNullOrEmpty(row["qcr_i_name"].ToString()))
                        {
                            EntitySysUser temp = listUserRole.Find(i => i.UserLoginid == row["qcr_i_name"].ToString() || i.UserName == row["qcr_i_name"].ToString());

                            if (temp != null && temp.UserSigninamge != null)
                            {
                                try
                                {
                                    row["signImage"] = temp.UserSigninamge;
                                }
                                catch
                                {

                                }

                            }
                        }
                        row["失控率"] = runRate;
                        row["偏倚率"] = ((Convert.ToDecimal(row["stActualAve"].ToString()) - Convert.ToDecimal(row["stave"].ToString())) / Convert.ToDecimal(row["stave"].ToString())).ToString("0.000") + "%" ;
                    }

                    dt.TableName = "可设计字段";

                    dsQC.Tables.Add(dt);

                    if (gcInfo.DataSource != null)
                    {
                        DataTable dtQCInfo = (DataTable)gcInfo.DataSource;
                        dtQCInfo.TableName = "统计信息";
                        dsQC.Tables.Add(dtQCInfo.Copy());
                    }

                }
                try
                {
                    EntityDCLPrintData printData = new EntityDCLPrintData();
                    printData.ReportName = "质控_报表";
                    printData.ReportData = dsQC;

                    if (isPrint)
                        DCLReportPrint.PrintByData(printData);
                    else
                        DCLReportPrint.PrintPreviewByData(printData);
                }
                catch (ReportNotFoundException ex1)
                {
                    lis.client.control.MessageDialog.Show(ex1.MSG);
                }
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


        /// <summary>
        /// 测定数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void testData_Click(object sender, EventArgs e)
        {
            sysToolBar1.Focus();
            FrmMensurate fm = new FrmMensurate(dtBegin.EditValue, dtEnd.EditValue, null);
            fm.ItrId = lue_Apparatus.valueMember;
            fm.TypeId = lueType.valueMember == null ? string.Empty : lueType.valueMember;
            fm.StartTime = dtBegin.DateTime;
            fm.EndTime = dtEnd.DateTime;
            fm.isFirstShow = true;
            for (int j = 0; j < tlQcItem.AllNodesCount; j++)
            {
                TreeListNode tn = tlQcItem.FindNodeByID(j);

                if (tn.Checked && tn["TvType"].ToString() == "1")//判断选中并且选中的为项目
                {
                    string[] itm = tn["TvId"].ToString().Split('&');
                    fm.MatSn = itm[0]; //勾选单个时默认显示勾选的数据
                    fm.ItmId = itm[1];
                }
            }

            fm.ShowDialog();
        }


        //数据审核
        private void dataAudit_Click(object sender, EventArgs e)
        {
            sysToolBar1.Focus();

            //允许半定量质控多项目审核
            if (UserInfo.GetSysConfigValue("QCAllowMuchItemAudit_BDL") == "是")
            {
                isAllowMuchItemAuditBDL = true;
            }

            if (UserInfo.GetSysConfigValue("QCAuditMode") == "启用")
                showItemsView(true);

            if (bsGcData.DataSource != null)
            {
                List<EntityObrQcResult> dtLot = (List<EntityObrQcResult>)bsGcData.DataSource;
                if (dtLot.Count == 0)
                {
                    MessageBox.Show("无数据审核");
                    return;
                }
                List<EntityObrQcResult> drnotAuditLot = dtLot.FindAll(w => w.QresAuditFlag == 0);//.Select("qcm_flg='0'");
                if (drnotAuditLot.Count > 0)
                {
                    FrmAudiData frmAudi = new FrmAudiData(dtLot, AuditType.Audit);
                    if (frmAudi.ShowDialog() == DialogResult.OK)
                    {
                        MessageDialog.ShowAutoCloseDialog("审核完成！");
                        showItemsView();
                    }

                }
                else
                    MessageBox.Show("该数据已经审核！");
            }

            isAllowMuchItemAuditBDL = false;
        }

        #endregion

        string userTypes = "";

        /// <summary>
        /// 数据移到质控图点上显示详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chartControl1_ObjectHotTracked(object sender, HotTrackEventArgs e)
        {
            SeriesPoint point = e.AdditionalObject as SeriesPoint;

            if (point != null)
            {
                string pt = point.Tag.ToString();
                if (toolTipController1 == null)
                    toolTipController1 = new DevExpress.Utils.ToolTipController();

                toolTipController1.ShowHint(pt);
            }
            else
            {
                toolTipController1.HideHint();
            }
        }

        /// <summary>
        /// 设置失控数据显示的颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            EntityObrQcResult dr = (EntityObrQcResult)this.gvLot.GetRow(e.RowHandle);
            if (dr != null && dr.QresRunawayFlag != null)
            {
                if (dr.QresAuditFlag != null && dr.QresAuditFlag.ToString() == "1")
                    e.Appearance.ForeColor = Color.Blue;//已审
                if (dr.QresRunawayFlag.ToString() == "1")
                    e.Appearance.ForeColor = Color.YellowGreen;//警告
                else if (dr.QresRunawayFlag.ToString() == "2")
                    e.Appearance.ForeColor = Color.Red;//失控

                if (isTwoAuditPower && dr.QresAuditFlag.ToString() == "1")
                {
                    int time = UserInfo.GetSysConfigValue("QCTwoAuditTime") == "" ? 24 : Convert.ToInt32(UserInfo.GetSysConfigValue("QCTwoAuditTime"));
                    if (dr.QresSecondauditUserId != null && dr.QresSecondauditUserId.ToString().Trim() != string.Empty && Convert.ToInt32(dr.QresSecondauditInterval) >= time)
                    {
                        //e.Appearance.BackColor = Color.Yellow;//超过二审时间
                    }
                }
            }
        }

        /// <summary>
        /// 仪器改变时，清空以前所选项目水平等
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void lue_Apparatus_ValueChanged(object sender, dcl.client.control.ValueChangeEventArgs args)
        {
            loadQcItem();
        }

        private void loadQcItem()
        {
            if (lue_Apparatus.valueMember != null && lue_Apparatus.valueMember != "" && dtBegin.EditValue != null && dtEnd.EditValue != null)
            {
                ProxyObrQcResult proxyQc = new ProxyObrQcResult();

                QcTreeViewType showType = QcTreeViewType.多水平;
                if (tabSub.SelectedTabPage == xtItem && ckShowType_I.Checked)
                    showType = QcTreeViewType.多项目;

                List<EntityQcTreeView> listQcView = proxyQc.Service.GetQcTreeView(lue_Apparatus.valueMember,
                                                                                 dtBegin.DateTime,
                                                                                 dtEnd.DateTime.AddDays(1),
                                                                                 showType,
                                                                                 ceType_Radar.Checked);

                this.tlQcItem.DataSource = listQcView;

                if (tabSub.SelectedTabPage == xtProcess)
                {
                    tlQcAuditItem.DataSource = EntityManager<EntityQcTreeView>.ListClone(listQcView);
                    tlQcAuditItem.ExpandAll();

                    if (tlQcAuditItem.Nodes.Count > 0)
                        tlQcAuditItem.FocusedNode = tlQcAuditItem.Nodes[0];
                }

                if (ceType_Radar.Checked)
                {
                    for (int j = 0; j < tlQcItem.AllNodesCount; j++)
                    {
                        TreeListNode tn = tlQcItem.FindNodeByID(j);

                        if (tn["TvType"].ToString() == "0")//判断选中并且选中的为项目
                        {
                            tn.Expanded = true;
                        }
                    }
                }
                else
                {
                    tlQcItem.ExpandAll();
                    if (tlQcItem.Nodes.Count > 0)
                        tlQcItem.FocusedNode = tlQcItem.Nodes[0];
                }
            }
        }

        /// <summary>
        /// 批号选择过滤
        /// </summary>
        /// <param name="strFilter"></param>
        private void selectDict_QC_Lot1_onBeforeFilter(ref string strFilter)
        {
            if (this.lue_Apparatus.valueMember == "" || this.lue_Apparatus.valueMember == null)
            {
                strFilter += "and 1<>1";
            }
            else
            {
                strFilter += "and qcr_id='" + this.lue_Apparatus.valueMember + "'";
            }
        }


        /// <summary>
        /// 清空列头和数据
        /// </summary>
        private void emptyQcDate()
        {
            bsGcData.DataSource = null;
        }

        /// <summary>
        /// 选择父节点，所有子节点都选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlQcItem_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            TreeListNode tn = e.Node;
            SetCheckedChildNodes(tn, tn.Checked);

            //if (tn["TvType"].ToString() == "1")
            showItemsView();
        }

        /// <summary>
        /// 焦点选中则显示该项及所有子节点的图表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlQcItem_AfterFocusedNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            for (int j = 0; j < tlQcItem.AllNodesCount; j++)
            {
                TreeListNode tn = tlQcItem.FindNodeByID(j);
                tn.Checked = false;
            }

            e.Node.CheckState = CheckState.Checked;
            SetCheckedChildNodes(e.Node, true);

            showItemsView();
        }

        /// <summary>
        /// 递归选择子节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="check"></param>
        private void SetCheckedChildNodes(TreeListNode node, bool check)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                node.Nodes[i].Checked = check;
                SetCheckedChildNodes(node.Nodes[i], check);
            }
        }

        /// <summary>
        /// 质控点数据的启用
        /// </summary>
        /// <param name="isTrue"></param>
        private void resEnabled(bool isTrue)
        {
            gbQcPointData.Enabled = isTrue;
        }

        /// <summary>
        /// 选中平均值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ceRes_CheckedChanged(object sender, EventArgs e)
        {
            resEnabled(ceRes_Ave.Checked);
        }

        /// <summary>
        /// 选中Monica时,不启用质控图中平均选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ceType_MC_CheckedChanged(object sender, EventArgs e)
        {
            if (ceRes_Ave.Checked)
                ceRes_All.Checked = true;
            ceRes_Ave.Properties.ReadOnly = ceType_MC.Checked;
        }

        /// <summary>
        /// 修改显示标识
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckEffective_CheckedChanged(object sender, EventArgs e)
        {
            EntityObrQcResult dr = (EntityObrQcResult)gvLot.GetFocusedRow();
            if (dr != null)
            {
                sysToolBar1.Focus();

                ProxyObrQcResult proxy = new ProxyObrQcResult();
                proxy.Service.UpdateQresDisplay(dr.QresSn.ToString(), dr.QresDisplay);
            }
        }

        /// <summary>
        /// 水平显示方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckShowType_CheckedChanged(object sender, EventArgs e)
        {
            checkEditAll.Checked = false;

            if (((DevExpress.XtraEditors.CheckEdit)sender).Checked)
                lue_Apparatus_ValueChanged(null, null);
            if (ckShowType_S.Checked)
            {
                sysToolBar1.BtnPageDown.Enabled = true;
                sysToolBar1.BtnPageUp.Enabled = true;
            }
            else
            {
                sysToolBar1.BtnPageDown.Enabled = false;
                sysToolBar1.BtnPageUp.Enabled = false;
            }
        }

        /// <summary>
        /// 下一条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnPageDownClicked(object sender, EventArgs e)
        {
            pageUpDown(false);
        }

        /// <summary>
        /// 上一条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnPageUpClicked(object sender, EventArgs e)
        {
            pageUpDown(true);
        }

        /// <summary>
        /// 上一条下一条方法
        /// </summary>
        /// <param name="up">是否为上</param>
        private void pageUpDown(bool up)
        {
            if (ckShowType_S.Checked && tlQcItem.AllNodesCount > 0 && tabSub.SelectedTabPage == xtItem)
            {
                TreeListNode tnFocus = tlQcItem.FocusedNode;
                foreach (TreeListNode tNote in tlQcItem.Nodes)
                {
                    if (tNote.Checked)
                    {
                        tnFocus = tNote;
                        break;
                    }
                }


                TreeListNode tnNode;

                if (ceType_Radar.Checked)
                {
                    if (!up)
                    {
                        if (tnFocus.ParentNode == null)
                            tnNode = tnFocus.Nodes[0].NextNode;
                        else if (tnFocus.Nodes.Count == 0)
                            tnNode = tnFocus.ParentNode.NextNode;
                        else
                            tnNode = tnFocus.NextNode;
                    }
                    else
                    {
                        if (tnFocus.ParentNode == null)
                            tnNode = tnFocus.Nodes[0].PrevNode;
                        else if (tnFocus.Nodes.Count == 0)
                            tnNode = tnFocus.ParentNode.PrevNode;
                        else
                            tnNode = tnFocus.PrevNode;
                    }
                }
                else
                {
                    if (!up)
                    {
                        if (tnFocus.ParentNode == null)
                            tnNode = tnFocus.NextNode;
                        else
                            tnNode = tnFocus.ParentNode.NextNode;
                    }
                    else
                    {
                        if (tnFocus.ParentNode == null)
                            tnNode = tnFocus.PrevNode;
                        else
                            tnNode = tnFocus.ParentNode.PrevNode;
                    }
                }
                if (tnNode != null)//tnFocus.ParentNode != null && 
                {
                    for (int j = 0; j < tlQcItem.AllNodesCount; j++)
                    {
                        TreeListNode tnCheck = tlQcItem.FindNodeByID(j);
                        tnCheck.Checked = false;
                    }
                    tnNode.Checked = true;
                    tlQcItem.FocusedNode = tnNode;
                    SetCheckedChildNodes(tnNode, tnNode.Checked);
                    showItemsView();
                }
            }
            else if (tabSub.SelectedTabPage == xtProcess && tlQcAuditItem.AllNodesCount > 0)  //审核 (上一条和下一条)
            {
                TreeListNode tnFocus = tlQcAuditItem.FocusedNode;
                foreach (TreeListNode tNote in tlQcAuditItem.Nodes)
                {
                    if (tNote.Checked)
                    {
                        tnFocus = tNote;
                        break;
                    }
                }


                TreeListNode tnNode;

                if (ceType_Radar.Checked)  //雷达图（是否选中）
                {
                    if (!up)
                    {
                        if (tnFocus.ParentNode == null)
                            tnNode = tnFocus.Nodes[0].NextNode;
                        else if (tnFocus.Nodes.Count == 0)
                            tnNode = tnFocus.ParentNode.NextNode;
                        else
                            tnNode = tnFocus.NextNode;
                    }
                    else
                    {
                        if (tnFocus.ParentNode == null)
                            tnNode = tnFocus.Nodes[0].PrevNode;
                        else if (tnFocus.Nodes.Count == 0)
                            tnNode = tnFocus.ParentNode.PrevNode;
                        else
                            tnNode = tnFocus.PrevNode;
                    }
                }
                else
                {
                    if (!up)
                    {
                        if (tnFocus.ParentNode == null)
                            tnNode = tnFocus.NextNode;
                        else
                            tnNode = tnFocus.ParentNode.NextNode;
                    }
                    else
                    {
                        if (tnFocus.ParentNode == null)
                            tnNode = tnFocus.PrevNode;
                        else
                            tnNode = tnFocus.ParentNode.PrevNode;
                    }
                }
                if (tnNode != null)//tnFocus.ParentNode != null && 
                {
                    for (int j = 0; j < tlQcAuditItem.AllNodesCount; j++)
                    {
                        TreeListNode tnCheck = tlQcAuditItem.FindNodeByID(j);
                        tnCheck.Checked = false;
                    }
                    tnNode.Checked = true;
                    tlQcAuditItem.FocusedNode = tnNode;
                    SetCheckedChildNodes(tnNode, tnNode.Checked);
                    showItemsView();
                }
            }
        }

        private void lueType_ValueChanged(object sender, dcl.client.control.ValueChangeEventArgs args)
        {
            lue_Apparatus.displayMember = "";
            lue_Apparatus.valueMember = "";

            if (!string.IsNullOrEmpty(lueType.valueMember))
            {
                List<EntityDicInstrument> listIns = lue_Apparatus.getDataSource();
                listIns = listIns.FindAll(w => w.ItrLabId == lueType.valueMember);

                if (!UserInfo.isAdmin)
                {
                    listIns = listIns.FindAll(w => UserInfo.entityUserInfo.UserItrsQc.FindIndex(f => f.ItrId == w.ItrId) > -1);
                }

                lue_Apparatus.SetFilter(listIns);
            }
            else
                lue_Apparatus.SetFilter(new List<EntityDicInstrument>());
        }

        private void sysToolBar1_OnPrintPreviewClicked(object sender, EventArgs e)
        {
            reportPrintorPreview(false);
        }

        private void sysToolBar1_OnBtnPrintClicked(object sender, EventArgs e)
        {
            reportPrintorPreview(true);
        }

        private void dtBegin_EditValueChanged(object sender, EventArgs e)
        {
            loadQcItem();
        }

        private void dtEnd_EditValueChanged(object sender, EventArgs e)
        {
            dtEnd.EditValueChanged -= new EventHandler(dtEnd_EditValueChanged);
            dtEnd.EditValue = dtEnd.DateTime.Date.AddDays(1).AddMilliseconds(-1);
            dtEnd.EditValueChanged += new EventHandler(dtEnd_EditValueChanged);
            loadQcItem();
        }

        /// <summary>
        /// 反审
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnQualityDataClicked(object sender, EventArgs e)
        {
            if (bsGcData.DataSource == null || ((List<EntityObrQcResult>)bsGcData.DataSource).Count == 0)
            {
                lis.client.control.MessageDialog.Show("无质控数据");
                return;
            }
            List<EntityObrQcResult> dtQcData = (List<EntityObrQcResult>)bsGcData.DataSource;
            FrmAudiData frmAudi = new FrmAudiData(dtQcData, AuditType.CancelAudit);
            if (frmAudi.ShowDialog() == DialogResult.OK)
                showItemsView();
        }

        /// <summary>
        /// 二次审核数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnQualityRuleClicked(object sender, EventArgs e)
        {
            sysToolBar1.Focus();
            if (bsGcData.DataSource != null)
            {
                List<EntityObrQcResult> dtLot = (List<EntityObrQcResult>)bsGcData.DataSource;
                if (dtLot.Count == 0)
                {
                    MessageBox.Show("无数据审核");
                    return;
                }
                List<EntityObrQcResult> drnotAuditLot = dtLot.FindAll(w => w.QresAuditFlag == 1 && string.IsNullOrEmpty(w.QresSecondauditUserId));//.Select("qcm_flg='1' and qcm_two_audit_name is null");
                if (drnotAuditLot.Count > 0)
                {
                    FrmAudiData frmAudi = new FrmAudiData(dtLot, AuditType.TwoAudit);
                    if (frmAudi.ShowDialog() == DialogResult.OK)
                    {
                        MessageDialog.ShowAutoCloseDialog("审核完成！");
                        showItemsView();
                    }
                }
                else
                    MessageBox.Show("没有可以二审的数据！");
            }
        }

        /// <summary>
        /// 导出统计数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnExportClicked(object sender, EventArgs e)
        {
            if (gcInfo.DataSource != null)
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
                        gcInfo.ExportToXls(ofd.FileName.Trim());
                        lis.client.control.MessageDialog.Show("导出成功！", "提示");
                    }
                    catch (Exception)
                    {
                    }
                }

            }
        }
        /// <summary>
        /// 刷新按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void sysToolBar1_OnBtnRefreshClicked(object sender, EventArgs e)
        {
            loadQcItem();  //重新加载数据，并清空勾选
            simpleButton1_Click(sender, e); //清空质控图和统计数据清空
        }

        FrmQcDataModify frmData;

        private void chartControl1_ObjectSelected(object sender, HotTrackEventArgs e)
        {
            SeriesPoint point = e.AdditionalObject as SeriesPoint;
            if (point != null)
            {
                string[] strPoint = point.Tag.ToString().Split('#');
                if (strPoint.Length > 2)
                {
                    string strPointId = strPoint[1];

                    List<EntityObrQcResult> listSour = (List<EntityObrQcResult>)bsGcData.DataSource;

                    EntityObrQcResult drPoint = listSour.Find(w => w.QresSn.ToString() == strPointId);//.Select("qcm_bs='" + strPointId + "'");

                    if (frmData == null || frmData.IsDisposed)
                    {
                        frmData = new FrmQcDataModify();
                        frmData.RefreshData += new FrmQcDataModify.RefreshQcData(showItemsView);
                        frmData.Show();
                    }

                    frmData.QcData = drPoint.QresValue.ToString();
                    frmData.QcId = strPointId;
                    frmData.QcIsAudit = drPoint.QresAuditFlag.ToString() == "0" ? true : false;
                    frmData.QcOutOfControl = drPoint.QresRunawayFlag.ToString() == "2" ? true : false;
                    frmData.QcDetailId = drPoint.QresMatDetId.ToString();
                    frmData.QcItemEcd = drPoint.ItmEcode.ToString();
                    frmData.setQcData();
                    frmData.Visible = true;
                    frmData.Left = Control.MousePosition.X;
                    frmData.Top = Control.MousePosition.Y - frmData.Height + 100;
                }
            }
        }

        private void sysToolBar1_BtnSaveDefaultClick(object sender, EventArgs e)
        {
            if (bsGcData.DataSource != null)
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
                        gcLot.ExportToXls(ofd.FileName.Trim());
                        lis.client.control.MessageDialog.Show("导出成功！", "提示");
                    }
                    catch (Exception)
                    {
                    }
                }

            }
        }

        /// <summary>
        /// 质控评价
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_BtnReturnClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lue_Apparatus.valueMember))
            {
                FrmQcDesc frm = new FrmQcDesc(lue_Apparatus.valueMember);
                frm.ShowDialog();
            }
            else {
                lis.client.control.MessageDialog.Show("请选择仪器！", "提示");
            }

        }
        /// <summary>
        /// 每天室内质控数据EXCEL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnResultViewClicked(object sender, EventArgs e)
        {
            
        }


        private void ceType_Radar_CheckedChanged(object sender, EventArgs e)
        {
            setPanelVisible(!ceType_Radar.Checked);

            List<String> tlSelection = new List<String>();
            for (int j = 0; j < tlQcItem.AllNodesCount; j++)
            {
                TreeListNode tn = tlQcItem.FindNodeByID(j);
                if (tn.Checked)
                    tlSelection.Add(tn["TvName"].ToString());
            }
            loadQcItem();
            if (!ceType_Radar.Checked && tlSelection.Count > 0)
            {
                for (int j = 0; j < tlQcItem.AllNodesCount; j++)
                {
                    TreeListNode tn = tlQcItem.FindNodeByID(j);
                    foreach (var item in tlSelection)
                    {
                        if (item == tn["TvName"].ToString())
                        {
                            tn.Checked = true;
                            SetCheckedChildNodes(tn, tn.Checked);
                        }
                    }
                }
                showItemsView();
            }
        }

        private void setPanelVisible(bool RadarChecked)
        {
            gbShowType.Visible = RadarChecked;
            gbQcType.Visible = RadarChecked;

            Point po = new Point(0, 0);
            if (RadarChecked && panelControl1.Visible)
                po = checkEditAll.Location;
            else if (RadarChecked && !panelControl1.Visible)
                po = new Point(checkEditAll.Location.X, checkEditAll.Location.Y + panelControl1.Height);
            else if (!RadarChecked && panelControl1.Visible)
                po = new Point(checkEditAll.Location.X, checkEditAll.Location.Y - panelControl1.Height);
            else
                po = checkEditAll.Location;
            panelControl1.Visible = RadarChecked;
            this.checkEditAll.Location = po;
        }

        private void gvLot_DoubleClick(object sender, EventArgs e)
        {
            EntityObrQcResult drQcValue = (EntityObrQcResult)gvLot.GetFocusedRow();
            if (ceType_Radar.Checked && drQcValue != null)
            {
                string strTypeId = drQcValue.QresMatDetId.ToString() + "&" + drQcValue.QresItmId.ToString();
                ceType_Radar.CheckedChanged -= new EventHandler(ceType_Radar_CheckedChanged);
                ceType_LJ.Checked = true;
                setPanelVisible(true);
                loadQcItem();
                ceType_Radar.CheckedChanged += new EventHandler(ceType_Radar_CheckedChanged);


                for (int j = 0; j < tlQcItem.AllNodesCount; j++)
                {
                    TreeListNode tn = tlQcItem.FindNodeByID(j);

                    if (tn["TvId"].ToString() == strTypeId)
                    {
                        tn.Checked = true;
                        SetCheckedChildNodes(tn, tn.Checked);
                    }
                }
                showItemsView();
            }
        }

        private void gvLot_ShowingEditor(object sender, CancelEventArgs e)
        {
            GridView grid = sender as GridView;

            EntityObrQcResult dr = (EntityObrQcResult)gvLot.GetRow(grid.FocusedRowHandle);

            if (!string.IsNullOrEmpty(dr.QresRemark))
            {
                toolTipController1.ShowHint(dr.QresRemark.Trim());
                e.Cancel = true;
            }

        }


        private void gvLot_MouseMove(object sender, MouseEventArgs e)
        {
            GridHitInfo info;
            Point pt = gvLot.GridControl.PointToClient(Control.MousePosition);
            info = gvLot.CalcHitInfo(pt);
            if (info.RowHandle >= 0)
            {
                EntityObrQcResult drRow = (EntityObrQcResult)gvLot.GetRow(info.RowHandle);
                if (drRow.QresRemark.Trim() != string.Empty)
                    toolTipController2.ShowHint(drRow.QresRemark);
            }
            else
                toolTipController2.HideHint();
        }

        private void tlQcAuditItem_AfterCheckNode(object sender, NodeEventArgs e)
        {
            TreeListNode tn = e.Node;

            if (tn.ParentNode != null)
            {
                tn.ParentNode.Checked = tn.Checked;
                tn = tn.ParentNode;
            }

            SetCheckedChildNodes(tn, tn.Checked);
        }

        private void btnDataAudit_Click(object sender, EventArgs e)
        {
            showItemsView(true);
            dataAudit_Click(sender, e);
        }

        private void tabSub_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            sysToolBar1.BtnQualityAudit.Enabled = tabSub.SelectedTabPage == xtProcess;
            sysToolBar1.BtnQualityRule.Enabled = tabSub.SelectedTabPage == xtProcess;

            loadQcItem();
        }

        private void checkEditAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int j = 0; j < tlQcItem.AllNodesCount; j++)
            {
                TreeListNode tn = tlQcItem.FindNodeByID(j);
                tn.Checked = checkEditAll.Checked;
            }
        }

        private void chartControl1_DoubleClick(object sender, EventArgs e)
        {
            FrmQcView qcView = new FrmQcView();

            ChartControl maxQc = (ChartControl)this.chartControl1.Clone();
            qcView.SetControl(maxQc);
            qcView.ShowDialog();
        }

        private void ckData_CheckedChanged(object sender, EventArgs e)
        {
            if (ckDataAll.Checked)
                bsGcData.DataSource = listQcResultFilter;
            else
                bsGcData.DataSource = listQcResultFilter.FindAll(w => w.QresAuditFlag == 1);// "qcm_flg='1'";
        }

        private void updataSimpleButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("是否将统计数据更新到该质控项目参数中?", "提示", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK && gcInfo.DataSource != null)
            {
                DataTable countData = ((DataTable)gcInfo.DataSource).Copy();
                List<EntityDicQcMateriaDetail> listUpdate = new List<EntityDicQcMateriaDetail>();
                if (countData != null && countData.Rows.Count > 0)
                {
                    for (int i = 0; i < countData.Rows.Count; i++)
                    {
                        EntityDicQcMateriaDetail qcMateria = new EntityDicQcMateriaDetail();
                        qcMateria.MatId = countData.Rows[i]["type_id"].ToString().Substring(0, 5);
                        qcMateria.MatItmId = countData.Rows[i]["stItemId"].ToString();
                        qcMateria.MatItrId = this.lue_Apparatus.valueMember;
                        qcMateria.MatItmX = countData.Rows[i]["stActualAve"] != null ? Convert.ToDecimal(countData.Rows[i]["stActualAve"].ToString()) : 0;
                        qcMateria.MatItmSd = countData.Rows[i]["stActualSD"] != null ? Convert.ToDecimal(countData.Rows[i]["stActualSD"].ToString()) : 0;
                        qcMateria.MatItmCv = countData.Rows[i]["stActualCV"] != null ? Convert.ToDecimal(countData.Rows[i]["stActualCV"].ToString()) : 0;
                        listUpdate.Add(qcMateria);
                    }
                    if (new ProxyQcMateriaDetail().Service.UpdateQcMateriaDetailCondition(listUpdate))
                    {
                        MessageBox.Show("数据更新成功！");
                    }
                    else {
                        MessageBox.Show("数据更新失败！");
                    }
                }
            }
            else
            {
                return;
            }
        }
    }
}
