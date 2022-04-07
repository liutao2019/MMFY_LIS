using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.msg
{
    internal class INSERTSQLSTRING
    {
        private string colNameStr { get; set; }
        private string valueStr { get; set; }
        private string tableNameStr { get; set; }

        public INSERTSQLSTRING(string tableName)
        {
            tableNameStr = tableName;
        }

        public void AddColValue(string colStr, string valStr)
        {
            //如果null则不添加
            if (valStr == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(colNameStr))
            {
                colNameStr = colStr;
            }
            else
            {
                colNameStr += "," + colStr;
            }

            if (string.IsNullOrEmpty(valueStr))
            {
                valueStr = valStr;
            }
            else
            {
                valueStr += "," + valStr;
            }
        }

        public void AddColValueOjb(string colStr, object valObj)
        {
            //如果null则不添加
            if (valObj == null)
            {
                return;
            }

            string valStr = "";

            if (valObj is decimal || valObj is int || valObj is bool || valObj is double || valObj is float)
            {
                if (valObj is bool)
                {
                    valStr = string.Format("{0}", (((bool)valObj) ? 1 : 0));
                }
                else
                {
                    valStr = string.Format("{0}", valObj);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(valObj.ToString()))
                {
                    return;
                }
                else
                {
                    valStr = string.Format("'{0}'", valObj);
                }
            }

            if (string.IsNullOrEmpty(colNameStr))
            {
                colNameStr = colStr;
            }
            else
            {
                colNameStr += "," + colStr;
            }

            if (string.IsNullOrEmpty(valueStr))
            {
                valueStr = valStr;
            }
            else
            {
                valueStr += "," + valStr;
            }
        }

        public string GetInsertSQL()
        {
            string sql = string.Format(@" INSERT INTO {0} ({1}) 
VALUES ({2}) ", tableNameStr, colNameStr, valueStr);
            return sql;
        }
    }
}
