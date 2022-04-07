using System;
using System.Collections.Generic;

using System.Text;

namespace dcl.client.sample
{
    /// <summary>
    /// 签名记录
    /// </summary>
    public class SignInfo : IEmpty
    {
        /// <summary>
        /// 签名时间
        /// </summary>
        public string SignTime { get; set; }

        /// <summary>
        /// 签名人登录ID
        /// </summary>
        public string LoginID { get; set; }

        /// <summary>
        /// 签名人姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 签名地点
        /// </summary>
        public string SignPlace { get; set; }

        /// <summary>
        /// 签名人工号
        /// </summary>
        public string SignWorkId { get; set; }

        /// <summary>
        /// 签名备注
        /// </summary>
        public string Remark { get; set; }

        public SignInfo()
        {
            IsEmpty = true;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="loginID">签名人登陆ID</param>
        /// <param name="userName">签名人</param>
        public SignInfo(string loginID, string userName)
        {
            this.LoginID = loginID;
            this.UserName = userName;
        }

        #region IEmpty 成员

        public bool IsEmpty
        {
            get;
            set;
        }

        #endregion
    }
}
