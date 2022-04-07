using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Lib.DataInterface.Implement
{
    public partial class ctrlDataInterfaceCommandEditor : UserControl
    {
        #region Property
        /// <summary>
        /// 是否显示工具栏
        /// </summary>
        public bool ShowMenubar
        {
            get
            {
                return this.toolStrip1.Visible;
            }
            set
            {
                this.toolStrip1.Visible = value;
            }
        }

        /// <summary>
        /// 默认模块名称
        /// </summary>
        public string DefaultModuleName { get; set; }

        /// <summary>
        /// 数据访问方式：直连数据库；自定义
        /// </summary>
        public EnumDataAccessMode DataAccessMode
        {
            get
            {
                return dac.DataAccessMode;
            }
            set
            {
                dac.DataAccessMode = value;
            }
        }

        private DACManager dac = new DACManager(EnumDataAccessMode.DirectToDB, false);

        /// <summary>
        /// 是否能编辑模块名
        /// </summary>
        public bool CanEditModuleName
        {
            get
            {
                return this.pnlModuleName.Visible;
            }
            set
            {
                this.pnlModuleName.Visible = value;
            }
        }

        #endregion

        /// <summary>
        /// .ctor
        /// </summary>
        public ctrlDataInterfaceCommandEditor()
        {
            InitializeComponent();
            this.DataAccessMode = EnumDataAccessMode.DirectToDB;
            CloseEditMode();
        }

        /// <summary>
        /// .load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctrlDataInterfaceCommandEditor_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                RefreshUIData();
                RefreshList();
            }
        }

        /// <summary>
        /// 初始化界面数据
        /// </summary>
        private void RefreshUIData()
        {
            #region 执行方式
            this.colFetchType.DisplayMember = "display";
            this.colFetchType.ValueMember = "value";

            this.txtCmdExeType.DisplayMember = "display";
            this.txtCmdExeType.ValueMember = "value";

            this.bsExecuteType.DataSource = ControlBindingData.GetCommandExecuteTypeSelectItems().DefaultView;
            this.txtCmdExeType.DataSource = this.bsExecuteType.DataSource;
            #endregion

            #region 数据连接
            //this.txtConnection.DisplayMember = "conn_name";
            //this.txtConnection.ValueMember = "conn_id";

            List<EntityDictDataInterfaceConnection> listConn = dac.GetConnections(this.DefaultModuleName);
            listConn.Insert(0, new EntityDictDataInterfaceConnection());
            this.bsConnection.DataSource = listConn;
            this.txtConnection.DataSource = bsConnection;
            #endregion

            #region 运行端
            this.txtCmdRunningSide.DisplayMember = "display";
            this.txtCmdRunningSide.ValueMember = "value";

            this.bsRunningSide.DataSource = ControlBindingData.GetRunningSideSelectItems().DefaultView;
            this.txtCmdRunningSide.DataSource = this.bsRunningSide;
            #endregion

            #region 命令类型
            this.txtCmdType.DisplayMember = "display";
            this.txtCmdType.ValueMember = "value";

            this.bsCommandType.DataSource = ControlBindingData.GetCommandTypeSelectItems().DefaultView;
            this.txtCmdType.DataSource = this.bsCommandType;
            #endregion

            #region 参数方向
            this.colParamDirection.DisplayMember = "display";
            this.colParamDirection.ValueMember = "value";
            this.colParamDirection.DataSource = ControlBindingData.GetParameterDirectionSelectItems().DefaultView;
            #endregion

            #region 参数数据类型
            this.colParamDataType.DisplayMember = "display";
            this.colParamDataType.ValueMember = "value";
            #endregion

            #region 参数转换规则

            this.colParamConverter.DisplayMember = "rule_name";
            this.colParamConverter.ValueMember = "rule_id";

            //暂时注释
            //List<EntityDictDataConverter> listConverter = dac.GetConverters(this.DefaultModuleName);
            List<EntityDictDataConverter> listConverter = new List<EntityDictDataConverter>();

            listConverter.Insert(0, new EntityDictDataConverter());
            this.colParamConverter.DataSource = listConverter;

            #endregion
        }

        public void SetGroupFieldStyle(string[] selectItems, string defaultValue, bool allowEdit, bool allowEmpty)
        {
            this.txtCmdGroup.Items.Clear();
            if (selectItems == null || selectItems.Length == 0)
            {
                txtCmdGroup.DropDownStyle = ComboBoxStyle.Simple;
            }
            else
            {
                if (allowEdit)
                {
                    txtCmdGroup.DropDownStyle = ComboBoxStyle.DropDown;
                }
                else
                {
                    txtCmdGroup.DropDownStyle = ComboBoxStyle.DropDownList;
                }

                if (allowEmpty)
                    txtCmdGroup.Items.Add(string.Empty);

                foreach (string valItem in selectItems)
                {
                    txtCmdGroup.Items.Add(valItem);
                }

                txtCmdGroup.Items.Add(ControlBindingData.COMMAND_GROUP_CONVERTER);

                txtCmdGroup.SelectedItem = defaultValue;
            }
        }

        Dictionary<string, string[]> CommandGroupNParamBindField = null;
        public void SetGroupFieldStyle(Dictionary<string, string[]> selectItems, string defaultValue, bool allowEdit, bool allowEmpty)
        {
            SetGroupFieldStyle((new List<string>(selectItems.Keys)).ToArray(), defaultValue, allowEdit, allowEmpty);

            if (selectItems.Count > 0)
            {
                CommandGroupNParamBindField = selectItems;
            }
            else
            {
                CommandGroupNParamBindField = null;
            }
        }


        /// <summary>
        /// 进入编辑状态
        /// </summary>
        private void EnterEditMode()
        {
            this.btnAddNew.Enabled = false;
            this.btnModify.Enabled = false;
            this.btnDelete.Enabled = false;
            this.btnSave.Enabled = true;
            this.btnCancel.Enabled = true;
            this.btnRefresh.Enabled = false;
            this.btnTestCommand.Enabled = false;
            this.btnImport.Enabled = false;

            this.splitContainer1.Panel2.Enabled = true;
            this.gridList.Enabled = false;
            this.gridParameter.ReadOnly = false;
        }

        /// <summary>
        /// 退出编辑状态
        /// </summary>
        private void CloseEditMode()
        {
            this.btnAddNew.Enabled = true;
            this.btnModify.Enabled = true;
            this.btnDelete.Enabled = true;
            this.btnSave.Enabled = false;
            this.btnCancel.Enabled = false;
            this.btnRefresh.Enabled = true;
            this.btnTestCommand.Enabled = true;
            this.btnImport.Enabled = true;

            this.splitContainer1.Panel2.Enabled = false;
            this.gridList.Enabled = true;
            this.gridParameter.ReadOnly = true;
        }

        #region 按钮事件
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            AddNew();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            EnterEditMode();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteCurrent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelEdit();
            this.RefreshList();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void btnTestCommand_Click(object sender, EventArgs e)
        {
            TestCommand();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (this.bsCmdItem.Current == null)
            {
                MessageBox.Show("请选择一条记录", "提示");
                return;
            }

            EntityDictDataInterfaceCommand obj = this.bsListCommand.Current as EntityDictDataInterfaceCommand;
            List<EntityDictDataInterfaceCommandParameter> listParam = this.bsListParameter.DataSource as List<EntityDictDataInterfaceCommandParameter>;

            this.AddNew(obj, listParam);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {

        }

        private void btnImport_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region 操作事件
        /// <summary>
        /// 加载列表
        /// </summary>
        public void RefreshList()
        {
            List<EntityDictDataInterfaceCommand> list = dac.GetCommands(this.DefaultModuleName);
            this.bsListCommand.DataSource = list;

            this.txtFilterCMDGroup.Items.Clear();
            this.txtFilterCMDGroup.Items.Add(string.Empty);
            foreach (EntityDictDataInterfaceCommand item in list)
            {
                if (!this.txtFilterCMDGroup.Items.Contains(item.cmd_group))
                {
                    this.txtFilterCMDGroup.Items.Add(item.cmd_group);
                }
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        public void AddNew()
        {
            AddNew(null, null);
        }

        private void AddNew(EntityDictDataInterfaceCommand objToClone, List<EntityDictDataInterfaceCommandParameter> listParamToClone)
        {
            List<EntityDictDataInterfaceCommandParameter> newListParam = new List<EntityDictDataInterfaceCommandParameter>();
            if (listParamToClone != null)
            {
                foreach (EntityDictDataInterfaceCommandParameter pToClone in listParamToClone)
                {
                    EntityDictDataInterfaceCommandParameter newParam = pToClone.Clone() as EntityDictDataInterfaceCommandParameter;
                    newParam.cmd_id = null;

                    newListParam.Add(newParam);
                }
            }

            EntityDictDataInterfaceCommand newObj = this.bsListCommand.AddNew() as EntityDictDataInterfaceCommand;

            if (objToClone != null)
            {
                EntityDictDataInterfaceCommand.PropertiesClone(objToClone, newObj);
                newObj.cmd_id = null;
                newObj.cmd_name = "复制_" + newObj.cmd_name;
            }
            else
            {
                newObj.sys_module = this.DefaultModuleName;
            }

            BindCmdItem(newObj);

            this.bsListParameter.DataSource = newListParam;
            this.bsListParameter.ResetBindings(false);

            this.EnterEditMode();

            if (objToClone != null)
            {
                //this.txtCode.Focus();
            }
        }


        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            this.bsCmdItem.EndEdit();
            this.gridParameter.EndEdit();
            this.bsListParameter.EndEdit();

            EntityDictDataInterfaceCommand objCmd = this.bsCmdItem.Current as EntityDictDataInterfaceCommand;

            List<EntityDictDataInterfaceCommandParameter> listParam = this.bsListParameter.DataSource as List<EntityDictDataInterfaceCommandParameter>;

            if (!BeforeSaveCheck(objCmd, listParam))
                return;

            dac.CommandSave(objCmd, listParam.ToArray());

            CloseEditMode();
        }

        /// <summary>
        /// 保存前检查
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="listParam"></param>
        /// <returns></returns>
        bool BeforeSaveCheck(EntityDictDataInterfaceCommand cmd, List<EntityDictDataInterfaceCommandParameter> listParam)
        {
            if (this.txtName.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入[名称]", "提示");
                this.txtName.Focus();
                return false;
            }

            //if (this.txtConnection.SelectedItem == null)
            //{
            //    MessageBox.Show("请选择接口连接", "提示");
            //    this.txtConnection.Focus();
            //    return false;
            //}

            if (this.txtCmdRunningSide.SelectedItem == null)
            {
                MessageBox.Show("请选择[运行端]", "提示");
                this.txtCmdRunningSide.Focus();
                return false;
            }

            if (this.txtCmdType.SelectedItem == null)
            {
                MessageBox.Show("请选择[命令类型]", "提示");
                this.txtCmdType.Focus();
                return false;
            }

            if (this.txtCmdExeType.SelectedItem == null)
            {
                MessageBox.Show("请选择[执行方式]", "提示");
                this.txtCmdExeType.Focus();
                return false;
            }

            if (this.txtConnection.SelectedItem == null)
            {
                MessageBox.Show("请选择[连接]", "提示");
                this.txtConnection.Focus();
                return false;
            }

            if (listParam != null)
            {
                foreach (EntityDictDataInterfaceCommandParameter p in listParam)
                {
                    if (string.IsNullOrEmpty(p.param_name))
                    {
                        MessageBox.Show("[参数名]不能为空", "提示");
                        return false;
                    }

                    if (string.IsNullOrEmpty(p.param_datatype))
                    {
                        MessageBox.Show("参数[数据类型]不能为空", "提示");
                        return false;
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// 放弃编辑
        /// </summary>
        public void CancelEdit()
        {
            if (MessageBox.Show("你确定要放弃当前的修改吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.bsListCommand.CancelEdit();
                this.bsListParameter.CancelEdit();
                CloseEditMode();
            }
        }

        /// <summary>
        /// 删除当前选中记录
        /// </summary>
        public void DeleteCurrent()
        {
            EntityDictDataInterfaceCommand obj = this.bsListCommand.Current as EntityDictDataInterfaceCommand;

            if (obj == null)
                MessageBox.Show("请选择要删除的记录", "提示");

            if (obj.sys_default == 1)
            {
                MessageBox.Show("系统预置数据不能删除", "提示");
                return;
            }


            if (MessageBox.Show(string.Format("你确定要删除当前选中的【{0}】这条记录吗？\r\n删除后将不可恢复", obj.cmd_name)
                , "提示"
                , MessageBoxButtons.YesNo
                , MessageBoxIcon.Warning
                , MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            dac.CommandDelete(obj);

            RefreshList();
        }

        /// <summary>
        /// 测试命令
        /// </summary>
        public void TestCommand()
        {
            this.bsCmdItem.EndEdit();
            this.gridParameter.EndEdit();
            this.bsListParameter.EndEdit();

            EntityDictDataInterfaceCommand dtoCmd = bsListCommand.Current as EntityDictDataInterfaceCommand;
            if (dtoCmd == null)
            {
                MessageBox.Show("请选择一个接口命令", "提示");
                return;
            }

            if (string.IsNullOrEmpty(dtoCmd.conn_id))
            {
                MessageBox.Show("未选择接口连接", "提示");
                return;
            }

            this.gridParameter.EndEdit();

            EntityDictDataInterfaceConnection dtoConn = dac.GetConnectionByID(dtoCmd.conn_id);
            List<EntityDictDataInterfaceCommandParameter> dtoListParam = this.bsListParameter.DataSource as List<EntityDictDataInterfaceCommandParameter>;
            frmTestCommand frm = new frmTestCommand(dtoConn, dtoCmd, dtoListParam, this.dac);
            frm.Show();
        }

        #endregion

        /// <summary>
        /// 绑定界面
        /// </summary>
        /// <param name="cmd"></param>
        void BindCmdItem(EntityDictDataInterfaceCommand cmd)
        {
            this.bsCmdItem.DataSource = cmd;
            this.bsCmdItem.ResetBindings(false);
        }

        /// <summary>
        /// 选择的命令改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bsListCommand_CurrentChanged(object sender, EventArgs e)
        {

            if (this.bsListCommand.Current != null)
            {
                EntityDictDataInterfaceCommand cmd = this.bsListCommand.Current as EntityDictDataInterfaceCommand;

                //绑定界面
                BindCmdItem(cmd);

                //绑定参数
                this.bsListParameter.DataSource = dac.GetParametersByCmdID(cmd.cmd_id);
            }
        }

        /// <summary>
        /// 数据连接下拉改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtConnection_SelectedIndexChanged(object sender, EventArgs e)
        {
            EntityDictDataInterfaceConnection conn = this.txtConnection.SelectedItem as EntityDictDataInterfaceConnection;
            if (conn != null)
            {
                if (string.IsNullOrEmpty(conn.conn_type) || conn.conn_type.ToLower() == "sql")
                {
                    this.lblCommandText.Text = "sql语句";
                    this.colParamDataType.DataSource = ControlBindingData.GetSupportedDbTypeNames_tb().DefaultView;
                    colParamDataLength.Visible = true;
                }
                else
                {
                    this.lblCommandText.Text = "方法名";
                    this.colParamDataType.DataSource = ControlBindingData.GetSupportedNetTypeNames().DefaultView;
                    colParamDataLength.Visible = false;
                }
            }
            else
            {
                this.lblCommandText.Text = "执行命令";
                colParamDataLength.Visible = true;
            }
        }

        /// <summary>
        /// bindingsource，命令添加对象事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bsListCommand_AddingNew(object sender, AddingNewEventArgs e)
        {
            EntityDictDataInterfaceCommand cmd = new EntityDictDataInterfaceCommand();
            cmd.cmd_command_type = CommandType.Text.ToString();
            cmd.cmd_enabled = 1;
            cmd.cmd_enabled_log = 0;
            cmd.cmd_exec_seq = 0;
            cmd.cmd_fetch_type = EnumCommandExecuteType.ExecuteNonQuery.ToString();
            cmd.cmd_running_side = EnumDeploymentMode.Server.ToString();
            cmd.sys_module = this.DefaultModuleName;
            e.NewObject = cmd;
        }

        /// <summary>
        /// bindingsource，参数添加对象事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bsListParameter_AddingNew(object sender, AddingNewEventArgs e)
        {
            EntityDictDataInterfaceCommandParameter param = new EntityDictDataInterfaceCommandParameter();

            EntityDictDataInterfaceConnection conn = this.txtConnection.SelectedItem as EntityDictDataInterfaceConnection;
            if (conn != null)
            {
                if (string.IsNullOrEmpty(conn.conn_type))
                {
                    param.param_datatype = "";
                }
                else if (conn.conn_type.ToLower() == "sql")
                {
                    param.param_datatype = DbType.AnsiString.ToString();
                }
                else
                {
                    param.param_datatype = typeof(string).FullName;
                }
            }
            param.param_direction = EnumDataInterfaceParameterDirection.Input.ToString();
            param.param_enabled = 1;
            param.param_isbound = 0;

            List<EntityDictDataInterfaceCommandParameter> list = this.bsListParameter.DataSource as List<EntityDictDataInterfaceCommandParameter>;

            if (list == null)
                list = new List<EntityDictDataInterfaceCommandParameter>();

            if (list.Count == 0)
            {
                param.param_seq = 0;
            }
            else
            {
                int max = int.MinValue;
                foreach (EntityDictDataInterfaceCommandParameter item in list)
                {
                    if (item.param_seq >= max)
                        max = item.param_seq;
                }
                param.param_seq = ((max + 10) / 10) * 10;
            }
            e.NewObject = param;
        }

        private void gridList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > gridList.Rows.Count - 1)
            {
                return;
            }

            DataGridViewRow dgr = gridList.Rows[e.RowIndex];
            DataGridViewCheckBoxCell cellSysDefault = dgr.Cells["colCmdSysDefault"] as DataGridViewCheckBoxCell;

            if (Convert.ToInt32(cellSysDefault.Value) == 1)
            {
                e.CellStyle.ForeColor = Color.Red;
            }
            else
            {
            }

            DataGridViewCheckBoxCell cellEnabled = dgr.Cells["colCmdEnabled"] as DataGridViewCheckBoxCell;
            if (Convert.ToInt32(cellEnabled.Value) == 0)
            {
                e.CellStyle.ForeColor = Color.Gray;
            }
        }

        private void gridParameter_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex > gridList.Rows.Count - 1)
            {
                return;
            }

            DataGridViewRow dgr = gridParameter.Rows[e.RowIndex];
            DataGridViewCheckBoxCell cellEnabled = dgr.Cells["colParamEnabled"] as DataGridViewCheckBoxCell;
            if (Convert.ToInt32(cellEnabled.Value) == 0)
            {
                e.CellStyle.ForeColor = Color.Gray;
            }
        }

        private void gridParameter_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void txtCmdGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string group = this.txtCmdGroup.Text;

            //List<EntityDictDataInterfaceCommandParameter> listParam = this.bsListParameter.DataSource as List<EntityDictDataInterfaceCommandParameter>;

            //if (CommandGroupNParamBindField != null)
            //{
            //    List<string> items = new List<string>();

            //    items.Add(string.Empty);

            //    if (!string.IsNullOrEmpty(group))
            //    {
            //        foreach (string item in CommandGroupNParamBindField[group])
            //        {
            //            items.Add(item);
            //        }
            //    }

            //    if (listParam != null)
            //    {
            //        foreach (EntityDictDataInterfaceCommandParameter p in listParam)
            //        {
            //            if (!items.Contains(p.param_databind))
            //            {
            //                p.param_databind = string.Empty;
            //            }
            //        }
            //        this.bsListParameter.ResetBindings(false);
            //    }

            //目前改为txt编辑
            //colParamDataBind.DataSource = items.ToArray();
            //}
        }

        private void gridParameter_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewTextBoxCell dgvTextBoxCell = new DataGridViewTextBoxCell();

            //this.dataGridView1.Rows[dt.Rows.Count - 1].Cells["GenderID"] = dgvTextBoxCell;
            //dgvTextBoxCell.Value = "总计";

            DataGridViewCell cell = gridParameter.CurrentCell;
            if (cell.OwningColumn.DataPropertyName == "param_databind")
            {
                DataGridViewComboBoxEditingControl cb = e.Control as DataGridViewComboBoxEditingControl;
                if (cb != null)
                {
                    object objVal = gridParameter.CurrentRow.Cells[gridParameter.Columns["Column5"].Index].Value;
                    bool val = Convert.ToBoolean(objVal);

                    if (val == false)
                    {
                        //    cb.DropDownStyle = ComboBoxStyle.Simple;
                        //    object o = cb.DataSource;
                    }
                    else
                    {
                        //cb.DropDownStyle = ComboBoxStyle.DropDown;
                    }
                }
            }


            //DataGridViewComboBoxEditingControl cb = e.Control as DataGridViewComboBoxEditingControl;
            //if (cb != null)
            //{
            //object objVal = gridParameter.CurrentRow.Cells[gridParameter.Columns["colParamDataBind"].Index].Value;

            //gridParameter.
            //cb.DropDownStyle
            //colParamDataBind.control
            //}
        }

        private void txtFilterCMDGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

            string selectedGroup = this.txtFilterCMDGroup.SelectedItem.ToString();

        }

        //private void gridParameter_CellEnter(object sender, DataGridViewCellEventArgs e)
        //{
        //DataGridViewColumn col = gridParameter.Columns[e.ColumnIndex];
        //if (col.DataPropertyName == "param_databind")
        //{
        //    DataGridViewCell cell = gridParameter.Rows[e.RowIndex].Cells["Column5"];
        //    bool val = Convert.ToBoolean(cell.Value);

        //    if (val == false)
        //    {
        //        colParamDataBind.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
        //    }
        //    else
        //    {
        //        colParamDataBind.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
        //    }
        //}
        //}
    }
}
