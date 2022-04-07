using dcl.dao.core;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoObrResultDesc : IDaoBase
    {
        /// <summary>
        /// 根据标识ID查询描述报告结果
        /// </summary>
        /// <param name="bsrId"></param>
        /// <returns></returns>
        List<EntityObrResultDesc> GetDescResultById(string obrId = "",string repFlag="1");


        /// <summary>
        /// 获取描述报告结果
        /// </summary>
        /// <param name="bsrId"></param>
        /// <returns></returns>
        List<EntityObrResultDesc> GetObrResultDescById(string obrId );

        /// <summary>
        /// 新增描述报告结果信息
        /// </summary>
        /// <param name="ObrResultDesc"></param>
        /// <returns></returns>
        bool InsertObrResultDesc(EntityObrResultDesc ObrResultDesc);

        /// <summary>
        /// 根据标识ID删除结果
        /// </summary>
        /// <param name="obrId"></param>
        /// <returns></returns>
        Boolean DeleteResultById(string obrId);

        /// <summary>
        /// 更新描述报告结果
        /// </summary>
        /// <param name="ObrResultDesc"></param>
        /// <returns></returns>
        Boolean UpdateObrResultDesc(EntityObrResultDesc ObrResultDesc);
    }
}
