using System;
using System.Collections.Generic;

using System.Text;

namespace dcl.client.common
{
    /// <summary>
    /// 操作条的异常
    /// </summary>
    public class BarActionException : Exception
    {
        public override string Message
        {
            get
            {
                return "操作条的错误操作: "+ base.Message;
            }
        }
    }
}
