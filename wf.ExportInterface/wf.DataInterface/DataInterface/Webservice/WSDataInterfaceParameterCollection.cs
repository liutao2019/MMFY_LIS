using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Lib.DataInterface
{
    class WSDataInterfaceParameterCollection : AbstractDataInterfaceParameterCollection
    {
        public new WSDataInterfaceParameter this[string parameterName]
        {
            get
            {
                for (int i = 0; i < base.Count; i++)
                {
                    WSDataInterfaceParameter p = (base[i] as WSDataInterfaceParameter);

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
                    WSDataInterfaceParameter p = (base[i] as WSDataInterfaceParameter);

                    if (p != null && p.Name == parameterName)
                    {
                        base[i] = value;
                    }
                }
                throw new ArgumentException();
            }
        }

        public new WSDataInterfaceParameter this[int index]
        {
            get
            {
                return base[index] as WSDataInterfaceParameter;
            }
            set
            {
                base[index] = value;
            }
        }

        public void Add(WSDataInterfaceParameter par)
        {
            base.Add(par);
        }

        public void Remove(WSDataInterfaceParameter value)
        {
            base.Remove(value);
        }

        public bool Contains(WSDataInterfaceParameter value)
        {
            return base.Contains(value as WSDataInterfaceParameter);
        }

        public int IndexOf(WSDataInterfaceParameter value)
        {
            return base.IndexOf(value);
        }

        public void Insert(int index, WSDataInterfaceParameter value)
        {
            base.Insert(index, value);
        }
    }
}
