using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Drawing;
using dcl.common;
using System.Diagnostics;
using dcl.svr.result;
using dcl.common;

namespace dcl.pub.wcf.web
{
    public partial class FrmHistoryAnaly : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //======= 1. 取出传入参数 =======
                string sPatID = Request["Pat_Id"];
                sPatID = "10001201305134";
                DataSet ds = new dcl.svr.result.PatReadBLL().GetPatCommonResultHistoryWithRef(sPatID, 30, null);



                //======= 2.1 病人基本信息 =======            
                DataTable dtPatInfo = ds.Tables[0];
                if (dtPatInfo == null || dtPatInfo.Rows.Count <= 0)
                    return;

                //======= 2.2 历史结果 =======
                DataTable dtPatHistory = ds.Tables[1];

                //======= 2.3 项目参考值 =======
                DataTable dtItemsRef = ds.Tables[2];
                ViewState["dtItemsRef"] = dtItemsRef;

                //======= 3. 规整历史结果数据 =======
                DataTable dtChr = convertDT(dtPatHistory);
                if (dtChr != null && dtChr.Rows.Count > 0)
                {
                    try
                    {
                        CaptionHistoryValue(dtChr);
                    }
                    catch (Exception ex)
                    {
                        Lib.LogManager.Logger.LogException(ex);
                    }
                }

                gridHistory.DataSource = dtChr;
                gridHistory.DataBind();
                ViewState["Table"] = dtChr;
                gridHistory.SelectedIndex = 0;
                gridHistory_SelectedIndexChanged(null, null);

                //if (dtChr.Rows.Count > 0)
                //{
                //    //设置列宽
                //    foreach (GridViewColumn col in gridHistory.Columns)
                //    {
                //        col.Width = new Unit("100px");
                //    }
                //}


            }
        }


        /// <summary>
        /// 隐藏列_如果直接设置Visible会导致无法取得值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridHistory_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                //e.Row.Cells[0].Visible = false;
                e.Row.Cells[2].Visible = false;
            }

        }


        /// <summary>
        /// 行选择效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //此语句需要模板列中有选择列才能正常执行
                e.Row.Attributes.Add("onclick", "javascript:__doPostBack('" + gridHistory.ID + "','Select$" + e.Row.RowIndex + "')");
            }
        }

        /// <summary>
        /// 画曲线图
        /// </summary>
        /// <param name="drHistory"></param>
        public void DrawChart(DataRow drHistory)
        {
            string item_ecd = drHistory[0].ToString();

            //统计的时间范围
            string dtTo = drHistory.Table.Columns[2].ColumnName;
            string dtFrom = drHistory.Table.Columns[drHistory.Table.Columns.Count - 1].ColumnName;

            //创建曲线图
            DevExpress.XtraCharts.Series series = new DevExpress.XtraCharts.Series(item_ecd, DevExpress.XtraCharts.ViewType.Line);
            this.WebChartControl1.Series.Clear();
            this.WebChartControl1.Series.Add(series);

            DevExpress.XtraCharts.XYDiagram xy = this.WebChartControl1.Diagram as DevExpress.XtraCharts.XYDiagram;
            xy.AxisX.ConstantLines.Clear();
            xy.AxisX.Label.Angle = 30;
            xy.AxisX.Label.Antialiasing = true;

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
            DataRow drItemRef = null;

            DataTable dtItemsRef = (DataTable)ViewState["dtItemsRef"];

            if (dtItemsRef != null)
            {
                xy.AxisY.ConstantLines.Clear();
                string itm_id = drHistory["res_itm_id"].ToString();

                DataRow[] drs = dtItemsRef.Select(string.Format("itm_id='{0}'", itm_id));
                if (drs.Length > 0)
                {
                    drItemRef = drs[0];
                }

                if (drItemRef != null)
                {
                    #region 画参考线
                    //参考值下限
                    if (drItemRef["itm_ref_l"] != null && drItemRef["itm_ref_l"] != DBNull.Value)
                    {
                        double d_itm_ref_l;
                        if (double.TryParse(drItemRef["itm_ref_l"].ToString(), out d_itm_ref_l))
                        {
                            valueList.Add(Convert.ToDecimal(d_itm_ref_l));
                            DevExpress.XtraCharts.ConstantLine line = new DevExpress.XtraCharts.ConstantLine("参考值下限 " + d_itm_ref_l.ToString(), d_itm_ref_l);
                            line.Color = Color.Blue;
                            line.Title.TextColor = Color.Blue;

                            xy.AxisY.ConstantLines.Add(line);
                        }
                    }

                    //参考值上限
                    if (drItemRef["itm_ref_h"] != null && drItemRef["itm_ref_h"] != DBNull.Value)
                    {
                        double d_itm_ref_h;
                        if (double.TryParse(drItemRef["itm_ref_h"].ToString(), out d_itm_ref_h))
                        {
                            valueList.Add(Convert.ToDecimal(d_itm_ref_h));
                            DevExpress.XtraCharts.ConstantLine line = new DevExpress.XtraCharts.ConstantLine("参考值上限 " + d_itm_ref_h.ToString(), d_itm_ref_h);
                            line.Color = Color.Red;
                            line.Title.TextColor = Color.Red;

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
        /// 计算百分比
        /// </summary>
        /// <param name="dtChr"></param>
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
        /// 点注释
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
        /// 纵表转横表
        /// </summary>
        /// <param name="dtPatHistory"></param>
        /// <returns></returns>
        private DataTable convertDT(DataTable dtPatHistory)
        {
            DataTable dtChr = new DataTable();

            DataTable dtItem = dtPatHistory.DefaultView.ToTable(true, new string[] { "res_itm_ecd", "res_itm_id" });
            DataTable dtDate = dtPatHistory.DefaultView.ToTable(true, new string[] { "res_date" });
            dtChr.Columns.Add("项目");
            dtChr.Columns.Add("res_itm_id");

            string dateFormat = "yyyy/MM/dd HH:mm:ss";

            foreach (DataRow drDate in dtDate.Rows)
            {
                dtChr.Columns.Add(((DateTime)drDate[0]).ToString(dateFormat));
            }

            foreach (DataRow drEcd in dtItem.Rows)
            {
                DataRow drChr = dtChr.NewRow();
                string ecd = drEcd["res_itm_ecd"].ToString();
                string itm_id = drEcd["res_itm_id"].ToString();
                drChr["项目"] = ecd;
                drChr["res_itm_id"] = itm_id;
                foreach (DataRow dr in dtPatHistory.Rows)
                {
                    if (dr["res_itm_ecd"].ToString() == ecd)
                    {
                        drChr[((DateTime)dr["res_date"]).ToString(dateFormat)] = dr["res_chr"].ToString();
                    }
                }
                //drChr["Visible"] = true;
                dtChr.Rows.Add(drChr);
            }

            return dtChr;
        }

        protected void gridHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gridHistory.SelectedRow != null)
            {
                string strid = gridHistory.SelectedRow.Cells[2].Text;

                DataTable dtSour = (DataTable)ViewState["Table"];
                DataRow[] drChrs = dtSour.Select("res_itm_id='" + strid + "'");
                if (drChrs.Length > 0)
                {
                    DrawChart(drChrs[0]);
                }
            }

        }
    }
}
