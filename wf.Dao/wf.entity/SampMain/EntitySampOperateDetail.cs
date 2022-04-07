using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 标本扩展表
    /// 旧表名:Samp_operate_detail 新表名:Sample_operate_detail 
    /// </summary>
    [Serializable]
    public class EntitySampOperateDetail
    {
        /// <summary>
        ///条码号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_bar_code", MedName = "samp_bar_code", WFName = "Soper_Sma_bar_id")]
        public String SampBarCode { get; set; }

        /// <summary>
        ///架子号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_rack_no", MedName = "rack_no", WFName = "Soper_rack_no")]
        public String RackNo { get; set; }

        /// <summary>
        ///孔位
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_rack_port", MedName = "rack_position", WFName = "Soper_position")]
        public String RackPosition { get; set; }

        /// <summary>
        ///分类位置
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_archive_position", MedName = "archive_position", WFName = "Soper_archive_position")]
        public String ArchivePosition { get; set; }

        /// <summary>
        ///存储位置
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_classification_position", MedName = "sorting_position", WFName = "Soper_sorting_position")]
        public String SortingPosition { get; set; }

        /// <summary>
        ///操作时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_rsa_scan_time", MedName = "rsa_scan_date", WFName = "Soper_scan_date")]
        public DateTime RsaScanDate { get; set; }

        /// <summary>
        ///标本状态
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_sample_status", MedName = "samp_status", WFName = "Soper_sam_status")]
        public String SampStatus { get; set; }

    }
}
