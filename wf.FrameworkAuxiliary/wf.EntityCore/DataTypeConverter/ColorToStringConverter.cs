using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Lib.EntityCore.DataTypeConverter
{
    public class ColorToStringConverter : DataTypeConverter<Color, String>
    {
        protected override string _ConvertTo(Color source)
        {
            string output = System.Drawing.ColorTranslator.ToHtml(source);
            return output;
        }

        protected override Color _ConvertFrom(string source)
        {
            if (source == null)
            {
                return Color.Empty;
            }
            Color output = System.Drawing.ColorTranslator.FromHtml(source);
            return output;
        }

        public override bool CanConvertTo(Type destinationType)
        {
            if (destinationType == typeof(Color))
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
