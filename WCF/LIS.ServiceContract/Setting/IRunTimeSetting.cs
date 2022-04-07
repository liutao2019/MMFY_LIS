using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 用户配置接口文件
    /// </summary>
    [ServiceContract]
    public interface IRunTimeSetting
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="eySysInfeLog"></param>
        /// <returns></returns>
        [OperationContract]
        void Save(string key, byte[] data);

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="eySysInfeLog"></param>
        /// <returns></returns>
        [OperationContract]
        byte[] Load(string key);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        [OperationContract]
        void Delete(string key);
    }
}
