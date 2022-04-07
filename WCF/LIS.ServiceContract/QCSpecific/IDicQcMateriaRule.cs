using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 质控规则关联表：接口
    /// </summary>
    [ServiceContract]
    public interface IDicQcMateriaRule
    {
        /// <summary>
        /// 查询质控规则关联数据
        /// </summary>
        /// <param name="matSn"></param>
        /// <param name="matItmId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicQcMateriaRule> SearchQcMateriaRule(string matSn, string matItmId);

        /// <summary>
        /// 保存质控规则关联数据
        /// </summary>
        /// <param name="qcMateriaRule"></param>
        /// <returns></returns>
        [OperationContract]
        bool SaveQcMateriaRule(EntityDicQcMateriaRule qcMateriaRule);

        /// <summary>
        /// 删除质控规则关联表数据
        /// </summary>
        /// <param name="matSn"></param>
        /// <param name="matId"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteQcMateriaRule(string matSn, string matId);

        /// <summary>
        /// 根据关联信息查询出质控规则
        /// </summary>
        /// <param name="strMatSn">质控物ID</param>
        /// <param name="strItmId">项目ID</param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicQcRule> GetQcRule(List<String> listMatSnItmId);
    }
}
