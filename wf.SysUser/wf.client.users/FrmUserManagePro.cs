using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using dcl.common;
using System.IO;
using System.Drawing.Imaging;
using dcl.client.wcf;
using Lib.LogManager;
using dcl.entity;
using System.Linq;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;
using dcl.client.cache;

namespace dcl.client.users
{
    public partial class FrmUserManagePro : FrmCommon
    {
        /// <summary>
        /// 带教老师_用户id
        /// </summary>
        private List<string> userid_teacher = new List<string>();
        /// <summary>
        /// 实习生_用户id
        /// </summary>
        private List<string> userid_medic = new List<string>();

        /// <summary>
        /// 是否带教老师角色
        /// </summary>
        private bool IsTeacherRole = false;
        /// <summary>
        /// CA签名模式
        /// </summary>
        private string CASignMode;
        public FrmUserManagePro()
        {
            InitializeComponent();
            this.tvUser.IndicatorWidth = 30;
            this.tvUser.CustomDrawNodeIndicator += new DevExpress.XtraTreeList.CustomDrawNodeIndicatorEventHandler(tvUser_CustomDrawNodeIndicator);
        }

        void tvUser_CustomDrawNodeIndicator(object sender, DevExpress.XtraTreeList.CustomDrawNodeIndicatorEventArgs e)
        {
            DevExpress.XtraTreeList.TreeList tmpTree = sender as DevExpress.XtraTreeList.TreeList;
            DevExpress.Utils.Drawing.IndicatorObjectInfoArgs args = e.ObjectArgs as DevExpress.Utils.Drawing.IndicatorObjectInfoArgs;
            if (args != null)
            {
                int rowNum = tmpTree.GetVisibleIndexByNode(e.Node) + 1;
                args.Appearance.Font = new Font("宋体", 7f);
                args.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                args.DisplayText = rowNum.ToString();


            }
        }

       
        /// <summary>
        /// 判断点击保存角色按钮时是Insert还是Update的标志位
        /// </summary>
        public enum OptionStatus
        {
            Insert,
            Update
        }
        OptionStatus optionStatus = OptionStatus.Update;

        //查询结果_缓存角色列表和权限树结构
        EntityPowerList dsUser = new EntityPowerList();

        //用户权限信息
        List<EntitySysRole> dtRole = new List<EntitySysRole>();
        //科室信息
        List<EntityDicPubDept> dtDepart = new List<EntityDicPubDept>();
        //物理组信息
        List<EntityDicPubOrganize> dtType = new List<EntityDicPubOrganize>();
        //用户信息
        List<EntitySysUser> dtUser = new List<EntitySysUser>();
        //用户与物理组关系
        List<EntityUserLab> dtPowerUserType = new List<EntityUserLab>();
        //用户与仪器关系
        List<EntityUserInstrmt> dtPowerUserInstrmt = new List<EntityUserInstrmt>();
        //用户与医院的关系
        List<EntityUserHospital> dtPowerUserHospital = new List<EntityUserHospital>();
        //质控物理组
        List<EntityUserLabQuality> dtPowerUserTypeQuality = new List<EntityUserLabQuality>();
        //质控仪器
        List<EntityUserItrQuality> dtPowerUserInstrmtQuality = new List<EntityUserItrQuality>();
        //医院质控
        List<EntityUserHosQuality> dtPowerUserHospitalQuality = new List<EntityUserHosQuality>();
        //用户与权限关系
        List<EntityUserRole> dtUserRole = new List<EntityUserRole>();
        //用户与科室关系
        List<EntityUserDept> dtUserDepart = new List<EntityUserDept>();
        //缓存指定用户的物理组和角色数据
        EntityPowerList dsRoleFuncUser = new EntityPowerList();
        //判断是否是第一次加载
        int isFirstLoad = 0;

        bool isPower = false;

        /// <summary>
        /// 窗体初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmUserManagePro_Load(object sender, EventArgs e)
        {
            //需要显示的按钮和顺序
            sysToolBar1.SetToolButtonStyle(new string[] { "BtnAdd", "BtnModify", "BtnDelete", "BtnSave", "BtnCancel", "BtnRefresh", sysToolBar1.BtnDesign.Name,sysToolBar1.BtnReset.Name,sysToolBar1.BtnQualityAudit.Name });

            sysToolBar1.BtnDesign.Caption = "追加密钥";
            sysToolBar1.BtnReset.Caption = "停止使用密钥";
            sysToolBar1.BtnQualityAudit.Caption = "密钥使用情况";

            this.sysToolBar1.OnBtnAddClicked += new System.EventHandler(this.btnAdd_Click);
            this.sysToolBar1.OnBtnModifyClicked += new System.EventHandler(this.btnModify_Click);
            this.sysToolBar1.OnBtnDeleteClicked += new System.EventHandler(this.btnDel_Click);
            this.sysToolBar1.OnBtnSaveClicked += new System.EventHandler(this.btnSave_Click);
            this.sysToolBar1.OnBtnCancelClicked += new System.EventHandler(this.btnCancel_Click);
            this.sysToolBar1.OnBtnRefreshClicked += new System.EventHandler(this.btnRefresh_Click);
            this.sysToolBar1.OnBtnQualityAuditClicked += new System.EventHandler(this.sysToolBar_OnBtnQualityAuditClicked);
            this.sysToolBar1.BtnResetClick += new System.EventHandler(this.sysToolBar_BtnResetClick);
            this.sysToolBar1.BtnDesignClick += new System.EventHandler(this.sysToolBar_BtnDesignClick);

            txtFindKey.TextChanged += TxtFindKey_TextChanged;

            //默认检验组
            txtUserType.SelectedIndex = 0;

            //增加配置输入码是否为必入项
            if (UserInfo.GetSysConfigValue("Power_NotNull_InCode") != "是")
            {
                layInCode.Text = "输入码";
                layInCode.AppearanceItemCaption.ForeColor = Color.Black;
            }

            BindCheckCombox();

            CASignMode = UserInfo.GetSysConfigValue("CASignMode");

            LoadData();
        }

        private void TxtFindKey_TextChanged(object sender, EventArgs e)
        {
            string filter = txtFindKey.Text.Trim();
            if (filter == string.Empty)
                this.bsUser.DataSource = dtUser;
            else
            {
                List<EntitySysUser> userList = dtUser.Where(w => (w.UserLoginid != null && w.UserLoginid.Contains(filter)) ||
                                                        (w.UserName != null && w.UserName.Contains(filter)) ||
                                                        (w.PyCode != null && w.PyCode.Contains(filter.ToUpper())) ||
                                                        (w.WbCode != null && w.WbCode.Contains(filter.ToUpper())) ||
                                                        (w.SortNo != null && w.SortNo.ToString().Contains(filter)) ||
                                                        w.UserType.Contains(filter)).ToList();
                this.bsUser.DataSource = userList;
            }
        }

