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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicMachineCode>))]
    public class DaoDicMachineCode : IDaoDic<EntityDicMachineCode>
    {
        public bool Save(EntityDicMachineCode code)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Rel_itr_channel_code");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Ricc_id", id);

                values.Add("Ricc_Ditr_id", code.ItrId);
                values.Add("Ricc_code", code.MacCode);
                values.Add("Ricc_Ditm_id", code.MacItmId);
                values.Add("Ricc_dec_place", code.MacDecPlace);
                values.Add("Ricc_position", code.MacPosition);
                values.Add("Ricc_type", code.MacType);
                values.Add("Ricc_res_len", code.MacResLen);
                values.Add("del_flag", code.DelFlag);
                values.Add("Ricc_itm_ecd", code.MacItmEcd);
                values.Add("Ricc_flag", code.MacFlag);
                values.Add("Ricc_receive_flag", code.MacReceiveFlag);

                helper.InsertOperation("Rel_itr_channel_code", values);

                code.MacId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicMachineCode code)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Ricc_Ditr_id", code.ItrId);
                values.Add("Ricc_code", code.MacCode);
                values.Add("Ricc_Ditm_id", code.MacItmId);
                values.Add("Ricc_dec_place", code.MacDecPlace);
                values.Add("Ricc_position", code.MacPosition);
                values.Add("Ricc_type", code.MacType);
                values.Add("Ricc_res_len", code.MacResLen);
                values.Add("del_flag", code.DelFlag);
                values.Add("Ricc_itm_ecd", code.MacItmEcd);
                values.Add("Ricc_flag", code.MacFlag);
                values.Add("Ricc_receive_flag", code.MacReceiveFlag);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Ricc_id", code.MacId);

                helper.UpdateOperation("Rel_itr_channel_code", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicMachineCode code)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Ricc_id", code.MacId);

                helper.UpdateOperation("Rel_itr_channel_code", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicMachineCode> Search(Object obj)
        {
            List<object> List = obj as List<object>;
            try
            {
                String sql = @"SELECT
Rel_itr_channel_code.Ricc_id, 
Rel_itr_channel_code.Ricc_code,
Rel_itr_channel_code.Ricc_dec_place,
Rel_itr_channel_code.Ricc_position,
Rel_itr_channel_code.Ricc_res_len,
Rel_itr_channel_code.Ricc_type, 
Rel_itr_channel_code.del_flag,
Rel_itr_channel_code.Ricc_Ditr_id,
Rel_itr_channel_code.Ricc_itm_ecd,
Rel_itr_channel_code.Ricc_Ditm_id,
Rel_itr_channel_code.Ricc_flag,

Rel_itr_channel_code.Ricc_receive_flag,

Dict_itr_instrument.Ditr_ename,
Dict_itr_instrument.del_flag instrument_del_flag,
Dict_itr_instrument.Ditr_name,

Dict_itm.Ditm_name, 
Dict_itm.del_flag item_del_flag,
Dict_itm.Ditm_ecode 
FROM Rel_itr_channel_code
LEFT OUTER JOIN Dict_itr_instrument ON Rel_itr_channel_code.Ricc_Ditr_id = Dict_itr_instrument.Ditr_id AND Dict_itr_instrument.del_flag = '0'
Left join Dict_itm on Dict_itm.Ditm_id = Rel_itr_channel_code.Ricc_Ditm_id 
WHERE Rel_itr_channel_code.del_flag = '0' and Rel_itr_channel_code.Ricc_id<>'-1'";

                if (obj != null)
                {
                    sql = string.Format(@"select * from dbo.Rel_itr_channel_code where Ricc_Ditr_id='{0}' and del_flag='0'
and Ricc_Ditm_id not in
(select Ricc_Ditm_id from dbo.Rel_itr_channel_code where Ricc_Ditr_id='{1}' and Ricc_Ditm_id is not null and del_flag='0' )",
                                             List[0], List[1]);
                }

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicMachineCode>();
            }
        }

        public List<EntityDicMachineCode> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicMachineCode> list = EntityManager<EntityDicMachineCode>.ConvertToList(dtSour);

            return list.ToList();
        }
    }
}
