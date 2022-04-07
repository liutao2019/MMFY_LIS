using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 操作日志记录
    /// 旧表名:sys_operation_log  新表名:Base_operation_log
    /// </summary>
    [Serializable()]
    public class EntitySysOperationLog : EntityBase
    {
        public EntitySysOperationLog()
        {

        }
        public EntitySysOperationLog(string group, string operationType, string content, string desc)
        {
            OperatGroup = group;
            OperatAction = operationType;
            OperatObject = content;
            OperatContent = desc;
        }
        /// <summary>
        ///日志ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "ID", MedName = "log_sn", WFName = "Boper_sn")]
        public Int32 OperatLogSn { get; set; }

        /// <summary>
        ///操作人ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "OperatorID", MedName = "operat_user_id", WFName = "Boper_Buser_id")]
        public String OperatUserId { get; set; }

        /// <summary>
        ///IP地址
        /// </summary>   
        [FieldMapAttribute(ClabName = "IPAddress", MedName = "operat_servername", WFName = "Boper_servername")]
        public String OperatServername { get; set; }

        /// <summary>
        ///操作时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "OperationTime", MedName = "operat_date", WFName = "Boper_date")]
        public DateTime OperatDate { get; set; }

        /// <summary>
        ///操作值
        /// </summary>   
        [FieldMapAttribute(ClabName = "OperationKey", MedName = "operat_key", WFName = "Boper_key")]
        public String OperatKey { get; set; }

        /// <summary>
        ///操作模块
        /// </summary>   
        [FieldMapAttribute(ClabName = "Module", MedName = "operat_module", WFName = "Boper_module")]
        public String OperatModule { get; set; }

        /// <summary>
        ///操作项目
        /// </summary>   
        [FieldMapAttribute(ClabName = "Group", MedName = "operat_group", WFName = "Boper_group")]
        public String OperatGroup { get; set; }

        /// <summary>
        ///操作方式
        /// </summary>   
        [FieldMapAttribute(ClabName = "OperationType", MedName = "operat_action", WFName = "Boper_action")]
        public String OperatAction { get; set; }

        /// <summary>
        ///操作内容
        /// </summary>   
        [FieldMapAttribute(ClabName = "OperationContent", MedName = "operat_object", WFName = "Boper_object")]
        public String OperatObject { get; set; }

        /// <summary>
        ///描述
        /// </summary>   
        [FieldMapAttribute(ClabName = "Description", MedName = "operat_content", WFName = "Boper_content")]
        public String OperatContent { get; set; }

        #region 附加字段
        /// <summary>
        /// 操作人姓名
        /// </summary>
        [FieldMapAttribute(ClabName = "userName", MedName = "user_name", WFName = "Buser_name", DBColumn = false)]
        public String OperatUserName { get; set; }
        #endregion
    }
}
