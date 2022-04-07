using System;
using System.Collections.Generic;
using System.Text;

namespace dcl.client.sample
{
    public class BarcodePrinterNotFoundException :Exception
    {
        public override string Message
        {
            get
            {
                return "没有选择条码打印机!";
            }
        }
    }
}
