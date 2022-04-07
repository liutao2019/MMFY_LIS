using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;


namespace Lib.EntityCore
{
    [Serializable]
    public class PropertiesValueLogs : Dictionary<string, PropertyValueLogList>
    {
        public PropertiesValueLogs()
        {
        }

        public bool ExistProperty(string propName)
        {
            foreach (string key in this.Keys)
            {
                if (propName == key)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
