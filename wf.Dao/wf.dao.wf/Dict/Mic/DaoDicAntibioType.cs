
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using dcl.common;
using System.ComponentModel.Composition;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicAntibioType>))]
    public class DaoDicAntibioType : IDaoDic<EntityDicAntibioType>
    {
        public List<EntityDicAntibioType> Search(Object obj)
        {

            try
            {
                string sql = @"SELECT * FROM Dict_antibio_type";
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(sql);
                List<EntityDicAntibioType> list = EntityManager<EntityDicAntibioType>.ToList(dt);
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicAntibioType>();
            }
        }

        public List<EntityDicAntibioType> ConvertToEntitys(DataTable dtSour)
        {
            throw new NotImplementedException();
        }

        public bool Save(EntityDicAntibioType sample)
        {
            bool isSave = false;
            if (sample != null)
            {
                try
                {
                    int id = IdentityHelper.GetMedIdentity("Dict_antibio_type");
                    DBManager helper = new DBManager();
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values.Add("Dtp_id", id);
                    // values.Add("tp_seq", sample.DelFlag);
                    values.Add("Dtp_code", sample.TpCode);
                    values.Add("Dtp_cname", sample.TpCName);
                    values.Add("py_code", sample.TpPY);
                    values.Add("wb_code", sample.TpWB);
                    values.Add("del_flag", sample.DelFlag);

                    helper.InsertOperation("Dict_antibio_type", values);
                    sample.TpID = id.ToString();
                    isSave = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    throw;
                }
            }
            return isSave;
        }

        public bool Update(EntityDicAntibioType sample)
        {
            bool isUpdate = false;
            if (sample != null)
            {

                try
                {
                    DBManager helper = new DBManager();
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    //values.Add("qab_id", sample.QabId);
                    values.Add("Dtp_cname", sample.TpCName);
                    values.Add("sort_no", sample.TpSeq);
                    values.Add("Dtp_code", sample.TpCode);
                    values.Add("py_code", sample.TpPY);
                    values.Add("wb_code", sample.TpWB);
                    values.Add("del_flag", sample.DelFlag);

                    Dictionary<string, object> keys = new Dictionary<string, object>();
                    keys.Add("Dtp_id", sample.TpID);
                    helper.UpdateOperation("Dict_antibio_type", values, keys);
                    isUpdate = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return isUpdate;
        }

        public bool Delete(EntityDicAntibioType sample)
        {
            bool isDelete = false;
            if (sample != null)
            {
                try
                {
                    DBManager helper = new DBManager();

                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values.Add("del_flag", "1");

                    Dictionary<string, object> keys = new Dictionary<string, object>();
                    keys.Add("Dtp_id", sample.TpID);

                    helper.UpdateOperation("Dict_antibio_type", values, keys);

                    isDelete = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    throw;
                }
            }
            return isDelete;

        }
    }
}
