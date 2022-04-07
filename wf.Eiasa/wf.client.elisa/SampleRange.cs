using System;
using System.Collections.Generic;

using System.Text;

namespace dcl.client.elisa
{
    public class SampleRange
    {
        public SampleRange(int start, int end)
        {
            this.End = end;
            this.Start = start;
        }

        public int Start { get; set; }
        public int End { get; set; }

        internal  bool Contains(int i)
        {
            return i >= Start && i <= End;
        }
    }
}
