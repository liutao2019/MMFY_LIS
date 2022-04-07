using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace Lib.DataInterface
{
    abstract class AbstractDataInterfaceParameterCollection : IDataInterfaceParameterCollection
    {
        public override int GetHashCode()
        {
            StringBuilder sb = new StringBuilder();
            foreach (AbstractDataInterfaceParameter item in _innerList)
            {
                sb.Append(item.GetHashCode().ToString() + "_");
            }
            return sb.ToString().GetHashCode();
        }

        List<AbstractDataInterfaceParameter> _innerList = new List<AbstractDataInterfaceParameter>();

        public AbstractDataInterfaceParameter GetReturnValueParameter()
        {
            foreach (AbstractDataInterfaceParameter par in this._innerList)
            {
                if (par.Direction == EnumDataInterfaceParameterDirection.ReturnValue)
                {
                    return par;
                }
            }

            return null;
        }

        #region IDataParameterCollection 成员

        public bool Contains(string parameterName)
        {
            foreach (AbstractDataInterfaceParameter item in _innerList)
            {
                if (item.Name == parameterName)
                {
                    return true;
                }
            }
            return false;
        }

        public int IndexOf(string parameterName)
        {
            for (int i = 0; i < _innerList.Count; i++)
            {
                if (_innerList[i].Name == parameterName)
                {
                    return i;
                }
            }
            return -1;
        }

        public void RemoveAt(string parameterName)
        {
            for (int i = this._innerList.Count - 1; i >= 0; i++)
            {
                string pName = this._innerList[i].Name;
                if (parameterName == pName)
                {
                    this._innerList.Remove(this._innerList[i]);
                    break;
                }
            }
        }

        public object this[string parameterName]
        {
            get
            {
                foreach (AbstractDataInterfaceParameter item in _innerList)
                {
                    if (item.Name == parameterName)
                    {
                        return item;
                    }
                }
                return null;
            }
            set
            {
                for (int i = 0; i < _innerList.Count; i++)
                {
                    if (_innerList[i].Name == parameterName)
                    {
                        _innerList[i] = value as AbstractDataInterfaceParameter;
                        break;
                    }
                }
                throw new ArgumentNullException();
            }
        }

        #endregion

        #region IList 成员

        public int Add(object value)
        {
            this._innerList.Add(value as AbstractDataInterfaceParameter);
            return -1;
        }

        public void Clear()
        {
            this._innerList.Clear();
        }

        public bool Contains(object value)
        {
            return this._innerList.Contains(value as AbstractDataInterfaceParameter);
        }

        public int IndexOf(object value)
        {
            return this._innerList.IndexOf(value as AbstractDataInterfaceParameter);
        }

        public void Insert(int index, object value)
        {
            this._innerList.Insert(index, value as AbstractDataInterfaceParameter);
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Remove(object value)
        {
            this._innerList.Remove(value as AbstractDataInterfaceParameter);
        }

        public void RemoveAt(int index)
        {
            this._innerList.RemoveAt(index);
        }

        public object this[int index]
        {
            get
            {
                return this._innerList[index] as AbstractDataInterfaceParameter;
            }
            set
            {
                this._innerList[index] = value as AbstractDataInterfaceParameter;
            }
        }

        #endregion

        #region ICollection 成员

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
            //this._innerList.CopyTo(
        }

        public int Count
        {
            get { return this._innerList.Count; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return null; }
        }

        #endregion

        #region IEnumerable 成员

        public System.Collections.IEnumerator GetEnumerator()
        {
            return this._innerList.GetEnumerator();
        }

        #endregion
    }
}
