using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicPubProfession>))]
    public class DaoDicPubProfession : IDaoDic<EntityDicPubProfession>
    {
        public bool Save(EntityDicPubProfession type)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_profession");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dpro_id", id);
                values.Add("Dpro_name", type.ProName);
                values.Add("Dpro_remark", type.ProRemark);
                values.Add("Dpro_type", type.ProType);
                values.Add("py_code", type.ProPyCode);
                values.Add("wb_code", type.ProWbCode);
                values.Add("sort_no", type.ProSortNo);
                values.Add("del_flag", type.ProDelFlag);
                values.Add("Dpro_Dorg_id", type.ProOrgId);
                values.Add("Dpro_link", type.ProTypeLink);
                helper.InsertOperation("Dict_profession", values);

                type.ProId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicPubProfession type)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dpro_name", type.ProName);
                values.Add("Dpro_remark", type.ProRemark);
                values.Add("Dpro_type", type.ProType);
                values.Add("py_code", type.ProPyCode);
                values.Add("wb_code", type.ProWbCode);
                values.Add("sort_no", type.ProSortNo);
                values.Add("del_flag", type.ProDelFlag);
                values.Add("Dpro_Dorg_id", type.ProOrgId);
                values.Add("Dpro_link", type.ProTypeLink);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dpro_id", type.ProId);

                helper.UpdateOperation("Dict_profession", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicPubProfession type)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dpro_id", type.ProId);

                helper.UpdateOperation("Dict_profession", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }


        public List<EntityDicPubProfession> Search(Object obj)
        {
            try
            {
               string sql = @"select Dict_profession.*,pro_pro_name=case  when Dict_profession.Dpro_type=1 then '物理组' else '专业组' end,
ISNULL(Dorg_name,'无') as Dorg_name ,ptype.Dpro_name as ptype_name,ptype.Dpro_name as ctype_name,
Dict_profession.Dpro_link as ctype_id
from Dict_profession 
left join Dict_organize on Dict_profession.Dpro_Dorg_id=Dict_organize.Dorg_id 
left join Dict_profession as ptype on Dict_profession.Dpro_link=ptype.Dpro_id
where Dict_profession.del_flag='0' ";
                if (obj is string)
                {
                    string  strSql = BuildTypeHospitalSqlWithAnd(obj.ToString());
                    if (!string.IsNullOrEmpty(strSql))
                    {
                        sql += strSql;
                    }
                }

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicPubProfession>();
            }
        }

        public List<EntityDicPubProfession> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicPubProfession> list = new List<EntityDicPubProfession>();
            foreach (DataRow item in dtSour.Rows)
            {
                EntityDicPubProfession type = new EntityDicPubProfession();

                type.ProId = item["Dpro_id"].ToString();
                type.ProName = item["Dpro_name"].ToString();
                type.ProRemark = item["Dpro_remark"].ToString();

                type.ProPyCode = item["py_code"].ToString();
                type.ProWbCode = item["wb_code"].ToString();
                type.ProOrgId = item["Dpro_Dorg_id"].ToString();
                type.ProTypeName = item["pro_pro_name"].ToString();
                type.ProOrgName = item["Dorg_name"].ToString();
                type.ProTypeLink = item["Dpro_link"].ToString();
                type.ProPtypeName = item["ptype_name"].ToString();
                type.ProCtypeName = item["ctype_name"].ToString();
                type.ProCtypeId = item["ctype_id"].ToString();
                int sort = 0, protype = 0;
                if (item["sort_no"] != null && item["sort_no"] != DBNull.Value)
                    int.TryParse(item["sort_no"].ToString(), out sort);
                type.ProSortNo = sort;
                if (item["Dpro_type"] != null && item["Dpro_type"] != DBNull.Value)
                    int.TryParse(item["Dpro_type"].ToString(), out protype);
                type.ProType = protype;
                type.ProDelFlag = item["del_flag"].ToString();
                type.Checked = false;

                list.Add(type);
            }
            return list.OrderBy(i => i.ProSortNo).ToList();
        }

        public string BuildTypeHospitalSqlWithAnd(string hosID)
        {
            if (hosID == null || string.IsNullOrEmpty(hosID))
                return string.Empty;

            return
                string.Format(
                    " and (Dict_profession.Dpro_Dorg_id='{0}' or Dict_profession.Dpro_Dorg_id='' or Dict_profession.Dpro_Dorg_id is null)  ", hosID);
        }
    }
}
