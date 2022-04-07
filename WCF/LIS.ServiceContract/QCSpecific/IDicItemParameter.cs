using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IDicItemParameter
    {
        /// <summary>
        /// 查询质控规则表数据
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicQcRule> SearchQcRule();

        /// <summary>
        /// 查询仪器通道码数据
        /// </summary>
        /// <param name="itrId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicMachineCode> SearchMitmNo(string itrId); //调用质控通道的查询方法

        /// <summary>
        /// 删除质控项目，质控物明细和qc_sample表数据
        /// </summary>
        /// <param name="drQc_Detail"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteParDetailNew(EntityDicQcMateria drQc_Detail);//qc_sample未处理
        
        /// <summary>
        /// 删除质控项目，质控规则关联数据
        /// </summary>
        /// <param name="drQc"></param>
        /// <param name="drParDeta"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteParNewRule(EntityDicQcMateriaDetail drQc, EntityDicQcMateria drParDeta);

        /// <summary>
        /// 先删除后插入质控规则关联数据
        /// </summary>
        /// <param name="dtSample"></param>
        [OperationContract]
        bool ViewQcParRule(List<EntityDicQcMateriaRule> dtSample);
    }
}
