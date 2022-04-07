using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.core;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoObrMessageContent))]
    public class DaoObrMessageContent : IDaoObrMessageContent
    {
        public bool SaveObrMessageContent(EntityDicObrMessageContent msgContent)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Lmsg_id", msgContent.ObrId);//客户端生成唯一主键
                values.Add("Lmsg_type", msgContent.ObrType);
                values.Add("Lmsg_send_user_id", msgContent.ObrSendUserId);
                values.Add("Lmsg_send_user_name", msgContent.ObrSendUserName);
                values.Add("Lmsg_send_server", msgContent.ObrSendServer);

                values.Add("Lmsg_create_time", msgContent.ObrCreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Lmsg_title", msgContent.ObrMessageTitle);
                values.Add("Lmsg_receive", msgContent.ObrReceive);
                values.Add("Lmsg_content", msgContent.ObrContent);
                values.Add("Lmsg_parent_Lmsg_id", msgContent.ObrParentId);

                values.Add("del_flag", msgContent.DelFlag);
                values.Add("Lmsg_expiration_date", msgContent.ObrExpirationDate);
                values.Add("Lmsg_extend_a", msgContent.ObrValueA);
                values.Add("Lmsg_extend_b", msgContent.ObrValueB);
                values.Add("Lmsg_extend_c", msgContent.ObrValueC);

                values.Add("Lmsg_extend_d", msgContent.ObrValueD);
                values.Add("Lmsg_audit_user_id", msgContent.ObrAuditUserId);
                values.Add("Lmsg_audit_user_name", msgContent.ObrAuditUserName);
                values.Add("Lmsg_confirm_type", msgContent.ObrConfirmType);
                values.Add("Lmsg_inside_flag", msgContent.ObrInsideFlag);

                values.Add("Lmsg_pid_name", msgContent.PidName);
                values.Add("Lmsg_pid_sex", msgContent.PidSex);
                values.Add("Lmsg_pid_age", msgContent.PidAge);
                values.Add("Lmsg_pid_bed_no", msgContent.PidBedNo);
                values.Add("Lmsg_pid_in_no", msgContent.PidInNo);

                values.Add("Lmsg_pid_Dsorc_id", msgContent.PidSrcId);
                values.Add("Lmsg_doctor_code", msgContent.ObrDoctorCode);
                values.Add("Lmsg_doctor_name", msgContent.ObrDoctorName);
                values.Add("Lmsg_send_dept_code", msgContent.ObrSendDeptCode);
                values.Add("Lmsg_tel", msgContent.PidTel);

                helper.InsertOperation("Lis_message_content", values);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool UpdateObrMessageContent(EntityDicObrMessageContent msgContent)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();

                values.Add("Lmsg_type", msgContent.ObrType);
                values.Add("Lmsg_send_user_id", msgContent.ObrSendUserId);
                values.Add("Lmsg_send_user_name", msgContent.ObrSendUserName);
                values.Add("Lmsg_send_server", msgContent.ObrSendServer);

                values.Add("Lmsg_create_time", msgContent.ObrCreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Lmsg_title", msgContent.ObrMessageTitle);
                values.Add("Lmsg_receive", msgContent.ObrReceive);
                values.Add("Lmsg_content", msgContent.ObrContent);
                values.Add("Lmsg_parent_Lmsg_id", msgContent.ObrParentId);

                values.Add("del_flag", msgContent.DelFlag);
                values.Add("Lmsg_expiration_date", msgContent.ObrExpirationDate);
                values.Add("Lmsg_extend_a", msgContent.ObrValueA);
                values.Add("Lmsg_extend_b", msgContent.ObrValueB);
                values.Add("Lmsg_extend_c", msgContent.ObrValueC);

                values.Add("Lmsg_extend_d", msgContent.ObrValueD);
                values.Add("Lmsg_audit_user_id", msgContent.ObrAuditUserId);
                values.Add("Lmsg_audit_user_name", msgContent.ObrAuditUserName);
                values.Add("Lmsg_confirm_type", msgContent.ObrConfirmType);
                values.Add("Lmsg_inside_flag", msgContent.ObrInsideFlag);

                values.Add("Lmsg_pid_name", msgContent.PidName);
                values.Add("Lmsg_pid_sex", msgContent.PidSex);
                values.Add("Lmsg_pid_age", msgContent.PidAge);
                values.Add("Lmsg_pid_bed_no", msgContent.PidBedNo);
                values.Add("Lmsg_pid_in_no", msgContent.PidInNo);

                values.Add("Lmsg_pid_Dsorc_id", msgContent.PidSrcId);
                values.Add("Lmsg_doctor_code", msgContent.ObrDoctorCode);
                values.Add("Lmsg_doctor_name", msgContent.ObrDoctorName);
                values.Add("Lmsg_send_dept_code", msgContent.ObrSendDeptCode);
                values.Add("Lmsg_tel", msgContent.PidTel);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Lmsg_id", msgContent.ObrId);

                helper.UpdateOperation("Lis_message_content", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        //不取消其他提醒，只取消内部提醒
        public bool UpdateObrMessageContentHaveIn(EntityDicObrMessageContent msgContent)
        {
            try
            {
                DBManager helper = new DBManager();
                //这里更新了标志obr_inside_flag及其他
                string sqlStr = string.Format(@"update Lis_message_content 
                                                set Lmsg_inside_flag ='1',
                                                    Lmsg_audit_user_id=@obr_audit_user_id,
                                                    Lmsg_audit_user_name=@obr_audit_user_name,
                                                    Lmsg_confirm_type=@obr_confirm_type  
                                              where  Lmsg_type in ('1024','4096','2024','3024') 
                                                   and del_flag = 0 
                                                   and Lmsg_extend_a =@obr_value_a");
                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@obr_audit_user_id", msgContent.ObrAuditUserId));
                paramHt.Add(new SqlParameter("@obr_audit_user_name", msgContent.ObrAuditUserName));
                paramHt.Add(new SqlParameter("@obr_confirm_type", msgContent.ObrConfirmType));
                paramHt.Add(new SqlParameter("@obr_value_a", msgContent.ObrValueA));

                return helper.ExecCommand(sqlStr, paramHt) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool UpdateObrMessageContentHaveInDelFlag(EntityDicObrMessageContent msgContent)
        {
            try
            {
                DBManager helper = new DBManager();
                //这里更新了删除标志del_flag及其他
                string sqlStr = string.Format(@"update Lis_message_content 
                                                set del_flag ='1',
                                                    Lmsg_audit_user_id=@obr_audit_user_id,
                                                    Lmsg_audit_user_name=@obr_audit_user_name,
                                                    Lmsg_confirm_type=@obr_confirm_type   
                                              where  Lmsg_type in ('1024','4096','2024','3024') 
                                                   and del_flag = 0 
                                                   and Lmsg_extend_a =@obr_value_a");

                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@obr_audit_user_id", msgContent.ObrAuditUserId));
                paramHt.Add(new SqlParameter("@obr_audit_user_name", msgContent.ObrAuditUserName));
                paramHt.Add(new SqlParameter("@obr_confirm_type", msgContent.ObrConfirmType));
                paramHt.Add(new SqlParameter("@obr_value_a", msgContent.ObrValueA));

                return helper.ExecCommand(sqlStr, paramHt) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool DeleteObrMessageContent(EntityDicObrMessageContent msgContent)
        {
            //物理删除 一般不会用到
            try
            {
                DBManager helper = new DBManager();

                if (!string.IsNullOrEmpty(msgContent.ObrId))
                {
                    string sqlStr = string.Format(@"delete from Lis_message_content where Lmsg_id = '{0}' ", msgContent.ObrId);
                    helper.ExecSql(sqlStr);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return false;
        }


        public List<EntityDicObrMessageContent> SearchAllMessageContent()
        {
            try
            {
                DBManager helper = new DBManager();

                string sqlStr = string.Format(@"select * from Lis_message_content where del_flag='0'");

                DataTable dtMsgContent = helper.ExecuteDtSql(sqlStr);
                List<EntityDicObrMessageContent> list = EntityManager<EntityDicObrMessageContent>.ConvertToList(dtMsgContent).OrderBy(i => i.ObrId).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicObrMessageContent>();
            }
        }

        public List<EntityDicObrMessageContent> GetBacPatientsMsg(string RepId)
        {
            try
            {
                DBManager helper = new DBManager();

                string strSql = string.Format(@"select 
Pat_lis_main.Pma_com_name,
Lis_message_content.Lmsg_extend_c,
Lis_message_content.Lmsg_extend_d 
from Lis_message_content
left join Pat_lis_main on Pat_lis_main.Pma_rep_id=Lis_message_content.Lmsg_extend_a
where Pat_lis_main.Pma_rep_id='{0}'", RepId);

                DataTable dtMsgContent = helper.ExecuteDtSql(strSql);
                List<EntityDicObrMessageContent> list = EntityManager<EntityDicObrMessageContent>.ConvertToList(dtMsgContent).OrderBy(i => i.ObrId).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicObrMessageContent>();
            }
        }

        public bool UpdateObrMsgConToInsignByID(EntityDicObrMessageContent msgContent)
        {
            try
            {
                DBManager helper = new DBManager();

                string sqlStr = string.Format(@"update Lis_message_content 
                                                set Lmsg_inside_flag ='1',
                                                    Lmsg_audit_user_id=@obr_audit_user_id,
                                                    Lmsg_audit_user_name=@obr_audit_user_name,
                                                    Lmsg_confirm_type=@obr_confirm_type 
                                               where Lmsg_id = @obr_id");

                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@obr_audit_user_id", msgContent.ObrAuditUserId));
                paramHt.Add(new SqlParameter("@obr_audit_user_name", msgContent.ObrAuditUserName));
                paramHt.Add(new SqlParameter("@obr_confirm_type", msgContent.ObrConfirmType));
                paramHt.Add(new SqlParameter("@obr_id", msgContent.ObrId));

                return helper.ExecCommand(sqlStr, paramHt) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool UpdateObrMsgConToInsignByRepID(string RepId)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Lmsg_inside_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Lmsg_extend_a", RepId);

                helper.UpdateOperation("Lis_message_content", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool UpdateObrMsgConToDelFlagByID(EntityDicObrMessageContent msgContent)
        {
            try
            {
                DBManager helper = new DBManager();

                string sqlStr = string.Format(@"update Lis_message_content 
                                                set del_flag ='1',
                                                    Lmsg_audit_user_id=@obr_audit_user_id,
                                                    Lmsg_audit_user_name=@obr_audit_user_name,
                                                    Lmsg_confirm_type=@obr_confirm_type  
                                                where Lmsg_id = @obr_id ");

                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@obr_audit_user_id", msgContent.ObrAuditUserId));
                paramHt.Add(new SqlParameter("@obr_audit_user_name", msgContent.ObrAuditUserName));
                paramHt.Add(new SqlParameter("@obr_confirm_type", msgContent.ObrConfirmType));
                paramHt.Add(new SqlParameter("@obr_id", msgContent.ObrId));

                return helper.ExecCommand(sqlStr, paramHt) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicObrMessageContent> GetLEDMessage(bool bGetDeleted, bool bGetExpired)
        {
            List<EntityDicObrMessageContent> listMsgContent = new List<EntityDicObrMessageContent>();
            try
            {
                string sql_content = string.Empty;

                if (bGetDeleted)
                {
                    sql_content = string.Format(@"select * from Lis_message_content where Lmsg_type in ({0},{1},{2})"
                                    , (int)EnumObrMessageType.LED1, (int)EnumObrMessageType.LED2, (int)EnumObrMessageType.LED3);
                }
                else
                {
                    sql_content = string.Format(@"select * from Lis_message_content where Lmsg_type in ({0},{1},{2}) and del_flag = 0"
                                                 , (int)EnumObrMessageType.LED1, (int)EnumObrMessageType.LED2, (int)EnumObrMessageType.LED3);
                }

                if (bGetExpired)
                {
                }
                else
                {
                    sql_content += @" and (Lmsg_expiration_date is null or getdate() <= Lmsg_expiration_date)";
                }

                DBManager helper = new DBManager();
                DataTable dtMsgContent = helper.ExecuteDtSql(sql_content);

                listMsgContent = EntityManager<EntityDicObrMessageContent>.ConvertToList(dtMsgContent).OrderBy(i => i.ObrCreateTime).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return listMsgContent;
        }

        public List<EntityDicObrMessageContent> GetMessageByCondition(EntityDicObrMessageContent content)
        {
            List<EntityDicObrMessageContent> listContent = new List<EntityDicObrMessageContent>();
            try
            {
                string sql = "select Lmsg_audit_user_name,Lmsg_extend_c,Lmsg_extend_a,Lmsg_id,Lmsg_nurse_read_flag,Lmsg_doctor_read_flag from Lis_message_content with(nolock)  where 1=1";
                if (!string.IsNullOrEmpty(content.ObrValueA))
                {
                    sql += string.Format(" and Lmsg_extend_a='{0}'", content.ObrValueA);
                }

                //微生物专用查询 ObrValueD 传入 pri_pat_key
                if (!string.IsNullOrEmpty(content.ObrValueD))
                {
                    sql += string.Format(" and Lmsg_extend_a in (select pri_id from obr_mic_print_report where  pri_pat_key='{0}' )", content.ObrValueD);
                }
                if (content.ObrType != 0)
                {
                    sql += string.Format(" and Lmsg_type={0} ", content.ObrType);
                }
                if (content.DelFlag)
                {
                    sql += string.Format(" and del_flag='{0}'", 0);
                }
                DBManager helper = new DBManager();
                DataTable dtMsgContent = helper.ExecuteDtSql(sql);

                listContent = EntityManager<EntityDicObrMessageContent>.ConvertToList(dtMsgContent);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return listContent;
        }

        public bool UpdateMessageDelFlag(string valueA, int type1, int type2)
        {
            bool result = false;
            try
            {
                string sql = string.Format(@"
                update Lis_message_content set del_flag = 1 where Lmsg_extend_a = '{0}' and (Lmsg_type = {1} or Lmsg_type = {2})", valueA, type1, type2);
                DBManager helper = new DBManager();
                result = helper.ExecCommand(sql) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }

        public bool UPdateMessageDelFlagByObrValueA(string obrValueA)
        {
            bool isDelFlag = false;
            if (!string.IsNullOrEmpty(obrValueA))
            {
                try
                {
                    DBManager helper = new DBManager();
                    string sql = string.Format(@"delete from Lis_message_content  where Lmsg_extend_a = '{0}' ", obrValueA);

                    isDelFlag = helper.ExecCommand(sql) > 0;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return isDelFlag;
        }

        public bool IsDateHoliday()
        {
            bool isHoliday = false;
            try
            {
                DBManager helper = new DBManager();

                string today = DateTime.Now.Date.ToString();//获取当天时间

                string sql = string.Format(@"select top 1 1 from holiday_set_warn where h_date='{0}' and h_flag='1'", today);
                DataTable holidayDT = helper.ExecuteDtSql(sql);
                if (holidayDT != null && holidayDT.Rows.Count > 0)
                {
                    isHoliday = true;
                }
                else
                {
                    isHoliday = false;
                }

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("节假日判断异常！", ex);
            }
            return isHoliday;
        }


        public List<EntityDicObrMessageContent> GetAllMessage(bool bGetDeleted, bool bGetExpired)
        {
            List<EntityDicObrMessageContent> listContent = new List<EntityDicObrMessageContent>();
            try
            {
                string sql = string.Empty;

                if (bGetDeleted)
                {
                    sql = @"select * from Lis_message_content where 1=1 ";
                }
                else
                {
                    sql = @"select * from Lis_message_content where del_flag = 0";
                }

                if (bGetExpired)
                {

                }
                else
                {
                    sql += @" and (Lmsg_expiration_date is null or getdate() <= Lmsg_expiration_date)";
                }

                DBManager helper = new DBManager();
                DataTable table = helper.ExecuteDtSql(sql);
                listContent = EntityManager<EntityDicObrMessageContent>.ConvertToList(table);

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return listContent;
        }
        public List<EntityDicObrMessageContent> GetDIYCriticalMsg(EntityDicObrMessageContent condition)
        {
            List<EntityDicObrMessageContent> listContent = new List<EntityDicObrMessageContent>();
            try
            {
                string sqlWhere = GetWhere(condition);
                string sql = string.Format(@"SELECT Lis_message_content.Lmsg_id, Lis_message_content.Lmsg_type,
 Lis_message_content.Lmsg_send_user_id, Lis_message_content.Lmsg_send_user_name,
 Lis_message_content.Lmsg_send_server, 
 Lis_message_content.Lmsg_create_time,
 Lis_message_content.Lmsg_title,
 Lis_message_content.Lmsg_content,
 Lis_message_content.del_flag,
 (case when Lis_message_content.del_flag=1 then '已查看' else '未查看' end) as msg_status,
 (case when Lis_message_content.Lmsg_inside_flag='1' then '是' else '' end) as msf_insgin_flag_str,
 Lis_message_content.Lmsg_extend_a,
 Lis_message_content.Lmsg_extend_c,                    
 Lis_message_content.Lmsg_audit_user_id,
 Lis_message_content.Lmsg_audit_user_name,
 Lis_message_content.Lmsg_pid_name, 
 Lis_message_content.Lmsg_pid_sex,
(case Lis_message_content.Lmsg_pid_sex when 1 then '男' when 2 then '女' else '未知' end) as pat_sex_str,
 Lis_message_content.Lmsg_pid_age,
 Lis_message_content.Lmsg_pid_bed_no,
 Lis_message_content.Lmsg_tel,
 Lis_message_content.Lmsg_pid_in_no,
 Lis_message_content.Lmsg_pid_Dsorc_id,
 Lis_message_content.Lmsg_doctor_code,
 Lis_message_content.Lmsg_doctor_name,
 Dict_source.Dsorc_name,
 Lis_message_receive.Lmsgrec_user_id,
Lis_message_receive.Lmsgrec_read_time as msg_date_look, 
 Lis_message_receive.Lmsgrec_user_name
FROM  Lis_message_content with(nolock) INNER JOIN
Lis_message_receive with(nolock) ON Lis_message_content.Lmsg_id = Lis_message_receive.Lmsgrec_Lmsg_id
left join Dict_source on Dict_source.Dsorc_id=Lis_message_content.Lmsg_pid_Dsorc_id
left join Pid_report_main_ext with(nolock) on Lis_message_content.Lmsg_id=Pid_report_main_ext.pid_id
where 1=1
{0}  and Lis_message_content.Lmsg_type=2024", sqlWhere);

                DBManager helper = new DBManager();
                DataTable table = helper.ExecuteDtSql(sql);
                listContent = EntityManager<EntityDicObrMessageContent>.ConvertToList(table).OrderByDescending(w => w.ObrCreateTime).ToList();

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return listContent;
        }

        public string GetWhere(EntityDicObrMessageContent condition)
        {
            string sqlWhere = string.Empty;
            //病人号
            if (!string.IsNullOrEmpty(condition.PidInNo))
            {
                sqlWhere += string.Format(" and Lis_message_content.Lmsg_pid_in_no='{0}'",
                        condition.PidInNo);
            }
            //病人姓名
            if (!string.IsNullOrEmpty(condition.PidName))
            {
                sqlWhere += string.Format("and Lis_message_content.Lmsg_pid_name like '{0}%'",
                        condition.PidName);
            }
            //床号
            if (!string.IsNullOrEmpty(condition.PidBedNo))
            {
                sqlWhere += string.Format(" and Lis_message_content.Lmsg_pid_bed_no='{0}'",
                        condition.PidBedNo);
            }
            //病人来源
            if (!string.IsNullOrEmpty(condition.PidSrcId))
            {
                sqlWhere += string.Format(" and Lis_message_content.Lmsg_pid_Dsorc_id='{0}'",
                        condition.PidSrcId);
            }
            //科室
            if (!string.IsNullOrEmpty(condition.ObrUserName))
            {
                sqlWhere += string.Format(" and Lis_message_receive.Lmsgrec_user_id='{0}'",
                        condition.ObrUserName);
            }
            //查看状态
            if (!string.IsNullOrEmpty(condition.MsgStatus))
            {
                sqlWhere += string.Format(" and Lis_message_content.del_flag='{0}'",
                        condition.MsgStatus);
            }
            //填写人科室
            if (!string.IsNullOrEmpty(condition.ObrSendDeptCode))
            {
                sqlWhere += string.Format(" and Lis_message_content.Lmsg_send_dept_code='{0}'",
                        condition.ObrSendDeptCode);
            }
            //日期
            if (!string.IsNullOrEmpty(condition.StartDate.ToString()) && !string.IsNullOrEmpty(condition.EndDate.ToString("yyyy-MM-dd HH:mm:ss")))
            {
                sqlWhere += string.Format(" and Lis_message_content.Lmsg_create_time>='{0}' and Lis_message_content.Lmsg_create_time<'{1}'",
                        condition.StartDate, condition.EndDate);
            }
            return sqlWhere;
        }

        public bool UpdateReadFlag(string userRole, string obrId)
        {
            bool result = false;
            try
            {
                string sql = string.Format(@"
                update Lis_message_content set Lmsg_nurse_read_flag = '1' where Lmsg_id = '{0}'", obrId);
                if (userRole == "doctor")
                {
                    sql= string.Format(@"
                update Lis_message_content set Lmsg_doctor_read_flag = '1' where Lmsg_id = '{0}'", obrId);
                }
                DBManager helper = new DBManager();
                result = helper.ExecCommand(sql) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }
    }
}
