using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.EntityCore.DataTypeConverter
{
    public interface IDataTypeConverter
    {
        object ConvertTo(object source);
        object ConvertFrom(object source);

        bool CanConvertTo(Type destinationType);
        bool CanConvertFrom(Type sourceType);
    }
}
