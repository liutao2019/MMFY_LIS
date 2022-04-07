using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 病人报告集合
    /// </summary>
    [Serializable]
    public class EntityQcResultList:EntityBase
    {
        public EntityQcResultList()
        {
            listAnti = new List<EntityObrResultAnti>();
            listDesc = new List<EntityObrResultDesc>();
            listBact = new List<EntityObrResultBact>();
            listResulto = new List<EntityObrResult>();
            listResultoNoFliter = new List<EntityObrResult>();
            listPatients = new List<EntityPidReportMain>();
            listRepDetail = new List<EntityPidReportDetail>();
        }
        /// <summary>
        /// 细菌药敏结果
        /// </summary>
        public List<EntityObrResultAnti> listAnti { get; set; }

        /// <summary>
        /// 描述报告结果
        /// </summary>
        public List<EntityObrResultDesc> listDesc { get; set; }

        /// <summary>
        /// 细菌菌名数据
        /// </summary>
        public List<EntityObrResultBact> listBact { get; set; }

        /// <summary>
        /// 检验结果数据
        /// </summary>
        public List<EntityObrResult> listResulto { get; set; }


        /// <summary>
        /// 检验结果原始数据（没有经过任何过滤如，空的检验结果不保存等）
        /// </summary>
        public List<EntityObrResult> listResultoNoFliter { get; set; }

        /// <summary>
        /// 病人列表
        /// </summary>
        public List<EntityPidReportMain> listPatients { get; set; }

        /// <summary>
        /// 病人组合明细列表
        /// </summary>
        public List<EntityPidReportDetail> listRepDetail { get; set; }

        /// <summary>
        /// 病人
        /// </summary>
        public EntityPidReportMain patient { get; set; }

        /// <summary>
        /// 实验序号（茂名资料导入）
        /// </summary>
        public string TestSeq { get; set; }
    }
}
