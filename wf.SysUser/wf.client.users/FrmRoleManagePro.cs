using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.entity;
using System.Linq;
using dcl.client.wcf;

namespace dcl.client.users
{
    public partial class FrmRoleManagePro : FrmCommon
    {
        ProxyRoleManagePro proxy = new ProxyRoleManagePro();

        List<EntitySysRole> roleList = new List<EntitySysRole>();//角色列表
        List<EntitySysUser> userList = new List<EntitySysUser>();//用户列表
        List<EntitySysFunction> funcList = new List<EntitySysFunction>();//功能列表

        List<string> roleUser = new List<string>();//角色包含用户
        List<string> roleFunc = new List<string>();//角色包含功能
        public FrmRoleManagePro()
        {
            InitializeComponent();
            this.txtRoleName.EditValueChanged += new System.EventHandler(this.txtRoleName_EditValueChanged);
            this.txtRoleDesc.EditValueChanged += new System.EventHandler(this.txtRoleDesc_EditValueChanged);

            this.tvPower.BeforeCheckNode += new DevExpress.XtraTreeList.CheckNodeEventHandler(this.tvPower_BeforeCheckNode);
            this.tvPower.AfterCheckNode += new DevExpress.XtraTreeList.NodeEventHandler(this.tvPower_AfterCheckNode);
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
        DataSet dsRole = new DataSet();
        //缓存指定角色的权限点和用户查询结果
        DataSet dsRoleFuncUser = new DataSet();

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmRoleManagePro_Load(object sender, EventArgs e)
        {
            //需要显示的按钮和顺序
            sysToolBar1.SetToolButtonStyle(new string[] { "BtnAdd", "BtnModify", "BtnDelete", "BtnSave", "BtnCancel", "BtnRefresh" });

            LoadData();
        }

        #region 与角色增删改相关的代码
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddRole_Click(object sender, EventArgs e)
        {
            EntitySysRole role = bsRole.AddNew() as EntitySysRole;
            tvRole.Enabled = false;

            optionStatus = OptionStatus.Insert;
            EnterEditingState(true);

            txtRoleName.Text = "";
            txtRoleDesc.Text = "";


            for (int i = 0; i < tvPower.AllNodesCount; i++)
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvPower.FindNodeByID(i);
                tn.Checked = false;
            }

            for (int i = 0; i < cbxRoleUser.ItemCount; i++)
            {
                cbxRoleUser.SetItemChecked(i, false);
            }

            txtRoleName.Focus();
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifyRole_Click(object sender, EventArgs e)
        {
            tvRole.Enabled = false;
            if (tvRole.Selection.Count < 1)
            {
                lis.client.control.MessageDialog.Show(PowerMessage.BASE_SELECT_NULL, PowerMessage.BASE_TITLE);
                return;
            }

            optionStatus = OptionStatus.Update;
            EnterEditingState(true);

            txtRoleName.Focus();
        }

        /// <summary>
        /// 保存角色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveRole_Click(object sender, EventArgs e)
        {
            tvRole.Enabled = true;
            this.isActionSuccess = false;

            if (txtRoleName.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show(PowerMessage.BASE_ROLENAME_NULL, PowerMessage.BASE_TITLE);
                txtRoleName.Focus();
                return;
            }

            if (bsRole.Current == null) return;

            EntitySysRole role = bsRole.Current as EntitySysRole;
            int roleId = -1;
            int.TryParse(role.RoleId, out roleId);
            //不允许重复的角色名
            if (roleList.Where(w => w.RoleName == role.RoleName).Count() > 1)
            {
                lis.client.control.MessageDialog.Show(PowerMessage.BASE_ROLENAME_SAME, PowerMessage.BASE_TITLE);
                txtRoleName.Focus();
                return;
            }
            var users = new List<EntityUserRole>();//临时用户列表
            var funcs = new List<EntitySysRoleFunction>();//临时功能列表

            //把勾选的功能添加到临时功能列表
            for (int i = 0; i < tvPower.AllNodesCount; i++)
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvPower.FindNodeByID(i);
                if (tn.Checked)
                {
                    EntitySysRoleFunction roleFunc = new EntitySysRoleFunction()
                    { RoleId = roleId, FuncId = (int)tn.GetValue(tvPower.KeyFieldName) };
                    funcs.Add(roleFunc);
                }
            }
            //把勾选的用户添加到临时用户列表
            for (int i = 0; i < cbxRoleUser.ItemCount; i++)
            {
                EntitySysUser dr = (EntitySysUser)cbxRoleUser.GetItem(i);
                string userInfoId = dr.UserId.ToString();
                if (cbxRoleUser.GetItemChecked(i))
                {
                    EntityUserRole userRole = new EntityUserRole()
                    {
                        UserId = (cbxRoleUser.GetItem(i) as EntitySysUser).UserId,
                        RoleId = roleId.ToString()
                    };
                    users.Add(userRole);
                }
            }
            role.listUser = users;
            role.listFunc = funcs;
            bool result;

            if (optionStatus == OptionStatus.Insert)
            {
                //Insert
                result = proxy.Service.InsertRoleInfo(role);
            }
            else
            {
                //Update
                result = proxy.Service.UpdateRoleInfo(role);
            }

            if (result)
            {
                sysToolBar1.LogMessage = string.Format("保存成功,角色名称: {0}", txtRoleName.Text.Trim());
            }

            //重新载入数据
            LoadData();
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelRole_Click(object sender, EventArgs e)
        {
            if (tvRole.Selection.Count < 1)
            {
                lis.client.control.MessageDialog.Show(PowerMessage.BASE_SELECT_NULL, PowerMessage.BASE_TITLE);
                return;
            }

            EntitySysRole role = bsRole.Current as EntitySysRole;
            string roleName = role.RoleName;
            bool result = false;
            DialogResult dresult = MessageBox.Show(PowerMessage.BASE_DELETE_CONFIRM, PowerMessage.BASE_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            switch (dresult)
            {
                case DialogResult.OK:
                    result = proxy.Service.DeleteRoleInfo(role);
                    break;
                case DialogResult.Cancel:
                    return;
            }

            if (result)
            {
                sysToolBar1.LogMessage = string.Format("删除成功,角色名称: {0}", roleName);
            }

            //删除记录后重新加载界面
            LoadData();
        }

        /// <summary>
        /// 放弃操作角色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelRole_Click(object sender, EventArgs e)
        {
            tvRole.Enabled = true;

            LoadData();
            tvRole_FocusedNodeChanged(null, null);
            //bsRole_CurrentChanged(null, null);
        }

        /// <summary>
        /// 控制所有输入框状态
        /// </summary>
        /// <param name="enable"></param>
        private void EnterEditingState(Boolean enable)
        {
            txtRoleName.Properties.ReadOnly = !enable;
            txtRoleDesc.Properties.ReadOnly = !enable;

            if (enable)
            {
                cbxRoleUser.SelectionMode = SelectionMode.One;
            }
            else
            {
                cbxRoleUser.SelectionMode = SelectionMode.None;
            }

        }

        /// <summary>
        /// 根据状态来控制树节点是否可选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvPower_BeforeCheckNode(object sender, DevExpress.XtraTreeList.CheckNodeEventArgs e)
        {
             if (txtRoleName.Properties.ReadOnly == false)
            {
                e.CanCheck = true;
            }
            else
            {
                e.CanCheck = false;
            }
        }
        #endregion

        #region 权限树的显示,保存和控制
        /// <summary>
        /// 显示角色权限
        /// </summary>
        private void ShowRoleFunc()
        {
            //遍历树显示角色权限
            for (int i = 0; i < tvPower.AllNodesCount; i++)
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvPower.FindNodeByID(i);

                string funcInfoId = tn.GetValue(tvPower.KeyFieldName).ToString();

                if (roleFunc.Contains(funcInfoId))
                {
                    tn.Checked = true;
                }
                else
                {
                    tn.Checked = false;
                }
            }
        }


        /// <summary>
        /// 选择父节点时同时选择所有子节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvPower_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
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
        #endregion

        #region 初始化数据和选中角色后的事件
        /// <summary>
        /// 初始化数据
        /// </summary>
        private void LoadData()
        {
            optionStatus = OptionStatus.Update;
            EnterEditingState(false);

            EntityResponse response = proxy.Service.GetAllInfo();
            Dictionary<string, object> dict = response.GetResult() as Dictionary<string, object>;

            //权限树
            funcList = dict["funcList"] as List<EntitySysFunction>;
            bsFunc.DataSource = funcList;

            //不用tvPower.ExpandAll(),只展开第1级
            for (int i = 0; i < tvPower.Nodes.Count; i++)
            {
                tvPower.Nodes[i].Expanded = true;
            }

            //用户列表
            xtraTabControl1.SelectedTabPageIndex = 1;
            userList = dict["userList"] as List<EntitySysUser>;
            bsUser.DataSource = userList;
            xtraTabControl1.SelectedTabPageIndex = 0;
            //角色列表_最后加载,以保证权限树和用户列表已生成
            roleList = dict["roleList"] as List<EntitySysRole>;
            bsRole.DataSource = roleList;

        }

        /// <summary>
        /// 选中角色事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvRole_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            sysToolBar1.EnableButton(false);

            if (tvRole.Selection.Count > 0)
            {
                optionStatus = OptionStatus.Update;
                EnterEditingState(false);

                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvRole.Selection[0];

                //查询指定角色的功能点和用户    
                GetRoleFuncUser();

                //显示角色权限
                ShowRoleFunc();

                //显示角色用户
                ShowRoleUser();
            }
        }



