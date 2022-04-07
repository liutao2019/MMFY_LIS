using System;
using System.Collections.Generic;
using System.Data;

namespace Lib.DAC
{
    /// <summary>
    /// 数据库操作类
    /// </summary>
    public class SqlHelper 
    {
        /// <summary>
        /// 连接串
        /// </summary>
        protected string connString = string.Empty;

        /// <summary>
        /// 数据库驱动对象
        /// </summary>
        private IDbDriver Driver { get; set; }


        /// <summary>
        /// 数据库类型
        /// </summary>
        private IDialet Dialet { get; set; }


        protected IDbTransaction transaction = null;

        /// <summary>
        /// 获取或设置在终止执行命令的尝试并生成错误之前的等待时间。
        /// </summary>
        public int CommandTimeout { get; set; }

        #region .ctor

        /// <summary>
        /// .ctor
        /// </summary>
        public SqlHelper()
            : this(DACConfig.Current.ConnectionString, DACConfig.Current.DriverType, DACConfig.Current.DataBaseDialet)
        {
            //this.DriverType = DACConfig.Current.DriverType;
            //this.Driver = DbDriverHelper.GetDriver(DACConfig.Current.DriverType);

            //this.DialetType = DACConfig.Current.DataBaseType;
            //this.Dialet = DbDialetHelper.GetDialet(this.DialetType);

            //this.connString = DACConfig.Current.ConnectionString;
        }

        public SqlHelper(ITransaction tran)
        {
            if (tran == null)
            {
                this.Driver = DbDriverHelper.GetCurrentDriver();
                this.connString = DACConfig.Current.ConnectionString;
                this.Dialet = DbDialetHelper.GetCurrentDialet();
            }
            else
            {
                this.Driver = (tran as AbstractTransaction).Driver;
                this.Dialet = (tran as AbstractTransaction).Dialet;
                this.transaction = (tran as AbstractTransaction).Trans;
            }
            this.CommandTimeout = 30;
        }

