using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 快速排样用于导入数据
    /// </summary>
    [Serializable]
    public class EntityPatientsToImport
    {
        /// <summary>
        /// 样本号
        /// </summary>
        public string RepSid { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string PidName { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public Int32 RegNumber { get; set; }
        /// <summary>
        /// 试管架子号  标本位置
        /// </summary>
        public string RegRackNo { get; set; }

        /// <summary>
        /// 检验时间
        /// </summary>
        public DateTime SampCheckDate { get; set; }

        /// <summary>
        /// 检验项目名称 可拼接
        /// </summary>
        public string ComNames { get; set; }

        /// <summary>
        /// 项目ID合集
        /// </summary>
        public string ComIds { get; set; }
        /// <summary>
        /// 仪器代码，对应dict_instrmt表中itr_id
        /// </summary>
        public string RepItrId { get; set; }
        /// <summary>
        /// 仪器名称
        /// </summary>
        public string RepItrName { get; set; }

        /// <summary>
        /// 录入日期
        /// </summary>
        public DateTime RepInDate { get; set; }

        /// <summary>
        /// 条码项目明细表 自增ID
        /// </summary>
        public string DetSn { get; set; }

        /// <summary>
        /// 条码号
        /// </summary>
        public string SampBarCode { get; set; }

        /// <summary>
        /// 实验组ID
        /// </summary>
        public string ProId { get; set; }

        /// <summary>
        /// 项目数量
        /// </summary>
        public Int32 ComCount { get; set; }
        /// <summary>
        /// 序号  双向
        /// </summary>
        public string RepSerialNum { get; set; }
    }
}
