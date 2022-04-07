using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IObrResultBakItm
    {
        /// <summary>
        /// 获取项目结果备份信息
        /// </summary>
        /// <param name="RepId"></param>
        /// <param name="whereType"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityObrResultBakItm> GetObrResultBakItm(string RepId, int whereType);

        /// <summary>
        /// 项目结果备份
        /// </summary>
        /// <param name="resId"></param>
        /// <param name="bakId"></param>
        /// <param name="bakDate"></param>
        /// <param name="resItmIds"></param>
        /// <param name="resKeys"></param>
        /// <returns></returns>
        [OperationContract]
        String InsertObrResultBakItm(string resId, List<string> resItmIds, List<string> resKeys);

        /// <summary>
        /// 项目结果还原
        /// </summary>
        /// <param name="resId"></param>
        /// <param name="resItmIds"></param>
        /// <param name="resKeys"></param>
        /// <returns></returns>
        [OperationContract]
        String InsertObrResultByBak(string resId, List<string> resItmIds, List<string> resKeys);
    }
}
