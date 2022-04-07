using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.core;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoSampOrder))]
    public class DaoSampOrder : IDaoSampOrder
    {
        public bool SaveSampOrder(List<EntitySampOrderHL7> listSampOrder)
        {
            try
            {
                DBManager helper = new DBManager();

                foreach (EntitySampOrderHL7 item in listSampOrder)
                {
                    Dictionary<string, object> value = helper.ConverToDBSaveParameter<EntitySampOrderHL7>(item);
                    helper.InsertOperation("samp_order", value);
                }

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool DeleteSampOrder(string strOrderSn)
        {
            throw new NotImplementedException();
        }


    }
}
