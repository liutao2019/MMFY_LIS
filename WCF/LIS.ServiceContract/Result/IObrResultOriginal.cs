using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 仪器原始数据操作接口
    /// </summary>
    [ServiceContract]
    public interface IObrResultOriginal
    {

       /// <summary>
       /// 获得仪器原始数据
       /// </summary>
       /// <param name="date"></param>
       /// <param name="itr_id"></param>
       /// <param name="result_type"></param>
       /// <param name="filter"></param>
       /// <returns></returns>
        [OperationContract]
        List<EntityObrResultOriginal> GetObrResult(DateTime date, string itr_id, int result_type, string filter);
        /// <summary>
        /// 获得仪器全部数据
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itr_id"></param>
        /// <param name="result_type"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityObrResult> GetAllObrResult(DateTime date, string itr_id, int result_type, string filter);

    }
}
