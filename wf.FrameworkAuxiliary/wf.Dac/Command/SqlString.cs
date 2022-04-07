using System;
using System.Collections.Generic;
using System.Text;


namespace Lib.DAC
{
    /// <summary>
    /// 封装的sql语句对象
    /// 分为3类：string类型存放普通的sql语句，SqlStringComment存放sql注释，SqlParameterFormatter存放sql参数
    /// </summary>
    [Serializable]
    public class SqlString : ISqlStringPart, ICloneable
    {
        protected List<object> sqlParts;

        #region ctor
        public SqlString(string sqlPart)
        {
            if (StringTool.IsNotEmpty(sqlPart))
            {
                this.sqlParts = new List<object> { sqlPart };
            }
            else
            {
                this.sqlParts = new List<object>();
            }
        }

        public SqlString(object[] sqlParts)
        {
            this.sqlParts = new List<object>(sqlParts);
        }

        public SqlString()
        {
            this.sqlParts = new List<object>();
        }
        #endregion

        protected static int NewLineLength = Environment.NewLine.Length;

        /// <summary>
        /// 把sql语句转换成SqlString对象：拆分注释、参数、普通sql语句
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public static SqlString Parse(string sqlString)
        {
            //把sql字符串转换成SqlString对象

            SqlString output = new SqlString();
            StringBuilder stringPart = new StringBuilder();

            bool hasMainOutputParameter = sqlString.IndexOf("call") > 0 &&
                                           sqlString.IndexOf("?") > 0 &&
                                           sqlString.IndexOf("=") > 0 &&
                                           sqlString.IndexOf("?") < sqlString.IndexOf("call") &&
                                           sqlString.IndexOf("=") < sqlString.IndexOf("call");
            //bool foundMainOutputParam = false;

            int stringLength = sqlString.Length;

            //是否在单引号内
            bool inQuote = false;
            //bool afterNewLine = false;

            //遍历sql字符串的每一个字符
            for (int indx = 0; indx < stringLength; indx++)
            {
                // 检查是否为/* */类型的注释
                if (indx + 1 < stringLength && sqlString.Substring(indx, 2) == "/*")
                {
                    if (stringPart.Length > 0)
                    {
                        output.Append(stringPart.ToString());
                        stringPart.Length = 0;
                    }

                    var closeCommentIdx = sqlString.IndexOf("*/", indx + 2);

                    if ((closeCommentIdx + Environment.NewLine.Length < stringLength)
                        && sqlString.Substring(closeCommentIdx + Environment.NewLine.Length, Environment.NewLine.Length) == Environment.NewLine)
                    {
                        closeCommentIdx = closeCommentIdx + Environment.NewLine.Length;
                    }

                    string comment = sqlString.Substring(indx, (closeCommentIdx - indx) + 2);

                    output.Append(new SqlStringComment(comment, true));

                    indx = closeCommentIdx + 1;

                    continue;
                }

                //检查是否为 -- 类型的注释
                if (!inQuote && (indx + 1 < stringLength) && sqlString.Substring(indx, 2) == "--")
                {
                    if (stringPart.Length > 0)
                    {
                        output.Append(stringPart.ToString());
                        stringPart.Length = 0;
                    }

                    var closeCommentIdx = sqlString.IndexOf(Environment.NewLine, indx + 2);
                    string comment;
                    if (closeCommentIdx == -1)
                    {
                        closeCommentIdx = sqlString.Length;
                        comment = sqlString.Substring(indx);
                    }
                    else
                    {
                        comment = sqlString.Substring(indx, closeCommentIdx - indx);// + Environment.NewLine.Length);
                    }

                    output.Append(new SqlStringComment(comment, false));

                    indx = closeCommentIdx - 1;// +NewLineLength - 1;
                    continue;
                }

                //检查是否为换行
                if (indx + NewLineLength - 1 < stringLength && sqlString.Substring(indx, NewLineLength) == Environment.NewLine)
                {
                    //afterNewLine = true;
                    indx += NewLineLength - 1;

                    stringPart.Append(Environment.NewLine);

                    continue;
                }

                //afterNewLine = false;

                char ch = sqlString[indx];

                switch (ch)
                {

                    case '?'://问号转换成SqlStringParameter参数，如果问号在单引号内则不转换
                        if (inQuote)
                        {
                            stringPart.Append(ch);
                        }
                        else
                        {
                            if (stringPart.Length > 0)
                            {
                                output.Append(stringPart.ToString(), false);
                                stringPart.Length = 0;
                            }
                            output.AppendParameter();
                        }
                        break;

                    case '\''://如果为单引号
                        inQuote = !inQuote;
                        stringPart.Append(ch);
                        break;

                    default:
                        stringPart.Append(ch);
                        break;
                }

                //if (inQuote)
                //{
                //    if ('\'' == c)
                //    {
                //        inQuote = false;
                //    }
                //    //recognizer.Other(c);
                //    stringPart.Append(c);
                //}
                //else if ('\'' == c)
                //{
                //    inQuote = true;
                //recognizer.Other(c);
                //}
                //    else
                //    {
                //        if (c == ':')
                //        {
                //            // named parameter
                //            //int right = StringHelper.FirstIndexOfChar(sqlString, ParserHelper.HqlSeparators, indx + 1);
                //            //int chopLocation = right < 0 ? sqlString.Length : right;
                //            //string param = sqlString.Substring(indx + 1, chopLocation - (indx + 1));
                //            //recognizer.NamedParameter(param, indx);
                //            //indx = chopLocation - 1;
                //        }
                //        else if (c == '?')
                //        {
                //            // could be either an ordinal or ejb3-positional parameter
                //            if (indx < stringLength - 1 && char.IsDigit(sqlString[indx + 1]))
                //            {
                //                // a peek ahead showed this as an ejb3-positional parameter
                //                //int right = StringHelper.FirstIndexOfChar(sqlString, ParserHelper.HqlSeparators, indx + 1);
                //                //int chopLocation = right < 0 ? sqlString.Length : right;
                //                //string param = sqlString.Substring(indx + 1, chopLocation - (indx + 1));
                //                // make sure this "name" is an integral
                //                //try
                //                //{
                //                //    int.Parse(param);
                //                //}
                //                //catch (FormatException e)
                //                //{
                //                //throw new QueryException("ejb3-style positional param was not an integral ordinal", e);
                //                //}
                //                //recognizer.JpaPositionalParameter(param, indx);
                //                //indx = chopLocation - 1;
                //            }
                //            else
                //            {
                //                if (hasMainOutputParameter && !foundMainOutputParam)
                //                {
                //                    foundMainOutputParam = true;
                //                    //recognizer.OutParameter(indx);
                //                }
                //                else
                //                {
                //                    //recognizer.OrdinalParameter(indx);
                //                }
                //            }
                //        }
                //        else
                //        {
                //            //recognizer.Other(c);
                //        }
                //    }
            }

            if (stringPart.Length > 0)
            {
                output.Append(stringPart.ToString(), false);
            }

            return output;

            #region MyRegion
            //SqlString sqlString = new SqlString(string.Empty);

            //bool inQuote = false;

            //StringBuilder stringPart = new StringBuilder();

            //foreach (char ch in sql)
            //{
            //    switch (ch)
            //    {
            //        case '?':
            //            if (inQuote)
            //            {
            //                stringPart.Append(ch);
            //            }
            //            else
            //            {
            //                if (stringPart.Length > 0)
            //                {
            //                    sqlString.Append(stringPart.ToString());
            //                    stringPart.Length = 0;
            //                }
            //                sqlString.AddParameter();
            //            }
            //            break;

            //        case '\'':
            //            inQuote = !inQuote;
            //            stringPart.Append(ch);
            //            break;

            //        default:
            //            stringPart.Append(ch);
            //            break;
            //    }
            //}

            //if (stringPart.Length > 0)
            //{
            //    sqlString.Append(stringPart.ToString());
            //}

            //return sqlString; 
            #endregion
        }

