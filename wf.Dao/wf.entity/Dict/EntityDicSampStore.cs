using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 冰箱字典
    /// 旧表名:dic_samp_store 新表名:Dict_sample_store
    /// </summary>
    [Serializable]
    public class EntityDicSampStore : EntityBase
    {
        public EntityDicSampStore()
        {
            StoreAmount = 0;
            StoreStatus = 0;
        }
        /// <summary>
        ///冰箱ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "ice_id", MedName = "store_id", WFName = "Dstore_id")]
        public String StoreId { get; set; }

        /// <summary>
        ///冰箱代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "ice_code", MedName = "store_code", WFName = "Dstore_code")]
        public String StoreCode { get; set; }

        /// <summary>
        ///冰箱名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "ice_name", MedName = "store_name", WFName = "Dstore_name")]
        public String StoreName { get; set; }

        /// <summary>
        ///实验组别ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "ice_ctype", MedName = "store_lab_id", WFName = "Dstore_lab_id")]
        public String StoreLabId { get; set; }

        /// <summary>
        ///存储数量
        /// </summary>   
        [FieldMapAttribute(ClabName = "ice_cups", MedName = "store_amount", WFName = "Dstore_amount")]
        public Int32 StoreAmount { get; set; }

        /// <summary>
        ///状态ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "ice_status", MedName = "store_status", WFName = "Dstore_Dstau_id")]
        public Int32 StoreStatus { get; set; }

        /// <summary>
        ///拼音码
        /// </summary>   
        [FieldMapAttribute(ClabName = "ice_py", MedName = "py_code", WFName = "py_code")]
        public String StorePyCode { get; set; }

        /// <summary>
        ///五笔码
        /// </summary>   
        [FieldMapAttribute(ClabName = "ice_wb", MedName = "wb_code", WFName = "wb_code")]
        public String StoreWbCode { get; set; }

        /// <summary>
        /// 温度范围ID
        /// </summary>
        [FieldMapAttribute(ClabName = "store_rg_id", MedName = "store_rg_id", WFName = "Dstore_rg_id")]
        public string StoreRgId { get; set; }

        /// <summary>
        /// 采集器ID
        /// </summary>
        [FieldMapAttribute(ClabName = "store_dh_id", MedName = "store_dh_id", WFName = "Dstore_dh_id")]
        public string StoreDhId { get; set; }


        #region  附加字段  采集器名称
        /// <summary>
        /// 采集器名称
        /// </summary>
        [FieldMapAttribute(ClabName = "dh_name", MedName = "dh_name", WFName = "dh_name", DBColumn = false)]
        public string DhName { get; set; }
        #endregion

        #region  附加字段   温度范围名称
        /// <summary>
        /// 温度范围名称
        /// </summary>
        [FieldMapAttribute(ClabName = "dt_name", MedName = "dt_name", WFName = "dt_name", DBColumn = false)]
        public string DtName { get; set; }
        #endregion

        #region 附加字段 实验组名称
        /// <summary>
        /// 实验组名称
        /// </summary>
        [FieldMapAttribute(ClabName = "type_name", MedName = "pro_name", WFName = "Dpro_name", DBColumn = false)]
        public String ProName { get; set; }
        #endregion

        #region 附加字段 状态描述
        /// <summary>
        /// 状态描述
        /// </summary>
        [FieldMapAttribute(ClabName = "status_name", MedName = "stau_name", WFName = "Dstau_name", DBColumn = false)]
        public String StauName { get; set; }
        #endregion

    }
}
