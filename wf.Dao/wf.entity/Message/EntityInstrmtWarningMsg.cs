using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public class EntityInstrmtWarningMsg : EntityBase
    {
        /// <summary>
        ///报警ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "warn_id", MedName = "warn_id", WFName = "warn_id")]
        public Int64 WarnId { get; set; }

        /// <summary>
        ///病人ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "warn_pat_id", MedName = "warn_pat_id", WFName = "warn_pat_id")]
        public String WarnPatId { get; set; }

        /// <summary>
        ///项目ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "warn_item_id", MedName = "warn_item_id", WFName = "warn_item_id")]
        public String WarnItemId { get; set; }

        /// <summary>
        ///报警信息代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "warn_msg_code", MedName = "warn_msg_code", WFName = "warn_msg_code")]
        public String WarnMsgCode { get; set; }

        /// <summary>
        ///报警信息
        /// </summary>   
        [FieldMapAttribute(ClabName = "warn_msg", MedName = "warn_msg", WFName = "warn_msg")]
        public String WarnMsg { get; set; }

        /// <summary>
        ///复查类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "warn_recheck_type", MedName = "warn_recheck_type", WFName = "warn_recheck_type")]
        public String WarnRecheckType { get; set; }

        /// <summary>
        ///报警日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "warn_date", MedName = "warn_date", WFName = "warn_date")]
        public DateTime WarnDate { get; set; }

        #region 附加字段 项目代码
        /// <summary>
        ///项目代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_ecd", MedName = "itm_ecode", WFName = "Ditm_ecode")]
        public String ItmEcode { get; set; }
        #endregion
    }
}
