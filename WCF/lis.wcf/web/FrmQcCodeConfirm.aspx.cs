using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using dcl.root.dto;
using dcl.common;

namespace dcl.pub.wcf.web
{
    public partial class FrmQcCodeConfirm : System.Web.UI.Page
    {
        //说明_此全局静态字符串变量保存Session名称而不是登录用户信息,使用Session[FrmLogin.SessionName]或Session[FrmLogin.SessionId]获得登录用户信息
        public static string SessionName = "UserName";
        public static string SessionId = "LoginId";

        public static string SessionText = "lblMsgText";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session[FrmQcCodeConfirm.SessionId] == null || Session[FrmQcCodeConfirm.SessionId].ToString()=="")
                {
                    Response.Redirect("FrmQcCodeLogin.aspx");
                }

                //Session[FrmQcCodeConfirm.SessionName] = "";
                //Session[FrmQcCodeConfirm.SessionId] = "";

                //txtUserCode.ReadOnly = false;
                //txtUserCode.Text = "";
                //Session[FrmQcCodeConfirm.SessionText] = "null";
                //txtQcCode.Text = "";
                //labMessage.Text = "请验证";
                //lblMsg.Text = "";
            }
        }

        protected void txtQcCode_TextChanged(object sender, EventArgs e)
        {
            if (txtQcCode.Text == null || txtQcCode.Text.Trim().Length <= 0)
            {
                lblMsg.Text = "";
                txtQcCode.Focus();
                return;
            }

            if (Session[FrmQcCodeConfirm.SessionId] == null || Session[FrmQcCodeConfirm.SessionId].ToString() == "")
            {
                lblMsg.Text = "未登录,不能签收";
                Response.Redirect("FrmQcCodeLogin.aspx", true);
            }
            else
            {
                //lblMsg.Text = txtQcCode.Text;

                lblMsg.Text = "";

                try
                {

                    string ddltypeValue = DDLType.SelectedValue;
                    string strbarcode = txtQcCode.Text;

                    txtQcCode.Text = "";

                    string sqlWhere = string.Format(" 1=1 and {0} = '{1}' ", "bc_bar_no", strbarcode);

                    sqlWhere = " WHERE " + sqlWhere;

                    dcl.svr.sample.BCPatientBIZ bcpat = new dcl.svr.sample.BCPatientBIZ();
                    DataSet results = bcpat.Search(sqlWhere);

                    if (results != null && results.Tables[0] != null && results.Tables[0].Rows.Count > 0
                        )
                    {
                        if (results.Tables[0].TableName == "_ERRORINFO")
                        {
                            throw new Exception(results.Tables[0].Rows[0][1].ToString());
                        }

                        if (ddltypeValue == "1")//核酸提取
                        {
                            #region 核酸提取

                            #region 检查状态

                            string status = results.Tables[0].Rows[0]["bc_status"].ToString();
                            string temp_bc_del = results.Tables[0].Rows[0]["bc_del"].ToString();
                            string temp_bc_receiver_flag = results.Tables[0].Rows[0]["bc_receiver_flag"].ToString();
                            string temp_bc_status_cname = results.Tables[0].Rows[0]["bc_status_cname"].ToString();
                            if (temp_bc_del != "0")
                            {
                                lblMsg.Text = "条码已被删除,不能核酸提取！";
                                txtQcCode.Focus();
                                return;
                            }

                            dcl.svr.sample.BCSignBIZ bcsign = new dcl.svr.sample.BCSignBIZ();
                            DataTable dtTsign = bcsign.SearchByBarcode(strbarcode);

                            //5501  核酸提取
                            //5502  核酸定量扩增
                            if (dtTsign != null && dtTsign.Rows.Count > 0)
                            {
                                if (dtTsign.Select("bc_status='5501'").Length > 0)
                                {
                                    lblMsg.Text = "条码已核酸提取,不能再确认！";
                                    txtQcCode.Focus();
                                    return;
                                }
                            }

                            #endregion

                            dcl.svr.sample.BarcodeBIZ barbiz = new dcl.svr.sample.BarcodeBIZ();
                            string sDti = barbiz.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                            string temp_bc_receiver_name = Session[FrmQcCodeConfirm.SessionName].ToString();
                            string temp_bc_login_id = Session[FrmQcCodeConfirm.SessionId].ToString();
                            string ip = Page.Request.UserHostAddress;

                            string colnm = "bc_login_id, bc_name, bc_date, bc_status, bc_bar_no, bc_place, bc_bar_code ,bc_flow ,bc_remark";
                            string colv = string.Format("'{0}', '{1}' , '{2}' ,'5501', '{3}', 'IP地址:{4}' ,'{3}' ,1 ,'核酸提取'",
                                temp_bc_login_id, temp_bc_receiver_name, sDti, strbarcode, ip);

                            bcsign.Insert(colnm, colv);

                            lblMsg.Text = strbarcode + ",OK!";

                            #endregion
                        }
                        else if (ddltypeValue == "2")//核酸定量扩增
                        {
                            #region 核酸定量扩增

                            #region 检查状态

                            string status = results.Tables[0].Rows[0]["bc_status"].ToString();
                            string temp_bc_del = results.Tables[0].Rows[0]["bc_del"].ToString();
                            string temp_bc_receiver_flag = results.Tables[0].Rows[0]["bc_receiver_flag"].ToString();
                            string temp_bc_status_cname = results.Tables[0].Rows[0]["bc_status_cname"].ToString();
                            if (temp_bc_del != "0")
                            {
                                lblMsg.Text = "条码已被删除,不能核酸定量扩增！";
                                txtQcCode.Focus();
                                return;
                            }

                            dcl.svr.sample.BCSignBIZ bcsign = new dcl.svr.sample.BCSignBIZ();
                            DataTable dtTsign = bcsign.SearchByBarcode(strbarcode);

                            //5501  核酸提取
                            //5502  核酸定量扩增
                            if (dtTsign != null && dtTsign.Rows.Count > 0)
                            {
                                //if (dtTsign.Select("bc_status='5501'").Length <= 0)
                                //{
                                //    lblMsg.Text = "条码未核酸提取,不能核酸定量扩增！";
                                //    return;
                                //}

                                if (dtTsign.Select("bc_status='5502'").Length > 0)
                                {
                                    lblMsg.Text = "条码未已核酸定量扩增,不能再确认！";
                                    txtQcCode.Focus();
                                    return;
                                }
                            }

                            #endregion

                            dcl.svr.sample.BarcodeBIZ barbiz = new dcl.svr.sample.BarcodeBIZ();
                            string sDti = barbiz.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                            string temp_bc_receiver_name = Session[FrmQcCodeConfirm.SessionName].ToString();
                            string temp_bc_login_id = Session[FrmQcCodeConfirm.SessionId].ToString();
                            string ip = Page.Request.UserHostAddress;

                            string colnm = "bc_login_id, bc_name, bc_date, bc_status, bc_bar_no, bc_place, bc_bar_code ,bc_flow ,bc_remark";
                            string colv = string.Format("'{0}', '{1}' , '{2}' ,'5502', '{3}', 'IP地址:{4}' ,'{3}' ,1 ,'核酸提取'",
                                temp_bc_login_id, temp_bc_receiver_name, sDti, strbarcode, ip);

                            bcsign.Insert(colnm, colv);

                            lblMsg.Text = strbarcode + ",OK!";

                            #endregion
                        }
                        else
                        {
                            #region 签收

                            #region 检查状态

                            string status = results.Tables[0].Rows[0]["bc_status"].ToString();
                            string temp_bc_del = results.Tables[0].Rows[0]["bc_del"].ToString();
                            string temp_bc_receiver_flag = results.Tables[0].Rows[0]["bc_receiver_flag"].ToString();
                            string temp_bc_status_cname = results.Tables[0].Rows[0]["bc_status_cname"].ToString();
                            if (temp_bc_del != "0")
                            {
                                lblMsg.Text = "条码已被删除,不能签收！";
                                txtQcCode.Focus();
                                return;
                            }
                            else if (status == "9")
                            {
                                lblMsg.Text = "条码已回退未打印,不能签收！";
                                txtQcCode.Focus();
                                return;
                            }
                            else if (status == "0" || status == "9")
                            {
                                lblMsg.Text = "条码未打印,不能签收！";
                                txtQcCode.Focus();
                                return;
                            }
                            else if (temp_bc_receiver_flag == "1" || status == "5")
                            {
                                lblMsg.Text = "条码已签收！";
                                txtQcCode.Focus();
                                return;
                            }
                            else if (status != "1" && status != "2" && status != "3" && status != "4" && !string.IsNullOrEmpty(temp_bc_status_cname)
                                && temp_bc_status_cname.Contains("已"))
                            {
                                lblMsg.Text = "条码" + temp_bc_status_cname + ",不能签收！";
                                txtQcCode.Focus();
                                return;
                            }
                            else if (status != "1" && status != "2" && status != "3" && status != "4" && !string.IsNullOrEmpty(temp_bc_status_cname)
                                && !temp_bc_status_cname.Contains("已"))
                            {
                                lblMsg.Text = "条码,已" + temp_bc_status_cname + ",不能签收！";
                                txtQcCode.Focus();
                                return;
                            }

                            #endregion

                            dcl.svr.sample.BarcodeBIZ barbiz = new dcl.svr.sample.BarcodeBIZ();
                            string sDti = barbiz.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                            string temp_bc_receiver_name = Session[FrmQcCodeConfirm.SessionName].ToString();
                            string temp_bc_login_id = Session[FrmQcCodeConfirm.SessionId].ToString();
                            string ip = Page.Request.UserHostAddress;


                            string setBc = string.Format("bc_status = '5', bc_status_cname = '已签收' ,  bc_receiver_flag = 1 , bc_receiver_date = '{0}' ,bc_receiver_code = 'admin',bc_receiver_name = '{1}',bc_lastaction_time = '{0}'", sDti, temp_bc_receiver_name);
                            string whereBc = string.Format("  bc_bar_no ='{0}'  ", strbarcode);



                            bcpat.Update(setBc, whereBc);

                            //INSERT INTO  bc_sign (  bc_login_id, bc_name, bc_date, bc_status, bc_bar_no, bc_place, bc_bar_code ,bc_flow ,bc_remark) VALUES (  'admin', '超级管理员' , '2016-05-06 17:14:39' ,'5', '18107703', '1号窗口' ,'18107703' ,1 ,'IP地址:172.17.24.1' )   
                            string colnm = "bc_login_id, bc_name, bc_date, bc_status, bc_bar_no, bc_place, bc_bar_code ,bc_flow ,bc_remark";
                            string colv = string.Format("'{0}', '{1}' , '{2}' ,'5', '{3}', 'IP地址:{4}' ,'{3}' ,1 ,'二维码签收'",
                                temp_bc_login_id, temp_bc_receiver_name, sDti, strbarcode, ip);

                            dcl.svr.sample.BCSignBIZ bcsign = new dcl.svr.sample.BCSignBIZ();
                            bcsign.Insert(colnm, colv);

                            lblMsg.Text = strbarcode + ",OK!";

                            #endregion
                        }
                    }
                    else
                    {
                        lblMsg.Text = "没此条码信息";
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    lblMsg.Text = "遇到错误,详情请查看日志。";
                }

                txtQcCode.Focus();
            }
        }
    }
}
