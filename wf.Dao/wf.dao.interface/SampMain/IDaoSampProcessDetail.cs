using dcl.dao.core;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoSampProcessDetail: IDaoBase
    {
        /// <summary>
        /// 查询标本流转明细数据
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        List<EntitySampProcessDetail> GetSampProcessDetail(String sampBarId);

        /// <summary>
        /// 获取标本流传状态  修改日志中使用
        /// </summary>
        /// <param name="repId"></param>
        /// <returns></returns>
        List<EntitySampProcessDetail> GetSamprocessDetailByRepId(String repId);

        /// <summary>
        /// 保存操作信息
        /// </summary>
        /// <param name="sampProcessDetial"></param>
        /// <returns></returns>
        Boolean SaveSampProcessDetail(EntitySampProcessDetail sampProcessDetial);

        /// <summary>
        /// 获取已被删除的病人id
        /// </summary>
        /// <param name="patId"></param>
        /// <param name="patName"></param>
        /// <param name="timeFrom"></param>
        /// <param name="timeTo"></param>
        /// <returns></returns>
        String GetDeletePatId(string patId, string patName, string timeFrom, string timeTo);
    }
}
