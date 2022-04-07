using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.core;
using System.Data;
using dcl.common;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoOaTableDetail))]
    public class DaoOaTableDetail : IDaoOaTableDetail
    {
        public bool DeleteTabDetail(EntityOaTableDetail detail)
        {
            try
            {
                DBManager helper = new DBManager();
                string sql1 = String.Format("delete from Oa_detail where Oadet_id={0}", detail.DetId);
                string sql2 = String.Format("delete from Oa_detail where Oadet_char_a={0} and Oadet_char_b='det_content'", detail.DetId);
                helper.ExecSql(sql1);
                helper.ExecSql(sql2);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool DeleteTabDetailByTypeCode(string typeCode)
        {
            try
            {
                DBManager helper = new DBManager();
                string sql1 = String.Format("delete from Oa_detail where Oadet_Dot_code={0}", typeCode);
                helper.ExecSql(sql1);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityOaTableDetail> GetTabDetailByTabCode(EntityOaTableDetail detail)
        {
            string where = string.Empty;
            where = GetWhere(detail);
            List<EntityOaTableDetail> tabDetail = null;
            try
            {
                DBManager helper = new DBManager();
                string sql = String.Format("select * from Oa_detail {0}", where);
                DataTable dt = helper.ExecuteDtSql(sql);
                tabDetail = EntityManager<EntityOaTableDetail>.ConvertToList(dt);
                return tabDetail;
            }
            catch(Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return tabDetail;
            }
        }

        public bool InsertNewTabDetail(EntityOaTableDetail detail)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Oa_detail");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Oadet_id", id);
                values.Add("Oadet_Dot_code", detail.TabCode);
                values.Add("Oadet_date", detail.DetDate.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Oadet_Buser_id", detail.DetUserId);
                values.Add("Oadet_content", detail.DetContent);
                values.Add("Oadet_char_a", detail.DetCharA);
                values.Add("Oadet_char_b", detail.DetCharB);
                values.Add("Oadet_char_c", detail.DetCharC);
                values.Add("Oadet_char_d", detail.DetCharD);
                values.Add("Oadet_char_e", detail.DetCharE);
                values.Add("Oadet_char_f", detail.DetCharF);
                values.Add("Oadet_char_g", detail.DetCharG);
                values.Add("Oadet_char_h", detail.DetCharH);

                helper.InsertOperation("Oa_detail", values);

                detail.DetId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool UpdateTabDetail(EntityOaTableDetail detail)
        {
            try
            {

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();

                values.Add("Oadet_Dot_code", detail.TabCode);
                values.Add("Oadet_date", detail.DetDate.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Oadet_Buser_id", detail.DetUserId);
                values.Add("Oadet_content", detail.DetContent);
                values.Add("Oadet_char_a", detail.DetCharA);
                values.Add("Oadet_char_b", detail.DetCharB);
                values.Add("Oadet_char_c", detail.DetCharC);
                values.Add("Oadet_char_d", detail.DetCharD);
                values.Add("Oadet_char_e", detail.DetCharE);
                values.Add("Oadet_char_f", detail.DetCharF);
                values.Add("Oadet_char_g", detail.DetCharG);
                values.Add("Oadet_char_h", detail.DetCharH);

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Oadet_id", detail.DetId);

                helper.UpdateOperation("Oa_detail", values,key);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        private string GetWhere(EntityOaTableDetail detail)
        {
            string where =  "where 1=1 ";

            if (!string.IsNullOrEmpty(detail.DetId))
            {
                where += " And Oadet_id = '" + detail.DetId + "'";
            }

            if (!string.IsNullOrEmpty(detail.TabCode))
            {
                where += " And Oadet_Dot_code = '" + detail.TabCode + "'";
            }

            if (!string.IsNullOrEmpty(detail.DetCharA))
            {
                where += " And Oadet_char_a = '" + detail.DetCharA + "'";
            }

            if (!string.IsNullOrEmpty(detail.DetCharB))
            {
                where += " And Oadet_char_b = '" + detail.DetCharB + "'";
            }

            if (!string.IsNullOrEmpty(detail.DetCharC))
            {
                where += " And Oadet_char_c = '" + detail.DetCharC + "'";
            }

            if (!string.IsNullOrEmpty(detail.DetCharD))
            {
                where += " And Oadet_char_d = '" + detail.DetCharD + "'";
            }

            if (detail.StartDate != null)
            {
                where += string.Format(" And Oadet_date >= '{0}'", detail.StartDate.Value.ToString("yyyy-MM-dd hh:mm:ss"));
            }
            if (detail.EndDate != null)
            {
                where += string.Format(" And Oadet_date <= '{0}'", detail.EndDate.Value.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd hh:mm:ss"));
            }
            return where;
        }
    }
}
