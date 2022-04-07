using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// CA认证表 ca_sign
    /// </summary>
    [Serializable]
    public class EntityCaSign : EntityBase
    {
        public EntityCaSign()
        {
        }
        /// <summary>
        ///CA Id 主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "ca_id", MedName = "ca_id", WFName = "ca_id")]
        public String CaId { get; set; }

        /// <summary>
        ///CA认证时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "ca_date", MedName = "ca_date", WFName = "ca_date")]
        public DateTime CaDate { get; set; }

        /// <summary>
        ///身份证号
        /// </summary>   
        [FieldMapAttribute(ClabName = "ca_cerid", MedName = "ca_cerid", WFName = "ca_cerid")]
        public String CaCerId { get; set; }

        /// <summary>
        ///登录ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "ca_login_id", MedName = "ca_login_id", WFName = "ca_login_id")]
        public String CaLoginId { get; set; }

        /// <summary>
        ///CA名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "ca_name", MedName = "ca_name", WFName = "ca_name")]
        public String CaName { get; set; }

        /// <summary>
        ///CA结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "ca_event", MedName = "ca_event", WFName = "ca_event")]
        public String CaEvent { get; set; }

        /// <summary>
        ///CA备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "ca_remark", MedName = "ca_remark", WFName = "ca_remark")]
        public String CaRemark { get; set; }

        /// <summary>
        ///CA唯一标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "ca_entity_id", MedName = "ca_entity_id", WFName = "ca_entity_id")]
        public String CaEntityId { get; set; }
        /// <summary>
        ///签名原文
        /// </summary>   
        [FieldMapAttribute(WFName = "ca_source_content")]
        public String CaSourceContent { get; set; }

        /// <summary>
        ///签名结果
        /// </summary>   
        [FieldMapAttribute(WFName = "ca_sign_content")]
        public String CaSignContent { get; set; }

        /// <summary>
        ///时间戳原文
        /// </summary>   
        [FieldMapAttribute(WFName = "ca_source_timestamp")]
        public String CaSourceTimestamp { get; set; }

        /// <summary>
        ///时间戳结果
        /// </summary>   
        [FieldMapAttribute(WFName = "ca_sign_timestamp")]
        public String CaSignTimestamp { get; set; }


    }
}
