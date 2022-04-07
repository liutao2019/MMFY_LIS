using System;
using System.Collections.Generic;
using System.Text;
using Lib.DAC.Function;

namespace Lib.DAC
{
    public abstract class IDialet
    {

        private readonly IDictionary<string, ISqlFunction> sqlFunctions = new Dictionary<string, ISqlFunction>(StringComparer.InvariantCultureIgnoreCase);

        void RegisterFunction(string name, ISqlFunction function)
        {
            sqlFunctions[name] = function;
        }

        /// <summary>
        /// 是否支持多条sql语句同时执行
        /// </summary>
        public virtual bool SupportMultiQueries
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 多条语句分隔符
        /// </summary>
        public virtual string MultiQueriesSperator
        {
            get
            {
                return ";";
            }
        }

        /// <summary>
        /// 标识查询语句
        /// </summary>
        public virtual string IdentitySelectString
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// 是否支持标识查询
        /// </summary>
        public virtual bool SupportIdentitySelect
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 是否支持查询当前时间
        /// </summary>
        public virtual bool SupportSelectCurrentDate
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 查询当前时间的sql语句
        /// </summary>
        public virtual string CurrentDateSelectString
        {
            get
            {
                return null;
            }
        }

        public virtual DateTime DateTimeMinValue
        {
            get
            {
                //sql server：1753-01-01 00:00:00
                return new DateTime(1753, 1, 1, 0, 0, 0, 0);
            }
        }

        public virtual DateTime DateTimeMaxValue
        {
            get
            {
                //sql server 9999-12-31 23:59:59.997
                return new DateTime(9999, 12, 31, 23, 59, 59, 997);
            }
        }

        protected abstract string KeywordFormatString
        {
            get;
        }

        public string CheckAndFormatKeywordString(string input)
        {
            if (IsKeyword(input))
            {
                return FormatKeywordString(input);
            }
            else
            {
                return input;
            }
        }

        private string FormatKeywordString(string input)
        {
            return string.Format(KeywordFormatString, input);
        }

        public bool IsKeyword(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            return SqlKeywords.Contains(input.ToLower());
        }

        public virtual List<string> SqlKeywords
        {
            get
            {
                List<string> lst = new List<string>();
                lst.Add("select");
                lst.Add("index");
                lst.Add("alter");
                lst.Add("update");
                lst.Add("delete");
                lst.Add("drop");
                lst.Add("table");
                lst.Add("from");
                lst.Add("key");
                lst.Add("group");
                lst.Add("order");
                lst.Add("group");
                lst.Add("by");
                lst.Add("in");
                return lst;
            }
        }

        //protected static List<string> _dbkeywords = null;
        //protected List<string> DbKeywords
        //{
        //    get
        //    {
        //        if(
        //    }
        //}
    }
}
