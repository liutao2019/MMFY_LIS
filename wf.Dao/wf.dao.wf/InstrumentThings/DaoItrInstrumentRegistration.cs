using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.core;
using System.Data;
using dcl.common;

namespace dcl.dao.clab
{
    [Export("wf.plugin.wf", typeof(IDaoItrInstrumentRegistration))]
    public class DaoItrInstrumentRegistration : IDaoItrInstrumentRegistration
    {
        public List<EntityDicInstrmtMaintainRegistration> GetRegistration(string strItrId)
        {
            try
            {
                String sql = string.Format(@"select  
            isnull(Dict_itr_instrument.del_flag,'') itr_del,
			isnull(Dict_itr_instrument.Ditr_status,'')  itr_status,
            Dim_id,Dim_content,Dim_Ditr_id,Dim_warning_time,Dim_closeaudit_time,Dim_interval,Dim_type,Dim_operate_tips,Dim_astrict,
            Dimr_register_date,  Dimr_content, Dimr_exp,
			isnull(Dim_fuzzy_warning_time,'') Dim_fuzzy_warning_time,
			isnull(Dim_fuzzy_closeAudit_time,'') Dim_fuzzy_closeAudit_time,
			isnull(Dim_fuzzy_interval_time,'') Dim_fuzzy_interval_time,
            Dim_desc
            from Dict_instrmt_maintain 
            left join Dict_instrmt_maintain_Registration as reg on reg.Dimr_Dim_id=Dict_instrmt_maintain.Dim_id
            and Dimr_id=(
            select top 1 Dimr_id from Dict_instrmt_maintain_Registration where Dimr_Dim_id=reg.Dimr_Dim_id order by Dimr_register_date desc)
            left join Dict_itr_instrument on Dict_instrmt_maintain.Dim_Ditr_id=Dict_itr_instrument.Ditr_id
            where Dict_instrmt_maintain.Dim_Ditr_id='{0}' ", strItrId);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntityDicInstrmtMaintainRegistration> list = EntityManager<EntityDicInstrmtMaintainRegistration>.ConvertToList(dt).OrderBy(i => i.MaiId).ToList();

                List<EntityDicInstrmtMaintainRegistration> listReturn = new List<EntityDicInstrmtMaintainRegistration>();
                foreach (var infoRegis in list)
                {
                    //EntityDicInstrmtMaintainRegistration eyRegistration = new EntityDicInstrmtMaintainRegistration();
                    var eyRegistration = (EntityDicInstrmtMaintainRegistration)infoRegis.Clone();

                    eyRegistration.RegItrId = infoRegis.MaiItrId;

                    if (infoRegis.MaiId.ToString() != "")
                        eyRegistration.RegMaiId = infoRegis.MaiId;

                    eyRegistration.RegInterval = infoRegis.MaiCloseAuditTime;

                    bool status = true;
                    if (infoRegis.ItrStatus.ToString() == "停用" || infoRegis.DelFlag.ToString() == "1")
                    {
                        status = false;
                    }
                    //保养登记中的时间计算不需要精确到小时，需要模糊时间为一天，即结束时间到当天的23：59
                    //停用的仪器在设备保养登记中设置开关，选择是否显示，并不作保养时间的计算
                    //佛山市一2011年10月27日12:22:25
                    if (infoRegis.RegRegisterDate != null && status)
                    {
                        DateTime? nextMaintainTime = null;
                        if (infoRegis.FuzzyIntervalTime == "1")
                        {
                            if (infoRegis.MaiInterval != 0)
                            {
                                nextMaintainTime = eyRegistration.RegRegisterDate.Value.AddHours(Convert.ToDouble(infoRegis.MaiInterval)).Date.AddDays(1).AddSeconds(-1);
                            }
                        }
                        else
                        {
                            if (infoRegis.MaiInterval != 0)
                            {
                                nextMaintainTime = eyRegistration.RegRegisterDate.Value.AddHours(Convert.ToDouble(infoRegis.MaiInterval));
                            }
                        }
                        if (nextMaintainTime.HasValue)
                        {
                            eyRegistration.NextMaintainTime = nextMaintainTime.Value.ToString();
                            if ((DateTime.Now - nextMaintainTime.Value).TotalMinutes > 0)
                            {
                                string result = string.Empty;
                                TimeSpan span = (DateTime.Now - nextMaintainTime.Value);
                                int t_day = span.Days;
                                int hour = span.Hours;
                                int min = span.Minutes;
                                if (hour > 0)
                                {
                                    if (t_day > 0)
                                        result = string.Format("{0}小时", t_day * 24 + hour);
                                    else
                                        result = string.Format("{0}小时", hour);
                                }
                                if (min > 0)
                                {
                                    result += string.Format("{0}分钟", min);
                                }

                                eyRegistration.OverrunIntervalTime = result;
                            }
                        }

                        if (infoRegis.FuzzyWarningTime == "1")
                        {
                            if (infoRegis.MaiWarningTime != 0)
                                eyRegistration.IsOverrunWaringTime = DateTime.Compare(eyRegistration.RegRegisterDate.Value.AddHours(Convert.ToDouble(infoRegis.MaiWarningTime)).Date.AddDays(1).AddSeconds(-1), DateTime.Now) <= 0;
                        }
                        else
                        {
                            if (infoRegis.MaiWarningTime != 0)
                                eyRegistration.IsOverrunWaringTime = DateTime.Compare(eyRegistration.RegRegisterDate.Value.AddHours(Convert.ToDouble(infoRegis.MaiWarningTime)), DateTime.Now) <= 0;

                        }

                        if (infoRegis.FuzzyCloseAuditTime == "1")
                        {
                            if (eyRegistration.RegInterval != 0)
                                eyRegistration.IsOverrunAuditTime = DateTime.Compare(eyRegistration.RegRegisterDate.Value.AddHours(Convert.ToDouble(infoRegis.MaiCloseAuditTime)).Date.AddDays(1).AddSeconds(-1), DateTime.Now) <= 0;
                        }
                        else
                        {
                            if (eyRegistration.RegInterval != 0)
                                eyRegistration.IsOverrunAuditTime = DateTime.Compare(eyRegistration.RegRegisterDate.Value.AddHours(Convert.ToDouble(infoRegis.MaiCloseAuditTime)), DateTime.Now) <= 0;
                        }
                    }

                    eyRegistration.RegOperateType = infoRegis.MaiType.ToString();
                    //eyRegistration.RegContent = string.Empty;//drRegis["reg_content"].ToString();
                    listReturn.Add(eyRegistration);
                }

                return listReturn;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicInstrmtMaintainRegistration>();
            }
        }

        public List<EntityDicInstrmtMaintainRegistration> GetRegistrationByDate(string strmai_id)
        {
            try
            {
                string sql = string.Format(@"select top 20 Dict_instrmt_maintain.Dim_content,
                                                     Dict_instrmt_maintain_Registration.Dimr_register_code,
                                                     Dict_instrmt_maintain_Registration.Dimr_content,
                                                     Dict_instrmt_maintain_Registration.Dimr_over_interval_time,
                                                     Dict_instrmt_maintain_Registration.Dimr_exp,
                                                     Base_user.Buser_name reg_register_code
                                             from Dict_instrmt_maintain_Registration
                                                left join Dict_instrmt_maintain on Dict_instrmt_maintain_Registration.Dimr_Dim_id=Dict_instrmt_maintain.Dim_id
                                                left join Base_user on Dict_instrmt_maintain_Registration.Dimr_register_code=Base_user.Buser_loginid
                                             where Dict_instrmt_maintain.Dim_id={0}
                                                 and Dict_instrmt_maintain_Registration.Dimr_register_date >='{1}' "
                           , strmai_id, DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd HH:mm:ss"));

                DBManager helper = new DBManager();

                DataTable dtRegistration = helper.ExecuteDtSql(sql);

                List<EntityDicInstrmtMaintainRegistration> lisRegistration = EntityManager<EntityDicInstrmtMaintainRegistration>.ConvertToList(dtRegistration).OrderByDescending(i => i.RegRegisterDate).ToList();

                List<EntityDicInstrmtMaintainRegistration> listReturn = new List<EntityDicInstrmtMaintainRegistration>();
                foreach (var drRegistration in lisRegistration)
                {
                    //EntityDicInstrmtMaintainRegistration regis = new EntityDicInstrmtMaintainRegistration();
                    var regis = (EntityDicInstrmtMaintainRegistration)drRegistration.Clone();

                    regis.RegContent = drRegistration.MaiContent;
                    regis.RegOperateContent = drRegistration.RegContent;

                    if (drRegistration.OverIntervalTime != null
                        && !string.IsNullOrEmpty(drRegistration.OverIntervalTime.ToString()))
                    {
                        int over_interval_time = Convert.ToInt32(drRegistration.OverIntervalTime);
                        TimeSpan time = new TimeSpan(0, over_interval_time, 0);
                        int hour = time.Days * 24 + time.Hours;
                        string hourStr = hour > 0 ? string.Format("{0}小时", hour) : "";
                        string min = time.Minutes > 0 ? string.Format("{0}分钟", time.Minutes) : "";
                        regis.OverrunIntervalTime = string.Format("{0}{1}", hourStr, min);
                    }

                    listReturn.Add(regis);
                }

                return listReturn;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicInstrmtMaintainRegistration>();
            }
        }

        public int MaintainRegistration(List<EntityDicInstrmtMaintainRegistration> listRegis)
        {
            int count = 0;
            try
            {
                DBManager helper = new DBManager();

                foreach (EntityDicInstrmtMaintainRegistration regis in listRegis)
                {
                    if (regis.RegInterval > 0)
                    {
                        DelMes(regis.RegMaiId, regis.RegItrId);

                        AddMes(regis.RegMaiId, DateTime.Now.AddHours(regis.RegInterval), regis.RegItrId);
                    }

                    //表名称太长，系统表Sys_tableid中允许存放32个字节，故换引用别名来获取动态ID
                    int id = IdentityHelper.GetMedIdentity("Dict_instrmt_maintain_Registration");
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values.Add("Dimr_id", id);
                    values.Add("Dimr_Ditr_id", regis.RegItrId);
                    values.Add("Dimr_register_date", DateTime.Now);
                    values.Add("Dimr_register_code", regis.RegRegisterCode);
                    values.Add("Dimr_Dim_id", regis.RegMaiId);
                    values.Add("Dimr_content", regis.RegContent);

                    if (regis.LastOperateTime.Subtract(Convert.ToDateTime("1900-01-01 0:00:00")).Seconds > 0)
                    {
                        TimeSpan tsTime = DateTime.Now.Subtract(regis.LastOperateTime);
                        values.Add("Dimr_interval", Convert.ToInt32(tsTime.TotalMinutes));
                    }
                    else
                    {
                        values.Add("Dimr_interval", 0);
                    }

                    values.Add("Dimr_exp", regis.RegExp != null ? regis.RegExp : string.Empty);
                    int overrunMin = 0;
                    if (!string.IsNullOrEmpty(regis.OverrunIntervalTime))
                    {
                        string[] value = regis.OverrunIntervalTime.Split(new string[] { "小时", "分钟" }, StringSplitOptions.RemoveEmptyEntries);

                        int hour = 0;
                        int min = 0;
                        if (regis.OverrunIntervalTime.Contains("小时"))
                        {
                            hour = int.Parse(value[0]);
                        }
                        if (regis.OverrunIntervalTime.Contains("分钟"))
                        {
                            min = int.Parse(value[value.Length - 1]);
                        }
                        overrunMin = (int)new TimeSpan(hour, min, 0).TotalMinutes;
                    }
                    values.Add("Dimr_over_interval_time", overrunMin.ToString());

                    helper.InsertOperation("Dict_instrmt_maintain_Registration", values);

                    regis.RegId = id;
                    count++;
                }
                return count;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return count;
            }
        }

        public int DelMes(int mai_id, string itr_id)
        {
            int count = 0;
            try
            {
                DBManager helper = new DBManager();
                if (mai_id != 0)
                {
                    string strDelMes = string.Format("delete Dict_qc_rule_mes where Dqrm_Ditr_id='{0}' and Dqrm_node_id={1} and Dqrm_type='到期'", itr_id, mai_id);
                    count += helper.ExecCommand(strDelMes);
                }
                else
                {
                    string strDelMes = string.Format("delete Dict_qc_rule_mes where Dqrm_Ditr_id='{0}' and  Dqrm_type='故障'", itr_id);
                    count += helper.ExecCommand(strDelMes);
                }
                return count;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return count;
            }
        }
        public int AddMes(int mai_id, DateTime end_time, string itr_id)
        {
            int count = 0;
            try
            {
                DBManager helper = new DBManager();
                if (mai_id != 0)
                {
                    string strAddMes = string.Format(" insert into Dict_qc_rule_mes  values (null,{0},null,getdate(),'{1}','到期',{2})", mai_id, end_time.ToString("yyyy-MM-dd HH:mm:ss"), itr_id);

                    count += helper.ExecCommand(strAddMes);
                }
                else
                {
                    string strAddMes = string.Format(" insert into Dict_qc_rule_mes  values (null,null,null,getdate(),getdate(),'故障',{0})", itr_id);

                    count += helper.ExecCommand(strAddMes);
                }

                return count;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return count;
            }
        }

        public List<EntityDicInstrmtMaintainRegistration> GetMaintainData(EntityDicInstrmtMaintainRegistration strWhere)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select  Ditr_ename,Buser_name,Dim_id,
Dpro_name,Dpro_id,
Dim_content,
Dim_Ditr_id,Dim_warning_time,Dim_closeaudit_time,
Dim_interval,Dim_type,Dim_operate_tips,Dim_astrict,
Dimr_register_date,  Dimr_content, 
Dimr_exp,cast(Dimr_over_interval_time as varchar(50)) Dimr_over_interval_time
from Dict_instrmt_maintain_Registration as reg
left join Dict_instrmt_maintain on reg.Dimr_Dim_id=Dict_instrmt_maintain.Dim_id
left join Dict_itr_instrument on Dict_instrmt_maintain.Dim_Ditr_id=Dict_itr_instrument.Ditr_id
left join Base_user on reg.Dimr_register_code=Base_user.Buser_loginid
left join Dict_profession on Dict_itr_instrument.Ditr_lab_id=Dict_profession.Dpro_id ");
                strSql.Append(" where");
                strSql.Append(" Ditr_ename is not null");
                strSql.Append(" and Dimr_register_date>='" + strWhere.DeStartTime.ToString("yyyy-MM-dd HH:mm:ss") + "'");
                strSql.Append(" and Dimr_register_date<='" + strWhere.DeEndTime.ToString("yyyy-MM-dd HH:mm:ss") + "'");
                if (strWhere.MaiItrId != null && strWhere.MaiItrId.Trim() != string.Empty)
                    strSql.Append(" and Dim_Ditr_id='" + strWhere.MaiItrId.Trim() + "'");
                if (strWhere.ProId != null && strWhere.ProId.Trim() != string.Empty)
                    strSql.Append(" and Dict_profession.Dpro_id='" + strWhere.ProId.Trim() + "'");

                if (strWhere.StrTimeType == "保养时间")
                {
                    strSql.Append(" and reg.Dimr_over_interval_time>" + (strWhere.Hour * 60).ToString() + "");
                }
                else
                {
                    if (strWhere.Hour > 0)
                    {
                        strSql.Append(" and Dimr_interval");

                        switch (strWhere.StrOperate)
                        {
                            case "大于":
                                strSql.Append(" >");
                                break;
                            case "等于":
                                strSql.Append(" =");
                                break;
                            default:
                                break;
                        }

                        switch (strWhere.StrTimeType)
                        {
                            case "提醒时间":
                                strSql.Append(string.Format("(Dim_warning_time*60+{0})", strWhere.Hour));
                                break;
                            case "间隔时间":
                                strSql.Append(string.Format("(Dim_interval*60+{0})", strWhere.Hour));
                                break;
                            case "关闭审核时间":
                                strSql.Append(string.Format("(Dim_closeaudit_time*60+{0})", strWhere.Hour));
                                break;
                            default:
                                break;
                        }
                    }
                }

                DBManager helper = new DBManager();

                DataTable dtMaintain = helper.ExecuteDtSql(strSql.ToString());

                List<EntityDicInstrmtMaintainRegistration> list = EntityManager<EntityDicInstrmtMaintainRegistration>.ConvertToList(dtMaintain).OrderBy(i => i.RegId).ToList();

                foreach (var item in list)
                {
                    if (item.OverIntervalTime != null)
                    {
                        string value = item.OverIntervalTime.ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            string result = string.Empty;
                            int valueInt = 0;
                            if (int.TryParse(value, out valueInt))
                            {
                                int hour = valueInt / 60;
                                int min = valueInt % 60;
                                if (hour > 0)
                                {
                                    result = string.Format("{0}小时", hour);
                                }
                                if (min > 0)
                                {
                                    result += string.Format("{0}分钟", min);
                                }
                                item.OverIntervalTime = result;
                            }
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicInstrmtMaintainRegistration>();
            }

        }
    }
}
