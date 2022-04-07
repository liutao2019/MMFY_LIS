using dcl.entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IObrResultDesc
    {
        /// <summary>
        /// 根据标识ID查询描述报告结果
        /// </summary>
        /// <param name="bsrId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityObrResultDesc> GetDescResultById(string obrId = "", string repFlag = "1");

        /// <summary>
        /// 新增描述报告结果信息
        /// </summary>
        /// <param name="listObrResultDesc"></param>
        /// <returns></returns>
        [OperationContract]
        bool InsertObrResultDesc(List<EntityObrResultDesc> listObrResultDesc);

        /// <summary>
        /// 根据标识ID删除结果
        /// </summary>
        /// <param name="obrId"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean DeleteResultById(string obrId);

        /// <summary>
        /// 更新描述结果
        /// </summary>
        /// <param name="ObrResultDesc"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean UpdateObrResultDesc(EntityObrResultDesc ObrResultDesc);

        /// <summary>
        /// 批量删除病人描述报告
        /// </summary>
        /// <param name="listPatID"></param>
        /// <param name="delWithResult"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityOperationResult> BatchDelPatDescResult(EntityRemoteCallClientInfo caller, List<string> listPatID, bool delWithResult);

    }
}
