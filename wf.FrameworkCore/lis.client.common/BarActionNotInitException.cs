using System;
using System.Collections.Generic;

using System.Text;

namespace dcl.client.common
{
    public class BarActionNotInitException : BarActionException
    {
        public override string Message
        {
            get
            {
                return "操作条的控制者不存在!";
            }
        }
    }
}
