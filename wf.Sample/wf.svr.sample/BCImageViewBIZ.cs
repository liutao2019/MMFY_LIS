using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using lis.dto;
using Lib.DAC;
using dcl.root.dto;

namespace dcl.svr.sample
{

    public class BCImageViewBIZ : dcl.svr.root.com.ICommonBIZ
    {
        private DbBase dao = DbBase.InConn();

        #region ICommonBIZ 成员

        public DataSet doDel(DataSet ds)
        {
            throw new NotImplementedException();
        }

        public DataSet doNew(DataSet ds)
        {
            throw new NotImplementedException();
        }

        public DataSet doOther(DataSet ds)
        {
            throw new NotImplementedException();
        }

        public DataSet doSearch(DataSet ds)
        {
            DataSet dsResult = null;
            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables["dtWhere"] != null)
                {
                    string strSqlWhere = ds.Tables["dtWhere"].Rows[0]["colWhere"].ToString(); //添加

                    string strSQL = string.Format(@"select * from bc_blood_pat_img
where 1=1
{0}", strSqlWhere);

                    dsResult = dao.GetDataSet(strSQL);
                    dsResult.Tables[0].TableName = "bc_blood_pat_img";
                }
            }
            catch (Exception objEx)
            {
                Lib.LogManager.Logger.LogException("BCImageViewBIZ", objEx);
            }

            return dsResult;
        }

        public DataSet doUpdate(DataSet ds)
        {
            DataSet dsResult = null;
            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables["dtUpdate"] != null)
                {
                    string bc_bar_code = ds.Tables["dtUpdate"].Rows[0]["bc_bar_code"].ToString(); 
                    byte[] bc_pic = null;
                    if (ds.Tables["dtUpdate"].Rows[0]["bc_pic"] != null && ds.Tables["dtUpdate"].Rows[0]["bc_pic"] is byte[])
                    {
                        bc_pic = (byte[])ds.Tables["dtUpdate"].Rows[0]["bc_pic"];
                    }

                    SqlHelper helper = new SqlHelper();
                    helper.ExecuteNonQuery(string.Format("delete bc_blood_pat_img where bc_bar_code='{0}'", bc_bar_code));
                    string sql = "INSERT bc_blood_pat_img VALUES (? ,?) ";

                    DbCommandEx cmd = helper.CreateCommandEx(sql);
                    cmd.AddParameterValue(bc_bar_code);
                    cmd.AddParameterValue(bc_pic);
                    int rv = helper.ExecuteNonQuery(cmd);

                    dsResult = new DataSet();
                    DataTable dtRv = new DataTable("dtRv");
                    dtRv.Columns.Add("RvMsg");
                    dtRv.Rows.Add(new object[] { rv });
                    dsResult.Tables.Add(dtRv);
                }
            }
            catch (Exception objEx)
            {
                Lib.LogManager.Logger.LogException("BCImageViewBIZ", objEx);
            }

            return dsResult;
        }

        public DataSet doView(DataSet ds)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
