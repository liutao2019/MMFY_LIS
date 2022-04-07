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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicUtgentValue>))]
    public class DaoDicUtgentValue : IDaoDic<EntityDicUtgentValue>
    {
        public bool Save(EntityDicUtgentValue UtgentValue)
        {
            try
            {

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Diuv_Ditm_id", UtgentValue.UgtItmId);
                values.Add("Diuv_Dsam_id", UtgentValue.UgtSamId);
                values.Add("Diuv_Ddept_code", UtgentValue.UgtDepCode);
                values.Add("Diuv_sex", UtgentValue.UgtSex);
                values.Add("Diuv_age_h", UtgentValue.UgtAgeH);
                values.Add("Diuv_age_hunit", UtgentValue.UgtAgeHunit);
                values.Add("Diuv_age_l", UtgentValue.UgtAgeL);
                values.Add("Diuv_age_lunit", UtgentValue.UgtAgeLunit);
                values.Add("Diuv_pan_h", UtgentValue.UgtPanH);
                values.Add("Diuv_pan_l", UtgentValue.UgtPanL);
                values.Add("Diuv_max", UtgentValue.UgtMax);
                values.Add("Diuv_min", UtgentValue.UgtMin);
                values.Add("Diuv_ext_max", UtgentValue.ExtUgtMax);
                values.Add("Diuv_ext_min", UtgentValue.ExtUgtMin);
                values.Add("Diuv_icd_name", UtgentValue.UgtIcdName);
                values.Add("Diuv_desc", UtgentValue.UgtDesc);
                helper.InsertOperation("Dict_item_urgentvalue", values);


                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicUtgentValue UtgentValue)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Diuv_Ditm_id", UtgentValue.UgtItmId);
                values.Add("Diuv_Dsam_id", UtgentValue.UgtSamId);
                values.Add("Diuv_Ddept_code", UtgentValue.UgtDepCode);
                values.Add("Diuv_sex", UtgentValue.UgtSex);
                values.Add("Diuv_age_h", UtgentValue.UgtAgeH);
                values.Add("Diuv_age_hunit", UtgentValue.UgtAgeHunit);
                values.Add("Diuv_age_l", UtgentValue.UgtAgeL);
                values.Add("Diuv_age_lunit", UtgentValue.UgtAgeLunit);
                values.Add("Diuv_pan_h", UtgentValue.UgtPanH);
                values.Add("Diuv_pan_l", UtgentValue.UgtPanL);
                values.Add("Diuv_max", UtgentValue.UgtMax);
                values.Add("Diuv_min", UtgentValue.UgtMin);
                values.Add("Diuv_ext_max", UtgentValue.ExtUgtMax);
                values.Add("Diuv_ext_min", UtgentValue.ExtUgtMin);
                values.Add("Diuv_icd_name", UtgentValue.UgtIcdName);
                values.Add("Diuv_desc", UtgentValue.UgtDesc);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Diuv_id", UtgentValue.UgtKey);

                helper.UpdateOperation("Dict_item_urgentvalue", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicUtgentValue UtgentValue)
        {
            try
            {
                DBManager helper = new DBManager();
                string delStr = string.Format("delete from Dict_item_urgentvalue where Diuv_id='{0}'", UtgentValue.UgtKey);
                helper.ExecCommand(delStr);


                return true;

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicUtgentValue> Search(Object obj)
        {
            try
            {
                String sql = string.Empty;
                if (obj.ToString() == "0") {
                    sql = String.Format(@"select
Dict_item_urgentvalue.*
,case when Diuv_Dsam_id = '-1' then '默认'
else Dict_sample.Dsam_name end as ugt_sam_name
, Dict_itm.Ditm_ecode as ugt_itm_ecd
,case when Diuv_Ddept_code = '-1' then '默认'
else (select top 1 Ddept_name from Dict_dept where Dict_dept.Ddept_code = Dict_item_urgentvalue.Diuv_Ddept_code and(del_flag is null or del_flag = '0'))
end as ugt_dep_name
from
Dict_item_urgentvalue
left join Dict_sample on Dict_sample.Dsam_id = Dict_item_urgentvalue.Diuv_Dsam_id
left join Dict_itm on Dict_itm.Ditm_id = Dict_item_urgentvalue.Diuv_Ditm_id");
                }
                else
                {
                    sql = String.Format(@"select * from Dict_item_urgentvalue where Diuv_Ditm_id ='{0}'", obj.ToString());
                }

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntityDicUtgentValue>.ConvertToList(dt).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicUtgentValue>();
            }
        }

    }
}
