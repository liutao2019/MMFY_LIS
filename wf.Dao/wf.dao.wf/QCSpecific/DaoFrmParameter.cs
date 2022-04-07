using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.dao.wf.QCSpecific
{
    [Export("wf.plugin.wf", typeof(IDaoFrmParameter))]
    public class DaoFrmParameter : IDaoFrmParameter
    {
        public List<EntityDicQcRule> SearchQcRule()
        {
            EntityResponse response = new EntityResponse();
            IDaoDic<EntityDicQcRule> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicQcRule>>();
            try
            {
                Object obj = null;
                List<EntityDicQcRule> list = dao.Search(obj);

                EntityDicQcRule qcRuleNew = new EntityDicQcRule();
                qcRuleNew.RulId = "-1";
                qcRuleNew.RulName = "Westgard";
                qcRuleNew.SortNo = 1000;

                list.Add(qcRuleNew);

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("质控物规则 Other-Search", ex);
                return new List<EntityDicQcRule>();
            }
        }

        public List<EntityDicMachineCode> SearchMitmNo(string itrId)
        {
            try
            {
                string strSql = string.Format(@"select * from Rel_itr_channel_code where Ricc_Ditr_id='{0}'", itrId);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(strSql);

                List<EntityDicMachineCode> list = EntityManager<EntityDicMachineCode>.ConvertToList(dt).OrderBy(i => i.MacId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicMachineCode>();
            }
        }
        
        public bool DeleteParDetailNew(EntityDicQcMateria drQc_Detail)
        {
            try
            {
                DBManager helper = new DBManager();

                if (drQc_Detail != null)
                {
                    string strSql_1 = string.Format(@"delete Dict_qc_materia where Dmat_id='{0}' ", drQc_Detail.MatSn);
                    helper.ExecSql(strSql_1);

                    string strSql_2 = string.Format(@"DELETE Rel_qc_materia_detail WHERE Rmatdet_Dmat_id='{0}' ", drQc_Detail.MatSn);
                    helper.ExecSql(strSql_2);

                    string strSql_3 = string.Format(@"DELETE qc_sample WHERE qcm_mid='{0}' ", drQc_Detail.MatSn);
                    helper.ExecSql(strSql_3);
                }
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return false;
        }


        public bool DeleteParNewRule(EntityDicQcMateriaDetail drQc, EntityDicQcMateria drParDeta)
        {
            try
            {
                DBManager helper = new DBManager();
                if (drQc != null && drParDeta != null)
                {
                    EntityDicQcMateriaDetail eyQMDetail = new EntityDicQcMateriaDetail();
                    eyQMDetail.MatDetId = drQc.MatDetId;
                    //删除质控项目数据
                    new DaoDicQcMateriaDetail().DeleteQcMateriaDetail(eyQMDetail);

                    //删除质控规则关联数据
                    new DaoQcMateriaRule().DeleteQcMateriaRule(drParDeta.MatSn, drQc.MatDetId);

                    //删除质控规则关联数据(参数值不同)
                    new DaoQcMateriaRule().DeleteQcMateriaRule(drQc.MatId, drQc.MatItmId);

                }
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return false;
        }

        public bool ViewQcParRule(List<EntityDicQcMateriaRule> dtSample)
        {
            try
            {
                DBManager helper = new DBManager();
                if (dtSample.Count > 0)
                {
                    new DaoQcMateriaRule().DeleteQcMateriaRule(dtSample[0].MatSn, dtSample[0].MatItmId);
                    //string delRele = string.Format(@"delete Def_qc_materia_rule where mat_sn='{0}' and mat_id='{1}' "
                    //         , dtSample[0].MatSn, dtSample[0].MatId);
                    //helper.ExecSql(delRele);

                    foreach (var info in dtSample)
                    {
                        if (info.RulId != "0")
                        {
                            new DaoQcMateriaRule().SaveQcMateriaRule(info);
                            //string insertSql = string.Format(@"insert into Def_qc_materia_rule(mat_sn,rul_id,mat_id)
                            //                                      values('{0}','{1}','{2}')"
                            //                                     , info.MatSn, info.RulId, info.MatId);
                            //helper.ExecSql(insertSql);
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return false;
        }

    }
}
