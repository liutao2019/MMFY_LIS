using System;
using System.Collections.Generic;
using System.Text;


namespace Lib.DAC
{
    public class SqlString2 : SqlString
    {

        public new static SqlString2 Parse(string sqlString)
        {
            SqlString2 output = new SqlString2();
            StringBuilder stringPart = new StringBuilder();

            bool hasMainOutputParameter = sqlString.IndexOf("call") > 0 &&
                                           sqlString.IndexOf("?") > 0 &&
                                           sqlString.IndexOf("=") > 0 &&
                                           sqlString.IndexOf("?") < sqlString.IndexOf("call") &&
                                           sqlString.IndexOf("=") < sqlString.IndexOf("call");
            //bool foundMainOutputParam = false;

            int stringLength = sqlString.Length;
            bool inQuote = false;
            bool inParam = false;
            StringBuilder strParamIndexString = new StringBuilder();
            //bool afterNewLine = false;

            for (int indx = 0; indx < stringLength; indx++)
            {
                // check comments
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

                //afterNewLine
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

                if (indx + NewLineLength - 1 < stringLength && sqlString.Substring(indx, NewLineLength) == Environment.NewLine)
                {
                    //afterNewLine = true;
                    if (inParam)
                    {
                        inParam = !inParam;

                        string strParamStr = strParamIndexString.ToString();

                        int paramindex = -1;

                        if (strParamStr == string.Empty)
                        {
                            throw new ArgumentException("sql参数必须带有序号并且从1开始");
                        }
                        else
                        {
                            paramindex = Convert.ToInt32(strParamStr);
                            if (paramindex <= 0)
                            {
                                throw new ArgumentException("sql参数必须从1开始");
                            }
                        }

                        (output.SqlParts[output.SqlParts.Count - 1] as SqlStringParameter).ParamIndex = paramindex;
                        strParamIndexString = new StringBuilder();
                    }

                    indx += NewLineLength - 1;

                    stringPart.Append(Environment.NewLine);

                    continue;
                }

                //afterNewLine = false;

                char ch = sqlString[indx];

                switch (ch)
                {
                    case '?':
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

                            inParam = true;
                            output.AddParameter();
                        }
                        break;

                    case '\'':
                        inQuote = !inQuote;
                        stringPart.Append(ch);
                        break;

                    case ',':
                    case '，':
                    case '；':
                    case ';':
                    case '）':
                    case ')':
                    case ' ':
                        if (inParam)
                        {
                            inParam = !inParam;

                            string strParamStr = strParamIndexString.ToString();

                            int paramindex = -1;

                            if (strParamStr == string.Empty)
                            {
                                throw new ArgumentException("sql参数必须带有序号并且从1开始");
                            }
                            else
                            {
                                paramindex = Convert.ToInt32(strParamStr);
                                if (paramindex <= 0)
                                {
                                    throw new ArgumentException("sql参数必须从1开始");
                                }
                            }

                            (output.SqlParts[output.SqlParts.Count - 1] as SqlStringParameter).ParamIndex = paramindex;
                            strParamIndexString = new StringBuilder();
                        }
                        stringPart.Append(ch);
                        break;

                    default:
                        if (inParam)
                        {
                            if (char.IsDigit(ch))
                            {
                                strParamIndexString.Append(ch);
                            }
                            else
                            {
                                throw new ArgumentException("sql参数后缀只能为数字");
                            }

                            if (indx == stringLength - 1)
                            {
                                int paramindex = Convert.ToInt32(strParamIndexString.ToString());
                                if (paramindex == 0)
                                {
                                    throw new ArgumentException("sql参数必须从1开始");
                                }
                                (output.SqlParts[output.SqlParts.Count - 1] as SqlStringParameter).ParamIndex = paramindex;
                            }
                        }
                        else
                        {
                            stringPart.Append(ch);
                        }
                        break;
                }
            }

            if (stringPart.Length > 0)
            {
                output.Append(stringPart.ToString(), false);
            }

            return output;

            #region MyRegion

            #endregion
        }

        public SqlString2 AddParameter()
        {
            this.sqlParts.Add(SqlStringParameter.Placeholder);
            return this;
        }
    }
}
