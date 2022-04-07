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
using System.Text;

namespace dcl.pub.wcf.web
{
    public partial class FrmHistoryAnalyNew : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sScript = "window.document.getElementById('" + ScrollPosY.ClientID + "').value = " +
                    "window.document.getElementById('" + PlResult.ClientID + "').scrollTop;";
            ScriptManager.RegisterOnSubmitStatement(this, this.GetType(), "SavePanelScroll", sScript);
            if (!IsPostBack)
            {
                //======= 1. 取出传入参数 =======
                string sPatID = Request["Pat_Id"];

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
            if (gridHistory.Rows.Count > 0 && gridHistory.SelectedRow != null)
            {
                string strid = gridHistory.SelectedRow.Cells[2].Text;

                DataTable dtSour = (DataTable)ViewState["Table"];
                DataRow[] drChrs = dtSour.Select("res_itm_id='" + strid + "'");
                //Response.Write("<script>alert('准备进入画图');</script>");
                //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(UpdatePanel), "popMessage", "alert('准备进入画图');", true);
                if (drChrs.Length > 0)
                {
                    StringBuilder cInfo = new StringBuilder();
                    StringBuilder dInfo = new StringBuilder();
                    StringBuilder strInfo = new StringBuilder();
                    strInfo.Append("({");
                    strInfo.Append("\"title\": \"历史结果对比\",");
                    strInfo.Append("\"subtitle\": \"\",");
                    strInfo.Append(string.Format("\"yunit\": \"{0}\",", drChrs[0][0].ToString()));
                    cInfo.Append("[");
                    dInfo.Append("[");

                    JsTime(drChrs[0], cInfo, dInfo);
                    cInfo.Append("]");
                    dInfo.Append("]");
                    strInfo.Append("\"categories\": " + cInfo.Remove(1, 1) + ",");
                    strInfo.Append("\"data\": " + dInfo.Remove(1, 1) + ",");
                    strInfo.Append("})");

                    txttest.Text = strInfo.ToString();
                    //Response.Write("<script>alert('" + strInfo.ToString() + "');</script>");
                    //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(UpdatePanel), "popMessage2", "alert('" + strInfo.ToString() + "');", true);
                    ScriptManager.RegisterStartupScript(this.gridHistory, this.GetType(), "", "LoadHigChar()", true);

                }
            }

        }

        private void JsTime(DataRow drHistory, StringBuilder cInfo, StringBuilder dInfo)
        {
            for (int i = drHistory.Table.Columns.Count - 1; i >= 0; i--)
            {
                DataColumn col = drHistory.Table.Columns[i];

                if (col.ColumnName != "res_itm_ecd" && col.ColumnName != "res_itm_id" && col.ColumnName != "项目")
                {
                    object objValue = drHistory[col.ColumnName];
                    decimal decValue;
                    if (decimal.TryParse(objValue.ToString().Split('(')[0], out decValue))
                    {
                        string date = Convert.ToDateTime(col.ColumnName).ToString("MM-dd");
                        string iuv = decValue.ToString();
                        //if (i == 2)
                        //{
                        //    cInfo.Append("\"" + date + "\"");
                        //    dInfo.Append(iuv);
                        //}
                        //else
                        //{
                        cInfo.Append(",\"" + date + "\"");
                        dInfo.Append("," + iuv);
                        //}
                    }
                }
            }
        }
    }
}
