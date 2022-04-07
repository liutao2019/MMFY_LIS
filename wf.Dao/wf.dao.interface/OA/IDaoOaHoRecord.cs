using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoOaHoRecord
    {
        /// <summary>
        /// /
        /// </summary>
        /// <param name="ctypeID"></param>
        /// <returns></returns>
        List<EntityHoRecord> GetDtNullResData(string ctypeID);


        /// <summary>
        /// 获取交接班信息
        /// </summary>
        /// <param name="dtFrom"></param>
        /// <param name="dtTo"></param>
        /// <param name="ctypeID"></param>
        /// <returns></returns>
        EntityHoRecord GetHandoverStat(DateTime dtFrom, DateTime dtTo, string ctypeID);

        /// <summary>
        /// 获取交班管理信息
        /// </summary>
        /// <param name="dtFrom"></param>
        /// <param name="dtTo"></param>
        /// <returns></returns>
        List<EntityHoRecord> GetHandoverList(DateTime dtFrom, DateTime dtTo);



        /// <summary>
        /// 保存交接班信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        bool SaveHandoverInfo(EntityHoRecord info);

        /// <summary>
        /// 删除交接班信息
        /// </summary>
        /// <param name="ho_id"></param>
        /// <returns></returns>
        bool DeleteHandover(string ho_id);
    }
}
