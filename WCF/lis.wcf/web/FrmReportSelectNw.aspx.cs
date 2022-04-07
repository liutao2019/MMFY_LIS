using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using dcl.svr.cache;
using dcl.common;

namespace dcl.pub.wcf.web
{
    public partial class FrmReportSelectNw : System.Web.UI.Page
    {
        /// <summary>
        /// 判断用户权限
        /// </summary>
        private void CheckAuthority()
        {
            //if (Session[FrmLogin.SessionId] == null)
            //{
            //    Response.Redirect("FrmLogin.aspx");
            //}
        }

        /// <summary>
        /// 页面载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //CheckAuthority();

            if (!IsPostBack)
            {
                string tip = CacheSysConfig.Current.GetSystemConfig("Custome_WebTip");

                if (!string.IsNullOrEmpty(tip))
                {
                    Lbl1.Text = tip;
                }
                if (CacheSysConfig.Current.GetSystemConfig("Open_WebReportPrint") != "是")
                {
                    Button1.Visible = false;
                    link1.Visible = false;
                }
                string pidname = CacheSysConfig.Current.GetSystemConfig("Web_PatientIdChineseName");
                if (!string.IsNullOrEmpty(pidname) && pidname != "默认")
                {
                    Label4.Text = pidname;
                }

                btModifyPwd.Visible = false;//默认隐藏密码修改按钮

                //if (Request["frmid"] == "2")//是否登录后才能进入,3为是
                {
                    if (Session[FrmLogin.SessionId] == null)
                    {
                        Response.Redirect("FrmLogin.aspx?frmid=3");
                    }
                    else
                    {
                        btModifyPwd.Visible = true;

                        #region 根据用户权限分配查询条件
                        dcl.svr.result.PatientEnterService patEnterService = new dcl.svr.result.PatientEnterService();

                        //性别
                        if (patEnterService.GetUserHaveFunctionByCode(Session[FrmLogin.SessionId].ToString(), "webReportSelectNw_Sex"))
                        {
                            ddlSex.Enabled = true;
                        }
                        else
                        {
                            ddlSex.Enabled = false;
                            ddlSex.SelectedValue = "0";
                        }

                        //床号
                        if (patEnterService.GetUserHaveFunctionByCode(Session[FrmLogin.SessionId].ToString(), "webReportSelectNw_Bed"))
                        {
                            txtPatBed.Enabled = true;
                        }
                        else
                        {
                            txtPatBed.Enabled = false;
                        }

                        //标本类别
                        if (patEnterService.GetUserHaveFunctionByCode(Session[FrmLogin.SessionId].ToString(), "webReportSelectNw_Sample"))
                        {
                            txtSample.Enabled = true;
                        }
                        else
                        {
                            txtSample.Enabled = false;
                        }
                        

                        //组合项目
                        if (patEnterService.GetUserHaveFunctionByCode(Session[FrmLogin.SessionId].ToString(), "webReportSelectNw_Combine"))
                        {
                            txtCombine.Enabled = true;
                        }
                        else
                        {
                            txtCombine.Enabled = false;
                        }

                        //临床诊断
                        if (patEnterService.GetUserHaveFunctionByCode(Session[FrmLogin.SessionId].ToString(), "webReportSelectNw_diagnos"))
                        {
                            txtdiagnos.Enabled = true;
                        }
                        else
                        {
                            txtdiagnos.Enabled = false;
                        }

                        #endregion
                    }
                }

                //跳转到危急报告。
                if (Request["returnCriticalReport"] == "true")
                {
                    string strPat_id = Request["pat_id"];
                    Response.Redirect("frmCriticalCheckReport.aspx?pat_id=" + strPat_id);
                }
                ResetData();

                //if (!string.IsNullOrEmpty(Request["p_id"])
                //     || !string.IsNullOrEmpty(Request.QueryString["Name"])
                //   )
                //{
                //    this.txtNo.Text = Request["p_id"];
                //    this.txtName.Text = Request["Name"];
                //    SearchData(0);
                //}

                //是否显示查询栏
                if (Request["ShowSearchbar"] == "false")
                {
                    tbSearchbar.Visible = false;
                    //trSearchbar2.Visible = false;
                }
                else
                {
                    tbSearchbar.Visible = true;
                    //trSearchbar2.Visible = true;
                }

                if (!string.IsNullOrEmpty(Request["CustomMessage"]))
                {
                    lblCustomMessage.Text = Request["CustomMessage"];
                    lblCustomMessage.Visible = true;
                    brCustomMessage.Visible = true;
                }
                else
                {
                    lblCustomMessage.Visible = false;
                    brCustomMessage.Visible = false;
                }


                //是否显示病人列表栏
                if (Request["ShowPatList"] == "false")
                {
                    tdLeft.Visible = false;
                }
                else
                {
                    tdLeft.Visible = true;
                }

                if (!string.IsNullOrEmpty(Request["pat_id"]))
                {
                    Session["pat_id"] = Request["pat_id"];
                    SearchData(0, false);
                }
                else
                {







                    this.txtNo.Text = Request["p_id"];

                    this.txtName.Text = Request["Name"];

                    this.txtNo2.Text = Request["p_id2"];
                    txtPrintFlag.Text = Request["AutoPrint"];
                    txtAutoClose.Text = Request["AutoClose"];

                    if (!string.IsNullOrEmpty(Request["sDate"]))
                    {
                        string sDate = Request["sDate"];

                        DateTime dtFrom;
                        if (DateTime.TryParse(sDate, out dtFrom))
                        {
                            this.txtTimeFrom.Text = dtFrom.ToString("yyyy-MM-dd");
                        }
                    }

                    if (!string.IsNullOrEmpty(Request["eDate"]))
                    {
                        string eDate = Request["eDate"];

                        DateTime dtTo;
                        if (DateTime.TryParse(eDate, out dtTo))
                        {
                            this.txtTimeTo.Text = dtTo.ToString("yyyy-MM-dd");
                        }
                    }

                    if (!string.IsNullOrEmpty(Request["depcode"]))
                    {
                        string depcode = Request["depcode"];
                        //txtDepart_AutoCompleteExtender.sel
                        //valDepart.Value = depcode;

                        if (AutoCompleteDict.dtDepart == null)
                        {
                            new AutoCompleteDict().GetDepart("", 10);
                        }

                        DataRow[] drs = AutoCompleteDict.dtDepart.Select(string.Format("dep_code = '{0}'", depcode));
                        if (drs.Length > 0)
                        {
                            valDepart.Value = depcode;
                            txtDepart.Text = drs[0]["dep_name"].ToString();
                        }

                    }


                    SearchData(0, false);
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        /// <summary>
        /// 重置搜索条件
        /// </summary>
        private void ResetData()
        {
            //在前台设置readonly会导致提交后文本内容丢失
            //txtTimeFrom.Attributes["readonly"] = "true";
            //txtTimeTo.Attributes["readonly"] = "true";

            int intDefaultQueryInterval = 7;

            string stringDefaultQueryInterval = dcl.common.SystemConfiguration.GetSystemConfig("Select_DefaultQueryInterval");

            if (stringDefaultQueryInterval != string.Empty)
            {
                int.TryParse(stringDefaultQueryInterval, out intDefaultQueryInterval);
                if (intDefaultQueryInterval == 0)
                {
                    intDefaultQueryInterval = 1;
                }
            }

            string now = DateTime.Now.ToString("yyyy-MM-dd");
            txtTimeFrom.Text = DateTime.Now.AddDays(1 - intDefaultQueryInterval).Date.ToString("yyyy-MM-dd");
            txtTimeTo.Text = DateTime.Now.ToString("yyyy-MM-dd");

            //txtNoType.Text = "";
            txtNo.Text = "";
            txtName.Text = "";
            txtDepart.Text = "";
            txtSample.Text = "";
            txtCombine.Text = "";
            ddlSex.SelectedValue = "0";
            txtPatBed.Text = "";
            txtdiagnos.Text = "";
            //txtOrigin.Text = "";
            //txtBarcode.Text = "";
            //txtReportType.Text = "";

            valNoType.Value = "";
            valOrigin.Value = "";
            valReportType.Value = "";
            valSample.Value = "";
            valDepart.Value = "";
            valCombine.Value = "";
        }

        /// <summary>
        /// 重置搜索条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ResetData();
        }

        /// <summary>
        /// 搜索病人资料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchData(0, true);
        }

