using System;

namespace dcl.svr.ca
{
    public class CASignatureFactory
    {
        public static ICASignature CreateCASignature(string p_strCAMode)
        {
            if (p_strCAMode != null && p_strCAMode == "BJCATimestamp")
            {
                return null;
                //return new BJCATimestamp();
            }
            return null;
        }
    }
}
