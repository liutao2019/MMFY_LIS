using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldMapAttribute : Attribute
    {
        public FieldMapAttribute()
        {
            DBColumn = true;
            DBIdentity = false;
        }

        /// <summary>
        /// Clab数据库字段名称
        /// </summary>
        public string ClabName { get; set; }

        /// <summary>
        /// Med数据库字段名称
        /// </summary>
        public string MedName { get; set; }

        /// <summary>
        /// 是否是数据库的列。默认为是。
        /// </summary>
        public bool DBColumn { get; set; }

        /// <summary>
        /// 是否是数据库自增列。默认为否
        /// </summary>
        public bool DBIdentity { get; set; }

        /// <summary>
        /// WF数据库名称
        /// </summary>
        public string WFName { get; set; }
    }
}
