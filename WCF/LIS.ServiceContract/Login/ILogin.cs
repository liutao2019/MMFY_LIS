using dcl.entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    ///系统登录
    /// </summary>
    [ServiceContract]
    public interface ILogin
    {
        /// <summary>
        /// 客户端登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        EntityLoginUserInfo CsLogin(EntityRequest request);

        /// <summary>
        /// 电子签名验证转换
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse ModeChange(EntityRequest request);


        /// <summary>
        /// 插入系统日志
        /// </summary>
        /// <param name="type"></param>
        /// <param name="module"></param>
        /// <param name="loginID"></param>
        /// <param name="ip"></param>
        /// <param name="mac"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [OperationContract]
        bool InsertSystemLog(string type, string module, string loginID, string ip, string mac, string message);
    }
}
