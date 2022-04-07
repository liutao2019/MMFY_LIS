using dcl.dao.core;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoObrResultBact : IDaoBase
    {
        /// <summary>
        /// 根据病人标识ID查找细菌结果
        /// </summary>
        /// <param name="obrId"></param>
        /// <returns></returns>
        List<EntityObrResultBact> GetBactResultById(string obrId = "");

        /// <summary>
        /// 根据标识ID删除结果
        /// </summary>
        /// <param name="obrId"></param>
        /// <returns></returns>
        Boolean DeleteResultById(string obrId);

        /// <summary>
        /// 保存细菌结果
        /// </summary>
        /// <param name="bactResult"></param>
        /// <returns></returns>
        Boolean SaveResultBact(EntityObrResultBact bactResult);
    }
}
