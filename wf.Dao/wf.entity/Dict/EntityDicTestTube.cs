using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 试管类型
    /// 旧表名:Dic_test_tube 新表名:Dict_test_tube
    /// </summary>
    [Serializable]
    public class EntityDicTestTube : EntityBase
    {
        /// <summary>
        ///采集容器编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "cuv_code", MedName = "tub_code", WFName = "Dtub_code")]
        public String TubCode { get; set; }

        /// <summary>
        ///采集容器名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "cuv_name", MedName = "tub_name", WFName = "Dtub_name")]
        public String TubName { get; set; }

        /// <summary>
        ///类型(0-抽血类 1-自留取)
        /// </summary>   
        [FieldMapAttribute(ClabName = "cuv_flag", MedName = "tub_flag", WFName = "Dtub_flag")]
        public Int32 TubFlag { get; set; }

        /// <summary>
        ///预制条码的最小条码号
        /// </summary>   
        [FieldMapAttribute(ClabName = "cuv_min_no", MedName = "tub_barcode_min", WFName = "Dtub_barcode_min")]
        public Decimal TubBarcodeMin { get; set; }

        /// <summary>
        ///预制条码的最大条码号
        /// </summary>   
        [FieldMapAttribute(ClabName = "cuv_max_no", MedName = "tub_barcode_max", WFName = "Dtub_barcode_max")]
        public Decimal TubBarcodeMax { get; set; }

        /// <summary>
        ///顺序号
        /// </summary>   
        [FieldMapAttribute(ClabName = "cuv_seq", MedName = "sort_no", WFName = "sort_no")]
        public Int32 TubSortNo { get; set; }

        /// <summary>
        ///拼音码
        /// </summary>   
        [FieldMapAttribute(ClabName = "cuv_py", MedName = "py_code", WFName = "py_code")]
        public String TubPyCode { get; set; }

        /// <summary>
        ///五笔码
        /// </summary>   
        [FieldMapAttribute(ClabName = "cuv_wb", MedName = "wb_code", WFName = "wb_code")]
        public String TubWbCode { get; set; }

        /// <summary>
        ///删除标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "cuv_del", MedName = "del_flag", WFName = "del_flag")]
        public String TubDelFlag { get; set; }

        /// <summary>
        ///项目最大采集量
        /// </summary>   
        [FieldMapAttribute(ClabName = "cuv_max_capacity", MedName = "tub_max_capcity", WFName = "Dtub_max_capcity")]
        public Decimal TubMaxCapcity { get; set; }

        /// <summary>
        ///采集量单位
        /// </summary>   
        [FieldMapAttribute(ClabName = "cuv_unit", MedName = "tub_unit", WFName = "Dtub_unit")]
        public String TubUnit { get; set; }

        /// <summary>
        ///项目最大数
        /// </summary>   
        [FieldMapAttribute(ClabName = "cuv_max_combine", MedName = "tub_max_com", WFName = "Dtub_max_com")]
        public Int32 TubMaxCom { get; set; }

        /// <summary>
        ///收费代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "cuv_fee_code", MedName = "tub_charge_code", WFName = "Dtub_charge_code")]
        public String TubChargeCode { get; set; }

        /// <summary>
        ///费用
        /// </summary>   
        [FieldMapAttribute(ClabName = "cuv_pri", MedName = "tub_price", WFName = "Dtub_price")]
        public Decimal TubPrice { get; set; }

        /// <summary>
        ///颜色
        /// </summary>   
        [FieldMapAttribute(ClabName = "cuv_color", MedName = "tub_color", WFName = "Dtub_color")]
        public String TubColor { get; set; }
    }
}
