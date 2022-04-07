using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;
using dcl.svr.dicbasic;

namespace dcl.svr.result
{
    public class ResulTempBIZ : IResultTemp
    {
        public List<EntityDicCombineDetail> GetCombDetail(List<string> comId)
        {
            List<EntityDicCombineDetail> CombDetails = new List<EntityDicCombineDetail>();
            List<EntityDicCombineDetail> listDetails = null;
            IDaoDicCombineDetail dao = DclDaoFactory.DaoHandler<IDaoDicCombineDetail>();
            if (dao != null)
            {
                foreach (var item in comId)
                {
                    listDetails = new List<EntityDicCombineDetail>();
                    listDetails = dao.Search(item);
                    if (listDetails != null)
                        CombDetails.AddRange(listDetails);
                }
            }
            else
            {
                return null;
            }
            return CombDetails;
        }

        public List<EntityDicCombineDetail> GetCombDetailByComIdAndItrId(List<string> comId)
        {
            List<EntityDicCombineDetail> CombDetails = null;
            IDaoDicCombineDetail dao = DclDaoFactory.DaoHandler<IDaoDicCombineDetail>();
            if (dao != null)
            {
                CombDetails = dao.Search(comId);
            }
            return CombDetails;
        }

        public List<EntityDefItmProperty> GetItemProp(List<string> listStr)
        {
            List<EntityDefItmProperty> props = null;
            IDaoDic<EntityDefItmProperty> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDefItmProperty>>();
            if (dao != null)
            {
                props = dao.Search(listStr);
            }
            return props;
        }

        public List<EntityDicItrCombine> GetItrCombine(string ItrId)
        {
            List<EntityDicItrCombine> itrCombs = null;
            IDaoDic<EntityDicItrCombine> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicItrCombine>>();
            if (dao != null)
            {
                itrCombs = dao.Search(ItrId);
            }
            return itrCombs;
        }

        public List<string> GetPatientsSid(EntityAnanlyseQC query)
        {
            List<string> listSid = null;
            IDaoResultTemp dao = DclDaoFactory.DaoHandler<IDaoResultTemp>();
            if (dao != null)
            {
                listSid = dao.GetPatientsSid(query);
            }
            return listSid;
        }

        public List<EntityDicMicSmear> GetSmearList(EntityRequest request)
        {
            DictNobactBIZ nobactBiz = new DictNobactBIZ();
            return nobactBiz.Search(request).GetResult() as List<EntityDicMicSmear>;
        }
    }
}
