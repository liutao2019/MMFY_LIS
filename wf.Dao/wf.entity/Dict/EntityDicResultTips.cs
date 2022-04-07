using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 数据表实体类：Def_itm_result_tips
    /// 新表名:Rel_itm_result_tips
    /// </summary>
    [Serializable()]
    public class EntityDicResultTips:EntityBase
    {
        public EntityDicResultTips()
        {
            Checked = false;
        }
        /// <summary>
        ///编码
        /// </summary> 
        [FieldMapAttribute(ClabName = "ID", MedName = "tip_id", WFName = "Rtip_id")]
        public Int32 TipId { get; set; }

        /// <summary>
        ///提示值
        /// </summary> 
        [FieldMapAttribute(ClabName = "value", MedName = "tip_value", WFName = "Rtip_value")]
        public String TipValue { get; set; }

        /// <summary>
        ///删除标志
        /// </summary>  
        [FieldMapAttribute(ClabName = "del_flag", MedName = "del_flag", WFName = "del_flag")]
        public Int32 DelFlag { get; set; }

        /// <summary>
        ///提示内容
        /// </summary> 
        [FieldMapAttribute(ClabName = "description", MedName = "tip_content", WFName = "Rtip_content")]
        public String TipContent { get; set; }

        #region 附加字段 统一ID

        /// <summary>
        /// 统一ID
        /// </summary>
        public String SpId
        {
            get
            {
                return TipId.ToString();
            }
        }
        #endregion

        #region 附加字段 是否选中

        /// <summary>
        /// 是否选中
        /// </summary>
        public Boolean Checked { get; set; } 
        #endregion
    }
}
