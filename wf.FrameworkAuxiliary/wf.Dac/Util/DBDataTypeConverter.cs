using System;
using System.Data;
using System.Data.SqlClient;

namespace Lib.DAC
{
    public class DBDataTypeConverter
    {

        public static Type DbDataTypeToSystemType(string dbDataType)
        {
            throw new NotImplementedException();
            if (string.IsNullOrEmpty(dbDataType))
            {
                throw new ArgumentException("参数不允许为空", "dbDataType");
            }

            Type t;



            return t;
        }

        public static string DbDataTypeNameToSystemTypeName(string dbDataType, bool nullable)
        {
            if (string.IsNullOrEmpty(dbDataType))
            {
                throw new ArgumentException("参数不允许为空", "dbDataType");
            }

            string sysTypeName = string.Empty;
            int keyCount = DBTypeConversionKey.Length;
            for (int i = 0; i < keyCount; i++)
            {
                if (DBTypeConversionKey[i, 0].ToLower() == dbDataType.ToLower())
                {
                    sysTypeName = DBTypeConversionKey[i, 1];

                    if (Type.GetType(sysTypeName).IsValueType && nullable)
                    {
                        return sysTypeName + "?";
                    }
                    else
                    {
                        return sysTypeName;
                    }

                    //break;
                }
            }
            return sysTypeName;
        }

        public static string DbDataTypeNameToDbTypeName(string dbDataType)
        {
            if (string.IsNullOrEmpty(dbDataType))
            {
                throw new ArgumentException("参数不允许为空", "dbDataType");
            }

            string dbTypeName = string.Empty;
            int keyCount = DBTypeConversionKey.Length;
            for (int i = 0; i < keyCount; i++)
            {
                if (DBTypeConversionKey[i, 0].ToLower() == dbDataType.ToLower())
                {
                    dbTypeName = DBTypeConversionKey[i, 2];
                    break;
                }
            }
            return dbTypeName;
        }

        public static Type DbTypeToSystemType(DbType sourceType)
        {
            string dbTypeName = sourceType.ToString();
            throw new NotImplementedException();
        }

        /// <summary>
        /// 实现 System.Type 转换成 DbType
        /// 因为只找到 SqlDbType 与 System.Type的对照表，而没有 DbType 与 System.Type的对照表，
        /// 所以还要通过SqlParameter把 SqlDbType 转成 DbType.
        /// </summary>
        /// <param name="SourceType">传入要转换的System.Type</param>
        /// <returns>返回对应的 DbType </returns>
        public static DbType SystemTypeToDbType(System.Type SourceType)
        {
            String dbTypeName = String.Empty;
            int keyCount = DBTypeConversionKey.GetLength(0);
            for (int i = 0; i < keyCount; i++)
            {
                if (DBTypeConversionKey[i, 1].ToLower() == SourceType.FullName.ToLower())
                {
                    dbTypeName = DBTypeConversionKey[i, 0];
                    break;
                }
            }
            if (dbTypeName == String.Empty) dbTypeName = "Variant";
            SqlDbType sqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), dbTypeName);

            System.Data.SqlClient.SqlParameter paraConver = new System.Data.SqlClient.SqlParameter();  //通过SqlParameter把 SqlDbType --> DbType
            paraConver.SqlDbType = sqlDbType;
            return paraConver.DbType;
        }

        #region
        /*
        * 数组参照：
        * Mapping .NET Framework Data Provider Data Types to .NET Framework Data Types
        *http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpguide/html/cpconmappingnetdataproviderdatatypestonetframeworkdatatypes.asp 
        * 
        */
        private static String[,] DBTypeConversionKey = new String[,] 
        {
            {"UniqueIdentifier","System.Guid","Guid"},
            {"NVarChar",  "System.String","String"},
            {"Int",    "System.Int32","Int32"},
            {"Decimal",   "System.Decimal","Decimal"},
            {"DateTime",  "System.DateTime","DateTime"},
            {"Date",  "System.DateTime","DateTime"},
            {"Bit",    "System.Boolean","Boolean"},
            {"SmallDateTime", "System.DateTime","DateTime"}, 
            {"Money",   "System.Decimal","Decimal"},
            {"SmallMoney",  "System.Decimal","Decimal"},
            {"TinyInt",   "System.Byte","Byte"},
            {"SmallInt",  "System.Int16","Int16"},
            {"BigInt",   "System.Int64","Int64"},
            {"VarChar",   "System.String","AnsiString"},
            {"VarChar2",   "System.String","AnsiString"},
            {"Text",   "System.String","AnsiString"},
            {"Char",   "System.String","AnsiString"},
            {"Float",   "System.Double","Double"},
            {"Real",   "System.Single",""},
            {"NChar",   "System.String","String"},
            {"NText",   "System.String","String"},
            {"Image",   "System.Byte[]","Binary"},
            {"Binary",   "System.Byte[]","Binary"},
            {"Timestamp",  "System.Byte[]","Binary"},
            {"VarBinary",  "System.Byte[]","Binary"},
            {"Variant",   "System.Object","Object"},
            {"Numeric","System.Decimal","Decimal"},
            {"Number","System.Decimal","Decimal"},
            {"LONG RAW","System.Byte[]","Binary"}
            
        };
        #endregion
    }
}
