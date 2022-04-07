using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IDicFastAddQcItem
    {
        /// <summary>
        /// 查询项目、质控规则项目、质控规则关联表数据
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse SearchItemQcParQcRule(List<EntityDicQcMateriaDetail> listMD, string insId);

        /// <summary>
        /// 插入质控项目、质控规则关联数据
        /// </summary>
        /// <param name="listMD"></param>
        /// <param name="listMR"></param>
        /// <returns></returns>
        [OperationContract]
        bool SaveQcMateriaDetailOrRule(List<EntityDicQcMateriaDetail> listMD, List<EntityDicQcMateriaRule> listMR);
    }
}
