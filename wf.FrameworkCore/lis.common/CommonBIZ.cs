namespace dcl.common
{
    using System.Data;

    public class CommonBIZ
    {
        public static DataTable createErrorInfo(string showMSG, string detail)
        {
            DataTable table = getErrorTable();
            table.Rows[0]["_ERROR_MSG"] = showMSG;
            table.Rows[0]["_ERROR_DETAIL"] = detail;
            return table;
        }

        public static DataTable getErrorTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("_ERROR_MSG");
            table.Columns.Add("_ERROR_DETAIL");
            table.Columns.Add("_ERROR_CODE");
            table.TableName = "_ERRORINFO";
            table.Rows.Add(new object[0]);
            table.Rows[0]["_ERROR_CODE"] = "0";
            return table;
        }
    }
}

