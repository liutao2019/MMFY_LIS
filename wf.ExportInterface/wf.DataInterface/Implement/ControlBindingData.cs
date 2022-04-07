using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Lib.DataInterface.Implement
{
    class ControlBindingData
    {
        public const string COMMAND_GROUP_CONVERTER = "@数据转换";



        #region datatable方式
        public static DataTable GetCommandExecuteTypeSelectItems()
        {
            DataTable table = GetTamplateDataTable();
            table.Rows.Add(new object[] { "", "" });
            table.Rows.Add(new object[] { EnumCommandExecuteType.ExecuteNonQuery.ToString(), "执行不返回值" });
            table.Rows.Add(new object[] { EnumCommandExecuteType.ExecuteScalar.ToString(), "返回单值" });
            table.Rows.Add(new object[] { EnumCommandExecuteType.ExecuteGetDataTable.ToString(), "获取DataTable" });
            table.Rows.Add(new object[] { EnumCommandExecuteType.ExecuteGetDataSet.ToString(), "获取DataSet" });
            return table;
        }

        public static DataTable GetRunningSideSelectItems()
        {
            DataTable table = GetTamplateDataTable();
            table.Rows.Add(new object[] { "", "" });
            table.Rows.Add(new object[] { EnumDeploymentMode.Server.ToString(), "服务端" });
            table.Rows.Add(new object[] { EnumDeploymentMode.Client.ToString(), "客户端" });
            return table;
        }

        public static DataTable GetCommandTypeSelectItems()
        {
            DataTable table = GetTamplateDataTable();
            table.Rows.Add(new object[] { "", "" });
            table.Rows.Add(new object[] { EnumCommandType.Text.ToString(), "文本" });
            table.Rows.Add(new object[] { EnumCommandType.StoredProcedure.ToString(), "存储过程" });
            return table;
        }

        public static DataTable GetSupportedNetTypeNames()
        {
            DataTable table = GetTamplateDataTable();

            table.Rows.Add(new object[] { "", "" });
            table.Rows.Add(typeof(string).FullName, "字符串");
            table.Rows.Add(typeof(Int16).FullName, "整型16");
            table.Rows.Add(typeof(int).FullName, "整型32");
            table.Rows.Add(typeof(long).FullName, "整型64位");
            table.Rows.Add(typeof(UInt16).FullName, "无符号整型16");
            table.Rows.Add(typeof(uint).FullName, "无符号整型32");
            table.Rows.Add(typeof(ulong).FullName, "无符号整型64");
            table.Rows.Add(typeof(decimal).FullName, "devimal");
            table.Rows.Add(typeof(double).FullName, "double");
            table.Rows.Add(typeof(float).FullName, "float");
            table.Rows.Add(typeof(bool).FullName, "布尔");
            table.Rows.Add(typeof(byte).FullName, "byte");
            table.Rows.Add(typeof(DateTime).FullName, "日期");
            table.Rows.Add(typeof(DataTable).AssemblyQualifiedName, "DataTable");
            table.Rows.Add(typeof(DataSet).AssemblyQualifiedName, "DataSet");
            table.Rows.Add(typeof(StringBuilder).AssemblyQualifiedName, "StringBuilder");
            table.Rows.Add(typeof(string).FullName + "[]", "string[]");
            table.Rows.Add(typeof(int).FullName + "[]", "int[]");
            table.Rows.Add(typeof(byte).FullName + "[]", "byte[]");

            return table;
        }

        public static DataTable GetSupportedDbTypeNames_tb()
        {
            DataTable table = GetTamplateDataTable();
            table.Rows.Add(new object[] { "", "" });
            table.Rows.Add(DbType.AnsiString.ToString(), "非Unicode字符");
            table.Rows.Add(DbType.String.ToString(), "Unicode字符");

            table.Rows.Add(DbType.DateTime.ToString(), "日期时间");
            table.Rows.Add(DbType.Binary.ToString(), "布尔");

            table.Rows.Add(DbType.Decimal.ToString(), "Decimal");
            table.Rows.Add(DbType.Double.ToString(), "Double");

            table.Rows.Add(DbType.Int32.ToString(), "整型32");
            table.Rows.Add(DbType.Int64.ToString(), "整型64");

            table.Rows.Add(DbType.Binary.ToString(), "二进制字节流");

            return table;
        }

        public static DataTable GetParameterDirectionSelectItems()
        {
            DataTable table = GetTamplateDataTable();
            table.Rows.Add(new object[] { "", "" });
            table.Rows.Add(new object[] { EnumDataInterfaceParameterDirection.Input.ToString(), "输入" });
            table.Rows.Add(new object[] { EnumDataInterfaceParameterDirection.InputOutput.ToString(), "输入输出" });
            table.Rows.Add(new object[] { EnumDataInterfaceParameterDirection.Output.ToString(), "输出" });
            table.Rows.Add(new object[] { EnumDataInterfaceParameterDirection.Reference.ToString(), "引用" });
            table.Rows.Add(new object[] { EnumDataInterfaceParameterDirection.ReturnValue.ToString(), "返回值" });
            return table;
        }

        public static DataTable GetRuleTypeSelectItems()
        {
            DataTable table = GetTamplateDataTable();
            table.Rows.Add(new object[] { "", "" });
            table.Rows.Add(new object[] { EnumDataInterfaceConverterType.Contrast.ToString(), "对照" });
            table.Rows.Add(new object[] { EnumDataInterfaceConverterType.DataInterface.ToString(), "数据接口" });
            //table.Rows.Add(new object[] { EnumDataInterfaceConverterType.PluginDll.ToString(), "插件" });
            table.Rows.Add(new object[] { EnumDataInterfaceConverterType.Script.ToString(), "脚本" });
            return table;
        }

        private static DataTable GetTamplateDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("value");
            table.Columns.Add("display");

            return table;
        }



        #endregion



    }
}
