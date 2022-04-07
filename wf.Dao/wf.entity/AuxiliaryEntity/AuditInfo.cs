using System;
using System.Collections.Generic;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public class AuditInfo
    {
        public string Password { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }

        /// <summary>
        /// 工号ID
        /// </summary>
        public string UserStfId { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string UserRole { get; set; }


        #region 危急值专用成员

        /// <summary>
        /// 危急值编辑内容
        /// </summary>
        public string msg_content { get; set; }

        /// <summary>
        /// 确认方式 1-自动确认 2-手工确认
        /// </summary>
        [System.ComponentModel.Description("确认方式")]
        public string msg_affirm_type { get; set; }

        /// <summary>
        /// 是否只内部确认
        /// </summary>
        [System.ComponentModel.Description("是否只内部确认")]
        public bool IsOnlyInsgin { get; set; }

        /// <summary>
        /// 是否保存危急值编辑内容
        /// </summary>
        public bool IsSaveMsg { get; set; }
        /// <summary>
        /// 扩展信息
        /// </summary>
        public string ExetMsg { get; set; }
        /// <summary>
        /// 地点
        /// </summary>
        public string Place { get; set; }

        /// <summary>
        /// 发送短信
        /// </summary>
        public bool SendMsgFlag { get; set; }

        /// <summary>
        /// 临床工号
        /// </summary>
        [System.ComponentModel.Description("临床工号")]
        public string msg_doc_num { get; set; }

        /// <summary>
        /// 临床姓名
        /// </summary>
        [System.ComponentModel.Description("临床姓名")]
        public string msg_doc_name { get; set; }

        /// <summary>
        /// 临床电话
        /// </summary>
        [System.ComponentModel.Description("临床电话")]
        public string msg_dep_tel { get; set; }

        #endregion


        public AuditInfo()
        {
            this.msg_affirm_type = "1";//默认为-自动确认
        }

    }
}
