using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 图像报告表
    /// 旧表名:Obr_result_image 新表名:Lis_result_image
    /// </summary>
    [Serializable]
    public class EntityObrResultImage : EntityBase
    { 
        /// <summary>
        ///标识ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "pres_id", MedName = "obr_id", WFName = "Lri_Lresdesc_id")]
        public String ObrId { get; set; }

        /// <summary>
        ///项目代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "pres_it_ecd", MedName = "obr_itm_ename", WFName = "Lri_Ditm_ename")]
        public String ObrItmEname { get; set; }

        /// <summary>
        ///图象日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "pres_date", MedName = "obr_date", WFName = "Lri_date")]
        public DateTime ObrDate { get; set; }

        /// <summary>
        ///样本号
        /// </summary>   
        [FieldMapAttribute(ClabName = "pres_sid", MedName = "obr_sid", WFName = "Lri_sid")]
        public Decimal ObrSid { get; set; }

        /// <summary>
        ///仪器代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "pres_mid", MedName = "obr_itr_id", WFName = "Lri_Ditr_id")]
        public String ObrItrId { get; set; }

        /// <summary>
        ///图象文件(BIN)
        /// </summary>   
        [FieldMapAttribute(ClabName = "pres_chr", MedName = "obr_image", WFName = "Lri_image")]
        public Byte[] ObrImage { get; set; }

        /// <summary>
        ///标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "pres_flag", MedName = "obr_flag", WFName = "Lri_flag")]
        public Int32 ObrFlag { get; set; }

        /// <summary>
        ///图像文件(BASE64) 不再使用
        /// </summary>   
        [FieldMapAttribute(ClabName = "pres_base64", MedName = "obr_base64", WFName = "Lri_base64")]
        public String ObrBase64 { get; set; }

        /// <summary>
        ///数据记录唯一标识,自增
        /// </summary>   
        [FieldMapAttribute(ClabName = "pres_key", MedName = "obr_sn", WFName = "Lri_id", DBIdentity = true)]
        public Int64 ObrSn { get; set; }

        /// <summary>
        ///路径
        /// </summary>   
        [FieldMapAttribute(ClabName = "pres_path", MedName = "obr_path", WFName = "Lri_path")]
        public String ObrPath { get; set; }
    }
}
