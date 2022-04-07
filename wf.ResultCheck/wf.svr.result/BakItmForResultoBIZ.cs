using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dcl.svr.root.com;
using dcl.root.dto;
using System.Collections;
using lis.dto;
using System.Data.OleDb;
using System.Data.SqlClient;
using dcl.root.dac;
using dcl.common;
using dcl.pub.entities;
using Lib.DAC;
using dcl.svr.cache;

namespace dcl.svr.result
{
    public class BakItmForResultoBIZ : ICommonBIZ
    {
        private DbBase dao = DbBase.InConn();

        #region ICommonBIZ 成员

        public DataSet doDel(DataSet ds)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 备份
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataSet doNew(DataSet ds)
        {
            DataSet dsRv = null;
            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables["dtBackup"] != null && ds.Tables["dtBackup"].Rows.Count > 0)
                {

                }
                else
                {
                    throw new Exception("传入参数有误");
                }

                DataRow drBackup = ds.Tables["dtBackup"].Rows[0];

                #region 字段

                string res_id = drBackup["res_id"].ToString();//
                string bak_id = Guid.NewGuid().ToString();//备份id
                DateTime bak_date = ServerDateTime.GetDatabaseServerDateTime();//备份时间
                string res_itm_ids = drBackup["res_itm_ids"].ToString();//项目集
                string res_keys = drBackup["res_keys"].ToString();//主键集
                string sqlappwhere = "";//额外where添加
                
                #endregion

                #region 生成where条件

