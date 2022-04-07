using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoFrmParameter
    {
        /// <summary>
        /// 查询质控规则表数据
        /// </summary>
        /// <returns></returns>
        List<EntityDicQcRule> SearchQcRule();

        /// <summary>
        /// 查询仪器通道码数据
        /// </summary>
        /// <param name="itrId"></param>
        /// <returns></returns>
        List<EntityDicMachineCode> SearchMitmNo(string itrId); 

        /// <summary>
        /// 删除质控项目，质控物明细和qc_sample表数据
        /// </summary>
        /// <param name="drQc_Detail"></param>
        /// <returns></returns>
        bool DeleteParDetailNew(EntityDicQcMateria drQc_Detail);

        /// <summary>
        /// 删除质控项目，质控规则关联数据
        /// </summary>
        /// <param name="drQc"></param>
        /// <param name="drParDeta"></param>
        /// <returns></returns>
        bool DeleteParNewRule(EntityDicQcMateriaDetail drQc, EntityDicQcMateria drParDeta);

        /// <summary>
        /// 先删除后插入质控规则关联数据
        /// </summary>
        /// <param name="dtSample"></param>
        bool ViewQcParRule(List<EntityDicQcMateriaRule> dtSample);

    }
}
