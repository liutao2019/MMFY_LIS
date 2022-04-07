using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.common;
using dcl.client.frame;
using dcl.common;

using dcl.entity;
using System.Linq;
using dcl.client.wcf;
using System.Configuration;
using dcl.client.cache;
using dcl.client.ca;

namespace lis.client.control
{
    public partial class FrmCheckPassword : FrmCommon, IsignInterface
    {
        public string OperatorID { get; set; }
        public string OperatorName { get; set; }

        //操作人工号(输入码)
        public string OperatorSftId { get; set; }
        public string FuncInfoID { get; set; }
        public string FuncCode { get; set; }
        public string ModuleName { get; set; }
        public bool Power { get; set; }

        public string PowerName { get; set; }
        public string Pat_i_code { get; set; }
        public string PassWord { get; set; }

        /// <summary>
        /// 内容返回
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 验证方式
        /// </summary>
        public string CheckType { get; set; }
        /// <summary>
        /// CA签名类
        /// </summary>
        public ICaPKI caPKI { get; set; }
        /// <summary>
        /// CA签名认证操作
        /// </summary>
        public Lis.Client.CASign.FrmUserInfo CAUserInfo { get; set; } = null;
        /// <summary>
        /// 是否CA电子签名验证模式
        /// </summary>
        public string strCASignMode { get; set; }

        public bool CheckUserLogin()
        {
            if (this.ShowDialog() == DialogResult.OK)
                return true;
            return false;
        }

        public FrmCheckPassword(string funcCode)
        {
            InitializeComponent();
            FuncCode = funcCode;
        }

        public FrmCheckPassword(string funcInfoID, string funcCode, string moduleName)
        {
            InitializeComponent();

            FuncInfoID = funcInfoID;
            FuncCode = funcCode;
            ModuleName = moduleName;
        }

        public FrmCheckPassword(string title, string funcInfoID, string funcCode, string moduleName)
        {
            InitializeComponent();

            this.Text = title;
            FuncInfoID = funcInfoID;
            FuncCode = funcCode;
            ModuleName = moduleName;
        }

        public FrmCheckPassword(string title, string funcInfoID, string funcCode, string moduleName, string powerName)
        {
            InitializeComponent();

            this.Text = title;
            FuncInfoID = funcInfoID;
            FuncCode = funcCode;
            ModuleName = moduleName;
            PowerName = powerName;
        }


        public FrmCheckPassword()
        {
            InitializeComponent();
        }

        public EnumOperationCode operationCode = EnumOperationCode.Unspecified;

        ProxySysUserInfo proxyUser = new ProxySysUserInfo();
        /// <summary>
        /// 确认是否有权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            EntityUserQc userQc = new EntityUserQc();
            string strCerId = "";
            if (CAUserInfo != null)
            {
                if (!string.IsNullOrEmpty(CAUserInfo.strCerId))
                {
                    userQc.UserCerId = CAUserInfo.strCerId;
                    strCerId = CAUserInfo.strCerId;
                }
            }

            userQc.LoginId = txtLoginid.Text.Trim();
            userQc.Password = EncryptClass.Encrypt(txtPassword.Text.Trim());
            if (string.IsNullOrEmpty(userQc.LoginId))
            {
                lis.client.control.MessageDialog.Show("请输入账号！");
                return;
            }

            #region CA密码独立验证

            //如果是ca用户,并且采用:密码独立验证,则使用当前密码
            //系统配置：CA密码验证方式
            if (strCASignMode == "深圳沙井医院" 
                && dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("CAPasswordMode") == "CA密码独立验证"
                && !string.IsNullOrEmpty(strCerId))
            {
                EntityUserQc otherUserQc = new EntityUserQc();
                otherUserQc.UserCaMode = strCASignMode;
                otherUserQc.UserCerId = strCerId;

                List<EntitySysUser> listSelUser = proxyUser.Service.SysUserQuery(otherUserQc);
                if (listSelUser.Count > 0 && listSelUser[0].UserCaFlag == true)
                {
                    userQc.Password = listSelUser[0].UserPassword;
                }

            }

            #endregion

