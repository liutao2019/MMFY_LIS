using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    /// <summary>
    /// 质控规则关联表：接口
    /// </summary>
    public interface IDaoQcMateriaRule
    {
        /// <summary>
        /// 查询质控规则关联数据
        /// </summary>
        /// <param name="matSn"></param>
        /// <param name="matItmId"></param>
        /// <returns></returns>
        List<EntityDicQcMateriaRule> SearchQcMateriaRule(string matSn, string matItmId);

        /// <summary>
        /// 保存质控规则关联数据
        /// </summary>
        /// <param name="qcMateriaRule"></param>
        /// <returns></returns>
        bool SaveQcMateriaRule(EntityDicQcMateriaRule qcMateriaRule);

        /// <summary>
        /// 删除质控规则关联表数据
        /// </summary>
        /// <param name="matSn"></param>
        /// <param name="matId"></param>
        /// <returns></returns>
        bool DeleteQcMateriaRule(string matSn, string matId);

        /// <summary>
        /// 根据关联信息查询出质控规则
        /// </summary>
        /// <returns></returns>
        List<EntityDicQcRule> GetQcRule(List<String> listMatSnItmId);


    }
}
