using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib.DAC;
using System.Data;
using dcl.entity;

namespace dcl.svr.resultcheck.Updater
{
    class UpdatePatientsMessage
    {
        public void Update(EntityPidReportMain patinfo, int pat_type)
        {
            string sqlDelete = string.Format("delete from patients_message where pat_id='{0}'", patinfo.RepId);
            string sqlInsert = string.Format("insert into patients_message(pat_id,pat_bar_code,pat_type,pat_msg_date,pat_remark) values ('{0}','{1}',{2},getdate(),null)", patinfo.RepId, patinfo.RepBarCode, pat_type);
            SqlHelper helper = new SqlHelper();
            DbCommandEx comDelete = helper.CreateCommandEx(sqlDelete);
            comDelete.AddParameterValue(patinfo.RepId);
            DbCommandEx comInsert = helper.CreateCommandEx(sqlInsert);
            comInsert.AddParameterValue(patinfo.RepId);
            comInsert.AddParameterValue(patinfo.RepBarCode);
            comInsert.AddParameterValue(pat_type);

            helper.ExecuteNonQuery(comDelete);
            helper.ExecuteNonQuery(comInsert);
        }

        public void updateBac(DataTable patinfo, int pat_type)
        {
            foreach (DataRow rows in patinfo.Rows)
            {
                string sqlDelete = string.Format("delete from patients_message where pat_id='{0}'", rows["pat_id"].ToString());
                string sqlInsert = string.Format("insert into patients_message(pat_id,pat_bar_code,pat_type,pat_msg_date,pat_remark) values ('{0}','{1}',{2},getdate(),null)", rows["pat_id"].ToString(), rows["pat_bar_code"].ToString(), pat_type);
                SqlHelper helper = new SqlHelper();
                DbCommandEx comDelete = helper.CreateCommandEx(sqlDelete);
                comDelete.AddParameterValue(rows["pat_id"].ToString());
                DbCommandEx comInsert = helper.CreateCommandEx(sqlInsert);
                comInsert.AddParameterValue(rows["pat_id"].ToString());
                comInsert.AddParameterValue(rows["pat_bar_code"].ToString());
                comInsert.AddParameterValue(pat_type);

                helper.ExecuteNonQuery(comDelete);
                helper.ExecuteNonQuery(comInsert);
            }
        }
    }
}
