using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.wcf;
using dcl.entity;
using DevExpress.XtraEditors;
using lis.client.control;
using dcl.common;
using Lib.LogManager;

namespace dcl.client.interfaces
{
    [DesignTimeVisible(false)]
    public partial class CommandNew : ConDicCommon
    {
        public CommandNew()
        {
            InitializeComponent();
            SetGroupFieldStyle(null, true, true);
        }

        enum SaveAction
        {
            Add,
            Edit,
            Unknown
        }

        private SaveAction saveAction = SaveAction.Unknown;
        ProxyDataInterfaceConnection proxyConn = new ProxyDataInterfaceConnection();
        List<EntityDicDataInterfaceConnection> listConn = new List<EntityDicDataInterfaceConnection>();
        ProxyDataInterfaceCommand proxy = new ProxyDataInterfaceCommand();
        List<EntityDicDataInterfaceCommand> listInterCommand = new List<EntityDicDataInterfaceCommand>();

        private void CommandNew_Load(object sender, EventArgs e)
        {
            //初始化工具条
            sysToolBar1.BtnCopy.Caption = "复制";
            sysToolBar1.BtnDeRef.Caption = "帮助";
            sysToolBar1.SetToolButtonStyle(new string[] {
                sysToolBar1.BtnAdd.Name, sysToolBar1.BtnModify.Name, sysToolBar1.BtnDelete.Name, sysToolBar1.BtnSave.Name, sysToolBar1.BtnCancel.Name,
                sysToolBar1.BtnRefresh.Name,sysToolBar1.BtnCopy.Name,sysToolBar1.BtnExport.Name,sysToolBar1.BtnImport.Name,sysToolBar1.BtnDeRef.Name,
                sysToolBar1.BtnClose.Name });

            RefreshUIData(); //初始化控件数据 
            DoRefresh(); //刷新

            EnableBaseInfo(false);
        }

        protected void EnableBaseInfo(bool enable)
        {
            foreach (Control item in groupBox1.Controls)
            {
                if (!(item is Label))
                {
                    if (item is TextEdit)
                        (item as TextEdit).Properties.ReadOnly = !enable;

                    if (item is MemoEdit)
                        (item as MemoEdit).Properties.ReadOnly = !enable;

                    if (item is HopePopSelect)
                        (item as HopePopSelect).Readonly = !enable;

                }
            }
        }


        /// <summary>
        /// 刷新
        /// </summary>
        public void DoRefresh()
        {
            EntityDicDataInterfaceCommand entityComd = new EntityDicDataInterfaceCommand();
            listInterCommand = proxy.Service.SearchDicDataInterfaceCommand(entityComd);
            bsListCommand.DataSource = listInterCommand;

            this.txtFilterCMDGroup.Items.Clear();
            this.txtFilterCMDGroup.Items.Add(string.Empty);
            foreach (EntityDicDataInterfaceCommand item in listInterCommand)
            {
                if (!this.txtFilterCMDGroup.Items.Contains(item.CmdGroup))
                {
                    this.txtFilterCMDGroup.Items.Add(item.CmdGroup);
                }
            }
        }

        private void EnableButtonStatus(bool enable)
        {
            sysToolBar1.BtnAdd.Enabled = enable;
            sysToolBar1.BtnModify.Enabled = enable;
            sysToolBar1.BtnDelete.Enabled = enable;
            sysToolBar1.BtnSave.Enabled = !enable;
            sysToolBar1.BtnCancel.Enabled = !enable;
            sysToolBar1.BtnRefresh.Enabled = enable;
            sysToolBar1.BtnImport.Enabled = enable;
            this.groupBox1.Enabled = !enable;
            this.btnTestConnenct.Enabled = enable;
            //this.gridControl1.Enabled = !enable;
            //this.gridControl2.Enabled = !enable;
            this.gridView2.OptionsBehavior.Editable = !enable;
        }