        /// <summary>
        /// 获得指定角色功能点和用户的数据集
        /// </summary>
        /// <param name="roleInfoId"></param>
        private void GetRoleFuncUser()
        {
            string roleInfoId = "-1";
            if (tvRole.Selection.Count > 0 && tvRole.Selection[0].GetValue(colRoleInfoId) != null)
            {
                roleInfoId = tvRole.Selection[0].GetValue(colRoleInfoId).ToString();
            }

            EntitySysRole role = proxy.Service.GetRoleUserAndFunc(roleInfoId);
            List<EntityUserRole> listUserRole = role.listUser;
            List<EntitySysRoleFunction> listRoleFunc = role.listFunc;
            roleUser = (from x in listUserRole
                        select x.UserId).ToList();

            roleFunc = (from x in listRoleFunc
                        select x.FuncId.ToString()).ToList();
        }



        /// <summary>
        /// 刷新全部界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefreshRole_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        #endregion

        #region 用户列表相关操作

        /// <summary>
        /// 显示角色下的用户
        /// </summary>
        private void ShowRoleUser()
        {
            //角色管理界面右边用户列表，需求把已经勾上的用户排在前面
            for (int i = 0; i < cbxRoleUser.ItemCount; i++)
            {
                EntitySysUser dr = (EntitySysUser)cbxRoleUser.GetItem(i);
                string userInfoId = dr.UserId.ToString();

                if (roleUser.Contains(userInfoId))
                {
                    dr.Checked = true;
                }
                else
                {
                    dr.Checked = false;
                }
            }
            bsUser.DataSource = userList.OrderByDescending(i => i.Checked).ToList();

            int x = 0;

            if (cbxRoleUser.ItemCount % 2 == 0)
                x = cbxRoleUser.ItemCount / 2;
            else
                x = (cbxRoleUser.ItemCount + 1) / 2;

            if (roleUser.Count > x)
            {
                cbxRoleUser.CheckAll();
                for (int i = 0; i < cbxRoleUser.ItemCount; i++)
                {
                    EntitySysUser dr = (EntitySysUser)cbxRoleUser.GetItem(i);
                    string userInfoId = dr.UserId.ToString();

                    if (!roleUser.Contains(userInfoId))
                    {
                        cbxRoleUser.SetItemChecked(i, false);
                    }
                }
            }
            else
            {
                cbxRoleUser.UnCheckAll();
                for (int i = 0; i < cbxRoleUser.ItemCount; i++)
                {
                    EntitySysUser dr = (EntitySysUser)cbxRoleUser.GetItem(i);
                    string userInfoId = dr.UserId.ToString();

                    if (roleUser.Contains(userInfoId))
                    {
                        cbxRoleUser.SetItemChecked(i, true);
                    }
                }
            }

        }

        #endregion

        private void txtRoleName_EditValueChanged(object sender, EventArgs e)
        {
            if (bsRole.Current == null) return;
            EntitySysRole role = bsRole.Current as EntitySysRole;
            role.RoleName = txtRoleName.Text.ToString();
        }

        private void txtRoleDesc_EditValueChanged(object sender, EventArgs e)
        {
            if (bsRole.Current == null) return;
            EntitySysRole role = bsRole.Current as EntitySysRole;
            role.RoleRemark = txtRoleDesc.Text.ToString();
        }

        private void bsRole_CurrentChanged(object sender, EventArgs e)
        {
            //sysToolBar1.EnableButton(false);

            //if (tvRole.Selection.Count > 0)
            //{
            //    optionStatus = OptionStatus.Update;
            //    EnterEditingState(false);

            //    DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvRole.Selection[0];

            //    //查询指定角色的功能点和用户    
            //    GetRoleFuncUser();

            //    //显示角色权限
            //    ShowRoleFunc();

            //    //显示角色用户
            //    ShowRoleUser();
            //}
        }
    }
}
