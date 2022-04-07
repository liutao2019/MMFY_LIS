using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicTestTube>))]
    public class DaoDicTestTube : IDaoDic<EntityDicTestTube>
    {
        public bool Delete(EntityDicTestTube tube)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Dtub_code", tube.TubCode);

                helper.UpdateOperation("Dict_test_tube", values, key);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDicTestTube tube)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_test_tube");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dtub_code", id);
                values.Add("Dtub_name", tube.TubName);
                values.Add("Dtub_flag", tube.TubFlag);
                values.Add("Dtub_barcode_min", tube.TubBarcodeMin);
                values.Add("Dtub_barcode_max", tube.TubBarcodeMax);
                values.Add("sort_no", tube.TubSortNo);
                values.Add("py_code", tube.TubPyCode);
                values.Add("wb_code", tube.TubWbCode);
                values.Add("del_flag", tube.TubDelFlag);
                values.Add("Dtub_max_capcity", tube.TubMaxCapcity);
                values.Add("Dtub_unit", tube.TubUnit);
                values.Add("Dtub_max_com", tube.TubMaxCom);
                values.Add("Dtub_charge_code", tube.TubChargeCode);
                values.Add("Dtub_price", tube.TubPrice);
                values.Add("Dtub_color", tube.TubColor);

                helper.InsertOperation("Dict_test_tube", values);

                tube.TubCode = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicTestTube> Search(object obj)
        {
            try
            {
                String sql = @"select * from Dict_test_tube where del_flag=0 or del_flag is null";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntityDicTestTube>.ConvertToList(dt).OrderBy(i => i.TubSortNo).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicTestTube>();
            }
        }

        public bool Update(EntityDicTestTube tube)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dtub_name", tube.TubName);
                values.Add("Dtub_flag", tube.TubFlag);
                values.Add("Dtub_barcode_min", tube.TubBarcodeMin);
                values.Add("Dtub_barcode_max", tube.TubBarcodeMax);
                values.Add("sort_no", tube.TubSortNo);
                values.Add("py_code", tube.TubPyCode);
                values.Add("wb_code", tube.TubWbCode);
                values.Add("del_flag", tube.TubDelFlag);
                values.Add("Dtub_max_capcity", tube.TubMaxCapcity);
                values.Add("Dtub_unit", tube.TubUnit);
                values.Add("Dtub_max_com", tube.TubMaxCom);
                values.Add("Dtub_charge_code", tube.TubChargeCode);
                values.Add("Dtub_price", tube.TubPrice);
                values.Add("Dtub_color", tube.TubColor);

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Dtub_code", tube.TubCode);

                helper.UpdateOperation("Dict_test_tube", values, key);
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
