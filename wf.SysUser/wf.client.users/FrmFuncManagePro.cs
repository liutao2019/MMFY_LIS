using System;
using System.Collections.Generic;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.entity;
using dcl.client.wcf;

namespace dcl.client.users
{
    public partial class FrmFuncManagePro : FrmCommon
    {
        List<EntitySysFunction> list = new List<EntitySysFunction>();
        ProxySysFunc proxy = new ProxySysFunc();
        public FrmFuncManagePro()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 判断点击保存按钮时是Insert还是Update的标志位
        /// </summary>
        public enum OptionStatus
        {
            Insert,
            Update
        }
        OptionStatus optionStatus = OptionStatus.Update;

        //数据源缓存

        /// <summary>
        /// 窗体载入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmFuncManagePro_Load(object sender, EventArgs e)
        {
            this.ShowSucessMessage = false;

            //需要显示的按钮和顺序
            sysToolBar.SetToolButtonStyle(new string[] { "BtnAdd", "BtnModify", "BtnDelete", "BtnSave", "BtnCancel", "BtnRefresh" });

            LoadData();
        }

        /// <summary>
        /// 新增按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            this.bsPower.EndEdit();
            EntitySysFunction func = bsPower.AddNew() as EntitySysFunction;

            optionStatus = OptionStatus.Insert;
            EnterEditingState(true);

            //新增时清空文本框,并把顺序号默认加1以方便批量录入
            txtFuncName.Text = "";
            txtModuleName.Text = "";
            txtSort.Value = txtSort.Value + 1;
            func.FuncSortNo = int.Parse(txtSort.Value.ToString());
            txtQuickLoad.Value = -1;
            txtImageSource.Text = "";
            this.txtShortcut.Text = "";
            txtFuncName.Focus();


        }

        /// <summary>
        /// 修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            if (tvPower.Selection.Count < 1)
            {
                lis.client.control.MessageDialog.Show(PowerMessage.BASE_SELECT_NULL, PowerMessage.BASE_TITLE);
                return;
            }

            optionStatus = OptionStatus.Update;
            EnterEditingState(true);

            txtFuncName.Focus();
        }


        /// <summary>
        /// 控制所有输入框状态
        /// </summary>
        /// <param name="enable"></param>
        private void EnterEditingState(Boolean enable)
        {
            txtFuncCode.Properties.ReadOnly = !enable;
            txtFuncName.Properties.ReadOnly = !enable;
            cbxFuncType.Properties.ReadOnly = !enable;
            txtParentId.Properties.ReadOnly = !enable;
            txtModuleName.Properties.ReadOnly = !enable;
            txtSort.Properties.ReadOnly = !enable;
            txtImageSource.Properties.ReadOnly = !enable;
            txtQuickLoad.Properties.ReadOnly = !enable;
            txtDictCache.Properties.ReadOnly = !enable;
            ckValidateUser.Properties.ReadOnly = !enable;
            txtShortcut.Properties.ReadOnly = !enable;
        }

        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (tvPower.Selection.Count < 1)
            {
                lis.client.control.MessageDialog.Show(PowerMessage.BASE_SELECT_NULL, PowerMessage.BASE_TITLE);
                return;
            }
            string funcName = tvPower.Selection[0].GetValue(colFuncName).ToString();

            if (bsPower.Current == null) return;

            EntitySysFunction func = bsPower.Current as EntitySysFunction;
            bool result = false;

            DialogResult dresult = MessageBox.Show(PowerMessage.BASE_DELETE_CONFIRM, PowerMessage.BASE_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            switch (dresult)
            {
                case DialogResult.OK:
                    result = proxy.Service.DeleteFunc(func);
                    break;
                case DialogResult.Cancel:
                    return;
            }

            if (result)
            {
                sysToolBar.LogMessage = string.Format("删除成功,功能名称: {0}", funcName);
            }

            //删除记录后重新加载界面
            LoadData();
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.isActionSuccess = false;
            this.bsPower.EndEdit();

            if (txtFuncName.Text.Trim() == "")
            {
                lis.client.control.MessageDialog.Show(PowerMessage.BASE_FUNCNAME_NULL, PowerMessage.BASE_TITLE);
                txtFuncName.Focus();
                return;
            }

            if (bsPower.Current == null) return;
            EntitySysFunction func = bsPower.Current as EntitySysFunction;
            func.FuncValiuser = ckValidateUser.Checked == true ? "1" : "0";
            func.FuncParentId = int.Parse(txtParentId.Text);
            func.FuncType = cbxFuncType.EditValue.ToString();
            func.FuncCode = txtFuncCode.Text;

            if (optionStatus == OptionStatus.Insert)
            {
                try
                {
                    proxy.Service.InsertAFunc(func);
                    this.isActionSuccess = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    this.isActionSuccess = false;
                }

            }
            else
            {
                try
                {
                    proxy.Service.UpdateAFunc(func);
                    this.isActionSuccess = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    this.isActionSuccess = false;
                }
            }

            if (this.isActionSuccess)
            {
                sysToolBar.LogMessage = string.Format("保存成功,功能名称: {0}", txtFuncName.Text.Trim());
            }

            LoadData();
        }

        int focusindex = -1;
        /// <summary>
        /// 放弃
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (optionStatus == OptionStatus.Insert)
            {
                bsPower.RemoveCurrent();
            }
            bsPower.ResetBindings(true);
            //tvPower_FocusedNodeChanged(null, null);
            LoadData();
        }
        int funid = -1;
        /// <summary>
        /// 选择节点时显示具体内容和重置状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvPower_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (bsPower.Current == null) return;
            EntitySysFunction func = bsPower.Current as EntitySysFunction;
            if (func.FuncId == 0 || func.FuncId == funid)
            {
                return;
            }
            funid = func.FuncId;
            sysToolBar.EnableButton(false);
            txtParentId.Text = func.FuncParentId.ToString();
            cbxFuncType.EditValue = func.FuncType;
            txtFuncCode.EditValue = func.FuncCode;

            if (tvPower.Selection.Count > 0)
            {
                optionStatus = OptionStatus.Update;
                EnterEditingState(false);

                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tvPower.Selection[0];
                if (tn.GetValue(colValidateUser) != null)
                {
                    string strValidateUser = tn.GetValue(colValidateUser).ToString();
                    if (strValidateUser.Trim() == "1")
                        ckValidateUser.Checked = true;
                    else
                        ckValidateUser.Checked = false;
                }
            }
        }

        /// <summary>
        /// 刷新按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// 重新读取数据
        /// </summary>
        private void LoadData()
        {
            optionStatus = OptionStatus.Update;
            EnterEditingState(false);

            list = proxy.Service.GetFuncList();
            bsPower.DataSource = list;
            tvPower.Refresh();
            tvPower.CollapseAll();
        }

        private void ckValidateUser_CheckedChanged(object sender, EventArgs e)
        {
            if (bsPower.Current == null) return;
            EntitySysFunction func = bsPower.Current as EntitySysFunction;
            if (Convert.ToInt32(func.FuncParentId) < 0 && ckValidateUser.Checked)
            {
                ckValidateUser.Checked = false;
                lis.client.control.MessageDialog.ShowAutoCloseDialog("父节点无法设置验证用户！");
            }
        }

    }
}
