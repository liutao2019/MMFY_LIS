using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.qc
{
    public class QcMateriaRuleBIZ : IDicQcMateriaRule
    {
        public bool DeleteQcMateriaRule(string matSn, string matId)
        {
            bool isDelete = false;
            IDaoQcMateriaRule dao = DclDaoFactory.DaoHandler<IDaoQcMateriaRule>();
            if (dao != null)
            {
                isDelete = dao.DeleteQcMateriaRule(matSn, matId);
            }
            return isDelete;
        }

        public bool SaveQcMateriaRule(EntityDicQcMateriaRule qcMateriaRule)
        {
            bool isSave = false;
            IDaoQcMateriaRule dao = DclDaoFactory.DaoHandler<IDaoQcMateriaRule>();
            if (dao != null)
            {
                isSave = dao.SaveQcMateriaRule(qcMateriaRule);
            }
            return isSave;
        }

        public List<EntityDicQcMateriaRule> SearchQcMateriaRule(string matSn, string matItmId)
        {
            List<EntityDicQcMateriaRule> list = new List<EntityDicQcMateriaRule>();
            IDaoQcMateriaRule dao = DclDaoFactory.DaoHandler<IDaoQcMateriaRule>();
            if (dao != null)
            {
                list = dao.SearchQcMateriaRule(matSn, matItmId);
            }
            return list;
        }

        public List<EntityDicQcRule> GetQcRule(List<String> listMatSnItmId)
        {
            List<EntityDicQcRule> list = new List<EntityDicQcRule>();
            IDaoQcMateriaRule dao = DclDaoFactory.DaoHandler<IDaoQcMateriaRule>();
            if (dao != null)
            {
                list = dao.GetQcRule(listMatSnItmId);
            }
            return list;
        }

    }
}
