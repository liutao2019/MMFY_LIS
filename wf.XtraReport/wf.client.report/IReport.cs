using System;
using System.Collections.Generic;

using System.Text;

namespace dcl.client.report
{
    public interface IReport
    {
        void Print(string where);
        void PrintPreview(string where);
    }
}