                if (!string.IsNullOrEmpty(res_itm_ids))
                {
                    string sqlappStr = "";
                    foreach(string strtemp in res_itm_ids.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (string.IsNullOrEmpty(sqlappStr))
                        {
                            sqlappStr = string.Format(" resulto.res_itm_id='{0}' ", strtemp);
                        }
                        else
                        {
                            sqlappStr += string.Format(" or resulto.res_itm_id='{0}' ", strtemp);
                        }
                    }

                    if (!string.IsNullOrEmpty(sqlappStr))
                    {
                        sqlappwhere += string.Format(@" and ({0}) 
", sqlappStr);
                    }
                }

                if (!string.IsNullOrEmpty(res_keys))
                {
                    string sqlappStr = "";
                    foreach (string strtemp in res_keys.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (string.IsNullOrEmpty(sqlappStr))
                        {
                            sqlappStr = string.Format(" resulto.res_key={0} ", strtemp);
                        }
                        else
                        {
                            sqlappStr += string.Format(" or resulto.res_key={0} ", strtemp);
                        }
                    }

                    if (!string.IsNullOrEmpty(sqlappStr))
                    {
                        sqlappwhere += string.Format(@" and ({0}) 
", sqlappStr);
                    }
                }

                #endregion

                #region 新增项目结果备份SQL语句

                string sqlInsertResultoBakitm = string.Format(@"INSERT INTO resulto_bakitm
           ([res_id]
           ,[res_itr_id]
           ,[res_sid]
           ,[res_itm_id]
           ,[res_itm_ecd]
           ,[res_chr]
           ,[res_od_chr]
           ,[res_cast_chr]
           ,[res_unit]
           ,[res_price]
           ,[res_ref_l]
           ,[res_ref_h]
           ,[res_ref_exp]
           ,[res_ref_flag]
           ,[res_meams]
           ,[res_date]
           ,[res_flag]
           ,[res_type]
           ,[res_rep_type]
           ,[res_com_id]
           ,[res_itm_rep_ecd]
           ,[res_itr_ori_id]
           ,[res_ref_type]
           ,[res_exp]
           ,[res_recheck_flag]
           ,[res_chr2]
           ,[res_chr3]
           ,bak_id
           ,bak_date)
select [res_id]
           ,[res_itr_id]
           ,[res_sid]
           ,[res_itm_id]
           ,[res_itm_ecd]
           ,[res_chr]
           ,[res_od_chr]
           ,[res_cast_chr]
           ,[res_unit]
           ,[res_price]
           ,[res_ref_l]
           ,[res_ref_h]
           ,[res_ref_exp]
           ,[res_ref_flag]
           ,[res_meams]
           ,[res_date]
           ,[res_flag]
           ,[res_type]
           ,[res_rep_type]
           ,[res_com_id]
           ,[res_itm_rep_ecd]
           ,[res_itr_ori_id]
           ,[res_ref_type]
           ,[res_exp]
           ,[res_recheck_flag]
           ,[res_chr2]
           ,[res_chr3]
           ,'{1}'
           ,'{2}'
from resulto
where res_id='{0}'
and res_flag=1
{3}
", res_id, bak_id, bak_date, sqlappwhere);


                #endregion


                //新增项目备份表resulto_bakitm的语句
                SqlCommand cmdAddResultoBakitm = new SqlCommand(sqlInsertResultoBakitm);

                using (DBHelper helper = DBHelper.BeginTransaction())//事务
                {
                    helper.ExecuteNonQuery(cmdAddResultoBakitm);//新增备份信息

                    helper.Commit();//提交事务
                }

                DataTable dtRv = new DataTable("dtMsg");
                dtRv.Columns.Add("msg");
                dtRv.Rows.Add(new object[] {"OK"});
                dsRv = new DataSet();
                dsRv.Tables.Add(dtRv);
            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException(this.GetType().Name, "doNew", ex.ToString());
                DataTable dtEx = new DataTable("dtEx");
                dtEx.Columns.Add("msg");
                dtEx.Rows.Add(new object[] { ex.Message });
                if (dsRv == null) dsRv = new DataSet();
                dsRv.Tables.Clear();//清除
                dsRv.Tables.Add(dtEx);
            }

            return dsRv;
        }

        public DataSet doOther(DataSet ds)
        {
            DataSet dsRv = null;
            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables["dtRevert"] != null && ds.Tables["dtRevert"].Rows.Count > 0)
                {

                }
                else
                {
                    throw new Exception("传入参数有误");
                }

                DataRow drRevert = ds.Tables["dtRevert"].Rows[0];

                #region 字段

                string res_id = drRevert["res_id"].ToString();//
                string res_itm_ids = drRevert["res_itm_ids"].ToString();//项目集--不为空
                string res_keys = drRevert["res_keys"].ToString();//主键集--不为空
                string sqldelwhere = "";//删除原来的信息
                string sqladdwhere = "";//还原备份的信息

                #endregion

                #region 生成where条件

                if (!string.IsNullOrEmpty(res_itm_ids))
                {
                    string sqlappStr = "";
                    foreach (string strtemp in res_itm_ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (string.IsNullOrEmpty(sqlappStr))
                        {
                            sqlappStr = string.Format(" resulto.res_itm_id='{0}' ", strtemp);
                        }
                        else
                        {
                            sqlappStr += string.Format(" or resulto.res_itm_id='{0}' ", strtemp);
                        }
                    }

                    if (!string.IsNullOrEmpty(sqlappStr))
                    {
                        sqldelwhere += string.Format(@" and ({0}) 
", sqlappStr);
                    }
                }

                if (!string.IsNullOrEmpty(res_keys))
                {
                    string sqlappStr = "";
                    foreach (string strtemp in res_keys.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (string.IsNullOrEmpty(sqlappStr))
                        {
                            sqlappStr = string.Format(" resulto_bakitm.res_key={0} ", strtemp);
                        }
                        else
                        {
                            sqlappStr += string.Format(" or resulto_bakitm.res_key={0} ", strtemp);
                        }
                    }

                    if (!string.IsNullOrEmpty(sqlappStr))
                    {
                        sqladdwhere = string.Format(@" where ({0}) 
", sqlappStr);
                    }
                }

                #endregion

                #region 删除要还原的项目结果信息

                string sqlDelResulto = string.Format(@"delete from resulto where res_id='{0}' and res_flag=1
{1}", res_id,sqldelwhere);

                #endregion

                #region 还原备份的项目结果SQL语句

                string sqlInsertResultoBakitmTo = string.Format(@"INSERT INTO resulto
           ([res_id]
           ,[res_itr_id]
           ,[res_sid]
           ,[res_itm_id]
           ,[res_itm_ecd]
           ,[res_chr]
           ,[res_od_chr]
           ,[res_cast_chr]
           ,[res_unit]
           ,[res_price]
           ,[res_ref_l]
           ,[res_ref_h]
           ,[res_ref_exp]
           ,[res_ref_flag]
           ,[res_meams]
           ,[res_date]
           ,[res_flag]
           ,[res_type]
           ,[res_rep_type]
           ,[res_com_id]
           ,[res_itm_rep_ecd]
           ,[res_itr_ori_id]
           ,[res_ref_type]
           ,[res_exp]
           ,[res_recheck_flag]
           ,[res_chr2]
           ,[res_chr3])
select [res_id]
           ,[res_itr_id]
           ,[res_sid]
           ,[res_itm_id]
           ,[res_itm_ecd]
           ,[res_chr]
           ,[res_od_chr]
           ,[res_cast_chr]
           ,[res_unit]
           ,[res_price]
           ,[res_ref_l]
           ,[res_ref_h]
           ,[res_ref_exp]
           ,[res_ref_flag]
           ,[res_meams]
           ,[res_date]
           ,[res_flag]
           ,[res_type]
           ,[res_rep_type]
           ,[res_com_id]
           ,[res_itm_rep_ecd]
           ,[res_itr_ori_id]
           ,[res_ref_type]
           ,[res_exp]
           ,[res_recheck_flag]
           ,[res_chr2]
           ,[res_chr3]
from resulto_bakitm
{0}
", sqladdwhere);


                #endregion


                //删除要还原的项目结果信息
                SqlCommand cmdDelResulto = new SqlCommand(sqlDelResulto);
                //还原备份的项目结果的语句
                SqlCommand cmdAddResultoBakitmTo = new SqlCommand(sqlInsertResultoBakitmTo);

                using (DBHelper helper = DBHelper.BeginTransaction())//事务
                {
                    helper.ExecuteNonQuery(cmdDelResulto);//删除要还原
                    helper.ExecuteNonQuery(cmdAddResultoBakitmTo);//还原备份

                    helper.Commit();//提交事务
                }

                DataTable dtRv = new DataTable("dtMsg");
                dtRv.Columns.Add("msg");
                dtRv.Rows.Add(new object[] { "OK" });
                dsRv = new DataSet();
                dsRv.Tables.Add(dtRv);
            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException(this.GetType().Name, "doOther", ex.ToString());
                DataTable dtEx = new DataTable("dtEx");
                dtEx.Columns.Add("msg");
                dtEx.Rows.Add(new object[] { ex.Message });
                if (dsRv == null) dsRv = new DataSet();
                dsRv.Tables.Clear();//清除
                dsRv.Tables.Add(dtEx);
            }

            return dsRv;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataSet doSearch(DataSet ds)
        {
            DataSet dsResult = null;

            if (ds != null && ds.Tables.Count > 0 && ds.Tables["dtbakdata"] != null)
            {
                string res_id = ds.Tables["dtbakdata"].Rows[0]["res_id"].ToString();

                string strSQL = string.Format(@"select res_id,bak_id,bak_date from resulto_bakitm whit(nolock)
where res_id='{0}' and bak_id is not null and bak_date is not null ", res_id);

                try
                {
                    dsResult = dao.GetDataSet(strSQL);
                    if (dsResult != null && dsResult.Tables.Count > 0)
                    {
                        dsResult.Tables[0].TableName = "dtResBakdata";

                        //排序并且，筛选不同备份日期
                        if (dsResult.Tables[0] != null && dsResult.Tables[0].Rows.Count > 0)
                        {
                            DataView dv = dsResult.Tables[0].DefaultView.ToTable().DefaultView;
                            dv.Sort = "bak_date asc";
                            dsResult.Tables[0].Rows.Clear();
                            foreach (DataRow dr in dv.ToTable().Rows)
                            {
                                //过滤不同的备份号
                                if (dsResult.Tables[0].Select(string.Format("bak_id='{0}'", dr["bak_id"])).Length <= 0)
                                {
                                    dsResult.Tables[0].Rows.Add(dr.ItemArray);
                                }
                            }
                        }
                    }
                }
                catch (Exception objEx)
                {
                    dcl.root.logon.Logger.WriteException("BakItmForResultoBIZ", "doSearch", objEx.ToString());
                }
            }
            else if (ds != null && ds.Tables.Count > 0 && ds.Tables["dtbakitm"] != null)
            {
                string bak_id = ds.Tables["dtbakitm"].Rows[0]["bak_id"].ToString();

                string strSQL = string.Format(@"select
bak_id,
dict_item.itm_name,
res_itm_ecd,
res_itm_id,
res_chr,
res_unit,
case when isnull(res_ref_l,'')<>'' and isnull(res_ref_h,'')<>'' then isnull(res_ref_l,'')+' - '+isnull(res_ref_h,'')
when isnull(res_ref_l,'')<>'' then isnull(res_ref_l,'')
when isnull(res_ref_h,'')<>'' then isnull(res_ref_h,'') else '' end as res_ref_range,
res_id,
res_key
from resulto_bakitm with(nolock)
left join dict_item on resulto_bakitm.res_itm_id=dict_item.itm_id
where bak_id='{0}'", bak_id);

                try
                {
                    dsResult = dao.GetDataSet(strSQL);
                    if (dsResult != null && dsResult.Tables.Count > 0)
                    {
                        dsResult.Tables[0].TableName = "dtResBakitm";
                    }
                }
                catch (Exception objEx)
                {
                    dcl.root.logon.Logger.WriteException("BakItmForResultoBIZ", "doSearch", objEx.ToString());
                }
            }

            return dsResult;
        }

        public DataSet doUpdate(DataSet ds)
        {
            throw new NotImplementedException();
        }

        public DataSet doView(DataSet ds)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
