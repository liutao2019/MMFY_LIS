using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC
{
    internal class StringTool
    {
        public static bool IsEmpty(string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNotEmpty(string str)
        {
            return !IsEmpty(str);
        }
    }
}