        /// <summary>
        /// 追加ISqlStringPart对象
        /// </summary>
        /// <param name="sqlPart"></param>
        /// <returns></returns>
        public SqlString Append(ISqlStringPart sqlPart)
        {
            if (sqlPart is SqlString)
            {
                foreach (object item in (sqlPart as SqlString).SqlParts)
                {
                    this.sqlParts.Add(item);
                }
            }
            else
            {
                this.sqlParts.Add(sqlPart);
            }
            return this;
        }

        /// <summary>
        /// 追加sql字符串
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parse"></param>
        /// <returns></returns>
        public SqlString Append(string sql, bool parse)
        {
            if (parse)
            {
                SqlString sqlString = SqlString.Parse(sql);

                foreach (object item in sqlString.SqlParts)
                {
                    this.sqlParts.Add(item);
                }
            }
            else
            {
                this.sqlParts.Add(sql);
            }

            return this;
        }

        /// <summary>
        /// 追加sql字符串
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public SqlString Append(string sql)
        {
            return Append(sql, true);
        }

        /// <summary>
        /// 追加新行
        /// </summary>
        /// <returns></returns>
        public SqlString AppendNewLine()
        {
            return Append(Environment.NewLine, false);
        }

        /// <summary>
        /// 追加sql参数
        /// </summary>
        /// <returns></returns>
        public SqlString AppendParameter()
        {
            this.sqlParts.Add(SqlStringParameter.Placeholder);
            return this;
        }

