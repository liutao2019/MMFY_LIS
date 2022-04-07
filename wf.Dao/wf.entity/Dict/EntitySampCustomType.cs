using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public class EntitySampCustomType : EntityBase
    {
        /// <summary>
        ///药敏标本组ID
        /// </summary>   
        public String ZoneSamCustomType { get; set; }

        /// <summary>
        ///药敏标本组名称
        /// </summary>   
        public String ZoneSamCustomName { get; set; }
    }
}
