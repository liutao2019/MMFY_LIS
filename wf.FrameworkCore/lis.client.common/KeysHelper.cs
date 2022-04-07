using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace dcl.client.common
{
    /// <summary>
    /// 按键助手
    /// </summary>
    public class KeysHelper
    {
        public static void Jump(Keys key)
        {
            if (key == Keys.Up)
            {
                SendKeys.Send("+{TAB}");
            }
            if (key == Keys.Down)
            {
                SendKeys.Send("{TAB}");
            }
        }
    }
}
