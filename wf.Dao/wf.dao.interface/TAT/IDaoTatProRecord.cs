using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoTatProRecord
    {
        /// <summary>
        /// 根据条码号查询记录数量
        /// </summary>
        /// <param name="barCord"></param>
        /// <returns></returns>
        int CountRecordByBarCode(string barCord);

        /// <summary>
        /// 插入tat_pro_recode记录
        /// </summary>
        /// <param name="col"></param>
        /// <param name="stepCode"></param>
        /// <param name="barCode"></param>
        /// <param name="dtToday"></param>
        /// <returns></returns>
        Boolean InsertTATProRecord(string stepCode, string barCode, string dtToday);

        /// <summary>
        /// 更新tat_pro_recode记录
        /// </summary>
        /// <param name="col"></param>
        /// <param name="stepCode"></param>
        /// <param name="barCode"></param>
        /// <param name="dtToday"></param>
        /// <returns></returns>
        Boolean UpdateTATProRecord( string stepCode, string barCode, string dtToday);

        /// <summary>
        /// 根据条码号删除记录
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        Boolean DeleteRecordByBarCode(string barCode);

        /// <summary>
        /// 获取该条码的tat记录
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        EntityTatProRecord GetTatRecord(string barCode);

    }
}
