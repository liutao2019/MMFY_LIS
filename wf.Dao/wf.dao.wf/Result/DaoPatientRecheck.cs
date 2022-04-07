using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.ComponentModel.Composition;
using dcl.dao.core;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoPatientRecheck))]
    class DaoPatientRecheck : IDaoPatientRecheck
    {
        public bool DeletePatientRecheck(EntityPatientRecheck recheck)
        {
            bool result = false;
            if (recheck != null)
            {
                try
                {
                    string sql = string.Format(@"delete from  [Pat_recheck] where Pr_pat_id='{0}' and Pr_Ditm_id='{1}'", recheck.ChkPatId, recheck.ChkItmId);
                    DBManager helper = new DBManager();
                    helper.ExecCommand(sql);
                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return result;
        }

        public bool InsertPatientRecheck(EntityPatientRecheck recheck)
        {
            bool result = false;
            if (recheck != null)
            {
                try
                {
                    DBManager helper = new DBManager();
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values = helper.ConverToDBSaveParameter(recheck);
                    if (helper.InsertOperation("Pat_recheck", values) > 0)
                    {
                        result = true;
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return result;
        }
    }
}
