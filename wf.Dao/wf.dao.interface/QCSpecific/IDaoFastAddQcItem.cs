using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoFastAddQcItem
    {
        /// <summary>
        /// 查询项目、质控规则项目、质控规则关联表数据
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        EntityResponse SearchItemQcParQcRule(List<EntityDicQcMateriaDetail> listMD, string insId);

        /// <summary>
        /// 插入质控项目、质控规则关联数据
        /// </summary>
        /// <param name="listMD"></param>
        /// <param name="listMR"></param>
        /// <returns></returns>
        bool SaveQcMateriaDetailOrRule(List<EntityDicQcMateriaDetail> listMD,List<EntityDicQcMateriaRule> listMR);
    }
}
