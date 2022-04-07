using System;
using System.Collections.Generic;
using System.Text;
using Lib.EntityCore.DataTypeConverter;

namespace Lib.EntityCore
{
    class ValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="destType">目标类型</param>
        /// <param name="sourceValue">原始值</param>
        /// <returns></returns>
        public static object ConvertValue(Type destType, object sourceValue)
        {
            //如果目标类型为泛型
            if (destType.IsGenericType && destType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (sourceValue == null || sourceValue == DBNull.Value)
                {
                    return null;
                }

                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(destType);
                destType = nullableConverter.UnderlyingType;
            }

            if ((sourceValue == null || sourceValue == DBNull.Value)
                && destType.IsValueType)
            {
                if (destType == typeof(int)
                    || destType == typeof(long)
                    || destType == typeof(sbyte)
                    || destType == typeof(short)
                    || destType == typeof(uint)
                    || destType == typeof(ulong)
                    || destType == typeof(ushort)
                   )
                {
                    return 0;
                }
                else if (destType == typeof(float))
                {
                    return default(float);
                }
                else if (destType == typeof(double))
                {
                    return default(double);
                }
                else if (destType == typeof(decimal))
                {
                    return default(decimal);
                }
                else if (destType == typeof(bool))
                {
                    return default(bool);
                }
                else if (destType == typeof(byte))
                {
                    return default(byte);
                }
                else if (destType == typeof(char))
                {
                    return default(char);
                }
                else if (destType == typeof(DateTime))
                {
                    return default(DateTime);
                }

                throw new NotSupportedException();
            }

            if (sourceValue == DBNull.Value)
            {
                if (destType == typeof(byte[]))
                {
                    return null;
                }
            }

            if (destType == typeof(bool))
            {
                if (sourceValue == null || sourceValue == DBNull.Value)
                {
                    return false;
                }
                else
                {
                    sourceValue = sourceValue.ToString().Trim();
                    if (sourceValue.ToString() == "0")
                    {
                        return false;
                    }
                    else if (sourceValue.ToString() == "1")
                    {
                        return true;
                    }
                    else if (sourceValue.ToString().ToLower() == "true")
                    {
                        return true;
                    }
                    else if (sourceValue.ToString().ToLower() == "false")
                    {
                        return false;
                    }
                }
                return Convert.ChangeType(sourceValue, destType);
            }
            else
            {
                return Convert.ChangeType(sourceValue, destType);
            }
        }

        private static object ConvertValueWithCustomConverter(object sourceValue, IDataTypeConverter converter)
        {
            if (converter == null)
            {
                return sourceValue;
            }

            object objValueConverted = converter.ConvertFrom(sourceValue);
            return objValueConverted;
        }

        public static object ConvertValue(EntityPropertyInfo epi, object sourceValue)
        {
            object objValueConverted = ConvertValue(epi.Property.PropertyType, sourceValue);
            object objValueConvertedWithCustomConverter = ConvertValueWithCustomConverter(objValueConverted, epi.DataTypeConverter == null ? null : epi.DataTypeConverter.converter);
            return objValueConvertedWithCustomConverter;
        }
    }
}
