using System;
using System.Collections.Generic;
using dcl.servececontract;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;

namespace dcl.svr.result
{
    public class RealTimeResultViewBIZ : IObrResultOriginal
    {

        #region ICommonBIZ 成员

        public List<EntityObrResultOriginal> GetObrResult(DateTime date, string itr_id, int result_type, string filter)
        {
            IDaoResultOriginal dao = DclDaoFactory.DaoHandler<IDaoResultOriginal>();
            List<EntityObrResultOriginal> list = dao.GetObrResult(date, itr_id, result_type, filter);
            return list;
        }

        public List<EntityObrResult> GetAllObrResult(DateTime date, string itr_id, int result_type, string filter)
        {
            IDaoResultOriginal dao = DclDaoFactory.DaoHandler<IDaoResultOriginal>();
            List<EntityObrResult> list = dao.GetAllObrResult(date, itr_id, result_type, filter);
            return list;
        }

        #endregion
    }
}
