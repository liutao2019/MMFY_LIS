using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 仪器维修故障信息表
    /// 旧表名:dic_itr_instrument_servicing 新表名:Dict_instrmt_servicing
    /// </summary>
    [Serializable]
    public class EntityDicItrInstrumentServicing : EntityBase
    {
        /// <summary>
        /// 故障表ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "ser_id", MedName = "ser_id", WFName = "Dis_id")]
        public Int32 SerId { get; set; }

        /// <summary>
        /// 仪器ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "ser_itr_id", MedName = "ser_itr_id", WFName = "Dis_Ditr_id")]
        public String SerItrId { get; set; }

        /// <summary>
        /// 故障内容
        /// </summary>   
        [FieldMapAttribute(ClabName = "ser_content", MedName = "ser_content", WFName = "Dis_content")]
        public String SerContent { get; set; }

        /// <summary>
        /// (暂时未用上)
        /// </summary>   
        [FieldMapAttribute(ClabName = "ser_putin_code", MedName = "ser_putin_code", WFName = "Dis_putin_code")]
        public String SerPutinCode { get; set; }

        /// <summary>
        /// 上报时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "ser_putin_date", MedName = "ser_putin_date", WFName = "Dis_putin_date")]
        public DateTime SerPutinDate { get; set; }

        /// <summary>
        /// 处理人编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "ser_handler_code", MedName = "ser_handler_code", WFName = "Dis_handler_code")]
        public String SerHandlerCode { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "ser_handle_date", MedName = "ser_handle_date", WFName = "Dis_handle_date")]
        public DateTime? SerHandleDate { get; set; }

        /// <summary>
        /// 处理结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "ser_handle_result", MedName = "ser_handle_result", WFName = "Dis_handle_result")]
        public String SerHandleResult { get; set; }

        /// <summary>
        /// 审核编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "ser_chk_code", MedName = "ser_chk_code", WFName = "Dis_chk_code")]
        public String SerChkCode { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>   
        [FieldMapAttribute(ClabName = "ser_chk_date", MedName = "ser_chk_date", WFName = "Dis_chk_date")]
        public DateTime? SerChkDate { get; set; }

        /// <summary>
        /// 审核结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "ser_chk_content", MedName = "ser_chk_content", WFName = "Dis_chk_content")]
        public String SerChkContent { get; set; }

        /// <summary>
        /// 维修报价
        /// </summary>   
        [FieldMapAttribute(ClabName = "ser_price", MedName = "ser_price", WFName = "Dis_price")]
        public Decimal SerPrice { get; set; }

        /// <summary>
        /// 间隔时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "ser_interval", MedName = "ser_interval", WFName = "Dis_interval")]
        public Int32 SerInterval { get; set; }

        #region 附加字段   仪器
        /// <summary>
        /// 仪器
        /// </summary>   
        [FieldMapAttribute(ClabName = "itr_mid", MedName = "itr_ename", WFName = "Ditr_ename", DBColumn = false)]
        public String ItrEname { get; set; }
        #endregion

        #region 附加字段   组别名称
        /// <summary>
        /// 组别名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "type_name", MedName = "pro_name", WFName = "Dpro_name", DBColumn = false)]
        public String ProName { get; set; }
        #endregion

        #region 附加字段   上报人
        /// <summary>
        /// 上报人
        /// </summary>   
        [FieldMapAttribute(ClabName = "ser_putin_code_user", MedName = "ser_putin_code_user", WFName = "ser_putin_code_user", DBColumn = false)]
        public String SerPutinCodeUser { get; set; }
        #endregion

        #region 附加字段   处理人
        /// <summary>
        /// 处理人
        /// </summary>   
        [FieldMapAttribute(ClabName = "ser_handler_code_user", MedName = "ser_handler_code_user", WFName = "ser_handler_code_user", DBColumn = false)]
        public String SerHandlerCodeUser { get; set; }
        #endregion

        #region 附加字段   审核结果
        /// <summary>
        /// 审核结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "ser_chk_code_user", MedName = "ser_chk_code_user", WFName = "ser_chk_code_user", DBColumn = false)]
        public String SerChkCodeUser { get; set; }
        #endregion

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime DeStartTime { get; set; }
        
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime DeEndTime { get; set; }

        /// <summary>
        /// 物理组ID
        /// </summary>
        public String StrCtypeId { get; set; }

    }
}
