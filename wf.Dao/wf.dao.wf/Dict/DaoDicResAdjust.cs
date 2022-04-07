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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicResAdjust>))]
    public class DaoDicResAdjust : IDaoDic<EntityDicResAdjust>
    {
        public bool Save(EntityDicResAdjust ResAdjust)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rira_Ditr_id", ResAdjust.ItrId);
                values.Add("Rira_mit_cno", ResAdjust.MitCno);
                values.Add("Rira_src_res", ResAdjust.SrcRes);
                values.Add("Rira_src_sid", ResAdjust.SrcSid);
                values.Add("Rira_multiple", ResAdjust.ResMultiple);
                values.Add("Rira_dec_place", ResAdjust.ResDecPlace);
                values.Add("Rira_dst_res", ResAdjust.DstRes);
                values.Add("Rira_dst_sid", ResAdjust.DstSid);

                helper.InsertOperation("Rel_itr_res_adjust", values);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicResAdjust ResAdjust)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rira_mit_cno", ResAdjust.MitCno);
                values.Add("Rira_src_res", ResAdjust.SrcRes);
                values.Add("Rira_src_sid", ResAdjust.SrcSid);
                values.Add("Rira_multiple", ResAdjust.ResMultiple);
                values.Add("Rira_dec_place", ResAdjust.ResDecPlace);
                values.Add("Rira_dst_res", ResAdjust.DstRes);
                values.Add("Rira_dst_sid", ResAdjust.DstSid);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rira_Ditr_id", ResAdjust.ItrId);

                helper.UpdateOperation("Rel_itr_res_adjust", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicResAdjust ResAdjust)
        {
            try
            {
                DBManager helper = new DBManager();

                string delStr = string.Empty;
                if (ResAdjust.MitCno != null && ResAdjust.MitCno != "")
                {
                    delStr = string.Format("delete from Rel_itr_res_adjust where Rira_Ditr_id='{0}' and Rira_mit_cno = '{1}'", ResAdjust.ItrId, ResAdjust.MitCno);
                }
                else
                {
                    delStr = string.Format("delete from Rel_itr_res_adjust where Rira_Ditr_id='{0}'", ResAdjust.ItrId);
                }
                helper.ExecCommand(delStr);


                return true;

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicResAdjust> Search(Object obj)
        {
            try
            {
                String sql = String.Format(@"SELECT Rira_id,Rira_Ditr_id,Rira_mit_cno,Rira_src_res,Rira_src_sid,Rira_multiple,Rira_dec_place,Rira_dst_res,Rira_dst_sid
FROM Rel_itr_res_adjust 
INNER JOIN Dict_itr_instrument ON Rel_itr_res_adjust.Rira_Ditr_id = Dict_itr_instrument.Ditr_id
WHERE (Dict_itr_instrument.del_flag = '0' and Rel_itr_res_adjust.Rira_Ditr_id = '{0}')", obj.ToString());
                List<string> list = obj as List<string>;
                if (obj != null && list != null)
                {
                    if (list[0].ToString() == "ResAdjust")
                    {
                        sql = string.Format(@"select * from Rel_itr_res_adjust where Rira_Ditr_id='{0}'
and Rira_mit_cno not in 
(select Rira_mit_cno from Rel_itr_res_adjust where Rira_Ditr_id='{1}' and Rira_Ditr_id is not null )", list[1], list[2]);
                    }
                }

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicResAdjust>();
            }
        }

        public List<EntityDicResAdjust> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicResAdjust> list = EntityManager<EntityDicResAdjust>.ConvertToList(dtSour);
            return list.OrderBy(i => i.ItrId).ToList();
        }
    }
}
