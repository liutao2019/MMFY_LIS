using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    public class EntityInterfaceResponse
    {
        /// <summary>
        /// 返回内容
        /// </summary>
        public string ResponseInfo { get; set; }

        /// <summary>
        /// 发送内容
        /// </summary>
        public string RequestInfo { get; set; }
    }
}
