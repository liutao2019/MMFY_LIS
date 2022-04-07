using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Lib.DataInterface.Implement;

namespace Lib.DataInterface
{
    /// <summary>
    /// 接口命令
    /// </summary>
    [Serializable]
    public class DataInterfaceCommand : ICloneable
    {
        /// <summary>
        /// 命令语句
        /// </summary>
        public string CommandText { get; set; }

        /// <summary>
        /// 命令类型
        /// </summary>
        public EnumCommandType CommandType { get; set; }

        /// <summary>
        /// 是否启用日志
        /// </summary>
        public bool EnabledLog { get; set; }

        #region Connection
        private DataInterfaceConnection _connection = null;

        /// <summary>
        /// 数据连接
        /// </summary>
        public DataInterfaceConnection Connection
        {
            get
            {
                return this._connection;
            }
            set
            {
                this._connection = value;
                //if (this._connection == null)
                //{
                //    this._executeConnection = null;
                //}
                //else
                //{
                //    this._executeConnection = DataInterfaceHelper.GetConnection(this._connection.ConnectionType);
                //    this._executeConnection.Catelog = this._connection.Catelog;
                //    this._executeConnection.DbDialet = this._connection.DbDialet;
                //    this._executeConnection.DbDriver = this._connection.DbDriver;
                //    this._executeConnection.LoginName = this._connection.LoginName;
                //    this._executeConnection.LoginPassword = this._connection.LoginPassword;
                //    this._executeConnection.ServerAddress = this._connection.ServerAddress;
                //}
            }
        }
        #endregion

        /// <summary>
        /// 命令参数
        /// </summary>
        public DataInterfaceParameterCollection Parameters { get; set; }

        #region .ctor
        public DataInterfaceCommand()
        {
            this.Connection = null;
            this.Parameters = new DataInterfaceParameterCollection();
            this.EnabledLog = false;
        }
        #endregion

        /// <summary>
        /// 创建命令
        /// </summary>
        /// <returns></returns>
        private IDataInterfaceCommand CreateCommand()
        {
            IDataInterfaceConnection conn = CreateConnection();

            IDataInterfaceCommand cmd;
            if (conn == null)
            {
                cmd = new SqlDataInterfaceCommand();
            }
            else
            {
                cmd = conn.CreateCommand();
            }

            cmd.CommandText = this.CommandText;
            cmd.CommandType = this.CommandType;
            cmd.Connection = conn;

            PrepareParameter(cmd);

            return cmd;
        }

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <returns></returns>
        private IDataInterfaceConnection CreateConnection()
        {

            //throw new Exception("连接尚未初始化");

            //IDataInterfaceConnection conn = DataInterfaceHelper.GetConnection(this._connection.ConnectionType);
            //conn.Catelog = this._connection.Catelog;
            //conn.DbDialet = this._connection.DbDialet;
            //conn.DbDriver = this._connection.DbDriver;
            //conn.LoginName = this._connection.LoginName;
            //conn.LoginPassword = this._connection.LoginPassword;
            //conn.ServerAddress = this._connection.ServerAddress;
            IDataInterfaceConnection conn;
            if (this._connection == null)
            {
                conn = null;// new SqlDataInterfaceConnection();
            }
            else
            {
                conn = this._connection.GetExecuteConnection();
            }
            return conn;
        }

        /// <summary>
        /// 配置命令参数
        /// </summary>
        /// <param name="cmd"></param>
        private void PrepareParameter(IDataInterfaceCommand cmd)
        {
            cmd.Parameters.Clear();
            foreach (DataInterfaceParameter p in this.Parameters)
            {
                IDataInterfaceParameter par;
                if (cmd is SqlDataInterfaceCommand)
                {
                    par = (cmd as SqlDataInterfaceCommand).CreateParameter(p);
                }
                else
                {
                    par = cmd.CreateParameter(p.Name);
                }
                par.DataType = p.DataType;
                par.Direction = p.Direction;
                par.Length = p.Length;
                //par.Converter = p.Converter;

                //配置数据转换，目前只针对input类型
                if (p.Direction == EnumDataInterfaceParameterDirection.Input
                    && p.Converter != null
                    )
                {
                    if (p.Value != null && p.Value.ToString() == ConverterSpecialValue.NULL)
                    {
                        par.Value = null;
                    }
                    else
                    {
                        par.Value = p.Converter.ConvertTo(p.Value);
                    }
                }
                else
                {
                    par.Value = p.Value;
                }

                cmd.Parameters.Add(par);
            }
        }

        public int ExecuteNonQuery()
        {
            IDataInterfaceCommand cmd = CreateCommand();
            int ret = cmd.ExecuteNonQuery();

            AfterExecute(cmd);

            return ret;
        }

        public DataTable ExecuteGetDataTable()
        {
            IDataInterfaceCommand cmd = CreateCommand();
            DataTable ret = cmd.ExecuteGetDataTable();

            AfterExecute(cmd);

            return ret;
        }

        public DataSet ExecuteGetDataSet()
        {
            IDataInterfaceCommand cmd = CreateCommand();
            DataSet ret = cmd.ExecuteGetDataSet();

            AfterExecute(cmd);

            return ret;
        }

