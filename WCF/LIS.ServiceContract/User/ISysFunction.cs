using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface ISysFunction
    {

        /// <summary>
        ///删除一条功能
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteFunc(EntitySysFunction func);

        /// <summary>
        /// 插入一条功能模块
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        [OperationContract]
        bool InsertAFunc(EntitySysFunction func);

        /// <summary>
        /// 更新功能模块
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateAFunc(EntitySysFunction func);

        /// <summary>
        /// 获取功能模块列表
        /// </summary>
        /// <param name="whereSql"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySysFunction> GetFuncList(string whereSql = "");
    }
}
