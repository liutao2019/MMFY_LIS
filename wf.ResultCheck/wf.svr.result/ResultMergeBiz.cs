using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using dcl.root.dac;
using lis.dto.Entity;
using dcl.common;
using lis.dto;
using System.Diagnostics;
using dcl.root.logon;
using dcl.pub.entities;
using dcl.entity;

namespace dcl.svr.result
{
    public class ResultMergeBiz
    {
        /// <summary>
        /// 获取仪器未审核纪录的病人列表
        /// </summary>
        /// <param name="itr_id"></param>
        /// <param name="pat_date"></param>
        /// <returns></returns>
        public DataTable GetCurrentItrPatientList(string itr_id, DateTime pat_date)
        {
            string sqlSelect = string.Format(@"
select
    pat_id,
    pat_itr_id,
    pat_sid,
    pat_host_order,
    pat_name,
pat_in_no,
pat_dep_name,
    dest_pat_sid = '',
    dest_pat_itr_id = '',
    dest_pat_id='',
    dest_all_itm='1',
    dest_itm_ids='',
    cast(null as datetime) as dest_pat_date
from patients
where pat_itr_id = '{0}'
and (pat_flag = '0' or pat_flag is null)
and pat_date >= @pat_date1
and pat_date < @pat_date2
order by len(pat_host_order),pat_host_order,len(pat_sid),pat_sid
", itr_id);
            SqlCommand cmd = new SqlCommand(sqlSelect);
            cmd.Parameters.AddWithValue("pat_date1", pat_date.Date);
            cmd.Parameters.AddWithValue("pat_date2", pat_date.AddDays(1).Date);


            DBHelper helper = new DBHelper();
            DataTable dt = helper.GetTable(cmd);
            dt.TableName = "GetCurrentItrPatientList";
            return dt;
        }

        /// <summary>
        /// 获取没有病人资料的结果
        /// </summary>
        /// <param name="itr_id"></param>
        /// <param name="pat_date"></param>
        /// <param name="onlyGetNonePatInfoResult">是否只获取无病人资料的结果</param>
        /// <returns></returns>
        public DataTable GetNonePatInfoResult(string itr_id, DateTime res_date, bool onlyGetNonePatInfoResult)
        {
            bool isCopy=itr_id.Split('|').Length>1;

            string subWhere = " and isnull(patients.pat_flag,'') not in (2,4) ";

            if (isCopy)
            {
                itr_id = itr_id.Split('|')[0];

                subWhere = "";
            }
            else
            {


                if (onlyGetNonePatInfoResult)
                {
                    subWhere += " and patients.pat_id is null  ";
                }
            }

            string sqlSelect = string.Format(@"
select
    res_key,
    res_itr_id,
    res_sid,
    res_id,
    --res_sid_int = cast(res_sid as bigint),
    --parent_res_sid = cast(null as bigint),
    res_itm_ecd,
    res_itm_id,
    select_res=cast(1 as bit),
    res_chr,
    res_od_chr,
    pat_date = @res_date1,
    res_date,
    pat_name,
pat_in_no,
pat_dep_name
from resulto with(nolock)
left join patients on  patients.pat_id = resulto.res_id
where
resulto.res_flag = 1 and res_sid is not null and res_sid <> ''
and res_itr_id = '{0}'
and res_date >= @res_date2
and res_date < @res_date3
{1}
--order by len(res_sid),res_sid
", itr_id, subWhere);



            SqlCommand cmd = new SqlCommand(sqlSelect);
            cmd.Parameters.AddWithValue("res_date1", res_date.Date);
            cmd.Parameters.AddWithValue("res_date2", res_date.Date);
            cmd.Parameters.AddWithValue("res_date3", res_date.AddDays(1).Date);


            DBHelper helper = new DBHelper();
            DataTable dt = helper.GetTable(cmd);

            if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains("res_sid"))
            {
                //添加字段排序
                dt.Columns.Add("res_sid_len", typeof(int));
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    dt.Rows[j]["res_sid_len"] = dt.Rows[j]["res_sid"].ToString().Length;
                }
                dt.AcceptChanges();

                DataView dvSotr = dt.DefaultView.ToTable().DefaultView;
                dvSotr.Sort = "res_sid_len asc,res_sid asc";

                dt = dvSotr.ToTable();

                if (dt.Columns.Contains("res_sid_len"))
                {
                    dt.Columns.Remove("res_sid_len");
                    dt.AcceptChanges();
                }
            }

            dt.TableName = "GetNonePatInfoResult";


            //DataTable dtParent = dt.Clone();
            //foreach (DataRow dr in dt.Rows)
            //{
            //    long res_sid_int = Convert.ToInt64(dr["res_sid_int"]);

            //    if (dtParent.Select(string.Format("parent_res_sid = {0} ", res_sid_int)).Length == 0)
            //    {
            //        DataRow drParent = dtParent.NewRow();
            //        drParent["parent_res_sid"] = dr["res_sid_int"];
            //        drParent["pat_date"] = res_date;
            //        drParent["res_itr_id"] = itr_id;
            //        dtParent.Rows.Add(drParent);
            //    }
            //}

            //foreach (DataRow drParent in dtParent.Rows)
            //{
            //    dt.Rows.Add(drParent.ItemArray);
            //}

            return dt;
        }

