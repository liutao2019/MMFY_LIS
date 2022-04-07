using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 标本进程监控实体类
    /// </summary>
    public class EntitySampProcessMonitor:EntitySampMain
    {
        public EntitySampProcessMonitor()
        {
            UrgentFlag = 0;
            BcIsTimeout = 0;
        }

        /// <summary>
        ///急查标志(字符表示)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_urgent_flagStr", MedName = "samp_urgent_flagStr" , WFName = "samp_urgent_flagStr", DBColumn = false)]
        public String SampUrgentFlagStr { get; set; }

        /// <summary>
        ///急查标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "urgent_flag", MedName = "urgent_flag", WFName = "urgent_flag", DBColumn = false)]
        public Int32 UrgentFlag { get; set; }

        /// <summary>
        ///时间差(最后操作时间与服务器当前时间差)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_time_difference", MedName = "bc_time_difference", WFName = "bc_time_difference", DBColumn = false)]
        public Int32? BcTimeDifference { get; set; }

        /// <summary>
        ///时间差是否超时(30天为标准；1:超了，0:未超)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_isTimeout", MedName = "bc_isTimeout", WFName = "bc_isTimeout", DBColumn = false)]
        public Int32 BcIsTimeout { get; set; }

        /// <summary>
        ///序号（双向）
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_host_order", MedName = "rep_serial_num", WFName = "Pma_serial_num", DBColumn = false)]
        public String RepSerialNum { get; set; }

        /// <summary>
        ///操作地点
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_place", MedName = "proc_place", WFName = "Sproc_place", DBColumn = false)]
        public String ProcPlace { get; set; }

        /// <summary>
        ///TAT标记(-1：没超TAT; 0：超出TATW; 1：超出TAT)
        /// </summary>   
        [FieldMapAttribute(ClabName = "over_tat", MedName = "over_tat", WFName = "over_tat", DBColumn = false)]
        public String OverTat { get; set; }

        /// <summary>
        ///个数
        /// </summary>   
        [FieldMapAttribute(ClabName = "count", MedName = "count", WFName = "count")]
        public Int64 Count { get; set; }

        /// <summary>
        ///操作人名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "userName", MedName = "user_name", WFName = "Buser_name")]
        public string OperationName { get; set; }
    }
}
