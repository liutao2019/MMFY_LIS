using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.EntityCore
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class FieldRowVersionAttribute : IEntityPropertyAttribute
    {
        /// <summary>
        /// RowVersion值是否由数据库生成，如果由手动生成，可以在属性中添加FieldDataCustomGenerateAttribute标签
        /// </summary>
        public bool IsGenerateByDB { get; private set; }

        public FieldRowVersionAttribute(bool isGenerateByDB)
        {
            this.IsGenerateByDB = isGenerateByDB;
        }

        public override bool AllowMultiPropertyWithSameAttribute
        {
            get
            {
                return false;
            }
        }

        public override Type[] AllowPropertyTypes
        {
            get
            {
                Type[] t = new Type[4];
                t[0] = typeof(Byte[]);
                t[1] = typeof(DateTime);
                t[2] = typeof(Int32);
                t[3] = typeof(String);
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
