using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Lib.DataInterface
{
    class NetDllDataInterfaceParameterCollection : AbstractDataInterfaceParameterCollection
    {
        public new NetDllDataInterfaceParameter this[string parameterName]
        {
            get
            {
                for (int i = 0; i < base.Count; i++)
                {
                    NetDllDataInterfaceParameter p = (base[i] as NetDllDataInterfaceParameter);

                    if (p != null && p.Name == parameterName)
                    {
                        return p;
                    }
                }
                return null;
            }
            set
            {
                for (int i = 0; i < base.Count; i++)
                {
                    NetDllDataInterfaceParameter p = (base[i] as NetDllDataInterfaceParameter);

                    if (p != null && p.Name == parameterName)
                    {
                        base[i] = value;
                    }
                }
                throw new ArgumentException();
            }
        }

        public new NetDllDataInterfaceParameter this[int index]
        {
            get
            {
                return base[index] as NetDllDataInterfaceParameter;
            }
            set
            {
                base[index] = value;
            }
        }

        public void Add(NetDllDataInterfaceParameter par)
        {
            base.Add(par);
        }

        public void Remove(NetDllDataInterfaceParameter value)
        {
            base.Remove(value);
        }

        public bool Contains(NetDllDataInterfaceParameter value)
        {
            return base.Contains(value as NetDllDataInterfaceParameter);
        }

        public int IndexOf(NetDllDataInterfaceParameter value)
        {
            return base.IndexOf(value);
        }

        public void Insert(int index, NetDllDataInterfaceParameter value)
        {
            base.Insert(index, value);
        }
    }
}
