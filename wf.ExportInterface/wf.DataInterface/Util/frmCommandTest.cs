using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//using Lib.DataInterface.DataModel;
using System.IO;
using Lib.DataInterface.DataModel;
using System.Reflection;
using Lib.DataInterface.Implement;

namespace Lib.DataInterface
{
    public partial class frmCommandTest : Form
    {
        DataInterfaceCommand _cmdcfg = null;

        public frmCommandTest(DataInterfaceCommand cmdcfg)
        {
            InitializeComponent();
            this._cmdcfg = cmdcfg;

            if (this._cmdcfg != null)
            {
                //this.tsOpen.Visible = false;
                //this.tsSave.Visible = false;
            }

            this.txtCmdCommandType.DataSource = Enum.GetNames(typeof(EnumCommandType));
            this.txtCmdExeType.DataSource = Enum.GetNames(typeof(EnumCommandExecuteType));
            this.txtConnType.DataSource = Enum.GetNames(typeof(EnumDataInterfaceConnectionType));
            this.txtConnDialet.DataSource = Enum.GetNames(typeof(Lib.DAC.EnumDataBaseDialet));
            this.txtConnDriver.DataSource = Enum.GetNames(typeof(Lib.DAC.EnumDbDriver));

            this.bsParameterDirection.DataSource = Enum.GetValues(typeof(EnumDataInterfaceParameterDirection));
            this.txtConnDialet.SelectedItem = Lib.DAC.EnumDataBaseDialet.SQL2005.ToString();
            this.txtConnDriver.SelectedItem = Lib.DAC.EnumDbDriver.MSSql.ToString();

            if (this._cmdcfg == null)
            {
                this._cmdcfg = new DataInterfaceCommand();
            }

            if (this._cmdcfg.Connection == null)
            {
                this._cmdcfg.Connection = new DataInterfaceConnection();
            }

            SetParameterGridBindingDataType();
            BindUI(this._cmdcfg);
        }

        public frmCommandTest()
            : this(null)
        {
        }

        private void SetParameterGridBindingDataType()
        {
            EnumDataInterfaceConnectionType conntype = (EnumDataInterfaceConnectionType)Enum.Parse(typeof(EnumDataInterfaceConnectionType), this.txtConnType.Text);
            if (conntype == EnumDataInterfaceConnectionType.SQL)
            {
                this.bsPDataType.DataSource = Enum.GetNames(typeof(DbType));
                pDataType.DisplayMember = null;
                pDataType.ValueMember = null;
            }
            else
            {
                this.bsPDataType.DataSource = ControlBindingData.GetSupportedNetTypeNames().DefaultView;
                pDataType.DisplayMember = "display";
                pDataType.ValueMember = "value";
            }
        }

        private void tsRun_Click(object sender, EventArgs e)
        {
            EndEdit();

            EnumCommandExecuteType exetype = (EnumCommandExecuteType)Enum.Parse(typeof(EnumCommandExecuteType), this.txtCmdExeType.Text);

            if (exetype == EnumCommandExecuteType.ExecuteGetDataTable)
            {
                DataSet ds = _cmdcfg.ExecuteGetDataSet();
                BindResultGrid(ds);
            }
            else if (exetype == EnumCommandExecuteType.ExecuteGetDataTable)
            {
                DataTable table = _cmdcfg.ExecuteGetDataTable();
                DataSet ds = new DataSet();
                ds.Tables.Add(table);
                BindResultGrid(ds);
            }
            else if (exetype == EnumCommandExecuteType.ExecuteNonQuery)
            {
                _cmdcfg.ExecuteNonQuery();
            }
            else if (exetype == EnumCommandExecuteType.ExecuteScalar)
            {
                object ret = _cmdcfg.ExecuteScalar();
            }

            this.bsParameter.ResetBindings(false);
        }

        void BindResultGrid(DataSet ds)
        {
            this.tabExecuteResult.TabPages.Clear();

            for (int i = 0; i < ds.Tables.Count; i++)
            {
                this.tabExecuteResult.TabPages.Add(ds.Tables[i].TableName);
                DataGridView grid = new DataGridView();
                grid.DataSource = ds.Tables[i];
                this.tabExecuteResult.TabPages[i].Controls.Add(grid);
                grid.Dock = DockStyle.Fill;
            }
        }

        private void frmCommandTest_Load(object sender, EventArgs e)
        {
            //this.bsConnection.DataSource = _cmdcfg.Connection;
            //this.bsCommand.DataSource = _cmdcfg;
            //this.bsParameter.DataSource = _cmdcfg.Parameters;
        }

