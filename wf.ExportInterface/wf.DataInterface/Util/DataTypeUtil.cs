using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Lib.DataInterface
{
    class DataTypeUtil
    {
        public static string GetNetTypeFullName(string datatype)
        {
            if (string.IsNullOrEmpty(datatype))
            {
                return null;
            }

            string ret = datatype;

            switch (datatype.ToLower())
            {
                case "system.data.datatable":
                case "datatable":
                    ret = typeof(DataTable).AssemblyQualifiedName;
                    break;

                case "system.data.dataset":
                case "dataset":
                    ret = typeof(DataSet).AssemblyQualifiedName;
                    break;

                case "int32":
                    ret = "System.Int32";
                    break;

                case "int64":
                    ret = "System.Int64";
                    break;

                case "string":
                    ret = "System.String";
                    break;

                case "bool":
                case "boolean":
                    ret = "System.Boolean";
                    break;

                case "char":
                    ret = "System.Char";
                    break;

                case "byte":
                    ret = "System.Byte";
                    break;

                case "int16":
                    ret = "System.Int16";
                    break;

                case "uint16":
                    ret = "System.UInt16";
                    break;

                case "uint32":
                    ret = "System.UInt32";
                    break;

                case "uint64":
                    ret = "System.UInt64";
                    break;

                case "double":
                    ret = "System.Double";
                    break;

                case "decimal":
                    ret = "System.Decimal";
                    break;

                case "datetime":
                    ret = "System.DateTime";
                    break;
            }
            
            return ret;
        }


        public static object ChangeType(object srcValue, string destDataType)
        {
            string tName = DataTypeUtil.GetNetTypeFullName(destDataType);
            Type dType = Type.GetType(tName);
            object objP = null;
            if (srcValue == null
                || srcValue == DBNull.Value
                )
            {
                objP = srcValue;
            }
            else
            {
                objP = Convert.ChangeType(srcValue, dType);
            }

            return objP;
        }
    }
}
