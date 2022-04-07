using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

namespace Lib.DataInterface.Implement
{
    partial class frmTestCommand : Form
    {
        EntityDictDataInterfaceCommand _dtoCmd = null;
        EntityDictDataInterfaceConnection _dtoConn = null;
        List<EntityDictDataInterfaceCommandParameter> _dtoListParam = null;
        DACManager _dac;

        public frmTestCommand(EntityDictDataInterfaceConnection dtoConn
            , EntityDictDataInterfaceCommand dtoCmd
            , List<EntityDictDataInterfaceCommandParameter> dtoListParam
            , DACManager dac
            )
        {
            InitializeComponent();
            toolStripStatusLabel1.Text = string.Empty;
            _dtoCmd = dtoCmd;
            _dtoConn = dtoConn;
            _dtoListParam = dtoListParam;
            _dac = dac;

            if (dtoCmd.cmd_fetch_type == EnumCommandExecuteType.ExecuteGetDataSet.ToString()
                || dtoCmd.cmd_fetch_type == EnumCommandExecuteType.ExecuteGetDataTable.ToString())
            {
                //this.splitContainer1.Panel2.Visible = true;
            }
            else
            {
                this.splitContainer1.Panel2.Visible = false;
                //this.Height = this.Height - 190;
                this.splitContainer1.SplitterDistance = 600;
            }
        }

        private void frmTestCommand_Load(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            DataInterfaceCommand cmd = DataInterfaceCommand.FromDTO(this._dtoCmd);
            DataInterfaceConnection conn = DataInterfaceConnection.FromDTO(this._dtoConn);
            DataInterfaceParameterCollection pars = DataInterfaceParameterCollection.FromDTO(_dtoListParam, _dac);

            sw.Stop();
            Debug.WriteLine("FromConfig:" + sw.ElapsedMilliseconds + "ms");

            cmd.Connection = conn;
            cmd.Parameters = pars;
            this.bsCommand.DataSource = cmd;

            if (_dtoCmd != null)
            {
                this.Text = "接口测试：" + _dtoCmd.cmd_name;
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            this.dataGridView1.EndEdit();

            DataInterfaceCommand cmd = this.bsCommand.DataSource as DataInterfaceCommand;

            if (this._dtoCmd.cmd_running_side == EnumDeploymentMode.Server.ToString())
            {
                //运行于服务端
                //List<InterfaceDataBindingItem> listBinding

                //cmd.Parameters
            }
            else if (this._dtoCmd.cmd_running_side == EnumDeploymentMode.Client.ToString())
            {
                //运行于客户端
            }


            try
            {
                this.lblScalarValue.Visible = false;
                pnlTableResult.TabPages.Clear();

                Stopwatch sw = new Stopwatch();
                sw.Start();

                long ElapsedMilliseconds = -1;

                if (this._dtoCmd.cmd_fetch_type == EnumCommandExecuteType.ExecuteNonQuery.ToString())
                {
                    cmd.ExecuteNonQuery();

                    sw.Stop();
                    Debug.WriteLine("FromConfig:" + sw.ElapsedMilliseconds + "ms");

                    ElapsedMilliseconds = sw.ElapsedMilliseconds;

                    this.bsCommand.ResetBindings(false);
                }
                else if (this._dtoCmd.cmd_fetch_type == EnumCommandExecuteType.ExecuteScalar.ToString())
                {
                    object ret = cmd.ExecuteScalar();

                    sw.Stop();
                    Debug.WriteLine("FromConfig:" + sw.ElapsedMilliseconds + "ms");

                    ElapsedMilliseconds = sw.ElapsedMilliseconds;

                    if (ret == null)
                    {
                        lblScalarValue.Text = "<null>";
                    }
                    else if (ret == DBNull.Value)
                    {
                        lblScalarValue.Text = "<DBNull>";
                    }
                    else
                    {
                        this.lblScalarValue.Text = ret.ToString();
                    }
                    this.lblScalarValue.Visible = true;
                    this.bsCommand.ResetBindings(false);
                }
                else if (this._dtoCmd.cmd_fetch_type == EnumCommandExecuteType.ExecuteGetDataSet.ToString())
                {
                    DataSet ds = cmd.ExecuteGetDataSet();

                    sw.Stop();
                    Debug.WriteLine("FromConfig:" + sw.ElapsedMilliseconds + "ms");
                    ElapsedMilliseconds = sw.ElapsedMilliseconds;

                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        //TabPage tp = new TabPage("table" + (i + 1));
                        TabPage tp = new TabPage(ds.Tables[i].TableName);

                        DataGridView dgv = new DataGridView();
                        tp.Controls.Add(dgv);
                        dgv.Dock = DockStyle.Fill;
                        dgv.DataSource = ds.Tables[i];

                        pnlTableResult.TabPages.Add(tp);
                    }
                    this.bsCommand.ResetBindings(false);
                }
                else if (this._dtoCmd.cmd_fetch_type == EnumCommandExecuteType.ExecuteGetDataTable.ToString())
                {
                    DataTable table = cmd.ExecuteGetDataTable();
                    sw.Stop();
                    Debug.WriteLine("FromConfig:" + sw.ElapsedMilliseconds + "ms");
                    ElapsedMilliseconds = sw.ElapsedMilliseconds;

                    TabPage tp = new TabPage("table1");
                    DataGridView dgv = new DataGridView();
                    tp.Controls.Add(dgv);
                    dgv.Dock = DockStyle.Fill;

                    dgv.DataSource = table;
                    pnlTableResult.TabPages.Add(tp);
                    this.bsCommand.ResetBindings(false);
                }
                toolStripStatusLabel1.Text = string.Format("执行时间：{0}ms", ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }

        }
    }
}
