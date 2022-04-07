using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IPatientRecheck
    {
        ///// <summary>
        ///// 插入病人复查信息
        ///// </summary>
        ///// <param name="recheck"></param>
        ///// <returns></returns>
        //[OperationContract]
        //bool InsertPatientRecheck(EntityPatientRecheck recheck);

        ///// <summary>
        ///// 删除病人复查信息
        ///// </summary>
        ///// <param name="recheck"></param>
        ///// <returns></returns>
        //[OperationContract]
        //bool DeletePatientRecheck(EntityPatientRecheck recheck);

        /// <summary>
        /// 复查病人结果
        /// </summary>
        /// <param name="repId"></param>
        /// <param name="listResult"></param>
        /// <returns></returns>
        [OperationContract]
        bool RecheckResultItem(string repId, List<EntityObrResult> listResult);
    }
}