            //使用admin账户时只验证用户名和密码
            if (txtLoginid.Text.Trim().ToLower() != "admin")
            {
                if (FuncInfoID != "")
                {
                    userQc.FuncId = FuncInfoID;
                }

                if (FuncCode != "")
                {
                    userQc.FuncCode = FuncCode;
                }

                if (ModuleName != "")
                {
                    userQc.ModuleName = ModuleName;
                }
            }

            List<EntitySysUser> listUser = proxyUser.Service.SysUserQuery(userQc);
            if (listUser.Count > 0)
            {
                EntitySysUser sysUser = listUser[0];
                
                OperatorID = sysUser.UserLoginid;
                OperatorName = sysUser.UserName;
                OperatorSftId = sysUser.UserIncode;
                Pat_i_code = txtPat_i_code.valueMember;

                #region 旧CA
                ////2013-10-14  添加省妇幼的CA登录 ：&& strCASignMode != "省妇幼"。
                //if ((operationCode == EnumOperationCode.Report || operationCode == EnumOperationCode.UndoReport)
                //    && txtLoginid.Text.Trim().ToLower() != "admin"
                //    && sysUser.UserCaFlag.ToString() == "True"
                //    && UserInfo.GetSysConfigValue("Audit_UKeyType") == "NetCA"
                //    && (strCASignMode != "深圳沙井医院" && strCASignMode != "惠州三院" && strCASignMode != "河池市人民医院"))
                //{
                //    List<EntityUserKey> listPoweruserkey = UserInfo.entityUserInfo.PowerUserKey;
                //    if (listPoweruserkey != null)
                //    {
                //        //NETCA简单模式,只验证当前ukey是否属于本人
                //        //系统配置：NETCA二审时简单验证
                //        if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Audit_Second_NetCaSimple") == "是")
                //        {
                //            string strThumbPrint = string.Empty;
                //            if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("NETCA_ModeType") == "通用"
                //                || dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("NETCA_ModeType") == "深圳新安")
                //            {
                //                strThumbPrint = GDNetCA.GetThumbPrint();
                //            }
                //            else
                //            {
                //                strThumbPrint = GDNetCA.GetUserID();
                //            }
                //            if (string.IsNullOrEmpty(strThumbPrint))
                //            {
                //                lis.client.control.MessageDialog.Show("验证失败，原因：该帐号验证时需要USBKEY！");
                //                return;
                //            }

                //            if (listPoweruserkey.Where(w => w.UserLoginId == txtLoginid.Text).ToList().Count <= 0)
                //            {
                //                lis.client.control.MessageDialog.Show("用户未追加密钥");
                //                return;
                //            }
                //            else if (listPoweruserkey.Where(w => w.Userkey == strThumbPrint && w.UserLoginId == txtLoginid.Text).ToList().Count <= 0)
                //            {
                //                lis.client.control.MessageDialog.Show("验证失败，原因：该帐号对应USBKEY错误！");
                //                return;
                //            }

                //            if (ConfigHelper.GetSysConfigValueWithoutLogin("NETCA_ModeType") == "深圳新安")
                //            {
                //                // <!--netca web service地址-->
                //                string NetCa_WSAddress = System.Configuration.ConfigurationManager.AppSettings["NetCa_WSAddress"];
                //                //医院业务系统使用的CA厂商批复码
                //                string NetCa_hispital_code = System.Configuration.ConfigurationManager.AppSettings["NetCa_hispital_code"];

                //                //上传ca信息给netca
                //                if (!string.IsNullOrEmpty(NetCa_WSAddress) && !string.IsNullOrEmpty(NetCa_hispital_code))
                //                {
                //                    DateTime dtitServer = ServerDateTime.GetServerDateTime();//取中间层的数据库时间
                //                    dcl.client.common.GDNetCA.LabUploadStringToCa("004", "99", dtitServer.ToString("yyyyMMddHHmmss"), NetCa_hispital_code, strThumbPrint, "无", sysUser.UserName, "02", "0");
                //                }
                //            }
                //        }
                //        else
                //        {
                //            if (ConfigHelper.GetSysConfigValueWithoutLogin("NETCA_ModeType") == "广州十二医院")
                //            {
                //                string result = GDNetCA.CheckCert(ConfigurationManager.AppSettings["GDCAUrl"], ConfigurationManager.AppSettings["DefaultServerCert"]);
                //                if (!String.IsNullOrEmpty(result) && !result.Contains("证书有效"))
                //                {
                //                    lis.client.control.MessageDialog.Show(result);
                //                    this.txtPassword.Text = string.Empty;
                //                    this.txtLoginid.Text = string.Empty;
                //                    txtLoginid.Focus();
                //                    return;
                //                }
                //            }
                //            else
                //            {
                //                NetcaSign signer = new NetcaSign();
                //                string strKey = string.Empty;
                //                if (signer.login(txtLoginid.Text, ref strKey))
                //                {
                //                    if (listPoweruserkey.Where(w => w.Userkey == strKey && w.UserLoginId == txtLoginid.Text).ToList().Count == 0)
                //                    {
                //                        lis.client.control.MessageDialog.Show("用户未追加密钥");
                //                        return;
                //                    }
                //                }
                //                else
                //                {
                //                    lis.client.control.MessageDialog.Show("密钥验证错误");
                //                    this.txtPassword.Text = string.Empty;
                //                    this.txtLoginid.Text = string.Empty;
                //                    txtLoginid.Focus();
                //                    return;
                //                }

                //                if (ConfigHelper.GetSysConfigValueWithoutLogin("NETCA_ModeType") == "深圳新安")
                //                {
                //                    // <!--netca web service地址-->
                //                    string NetCa_WSAddress = System.Configuration.ConfigurationManager.AppSettings["NetCa_WSAddress"];
                //                    //医院业务系统使用的CA厂商批复码
                //                    string NetCa_hispital_code = System.Configuration.ConfigurationManager.AppSettings["NetCa_hispital_code"];

                //                    //上传ca信息给netca
                //                    if (!string.IsNullOrEmpty(NetCa_WSAddress) && !string.IsNullOrEmpty(NetCa_hispital_code))
                //                    {
                //                        DateTime dtitServer = ServerDateTime.GetServerDateTime();//取中间层的数据库时间
                //                        dcl.client.common.GDNetCA.LabUploadStringToCa("004", "99", dtitServer.ToString("yyyyMMddHHmmss"), NetCa_hispital_code, strKey, "无", sysUser.UserName, "02", "0");
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}

