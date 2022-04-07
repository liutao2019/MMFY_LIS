
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using dcl.common;
using System.ComponentModel.Composition;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoSysOperationLog))]
    public class DaoDicOperationLog : DclDaoBase, IDaoSysOperationLog
    {
        public bool SaveSysOperationLog(EntitySysOperationLog oper)
        {
            try
            {

                DBManager helper = new DBManager();
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Boper_Buser_id", oper.OperatUserId);
                values.Add("Boper_servername", oper.OperatServername);
                values.Add("Boper_date", oper.OperatDate.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Boper_key", oper.OperatKey);
                values.Add("Boper_module", oper.OperatModule);
                values.Add("Boper_group", oper.OperatGroup);
                values.Add("Boper_action", oper.OperatAction);
                values.Add("Boper_object", oper.OperatObject);
                values.Add("Boper_content", oper.OperatContent);
                helper.InsertOperation("Base_operation_log", values);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool UpdateSysOperationLog(EntitySysOperationLog oper)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Boper_sn", oper.OperatLogSn);
                values.Add("Boper_Buser_id", oper.OperatUserId);
                values.Add("Boper_servername", oper.OperatServername);
                values.Add("Boper_date", oper.OperatDate.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Boper_key", oper.OperatKey);
                values.Add("Boper_module", oper.OperatModule);
                values.Add("Boper_group", oper.OperatGroup);
                values.Add("Boper_action", oper.OperatAction);
                values.Add("Boper_object", oper.OperatObject);
                values.Add("Boper_content", oper.OperatContent); ;

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Boper_sn", oper.OperatLogSn);

                helper.UpdateOperation("Base_operation_log", values, keys);
              
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool DeleteSysOperationLog(EntitySysOperationLog oper)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Boper_sn", oper.OperatLogSn);

                helper.DeleteOperation("Base_operation_log", keys);


                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntitySysOperationLog> Search(Object obj)
        {

            try
            {
                String sql = @"select *  from Base_operation_log";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);
                List<EntitySysOperationLog> list = EntityManager<EntitySysOperationLog>.ConvertToList(dt).OrderBy(i => i.OperatLogSn).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySysOperationLog>();
            }
        }

        public List<EntitySysOperationLog> SearchSysOperationLog(EntityLogQc qc)
        {

            try
            {
                string where = string.Empty;
                if (qc != null)
                {
                    if (!string.IsNullOrEmpty(qc.OperationUserId))
                    {
                        where +=string.Format("and Boper_Buser_id='{0}'", qc.OperationUserId);
                    }
                    if (!string.IsNullOrEmpty(qc.OperatModule))
                    {
                        where += string.Format("and Boper_module='{0}'", qc.OperatModule);
                    }
                    else {
                        where += "and Boper_module !='病人资料' ";
                    }
                    if (!string.IsNullOrEmpty(qc.DateStart))
                    {
                        where +=string.Format("and Boper_date>='{0}'", qc.DateStart);
                    }
                    if (!string.IsNullOrEmpty(qc.DateEnd))
                    {
                        where += string.Format("and Boper_date<'{0}'", qc.DateEnd);
                    }
                    if (!string.IsNullOrEmpty(qc.Operatkey))
                    {
                        where += string.Format("and Boper_key='{0}'", qc.Operatkey);
                    }
                }
                string sql =string.Format(@"select Base_operation_log.*,Base_user.Buser_name 
from Base_operation_log 
left join Base_user on Base_user.Buser_loginid=Base_operation_log.Boper_Buser_id where 1=1 {0}", where);
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(sql);
                List<EntitySysOperationLog> list = EntityManager<EntitySysOperationLog>.ConvertToList(dt).OrderBy(i => i.OperatLogSn).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySysOperationLog>();
            }
        }


        public List<EntitySysOperationLog> SearchSysQcOperationLog(EntityLogQc qc)
        {

            try
            {
                string where = string.Empty;
                if (qc != null)
                {
                    if (!string.IsNullOrEmpty(qc.OperationUserId))
                    {
                        where += string.Format("and Boper_Buser_id='{0}'", qc.OperationUserId);
                    }
                    if (!string.IsNullOrEmpty(qc.OperatModule))
                    {
                        where += string.Format("and Boper_module='{0}'", qc.OperatModule);
                    }
                    if (!string.IsNullOrEmpty(qc.OperatGroup))
                    {
                        where += string.Format("and Boper_group='{0}'", qc.OperatGroup);
                    }
                    if (!string.IsNullOrEmpty(qc.DateStart))
                    {
                        where += string.Format("and Boper_date>='{0}'", qc.DateStart);
                    }
                    if (!string.IsNullOrEmpty(qc.DateEnd))
                    {
                        where += string.Format("and Boper_date<'{0}'", qc.DateEnd);
                    }
                    if (!string.IsNullOrEmpty(qc.Operatkey))
                    {
                        where += string.Format("and Boper_key='{0}'", qc.Operatkey);
                    }
                    if (!string.IsNullOrEmpty(qc.OperatObject))
                    {
                        where += string.Format("and Boper_object like  '{0}%'", qc.OperatObject);
                    }
                }
                string sql = string.Format(@"select Base_operation_log.*,
                    Base_user.Buser_name, 
                    Dmat_level+'-'+Dict_qc_materia.Dmat_batch_no AS mat_level,
                    Dict_itr_instrument.Ditr_name,Lis_qc_result.Lres_date 
                    from Base_operation_log 
                    LEFT JOIN Lis_qc_result ON  convert(nvarchar(50),Lis_qc_result.Lres_id) = Base_operation_log.Boper_key
                    LEFT JOIN Dict_qc_materia Dict_qc_materia ON  Dict_qc_materia.Dmat_id=Lis_qc_result.Lres_Rmatdet_id
                    LEFT JOIN Dict_itr_instrument  ON Dict_itr_instrument.Ditr_id=Lis_qc_result.Lres_Ditr_id
                    left join Base_user on Base_user.Buser_loginid=Base_operation_log.Boper_Buser_id
                     where 1=1 {0}", where);
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(sql);
                List<EntitySysOperationLog> list = EntityManager<EntitySysOperationLog>.ConvertToList(dt).OrderBy(i => i.OperatLogSn).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySysOperationLog>();
            }
        }

        public List<EntityPidReportMain> GetPatients(string timeFrom, string timeTo, string patNo, string itrType, string patInNo, string patName, string patItrId, string patCheck)
        {
            try
            {
                string sql = @"select Pma_rep_id,cast(Pma_rep_id as bigint) Pma_sid,Ditr_ename rep_itr_id,Pma_pat_name,Pma_in_date,
(case Pma_pat_sex when 1 then '男'  when 2 then '女' else '未知' end) Pma_pat_sex,
Pma_pat_age,Pma_com_name from Pat_lis_main 
left join Dict_itr_instrument on Pat_lis_main.Pma_Ditr_id=Dict_itr_instrument.Ditr_id 
where 1=1 and Pma_modify_frequency>0  ";
                sql += " and Pma_in_date>='" + timeFrom + " 00:00:00' and Pma_in_date<='" + timeTo + " 23:59:59'";
                List<EntityDicPubIdent> listIdent = new DaoDicPubIdent().Search(null).FindAll(w => w.IdtId == patNo);
                EntityDicPubIdent ident = new EntityDicPubIdent();
                if (listIdent != null && listIdent.Count > 0)
                {
                    ident = listIdent[0];
                }
                if (!string.IsNullOrEmpty(patNo))
                {
                    if (!string.IsNullOrEmpty(ident.IdtName) && ident.IdtName != "条码号")
                    {
                        sql += " and Pma_pat_Didt_id ='" + patNo + "'";
                    }
                }
                if (!string.IsNullOrEmpty(itrType))
                {
                    sql += " and Dict_itr_instrument.Ditr_lab_id ='" + itrType + "'";
                }

                if (!string.IsNullOrEmpty(patInNo))
                {
                    if (!string.IsNullOrEmpty(ident.IdtName) && ident.IdtName == "条码号")
                    {
                        sql += " and Pma_bar_code like '%" + patInNo + "%'";
                    }
                    else {
                        sql += " and Pma_pat_in_no like '%" + patInNo + "%'";
                    }
                   
                }

                if (!string.IsNullOrEmpty(patName))
                {
                    sql += " and Pma_pat_name like '%" + patName + "%'";
                }

                if (!string.IsNullOrEmpty(patItrId))
                {
                    sql += " and Pma_Ditr_id = '" + patItrId + "'";
                }

                if (!string.IsNullOrEmpty(patCheck))
                {
                    sql += " and Pma_audit_Buser_id like '%" + patCheck + "%'";
                }
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(sql);
                List<EntityPidReportMain> list = EntityManager<EntityPidReportMain>.ConvertToList(dt).OrderBy(i => i.PatSort).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityPidReportMain>();
            }

        }

        public string GetPatId(string timeFrom, string timeTo, string patId, string userName)
        {
            try
            {
                string sql = string.Format(@"select Pma_rep_id from Pat_lis_main where
                                                       Pma_in_date>='{0} 00:00:00' and Pma_in_date<='{1} 23:59:59'", timeFrom, timeTo);
                if (!string.IsNullOrEmpty(patId))
                {
                    sql += " and Pma_pat_in_no like '%" + patId + "%'";
                }

                if (!string.IsNullOrEmpty(userName))
                {
                    sql += " and Pma_pat_name like '%" + userName + " % '";
                }
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(sql);
                List<EntityPidReportMain> list = EntityManager<EntityPidReportMain>.ConvertToList(dt).OrderBy(i => i.PatSort).ToList();
                return list[0].RepId;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return null;
            }

        }

    }
}
