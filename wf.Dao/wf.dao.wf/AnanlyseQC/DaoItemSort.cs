using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;
using dcl.dao.core;
using dcl.common;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoItemSort))]
    public class DaoItemSort : IDaoItemSort
    {
        public EntityDCLPrintData GetReportData(EntityAnanlyseQC query)
        {
            EntityDCLPrintData printer = new EntityDCLPrintData();
            DataSet ds = new DataSet();
            string where = "select * from Base_report where Brep_code='itemSort'";
            DBManager helper = new DBManager();
            DataTable dtEx = helper.ExecuteDtSql(where);
            if (dtEx.Rows.Count > 0)
            {
                EntitySysReport report = EntityManager<EntitySysReport>.ConvertToEntity(dtEx.Rows[0]);
                string condition = GetSqlString(query);
                string sql = EncryptClass.Decrypt(report.RepSql);
                sql = sql.Replace("&where&", condition);
                DataTable an = helper.ExecuteDtSql(sql);
                an.TableName = "ItemSort";
                ds.Tables.Add(an.Copy());
                printer.ReportData = ds;
                printer.ReportName = report.RepLocation.Replace(".repx", "");
            }

            return printer;
        }

        public string GetSqlString(EntityAnanlyseQC query)
        {
            string where = string.Empty;

            where += " and Lis_result.Lres_date>= '" + query.DateStart.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            where += " and Lis_result.Lres_date<'" + query.DateEnd.ToString("yyyy-MM-dd HH:mm:ss") + "'";

            if (query.listSid.Count > 0)
            {
                if (query.listSid[0].StartSid > 0)
                    where += " and dbo.convertSidToNumeric(Lis_result.Lres_sid)>=" + query.listSid[0].StartSid;
                if (query.listSid[0].EndSid != null)
                    where += " and dbo.convertSidToNumeric(Lis_result.Lres_sid)<=" + query.listSid[0].EndSid.Value;
            }

            if (query.listSort.Count > 0)
            {
                if (query.listSort[0].StartNo > 0)
                    where += " and dbo.convertSidToNumeric(Pat_lis_main.Pma_serial_num)>=" + query.listSort[0].StartNo;
                if (query.listSort[0].EndNo != null)
                    where += " and dbo.convertSidToNumeric(Pat_lis_main.Pma_serial_num)<=" + query.listSort[0].EndNo.Value;
            }

            if (query.ItrId != null)
                where += " and Lis_result.Lres_Ditr_id='" + query.ItrId + "'";

            if (query.ItmId != null)
                where += " and Lis_result.Lres_Ditm_id='" + query.ItmId + "'";

            if (query.DepId != null)
                where += " and Pat_lis_main.Pma_pat_dept_id = '" + query.DepId + "'";

            return where;
        }
    }
}
