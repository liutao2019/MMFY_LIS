using System;
using System.Collections.Generic;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public class BatchEditDest : BatchEditSrc
    {
        /// <summary>
        /// 病人来源
        /// </summary>
        public string pat_ori_id { get; set; }

        /// <summary>
        /// 工作单位
        /// </summary>
        public string pat_unit { get; set; }

        /// <summary>
        /// 申请医生
        /// </summary>
        public string pat_doc_id { get; set; }

        /// <summary>
        /// 标本状态
        /// </summary>
        public string pat_rem { get; set; }

        /// <summary>
        /// 采样时间
        /// </summary>
        public DateTime? pat_sample_date { get; set; }

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime? pat_apply_date { get; set; }

        /// <summary>
        /// 送检时间
        /// </summary>
        public DateTime? pat_sdate { get; set; }

        /// <summary>
        /// 检验时间
        /// </summary>
        public DateTime? pat_jy_date { get; set; }

        /// <summary>
        /// 一审时间
        /// </summary>
        public DateTime? pat_chk_date { get; set; }


        /// <summary>
        /// 二审时间
        /// </summary>
        public DateTime? pat_report_date { get; set; }

        /// <summary>
        /// 检验组合
        /// </summary>
        public List<EntityPatientsMi_4Barcode> PatientsMi { get; set; }

        /// <summary>
        /// 匹配方式
        /// 0=按样本号
        /// 1=按序号
        /// </summary>
        public string MatchMode { get; set; }

        public BatchEditDest()
        {
            PatientsMi = new List<EntityPatientsMi_4Barcode>();
        }

        /// <summary>
        /// 病人性别
        /// </summary>
        public string pat_sex { get; set; }


        /// <summary>
        /// 年龄储存为分钟
        /// </summary>
        public int pat_age { get; set; }

        /// <summary>
        /// 年龄显示值
        /// </summary>
        public string pat_age_exp { get; set; }

        public string pat_exp { get; set; }


        public string pat_diag { get; set; }
    }
}
