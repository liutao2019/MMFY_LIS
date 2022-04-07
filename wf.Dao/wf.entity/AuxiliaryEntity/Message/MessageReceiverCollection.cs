using System;
using System.Collections.Generic;
using System.Text;

namespace dcl.entity
{
    public class MessageReceiverCollection : List<EntityMessageReceiver>
    {
        public MessageReceiverCollection()
            : base()
        {

        }

        public MessageReceiverCollection(IEnumerable<EntityMessageReceiver> collection)
            : base(collection)
        {

        }

        public string GetHashString()
        {
            string ret = string.Empty;

            foreach (EntityMessageReceiver item in this)
            {
                ret += item.MessageID;
            }

            return ret;
        }
    }
}
