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
    [Export("wf.plugin.wf", typeof(IDaoLisDoc))]
    public class DaoLisDoc : IDaoLisDoc
    {
        public int Save(EntityLisDoc doc)
        {
            try
            {
                DBManager helper = new DBManager();
                string queryModelSql = string.Format("select 1 from lis_report_doc where doc_title = '{0}' and doc_type = '0'", doc.docTitle);
                //string queryDateSql = string.Format("select 1 from lis_report_doc where doc_title = '{0}' and doc_type = '1'", doc.docTitle);

                Dictionary<string, object> values = new Dictionary<string, object>();
                Dictionary<string, object> vkey = new Dictionary<string, object>();

                values.Add("doc_date", doc.docDate);
                values.Add("doc_type", doc.docType);
                values.Add("doc_title", doc.docTitle);
                values.Add("doc_content", doc.docContent);

                if (helper.ExecuteDtSql(queryModelSql).Rows.Count > 0 && doc.docType == "0") //已有记录并且存为模板
                {
                    vkey.Add("doc_title", doc.docTitle);
                    vkey.Add("doc_type", doc.docType);
                    helper.UpdateOperation("lis_report_doc", values, vkey);
                }
                //else if(helper.ExecuteDtSql(queryDateSql).Rows.Count > 0)
                //{
                //    vkey.Add("doc_title", doc.docTitle);
                //    vkey.Add("doc_type", doc.docType);
                //    helper.UpdateOperation("lis_report_doc", values, vkey);
                //}
                else
                {
                    helper.InsertOperation("lis_report_doc", values);
                }

                return 1;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return -1;
            }
        }

        /// <summary>
        /// 查找文档
        /// </summary>
        /// <returns></returns>
        public List<EntityLisDoc> QueryAll()
        {
            try
            {
                String sql = String.Format(@"select * from lis_report_doc");
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);
                List<EntityLisDoc> list = EntityManager<EntityLisDoc>.ConvertToList(dt).OrderBy(i => i.docId).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityLisDoc>();
            }
        }

        /// <summary>
        /// 按日期和类型查找数据文档
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public List<EntityLisDoc> Query(DateTime beginTime, DateTime endTime, string docType)
        {
            try
            {
                String sql = String.Format(@"select * from lis_report_doc where doc_date BETWEEN '{0}' AND '{1}' AND doc_title = '{2}' and doc_type = '1'", beginTime, endTime, docType);
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);
                List<EntityLisDoc> list = EntityManager<EntityLisDoc>.ConvertToList(dt).OrderBy(i => i.docId).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityLisDoc>();
            }
        }

        public int Update(EntityLisDoc doc)
        {
            throw new NotImplementedException();
        }

        public int Delete(EntityLisDoc doc)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("doc_id", doc.docId);

                return helper.DeleteOperation("lis_report_doc", keys);
            }
            catch(Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return -1;
            }
        }
    }
}
