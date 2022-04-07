using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 操作记录实体类文件
    /// </summary>
    [Serializable]
    public class EntitySysInterfaceLog : EntityBase
    {
        public EntitySysInterfaceLog()
        {
            OperationTime = DateTime.Now;
            RepeatCount = 0;
        }

        public EntitySysInterfaceLog(string sampBarId,
                                     string orderSn,
                                     string operationName,
                                     string operationUserCode,
                                     string operationUserName,
                                     bool success,
                                     string operationContent)
        {
            SampBarId = sampBarId;
            OrderSn = orderSn;
            OperationName = operationName;
            OperationUserCode = operationUserCode;
            OperationUserName = operationUserName;
            OperationTime = DateTime.Now;
            OperationSuccess = success ? 1 : 0;
            OperationContent = operationContent;
            RepeatCount = 0;
        }
        /// <summary>
        /// 主键 自增ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "operation_key", MedName = "operation_key", WFName = "operation_key", DBIdentity = true)]
        public Int32 OperationKey { get; set; }

        /// <summary>
        /// 条码号
        /// </summary>   
        [FieldMapAttribute(ClabName = "samp_bar_id", MedName = "samp_bar_id", WFName = "samp_bar_id")]
        public String SampBarId { get; set; }

        /// <summary>
        /// 医嘱ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "order_sn", MedName = "order_sn", WFName = "order_sn")]
        public String OrderSn { get; set; }

        /// <summary>
        /// 报告标识ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "rep_id", MedName = "rep_id", WFName = "rep_id")]
        public String RepId { get; set; }

        /// <summary>
        /// 执行操作名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "operation_name", MedName = "operation_name", WFName = "operation_name")]
        public String OperationName { get; set; }

        /// <summary>
        /// 操作人代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "operation_user_code", MedName = "operation_user_code", WFName = "operation_user_code")]
        public String OperationUserCode { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "operation_user_name", MedName = "operation_user_name", WFName = "operation_user_name")]
        public String OperationUserName { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "operation_time", MedName = "operation_time", WFName = "operation_time")]
        public DateTime OperationTime { get; set; }

        /// <summary>
        /// 操作状态(0失败,1成功)
        /// </summary>   
        [FieldMapAttribute(ClabName = "operation_success", MedName = "operation_success", WFName = "operation_success")]
        public Int32? OperationSuccess { get; set; }

        /// <summary>
        /// 操作信息
        /// </summary>   
        [FieldMapAttribute(ClabName = "operation_content", MedName = "operation_content", WFName = "operation_content")]
        public String OperationContent { get; set; }

        /// <summary>
        /// 重复次数
        /// </summary>   
        [FieldMapAttribute(ClabName = "repeat_count", MedName = "repeat_count", WFName = "repeat_count")]
        public Int32 RepeatCount { get; set; }

        #region 附加字段 操作开始时间
        public DateTime? OperaBeginDateTime { get; set; }
        #endregion

        #region 附加字段 操作结束时间
        public DateTime? OperaEndDateTime { get; set; }
        #endregion

        #region 附加字段 操作状态名称
        public String OperationSuccessName
        {
            get
            {
                string strName = string.Empty;
                if (OperationSuccess == 1)
                {
                    strName = "操作成功";
                }
                else if (OperationSuccess == 0)
                {
                    strName = "操作失败";
                }
                else
                    strName = "";

                return strName;
            }
        }
        #endregion
    }
}