                //if (operationCode == EnumOperationCode.Report || operationCode == EnumOperationCode.UndoReport)
                //{
                //    //2013-10-14  添加省妇幼的CA登录：||strCASignMode =="省妇幼"
                //    if ((strCASignMode == "深圳沙井医院" 
                //        || strCASignMode == "河池市人民医院" 
                //        || strCASignMode == "广东医学院附属医院") && sysUser.UserCaFlag.ToString() == "True")
                //    {
                //        if (CAUserInfo.strCerId == null)
                //        {
                //            lis.client.control.MessageDialog.Show("没有插入USB电子签名证书");
                //            this.txtPassword.Text = string.Empty;

                //            txtLoginid.Focus();
                //            return;
                //        }
                //        if (!CAUserInfo.UserLoginCherk(this.txtLoginid.Text.Trim(), this.txtPassword.Text.Trim()))
                //        {
                //            lis.client.control.MessageDialog.Show("电子签名账号验证错误或无权限");
                //            this.txtPassword.Text = string.Empty;

                //            txtLoginid.Focus();
                //            return;
                //        }
                //    }
                //    else if ((strCASignMode == "深圳沙井医院" || strCASignMode == "河池市人民医院" ) && operationCode == EnumOperationCode.Report)
                //    {
                //        //系统配置：CA模式时电子验证用户才能二审
                //        if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Audit_Second_OnlyCaUserByCaMode") == "是")
                //        {
                //            lis.client.control.MessageDialog.Show("CA模式非CA用户不能二审");
                //            this.txtPassword.Text = string.Empty;

                //            txtLoginid.Focus();
                //            return;
                //        }
                //    }
                //    else if (strCASignMode == "惠州三院" && sysUser.UserCaFlag.ToString() == "True")
                //    {
                //        GDCAReader gdca = new GDCAReader();
                //        try
                //        {
                //            PassWord = this.txtPassword.Text.Trim();
                //            if (gdca.NoneUsbKey())
                //            {
                //                lis.client.control.MessageDialog.Show("没有插入USB电子签名证书");
                //                this.txtPassword.Text = string.Empty;

