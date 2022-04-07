using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Lib.DataInterface
{
    interface IDataInterfaceCommand
    {
        /// <summary>
        /// 命令执行内容
        /// </summary>
        string CommandText { get; set; }

        /// <summary>
        /// 命令类型
        /// </summary>
        EnumCommandType CommandType { get; set; }

        /// <summary>
        /// 接口
        /// </summary>
        IDataInterfaceConnection Connection { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        IDataInterfaceParameterCollection Parameters { get; }

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        IDataInterfaceParameter CreateParameter(string paramName);

        /// <summary>
        /// 执行命令，返回受影响的记录数
        /// </summary>
        /// <returns></returns>
        int ExecuteNonQuery();

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <returns></returns>
        DataTable ExecuteGetDataTable();

        /// <summary>
        /// 获取DataSet
        /// </summary>
        /// <returns></returns>
        DataSet ExecuteGetDataSet();

        /// <summary>
        /// 返回第一行第一列数据
        /// </summary>
        /// <returns></returns>
        object ExecuteScalar();

        int GetHashCode();
    }
}
