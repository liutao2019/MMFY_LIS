using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Data;
using dcl.dao.core;
using dcl.common;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicPubDept>))]
    public class DaoDicPubDept : IDaoDic<EntityDicPubDept>
    {
        public List<EntityDicPubDept> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicPubDept> list = new List<EntityDicPubDept>();
            foreach (DataRow item in dtSour.Rows)
            {
                EntityDicPubDept depart = new EntityDicPubDept();
                depart.DeptId = item["Ddept_id"].ToString();
                depart.DeptName = item["Ddept_name"].ToString();
                depart.DeptCode = item["Ddept_code"].ToString();
                depart.DeptCCode = item["Ddept_c_code"].ToString();
                depart.DeptAccesPassword = item["Ddept_acces_pssword"].ToString();
                depart.DeptAccesType = item["Ddept_acces_type"].ToString();
                depart.DeptSource = item["Ddept_Dsorc_id"].ToString();
                depart.DeptShortName = item["Dept_shortname"].ToString();
                depart.DeptParentId = item["Ddept_parent_Ddept_id"].ToString();
                depart.DeptPyCode = item["py_code"].ToString();
                depart.DeptWbCode = item["wb_code"].ToString();

                int sort = 0;
                if (item["sort_no"] != null && item["sort_no"] != DBNull.Value)
                    int.TryParse(item["sort_no"].ToString(), out sort);
                depart.DeptSortNo = sort;

                depart.DeptDelFlag = item["del_flag"].ToString();
                depart.DeptOrgId = item["Ddept_select_code"].ToString();
                depart.DeptHospital = item["Ddept_Dorg_id"].ToString();
                depart.DeptTel = item["Ddept_tel"].ToString();

                depart.HosName = item["Dorg_name"].ToString();
                depart.OriName = item["Dsorc_name"].ToString();
                depart.Checked = false;

                list.Add(depart);
            }
            return list.OrderBy(i => i.DeptSortNo).ToList();
        }

        public bool Delete(EntityDicPubDept depart)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Ddept_id", depart.DeptId);

                helper.UpdateOperation("Dict_dept", values, key);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDicPubDept depart)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_dept");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Ddept_id", id);
                values.Add("Ddept_name", depart.DeptName);
                values.Add("Ddept_code", depart.DeptCode);
                values.Add("Ddept_c_code", depart.DeptCCode);
                values.Add("Ddept_acces_pssword", depart.DeptAccesPassword);
                values.Add("Ddept_acces_type", depart.DeptAccesType);
                values.Add("Ddept_Dsorc_id", depart.DeptSource);
                values.Add("Dept_shortname", depart.DeptShortName);
                values.Add("Ddept_parent_Ddept_id", depart.DeptParentId);
                values.Add("py_code", depart.DeptPyCode);
                values.Add("wb_code", depart.DeptWbCode);
                values.Add("sort_no", depart.DeptSortNo);
                values.Add("del_flag", depart.DeptDelFlag);
                values.Add("Ddept_select_code", depart.DeptOrgId);
                values.Add("Ddept_Dorg_id", depart.DeptHospital);
                values.Add("Ddept_tel", depart.DeptTel);

                helper.InsertOperation("Dict_dept", values);

                depart.DeptId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicPubDept> Search(Object obj)
        {
            try
            {
                string strSql = "";
                String sql = string.Format(@"SELECT Dict_dept.*,0 sp_select,Dict_source.Dsorc_name, Dict_dept.Ddept_Dorg_id,Dict_organize.Dorg_name 
from Dict_dept
left  join Dict_source on Dict_dept.Ddept_Dsorc_id=Dict_source.Dsorc_id 
left  join Dict_organize on Dict_organize.Dorg_id=Dict_dept.Ddept_Dorg_id 
where Dict_dept.del_flag=0 ");

                if (obj is string)
                {
                    strSql = BuildHospitalSqlWhere("Dict_dept.Ddept_Dorg_id ", obj.ToString());
                    if (!string.IsNullOrEmpty(strSql))
                    {
                        sql = string.Format(@"SELECT Dict_dept.*,0 sp_select,Dict_source.Dsorc_name, Dict_dept.Ddept_Dorg_id,Dict_organize.Dorg_name 
from Dict_dept
left  join Dict_source on Dict_dept.Ddept_Dsorc_id=Dict_source.Dsorc_id 
left  join Dict_organize on Dict_organize.Dorg_id=Dict_dept.Ddept_Dorg_id 
where Dict_dept.del_flag=0  '{0}'", strSql);
                    }
                }

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicPubDept>();
            }
        }

        public bool Update(EntityDicPubDept depart)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Ddept_name", depart.DeptName);
                values.Add("Ddept_code", depart.DeptCode);
                values.Add("Ddept_c_code", depart.DeptCCode);
                values.Add("Ddept_acces_pssword", depart.DeptAccesPassword);
                values.Add("Ddept_acces_type", depart.DeptAccesType);
                values.Add("Ddept_Dsorc_id", depart.DeptSource);
                values.Add("Dept_shortname", depart.DeptShortName);
                values.Add("Ddept_parent_Ddept_id", depart.DeptParentId);
                values.Add("py_code", depart.DeptPyCode);
                values.Add("wb_code", depart.DeptWbCode);
                values.Add("sort_no", depart.DeptSortNo);
                values.Add("del_flag", depart.DeptDelFlag);
                values.Add("Ddept_select_code", depart.DeptOrgId);
                values.Add("Ddept_Dorg_id", depart.DeptHospital);
                values.Add("Ddept_tel", depart.DeptTel);

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Ddept_id", depart.DeptId);

                helper.UpdateOperation("Dict_dept", values, key);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public static string BuildHospitalSqlWhere(string column, string hosID)
        {
            if (hosID == null || string.IsNullOrEmpty(hosID))
                return string.Empty;

            return string.Format(" and ({0}='{1}' or {0}='' or {0} is null) ", column, hosID);
        }
    }
}
