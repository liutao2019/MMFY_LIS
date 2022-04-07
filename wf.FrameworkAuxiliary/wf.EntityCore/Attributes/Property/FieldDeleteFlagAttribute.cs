using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.EntityCore
{
    /// <summary>
    /// 字段删除标志标签
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class FieldDeleteFlagAttribute : IEntityPropertyAttribute
    {
        /// <summary>
        /// 删除时要设置的值
        /// </summary>
        public object DeletedValue { get; private set; }

        /// <summary>
        /// 取消删除时要设置的值
        /// </summary>
        public object UndeletedValue { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enableValue">此记录为有效时的值</param>
        /// <param name="disableValue">此记录为无效时的值</param>
        public FieldDeleteFlagAttribute(object deletedValue, object undeletedValue)
        {
            this.DeletedValue = deletedValue;
            this.UndeletedValue = undeletedValue;
        }

        /// <summary>
        /// 字段允许的类型
        /// </summary>
        public override Type[] AllowPropertyTypes
        {
            get
            {
                Type[] t = new Type[3];
                t[0] = typeof(bool);
                t[1] = typeof(int);
                t[2] = typeof(string);
                return t;
            }
        }

        public override IList<Type> NotAllowExistAttributeTypes
        {
            get
            {
                List<Type> t = new List<Type>();
                //t.Add(typeof(FieldCreateTimeAttribute));
                //t.Add(typeof(FieldModifyTimeAttribute));
                return t;
            }
        }

        public override IList<Type> AttributeTypesNeeded
        {
            get
            {
                List<Type> t = new List<Type>();
                t.Add(typeof(FieldMapAttribute));
                return t;
            }
        }
    }
}
