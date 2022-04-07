using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 专业组
    /// </summary>
    [Serializable]
    public class EntityDicPubProfession : EntityBase
    {
        public EntityDicPubProfession()
        {
            ProSortNo = 0;
            Checked = false;
        }

        /// <summary>
        /// 编码
        /// </summary>
        public string ProId { get; set; }

        /// <summary>
        /// 组别名称
        /// </summary>
        public string ProName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string ProRemark { get; set; }

        /// <summary>
        /// 类型 0-专业组 1-物理组
        /// </summary>
        public int ProType { get; set; }


        /// <summary>
        /// 拼音码
        /// </summary>
        public string ProPyCode { get; set; }

        /// <summary>
        /// 五笔码
        /// </summary>
        public string ProWbCode { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int ProSortNo { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        public string ProDelFlag { get; set; }

        /// <summary>
        ///机构D
        /// </summary>
        public string ProOrgId { get; set; }

        /// <summary>
        ///类型名称
        /// </summary>
        public string ProTypeName { get; set; }

        /// <summary>
        ///机构名称
        /// </summary>
        public string ProOrgName { get; set; }

        #region 附加字段
        /// <summary>
        ///实验组专业组对应
        /// </summary>
        public string ProTypeLink { get; set; }

        /// <summary>
        ///实验组对应专业组
        /// </summary>
        public string ProPtypeName { get; set; }

        /// <summary>
        ///专业组对应实验组
        /// </summary>
        public string ProCtypeName { get; set; }
        #endregion

        #region 附加字段 是否选中
        /// <summary>
        /// 是否选中
        /// </summary>
        public Boolean Checked { get; set; }
        #endregion

        /// <summary>
        ///专业组对应实验组
        /// </summary>
        public string ProCtypeId { get; set; }

        #region 附加字段 统一ID
        /// <summary>
        /// 统一ID
        /// </summary>
        public String SpId
        {
            get
            {
                return ProId;
            }
        }
        #endregion

    }
}
