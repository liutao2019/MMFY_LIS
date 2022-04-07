using System;

namespace dcl.entity
{
    /// <summary>
    /// 细菌名称表
    /// </summary>
    [Serializable]
    public class EntityDicMicBacteria : EntityBase
    {
        public EntityDicMicBacteria()
        {
            BacSortNo = 0;
            Checked = false;
        }

        /// <summary>
        /// 细菌编码
        /// </summary>
        public String BacId { get; set; }

        /// <summary>
        /// 细菌英文
        /// </summary>
        public String BacEname { get; set; }

        /// <summary>
        /// 细菌名称
        /// </summary>
        public String BacCname { get; set; }

        /// <summary>
        /// 细菌类别
        /// </summary>
        public String BacBtId { get; set; }

        /// <summary>
        /// 细菌通道码
        /// </summary>
        public String BacMitNo { get; set; }

        /// <summary>
        /// 细菌WHO编码
        /// </summary>
        public String BacWhoNo { get; set; }

        /// <summary>
        /// 细菌代码
        /// </summary>
        public String BacCode { get; set; }

        /// <summary>
        /// 标志
        /// </summary>
        public Int32 BacFlag { get; set; }

        /// <summary>
        /// 简码
        /// </summary>
        public String BacCCode { get; set; }

        /// <summary>
        /// 阳性标志
        /// </summary>
        public Int32 BacPositiveFlag { get; set; }

        /// <summary>
        /// 拼音码
        /// </summary>
        public String BacPyCode { get; set; }

        /// <summary>
        /// 五笔码
        /// </summary>
        public String BacWbCode { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public Int32 BacSortNo { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        public String BacDelFlag { get; set; }

        /// <summary>
        /// 备注（来源模板字典）
        /// </summary>
        public String BacRemark { get; set; }


        /// <summary>
        /// 大小（来源模板字典）
        /// </summary>
        public String BacDx { get; set; }

        /// <summary>
        /// 形态（来源模板字典）
        /// </summary>
        public String BacXt { get; set; }

        /// <summary>
        /// 表面（来源模板字典）
        /// </summary>
        public String BacBm { get; set; }

        /// <summary>
        /// 颜色（来源模板字典）
        /// </summary>
        public String BacYs { get; set; }

        /// <summary>
        /// 边缘（来源模板字典）
        /// </summary>
        public String BacBy { get; set; }


        /// <summary>
        /// 溶血（来源模板字典）
        /// </summary>
        public String BacRx { get; set; }

        /// <summary>
        /// 透明度（来源模板字典）
        /// </summary>
        public String BacTmd { get; set; }

        /// <summary>
        /// 生成方式（来源模板字典）
        /// </summary>
        public String BacScfs { get; set; }

        /// <summary>
        /// 细菌计数（来源模板字典）
        /// </summary>
        public String BacXjjs { get; set; }


        #region 附加字段 是否选中

        /// <summary>
        /// 是否选中
        /// </summary>
        public Boolean Checked { get; set; }

        #endregion 附加字段 是否选中

        #region 附加码

        /// <summary>
        /// 细菌类别名
        /// </summary>
        public string BTypeCname { get; set; }

        #endregion 附加码

        #region 附加字段 统一ID

        /// <summary>
        /// 统一ID
        /// </summary>
        public String SpId
        {
            get
            {
                return BacId;
            }
        }

        #endregion 附加字段 统一ID
    }
}