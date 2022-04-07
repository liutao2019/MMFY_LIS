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
using System.Linq;
using System.Text;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;

namespace wf.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoReaLossReportDetail))]
    public class DaoReaLossReportDetail : DclDaoBase, IDaoReaLossReportDetail
    {
        public bool DeleteObrResultByObrSn(string obrSn)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(obrSn))
            {
                string sql = string.Format("delete from Rea_lossreport_detail where Rld_id = '{0}'", obrSn);
                DBManager helper = GetDbManager();
                try
                {
                    helper.ExecCommand(sql);
                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    throw;
                }
            }
            return result;
        }
        public bool CancelReaLossReportDetail(EntityReaLossReportDetail detail)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rld_no", detail.Rld_no);

                helper.UpdateOperation("Rea_lossreport_detail", values, keys);


                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool DeleteReaLossReportDetail(string LossReportId, string rea_id)
        {
            bool result = false;
            try
            {
                DBManager helper = GetDbManager();

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rld_no", LossReportId);
                if (!string.IsNullOrEmpty(rea_id))
                {
                    keys.Add("Rld_reaid", rea_id);
                }


                helper.DeleteOperation("Rea_lossreport_detail", keys);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return result;
            }
            return true;
        }

        public bool InsertNewReaLossReportDetail(EntityReaLossReportDetail LossReport)
        {
            bool result = false;

            DBManager helper = GetDbManager();

            if (LossReport != null)
            {
                try
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values = helper.ConverToDBSaveParameter(LossReport);

                    helper.InsertOperation("Rea_lossreport_detail", values);

                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    throw;
                }
            }
            return result;
        }

        public List<EntityReaLossReportDetail> GetReaLossReportDetail(EntityReaQC reaQC)
        {
            List<EntityReaLossReportDetail> detailList = new List<EntityReaLossReportDetail>();
            DataTable dtDetail = new DataTable();
            DBManager helper = new DBManager();
            string sql = @"
SELECT 
Dict_rea_setting.Drea_name as ReagentName,
Rea_lossreport_detail.package as ReagentPackage,
Rea_lossreport_detail.*
FROM Rea_lossreport_detail 
left JOIN Dict_rea_setting ON Rea_lossreport_detail.Rld_reaid = Dict_rea_setting.Drea_id
WHERE 1=1  and Rea_lossreport_detail.del_flag = '0' {0}
";
            string sqlWhere = string.Empty;
            if (!string.IsNullOrEmpty(reaQC.ReaNo))
            {
                sqlWhere += string.Format(@" and Rea_lossreport_detail.Rld_no='{0}' ", reaQC.ReaNo);
            }
            if (!string.IsNullOrEmpty(reaQC.ReaId))
            {
                sqlWhere += string.Format(@" and Rea_lossreport_detail.Rld_reaid='{0}' ", reaQC.ReaId);
            }
            if (!string.IsNullOrEmpty(reaQC.Barcode))
            {
                sqlWhere += string.Format(@" and Rea_lossreport_detail.Rld_barcode='{0}' ", reaQC.Barcode);
            }
            try
            {
                dtDetail = helper.ExecuteDtSql(string.Format(sql, sqlWhere));
                detailList = EntityManager<EntityReaLossReportDetail>.ConvertToList(dtDetail);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return detailList;
        }

    }
}
