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
    public class ViewBIZ : ICommonBIZ
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
                DataTable dt_bid = ds.Tables["anr_id"];
                String anr_id = dt_bid.Rows[0][0].ToString();
                String sql = @"select anr_bid,bac_cname,bac_ename from an_rlts a,dict_bacteri b 
where anr_id='{0}'and anr_bid=bac_id 
group by anr_bid,bac_cname,bac_ename";
                sql = string.Format(sql, anr_id);
                DataTable dt = dao.GetDataTable(sql);
                dt.TableName = "dict_bacteri";
                result.Tables.Add(dt);
                String sql2 = "select bsr_cname from cs_rlts where bsr_id='" + anr_id + "'";
                DataTable dtRlts = dao.GetDataTable(sql2);
                dtRlts.TableName = "cs_rlts";
                result.Tables.Add(dtRlts);
                return result;
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString())); ;
            }
            return result;

        }


        public DataSet doSearch(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                DataTable dtPa = ds.Tables["para"];
                if (dtPa != null)
                {
                    DateTime data = Convert.ToDateTime(dtPa.Rows[0]["pat_date"]);
                    //string data = dtPa.Rows[0]["pat_date"].ToString();
                    DateTime edate = data.Date;
                    DateTime sdate = data.Date.AddDays(1).AddMilliseconds(-1);
                    string sql = @"select '-1' type_id,'药敏-鉴定结果' type_name,'' type_mic,'' type_anti,'0' type_relevance 
union

select bar_id type_id,'样本号:'+cast(bar_sid as varchar(12)) type_name,'' type_mic,'' type_anti,'-1' type_relevance 
from ba_rlts where bar_mid='{0}' and bar_date>='{1}' and bar_date<='{2}'
group by bar_id,bar_sid
--order by bar_sid
union

select  bar_id+'&'+cast(bar_bid as varchar) type_id,(case  when isnull(bac_id,'')='' then '未知菌株['+cast(bar_bid as varchar)+']' else bac_cname+'('+bac_ename+')' end) type_name,'' type_mic,'' type_anti,bar_id type_relevance from ba_rlts 
left join dict_bacteri on dict_bacteri.bac_id=ba_rlts.bar_bid
where bar_mid='{0}' and bar_date>='{1}' and bar_date<='{2}'
union

select anr_id+'&'+anr_bid+'&'+anr_aid type_id,(case  when isnull(anti_id,'')='' then '未知抗生素['+anr_aid+']' else anti_cname+'('+anti_ename+')' end) type_name,anr_smic1 type_mic,anr_mic type_anti,anr_id+'&'+anr_bid type_relevance from an_rlts 
left join dict_antibio on dict_antibio.anti_id=an_rlts.anr_aid
where anr_mid='{0}' and anr_date>='{1}' and anr_date<='{2}'";
                    sql = string.Format(sql, dtPa.Rows[0]["pat_itr_id"], edate.ToString("yyyy-MM-dd HH:mm:ss"), sdate.ToString("yyyy-MM-dd HH:mm:ss"));
                    DataTable dt = dao.GetDataTable(sql);
                    dt.TableName = "patients";
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

        public DataSet doUpdate(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                DataTable dtPatients = ds.Tables["patients"];
                DataTable dtPatients_mi = ds.Tables["patients_mi"];
                DataTable dict_btype = ds.Tables["ba_rlts"];
                DataTable dtAntibio = ds.Tables["an_rlts"];
                DataTable dtCs_res = ds.Tables["cs_rlts"];
                DataRow drPat = dtPatients.Rows[0];
                String pat_id = drPat["pat_id"].ToString();
                ArrayList arr = dao.GetUpdateSql(dtPatients, new string[] { "pat_id" });
                arr.Add(String.Format("delete from Patients_mi where pat_id='{0}'", pat_id));
                arr.Add(String.Format("delete from ba_rlts where bar_id='{0}'", pat_id));
                arr.Add(String.Format("delete from an_rlts where anr_id='{0}'", pat_id));
                arr.Add(String.Format("delete from cs_rlts where bsr_id='{0}'", pat_id));
                foreach (DataRow dr in dtPatients_mi.Rows)
                {
                    dr["pat_id"] = pat_id;
                }
                if (dict_btype != null)
                {
                    foreach (DataRow drType in dict_btype.Rows)
                    {
                        drType["bar_id"] = pat_id;
                    }
                    arr.AddRange(dao.GetInsertSql(dict_btype));
                }
                if (dtAntibio != null)
                {
                    foreach (DataRow drAnti in dtAntibio.Rows)
                    {
                        drAnti["anr_id"] = pat_id;
                    }
                    arr.AddRange(dao.GetInsertSql(dtAntibio));
                }
                if (dtCs_res != null)
                {
                    foreach (DataRow dtCs in dtCs_res.Rows)
                    {
                        dtCs["bsr_id"] = pat_id;
                    }
                    arr.AddRange(dao.GetInsertSql(dtCs_res));
                }

                arr.AddRange(dao.GetInsertSql(dtPatients_mi));
                dao.DoTran(arr);
                DataTable dt = new DataTable("patients");
                dt.Columns.Add("pat_id");
                dt.Rows.Add(new String[] { pat_id });
                result.Tables.Add(dt);
                return result;
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("更新信息出错!", ex.ToString())); ;
            }
            return result;
        }

        public DataSet doView(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                DataTable dt_bid = ds.Tables["an_rlts"];
                DataTable dt_type = ds.Tables["type"];
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

                if (dt_type != null)
                {
                    String sql = "select * from dict_bscripe where br_flag='" + dt_type.Rows[0]["id"] + "' order by cast(br_seq as int) asc";
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