        public List<EntityOperationResult> Merge(DataTable dtData)
        {
            List<EntityOperationResult> listResult = new List<EntityOperationResult>();


            bool iscopy = dtData.Columns.Contains("IsCopy");

            if (iscopy)
                dtData.Columns.Remove("IsCopy");

            //pat_id,
            //pat_itr_id,
            //pat_sid,
            //pat_host_order,
            //pat_name,
            //dest_pat_sid = '',
            //dest_pat_itr_id = '',
            //cast(null as datetime) as dest_pat_date

            DBHelper helper = new DBHelper();

            string sqlSelectSource = string.Format(@"
select
    *
from resulto
where
    res_sid = @res_sid
    and res_itr_id = @res_itr_id
    and res_date >= @res_date1
    and res_date < @res_date2
    and resulto.res_flag = 1 and res_sid is not null
    and res_sid <> ''
");
            //更新目标结果数据中的重复结果为无效结果
            string sqlUpdateDestDupResult = "update resulto set res_flag = 0 where res_id='{0}' and res_itm_id in ({1})";

            foreach (DataRow drData in dtData.Rows)
            {
                if (!Compare.IsEmpty(drData["dest_pat_sid"]))
                {
                    //源结果样本号
                    string src_pat_sid = drData["dest_pat_sid"].ToString();

                    //源结果时间
                    DateTime src_res_date = Convert.ToDateTime(drData["dest_pat_date"]);

                    //源结果时间
                    string src_itr_id = drData["dest_pat_itr_id"].ToString();

                    //原病人ID
                    string src_pat_id = drData["dest_pat_id"].ToString();

                    //目标病人样本号
                    string dest_res_sid = drData["pat_sid"].ToString();

                    //目标病人ID
                    string dest_pat_id = drData["pat_id"].ToString();

                    //目标仪器ID
                    string dest_pat_itr_id = drData["pat_itr_id"].ToString();

                    SqlCommand cmdSource = new SqlCommand(sqlSelectSource);
                    cmdSource.Parameters.AddWithValue("res_sid", src_pat_sid);
                    cmdSource.Parameters.AddWithValue("res_itr_id", src_itr_id);
                    cmdSource.Parameters.AddWithValue("res_date1", src_res_date.Date);
                    cmdSource.Parameters.AddWithValue("res_date2", src_res_date.AddDays(1).Date);

                    //获取源结果数据
                    DataTable dtSource = helper.GetTable(cmdSource);


                    //过滤只合并勾选了的项目id
                    if (dtSource != null && dtSource.Rows.Count > 0 
                        && drData.Table.Columns.Contains("dest_all_itm")
                        &&drData["dest_all_itm"].ToString()!="1")
                    {
                        string dest_itm_ids = drData["dest_itm_ids"].ToString();

                        if (string.IsNullOrEmpty(dest_itm_ids))
                        {
                            //如果没有选择要合并的项目,则不往下执行
                            continue;
                        }

                        DataTable dtSourceCopy = dtSource.Copy();
                        dtSource.Clear();
                        dtSource.AcceptChanges();
                        foreach (DataRow drSourceCopy in dtSourceCopy.Rows)
                        {
                            if (dest_itm_ids.Contains(drSourceCopy["res_itm_id"].ToString()))
                            {
                                dtSource.Rows.Add(drSourceCopy.ItemArray);
                            }
                        }
                        dtSource.AcceptChanges();

                        if (dtSource.Rows.Count <= 0)
                        {
                            //如果没有选择要合并的项目,则不往下执行
                            continue;
                        }
                    }

                    //删除目标结果数据中的重复结果
                    StringBuilder sb = new StringBuilder();
                    bool needComma = false;
                    foreach (DataRow drSource in dtSource.Rows)
                    {
                        if (needComma)
                        {
                            sb.Append(",");
                        }
                        string itm_id = drSource["res_itm_id"].ToString();

                        sb.Append(string.Format("'{0}'", itm_id));

                        needComma = true;


                        drSource["res_id"] = dest_pat_id;
                        drSource["res_sid"] = dest_res_sid;
                        drSource["res_itr_id"] = dest_pat_itr_id;
                    }

                    string sqlUpdate = string.Format(sqlUpdateDestDupResult, dest_pat_id, sb.ToString());

                    //string sqlUpdateSoureData = string.Format(sqlUpdateDestDupResult, src_pat_id, sb.ToString());
                    List<SqlCommand> cmdsResult;
                    if (iscopy)
                    {
                        cmdsResult = DBTableHelper.GenerateInsertCommand(PatientTable.PatientResultTableName, new string[] { "res_key" }, dtSource);
                    }
                    else
                    {
                        cmdsResult = DBTableHelper.GenerateUpdateCommand(PatientTable.PatientResultTableName, new string[] { "res_key" }, dtSource);
                    }

                    try
                    {
                        using (DBHelper transHelper = DBHelper.BeginTransaction())
                        {
                            //transHelper.ExecuteNonQuery(sqlUpdateSoureData);

                            transHelper.ExecuteNonQuery(sqlUpdate);

                            foreach (SqlCommand cmd in cmdsResult)
                            {
                                transHelper.ExecuteNonQuery(cmd);
                            }
                            transHelper.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteException(this.GetType().Name, "Merge", ex.ToString());
                        //throw;
                    }

                }
            }

            return listResult;
        }
    }
}
