using System;
using System.Data;

namespace dcl.svr.ca
{
    public interface ICASignature
    {
        bool Sign(ref DataTable p_dtbCASignContent, out DateTime p_dateTime);

        bool Sign(ref DataTable p_dtbCASignContent);

        bool Sign(string p_strSourceContent, out string p_strSignContent);
    }
}
