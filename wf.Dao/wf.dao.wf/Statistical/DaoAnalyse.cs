using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoAnalyse))]
    public class DaoAnalyse : IDaoAnalyse
    {
        List<EntitySysReport> GetSysReportByCode(string repCode)
        {
            try
            {
                String sql = string.Format(@"select * from Base_report where Brep_code = '{0}'", repCode);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntitySysReport> list = EntityManager<EntitySysReport>.ConvertToList(dt).OrderBy(i => i.RepId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySysReport>();
            }
        }

        public DataSet GetReportData(EntityStatisticsQC StatQc)
        {
            DataSet result = new DataSet();
            try
            {
                StatQc = GetWhere(StatQc);
                string code = StatQc.ReportCode.ToString();

                List<EntitySysReport> dtEx = GetSysReportByCode(code);
                if (dtEx.Count > 0)
                {
                    string sql = EncryptClass.Decrypt(dtEx[0].RepSql.ToString());
                    sql = sql.Replace("&where&", StatQc.Where);
                    sql = sql.Replace("&groupName&", StatQc.GroupName);
                    sql = sql.Replace("&group&", StatQc.Group);
                    sql = sql.Replace("&order&", StatQc.Order);

                    DBManager helper = new DBManager();

                    DataTable an = helper.ExecuteDtSql(sql);
                    an.TableName = "可设计字段";
                    result.Tables.Add(an.Copy());
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                DataTable dt = new DataTable();
                dt.Columns.Add("ErrorMessage");
                DataRow dr = dt.NewRow();
                dr["ErrorMessage"] = "报表查询语句有误，请联系管理员";
                dt.Rows.Add(dr);
                result.Tables.Add(dt);
            }
            return result;
        }

        EntityStatisticsQC GetWhere(EntityStatisticsQC statQC)
        {
            string where = " and Pat_lis_main.Pma_status in (2,4)";

            string strTime = "Pat_lis_main.Pma_in_date";
            if (statQC.TimeType == "标本签收时间")
                strTime = "Pat_lis_main.Pma_apply_date";

            if (statQC.TimeType == "报告时间")
                strTime = "Pat_lis_main.Pma_report_date";

            //排除回退条码的(含无条码的)
            if (statQC.cbWithoutReturn)
            {
                where += " and exists (select top 1 1 from Sample_main where Sample_main.Sma_bar_id=Sample_main.Sma_bar_code and (Sample_main.samp_return_times=0 or Sample_main.samp_return_times is null)) ";
            }
            if (!string.IsNullOrEmpty(statQC.DateEditStart))
            {
                where += " and " + strTime + " >= '" + statQC.DateEditStart + "'";
            }
            if (!string.IsNullOrEmpty(statQC.DateEditEnd))
            {
                where += " and " + strTime + " <'" + Convert.ToDateTime(statQC.DateEditEnd).AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }


            if (!string.IsNullOrEmpty(statQC.EditYBStart))
            {
                if (Convert.ToInt32(statQC.EditYBStart) > 0)
                {
                    where += " and cast(Pat_lis_main.Pma_sid as bigint)>=" + statQC.EditYBStart;
                }
            }
            if (!string.IsNullOrEmpty(statQC.EditYBEnd))
            {
                if (Convert.ToInt32(statQC.EditYBEnd) > 0)
                {
                    where += " and cast(Pat_lis_main.Pma_sid as bigint)<=" + statQC.EditYBEnd;
                }
            }
            for (int i = 0; i < statQC.typeList.Count; i++)
            {
                string type = statQC.typeList[i].ToString();
                if (!string.IsNullOrEmpty(statQC.GroupName))
                    statQC.GroupName += ",";
                if (!string.IsNullOrEmpty(statQC.Group))
                    statQC.Group += ",";
                switch (type)
                {
                    case "仪器":
                        statQC.GroupName += "Dict_itr_instrument.Ditr_ename 仪器";
                        statQC.Group += "Pat_lis_main.Pma_Ditr_id,Dict_itr_instrument.Ditr_ename ";
                        where += " and Dict_itr_instrument.Ditr_ename is not null";
                        statQC.Order += " order by Dict_itr_instrument.Ditr_ename";
                        break;
                    case "科别":
                        statQC.GroupName += "isnull(Dict_dept.Ddept_name,'未选科室') 科室";
                        statQC.Group += "Dict_dept.Ddept_name,Dict_dept.Ddept_id";
                        break;
                    case "标本":
                        statQC.GroupName += "isnull(Dict_sample.Dsam_name,'未选标本') 标本";
                        statQC.Group += "Dict_sample.Dsam_name,Dict_sample.Dsam_id";
                        break;
                    case "标识":
                        statQC.GroupName += "case Pat_lis_main.Pma_ctype when 1 then '常规' when 4 then '溶栓' when 2 then '急查' when 3 then '保密' else '未选标识' end 标识";
                        statQC.Group += "Pat_lis_main.Pma_ctype";
                        where += " and (Pat_lis_main.Pma_ctype is null or Pat_lis_main.Pma_ctype<> '')";
                        break;
                    case "实验组":
                        statQC.GroupName += "Dict_profession.Dpro_name 实验组";
                        statQC.Group += "Dict_profession.Dpro_id,Dict_profession.Dpro_name";
                        break;
                    case "录入者":
                        statQC.GroupName += "isnull(Base_user.Buser_name,'无录入人员') 录入者";
                        statQC.Group += "Base_user.Buser_id,Base_user.Buser_name";
                        break;
                    case "送检者":
                        statQC.GroupName += "isnull(Dict_doctor.Ddoctor_name,'无送检人员') 送检者";
                        statQC.Group += "Dict_doctor.Ddoctor_name,Dict_doctor.Ddoctor_id";
                        break;
                    case "来源":
                        statQC.GroupName += "isnull(Dict_source.Dsorc_name,'无来源') 来源";
                        statQC.Group += "Dict_source.Dsorc_name,Dict_source.Dsorc_id";
                        break;
                    case "标本ID":
                        statQC.GroupName += " Pat_lis_main.Lmsg_pid_name 姓名,dbo.getage(Pma_pat_age_exp) 年龄,Pat_lis_main.Pma_pat_in_no 标识ID";
                        statQC.Group += "Pat_lis_main.Pma_pat_name,Pma_pat_age_exp,Pma_pat_in_no";
                        break;
                    default:
                        return statQC;
                }
            }
            statQC.Where += where;
            return statQC;
        }


    }
}