                //                txtLoginid.Focus();
                //                return;
                //            }
                //            if (!gdca.GDCALoginKey(this.txtPassword.Text.Trim()))
                //            {
                //                lis.client.control.MessageDialog.ShowAutoCloseDialog("USBKEY密码验证错误");
                //                this.txtPassword.Text = string.Empty;

                //                txtLoginid.Focus();
                //                return;
                //            }
                //            if (sysUser.UserCerid != null && sysUser.UserCerid != gdca.GetCertKey())
                //            {
                //                lis.client.control.MessageDialog.Show("验证失败，原因：该帐号对应USBKEY错误！");
                //                this.txtPassword.Text = string.Empty;

                //                txtLoginid.Focus();
                //                return;
                //            }

                //            string wsdlAddress = ConfigHelper.GetSysConfigValueWithoutLogin("GDCA_AUDIT_URLS");
                //            if (!string.IsNullOrEmpty(wsdlAddress))
                //            {
                //                if (string.IsNullOrEmpty(gdca.GDCARemoteLogin(this.txtPassword.Text.Trim(), wsdlAddress)))
                //                {
                //                    lis.client.control.MessageDialog.Show("用户证书验证失败");
                //                    return;
                //                }
                //            }
                //        }
                //        finally
                //        {
                //            gdca.Release();
                //        }
                //    }
                //}
                #endregion

                if (operationCode == EnumOperationCode.Report || operationCode == EnumOperationCode.UndoReport)
                {
                    if (!CACheck(sysUser))
                    {
                        return;
                    }
                }

                if (UserInfo.GetSysConfigValue("Audit_Second_CancelPrintPower") == "是" && PowerName != string.Empty)
                {
                    if (txtLoginid.Text.Trim().ToLower() == "admin")
                    {
                        Power = true;
                    }
                    else
                    {
                        EntityUserQc otherUserQc = new EntityUserQc();
                        otherUserQc.LoginId = txtLoginid.Text.Trim();
                        otherUserQc.ModuleName = PowerName;
                        List<EntitySysUser> listSelUser = proxyUser.Service.SysUserQuery(otherUserQc);
                        if (listSelUser.Count > 0)
                            Power = true;
                    }
                }

