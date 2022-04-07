using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace Lib.EntityCore.DataTypeConverter
{
    public class ByteToBase64StringConverter : DataTypeConverter<byte[], String>
    {
        protected override string _ConvertTo(byte[] source)
        {
            if (source == null)
            {
                return null;
            }

            return Convert.ToBase64String(source);
        }

        protected override byte[] _ConvertFrom(string source)
        {
            if (source == null)
            {
                return null;
            }

            byte[] buff = Convert.FromBase64String(source);
            return buff;
        }

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
    }
}
