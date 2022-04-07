using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.core;
using System.Data;

namespace dcl.dao.wf
{
    /// <summary>
    /// 报告解读评价
    /// </summary>
    [Export("wf.plugin.wf", typeof(IDaoReportComment))]
    public class DaoReportComment : IDaoReportComment
    {
        public bool DeleteReportComment(string rcKey)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(rcKey))
            {
                try
                {
                    DBManager helper = new DBManager();
                    string sqlDelete = string.Format("delete from Pat_report_comment where Prc_id='{0}'", rcKey);
                    helper.ExecSql(sqlDelete);
                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("DeleteReportComment", ex);
                    throw;
                }
            }
            return result;
        }


        public List<EntityReportComment> GetReportComment(string pat_id)
        {
            List<EntityReportComment> listObrMsgImage = new List<EntityReportComment>();
            if (!string.IsNullOrEmpty(pat_id))
            {
                try
                {
                    DBManager helper = new DBManager();

                    string sql = string.Format("select * from Pat_report_comment where Prc_rep_id='{0}' ", pat_id);

                    DataTable dtObrResImage = helper.ExecuteDtSql(sql);

                    listObrMsgImage = EntityManager<EntityReportComment>.ToList(dtObrResImage).OrderBy(i => i.RcDate).ToList();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("GetReportComment：病人ID=" + pat_id, ex);
                    throw;
                }
            }
            return listObrMsgImage;
        }

        public int SaveReportComment(EntityReportComment model)
        {
            int saveInt = 0;
            if (model != null)
            {
                try
                {
                    DBManager helper = new DBManager();

                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values.Add("Prc_rep_id", model.RcRepId);
                    values.Add("Prc_comment", model.RcComment);
                    values.Add("Prc_date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    values.Add("Prc_user_name", model.RcOpname);
                    values.Add("Prc_user_id", model.RcOpcode);

                    saveInt = helper.InsertOperation("Pat_report_comment", values);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("保存失败SaveReportComment", ex);
                    throw;
                }
            }
            return saveInt;
        }
    }

}
