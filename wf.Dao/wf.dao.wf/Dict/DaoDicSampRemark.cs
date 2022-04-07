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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicSampRemark>))]
    public class DaoDicSampRemark : IDaoDic<EntityDicSampRemark>
    {
        public List<EntityDicSampRemark> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicSampRemark> list = new List<EntityDicSampRemark>();
            foreach(DataRow item in dtSour.Rows)
            {
                EntityDicSampRemark samRemark = new EntityDicSampRemark();
                samRemark.RemId = item["Dsamr_id"].ToString();
                samRemark.RemContent = item["Dsamr_content"].ToString();
                samRemark.RemCCode = item["Dsamr_c_code"].ToString();
                samRemark.RemPyCode = item["py_code"].ToString();
                samRemark.RemWbCode = item["wb_code"].ToString();

                int sort = 0;
                if (item["sort_no"] != null && item["sort_no"] != DBNull.Value)
                    int.TryParse(item["sort_no"].ToString(), out sort);
                samRemark.RemSortNo = sort;

                samRemark.RemNewborn = item["Dsamr_newborn"].ToString();

                list.Add(samRemark);
            }
            return list.OrderBy(i => i.RemSortNo).ToList();
        }

        public bool Delete(EntityDicSampRemark samRemark)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Dsamr_id", samRemark.RemId);

                helper.DeleteOperation("Dict_sample_remark", key);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDicSampRemark samRemark)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_sample_remark");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dsamr_id", id);
                values.Add("Dsamr_content", samRemark.RemContent);
                values.Add("Dsamr_c_code", samRemark.RemCCode);
                values.Add("py_code", samRemark.RemPyCode);
                values.Add("wb_code", samRemark.RemWbCode);
                values.Add("sort_no", samRemark.RemSortNo);
                values.Add("Dsamr_newborn", samRemark.RemNewborn);
                
                helper.InsertOperation("Dict_sample_remark", values);

                samRemark.RemId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicSampRemark> Search(object obj)
        {
            try
            {
                String sql = @"select * from dbo.Dict_sample_remark";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicSampRemark>();
            }
        }

        public bool Update(EntityDicSampRemark samRemark)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dsamr_content", samRemark.RemContent);
                values.Add("Dsamr_c_code", samRemark.RemCCode);
                values.Add("py_code", samRemark.RemPyCode);
                values.Add("wb_code", samRemark.RemWbCode);
                values.Add("sort_no", samRemark.RemSortNo);
                values.Add("Dsamr_newborn", samRemark.RemNewborn);

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Dsamr_id", samRemark.RemId);

                helper.UpdateOperation("Dict_sample_remark", values, key);
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
