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
    /// <summary>
    ///  镜检模块业务层
    /// </summary>
    public class MicroscopeBIZ
    {
        //DBHelper helper;

        public int Add(int a, int b)
        {
            return a + b;
        }

        /// <summary>
        /// 返回DATATABLE 仪器默认组合查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable getTable(string instrument)
        {
            DBHelper helper = new DBHelper();
            //string sql = "select com_id,com_name from dict_combine where com_id = " +
            //    "(select pat_com_id from patients_mi where pat_id ='" + instrument + "')";
            string sql = "select com_id,com_name from dict_combine where com_id = (select itr_com_id from dict_instrmt where itr_id='" + instrument + "')";
            DataTable table = helper.GetTable(sql);
            table.TableName = "table";
            return table;
        }

        /// <summary>
        /// 返回DATATABLE 病人组合查询
        /// </summary>
        /// <param name="stylebook"></param>
        /// <returns></returns>
        public DataTable getCom(string stylebook)
        {
            DBHelper helper = new DBHelper();
            //string sql = "select com_id,com_name from dict_combine where com_id in (select pat_com_id from patients_mi where pat_id='" + stylebook + "')";
            string sql = "select top 1 pat_c_name from patients where pat_id = '" + stylebook + "'";
            DataTable table = helper.GetTable(sql);
            table.TableName = "table";
            return table;
        }

        /// <summary>
        /// 病人信息
        /// </summary>
        /// <param name="stylebook"></param>
        /// <returns></returns>
        public DataTable getPatients(string stylebook)
        {
            DBHelper helper = new DBHelper();
            string sql = "select pat_id,pat_flag from patients where pat_id='" + stylebook + "'";
            DataTable table = helper.GetTable(sql);
            table.TableName = "table";
            return table;
        }

        /// <summary>
        /// 返回DATATABLE 组合查询
        /// </summary>
        /// <param name="sql"></param>
        ///<returns></returns>
        #region
        public DataTable getTable(string pat_id, string com_id, int state)
        {
            DBHelper helper = new DBHelper();
            string sql = "";
            if (state == 0)
            {
                sql = string.Format(@"
select 
*
from
dict_combine_mi
left join resulto on dict_combine_mi.com_itm_id = resulto.res_itm_id and resulto.res_id ='{0}'
left join dict_item on dict_combine_mi.com_itm_id = dict_item.itm_id
where dict_combine_mi.com_id = '{1}' and (dict_item.itm_ugr_flag = '0' or dict_item.itm_ugr_flag is null) and (resulto.res_flag=1 or resulto.res_flag is null)
order by dict_combine_mi.com_sort asc
                ", pat_id, com_id);
            }

            if (state == 1)
            {

                sql = string.Format(@"
select 
*
from
dict_combine_mi
left join resulto on dict_combine_mi.com_itm_id = resulto.res_itm_id and resulto.res_id ='{0}'
left join dict_item on dict_combine_mi.com_itm_id = dict_item.itm_id
where dict_combine_mi.com_id = '{1}' and dict_item.itm_ugr_flag = '1' and (resulto.res_flag=1 or resulto.res_flag is null)
order by dict_combine_mi.com_sort asc
                ", pat_id, com_id);
            }
            DataTable table = helper.GetTable(sql);
            table.TableName = "table";
            return table;
        }
        #endregion

        /// <summary>
        /// 返回DATATABLE 组合查询
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public DataTable getTable1(string pat_id)
        {
            DBHelper helper = new DBHelper();
            string sql = "";
            sql = string.Format(@"
                    select 
                    dict_item.itm_id,
                    dict_item.itm_ecd,
                    dict_item.itm_name,
                    dict_combine_mi.com_id,
                    resulto.res_chr,
                    input_value='',
                    itm_ugr_flag = case when dict_item.itm_ugr_flag is null then 0
                                   else dict_item.itm_ugr_flag end
                    from patients_mi
                    left join dict_combine_mi on dict_combine_mi.com_id = patients_mi.pat_com_id
                    left join resulto on resulto.res_id = patients_mi.pat_id and resulto.res_itm_id = dict_combine_mi.com_itm_id and resulto.res_flag=1 
                    inner join dict_item on dict_item.itm_id = dict_combine_mi.com_itm_id
                    where pat_id = '{0}'", pat_id);

            DataTable table = helper.GetTable(sql);
            table.TableName = "table";
            return table;
        }



        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public int DoSomething(DataTable table)
        {
            //helper = new DBHelper();
            string sql = "";
            int success = 0;
            try
            {
                using (DBHelper helper1 = DBHelper.BeginTransaction())
                {
                    #region 数据处理
                    for (int i = 0; i < table.Rows.Count; i++)
                    {

                        //DBHelper helper = new DBHelper();
                        string sql1 = string.Format("select * from resulto where res_id=@res_id and res_itm_id=@res_itm_id");
                        SqlCommand sqlcmd = new SqlCommand(sql1);
                        SqlParameter p1 = sqlcmd.Parameters.AddWithValue("res_id", table.Rows[i]["res_id"]);
                        p1.DbType = DbType.AnsiString;

                        SqlParameter p2 = sqlcmd.Parameters.AddWithValue("res_itm_id", table.Rows[i]["res_itm_id"]);
                        p2.DbType = DbType.AnsiString;
                        // table.Rows[i]["res_id"], table.Rows[i]["res_itm_id"]);
                        DataTable tb = helper1.GetTable(sqlcmd);


                        if (tb.Rows.Count == 0)
                        {
                            //                            sql = string.Format(@"
                            //insert into resulto(res_id,res_itr_id,res_sid,res_itm_ecd,res_itm_id,res_chr,
                            //res_flag,res_date,res_type,res_rep_type,res_com_id,res_itr_ori_id)
                            //values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}',{9},{10},'{11}')"
                            //                                , table.Rows[i][0], table.Rows[i][1], table.Rows[i][2], table.Rows[i][3], table.Rows[i][4], table.Rows[i][5], table.Rows[i][6],
                            //                                         table.Rows[i][7], table.Rows[i][8], table.Rows[i][9], table.Rows[i][10], table.Rows[i][11]);

                            //                            helper.ExecuteNonQuery(sql);
                            sql = string.Format(@"
insert into resulto(res_id,res_itr_id,res_sid,res_itm_ecd,res_itm_id,res_chr,
res_flag,res_date,res_type,res_rep_type,res_com_id,res_itr_ori_id)
values(@res_id,@res_itr_id,@res_sid,@res_itm_ecd,@res_itm_id,@res_chr,
@res_flag,@res_date,@res_type,@res_rep_type,@res_com_id,@res_itr_ori_id)
");
                            SqlCommand sqlcmd1 = new SqlCommand(sql);
                            sqlcmd1.Parameters.AddWithValue("res_id", table.Rows[i][0]);
                            sqlcmd1.Parameters.AddWithValue("res_itr_id", table.Rows[i][1]);
                            sqlcmd1.Parameters.AddWithValue("res_sid", table.Rows[i][2]);
                            sqlcmd1.Parameters.AddWithValue("res_itm_ecd", table.Rows[i][3]);
                            sqlcmd1.Parameters.AddWithValue("res_itm_id", table.Rows[i][4]);
                            sqlcmd1.Parameters.AddWithValue("res_chr", table.Rows[i][5]);
                            sqlcmd1.Parameters.AddWithValue("res_flag", table.Rows[i][6]);
                            sqlcmd1.Parameters.AddWithValue("res_date", table.Rows[i][7]);
                            sqlcmd1.Parameters.AddWithValue("res_type", table.Rows[i][8]);
                            sqlcmd1.Parameters.AddWithValue("res_rep_type", table.Rows[i][9]);
                            sqlcmd1.Parameters.AddWithValue("res_com_id", table.Rows[i][10]);
                            sqlcmd1.Parameters.AddWithValue("res_itr_ori_id", table.Rows[i][11]);

                            helper1.ExecuteNonQuery(sqlcmd1);
                        }
                        else
                        {
                            sql = string.Format("update resulto set res_chr = @res_chr where res_id=@res_id and res_itm_id=@res_itm_id");
                            SqlCommand sqlcmd2 = new SqlCommand(sql);
                            sqlcmd2.Parameters.AddWithValue("res_chr", table.Rows[i]["res_chr"]);
                            sqlcmd2.Parameters.AddWithValue("res_id", table.Rows[i]["res_id"]);
                            sqlcmd2.Parameters.AddWithValue("res_itm_id", table.Rows[i]["res_itm_id"]);
                            helper1.ExecuteNonQuery(sqlcmd2);

                            //, table.Rows[i]["res_chr"], table.Rows[i]["res_id"], table.Rows[i]["res_itm_id"]);
                        }

                    }
                    #endregion
                    helper1.Commit();
                }

                success = 1;
            }
            catch (Exception ex)
            {
                //helper1.RollBack();
                success = -1;
                Logger.WriteException("MicroscopeBiz", "插入病人结果", ex.ToString());
            }
            return success;

        }

        /// <summary>
        /// 保存图像结果
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public int saveImage(DataTable tb)
        {
            try
            {
                DBHelper helper = new DBHelper();

                string sql = string.Format(@"
                insert into resulto_p(pres_id, pres_it_ecd, pres_date, pres_sid, pres_mid, pres_chr, pres_flag)
                              values(@pres_id,@pres_it_ecd,@pres_date,@pres_sid,@pres_mid,@pres_chr,@pres_flag)");

                //            string sql = string.Format(@"
                //                insert into resulto_p(pres_id, pres_it_ecd, pres_date, pres_sid, pres_mid, pres_flag)
                //                              values(@pres_id,@pres_it_ecd,@pres_date,@pres_sid,@pres_mid,@pres_flag)");
                SqlCommand sqlcom = new SqlCommand(sql);
                sqlcom.Parameters.AddWithValue("pres_id", tb.Rows[0]["pres_id"]);
                sqlcom.Parameters.AddWithValue("pres_it_ecd", tb.Rows[0]["pres_it_ecd"]);
                sqlcom.Parameters.AddWithValue("pres_date", tb.Rows[0]["pres_date"]);
                sqlcom.Parameters.AddWithValue("pres_sid", tb.Rows[0]["pres_sid"]);
                sqlcom.Parameters.AddWithValue("pres_mid", tb.Rows[0]["pres_mid"]);

                sqlcom.Parameters.AddWithValue("pres_chr", tb.Rows[0]["pres_chr"]);

                //SqlParameter paramImage = new SqlParameter("pres_chr", SqlDbType.Image);
                //paramImage.Value = (byte[])tb.Rows[0]["pres_chr"];
                //sqlcom.Parameters.Add(paramImage);
                sqlcom.Parameters.AddWithValue("pres_flag", tb.Rows[0]["pres_flag"]);

                int i = helper.ExecuteNonQuery(sqlcom);
                return i;
            }
            catch
            {
                return 0;
            }
        }

        //public int DoSomething(DataTable table)
        //{
        //    DBHelper helper = new DBHelper();
        //    DataTable dt = helper.GetTable("select * from patients");

        //    int success = 0;

        //    try
        //    {
        //        using (DBHelper helper = DBHelper.BeginTransaction())
        //        {
        //            helper.ExecuteNonQuery("delete from xxxx");
        //            foreach (DataRow row in table.Rows)
        //            {
        //                string sql = "insert into xxxx";


        //                helper.ExecuteNonQuery(sql);
        //            }
        //            helper.Commit();
        //        }

        //        success = 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        success = -1;
        //        Logger.WriteException("MicroscopeBIZ", "插入病人结果", ex.ToString());
        //    }


        //    return success;
        //}

    }




}
