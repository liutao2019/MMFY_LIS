using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    ///院网接口操作接口
    /// </summary>
    [ServiceContract]
    public interface ISysItfContrast
    {
        /// <summary>
        /// 保存数据对照
        /// </summary>
        /// <param name="patid"></param>
        /// <returns></returns>
        [OperationContract]
        bool SaveSysContrast(EntitySysItfContrast entity);

        /// <summary>
        /// 更新数据对照
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateSysContrast(EntitySysItfContrast entity);


        /// <summary>
        /// 删除数据对照
        /// </summary>
        /// <param name="conId"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteSysContrast(string conId);

        /// <summary>
        /// 获取接口
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntitySysItfInterface> GetSysInterface();

        /// <summary>
        /// 获取数据对照
        /// </summary>
        /// <param name="interfaceId">接口ID</param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySysItfContrast> GetSysContrast(string interfaceId);


    }
}
