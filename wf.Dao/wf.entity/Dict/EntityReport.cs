using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 标本状态明细表 
    /// 旧表名:Dic_pub_status 新表名:Dict_status
    /// </summary>
    [Serializable]
    public class EntityDicPubStatus : EntityBase
    {
        /// <summary>
        ///编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_id", MedName = "status_id", WFName = "Dsta_id")]
        public String StatusId { get; set; }

        /// <summary>
        ///状态简称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_name", MedName = "status_name", WFName = "Dsta_name")]
        public String StatusName { get; set; }

        /// <summary>
        ///状态中文简称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_cname", MedName = "status_content", WFName = "Dsta_content")]
        public String StatusContent { get; set; }

        /// <summary>
        ///状态描述
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_desc", MedName = "status_remark", WFName = "Dsta_remark")]
        public String StatusRemark { get; set; }

    }
}
