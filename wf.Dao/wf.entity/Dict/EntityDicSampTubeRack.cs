using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 架子信息
    /// 旧表名:dic_samp_tube_rack 新表名:Dict_sample_tube_rack
    /// </summary>
    [Serializable]
    public class EntityDicSampTubeRack : EntityBase
    {
        public EntityDicSampTubeRack()
        {
            SrAmount = 0;
            RackStatus = 0;
            RackDelFlag = 0;
            RackPrintFlag = 0;
            SrStatus = 0;
            RackCreatetime = DateTime.Now;
        }
        /// <summary>
        ///架子编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "rack_id", MedName = "rack_id", WFName = "Drack_id")]
        public String RackId { get; set; }

        /// <summary>
        ///架子名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "rack_name", MedName = "rack_name", WFName = "Drack_name")]
        public String RackName { get; set; }

        /// <summary>
        ///规格
        /// </summary>   
        [FieldMapAttribute(ClabName = "rack_spec", MedName = "rack_spec", WFName = "Drack_Dtrack_code")]
        public String RackSpec { get; set; }

        /// <summary>
        ///架子代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "rack_code", MedName = "rack_code", WFName = "Drack_code")]
        public String RackCode { get; set; }

        /// <summary>
        ///实验组别
        /// </summary>   
        [FieldMapAttribute(ClabName = "rack_ctype", MedName = "rack_type", WFName = "Drack_Dpro_id")]
        public String RackType { get; set; }

        /// <summary>
        ///架子状态
        /// </summary>   
        [FieldMapAttribute(ClabName = "rack_status", MedName = "rack_status", WFName = "Drack_status")]
        public Int32 RackStatus { get; set; }

        /// <summary>
        ///架子条码
        /// </summary>   
        [FieldMapAttribute(ClabName = "rack_barcode", MedName = "rack_barcode", WFName = "Drack_barcode")]
        public String RackBarcode { get; set; }

        /// <summary>
        ///生成时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "rack_createtime", MedName = "createtime", WFName = "Drack_createtime")]
        public DateTime RackCreatetime { get; set; }

        /// <summary>
        ///删除标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "rack_del", MedName = "del_flag", WFName = "del_flag")]
        public Int32 RackDelFlag { get; set; }

        /// <summary>
        ///打印标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "print_flag", MedName = "print_flag", WFName = "Drack_print_flag")]
        public Int32 RackPrintFlag { get; set; }

        /// <summary>
        ///架子颜色
        /// </summary>   
        [FieldMapAttribute(ClabName = "rack_color", MedName = "rack_colour", WFName = "Drack_colour")]
        public String RackColour { get; set; }

        /// <summary>
        ///周
        /// </summary>   
        [FieldMapAttribute(ClabName = "rack_week", MedName = "rack_week", WFName = "Drack_week")]
        public String RackWeek { get; set; }

       

        #region 附加字段 试管架名称
        [FieldMapAttribute(ClabName = "cus_code", MedName = "cus_code", WFName = "Dtrack_code", DBColumn = false)]
        public String CusCode { get; set; }

        [FieldMapAttribute(ClabName = "rack_cup_name", MedName = "", WFName = "", DBColumn = false)]
        public String RackCupName { get; set; }
        #endregion

        #region 附加字段 试管架名称
        [FieldMapAttribute(ClabName = "cus_name", MedName = "cus_name", WFName = "cus_name", DBColumn = false)]
        public String CusName { get; set; }
        #endregion
        
        #region 附加字段 试管横向孔数
        [FieldMapAttribute(ClabName = "cus_x_num", MedName = "rack_x_amount", WFName = "Dtrack_x_amount", DBColumn = false)]
        public Int32 RackXAmount { get; set; }
        #endregion

        #region 附加字段 试管纵向孔数
        [FieldMapAttribute(ClabName = "cus_y_num", MedName = "rack_y_amount", WFName = "Dtrack_y_amount", DBColumn = false)]
        public Int32 RackYAmount { get; set; }
        #endregion

        #region 附加字段 组别
        [FieldMapAttribute(ClabName = "type_name", MedName = "pro_name", WFName = "Dpro_name", DBColumn = false)]
        public String ProName { get; set; }
        #endregion

        #region 附加字段 归档架子编码
        /// <summary>
        /// 归档架子编码
        /// </summary>
        [FieldMapAttribute(ClabName = "ss_id", MedName = "sr_id", WFName = "Ssr_id", DBColumn = false)]
        public String SrId { get; set; }
        #endregion

        #region 附加字段 架子状态
        /// <summary>
        /// 架子状态
        /// </summary>
        [FieldMapAttribute(ClabName = "ss_status", MedName = "sr_status", WFName = "Ssr_status", DBColumn = false)]
        public Int32 SrStatus { get; set; }
        #endregion

        #region 附加字段 架子位置
        /// <summary>
        /// 架子位置
        /// </summary>
        [FieldMapAttribute(ClabName = "ss_place", MedName = "sr_place", WFName = "Ssr_Dpos_id", DBColumn = false)]
        public String SrPlace { get; set; }
        #endregion

        #region 附加字段 冰箱ID
        /// <summary>
        /// 冰箱ID
        /// </summary>
        [FieldMapAttribute(ClabName = "ss_icebox", MedName = "sr_store_id", WFName = "Ssr_Dstore_id", DBColumn = false)]
        public String SrStoreId { get; set; }
        #endregion

        #region 附加字段 审计日期
        /// <summary>
        /// 审计日期
        /// </summary>
        [FieldMapAttribute(ClabName = "ss_chk_date", MedName = "sr_audit_date", WFName = "Ssr_audit_date", DBColumn = false)]
        public DateTime SrAuditDate { get; set; }
        #endregion

        #region 附加字段 数量
        /// <summary>
        /// 数量
        /// </summary>
        [FieldMapAttribute(ClabName = "ss_num", MedName = "sr_amount", WFName = "Ssr_amount", DBColumn = false)]
        public Int32 SrAmount { get; set; }
        #endregion

        #region 附加字段 使用状态
        /// <summary>
        /// 使用状态
        /// </summary>
        [FieldMapAttribute(ClabName = "usestatus", MedName = "usestatus", WFName = "usestatus", DBColumn = false)]
        public String UseStatus { get; set; }
        #endregion

        #region 附加字段 架子使用数
        /// <summary>
        /// 架子使用数   usestatus_xy
        /// </summary>
        /// CAST(isnull(SamStore_Rack.ss_num,0) AS VARCHAR)+'/'+CAST((dict_cuv_shelf.cus_x_num*dict_cuv_shelf.cus_y_num)AS VARCHAR) AS usestatus_xy,
        //[FieldMapAttribute(ClabName = "usestatus_xy", MedName = "usestatus_xy")]
        public String UseStatusXY {
            get
            {
                return SrAmount.ToString() + "/" + (RackXAmount * RackYAmount).ToString(); ;
            }
        }
        #endregion

        #region 附加字段 是否选中
        /// <summary>
        /// 是否选中  isselected
        /// </summary>
        [FieldMapAttribute(ClabName = "isselected", MedName = "isselected", WFName = "isselected", DBColumn = false)]
        public bool IsSelected { get; set; } 
        #endregion
    }
}
