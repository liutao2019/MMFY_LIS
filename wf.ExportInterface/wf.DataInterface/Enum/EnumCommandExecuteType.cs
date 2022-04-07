using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DataInterface
{
    public enum EnumCommandExecuteType
    {
        /// <summary>
        /// 执行并返回受影响的记录数
        /// </summary>
        ExecuteNonQuery,

        /// <summary>
        /// 执行并返回第一行第一列的数据
        /// 执行并返回返回值
        /// </summary>
        ExecuteScalar,

        /// <summary>
        /// 执行并返回DataTable
        /// </summary>
        ExecuteGetDataTable,

        /// <summary>
        /// 执行并返回DataSet
        /// </summary>
        ExecuteGetDataSet
    }
}