        ///// <summary>
        ///// .ctor
        ///// </summary>
        ///// <param name="strConnectionString">数据库连接串</param>
        //public DBHelper(string strConnectionString)
        //{
        //    this.Driver = ConnectionPrivider.Current.CurrentDriver;
        //    this.connString = strConnectionString;
        //}

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="strConnectionString">数据库连接串</param>
        /// <param name="driver">数据库驱动类型</param>
        public SqlHelper(string strConnectionString, EnumDbDriver driver)
            : this(strConnectionString, driver, EnumDataBaseDialet.SQL2005)
        {
            //this.Driver = DbDriverHelper.GetDriver(driver);
            //this.connString = strConnectionString;
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="strConnectionString">>数据库连接串</param>
        /// <param name="driver">数据库驱动类型</param>
        /// <param name="dialet">数据库类型</param>
        public SqlHelper(string strConnectionString, EnumDbDriver driver, EnumDataBaseDialet dialet)
            : this(strConnectionString, DbDriverHelper.GetDriver(driver), DbDialetHelper.GetDialet(dialet))
        {

        }

        public SqlHelper(string strConnectionString, IDbDriver driver, IDialet dialet)
        {
            this.Driver = driver;

            this.connString = strConnectionString;

            this.Dialet = dialet;

            this.CommandTimeout = 30;
        }

        ///// <summary>
        ///// .ctor
        ///// </summary>
        ///// <param name="trans"></param>
        //private SqlHelper(IDbTransaction trans)
        //{
        //    this.Driver = ConnectionPrivider.Current.CurrentDriver;
        //    //this.connString = trans.Connection.ConnectionString;
        //    this.trans_connection = trans.Connection;
        //    this.transaction = trans;
        //}
        #endregion

        #region CreateCommand

        /// <summary>
        /// 创建与当前驱动对应的IDbCommand
        /// </summary>
        /// <returns></returns>
        public IDbCommand CreateCommand()
        {
            IDbCommand cmd = Driver.CreateCommand();
            cmd.CommandTimeout = this.CommandTimeout;
            return cmd;
        }

        /// <summary>
        ///  创建与当前驱动对应的IDbCommand
        /// </summary>
        /// <param name="commandText">sql语句</param>
        /// <returns></returns>
        public IDbCommand CreateCommand(string commandText)
        {
            IDbCommand cmd = CreateCommand();
            cmd.CommandText = commandText;
            cmd.CommandTimeout = this.CommandTimeout;
            return cmd;
        }

        #endregion

        #region AddParameterWidthValue

        ///// <summary>
        ///// 为IDbCommand添加参数与参数值
        ///// </summary>
        ///// <param name="command">需要添加参数的IDbCommand</param>
        ///// <param name="paramName">参数名</param>
        ///// <param name="value">值</param>
        //public void AddParameterWidthValue(IDbCommand command, string paramName, object value)
        //{
        //    //IDbDataParameter param = command.CreateParameter();
        //    //param.ParameterName = paramName;
        //    //param.Value = value;
        //    //command.Parameters.Add(param);

        //    this.AddParameterWidthValue(command, paramName, value, DbType.Object);
        //}

        ///// <summary>
        ///// 为IDbCommand添加参数与参数值，并制定数据类型
        ///// </summary>
        ///// <param name="command">需要添加参数的IDbCommand</param>
        ///// <param name="paramName">参数名</param>
        ///// <param name="value">值</param>
        ///// <param name="dbtype">数据类型</param>
        //public void AddParameterWidthValue(IDbCommand command, string paramName, object value, DbType dbtype)
        //{
        //    IDbDataParameter param = command.CreateParameter();
        //    param.ParameterName = paramName;
        //    param.Value = value;
        //    command.Parameters.Add(param);
        //}
        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// 执行sql并返回影响的行数
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(System.Data.IDbCommand cmd)
        {
            int rowsAffact = -1;
            cmd.CommandTimeout = this.CommandTimeout;
            try
            {
                if (this.transaction != null)
                {
                    if (this.transaction.Connection == null)
                    {
                        throw new InvalidOperationException("事务已终止，已无法再使用");
                    }

                    cmd.Connection = this.transaction.Connection;
                    cmd.Transaction = this.transaction;
                    rowsAffact = cmd.ExecuteNonQuery();
                }
                else
                {
                    using (IDbConnection conn = this.Driver.CreateConnection(this.connString))
                    {
                        if (conn.State != ConnectionState.Open)
                        {
                            conn.Open();
                        }

                        cmd.Connection = conn;
                        rowsAffact = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch //(Exception ex)
            {
                throw;
            }

            return rowsAffact;
        }


        /// <summary>
        /// 执行sql并返回影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            IDbCommand cmd = CreateCommand();
            cmd.CommandText = sql;
            return ExecuteNonQuery(cmd);
        }
        #endregion

        #region GetTable

        /// <summary>
        /// 执行sql并返回第一个数据表
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public DataTable GetTable(IDbCommand cmd)
        {
            DataTable table = new DataTable();
            cmd.CommandTimeout = this.CommandTimeout;
            try
            {
                if (this.transaction != null)//启动事务
                {
                    if (this.transaction.Connection == null)//判断事务是否已终止(提交/回滚)
                    {
                        throw new InvalidOperationException("事务已终止，已无法再使用");
                    }
                    cmd.Connection = this.transaction.Connection;
                    cmd.Transaction = this.transaction;

                    DbDataAdapterEx adapterEx = new DbDataAdapterEx(this.Driver.CreateDbDataAdapter());

                    adapterEx.SelectCommand = cmd;

                    adapterEx.Fill(table);//填充数据
                }
                else
                {
                    using (IDbConnection conn = this.Driver.CreateConnection(this.connString))
                    {
                        if (conn.State != ConnectionState.Open)
                        {
                            conn.Open();
                        }
                        cmd.Connection = conn;

                        DbDataAdapterEx adapterEx = new DbDataAdapterEx(this.Driver.CreateDbDataAdapter());

                        adapterEx.SelectCommand = cmd;
                        adapterEx.Fill(table);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return table;
        }

        /// <summary>
        /// 执行IDbCommand指令集合(实物)，并返回最后一条指令的数据
        /// </summary>
        /// <param name="cmds"></param>
        /// <returns></returns>
        public DataTable GetLastCmdTable(IDbCommand[] cmds)
        {
            DataTable table = new DataTable();

            try
            {
                if (this.transaction != null)//启动事务
                {
                    if (this.transaction.Connection == null)//判断事务是否已终止(提交/回滚)
                    {
                        throw new InvalidOperationException("事务已终止，已无法再使用");
                    }

                    for (int i = 0; i < cmds.Length; i++)
                    {
                        cmds[i].Connection = this.transaction.Connection;
                        cmds[i].Transaction = this.transaction;
                        cmds[i].CommandTimeout = this.CommandTimeout;
                        if (i == cmds.Length - 1)
                        {
                            cmds[i].ExecuteNonQuery();
                        }
                        else
                        {
                            DbDataAdapterEx adapterEx = new DbDataAdapterEx(this.Driver.CreateDbDataAdapter());
                            adapterEx.Fill(table);
                            //reader = cmds[i].ExecuteReader();
                        }
                    }

                    //DbDataAdapterEx adapterEx = new DbDataAdapterEx(this.Driver.CreateDbDataAdapter());

                    //adapterEx.SelectCommand = cmd;

                    //adapterEx.Fill(table);//填充数据
                }
                else
                {
                    using (IDbConnection conn = this.Driver.CreateConnection(this.connString))
                    {
                        if (conn.State != ConnectionState.Open)
                        {
                            conn.Open();
                        }

                        for (int i = 0; i < cmds.Length; i++)
                        {
                            cmds[i].Connection = conn;
                            cmds[i].CommandTimeout = this.CommandTimeout;
                            if (i == cmds.Length - 1)
                            {
                                cmds[i].ExecuteNonQuery();
                            }
                            else
                            {
                                DbDataAdapterEx adapterEx = new DbDataAdapterEx(this.Driver.CreateDbDataAdapter());
                                adapterEx.Fill(table);
                                //reader = cmds[i].ExecuteReader();
                            }
                        }

                        //cmd.Connection = conn;

                        //DbDataAdapterEx adapterEx = new DbDataAdapterEx(this.Driver.CreateDbDataAdapter());

                        //adapterEx.SelectCommand = cmd;
                        //adapterEx.Fill(table);
                    }
                }
            }
            catch// (Exception ex)
            {
                throw;
            }
            return table;
        }

        /// <summary>
        /// 执行sql并返回第一个数据表
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tableName">指定的表名</param>
        /// <returns></returns>
        public DataTable GetTable(IDbCommand cmd, string tableName)
        {
            DataTable table = GetTable(cmd);
            table.TableName = tableName;
            return table;
        }

        /// <summary>
        /// 执行sql并返回第一个数据表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetTable(string sql)
        {
            IDbCommand cmd = CreateCommand();
            cmd.CommandText = sql;

            return GetTable(cmd);
        }

        /// <summary>
        /// 执行sql并返回第一个数据表
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        //public DataTable GetTable(string sql, string sort)
        //{
        //    DataTable table = GetTable(sql);

        //    if (!string.IsNullOrEmpty(sort))
        //    {
        //        table.DefaultView.Sort = sort;
        //        return table.DefaultView.ToTable();
        //    }
        //    else
        //    {
        //        return table;
        //    }
        //}

        /// <summary>
        /// 执行sql并返回第一个数据表
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tableName">指定的表名</param>
        /// <returns></returns>
        public DataTable GetTable(string sql, string tableName)
        {
            DataTable table = GetTable(sql);
            table.TableName = tableName;
            return table;
        }
        #endregion

        #region ExecuteScalar


        /// <summary>
        /// 执行sql并返回第一行第一列的结果
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public object ExecuteScalar(IDbCommand cmd)
        {
            cmd.CommandTimeout = this.CommandTimeout;
            object obj = null;
            try
            {
                if (this.transaction != null)
                {
                    if (this.transaction.Connection == null)
                    {
                        throw new InvalidOperationException("事务已终止，已无法再使用");
                    }
                    cmd.Connection = this.transaction.Connection;
                    cmd.Transaction = this.transaction;

                    obj = cmd.ExecuteScalar();
                }
                else
                {
                    using (IDbConnection conn = this.Driver.CreateConnection(this.connString))
                    {
                        //conn.ConnectionTimeout = _connectionTimeout;
                        if (conn.State != ConnectionState.Open)
                        {
                            conn.Open();
                        }
                        cmd.Connection = conn;
                        obj = cmd.ExecuteScalar();
                    }
                }
            }
            catch// (Exception ex)
            {
                throw;
            }
            return obj;
        }

        /// <summary>
        /// 执行sql并返回第一行第一列的结果
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql)
        {
            IDbCommand cmd = CreateCommand();
            cmd.CommandText = sql;
            return ExecuteScalar(cmd);
        }
        #endregion

        #region GetDataSet


        /// <summary>
        /// 执行sql并返回DataSet
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public DataSet GetDataSet(IDbCommand cmd)
        {
            DataSet ds = new DataSet();

            try
            {
                if (this.transaction != null)//当前dbhelper未启动事务
                {
                    if (this.transaction.Connection == null)//判断事务是否已终止(提交/回滚)
                    {
                        throw new InvalidOperationException("事务已终止，已无法再使用");
                    }
                    cmd.Connection = this.transaction.Connection;
                    cmd.Transaction = this.transaction;

                    DbDataAdapterEx adapterEx = new DbDataAdapterEx(this.Driver.CreateDbDataAdapter());

                    adapterEx.SelectCommand = cmd;
                    adapterEx.Fill(ds);

                    //IDbDataAdapter adapter = this.Driver.CreateDbDataAdapter();//从数据库驱动对象中创建数据适配器

                    //if (cmd is DbCommandEx)//如果是扩展型Command
                    //{
                    //    adapter.SelectCommand = (cmd as DbCommandEx).InnerCommand;
                    //}
                    //else
                    //{
                    //    adapter.SelectCommand = cmd;
                    //}

                    //adapter.Fill(ds);//填充数据
                }
                else
                {
                    using (IDbConnection conn = this.Driver.CreateConnection(this.connString))
                    {
                        //conn.ConnectionTimeout = _connectionTimeout;
                        if (conn.State != ConnectionState.Open)
                        {
                            conn.Open();
                        }
                        cmd.Connection = conn;

                        //IDbDataAdapter adapter = this.Driver.CreateDbDataAdapter();

                        //if (cmd is DbCommandEx)
                        //{
                        //    adapter.SelectCommand = (cmd as DbCommandEx).InnerCommand;
                        //}
                        //else
                        //{
                        //    adapter.SelectCommand = cmd;
                        //}

                        //adapter.Fill(ds);
                        DbDataAdapterEx adapterEx = new DbDataAdapterEx(this.Driver.CreateDbDataAdapter());

                        adapterEx.SelectCommand = cmd;
                        adapterEx.Fill(ds);
                    }
                }
            }
            catch// (Exception ex)
            {
                throw;
            }
            return ds;
        }

        /// <summary>
        /// 执行sql并返回DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string sql)
        {
            IDbCommand cmd = CreateCommand();
            cmd.CommandText = sql;

            return GetDataSet(cmd);
        }
        #endregion

        #region 事务相关
        [Obsolete("使用DACHelper.BeginTransaction()")]
        public ITransaction BeginTransaction()
        {
            return BeginTransaction(IsolationLevel.ReadCommitted);
        }

        [Obsolete("使用DACHelper.BeginTransaction(IsolationLevel il)")]
        public ITransaction BeginTransaction(IsolationLevel il)
        {
            IDbConnection conn = this.Driver.CreateConnection(this.connString);
            conn.Open();
            this.transaction = conn.BeginTransaction(il);

            AdoTransaction tran = new AdoTransaction(this.transaction, this.Driver, this.Dialet);

            return tran;
        }

        #endregion

        /// <summary>
        /// 创建扩展sqlcommand
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DbCommandEx CreateCommandEx(string sql)
        {
            DbCommandEx cmd = new DbCommandEx(this.Driver, this.Dialet, sql);
            cmd.CommandTimeout = this.CommandTimeout;
            return cmd;
        }


        /// <summary>
        /// 创建扩展sqlcommand
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DbCommandEx2 CreateCommandEx2(string sql)
        {
            DbCommandEx2 cmd = new DbCommandEx2(this.Driver, this.Dialet, sql);
            cmd.CommandTimeout = this.CommandTimeout;
            return cmd;
        }


        public string GetInsertSQL(string TableName, Dictionary<String, OracleValue> parm)
        {
            string Fields = "";
            string value = "";
            string insertSQL = "insert into {0} ({1}) values ({2})";
            foreach (var v in parm)
            {
                Fields += "," + v.Key + "";
                string _value = "";
                switch(v.Value._Type)
                {
                    case DataType.DateTime:
                        if(!string.IsNullOrEmpty(v.Value._Value?.ToString()))
                            _value = string.Format("to_date('{0}','yyyy-mm-dd hh24:mi:ss')",v.Value._Value);
                        else
                            _value = "''";
                        break;
                    default:
                        _value = string.Format("'{0}'",v.Value._Value);
                        break;
                }
                value += "," + _value ;
            }
            if (parm.Count > 0)
            {
                Fields = Fields.Substring(1);
                value = value.Substring(1);
            }
            string sql = string.Format(insertSQL, TableName, Fields, value);
            return sql;
        }

        /// <summary>
        /// add this method to GetUpdateSqlBag
        /// 2017.09.15 Tiger
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Values"></param>
        /// <param name="Keys"></param>
        /// <returns></returns>
        public string GetUpdateSql(string TableName, Dictionary<String, object> Values,
            Dictionary<String, object> Keys, bool outPutInsert = false)
        {
            string updateSQL = string.Empty;
            if (outPutInsert)
                updateSQL = "update {0} set {1} where {2}  OUTPUT INSERTED.* ";
            else
                updateSQL = "update {0} set {1} where {2} ";

            string setPart = "";
            string wherePart = "";

            foreach (var v in Values)
            {
                if (v.Value == null)
                    setPart += ",[" + v.Key + "] = NULL ";
                else
                    setPart += ",[" + v.Key + "] = " + "'" + v.Value + "'";

            }
            if (Values.Count > 0)
            {
                setPart = setPart.Substring(1);
            }

            foreach (var v in Keys)
            {
                if (v.Value == null)
                    wherePart += " and [" + v.Key + "] IS NULL ";
                else
                    wherePart += " and [" + v.Key + "] = " + "'" + v.Value + "'";
            }
            if (Keys.Count > 0)
            {
                wherePart = wherePart.Substring(5);
            }
            return string.Format(updateSQL, TableName, setPart, wherePart);
        }

    }

    public enum DataType
    {
        Varchar=1,
        DateTime=2,
    }

    public class OracleValue
    {
        /// <summary>
        /// 数据类型
        /// </summary>
        public DataType _Type = DataType.Varchar;

        /// <summary>
        /// 数据值
        /// </summary>
        public object _Value { get; set; }

        public OracleValue()
        {

        }

        public OracleValue(object value)
        {
            _Value = value;
            _Type = DataType.Varchar;
        }

        public OracleValue(object value, DataType type)
        {
            _Value = value;
            _Type = type;
        }
    }
}
