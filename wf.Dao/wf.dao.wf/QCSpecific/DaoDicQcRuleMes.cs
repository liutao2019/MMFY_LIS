using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoQcRuleMes))]
    public class DaoDicQcRuleMes : IDaoQcRuleMes
    {
        public bool Delete(EntityDicQcRuleMes ruleMes)
        {
            try
            {
                DBManager helper = new DBManager();
                string sql = "";

                if (ruleMes != null)
                {
                    sql = string.Format(@"delete Dict_qc_rule_mes where Dqrm_Ditr_id = '{0}' and Dqrm_rootNode_id = '{1}' ", ruleMes.QcmItrId, ruleMes.QrmRootNodeId);
                }
                helper.ExecSql(sql);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicQcRuleMes> GetQcRuleMes(string obr_id)
        {
            List<EntityDicQcRuleMes> listQcRuleMes = new List<EntityDicQcRuleMes>();
            try
            {
                string sqlGetRuleMes = string.Format(@"
select Lres_itm_ename as itm_ecode,Dict_qc_rule_mes.* from Dict_qc_rule_mes
inner join Lis_result  with(nolock)
on Lis_result.Lres_Pma_rep_id ='{0}'
and Dict_qc_rule_mes.Dqrm_Ditm_id=Lis_result.Lres_Ditm_id
and Dict_qc_rule_mes.Dqrm_Ditr_id =isnull(Lres_source_Ditr_id,Lres_Ditr_id)
", obr_id);

                DBManager helper = new DBManager();
                DataTable dtQcRuleMes = helper.ExecuteDtSql(sqlGetRuleMes);

                listQcRuleMes = EntityManager<EntityDicQcRuleMes>.ConvertToList(dtQcRuleMes).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return listQcRuleMes;
        }

        public bool Save(EntityDicQcRuleMes sample)
        {
            throw new NotImplementedException();
        }

        public List<EntityDicQcRuleMes> Search(object obj)
        {
            List<EntityDicQcRuleMes> listQcRuleMes = new List<EntityDicQcRuleMes>();

            string days = obj as string;
            if (days != null)
            {
                try
                {
                    string strWhere = "";//暂时没有查询条件，日后待需要时添加（改危急值FrmQCNotify界面用的到）

                    string sqlGetItrQcMsg = string.Format(@"
select Dict_qc_rule_mes.*,Dict_itm.Ditm_ecode,Dict_itr_instrument.Ditr_ename,Dict_itr_instrument.Ditr_lab_id,
Dict_profession.Dpro_name 
from Dict_qc_rule_mes WITH (NOLOCK)
left join Dict_itm on Dict_qc_rule_mes.Dqrm_Ditm_id=Dict_itm.Ditm_id
left join Dict_itr_instrument on Dict_qc_rule_mes.Dqrm_Ditr_id=Dict_itr_instrument.Ditr_id
left join Dict_profession on Dict_itr_instrument.Ditr_lab_id=Dict_profession.Dpro_id
where Dqrm_start_time>(getdate()-{1}) and (Dqrm_type='失控' or Dqrm_type='新增')
{0}
", strWhere, days);

                    DBManager helper = new DBManager();
                    DataTable dtQcRuleMes = helper.ExecuteDtSql(sqlGetItrQcMsg);

                    listQcRuleMes = EntityManager<EntityDicQcRuleMes>.ConvertToList(dtQcRuleMes).ToList();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("获取仪器质控信息", ex);
                }
            }
            return listQcRuleMes;
        }

        public bool Update(EntityDicQcRuleMes sample)
        {
            throw new NotImplementedException();
        }

        public List<EntityDicQcRuleMes> GetQcRuleMesByItrId(string itrId)
        {
            List<EntityDicQcRuleMes> listQcRuleMes = new List<EntityDicQcRuleMes>();
            try
            {
                string sql = string.Format(@"select Dqrm_start_time,Dqrm_end_time,Dqrm_type,Dict_instrmt_maintain.Dim_content from Dict_qc_rule_mes 
inner join  Dict_instrmt_maintain on Dict_qc_rule_mes.Dqrm_node_id=Dict_instrmt_maintain.Dim_id
where Dqrm_Ditr_id = '{0}' ", itrId);

                DBManager helper = new DBManager();
                DataTable dtQcRuleMes = helper.ExecuteDtSql(sql);

                listQcRuleMes = EntityManager<EntityDicQcRuleMes>.ConvertToList(dtQcRuleMes).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return listQcRuleMes;
        }
    }
}
