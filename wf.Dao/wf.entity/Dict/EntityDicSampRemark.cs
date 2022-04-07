using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 标本备注
    /// </summary>
    [Serializable]
    public class EntityDicSampRemark : EntityBase
    {

        public EntityDicSampRemark()
        {
            Checked= false;
        }
        /// <summary>
        /// 编码
        /// </summary>                       
        public String RemId { get; set; }

        /// <summary>
        /// 标本备注
        /// </summary>                       
        public String RemContent { get; set; }

        /// <summary>
        /// 拼音码
        /// </summary>                       
        public String RemPyCode { get; set; }

        /// <summary>
        /// 五笔码
        /// </summary>                       
        public String RemWbCode { get; set; }

        /// <summary>
        /// 输入码
        /// </summary>                       
        public String RemCCode { get; set; }

        /// <summary>
        /// 新生
        /// </summary>                       
        public String RemNewborn { get; set; }

        /// <summary>
        /// 序号
        /// </summary>                       
        public Int32 RemSortNo { get; set; }


        #region 附加字段 是否选中
        public Boolean Checked { get; set; }
        #endregion

        public String SpId
        {
            get
            {
                return RemId;
            }
        }
    }
}
