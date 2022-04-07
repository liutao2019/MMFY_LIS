using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.ServiceModel;

using dcl.entity;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IOfficeHoRecord
    {


        /// <summary>
        /// 获得交接班状态
        /// </summary>
        /// <param name="dtFrom"></param>
        /// <param name="dtTo"></param>
        /// <param name="ctypeID"></param>
        /// <returns></returns>
        [OperationContract]
        EntityHoRecord GetHandoverStat(DateTime dtFrom, DateTime dtTo, string ctypeID);

        /// <summary>
        /// 获得交接班信息
        /// </summary>
        /// <param name="dtFrom"></param>
        /// <param name="dtTo"></param>
        /// <returns></returns>
        [OperationContract]
        List <EntityHoRecord> GetHandoverList(DateTime dtFrom, DateTime dtTo);

        /// <summary>
        /// 获得未交接标本
        /// </summary>
        /// <param name="ctypeID"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityHoRecord> GetDtNullResData(string ctypeID);

        /// <summary>
        /// 删除交接班信息
        /// </summary>
        /// <param name="ho_id"></param>
        /// <returns></returns>
       [OperationContract]
        bool DeleteHandover(string ho_id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateHandoverInfo(EntityHoRecord info);
    }
}
