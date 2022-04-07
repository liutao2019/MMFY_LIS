
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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicItmCombine>))]
    public class DaoDicItmCombine : IDaoDic<EntityDicItmCombine>
    {
        public bool Save(EntityDicItmCombine ite)
        {
            throw new NotImplementedException();
        }

        public bool Update(EntityDicItmCombine ite)
        {
            throw new NotImplementedException();
        }

        public bool Delete(EntityDicItmCombine ite)
        {
            throw new NotImplementedException();
        }

        public List<EntityDicItmCombine> Search(Object obj)
        {
            List<EntityDicItmCombine> list = new List<EntityDicItmCombine>();
            try
            {
                string samWhere = "";
                if (obj!=null && !string.IsNullOrEmpty(((EntityDicCombine)obj).ComSamId))
                {
                    samWhere = string.Format("inner join  Rel_itm_sample on (Rel_itm_sample.Ritm_id=Dict_itm.Ditm_id and Ritm_sam_id='{0}')", ((EntityDicCombine)obj).ComSamId);
                }
                String sql = string.Format(@"SELECT  distinct '' Rici_Dcom_id,Dict_itm.Ditm_id Rici_Ditm_id,Dict_itm.Ditm_ecode Rici_Ditm_ecode,null Rici_flag, Dict_itm.Ditm_name,1 Rici_must_item,1 Rici_print_flag,0 sort_no,Dict_itm.Ditm_ecode, Dict_itm.py_code, 
                    Dict_itm.wb_code,Ditm_pri_id,IsNull((select top 1 Ritm_cost from Rel_itm_sample where Rel_itm_sample.Ritm_id=Dict_itm.Ditm_id),0)  Ritm_cost
                    ,IsNull((select top 1 Ritm_price from Rel_itm_sample where Rel_itm_sample.Ritm_id=Dict_itm.Ditm_id),0) Ritm_price
                    FROM Dict_itm   
                    left outer join Dict_profession on Dict_profession.Dpro_id = Dict_itm.Ditm_pri_id
                    {0}  
                    WHERE  Dict_itm.del_flag='0' ", samWhere);

                if (obj == null)
                {
                    sql = string.Format("select Rici_Dcom_id, Rici_Ditm_id from Rel_itm_combine_item");
                }
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                list = EntityManager<EntityDicItmCombine>.ConvertToList(dt).OrderBy(i => i.ItmSort).ToList(); ;

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;
        }
    }
}
