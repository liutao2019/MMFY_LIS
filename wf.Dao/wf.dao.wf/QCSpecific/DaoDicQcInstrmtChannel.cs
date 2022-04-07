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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicQcInstrmtChannel>))]
    public class DaoDicQcInstrmtChannel : IDaoDic<EntityDicQcInstrmtChannel>
    {
        public bool Save(EntityDicQcInstrmtChannel channel)
        {
            try
            {
                DBManager helper = new DBManager();
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rchan_Ditr_id", channel.ItrIdNew);
                values.Add("Rchan_Ditm_id", channel.ItmId);
                values.Add("Rchan_level", channel.MatLevelNew);
                values.Add("Rchan_type", channel.ChannelTypeNew);

                values.Add("Rchan_sid_ident", channel.SidIdent);
                values.Add("Rchan_batch_ident", channel.BatchIdent);
                values.Add("Rchan_batch_no", channel.MatBatchNoNew);
                values.Add("Rchan_Dmat_id", channel.MatIdNew);
                values.Add("Rchan_Rqrt_id", channel.MatTimeId);

                helper.InsertOperation("Rel_qc_instrmt_channel", values);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicQcInstrmtChannel channel)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();

                values.Add("Rchan_Ditr_id", channel.ItrIdNew);
                values.Add("Rchan_Ditm_id", channel.ItmId);
                values.Add("Rchan_level", channel.MatLevelNew);
                values.Add("Rchan_type", channel.ChannelTypeNew);

                values.Add("Rchan_sid_ident", channel.SidIdent);
                values.Add("Rchan_batch_ident", channel.BatchIdent);
                values.Add("Rchan_batch_no", channel.MatBatchNoNew);
                values.Add("Rchan_Dmat_id", channel.MatIdNew);
                values.Add("Rchan_Rqrt_id", channel.MatTimeId);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rchan_id", channel.ChannelSn);

                helper.UpdateOperation("Rel_qc_instrmt_channel", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex); 
                return false;
            }
        }

        public bool Delete(EntityDicQcInstrmtChannel channel)
        {
            try
            {
                DBManager helper = new DBManager();
                string sql = "";
                if (channel != null)
                {
                    sql = string.Format(@"delete Rel_qc_instrmt_channel where Rchan_Ditr_id='{0}' and Rchan_Rqrt_id='{1}' ", channel.ItrId, channel.MatTimeId);
                }

                helper.ExecSql(sql);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicQcInstrmtChannel> Search(Object obj)
        {
            List<EntityDicQcInstrmtChannel> listChannel = new List<EntityDicQcInstrmtChannel>();

            EntityDicQcInstrmtChannel channel = obj as EntityDicQcInstrmtChannel;
            if (channel != null)
            {
                try
                {
                    String sql = string.Format(@"select detail.Dmat_id mat_id_new,  
detail.Dmat_Ditr_id itr_id_new, 
detail.Dmat_batch_no mat_batch_no_new, 
detail.Dmat_level mat_level_new, 
isnull(inter.Rchan_type,'0') channel_type_new,
inter.Rchan_sid_ident,
inter.Rchan_Rqrt_id
--,(case inter.channel_type when '0' then '样本号' when '1' then '质控标识' else  '' end) channel_type_name
from dbo.Dict_qc_materia detail
left join Rel_qc_instrmt_channel inter on inter.Rchan_Dmat_id=detail.Dmat_id  
and inter.Rchan_Ditr_id=detail.Dmat_Ditr_id
where detail.Dmat_Ditr_id='{0}' and detail.Dmat_date_end > getdate() and
(inter.Rchan_Rqrt_id ='{1}' or inter.Rchan_Rqrt_id is null) "
                                             , channel.ItrIdNew, channel.MatTimeId);

                    DBManager helper = new DBManager();

                    DataTable dt = helper.ExecuteDtSql(sql);

                    listChannel = EntityManager<EntityDicQcInstrmtChannel>.ConvertToList(dt).ToList();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return listChannel;
        }

    }
}
