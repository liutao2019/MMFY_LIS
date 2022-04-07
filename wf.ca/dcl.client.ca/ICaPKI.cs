using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.client.ca
{
    /// <summary>
    /// CA签名接口
    /// </summary>
    public interface ICaPKI
    {
        string UserId { get; set; }
        /// <summary>
        /// CA模式(SZCA/NETCA...)
        /// </summary>
        string CAMode { get; }


        /// <summary>
        /// 错误信息
        /// </summary>
        string ErrorInfo { get; set; }


        /// <summary>
        /// 当前证书公钥
        /// </summary>
        string CurrentKey { get; set; }

        /// <summary>
        /// 数字签名
        /// </summary>
        /// <param name="plainData"></param>
        /// <returns></returns>
        string CASignature(string plainData);


        /// <summary>
        /// 时间戳签名
        /// </summary>
        /// <param name="plainData"></param>
        /// <returns></returns>
        string CATimeStamp(string plainData);

        /// <summary>
        /// 获取用户证书绑定值
        /// </summary>
        /// <returns></returns>
        string GetIdentityID();

        /// <summary>
        /// CA认证登录
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        bool LoginWithCA(EntityLogin login);
    }
}
