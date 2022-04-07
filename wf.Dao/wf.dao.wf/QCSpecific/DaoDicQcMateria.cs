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
    [Export("wf.plugin.wf", typeof(IDaoQcMateria))]
    public class DaoDicQcMateria : IDaoQcMateria
    {
        public EntityResponse SaveQcMateria(EntityDicQcMateria qcMateria)
        {
            EntityResponse result = new EntityResponse();
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_qc_materia");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dmat_id", id);
                values.Add("Dmat_Ditr_id", qcMateria.MatId);
                values.Add("Dmat_level", qcMateria.MatLevel);
                values.Add("Dmat_c_no", qcMateria.MatCNo);
                values.Add("Dmat_batch_no", qcMateria.MatBatchNo);
                values.Add("Dmat_ename", qcMateria.MatEname);
                values.Add("Dmat_cname", qcMateria.MatCname);
                values.Add("Dmat_manufacturer", qcMateria.MatManufacturer);
                values.Add("Dmat_date_end", qcMateria.MatDateEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Dmat_Buser_name", qcMateria.MatUserId);
                values.Add("Dmat_sam_lower_limit", qcMateria.MatSamLowerLimit);
                values.Add("Dmat_sam_upper_limit", qcMateria.MatSamUpperLimit);
                values.Add("Dmat_sam_num", qcMateria.MatSamNum);
                values.Add("Dmat_method", qcMateria.MatMethod);
                values.Add("Dmat_staus", qcMateria.MatStaus);
                values.Add("Dmat_date_start", qcMateria.MatDateStart?.ToString("yyyy-MM-dd HH:mm:ss"));

                helper.InsertOperation("Dict_qc_materia", values);

                qcMateria.MatSn = id.ToString();

                result.Scusess = true;
                result.SetResult(qcMateria);
                return result;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                result.Scusess = false;
                return result;
            }
        }

        public bool UpdateQcMateria(EntityDicQcMateria qcMateria)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();

                values.Add("Dmat_Ditr_id", qcMateria.MatId);
                values.Add("Dmat_level", qcMateria.MatLevel);
                values.Add("Dmat_c_no", qcMateria.MatCNo);
                values.Add("Dmat_batch_no", qcMateria.MatBatchNo);
                values.Add("Dmat_ename", qcMateria.MatEname);
                values.Add("Dmat_cname", qcMateria.MatCname);
                values.Add("Dmat_manufacturer", qcMateria.MatManufacturer);
                values.Add("Dmat_date_end", qcMateria.MatDateEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Dmat_Buser_name", qcMateria.MatUserId);
                values.Add("Dmat_sam_lower_limit", qcMateria.MatSamLowerLimit);
                values.Add("Dmat_sam_upper_limit", qcMateria.MatSamUpperLimit);
                values.Add("Dmat_sam_num", qcMateria.MatSamNum);
                values.Add("Dmat_method", qcMateria.MatMethod);
                values.Add("Dmat_staus", qcMateria.MatStaus);
                values.Add("Dmat_date_start", qcMateria.MatDateStart?.ToString("yyyy-MM-dd HH:mm:ss"));

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dmat_id", qcMateria.MatSn);

                helper.UpdateOperation("Dict_qc_materia", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        //根据仪器ID查询质控物明细数据
        public List<EntityDicQcMateria> SearchQcMateriaByMatId(string matId, string startDate)
        {
            try
            {
                string strSql = string.Format(@"select * from dbo.Dict_qc_materia where Dmat_Ditr_id='{0}' ", matId);

                if (startDate != null)
                {
                    strSql += string.Format(@"and Dmat_date_start>'{0}' ", startDate);
                }

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(strSql);

                List<EntityDicQcMateria> list = EntityManager<EntityDicQcMateria>.ConvertToList(dt).OrderBy(i => i.MatSn).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicQcMateria>();
            }
        }

        //查询该仪器时间范围内存在的质控物主键
        public List<EntityDicQcMateria> SearchMatSnInQcMateria(EntityDicQcMateria qcMateria)
        {
            try
            {
                string strSql = "";
                if (qcMateria != null)
                {
                    string strKey = "";
                    if (!string.IsNullOrEmpty(qcMateria.MatSn))
                    {
                        strKey = " and Dmat_id !='" + qcMateria.MatSn + "'";
                    }
                    //查询该仪器时间范围内存在的质控物
                    strSql = @"select Dmat_id from Dict_qc_materia 
                                 where Dmat_Ditr_id='{0}' and Dmat_batch_no='{1}' and Dmat_level='{2}' {3}";
                    strSql = string.Format(strSql, qcMateria.MatId, qcMateria.MatBatchNo, qcMateria.MatLevel, strKey);

                }
                DBManager helper = new DBManager(); 

                DataTable dt = helper.ExecuteDtSql(strSql);

                List<EntityDicQcMateria> list = EntityManager<EntityDicQcMateria>.ConvertToList(dt).OrderBy(i => i.MatSn).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicQcMateria>();
            }
        }

        public List<EntityDicQcMateria> GetMatSn(string QcItrId, string QcNoType, string QcItmId)
        {
            try
            {
                String strSql = string.Format(@"select Dict_qc_materia.Dmat_id from Rel_qc_materia_detail
left join Dict_qc_materia on Rel_qc_materia_detail.Rmatdet_Dmat_id=Dict_qc_materia.Dmat_id
where Rmatdet_Ditr_id='{0}' and Dmat_date_end >getdate() and Dmat_level='{1}' and Rmatdet_Ditm_id='{2}'",
                    QcItrId, QcNoType, QcItmId);
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(strSql);

                List<EntityDicQcMateria> list = EntityManager<EntityDicQcMateria>.ConvertToList(dt).OrderBy(i => i.MatSn).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicQcMateria>();
            }
        }

        //查询该仪器时间范围历史质控批号
        public List<EntityDicQcMateria> SearchQcMateriaLeftRuleTimeAndInterface(string matId, string matLevel)
        {
            try
            {
                string strKey = "";
                if (!string.IsNullOrEmpty(matId))
                {
                    strKey = " and Dict_qc_materia.Dmat_Ditr_id ='" + matId + "'";
                }
                if (!string.IsNullOrEmpty(matLevel))
                {
                    strKey += " and Dict_qc_materia.Dmat_level ='" + matLevel + "'";
                }
                //查询该仪器时间范围历史质控批号
                string strSql = @"select Dict_qc_materia.Dmat_id as qcm_par_detail_id,
Dict_qc_materia.Dmat_Ditr_id as qcm_mid,
Dict_qc_materia.Dmat_batch_no as qcm_r_no,
Dict_qc_materia.Dmat_level as qcm_c_no,
isnull(Rel_qc_instrmt_channel.Rchan_type,'0') as channel_type,
Rel_qc_instrmt_channel.Rchan_sid_ident,
Rel_qc_instrmt_channel.Rchan_Rqrt_id,
Rel_qc_rule_time.Rqrt_start_time,
Rel_qc_rule_time.Rqrt_end_time,
Rel_qc_rule_time.Rqrt_day,
(case Rel_qc_instrmt_channel.Rchan_type when '0' then '样本号' when '1' then '质控标识' else  '' end) as qcm_type_name
from dbo.Dict_qc_materia
left join Rel_qc_instrmt_channel on Rel_qc_instrmt_channel.Rchan_Dmat_id=Dict_qc_materia.Dmat_id  
and Rel_qc_instrmt_channel.Rchan_Ditr_id=Dict_qc_materia.Dmat_Ditr_id
left join Rel_qc_rule_time on Rel_qc_instrmt_channel.Rchan_Rqrt_id=Rel_qc_rule_time.Rqrt_id
where 1=1  {0}	 ";
                strSql = string.Format(strSql, strKey);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(strSql);

                List<EntityDicQcMateria> list = EntityManager<EntityDicQcMateria>.ConvertToList(dt).OrderBy(i => i.MatSn).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicQcMateria>();
            }
        }

        public bool DeleteQcMateria(EntityDicQcMateria qcMateria)
        {
            try
            {
                DBManager helper = new DBManager();
                string sqlStr = string.Format(@"delete Dict_qc_materia where Dmat_id='{0}' ", qcMateria.MatSn);
                helper.ExecSql(sqlStr);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicQcMateria> SearchQcMateriaAll()
        {
            List<EntityDicQcMateria> listQcMatAll = new List<EntityDicQcMateria>();
            try
            {
                DBManager helper = new DBManager();

                string sqlQcMatAll = @"SELECT Dict_qc_materia.*,dbo.Dict_itr_instrument.Ditr_ename,Dict_profession.Dpro_name 
FROM dbo.Dict_qc_materia 
left join dbo.Dict_itr_instrument ON dbo.Dict_qc_materia.Dmat_Ditr_id=dbo.Dict_itr_instrument.Ditr_id 
left join Dict_profession on Dict_profession.Dpro_id=Dict_itr_instrument.Ditr_lab_id
WHERE Dict_qc_materia.Dmat_c_no IS NULL";
                DataTable dtQcMatAll = helper.ExecuteDtSql(sqlQcMatAll);

                listQcMatAll = EntityManager<EntityDicQcMateria>.ConvertToList(dtQcMatAll).OrderBy(i => i.MatSn).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("查询质控物数据SearchQcMateriaAll", ex);
            }

            return listQcMatAll;
        }

        public List<EntityDicQcMateria> GetMatSn(string QcItrId,DateTime dtStartTime, DateTime dtEndTime, string QcItmId)
        {
            List<EntityDicQcMateria> listQcMateria = new List<EntityDicQcMateria>();
            try
            {
                String strSql = string.Format(@"select Dmat_id,Dmat_batch_no,Dmat_level,0 qcr_select 
from dbo.Dict_qc_materia  
left join Rel_qc_materia_detail on Dict_qc_materia.Dmat_id=Rel_qc_materia_detail.Rmatdet_Dmat_id and Dict_qc_materia .Dmat_Ditr_id=Rel_qc_materia_detail.Rmatdet_Ditr_id
where NOT(Dict_qc_materia.Dmat_date_end < '{2}' or Dict_qc_materia.Dmat_date_start > '{3}')  and Dict_qc_materia.Dmat_Ditr_id={0} and Rel_qc_materia_detail.Rmatdet_Ditm_id={1}
group by Dmat_batch_no,Dmat_level,Dmat_id",
                                                QcItrId, QcItmId, dtStartTime.ToString("yyyy-MM-dd HH:mm:ss"), dtEndTime.ToString("yyyy-MM-dd HH:mm:ss"));

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(strSql);

                listQcMateria = EntityManager<EntityDicQcMateria>.ConvertToList(dt).OrderBy(i => i.MatSn).ToList();

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return listQcMateria;
        }
    }
}
