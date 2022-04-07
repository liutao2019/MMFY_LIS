using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// tat图形实体
    /// </summary>
    [Serializable]
    public class EntityTimeLineParameters : EntityBase
    {
        public EntityTimeLineParameters()
        {
            OverTime = false;
        }
        /// <summary>
        /// 时间名称
        /// </summary>
        public string DateName;

        /// <summary>
        /// 是否超时
        /// </summary>
        public bool OverTime; 

        /// <summary>
        /// 时间
        /// </summary>
        public string DateTime;

        /// <summary>
        /// 状态
        /// </summary>
        public string Status;
    }
}
