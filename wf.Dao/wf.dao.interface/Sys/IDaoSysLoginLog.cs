using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoSysLoginLog
    {
        /// <summary>
        /// 记录登录日志
        /// </summary>
        /// <param name="type">登录状态</param>
        /// <param name="module">登录模块</param>
        /// <param name="loginID">登录帐号</param>
        /// <param name="ip">ip地址</param>
        /// <param name="mac">mac地址</param>
        /// <param name="message">详细信息</param>
        void LogLogin(string type, string module, string loginID, string ip, string mac, string message);

        /// <summary>
        /// 记录登出日志
        /// </summary>
        /// <param name="module">模块</param>
        /// <param name="loginID"></param>
        /// <param name="ip"></param>
        /// <param name="mac"></param>
        void LogLogout(string module, string loginID, string ip, string mac);

        /// <summary>
        /// 新增登录日志
        /// </summary>
        /// <param name="type"></param>
        /// <param name="module"></param>
        /// <param name="loginID"></param>
        /// <param name="ip"></param>
        /// <param name="mac"></param>
        /// <param name="message"></param>
        void Log(string type, string module, string loginID, string ip, string mac, string message);

    }
}
