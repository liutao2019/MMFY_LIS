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
    public class QcMateriaBIZ : IDicQcMateria
    {
        public bool DeleteQcMateria(EntityDicQcMateria qcMateria)
        {
            bool isDelete = false;
            IDaoQcMateria dao = DclDaoFactory.DaoHandler<IDaoQcMateria>();
            if (dao != null)
            {
                isDelete = dao.DeleteQcMateria(qcMateria);
            }
            return isDelete;
        }

        public EntityResponse SaveQcMateria(EntityDicQcMateria qcMateria)
        {
            EntityResponse result = new EntityResponse();
            IDaoQcMateria dao = DclDaoFactory.DaoHandler<IDaoQcMateria>();
            if (dao != null)
            {
                result = dao.SaveQcMateria(qcMateria);
            }
            return result;
        }

        public List<EntityDicQcMateria> SearchMatSnInQcMateria(EntityDicQcMateria qcMateria)
        {
            List<EntityDicQcMateria> list = new List<EntityDicQcMateria>();
            IDaoQcMateria dao = DclDaoFactory.DaoHandler<IDaoQcMateria>();
            if (dao != null)
            {
                list = dao.SearchMatSnInQcMateria(qcMateria);
            }
            return list;
        }

        public List<EntityDicQcMateria> SearchQcMateriaByMatId(string matId, string startDate)
        {
            List<EntityDicQcMateria> list = new List<EntityDicQcMateria>();
            IDaoQcMateria dao = DclDaoFactory.DaoHandler<IDaoQcMateria>();
            if (dao != null)
            {
                list = dao.SearchQcMateriaByMatId(matId, startDate);
            }
            return list;
        }

        public List<EntityDicQcMateria> SearchQcMateriaLeftRuleTimeAndInterface(string matId, string matLevel)
        {
            List<EntityDicQcMateria> list = new List<EntityDicQcMateria>();
            IDaoQcMateria dao = DclDaoFactory.DaoHandler<IDaoQcMateria>();
            if (dao != null)
            {
                list = dao.SearchQcMateriaLeftRuleTimeAndInterface(matId, matLevel);
            }
            return list;
        }

        public bool UpdateQcMateria(EntityDicQcMateria qcMateria)
        {
            bool isUpdate = false;
            IDaoQcMateria dao = DclDaoFactory.DaoHandler<IDaoQcMateria>();
            if (dao != null)
            {
                isUpdate = dao.UpdateQcMateria(qcMateria);
            }
            return isUpdate;
        }

        public List<EntityDicQcMateria> GetMatSn(string QcItrId, string QcNoType, string QcItmId)
        {
            List<EntityDicQcMateria> list = new List<EntityDicQcMateria>();
            IDaoQcMateria dao = DclDaoFactory.DaoHandler<IDaoQcMateria>();
            if (dao != null)
            {
                list = dao.GetMatSn(QcItrId, QcNoType, QcItmId);
            }
            return list;
        }


        public List<EntityDicQcMateria> GetMatSnByItem(string QcItrId,DateTime dtStartTime, DateTime dtEndTime, string QcItmId)
        {
            List<EntityDicQcMateria> list = new List<EntityDicQcMateria>();
            IDaoQcMateria dao = DclDaoFactory.DaoHandler<IDaoQcMateria>();
            if (dao != null)
            {
                list = dao.GetMatSn(QcItrId,dtStartTime, dtEndTime, QcItmId);
            }
            return list;
        }

        public List<EntityDicQcMateria> SearchQcMateriaAll()
        {
            List<EntityDicQcMateria> listQcMatAll = new List<EntityDicQcMateria>();
            IDaoQcMateria dao = DclDaoFactory.DaoHandler<IDaoQcMateria>();
            if (dao != null)
            {
                listQcMatAll = dao.SearchQcMateriaAll();
            }
            return listQcMatAll;
        }

        public bool CopyMethodQcMateria(EntityDicQcMateria eyQcMateria)
        {
            bool isCopySuccess = false;

            if (eyQcMateria != null)
            {
                try
                {
                    string mat_sn = eyQcMateria.MatSn;//记录质控物主键的值(eyQcMateria主键的值在执行保存之后会改变)
                    string mat_id = eyQcMateria.MatId;//记录仪器ID

                    SaveQcMateria(eyQcMateria);//保存质控物明细数据

                    List<EntityDicQcMateriaDetail> listPar = new List<EntityDicQcMateriaDetail>();
                    listPar = new QcMateriaDetailBIZ().SearchQcMateriaDetail(mat_sn);//查询质控项目数据

                    if (listPar.Count > 0)
                    {
                        foreach (var infoPar in listPar)
                        {
                            infoPar.MatId = eyQcMateria.MatSn;// mat_sn;
                            infoPar.MatItrId = mat_id;
                            new QcMateriaDetailBIZ().SaveQcMateriaDetail(infoPar);//保存质控项目数据
                        }
                    }
                    List<EntityDicQcMateriaRule> listQcMateriaRule = new List<EntityDicQcMateriaRule>();
                    listQcMateriaRule = new QcMateriaRuleBIZ().SearchQcMateriaRule(mat_sn, null);//查询质控规则关联数据

                    if (listQcMateriaRule.Count > 0)
                    {
                        foreach (var infoSam in listQcMateriaRule)
                        {
                            infoSam.MatSn = eyQcMateria.MatSn;// mat_sn;
                            new QcMateriaRuleBIZ().SaveQcMateriaRule(infoSam);//保存质控规则关联数据
                        }
                    }
                    isCopySuccess = true;
                }
                catch(Exception ex)
                {
                    throw;
                }
            }

            return isCopySuccess;
        }
    }
}