        Dictionary<string, string[]> CommandGroupNParamBindField = null;
        public void SetGroupFieldStyle(string defaultValue, bool allowEdit, bool allowEmpty)
        {
            Dictionary<string, string[]> selectItems = GetListGroupItem();
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

        private static Dictionary<string, string[]> GetListGroupItem()
        {
            Dictionary<string, string[]> list = new Dictionary<string, string[]>();

            List<string> listBarcodeBinding = new List<string>();
            listBarcodeBinding.Add("bc_in_no");
            listBarcodeBinding.Add("bc_pid");
            listBarcodeBinding.Add("bc_yz_id");
            listBarcodeBinding.Add("bc_his_code");
            listBarcodeBinding.Add("bc_his_name");
            listBarcodeBinding.Add("bc_name");
            listBarcodeBinding.Add("op_code");
            listBarcodeBinding.Add("op_time");
            listBarcodeBinding.Add("bc_times");

            list.Add("条码_门诊_下载后", listBarcodeBinding.ToArray());
            list.Add("条码_住院_下载后", listBarcodeBinding.ToArray());
            list.Add("条码_体检_下载后", listBarcodeBinding.ToArray());
            list.Add("条码_其他_下载后", listBarcodeBinding.ToArray());

            list.Add("条码_门诊_签收后", listBarcodeBinding.ToArray());
            list.Add("条码_住院_签收后", listBarcodeBinding.ToArray());
            list.Add("条码_体检_签收后", listBarcodeBinding.ToArray());
            list.Add("条码_其他_签收后", listBarcodeBinding.ToArray());

            list.Add("条码_门诊_删除后", listBarcodeBinding.ToArray());
            list.Add("条码_住院_删除后", listBarcodeBinding.ToArray());
            list.Add("条码_体检_删除后", listBarcodeBinding.ToArray());
            list.Add("条码_其他_删除后", listBarcodeBinding.ToArray());

            list.Add("二审后_危急值", new string[] { });
            list.Add("二审后_门诊_危急值", new string[] { });
            list.Add("二审后_住院_危急值", new string[] { });
            list.Add("二审后_体检_危急值", new string[] { });
            list.Add("二审后_其他_危急值", new string[] { });

            //list.Add("二审后_门诊_成功后", new string[] { });
            //list.Add("二审后_住院_成功后", new string[] { });
            //list.Add("二审后_体检_成功后", new string[] { });
            //list.Add("二审后_其他_成功后", new string[] { });

            List<string> listLabBinding = new List<string>();
            listLabBinding.Add("pat_id");
            listLabBinding.Add("pat_name");
            listLabBinding.Add("pat_in_no");
            listLabBinding.Add("pat_c_name");
            listLabBinding.Add("pat_report_code");
            listLabBinding.Add("pat_report_date");
            listLabBinding.Add("op_time");

            list.Add("检验_门诊_二审后", listLabBinding.ToArray());
            list.Add("检验_住院_二审后", listLabBinding.ToArray());
            list.Add("检验_体检_二审后", listLabBinding.ToArray());
            list.Add("检验_其他_二审后", listLabBinding.ToArray());

            list.Add("@ID号转换", new string[] { });

            return list;
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

                txtCmdGroup.Items.Add("@数据转换");

                txtCmdGroup.SelectedItem = defaultValue;
            }
        }

