using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC.DbSchema
{
    public class DbSchemaView
    {
        /// <summary>
        /// 视图名
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// 拥有者
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// 列
        /// </summary>
        public DbSchemaColumnCollection Columns { get; set; }

        /// <summary>
        /// 视图定义
        /// </summary>
        public string Difinition { get; set; }

        public DbSchemaView()
        {
            this.Columns = null;
        }

        public override string ToString()
        {
            return this.ViewName;
        }
    }
}