        /// <summary>
        /// 搜索数据
        /// </summary>
        /// <param name="pageIndex"></param>
        private void SearchData(int pageIndex, bool isManu)
        {
            try
            {
                this.Focus();
                erroMes.Text = "";
                //为方便录入设置时间文本框可录入
                string now = DateTime.Now.ToString("yyyy-MM-dd");
                try
                {
                    Convert.ToDateTime(txtTimeFrom.Text);
                }
                catch
                {
                    txtTimeFrom.Text = DateTime.Now.AddDays(-1).Date.ToString("yyyy-MM-dd");
                }
                try
                {
                    Convert.ToDateTime(txtTimeTo.Text);
                }
                catch
                {
                    txtTimeTo.Text = now;
                }
                if (txtDepart.Text.Trim() == "")
                    valDepart.Value = "";
                string where = "";
                string CDRWhere = string.Empty;
                System.TimeSpan ts = Convert.ToDateTime(txtTimeTo.Text).Subtract(Convert.ToDateTime(txtTimeFrom.Text));
                int day = ts.Days;
                if (day < 0)
                {
                    string popMessage = "alert('结束日期不能大于开始日期！');";
                    ShowMessage(popMessage);
                    return;
                }

                //if (day > 30)
                //{
                //    string popMessage = "alert('日期范围不能超过1个月！');";
                //    ShowMessage(popMessage);
                //    return;
                //}

                if (Session["pat_id"] != null && !string.IsNullOrEmpty(Session["pat_id"].ToString()))
                {
                    where += " and patients.pat_id= '" + Session["pat_id"].ToString() + "'";
                    CDRWhere += " and cdr_lis_main.lismain_repno= '" + Session["pat_id"].ToString() + "'";
                    Session.Clear();
                }
                else
                {
                    if (pageIndex == 0 && isManu && CacheSysConfig.Current.GetSystemConfig("WebSelect_NeedAllCondition") == "是")
                    {
                        if (txtTimeFrom.Text == "" || txtNo.Text.Trim() == "" || txtName.Text.Trim() == "" || txtDepart.Text.Trim() == "")
                        {

                            ScriptManager.RegisterStartupScript(btnSearch, this.GetType(), "myscriptser", "AlterMessage('查询条件（时间、科室、病人号、姓名）都不能为空！')" + ";", true);
                            //erroMes.Text = "查询条件（时间、科室、病人号、姓名）都不能为空！";
                            //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "hello", "<script>alert('请输入查询条件！')</script>");
                            return;
                        }
                    }
                    if (valDepart.Value.Trim() == "" && txtNo.Text.Trim() == "" && txtName.Text.Trim() == "" &&
                        txtDepart.Text.Trim() == "" && txtPatBed.Text.Trim() == "" && valSample.Value.Trim()==""
                        && txtCombine.Text.Trim() == "" && txtdiagnos.Text.Trim()=="")
                    {
                        erroMes.Text = "请输入查询条件！";
                        //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "hello", "<script>alert('请输入查询条件！')</script>");
                        return;
                    }
                    //起始时间
                    where += " and patients.pat_date>= '" + Convert.ToDateTime(txtTimeFrom.Text) + "'";
                    CDRWhere += " and cdr_lis_main.lismain_receive_time>= '" + Convert.ToDateTime(txtTimeFrom.Text) + "'";

                    //结束时间
                    where += " and patients.pat_date<='" + Convert.ToDateTime(txtTimeTo.Text).AddDays(1) + "'";
                    CDRWhere += " and cdr_lis_main.lismain_receive_time<='" + Convert.ToDateTime(txtTimeTo.Text).AddDays(1) + "'";
                }


                //ID类型
                //if (valNoType.Value != "")
                //{
                //    where += "and patients.pat_no_id='" + valNoType.Value + "'";
                //}

                //病人ID
                if (txtNo.Text.Trim() != "" && string.IsNullOrEmpty(txtNo2.Text))
                {
                    where +=
                        string.Format(
                            " and (patients.pat_in_no in ('{0}')  or patients.pat_pid in ('{0}') or patients.pat_social_no in ('{0}'))",
                            txtNo.Text.Trim());
                    CDRWhere += string.Format(" and cdr_lis_main.lismain_hospitalno='{0}'", txtNo.Text.Trim());
                    //where += string.Format(" and (patients.pat_in_no like '%{0}%' or patients.pat_pid = '{0}')", txtNo.Text.Trim());
                }

                if (txtNo.Text.Trim() != "" && !string.IsNullOrEmpty(txtNo2.Text))
                {
                    where +=
                        string.Format(
                            " and (patients.pat_in_no = '{0}' or patients.pat_in_no = '{1}' or patients.pat_pid = '{0}'  )",
                            txtNo.Text.Trim(), txtNo2.Text.Trim());
                    CDRWhere += string.Format(" and cdr_lis_main.lismain_hospitalno='{0}'", txtNo.Text.Trim());
                    //where += string.Format(" and (patients.pat_in_no like '%{0}%' or patients.pat_pid = '{0}')", txtNo.Text.Trim());
                }

                //姓名
                if (txtName.Text.Trim() != "")
                {
                    where += " and patients.pat_name like '" + txtName.Text.Trim() + "%'";
                    CDRWhere += " and cdr_lis_main.lismain_name like '%" + txtName.Text.Trim() + "%'";
                }

                //性别
                if (ddlSex.SelectedValue != "0")
                {
                    if (ddlSex.SelectedValue == "1")
                    {
                        where += " and patients.pat_sex='1'";  
                    }
                    else
                    {
                        where += " and patients.pat_sex='2'";
                    }
                }

                //床号
                if (txtPatBed.Text != null && txtPatBed.Text.Trim().Length > 0)
                {
                    where += " and patients.pat_bed_no='" + txtPatBed.Text.Trim() + "'";  
                }

                //标本类别
                if (valSample.Value != "")
                {
                    where += " and patients.pat_sam_id='" + txtPatBed.Text.Trim() + "'";
                }

                //组合项目
                if (txtCombine.Text != null && txtCombine.Text.Length > 0)
                {
                    where += " and patients.pat_c_name like '%" + txtCombine.Text.Trim() + "%'";
                }

                //临床诊断
                if (txtdiagnos.Text != null && txtdiagnos.Text.Trim().Length > 0)
                {
                    where += " and patients.pat_diag like '%" + txtdiagnos.Text.Trim() + "%'";
                }

                //科室
                if (valDepart.Value != "" || txtDepart.Text.Trim() != "")
                {
                    if (valDepart.Value == "" && txtDepart.Text.Trim() != "")
                    {
                        where += " and  patients.pat_dep_name='" + txtDepart.Text + "'";

                    }
                    else if (valDepart.Value != "" && txtDepart.Text.Trim() == "")
                    {
                        where += " and patients.pat_ward_id='" + valDepart.Value + "'";
                    }
                    else
                    {
                        where += " and (patients.pat_ward_id='" + valDepart.Value + "' or patients.pat_dep_name='" +
                                 txtDepart.Text + "')";
                    }



                }
                if (txtDepart.Text.Trim() != string.Empty)
                    CDRWhere += string.Format(" and (cdr_lis_main.lismain_appdept_name like '{0}%') ", txtDepart.Text);


                //标本类别
                //if (valSample.Value != "")
                //{
                //    where += " and patients.pat_sam_id='" + valSample.Value + "'";
                //}

                //病人来源
                //if (valOrigin.Value != "")
                //{
                //    where += " and patients.pat_ori_id='" + valOrigin.Value + "'";
                //}

                //不允许查看保密资料
                where += " and pat_flag in ('2','4')"; //

                //项目组合

                //条形码

                //报告类型

                DataSet dsWhere = new DataSet();
                DataTable dtWhere = new DataTable("Sqlwhere");
                dtWhere.Columns.Add("code");
                dtWhere.Columns.Add("type", typeof(String));
                dtWhere.Rows.Add(where, "Lis");
                dsWhere.Tables.Add(dtWhere);



                if (dcl.common.SystemConfiguration.GetSystemConfig("InpatientsReportCDR") == "是")
                {
                    dtWhere.Rows.Add(CDRWhere, "CDR");
                }


                dcl.svr.resultquery.CombineModeSelBIZ biz = new dcl.svr.resultquery.CombineModeSelBIZ();
                DataTable dtPatients = new DataTable();
                dtPatients = biz.doOther(dsWhere).Tables["patients"];

                //病人列表按照时间，名称排序
                if (dtPatients != null && dtPatients.Rows.Count > 0
                    && dtPatients.Columns.Contains("pat_date")
                    && dtPatients.Columns.Contains("pat_name"))
                {
                    DataView dvtemp = dtPatients.DefaultView;
                    string strDTName = dtPatients.TableName;
                    dvtemp.Sort = " pat_date desc,pat_name asc";
                    dtPatients = dvtemp.ToTable(strDTName);
                }


                gvPatients.DataSource = dtPatients;
                gvPatients.PageIndex = pageIndex;
                gvPatients.DataBind();

                if (gvPatients.Rows.Count > 0)
                {
                    gvPatients.SelectedIndex = 0;
                    gvPatients_SelectedIndexChanged(null, null);
                }
                else
                {
                    clearResulto();
                }
                if (!string.IsNullOrEmpty(txtPrintFlag.Text) && txtPrintFlag.Text == "1")
                {
                    if (dtPatients != null && dtPatients.Rows.Count > 0)
                    {
                        DataRow[] rows = dtPatients.Select("pat_flag=2");
                        if (rows.Length > 0)
                        {
                            Dictionary<string, Hashtable> dic = new Dictionary<string, Hashtable>();

                            foreach (DataRow row in rows)
                            {
                                string patId = row["pat_id"].ToString();

                                //报表代码
                                string reportId = row["itr_rep_id"].ToString();
                                if (patId != null && patId != "" && reportId != null && reportId != "")
                                {
                                    //特殊情况_因为现在系统一种仪器只对应一个报表类型,而细菌报表药敏/无菌涂片的格式和数据表均不同,所以根据cs_rlts表有无数据来做区分
                                    if (reportId == "bacilli")
                                    {
                                        DataSet dsParId = new DataSet();
                                        DataTable dtParId = new DataTable("cs_rlts");
                                        dtParId.Columns.Add("parId");
                                        dtParId.Rows.Add(patId);
                                        dsParId.Tables.Add(dtParId);

                                        if (biz.doView(dsParId).Tables[0].Rows.Count > 0)
                                            reportId = "smear";
                                    }
                                    Hashtable par = new Hashtable();
                                    Hashtable reportPar = new Hashtable();
                                    reportPar["&where&"] = " and patients.pat_id ='" + patId + "'";
                                    par[reportId] = reportPar;
                                    dic.Add(patId, par);
                                }
                            }
                            //dcl.svr.result.PatientEnterService
                            List<string> patList = new List<string>();
                            XtraReport xr = Reports(dic, patList);
                            if (xr.Pages.Count > 0)
                            {
                                string path = @"..\uploadFile\";
                                DirectoryInfo dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(path));
                                if (!dir.Exists)
                                {
                                    dir.Create();
                                }
                                string file = path + Guid.NewGuid().ToString();

                                ImageExportOptions ieo = new ImageExportOptions();
                                ieo.ExportMode = ImageExportMode.SingleFilePageByPage;
                                ieo.Format = ImageFormat.Jpeg;
                                ieo.Resolution = 96;
                                string imagestr = string.Empty;
                                for (int i = 0; i < xr.Pages.Count; i++)
                                {

                                    string realFile = file + i + ".jpg";
                                    ieo.PageRange = (i + 1).ToString();
                                    xr.ExportToImage(Server.MapPath(realFile), ieo);
                                    if (string.IsNullOrEmpty(imagestr))
                                    {
                                        imagestr = realFile;
                                    }
                                    else
                                    {
                                        imagestr += "," + realFile;
                                    }
                                }
                                //ms.Seek(0, SeekOrigin.Begin);
                                //report = ms.ToArray();
                                if (txtAutoClose.Text == "1")
                                {
                                    ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>" + "CreateManyPrintPageWithClose('" + imagestr.Replace("\\", "|") + "')" + ";</script>");
                                }
                                else
                                {
                                    ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>" + "CreateManyPrintPage('" + imagestr.Replace("\\", "|") + "')" + ";</script>");
                                }
                                new dcl.svr.result.PatientEnterService().UpdatePrintState(patList);

                                foreach (string d in Directory.GetFileSystemEntries(HttpContext.Current.Server.MapPath(path)))
                                {
                                    if (File.Exists(d))
                                    {
                                        System.IO.FileInfo objFI = new System.IO.FileInfo(d);
                                        if (objFI.CreationTime < DateTime.Now.AddHours(-2))
                                            File.Delete(d);
                                    }

                                }

                            }
                        }
                    }
                    else
                    {
                        if (txtAutoClose.Text == "1")
                        {
                            ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>" + "ClosePageIn60('" + 60 + "')" + ";</script>");
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("网页查询", ex);
            }
        }

        /// <summary>
        /// 清空结果
        /// </summary>
        private void clearResulto()
        {
            gvResulto.DataSource = null;
            gvResulto.DataBind();
            lblZY.Text = "";
            lblSJDate.Text = "";
            lblSJKS.Text = "";
            lblCH.Text = "";
            lblZD.Text = "";
            lblYBNumber.Text = "";
            lblBBType.Text = "";
            lblBBBZ.Text = "";
            lblBBZT.Text = "";
            lblYBTM.Text = "";
            lblZH.Text = "";
            lblName.Text = "";
            lblSex.Text = "";
            lblAge.Text = "";
        }

        /// <summary>
        /// 消息提示
        /// </summary>
        /// <param name="popMessage"></param>
        private void ShowMessage(string popMessage)
        {
            //ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(UpdatePanel), "popMessage", popMessage, true);
        }

        /// <summary>
        /// 显示检验报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPatients_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvPatients.SelectedRow != null)
            {
                erroMes.Text = "";

                //病人资料表_主键
                string patId = gvPatients.SelectedRow.Cells[0].Text;

                lblZY.Text = gvPatients.SelectedRow.Cells[1].Text;
                lblSJDate.Text = gvPatients.SelectedRow.Cells[2].Text;
                lblSJKS.Text = gvPatients.SelectedRow.Cells[3].Text;
                lblCH.Text = gvPatients.SelectedRow.Cells[4].Text;
                lblZD.Text = gvPatients.SelectedRow.Cells[5].Text;
                lblYBNumber.Text = gvPatients.SelectedRow.Cells[6].Text;
                lblBBType.Text = gvPatients.SelectedRow.Cells[7].Text;
                lblBBBZ.Text = gvPatients.SelectedRow.Cells[8].Text;
                lblBBZT.Text = gvPatients.SelectedRow.Cells[9].Text;
                lblYBTM.Text = gvPatients.SelectedRow.Cells[10].Text;
                lblZH.Text = "(" + gvPatients.SelectedRow.Cells[11].Text + ")";
                lblMid.Text = gvPatients.SelectedRow.Cells[12].Text;
                lblCYdate.Text = gvPatients.SelectedRow.Cells[13].Text;
                lblDydate.Text = gvPatients.SelectedRow.Cells[14].Text;
                lblSQdoc.Text = gvPatients.SelectedRow.Cells[15].Text;
                lblLR.Text = gvPatients.SelectedRow.Cells[16].Text;
                lblSH.Text = gvPatients.SelectedRow.Cells[17].Text;
                lblBG.Text = gvPatients.SelectedRow.Cells[18].Text;
                lblSQdate.Text = gvPatients.SelectedRow.Cells[19].Text;
                lblJYdate.Text = gvPatients.SelectedRow.Cells[20].Text;
                lblSHdate.Text = gvPatients.SelectedRow.Cells[21].Text;
                lblBGdate.Text = gvPatients.SelectedRow.Cells[22].Text;
                lblName.Text = gvPatients.SelectedRow.Cells[24].Text;
                lblSex.Text = gvPatients.SelectedRow.Cells[28].Text;
                lblAge.Text = gvPatients.SelectedRow.Cells[29].Text;
                string type = gvPatients.SelectedRow.Cells[31].Text;
                if (patId != "")
                {
                    DataSet dsWhere = new DataSet();
                    DataTable dtPat = new DataTable();
                    dtPat.Columns.Add("pat_id");
                    dtPat.Columns.Add("pat_ctype");
                    dtPat.TableName = "patients";
                    dtPat.Rows.Add(patId, type);
                    dsWhere.Tables.Add(dtPat);
                    dcl.svr.resultquery.CombineModeSelBIZ biz = new dcl.svr.resultquery.CombineModeSelBIZ();
                    DataSet dsResulto = biz.doView(dsWhere);

                    if (dsResulto.Tables.Count > 1)
                    {
                        gvResulto.Visible = false;
                        gvBac.Visible = true;
                        gvAnti.Visible = true;
                        gvSmear.Visible = false;
                        gvBac.DataSource = dsResulto.Tables["ba_rlts"];
                        gvBac.DataBind();
                        gvAnti.DataSource = dsResulto.Tables["an_rlts"];
                        gvAnti.DataBind();
                    }
                    else
                    {
                        if (dsResulto.Tables["cs_rlts"] != null)
                        {
                            gvSmear.Visible = true;
                            gvResulto.Visible = false;
                            gvBac.Visible = false;
                            gvAnti.Visible = false;
                            gvSmear.DataSource = dsResulto.Tables["cs_rlts"];
                            gvSmear.DataBind();
                        }
                        else
                        {
                            gvResulto.Visible = true;
                            gvSmear.Visible = false;
                            gvBac.Visible = false;
                            gvAnti.Visible = false;
                            DataTable dtPatients = dsResulto.Tables["resulto"];

                            //系统配置：Web查询显示多参考值
                            if (CacheSysConfig.Current.GetSystemConfig("webSel_showRepExp") == "是"
                                && dtPatients != null && dtPatients.Rows.Count > 0 && dtPatients.Columns.Contains("res_ref_exp"))
                            {
                                foreach (DataRow drResulto in dtPatients.Rows)
                                {
                                    if (drResulto["res_ref"].ToString().Length <= 0
                                        && drResulto["res_ref_exp"].ToString().Length > 0)
                                    {
                                        drResulto["res_ref"] = drResulto["res_ref_exp"].ToString();
                                    }
                                }
                                dtPatients.AcceptChanges();
                            }

                            gvResulto.DataSource = dtPatients;
                            gvResulto.DataBind();

                            gvResulto.Columns[2].HeaderText = "结果";
                            gvResulto.Columns[3].Visible = true;
                            gvResulto.Columns[4].Visible = true;

                            //if (dtPatients != null && dtPatients.Rows.Count > 0)
                            //{
                            //    DataRow drResulto = dtPatients.Rows[0];
                            //    if (drResulto.Table.Columns.Contains("res_chr2") &&
                            //        drResulto["res_chr2"] != DBNull.Value &&
                            //        drResulto["res_chr2"].ToString().Trim() != string.Empty)
                            //    {
                            //        gvResulto.Columns[2].HeaderText = "结果";
                            //        gvResulto.Columns[3].Visible = true;
                            //        gvResulto.Columns[4].Visible = true;
                            //    }
                            //    else
                            //    {
                            //        gvResulto.Columns[2].HeaderText = "OD值";
                            //        gvResulto.Columns[3].Visible = false;
                            //        gvResulto.Columns[4].Visible = false;
                            //    }
                            //}
                            if (true)//显示异常提示和单位
                            {
                                gvResulto.Columns[3].Visible = true;
                                gvResulto.Columns[4].Visible = true;
                            }
                        }

                    }
                }
                //if (patId != "" && reportId != "")
                //{
                //    string showReport = "document.getElementById(('showReport')).src='FrmReport.aspx?patid=" + patId + "&reportid=" + reportId + "'";
                //    ScriptManager.RegisterClientScriptBlock(UpdatePanel1, typeof(UpdatePanel), "showReport", showReport, true); 
                //}
            }
        }

        /// <summary>
        /// 返回浏览的报表对象_参数对象内部结构与CS相同
        /// </summary>
        /// <param name="par"></param>
        /// <param name="patList"></param>
        /// <returns></returns>
        public XtraReport Reports(Dictionary<string, Hashtable> par, List<string> patList)
        {
            return null;
            //XtraReport xr = new XtraReport();
            //dcl.svr.report.ReportPrintBIZ biz = new dcl.svr.report.ReportPrintBIZ();

            //foreach (string key in par.Keys)
            //{
            //    foreach (DictionaryEntry entry in par[key])
            //    {
            //        try
            //        {
            //            DataSet dsRep = new DataSet();
            //            DataTable dtRep = new DataTable("rep");
            //            dtRep.Columns.Add("code");
            //            dtRep.Rows.Add(entry.Key.ToString());
            //            dsRep.Tables.Add(dtRep);
            //            DataTable dtReport = biz.doSearch(dsRep).Tables["report"];

            //            if (dtReport != null && dtReport.Rows.Count > 0)
            //            {
            //                DataRow drReports = dtReport.Rows[0];
            //                string path = Server.MapPath(@"..\xtraReport\" + drReports["repAddress"].ToString());
            //                string sql = EncryptClass.Decrypt(drReports["repSql"].ToString());
            //                Hashtable htRep = (Hashtable)entry.Value;
            //                foreach (DictionaryEntry de in htRep)
            //                {
            //                    sql = sql.Replace(de.Key.ToString(), de.Value.ToString());
            //                }

            //                DataSet dsSql = new DataSet();
            //                DataTable dtSql = new DataTable("Data");
            //                dtSql.Columns.Add("sql");
            //                dtSql.Rows.Add(sql);
            //                dsSql.Tables.Add(dtSql);
            //                DataSet dsReport = biz.doOther(dsSql);

            //                XtraReport xrRpe = new XtraReport();
            //                xrRpe.LoadLayout(path);
            //                xrRpe.DataSource = dsReport;
            //                xrRpe.CreateDocument();
            //                for (int i = 0; i < xrRpe.Pages.Count; i++)
            //                {
            //                    xr.Pages.Insert(xr.Pages.Count, xrRpe.Pages[i]);
            //                }
            //                patList.Add(key);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            Lib.LogManager.Logger.LogException("pat_id为:" + key + "打印失败", ex);
            //        }


            //    }
            //}

            //return xr;
        }

        /// <summary>
        /// 行选择效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPatients_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //此语句需要模板列中有选择列才能正常执行
                e.Row.Attributes.Add("onclick", "javascript:__doPostBack('" + gvPatients.ID + "','Select$" + e.Row.RowIndex + "')");

                //控制组合的长度
                for (int i = 0; i < gvPatients.Columns.Count; i++)
                {
                    if (gvPatients.Columns[i].HeaderText == "组合")
                    {
                        if (e.Row.Cells[i].Text != null)
                        {
                            if (e.Row.Cells[i].Text.Length > 10)
                            {
                                e.Row.Cells[i].Text = e.Row.Cells[i].Text.Substring(0, 10) + "...";
                            }
                        }
                        break;
                    }
                }

            }
        }