        //public SqlString AddParameter(string paramName)
        //{
        //    this.sqlParts.Add(new SqlParameter(paramName));
        //    return this;
        //}

        public List<object> SqlParts
        {
            get
            {
                return this.sqlParts;
            }
        }

        /// <summary>
        /// sql元素个数
        /// </summary>
        public int Count
        {
            get { return this.sqlParts.Count; }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(sqlParts.Count * 15);

            for (int i = 0; i < sqlParts.Count; i++)
            {
                builder.Append(sqlParts[i].ToString());
            }

            return builder.ToString();
        }

        //public string ToParameterizedString()
        //{
        //    StringBuilder builder = new StringBuilder(sqlParts.Count * 15);

        //    for (int i = 0; i < sqlParts.Count; i++)
        //    {
        //        if (sqlParts[i] is SqlParameter)
        //        {
        //            builder.Append((sqlParts[i] as SqlParameter).ParameterName);
        //        }
        //        else
        //        {
        //            builder.Append(sqlParts[i].ToString());
        //        }
        //    }

        //    return builder.ToString();
        //}


        bool _isCompact = false;

        /// <summary>
        /// 压缩SqlString语句
        /// </summary>
        /// <returns></returns>
        public SqlString Compat()
        {
            if (!_isCompact)//如果已经压缩过则不再压缩，避免性能损耗
            {
                SqlString sql = _Compat();
                sql = sql._Compat();
                this._isCompact = true;
                return sql;
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// 压缩SqlString语句
        /// </summary>
        /// <returns></returns>
        private SqlString _Compat()
        {
            //遍历每一个元素，移除可以移除的备注、拼接相邻相同的两种ISqlStringPart,最后生成新的SqlString

            List<object> newParts = new List<object>();

            for (int i = 0; i < this.sqlParts.Count; i++)//遍历SqlString的sqlParts每一个元素
            {
                object part = this.sqlParts[i];
                if (part is SqlStringComment)//如果为备注
                {
                    if ((part as SqlStringComment).CanRemove)//如果能移除，则直接跳过
                    {
                        continue;
                    }
                    newParts.Add(part);
                }
                else
                {
                    //如果相邻的两个ISqlStringPart为同一类型则拼接
                    if (part != null && part.ToString() != string.Empty)
                    {
                        if (part.ToString() == Environment.NewLine
                            && i < this.sqlParts.Count - 1
                            && this.sqlParts[i + 1].ToString().StartsWith(Environment.NewLine)
                            )
                        {
                            continue;
                        }

                        if (part is string && newParts.Count > 0 && newParts[newParts.Count - 1] is string)
                        {
                            newParts[newParts.Count - 1] = newParts[newParts.Count - 1].ToString() + part as string;
                            continue;
                        }

                        newParts.Add(part);
                    }
                }
            }

            this.sqlParts = newParts;
            return this;
        }

        /// <summary>
        /// 获取所有参数对象
        /// </summary>
        /// <returns></returns>
        public List<SqlStringParameter> GetParameter()
        {
            List<SqlStringParameter> list = new List<SqlStringParameter>();

            foreach (object item in this.sqlParts)
            {
                if (item is SqlStringParameter)
                {
                    list.Add(item as SqlStringParameter);
                }
            }

            return list;
        }

        /// <summary>
        /// 设置备注
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public SqlString SetComment(string comment)
        {
            SqlStringComment c = new SqlStringComment(string.Format("/*{0}*/{1}", comment, Environment.NewLine), true);
            c.CanRemove = false;

            this.sqlParts.Insert(0, c);
            return this;
        }

        #region ICloneable 成员

        public object Clone()
        {
            SqlString ret = new SqlString(this.sqlParts.ToArray());
            return ret;
        }

        #endregion
    }
}
