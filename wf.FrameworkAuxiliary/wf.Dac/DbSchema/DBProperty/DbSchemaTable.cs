using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC.DbSchema
{
    /// <summary>
    /// 数据库表信息
    /// </summary>
    public class DbSchemaTable
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 拥有者
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// 列
        /// </summary>
        public DbSchemaColumnCollection Columns { get; set; }


        public DbSchemaTable()
        {
            this.Columns = null;
        }

        public override string ToString()
        {
            return this.TableName;
        }
    }
}
