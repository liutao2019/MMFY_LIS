using System;
using System.Collections.Generic;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    [Flags]
    public enum EnumMessageType
    {
        /// <summary>
        /// 未知
        /// </summary>
        UNKNOW = 0,

        /// <summary>
        /// 公告
        /// </summary>
        BULLETIN = 1,

        /// <summary>
        /// 个人消息
        /// </summary>
        USER = 2,

        /// <summary>
        /// 阅读回复
        /// </summary>
        REPLY = 4,

        /// <summary>
        /// 发送到角色的消息
        /// </summary>
        Role = 8,

        /// <summary>
        /// led屏消息1
        /// </summary>
        LED1 = 16,

        /// <summary>
        /// led屏消息2
        /// </summary>
        LED2 = 32,

        /// <summary>
        /// led屏消息3
        /// </summary>
        LED3 = 2048,

        /// <summary>
        /// 标本回退
        /// </summary>
        SAMPLE_RETURN = 64,

        /// <summary>
        /// 系统消息
        /// </summary>
        SYSTEM = 128,

        /// <summary>
        /// 触摸屏打印机消息
        /// </summary>
        TOUCHPRINT_PRINTER = 256,

        /// <summary>
        /// 召回病人
        /// </summary>
        CALL_BACK_PATIENT = 512,

        /// <summary>
        /// 危急值消息
        /// </summary>
        CRITICAL_MESSAGE = 1024,

        /// <summary>
        /// 自定义危急值消息
        /// </summary>
        DIY_CL_MESSAGE = 2024,

        /// <summary>
        ///  体检危急值消息
        /// </summary>
        TJ_CRITICAL_MESSAGE = 3024,

        /// <summary>
        /// 急查标志消息
        /// </summary>
        URGENT_MESSAGE = 4096,

       
    }
}
