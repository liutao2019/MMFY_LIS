using System;
using System.Collections.Generic;

using System.Text;

namespace dcl.client.report
{
    public class ReportFactory
    {
        public static IReport GetReport(string repCode)
        {
            switch (repCode)
            {
                case "bacilli":
                    return new BacReport();                  
                default:
                    break;
            }

            return null;
        }
    }
}
