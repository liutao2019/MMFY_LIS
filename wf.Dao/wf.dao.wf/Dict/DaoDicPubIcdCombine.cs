using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.dao.wf.Dict
{
    [Export("wf.plugin.wf", typeof(interfaces.IDaoDic<EntityDicPubIcdCombine>))]
    public  class DaoDicPubIcdCombine : IDaoDic<EntityDicPubIcdCombine>
    {
        public bool Delete(EntityDicPubIcdCombine sample)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(sample.IcdId))
            {
                try
                {
                    DBManager helper = new DBManager();
                    string sql = string.Format("delete from Dict_icd_combine where Dicdc_Dicd_id = '{0}'", sample.IcdId);
                    helper.ExecSql(sql);
                   // CacheClient.ClearCache();
                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return result;
        }

        public bool Save(EntityDicPubIcdCombine sample)
        {
            try
            {
                DBManager manager = new DBManager();
                Dictionary<string, object> value = new Dictionary<string, object>();
                value.Add("Dicdc_Dicd_id", sample.IcdId);
                value.Add("Dicdc_Dcom_id", sample.ComId);
                value.Add("sort_no", sample.SortNo);
                manager.InsertOperation("Dict_icd_combine", value);
                return true;
            }catch(Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicPubIcdCombine> Search(object obj)
        {
            List<EntityDicPubIcdCombine> list = new List<EntityDicPubIcdCombine>();
            DBManager helper = new DBManager();
            if (obj!=null)
            {
                try
                {

                    string sql = string.Format(" select * from Dict_icd_combine where Dicdc_Dicd_id='{0}'", obj.ToString());
                    DataTable dtData = helper.ExecSel(sql);
                    list = EntityManager<EntityDicPubIcdCombine>.ConvertToList(dtData).OrderBy(i => i.SortNo).ToList();
                }
                catch (Exception ex)
                {

                    Lib.LogManager.Logger.LogException(ex);
                    throw;
                }
            }
            else
            {
                string sql = string.Format(" select * from Dict_icd_combine ");
                DataTable dtData = helper.ExecSel(sql);
                list = EntityManager<EntityDicPubIcdCombine>.ConvertToList(dtData).OrderBy(i => i.SortNo).ToList();
            }
            return list;
        }

        public bool Update(EntityDicPubIcdCombine sample)
        {
            throw new NotImplementedException();
        }
    }
}
