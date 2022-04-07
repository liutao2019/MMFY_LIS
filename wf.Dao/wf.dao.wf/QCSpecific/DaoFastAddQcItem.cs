using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.ComponentModel.Composition;
using dcl.dao.core;
using System.Data;
using dcl.dao.wf;

namespace dcl.dao.clab
{
    /// <summary>
    /// 快速查询通道
    /// </summary>
    [Export("wf.plugin.wf", typeof(IDaoFastAddQcItem))]
    public class DaoFastAddQcItem : IDaoFastAddQcItem
    {
        public EntityResponse SearchItemQcParQcRule(List<EntityDicQcMateriaDetail> listMD, string insId)
        {
            EntityResponse result = new EntityResponse();
            try
            {
                DBManager helper = new DBManager();

                List<Object> listObj = new List<Object>();

                string where = " where Ricc_Ditr_id='" + insId + "'";

                if (listMD.Count > 0)
                {
                    where += " and Ricc_Ditm_id not in ('";

                    int i = 0;
                    foreach (var info in listMD)
                    {
                        if (i != (listMD.Count - 1))
                        {
                            where += info.MatItmId + "','";
                        }
                        else
                        {
                            where += info.MatItmId + "')";
                        }
                        i++;
                    }
                }

                if (where.Length > 0)
                {
                    where += " and Rel_itr_channel_code.del_flag='0'";//明确是哪个表的，不然会报错
                }

                string sqlItem = @"select  0 itm_select,Ditm_id,Ditm_name,Ditm_ecode,py_code,wb_code 
from Rel_itr_channel_code 
left join Dict_itm on Dict_itm.Ditm_id=Rel_itr_channel_code.Ricc_Ditm_id 
and Dict_itm.del_flag='0'" + where + " group by Ditm_id,Ditm_name,Ditm_ecode,py_code,wb_code,Ricc_Ditr_id ";

                string sqlMD = string.Format(@"select Rmatdet_Ditr_id,Rmatdet_Ditm_id,Rmatdet_Dmat_id,'' mat_itm_x,'' mat_itm_sd,
'' mat_itm_cv,'' mat_itm_ccv,'' qcr_itm_name,
'' py_code,'' wb_code,'' itm_name 
from Rel_qc_materia_detail where Rmatdet_id is null ");

                string strRule = "select Drule_id,Drule_name from Dict_qc_rule where del_flag='0' ";

                DataTable dtItem = helper.ExecuteDtSql(sqlItem);
                List<EntityDicQcItem> listItem = EntityManager<EntityDicQcItem>.ConvertToList(dtItem).OrderBy(i => i.ItmId).ToList();

                DataTable dtMd = helper.ExecuteDtSql(sqlMD);
                List<EntityDicQcMateriaDetail> listMd = EntityManager<EntityDicQcMateriaDetail>.ConvertToList(dtMd).OrderBy(i => i.MatItrId).ToList();

                DataTable dtRule = helper.ExecuteDtSql(strRule);
                List<EntityDicQcRule> listRule = EntityManager<EntityDicQcRule>.ConvertToList(dtRule).OrderBy(i => i.RulId).ToList();

                listObj.Add(listItem);
                listObj.Add(listMd);
                listObj.Add(listRule);

                result.SetResult(listObj);

                return result;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                result.Scusess = false;
                return result;
            }
        }

        public bool SaveQcMateriaDetailOrRule(List<EntityDicQcMateriaDetail> listMD, List<EntityDicQcMateriaRule> listMR)
        {
            bool isSaveTrue = false;
            try
            {
                DBManager helper = new DBManager();
                if (listMD.Count > 0)
                {
                    List<EntityDicQcMateriaRule> saveRule = new List<EntityDicQcMateriaRule>(); //质控规则

                    foreach (var infoMd in listMD)
                    {
                        foreach (var infoMR in listMR)
                        {
                            EntityDicQcMateriaRule materiaRule = new EntityDicQcMateriaRule();
                            materiaRule.MatSn = infoMd.MatId;
                            materiaRule.RulId = infoMR.RulId;
                            materiaRule.MatItmId = infoMd.MatItmId;
                            saveRule.Add(materiaRule);
                        }
                    }

                    foreach (var infoMd in listMD)
                    {
                        EntityResponse result = new EntityResponse();
                        result = new DaoDicQcMateriaDetail().SaveQcMateriaDetail(infoMd);
                        isSaveTrue = result.Scusess;
                    }

                    foreach (var infoRule in saveRule)
                    {
                        string sqlRule = string.Format(@" insert into Rel_qc_materia_rule([Rmr_Dmat_id],[Rmr_Drule_id],[Rmr_id]) 
                                                               VALUES('{0}','{1}','{2}')",
                                                              infoRule.MatSn, infoRule.RulId, infoRule.MatItmId);
                        helper.ExecSql(sqlRule);
                    }

                    return isSaveTrue;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                isSaveTrue = false;
            }
            return isSaveTrue;
        }

    }
}