                this.isActionSuccess = true;
                sysToolBar1.LogMessage = "使用账号[" + txtLoginid.Text.Trim() + "]通过验证";
                this.PassWord = this.txtPassword.Text.TrimEnd();
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                lis.client.control.MessageDialog.Show("账号错误或无权限或已停用");
                this.txtPassword.Text = string.Empty;
                Pat_i_code = string.Empty;
                txtLoginid.Focus();
                return;
            }
        }

        public bool Valid(string Loginid,string Password)
        {
            txtPat_i_code.SetFilter(txtPat_i_code.getDataSource().FindAll(i => i.UserType == "检验组"));

            if (UserInfo.GetSysConfigValue("OperationVerifyLockOnCurrentUser") == "是")
                this.txtPat_i_code.SelectByID(UserInfo.loginID);

            EntityUserQc userQc = new EntityUserQc();
            string strCerId = "";
            if (CAUserInfo != null)
            {
                if (!string.IsNullOrEmpty(CAUserInfo.strCerId))
                {
                    userQc.UserCerId = CAUserInfo.strCerId;
                    strCerId = CAUserInfo.strCerId;
                }
            }

            userQc.LoginId = Loginid.Trim();
            userQc.Password = EncryptClass.Encrypt(Password.Trim());
            if (string.IsNullOrEmpty(userQc.LoginId))
            {
                lis.client.control.MessageDialog.Show("请输入账号！");
                return false; 
            }

            #region CA密码独立验证

            //如果是ca用户,并且采用:密码独立验证,则使用当前密码
            //系统配置：CA密码验证方式
            if (strCASignMode == "深圳沙井医院"
                && dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("CAPasswordMode") == "CA密码独立验证"
                && !string.IsNullOrEmpty(strCerId))
            {
                EntityUserQc otherUserQc = new EntityUserQc();
                otherUserQc.UserCaMode = strCASignMode;
                otherUserQc.UserCerId = strCerId;

                List<EntitySysUser> listSelUser = proxyUser.Service.SysUserQuery(otherUserQc);
                if (listSelUser.Count > 0 && listSelUser[0].UserCaFlag == true)
                {
                    userQc.Password = listSelUser[0].UserPassword;
                }

            }

            #endregion

            //使用admin账户时只验证用户名和密码
            if (Loginid.Trim().ToLower() != "admin")
            {
                if (FuncInfoID != "")
                {
                    userQc.FuncId = FuncInfoID;
                }

                if (FuncCode != "")
                {
                    userQc.FuncCode = FuncCode;
                }

                if (ModuleName != "")
                {
                    userQc.ModuleName = ModuleName;
                }
            }

            List<EntitySysUser> listUser = proxyUser.Service.SysUserQuery(userQc);
            if (listUser.Count > 0)
            {
                EntitySysUser sysUser = listUser[0];

                if (operationCode == EnumOperationCode.Report || operationCode == EnumOperationCode.UndoReport)
                {
                    if (!CACheck(sysUser))
                    {
                        return false;
                    }
                }
                //2013-10-14  添加省妇幼的CA登录 ：&& strCASignMode != "省妇幼"。
                if (
                    (operationCode == EnumOperationCode.Report || operationCode == EnumOperationCode.UndoReport) &&
                    Loginid.Trim().ToLower() != "admin" && sysUser.UserCaFlag.ToString() == "True" &&
                    UserInfo.GetSysConfigValue("Audit_UKeyType") == "NetCA" && (strCASignMode != "深圳沙井医院" && strCASignMode != "惠州三院" && strCASignMode != "河池市人民医院"))
                {
                    List<EntityUserKey> listPoweruserkey = UserInfo.entityUserInfo.PowerUserKey;
                    if (listPoweruserkey != null)
                    {
                        //NETCA简单模式,只验证当前ukey是否属于本人
                        //系统配置：NETCA二审时简单验证
                        if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Audit_Second_NetCaSimple") == "是")
                        {
                            string strThumbPrint = string.Empty;
                            if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("NETCA_ModeType") == "通用"
                                || dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("NETCA_ModeType") == "深圳新安")
                            {
                                strThumbPrint = GDNetCA.GetThumbPrint();
                            }
                            else
                            {
                                strThumbPrint = GDNetCA.GetUserID();
                            }
                            if (string.IsNullOrEmpty(strThumbPrint))
                            {
                                lis.client.control.MessageDialog.Show("验证失败，原因：该帐号验证时需要USBKEY！");
                                return false;
                            }

                            if (listPoweruserkey.Where(w => w.UserLoginId == Loginid).ToList().Count <= 0)
                            {
                                lis.client.control.MessageDialog.Show("用户未追加密钥");
                                return false;
                            }
                            else if (listPoweruserkey.Where(w => w.Userkey == strThumbPrint && w.UserLoginId == Loginid).ToList().Count <= 0)
                            {
                                lis.client.control.MessageDialog.Show("验证失败，原因：该帐号对应USBKEY错误！");
                                return false;
                            }

                            if (ConfigHelper.GetSysConfigValueWithoutLogin("NETCA_ModeType") == "深圳新安")
                            {
                                // <!--netca web service地址-->
                                string NetCa_WSAddress = System.Configuration.ConfigurationManager.AppSettings["NetCa_WSAddress"];
                                //医院业务系统使用的CA厂商批复码
                                string NetCa_hispital_code = System.Configuration.ConfigurationManager.AppSettings["NetCa_hispital_code"];

                                //上传ca信息给netca
                                if (!string.IsNullOrEmpty(NetCa_WSAddress) && !string.IsNullOrEmpty(NetCa_hispital_code))
                                {
                                    DateTime dtitServer = ServerDateTime.GetServerDateTime();//取中间层的数据库时间
                                    dcl.client.common.GDNetCA.LabUploadStringToCa("004", "99", dtitServer.ToString("yyyyMMddHHmmss"), NetCa_hispital_code, strThumbPrint, "无", sysUser.UserName, "02", "0");
                                }
                            }
                        }
                        else
                        {
                            if (ConfigHelper.GetSysConfigValueWithoutLogin("NETCA_ModeType") == "广州十二医院")
                            {
                                string result = GDNetCA.CheckCert(ConfigurationManager.AppSettings["GDCAUrl"], ConfigurationManager.AppSettings["DefaultServerCert"]);
                                if (!String.IsNullOrEmpty(result) && !result.Contains("证书有效"))
                                {
                                    lis.client.control.MessageDialog.Show(result);
                                    return false;
                                }
                            }
                            else
                            {
                                NetcaSign signer = new NetcaSign();
                                string strKey = string.Empty;
                                if (signer.login(Loginid, ref strKey))
                                {
                                    if (listPoweruserkey.Where(w => w.Userkey == strKey && w.UserLoginId == Loginid).ToList().Count == 0)
                                    {
                                        lis.client.control.MessageDialog.Show("用户未追加密钥");
                                        return false;
                                    }
                                }
                                else
                                {
                                    lis.client.control.MessageDialog.Show("密钥验证错误");
                                    return false;
                                }

                                if (ConfigHelper.GetSysConfigValueWithoutLogin("NETCA_ModeType") == "深圳新安")
                                {
                                    // <!--netca web service地址-->
                                    string NetCa_WSAddress = System.Configuration.ConfigurationManager.AppSettings["NetCa_WSAddress"];
                                    //医院业务系统使用的CA厂商批复码
                                    string NetCa_hispital_code = System.Configuration.ConfigurationManager.AppSettings["NetCa_hispital_code"];

                                    //上传ca信息给netca
                                    if (!string.IsNullOrEmpty(NetCa_WSAddress) && !string.IsNullOrEmpty(NetCa_hispital_code))
                                    {
                                        DateTime dtitServer = ServerDateTime.GetServerDateTime();//取中间层的数据库时间
                                        dcl.client.common.GDNetCA.LabUploadStringToCa("004", "99", dtitServer.ToString("yyyyMMddHHmmss"), NetCa_hispital_code, strKey, "无", sysUser.UserName, "02", "0");
                                    }
                                }
                            }
                        }
                    }
                }
                OperatorID = sysUser.UserLoginid;
                OperatorName = sysUser.UserName;
                OperatorSftId = sysUser.UserIncode;
                Pat_i_code = txtPat_i_code.valueMember;
                if (operationCode == EnumOperationCode.Report || operationCode == EnumOperationCode.UndoReport)
                {

                    //2013-10-14  添加省妇幼的CA登录：||strCASignMode =="省妇幼"
                    if ((strCASignMode == "深圳沙井医院" || strCASignMode == "河池市人民医院" || strCASignMode == "广东医学院附属医院") && sysUser.UserCaFlag.ToString() == "True")
                    {
                        if (CAUserInfo.strCerId == null)
                        {
                            lis.client.control.MessageDialog.Show("没有插入USB电子签名证书");
                            return false;
                        }
                        if (!CAUserInfo.UserLoginCherk(Loginid.Trim(), Password.Trim()))
                        {
                            lis.client.control.MessageDialog.Show("电子签名账号验证错误或无权限");
                            return false;
                        }
                    }
                    else if ((strCASignMode == "深圳沙井医院" || strCASignMode == "河池市人民医院") && operationCode == EnumOperationCode.Report)
                    {
                        //系统配置：CA模式时电子验证用户才能二审
                        if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Audit_Second_OnlyCaUserByCaMode") == "是")
                        {
                            lis.client.control.MessageDialog.Show("CA模式非CA用户不能二审");
                            return false;
                        }
                    }
                    else if (strCASignMode == "惠州三院" && sysUser.UserCaFlag.ToString() == "True")
                    {
                        GDCAReader gdca = new GDCAReader();
                        try
                        {
                            PassWord = Password.Trim();
                            if (gdca.NoneUsbKey())
                            {
                                lis.client.control.MessageDialog.Show("没有插入USB电子签名证书");
                                return false;
                            }
                            if (!gdca.GDCALoginKey(Password.Trim()))
                            {
                                lis.client.control.MessageDialog.ShowAutoCloseDialog("USBKEY密码验证错误");
                                return false;
                            }
                            if (sysUser.UserCerid != null && sysUser.UserCerid != gdca.GetCertKey())
                            {
                                lis.client.control.MessageDialog.Show("验证失败，原因：该帐号对应USBKEY错误！");
                                return false;
                            }

                            string wsdlAddress = ConfigHelper.GetSysConfigValueWithoutLogin("GDCA_AUDIT_URLS");
                            if (!string.IsNullOrEmpty(wsdlAddress))
                            {
                                if (string.IsNullOrEmpty(gdca.GDCARemoteLogin(Password, wsdlAddress)))
                                {
                                    lis.client.control.MessageDialog.Show("用户证书验证失败");
                                    return false;
                                }
                            }
                        }
                        finally
                        {
                            gdca.Release();
                        }
                    }
                }
                if (UserInfo.GetSysConfigValue("Audit_Second_CancelPrintPower") == "是" && PowerName != string.Empty)
                {
                    if (Loginid.Trim().ToLower() == "admin")
                    {
                        Power = true;
                    }
                    else
                    {
                        EntityUserQc otherUserQc = new EntityUserQc();
                        otherUserQc.LoginId = Loginid.Trim();
                        otherUserQc.ModuleName = PowerName;
                        List<EntitySysUser> listSelUser = proxyUser.Service.SysUserQuery(otherUserQc);

                        if (listSelUser.Count > 0)
                            Power = true;
                    }
                }
                this.isActionSuccess = true;
                this.PassWord = Password;
                return true;
            }
            else
            {
                lis.client.control.MessageDialog.Show("账号错误或无权限或已停用");
                Pat_i_code = string.Empty;
                return false;
            }
        }

        /// <summary>
        /// 数字证书校验（签名前）
        /// </summary>
        /// <param name="sysUser"></param>
        /// <returns></returns>
        private bool CACheck(EntitySysUser sysUser)
        {
            if (strCASignMode == null)
            {
                strCASignMode = ConfigHelper.GetSysConfigValueWithoutLogin("CASignMode");
            }

            if (strCASignMode != "无" && sysUser.UserCaFlag)
            {
                //已经有登录验证，这里只读UKey取绑定值来比较
                caPKI = CaPKIFactory.CreateCASignature(strCASignMode);

                if (caPKI == null)
                {
                    MessageDialog.Show("数字证书验证出错：" + CaPKIFactory.errorInfo);
                    return false;
                }
                caPKI.UserId = sysUser.UserLoginid; 
                bool res = caPKI.LoginWithCA(new EntityLogin());

                if (!res)
                {
                    MessageDialog.Show("数字证书验证出错：" + caPKI.ErrorInfo);
                    return false;
                }
                //if (string.IsNullOrEmpty(oid))
                //{
                //    MessageDialog.Show("数字证书验证出错：" + caPKI.ErrorInfo);
                //    return false;
                //}
                //else if (oid != sysUser.CaEntityId)
                //{
                //    MessageDialog.Show(string.Format("数字证书验证出错：请使用当前账号({0})的数字证书进行该操作", sysUser.LoginId));
                //    return false;
                //}
            }
            return true;
        }


        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmCheckPassword_Load(object sender, EventArgs e)
        {
            labelControl1.Text = "身份帐号";
            labelControl2.Text = "身份密码";
            if (operationCode == EnumOperationCode.Report)
            {
                labelControl1.Text = LocalSetting.Current.Setting.ReportWord + "帐号";
                labelControl2.Text = LocalSetting.Current.Setting.ReportWord + "密码";
            }

            PassWord = string.Empty;
            UserInfo.SkipPower = true;
            sysToolBar1.SetToolButtonStyle(new string[] { "BtnConfirm", "BtnClose" });
            sysToolBar1.BtnConfirm.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right;
            sysToolBar1.BtnClose.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right;
            sysToolBar1.CheckPower = false;
            if (UserInfo.GetSysConfigValue("OperationVerifyLockOnCurrentUser") == "是")
            {
                this.txtPat_i_code.SelectByID(UserInfo.loginID);

                this.txtLoginid.Focus();
                this.ActiveControl = this.txtLoginid;
            }
            else
            {

            }

            //一审、取消一审
            if (operationCode == EnumOperationCode.Audit
                || operationCode == EnumOperationCode.UndoAudit)
            {
                string cfg = UserInfo.GetSysConfigValue("Audit_First_UserVerifyUseCurrentLoginName");

                if (cfg == "处理者为当前用户")
                {
                    this.txtLoginid.Text = UserInfo.loginID;
                    this.txtPassword.Focus();
                    this.ActiveControl = this.txtPassword;
                }
                else if (cfg == "处理者为当前用户并锁定")
                {
                    this.txtLoginid.Text = UserInfo.loginID;
                    this.txtLoginid.Enabled = false;
                    this.txtPassword.Focus();
                    this.ActiveControl = this.txtPassword;
                }
            }
            //二审、取消二审
            else if (operationCode == EnumOperationCode.Report
                || operationCode == EnumOperationCode.UndoReport)
            {
                string cfg = UserInfo.GetSysConfigValue("Audit_Second_UserVerifyUseCurrentLoginName");

                if (cfg == "处理者为当前用户")
                {
                    this.txtLoginid.Text = UserInfo.loginID;
                    this.txtPassword.Focus();
                    this.ActiveControl = this.txtPassword;
                }
                else if (cfg == "处理者为当前用户并锁定")
                {
                    this.txtLoginid.Text = UserInfo.loginID;
                    this.txtLoginid.Enabled = false;
                    this.txtPassword.Focus();
                    this.ActiveControl = this.txtPassword;
                }
            }


            //判断为二审时是否为电子签名验证模式
            if (operationCode == EnumOperationCode.Report || operationCode == EnumOperationCode.UndoReport)
            {
                //判断是否为电子签名验证模式
                //2013-10-14 添加省妇幼的CA登录： || strCASignMode == "省妇幼"
                strCASignMode = dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("CASignMode");
                if (strCASignMode == "深圳沙井医院" || strCASignMode == "河池市人民医院" || strCASignMode == "广东医学院附属医院")
                {
                    //先读取多个key
                    CAUserInfo = new Lis.Client.CASign.FrmUserInfo();
                    //梧州人医有一台电脑查多个key的情况  所以审核时谁登陆读取谁的key
                    CAUserInfo = new Lis.Client.CASign.FrmUserInfo(UserInfo.userName);
                    if (!string.IsNullOrEmpty(CAUserInfo.strUserName))
                    {
                        //如果使用UKEY则不能编辑用户名
                        this.txtLoginid.Text = CAUserInfo.strUserName;
                        this.txtLoginid.Properties.ReadOnly = true;
                        //this.ActiveControl = this.txtPassword;
                    }
                }
            }

            //报告(二审)签名时，签名弹窗是否显示一审人
            if (ConfigHelper.GetSysConfigValueWithoutLogin("report_checkform_showlPat_i_code") == "是")
            {
                lblPat_i_code.Visible = true;
                txtPat_i_code.Visible = true;
            }
            else
            {
                lblPat_i_code.Visible = false;
                txtPat_i_code.Visible = false;
            }

            //报告(二审)时允许修改一审人(检验者)
            if (ConfigHelper.GetSysConfigValueWithoutLogin("report_Allowedit_auditercode") == "是")
            {
                txtPat_i_code.Readonly = false;
            }
            else
            {
                txtPat_i_code.Readonly = true;
            }


            ////允许审核报告时修改录入者
            //if ((operationCode == EnumOperationCode.Report || operationCode == EnumOperationCode.Audit)
            //    && ConfigHelper.GetSysConfigValueWithoutLogin("Audit_AlloweditPat_i_code") == "是")
            //{
            //    txtPat_i_code.Readonly = false;
            //}
            //else if ((operationCode == EnumOperationCode.Report || operationCode == EnumOperationCode.Audit)
            //    && ConfigHelper.GetSysConfigValueWithoutLogin("Audit_AlloweditPat_i_code") == "否")
            //{
            //    txtPat_i_code.Readonly = true;
            //}


            UserInfo.SkipPower = false;
            txtPat_i_code.SetFilter(txtPat_i_code.getDataSource().FindAll(i => i.UserType == "检验组"));
        }

        private void Enter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                btnOk_Click(null, null);
            }
        }
    }
}
