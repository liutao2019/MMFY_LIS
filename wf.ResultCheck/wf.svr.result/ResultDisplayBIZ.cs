using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.root.dac;
using System.Data;
using System.Data.SqlClient;
using dcl.root.logon;
using System.Runtime.InteropServices;

namespace dcl.svr.result
{
    //实时结果视窗中间层 
    public class ResultDisplayBIZ
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="itr_id">仪器ID</param>
        /// <param name="timer">日期</param>
        /// <param name="sid">样本号条件</param>
        /// <returns>DataTable</returns>
        public DataTable getResult(int itr_id, DateTime timer, string sid)
        {
            DBHelper helper = new DBHelper();
            SqlCommand sqlcom1;
            if (sid == "-1")
            {
                string sql = string.Format(@"
                    select 
                    resulto.res_sid,
                    resulto.res_itm_ecd,
                    resulto.res_chr,
                    resulto.res_od_chr,
                    resulto.res_date,
                    resulto.res_itr_id,
                    resulto.res_itr_ori_id,
                    dict_instrmt.itr_name
                    from resulto inner join 
                    dict_instrmt
                    on dict_instrmt.itr_id = resulto.res_itr_id
                    where 
                    res_date >@res_date and res_date <= @res_date1
                    and resulto.res_itr_id=@res_itr_id
                    order by len(res_sid)desc ,res_sid desc ");

                sqlcom1 = new SqlCommand(sql);
                sqlcom1.Parameters.AddWithValue("res_date", timer);
                sqlcom1.Parameters.AddWithValue("res_date1", timer.Date.AddDays(1));
                SqlParameter pItrID = sqlcom1.Parameters.AddWithValue("res_itr_id", itr_id);
                pItrID.DbType = DbType.AnsiString;
            }
            else
            {
                string sql = string.Format(@"
                    select 
                    resulto.res_sid,
                    resulto.res_itm_ecd,
                    resulto.res_chr,
                    resulto.res_od_chr,
                    resulto.res_date,
                    resulto.res_itr_id,
                    resulto.res_itr_ori_id,
                    dict_instrmt.itr_name
                    from resulto inner join 
                    dict_instrmt
                    on dict_instrmt.itr_id = resulto.res_itr_id
                    where 
                    res_date >@res_date and res_date <= @res_date1
                    and resulto.res_itr_id=@res_itr_id
                    and resulto.res_sid='{0}'
                    order by len(res_sid)desc ,res_sid desc ", sid);
                sqlcom1 = new SqlCommand(sql);
                sqlcom1.Parameters.AddWithValue("res_date", timer.Date);
                sqlcom1.Parameters.AddWithValue("res_date1", timer.Date.AddDays(1));
                SqlParameter pItrID = sqlcom1.Parameters.AddWithValue("res_itr_id", itr_id);
                pItrID.DbType = DbType.AnsiString;
            }

            DataTable table = helper.GetTable(sqlcom1);
            table.TableName = "ResultDisplay";
            return table;
        }


        #region
        /// <summary>
        /// 测试用
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public int Sun(int a, int b)
        {
            return a + b;
        }
        #endregion

    }
}
