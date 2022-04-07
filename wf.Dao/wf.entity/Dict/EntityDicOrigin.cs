using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 病人来源类型
    /// </summary>
    [Serializable]
    public class EntityDicOrigin : EntityBase
    {
        public EntityDicOrigin()
        {
            SortNo = 0;
            Checked = false;
        }
        /// <summary>
        /// 编码
        /// </summary>                       
        public String SrcId { get; set; }

        /// <summary>
        /// 来源名称
        /// </summary>                       
        public String SrcName { get; set; }

        /// <summary>
        /// 输入码
        /// </summary>                       
        public String CCode { get; set; }

        /// <summary>
        /// 拼音码
        /// </summary>                       
        public String PyCode { get; set; }

        /// <summary>
        /// 五笔码
        /// </summary>                       
        public String WbCode { get; set; }

        /// <summary>
        /// 排序
        /// </summary>                       
        public Int32 SortNo { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>                       
        public String DelFlag { get; set; }

        #region 附加字段 是否选中
        public Boolean Checked { get; set; }

        /// <summary>
        /// 统一ID
        /// </summary>
        public String SpId
        {
            get
            {
                return SrcId;
            }
        }
        #endregion
    }
}
