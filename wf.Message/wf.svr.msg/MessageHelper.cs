using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.msg
{
    internal class MessageHelper
    {
        public static string GetNewMessageID()
        {
            return Guid.NewGuid().ToString().ToLower().Replace("-", "");
        }
    }
}
