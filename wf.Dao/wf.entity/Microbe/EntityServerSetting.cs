using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{

    /// <summary>
    /// 服务器相关配置
    /// </summary>
    [Serializable]
    public class EntityServerSetting : EntityBase
    {
        public string SystemType { get; set; }
        public string ExtDataInterface { get; set; }
    }
}
