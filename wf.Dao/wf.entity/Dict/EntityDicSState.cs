using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 标本状态
    /// </summary>
    [Serializable]
    public class EntityDicSState : EntityBase
    {
        public EntityDicSState()
        {
            SortNo = 0;
            Checked = false;
        }
        /// <summary>
        ///  编码
        /// </summary>                       
        public String StauId { get; set; }

        /// <summary>
        /// 标本状态
        /// </summary>                       
        public String StauName { get; set; }

        /// <summary>
        /// 输入码
        /// </summary>                       
        public String CCode { get; set; }

        /// <summary>
        /// 组别
        /// </summary>                       
        public String ProId { get; set; }

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
        public string ProName { get; set; }
        #endregion

        #region 附加字段 是否选中
        public Boolean Checked { get; set; }
        #endregion

        public String SpId
        {
            get
            {
                return StauId;
            }
        }
    }
}
