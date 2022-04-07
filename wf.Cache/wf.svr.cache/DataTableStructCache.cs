using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace dcl.svr.cache
{
    /// <summary>
    /// 表结构缓存
    /// </summary>
    public class DataTableStructCache
    {
        private static DataTableStructCache _instance = null;
        private static object padlock = new object();

        public static DataTableStructCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DataTableStructCache();
                        }
                    }
                }
                return _instance;
            }
        }


        private Dictionary<string, DataTable> tableCache = null;

        private DataTableStructCache()
        {
            tableCache = new Dictionary<string, DataTable>();
        }

        public void AddTableStruct(DataTable table)
        {
            if (string.IsNullOrEmpty(table.TableName))
            {
                return;
            }

            if (!tableCache.Keys.Contains(table.TableName))
            {
                lock (tableCache)
                {
                    if (!tableCache.Keys.Contains(table.TableName))
                    {
                        tableCache.Add(table.TableName, table);
                    }
                }
            }
        }

        public DataTable GetTableStruct(string tableName)
        {
            if (tableCache.Keys.Contains(tableName))
            {
                DataTable table = tableCache[tableName].Clone();
                return table;
            }
            return null;
        }
    }
}
