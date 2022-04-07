using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using dcl.root.dac;
using System.Data.SqlClient;

namespace dcl.svr.result
{
    public class PatientCombineBLL
    {
        #region 更新医嘱id：如果有使用条码则更新patients_mi表的pat_yz_id为bc_cname表的bc_yz_id

        private void UpdatePatientCombineYZID(DataTable dtPatientsMi)
        {
            List<SqlCommand> cmdList = new List<SqlCommand>();
            if (dtPatientsMi.Rows.Count > 0)
            {
                //foreach (DataColumn col in dtPatientsMi.Columns)
                //{
                //    if (col.ColumnName == "pat_id"
                //        || col.ColumnName == "pat_com_id"
                //        || col.ColumnName == "pat_his_code"
                //        || col.ColumnName == "pat_com_price"
                //        || col.ColumnName == "pat_yz_id"
                //        || col.ColumnName == "pat_seq"
                //        )
                //    {

                //    }
                //}

                string sqlTemplate = "update patients_mi set pat_yz_id = @pat_yz_id where pat_id = @pat_id and pat_com_id = @pat_com_id";
                foreach (DataRow row in dtPatientsMi.Rows)
                {
                    SqlCommand cmd = new SqlCommand(sqlTemplate);
                    SqlParameter pPatID = cmd.Parameters.AddWithValue("pat_id", row["pat_id"]);
                    pPatID.DbType = DbType.AnsiString;

                    SqlParameter pComID = cmd.Parameters.AddWithValue("pat_com_id", row["pat_com_id"]);
                    pComID.DbType = DbType.AnsiString;


                    if (row["pat_yz_id"] == null)
                    {
                        SqlParameter pYZID = cmd.Parameters.AddWithValue("pat_yz_id", DBNull.Value);
                        pYZID.DbType = DbType.AnsiString;
                    }
                    else
                    {
                        SqlParameter pYZID = cmd.Parameters.AddWithValue("pat_yz_id", row["pat_yz_id"]);
                        pYZID.DbType = DbType.AnsiString;
                    }

                    cmdList.Add(cmd);
                }

                DBHelper helper = new DBHelper();
                foreach (SqlCommand cmd in cmdList)
                {
                    helper.ExecuteNonQuery(cmd);
                }
            }
        }



        /// <summary>
        /// 如果有使用条码则更新patients_mi表的pat_yz_id为bc_cname表的bc_yz_id
        /// </summary>
        /// <param name="pat_id"></param>
        public void UpdatePatientCombineYZID(string pat_id)
        {
            DBHelper helper = new DBHelper();
            object objPatBarCode = helper.ExecuteScalar(string.Format("select top 1 pat_bar_code from patients with(nolock) where pat_id = '{0}'", pat_id));

            if (objPatBarCode != null && objPatBarCode != DBNull.Value && objPatBarCode.ToString().Trim() != string.Empty)
            {
                UpdatePatientCombineYZID(pat_id, objPatBarCode.ToString());
            }
        }

        public void UpdatePatientCombineYZID(string pat_id, string pat_bar_code)
        {
            if (!string.IsNullOrEmpty(pat_bar_code) && pat_bar_code.Trim() != string.Empty)
            {
                DBHelper helper = new DBHelper();

                DataTable dtPatientsMi = helper.GetTable(string.Format("select pat_id,pat_com_id,pat_yz_id from patients_mi with(nolock) where pat_id = '{0}'", pat_id));
                DataTable dtBcCname = helper.GetTable(string.Format("select bc_lis_code,bc_yz_id from bc_cname with(nolock) where bc_bar_code = '{0}'", pat_bar_code));

                FillPatientCombineYZID(ref dtPatientsMi, dtBcCname);

                UpdatePatientCombineYZID(dtPatientsMi);
            }
        }

        /// <summary>
        /// 把bc_cname的bc_yz_id填充到patients_mi的pat_yz_id中
        /// </summary>
        /// <param name="dtPatientsMi"></param>
        /// <param name="dtBcCname"></param>
        public void FillPatientCombineYZID(ref DataTable dtPatientsMi, DataTable dtBcCname)
        {
            if (dtPatientsMi != null && dtPatientsMi.Rows.Count > 0)
            {
                foreach (DataRow drPatientsMi in dtPatientsMi.Rows)
                {
                    //医嘱id为空才更新
                    if (drPatientsMi["pat_yz_id"] == null
                        || drPatientsMi["pat_yz_id"] == DBNull.Value
                        || drPatientsMi["pat_yz_id"].ToString().Trim() == string.Empty)
                    {
                        //组合id
                        string pat_com_id = drPatientsMi["pat_com_id"].ToString();

                        if (!string.IsNullOrEmpty(pat_com_id))
                        {
                            //在bc_cname中查找组合对应的医嘱id并填充到病人组合表中
                            DataRow[] drsBcCname = dtBcCname.Select(string.Format("bc_lis_code = '{0}'", pat_com_id));
                            if (drsBcCname.Length > 0)
                            {
                                drPatientsMi["pat_yz_id"] = drsBcCname[0]["bc_yz_id"];
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