        private void txtConnType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.txtConnType.Text == EnumDataInterfaceConnectionType.SQL.ToString())
            {
                this.lblConnDialet.Visible = true;
                this.lblConnDriver.Visible = true;
                this.txtConnDialet.Visible = true;
                this.txtConnDriver.Visible = true;
                this.lblConnCatelog.Visible = true;
                this.txtConnCatelog.Visible = true;

                this.lblConnLoginName.Visible = true;
                this.txtConnLoginName.Visible = true;
                this.lblConnLoginPass.Visible = true;
                this.txtConnLoginPass.Visible = true;

                this.lblConnAddress.Text = "数据库地址：";
            }
            else
            {
                this.lblConnDialet.Visible = false;
                this.lblConnDriver.Visible = false;
                this.txtConnDialet.Visible = false;
                this.txtConnDriver.Visible = false;
                this.lblConnCatelog.Visible = false;
                this.txtConnCatelog.Visible = false;
                this.lblConnLoginName.Visible = false;
                this.txtConnLoginName.Visible = false;
                this.lblConnLoginPass.Visible = false;
                this.txtConnLoginPass.Visible = false;


                if (this.txtConnType.Text == EnumDataInterfaceConnectionType.WebService.ToString()
                    || this.txtConnType.Text == EnumDataInterfaceConnectionType.WCF.ToString()
                    )
                {
                    this.lblConnAddress.Text = "WSDL地址：";
                }
                else if (this.txtConnType.Text == EnumDataInterfaceConnectionType.DotNetDll.ToString()
                        || this.txtConnType.Text == EnumDataInterfaceConnectionType.BiniaryDll.ToString()
                    )
                {
                    this.lblConnAddress.Text = "DLL地址：";

                    if (this.txtConnType.Text == EnumDataInterfaceConnectionType.DotNetDll.ToString())
                    {
                        this.lblConnCatelog.Visible = true;
                        this.txtConnCatelog.Visible = true;
                        this.lblConnCatelog.Text = "类名全名";
                    }
                }
            }

            SetParameterGridBindingDataType();
        }

        private void tabSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabSetting.SelectedTab == tbCommand
                || this.tabSetting.SelectedTab == tbConnection)
            {
                this.tsRun.Visible = false;
            }
            else
            {
                this.tsRun.Visible = true;
            }
        }

        private void tsSave_Click(object sender, EventArgs e)
        {
            EndEdit();
            saveFileDialog1.FileName = "LisInterfaceSetting.XML";
            saveFileDialog1.Filter = "xml文件|*.xml";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string xmlString = SettingSerializer.EntityToXMLString(this._cmdcfg);
                using (StreamWriter sw = File.CreateText(saveFileDialog1.FileName))
                {
                    sw.Write(xmlString);
                }
            }
        }

        private void tsOpen_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "LisInterfaceSetting.xml";
            openFileDialog1.Filter = "xml文件|*.xml";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string xmlString = null;
                using (StreamReader sr = File.OpenText(openFileDialog1.FileName))
                {
                    xmlString = sr.ReadToEnd();
                }
                this._cmdcfg = SettingSerializer.XMLStringToEntity(xmlString);

                BindUI(this._cmdcfg);
            }
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            EndEdit();
            IDataInterfaceConnection conn = ConnectionHelper.GetConnection(this._cmdcfg.Connection.ConnectionType);
            conn.Catelog = this._cmdcfg.Connection.Catelog;
            conn.DbDialet = this._cmdcfg.Connection.DbDialet;
            conn.DbDriver = this._cmdcfg.Connection.DbDriver;
            conn.LoginName = this._cmdcfg.Connection.LoginName;
            conn.LoginPassword = this._cmdcfg.Connection.LoginPassword;
            conn.ServerAddress = this._cmdcfg.Connection.ServerAddress;

            string errMsg;
            if (conn.TestConnection(out errMsg))
            {
                MessageBox.Show("连接成功", "连接成功");
            }
            else
            {
                MessageBox.Show(errMsg, "连接失败");
            }
        }
        private void EndEdit()
        {
            this.dataGridView1.EndEdit();

            this._cmdcfg.CommandText = this.txtCmdCommandText.Text;
            this._cmdcfg.CommandType = (EnumCommandType)Enum.Parse(typeof(EnumCommandType), this.txtCmdCommandType.Text, true);

            this._cmdcfg.Connection.Catelog = this.txtConnCatelog.Text;
            this._cmdcfg.Connection.ConnectionType = (EnumDataInterfaceConnectionType)Enum.Parse(typeof(EnumDataInterfaceConnectionType), this.txtConnType.Text, true);
            this._cmdcfg.Connection.DbDialet = this.txtConnDialet.Text;
            this._cmdcfg.Connection.DbDriver = this.txtConnDriver.Text;
            this._cmdcfg.Connection.LoginName = this.txtConnLoginName.Text;
            this._cmdcfg.Connection.LoginPassword = this.txtConnLoginPass.Text;
            this._cmdcfg.Connection.ServerAddress = this.txtConnAddress.Text;
        }

        private void BindUI(DataInterfaceCommand cmd)
        {
            this.txtCmdCommandText.Text = cmd.CommandText;
            this.txtCmdCommandType.Text = cmd.CommandType.ToString();

            this.txtConnCatelog.Text = cmd.Connection.Catelog;
            this.txtConnType.Text = cmd.Connection.ConnectionType.ToString();
            this.txtConnDialet.Text = cmd.Connection.DbDialet;
            this.txtConnDriver.Text = cmd.Connection.DbDriver;
            this.txtConnLoginName.Text = cmd.Connection.LoginName;
            this.txtConnLoginPass.Text = cmd.Connection.LoginPassword;
            this.txtConnAddress.Text = cmd.Connection.ServerAddress;

            this.bsParameter.DataSource = cmd.Parameters;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
    }
}