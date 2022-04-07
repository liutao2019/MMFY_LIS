using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.EntityCore
{
    /// <summary>
    /// 实体基类
    /// </summary>
    [Serializable]
    public abstract class BaseEntity
    {
        private bool _logStackFrame = false;


        private PropertiesValueLogs _propertiesValueChanedLog = null;

        public bool EnablePropertiesValueChangedLog = true;

        public PropertiesValueLogs PropertiesValueChanedLog
        {
            get
            {
                if (_propertiesValueChanedLog == null)
                {
                    _propertiesValueChanedLog = new PropertiesValueLogs();
                }
                return _propertiesValueChanedLog;
            }
        }


    }
}
