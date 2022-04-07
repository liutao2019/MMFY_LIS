/*  
 * 警告：
 * 本源代码所有权归广州慧扬健康科技有限公司(下称“本公司”)所有，已采取保密措施加以保护。  受《中华人民共和国刑法》、
 * 《反不正当竞争法》和《国家工商行政管理局关于禁止侵犯商业秘密行为的若干规定》等相关法律法规的保护。未经本公司书面
 * 许可，任何人披露、使用或者允许他人使用本源代码，必将受到相关法律的严厉惩罚。
 * Warning: 
 * The ownership of this source code belongs to Guangzhou Wisefly Technology Co., Ltd.(hereinafter referred to as "the company"), 
 * which is protected by the criminal law of the People's Republic of China, the anti unfair competition law and the 
 * provisions of the State Administration for Industry and Commerce on prohibiting the infringement of business secrets, etc. 
 * Without the written permission of the company, anyone who discloses, uses or allows others to use this source code 
 * will be severely punished by the relevant laws.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.common;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoReagentSetting))]
    public class DaoReaSetting : IDaoReagentSetting
    {

        public bool DeleteReaSetting(EntityReaSetting reaSetting)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Drea_id", reaSetting.Drea_id);

                helper.UpdateOperation("Dict_rea_setting", values, keys);


                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public EntityResponse SaveReaSetting(EntityReaSetting reaSetting)
        {
            EntityResponse result = new EntityResponse();
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_rea_setting");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Drea_id", id);
                values.Add("Drea_name", reaSetting.Drea_name);
                values.Add("Drea_package", reaSetting.Drea_package);
                values.Add("Drea_unit", reaSetting.Drea_unit);
                values.Add("Drea_product", reaSetting.Drea_product);
                values.Add("Drea_group", reaSetting.Drea_group);
                values.Add("Drea_position", reaSetting.Drea_position);
                values.Add("Drea_supplier", reaSetting.Drea_supplier);
                values.Add("Drea_condition", reaSetting.Drea_condition);
                values.Add("Drea_lower_limit", reaSetting.Drea_lower_limit);
                values.Add("Drea_upper_limit", reaSetting.Drea_upper_limit);
                values.Add("Drea_certificate", reaSetting.Drea_certificate);
                values.Add("Drea_tender", reaSetting.Drea_tender);
                values.Add("Drea_provincialno", reaSetting.Drea_provincialno);
                values.Add("Drea_method", reaSetting.Drea_method);
                values.Add("Drea_remark", reaSetting.Drea_remark);
                values.Add("Drea_printflag", reaSetting.Drea_printflag);
                values.Add("del_flag", reaSetting.del_flag);
                values.Add("py_code", reaSetting.py_code);
                values.Add("wb_code", reaSetting.wb_code);
                values.Add("sort_no", reaSetting.sort_no);
                values.Add("Drea_alarmdays", reaSetting.Drea_alarmdays);
                helper.InsertOperation("Dict_rea_setting", values);

                reaSetting.Drea_id = id.ToString();

                result.Scusess = true;
                result.SetResult(reaSetting);
                return result;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                result.Scusess = false;
                return result;
            }
        }

        public List<EntityReaSetting> SearchReaSettingAll()
        {
            try
            {
                String sql = @"select Dict_rea_setting.*,Dict_rea_unit.Runit_name,Dict_rea_supplier.Rsupplier_name,Dict_rea_product.Rpdt_name,Dict_rea_group.Rea_group,
Dict_rea_store_position.Rstr_position,Dict_rea_strcondition.Rstr_condition,Rel_rea_inventory.Rri_Count,Dict_rea_setting_barcode.Barcode from Dict_rea_setting
left join Dict_rea_unit on Dict_rea_unit.Runit_id = Drea_unit
left join Dict_rea_product on Dict_rea_product.Rpdt_id = Drea_product
left join Dict_rea_supplier on Dict_rea_supplier.Rsupplier_id = Drea_supplier
left join Dict_rea_group on Dict_rea_group.Rea_group_id = Drea_group
left join Dict_rea_store_position on Dict_rea_store_position.Rstr_position_id = Drea_position
left join Rel_rea_inventory on Rel_rea_inventory.Rri_Drea_id = Drea_id
left join Dict_rea_strcondition on Dict_rea_strcondition.Rstr_condition_id = Drea_condition
left join Dict_rea_setting_barcode on Dict_rea_setting_barcode.rea_id = Dict_rea_setting.Drea_id where Dict_rea_setting.del_flag=0
--and Dict_rea_unit.del_flag = 0 and Dict_rea_product.del_flag = 0 and Dict_rea_group.del_flag = 0 and Dict_rea_store_position.del_flag = 0
--and Dict_rea_strcondition.del_flag = 0";
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityReaSetting>();
            }
        }
        public List<EntityReaSetting> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityReaSetting> list = EntityManager<EntityReaSetting>.ConvertToList(dtSour);
            return list.OrderBy(i => i.Drea_id).ToList();
        }
        public bool UpdateReaSetting(EntityReaSetting reaSetting)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();

                values.Add("Drea_name", reaSetting.Drea_name);
                values.Add("Drea_package", reaSetting.Drea_package);
                values.Add("Drea_unit", reaSetting.Drea_unit);
                values.Add("Drea_product", reaSetting.Drea_product);
                values.Add("Drea_group", reaSetting.Drea_group);
                values.Add("Drea_position", reaSetting.Drea_position);
                values.Add("Drea_supplier", reaSetting.Drea_supplier);
                values.Add("Drea_condition", reaSetting.Drea_condition);
                values.Add("Drea_lower_limit", reaSetting.Drea_lower_limit);
                values.Add("Drea_upper_limit", reaSetting.Drea_upper_limit);
                values.Add("Drea_certificate", reaSetting.Drea_certificate);
                values.Add("Drea_tender", reaSetting.Drea_tender);
                values.Add("Drea_provincialno", reaSetting.Drea_provincialno);
                values.Add("Drea_method", reaSetting.Drea_method);
                values.Add("Drea_remark", reaSetting.Drea_remark);
                values.Add("Drea_printflag", reaSetting.Drea_printflag);
                values.Add("del_flag", reaSetting.del_flag);
                values.Add("py_code", reaSetting.py_code);
                values.Add("wb_code", reaSetting.wb_code);
                values.Add("sort_no", reaSetting.sort_no);
                values.Add("Drea_alarmdays", reaSetting.Drea_alarmdays);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Drea_id", reaSetting.Drea_id);

                helper.UpdateOperation("Dict_rea_setting", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }
        public bool UpdateBarcode(EntityReaSetting reaSetting)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();

                values.Add("Barcode", reaSetting.Barcode);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("rea_id", reaSetting.Drea_id);

                helper.UpdateOperation("Dict_rea_setting_barcode", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }
        public string GetReaBarcodeByID(string reaid)
        {
            try
            {
                String sql = @"select Barcode from Dict_rea_setting_barcode where rea_id = '{0}'";
                sql = string.Format(sql, reaid);
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecSel(sql);
                if (dt == null || dt.Rows.Count <= 0)
                    return string.Empty;
                return dt.Rows[0]["Barcode"].ToString();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return string.Empty;
            }
        }

        public int InsertBarcode(string barcode, string reaid)
        {
            try
            {
                String sql = @"INSERT INTO Dict_rea_setting_barcode (rea_id,Barcode) VALUES('{0}','{1}')";

                sql = string.Format(sql, reaid, barcode);
                DBManager helper = new DBManager();

                int i = helper.ExecCommand(sql);

                return i;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return 0;
            }
        }
    }
}
