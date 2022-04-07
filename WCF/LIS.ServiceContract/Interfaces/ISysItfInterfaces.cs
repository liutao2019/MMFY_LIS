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
    ///院网接口操作接口
    /// </summary>
    [ServiceContract]
    public interface ISysItfInterfaces
    {
        /// <summary>
        /// 保存接口
        /// </summary>
        /// <param name="patid"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse SaveSysInterface(EntityRequest request);

        /// <summary>
        /// 更新接口
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse UpdateSysInterface(EntityRequest request);



        /// <summary>
        /// 删除接口
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        EntityResponse DeleteSysInterface(string  id);

        /// <summary>
        /// 获取接口
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntitySysItfInterface> GetSysInterface();

        /// <summary>
        /// 接口测试连接
        /// </summary>
        /// <param name="inter"></param>
        /// <returns></returns>
        [OperationContract]
        DataSet TestConnection(EntitySysItfInterface inter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardData"></param>
        /// <param name="printType"></param>
        /// <returns></returns>
        [OperationContract]
        EntityOperationResult CardDataConvert(string cardData, string interfaceKey);




    }
}
