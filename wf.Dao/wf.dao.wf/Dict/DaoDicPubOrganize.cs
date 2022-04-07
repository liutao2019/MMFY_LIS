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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicPubOrganize>))]
    public class DaoDicPubOrganize : IDaoDic<EntityDicPubOrganize>
    {
        public bool Save(EntityDicPubOrganize pubOrganize)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_organize");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dorg_id", id);
                values.Add("Dorg_code", pubOrganize.OrgCode);
                values.Add("Dorg_name", pubOrganize.OrgName);
                values.Add("Dorg_address", pubOrganize.OrgAddress);
                values.Add("Dorg_tel", pubOrganize.OrgTel);
                values.Add("sort_no", pubOrganize.OrgSortNo);
                values.Add("py_code", pubOrganize.OrgPyCode);
                values.Add("wb_code", pubOrganize.OrgWbCode);
                values.Add("del_flag", pubOrganize.OrgDelFlag);

                helper.InsertOperation("Dict_organize", values);

                pubOrganize.OrgId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicPubOrganize pubOrganize)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dorg_code", pubOrganize.OrgCode);
                values.Add("Dorg_name", pubOrganize.OrgName);
                values.Add("Dorg_address", pubOrganize.OrgAddress);
                values.Add("Dorg_tel", pubOrganize.OrgTel);
                values.Add("sort_no", pubOrganize.OrgSortNo);
                values.Add("py_code", pubOrganize.OrgPyCode);
                values.Add("wb_code", pubOrganize.OrgWbCode);
                values.Add("del_flag", pubOrganize.OrgDelFlag);


                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dorg_id", pubOrganize.OrgId);

                helper.UpdateOperation("Dict_organize", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicPubOrganize pubOrganize)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dorg_id", pubOrganize.OrgId);

                helper.UpdateOperation("Dict_organize", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicPubOrganize> Search(Object obj)
        {
            try
            {
                String sql = @"SELECT  * FROM Dict_organize WHERE del_flag='0' and Dorg_id<>'-1'";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicPubOrganize>();
            }
        }

        public List<EntityDicPubOrganize> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicPubOrganize> list = new List<EntityDicPubOrganize>();
            foreach (DataRow item in dtSour.Rows)
            {
                EntityDicPubOrganize pubOrganize = new EntityDicPubOrganize();

                pubOrganize.OrgId = item["Dorg_id"].ToString();
                pubOrganize.OrgCode = item["Dorg_code"].ToString();
                pubOrganize.OrgName = item["Dorg_name"].ToString();
                pubOrganize.OrgAddress = item["Dorg_address"].ToString();
                pubOrganize.OrgTel = item["Dorg_tel"].ToString();

                int sort = 0;
                if (item["sort_no"] != null && item["sort_no"] != DBNull.Value)
                    int.TryParse(item["sort_no"].ToString(), out sort);
                pubOrganize.OrgSortNo = sort;

                pubOrganize.OrgPyCode = item["py_code"].ToString();
                pubOrganize.OrgWbCode = item["wb_code"].ToString();
                pubOrganize.OrgDelFlag = item["del_flag"].ToString();

                list.Add(pubOrganize);
            }
            return list.OrderBy(i => i.OrgSortNo).ToList();
        }
    }
}