        private static DataTable GetTamplateDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("value");
            table.Columns.Add("display");
            return table;
        }
        /// <summary>
        /// 初始化界面数据
        /// </summary>
        private void RefreshUIData()
        {
            #region 执行方式
            DataTable tableFachType = GetTamplateDataTable();
            tableFachType.Rows.Add(new object[] { "", "" });
            tableFachType.Rows.Add(new object[] { EnumCommandExecuteTypeNew.ExecuteNonQuery.ToString(), "执行不返回值" });
            tableFachType.Rows.Add(new object[] { EnumCommandExecuteTypeNew.ExecuteScalar.ToString(), "返回单值" });
            tableFachType.Rows.Add(new object[] { EnumCommandExecuteTypeNew.ExecuteGetDataTable.ToString(), "获取DataTable" });
            tableFachType.Rows.Add(new object[] { EnumCommandExecuteTypeNew.ExecuteGetDataSet.ToString(), "获取DataSet" });
            this.txtCmdExeType.DataSource = tableFachType;
            #endregion

            #region 连接

            listConn = proxyConn.Service.SearchDataInterfaceConnection(new EntityDicDataInterfaceConnection());
            listConn.Insert(0, new EntityDicDataInterfaceConnection());
            foreach (var info in listConn)
            {
                info.DisPlayName = info.ConnId + " |" + info.ConnName;
            }
            this.txtConnection.DataSource = listConn;
            #endregion

            #region 运行端
            DataTable tableRunSide = GetTamplateDataTable();
            tableRunSide.Rows.Add(new object[] { "", "" });
            tableRunSide.Rows.Add(new object[] { EnumDeploymentModeNew.Server.ToString(), "服务端" });
            tableRunSide.Rows.Add(new object[] { EnumDeploymentModeNew.Client.ToString(), "客户端" });
            this.txtCmdRunningSide.DataSource = tableRunSide;
            #endregion

            #region 命令类型
            DataTable tableCmdType = GetTamplateDataTable();
            tableCmdType.Rows.Add(new object[] { "", "" });
            tableCmdType.Rows.Add(new object[] { EnumCommandTypeNew.Text.ToString(), "文本" });
            tableCmdType.Rows.Add(new object[] { EnumCommandTypeNew.StoredProcedure.ToString(), "存储过程" });
            this.txtCmdType.DataSource = tableCmdType;
            #endregion

            #region 参数方向
            DataTable tableParm = GetTamplateDataTable();
            tableParm.Rows.Add(new object[] { "", "" });
            tableParm.Rows.Add(new object[] { EnumDataInterfaceParameterDirectionNew.Input.ToString(), "输入" });
            tableParm.Rows.Add(new object[] { EnumDataInterfaceParameterDirectionNew.InputOutput.ToString(), "输入输出" });
            tableParm.Rows.Add(new object[] { EnumDataInterfaceParameterDirectionNew.Output.ToString(), "输出" });
            tableParm.Rows.Add(new object[] { EnumDataInterfaceParameterDirectionNew.Reference.ToString(), "引用" });
            tableParm.Rows.Add(new object[] { EnumDataInterfaceParameterDirectionNew.ReturnValue.ToString(), "返回值" });
            this.colParamDirection.DataSource = tableParm;
            #endregion

            #region 转换规则 (待修改)
            //List<EntityDictDataConverter> listConverter = new List<EntityDictDataConverter>();

            //listConverter.Insert(0, new EntityDictDataConverter());
            //this.colParamConverter.DataSource = listConverter;
            #endregion
        }

        /// <summary>
        /// 新增按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnAddClicked(object sender, EventArgs e)
        {
            EntityDicDataInterfaceCommand entity = (EntityDicDataInterfaceCommand)bsListCommand.AddNew();
            EnableButtonStatus(false);
            saveAction = SaveAction.Add;
            EnableBaseInfo(true);
            this.txtModule.Focus();
        }

        /// <summary>
        /// 放弃按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnCancelClicked(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要放弃当前的修改吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.bsListCommand.CancelEdit();
                this.bsListParameter.CancelEdit();

