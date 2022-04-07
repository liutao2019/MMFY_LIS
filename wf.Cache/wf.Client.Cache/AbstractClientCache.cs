using System;
using System.Collections.Generic;
using System.Text;

namespace dcl.client.cache
{
    public abstract class AbstractClientCache<T> where T : class
    {
        public abstract void Refresh();

        protected List<T> _cache = null;

        public AbstractClientCache()
        {
            this.Refresh();
        }

        public abstract List<T> SelectAll();
    }
}
