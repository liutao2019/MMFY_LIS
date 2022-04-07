using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.EntityCore
{
    /// <summary>
    /// 实体-数据库表映射属性
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class EntityTableAttribute : IEntityAttribute
    {
        /// <summary>
        /// 数据库表名
        /// </summary>
        public string TableName { get; set; }

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
                    return TableName;
                }
            }
            set
            {
                displayName = value;
            }
        }

        //public EntityTableAttribute(string TableName)
        //{
        //    this.TableName = TableName;
        //}

        public EntityTableAttribute()
        {

        }
    }
}
