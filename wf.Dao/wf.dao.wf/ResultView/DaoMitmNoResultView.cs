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
    [Export("wf.plugin.wf", typeof(IDaoMitmNoResultView))]
    public class DaoMitmNoResultView : IDaoMitmNoResultView
    {
        /// <summary>
        /// 获取指定仪器和日期的所有样本和结果(从中间表获取)
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itr_id"></param>
        /// <param name="result_type"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<EntityDicObrResultOriginal> GetInstructmentResult2(DateTime date, string itr_id, int result_type, string filter)
        {
            DateTime dtSearchDate = date.Date;

            //获取样本号格式
            string filterStr = "";
            if (string.IsNullOrEmpty(filter))
            {
                filterStr = " 1=1 ";
            }
            else
            {
                bool blnBetween = false;
                string sqlSid = "";
                if (filter.Length > 2 && filter.Contains("-") && (!filter.StartsWith("-")) && (!filter.EndsWith("-")))//如 1-2
                {
                    char[] ch = filter.ToCharArray();

                    for (int i = 0; i < ch.Length; i++)//必须为数值型或带-
                    {
                        if (ch[i] == '-' || (ch[i] >= '0' && ch[i] <= '9')) { blnBetween = true; }
                        else { blnBetween = false; break; }
                    }

                    if (blnBetween)
                    {
                        string[] spTemp = filter.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

                        decimal dlTempStart = 1;//起始样本号
                        decimal dlTempEnd = 2;//结束样本号

                        blnBetween = false;

                        if (spTemp.Length > 1)
                        {
                            if (decimal.TryParse(spTemp[0], out dlTempStart) && decimal.TryParse(spTemp[1], out dlTempEnd))
                            {
                                if (dlTempStart < dlTempEnd)
                                {
                                    sqlSid = string.Format(@" isnull(Lro_sid,'')<>''
    and Isnumeric(Lro_sid)=1
    and cast(Lro_sid as decimal(18,0))>={0} and cast(Lro_sid as decimal(18,0))<={1} "
                                        , dlTempStart.ToString()
                                        , dlTempEnd.ToString());

                                }
                                else if (dlTempStart == dlTempEnd)
                                {
                                    sqlSid = string.Format(" Lro_sid='{0}' ", dlTempStart.ToString());
                                }
                                else if (dlTempStart > dlTempEnd)
                                {
                                    sqlSid = string.Format(@" isnull(Lro_sid,'')<>''
    and Isnumeric(Lro_sid)=1
    and cast(Lro_sid as decimal(18,0))>={0} and cast(Lro_sid as decimal(18,0))<={1} "
                                        , dlTempEnd.ToString()
                                        , dlTempStart.ToString());
                                }

                                blnBetween = true;
                            }
                        }
                    }
                }

                if (blnBetween)//使用区间查询
                {
                    filterStr = sqlSid;
                }
                else
                {
                    filterStr = string.Format("Lro_sid = '{0}'", SQLFormater.Format(filter));
                }
            }


            string sqlSelect;
            if (result_type == 0)
            {
                sqlSelect = string.Format(@"select
Lro_id,--.obr_sn,
Lro_Ditr_id,--.obr_itr_id,
Lro_source_Ditr_id,--.obr_source_itr_id,
Lro_sid,--.obr_sid,
Lro_Ricc_code,--.obr_mac_code,
Lro_value,--.obr_value,
Lro_value2,--.obr_value2,
Lro_value3,--.obr_value3,
Lro_value4,--.obr_value4,
Lro_date,--.obr_date,
Lro_remark,--.obr_remark,
Lro_data_type,--.obr_data_type,
Lro_receiver_flag,--.obr_receiver_flag,
Lro_Lresdesc_id,---.obr_id,
Lro_critical_flag,--.obr_critical_flag,

Rel_itr_channel_code.Ricc_Ditm_id as itm_id,
Dict_itm.Ditm_ecode as itm_ecd,
msg = case when Rel_itr_channel_code.Ricc_Ditm_id is null then '通道码设置错误'
else '' end

from Lis_result_original
left outer join Rel_itr_channel_code on (Rel_itr_channel_code.Ricc_code = Lis_result_original.Lro_Ricc_code and 
Lis_result_original.Lro_Ditr_id = Rel_itr_channel_code.Ricc_Ditr_id and Rel_itr_channel_code.del_flag='0')
left outer join Dict_itm on Rel_itr_channel_code.Ricc_Ditm_id = Dict_itm.Ditm_id
where
(Lis_result_original.Lro_date >='{0}' and Lis_result_original.Lro_date <'{1}')
and Lis_result_original.Lro_Ditr_id = '{2}'
and {3}
", date.Date.ToString("yyyy-MM-dd HH:mm:ss"), date.Date.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), itr_id, filterStr);

            }
            else if (result_type == 1)
            {
                sqlSelect = string.Format(@"select
Lres_id as Lro_id,
Lres_Ditr_id as Lro_Ditr_id,
Lres_source_Ditr_id as Lro_source_Ditr_id,
Lres_sid as Lro_sid,
Lro_Ricc_code = '',
Lres_value as Lro_value,
Lres_value2 as Lro_value2,
Lro_value3 = '',
Lro_value4 = '',
Lres_date as Lro_date,
Lres_Pma_rep_id as Lro_Lresdesc_id,
Lres_Ditm_id as itm_id,
Lres_itm_ename as itm_ecd,
msg = ''
from Lis_result
left join Pat_lis_main on Pat_lis_main.Pma_rep_id = Lis_result.Lres_Pma_rep_id
where
--res_flag = 1 and
((Lis_result.Lres_date >='{0}' and Lis_result.Lres_date <'{1}') )
and Lis_result.Lres_Ditr_id = '{2}'
and {3}
", date.Date.ToString("yyyy-MM-dd HH:mm:ss"), date.Date.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), itr_id, filterStr);

            }

            else
            {
                sqlSelect = string.Format(@"select
Lro_id,
Lro_Ditr_id,
Lro_source_Ditr_id,
Lro_sid,
Lro_Ricc_code,
Lro_value,
Lro_value2,
Lro_value3,
Lro_value4,
Lro_date,
Lro_remark,
Lro_data_type,
Lro_receiver_flag,
Lro_Lresdesc_id,
Lro_critical_flag,

Rel_itr_channel_code.Ricc_Ditm_id as itm_id,
Dict_itm.Ditm_ecode as itm_ecd,
msg = (case when Rel_itr_channel_code.Ricc_Ditm_id is null or Rel_itr_channel_code.del_flag = '1' then '通道码设置错误'
when Rel_itr_channel_code.del_flag  = '1' then '通道码已失效'
else '' end)

from Lis_result_original
left outer join Rel_itr_channel_code on (Rel_itr_channel_code.Ricc_code = Lis_result_original.Lro_Ricc_code 
and Lis_result_original.Lro_Ditr_id = Rel_itr_channel_code.Ricc_Ditr_id)
left outer join Dict_itm on Rel_itr_channel_code.Ricc_Ditm_id = Dict_itm.Ditm_id
where
(Lis_result_original.Lro_date >='{0}' and Lis_result_original.Lro_date <'{1}')
and Lis_result_original.Lro_Ditr_id = '{2}'
and {3}
", date.Date.ToString("yyyy-MM-dd HH:mm:ss"), date.Date.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"), itr_id, filterStr);
            }

            DBManager helper = new DBManager();

            DataTable dt = helper.ExecuteDtSql(sqlSelect);

            if (!dt.Columns.Contains("res_sid_int"))
            {
                dt.Columns.Add("res_sid_int", typeof(double));
            }

            List<EntityDicObrResultOriginal> list = EntityManager<EntityDicObrResultOriginal>.ConvertToList(dt).OrderBy(i => i.ObrSn).ToList();

            foreach (var item in list)
            {
                if (item.ObrSid != null
                    && item.ObrSid != ""
                   && !string.IsNullOrEmpty(item.ObrSid)
                    )
                {
                    string sid = item.ObrSid;
                    double pat_sid_int = 0;
                    if (double.TryParse(sid, out pat_sid_int))
                    {
                        item.ResSidInt = pat_sid_int;
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 插入或更新仪器通道字典数据
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public EntityResponse SaveOrUpdateMitmNo(List<EntityDicMachineCode> ds)
        {
            EntityResponse response = new EntityResponse();

            DataSet result = new DataSet();
            try
            {
                List<EntityDicMachineCode> dtUpdate = new List<EntityDicMachineCode>();
                List<EntityDicMachineCode> dtNew = new List<EntityDicMachineCode>();

                DaoDicMachineCode dao = new DaoDicMachineCode();

                if (dao == null)
                {
                    response.Scusess = false;
                    response.ErroMsg = "未找到数据访问";
                }
                else
                {
                    if (ds.Count > 0)
                    {
                        foreach (var dr in ds)
                        {
                            string mit_id = dr.MacId;
                            if (mit_id != "")
                            {
                                dtUpdate.Add(dr);
                            }
                            else
                            {
                                dtNew.Add(dr);
                            }
                        }

                        foreach (var infoUpdate in dtUpdate)
                        {
                            dao.Update(infoUpdate);
                        }

                        foreach (var infoSave in dtNew)
                        {
                            dao.Save(infoSave);
                        }

                        response.Scusess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                response.Scusess = false;
            }
            return response;
        }


    }
}