        public object ExecuteScalar()
        {
            IDataInterfaceCommand cmd = CreateCommand();
            object ret = cmd.ExecuteScalar();

            AfterExecute(cmd);

            return ret;
        }

        void AfterExecute(IDataInterfaceCommand cmd)
        {
            #region log
            StringBuilder sbLog = null;
            if (this.EnabledLog)
            {
                sbLog = new StringBuilder();
                sbLog.AppendLine("接口类型：" + this.Connection.ConnectionType);
                sbLog.AppendLine("接口执行语句：\r\n" + this.CommandText + "\r\n");
                sbLog.AppendLine("接口类型：" + this.CommandType);
                sbLog.AppendLine("连接地址：" + this.Connection.ServerAddress);
                sbLog.AppendLine("");
                sbLog.Append("参数名".PadRight(16));
                sbLog.Append("数据类型".PadRight(15));
                sbLog.Append("方向".PadRight(10));
                sbLog.Append("执行前值".PadRight(15));
                sbLog.Append("执行后值".PadRight(15));
                sbLog.Append("\r\n");
                sbLog.AppendLine("-".PadRight(80, '-'));
            }
            #endregion

            foreach (IDataInterfaceParameter sqlpar in cmd.Parameters)
            {
                string pName = sqlpar.Name;
                DataInterfaceParameter p = this.Parameters[pName] as DataInterfaceParameter;

                #region log
                if (this.EnabledLog)
                {
                    sbLog.Append(pName.PadRight(19));
                    sbLog.Append(sqlpar.DataType.PadRight(19));

                    if (sqlpar.Direction == EnumDataInterfaceParameterDirection.Input)
                    {
                        sbLog.Append("输入".PadRight(10));
                    }
                    else if (sqlpar.Direction == EnumDataInterfaceParameterDirection.InputOutput)
                    {
                        sbLog.Append("输入输出".PadRight(8));
                    }
                    else if (sqlpar.Direction == EnumDataInterfaceParameterDirection.Output)
                    {
                        sbLog.Append("输出".PadRight(10));
                    }
                    else if (sqlpar.Direction == EnumDataInterfaceParameterDirection.Reference)
                    {
                        sbLog.Append("引用".PadRight(10));
                    }
                    else if (sqlpar.Direction == EnumDataInterfaceParameterDirection.ReturnValue)
                    {
                        sbLog.Append("返回".PadRight(10));
                    }

                    if (p.Value == null || p.Value.ToString() == ConverterSpecialValue.NULL)
                    {
                        sbLog.Append("<null>".PadRight(19));
                    }
                    else if (p.Value == DBNull.Value || p.Value.ToString() == ConverterSpecialValue.DBNULL)
                    {
                        sbLog.Append("<dbnull>".PadRight(19));
                    }
                    else
                    {
                        sbLog.Append(p.Value.ToString().PadRight(19));
                    }
                }
                #endregion

                if (sqlpar.Direction == EnumDataInterfaceParameterDirection.Output
                    || sqlpar.Direction == EnumDataInterfaceParameterDirection.InputOutput
                    || sqlpar.Direction == EnumDataInterfaceParameterDirection.Reference
                    || sqlpar.Direction == EnumDataInterfaceParameterDirection.ReturnValue)
                {

                    if (p != null)
                    {
                        if (p.Converter != null)
                        {
                            p.Value = p.Converter.ConvertTo(sqlpar.Value);
                        }
                        else
                        {
                            p.Value = sqlpar.Value;
                        }
                    }
                }

                #region log
                if (this.EnabledLog)
                {
                    if (p.Value == null || p.Value.ToString() == ConverterSpecialValue.NULL)
                    {
                        sbLog.Append("<null>".PadRight(19));
                    }
                    else if (p.Value == DBNull.Value || p.Value.ToString() == ConverterSpecialValue.DBNULL)
                    {
                        sbLog.Append("<dbnull>".PadRight(19));
                    }
                    else
                    {
                        sbLog.Append(p.Value.ToString());
                    }
                    sbLog.Append("\r\n");
                }
                #endregion
            }

            #region log
            if (this.EnabledLog)
            {
                LogManager.Logger.LogInfo("接口调用日志", sbLog.ToString());
            }
            #endregion
        }

        internal static DataInterfaceCommand FromDTO(EntityDictDataInterfaceCommand dtoCommand)
        {
            DataInterfaceCommand cmd = new DataInterfaceCommand();
            cmd.CommandText = dtoCommand.cmd_command_text;
            cmd.CommandType = (EnumCommandType)Enum.Parse(typeof(EnumCommandType), dtoCommand.cmd_command_type, true);

            //if (config.Connection != null)
            //    cmd.Connection = DataInterfaceConnection.FromConfig(config.Connection);


            //cmd.Parameters = DataInterfaceParameterCollection.FromConfig(config.Parameters);

            cmd.EnabledLog = (dtoCommand.cmd_enabled_log == 1);

            return cmd;
        }

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

        public override string ToString()
        {
            return string.Format("Type：{0}；Text：{1}", this.CommandType, this.CommandText);
        }
    }
}
