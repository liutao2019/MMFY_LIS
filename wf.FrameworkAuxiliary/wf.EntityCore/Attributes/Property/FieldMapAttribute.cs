using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Lib.EntityCore
{
    /// <summary>
    /// 数据库字段-实体属性映射标签
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class FieldMapAttribute : IEntityPropertyAttribute
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string DBColumnName { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public DbType DbType { get; set; }

        private string displayName = null;

        /// <summary>
        /// 显示名
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(displayName) && displayName.Trim() != string.Empty)
                {
                    return displayName;
                }
                else
                {
                    return DBColumnName;
                }
            }
            set
            {
                displayName = value;
            }
        }

        /// <summary>
        /// 是否为主键
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        private bool isNullable = true;

        /// <summary>
        /// 是否可空
        /// </summary>
        public bool IsNullable
        {
            get
            {
                if (this.IsPrimaryKey)
                {
                    return false;
                }
                else
                {
                    return isNullable;
                }
            }
            set
            {
                if (this.IsPrimaryKey)
                {
                    isNullable = false;
                }
                else
                {
                    isNullable = value;
                }
            }
        }

        /// <summary>
        /// 是否由数据库生成
        /// </summary>
        public bool IsDBGenerate { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// 排列顺序
        /// </summary>
        public int Ordinal { get; set; }

        public FieldMapAttribute()
        {
            this.IsPrimaryKey = false;
            this.IsDBGenerate = false;
            this.Visible = true;
            this.Ordinal = 0;
        }

        public override string ToString()
        {
            return this.DBColumnName;
        }
    }
}
