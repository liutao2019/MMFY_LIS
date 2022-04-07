using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 取报告时间字典表：Def_com_reptime 
    /// 旧表名:Def_com_reptime 新表名:Dict_reptime
    /// </summary>
    [Serializable]
    public class EntityDicComReptime : EntityBase
    {
        /// <summary>
        ///  程序内部标识，使用SysTableID的自增方式
        /// </summary>   
        [FieldMapAttribute(ClabName = "rt_id", MedName = "ret_id", WFName = "Dret_id")]
        public String RetId { get; set; }

        /// <summary>
        /// 代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "rt_code", MedName = "ret_code", WFName = "Dret_code")]
        public String RetCode { get; set; }

        /// <summary>
        /// 显示名
        /// </summary>   
        [FieldMapAttribute(ClabName = "rt_name", MedName = "ret_name", WFName = "Dret_name")]
        public String RetName { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "rt_time_start", MedName = "start_time", WFName = "Dstart_time")]
        public String StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "rt_time_end", MedName = "end_time", WFName = "Dend_time")]
        public String EndTime { get; set; }

        /// <summary>
        /// 类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "rt_type", MedName = "ret_type", WFName = "Dret_type")]
        public Int32 RetType { get; set; }

        /// <summary>
        /// 天数
        /// </summary>   
        [FieldMapAttribute(ClabName = "rt_day", MedName = "ret_day", WFName = "Dret_day")]
        public String RetDay { get; set; }

        /// <summary>
        /// 时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "rt_time", MedName = "ret_time", WFName = "Dret_time")]
        public String RetTime { get; set; }

    }
}
