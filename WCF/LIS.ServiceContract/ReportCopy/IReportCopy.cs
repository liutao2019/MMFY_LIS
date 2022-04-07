using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 常规检验：报告复制（方法对应的接口）
    /// </summary>
    [ServiceContract]
    public interface IReportCopy
    {
        /// <summary>
        /// 复制患者信息(可指定目标样本号)
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="dtTime"></param>
        /// <param name="strItrId"></param>
        /// <param name="newSid"></param>
        /// <returns></returns>
        [OperationContract]
        String CopyPatientsInfoCustomSid(List<string> pat_id, DateTime dtTime, String strItrId, List<decimal> newSid);

        /// <summary>
        /// 复制患者信息(非指定目标样本号)
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="dtTime"></param>
        /// <param name="strItrId"></param>
        /// <returns></returns>
        [OperationContract]
        String CopyPatientsInfo(List<string> pat_id, DateTime dtTime, String strItrId);
    }
}
