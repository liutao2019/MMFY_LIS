using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dcl.servececontract;
using dcl.svr.root.com;
using dcl.root.dto;
using System.Collections;
using lis.dto;

namespace dcl.svr.result
{
    public class SetLayoutBIZ : ICommonBIZ
    {
        private DbBase dao = DbBase.InConn();

        #region ICommonBiz成员
        public DataSet doDel(DataSet ds)
        {
            return null;
        }

        public DataSet doNew(DataSet ds)
        {
            DataSet result = new DataSet();

            DataTable dtNew = ds.Tables["dict_antibio"];
            int id = dao.GetID("dict_antibio", dtNew.Rows.Count);
            foreach (DataRow dr in dtNew.Rows)
            {
                dr["anti_id"] = id - dtNew.Rows.Count + 1;
                id++;
            }
            //dtNew.Rows[0]["bac_del"] = '0';
            ArrayList arrNew = dao.GetInsertSql(dtNew);
            for (int i = 0; i < arrNew.Count; i++)
            {
                ArrayList al = new ArrayList();
                al.Add(arrNew[i].ToString());
                try
                {
                    dao.DoTran(al);
                }
                catch (Exception)
                {
                }

            }
            //dao.DoTran(arrNew);
            result.Tables.Add(dtNew.Copy());


            return result;
        }



        public DataSet doOther(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                DataTable dtPat = ds.Tables["sys_form_desig"];
                ArrayList arr = dao.getDeleteSQL(dtPat, new string[] { "des_id", "des_type", "des_object" });
                dao.DoTran(arr);
                return result;
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("删除信息出错!", ex.ToString())); ;
            }
            return result;
        }


        public DataSet doSearch(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                String sql = "select *,0 type_select from dict_type where type_del=0 and type_flag=1";
                    DataTable dt = dao.GetDataTable(sql);
                    dt.TableName = "dict_type";
                    result.Tables.Add(dt);
                    return result;

            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString())); ;
            }
            return result;
        }

        public DataSet doUpdate(DataSet ds)
        {
            return null;
        }

        public DataSet doView(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                DataTable dt_bid = ds.Tables["an_rlts"];
                if (dt_bid != null)
                {
                    String sql = @"select anr_mic,anr_smic1,anti_cname,anti_ename from an_rlts,dict_antibio where anr_id='{0}' and anr_bid='{1}'
and anr_aid=anti_id";
                    sql = string.Format(sql, dt_bid.Rows[0][0].ToString(), dt_bid.Rows[0][1].ToString());
                    DataTable dt = dao.GetDataTable(sql);
                    dt.TableName = "an_rlts";
                    result.Tables.Add(dt);
                    return result;
                }
                else
                {
                    String sql = "select * from dict_bscripe";
                    DataTable dt = dao.GetDataTable(sql);
                    dt.TableName = "dict_bscripe";
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
        #endregion
    }
}
