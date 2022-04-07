using dcl.entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IShelfSampRegister
    {

        /// <summary>
        /// 试管条码病人登记
        /// </summary>
        /// <param name="receviceDeptID">接收室Id</param>
        /// <param name="regDateFrom">开始登记日期</param>
        /// <param name="regDateTo">结束登记日期</param>
        /// <param name="shelfNoFrom">开始架子号</param>
        /// <param name="shelfNoTo">结束架号</param>
        /// <param name="seqFrom">开始编号</param>
        /// <param name="seqTo">结束编号</param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampRegister> GetCuvetteShelfInfo(string receviceDeptID, DateTime regDateFrom, DateTime regDateTo, int ?shelfNoFrom, int ?shelfNoTo, int ?seqFrom, int ?seqTo);
        /// <summary>
        /// 保存病人信息
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="listEntity"></param>
        /// <returns></returns>
        [OperationContract]
       EntityResponse SavePatients(EntityRemoteCallClientInfo caller, List<EntityShelfSampToReportMain> listEntity);

        /// <summary>
        /// 获取仪器组合
        /// </summary>
        /// <param name="itrId">仪器id</param>
        /// <param name="getMergeItrCombine">是否获取</param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicItrCombine> GetItrCombine(string itrId, bool getMergeItrCombine);
    }
}
