using dcl.servececontract;
using System.Data;
using dcl.pub.entities.Lab;
using dcl.svr.tools;

namespace dcl.svr.wcf
{
    public class ResultMergeNewService : WCFServiceBase, IResultMergeNew
    {
        #region IResultMergeNew 成员

        public DataTable GetResult(QueryMergeResult query)
        {
            return new ResultMergeNewBiz().GetResult(query);
        }

        public DataSet MergeData(QueryMergeResult source, QueryMergeResult target)
        {
            return new ResultMergeNewBiz().MergeData(source, target);
        }

        public string UpdateMergeData(DataTable mergeDt)
        {
            return new ResultMergeNewBiz().UpdateMergeData(mergeDt);
        }

        #endregion
    }

  
}
