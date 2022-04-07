using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoSysParameter
    {
        /// <summary>
        /// 根据配置类型查找配置
        /// </summary>
        /// <returns></returns>
        List<EntitySysParameter> GetSysParaByConfigType(string configType="system");

        /// <summary>
        /// 获取配置的缓存
        /// </summary>
        /// <returns></returns>
        List<EntitySysParameter> GetSysParaCache();

        /// <summary>
        /// 根据配置代码查找配置
        /// </summary>
        /// <param name="configCode"></param>
        /// <returns></returns>
        List<EntitySysParameter> GetSysParaByConfigCode(string configCode);

        /// <summary>
        /// 根据配置代码查找配置
        /// </summary>
        /// <param name="configCode"></param>
        /// <returns></returns>
        List<EntitySysParameter> GetSysParaByType(string configType);


        /// <summary>
        /// 根据配置ID更新配置
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        bool UpdateSysParaByConfigId(EntitySysParameter para);

        /// <summary>
        /// 插入一条配置信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        bool InsertSysPara(EntitySysParameter para);

    } 
}
