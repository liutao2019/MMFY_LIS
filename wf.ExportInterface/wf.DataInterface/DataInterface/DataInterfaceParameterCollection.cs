using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Lib.DataInterface.Implement;

namespace Lib.DataInterface
{
    [Serializable]
    public class DataInterfaceParameterCollection : List<DataInterfaceParameter>// : IDataParameterCollection
    {
        //List<DataInterfaceParameter> _innerList = new List<DataInterfaceParameter>();

        internal static DataInterfaceParameterCollection FromDTO(List<EntityDictDataInterfaceCommandParameter> dtoParams, DACManager dac)
        {
            DataInterfaceParameterCollection listPar = new DataInterfaceParameterCollection();

            foreach (EntityDictDataInterfaceCommandParameter item in dtoParams)
            {
                if (item.param_enabled == 0)
                    continue;

                DataInterfaceParameter p = DataInterfaceParameter.FromDTO(item, dac);
                listPar.Add(p);
            }

            return listPar;
        }


        public DataInterfaceParameter GetReturnValueParameter()
        {
            foreach (DataInterfaceParameter par in this)
            {
                if (par.Direction == EnumDataInterfaceParameterDirection.ReturnValue)
                {
                    return par;
                }
            }

            return null;
        }

        //#region IDataParameterCollection 成员

        //public bool Contains(string parameterName)
        //{
        //    foreach (DataInterfaceParameter item in _innerList)
        //    {
        //        if (item.Name == parameterName)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //public int IndexOf(string parameterName)
        //{
        //    for (int i = 0; i < _innerList.Count; i++)
        //    {
        //        if (_innerList[i].Name == parameterName)
        //        {
        //            return i;
        //        }
        //    }
        //    return -1;
        //}

        //public void RemoveAt(string parameterName)
        //{
        //    for (int i = this._innerList.Count - 1; i >= 0; i++)
        //    {
        //        string pName = this._innerList[i].Name;
        //        if (parameterName == pName)
        //        {
        //            this._innerList.Remove(this._innerList[i]);
        //            break;
        //        }
        //    }
        //}

        public DataInterfaceParameter this[string parameterName]
        {
            get
            {
                foreach (DataInterfaceParameter item in this)
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
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].Name == parameterName)
                    {
                        this[i] = value;
                        break;
                    }
                }
                throw new ArgumentNullException();
            }
        }

        //#endregion

        //#region IList 成员

        //public int Add(object value)
        //{
        //    this._innerList.Add(value as DataInterfaceParameter);
        //    return -1;
        //}

        //public void Clear()
        //{
        //    this._innerList.Clear();
        //}

        //public bool Contains(object value)
        //{
        //    return this._innerList.Contains(value as DataInterfaceParameter);
        //}

        //public int IndexOf(object value)
        //{
        //    return this._innerList.IndexOf(value as DataInterfaceParameter);
        //}

        //public void Insert(int index, object value)
        //{
        //    this._innerList.Insert(index, value as DataInterfaceParameter);
        //}

        //public bool IsFixedSize
        //{
        //    get { return false; }
        //}

        //public bool IsReadOnly
        //{
        //    get { return false; }
        //}

        //public void Remove(object value)
        //{
        //    this._innerList.Remove(value as DataInterfaceParameter);
        //}

        //public void RemoveAt(int index)
        //{
        //    this._innerList.RemoveAt(index);
        //}

        //public object this[int index]
        //{
        //    get
        //    {
        //        return this._innerList[index] as DataInterfaceParameter;
        //    }
        //    set
        //    {
        //        this._innerList[index] = value as DataInterfaceParameter;
        //    }
        //}

        //#endregion

        ////public DataInterfaceParameter this[int index]
        ////{
        ////    get
        ////    {
        ////        return this._innerList[index] as DataInterfaceParameter;
        ////    }
        ////    set
        ////    {
        ////        this._innerList[index] = value as DataInterfaceParameter;
        ////    }
        ////}

        //#region ICollection 成员

        //public void CopyTo(Array array, int index)
        //{
        //    throw new NotImplementedException();
        //    //this._innerList.CopyTo(
        //}

        //public int Count
        //{
        //    get { return this._innerList.Count; }
        //}

        //public bool IsSynchronized
        //{
        //    get { return false; }
        //}

        //public object SyncRoot
        //{
        //    get { return null; }
        //}

        //#endregion

        //#region IEnumerable 成员

        //public System.Collections.IEnumerator GetEnumerator()
        //{
        //    return this._innerList.GetEnumerator();
        //}

        //#endregion

        public override string ToString()
        {
            return string.Format("Count = {0}", this.Count);
        }
    }
}
