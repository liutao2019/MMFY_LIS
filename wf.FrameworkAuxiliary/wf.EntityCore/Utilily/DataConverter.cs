using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.EntityCore
{
    class DataConverter
    {
        public static object ConvertData(object objVal, Type sourceType, Type destType)
        {
            object retVal = objVal;

            if (destType == typeof(System.Int16))
            {
                retVal = ObjToInt16(objVal);
            }
            else if (destType == typeof(System.Int32))
            {
                retVal = ObjToInt32(objVal);
            }
            else if (destType == typeof(System.Int64))
            {
                retVal = ObjToInt64(objVal);
            }
            else if (destType == typeof(System.Decimal))
            {
                retVal = ObjToDecimal(objVal);
            }
            else if (destType == typeof(System.String))
            {
                retVal = ObjToString(objVal);
            }

            return retVal;
        }



        private static Int16 ObjToInt16(object objVal)
        {
            return Convert.ToInt16(objVal);
        }

        private static Int32 ObjToInt32(object objVal)
        {
            return Convert.ToInt32(objVal);
        }

        private static Int64 ObjToInt64(object objVal)
        {
            return Convert.ToInt64(objVal);
        }

        private static Decimal ObjToDecimal(object objVal)
        {
            return Convert.ToDecimal(objVal);
        }

        private static String ObjToString(object objVal)
        {
            if (objVal != null)
            {
                return objVal.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
