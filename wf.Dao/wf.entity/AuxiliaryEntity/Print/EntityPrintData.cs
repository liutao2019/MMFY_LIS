using System;
using System.Collections.Generic;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 打印数据对象
    /// </summary>
    [Serializable]
    public class EntityPrintData
    {
        /// <summary>
        /// 打印条件参数
        /// </summary>
        public List<EntityPrintParameter> Parameters { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 批量打印报告顺序号
        /// </summary>
        public int SeqComRep { get; set; }

        /// <summary>
        /// 报表代码
        /// </summary>
        public string ReportCode { get; set; }

        /// <summary>
        /// 输出打印者
        /// </summary>
        [System.ComponentModel.Description("输出打印者")]
        public string outputPrintPerson { get; set; }

        /// <summary>
        /// 输出打印时间
        /// </summary>
        [System.ComponentModel.Description("输出打印时间")]
        public string outputPrintTime { get; set; }


        #region 辅助信息(可选)
        /// <summary>
        /// 病人ID
        /// </summary>
        public string pat_id { get; set; }

        /// <summary>
        /// 病人姓名
        /// </summary>
        public string pat_name { get; set; }

        /// <summary>
        /// 病人pat_in_no
        /// </summary>
        public string pat_in_no { get; set; }
        #endregion

        /// <summary>
        /// 添加打印sql条件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddParam(string name, string value)
        {
            EntityPrintParameter entityParam = new EntityPrintParameter();
            entityParam.Name = name;
            entityParam.Value = value;
            this.Parameters.Add(entityParam);
        }

        public EntityPrintData()
        {
            this.pat_id = string.Empty;
            this.Parameters = new List<EntityPrintParameter>();
            this.Sequence = 0;
            this.pat_name = string.Empty;
        }


        public override string ToString()
        {
            string txt = string.Format("{0} {1} （{2}）", pat_name, pat_id, ReportCode);
            return txt;
        }
    }
}
