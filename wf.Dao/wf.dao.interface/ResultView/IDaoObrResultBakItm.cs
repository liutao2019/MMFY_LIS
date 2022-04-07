using dcl.entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{

    public interface IDaoObrResultBakItm
    {

        /// <summary>
        /// 获取项目结果备份信息
        /// </summary>
        /// <param name="RepId"></param>
        /// <param name="whereType"></param>
        /// <returns></returns>
        List<EntityObrResultBakItm> GetObrResultBakItm(string RepId, int whereType);

        /// <summary>
        /// 新增项目结果备份信息
        /// </summary>
        /// <param name="resId"></param>
        /// <param name="bakId"></param>
        /// <param name="bakDate"></param>
        /// <param name="resItmIds"></param>
        /// <param name="resKeys"></param>
        /// <returns></returns>
        String InsertObrResultBakItm(string resId, string bakId, DateTime bakDate, List<string> resItmIds, List<string> resKeys);

        /// <summary>
        /// 还原项目结果备份
        /// </summary>
        /// <param name="resKeys"></param>
        /// <returns></returns>
        String InsertObrResultByBak(List<string> resKeys);

    }
}
