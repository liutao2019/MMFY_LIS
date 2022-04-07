using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Lib.DataInterface
{
    class WCFDataInterfaceParameterCollection : AbstractDataInterfaceParameterCollection
    {
        public new WCFDataInterfaceParameter this[string parameterName]
        {
            get
            {
                for (int i = 0; i < base.Count; i++)
                {
                    WCFDataInterfaceParameter p = (base[i] as WCFDataInterfaceParameter);

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
                    WCFDataInterfaceParameter p = (base[i] as WCFDataInterfaceParameter);

                    if (p != null && p.Name == parameterName)
                    {
                        base[i] = value;
                    }
                }
                throw new ArgumentException();
            }
        }

        public new WCFDataInterfaceParameter this[int index]
        {
            get
            {
                return base[index] as WCFDataInterfaceParameter;
            }
            set
            {
                base[index] = value;
            }
        }

        public void Add(WCFDataInterfaceParameter par)
        {
            base.Add(par);
        }

        public void Remove(WCFDataInterfaceParameter value)
        {
            base.Remove(value);
        }

        public bool Contains(WCFDataInterfaceParameter value)
        {
            return base.Contains(value as WCFDataInterfaceParameter);
        }

        public int IndexOf(WCFDataInterfaceParameter value)
        {
            return base.IndexOf(value);
        }

        public void Insert(int index, WCFDataInterfaceParameter value)
        {
            base.Insert(index, value);
        }
    }
}
