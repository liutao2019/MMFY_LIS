using dcl.entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IBatchEditNew
    {
        /// <summary>
        /// 批量复制源病人结果到目标病人
        /// </summary>
        /// <param name="listPat"></param>
        /// <param name="listComb"></param>
        /// <returns></returns>
        [OperationContract]
        bool BatchCopyPatientData(EntityRemoteCallClientInfo caller, List<EntityPidReportMain> listPat, List<EntityDicCombine> listComb);

        /// <summary>
        /// 根据病人ID和组合ID更新病人检验结果
        /// </summary>
        /// <param name="patIdList"></param>
        /// <param name="comCode"></param>
        /// <returns></returns>
        [OperationContract]
        bool SetResultoBcFlag(List<string> patIdList, List<string> comCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityOperateResult> BatchUpdatePatientData(BatchEditSrc source, BatchEditDest dest);
    }
}