        /// <summary>
        /// 绑定勾选下拉框
        /// 
        /// </summary>
        private void BindCheckCombox()
        {
            
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void LoadData()
        {
            ProxySysUserInfo proxyUser = new ProxySysUserInfo();
            userid_teacher = proxyUser.Service.GetUserIDForRoleName("带教老师");
            userid_medic = proxyUser.Service.GetUserIDForRoleName("实习生");
            //判断当前用户是否为带教老师角色
            if (userid_teacher != null && userid_teacher.Contains(UserInfo.userInfoId)
                && !UserInfo.isAdmin)
            {
                IsTeacherRole = true;
            }
            else
            {
                IsTeacherRole = false;
            }

            optionStatus = OptionStatus.Update;
            EnterEditingState(false);

            isPower = UserInfo.HaveFunction(274);

            ProxyUserManage proxy = new ProxyUserManage();
             dsUser = proxy.Service.GetViewData(isPower);

            //角色列表
            dtRole = dsUser?.SysRole;

            //科室列表
            dtDepart = dsUser?.DicDept;

            //物理组列表
            dtType = dsUser?.DicType;

            //用户列表
            dtUser = dsUser?.SysUser;

            ///绑定科室多选下拉框
            BindDept();
            ///绑定角色多选下拉框
            BindRole();
            
            this.bsRole.DataSource = dtRole;

            this.bsDicDept.DataSource = dtDepart;

            this.bsType.DataSource = dtType;
            
            try
            {
                #region 带教老师与实习生

                //如果是带教老师角色,则过滤用户列表,只能查看实习生角色
                if (IsTeacherRole)
                {
                    List<EntitySysUser> dtTemp_PowerUserInfo = new List<EntitySysUser>();

                    dtTemp_PowerUserInfo = dsUser.SysUser;

                    if (dtTemp_PowerUserInfo != null)
                    {
                        for (int k = dtTemp_PowerUserInfo.Count - 1; k >= 0; k--)
                        {
                            string strTemp_userName = dtTemp_PowerUserInfo[k].UserName.ToString();
                            string strTeacherName = "/" + UserInfo.userName;

                            //筛选实习生角色用户
                            if (userid_medic != null
                                && userid_medic.Contains(dtTemp_PowerUserInfo[k].UserId.ToString())
                                && (!string.IsNullOrEmpty(strTemp_userName)
                                && (strTemp_userName.Contains(strTeacherName) || (!strTemp_userName.Contains("/"))
                                || strTemp_userName.EndsWith("/"))))
                            {

                            }
                            else
                            {
                                //非实习生角色用户,移除
                                dtTemp_PowerUserInfo.RemoveAt(k);
                            }
                        }

                        //dtTemp_PowerUserInfo.AcceptChanges();
                    }
                }

                #endregion

                //用户列表_最后加载,以保证权限树和角色列表已生成
                // tvUser.DataSource = dsUser.Tables["PowerUserInfo"];
                this.bsUser.DataSource = dtUser;

            }
            catch (Exception ex)
            {
                Logger.LogException("LoadData(用户列表_最后加载,以保证权限树和角色列表已生成)", ex);
                throw;
            }
        }

        /// <summary>
        /// 控制所有输入框状态
        /// </summary>
        /// <param name="enable"></param>
        private void EnterEditingState(Boolean enable)
        {
            txtUserName.Properties.ReadOnly = !enable;
            txtLoginId.Properties.ReadOnly = !enable;
            txtPassword.Properties.ReadOnly = !enable;
            txtPy.Properties.ReadOnly = !enable;
            txtWb.Properties.ReadOnly = !enable;
            txtIncode.Properties.ReadOnly = !enable;
            ckeDel.Properties.ReadOnly = !enable;
            cmbDefault_Type.Readonly = !enable;
            cmbITR_Id.Readonly = !enable;
            txtSeq.Properties.ReadOnly = !enable;
            buttonImage.Properties.ReadOnly = !enable;
            cmbInd.Properties.ReadOnly = !enable;
            chkDept.Properties.ReadOnly = !enable;
            chkRole.Properties.ReadOnly = !enable;

            //CA验证所需信息：身份证+是否启用
            txtCerID.Properties.ReadOnly = !enable;
            ckeCASign.Properties.ReadOnly = !enable;
            cmbHospital.Readonly = !enable;
            if (!isPower)
            {
                txtUserType.Properties.ReadOnly = true;
                txtUserType.EditValue = "护工组";
            }
            else
                txtUserType.Properties.ReadOnly = !enable;
            
        }

        /// <summary>
        /// 物理组树型只读控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvType_BeforeCheckNode(object sender, DevExpress.XtraTreeList.CheckNodeEventArgs e)
        {
            if (txtLoginId.Properties.ReadOnly == false && layoutDefaultType.Visibility ==  DevExpress.XtraLayout.Utils.LayoutVisibility.Always)
            {
                e.CanCheck = true;
            }
            else
            {
                e.CanCheck = false;
            }
        }

        /// <summary>
        /// 质控设备树形只读控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvQualityType_BeforeCheckNode(object sender, DevExpress.XtraTreeList.CheckNodeEventArgs e)
        {
            if (txtLoginId.Properties.ReadOnly == false && layoutDefaultType.Visibility == DevExpress.XtraLayout.Utils.LayoutVisibility.Always)
            {
                e.CanCheck = true;
            }
            else
            {
                e.CanCheck = false;
            }
        }

        /// <summary>
        /// 新增按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.bsUser.AddNew();
            optionStatus = OptionStatus.Insert;
            EnterEditingState(true);
            
            ///重置多选下拉框的勾选项目
            SetCheckedListComboboxFalse(chkDept);
            SetCheckedListComboboxFalse(chkRole);

            for (int i = 0; i < tvType.AllNodesCount; i++)
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvType.FindNodeByID(i);
                tn.Checked = false;
            }
            for (int i = 0; i < tvQualityType.AllNodesCount; i++)
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvQualityType.FindNodeByID(i);
                tn.Checked = false;
            }

