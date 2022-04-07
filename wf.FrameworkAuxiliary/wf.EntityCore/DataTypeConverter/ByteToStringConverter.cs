using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace Lib.EntityCore.DataTypeConverter
{
    class ByteToStringConverter : DataTypeConverter<byte[], String>
    {
        public override bool CanConvertTo(Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }
            return false;
        }

        public override bool CanConvertFrom(Type sourceType)
        {
            if (sourceType == typeof(byte[]))
            {
                return true;
            }
            return false;
        }

        protected override string _ConvertTo(byte[] source)
        {
            if (source == null)
            {
                return null;
            }
            string ret = Encoding.UTF8.GetString(source);
            return ret;
        }

        protected override byte[] _ConvertFrom(string source)
        {
            if (source == null)
            {
                return null;
            }
            byte[] buff = Encoding.UTF8.GetBytes(source);
            return buff;
        }
    }
}
