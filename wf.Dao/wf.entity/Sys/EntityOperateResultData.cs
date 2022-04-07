using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 操作结果数据
    /// </summary>
    [Serializable]
    public class EntityOperateResultData : EntityBase
    {
        public EntityPidReportMain Patient { get; set; }

        public EntityOperateResultData()
        {
            this.Patient = new EntityPidReportMain();
        }
    }
}
