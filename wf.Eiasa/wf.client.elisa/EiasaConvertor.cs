using System;
using System.Collections.Generic;

using System.Text;

namespace dcl.client.elisa
{
    public class EiasaConvertor
    {
        public static List<string> FormatValueToList(string formatValue)
        {
            List<string> result = new List<string>();

            if (String.IsNullOrEmpty(formatValue) || formatValue[0] != ',')
                return result;

            formatValue = formatValue.Substring(1);
            result = new List<string>(formatValue.Split(','));
            return result;
        }
    }
}
