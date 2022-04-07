using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 排班模板表
    /// 旧表名:dic_oa_shift_template 新表名:Dict_oa_duty_template
    /// </summary>
    [Serializable]
    public class EntityOaShiftTemplate : EntityBase
    {

        /// <summary>
        ///模板id
        /// </summary>   
        [FieldMapAttribute(ClabName = "", MedName = "temp_id", WFName = "Ddtem_id")]
        public String TempId { get; set; }

        /// <summary>
        ///模板名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "", MedName = "temp_name", WFName = "Ddtem_name")]
        public String TempName { get; set; }

        /// <summary>
        ///模板文件
        /// </summary>   
        [FieldMapAttribute(ClabName = "", MedName = "temp_file", WFName = "Ddtem_file")]
        public Byte[] TempFile { get; set; }

        /// <summary>
        ///模板类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "", MedName = "temp_type", WFName = "Ddtem_type")]
        public String TempType { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "", MedName = "temp_user_id", WFName = "Ddtem_Buser_id")]
        public String TempUserId { get; set; }

        /// <summary>
        ///备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "", MedName = "temp_remark", WFName = "Ddtem_remark")]
        public String TempRemark { get; set; }
    }
}
