using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.EntityCore
{
    public abstract class IEntityPropertyAttribute : Attribute
    {
        /// <summary>
        /// 是否允许多个属性拥有此attribute
        /// </summary>
        public virtual bool AllowMultiPropertyWithSameAttribute
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 允许的属性类型
        /// </summary>
        public virtual Type[] AllowPropertyTypes
        {
            get
            {
                //null为全部
                return null;
            }
        }

        /// <summary>
        /// 不允许并存的标签,null为没有不允许
        /// </summary>
        public virtual IList<Type> NotAllowExistAttributeTypes
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// 必须与此标签共存的其他标签,null表示没有必须并存的标签
        /// </summary>
        public virtual IList<Type> AttributeTypesNeeded
        {
            get
            {
                return null;
            }
        }
    }
}
