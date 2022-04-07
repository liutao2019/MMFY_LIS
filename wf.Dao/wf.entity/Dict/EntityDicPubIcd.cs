using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 诊断字典表
    /// 旧表名:Dic_Pub_icd 新表名:Dict_icd 
    /// </summary>
    [Serializable]
    public class EntityDicPubIcd : EntityBase
    {
        public EntityDicPubIcd()
        {
            Checked = false;
        }
        /// <summary>
        /// 诊断编码
        /// </summary>                       
        public String IcdId { get; set; }

        /// <summary>
        /// 诊断名称
        /// </summary>                       
        public String IcdName { get; set; }

        /// <summary>
        /// ICD编码
        /// </summary>                       
        public String IcdGbCode { get; set; }

        /// <summary>
        /// 输入码
        /// </summary>                       
        public String IcdCCode { get; set; }

        /// <summary>
        /// 拼音码
        /// </summary>                       
        public String IcdPyCode { get; set; }

        /// <summary>
        /// 五笔码
        /// </summary>                       
        public String IcdWbCode { get; set; }

        /// <summary>
        /// 序号
        /// </summary>                       
        public Int32 IcdSortNo { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>                       
        public String IcdDelFlag { get; set; }

        public String IcdContent { get; set; }

        #region 附加字段 是否选中
        public Boolean Checked { get; set; }
        #endregion

        #region 附加字段 统一ID
        /// <summary>
        /// 统一ID
        /// </summary>
        public String SpId
        {
            get
            {
                return IcdName;
            }
        }
        #endregion
    }
}
