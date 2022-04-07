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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicItrCombine>))]
    public class DaoDicItrCombine : IDaoDic<EntityDicItrCombine>
    {
        public bool Save(EntityDicItrCombine combine)
        {
            try
            {
                //int id = IdentityHelper.GetMedIdentity("Def_itr_combine");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                //values.Add("itr_id", id);
                values.Add("Ric_Ditr_id", combine.ItrId);
                values.Add("Ric_Dcom_id", combine.CombineComId);
                values.Add("Ric_start_sid", combine.StartSid);
                values.Add("Ric_end_sid", combine.EndSid);

                helper.InsertOperation("Rel_itr_combine", values);

                //combine.ItrId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicItrCombine combine)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Ric_Dcom_id", combine.ComId);
                values.Add("Ric_start_sid", combine.StartSid);
                values.Add("Ric_end_sid", combine.EndSid);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Ric_Ditr_id", combine.ItrId);

                helper.UpdateOperation("Rel_itr_combine", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicItrCombine combine)
        {
            try
            {
                DBManager helper = new DBManager();

                ////Dictionary<string, object> values = new Dictionary<string, object>();
                ////values.Add("del_flag", "1");

                //Dictionary<string, object> keys = new Dictionary<string, object>();
                //keys.Add("itr_id", combine.ItrId);

                //helper.UpdateOperation("Def_itr_combine", values, keys);

                string sql = string.Format(@"delete Rel_itr_combine where Ric_Ditr_id='{0}' ", combine.ItrId);
                helper.ExecSql(sql);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicItrCombine> Search(Object obj)
        {
            try
            {
                string sqlStr;
                if (obj != null && obj.ToString() != "cache")
                {
                    EntityDicItrCombine info = obj as EntityDicItrCombine;
                    string itrID = info.ItrId;
                    if (info.IsNotIn)
                    {
                        sqlStr = string.Format(@"select Dict_itm_combine.Dcom_id,.Dict_itm_combine.Dcom_name,
Dict_itm_combine.Dcom_code,
Dict_itm_combine.py_code, --拼音码
Dict_itm_combine.wb_code, --五笔码
Dict_profession.Dpro_name,Dict_profession.Dpro_id,
cast(null as int) start_sid_notin,
cast(null as int) end_sid_notin
from Dict_itm_combine
left join Dict_profession on Dict_itm_combine.Dcom_lab_id = Dict_profession.Dpro_id 
AND Dict_profession.del_flag = '0'
where Dict_itm_combine.del_flag='0' ");
                        // and com_id not in (select com_id from Def_itr_combine where itr_id='{0}' and com_id is not null and com_id<>'') 
                                //, itrID);
                    }
                    else
                    {
                        sqlStr = string.Format(@"SELECT Dict_itm_combine.Dcom_id,Dict_itm_combine.Dcom_name,
                                      Dict_itm_combine.Dcom_code,
                                      Dict_itm_combine.py_code, --拼音码
                                      Dict_itm_combine.wb_code, --五笔码
                                      Dict_profession.Dpro_name,Dict_profession.Dpro_id,
                                      Rel_itr_combine.Ric_Ditr_id,
                                      Rel_itr_combine.Ric_Dcom_id,
                                      Rel_itr_combine.Ric_start_sid,
                                      Rel_itr_combine.Ric_end_sid
 From Rel_itr_combine 
   left join Dict_itm_combine on Rel_itr_combine.Ric_Dcom_id=Dict_itm_combine.Dcom_id  and Dict_itm_combine.del_flag='0'
    left join Dict_profession on Dict_itm_combine.Dcom_lab_id = Dict_profession.Dpro_id and Dict_profession.del_flag = '0'
   WHERE  Dict_itm_combine.del_flag='0' ");
    //Def_itr_combine.itr_id='{0}' and Def_itr_combine.itr_id<>'-1'"
                                            // , itrID);
                    }
          
                }
                else
                {
                    sqlStr = string.Format("select dic.Ric_Ditr_id, dic.Ric_Dcom_id from Rel_itr_combine dic");
                }

                if (obj != null && obj.ToString() == "cache")
                {
                    sqlStr = "select * from Rel_itr_combine";
                }
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sqlStr);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicItrCombine>();
            }
        }

        public List<EntityDicItrCombine> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicItrCombine> list = EntityManager<EntityDicItrCombine>.ConvertToList(dtSour);
            return list.ToList();
        }
    }
}
