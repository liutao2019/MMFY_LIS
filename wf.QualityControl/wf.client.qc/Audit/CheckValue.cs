using System;
using System.Collections.Generic;

using System.Text;

namespace dcl.client.qc
{
    class CheckValue
    {
        private string displayMember;

        public string DisplayMember
        {
            get { return displayMember; }
            set { displayMember = value; }
        }

        private string valueMember;

        public string ValueMember
        {
            get { return valueMember; }
            set { valueMember = value; }
        }

        public override string ToString()
        {
            return DisplayMember;
        }
    }
}
