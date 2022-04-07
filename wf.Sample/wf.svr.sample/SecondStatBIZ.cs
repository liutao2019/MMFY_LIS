using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using dcl.svr.root.com;
using dcl.common;
using dcl.root.dto;
//using lib.client.common;

namespace dcl.svr.sample
{
    class SecondStatBIZ : dcl.svr.root.com.ICommonBIZ
    {
        private DbBase dao = DbBase.InConn();

        #region ICommonBIZ 成员

        public System.Data.DataSet doDel(System.Data.DataSet ds)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataSet doNew(System.Data.DataSet ds)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataSet doOther(System.Data.DataSet ds)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataSet doSearch(System.Data.DataSet ds)
        {
            DataSet result = new DataSet(); 
            try
            {
                DataTable where = ds.Tables["specimenStat"];

                DataTable dtEx = dao.GetDataTable("select * from report where repCode='specimenStat'");
                if (dtEx.Rows.Count > 0)
                {
                    DataRow drWhere = where.Rows[0];
                    string sql = EncryptClass.Decrypt(dtEx.Rows[0]["repSql"].ToString());
                    foreach (DataColumn dc in where.Columns)
                    {
                        sql = sql.Replace(dc.ColumnName, drWhere[dc.ColumnName].ToString());
                    }
                    DataTable an = dao.GetDataSet(sql).Tables[0];
                    an.TableName = "可设计字段";
                    result.Tables.Add(an.Copy());
                    DataTable dt = new DataTable();
                    dt.TableName = "Address";
                    dt.Columns.Add("path");
                    dt.Rows.Add(dtEx.Rows[0]["repAddress"].ToString());
                    result.Tables.Add(dt);
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString())); ;
            }
            return result;
        }

        public System.Data.DataSet doUpdate(System.Data.DataSet ds)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataSet doView(System.Data.DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                DataTable dt = ds.Tables["template"];
                string sql = "select * from tp_template where tp_template.st_name='{0}' AND dbo.tp_template.st_type='{1}'";
                sql = string.Format(sql, dt.Rows[0]["sp_name"],dt.Rows[0]["sp_type"]);
                DataTable dtTem = dao.GetDataTable(sql);
                dtTem.TableName = "tp_template";
                result.Tables.Add(dtTem);
                return result;
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString())); ;
            }
            return result;
        }

        #endregion
    }
}
