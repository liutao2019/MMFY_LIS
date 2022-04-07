using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Lib.DataInterface
{
    class SqlDataInterfaceParameterCollection : AbstractDataInterfaceParameterCollection
    {
        public new SqlDataInterfaceParameter this[string parameterName]
        {
            get
            {
                for (int i = 0; i < base.Count; i++)
                {
                    SqlDataInterfaceParameter p = (base[i] as SqlDataInterfaceParameter);

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
                    SqlDataInterfaceParameter p = (base[i] as SqlDataInterfaceParameter);

                    if (p != null && p.Name == parameterName)
                    {
                        base[i] = value;
                    }
                }
                throw new ArgumentException();
            }
        }

        public new SqlDataInterfaceParameter this[int index]
        {
            get
            {
                return base[index] as SqlDataInterfaceParameter;
            }
            set
            {
                base[index] = value;
            }
        }

        public void Add(SqlDataInterfaceParameter par)
        {
            base.Add(par);
        }

        public void Remove(SqlDataInterfaceParameter value)
        {
            base.Remove(value);
        }

        public bool Contains(SqlDataInterfaceParameter value)
        {
            return base.Contains(value as SqlDataInterfaceParameter);
        }

        public int IndexOf(SqlDataInterfaceParameter value)
        {
            return base.IndexOf(value);
        }

        public void Insert(int index, SqlDataInterfaceParameter value)
        {
            base.Insert(index, value);
        }
    }
}
