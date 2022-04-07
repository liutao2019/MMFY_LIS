using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public class EntityKeyValue : EntityBase
    {
        /// <summary>
        ///编码
        /// </summary>   
        public int key { get; set; }

        /// <summary>
        ///名称
        /// </summary>   
        public String name { get; set; }
    }
}
