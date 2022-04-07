using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// whonet操作接口
    /// </summary>
    [ServiceContract]
    public interface IWhonet
    {
        /// <summary>
        /// 获取记录数量(抗生素)只获取抗生素名称，用于分段获取防止超时
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        [OperationContract]
        List<string> GetAntibosName(EntityAntiQc qc);

        /// <summary>
        /// 获取药敏数据
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityWhonet> GetAntiData(EntityAntiQc qc);

        /// <summary>
        /// 更新药敏信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean UpdateAnti(EntityRequest request);

        /// <summary>
        /// 更新细菌信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean UpdateBac(EntityRequest request);

    }
}
