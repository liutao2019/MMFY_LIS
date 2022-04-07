using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.EntityCore.DataTypeConverter
{
    public abstract class DataTypeConverter<TSource, TDest> : IDataTypeConverter
    {
        protected abstract TDest _ConvertTo(TSource source);
        protected abstract TSource _ConvertFrom(TDest source);

        public abstract bool CanConvertTo(Type destinationType);
        public abstract bool CanConvertFrom(Type sourceType);

        #region IDataTypeConverter 成员

        public object ConvertTo(object source)
        {
            return _ConvertTo((TSource)source);
        }

        public object ConvertFrom(object source)
        {
            return _ConvertFrom((TDest)source);
        }

        //public bool CanConvertTo(Type destinationType)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool CanConvertFrom(Type sourceType)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion
    }
}
