using System;
using System.Collections.Generic;
using System.Text;

namespace wf.client.reagent
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
