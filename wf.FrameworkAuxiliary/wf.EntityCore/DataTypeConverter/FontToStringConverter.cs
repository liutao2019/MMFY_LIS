using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace Lib.EntityCore.DataTypeConverter
{
    public class FontToStringConverter : DataTypeConverter<Font, String>
    {
        protected override string _ConvertTo(Font source)
        {
            TypeConverter tc = TypeDescriptor.GetConverter(typeof(Font));
            string fontString = tc.ConvertToString(source);
            return fontString;
        }

        protected override Font _ConvertFrom(string source)
        {
            if (source == null)
            {
                return new Font("宋体", 9f);
            }
            TypeConverter tc = TypeDescriptor.GetConverter(typeof(Font));
            Font font = tc.ConvertFromString(source) as Font;
            return font;
        }

        public override bool CanConvertTo(Type destinationType)
        {
            if (destinationType == typeof(Font))
            {
                return true;
            }
            return false;
        }

        public override bool CanConvertFrom(Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return false;
        }
    }
}
