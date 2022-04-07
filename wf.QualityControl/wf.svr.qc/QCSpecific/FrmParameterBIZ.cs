using dcl.common;
using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;

namespace dcl.svr.qc
{
    /// <summary>
    /// 项目参数
    /// </summary>
    public class FrmParameterBIZ : dcl.servececontract.IDicItemParameter
    {
        public bool DeleteParDetailNew(EntityDicQcMateria drQc_Detail)
        {
            bool isTrue = false;
            IDaoFrmParameter dao = DclDaoFactory.DaoHandler<IDaoFrmParameter>();
            if (dao != null)
            {
                isTrue = dao.DeleteParDetailNew(drQc_Detail);
            }
            return isTrue;
        }

        public bool DeleteParNewRule(EntityDicQcMateriaDetail drQc, EntityDicQcMateria drParDeta)
        {
            bool isTrue = false;
            IDaoFrmParameter dao = DclDaoFactory.DaoHandler<IDaoFrmParameter>();
            if (dao != null)
            {
                isTrue = dao.DeleteParNewRule(drQc, drParDeta);
            }
            return isTrue;
        }

        public List<EntityDicQcRule> SearchQcRule()
        {
            List<EntityDicQcRule> list = new List<EntityDicQcRule>();
            IDaoFrmParameter dao = DclDaoFactory.DaoHandler<IDaoFrmParameter>();
            if (dao != null)
            {
                list = dao.SearchQcRule();
            }
            return list;
        }

        public List<EntityDicMachineCode> SearchMitmNo(string itrId)
        {
            List<EntityDicMachineCode> list = new List<EntityDicMachineCode>();
            IDaoFrmParameter dao = DclDaoFactory.DaoHandler<IDaoFrmParameter>();
            if (dao != null)
            {
                list = dao.SearchMitmNo(itrId);
            }
            return list;
        }

        /// <summary>
        /// 先删除后插入质控规则关联数据
        /// </summary>
        /// <param name="dtSample"></param>
        public bool ViewQcParRule(List<EntityDicQcMateriaRule> dtSample)
        {
            bool isTrue = false;
            IDaoFrmParameter dao = DclDaoFactory.DaoHandler<IDaoFrmParameter>();
            if (dao != null)
            {
                isTrue = dao.ViewQcParRule(dtSample);
            }
            return isTrue;
        }
    }
}
