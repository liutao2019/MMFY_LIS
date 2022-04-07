using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 医院字典表
    /// </summary>
    [Serializable]
    public class EntityDicPubOrganize : EntityBase
    {
        public EntityDicPubOrganize()
        {
            OrgSortNo = 0;
        }

        /// <summary>
        /// 医院编码
        /// </summary>
        public string OrgId { get; set; }

        /// <summary>
        /// 医院代码
        /// </summary>
        public string OrgCode { get; set; }

        /// <summary>
        /// 医院名称
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string OrgAddress { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string OrgTel { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int OrgSortNo { get; set; }

        /// <summary>
        /// 拼音码
        /// </summary>
        public string OrgPyCode { get; set; }

        /// <summary>
        /// 五笔码
        /// </summary>
        public string OrgWbCode { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        public string OrgDelFlag { get; set; }

        #region 附加字段
        /// <summary>
        /// 父节点
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 仪器ID
        /// </summary>
        public string ItrId { get; set; }
        /// <summary>
        /// 仪器名字
        /// </summary>
        public string ItrName { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public string SortId { get; set; }
        #endregion
    }

}
