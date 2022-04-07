using System;
using System.Collections.Generic;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 打印参数
    /// </summary>
    [Serializable]
    public class EntityPrintParameter
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public EntityPrintParameter()
        {
            this.Name = string.Empty;
            this.Value = string.Empty;
        }
    }
}
