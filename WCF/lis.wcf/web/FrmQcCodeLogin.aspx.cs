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
    public partial class FrmQcCodeLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtUserName.Attributes.Add("onkeydown", "return doButton()");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text.Trim() == "" || txtPassword.Text.Trim() == "")
            {
                labMessage.Text = "用户名或密码不能为空";
            }
            else
            {
                DataSet dsView = new DataSet();
                DataTable dtView = new DataTable("login");
                dtView.Columns.Add("loginID");
                dtView.Columns.Add("password");
                dtView.Columns.Add("ip");
                dtView.Columns.Add("mac");
                dtView.Columns.Add("action");

                DataRow dr = dtView.NewRow();
                dr["loginID"] = txtUserName.Text.Trim();
                dr["password"] = EncryptClass.Encrypt(txtPassword.Text.Trim());
                dr["ip"] = Page.Request.UserHostAddress;
                dr["mac"] = "";
                dr["action"] = "login";

                dtView.Rows.Add(dr);
                dsView.Tables.Add(dtView);

                dcl.svr.frame.LoginBIZ biz = new dcl.svr.frame.LoginBIZ();
                DataSet ds = biz.doView(dsView);

                switch (ds.Tables[UT_SearchTableName.ERROEINFO].Rows[0]["loginStatus"].ToString())
                {
                    case "2":
                        {
                            labMessage.Text = "用户不存在";
                            break;
                        }
                    case "3":
                        {
                            labMessage.Text = "密码错误";
                            break;
                        }
                    case "4":
                        {
                            labMessage.Text = "用户没有查询权限";
                            break;
                        }
                    default:
                        {
                            labMessage.Text = "登录成功";

                            //保存登录用户并跳转页面
                            Session[FrmQcCodeConfirm.SessionName] = ds.Tables["userInfo"].Rows[0]["userName"].ToString();
                            Session[FrmQcCodeConfirm.SessionId] = ds.Tables["userInfo"].Rows[0]["LoginId"].ToString();

                            Response.Redirect("FrmQcCodeConfirm.aspx", true);
                            break;
                        }

                }
            }

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtUserName.Text = "";
            txtPassword.Text = "";
        }
    }
}
