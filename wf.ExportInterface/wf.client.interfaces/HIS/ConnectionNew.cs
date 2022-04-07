using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using dcl.client.common;
using lis.client.control;

using dcl.common.extensions;
using dcl.client.frame;
using dcl.entity;
using dcl.client.wcf;
using dcl.common;
using Lib.LogManager;

namespace dcl.client.interfaces
{
    [DesignTimeVisible(false)]
    public partial class ConnectionNew : ConDicCommon
    {
        public ConnectionNew()
        {
            InitializeComponent();
            EnableButtonStatus(true);
        }
        enum SaveAction
        {
            Add,
            Edit,
            Unknown
        }

        private SaveAction saveAction = SaveAction.Unknown;
        ProxyDataInterfaceConnection proxy = new ProxyDataInterfaceConnection();
        List<EntityDicDataInterfaceConnection> listInterConn = new List<EntityDicDataInterfaceConnection>();

        private void ConnectionNew_Load(object sender, EventArgs e)
        {
            //初始化工具条
            sysToolBar1.BtnCopy.Caption = "复制";
            sysToolBar1.BtnDeRef.Caption = "帮助";
            sysToolBar1.SetToolButtonStyle(new string[] {
                sysToolBar1.BtnAdd.Name, sysToolBar1.BtnModify.Name, sysToolBar1.BtnDelete.Name, sysToolBar1.BtnSave.Name, sysToolBar1.BtnCancel.Name,
                sysToolBar1.BtnRefresh.Name,sysToolBar1.BtnCopy.Name,sysToolBar1.BtnExport.Name,sysToolBar1.BtnImport.Name,sysToolBar1.BtnDeRef.Name,
                sysToolBar1.BtnClose.Name });

            this.comboBoxEdit1.Properties.Items.AddRange(Enum.GetNames(typeof(EnumDeploymentModeNew)));
            this.cbConnectType.Properties.Items.AddRange(Enum.GetNames(typeof(EnumDataInterfaceConnectionTypeNew)));

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
        /// 测试连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTestConnenct_Click(object sender, EventArgs e)
        {
            EntityDicDataInterfaceConnection inter = (EntityDicDataInterfaceConnection)bsDataInterConn.Current;
            string result = proxy.Service.TestConnection(inter);
            //if (string.IsNullOrEmpty(result))
            //    MessageDialog.Show("连接成功!", "提示");
            //else
            //{
            //    MessageDialog.Show("连接失败!", "提示");
            //}
        }

        private void cbInterfaceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lblRemark.Visible = (cbInterfaceType.Text == "SQL");
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public void DoRefresh()
        {
            listInterConn = proxy.Service.SearchDataInterfaceConnection(null);
            bsDataInterConn.DataSource = listInterConn;
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
        }
        /// <summary>
        /// 新增按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnAddClicked(object sender, EventArgs e)
        {
            EntityDicDataInterfaceConnection entity = (EntityDicDataInterfaceConnection)bsDataInterConn.AddNew();
            EnableButtonStatus(false);
            saveAction = SaveAction.Add;
            EnableBaseInfo(true);
        }

        /// <summary>
        /// 放弃按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnCancelClicked(object sender, EventArgs e)
        {
            EnableButtonStatus(true);
            EnableBaseInfo(false);
            DoRefresh();
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
            bsDataInterConn.EndEdit();
            if (this.gridView3.FocusedRowHandle < 0 && saveAction == SaveAction.Edit)
            {
                return;
            }
            if (!BeforeSaveCheck())
                return;

            bool success = false;
            if (saveAction == SaveAction.Add)
            {
                EntityDicDataInterfaceConnection saveInterConn = (EntityDicDataInterfaceConnection)bsDataInterConn.Current;

                success = proxy.Service.SaveDataInterfaceConnection(saveInterConn);
            }
            else if (saveAction == SaveAction.Edit)
            {
                EntityDicDataInterfaceConnection updateInterConn = (EntityDicDataInterfaceConnection)bsDataInterConn.Current;
                success = proxy.Service.UpdateDataInterfaceConnection(updateInterConn);
            }
            else
            {
                return;
            }

            if (success)
            {
                MessageDialog.ShowAutoCloseDialog("保存成功!");
                DoRefresh();
                EnableButtonStatus(true);
                EnableBaseInfo(true);
            }
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

        private void AddNew(EntityDicDataInterfaceConnection newInterConn)
        {
            if (newInterConn != null)
            {
                newInterConn.ConnId = null;
                newInterConn.ConnCode = null;
                newInterConn.ConnName = "复制_" + newInterConn.ConnName;

                bsDataInterConn.EndEdit();

                listInterConn.Add(newInterConn);
                bsDataInterConn.DataSource = listInterConn;
                bsDataInterConn.ResetBindings(true);

                gridView3.MoveLast();
                bsDataInterConn.ResetCurrentItem();

                this.EnableButtonStatus(false);

            }
            if (newInterConn != null)
            {
                this.txtCode.Focus();
            }
        }
        /// <summary>
        /// 需要考虑对密码数据库名等加密，防可见
        /// </summary>
        /// <param name="inter"></param>
        private void encryption(EntitySysItfInterface inter)
        {
            if (!string.IsNullOrEmpty(inter.ItfaceServer))
            {
                inter.ItfaceServer = EncryptClass.Encrypt(inter.ItfaceServer);
            }
            if (!string.IsNullOrEmpty(inter.ItfaceDatabase))
            {
                inter.ItfaceDatabase = EncryptClass.Encrypt(inter.ItfaceDatabase);
            }
            if (!string.IsNullOrEmpty(inter.ItfaceLogid))
            {
                inter.ItfaceLogid = EncryptClass.Encrypt(inter.ItfaceLogid);
            }
            if (!string.IsNullOrEmpty(inter.ItfacePassword))
            {
                inter.ItfacePassword = EncryptClass.Encrypt(inter.ItfacePassword);
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
                this.bsDataInterConn.EndEdit();
                if (this.gridView3.FocusedRowHandle < 0)
                {
                    MessageBox.Show("未选择需删除行！", "提示");
                    return;
                }

                EntityDicDataInterfaceConnection inter = bsDataInterConn.Current as EntityDicDataInterfaceConnection;
                string id = "";
                if (!string.IsNullOrEmpty(inter.ConnId))
                {
                    id = inter.ConnId;
                }
                bool success = false;
                DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                switch (dresult)
                {
                    case DialogResult.OK:
                        success = proxy.Service.DeleteDataInterfaceConnection(id);
                        break;
                    case DialogResult.Cancel:
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
            if (this.bsDataInterConn.Current == null)
            {
                MessageBox.Show("请选择一条记录", "提示");
                return;
            }

            EntityDicDataInterfaceConnection obj = this.bsDataInterConn.Current as EntityDicDataInterfaceConnection;

            EntityDicDataInterfaceConnection clonedObj = EntityManager<EntityDicDataInterfaceConnection>.EntityClone(obj);

            SaveFileDialog diag = new SaveFileDialog();
            diag.Filter = "xml文件|*.xml";
            diag.FileName = "数据源_" + clonedObj.ConnName + ".xml";
            if (diag.ShowDialog() == DialogResult.OK)
            {
                clonedObj.ConnId = null;
                clonedObj.SysDefault = 0;
                string strXML = Lib.EntityCore.EntityXMLConverter.EntityToXMLString<EntityDicDataInterfaceConnection>(clonedObj);
                using (System.IO.StreamWriter sw = System.IO.File.CreateText(diag.FileName))
                {
                    sw.WriteLine(strXML);
                }
            }
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnImportClicked(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            diag.Filter = "xml文件|*.xml";
            if (diag.ShowDialog() == DialogResult.OK)
            {
                string strXML = System.IO.File.ReadAllText(diag.FileName);

                try
                {
                    EntityDicDataInterfaceConnection obj = Lib.EntityCore.EntityXMLConverter.XMLStringToEntity<EntityDicDataInterfaceConnection>(strXML);
                    AddNew(obj);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导入失败\r\n" + ex.Message, "提示");
                }
            }
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_BtnCopyClick(object sender, EventArgs e)
        {
            if (this.bsDataInterConn.Current == null)
            {
                MessageBox.Show("请选择一条记录", "提示");
                return;
            }
            saveAction = SaveAction.Add;
            EntityDicDataInterfaceConnection currentData = this.bsDataInterConn.Current as EntityDicDataInterfaceConnection;

            EntityDicDataInterfaceConnection copyInterConn = EntityManager<EntityDicDataInterfaceConnection>.EntityClone(currentData);
            this.AddNew(copyInterConn);
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

        /// <summary>
        /// 类型选择改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbConnectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = this.cbConnectType.Text.ToLower();
            if (type == EnumDataInterfaceConnectionTypeNew.SQL.ToString().ToLower())
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

                this.txtDriver.Properties.Items.Clear();
                this.txtDialet.Properties.Items.Clear();

                //暂时先注释
                //this.txtDriver.DropDownStyle = ComboBoxStyle.DropDownList;
                //this.txtDialet.DropDownStyle = ComboBoxStyle.DropDownList;

                this.txtDriver.Properties.Items.AddRange(Enum.GetNames(typeof(Lib.DAC.EnumDbDriver)));
                this.txtDialet.Properties.Items.AddRange(Enum.GetNames(typeof(Lib.DAC.EnumDataBaseDialet)));
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
            else if (type == EnumDataInterfaceConnectionTypeNew.DotNetDll.ToString().ToLower())
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
            else if (type == EnumDataInterfaceConnectionTypeNew.BiniaryDll.ToString().ToLower())
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
            else if (type == EnumDataInterfaceConnectionTypeNew.DOSCommand.ToString().ToLower())
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
    }
}

