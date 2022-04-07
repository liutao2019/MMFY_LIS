using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IResultTemp
    {
        /// <summary>
        /// 根据项目ID获取项目特征
        /// </summary>
        /// <param name="listStr"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDefItmProperty> GetItemProp(List<string> listStr);

        /// <summary>
        /// 根据仪器ID获取仪器组合
        /// </summary>
        /// <param name="ItrId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicItrCombine> GetItrCombine(string ItrId);

        /// <summary>
        /// 根据组合ID集合获取组合明细
        /// </summary>
        /// <param name="comId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicCombineDetail> GetCombDetail(List<string> comId);

        /// <summary>
        /// 根据组合ID和仪器ID获取组合明细
        /// </summary>
        /// <param name="comId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicCombineDetail> GetCombDetailByComIdAndItrId(List<string> comId);

        /// <summary>
        /// 根据条件获取病人样本号集合
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [OperationContract]
        List<string> GetPatientsSid(EntityAnanlyseQC query);

        /// <summary>
        /// 获取无菌列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicMicSmear> GetSmearList(EntityRequest request);
    }
}
