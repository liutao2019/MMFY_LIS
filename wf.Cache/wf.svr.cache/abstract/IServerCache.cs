using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.cache
{
    public interface IServerCache<T> where T : class
    {
        List<T> GetAll();
        void Refresh();
        //void Clear();
    }
}
