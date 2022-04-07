using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace Lib.DAC
{
    public interface IDbDriver
    {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        IDbConnection CreateConnection();

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        IDbConnection CreateConnection(string connectionString);

        /// <summary>
        /// 创建DbCommand
        /// </summary>
        /// <returns></returns>
        IDbCommand CreateCommand();

        /// <summary>
        /// 创建数据适配器
        /// </summary>
        /// <returns></returns>
        IDbDataAdapter CreateDbDataAdapter();

        IDbDataParameter GenerateParameter(IDbCommand command, string paramName);
        string GetParameterName(int index);


        bool SupportsMultipleQueries { get; }
        string MultipleQueriesSeparator { get; }
    }
}
