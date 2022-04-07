using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 病人信息扩展表实体类(部分字段)
    /// </summary>
    [Serializable]
    public class EntityAuditInfo : EntityBase
    {
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
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
        public string MsgContent { get; set; }

        /// <summary>
        /// 确认方式 1-自动确认 2-手工确认
        /// </summary>
        [System.ComponentModel.Description("确认方式")]
        public string MsgAffirmType { get; set; }

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
        public string MsgDocNum { get; set; }

        /// <summary>
        /// 临床姓名
        /// </summary>
        [System.ComponentModel.Description("临床姓名")]
        public string MsgDocName { get; set; }

        /// <summary>
        /// 临床电话
        /// </summary>
        [System.ComponentModel.Description("临床电话")]
        public string MsgDepTel { get; set; }

        #endregion


        public EntityAuditInfo()
        {
            this.MsgAffirmType = "1";//默认为-自动确认
        }

        public EntityAuditInfo(string userId, string userName)
        {
            this.UserId = userId;
            this.UserName = userName;
            this.MsgAffirmType = "1";//默认为-自动确认
        }

    }
}
