using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 数据表实体类： Dic_itm_reftype 参考值
    /// </summary>
    [Serializable()]
    public class EntityDicItmReftype : EntityBase
    {
        /// <summary>
        ///编码
        /// </summary>                       
        public String RefId { get; set; }

        /// <summary>
        ///参考值名称
        /// </summary>                       
        public String RefName { get; set; }

        /// <summary>
        ///年龄上限
        /// </summary>                       
        public Int32 RefAgeHigh { get; set; }

        /// <summary>
        ///年龄下限
        /// </summary>                       
        public Int32 RefAgeLower { get; set; }

        /// <summary>
        ///拼音码
        /// </summary>                       
        public String RefPyCode { get; set; }

        /// <summary>
        ///五笔码
        /// </summary>                       
        public String RefWbCode { get; set; }

        /// <summary>
        ///排序
        /// </summary>                       
        public Int32 RefSortNo { get; set; }

        /// <summary>
        ///删除标志
        /// </summary>                       
        public String RefDelFlag { get; set; }
        /// <summary>
        /// 年龄上限单位
        /// </summary>
        public String RefAgeHighUnit { get; set; }

        /// <summary>
        /// 年龄下限单位
        /// </summary>
        public String RefAgeLowerUnit { get; set; }
    }
}
