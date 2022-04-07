using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC.DbSchema
{
    public class DbSchemaColumn
    {

        public DbSchemaColumn(string tableName, string columnName)
        {
            this.TableName = tableName;
            this.ColumnName = columnName;
            this.IsPrimaryKey = false;
        }

        /// <summary>
        /// 表明
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { get; private set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 是否可为空
        /// </summary>
        public bool Nullable { get; set; }

        /// <summary>
        /// 是否为主键
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public int? DataLength { get; set; }

        public int? DataPrecision { get; set; }

        public int? DataScale { get; set; }

        /// <summary>
        /// 在表中的顺序
        /// </summary>
        public int OrdinalPosition { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Description { get; set; }


        public override string ToString()
        {
            string txt = string.Format("{0}   {1}", ColumnName, DataType, DataLength);
            return txt;
        }

    }
}
