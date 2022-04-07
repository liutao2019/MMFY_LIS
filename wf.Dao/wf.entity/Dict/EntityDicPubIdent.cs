using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 病人标识类型
    /// 旧表名:Dic_pub_ident 新表名:Dict_ident
    /// </summary>
    [Serializable]
    public class EntityDicPubIdent : EntityBase
    {
        /// <summary>
        /// 编码
        /// </summary>      
        [FieldMapAttribute(ClabName = "no_id", MedName = "idt_id", WFName = "Didt_id")]
        public String IdtId { get; set; }

        /// <summary>
        /// 标识名称
        /// </summary>     
        [FieldMapAttribute(ClabName = "no_name", MedName = "idt_name", WFName = "Didt_name")]
        public String IdtName { get; set; }

        /// <summary>
        /// 标识代码
        /// </summary>            
        [FieldMapAttribute(ClabName = "no_code", MedName = "idt_code", WFName = "Didt_code")]
        public String IdtCode { get; set; }

        /// <summary>
        /// 输入码
        /// </summary>          
        [FieldMapAttribute(ClabName = "no_incode", MedName = "c_code", WFName = "Didt_c_code")]
        public String IdtCCode { get; set; }

        /// <summary>
        /// 病人来源类型
        /// </summary>             
        [FieldMapAttribute(ClabName = "no_ori_id", MedName = "idt_src_id", WFName = "Didt_Dsorc_id")]
        public String IdtSrcId { get; set; }

        /// <summary>
        /// 拼音码
        /// </summary>               
        [FieldMapAttribute(ClabName = "no_py", MedName = "py_code", WFName = "py_code")]
        public String IdtPyCode { get; set; }

        /// <summary>
        /// 五笔码
        /// </summary>                      
        [FieldMapAttribute(ClabName = "no_wb", MedName = "wb_code", WFName = "wb_code")]
        public String IdtWbCode { get; set; }

        /// <summary>
        /// 序号
        /// </summary>                       
        [FieldMapAttribute(ClabName = "no_seq", MedName = "sort_no", WFName = "sort_no")]
        public Int32 IdtSortNo { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>                       
        [FieldMapAttribute(ClabName = "no_del", MedName = "del_flag", WFName = "del_flag")]
        public String IdtDelFlag { get; set; }

        /// <summary>
        /// 接口ID
        /// </summary>                       
        [FieldMapAttribute(ClabName = "no_interface_id", MedName = "interface_id", WFName = "Didt_interface_id")]
        public String IdtInterfaceId { get; set; }

        /// <summary>
        /// 接口类型
        /// </summary>                       
        [FieldMapAttribute(ClabName = "no_interface_type", MedName = "interface_type", WFName = "Didt_interface_type")]
        public String IdtInterfaceType { get; set; }

        /// <summary>
        /// 是否必录
        /// </summary>                       
        [FieldMapAttribute(ClabName = "no_patinno_notnull", MedName = "patinno_notnull", WFName = "Didt_notnull")]
        public Boolean IdtPatinnoNotnull { get; set; }

        #region 附加码
        /// <summary>
        /// 病人来源
        /// </summary>
        [FieldMapAttribute(ClabName = "ori_name", MedName = "src_name",WFName = "Dsorc_name", DBColumn = false)]
        public string SrcName { get; set; }
        #endregion
    }
}
