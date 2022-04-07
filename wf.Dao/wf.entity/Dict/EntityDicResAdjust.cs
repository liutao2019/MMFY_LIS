using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 仪器结果调整表
    /// 旧表名:Def_itr_res_adjust 新表名:Rel_itr_res_adjust
    /// </summary>
    [Serializable]
    
    public class EntityDicResAdjust : EntityBase
    {
        public EntityDicResAdjust()
        {
            ResDecPlace = 0;
            ResMultiple = 0;
        }
        /// <summary>
        /// 主键
        /// </summary>            
        [FieldMapAttribute(ClabName = "adj_key", MedName = "adj_key", WFName = "Rira_id")]
        public Int32 Adjkey { get; set; }

        /// <summary>
        /// 仪器代码
        /// </summary>            
        [FieldMapAttribute(ClabName = "Instrmt_no", MedName = "itr_id", WFName = "Rira_Ditr_id")]           
        public String ItrId { get; set; }

        /// <summary>
        /// 通道码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "cno", MedName = "mit_cno", WFName = "Rira_mit_cno")]
        public String MitCno { get; set; }


        /// <summary>
        /// 原结果
        /// </summary>                       
        [FieldMapAttribute(ClabName = "src_res", MedName = "src_res", WFName = "Rira_src_res")]
        public String SrcRes { get; set; }

        /// <summary>
        /// 原样本号
        /// </summary>                       
        [FieldMapAttribute(ClabName = "src_sid", MedName = "src_sid", WFName = "Rira_src_sid")]
        public String SrcSid { get; set; }

        /// <summary>
        /// 结果调整因子
        /// </summary>                       
        [FieldMapAttribute(ClabName = "multiple", MedName = "res_multiple", WFName = "Rira_multiple")]
        public float ResMultiple { get; set; }

        /// <summary>
        /// 小数保留位数
        /// </summary>                       
        [FieldMapAttribute(ClabName = "decimal_place", MedName = "res_dec_place", WFName = "Rira_dec_place")]
        public Int32 ResDecPlace { get; set; }

        /// <summary>
        /// 替换结果
        /// </summary>                       
        [FieldMapAttribute(ClabName = "dst_res", MedName = "dst_res", WFName = "Rira_dst_res")]
        public String DstRes { get; set; }
        /// <summary>
        /// 替换样本号
        /// </summary>                       
        [FieldMapAttribute(ClabName = "dst_sid", MedName = "dst_sid", WFName = "Rira_dst_sid")]
        public String DstSid { get; set; }
    }
}
