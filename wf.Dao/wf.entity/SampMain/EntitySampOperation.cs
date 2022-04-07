using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 条码操作信息表
    /// </summary>
    [Serializable]
    public class EntitySampOperation : EntityBase
    {
        public EntitySampOperation()
        {

        }

        public EntitySampOperation(string operationID, string operationName)
        {
            this.OperationID = operationID;
            this.OperationName = operationName;
        }

        /// <summary>
        /// 条码操作信息表-操作状态(必填)
        /// 0-未打印,1-打印,2-采集, 3-已收取,4-已送检,5-签收,6-检验中,7-已检验,8-二次送检,9-条码回退
        /// 20-资料登记,530-删除病人资料,40-一审,50-一审反审,60-二审,70-二审反审，80-中期报告(细菌)
        /// </summary>
        public string OperationStatus { get; set; }

        /// <summary>
        /// 操作状态名称
        /// </summary>
        public string OperationStatusName { get; set; }

        /// <summary>
        /// 操作时间(必填)
        /// </summary>
        public DateTime OperationTime { get; set; }

        /// <summary>
        /// 操作人登录ID
        /// </summary>
        public string OperationID { get; set; }

        /// <summary>
        /// 操作人姓名
        /// </summary>
        public string OperationName { get; set; }

        /// <summary>
        /// 操作地点
        /// </summary>
        public string OperationPlace { get; set; }

        /// <summary>
        /// 操作人工号
        /// </summary>
        public string OperationWorkId { get; set; }

        /// <summary>
        /// 操作人输入码（对照码，一般用于对照外部系统id）
        /// </summary>
        public string OperationUserInCode { get; set; }

        /// <summary>
        /// 操作备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 操作地点IP
        /// </summary>
        public string OperationIP { get; set; }

        /// <summary>
        /// 病人标识ID
        /// </summary>
        public String RepId { get; set; }
    }
}
