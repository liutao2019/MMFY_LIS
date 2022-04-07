using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.EntityCore
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class FieldAssociationAttribute : IEntityPropertyAttribute
    {
        public string TableAssociationName { get; set; }
        public string OtherField { get; set; }
        public string Alias { get; set; }

        public override IList<Type> AttributeTypesNeeded
        {
            get
            {
                List<Type> t = new List<Type>();
                t.Add(typeof(FieldMapAttribute));
                return t;
            }
        }

        public override bool AllowMultiPropertyWithSameAttribute
        {
            get
            {
                return false;
            }
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
    }
}
