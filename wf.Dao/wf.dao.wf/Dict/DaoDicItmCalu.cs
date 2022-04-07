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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicItmCalu>))]
    public class DaoDicItmCalu : IDaoDic<EntityDicItmCalu>
    {
        public bool Save(EntityDicItmCalu ItmCalu)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Rel_itm_calculaformula");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rcalfor_id", id);
                values.Add("Rcalfor_Ditm_id", ItmCalu.ItmId);
                values.Add("Rcalfor_expression", ItmCalu.CalExpression);
                values.Add("sort_no", ItmCalu.SortNo);
                values.Add("Rcalfor_flag", ItmCalu.CalFlag);
                values.Add("Rcalfor_variable", ItmCalu.CalVariable);
                values.Add("Rcalfor_name", ItmCalu.CalName);
                values.Add("Rcalfor_supportitrcal", ItmCalu.CalSupportItrCal);
                values.Add("Rcalfor_Ditr_id", ItmCalu.CalItrId);
                values.Add("Rcalfor_special", ItmCalu.CalSpFormula);

                helper.InsertOperation("Rel_itm_calculaformula", values);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicItmCalu ItmCalu)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rcalfor_Ditm_id", ItmCalu.ItmId);
                values.Add("Rcalfor_expression", ItmCalu.CalExpression);
                values.Add("sort_no", ItmCalu.SortNo);
                values.Add("Rcalfor_flag", ItmCalu.CalFlag);
                values.Add("Rcalfor_variable", ItmCalu.CalVariable);
                values.Add("Rcalfor_name", ItmCalu.CalName);
                values.Add("Rcalfor_supportitrcal", ItmCalu.CalSupportItrCal);
                values.Add("Rcalfor_Ditr_id", ItmCalu.CalItrId);
                values.Add("Rcalfor_special", ItmCalu.CalSpFormula);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rcalfor_id", ItmCalu.CalId);

                helper.UpdateOperation("Rel_itm_calculaformula", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicItmCalu ItmCalu)
        {
            try
            {
                DBManager helper = new DBManager();

                string delStr = string.Format("delete from Rel_itm_calculaformula where Rcalfor_id='{0}'", ItmCalu.CalId);
                helper.ExecCommand(delStr);


                return true;

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicItmCalu> Search(Object obj)
        {
            try
            {
                String sql = String.Format(@"SELECT Rel_itm_calculaformula.*,
Ditm_name,Ditm_ecode,Dict_itr_instrument.Ditr_name cal_itr_name,Dict_itr_instrument.Ditr_ename
from Rel_itm_calculaformula 
left join Dict_itm on Rel_itm_calculaformula.Rcalfor_Ditm_id=Dict_itm.Ditm_id  
left join Dict_profession on Dict_itm.Ditm_pri_id=Dict_profession.Dpro_id
left join Dict_itr_instrument on  Dict_itr_instrument.Ditr_id = Rel_itm_calculaformula.Rcalfor_Ditr_id");

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicItmCalu>();
            }
        }

        public List<EntityDicItmCalu> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicItmCalu> list = new List<EntityDicItmCalu>();
            foreach (DataRow item in dtSour.Rows)
            {
                EntityDicItmCalu value = new EntityDicItmCalu();

                value.CalId = item["Rcalfor_id"].ToString();
                value.ItmId = item["Rcalfor_Ditm_id"].ToString();
                value.CalExpression = item["Rcalfor_expression"].ToString();
                if (item["sort_no"] != DBNull.Value)
                {
                    value.SortNo = Convert.ToInt32(item["sort_no"].ToString());
                }
                if (item["Rcalfor_flag"] != DBNull.Value)
                {
                    value.CalFlag = Convert.ToInt32(item["Rcalfor_flag"].ToString());
                }
                value.CalVariable = item["Rcalfor_variable"].ToString();
                value.CalName = item["Rcalfor_name"].ToString();
                value.CalSupportItrCal = item["Rcalfor_supportitrcal"].ToString();
                value.CalItrId = item["Rcalfor_Ditr_id"].ToString();
                value.CalSpFormula = item["Rcalfor_special"].ToString();

                value.CalItrName = item["cal_itr_name"].ToString();
                value.ItmName = item["Ditm_name"].ToString();
                value.ItmEcode = item["Ditm_ecode"].ToString();
                value.ItrEname = item["Ditr_ename"].ToString();

                list.Add(value);
            }
            return list.OrderBy(i => i.CalId).ToList();
        }
    }
}
