using dcl.dao.core;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoObrResultAnti:IDaoBase
    {
        /// <summary>
        /// 根据标识ID查找药敏结果
        /// </summary>
        /// <param name="obrId"></param>
        /// <param name="searchFlag"></param>
        /// <returns></returns>
        List<EntityObrResultAnti> GetAntiResultById(string obrId);

        /// <summary>
        /// 根据标识ID查找药敏结果及历史结果
        /// </summary>
        /// <param name="obrId"></param>
        /// <param name="searchFlag"></param>
        /// <returns></returns>
        List<EntityObrResultAnti> GetAntiWithHistoryResultById(string obrId);

        /// <summary>
        /// 根据标识ID删除结果
        /// </summary>
        /// <param name="obrId"></param>
        /// <returns></returns>
        Boolean DeleteResultById(string obrId);

        /// <summary>
        /// 保存药敏结果
        /// </summary>
        /// <param name="antiResult"></param>
        /// <returns></returns>
        Boolean SaveResultAnti(EntityObrResultAnti antiResult);

        /// <summary>
        /// 根据标识ID集合查找药敏结果（查找历史结果）
        /// </summary>
        /// <param name="listObrId"></param>
        /// <returns></returns>
        List<EntityObrResultAnti> GetAntiResultByListObrId(List<string>listObrId);

        /// <summary>
        /// 获取抗生素名称
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        List<string> GetAntibosName(EntityAntiQc qc);

        /// <summary>
        /// 获取药敏数据
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        List<EntityWhonet> GetAntiData(EntityAntiQc qc);

    }
}
