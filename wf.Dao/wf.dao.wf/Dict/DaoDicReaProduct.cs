using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.dao.core;
using dcl.common;
using System.ComponentModel.Composition;

namespace wf.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicReaProduct>))]
    public class DaoDicReaProduct : IDaoDic<EntityDicReaProduct>
    {
        public bool Delete(EntityDicReaProduct sample)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rpdt_id", sample.Rpdt_id);

                helper.UpdateOperation("Dict_rea_product", values, keys);


                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDicReaProduct sample)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_rea_product");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rpdt_id", id);
                values.Add("Rpdt_name", sample.Rpdt_name);
                values.Add("py_code", sample.py_code);
                values.Add("wb_code", sample.wb_code);
                values.Add("sort_no", sample.sort_no);
                values.Add("Rpdt_postcode", sample.Rpdt_postcode);
                values.Add("Rpdt_address", sample.Rpdt_address);
                values.Add("Rpdt_contacts", sample.Rpdt_contacts);
                values.Add("Rpdt_email", sample.Rpdt_email);
                values.Add("Rpdt_phone", sample.Rpdt_phone);
                values.Add("Rpdt_post", sample.Rpdt_post);
                values.Add("Rpdt_website", sample.Rpdt_website);
                helper.InsertOperation("Dict_rea_product", values);

                sample.Rpdt_id = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicReaProduct> Search(object obj)
        {
            try
            {
                String sql = "select * from Dict_rea_product where del_flag=0";
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicReaProduct>();
            }
        }

        public bool Update(EntityDicReaProduct sample)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rpdt_name", sample.Rpdt_name);
                values.Add("py_code", sample.py_code);
                values.Add("wb_code", sample.wb_code);
                values.Add("sort_no", sample.sort_no);
                values.Add("Rpdt_postcode", sample.Rpdt_postcode);
                values.Add("Rpdt_address", sample.Rpdt_address);
                values.Add("Rpdt_contacts", sample.Rpdt_contacts);
                values.Add("Rpdt_email", sample.Rpdt_email);
                values.Add("Rpdt_phone", sample.Rpdt_phone);
                values.Add("Rpdt_post", sample.Rpdt_post);
                values.Add("Rpdt_website", sample.Rpdt_website);
                values.Add("del_flag", sample.del_flag);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rpdt_id", sample.Rpdt_id);

                helper.UpdateOperation("Dict_rea_product", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicReaProduct> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicReaProduct> list = EntityManager<EntityDicReaProduct>.ConvertToList(dtSour);
            return list.OrderBy(i => i.sort_no).ToList();
        }
    }
}
