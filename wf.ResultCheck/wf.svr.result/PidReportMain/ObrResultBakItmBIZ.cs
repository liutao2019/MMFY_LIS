using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;
using dcl.svr.cache;

namespace dcl.svr.result
{
    public class ObrResultBakItmBIZ : IObrResultBakItm
    {
        public List<EntityObrResultBakItm> GetObrResultBakItm(string RepId, int whereType)
        {
            List<EntityObrResultBakItm> listBakItm = new List<EntityObrResultBakItm>();
            IDaoObrResultBakItm daoBakItm = DclDaoFactory.DaoHandler<IDaoObrResultBakItm>();
            if (daoBakItm != null)
            {
                listBakItm = daoBakItm.GetObrResultBakItm(RepId, whereType);
                if (whereType == 1 && listBakItm.Count > 0)
                {
                    //bak_date排序并根据bak_id去重
                    listBakItm = listBakItm.GroupBy(i => i.BakId).Select(i => i.First()).OrderBy(i => i.BakDate).ToList();
                }
            }
            return listBakItm;
        }

        public String InsertObrResultBakItm(string resId, List<string> resItmIds, List<string> resKeys)
        {
            String errorMsg = string.Empty;
            IDaoObrResultBakItm bakItmDao = DclDaoFactory.DaoHandler<IDaoObrResultBakItm>();
            if (bakItmDao != null)
            {
                string bak_id = Guid.NewGuid().ToString();//备份id
                DateTime bak_date = ServerDateTime.GetDatabaseServerDateTime();//备份时间
                errorMsg = bakItmDao.InsertObrResultBakItm(resId, bak_id, bak_date, resItmIds, resKeys);
            }
            return errorMsg;
        }

        public string InsertObrResultByBak(string resId, List<string> resItmIds, List<string> resKeys)
        {
            string errorMsg = string.Empty;
            EntityResultQC resultQc = new EntityResultQC();
            resultQc.ListObrId.Add(resId);
            resultQc.listItmIds = resItmIds;
            bool result = new ObrResultBIZ().DeleteObrResultByResultQc(resultQc);
            if (result)
            {
                IDaoObrResultBakItm bakItmDao = DclDaoFactory.DaoHandler<IDaoObrResultBakItm>();
                errorMsg = bakItmDao.InsertObrResultByBak(resKeys);
            }
            return errorMsg;
        }
    }
}
