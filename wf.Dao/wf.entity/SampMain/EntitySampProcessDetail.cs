using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 标本流转明细表
    /// 旧表名:Samp_process_detial 新表名:Sample_process_detial
    /// </summary>
    [Serializable]
    public class EntitySampProcessDetail : EntityBase
    {
        public EntitySampProcessDetail()
        {

        }

        /// <summary>
        /// 编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_id", MedName = "proc_no", WFName = "Sproc_id", DBIdentity = true)]
        public Int64 ProcNo { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_date", MedName = "proc_date", WFName = "Sproc_date")]
        public DateTime ProcDate { get; set; }

        /// <summary>
        /// 操作者工号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_login_id", MedName = "proc_usercode", WFName = "Sproc_user_id")]
        public String ProcUsercode { get; set; }

        /// <summary>
        /// 操作者姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_name", MedName = "proc_username", WFName = "Sproc_user_name")]
        public String ProcUsername { get; set; }

        /// <summary>
        /// 类型（不使用）
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_type", MedName = "proc_type", WFName = "Sproc_type")]
        public String ProcType { get; set; }

        /// <summary>
        /// 操作状态  (0-未打印,1-打印,2-采集, 3-已收取,4-已送检,5-签收,6-检验中,7-已检验)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_status", MedName = "proc_status", WFName = "Sproc_status")]
        public String ProcStatus { get; set; }


        /// <summary>
        /// 内部关联ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_bar_no", MedName = "proc_barno", WFName = "Sproc_Sma_bar_id")]
        public String ProcBarno { get; set; }

        /// <summary>
        /// 条码号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_bar_code", MedName = "proc_barcode", WFName = "Sproc_Sma_bar_code")]
        public String ProcBarcode { get; set; }

        /// <summary>
        /// 操作地点
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_place", MedName = "proc_place", WFName = "Sproc_place")]
        public String ProcPlace { get; set; }

        /// <summary>
        /// 流程次数（回退后次数+1）
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_flow", MedName = "proc_times", WFName = "Sproc_times")]
        public Int32 ProcTimes { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_remark", MedName = "proc_content", WFName = "Sproc_content")]
        public String ProcContent { get; set; }

        /// <summary>
        /// 报告ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_id", MedName = "rep_id", WFName = "Sproc_rep_id")]
        public String RepId { get; set; }

        #region 附加字段  状态简称(界面显示为操作)
        /// <summary>
        /// 操作状态中文名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_status_name", MedName = "proc_status_name", WFName = "proc_status_name", DBColumn = false)]
        public String ProcStatusName { get; set; }
        #endregion
    }
}
