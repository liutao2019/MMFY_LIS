using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 细菌菌类
    /// 旧表名:Dic_mic_bacttype 新表名:Dict_mic_bacttype
    /// </summary>
    [Serializable]
    public class EntityDicMicBacttype : EntityBase
    {
        public EntityDicMicBacttype()
        {
            BtypeSortNo = 0;
            Checked = false;
        }

        /// <summary>
        ///编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bt_id", MedName = "btype_id", WFName = "Dbactt_id")]
        public String BtypeId { get; set; }

        /// <summary>
        ///英文代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bt_ename", MedName = "btype_ename", WFName = "Dbactt_ename")]
        public String BtypeEname { get; set; }

        /// <summary>
        ///菌类名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bt_cname", MedName = "btype_cname", WFName = "Dbactt_cname")]
        public String BtypeCname { get; set; }

        /// <summary>
        ///药敏卡编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bt_stid", MedName = "btype_atype_id", WFName = "Dbactt_Dantitype_id")]
        public String BtypeAtypeId { get; set; }

        /// <summary>
        ///菌类代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bt_code", MedName = "btype_code", WFName = "Dbactt_code")]
        public String BtypeCode { get; set; }

        /// <summary>
        ///输入码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bt_incode", MedName = "c_code", WFName = "Dbactt_c_code")]
        public String BtypeCCode { get; set; }

        /// <summary>
        ///拼音码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bt_py", MedName = "py_code", WFName = "py_code")]
        public String BtypePyCode { get; set; }

        /// <summary>
        ///五笔码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bt_wb", MedName = "wb_code", WFName = "wb_code")]
        public String BtypeWbCode { get; set; }

        /// <summary>
        ///序号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bt_seq", MedName = "sort_no", WFName = "sort_no")]
        public Int32 BtypeSortNo { get; set; }

        /// <summary>
        ///删除标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "bt_del", MedName = "del_flag", WFName = "del_flag")]
        public String BtypeDelFlag { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "bt_bbtype", MedName = "btype_type", WFName = "Dbactt_type")]
        public String BtypeType { get; set; }

        #region 附加字段 是否选中
        /// <summary>
        /// 是否选中
        /// </summary>
        public Boolean Checked { get; set; }
        #endregion

        #region 附加字段 药敏卡
        /// <summary>
        /// 药敏卡名称
        /// </summary>
        [FieldMapAttribute(ClabName = "st_cname", MedName = "atype_name", WFName = "Dantitype_name", DBColumn = false)]
        public String ATypeName { get; set; }
        #endregion

        #region 附加字段 统一ID
        /// <summary>
        /// 统一ID
        /// </summary>
        public String SpId
        {
            get
            {
                return BtypeId;
            }
        }
        #endregion
    }
}
