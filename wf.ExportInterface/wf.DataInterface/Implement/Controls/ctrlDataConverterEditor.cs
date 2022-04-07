using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Lib.DataInterface.Implement
{
    public partial class ctrlDataConverterEditor : UserControl
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public ctrlDataConverterEditor()
        {
            InitializeComponent();

            CloseEditMode();
        }

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

        #endregion

        private void ctrlDataConverterEditor_Load(object sender, EventArgs e)
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
            #region 转换类型
            this.txtRuleType.DisplayMember = "display";
            this.txtRuleType.ValueMember = "value";

            this.colRuleType.DisplayMember = "display";
            this.colRuleType.ValueMember = "value";

            this.txtRuleType.DataSource = ControlBindingData.GetRuleTypeSelectItems().DefaultView;
            this.colRuleType.DataSource = ControlBindingData.GetRuleTypeSelectItems().DefaultView;
            #endregion

            #region 数据类型
            this.txtSrcDataType.DisplayMember = "display";
            this.txtSrcDataType.ValueMember = "value";

            this.txtDestDataType.DisplayMember = "display";
            this.txtDestDataType.ValueMember = "value";

            this.txtSrcDataType.DataSource = ControlBindingData.GetSupportedNetTypeNames().DefaultView;
            this.txtDestDataType.DataSource = ControlBindingData.GetSupportedNetTypeNames().DefaultView;
            #endregion
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

            this.btnImport.Enabled = false;

            this.splitContainer1.Panel2.Enabled = true;
            //this.gridList.Enabled = false;
            this.gridContrast.ReadOnly = false;
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

            this.btnImport.Enabled = true;

            this.splitContainer1.Panel2.Enabled = false;
            this.gridList.Enabled = true;
            this.gridContrast.ReadOnly = true;
        }

        #region 操作事件
        /// <summary>
        /// 加载列表
        /// </summary>
        public void RefreshList()
        {
            List<EntityDictDataConverter> list = dac.GetConverters(this.DefaultModuleName);

            this.bsListRule.DataSource = list;
        }

        /// <summary>
        /// 新增
        /// </summary>
        public void AddNew()
        {
            AddNew(null);
        }

        private void AddNew(EntityDictDataConverter objToClone)
        {
            EntityDictDataConverter newObj = this.bsListRule.AddNew() as EntityDictDataConverter;

            if (objToClone != null)
            {
                EntityDictDataConverter.PropertiesClone(objToClone, newObj);
                newObj.rule_id = null;
            }
            else
            {
                newObj.sys_module = this.DefaultModuleName;
            }

            BindCmdItem(newObj);

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
            this.bsItem.EndEdit();
            this.gridContrast.EndEdit();
            this.bsListContrast.EndEdit();

            EntityDictDataConverter objConverter = this.bsItem.Current as EntityDictDataConverter;

            List<EntityDictDataConvertContrast> listContrast = this.bsListContrast.DataSource as List<EntityDictDataConvertContrast>;

            if (!BeforeSaveCheck(objConverter, listContrast))
                return;

            dac.ConverterSave(objConverter, listContrast.ToArray());
            CloseEditMode();
        }

        /// <summary>
        /// 保存前检查
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="listParam"></param>
        /// <returns></returns>
        bool BeforeSaveCheck(EntityDictDataConverter cmd, List<EntityDictDataConvertContrast> listParam)
        {
            if (this.txtName.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入名称", "提示");
                this.txtName.Focus();
                return false;
            }

            //if (this.txtConnection.SelectedItem == null)
            //{
            //    MessageBox.Show("请选择接口连接", "提示");
            //    this.txtConnection.Focus();
            //    return false;
            //}

            //if (this.txtCmdRunningSide.SelectedItem == null)
            //{
            //    MessageBox.Show("请选择运行端", "提示");
            //    this.txtCmdRunningSide.Focus();
            //    return false;
            //}

            //if (this.txtCmdType.SelectedItem == null)
            //{
            //    MessageBox.Show("请类型", "提示");
            //    this.txtCmdType.Focus();
            //    return false;
            //}

            //if (this.txtCmdExeType.SelectedItem == null)
            //{
            //    MessageBox.Show("请选择执行方式", "提示");
            //    this.txtCmdExeType.Focus();
            //    return false;
            //}

            if (listParam != null)
            {
                foreach (EntityDictDataConvertContrast p in listParam)
                {
                    //if (string.IsNullOrEmpty(p.param_name))
                    //{
                    //    MessageBox.Show("[参数名]不能为空", "提示");
                    //    return false;
                    //}

                    //if (string.IsNullOrEmpty(p.param_datatype))
                    //{
                    //    MessageBox.Show("参数[数据类型]不能为空", "提示");
                    //    return false;
                    //}
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
                this.bsListRule.CancelEdit();
                this.bsListContrast.CancelEdit();
                CloseEditMode();
            }
        }

        /// <summary>
        /// 删除当前选中记录
        /// </summary>
        public void DeleteCurrent()
        {
            EntityDictDataConverter objConverter = this.bsListRule.Current as EntityDictDataConverter;

            if (objConverter == null)
                MessageBox.Show("请选择要删除的记录", "提示");

            if (objConverter.sys_default)
            {
                MessageBox.Show("系统预置数据不能删除", "提示");
                return;
            }


            if (MessageBox.Show(string.Format("你确定要删除当前选中的【{0}】这条记录吗？\r\n删除后将不可恢复", objConverter.rule_name)
                , "提示"
                , MessageBoxButtons.YesNo
                , MessageBoxIcon.Warning
                , MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            dac.ConverterDelete(objConverter);

            RefreshList();
        }
        #endregion

        /// <summary>
        /// 绑定界面
        /// </summary>
        /// <param name="cmd"></param>
        void BindCmdItem(EntityDictDataConverter cmd)
        {
            this.bsItem.DataSource = cmd;
            this.bsItem.ResetBindings(false);
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
            //TestCommand();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {

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

        /// <summary>
        /// 选择的命令改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bsListRule_CurrentChanged(object sender, EventArgs e)
        {

            if (this.bsListRule.Current != null)
            {
                EntityDictDataConverter converter = this.bsListRule.Current as EntityDictDataConverter;

                //绑定界面
                BindCmdItem(converter);

                this.bsListContrast.DataSource = dac.GetConverterContrastsByConverterID(converter.rule_id);

            }
        }

        /// <summary>
        /// bindingsource，命令添加对象事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bsListRule_AddingNew(object sender, AddingNewEventArgs e)
        {
            EntityDictDataConverter rule = new EntityDictDataConverter();

            rule.rule_type = EnumDataInterfaceConverterType.Contrast.ToString();
            rule.sys_default = false;
            rule.rule_dest_datatype = typeof(string).FullName;
            rule.rule_seq = 0;
            rule.sys_module = this.DefaultModuleName;
            e.NewObject = rule;
        }

        /// <summary>
        /// bindingsource，参数添加对象事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bsListContrast_AddingNew(object sender, AddingNewEventArgs e)
        {
            EntityDictDataConvertContrast contrast = new EntityDictDataConvertContrast();

            //EntityDictDataInterfaceConnection conn = this.txtConnection.SelectedItem as EntityDictDataInterfaceConnection;
            //if (conn != null)
            //{
            //    if (string.IsNullOrEmpty(conn.conn_type))
            //    {
            //        param.param_datatype = "";
            //    }
            //    else if (conn.conn_type.ToLower() == "sql")
            //    {
            //        param.param_datatype = DbType.AnsiString.ToString();
            //    }
            //    else
            //    {
            //        param.param_datatype = typeof(string).FullName;
            //    }
            //}
            //param.param_direction = EnumDataInterfaceParameterDirection.Input.ToString();
            //param.param_enabled = true;
            //param.param_isbound = false;

            List<EntityDictDataConvertContrast> list = this.bsListContrast.DataSource as List<EntityDictDataConvertContrast>;

            if (list == null)
                list = new List<EntityDictDataConvertContrast>();

            if (list.Count == 0)
            {
                contrast.con_seq = 0;
            }
            else
            {
                int max = int.MinValue;
                foreach (EntityDictDataConvertContrast item in list)
                {
                    if (item.con_seq >= max)
                        max = item.con_seq;
                }
                contrast.con_seq = max + 10;
            }
            e.NewObject = contrast;
        }

        private void txtRuleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.txtRuleType.SelectedValue != null)
            {
                string selectType = this.txtRuleType.SelectedValue.ToString();

                if (selectType == EnumDataInterfaceConverterType.Contrast.ToString())
                {
                    this.splitContainer2.Panel2Collapsed = false;
                    this.pnlDataType.Visible = true;
                    this.pnlRefID.Visible = false;
                }
                else if (selectType == EnumDataInterfaceConverterType.DataInterface.ToString())
                {
                    this.splitContainer2.Panel2Collapsed = true;
                    this.pnlDataType.Visible = false;
                    this.pnlRefID.Visible = true;
                    this.lblRefID.Text = "接   口：";
                }
                else if (selectType == EnumDataInterfaceConverterType.Script.ToString())
                {
                    this.splitContainer2.Panel2Collapsed = true;
                    this.pnlDataType.Visible = false;
                    this.pnlRefID.Visible = true;
                    this.lblRefID.Text = "脚   本：";
                }
            }
        }
    }
}
