using System;
using System.Collections.Generic;
using dcl.entity;
using System.Text;
using System.Data;

namespace dcl.client.result
{
    public class ClickEventArgs : EventArgs
    {
        public DataTable Antibio
        {
            get;
            set;
        }

        public List<EntityDicMicAntidetail> Antibio2
        {
            get;
            set;
        }
    }
}
