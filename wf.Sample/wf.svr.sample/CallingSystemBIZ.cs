using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib.DAC;
using System.Data;
using dcl.servececontract;

namespace dcl.svr.sample
{
    public class CallingSystemBIZ : ICallingSystem
    {
        #region ICallingSystem 成员

        public DataSet GetCallInfo(string strWinNum)
        {
            DataSet dsResult = new DataSet();
            string sqlAppend;

            if (strWinNum == "5号窗口")
            {
                sqlAppend = " AND bc_type='B' ";
            }
            else
            {
                sqlAppend = " AND bc_type='A' ";
            }

            string sql = string.Format(@"select bc_no,bc_name,bc_type,bc_flag 
                           from bc_blood 
                           where bc_flag in (0,1,2){0}
                           group by bc_no,bc_name,bc_type,bc_flag
                           order by bc_flag,case when bc_no like 'T%' then 0 else 1 end,len(bc_no) asc,bc_no asc", sqlAppend);

            SqlHelper helper = new SqlHelper();

            DataTable dtCallingInfo = helper.GetTable(sql);

            dtCallingInfo.TableName = "bcCallingInfo";

            dsResult.Tables.Add(dtCallingInfo);

            string sqlCalled = string.Format(@"select bc_no,bc_name,bc_type 
                                 from bc_blood 
                                 where bc_flag=3{0}
                                 group by bc_no,bc_name,bc_type order by len(bc_no),bc_no", sqlAppend);

            DataTable dtCalledInfo = helper.GetTable(sqlCalled);

            dtCalledInfo.TableName = "bcCalledInfo";

            dsResult.Tables.Add(dtCalledInfo);

            return dsResult;
        }

        public DataSet GetCallPatInfo(string strWinNum, string strBcNo)
        {
            DataSet dtResult = new DataSet();

            string sql = string.Empty;
            string sqlAppend;
            if (strWinNum == "5号窗口")
            {
                sqlAppend = " AND bc_type='B' ";
            }
            else
            {
                sqlAppend = " AND bc_type='A' ";
            }


            if (strBcNo == string.Empty)
            {
                sql = string.Format(@"select bc_bar_code,bc_no from bc_blood
                        where bc_no=
                        (
                        SELECT TOP 1  bc_no FROM 
                         (SELECT  bc_no  FROM bc_blood bb WHERE bc_flag=0 AND ISNUMERIC(bc_no)=0 {0} AND SUBSTRING(bc_no,2,LEN(bc_no))=
                         (SELECT MIN(CAST(SUBSTRING(bc_no,2,LEN(bc_no)) AS INT)) FROM bc_blood bb WHERE bc_flag=0 AND ISNUMERIC(bc_no)=0 {0} ) 
                            UNION ALL
                            select CAST(min(CAST(bc_no AS INT)) AS VARCHAR) bc_no from bc_blood where bc_flag=0 AND ISNUMERIC(bc_no)=1 {0})P ORDER BY bc_no DESC
                        )", sqlAppend);
            }
            else
            {
                sql = string.Format(@"select 
                                    bc_bar_code,bc_no 
                                    from bc_blood 
                                    where bc_no='{0}'", strBcNo);
            }

            SqlHelper helper = new SqlHelper();


            string strUpdateFlag = string.Format("update bc_blood set bc_flag=2 where bc_flag=1 and  bc_add='{0}'", strWinNum);

            helper.ExecuteNonQuery(strUpdateFlag);

            DataTable dtBarCode = helper.GetTable(sql);

            if (dtBarCode.Rows.Count > 0)
            {
                string barCode = string.Empty;

                string bc_no = dtBarCode.Rows[0]["bc_no"].ToString();

                foreach (DataRow drBarCode in dtBarCode.Rows)
                {
                    barCode += string.Format(",'{0}'", drBarCode["bc_bar_code"].ToString());
                }

                barCode = barCode.Remove(0, 1);

                string strUpdate = string.Format("update bc_blood set bc_flag=1,bc_v=0,bc_add='{1}' where bc_bar_code in ({0})", barCode, strWinNum);

                helper.ExecuteNonQuery(strUpdate);


                string strInfo = string.Format(@"select
                                                bc_patients.bc_id,
                                                bc_patients.bc_notice,
	                                            bc_patients.bc_bar_no,
	                                            bc_patients.bc_bar_code,
	                                            bc_patients.bc_no_id,
	                                            bc_patients.bc_in_no,
	                                            bc_patients.bc_bed_no,
	                                            bc_patients.bc_name,
	                                            bc_patients.bc_sex,
                                                bc_patients.bc_status,
                                                bc_patients.bc_ctype,
	                                            case bc_patients.bc_sex when '1' then '男'
	                                                                    when '2' then '女'
	                                                                    else '' end as bc_sex_name,
	                                            bc_patients.bc_age,
                                                dbo.getAge(bc_patients.bc_age) barcode_age,
                                                bc_patients.bc_diag,
                                                bc_patients.bc_doct_code,
                                                bc_patients.bc_doct_name,
                                                bc_patients.bc_his_name,
                                                bc_patients.bc_sam_name,bc_print_time,
                                                isnull(dict_type.[type_name],'') barcode_type_name,
                                                dict_origin.ori_name barcode_ori_name,
                                                dict_sample.sam_name barcode_sam_name,
                                                (case isnull(bc_patients.bc_exp,'') when '' then isnull(dict_sample_remarks.rem_cont,'') else bc_patients.bc_exp end ) barcode_sample_remarks,
                                                isnull(dict_cuvette.cuv_name,'') barcode_cuv_name,
                                                bc_patients.bc_exp,
	                                            bc_patients.bc_d_name,
                                                bc_patients.bc_occ_date,
                                                '{1}' bc_no
                                            from bc_patients  with(nolock)
                                                LEFT OUTER JOIN dict_type ON bc_patients.bc_ctype = dict_type.type_id LEFT OUTER JOIN 
                                                dict_origin on bc_patients.bc_ori_id = dict_origin.ori_id LEFT OUTER JOIN  
                                                dict_cuvette ON bc_patients.bc_cuv_code = dict_cuvette.cuv_code  LEFT OUTER JOIN 
                                                dict_sample ON bc_patients.bc_sam_id = dict_sample.sam_id LEFT OUTER JOIN
                                                dict_sample_remarks ON bc_patients.bc_sam_rem_id = dict_sample_remarks.rem_id 
                                                where bc_patients.bc_bar_code in
                                                ({0})
                                                ", barCode, bc_no);

                DataTable dtCallPatInfo = helper.GetTable(strInfo);

                if (dtCallPatInfo.Rows.Count <= 0)
                {
                    DataRow drCallPatInfo = dtCallPatInfo.NewRow();
                    drCallPatInfo["bc_no"] = bc_no;
                    drCallPatInfo["bc_name"] = bc_no;
                    drCallPatInfo["bc_bar_code"] = bc_no;
                    dtCallPatInfo.Rows.Add(drCallPatInfo);
                }

                dtCallPatInfo.TableName = "bc_patients";

                dtResult.Tables.Add(dtCallPatInfo);
            }


            return dtResult;
        }

        public DataTable GetBcCname(string strBcBarCode)
        {
            SqlHelper helper = new SqlHelper();

            string strBcCname = string.Format(@"select 
                                                bc_id,
                                                bc_name,
                                                bc_lis_code,
                                                bc_his_code,
                                                bc_blood_notice,
                                                bc_save_notice 
                                                from bc_cname
                                                where bc_bar_code ='{0}'", strBcBarCode);

            DataTable dtBcCname = helper.GetTable(strBcCname);

            dtBcCname.TableName = "bc_cname";

            return dtBcCname;
        }

        public DataTable GetBcPatInfo(string strBcNo)
        {
            DataTable dtResult = new DataTable();
            SqlHelper helper = new SqlHelper();

            string strBcBarcode = string.Format(@"select 
                                                bc_bar_code,
                                                bc_no
                                                from bc_blood
                                                where bc_no ='{0}'", strBcNo);

            DataTable dtBarCode = helper.GetTable(strBcBarcode);

            if (dtBarCode.Rows.Count > 0)
            {
                string barCode = string.Empty;

                foreach (DataRow drBarCode in dtBarCode.Rows)
                {
                    barCode += string.Format(",'{0}'", drBarCode["bc_bar_code"].ToString());
                }

                barCode = barCode.Remove(0, 1);

                string strInfo = string.Format(@"select
                                                bc_patients.bc_id,
                                                bc_patients.bc_notice,
	                                            bc_patients.bc_bar_no,
	                                            bc_patients.bc_bar_code,
	                                            bc_patients.bc_no_id,
	                                            bc_patients.bc_in_no,
	                                            bc_patients.bc_bed_no,
	                                            bc_patients.bc_name,
	                                            bc_patients.bc_sex,
                                                bc_patients.bc_status,
                                                bc_patients.bc_ctype,
	                                            case bc_patients.bc_sex when '1' then '男'
	                                                                    when '2' then '女'
	                                                                    else '' end as bc_sex_name,
	                                            bc_patients.bc_age,
                                                dbo.getAge(bc_patients.bc_age) barcode_age,
                                                bc_patients.bc_diag,
                                                bc_patients.bc_doct_code,
                                                bc_patients.bc_doct_name,
                                                bc_patients.bc_his_name,
                                                bc_patients.bc_sam_name,
                                                isnull(dict_type.[type_name],'') barcode_type_name,
                                                dict_origin.ori_name barcode_ori_name,
                                                dict_sample.sam_name barcode_sam_name,
                                                (case isnull(bc_patients.bc_exp,'') when '' then isnull(dict_sample_remarks.rem_cont,'') else bc_patients.bc_exp end ) barcode_sample_remarks,
                                                isnull(dict_cuvette.cuv_name,'') barcode_cuv_name,
                                                bc_patients.bc_exp,
	                                            bc_patients.bc_d_name,
                                                bc_patients.bc_occ_date,
                                                '{1}' bc_no
                                            from bc_patients  with(nolock)
                                                LEFT OUTER JOIN dict_type ON bc_patients.bc_ctype = dict_type.type_id LEFT OUTER JOIN 
                                                dict_origin on bc_patients.bc_ori_id = dict_origin.ori_id LEFT OUTER JOIN  
                                                dict_cuvette ON bc_patients.bc_cuv_code = dict_cuvette.cuv_code  LEFT OUTER JOIN 
                                                dict_sample ON bc_patients.bc_sam_id = dict_sample.sam_id LEFT OUTER JOIN
                                                dict_sample_remarks ON bc_patients.bc_sam_rem_id = dict_sample_remarks.rem_id 
                                                where bc_patients.bc_bar_code in
                                                ({0})
                                                ", barCode, strBcNo);

                DataTable dtBarCodeInfo = helper.GetTable(strInfo);
                dtBarCodeInfo.TableName = "bc_patients";
                dtResult = dtBarCodeInfo;
            }

            return dtResult;
        }

        public bool CancelBcNo(string strBcNo)
        {
            string strUpdate = string.Format(@"update bc_blood set bc_flag=2 where bc_no='{0}'", strBcNo);

            SqlHelper helper = new SqlHelper();

            return helper.ExecuteNonQuery(strUpdate) > 0;
        }

        public bool ConfirmBcBarCode(string strBcNo, string strWinNum, string userId, string userName, byte[] pic)
        {
            string sql = string.Format(@"select 
                                    bc_patients.bc_bar_code,bc_no,bc_status 
                                    from bc_blood
                                    left join bc_patients on bc_patients.bc_bar_code=bc_blood.bc_bar_code 
                                    where bc_no='{0}' and bc_patients.bc_bar_code is not null", strBcNo);

            SqlHelper helper = new SqlHelper();

            DataTable dtBarCode = helper.GetTable(sql);

            if (dtBarCode.Rows.Count > 0)
            {
                foreach (DataRow drBarCode in dtBarCode.Rows)
                {
                    string strUpdateBloodDate = string.Empty;
                    if (drBarCode["bc_status"].ToString() == "0" || drBarCode["bc_status"].ToString() == "1")
                    {
                        strUpdateBloodDate = string.Format(@"update bc_patients set bc_blood_date='{1}',bc_status='2',bc_lastaction_time=getdate() 
                                                            where bc_bar_code='{0}'", drBarCode["bc_bar_code"].ToString(), DateTime.Now);

                        helper.ExecuteNonQuery(strUpdateBloodDate);

                        string strInsertBcSign = string.Format(@"insert into bc_sign (bc_date,bc_login_id,bc_name,bc_status,bc_bar_no,bc_bar_code,bc_remark) 
                                                values (getdate(),'{3}','{0}',2,'{1}','{1}','{2}')", userName, drBarCode["bc_bar_code"].ToString(), strWinNum, userId, DateTime.Now);

                        helper.ExecuteNonQuery(strInsertBcSign);

                        
                    }
                    UpdatePatPic(drBarCode["bc_bar_code"].ToString(), pic);
                }
            }

            string strUpdate = string.Format(@"update bc_blood set bc_flag=3 where bc_no='{0}'", strBcNo);

            return helper.ExecuteNonQuery(strUpdate) > 0;
        }
        public byte[] GetPatPicByBarcode(string barcode)
        {
            string sql = string.Format(@"select top 1
                                    bc_blood_pat_img.bc_pic
                                    from bc_blood_pat_img  with(nolock)
                                    where bc_bar_code='{0}' order by bc_blood_pat_img.bc_id desc", barcode);

            SqlHelper helper = new SqlHelper();

            object pic = helper.ExecuteScalar(sql);
            if (pic == null)
            {
                return null;
            }
            else
            {
                return pic as byte[];
            }
        }
        public byte[] GetPatPicByCallNo(string strBcNo, string strWinNum)
        {
            string sql = string.Format(@"select top 1
                                    bc_blood_pat_img.bc_pic
                                    from bc_blood_pat_img  with(nolock)
                                    left join bc_blood  with(nolock) on bc_blood.bc_bar_code=bc_blood_pat_img.bc_bar_code 
                                    where bc_no='{0}' order by bc_blood_pat_img.bc_id desc", strBcNo);

            SqlHelper helper = new SqlHelper();

            object pic = helper.ExecuteScalar(sql);
            if (pic == null)
            {
                return null;
            }
            else
            {
                return pic as byte[];
            }

        }
        public void UpdatePatPic(string strBcNo, string strWinNum, byte[] pic)
        {
            if (pic != null)
            {
                string sql = string.Format(@"select 
                                    bc_patients.bc_bar_code,bc_no,bc_status 
                                    from bc_blood with(nolock)
                                    left join bc_patients with(nolock) on bc_patients.bc_bar_code=bc_blood.bc_bar_code 
                                    where bc_no='{0}' and bc_patients.bc_bar_code is not null", strBcNo);

                SqlHelper helper = new SqlHelper();

                DataTable dtBarCode = helper.GetTable(sql);

                if (dtBarCode.Rows.Count > 0)
                {
                    foreach (DataRow drBarCode in dtBarCode.Rows)
                    {
                        UpdatePatPic(drBarCode["bc_bar_code"].ToString(), pic);
                    }

                }
            }
        }

        void UpdatePatPic(string bar_code, byte[] pic)
        {
            if (pic != null)
            {
                SqlHelper helper = new SqlHelper();
                helper.ExecuteNonQuery(string.Format("delete bc_blood_pat_img where bc_bar_code='{0}'", bar_code));
                string sql = "INSERT bc_blood_pat_img VALUES (? ,?) ";

                DbCommandEx cmd = helper.CreateCommandEx(sql);
                cmd.AddParameterValue(bar_code);
                cmd.AddParameterValue(pic);
                helper.ExecuteNonQuery(cmd);
            }
        }

        public bool ResetCall(string strBcNo)
        {
            string strUpdate = string.Format(@"update bc_blood set bc_flag=1,bc_v=0 where bc_no='{0}'", strBcNo);

            SqlHelper helper = new SqlHelper();

            return helper.ExecuteNonQuery(strUpdate) > 0;
        }

        public string ZLYYCall()
        {
            string sql = "SELECT isnull(max(cast(isnull(CASE WHEN ISNUMERIC(bc_no)=1 THEN bc_no ELSE SUBSTRING(bc_no,2,LEN(bc_no))END,'1') AS INT))+1,1) bc_no  FROM bc_blood bb";

            SqlHelper helper = new SqlHelper();
            string bc_no = helper.GetTable(sql).Rows[0]["bc_no"].ToString();

            string strInster = string.Format("insert into bc_blood(bc_bar_code,bc_name,bc_no,bc_type,bc_flag,bc_date,bc_v) values ({0},{0},'{0}','A',0,getdate(),0)", bc_no);

            helper.ExecuteNonQuery(strInster);

            return bc_no;
        }

        #endregion
    }
}
