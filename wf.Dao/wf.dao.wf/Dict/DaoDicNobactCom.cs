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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicNobactCom>))]
    public class DaoDicNobactCom : IDaoDic<EntityDicNobactCom>
    {
        public bool Delete(EntityDicNobactCom nobactCom)
        {
            try
            {
                DBManager helper = new DBManager();


                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("nob_id", nobactCom.NobId);

                helper.DeleteOperation("dict_nobact_com", key);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDicNobactCom nobactCom)
        {
            try
            {

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("nob_id", nobactCom.NobId);
                values.Add("com_id", nobactCom.ComId);

                helper.InsertOperation("dict_nobact_com", values);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicNobactCom> Search(object obj)
        {
            try
            {
                string nobId = obj.ToString();

                String sql = string.Format(@"select * from dict_nobact_com where nob_id = {0}",nobId);
                if (obj != null && obj.ToString() == "Cache")
                {
                    sql= string.Format(@"select * from dict_nobact_com");
                }
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntityDicNobactCom>.ConvertToList(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicNobactCom>();
            }
        }

        public bool Update(EntityDicNobactCom nobactCom)
        {
            throw new NotImplementedException();
        }
    }
}
