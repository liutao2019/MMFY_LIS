using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.servececontract;
using dcl.root.dac;
using System.Data;

namespace dcl.svr.result
{
    class PatientBacteriBIZ : IPatientsBacteri
    {
        #region IPatientsBacteri 成员

        public DataTable GetBacteriTypeData()
        {
            throw new NotImplementedException();
        }

        public DataTable GetBacteriData()
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable GetAntiData()
        {
            string sql = "select top 0 * from cs_rlts";
            DBHelper helper = new DBHelper();
            DataTable dtCS = helper.GetTable(sql);
            dtCS.TableName = "cs_rlts";
            return dtCS;
        }

        #endregion
    }
}
