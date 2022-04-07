using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Lib.DataInterface
{
    class BinDllDataInterfaceParameterCollection : AbstractDataInterfaceParameterCollection
    {
        public new BinDllDataInterfaceParameter this[string parameterName]
        {
            get
            {
                for (int i = 0; i < base.Count; i++)
                {
                    BinDllDataInterfaceParameter p = (base[i] as BinDllDataInterfaceParameter);

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
                    BinDllDataInterfaceParameter p = (base[i] as BinDllDataInterfaceParameter);

                    if (p != null && p.Name == parameterName)
                    {
                        base[i] = value;
                    }
                }
                throw new ArgumentException();
            }
        }

        public new BinDllDataInterfaceParameter this[int index]
        {
            get
            {
                return base[index] as BinDllDataInterfaceParameter;
            }
            set
            {
                base[index] = value;
            }
        }

        public void Add(BinDllDataInterfaceParameter par)
        {
            base.Add(par);
        }

        public void Remove(BinDllDataInterfaceParameter value)
        {
            base.Remove(value);
        }

        public bool Contains(BinDllDataInterfaceParameter value)
        {
            return base.Contains(value as BinDllDataInterfaceParameter);
        }

        public int IndexOf(BinDllDataInterfaceParameter value)
        {
            return base.IndexOf(value);
        }

        public void Insert(int index, BinDllDataInterfaceParameter value)
        {
            base.Insert(index, value);
        }
    }
}
