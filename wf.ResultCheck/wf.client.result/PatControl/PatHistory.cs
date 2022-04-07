using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using DevExpress.XtraCharts;
using dcl.client.result.CommonPatientInput;
using dcl.common;
using DevExpress.XtraGrid.Columns;
using dcl.client.wcf;

using System.Diagnostics;
using dcl.root.logon;
using dcl.entity;
using dcl.client.frame;
using System.Linq;

namespace dcl.client.result.PatControl
{
    public partial class PatHistory : UserControl
    {
        public PatHistory()
        {
            InitializeComponent();

            this.chartControl1.ObjectHotTracked += new DevExpress.XtraCharts.HotTrackEventHandler(this.chartControl1_ObjectHotTracked);
            PatID = string.Empty;
        }

        /// <summary>
        /// 当前的原项目结果
        /// </summary>
        public List<EntityObrResult> dtResultShowDataSort { get; set; }

        private const int ResultCount = 30;

        string PatID;
        string Itr_ptype;
        string SamType_id;
        int Age;
        string Sex;

        public void LoadPatHistory(string pat_id, string itr_id, string pat_sam_id, string pat_no_id, string pat_in_no, string pat_name, int age, string sex)
        {
            LoadPatHistory(pat_id);
        }

        List<EntityItmRefInfo> dtItemsRef = new List<EntityItmRefInfo>();

        /// <summary>
        /// 加载历史结果
        /// </summary>
        /// <param name="pat_id"></param>
        public void LoadPatHistory(string pat_id)
        {
            Reset();

            PatID = pat_id;

            int resultCount = Convert.ToInt32(txtResultCount.EditValue);

            EntityPidReportMain patient = new ProxyPidReportMain().Service.GetPatientByPatId(PatID, false);
            List<EntityObrResult> listHistoryResult = new ProxyPatResult().Service.GetPatCommonResultHistoryWithRef(PatID, resultCount, null, true)
                                                                                .OrderByDescending(i => i.ObrDate).ThenBy(i => i.ItmSeq).ToList();
            List<string> itemIds = new List<string>();
            foreach (EntityObrResult obrResult in listHistoryResult)
            {
                if (!itemIds.Contains(obrResult.ItmId))
                {
                    itemIds.Add(obrResult.ItmId);
                }
            }
            string patDepCode = string.Empty;
            if (!string.IsNullOrEmpty(patient.PidDeptId))
            {
                patDepCode = patient.PidDeptId;
            }
            List<EntityItmRefInfo> listItemRefInfo = new ProxyPatResult().Service.GetItemRefInfo(itemIds,
                                                                patient.PidSamId, GetConfigOnNullAge(Convert.ToInt32(patient.PidAge)),
                                GetConfigOnNullSex(patient.PidSex), patient.SampRemark, patient.RepItrId, patDepCode, "");

            BindData(patient, listHistoryResult, listItemRefInfo, resultCount);
        }


        /// <summary>
        /// 加载历史结果-报告单查询
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="pat_date"></param>
        public void LoadPatHistoryFromSelect(string pat_id, DateTime pat_date)
        {
            Reset();

            PatID = pat_id;

            int resultCount = Convert.ToInt32(txtResultCount.EditValue);

            EntityPidReportMain patient = new ProxyPidReportMain().Service.GetPatientByPatId(PatID, false);
            List<EntityObrResult> listHistoryResult = new ProxyPatResult().Service.GetPatCommonResultHistoryWithRef(PatID, resultCount, pat_date, true)
                                                 .OrderByDescending(i => i.ObrDate).ThenBy(i => i.ItmSeq).ToList();
            List<string> itemIds = new List<string>();
            foreach (EntityObrResult obrResult in listHistoryResult)
            {
                if (!itemIds.Contains(obrResult.ItmId))
                {
                    itemIds.Add(obrResult.ItmId);
                }
            }
            string patDepCode = string.Empty;
            if (!string.IsNullOrEmpty(patient.PidDeptId))
            {
                patDepCode = patient.PidDeptId;
            }
            List<EntityItmRefInfo> listItemRefInfo = new ProxyPatResult().Service.GetItemRefInfo(itemIds,
                                                                patient.PidSamId, GetConfigOnNullAge(Convert.ToInt32(patient.PidAge)),
                                GetConfigOnNullSex(patient.PidSex), patient.SampRemark, patient.RepItrId, patDepCode, "");

            BindData(patient, listHistoryResult, listItemRefInfo, resultCount);
        }


