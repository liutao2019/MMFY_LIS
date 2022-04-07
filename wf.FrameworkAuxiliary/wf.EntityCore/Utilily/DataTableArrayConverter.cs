using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Lib.EntityCore
{
    public static class DataTableArrayConverter
    {
        public static object[] DataTableToArray(DataTable table)
        {
            return DataTableToArray(table, 0);
        }

        public static object[] DataTableToArray(DataTable table, string columnName)
        {
            object[] ret = new object[table.Rows.Count];

            for (int i = 0; i < table.Rows.Count; i++)
            {
                ret[i] = table.Rows[i];
            }

            return ret;
        }

        public static object[] DataTableToArray(DataTable table, int columnIndex)
        {
            string columnName = table.Columns[columnIndex].ColumnName;
            return DataTableToArray(table, columnName);
        }

        public static DataTable ArrayToDataTable(object[] array, string columnName)
        {
            Type datataType = typeof(object);
            if (array.Length > 0)
            {
                datataType = array[0].GetType();
            }

            DataTable table = new DataTable();
            table.Columns.Add(columnName, datataType);

            foreach (object item in array)
            {
                table.Rows.Add(new object[] { item });
            }

            return table;
        }
    }
}
