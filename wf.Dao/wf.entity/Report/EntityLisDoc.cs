using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 检验文档
    /// </summary>
    [Serializable]
    public class EntityLisDoc : EntityBase
    {
        /// <summary>
        ///主键
        /// </summary>   
        [FieldMapAttribute(WFName = "doc_id")]
        public Int32 docId { get; set; }

        /// <summary>
        ///文档日期
        /// </summary>   
        [FieldMapAttribute(WFName = "doc_date")]
        public DateTime docDate { get; set; }

        /// <summary>
        ///文档类型（0-模板 1-数据）
        /// </summary>   
        [FieldMapAttribute(WFName = "doc_type")]
        public String docType { get; set; }

        /// <summary>
        ///模板类型（文档标题）
        /// </summary>   
        [FieldMapAttribute(WFName = "doc_title")]
        public String docTitle { get; set; }

        /// <summary>
        ///文档内容
        /// </summary>   
        [FieldMapAttribute(WFName = "doc_content")]
        public String docContent { get; set; }

    }
}
