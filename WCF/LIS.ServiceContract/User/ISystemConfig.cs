using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface ISystemConfig
    {
        /// <summary>
        /// 根据配置类型获取配置列表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntitySysParameter> GetSysParaListByConfigType(string configType = "system");

        /// <summary>
        /// 获取系统配置缓存
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntitySysParameter> GetSysParaCaChe();


        /// <summary>
        /// 根据配置代码获取配置列表
        /// </summary>
        /// <param name="configCode"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySysParameter> GetSysParaListByConfigCode(string configCode);

        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateSysPara(List<EntitySysParameter> para);

        /// <summary>
        /// 插入一条配置
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [OperationContract]
        bool InsertSysPara(EntitySysParameter para);
    }
}
