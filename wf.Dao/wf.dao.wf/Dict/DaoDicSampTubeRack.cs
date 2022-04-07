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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicSampTubeRack>))]
    class DaoDicSampTubeRack : IDaoDic<EntityDicSampTubeRack>
    {
        public bool Delete(EntityDicSampTubeRack rack)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", 1);

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Drack_id", rack.RackId);

                helper.UpdateOperation("Dict_sample_tube_rack", values, key);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDicSampTubeRack rack)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_sample_tube_rack");

                DBManager helper = new DBManager();
                string barCode =new DaoSysBarcodeGenerator().GetNextMaxBarCode();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Drack_id", id);
                values.Add("Drack_name", rack.RackName);
                values.Add("Drack_Dtrack_code", rack.RackSpec);
                values.Add("Drack_code", rack.RackCode);
                values.Add("Drack_Dpro_id", rack.RackType);
                values.Add("Drack_status", rack.RackStatus);
                values.Add("Drack_barcode", rack.RackBarcode==null?barCode: rack.RackBarcode);
                values.Add("Drack_createtime", rack.RackCreatetime);
                values.Add("del_flag", rack.RackDelFlag);
                values.Add("Drack_print_flag", rack.RackPrintFlag);
                values.Add("Drack_colour", rack.RackColour);
                values.Add("Drack_week", rack.RackWeek);

                helper.InsertOperation("Dict_sample_tube_rack", values);

                rack.RackId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicSampTubeRack> Search(object obj)
        {
            string where = string.Empty;
            DateTime date1, date2;
            if (obj.GetType() == typeof(String))
            {
                where = string.Format("WHERE Dict_sample_tube_rack.Drack_id={0}", obj.ToString());
            }
            else if(obj.GetType() == typeof(Dictionary<string, object>))
            {
                Dictionary<string, object> dict = obj as Dictionary<string, object>;
                where = string.Format("WHERE Dict_sample_tube_rack.Drack_code={0}", dict["rackCode"].ToString());
            }
            else
            {
                DateTime[] dates = obj as DateTime[];
                date1 = dates[0];
                date2 = dates[1];
                where = string.Format(@"where ISNULL(Dict_sample_tube_rack.del_flag,0)<>1 
                                       and Dict_sample_tube_rack.Drack_createtime  between '{0}' and '{1}'", date1, date2);
            }
            
            try
            {
                String sql = string.Format(@"select Dict_sample_tube_rack.*,                                     
Dict_tube_rack.Dtrack_name as cus_name,
Dict_tube_rack.Dtrack_x_amount,
Dict_tube_rack.Dtrack_y_amount,
Dict_profession.Dpro_name,
0 as isselected
from Dict_sample_tube_rack
inner join Dict_tube_rack on  Dict_tube_rack.Dtrack_code = Dict_sample_tube_rack.Drack_Dtrack_code
left join Dict_profession on  Dict_profession.Dpro_id = Dict_sample_tube_rack.Drack_Dpro_id  {0}", where);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntityDicSampTubeRack>.ConvertToList(dt).OrderBy(i => i.RackCode).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicSampTubeRack>();
            }
        }

        public bool Update(EntityDicSampTubeRack rack)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Drack_name", rack.RackName);
                values.Add("Drack_Dtrack_code", rack.RackSpec);
                values.Add("Drack_code", rack.RackCode);
                values.Add("Drack_Dpro_id", rack.RackType);
                values.Add("Drack_status", rack.RackStatus);
                values.Add("Drack_barcode", rack.RackBarcode);
                values.Add("Drack_createtime", rack.RackCreatetime);
                values.Add("del_flag", rack.RackDelFlag);
                values.Add("Drack_print_flag", rack.RackPrintFlag);
                values.Add("Drack_colour", rack.RackColour);
                values.Add("Drack_week", rack.RackWeek);

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Drack_id", rack.RackId);

                helper.UpdateOperation("Dict_sample_tube_rack", values, key);
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
