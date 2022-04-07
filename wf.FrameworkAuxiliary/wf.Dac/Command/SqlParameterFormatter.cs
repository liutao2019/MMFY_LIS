using System;
using System.Collections.Generic;
using System.Text;
using Lib.DAC.DbDriver;

namespace Lib.DAC
{
    internal class SqlParameterFormatter
    {
        /// <summary>
        /// SqlString参数格式化
        /// 根据驱动的不同生成不同的参数形式，如：Oracle生成 :p1 ；SqlServer生成 @p1 ,ODBC与Oledb生成 ？
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="driver"></param>
        /// <returns></returns>
        public string GetFormatedSQL(SqlString sqlString, IDbDriver driver)
        {
            StringBuilder builder = new StringBuilder(sqlString.SqlParts.Count * 15);

            int paramIndex = 1;

            //遍历SqlString的所有sql元素
            for (int i = 0; i < sqlString.SqlParts.Count; i++)
            {
                if (sqlString.SqlParts[i] is SqlStringParameter)
                {
                    SqlStringParameter param = sqlString.SqlParts[i] as SqlStringParameter;

                    if (param.ParamIndex <= 0)//如果为无指定顺序的sql参数
                    {
                        //如果为参数则根据驱动生成参数名，:p1 :p2；@p1 @p2
                        builder.Append(driver.GetParameterName(paramIndex));
                        paramIndex++;
                    }
                    else
                    {
                        builder.Append(driver.GetParameterName(param.ParamIndex));
                        paramIndex++;
                    }
                }
                else
                {
                    //不为参数则继续累加
                    builder.Append(sqlString.SqlParts[i].ToString());
                }
            }

            return builder.ToString();
        }
    }
}
