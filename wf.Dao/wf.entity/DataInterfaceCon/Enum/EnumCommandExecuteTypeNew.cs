using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    public enum EnumCommandExecuteTypeNew
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
