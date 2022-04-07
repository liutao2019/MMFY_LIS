using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoSysFunction
    {
        /// <summary>
        ///删除一条功能
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        bool DeleteFunc(EntitySysFunction func);

        /// <summary>
        /// 插入一条功能模块
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        bool InsertAFunc(EntitySysFunction func);

        /// <summary>
        /// 更新功能模块
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        bool UpdateAFunc(EntitySysFunction func);

        /// <summary>
        /// 获取功能模块列表
        /// </summary>
        /// <param name="whereSql"></param>
        /// <returns></returns>
        List<EntitySysFunction> GetFuncList(string whereSql="");

        /// <summary>
        /// 根据登录ID来获取功能模块列表
        /// </summary>
        /// <param name="logionid"></param>
        /// <param name="whereSql"></param>
        /// <returns></returns>
        List<EntitySysFunction> GetFuncListByLogionId(string logionid,string whereSql);

        /// <summary>
        /// 获得功能模块名称
        /// </summary>
        /// <returns></returns>
        List<EntitySysFunction> GetFuncName();
    }
}
