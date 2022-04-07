using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.servececontract;
using dcl.svr.result;
using lis.dto.Entity;
using dcl.pub.entities;
using dcl.entity;

namespace dcl.svr.wcf
{
    public class ResultMergeService : IResultMerge
    {
        #region IResultMerge 成员

        public System.Data.DataTable GetCurrentItrPatientList(string itr_id, DateTime pat_date)
        {
            ResultMergeBiz bll = new ResultMergeBiz();
            return bll.GetCurrentItrPatientList(itr_id, pat_date);
        }

        public System.Data.DataTable GetNonePatInfoResult(string itr_id, DateTime pat_date, bool onlyGetNonePatInfoResult)
        {
            ResultMergeBiz bll = new ResultMergeBiz();
            return bll.GetNonePatInfoResult(itr_id, pat_date, onlyGetNonePatInfoResult);
        }


        public List<EntityOperationResult> Merge(System.Data.DataTable dtData)
        {
            ResultMergeBiz bll = new ResultMergeBiz();
            return bll.Merge(dtData);
        }

        #endregion
    }
}
