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
    [Export("wf.plugin.wf", typeof(IDaoSummaryPrint))]
    public class DaoSummaryPrint : IDaoSummaryPrint
    {
        public EntityDCLPrintData GetReportData(EntityAnanlyseQC query)
        {
            DataSet ds = new DataSet();
            EntityDCLPrintData printer = new EntityDCLPrintData();
            string code = query.RepCode;
            string where = "select * from Base_report where Brep_code='" + code + "'";
            DBManager helper = new DBManager();
            DataTable dtEx = helper.ExecuteDtSql(where);
            if (dtEx.Rows.Count > 0)
            {
                EntitySysReport report = EntityManager<EntitySysReport>.ConvertToEntity(dtEx.Rows[0]);
                string condition = GetWhere(query);
                string sql = EncryptClass.Decrypt(report.RepSql);
                sql = sql.Replace("&where&", condition);
                try
                {
                    DataTable an = helper.ExecuteDtSql(sql);
                    an.TableName = "可设计字段";
                    ds.Tables.Add(an);
                    printer.ReportData = ds;
                    printer.ReportName = report.RepLocation.Replace(".repx", "");
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }

            return printer;
        }
        public string GetWhere(EntityAnanlyseQC query)
        {
            string condition = "";

            //录入日期
            condition += " and Pat_lis_main.Pma_in_date>= '" + query.DateStart.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            condition += " and Pat_lis_main.Pma_in_date<'" + query.DateEnd.ToString("yyyy-MM-dd HH:mm:ss") + "'";

            //病人ID
            if (query.ItrId != null)
            {
                condition += " and Pat_lis_main.Pma_Ditr_id='" + query.ItrId + "'";
            }

            if (query.listSid.Count > 0)
            {
                string strSidRange = string.Empty;

                foreach (var item in query.listSid)
                {
                    //范围
                    if (item.EndSid != null)
                    {
                        strSidRange += string.Format(@" or( cast(Pat_lis_main.Pma_sid as BIGINT)>={0} 
                                                        and cast(Pat_lis_main.Pma_sid as BIGINT)<={1})",
                                                        item.StartSid,
                                                        item.EndSid);
                    }
                    //单个
                    else if (item.EndSid == null)
                    {
                        strSidRange += string.Format(@" or( cast(Pat_lis_main.Pma_sid as BIGINT)={0})",
                                                       item.StartSid);
                    }
                }

                strSidRange = strSidRange.Remove(0, 3);
                condition += " and (" + strSidRange + ")";
            }
            if (query.listSort.Count > 0)
            {
                string strSidRange = string.Empty;

                foreach (var item in query.listSort)
                {
                    //范围
                    if (item.EndNo != null)
                    {
                        strSidRange += string.Format(@" or( cast(Pat_lis_main.Pma_serial_num as BIGINT)>={0} 
                                                        and cast(Pat_lis_main.Pma_serial_num as BIGINT)<={1})",
                                                        item.StartNo,
                                                        item.EndNo);
                    }
                    //单个
                    else if (item.EndNo == null)
                    {
                        strSidRange += string.Format(@" or( cast(Pat_lis_main.Pma_serial_num as BIGINT)={0})",
                                                       item.StartNo);
                    }
                }

                strSidRange = strSidRange.Remove(0, 3);
                condition += " and (" + strSidRange + ")";
            }

            if (query.SamId != null)
            {
                condition += " and Pat_lis_main.Pma_Dsam_id='" + query.SamId + "'";
            }

            switch (query.reportType)
            {
                case ReportType.SHCDJG:
                    condition += " and Pat_lis_main.Pma_status>0";
                    break;
                case ReportType.YXCDJG:
                    condition += " and((Lis_result.Lres_value like '%阳%') OR  ( Lis_result.Lres_value like '%+%'))";
                    break;
                case ReportType.WSCDJG:
                    condition += " and Pat_lis_main.Pma_status=0";
                    break;
                default:
                    break;
            }

            return condition;
        }
    }
}
