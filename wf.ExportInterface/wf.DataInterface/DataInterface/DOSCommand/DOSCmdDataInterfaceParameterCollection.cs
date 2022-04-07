using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Lib.DataInterface
{
    class DOSCmdDataInterfaceParameterCollection : AbstractDataInterfaceParameterCollection
    {
        public new DOSCmdDataInterfaceParameter this[string parameterName]
        {
            get
            {
                for (int i = 0; i < base.Count; i++)
                {
                    DOSCmdDataInterfaceParameter p = (base[i] as DOSCmdDataInterfaceParameter);

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
                    DOSCmdDataInterfaceParameter p = (base[i] as DOSCmdDataInterfaceParameter);

                    if (p != null && p.Name == parameterName)
                    {
                        base[i] = value;
                    }
                }
                throw new ArgumentException();
            }
        }

        public new DOSCmdDataInterfaceParameter this[int index]
        {
            get
            {
                return base[index] as DOSCmdDataInterfaceParameter;
            }
            set
            {
                base[index] = value;
            }
        }

        public void Add(DOSCmdDataInterfaceParameter par)
        {
            base.Add(par);
        }

        public void Remove(DOSCmdDataInterfaceParameter value)
        {
            base.Remove(value);
        }

        public bool Contains(DOSCmdDataInterfaceParameter value)
        {
            return base.Contains(value as DOSCmdDataInterfaceParameter);
        }

        public int IndexOf(DOSCmdDataInterfaceParameter value)
        {
            return base.IndexOf(value);
        }

        public void Insert(int index, DOSCmdDataInterfaceParameter value)
        {
            base.Insert(index, value);
        }
    }
}
