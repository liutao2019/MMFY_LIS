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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicCheckPurpose>))]
    public class DaoDicCheckPurpose : IDaoDic<EntityDicCheckPurpose>
    {
        public bool Save(EntityDicCheckPurpose check)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_check_purpose");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dpurp_id", id);
                values.Add("Dpurp_name", check.PurpName);
                values.Add("Dpurp_lab_id", check.LabId);
                values.Add("py_code", check.PyCode);
                values.Add("wb_code", check.WbCode);
                values.Add("del_flag", check.DelFlag);
                values.Add("sort_no", check.SortNo);
                values.Add("Dpurp_Dpro_id", check.ProId);
                values.Add("Dpurp_c_code", check.CCode);
                
                helper.InsertOperation("Dict_check_purpose", values);

                check.PurpId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicCheckPurpose check)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dpurp_name", check.PurpName);
                values.Add("Dpurp_lab_id", check.LabId);
                values.Add("py_code", check.PyCode);
                values.Add("wb_code", check.WbCode);
                values.Add("del_flag", check.DelFlag);
                values.Add("sort_no", check.SortNo);
                values.Add("Dpurp_Dpro_id", check.ProId);
                values.Add("Dpurp_c_code", check.CCode);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dpurp_id", check.PurpId);

                helper.UpdateOperation("Dict_check_purpose", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicCheckPurpose check)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dpurp_id", check.PurpId);

                helper.UpdateOperation("Dict_check_purpose", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicCheckPurpose> Search(Object obj)
        {
            try
            {
                String sql = @"SELECT  Dict_check_purpose.Dpurp_id,Dict_check_purpose.Dpurp_name,Dict_check_purpose.Dpurp_lab_id,Dict_check_purpose.py_code,
Dict_check_purpose.wb_code,Dict_check_purpose.del_flag,Dict_check_purpose.sort_no,Dict_check_purpose.Dpurp_Dpro_id,
Dict_check_purpose.Dpurp_c_code,Dict_profession.Dpro_name type_name,Dict_profession.Dpro_name ptype_name
FROM  Dict_check_purpose 
LEFT OUTER JOIN Dict_profession ON Dict_check_purpose.Dpurp_Dpro_id = Dict_profession.Dpro_id and Dict_profession.del_flag='0'
WHERE  Dict_check_purpose.del_flag ='0' and Dict_check_purpose.Dpurp_id<>'-1'";


                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicCheckPurpose>();
            }
        }

        public List<EntityDicCheckPurpose> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicCheckPurpose> list = new List<EntityDicCheckPurpose>();
            foreach (DataRow item in dtSour.Rows)
            {
                EntityDicCheckPurpose check = new EntityDicCheckPurpose();

                check.PurpId = item["Dpurp_id"].ToString();
                check.PurpName = item["Dpurp_name"].ToString();
                check.LabId = item["Dpurp_lab_id"].ToString();
                check.PyCode = item["py_code"].ToString();
                check.WbCode = item["wb_code"].ToString();
                check.DelFlag = item["del_flag"].ToString();
                
                int sort = 0;
                if (item["sort_no"] != null && item["sort_no"] != DBNull.Value)
                    int.TryParse(item["sort_no"].ToString(), out sort);
                check.SortNo = sort;

                check.ProId = item["Dpurp_Dpro_id"].ToString();
                check.CCode = item["Dpurp_c_code"].ToString();
                check.TypeName = item["type_name"].ToString(); 
                check.PTypeName = item["ptype_name"].ToString();

                list.Add(check);
            }
            return list.OrderBy(i => i.SortNo).ToList();
        }
    }
}
