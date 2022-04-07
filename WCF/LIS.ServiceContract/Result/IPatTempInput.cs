using dcl.entity;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IPatTempInput
    {
        /// <summary>
        /// 获取病人列表(详细信息)
        /// 查找指定组别的所有病人资料时,把itr_id赋空字串
        /// </summary>
        /// <param name="dtFrom"></param>
        /// <param name="dtTo"></param>
        /// <param name="type_id"></param>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        [OperationContract]
        List<entity.EntityPidReportMain> GetPatientsDetail(DateTime dtFrom, DateTime dtTo, string type_id, string itr_id,string sid);

        /// <summary>
        /// 保存病人、普通结果信息
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="patients"></param>
        /// <param name="dsData"></param>
        /// <returns></returns>
        [OperationContract]
        EntityOperateResult InsertPatCommonResult(EntityRemoteCallClientInfo caller, EntityQcResultList resultList);

        /// <summary>
        /// 根据病人标识ID获取病人组合明细
        /// </summary>
        /// <param name="repId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportDetail> GetPidReportDetailByRepId(string repId);
    }
}
