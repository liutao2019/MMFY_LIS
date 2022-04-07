using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 归档架子记录
    /// 旧表名:samp_store_rack 新表名:Sample_store_rack
    /// </summary>
    [Serializable]
    public class EntitySampStoreRack : EntityBase
    {
        public EntitySampStoreRack()
        {
            SrStatus = 0;
            SrAuditDate = DateTime.Now;
            SrDestroyDate = DateTime.Now;
        }
        /// <summary>
        ///归档架子编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_id", MedName = "sr_id", WFName = "Ssr_id")]
        public String SrId { get; set; }

        /// <summary>
        ///架子编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_rack_id", MedName = "sr_rack_id", WFName = "Ssr_Drack_id")]
        public String SrRackId { get; set; }

        /// <summary>
        ///架子状态
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_status", MedName = "sr_status", WFName = "Ssr_status")]
        public Int32 SrStatus { get; set; }

        /// <summary>
        ///审计者名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_chk_name", MedName = "sr_audit_user_name", WFName = "Ssr_audit_user_name")]
        public String SrAuditUserName { get; set; }

        /// <summary>
        ///审计者ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_chk_code", MedName = "sr_audit_user_id", WFName = "Ssr_audit_user_id")]
        public String SrAuditUserId { get; set; }

        /// <summary>
        ///审计日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_chk_date", MedName = "sr_audit_date", WFName = "Ssr_audit_date")]
        public DateTime SrAuditDate { get; set; }

        /// <summary>
        ///归档者名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_store_name", MedName = "sr_store_user_name", WFName = "Ssr_store_user_name")]
        public String SrStoreUserName { get; set; }

        /// <summary>
        ///归档者ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_store_code", MedName = "sr_store_user_code", WFName = "Ssr_store_user_code")]
        public String SrStoreUserCode { get; set; }

        /// <summary>
        ///归档日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_store_date", MedName = "sr_store_date", WFName = "Ssr_store_date")]
        public DateTime SrStoreDate { get; set; }

        /// <summary>
        ///位置
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_place", MedName = "sr_place", WFName = "Ssr_Dpos_id")]
        public String SrPlace { get; set; }

        /// <summary>
        ///冰箱ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_icebox", MedName = "sr_store_id", WFName = "Ssr_Dstore_id")]
        public String SrStoreId { get; set; }

        /// <summary>
        ///数量
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_num", MedName = "sr_amount", WFName = "Ssr_amount")]
        public Int32 SrAmount { get; set; }

        /// <summary>
        ///销毁者名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_des_name", MedName = "sr_Destroy_name", WFName = "Ssr_Destroy_name")]
        public String SrDestroyName { get; set; }

        /// <summary>
        ///销毁者ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_des_code", MedName = "sr_Destroy_code", WFName = "Ssr_Destroy_code")]
        public String SrDestroyCode { get; set; }

        /// <summary>
        ///销毁日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "ss_des_date", MedName = "sr_Destroy_date", WFName = "Ssr_Destroy_date")]
        public DateTime SrDestroyDate { get; set; }

        #region 附加字段1 是否选中 
        /// <summary>
        /// 是否选中 
        /// </summary>
        [FieldMapAttribute(ClabName = "isselected", MedName = "isselected", WFName = "isselected", DBColumn = false)]
        public Int32 IsSelected { get; set; }
        #endregion

        #region 附加字段2  架子编码
        /// <summary>
        /// 架子编码 
        /// </summary>
        [FieldMapAttribute(ClabName = "rack_id", MedName = "rack_id", WFName = "Drack_id", DBColumn = false)]
        public String RackId { get; set; }
        #endregion

        #region 附加字段3  架子名称  
        /// <summary>
        /// 架子名称 
        /// </summary>
        [FieldMapAttribute(ClabName = "rack_name", MedName = "rack_name", WFName = "Dtrack_name", DBColumn = false)]
        public String RackName { get; set; }
        #endregion

        #region 附加字段4  架子规格 
        /// <summary>
        /// 架子规格
        /// </summary>
        [FieldMapAttribute(ClabName = "rack_spec", MedName = "rack_spec", WFName = "Drack_Dtrack_code", DBColumn = false)]
        public String RackSpec { get; set; }
        #endregion

        #region 附加字段5 架子代码 
        /// <summary>
        /// 架子代码 
        /// </summary>
        [FieldMapAttribute(ClabName = "rack_code", MedName = "rack_code", WFName = "Drack_code", DBColumn = false)]
        public String RackCode { get; set; }
        #endregion

        #region 附加字段6  实验组别  
        /// <summary>
        /// 实验组别 
        /// </summary>
        [FieldMapAttribute(ClabName = "rack_ctype", MedName = "rack_type", WFName = "Drack_Dpro_id", DBColumn = false)]
        public String RackType { get; set; }
        #endregion

        #region 附加字段7  架子条码 
        /// <summary>
        /// 架子条码 
        /// </summary>
        [FieldMapAttribute(ClabName = "rack_barcode", MedName = "rack_barcode", WFName = "Drack_barcode", DBColumn = false)]
        public String RackBarcode { get; set; }
        #endregion

        #region 附加字段8  试管架名称
        /// <summary>
        /// 试管架名称
        /// </summary>
        [FieldMapAttribute(ClabName = "cus_name", MedName = "rack_name_new", WFName = "rack_name_new", DBColumn = false)]
        public String RackNameNew { get; set; }
        #endregion

        #region 附加字段9  试管横向孔数
        /// <summary>
        /// 试管横向孔数
        /// </summary>
        [FieldMapAttribute(ClabName = "cus_x_num", MedName = "rack_x_amount", WFName = "Dtrack_x_amount", DBColumn = false)]
        public Int32 RackXAmount { get; set; }
        #endregion

        #region 附加字段10  试管纵向孔数 
        /// <summary>
        /// 试管纵向孔数
        /// </summary>
        [FieldMapAttribute(ClabName = "cus_y_num", MedName = "rack_y_amount", WFName = "Dtrack_y_amount", DBColumn = false)]
        public Int32 RackYAmount { get; set; }
        #endregion

        #region 附加字段11  柜子名称 
        /// <summary>
        /// 柜子名称 
        /// </summary>
        [FieldMapAttribute(ClabName = "cup_name", MedName = "area_name", WFName = "Dpos_name", DBColumn = false)]
        public String AreaName { get; set; }
        #endregion

        #region 附加字段12  冰箱名称 
        /// <summary>
        /// 冰箱名称 
        /// </summary>
        [FieldMapAttribute(ClabName = "ice_name", MedName = "store_name", WFName = "Dstore_name", DBColumn = false)]
        public String StoreName { get; set; }
        #endregion

        #region 附加字段13  组别名称
        /// <summary>
        /// 组别名称
        /// </summary>
        [FieldMapAttribute(ClabName = "type_name", MedName = "pro_name", WFName = "Dpro_name", DBColumn = false)]
        public String ProName { get; set; }
        #endregion

    }
}
