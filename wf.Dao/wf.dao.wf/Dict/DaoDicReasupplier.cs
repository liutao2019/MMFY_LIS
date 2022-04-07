using dcl.dao.interfaces;
using dcl.entity;
using dcl.dao.core;
using dcl.common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace wf.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicReaSupplier>))]
    public class DaoDicReaSupplier : IDaoDic<EntityDicReaSupplier>
    {
        public bool Delete(EntityDicReaSupplier sample)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rsupplier_id", sample.Rsupplier_id);

                helper.UpdateOperation("Dict_rea_supplier", values, keys);


                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDicReaSupplier sample)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_rea_supplier");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rsupplier_id", id);
                values.Add("Rsupplier_name", sample.Rsupplier_name);
                values.Add("py_code", sample.py_code);
                values.Add("wb_code", sample.wb_code);
                values.Add("sort_no", sample.sort_no);
                values.Add("Rsupplier_postcode", sample.Rsupplier_postcode);
                values.Add("Rsupplier_address", sample.Rsupplier_address);
                values.Add("Rsupplier_contacts", sample.Rsupplier_contacts);
                values.Add("Rsupplier_email", sample.Rsupplier_email);
                values.Add("Rsupplier_phone", sample.Rsupplier_phone);
                values.Add("Rsupplier_post", sample.Rsupplier_post);
                values.Add("Rsupplier_website", sample.Rsupplier_website);
                helper.InsertOperation("Dict_rea_supplier", values);

                sample.Rsupplier_id = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicReaSupplier> Search(object obj)
        {
            try
            {
                String sql = "select * from Dict_rea_supplier where del_flag=0";
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicReaSupplier>();
            }
        }

        public bool Update(EntityDicReaSupplier sample)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rsupplier_name", sample.Rsupplier_name);
                values.Add("py_code", sample.py_code);
                values.Add("wb_code", sample.wb_code);
                values.Add("sort_no", sample.sort_no);
                values.Add("Rsupplier_postcode", sample.Rsupplier_postcode);
                values.Add("Rsupplier_address", sample.Rsupplier_address);
                values.Add("Rsupplier_contacts", sample.Rsupplier_contacts);
                values.Add("Rsupplier_email", sample.Rsupplier_email);
                values.Add("Rsupplier_phone", sample.Rsupplier_phone);
                values.Add("Rsupplier_post", sample.Rsupplier_post);
                values.Add("Rsupplier_website", sample.Rsupplier_website);
                values.Add("del_flag", sample.del_flag);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rsupplier_id", sample.Rsupplier_id);

                helper.UpdateOperation("Dict_rea_supplier", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicReaSupplier> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicReaSupplier> list = EntityManager<EntityDicReaSupplier>.ConvertToList(dtSour);
            return list.OrderBy(i => i.sort_no).ToList();
        }
    }
}
