using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoResultOriginal
    {
        /// <summary>
        /// 获得仪器原始数据
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itr_id"></param>
        /// <param name="result_type"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        List<EntityObrResultOriginal> GetObrResult(DateTime date, string itr_id, int result_type, string filter);



        /// <summary>
        /// 获得仪器原始数据
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itr_id"></param>
        /// <param name="sid"></param>
        /// <returns></returns>
        List<EntityObrResultOriginalEx> GetOrignObrResult(EntityPidReportMain pat);

        /// <summary>
        /// 获得仪器全部数据
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itr_id"></param>
        /// <param name="result_type"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        List<EntityObrResult>GetAllObrResult(DateTime date, string itr_id, int result_type, string filter);
    }
}
