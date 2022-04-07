using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ServiceModel;



using dcl.entity;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IElisaAnalyse
    {
        /// <summary>
        /// 获取酶标板孔原始结果
        /// </summary>
        /// <param name="ItrId"></param>
        /// <param name="dtImmDate"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityObrElisaResult> GetElisaResult(string ItrId, DateTime dtImmDate);
        /// <summary>
        /// 根据仪器ID获取半定量数据
        /// </summary>
        /// <param name="ItrId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicQcConvert> GetQCConvert(string ItrId);

        /// <summary>
        /// 根据仪器ID获取质控结果数据
        /// </summary>
        /// <param name="ItrId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityObrQcResult> GetQCResult(string ItrId);

        /// <summary>
        /// 更新酶标板孔原始结果的原始结果值
        /// </summary>
        /// <param name="ResId"></param>
        /// <param name="ResValue"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateResValue(string ResId, string ResValue);

        /// <summary>
        /// 更新酶标板孔原始结果数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse UpdateElisaResult(EntityRequest request);
        /// <summary>
        /// 保存质控结果表信息
        /// </summary>
        /// <param name="dtEiasa"></param>
        /// <returns></returns>
        [OperationContract]
        bool SaveQcValue(List<EntityElisaQc> dtEiasa);
        /// <summary>
        /// 保存批量录入的数据结果
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        [OperationContract]
        string SaveBatchObrResult(List<EntityObrResult> dt);
    }
}
