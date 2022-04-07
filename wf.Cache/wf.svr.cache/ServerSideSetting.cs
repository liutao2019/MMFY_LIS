using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace dcl.svr.cache
{
    public class ServerCacheConfig
    {
        public static string GetHospitalID()
        {
            return ConfigurationManager.AppSettings["HospitalID"];
        }

        public static string BuildTypeHospitalSqlWithWhere()
        {
            string hosID = ConfigurationManager.AppSettings["HospitalID"];
            if (hosID == null || string.IsNullOrEmpty(hosID))
                return string.Empty;

            return
                string.Format(
                    " where (dict_type.tyep_hospitalName='{0}' or dict_type.tyep_hospitalName='' or dict_type.tyep_hospitalName is null)  ", hosID);
        }

        public static string BuildTypeHospitalSqlWithAnd()
        {
            string hosID = ConfigurationManager.AppSettings["HospitalID"];
            if (hosID == null || string.IsNullOrEmpty(hosID))
                return string.Empty;

            return
                string.Format(
                    " and (dict_type.tyep_hospitalName='{0}' or dict_type.tyep_hospitalName='' or dict_type.tyep_hospitalName is null)  ", hosID);
        }


        public static string BuildTypeHospitalSqlLeftJoinType(string tableName)
        {
            string hosID = ConfigurationManager.AppSettings["HospitalID"];
            if (hosID == null || string.IsNullOrEmpty(hosID))
                return string.Empty;

            return string.Format(" LEFT OUTER JOIN dict_type ON {0}.itr_type = dict_type.[type_id] AND dict_type.type_del = '0'  ", tableName);
        }

        public static string BuildHospitalSqlWhere(string column)
        {
            string hosID = ConfigurationManager.AppSettings["HospitalID"];
            if (hosID == null || string.IsNullOrEmpty(hosID))
                return string.Empty;

            return string.Format(" and ({0}='{1}' or {0}='' or {0} is null) ", column, hosID);
        }

    }

    public class ServerSideSetting
    {
        public static string FirstAuditDisplayWord
        {
            get
            {
                string text = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("AuditWord");

                if (string.IsNullOrEmpty(text))
                {
                    text = "报告";
                }

                return text;
            }
        }

        public static string SecondAuditDisplayWord
        {
            get
            {
                string text = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportWord");

                if (string.IsNullOrEmpty(text))
                {
                    text = "审核";
                }

                return text;
            }
        }
    }
}
