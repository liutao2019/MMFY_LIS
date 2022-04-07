using System;
using System.Collections.Generic;

using System.Text;

namespace lis.client.control.LISControl
{
    public interface ILISControl
    {
        string Text { get; set; }
        object Value { get; set; }
    }
}
