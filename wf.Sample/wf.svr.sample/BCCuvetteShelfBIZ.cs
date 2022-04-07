using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using lis.dto;
using dcl.root.dac;
using System.Data.SqlClient;
using lis.dto.Entity;
using Lib.LogManager;
using dcl.svr.frame;
using dcl.pub.entities;
using dcl.svr.cache;
using dcl.entity;

namespace dcl.svr.sample
{
    /// <summary>
    /// 试管架条码登记
    /// </summary>
    public class BCCuvetteShelfBIZ : BIZBase
    {
        public override string MainTable
        {
            get { return BarcodeTable.CuvetteShelf.TableName; }
        }

        public override string PrimaryKey
        {
            get { return BarcodeTable.CuvetteShelf.ID; }
        }

        public override DataSet Search(string where)
        {
            return doSearch(new DataSet(), SearchSQL + where);
        }

        /// <summary>
        /// 获取当天登记试管架
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="cuvsheldid"></param>
        /// <returns></returns>
        public DataTable GetCuvetteRegisteredBarcodeInfo(string deptid, DateTime depTime)
        {
            //            string sqlSelect = string.Format(@"
            //select
            //bc_stand_temp.st_id,
            //bc_stand_temp.st_type,--接收室名称
            //dict_type.type_name,
            //bc_stand_temp.st_date,
            //bc_stand_temp.st_bar_code,
            //bc_stand_temp.st_no,
            //bc_stand_temp.st_cus_code,
            //bc_stand_temp.st_etagere,
            //bc_stand_temp.st_place_x,
            //bc_stand_temp.st_place_y,
            //bc_stand_temp.st_i_code,
            //bc_stand_temp.st_bc_cname,
            //
            //bc_patients.bc_ori_id,
            //dict_origin.ori_name,--病人来源
            //
            //bc_patients.bc_name,
            //bc_patients.bc_in_no,--住院号
            //bc_patients.bc_sex,
            //bc_patients.bc_age,
            //bc_patients.bc_d_name,
            //bc_patients.bc_bed_no,
            //bc_patients.bc_doct_name,
            //bc_patients.bc_times, --住院次数
            //bc_patients.bc_diag, --临床诊断
            //poweruserinfo.username,
            //bc_patients.bc_sam_id,
            //bc_patients.bc_sam_name,
            //st_bc_occ_date = (select top 1 bc_occ_date from bc_cname where bc_stand_temp.st_bar_code = bc_cname.bc_bar_code)
            //
            //
            //from
            //bc_stand_temp
            //left join dict_type on dict_type.type_id = bc_stand_temp.st_type
            //inner join bc_patients on bc_bar_code = bc_stand_temp.st_bar_code
            //left join dict_origin on dict_origin.ori_id = bc_patients.bc_ori_id
            //left join poweruserinfo on bc_stand_temp.st_i_code = poweruserinfo.userInfoId
            //where bc_stand_temp.st_type = '{0}' and bc_stand_temp.st_date >= @st_date1 and bc_stand_temp.st_date < @st_date2
            //order by bc_stand_temp.st_no asc
            //", deptid);
            string sqlSelect = string.Format(@"
select
cast(0 as bit) as  pat_select,
bc_stand_temp.st_id,
bc_stand_temp.st_no,
bc_stand_temp.st_bc_cname,
bc_stand_temp.st_bar_code,
bc_patients.bc_name
from bc_stand_temp
inner join bc_patients on bc_bar_code = bc_stand_temp.st_bar_code
where bc_stand_temp.st_type = '{0}' and bc_stand_temp.st_date >= @st_date1 and bc_stand_temp.st_date < @st_date2
order by bc_stand_temp.st_no
", deptid);

            DBHelper helper = new DBHelper();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlSelect; ;
            cmd.Parameters.AddWithValue("st_date1", depTime);
            cmd.Parameters.AddWithValue("st_date2", depTime.AddDays(1).AddSeconds(-1));


            DataTable dt = helper.GetTable(cmd);
            dt.TableName = "GetCuvetteRegisteredBarcodeInfo";
            return dt;
        }


        public DataTable GetCuvDetails(long st_id)
        {
            string sqlSelect = string.Format(@"
Select
            bc_stand_temp.st_id,
            bc_stand_temp.st_type,--接收室名称
            dict_type.type_name,
            bc_stand_temp.st_date,
            bc_stand_temp.st_bar_code,
            bc_stand_temp.st_no,
            bc_stand_temp.st_cus_code,
            bc_stand_temp.st_etagere,
            bc_stand_temp.st_place_x,
            bc_stand_temp.st_place_y,
            bc_stand_temp.st_i_code,
            bc_stand_temp.st_bc_cname,
            bc_patients.bc_ori_id,
            dict_origin.ori_name,--病人来源
            
            bc_patients.bc_name,
            bc_patients.bc_in_no,--住院号
            bc_patients.bc_upid,--住院号
            bc_patients.bc_sex,
            bc_patients.bc_age,
            bc_patients.bc_d_name,
            bc_patients.bc_bed_no,
            bc_patients.bc_doct_name,
            bc_patients.bc_times, --住院次数
            bc_patients.bc_diag, --临床诊断
            poweruserinfo.username,
            bc_patients.bc_sam_id,
            bc_patients.bc_sam_name,
            st_bc_occ_date = (select top 1 bc_occ_date from bc_cname where bc_stand_temp.st_bar_code = bc_cname.bc_bar_code)
From bc_stand_temp
            left join dict_type on dict_type.type_id = bc_stand_temp.st_type
            inner join bc_patients on bc_bar_code = bc_stand_temp.st_bar_code
            left join dict_origin on dict_origin.ori_id = bc_patients.bc_ori_id
            left join poweruserinfo on bc_stand_temp.st_i_code = poweruserinfo.userInfoId
WHERE bc_stand_temp.st_id = @st_id");

            SqlCommand cmd = new SqlCommand(sqlSelect);
            cmd.Parameters.AddWithValue("st_id", st_id);

            DataTable dt = new DBHelper().GetTable(cmd);
            dt.TableName = "GetCuvDetails";
            return dt;
        }

        /// <summary>
        /// 判断当天是否已录入指定的条码号
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        private bool ExistShelfBarcode(string barcode, DateTime date)
        {
            //            string sqlSelect = string.Format(@"
            //select
            //st_id
            //from bc_stand_temp
            //where st_bar_code = '{0}' and st_date >= @st_date1 and st_date < @st_date2
            //", barcode);

            //            SqlCommand cmd = new SqlCommand();
            //            cmd.CommandText = sqlSelect;
            //            cmd.Parameters.AddWithValue("st_date1", DateTime.Now.Date);
            //            cmd.Parameters.AddWithValue("st_date2", DateTime.Now.AddDays(1).Date);

            //            DBHelper helper = new DBHelper();
            //            object obj = helper.ExecuteScalar(cmd);
            //            if (obj != null)
            //            {
            //                return true;
            //            }
            //            else
            //            {
            //                return false;
            //            }
            //20120920
            string sqlSelect = string.Format(@"
select
st_id
from bc_stand_temp
where st_bar_code = '{0}' and st_date >= @st_date1 and st_date < @st_date2
", barcode);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlSelect;
            cmd.Parameters.AddWithValue("st_date1", date.Date);
            cmd.Parameters.AddWithValue("st_date2", date.AddDays(1).Date);

            DBHelper helper = new DBHelper();
            object obj = helper.ExecuteScalar(cmd);
            if (obj != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 顺序号是否存在
        /// </summary>
        /// <param name="seqno"></param>
        /// <param name="deptid"></param>
        /// <returns></returns>
        private bool ExistSeqNo(int seqno, string deptid)
        {
            string sqlSelect = string.Format(@"
select
st_id
from bc_stand_temp
where st_type = '{0}' and st_no = {1} and st_date >= @st_date1 and st_date < @st_date2
", deptid, seqno);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlSelect;
            cmd.Parameters.AddWithValue("st_date1", DateTime.Now.Date);
            cmd.Parameters.AddWithValue("st_date2", DateTime.Now.AddDays(1).Date);

            DBHelper helper = new DBHelper();
            object obj = helper.ExecuteScalar(cmd);
            if (obj != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 保存试管架条码信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns>成功状态 1保存成功  -1条码号存在  -2顺序号存在  -3保存异常</returns>
        public int SaveShelfBarcode(EntityBCStandTemp data)
        {

            DateTime stDate = ServerDateTime.GetDatabaseServerDateTime();

            if (data.st_date == stDate.Date)
                data.st_date = stDate;
            else
                data.st_date = data.st_date.AddHours(stDate.Hour).AddMinutes(stDate.Minute).AddSeconds(stDate.Second);


            int ret = 1;

            try
            {
                if (ExistShelfBarcode(data.st_bar_code, data.st_date))
                {
                    return -1;
                }

                if (ExistSeqNo(data.st_no, data.st_type))
                {
                    return -2;
                }
                if (Returned(data.st_bar_code))
                {
                    return -3;
                }

                string sqlInsert = string.Format(@"
insert into bc_stand_temp(st_type,st_date,st_bar_code,st_no,st_cus_code,st_etagere,st_place_x,st_place_y,st_i_code,st_bc_cname) values(@st_type,@st_date,@st_bar_code,@st_no,@st_cus_code,@st_etagere,@st_place_x,@st_place_y,@st_i_code,@st_bc_cname);
select scope_identity();
");

                DBHelper helper = new DBHelper();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlInsert;
                cmd.Parameters.AddWithValue("st_type", data.st_type);
                cmd.Parameters.AddWithValue("st_date", data.st_date);
                cmd.Parameters.AddWithValue("st_bar_code", data.st_bar_code);
                cmd.Parameters.AddWithValue("st_no", data.st_no);
                cmd.Parameters.AddWithValue("st_cus_code", data.st_cus_code);
                cmd.Parameters.AddWithValue("st_etagere", data.st_etagere);
                cmd.Parameters.AddWithValue("st_place_x", data.st_place_x);
                cmd.Parameters.AddWithValue("st_place_y", data.st_place_y);
                cmd.Parameters.AddWithValue("st_i_code", data.st_i_code);

                //data.st_bc_cname == null ? DBNull.Value : data.st_bc_cname

                if (data.st_bc_cname == null)
                {
                    cmd.Parameters.AddWithValue("st_bc_cname", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("st_bc_cname", data.st_bc_cname);
                }

                object obj = helper.ExecuteScalar(cmd);

                ret = Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                Logger.LogException("SaveShelfBarcode", ex);
                ret = -3;
            }

            return ret;
        }

        /// <summary>
        /// 删除试管架条码信息
        /// </summary>
        /// <param name="st_id"></param>
        /// <returns></returns>
        public EntityOperationResult DeleteShelfBarcode(Int64 st_id)
        {
            string sqlDelete = @"delete from bc_stand_temp where st_id = @st_id";

            EntityOperationResult result = new EntityOperationResult();
            //result.Key = st_id.ToString();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlDelete;

            cmd.Parameters.AddWithValue("st_id", st_id);

            DBHelper helper = new DBHelper();

            try
            {
                int rowsAffect = helper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                result.AddMessage(EnumOperationErrorCode.Exception, EnumOperationErrorLevel.Error);
                Logger.LogException("DeleteShelfBarcode", ex);
            }

            return result;
        }

        /// <summary>
        /// 删除试管架条码信息时，插入bc_sign跟踪记录 20120920
        /// </summary>
        /// <param name="st_id"></param>
        /// <returns></returns>
        public EntityOperationResult insertBcsignShelfBarcode(string bc_login_id, string bc_name, string bc_bar_no, string bc_remark)
        {
            DateTime time = ServerDateTime.GetDatabaseServerDateTime();
            string sqlInsertBcSign = string.Format(@"insert into bc_sign(bc_date, bc_login_id, bc_name, bc_status, bc_bar_no, bc_bar_code, bc_remark)
                                       values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", time, bc_login_id, bc_name, 530, bc_bar_no, bc_bar_no, "组合：" + bc_remark);
            EntityOperationResult result = new EntityOperationResult();
            //result.Key = st_id.ToString();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlInsertBcSign;
            //cmd.Parameters.AddWithValue("st_id", st_id);

            DBHelper helper = new DBHelper();

            try
            {
                int rowsAffect = helper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                result.AddMessage(EnumOperationErrorCode.Exception, EnumOperationErrorLevel.Error);
                Logger.LogException("insertBcsignShelfBarcode", ex);
            }

            return result;
        }

        /// <summary>
        /// 判断条码号是否已回退
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public bool Returned(string barCode)
        {
            string sql = string.Format(@"select bc_status from bc_patients where bc_bar_no = '{0}'", barCode);

            DataTable dt = new DBHelper().GetTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["bc_status"].ToString() == "9")
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
    }
}