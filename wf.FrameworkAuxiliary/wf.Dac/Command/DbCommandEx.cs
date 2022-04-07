using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Lib.DAC.DbDriver;

namespace Lib.DAC
{
    /// <summary>
    /// 扩展DbCommand，使用?作为sql参数，支持不同类型的数据库驱动
    /// </summary>
    public class DbCommandEx : IDbCommand
    {
        /// <summary>
        /// 内部IDbCommand，根据驱动的不同实例化不同的IDbCommand：SqlCommand,OleDbCommand,ODBCDbCommand等
        /// </summary>
        public IDbCommand InnerCommand { get; private set; }

        /// <summary>
        /// 驱动
        /// </summary>
        IDbDriver Driver = null;

        /// <summary>
        /// 数据库类型
        /// </summary>
        IDialet Dialet = null;

        /// <summary>
        /// 封装的SqlString
        /// </summary>
        SqlString sqlstring = null;

        #region .ctor

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="driverType">数据库驱动类型</param>
        public DbCommandEx(EnumDbDriver driverType, EnumDataBaseDialet dialetType)
            : this(driverType, dialetType, null)
        {

        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="driverType">数据库驱动类型</param>
        /// <param name="commandText">支持带?号作为参数sql语句，支持odbc,sql,oracle等的专用参数</param>
        public DbCommandEx(EnumDbDriver driverType, EnumDataBaseDialet dialetType, string commandText)
            : this(DbDriverHelper.GetDriver(driverType), DbDialetHelper.GetDialet(dialetType), commandText)
        {
        }

        public DbCommandEx(IDbDriver driver, IDialet dialet, string commandText)
        {
            this.Driver = driver;
            this.Dialet = dialet;
            this.InnerCommand = this.Driver.CreateCommand();

            if (commandText != null)
            {
                this.CommandText = commandText;
            }
        }
        #endregion

        /// <summary>
        /// 添加sql备注
        /// </summary>
        /// <param name="comment"></param>
        public void AddSqlComment(string comment)
        {
            throw new NotImplementedException();
        }

        #region IDbCommand 成员

        public void Cancel()
        {
            this.InnerCommand.Cancel();
        }

        public string CommandText
        {
            get
            {
                return this.InnerCommand.CommandText;
            }
            set
            {
                //把带？号的sql语句转换成SqlString对象
                this.sqlstring = SqlString.Parse(value);
                this.commandParameters = this.sqlstring.GetParameter();

                SqlParameterFormatter formatter = new SqlParameterFormatter();

                //格式化sql字符串，根据驱动类型生成参数的可执行的sql语句
                string formattedString = formatter.GetFormatedSQL(this.sqlstring, this.Driver);

                //设置当前参数起始值
                this.currentParamIndex = 1;
                this.InnerCommand.CommandText = formattedString;
            }
        }

        public int CommandTimeout
        {
            get
            {
                return this.InnerCommand.CommandTimeout;
            }
            set
            {
                this.InnerCommand.CommandTimeout = value;
            }
        }

        public CommandType CommandType
        {
            get
            {
                return this.InnerCommand.CommandType;
            }
            set
            {
                this.InnerCommand.CommandType = value;
            }
        }

        public IDbConnection Connection
        {
            get
            {
                return this.InnerCommand.Connection;
            }
            set
            {
                this.InnerCommand.Connection = value;
            }
        }

        public IDbDataParameter CreateParameter()
        {
            return this.InnerCommand.CreateParameter();
        }

        [Obsolete]
        public int ExecuteNonQuery()
        {
            return this.InnerCommand.ExecuteNonQuery();
        }

        [Obsolete]
        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            return this.InnerCommand.ExecuteReader(behavior);
        }

        [Obsolete]
        public IDataReader ExecuteReader()
        {
            return this.InnerCommand.ExecuteReader();
        }

        [Obsolete]
        public object ExecuteScalar()
        {
            return this.InnerCommand.ExecuteScalar();
        }

        public IDataParameterCollection Parameters
        {
            get { return this.InnerCommand.Parameters; }
        }

        public void Prepare()
        {
            this.InnerCommand.Prepare();
        }

        public IDbTransaction Transaction
        {
            get
            {
                return this.InnerCommand.Transaction;
            }
            set
            {
                this.InnerCommand.Transaction = value;
            }
        }

        public UpdateRowSource UpdatedRowSource
        {
            get
            {
                return this.InnerCommand.UpdatedRowSource;
            }
            set
            {
                this.InnerCommand.UpdatedRowSource = value;
            }
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            this.InnerCommand.Dispose();
        }

        #endregion

        #region AddParameterWithValue

        /// <summary>
        /// 添加指定名称的参数并赋值（使用带'@'或':'开头的这类型参数时使用）
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IDbDataParameter AddParameterWithValue(string paramName, object value)
        {
            Type datatype = value.GetType();

            DbType dbtype = Lib.DAC.DBDataTypeConverter.SystemTypeToDbType(datatype);

            return AddParameterWithValue(paramName, value, dbtype);
        }

        /// <summary>
        /// 添加指定名称的参数并赋值（使用带'@'或':'开头的这类型参数时使用）
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="value"></param>
        /// <param name="dbDataType"></param>
        /// <returns></returns>
        public IDbDataParameter AddParameterWithValue(string paramName, object value, DbType dbDataType)
        {
            IDbDataParameter param = this.InnerCommand.CreateParameter(); //this.Driver.GenerateParameter(this.innerCommand, paramName);
            param.ParameterName = paramName;

            if (value == null || value == DBNull.Value)
            {
                param.Value = DBNull.Value;
            }
            else
            {
                if (dbDataType == DbType.DateTime)
                {
                    DateTime dtValue;
                    //处理日期类型数据
                    if (value is DateTime)
                    {
                        dtValue = (DateTime)value;
                    }
                    else
                    {
                        dtValue = Convert.ToDateTime(value);
                    }

                    if (dtValue > this.Dialet.DateTimeMaxValue)
                    {
                        dtValue = this.Dialet.DateTimeMaxValue;
                    }
                    else if (dtValue < this.Dialet.DateTimeMinValue)
                    {
                        dtValue = this.Dialet.DateTimeMinValue;
                    }
                    param.Value = dtValue;
                }
                else
                {
                    param.Value = value;
                }
            }

            if (dbDataType != DbType.Object)
            {
                param.DbType = dbDataType;
            }

            this.InnerCommand.Parameters.Add(param);

            return param;
        }
        #endregion

        #region AddParameterValue

        /// <summary>
        /// 当使用'?'作为参数时，按顺序给参数赋值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IDbDataParameter AddParameterValue(object value)
        {
            //Type datatype = value.GetType();
            //DbType dbtype = Lib.DAC.DBTypeConverter.SystemTypeToDbType(datatype);
            //return AddParameterValue(value, dbtype);

            return AddParameterValue(value, DbType.Object);
        }


        int currentParamIndex = 1;
        List<SqlStringParameter> commandParameters = null;

        /// <summary>
        /// 当使用'?'作为参数时，按顺序给参数赋值，并且指定数据类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dbDataType"></param>
        /// <returns></returns>
        public IDbDataParameter AddParameterValue(object value, DbType dbDataType)
        {
            IDbDataParameter param = null;
            if (this.sqlstring != null)
            {
                if (commandParameters.Count >= currentParamIndex)
                {
                    string paramname = this.Driver.GetParameterName(currentParamIndex);

                    param = this.AddParameterWithValue(paramname, value, dbDataType);

                    currentParamIndex++;
                }
            }
            return param;
        }
        #endregion

        public override string ToString()
        {
            return this.CommandText;
        }
    }
}