            txtPassword.Text = "";
            txtUserName.Focus();
        }

        /// <summary>
        /// 修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            if (tvUser.Selection.Count < 1)
            {
                lis.client.control.MessageDialog.Show(PowerMessage.BASE_SELECT_NULL, PowerMessage.BASE_TITLE);
                return;
            }

            optionStatus = OptionStatus.Update;
            EnterEditingState(true);
            tvUser.Enabled = false;
            txtUserName.Focus();
        }

        public byte[] ConvertToBtye(Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            byte[] imageB = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(imageB, 0, Convert.ToInt32(ms.Length));
            ms.Close();
            return imageB;
        }

        public Image m_mthConvertToImage(byte[] p_btyeImage)
        {

            MemoryStream ms = new MemoryStream(p_btyeImage);
            Image image = Image.FromStream(ms, true);

            return image;
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.isActionSuccess = false;

            if (txtUserName.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show(PowerMessage.BASE_USERNAME_NULL, PowerMessage.BASE_TITLE);
                txtUserName.Focus();
                return;
            }

            if (txtLoginId.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show(PowerMessage.BASE_LOGINID_NULL, PowerMessage.BASE_TITLE);
                txtLoginId.Focus();
                return;
            }

            if (txtPassword.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show(PowerMessage.BASE_PASSWORD_NULL, PowerMessage.BASE_TITLE);
                txtPassword.Focus();
                return;
            }
            if (txtIncode.Text.Trim() == "" && UserInfo.GetSysConfigValue("Power_NotNull_InCode") == "是")
            {
                lis.client.control.MessageDialog.Show(PowerMessage.BASE_INCODE_NULL, PowerMessage.BASE_TITLE);
                txtIncode.Focus();
                return;
            }
            //不允许重复的登录账户_姓名可以重复
            for (int i = 0; i < tvUser.AllNodesCount; i++)
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvUser.FindNodeByID(i);
                string loginId = tn.GetValue(colLoginId).ToString();
                var userid = tn.GetValue(colUserInfoId);
                if ((optionStatus == OptionStatus.Insert && loginId == txtLoginId.Text.Trim() && tn.GetValue(colUserInfoId) != null) || (optionStatus == OptionStatus.Update && tn != tvUser.Selection[0] && loginId == txtLoginId.Text.Trim()))
                {
                    lis.client.control.MessageDialog.Show(PowerMessage.BASE_LOGINID_SAME, PowerMessage.BASE_TITLE);
                    txtLoginId.Focus();
                    return;
                }
            }

            //用户信息
            List<EntitySysUser> dtPowerUserInfo = new List<EntitySysUser>();

            EntitySysUser drPowerUserInfo = new EntitySysUser();
            drPowerUserInfo.UserName = txtUserName.Text.Trim();
            drPowerUserInfo.UserLoginid = txtLoginId.Text.Trim();
            drPowerUserInfo.UserPassword = EncryptClass.Encrypt(txtPassword.Text.Trim());
            drPowerUserInfo.PyCode = txtPy.Text.Trim();
            drPowerUserInfo.WbCode = txtWb.Text.Trim();
            drPowerUserInfo.UserIncode = txtIncode.Text.Trim();
            drPowerUserInfo.UserDefaultLabId = cmbDefault_Type.valueMember;
            drPowerUserInfo.ItrId = cmbITR_Id.valueMember;
            drPowerUserInfo.DelFlag = "0";
            drPowerUserInfo.UserDepartId = "";
            drPowerUserInfo.UserType = txtUserType.Text;
            drPowerUserInfo.SortNo = txtSeq.Text.Trim();
            drPowerUserInfo.Identity = cmbInd.Text;

            drPowerUserInfo.UserOrgId = cmbHospital.valueMember;

            //身份证号
            drPowerUserInfo.UserCerid = txtCerID.Text.Trim();
            //是否使用电子验证
            if (ckeCASign.Checked)
            {
                drPowerUserInfo.UserCaFlag = true;
            }
            else
            {
                drPowerUserInfo.UserCaFlag = false;
            }
            if (!string.IsNullOrEmpty(buttonImage.Text) && File.Exists(buttonImage.Text))  //如果有签名图片
            {
                drPowerUserInfo.UserSigninamge = ConvertToBtye(Image.FromFile(buttonImage.Text));

            }
            else if (!string.IsNullOrEmpty(buttonImage.Text) && buttonImage.Text == "提取CA图片"
                && pictureBox1.Image != null)  //如果为ca导入方式
            {
                drPowerUserInfo.UserSigninamge = ConvertToBtye(pictureBox1.Image);
            }
            if (ckeDel.Checked)
            {
                drPowerUserInfo.DelFlag = "1";
            }

            if (optionStatus == OptionStatus.Update)
            {
                drPowerUserInfo.UserId = tvUser.Selection[0].GetValue(colUserInfoId).ToString();
            }

            dtPowerUserInfo.Add(drPowerUserInfo);
            EntityPowerList dsResult = new EntityPowerList();
            dsResult.SysUser = dtPowerUserInfo;

            //物理组和角色
            string userInfoId = "";
            if (optionStatus == OptionStatus.Update)
            {
                //取得用户编号
                userInfoId = tvUser.Selection[0].GetValue(colUserInfoId).ToString();
            }

            //遍历树获取物理组和仪器
            if (dsRoleFuncUser == null)
            {
                GetUserTypeRole();
            }
            dtPowerUserType = new List<EntityUserLab>();
            dtPowerUserInstrmt = new List<EntityUserInstrmt>();
            dtPowerUserHospital = new List<EntityUserHospital>();

            for (int i = 0; i < tvType.AllNodesCount; i++)
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvType.FindNodeByID(i);
                string sortId = tn.GetValue(colSortId).ToString();

                if (tn.Checked == true && sortId == "0")
                {
                    //医院
                    EntityUserHospital dr = new EntityUserHospital();
                    if (userInfoId != "")
                    {
                        dr.UserId = userInfoId;
                    }
                    dr.OrgId = tn.GetValue(colTypeId).ToString();
                    dtPowerUserHospital.Add(dr);
                }
                if (tn.Checked == true && sortId == "1")
                {
                    //物理组
                    EntityUserLab dr = new EntityUserLab();
                    if (userInfoId != "")
                    {
                        dr.UserId = userInfoId;
                    }
                    dr.LabId = tn.GetValue(colItr_Id).ToString();
                    dtPowerUserType.Add(dr);
                }

                if (tn.Checked == true && sortId == "2")
                {
                    //仪器
                    EntityUserInstrmt dr = new EntityUserInstrmt();
                    if (userInfoId != "")
                    {
                        dr.UserId = userInfoId;
                    }
                    dr.ItrId = tn.GetValue(colTypeItr_id).ToString();
                    dtPowerUserInstrmt.Add(dr);
                }

            }
            dsResult.UserLab = dtPowerUserType;
            dsResult.UserItr = dtPowerUserInstrmt;
            dsResult.UserHospital = dtPowerUserHospital;

            if (isFirstLoad != 0)
            {
                dtPowerUserTypeQuality = new List<EntityUserLabQuality>();
                dtPowerUserInstrmtQuality = new List<EntityUserItrQuality>();
                dtPowerUserHospitalQuality = new List<EntityUserHosQuality>();
                //遍历树获取物理组和仪器_质控专用
                for (int i = 0; i < tvQualityType.AllNodesCount; i++)
                {
                    DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvQualityType.FindNodeByID(i);
                    string sortId = tn.GetValue(colQualitySortId).ToString();

                    if (tn.Checked == true && sortId == "0")
                    {
                        //医院
                        EntityUserHosQuality dr = new EntityUserHosQuality();
                        if (userInfoId != "")
                        {
                            dr.UserId = userInfoId;
                        }
                        dr.OrgId = tn.GetValue(colQualityTypeId).ToString();
                        dtPowerUserHospitalQuality.Add(dr);
                    }

                    if (tn.Checked == true && sortId == "1")
                    {
                        //物理组
                        EntityUserLabQuality dr = new EntityUserLabQuality();
                        if (userInfoId != "")
                        {
                            dr.UserId = userInfoId;
                        }
                        dr.LabId = tn.GetValue(colQualityItrId).ToString();
                        dtPowerUserTypeQuality.Add(dr);
                    }

                    if (tn.Checked == true && sortId == "2")
                    {
                        //仪器
                        EntityUserItrQuality dr = new EntityUserItrQuality();
                        if (userInfoId != "")
                        {
                            dr.UserId = userInfoId;
                        }
                        dr.ItrId = tn.GetValue(colTypeItr_id).ToString();
                        dtPowerUserInstrmtQuality.Add(dr);
                    }
                }
            }
            dsResult.UserLabQuality = dtPowerUserTypeQuality;
            dsResult.UserItrQuality = dtPowerUserInstrmtQuality;
            dsResult.UserHosQuality = dtPowerUserHospitalQuality;
            //获取所选角色
            List<EntityUserRole> dtPowerUserRole = GetChooseRole(userInfoId);
            dsResult.UserRole = dtPowerUserRole;

            //获取所选科室
            List<EntityUserDept> dtPowerUserDepart = GetChooseDept(userInfoId);

            dsResult.UserDept = dtPowerUserDepart;

            ProxyUserManage proxy = new ProxyUserManage();
            if (optionStatus == OptionStatus.Insert)
            {
                //Insert
               this.isActionSuccess = proxy.Service.AddUserInfo(dsResult);
            }
            else
            {
                //Update
               this.isActionSuccess = proxy.Service.UpdateUserInfo(dsResult);
            }

            if (this.isActionSuccess)
            {
                sysToolBar1.LogMessage = string.Format("保存成功,账号: {0}", txtLoginId.Text.Trim());

                if (optionStatus == OptionStatus.Insert)
                {
                    //重新载入数据
                    LoadData();
                }
                else
                {
                    //修改列表值_避免刷新
                    DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvUser.Selection[0];
                    tn.SetValue(colUserName, txtUserName.Text.Trim());
                    tn.SetValue(colLoginId, txtLoginId.Text.Trim());
                    tn.SetValue(colPy, txtPy.Text.Trim());
                    tn.SetValue(colWb, txtWb.Text.Trim());
                    tn.SetValue(colIncode, txtIncode.Text.Trim());
                    tn.SetValue(colPassword, EncryptClass.Encrypt(txtPassword.Text.Trim()));
                    tn.SetValue(colUserType, txtUserType.Text.Trim());
                    tn.SetValue(colUserSeq, txtSeq.Text.Trim());
                    tn.SetValue(colUserCASignMode, ckeCASign.Checked ? "True" : "False");
                    tn.SetValue(colUser_hos_id, cmbHospital.valueMember);
                    
                    tn.SetValue(colIdentity, cmbInd.Text);

                    if (cmbInd.Text != null)
                    {
                        tn.SetValue(colIdentity, cmbInd.Text);
                    }
                    else
                    {
                        tn.SetValue(colIdentity, "");
                    }

                  

                    if (cmbDefault_Type.valueMember != null)
                    {
                        tn.SetValue(colDefault_Type, cmbDefault_Type.valueMember);
                    }
                    else
                    {
                        tn.SetValue(colDefault_Type, "");
                    }

                    if (cmbITR_Id.valueMember != null)
                    {
                        tn.SetValue(colItr_Id, cmbITR_Id.valueMember);
                    }
                    else
                    {
                        tn.SetValue(colItr_Id, "");
                    }

                    if (ckeDel.Checked)
                    {
                        tn.SetValue(colDel, "1");
                    }
                    else
                    {
                        tn.SetValue(colDel, "0");
                    }
                    if (!string.IsNullOrEmpty(buttonImage.Text) && File.Exists(buttonImage.Text))  //如果有签名图片
                    {
                        tn.SetValue(colUserSign, this.ConvertToBtye(Image.FromFile(this.buttonImage.Text.Trim())));
                    }

                    optionStatus = OptionStatus.Update;
                    EnterEditingState(false);
                    tvUser.Enabled = true;
                }
            }

        }

        /// <summary>
        /// 放弃修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            LoadData();
            //tvUser_FocusedNodeChanged(null, null);
            tvUser.Enabled = true;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (tvUser.Selection.Count < 1)
            {
                lis.client.control.MessageDialog.Show(PowerMessage.BASE_SELECT_NULL, PowerMessage.BASE_TITLE);
                return;
            }
            List<EntitySysUser> dtResult = dtUser;

            //删除只需要业务主键的列数据
            EntitySysUser dr = new EntitySysUser();
            dr.UserId = tvUser.Selection[0].GetValue(colUserInfoId).ToString();
            string loginId = tvUser.Selection[0].GetValue(colLoginId).ToString();

            dtResult.Add(dr);

            DialogResult dresult = MessageBox.Show(PowerMessage.BASE_DELETE_CONFIRM, PowerMessage.BASE_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            switch (dresult)
            {
                case DialogResult.OK:
                    ProxyUserManage proxy = new ProxyUserManage();
                    this.isActionSuccess = proxy.Service.DeleteUserInfo(dr);
                    break;
                case DialogResult.Cancel:
                    return;
            }

            if (this.isActionSuccess)
            {
                sysToolBar1.LogMessage = string.Format("删除成功,账号: {0}", loginId);
            }

            //删除记录后重新加载界面
            LoadData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// 选择用户时显示详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvUser_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            sysToolBar1.EnableButton(false);

            if (tvUser.Selection.Count > 0)
            {
                optionStatus = OptionStatus.Update;
                EnterEditingState(false);

                //显示内容
                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvUser.Selection[0];
                if (tn.GetValue(colUserName) != null)
                {
                    txtUserName.Text = tn.GetValue(colUserName).ToString();
                }
                if (tn.GetValue(colLoginId) != null)
                {
                    txtLoginId.Text = tn.GetValue(colLoginId).ToString();
                }
                if (tn.GetValue(colPy) != null)
                {
                    txtPy.Text = tn.GetValue(colPy).ToString();
                }
                if (tn.GetValue(colWb) != null)
                {
                    txtWb.Text = tn.GetValue(colWb).ToString();
                }
                if (tn.GetValue(colIncode) != null)
                {
                    txtIncode.Text = tn.GetValue(colIncode).ToString();
                }
                if (tn.GetValue(colPassword) != null)
                {
                    txtPassword.Text = EncryptClass.Decrypt(tn.GetValue(colPassword).ToString());
                }
                if (tn.GetValue(colUserType) != null)
                {
                    txtUserType.Text = tn.GetValue(colUserType).ToString();
                }
                if (tn.GetValue(colUserSeq) != null)
                {
                    txtSeq.Text = tn.GetValue(colUserSeq).ToString();
                }
                if (tn.GetValue(colUser_hos_id) != null)
                {
                    cmbHospital.SelectByID(tn.GetValue(colUser_hos_id).ToString());
                }
                

                if (tn.GetValue(colIdentity) != null)
                {
                    cmbInd.Text = tn.GetValue(colIdentity).ToString();
                }
                buttonImage.Text = "";
                //CA认证新增
                if (tn.GetValue(colUserCerId) != null)
                {
                    txtCerID.Text = tn.GetValue(colUserCerId).ToString();
                }
                if (tn.GetValue(colUserCASignMode).ToString() == "True")
                {
                    ckeCASign.Checked = true;
                }
                else
                {
                    ckeCASign.Checked = false;
                }

                try
                {
                    pictureBox1.Image = m_mthConvertToImage((byte[])tn.GetValue(colUserSign));//预览图片
                }
                catch
                {


                    pictureBox1.Image = null;
                }


                string default_type = string.Empty;
                if (tn.GetValue(colDefault_Type) != null)
                {
                   default_type = tn.GetValue(colDefault_Type).ToString();
                }
                List<EntityDicPubProfession> drs = cmbDefault_Type.dtSource.Where(i => i.ProId == default_type).ToList(); ;

                if (drs.Count > 0)
                {
                    cmbDefault_Type.valueMember = default_type;
                    cmbDefault_Type.displayMember = drs[0].ProName.ToString();
                }
                else
                {
                    cmbDefault_Type.valueMember = "";
                    cmbDefault_Type.displayMember = "";
                }
                string itrId = string.Empty;
                if (tn.GetValue(colItr_Id) != null)
                {
                    itrId = tn.GetValue(colItr_Id).ToString();
                }
                List<EntityDicInstrument> drsItr = cmbITR_Id.dtSource.Where(i => i.ItrId == itrId).ToList();
                if (drsItr.Count > 0)
                {
                    cmbITR_Id.valueMember = itrId;
                    cmbITR_Id.displayMember = drsItr[0].ItrName.ToString();
                }
                else
                {
                    cmbITR_Id.valueMember = "";
                    cmbITR_Id.displayMember = "";
                }
                if (tn.GetValue(colDel) != null)
                {
                    if (tn.GetValue(colDel).ToString() == "1")
                    {
                        ckeDel.Checked = true;
                    }
                    else
                    {
                        ckeDel.Checked = false;
                    }
                }
                else
                {
                    ckeDel.Checked = false;
                }
                //查询指定角色的功能点和用户    
                GetUserTypeRole();

                //显示用户物理组
                ShowUserType();

                //显示用户物理组_质控
                ShowUserTypeQuality();

                //显示用户角色
                ShowUserRole();

                //显示用户科室
                ShowUserDepart();
            }
        }

        

        //分组角色判断权限(角色id,角色id) 当前登录用户与要编辑的用户包含系统配置里其中一个相同的角色就有权限进行操作
        void SetCanEditStatus(List<string> userRoleList)
        {
            string settingRoleStr = UserInfo.GetSysConfigValue("Power_CheckPowerGroupByRole");
            if (!string.IsNullOrEmpty(settingRoleStr))
            {

                string[] list = settingRoleStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                List<string> settingRolelist = new List<string>(list);


                List<string> curLoginUserRolelist = new List<string>();


                if (UserInfo.entityUserInfo != null && UserInfo.entityUserInfo.PowerUserRole.Count>0)
                {
                    bool canEdit = false;

                    List<EntityUserRole> userRole = UserInfo.entityUserInfo.PowerUserRole;
                    foreach (EntityUserRole item in userRole)
                    {
                        string roleID = item.RoleId;
                        if (userRoleList.Contains(roleID) && settingRolelist.Contains(roleID))
                        {
                            canEdit = true;
                            break;
                        }
                    }

                    sysToolBar1.BtnAdd.Visibility = canEdit ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
                    sysToolBar1.BtnModify.Visibility = canEdit ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
                    sysToolBar1.BtnDelete.Visibility = canEdit ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
                    sysToolBar1.BtnSave.Visibility = canEdit ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
                    sysToolBar1.BtnCancel.Visibility = canEdit ? DevExpress.XtraBars.BarItemVisibility.Always : DevExpress.XtraBars.BarItemVisibility.Never;
                }


            }
        }

        

        /// <summary>
        /// 显示用户物理组
        /// </summary>
        private void ShowUserType()
        {

            //遍历树显示角色权限
            for (int i = 0; i < tvType.AllNodesCount; i++)
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvType.FindNodeByID(i);
                string sortId = tn.GetValue(colSortId).ToString();

                if (sortId == "1")
                {
                    //物理组
                    string typeId = tn.GetValue(colItr_Id).ToString();
                    if (dtPowerUserType.Where(n => n.LabId == typeId).ToList().Count > 0)
                    {
                        tn.Checked = true;
                    }
                    else
                    {
                        tn.Checked = false;
                    }
                }
                else if (sortId == "2")
                {
                    //仪器
                    string itrId = tn.GetValue(colItr_Id).ToString();
                    if (dtPowerUserInstrmt != null && itrId != "")
                    {
                        if (dtPowerUserInstrmt.Where(r => r.ItrId == itrId).ToList().Count > 0)
                        {
                            tn.Checked = true;
                        }
                        else
                        {
                            tn.Checked = false;
                        }
                    }
                }
                else
                {
                    //医院
                    string hosId = tn.GetValue(colTypeId).ToString();
                    if (dtPowerUserHospital != null && hosId != "")
                    {
                        if (dtPowerUserHospital.Where(r => r.OrgId == hosId).ToList().Count > 0)
                        {
                            tn.Checked = true;
                        }
                        else
                        {
                            tn.Checked = false;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 显示用户物理组_质控
        /// </summary>
        private void ShowUserTypeQuality()
        {

            //遍历树显示角色权限
            for (int i = 0; i < tvQualityType.AllNodesCount; i++)
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvQualityType.FindNodeByID(i);
                string sortId = tn.GetValue(colQualitySortId).ToString();
                int iPase = 0;
                if (sortId == "1")
                {
                    //物理组
                    string typeId = tn.GetValue(colQualityItrId).ToString();
                    if (int.TryParse(typeId, out iPase) && dtPowerUserTypeQuality.Where(r => r.LabId == typeId).ToList().Count > 0)
                    {
                        tn.Checked = true;
                    }
                    else
                    {
                        tn.Checked = false;
                    }
                }
                else if (sortId == "2")
                {
                    //仪器
                    string itrId = tn.GetValue(colQualityItrId).ToString();
                    if (int.TryParse(itrId, out iPase) && dtPowerUserInstrmtQuality.Where(r => r.ItrId == itrId).ToList().Count > 0)
                    {
                        tn.Checked = true;
                    }
                    else
                    {
                        tn.Checked = false;
                    }
                }
                else
                {
                    //医院
                    string hosId = tn.GetValue(colQualityTypeId).ToString();
                    if (dtPowerUserHospitalQuality.Where(r => r.OrgId == hosId).ToList().Count > 0)
                    {
                        tn.Checked = true;
                    }
                    else
                    {
                        tn.Checked = false;
                    }
                }
            }
        }

        private void GetUserTypeRole()
        {
            string userInfoId = "-1";
            if (tvUser.Selection.Count > 0)
            {
                if (tvUser.Selection[0].GetValue(colUserInfoId) != null)
                {
                    userInfoId = tvUser.Selection[0].GetValue(colUserInfoId).ToString();
                }
            }
            EntitySysUser dt = new EntitySysUser();
            dt.UserId = userInfoId;
            ProxyUserManage proxy = new ProxyUserManage();
            dsRoleFuncUser = proxy.Service.GetEntityList(dt);
            dtPowerUserType = dsRoleFuncUser.UserLab;
            dtPowerUserInstrmt = dsRoleFuncUser.UserItr;
            dtPowerUserHospital = dsRoleFuncUser.UserHospital;
            dtPowerUserTypeQuality = dsRoleFuncUser.UserLabQuality;
            dtPowerUserInstrmtQuality = dsRoleFuncUser.UserItrQuality;
            dtPowerUserHospitalQuality = dsRoleFuncUser.UserHosQuality;
            dtUserRole = dsRoleFuncUser.UserRole;
            dtUserDepart = dsRoleFuncUser.UserDept;
        }

        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();
        /// <summary>
        /// 自动生成拼音和五笔
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            EntitySysUser userInfo = this.bsUser.Current as EntitySysUser;
            if (userInfo != null)
            {
                this.txtPy.Text = tookit.GetSpellCode(txtUserName.Text);
                this.txtWb.Text = tookit.GetWBCode(txtUserName.Text);
                userInfo.PyCode = tookit.GetSpellCode(txtUserName.Text);
                userInfo.WbCode = tookit.GetWBCode(txtUserName.Text);
            }
        }

        #region 过滤物理组和仪器下拉框
        /// <summary>
        /// 选择默认仪器时添加默认物理组过滤
        /// </summary>
        /// <param name="strFilter"></param>


        /// <summary>
        /// 选择默认物理组时添加已选择物理组过滤
        /// </summary>
        /// <param name="strFilter"></param>
        #endregion

        #region 物理和仪器树选择控制
        /// <summary>
        /// 选择父节点时同时选择所有子节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvQualityType_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            DevExpress.XtraTreeList.Nodes.TreeListNode tn = e.Node;
            SetCheckedChildNodes(tn, tn.Checked);
        }

        /// <summary>
        /// 选择父节点时同时选择所有子节点_默认物理组和仪器下拉框过滤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvType_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            DevExpress.XtraTreeList.Nodes.TreeListNode tn = e.Node;
            SetCheckedChildNodes(tn, tn.Checked);

            //检查物理组
            string valType = cmbDefault_Type.valueMember;
            if (!string.IsNullOrEmpty(valType))
            {
                bool typeOk = false;
                for (int i = 0; i < tvType.AllNodesCount; i++)
                {
                    DevExpress.XtraTreeList.Nodes.TreeListNode eachTn = tvType.FindNodeByID(i);
                    string parentId = eachTn.GetValue(colParentId).ToString();
                    if (eachTn.Checked == true)
                    {
                        string[] array = eachTn.GetValue(colTypeId).ToString().Split('^');

                        if (array.Length > 0 && array[0] == valType)
                        {
                            typeOk = true;
                            break;
                        }
                    }
                }
                if (typeOk == false)
                {
                    cmbDefault_Type.valueMember = "";
                    cmbDefault_Type.displayMember = "";
                }
            }
            //检查仪器
            string valItr = cmbITR_Id.valueMember;
            if (!string.IsNullOrEmpty(valItr))
            {
                bool itrOk = false;
                for (int i = 0; i < tvType.AllNodesCount; i++)
                {
                    DevExpress.XtraTreeList.Nodes.TreeListNode eachTn = tvType.FindNodeByID(i);
                    string parentId = eachTn.GetValue(colParentId).ToString();
                    if (eachTn.Checked == true && parentId != "-1")
                    {
                        if (eachTn.GetValue(colTypeItr_id).ToString() == valItr)
                        {
                            itrOk = true;
                            break;
                        }
                    }
                }
                if (itrOk == false)
                {
                    cmbITR_Id.valueMember = "";
                    cmbITR_Id.displayMember = "";
                }
            }
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
        #endregion

        /// <summary>
        /// 用户类型选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (txtUserType.Text)
            {
                case "条码组":
                    {
                        //条码组
                        layoutDept.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        layoutDefaultType.Visibility =  DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        layoutDefaultITR.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        layitemgcType.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                        for (int i = 0; i < tvType.AllNodesCount; i++)
                        {
                            DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvType.FindNodeByID(i);
                            tn.Checked = false;
                        }
                        for (int i = 0; i < tvQualityType.AllNodesCount; i++)
                        {
                            DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvQualityType.FindNodeByID(i);
                            tn.Checked = false;
                        }

                        SetCheckedListComboboxFalse(chkDept);

                        cmbDefault_Type.displayMember = "";
                        cmbDefault_Type.valueMember = "";
                        cmbITR_Id.displayMember = "";
                        cmbITR_Id.valueMember = "";

                        break;
                    }
                case "护工组":
                    {
                        layoutDept.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        layoutDefaultType.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        layoutDefaultITR.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                        layitemgcType.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                        for (int i = 0; i < tvType.AllNodesCount; i++)
                        {
                            DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvType.FindNodeByID(i);
                            tn.Checked = false;
                        }
                        for (int i = 0; i < tvQualityType.AllNodesCount; i++)
                        {
                            DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvQualityType.FindNodeByID(i);
                            tn.Checked = false;
                        }

                        cmbDefault_Type.displayMember = "";
                        cmbDefault_Type.valueMember = "";
                        cmbITR_Id.displayMember = "";
                        cmbITR_Id.valueMember = "";

                        break;
                    }
                default:
                    {
                        //检验组

                        layoutDept.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        layoutDefaultType.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        layoutDefaultITR.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                        SetCheckedListComboboxFalse(chkDept);
                        layitemgcType.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        break;
                    }
            }
        }

       

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (isFirstLoad == 0)
            {
                ShowUserTypeQuality();
                isFirstLoad++;
            }
        }

        private void buttonImage_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                buttonImage.Text = openFileDialog1.FileName;

                pictureBox1.Image = Image.FromFile(buttonImage.Text);


            }
        }

        
        //追加密钥
        private void sysToolBar_BtnDesignClick(object sender, EventArgs e)
        {
            if (tvUser.Selection.Count > 0)
            {
                #region 旧CA
                //string CASignMode = UserInfo.GetSysConfigValue("CASignMode");
                //if (UserInfo.GetSysConfigValue("CASignMode") == "深圳沙井医院" || UserInfo.GetSysConfigValue("CASignMode") == "广州中医大")
                //{
                //    try
                //    {
                //        Lis.Client.CASign.FrmUserInfo CAUserInfo = new Lis.Client.CASign.FrmUserInfo();
                //        if (CAUserInfo != null)
                //        {
                //            if (!string.IsNullOrEmpty(CAUserInfo.strCerId))
                //            {
                //                this.txtCerID.Text = CAUserInfo.strCerId;
                //                //获取bjca里面的签章图片
                //                if (UserInfo.GetSysConfigValue("CASignMode") == "深圳沙井医院")
                //                {
                //                    try
                //                    {
                //                        GETKEYPICLib.GetPic gp = new GETKEYPICLib.GetPic();
                //                        var pic = gp.GetPic(CAUserInfo.strCerId);
                //                        if (pic != null)
                //                        {
                //                            var jpgSvg = gp.ConvertSvg2Png(pic);//将SVG图片转换成PNG格式

                //                            if (jpgSvg == null)
                //                            {
                //                                jpgSvg = gp.ConvertGif2Jpg(pic);
                //                            }

                //                            byte[] bt = Convert.FromBase64String(jpgSvg);
                //                            System.IO.MemoryStream streamTemp = new System.IO.MemoryStream(bt);
                //                            Bitmap bitmapTemp = new Bitmap(streamTemp);
                //                            if (bitmapTemp != null)
                //                            {
                //                                pictureBox1.Image = bitmapTemp;//提取CA图片
                //                                buttonImage.Text = "提取CA图片";
                //                            }
                //                        }
                //                    }
                //                    catch (Exception caGetPicEx)
                //                    {
                //                        lis.client.control.MessageDialog.ShowAutoCloseDialog("获取ukey的图片失败！");
                //                    }
                //                }

                //                lis.client.control.MessageDialog.ShowAutoCloseDialog("密钥追加成功,请保存修改！");
                //            }
                //            else
                //            {
                //                lis.client.control.MessageDialog.ShowAutoCloseDialog("无密钥追加,请确认！");
                //            }


                //        }
                //    }
                //    catch (Exception ex)
                //    {

                //        lis.client.control.MessageDialog.Show(ex.ToString());
                //    }


                //}
                //else if (CASignMode == "河池市人民医院" || CASignMode == "广东医学院附属医院")
                //{
                //    try
                //    {
                //        Lis.Client.CASign.FrmUserInfo CAUserInfo = new Lis.Client.CASign.FrmUserInfo();
                //        if (CAUserInfo != null)
                //        {
                //            if (!string.IsNullOrEmpty(CAUserInfo.strCerId))
                //            {
                //                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvUser.Selection[0];
                //                string loginId = tn.GetValue(colLoginId).ToString();
                //                string userName = tn.GetValue(colUserName).ToString();
                //                string cerid = tn.GetValue(colUserCerId).ToString();
                //                string entity_id = tn.GetValue(colCaEntityId).ToString();
                //                if (CASignMode == "河池市人民医院" && !CAUserInfo.strUserList.Contains(CAUserInfo.strCerId))
                //                {
                //                    entity_id = CAUserInfo.strUserList;
                //                }
                //                if (!string.IsNullOrEmpty(cerid))
                //                {
                //                    if (lis.client.control.MessageDialog.Show("用户正在使用密钥,是否覆盖追加?", MessageBoxButtons.YesNo) != DialogResult.Yes)
                //                        return;
                //                }
                //                ProxyUserManage proxyUser = new ProxyUserManage();
                //                ProxySysUserInfo proxy = new ProxySysUserInfo();
                //                EntityUserQc userQc = new EntityUserQc();
                //                userQc.LoginId = loginId;
                //                userQc.UserCerId = CAUserInfo.strCerId;
                //                userQc.NotEqual = true;
                //                List<EntityCaSign> dtCaSign = new List<EntityCaSign>();
                //                List<EntitySysUser> dtUsers = proxy.Service.SysUserQuery(userQc);
                //                if (dtUsers != null && dtUsers.Count > 0)
                //                {
                //                    List<string> users = new List<string>();
                //                    List<string> ids = new List<string>();
                //                    foreach (EntitySysUser dr in dtUsers)
                //                    {
                //                        users.Add(dr.UserLoginid + "(" + dr.UserName + ")");
                //                        ids.Add(dr.UserLoginid.ToString());
                //                        EntityCaSign drDel = new EntityCaSign();
                //                        dtCaSign.Add(drDel);
                //                        drDel.CaCerId = CAUserInfo.strCerId;
                //                        if (CASignMode == "河池市人民医院" && !CAUserInfo.strUserList.Contains(CAUserInfo.strCerId))
                //                        {
                //                            drDel.CaEntityId = CAUserInfo.strUserList;
                //                        }
                //                        drDel.CaLoginId = UserInfo.loginID;
                //                        drDel.CaName = UserInfo.userName;
                //                        drDel.CaEvent = "停止使用";
                //                        drDel.CaRemark = dr.LoginId + " - " + dr.UserName + " 停止使用";
                //                    }
                //                    string msg = "以下工号正在使用此密钥,是否取消该工号的密钥,并追加到当前工号?\r\n";
                //                    msg += string.Join("\r\n", users.ToArray());
                //                    msg += "\r\n\r\n【是】取消并追加到当前工号!\r\n【否】取消本次操作";
                //                    DialogResult result = lis.client.control.MessageDialog.Show(msg, MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
                //                    if (result != DialogResult.Yes)
                //                    {
                //                        return;
                //                    }
                //                    proxyUser.Service.DelCerid(CAUserInfo.strCerId, entity_id);

                //                }
                //                GETKEYPICLib.GetPic getpic = new GETKEYPICLib.GetPic();
                //                byte[] bytes = Convert.FromBase64String(getpic.GetPic(CAUserInfo.strCerId));
                //                MemoryStream memStream = new MemoryStream(bytes);
                //                Image image = Image.FromStream(memStream);
                //                pictureBox1.Image = image;
                //                buttonImage.Text = "提取CA图片";
                //                proxyUser.Service.SetCerid(loginId, CAUserInfo.strCerId, entity_id);
                //                List<EntityCaSign> CaSignList = new List<EntityCaSign>();
                //                EntityCaSign drCaSign = new EntityCaSign();
                //                drCaSign.CaCerId = CAUserInfo.strCerId;
                //                if (CASignMode == "河池市人民医院" && !CAUserInfo.strUserList.Contains(CAUserInfo.strCerId))
                //                {
                //                    drCaSign.CaEntityId = CAUserInfo.strUserList;
                //                }
                //                drCaSign.CaDate = ServerDateTime.GetServerDateTime();
                //                drCaSign.CaLoginId = UserInfo.loginID;
                //                drCaSign.CaName = UserInfo.userName;
                //                drCaSign.CaEvent = "追加密钥";
                //                drCaSign.CaRemark = loginId + " - " + userName + " 追加密钥";
                //                CaSignList.Add(drCaSign);
                //                proxyUser.Service.InsertCaSign(CaSignList);

                //                this.txtCerID.Text = CAUserInfo.strCerId;
                //                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode node in tvUser.Nodes)
                //                {
                //                    if (node.GetValue(colUserCerId).ToString() == CAUserInfo.strCerId)
                //                        node.SetValue(colUserCerId, "");
                //                }
                //                tn.SetValue(colUserCerId, CAUserInfo.strCerId);
                //                lis.client.control.MessageDialog.ShowAutoCloseDialog("密钥追加成功！");
                //            }
                //            else
                //            {
                //                lis.client.control.MessageDialog.ShowAutoCloseDialog("无密钥追加,请确认！");
                //            }


                //        }
                //    }
                //    catch (Exception ex)
                //    {

                //        lis.client.control.MessageDialog.Show(ex.ToString());
                //    }


                //}
                //else if (UserInfo.GetSysConfigValue("CASignMode") == "惠州三院")
                //{
                //    try
                //    {
                //        string password = string.Empty;
                //        FrmNetCASignRecord record = new FrmNetCASignRecord("GDCA");
                //        if (record.ShowDialog() == DialogResult.Yes)
                //        {
                //            password = record.Password;
                //        }

                //        if (string.IsNullOrEmpty(password))
                //        {
                //            lis.client.control.MessageDialog.ShowAutoCloseDialog("请输入USBKEY的密码！");
                //            return;
                //        }


                //        GDCAReader CAUserInfo = new GDCAReader();


                //        bool issucc = CAUserInfo.GDCALoginKey(password);

                //        if (!issucc)
                //        {
                //            lis.client.control.MessageDialog.ShowAutoCloseDialog("密码错误！");
                //            return;
                //        }

                //        this.txtCerID.Text = CAUserInfo.GetCertKey();

                //        CAUserInfo.Release();
                //        lis.client.control.MessageDialog.ShowAutoCloseDialog("密钥追加成功,请保存修改！");


                //    }
                //    catch (Exception ex)
                //    {

                //        lis.client.control.MessageDialog.Show(ex.ToString());
                //    }


                //}
                //else
                //{
                //    if (UserInfo.GetSysConfigValue("CASignMode") == "中大肿瘤医院")
                //    {

                //        ProxySysUserInfo proxyUser = new ProxySysUserInfo();

                //        //NETCA模式
                //        string strNETCA_ModeType = ConfigHelper.GetSysConfigValueWithoutLogin("NETCA_ModeType");


                //        NetcaSign signer = new NetcaSign();

                //        string strKeyCode = string.Empty;
                //        string password = string.Empty;
                //        X509Certificate2 certificate2 = null;

                //        string subjectCN = null;//证书的主题CN项

                //        object cert_code = null;//

                //        if (signer.login(UserInfo.loginID, ref strKeyCode))
                //        {
                //            try
                //            {
                //                if (strNETCA_ModeType == "深圳新安")
                //                {
                //                    signer.GetCertStringAttribute(ref subjectCN, ref cert_code);

                //                    if (!string.IsNullOrEmpty(subjectCN))
                //                    {
                //                        if (true)
                //                        {
                //                            password = "123456";

                //                            //获取netca里面的签章图片
                //                            if (true)
                //                            {
                //                                try
                //                                {
                //                                    var ulist = SGL_CaClass.CaHelper.GetUserList();//获取证书列表
                //                                    IDictionary<string, object> pics = SGL_CaClass.CaHelper.GetPicS();
                //                                    foreach (KeyValuePair<string, object> pic in pics)
                //                                    {
                //                                        byte[] Pic_byte = pic.Value as Byte[];
                //                                        if (Pic_byte != null)
                //                                        {
                //                                            System.IO.MemoryStream ms = new System.IO.MemoryStream(Pic_byte);
                //                                            Bitmap bitmapTemp = new Bitmap(ms);
                //                                            if (bitmapTemp != null)
                //                                            {
                //                                                pictureBox1.Image = bitmapTemp;//提取CA图片
                //                                                buttonImage.Text = "提取CA图片";
                //                                            }
                //                                            break;
                //                                        }
                //                                    }
                //                                }
                //                                catch (Exception caGetPicEx)
                //                                {
                //                                    Lib.LogManager.Logger.LogException("获取ukey的图片", caGetPicEx);
                //                                    lis.client.control.MessageDialog.ShowAutoCloseDialog("获取ukey的图片失败！详情查看日志");
                //                                }
                //                            }
                //                        }
                //                    }
                //                }
                //                else
                //                {
                //                    certificate2 = signer.Sign_Cert();
                //                    if (certificate2 != null)
                //                    {
                //                        FrmNetCASignRecord record = new FrmNetCASignRecord(certificate2.FriendlyName);
                //                        if (record.ShowDialog() == DialogResult.Yes)
                //                        {
                //                            password = record.Password;
                //                        }
                //                    }
                //                }

                //                if (!string.IsNullOrEmpty(password))
                //                {
                //                    txtPassword.Text = password;
                //                    password = EncryptClass.Encrypt(password);
                //                }
                //            }
                //            catch (Exception ex2)
                //            {
                //                Lib.LogManager.Logger.LogException("追加密钥", ex2);
                //            }

                //            if (certificate2 != null && !string.IsNullOrEmpty(password))
                //            {
                //                proxyUser.Service.AddUserinfoKey(tvUser.Selection[0].GetValue(colLoginId).ToString(),
                //                                                     strKeyCode, certificate2.Export(X509ContentType.SerializedCert), password);

                //                lis.client.control.MessageDialog.ShowAutoCloseDialog("密钥与证书签章信息追加成功！请重新登录");

                //                //系统配置：NETCA模式
                //                if (UserInfo.GetSysConfigValue("NETCA_ModeType") == "通用")
                //                {
                //                    this.txtCerID.Text = strKeyCode;
                //                }
                //                else if (UserInfo.GetSysConfigValue("NETCA_ModeType") == "身份证")
                //                {
                //                    this.txtCerID.Text = GDNetCA.GetUserID();
                //                }
                //            }
                //            else if (!string.IsNullOrEmpty(subjectCN) && cert_code != null && !string.IsNullOrEmpty(password))
                //            {
                //                proxyUser.Service.AddUserinfoKey(tvUser.Selection[0].GetValue(colLoginId).ToString(),
                //                                                     strKeyCode, (byte[])cert_code, password);
                //                lis.client.control.MessageDialog.ShowAutoCloseDialog("密钥与证书签章信息追加成功！请重新登录");

                //                this.txtCerID.Text = strKeyCode;

                //            }
                //            else
                //            {
                //                proxyUser.Service.AddUserinfoKey(tvUser.Selection[0].GetValue(colLoginId).ToString(),
                //                                                     strKeyCode, null, password);
                //                lis.client.control.MessageDialog.ShowAutoCloseDialog("密钥追加成功！证书签章信息追加失败！请重新登录！");
                //            }

                //        }
                //        else
                //        {
                //            lis.client.control.MessageDialog.ShowAutoCloseDialog("密钥读取失败！");
                //        }
                //    }
                //}
                #endregion

                dcl.client.ca.ICaPKI caPKI = ca.CaPKIFactory.CreateCASignature(CASignMode);
                if (caPKI != null)
                {
                    DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvUser.Selection[0];
                    string loginId = tn.GetValue(colLoginId).ToString();
                    caPKI.UserId = loginId;
                    string caEntityID = caPKI.GetIdentityID();
                    if (!string.IsNullOrEmpty(caEntityID))
                    {
                        this.txtCAEntityID.Text = caEntityID;
                    }
                    else
                    {
                        MessageBox.Show(caPKI.ErrorInfo, "CA认证", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    MessageBox.Show(ca.CaPKIFactory.errorInfo, "CA认证", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                lis.client.control.MessageDialog.ShowAutoCloseDialog("请选择要添加密钥的用户！");
        }
        //停止使用密钥
        private void sysToolBar_BtnResetClick(object sender, EventArgs e)
        {
            DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvUser.Selection[0];
            string loginId = tn.GetValue(colLoginId).ToString();
            string userName = tn.GetValue(colUserName).ToString();
            string cerid = tn.GetValue(colUserCerId).ToString();
            string entity_id = tn.GetValue(colCaEntityId).ToString();
            if (!string.IsNullOrEmpty(cerid))
            {
                try
                {
                    ProxyUserManage proxyUser = new ProxyUserManage();
                    proxyUser.Service.SetCerid(loginId, "", "");
                    List<EntityCaSign> CaSignList = new List<EntityCaSign>();
                    EntityCaSign dr = new EntityCaSign();
                    dr.CaCerId = cerid;
                    dr.CaDate = ServerDateTime.GetServerDateTime();
                    dr.CaEntityId = entity_id;
                    dr.CaLoginId = UserInfo.loginID;
                    dr.CaName = UserInfo.userName;
                    dr.CaEvent = "停止使用";
                    dr.CaRemark = loginId + " - " + userName + " 停止使用";
                    CaSignList.Add(dr);
                    proxyUser.Service.InsertCaSign(CaSignList);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("停止使用密钥", ex);
                }

                tn.SetValue(colUserCerId, "");
                tn.SetValue(colCaEntityId, "");
                txtCerID.Text = "";
                lis.client.control.MessageDialog.ShowAutoCloseDialog("密钥已取消使用.");
            }
            else
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("该用户并未使用密钥.");
            }

        }
        //CA密钥使用情况
        private void sysToolBar_OnBtnQualityAuditClicked(object sender, EventArgs e)
        {
            DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvUser.Selection[0];
            string loginId = tn.GetValue(colLoginId).ToString();
            string userName = tn.GetValue(colUserName).ToString();
            string cerid = tn.GetValue(colUserCerId).ToString();
            string entity_id = tn.GetValue(colCaEntityId).ToString();
            if (!string.IsNullOrEmpty(cerid))
            {
                try
                {
                    ProxyUserManage proxyUser = new ProxyUserManage();
                    List<EntityCaSign> dsCaSign = proxyUser.Service.GetCaSign(cerid, entity_id);
                    if (dsCaSign != null && dsCaSign.Count > 0)
                    {
                        FrmCaSign frm = new FrmCaSign(dsCaSign);
                        frm.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("密钥使用情况", ex);
                }

            }
            else
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("该用户并未使用密钥.");
            }

        }

        private void tvUser_NodeCellStyle(object sender, DevExpress.XtraTreeList.GetCustomNodeCellStyleEventArgs e)
        {
            if (e.Column.FieldName != "UserLoginid") return;
            string loginid = e.Node.GetValue(e.Column.AbsoluteIndex)?.ToString();
            if (string.IsNullOrEmpty(loginid)) return;
            var users= from a in dtUser where a.UserLoginid == loginid select a;
            if (users.Count() == 0)
                return;
            foreach(EntitySysUser user in users)
            {
                if (!string.IsNullOrEmpty(user.DelFlag) && user.DelFlag == "1")
                    e.Appearance.ForeColor = Color.Red;
                else
                    e.Appearance.ForeColor = Color.Black;
            }

        }

        private void cmbITR_Id_Load(object sender, EventArgs e)
        {
            string filter = "";
            for (int i = 0; i < tvType.AllNodesCount; i++)
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvType.FindNodeByID(i);
                string parentId = tn.GetValue(colParentId).ToString();
                if (tn.Checked == true && parentId != "-1")
                {
                    string itrId = tn.GetValue(colTypeItr_id).ToString();
                    filter += itrId + ",";
                }
            }
            if (filter != "")
            {
                cmbITR_Id.dtSource = cmbITR_Id.dtSource.Where(i => i.ItrId.Contains(filter.Substring(0, filter.Length - 1))).ToList();
            }
        }

        #region 多选下拉框 清空所有选中项目
        /// <summary>
        /// 将多选下拉框的所有勾选置为False
        /// </summary>
        /// <param name="control"></param>
        private void SetCheckedListComboboxFalse(CheckedComboBoxEdit control)
        {
            foreach (CheckedListBoxItem i in control.Properties.Items)
            {
                i.CheckState = CheckState.Unchecked;
            }
        }
        #endregion

        #region 多选下拉框 用户科室问题
        /// <summary>
        /// 科室绑定数据源
        /// </summary>
        /// <returns></returns>
        public BindingList<EntityDicPubDept> IniFuncDept()
        {
            BindingList<EntityDicPubDept> bindlist = new BindingList<EntityDicPubDept>();
            foreach(EntityDicPubDept dict in dtDepart)
            {
                bindlist.Add(dict);
            }
            return bindlist;
        }

        private void BindDept()
        {
            chkDept.Properties.Items.Clear();
            BindingList<EntityDicPubDept> result = IniFuncDept();
            for (int i = 0; i < result.Count; i++)
            {
                chkDept.Properties.Items.Add(result[i].DeptId, result[i].DeptName, CheckState.Checked, true);
            }
        }

        /// <summary>
        /// 显示用户所属科室
        /// </summary>
        private void ShowUserDepart()
        {
            foreach (CheckedListBoxItem ss in chkDept.Properties.Items)
            {
                string DeptID = ss.Value.ToString();
                var dept = from a in dtUserDepart where a.DeptId == DeptID select a;
                if (dept.Count() > 0)
                    ss.CheckState = CheckState.Checked;
                else
                    ss.CheckState = CheckState.Unchecked;
            }

        }

        /// <summary>
        /// 获取当前选中的科室集合
        /// </summary>
        /// <returns></returns>
        private List<EntityUserDept> GetChooseDept(string userInfoId)
        {
            List<EntityUserDept> dtPowerUserDepart = new List<EntityUserDept>();
            foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem s in chkDept.Properties.Items)
            {
                if (s.CheckState == CheckState.Checked)
                {
                    List<EntityDicPubDept> Depts = (from a in dtDepart where a.DeptId == s.Value.ToString() select a).ToList<EntityDicPubDept>();
                    if (Depts.Count() == 0)
                        continue;
                    EntityDicPubDept Dept = Depts[0];

                    EntityUserDept dr = new EntityUserDept();
                    if (userInfoId != "")
                    {
                        dr.UserId = userInfoId;
                    }
                    dr.DeptId = Dept.DeptId.ToString();
                    dtPowerUserDepart.Add(dr);
                }
            }
            return dtPowerUserDepart;
        }

        #endregion

        #region 多选下拉框 用户角色问题
        public BindingList<EntitySysRole> IniFuncRole()
        {
            BindingList<EntitySysRole> bindlist = new BindingList<EntitySysRole>();
            foreach (EntitySysRole dict in dtRole)
            {
                bindlist.Add(dict);
            }
            return bindlist;
        }

        private void BindRole()
        {
            chkRole.Properties.Items.Clear();
            BindingList<EntitySysRole> result = IniFuncRole();
            for (int i = 0; i < result.Count; i++)
            {
                chkRole.Properties.Items.Add(result[i].RoleId, result[i].RoleName, CheckState.Checked, true);
            }
        }

        /// <summary>
        /// 获取用户所选择的角色集合
        /// </summary>
        /// <returns></returns>
        private List<EntityUserRole> GetChooseRole(string userInfoId)
        {
            List<EntityUserRole> dtPowerUserRole = new List<EntityUserRole>();

            foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem s in chkRole.Properties.Items)
            {
                if (s.CheckState == CheckState.Checked)
                {
                    List<EntitySysRole> Roles = (from a in dtRole where a.RoleId == s.Value.ToString() select a).ToList<EntitySysRole>();
                    if (Roles.Count() == 0)
                        continue;
                    EntitySysRole role = Roles[0];

                    EntityUserRole dr = new EntityUserRole();
                    if (userInfoId != "")
                    {
                        dr.UserId = userInfoId;
                    }
                    dr.RoleId = role.RoleId.ToString();
                    dtPowerUserRole.Add(dr);
                }
            }
            return dtPowerUserRole;
        }

        /// <summary>
        /// 显示用户所属角色
        /// </summary>
        private void ShowUserRole()
        {
            foreach (CheckedListBoxItem ss in chkRole.Properties.Items)
            {
                string roleID = ss.Value.ToString();
                var dept = from a in dtUserRole where a.RoleId == roleID select a;
                if (dept.Count() > 0)
                    ss.CheckState = CheckState.Checked;
                else
                    ss.CheckState = CheckState.Unchecked;
            }

        }
        #endregion

        private void btReadKey_Click(object sender, EventArgs e)
        {
            dcl.client.ca.ICaPKI caPKI = ca.CaPKIFactory.CreateCASignature(CASignMode);
            if (caPKI != null)
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvUser.Selection[0];
                string loginId = tn.GetValue(colLoginId).ToString();
                caPKI.UserId = loginId;
                string caVal = caPKI.GetIdentityID();
                if (string.IsNullOrEmpty(caVal))
                {
                    MessageBox.Show(caPKI.ErrorInfo, "CA认证", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                string caEntityID = caVal.Split(':')[0];
                string caImage = caVal.Split(':')[1];
                if (!string.IsNullOrEmpty(caEntityID))
                {
                    this.txtCAEntityID.Text = caEntityID;
                }
                if (!string.IsNullOrEmpty(caImage))
                {
                    byte[] bt = Convert.FromBase64String(caImage);
                    System.IO.MemoryStream streamTemp = new System.IO.MemoryStream(bt);
                    Image image = Image.FromStream(streamTemp);
                    Bitmap bitmapTemp = new Bitmap(streamTemp);
                    if (bitmapTemp != null)
                    {
                        pictureBox1.Image = bitmapTemp;//提取CA图片
                        buttonImage.Text = "提取CA图片";
                    }
                }

            }
            else
            {
                MessageBox.Show(ca.CaPKIFactory.errorInfo, "CA认证", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
