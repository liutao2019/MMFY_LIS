using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.core;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace dcl.dao.wf
{
    /// <summary>
    /// 病人信息扩展表
    /// </summary>
    [Export("wf.plugin.wf", typeof(IDaoPidReportMainExt))]
    public class DaoPidReportMainExt : IDaoPidReportMainExt
    {
        public bool DeletePatientExt(string repId)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(repId))
            {
                string sql = string.Format("delete from Pid_report_main_ext where rep_id='{0}'", repId);
                DBManager helper = new DBManager();
                try
                {
                    helper.ExecCommand(sql);
                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return result;
        }

        public List<EntityDicPidReportMainExt> GetPatientExtDataByPatID(string pat_id)
        {
            List<EntityDicPidReportMainExt> listPidExt = new List<EntityDicPidReportMainExt>();
            try
            {
                if (string.IsNullOrEmpty(pat_id))
                    pat_id = "";

                string sqlSelect = string.Format("select * from Pid_report_main_ext where rep_id='{0}'", pat_id);

                DBManager helper = new DBManager();
                DataTable dtPatExt = helper.ExecuteDtSql(sqlSelect);

                listPidExt = EntityManager<EntityDicPidReportMainExt>.ConvertToList(dtPatExt).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return listPidExt;
        }

        public bool InsertPatientExtInfoCMD(EntityDicPidReportMainExt ext)
        {
            bool isInsert = false;
            try
            {
                DBManager helper = new DBManager();
                Dictionary<string, object> values = new Dictionary<string, object>();
                values = helper.ConverToDBSaveParameter(ext);
                helper.InsertOperation("Pid_report_main_ext", values);
                isInsert = true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return isInsert;
        }

        public bool SavePidReportMainExt(EntityAuditInfo objAuditInfo, string pat_id)
        {
            try
            {
                DBManager helper = new DBManager();

                string sqlInsertPatExt = string.Format(@"INSERT INTO Pid_report_main_ext
                                                  (msg_content,msg_checker_id,msg_checker_name,rep_id)
                                        VALUES ('{0}','{1}','{2}','{3}')"
                                  , objAuditInfo.MsgContent, objAuditInfo.UserId
                                  , objAuditInfo.UserName, pat_id);
                helper.ExecSql(sqlInsertPatExt);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool SearchPatExtExistByID(string pat_id)
        {
            try
            {
                DBManager helper = new DBManager();

                string strSQL = string.Format("select top 1 1 from Pid_report_main_ext where rep_id=@rep_id ");
                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@rep_id", pat_id));

                Object obj = helper.SelOne(strSQL, paramHt);
                if (obj != null && Convert.ToInt32(obj) > 0)
                    return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return false;
        }

        public bool UpdatePatientExtInfoCMD(EntityDicPidReportMainExt ext)
        {
            bool isUpdate = false;
            try
            {
                DBManager helper = new DBManager();
                Dictionary<string, object> values = new Dictionary<string, object>();
                values = helper.ConverToDBSaveParameter(ext);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("rep_id", ext.RepId);
                helper.UpdateOperation("Pid_report_main_ext", values, keys);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return isUpdate;
        }

        public bool InsertPatientExt(EntityDicPidReportMainExt patientExt)
        {
            bool isUpdate = false;
            try
            {
                DBManager helper = new DBManager();

                string strUpdateSql = string.Format("insert into Pid_report_main_ext (rep_id,msg_content,msg_doc_num,msg_doc_name,msg_dep_tel,msg_date,msg_register_loginId,msg_register_userName) values ('{0}','{1}','{2}','{3}','{4}',getdate(),'{5}','{6}')",
                                        patientExt.RepId,
                                        patientExt.MsgContent,
                                        patientExt.MsgDocNum,
                                        patientExt.MsgDocName,
                                        patientExt.MsgDepTel,
                                        patientExt.MsgRegisterLoginId,
                                        patientExt.MsgRegisterUserName);
                helper.ExecSql(strUpdateSql);
                isUpdate = true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return isUpdate;
        }

        public bool UpdatePatientExt(EntityDicPidReportMainExt patientExt)
        {
            bool isUpdate = false;
            try
            {
                DBManager helper = new DBManager();
                string strUpdateSql = string.Format("update Pid_report_main_ext set msg_content='{1}',msg_doc_num='{2}',msg_doc_name='{3}',msg_dep_tel='{4}',msg_register_loginId='{5}',msg_register_userName='{6}',msg_date=getdate() where rep_id='{0}'",
                                                                        patientExt.RepId,
                                                                        patientExt.MsgContent,
                                                                        patientExt.MsgDocNum,
                                                                        patientExt.MsgDocName,
                                                                        patientExt.MsgDepTel,
                                                                        patientExt.MsgRegisterLoginId,
                                                                        patientExt.MsgRegisterUserName);
                int k = helper.ExecCommand(strUpdateSql);
                if (k > 0)
                {
                    isUpdate = true;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return isUpdate;
        }

        public bool UpdatePidReportMainExt(EntityAuditInfo objAuditInfo, string pat_id)
        {
            try
            {
                DBManager helper = new DBManager();

                string sqlInsertPatExt = @"UPDATE Pid_report_main_ext
                                    SET  msg_content = @msg_content
                                         ,msg_checker_id = @msg_checker_id
                                         ,msg_checker_name = @msg_checker_name
                                   WHERE rep_id=@rep_id";

                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@msg_content", objAuditInfo.MsgContent));
                paramHt.Add(new SqlParameter("@msg_checker_id", objAuditInfo.UserId));
                paramHt.Add(new SqlParameter("@msg_checker_name", objAuditInfo.UserName));
                paramHt.Add(new SqlParameter("@rep_id", pat_id));

                return helper.ExecCommand(sqlInsertPatExt, paramHt) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }
        public bool InsertReportCASignature(DataRow dr)
        {
            bool result = false;
            try
            {
                DBManager helper = new DBManager();
                string sql = string.Format(@"if exists(select rep_id from Pid_report_main_ext where rep_id='{0}')
begin
update Pid_report_main_ext set SourceContent = '{1}',SignContent = '{2}',SignerImage = null,SignType = '{3}' where rep_id = '{0}'
end
else
begin
insert into Pid_report_main_ext ([rep_id],[SourceContent],[SignContent],[SignerImage],[SignType])  VALUES('{0}', '{1}','{2}', null, '1')
end", dr["RepId"], dr["SourceContent"], dr["SignContent"], dr["SignType"]);
                result = true;
                helper.ExecCommand(sql);
            }
            catch (Exception ex)
            { 
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }
    }
}
