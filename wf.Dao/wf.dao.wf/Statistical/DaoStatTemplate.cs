using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoSaveStatTemp))]
    public class DaoStatTemplate : IDaoSaveStatTemp
    {
        public bool DeleteStatTemp(string StaName, string StaType)
        {
            try
            {
                String sql = string.Format("DELETE tp_template WHERE tp_template.st_name='{0}' AND tp_template.st_type='{1}'", StaName, StaType);

                DBManager helper = new DBManager();

                helper.ExecCommand(sql); ;

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool InsertStatTemp(EntityTpTemplate TpTemplate)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("st_type", TpTemplate.StType);
                values.Add("st_name", TpTemplate.StName);
                values.Add("st_tableId", TpTemplate.StTableId);
                values.Add("st_tableName", TpTemplate.StTableName);
                values.Add("res_or", TpTemplate.ResOr);
                values.Add("res_itm_ecd", TpTemplate.ResItmEcd);
                values.Add("res_chr", TpTemplate.ResChr);
                values.Add("res_od_chr", TpTemplate.ResOdChr);
                values.Add("res_unit", TpTemplate.ResUnit);

                helper.InsertOperation("tp_template", values);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }
    }
}
