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
    [Export("wf.plugin.wf", typeof(IDaoTypeBarcode))]
    public class DaoTypeBarcode : IDaoTypeBarcode
    {
        public List<EntityTypeBarcode> SearchTypeBarcode(string hosID)
        {
            List<EntityTypeBarcode> listBarcode = new List<EntityTypeBarcode>();

            string firstStr = string.Empty;
            string secondStr = string.Empty;

            if (!string.IsNullOrEmpty(hosID))
            {
                firstStr = string.Format(@" and (Dict_profession.Dpro_Dorg_id='{0}' or Dict_profession.Dpro_Dorg_id='' or Dict_profession.Dpro_Dorg_id is null)  ", hosID);
                secondStr = string.Format(" and (Dict_organize.Dorg_id='{0}' or Dict_organize.Dorg_id='' or Dict_organize.Dorg_id is null) ", hosID);
            }
            try
            {
                string sqlStr = string.Format(@"select Dorg_id type_nodeId,Dorg_name pro_name,0 pro_id,Dorg_id type_node 
from Dict_organize where del_flag=0 {1}
union
select Dpro_id+'_'+isnull(Dpro_Dorg_id,'') type_nodeId,Dpro_name,
Dpro_id,isnull(Dpro_Dorg_id,0) type_node 
from Dict_profession where del_flag=0 and Dpro_type=1 {0} ", firstStr, secondStr);
                 
                DBManager helper = new DBManager();

                DataTable dtTypeBarcode = helper.ExecuteDtSql(sqlStr);

                listBarcode = EntityManager<EntityTypeBarcode>.ConvertToList(dtTypeBarcode).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("SearchRuleByHisCode", ex);
            }
            return listBarcode;
        }

    }
}
