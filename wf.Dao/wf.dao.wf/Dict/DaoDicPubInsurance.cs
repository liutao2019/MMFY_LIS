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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicPubInsurance>))]
    public class DaoDicPubInsurance : IDaoDic<EntityDicPubInsurance>
    {
        public List<EntityDicPubInsurance> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicPubInsurance> list = new List<EntityDicPubInsurance>();
            foreach(DataRow item in dtSour.Rows)
            {
                EntityDicPubInsurance feesType = new EntityDicPubInsurance();
                feesType.FeesTypeId = item["Dinsu_id"].ToString();
                feesType.FeesTypeName = item["Dinsu_name"].ToString();
                feesType.FeesTypePyCode = item["py_code"].ToString();
                feesType.FeesTypeWbCode = item["wb_code"].ToString();
                feesType.FeesTypeDelFlag = item["del_flag"].ToString();

                int sort = 0;
                if (item["sort_no"] != null && item["sort_no"] != DBNull.Value)
                    int.TryParse(item["sort_no"].ToString(), out sort);
                feesType.FeesTypeSortNo = sort;

                list.Add(feesType);
            }
            return list.OrderBy(i => i.FeesTypeSortNo).ToList();
        }

        public bool Delete(EntityDicPubInsurance feesType)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Dinsu_id", feesType.FeesTypeId);

                helper.UpdateOperation("Dict_insurance", values, key);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDicPubInsurance feesType)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_insurance");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dinsu_id", id);
                values.Add("Dinsu_name", feesType.FeesTypeName);
                values.Add("py_code", feesType.FeesTypePyCode);
                values.Add("wb_code", feesType.FeesTypeWbCode);
                values.Add("sort_no", feesType.FeesTypeSortNo);
                values.Add("del_flag", feesType.FeesTypeDelFlag);

                helper.InsertOperation("Dict_insurance", values);

                feesType.FeesTypeId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicPubInsurance> Search(object obj)
        {
            try
            {
                String sql = @"select * from Dict_insurance where del_flag is null or del_flag<>1";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicPubInsurance>();
            }
        }

        public bool Update(EntityDicPubInsurance feesType)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dinsu_name", feesType.FeesTypeName);
                values.Add("py_code", feesType.FeesTypePyCode);
                values.Add("wb_code", feesType.FeesTypeWbCode);
                values.Add("sort_no", feesType.FeesTypeSortNo);
                values.Add("del_flag", feesType.FeesTypeDelFlag);

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Dinsu_id", feesType.FeesTypeId);

                helper.UpdateOperation("Dict_insurance", values, key);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }
    }
}
