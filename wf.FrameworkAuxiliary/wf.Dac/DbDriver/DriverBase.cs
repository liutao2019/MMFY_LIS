using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Lib.DAC;

namespace Lib.DAC.DbDriver
{
    /// <summary>
    /// 数据库驱动基类
    /// </summary>
    internal abstract class DriverBase : IDbDriver// : ISqlFormatter
    {
        /// <summary>
        /// 创建数据库连接对象
        /// </summary>
        /// <returns></returns>
        public abstract IDbConnection CreateConnection();

        public abstract IDbDataAdapter CreateDbDataAdapter();

        public abstract IDbCommand CreateCommand();

        /// <summary>
        /// 根据数据库连接串创建数据库连接对象
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public virtual IDbConnection CreateConnection(string connectionString)
        {
            IDbConnection conn = CreateConnection();
            conn.ConnectionString = connectionString;
            return conn;
        }

        /// <summary>
        /// 执行的sql语句中，参数是否使用前缀
        /// </summary>
        public abstract bool UseNamedPrefixInSql { get; }

        /// <summary>
        /// DbCommand中参数是否使用前缀
        /// </summary>
        public abstract bool UseNamedPrefixInParameter { get; }

        /// <summary>
        /// 参数前缀
        /// sql ： '@'
        /// oracle ： ':'
        /// </summary>
        public abstract string NamedPrefix { get; }

        /// <summary>
        /// 格式化sql语句中的参数
        /// </summary>
        /// <param name="parameterName">变量名</param>
        /// <returns></returns>
        public string FormatNameForSql(string parameterName)
        {
            return UseNamedPrefixInSql ? (NamedPrefix + parameterName) : "?";
        }

        /// <summary>
        /// 格式化DbParameter中参数的名称
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public string FormatNameForParameter(string parameterName)
        {
            return UseNamedPrefixInParameter ? (NamedPrefix + parameterName) : parameterName;
        }

        /// <summary>
        /// 是否支持多查询(单此操作中是否支持多条查询语句)
        /// </summary>
        public virtual bool SupportsMultipleQueries
        {
            get { return false; }
        }

        /// <summary>
        /// 多查询分割符
        /// </summary>
        public virtual string MultipleQueriesSeparator
        {
            get { return ";"; }
        }

        /// <summary>
        /// 根据IDbCommand创建IDbDataParameter,并指定参数名
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="sqlType"></param>
        /// <returns></returns>
        public IDbDataParameter GenerateParameter(IDbCommand command, string paramName)
        {
            IDbDataParameter dbParam = command.CreateParameter();
            InitializeParameter(dbParam, paramName);

            return dbParam;
        }

        protected virtual void InitializeParameter(IDbDataParameter dbParam, string paramName)
        {
            //if (sqlType == null)
            //{
            //    throw new QueryException(String.Format("No type assigned to parameter '{0}'", name));
            //}

            dbParam.ParameterName = FormatNameForParameter(paramName);
            //dbParam.DbType = sqlType.DbType;
        }

        /// <summary>
        /// 创建参数名，默认 "p"+序号
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private static string ToParameterName(int index)
        {
            return "p" + index;
        }

        public string GetParameterName(int index)
        {
            return FormatNameForSql(ToParameterName(index));
        }

    }
}
