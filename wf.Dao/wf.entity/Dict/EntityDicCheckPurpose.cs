using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 检查目的字典表
    /// 旧表名:Dic_pub_check_purpose 新表名:Dict_check_purpose
    /// </summary>
    [Serializable]
    public class EntityDicCheckPurpose : EntityBase
    {
        public EntityDicCheckPurpose()
        {
            SortNo = 0;
        }

        /// <summary>
        /// 编码
        /// </summary>                       
        public String PurpId { get; set; }

        /// <summary>
        /// 检查目的
        /// </summary>                       
        public String PurpName { get; set; }

        /// <summary>
        /// 输入码
        /// </summary>                       
        public String CCode { get; set; }

        /// <summary>
        /// 专业组
        /// </summary>                       
        public String ProId { get; set; }

        /// <summary>
        /// 组别
        /// </summary>                       
        public String LabId { get; set; }

        /// <summary>
        /// 拼音码
        /// </summary>                       
        public String PyCode { get; set; }

        /// <summary>
        /// 五笔码
        /// </summary>                       
        public String WbCode { get; set; }

        /// <summary>
        /// 序号
        /// </summary>                       
        public Int32 SortNo { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>                       
        public String DelFlag { get; set; }

        #region 附加字段
        /// <summary>
        /// 组别名称
        /// </summary>                       
        public String TypeName { get; set; }

        /// <summary>
        /// P组别名称
        /// </summary>                       
        public String PTypeName { get; set; }
        #endregion
    }
}