                EnableButtonStatus(true);
                EnableBaseInfo(false);
                DoRefresh();
            }
        }

        /// <summary>
        /// 修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnModifyClicked(object sender, EventArgs e)
        {
            EnableButtonStatus(false);
            saveAction = SaveAction.Edit;
            EnableBaseInfo(true);
        }
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnSaveClicked(object sender, EventArgs e)
        {
            this.bsListCommand.EndEdit();
            this.bsListParameter.EndEdit();

            if (this.gridView3.FocusedRowHandle < 0 && saveAction == SaveAction.Edit)
            {
                return;
            }

            EntityDicDataInterfaceCommand eyInterComd = (EntityDicDataInterfaceCommand)bsListCommand.Current;
            List<EntityDicDataInterfaceCommandParameter> listInterParm = this.bsListParameter.DataSource as List<EntityDicDataInterfaceCommandParameter>;
            List<EntityDicDataInterfaceCommandParameter> listGrid = this.gridControl2.DataSource as List<EntityDicDataInterfaceCommandParameter>;
            if (!BeforeSaveCheck(eyInterComd, listInterParm)) //保存前检查
                return;

            EntityRequest request = new EntityRequest();
            List<Object> listReq = new List<object>();
            listReq.Add(eyInterComd);
            listReq.Add(listInterParm);
            request.SetRequestValue(listReq);

            bool success = false;
            if (saveAction == SaveAction.Add)
                success = proxy.Service.SaveDicDataInterCommandAndParm(request); //保存
            else if (saveAction == SaveAction.Edit)
                success = proxy.Service.UpdateDicDataInterCommandAndParm(request); //更新
            else
                return;

            if (success)
            {
                MessageDialog.ShowAutoCloseDialog("保存成功!");
                DoRefresh();
                EnableButtonStatus(true);
                EnableBaseInfo(true);
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("保存失败!");
                DoRefresh();
                EnableButtonStatus(true);
                EnableBaseInfo(true);
            }
        }

        /// <summary>
        /// 保存前检查
        /// </summary>
        /// <returns></returns>
        private bool BeforeSaveCheck(EntityDicDataInterfaceCommand cmd, List<EntityDicDataInterfaceCommandParameter> listParam)
        {
            if (this.txtModule.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入[名称]", "提示");
                this.txtModule.Focus();
                return false;
            }
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
                foreach (EntityDicDataInterfaceCommandParameter p in listParam)
                {
                    if (string.IsNullOrEmpty(p.ParamName))
                    {
                        MessageBox.Show("[参数名]不能为空", "提示");
                        return false;
                    }

                    if (string.IsNullOrEmpty(p.ParamDatatype))
                    {
                        MessageBox.Show("参数[数据类型]不能为空", "提示");
                        return false;
                    }
                }
            }
            return true;
        }

        private void AddNew(EntityDicDataInterfaceCommand newInterConn, List<EntityDicDataInterfaceCommandParameter> newListComd)
        {
            if (newInterConn != null)
            {
                newInterConn.CmdId = null;
                newInterConn.CmdName = "复制_" + newInterConn.CmdName;

                //List<EntityDicDataInterfaceCommandParameter> listCloneParm = new List<EntityDicDataInterfaceCommandParameter>();
                //foreach (var infoParm in newListComd)
                //{
                //    EntityDicDataInterfaceCommandParameter entityNew = new EntityDicDataInterfaceCommandParameter();
                //    entityNew.CmdId = null;
                //    listCloneParm.Add(entityNew);
                //}

                bsListCommand.EndEdit();

                listInterCommand.Add(newInterConn);
                bsListCommand.DataSource = listInterCommand;
                bsListCommand.ResetBindings(true);

                gridView3.MoveLast();
                bsListCommand.ResetCurrentItem();

                this.bsListParameter.DataSource = newListComd;
                this.bsListParameter.ResetBindings(true);
            }
            if (newInterConn != null)
            {
                this.txtModule.Focus();
            }
        }

        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            try
            {
                this.bsListCommand.EndEdit();
                EntityDicDataInterfaceCommand obj = this.bsListCommand.Current as EntityDicDataInterfaceCommand;

                if (obj == null)
                    MessageBox.Show("请选择要删除的记录", "提示");

                if (obj.SysDefault == true)
                {
                    MessageBox.Show("系统预置数据不能删除", "提示");
                    return;
                }

                string id = "";
                if (!string.IsNullOrEmpty(obj.CmdId))
                {
                    id = obj.CmdId;
                }
                bool success = false;
                DialogResult dresult = MessageBox.Show(string.Format("你确定要删除当前选中的【{0}】这条记录吗？\r\n删除后将不可恢复", obj.CmdName), "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                switch (dresult)
                {
                    case DialogResult.Yes:
                        success = proxy.Service.DeleteDicDataInterCommandAndParm(id);
                        break;
                    case DialogResult.No:
                        return;
                }

                if (success)
                {
                    DoRefresh();
                    gridView3.RefreshData();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("删除出错", ex);
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnExportClicked(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnImportClicked(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_BtnCopyClick(object sender, EventArgs e)
        {
            if (this.bsListCommand.Current == null)
            {
                MessageBox.Show("请选择一条记录", "提示");
                return;
            }
            saveAction = SaveAction.Add; //新增标志
            this.EnableButtonStatus(false);

            EntityDicDataInterfaceCommand currentData = this.bsListCommand.Current as EntityDicDataInterfaceCommand;
            EntityDicDataInterfaceCommand copyInterComd = EntityManager<EntityDicDataInterfaceCommand>.EntityClone(currentData);

            List<EntityDicDataInterfaceCommandParameter> listInterComd = this.bsListParameter.DataSource as List<EntityDicDataInterfaceCommandParameter>;
            this.AddNew(copyInterComd, listInterComd);

            this.txtModule.Enabled = true;
            this.txtName.Enabled = true;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnRefreshClicked(object sender, EventArgs e)
        {
            DoRefresh();
        }

        /// <summary>
        /// 帮助
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_BtnDeRefClick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void bsListCommand_CurrentChanged(object sender, EventArgs e)
        {
            if (this.bsListCommand.Current != null)
            {
                EntityDicDataInterfaceCommand cmd = this.bsListCommand.Current as EntityDicDataInterfaceCommand;

                List<EntityDicDataInterfaceCommandParameter> listParm = new List<EntityDicDataInterfaceCommandParameter>();
                listParm = proxy.Service.GetParametersByCmdID(cmd.CmdId);
                //绑定参数
                this.bsListParameter.DataSource = listParm;
            }
        }

        /// <summary>
        /// 连接下拉改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtConnection_SelectedIndexChanged(object sender, EventArgs e)
        {
            EntityDicDataInterfaceConnection conn = this.txtConnection.SelectedItem as EntityDicDataInterfaceConnection;
            if (conn != null)
            {
                if (string.IsNullOrEmpty(conn.ConnType) || conn.ConnType.ToLower() == "sql")
                {
                    this.lblCommandText.Text = "sql语句";
                    this.colParamDataType.DataSource = GetSupportedDbTypeNames_tb();
                    colParamDataLength.Visible = true;
                }
                else
                {
                    this.lblCommandText.Text = "方法名";
                    this.colParamDataType.DataSource = GetSupportedNetTypeNames();
                    colParamDataLength.Visible = false;
                }
            }
            else
            {
                this.lblCommandText.Text = "执行命令";
                colParamDataLength.Visible = true;
            }
        }

        private static DataTable GetSupportedDbTypeNames_tb()
        {
            DataTable table = GetTamplateDataTable();
            table.Rows.Add(new object[] { "", "" });
            table.Rows.Add(DbType.AnsiString.ToString(), "非Unicode字符");
            table.Rows.Add(DbType.String.ToString(), "Unicode字符");

            table.Rows.Add(DbType.DateTime.ToString(), "日期时间");
            table.Rows.Add(DbType.Binary.ToString(), "布尔");

            table.Rows.Add(DbType.Decimal.ToString(), "Decimal");
            table.Rows.Add(DbType.Double.ToString(), "Double");

            table.Rows.Add(DbType.Int32.ToString(), "整型32");
            table.Rows.Add(DbType.Int64.ToString(), "整型64");

            table.Rows.Add(DbType.Binary.ToString(), "二进制字节流");

            return table;
        }

        public static DataTable GetSupportedNetTypeNames()
        {
            DataTable table = GetTamplateDataTable();

            table.Rows.Add(new object[] { "", "" });
            table.Rows.Add(typeof(string).FullName, "字符串");
            table.Rows.Add(typeof(Int16).FullName, "整型16");
            table.Rows.Add(typeof(int).FullName, "整型32");
            table.Rows.Add(typeof(long).FullName, "整型64位");
            table.Rows.Add(typeof(UInt16).FullName, "无符号整型16");
            table.Rows.Add(typeof(uint).FullName, "无符号整型32");
            table.Rows.Add(typeof(ulong).FullName, "无符号整型64");
            table.Rows.Add(typeof(decimal).FullName, "devimal");
            table.Rows.Add(typeof(double).FullName, "double");
            table.Rows.Add(typeof(float).FullName, "float");
            table.Rows.Add(typeof(bool).FullName, "布尔");
            table.Rows.Add(typeof(byte).FullName, "byte");
            table.Rows.Add(typeof(DateTime).FullName, "日期");
            table.Rows.Add(typeof(DataTable).AssemblyQualifiedName, "DataTable");
            table.Rows.Add(typeof(DataSet).AssemblyQualifiedName, "DataSet");
            table.Rows.Add(typeof(StringBuilder).AssemblyQualifiedName, "StringBuilder");
            table.Rows.Add(typeof(string).FullName + "[]", "string[]");
            table.Rows.Add(typeof(int).FullName + "[]", "int[]");
            table.Rows.Add(typeof(byte).FullName + "[]", "byte[]");

            return table;
        }
        /// <summary>
        /// (bsListCommand)BindingSource，参数添加对象事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bsListCommand_AddingNew(object sender, AddingNewEventArgs e)
        {
            EntityDicDataInterfaceCommand cmd = new EntityDicDataInterfaceCommand();
            cmd.CmdCommandType = CommandType.Text.ToString();
            cmd.CmdEnabled = true;
            cmd.CmdEnabledLog = false;
            cmd.CmdExecSeq = 0;
            cmd.CmdFetchType = EnumCommandExecuteTypeNew.ExecuteNonQuery.ToString();
            cmd.CmdRunningSide = EnumDeploymentModeNew.Server.ToString();
            cmd.SysModule = null;
            e.NewObject = cmd;
        }

        private void bsListParameter_AddingNew(object sender, AddingNewEventArgs e)
        {
            EntityDicDataInterfaceCommandParameter param = new EntityDicDataInterfaceCommandParameter();

            EntityDicDataInterfaceConnection conn = this.txtConnection.SelectedItem as EntityDicDataInterfaceConnection;
            if (conn != null)
            {
                if (string.IsNullOrEmpty(conn.ConnType))
                {
                    param.ParamDatatype = "";
                }
                else if (conn.ConnType.ToLower() == "sql")
                {
                    param.ParamDatatype = DbType.AnsiString.ToString();
                }
                else
                {
                    param.ParamDatatype = typeof(string).FullName;
                }
            }
            param.ParamDirection = EnumDataInterfaceParameterDirectionNew.Input.ToString();
            param.ParamEnabled = true;
            param.ParamIsbound = false;

            List<EntityDicDataInterfaceCommandParameter> list = this.bsListParameter.DataSource as List<EntityDicDataInterfaceCommandParameter>;

            if (list == null)
                list = new List<EntityDicDataInterfaceCommandParameter>();

            if (list.Count == 0)
            {
                param.ParamSeq = 0;
            }
            else
            {
                int max = int.MinValue;
                foreach (EntityDicDataInterfaceCommandParameter item in list)
                {
                    if (item.ParamSeq >= max)
                        max = item.ParamSeq;
                }
                param.ParamSeq = ((max + 10) / 10) * 10;
            }
            e.NewObject = param;
        }
    }
}
