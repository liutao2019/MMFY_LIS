using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Lib.DataInterface.Implement
{
    public partial class ctrlDataInterfaceConnectionEditor : UserControl
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

        /// <summary>
        /// .ctor
        /// </summary>
        public ctrlDataInterfaceConnectionEditor()
        {
            InitializeComponent();

            CloseEditMode();

            this.DataAccessMode = EnumDataAccessMode.DirectToDB;
        }

        /// <summary>
        /// .load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctrlDataInterfaceConnectionEditor_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                this.txtType.Items.AddRange(Enum.GetNames(typeof(EnumDataInterfaceConnectionType)));
                txtType_SelectedIndexChanged(this.txtType, EventArgs.Empty);

                this.txtRunningSide.Items.AddRange(Enum.GetNames(typeof(EnumDeploymentMode)));

                RefreshList();
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

            this.btnImport.Enabled = false;

            this.splitContainer1.Panel2.Enabled = true;
            this.gridList.Enabled = false;


            //系统预置的，不能编辑编码与名称
            if (this.bsList.Current != null)
            {
                EntityDictDataInterfaceConnection conn = this.bsList.Current as EntityDictDataInterfaceConnection;
                this.txtCode.Enabled = !(conn.sys_default == 1);
                this.txtName.Enabled = !(conn.sys_default == 1);
            }
            else
            {
                this.txtCode.Enabled = true;
                this.txtName.Enabled = true;
            }
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
        }

        private void txtType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = this.txtType.Text.ToLower();
            if (type == EnumDataInterfaceConnectionType.SQL.ToString().ToLower())
            {
                this.lblAddress.Text = "数据库地址：";
                this.txtAddress.Enabled = true;

                this.lblCatelog.Text = "数据库名称：";

                this.lblDriver.Text = "数据驱动：";
                this.lblDialet.Text = "数据库类别：";

                this.txtCatelog.Enabled = true;
                this.txtLogin.Enabled = true;
                this.txtLoginPass.Enabled = true;

                this.lblCatelog.Text = "数据库";
                this.lblLogin.Text = "登录名";
                this.lblLoginPass.Text = "密码";

                this.txtDialet.Enabled = true;
                this.txtDriver.Enabled = true;

                this.txtDriver.Items.Clear();
                this.txtDialet.Items.Clear();

                this.txtDriver.DropDownStyle = ComboBoxStyle.DropDownList;
                this.txtDialet.DropDownStyle = ComboBoxStyle.DropDownList;

                this.txtDriver.Items.AddRange(Enum.GetNames(typeof(Lib.DAC.EnumDbDriver)));
                this.txtDialet.Items.AddRange(Enum.GetNames(typeof(Lib.DAC.EnumDataBaseDialet)));
            }
            else if (type == "wcf" || type == "webservice")
            {
                this.lblAddress.Text = "wsdl地址：";

                this.txtDialet.Enabled = false;
                this.txtDriver.Enabled = false;
                this.txtCatelog.Enabled = false;
                this.txtLogin.Enabled = false;
                this.txtLoginPass.Enabled = false;

                this.lblDialet.Text = string.Empty;
                this.lblDriver.Text = string.Empty;
                this.lblCatelog.Text = string.Empty;
                this.lblLogin.Text = string.Empty;
                this.lblLoginPass.Text = string.Empty;

            }
            else if (type == EnumDataInterfaceConnectionType.DotNetDll.ToString().ToLower())
            {
                this.txtAddress.Enabled = true;
                this.txtDialet.Enabled = false;
                this.txtDriver.Enabled = false;
                this.txtCatelog.Enabled = true;
                this.txtLogin.Enabled = false;
                this.txtLoginPass.Enabled = false;

                this.lblAddress.Text = "dll路径：";
                this.lblDialet.Text = string.Empty;
                this.lblDriver.Text = string.Empty;
                this.lblCatelog.Text = "类全名：";
                this.lblLogin.Text = string.Empty;
                this.lblLoginPass.Text = string.Empty;
            }
            else if (type == EnumDataInterfaceConnectionType.BiniaryDll.ToString().ToLower())
            {
                this.txtAddress.Enabled = true;
                this.txtDialet.Enabled = false;
                this.txtDriver.Enabled = false;
                this.txtCatelog.Enabled = false;
                this.txtLogin.Enabled = false;
                this.txtLoginPass.Enabled = false;

                this.lblAddress.Text = "dll路径：";
                this.lblDialet.Text = string.Empty;
                this.lblDriver.Text = string.Empty;
                this.lblCatelog.Text = string.Empty;
                this.lblLogin.Text = string.Empty;
                this.lblLoginPass.Text = string.Empty;
            }
            else if (type == EnumDataInterfaceConnectionType.DOSCommand.ToString().ToLower())
            {
                this.txtAddress.Enabled = false;
                this.txtDialet.Enabled = false;
                this.txtDriver.Enabled = false;
                this.txtCatelog.Enabled = false;
                this.txtLogin.Enabled = false;
                this.txtLoginPass.Enabled = false;

                this.lblAddress.Text = string.Empty;
                this.lblDialet.Text = string.Empty;
                this.lblDriver.Text = string.Empty;
                this.lblCatelog.Text = string.Empty;
                this.lblLogin.Text = string.Empty;
                this.lblLoginPass.Text = string.Empty;
            }
        }

        void BindUI(EntityDictDataInterfaceConnection obj)
        {
            //if (obj == null)
            //{
            //    this.bsItem.Clear();
            //}
            //else
            //{
            this.bsItem.DataSource = obj;
            this.bsItem.ResetBindings(false);
            //}
        }

        /// <summary>
        /// 列表选中项目改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bsList_CurrentChanged(object sender, EventArgs e)
        {
            EntityDictDataInterfaceConnection obj = this.bsList.Current as EntityDictDataInterfaceConnection;
            this.bsItem.DataSource = obj;
            this.btnModify.Enabled = obj != null;
            this.btnDelete.Enabled = obj != null;
        }

        #region 操作事件
        /// <summary>
        /// 新增
        /// </summary>
        public void AddNew()
        {
            AddNew(null);
        }

        private void AddNew(EntityDictDataInterfaceConnection objToClone)
        {
            EntityDictDataInterfaceConnection newObj = this.bsList.AddNew() as EntityDictDataInterfaceConnection;

            if (objToClone != null)
            {
                EntityDictDataInterfaceConnection.PropertiesClone(objToClone, newObj);
                newObj.conn_id = null;
                newObj.conn_code = null;
                newObj.conn_name = "复制_" + newObj.conn_name;
            }
            else
            {
                newObj.sys_module = this.DefaultModuleName;
            }

            BindUI(newObj);

            this.EnterEditMode();

            if (objToClone != null)
            {
                this.txtCode.Focus();
            }
        }

        /// <summary>
        /// 删除当前选中记录
        /// </summary>
        public void DeleteCurrent()
        {
            EntityDictDataInterfaceConnection obj = this.bsList.Current as EntityDictDataInterfaceConnection;

            if (obj == null)
                MessageBox.Show("请选择要删除的记录", "提示");

            if (obj.sys_default == 1)
            {
                MessageBox.Show("系统预置数据不能删除", "提示");
                return;
            }


            if (MessageBox.Show(string.Format("你确定要删除当前选中的【{0}】这条记录吗？\r\n删除后将不可恢复", obj.conn_name)
                , "提示"
                , MessageBoxButtons.YesNo
                , MessageBoxIcon.Warning
                , MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            dac.ConnectionDelete(obj);

            RefreshList();
        }

        /// <summary>
        /// 加载列表
        /// </summary>
        public void RefreshList()
        {
            this.bsList.DataSource = dac.GetConnections(this.DefaultModuleName);
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            this.bsItem.EndEdit();
            this.bsList.EndEdit();

            if (!BeforeSaveCheck())
                return;

            EntityDictDataInterfaceConnection obj = this.bsItem.Current as EntityDictDataInterfaceConnection;

            dac.ConnectionSave(obj);

            CloseEditMode();
        }

        /// <summary>
        /// 保存前检查
        /// </summary>
        /// <returns></returns>
        private bool BeforeSaveCheck()
        {
            if (this.txtName.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入名称", "提示");
                this.txtName.Focus();
                return false;
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
                this.bsList.CancelEdit();
                CloseEditMode();
            }
        }

        #endregion

        /// <summary>
        /// bsList AddingNew事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bsList_AddingNew(object sender, AddingNewEventArgs e)
        {
            EntityDictDataInterfaceConnection conn = new EntityDictDataInterfaceConnection();
            e.NewObject = conn;
            conn.conn_db_dialet = Lib.DAC.EnumDataBaseDialet.SQL2005.ToString();
            conn.conn_db_driver = Lib.DAC.EnumDbDriver.MSSql.ToString();
            conn.conn_type = EnumDataInterfaceConnectionType.SQL.ToString();
        }

        #region 按钮事件
        /// <summary>
        /// 新增按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            if (this.bsItem.Current == null)
            {
                MessageBox.Show("请选择一条记录", "提示");
                return;
            }

            EntityDictDataInterfaceConnection dtoConn = this.bsItem.Current as EntityDictDataInterfaceConnection;

            string msg = dac.TestConnection(dtoConn);

            if (string.IsNullOrEmpty(msg))
            {
                MessageBox.Show("连接成功", "提示");
            }
            else
            {
                MessageBox.Show(msg, "提示");
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (this.bsItem.Current == null)
            {
                MessageBox.Show("请选择一条记录", "提示");
                return;
            }

            EntityDictDataInterfaceConnection obj = this.bsList.Current as EntityDictDataInterfaceConnection;

            this.AddNew(obj);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (this.bsItem.Current == null)
            {
                MessageBox.Show("请选择一条记录", "提示");
                return;
            }

            EntityDictDataInterfaceConnection obj = this.bsList.Current as EntityDictDataInterfaceConnection;

            EntityDictDataInterfaceConnection clonedObj = obj.Clone() as EntityDictDataInterfaceConnection;

            SaveFileDialog diag = new SaveFileDialog();
            diag.Filter = "xml文件|*.xml";
            diag.FileName = "数据源_" + clonedObj.conn_name + ".xml";
            if (diag.ShowDialog() == DialogResult.OK)
            {
                clonedObj.conn_id = null;
                clonedObj.sys_default = 0;
                string strXML = Lib.EntityCore.EntityXMLConverter.EntityToXMLString<EntityDictDataInterfaceConnection>(clonedObj);
                using (System.IO.StreamWriter sw = System.IO.File.CreateText(diag.FileName))
                {
                    sw.WriteLine(strXML);
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Filter = "xml文件|*.xml";
            if (diag.ShowDialog() == DialogResult.OK)
            {
                string strXML = System.IO.File.ReadAllText(diag.FileName);

                try
                {
                    EntityDictDataInterfaceConnection obj = Lib.EntityCore.EntityXMLConverter.XMLStringToEntity<EntityDictDataInterfaceConnection>(strXML);
                    AddNew(obj);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导入失败\r\n" + ex.Message, "提示");
                }
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Control parent = this.Parent;
            while (!(parent is Form) && parent != null)
            {
                parent = parent.Parent;
            }

            if (parent != null)
            {
                if (this.btnSave.Enabled)
                {
                    DialogResult dResult = MessageBox.Show("当前未保存，是否保存后再退出？\r\n【是】保存并退出\r\n【否】不保存直接退出\r\n【取消】取消当前操作", "提示", MessageBoxButtons.YesNoCancel);
                    if (dResult == DialogResult.Yes)
                    {
                        this.Save();
                    }

                    if (dResult == DialogResult.Yes
                           || dResult == DialogResult.No)
                    {
                        (parent as Form).Close();
                    }
                    else if (dResult == DialogResult.Cancel)
                    {

                    }
                }
                else
                {
                    //if (e.CloseReason == CloseReason.UserClosing)
                    //{
                    if (MessageBox.Show("是否关闭当前窗口？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        (parent as Form).Close();
                    }
                    //}
                }

            }
        }
        #endregion
    }
}