        private int GetConfigOnNullAge(int age)
        {
            if (age < 0)
            {
                //获取参考值：年龄/性别为空时的计算规则
                string configCalAge = UserInfo.GetSysConfigValue("GetRefOnNullAge");

                int calage = -1;

                if (!string.IsNullOrEmpty(configCalAge)
                    && configCalAge != "不计算参考值")
                {
                    calage = AgeConverter.YearToMinute(Convert.ToInt32(configCalAge));
                    if (age >= 0)
                    {
                        calage = age;
                    }
                }
                return calage;
            }
            else
            {
                return age;
            }
        }

        private string GetConfigOnNullSex(string sex)
        {
            if (string.IsNullOrEmpty(sex)

                || (sex != "1"
                && sex != "2"
                && sex != "0"))
            {
                //获取参考值：年龄/性别为空时的计算规则
                string configCalSex = UserInfo.GetSysConfigValue("GetRefOnNullSex");

                if (configCalSex.Contains("男"))
                {
                    return "1";
                }
                else if (configCalSex.Contains("女"))
                {
                    return "2";
                }

                return "0";
            }
            else
            {
                return sex;
            }
        }
        private void BindData(EntityPidReportMain patient, List<EntityObrResult> listHistoryResult, List<EntityItmRefInfo> listItemRefInfo, int resultCount)
        {
            EntityPidReportMain patInfo = patient;
            if (patInfo != null)
            {
                dtItemsRef = listItemRefInfo;

                //设置姓名
                this.lbName.Text = string.Format("{0}", patInfo.PidName);

                //设置年龄
                string pat_age = string.Empty;
                if (!Compare.IsNullOrDBNull(patInfo.PidAgeExp))
                {
                    pat_age = patInfo.PidAgeExp.ToString();
                    pat_age = AgeConverter.TrimZeroValue(pat_age);
                    pat_age = AgeConverter.ValueToText(pat_age);
                }
                this.lbAge.Text = pat_age;


                DataTable dtChr = convertDT(listHistoryResult, resultCount);

                #region 重新排序历史结果的项目顺序,令其跟原始结果项目排序一样

                //如果当前原始结果不为空,则用当前的项目顺序,重新排序历史结果
                if (dtResultShowDataSort != null && dtResultShowDataSort.Count > 0
                            && dtChr != null && dtChr.Rows.Count > 0)
                {
                    if (dtChr.Columns.Contains("res_itm_id"))
                    {
                        if (!dtChr.Columns.Contains("temp_seq"))
                        {
                            dtChr.Columns.Add("temp_seq", Type.GetType("System.Int32"));

                            for (int i = 0; i < dtChr.Rows.Count; i++)
                            {
                                dtChr.Rows[i]["temp_seq"] = 9999;
                            }
                        }

                        for (int i = 0; i < dtResultShowDataSort.Count; i++)
                        {
                            DataRow[] arrery_dr = dtChr.Select(string.Format("res_itm_id='{0}'", dtResultShowDataSort[i].ItmId));
                            if (arrery_dr != null && arrery_dr.Length > 0)
                            {
                                foreach (DataRow p_dr in arrery_dr)
                                {
                                    p_dr["temp_seq"] = i;
                                }
                            }
                        }

                        dtChr.DefaultView.Sort = "temp_seq asc, 代码 asc";
                        dtChr = dtChr.DefaultView.ToTable();

                        if (dtChr.Columns.Contains("temp_seq"))
                        {
                            dtChr.Columns.Remove("temp_seq");
                            dtChr.AcceptChanges();
                        }
                    }
                }
                #endregion

                if (dtChr != null && dtChr.Rows.Count > 0)
                    CaptionHistoryValue(dtChr);

                this.gridControl1.DataSource = dtChr;
                this.gridView1.Columns["res_itm_id"].Visible = false;

                if (dtChr.Rows.Count > 0)
                {
                    //设置列宽
                    foreach (GridColumn col in gridView1.Columns)
                    {
                        col.Width = 100;
                    }

                    this.gridView1.MoveFirst();
                    DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs args =
                        new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(0, 0);
                    gridView1_FocusedRowChanged(this.gridView1, args);
                }
            }
        }


