using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;

namespace dcl.svr.qc
{
    /// <summary>
    /// 快速录入通道
    /// </summary>
    public class FastAddQcItemBIZ : dcl.servececontract.IDicFastAddQcItem
    {

        public EntityResponse SearchItemQcParQcRule(List<EntityDicQcMateriaDetail> listMD, string insId)
        {
            EntityResponse result = new EntityResponse();

            IDaoFastAddQcItem dao = DclDaoFactory.DaoHandler<IDaoFastAddQcItem>();
            if(dao!=null)
            {
                result = dao.SearchItemQcParQcRule(listMD, insId);
            }
            return result;
        }

        public bool SaveQcMateriaDetailOrRule(List<EntityDicQcMateriaDetail> listMD, List<EntityDicQcMateriaRule> listMR)
        {
            bool isSaveTrue = false;
            IDaoFastAddQcItem dao = DclDaoFactory.DaoHandler<IDaoFastAddQcItem>();
            if (dao != null)
            {
                isSaveTrue = dao.SaveQcMateriaDetailOrRule(listMD, listMR);
            }
            return isSaveTrue;
        }

    }
}
