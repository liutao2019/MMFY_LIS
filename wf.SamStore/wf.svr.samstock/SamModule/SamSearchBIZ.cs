using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.samstock
{
    /// <summary>
    /// 归档查询
    /// </summary>
    public class SamSearchBIZ : IDicSamSearch
    {
        public List<EntitySampStoreDetail> GetRackQueryData(EntityDicSamSearchParamter samSearchParam)
        {
            List<EntitySampStoreDetail> listSSDetail = new List<EntitySampStoreDetail>();
            IDaoSamSearch dao = DclDaoFactory.DaoHandler<IDaoSamSearch>();
            if (dao != null)
            {
                if (samSearchParam != null)
                    listSSDetail = dao.GetRackQueryData(samSearchParam);
            }
            return listSSDetail;
        }

        public List<EntitySampStoreDetail> GetRackQueryDataByBarcode(string rackBarcode, string samBarcode)
        {
            List<EntitySampStoreDetail> listSSD = new List<EntitySampStoreDetail>();
            IDaoSamSearch dao = DclDaoFactory.DaoHandler<IDaoSamSearch>();
            if (dao != null)
            {
                listSSD = dao.GetRackQueryDataByBarcode(rackBarcode, samBarcode);
            }
            return listSSD;
        }
    }
}
