using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using dcl.entity;
using dcl.dao.core;
using System.Data;
using System.ComponentModel.Composition;
using dcl.common;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoMergeResult))]
    public class DaoMergeResult : IDaoMergeResult
    {
        public List<EntityObrResult> GetMergeResult(EntityMergeResultQC qc)
        {
            string strWhere = null;
            strWhere += " and Lis_result.Lres_Ditr_id='" + qc.RepItrId + "' ";

            strWhere += string.Format(" and Pat_lis_main.Pma_in_date >= DateAdd(d,0,'{0}') and Pat_lis_main.Pma_in_date < DateAdd(d,1,'{0}') ",
                qc.ObrDate.ToShortDateString());

            if (qc.MatchMode == "1")
            {
                strWhere += " and dbo.convertSidToNumeric(Pat_lis_main.Pma_serial_num) >= " + qc.IdStart + " ";
                strWhere += " and dbo.convertSidToNumeric(Pat_lis_main.Pma_serial_num) <= " + qc.IdEnd + " ";
            }
            else
            {
                strWhere += " and dbo.convertSidToNumeric(Lis_result.Lres_sid) >= " + qc.IdStart + " ";
                strWhere += " and dbo.convertSidToNumeric(Lis_result.Lres_sid) <= " + qc.IdEnd + " ";
            }

            //涉及到合并，需要查出所有Obr_result的数据，所以这里用*查询
            string strSql = @"select
Lis_result.*,
Pat_lis_main.Pma_status,
Pat_lis_main.Pma_serial_num,
Pat_lis_main.Pma_bar_code,
Pat_lis_main.Pma_pat_in_no,
Dict_itr_instrument.Ditr_ename
from
Lis_result
LEFT JOIN Dict_itr_instrument ON Lis_result.Lres_Ditr_id = Dict_itr_instrument.Ditr_id 
LEFT JOIN Pat_lis_main ON Pat_lis_main.Pma_rep_id = Lis_result.Lres_Pma_rep_id 
where 1=1  "
                                + strWhere;

            DBManager helper = new DBManager();

            DataTable dt = helper.ExecuteDtSql(strSql);

            return EntityManager<EntityObrResult>.ConvertToList(dt);
        }


        public List<EntityPidReportMain> GetMergePatients(EntityMergeResultQC qc)
        {
            string strSql = String.Format(@"select 
Pma_rep_id,
Pma_sid,
Pma_bar_code,
Pma_pat_in_no,
Pma_pat_Didt_id,
Pma_Ditr_id,
Pma_serial_num,
isnull(Pma_status,0) as Pma_status 
from Pat_lis_main
where 
Pat_lis_main.Pma_Ditr_id='{0}' 
and Pat_lis_main.Pma_in_date>='{1}' 
and Pat_lis_main.Pma_in_date<=DateAdd(d,1,'{1}')"
                , qc.RepItrId, qc.ObrDate);

            if (qc.MatchMode == "1")
            {
                strSql += " and dbo.convertSidToNumeric(Pat_lis_main.Pma_serial_num) >= " + qc.IdStart + " ";
                strSql += " and dbo.convertSidToNumeric(Pat_lis_main.Pma_serial_num) <= " + qc.IdEnd + " ";
            }
            else
            {
                strSql += " and dbo.convertSidToNumeric(Pat_lis_main.Pma_sid) >= " + qc.IdStart + " ";
                strSql += " and dbo.convertSidToNumeric(Pat_lis_main.Pma_sid) <= " + qc.IdEnd + " ";
            }

            DBManager helper = new DBManager();

            DataTable dt = helper.ExecuteDtSql(strSql);

            return EntityManager<EntityPidReportMain>.ConvertToList(dt);
        }

        public List<EntityObrResult> GetSourceResult(EntityMergeResultQC qc)
        {
            string strWhere = null;
            strWhere += " and lis_result_originalex.Lro_Ditr_id='" + qc.RepItrId + "' ";

            strWhere += string.Format(" and substring(Lis_result_originalex.Lro_Lresdesc_id, len(Lis_result_originalex.Lro_Ditr_id) + 1, 8) >= convert(VARCHAR(100), DateAdd(d,0,'{0}'), 112) and substring(Lis_result_originalex.Lro_Lresdesc_id, len(Lis_result_originalex.Lro_Ditr_id) + 1, 8) < convert(VARCHAR(100), DateAdd(d,1,'{0}'), 112) ",
                qc.ObrDate.ToShortDateString());

            if (qc.MatchMode == "1")
            {
                return GetMergeResult(qc);
                //strWhere += " and dbo.convertSidToNumeric(Pat_lis_main.Pma_serial_num) >= " + qc.IdStart + " ";
                //strWhere += " and dbo.convertSidToNumeric(Pat_lis_main.Pma_serial_num) <= " + qc.IdEnd + " ";
            }
            else
            {
                strWhere += " and dbo.convertSidToNumeric(Lis_result_originalex.Lro_sid) >= " + qc.IdStart + " ";
                strWhere += " and dbo.convertSidToNumeric(Lis_result_originalex.Lro_sid) <= " + qc.IdEnd + " ";
            }

            //涉及到合并，需要查出所有Obr_result_originalex的数据，所以这里用*查询
            string strSql = @"SELECT
Lis_result_originalex.*,
Dict_itr_instrument.Ditr_ename
FROM Lis_result_originalex LEFT JOIN Dict_itr_instrument ON Lis_result_originalex.Lro_Ditr_id = Dict_itr_instrument.Ditr_id
WHERE 1=1  " + strWhere;

            DBManager helper = new DBManager();

            DataTable dt = helper.ExecuteDtSql(strSql);

            var list =  ConvertToEntityObrResult(dt);

            return list;
        }

        private List<EntityObrResult> ConvertToEntityObrResult(DataTable dtSour)
        {
            List<EntityObrResult> list = new List<EntityObrResult>();
            IDaoDicItmItem dao = DclDaoFactory.DaoHandler<IDaoDicItmItem>();

            if (dtSour != null && dtSour.Rows.Count > 0)
            {
                foreach (DataRow item in dtSour.Rows)
                {
                    EntityObrResult obrResult = new EntityObrResult();
                    obrResult.ObrSn = Convert.ToInt64(item["Lro_id"]);
                    obrResult.ObrId = Convert.ToString(item["Lro_Lresdesc_id"]);
                    obrResult.ObrItrId = Convert.ToString(item["Lro_Ditr_id"]);
                    //obrResult.RepStatus = Convert.ToInt64(item["Pma_status"]);
                    //obrResult.RepSerialNum = Convert.ToString(item["Pma_serial_num"]);
                    //obrResult.PidInNo = Convert.ToString(item["Pma_pat_in_no"]);
                    obrResult.ItrEname = Convert.ToString(item["Ditr_ename"]);
                    obrResult.ObrSid = Convert.ToString(item["Lro_sid"]);
                    obrResult.ItmId = Convert.ToString(item["Lro_Ricc_code"]);
                    obrResult.ObrValue = Convert.ToString(item["Lro_value"]);
                    obrResult.ObrValue2 = Convert.ToString(item["Lro_value2"]);
                    obrResult.ObrValue3 = Convert.ToString(item["Lro_value3"]);
                    obrResult.ObrValue4 = Convert.ToString(item["Lro_value4"]);
                    obrResult.ObrDate = Convert.ToDateTime(item["Lro_date"]);
                    obrResult.ObrRemark = Convert.ToString(item["Lro_remark"]);
                    obrResult.ItmEname = dao.Search(item["Lro_Ricc_code"].ToString(), null)[0].ItmName;

                    list.Add(obrResult);
                }
            }

            return list;
        }
    }
}
