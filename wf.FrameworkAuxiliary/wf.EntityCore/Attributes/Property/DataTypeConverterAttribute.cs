using System;
using System.Collections.Generic;
using System.Text;
using Lib.EntityCore.DataTypeConverter;

namespace Lib.EntityCore
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class DataTypeConverterAttribute : IEntityPropertyAttribute
    {
        internal IDataTypeConverter converter;
        public DataTypeConverterAttribute(Type typeConverter)
        {
            if (typeConverter.GetInterface(typeof(IDataTypeConverter).Name) == null)
            {
                throw new ArgumentException("参数typeConverter必须实现接口" + typeof(IDataTypeConverter).Name);
            }

            converter = Activator.CreateInstance(typeConverter) as IDataTypeConverter;
        }
    }
}