        private void CaptionHistoryValue(DataTable dtChr)
        {
            foreach (DataRow item in dtChr.Rows)
            {
                for (int i = dtChr.Columns.Count - 1; i > -1; i--)
                {
                    if (item[i] != null && item[i] != DBNull.Value && item[i].ToString().Trim() != string.Empty)
                    {
                        double dbValue = 0;
                        if (double.TryParse(item[i].ToString().Split('(')[0], out dbValue))
                        {
                            for (int j = i - 1; j >= 2; j--)
                            {
                                if (item[j] != null && item[j] != DBNull.Value && item[j].ToString().Trim() != string.Empty)
                                {
                                    double lastValue = 0;
                                    if (double.TryParse(item[j].ToString(), out lastValue))
                                    {
                                        string percentageValue = ((lastValue - dbValue) / dbValue * 100).ToString("0.00");
                                        item[j] = item[j].ToString() + " (" + percentageValue + "%)";
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 纵表转横表
        /// </summary>
        /// <param name="dtPatHistory"></param>
        /// <param name="resultCount"></param>
        /// <returns></returns>
        private DataTable convertDT(List<EntityObrResult> dtPatHistory, int resultCount)
        {
            Dictionary<string, int> itemDic = new Dictionary<string, int>();
            List<EntityObrResult> listResult = new List<EntityObrResult>();
            EntityObrResult row;
            for (int i = 0; i < dtPatHistory.Count; i++)
            {
                row = dtPatHistory[i];
                if (!itemDic.ContainsKey(row.ItmId))
                {
                    itemDic.Add(row.ItmId, 0);
                }
                if (itemDic[row.ItmId] >= resultCount)
                {
                    continue;
                }
                listResult.Add(row);

                itemDic[row.ItmId] += 1;
            }
            DataTable dtChr = new DataTable();
            List<DateTime> listDateTime = listResult.Select(i => i.ObrDate).Distinct().ToList();
            List<string> listItmEname = listResult.Select(i => i.ItmEname).Distinct().ToList();
            dtChr.Columns.Add("名称");
            dtChr.Columns.Add("代码");
            dtChr.Columns.Add("res_itm_id");

            string dateFormat = "yy/MM/dd HH:mm:ss";

            foreach (DateTime drDate in listDateTime)
            {
                dtChr.Columns.Add(drDate.ToString(dateFormat));
            }


            foreach (string drEcd in listItmEname)
            {
                DataRow drChr = dtChr.NewRow();
                string ecd = drEcd;
                drChr["名称"] = listResult.Find(t=>t.ItmEname == ecd).ItmName;
                drChr["代码"] = ecd;
                foreach (EntityObrResult dr in listResult)
                {
                    if (dr.ItmEname == ecd)
                    {
                        drChr[((DateTime)dr.ObrDate).ToString(dateFormat)] = dr.ObrValue;
                        drChr["res_itm_id"] = dr.ItmId;
                    }
                }
                dtChr.Rows.Add(drChr);
            }

            return dtChr;
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            int rowIndex = e.FocusedRowHandle;

            DataRow dr = this.gridView1.GetDataRow(rowIndex);

            if (dr != null)
            {
                DrawChart(dr);
            }
        }


        public void DrawChart(DataRow drHistory)
        {
            ResetChart();

            string item_ecd = drHistory[0].ToString();

            //统计的时间范围
            string dtTo = drHistory.Table.Columns[2].ColumnName;
            string dtFrom = drHistory.Table.Columns[drHistory.Table.Columns.Count - 1].ColumnName;

            this.lbDateInterval.Text = string.Format("{0} - {1}", dtFrom, dtTo);
            this.lbItmEcd.Text = item_ecd;


            //创建曲线图
            DevExpress.XtraCharts.Series series = new DevExpress.XtraCharts.Series(item_ecd, DevExpress.XtraCharts.ViewType.Line);

            this.chartControl1.Series.Clear();
            this.chartControl1.Series.Add(series);
            DevExpress.XtraCharts.XYDiagram xy = this.chartControl1.Diagram as DevExpress.XtraCharts.XYDiagram;

            xy.AxisY.ConstantLines.Clear();
            xy.AxisX.Label.Angle = 30;
            xy.AxisX.Label.Antialiasing = true;
            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            //所有统计点的集合
            List<decimal> valueList = new List<decimal>();

            //画点（右边为最近日期）
            for (int i = drHistory.Table.Columns.Count - 1; i >= 0; i--)
            {
                DataColumn col = drHistory.Table.Columns[i];

                if (col.ColumnName != "res_itm_ecd" && col.ColumnName != "res_itm_id")
                {
                    object objValue = drHistory[col.ColumnName];

                    if (objValue != null && objValue != DBNull.Value)
                    {
                        decimal decValue;

                        if (decimal.TryParse(objValue.ToString().Split('(')[0], out decValue))
                        {
                            valueList.Add(decValue);

                            DevExpress.XtraCharts.SeriesPoint sPoint = new DevExpress.XtraCharts.SeriesPoint(col.ColumnName, decValue);
                            //把当点结果值,日期存到点的tag值中,在鼠标移动到点上面时拿出来显示
                            PointTips pt = new PointTips(col.ColumnName, decValue.ToString());
                            sPoint.Tag = pt;
                            series.Points.Add(sPoint);
                        }
                    }
                }
            }


            //获取参考值
            EntityItmRefInfo drItemRef = null;

            if (this.dtItemsRef.Count > 0)
            {
                string itm_id = drHistory["res_itm_id"].ToString();

                int itmIndex = this.dtItemsRef.FindIndex(i => i.ItmId == itm_id);
                if (itmIndex > -1)
                {
                    drItemRef = this.dtItemsRef[itmIndex];
                }

                if (drItemRef != null)
                {
                    #region 画参考线
                    //参考值下限
                    if (!string.IsNullOrEmpty(drItemRef.ItmLowerLimitValue) && chkShowRef.Checked)
                    {
                        double d_itm_ref_l;
                        if (double.TryParse(drItemRef.ItmLowerLimitValue, out d_itm_ref_l))
                        {
                            valueList.Add(Convert.ToDecimal(d_itm_ref_l));
                            DevExpress.XtraCharts.ConstantLine line = new DevExpress.XtraCharts.ConstantLine("参考值下限 " + d_itm_ref_l.ToString(), d_itm_ref_l);
                            line.Color = Color.Blue;
                            line.Title.TextColor = Color.Blue;

                            xy.AxisY.ConstantLines.Add(line);
                            //xy.AxisY.Range.MinValue = d_itm_ref_l * 0.1;
                        }
                    }

                    //参考值上限
                    if (!string.IsNullOrEmpty(drItemRef.ItmUpperLimitValue) && chkShowRef.Checked)
                    {
                        double d_itm_ref_h;
                        if (double.TryParse(drItemRef.ItmUpperLimitValue, out d_itm_ref_h))
                        {
                            valueList.Add(Convert.ToDecimal(d_itm_ref_h));
                            DevExpress.XtraCharts.ConstantLine line = new DevExpress.XtraCharts.ConstantLine("参考值上限 " + d_itm_ref_h.ToString(), d_itm_ref_h);
                            line.Color = Color.Red;
                            line.Title.TextColor = Color.Red;

                            //xy.AxisY.Range.MaxValue = d_itm_ref_h * 10;

                            xy.AxisY.ConstantLines.Add(line);
                        }
                    }

                    //极小阈值
                    if (!string.IsNullOrEmpty(drItemRef.ItmMinValue) && chkShowThreshold.Checked)
                    {
                        double d_itm_min;
                        if (double.TryParse(drItemRef.ItmMinValue, out d_itm_min))
                        {
                            valueList.Add(Convert.ToDecimal(d_itm_min));
                            DevExpress.XtraCharts.ConstantLine line = new DevExpress.XtraCharts.ConstantLine("极小阈值 " + d_itm_min.ToString(), d_itm_min);
                            line.Color = Color.Blue;
                            line.Title.TextColor = Color.Blue;
                            xy.AxisY.ConstantLines.Add(line);
                        }
                    }

                    //极大阈值
                    if (!string.IsNullOrEmpty(drItemRef.ItmMaxValue) && chkShowThreshold.Checked)
                    {
                        double d_itm_max;
                        if (double.TryParse(drItemRef.ItmMaxValue, out d_itm_max))
                        {
                            valueList.Add(Convert.ToDecimal(d_itm_max));
                            DevExpress.XtraCharts.ConstantLine line = new DevExpress.XtraCharts.ConstantLine("极大阈值 " + d_itm_max.ToString(), d_itm_max);
                            line.Color = Color.Red;
                            line.Title.TextColor = Color.Red;

                            xy.AxisY.ConstantLines.Add(line);
                        }
                    }

                    //危急值下限
                    if (!string.IsNullOrEmpty(drItemRef.ItmDangerLowerLimit) && chkShowCritical.Checked)
                    {
                        double d_itm_pan_l;
                        if (double.TryParse(drItemRef.ItmDangerLowerLimit, out d_itm_pan_l))
                        {
                            valueList.Add(Convert.ToDecimal(d_itm_pan_l));
                            DevExpress.XtraCharts.ConstantLine line = new DevExpress.XtraCharts.ConstantLine("危急值下限 " + d_itm_pan_l.ToString(), d_itm_pan_l);
                            line.Color = Color.Blue;
                            line.Title.TextColor = Color.Blue;
                            line.Title.Alignment = ConstantLineTitleAlignment.Far;
                            xy.AxisY.ConstantLines.Add(line);
                        }
                    }

                    //危急值上限
                    if (!string.IsNullOrEmpty(drItemRef.ItmDangerUpperLimit) && chkShowCritical.Checked)
                    {
                        double d_itm_pan_h;
                        if (double.TryParse(drItemRef.ItmDangerUpperLimit, out d_itm_pan_h))
                        {
                            valueList.Add(Convert.ToDecimal(d_itm_pan_h));
                            DevExpress.XtraCharts.ConstantLine line = new DevExpress.XtraCharts.ConstantLine("危急值上限 " + d_itm_pan_h.ToString(), d_itm_pan_h);
                            line.Color = Color.Red;
                            line.Title.TextColor = Color.Red;
                            line.Title.Alignment = ConstantLineTitleAlignment.Far;
                            xy.AxisY.ConstantLines.Add(line);
                        }
                    }
                    #endregion
                }
            }

            //从所有统计点中找出最大最小值作为图标的最大最小值
            if (valueList.Count > 0)
            {
                xy.AxisY.Range.MaxValue = decimal.MaxValue;
                xy.AxisY.Range.MinValue = decimal.MinValue;

                xy.AxisY.Range.MaxValue = IEnumerableUtil.Max(valueList) / 0.94m;
                xy.AxisY.Range.MinValue = IEnumerableUtil.Min(valueList) * 0.94m;
            }
        }

        /// <summary>
        /// 重置图表
        /// </summary>
        private void ResetChart()
        {
            this.chartControl1.Series.Clear();
            this.chartControl1.Titles.Clear();
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            ResetChart();
            this.gridControl1.DataSource = null;
            this.gridView1.Columns.Clear();


            this.lbName.Text = string.Empty;
            this.lbAge.Text = string.Empty;
            this.lbItmEcd.Text = string.Empty;
            this.lbDateInterval.Text = string.Empty;
        }

        /// <summary>
        /// HotTrack事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chartControl1_ObjectHotTracked(object sender, DevExpress.XtraCharts.HotTrackEventArgs e)
        {
            SeriesPoint point = e.AdditionalObject as SeriesPoint;

            if (point != null)//如果移动到点上面
            {
                PointTips pt = point.Tag as PointTips;
                if (pt != null)
                {
                    toolTipController1.ShowHint(pt.ToString());
                }
            }
            else
            {
                toolTipController1.HideHint();
            }
        }

        /// <summary>
        /// 点 注释
        /// </summary>
        internal class PointTips
        {
            public PointTips(string date, string value)
            {
                ResDate = date;
                Value = value;
            }

            public string ResDate { get; set; }
            public string Value { get; set; }

            public override string ToString()
            {
                return ResDate;
            }
        }

        /// <summary>
        /// 点击刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPatHistory(this.PatID);
        }

        private void btnExportImg_Click(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.DefaultExt = "jpg";
            ofd.Filter = "图片文件文件(*.jpg)|*.jpg";
            ofd.Title = "导出到图片";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileName.Trim() == string.Empty)
                {
                    lis.client.control.MessageDialog.Show("文件名不能为空！");
                    return;
                }

                try
                {
                    chartControl1.ExportToImage(ofd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    lis.client.control.MessageDialog.Show("导出成功！");
                }
                catch (Exception)
                {
                }
            }
        }

        private void btnExportResult_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.DefaultExt = "xls";
                ofd.Filter = "Excel文件(*.xls)|*.xls";
                ofd.Title = "导出到Excel";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.Trim() == string.Empty)
                    {
                        lis.client.control.MessageDialog.Show("文件名不能为空！");
                        return;
                    }

                    try
                    {
                        gridControl1.ExportToXls(ofd.FileName);
                        lis.client.control.MessageDialog.Show("导出成功！");
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            else
            {
                lis.client.control.MessageDialog.Show("无导出数据！");
            }
        }


    }
}