        /// <summary>
        /// 翻页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPatients_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (txtNo.Text.Trim() == "" && txtName.Text.Trim() == "" && txtDepart.Text.Trim() == "")
                return;

            gvPatients.PageIndex = e.NewPageIndex;
            SearchData(gvPatients.PageIndex, false);
        }

        /// <summary>
        /// 隐藏列_如果直接设置Visible会导致无法取得值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPatients_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < 23; i++)
                {
                    e.Row.Cells[i].Visible = false;
                }
                e.Row.Cells[30].Visible = false;
            }

        }
        /// <summary>
        /// 改变行颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvResulto_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
                if (e.Row.RowIndex >= 0)
                {
                    int rowIndex = e.Row.RowIndex;
                    DataRowView drv = e.Row.DataItem as DataRowView;

                    bool blnChangeRowColor = false;//记录是否改变行颜色

                    try
                    {
                        string res_ref_flag = drv["res_ref_flag"].ToString();//如果更新不全面,此字段容易异常
                        switch (res_ref_flag)
                        {
                            case "3": blnChangeRowColor = true; break;//阳性
                            case "6": blnChangeRowColor = true; break;//自定义危急值
                            case "16": blnChangeRowColor = true; break;//高于危急值上限
                            case "24": blnChangeRowColor = true; break;//高于参考值上限 并且 高于危急值上限
                            case "32": blnChangeRowColor = true; break;//高于阈值上限
                            case "40": blnChangeRowColor = true; break;//高于参考值上限 并且 高于阈值上限
                            case "48": blnChangeRowColor = true; break;//高于危急值上限 并且 高于阈值上限
                            case "56": blnChangeRowColor = true; break;//高于参考值上限 并且 高于危急值上限 并且 高于阈值上限
                            case "256": blnChangeRowColor = true; break;//低于危急值下限
                            case "384": blnChangeRowColor = true; break;//低于参考值下限 并且 低于危急值下限
                            case "512": blnChangeRowColor = true; break;//低于阈值下限
                            case "640": blnChangeRowColor = true; break;//低于参考值下限 并且 低于阈值下限
                            case "768": blnChangeRowColor = true; break;//低于危急值下限 并且 低于阈值下限
                            case "896": blnChangeRowColor = true; break;//低于参考值下限 并且 低于危急值下限 并且 低于阈值下限
                            default: blnChangeRowColor = false; break;
                        }
                        if (blnChangeRowColor)
                        {
                            //gvResulto.Rows[rowIndex].ForeColor = System.Drawing.Color.Red;
                            e.Row.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            erroMes.Text = "";
            if (gvPatients.SelectedRow != null)
            {
                string repid = gvPatients.SelectedRow.Cells[30].Text;
                string patid = gvPatients.SelectedRow.Cells[0].Text;
                string patitrid = gvPatients.SelectedRow.Cells[32].Text;//仪器id

                #region 杏坛医院特殊需求

                //杏坛医院,特殊的报告需要到检验科打印，自助和临床只能查询预览，不能打印。 
                //针对某些仪器的报告只能预览，不能打印。 
                //系统配置：住院检验报告查询不能打印的仪器ID(id1,id2)
                string s_InpatientsReportSelect_notprintItrIDs = CacheSysConfig.Current.GetSystemConfig("InpatientsReportSelect_notprintItrIDs");
                if (!string.IsNullOrEmpty(s_InpatientsReportSelect_notprintItrIDs) && !string.IsNullOrEmpty(patitrid))
                {
                    //检查是否不能打印的仪器id
                    if (s_InpatientsReportSelect_notprintItrIDs.Contains(patitrid))
                    {
                        erroMes.Text = "当前报告不能打印！请到检验科一楼咨询台打印！";
                        return;
                    }
                }
                #endregion

                LoadReport(patid, repid);
                //this.ClientScript.RegisterStartupScript(this.GetType(), "", "OpenDialogWithReturn('FrmReport.aspx',520,410,form1,'FrmReportSelect.aspx');", true);
            }
        }

        /// <summary>
        /// 载入报表
        /// </summary>
        private void LoadReport(string patId, string reportId)
        {
            ////病人资料表主键
            //string patId = Request["patid"];
            ////报表类型代码
            //string reportId = Request["reportid"];

            ////病人资料表主键
            //string patId = "10016201407141";
            ////报表类型代码
            //string reportId = "qyryPT_report";

            if (patId != null && patId != "" && reportId != null && reportId != "")
            {
                //特殊情况_因为现在系统一种仪器只对应一个报表类型,而细菌报表药敏/无菌涂片的格式和数据表均不同,所以根据cs_rlts表有无数据来做区分
                if (reportId == "bacilli")
                {
                    DataSet dsParId = new DataSet();
                    DataTable dtParId = new DataTable("cs_rlts");
                    dtParId.Columns.Add("parId");
                    dtParId.Rows.Add(patId);
                    dsParId.Tables.Add(dtParId);

                    dcl.svr.resultquery.CombineModeSelBIZ biz = new dcl.svr.resultquery.CombineModeSelBIZ();
                    DataSet dsBac = biz.doView(dsParId);
                    if (dsBac.Tables[0].Rows.Count > 0)
                    {
                        //系统配置：细菌管理同时有药敏与无菌结果时优先药敏
                        if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("BacLab_ExistsAnAndCs_SelAn") == "是"
                            && dsBac.Tables[1].Rows.Count > 0)
                        {
                        }
                        else
                        {
                            reportId = "smear";
                        }
                    }
                }

                Hashtable par = new Hashtable();
                Hashtable reportPar = new Hashtable();
                reportPar["&where&"] = " and patients.pat_id ='" + patId + "'";
                par[reportId] = reportPar;
                XtraReport xr = Reports(par);
                if (xr != null && xr.Pages.Count > 0)
                {
                    string path = @"..\uploadFile\";
                    DirectoryInfo dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(path));
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }
                    string file = path + Guid.NewGuid().ToString();

                    ImageExportOptions ieo = new ImageExportOptions();
                    ieo.ExportMode = ImageExportMode.SingleFilePageByPage;
                    ieo.Format = ImageFormat.Jpeg;
                    ieo.Resolution = 96;
                    string imagestr = string.Empty;
                    for (int i = 0; i < xr.Pages.Count; i++)
                    {

                        string realFile = file + i + ".jpg";
                        ieo.PageRange = (i + 1).ToString();
                        xr.ExportToImage(Server.MapPath(realFile), ieo);
                        if (string.IsNullOrEmpty(imagestr))
                        {
                            imagestr = realFile;
                        }
                        else
                        {
                            imagestr += "," + realFile;
                        }
                    }
                    //ms.Seek(0, SeekOrigin.Begin);
                    //report = ms.ToArray();

                    // ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>" + "CreateManyPrintPage('" + imagestr.Replace("\\", "|") + "')" + ";</script>");
                    // new dcl.svr.result.PatientEnterService().UpdatePrintState(new List<string>({patId});
                    ScriptManager.RegisterStartupScript(Button1, this.GetType(), "myscript22", "CreateManyPrintPage('" + imagestr.Replace("\\", "|") + "')" + ";", true);
                    List<string> patList = new List<string>();
                    patList.Add(patId);
                    new dcl.svr.result.PatientEnterService().UpdatePrintState(patList);

                    foreach (string d in Directory.GetFileSystemEntries(HttpContext.Current.Server.MapPath(path)))
                    {
                        if (File.Exists(d))
                        {
                            System.IO.FileInfo objFI = new System.IO.FileInfo(d);
                            if (objFI.CreationTime < DateTime.Now.AddHours(-2))
                                File.Delete(d);
                        }

                    }
                }
            }
        }

        /// <summary>
        /// 返回浏览的报表对象_参数对象内部结构与CS相同
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        private XtraReport Reports(Hashtable par)
        {
            return null;
            //XtraReport xr = new XtraReport();
            //dcl.svr.report.ReportPrintBIZ biz = new dcl.svr.report.ReportPrintBIZ();

            //foreach (DictionaryEntry deRep in par)
            //{
            //    DataSet dsRep = new DataSet();
            //    DataTable dtRep = new DataTable("rep");
            //    dtRep.Columns.Add("code");
            //    dtRep.Rows.Add(deRep.Key.ToString());
            //    dsRep.Tables.Add(dtRep);
            //    DataTable dtReport = biz.doSearch(dsRep).Tables["report"];

            //    if (dtReport != null && dtReport.Rows.Count > 0)
            //    {
            //        DataRow drReports = dtReport.Rows[0];
            //        string path = Server.MapPath(@"..\xtraReport\" + drReports["repAddress"].ToString());
            //        string sql = EncryptClass.Decrypt(drReports["repSql"].ToString());
            //        Hashtable htRep = (Hashtable)deRep.Value;
            //        foreach (DictionaryEntry de in htRep)
            //        {
            //            sql = sql.Replace(de.Key.ToString(), de.Value.ToString());
            //        }

            //        DataSet dsSql = new DataSet();
            //        DataTable dtSql = new DataTable("Data");
            //        dtSql.Columns.Add("sql");
            //        dtSql.Rows.Add(sql);
            //        dsSql.Tables.Add(dtSql);
            //        DataSet dsReport = biz.doOther(dsSql);

            //        XtraReport xrRpe = new XtraReport();
            //        xrRpe.LoadLayout(path);

            //        foreach (Band band in xrRpe.Bands)
            //        {
            //            XRControl ctrl = band.FindControl("xrlblHospitalName", true);
            //            if (ctrl != null)
            //            {
            //                ctrl.Text = Session[FrmLogin.SessionHospital].ToString();
            //                break;
            //            }
            //        }
            //        xrRpe.DataSource = dsReport;
            //        xrRpe.CreateDocument();
            //        for (int i = 0; i < xrRpe.Pages.Count; i++)
            //        {
            //            xr.Pages.Insert(xr.Pages.Count, xrRpe.Pages[i]);
            //        }
            //    }
            //}

            //return xr;
        }

        protected void GV_show_RowCreated(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Wrap = false;
                e.Row.Cells[i].Attributes.Add("style", "word-break :keep-all ; word-wrap:keep-all");
            }
        }

        protected void btModifyPwd_Click(object sender, EventArgs e)
        {
            if (true)//是否登录后才能进入,2为是
            {
                if (Session[FrmLogin.SessionId] != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myscript", "window.open('FrmModifyPwd.aspx?LoginId=" + Session[FrmLogin.SessionId].ToString() + "','_blank')", true);
                }
                else
                {
                    Response.Write("<script>window.open(FrmModifyPwd.aspx','_blank')</script>");
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "myscript", "window.open('FrmModifyPwd.aspx','_blank')", true);
            }
        }
    }
}
