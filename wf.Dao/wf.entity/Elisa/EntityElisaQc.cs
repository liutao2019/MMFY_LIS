using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 酶标质控实体
    /// </summary>
    [Serializable]
    public class EntityElisaQc : EntityBase
    {
        /// <summary>
        ///仪器编号
        /// </summary>   
        public String QcItrId { get; set; }

        /// <summary>
        ///质控日期
        /// </summary>   
        public DateTime QcDate { get; set; }

        /// <summary>
        ///项目ID
        /// </summary>   
        public String QcItmId { get; set; }

        /// <summary>
        ///质控类型
        /// </summary>   
        public String QcNoType { get; set; }

        /// <summary>
        ///质控值
        /// </summary>   
        public String QcValue { get; set; }

    }
}
