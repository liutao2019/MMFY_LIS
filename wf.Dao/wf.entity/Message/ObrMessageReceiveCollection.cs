using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    public class ObrMessageReceiveCollection:List<EntityDicObrMessageReceive>
    {

        public ObrMessageReceiveCollection()
            : base()
        {

        }

        public ObrMessageReceiveCollection(IEnumerable<EntityDicObrMessageReceive> collection)
            : base(collection)
        {

        }

        public string GetHashString()
        {
            string ret = string.Empty;

            foreach (EntityDicObrMessageReceive item in this)
            {
                ret += item.ObrId;
            }

            return ret;
        }
    }
}
