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

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicDoctor>))]
    public class DaoDicDoctor : IDaoDic<EntityDicDoctor>
    {
        public bool Save(EntityDicDoctor doctor)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_doctor");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Ddoctor_id", id);
                values.Add("Ddoctor_Ddept_id", doctor.DeptId);
                values.Add("Ddoctor_name", doctor.DoctorName);
                values.Add("Ddoctor_c_code", doctor.CCode);
                values.Add("Ddoctor_code", doctor.DoctorCode);
                values.Add("sort_no", doctor.SortNo);
                values.Add("py_code", doctor.PyCode);
                values.Add("wb_code", doctor.WbCode);
                values.Add("del_flag", doctor.DelFlag);
                values.Add("Ddoctor_tel", doctor.DoctorTel);
                values.Add("Ddoctor_hospital", doctor.DoctorHospital);

                helper.InsertOperation("Dict_doctor", values);

                doctor.DoctorId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicDoctor doctor)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Ddoctor_Ddept_id", doctor.DeptId);
                values.Add("Ddoctor_name", doctor.DoctorName);
                values.Add("Ddoctor_c_code", doctor.CCode);
                values.Add("Ddoctor_code", doctor.DoctorCode);
                values.Add("sort_no", doctor.SortNo);
                values.Add("py_code", doctor.PyCode);
                values.Add("wb_code", doctor.WbCode);
                values.Add("del_flag", doctor.DelFlag);
                values.Add("Ddoctor_tel", doctor.DoctorTel);
                values.Add("Ddoctor_hospital", doctor.DoctorHospital);


                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Ddoctor_id", doctor.DoctorId);

                helper.UpdateOperation("Dict_doctor", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicDoctor doctor)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Ddoctor_id", doctor.DoctorId);

                helper.UpdateOperation("Dict_doctor", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicDoctor> Search(Object obj)
        {
            try
            {
                String sql = @"SELECT Dict_doctor.Ddoctor_id, Dict_doctor.Ddoctor_Ddept_id, Dict_doctor.Ddoctor_name, 
Dict_doctor.Ddoctor_code, Dict_doctor.Ddoctor_c_code, Dict_doctor.py_code, 
Dict_doctor.wb_code,Dict_doctor.del_flag,Dict_doctor.sort_no,
Dict_doctor.Ddoctor_tel,Dict_doctor.Ddoctor_hospital,Dict_organize.Dorg_name,
Dict_dept.Ddept_name,Dict_dept.del_flag dept_flag_del
FROM Dict_doctor 
LEFT JOIN Dict_organize ON Dict_organize.Dorg_id=Dict_doctor.Ddoctor_hospital and Dict_organize.del_flag='0' 
LEFT OUTER JOIN Dict_dept  ON Dict_doctor.Ddoctor_Ddept_id = Dict_dept.Ddept_code and Dict_dept.del_flag='0'
WHERE Dict_doctor.del_flag='0' and Dict_doctor.Ddoctor_id<>'-1'";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicDoctor>();
            }
        }

        public List<EntityDicDoctor> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicDoctor> list = new List<EntityDicDoctor>();
            foreach (DataRow item in dtSour.Rows)
            {
                EntityDicDoctor doctor = new EntityDicDoctor();

                doctor.DoctorId = item["Ddoctor_id"].ToString();
                doctor.DeptId = item["Ddoctor_Ddept_id"].ToString();
                doctor.DoctorName = item["Ddoctor_name"].ToString();
                doctor.CCode = item["Ddoctor_c_code"].ToString();
                doctor.DoctorCode = item["Ddoctor_code"].ToString();

                int sort = 0;
                if (item["sort_no"] != null && item["sort_no"] != DBNull.Value)
                    int.TryParse(item["sort_no"].ToString(), out sort);
                doctor.SortNo = sort;

                doctor.PyCode = item["py_code"].ToString();
                doctor.WbCode = item["wb_code"].ToString();
                doctor.DelFlag = item["del_flag"].ToString();
                doctor.DoctorTel = item["Ddoctor_tel"].ToString();
                doctor.DoctorHospital = item["Ddoctor_hospital"].ToString();
                doctor.DoctorOrgName = item["Dorg_name"].ToString();
                doctor.DoctorDeptName = item["Ddept_name"].ToString();
                doctor.DoctorDeptDelFlag = item["dept_flag_del"].ToString();
                doctor.Checked = false;
                list.Add(doctor);
            }
            return list.OrderBy(i => i.SortNo).ToList();
        }
    }
}
