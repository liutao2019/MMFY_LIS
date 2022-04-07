using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Text.RegularExpressions;

namespace dcl.common
{
    public enum SplitType
    {
        /// <summary>
        /// 住院
        /// </summary>
        PatInfo,
        /// <summary>
        /// 门诊
        /// </summary>
        MzInfo
    }

    public class ConvertHelper
    {
        static ConvertHelper()
        {

        }

        public static string BuildHospitalSqlWhere(string column)
        {
            string hosID = ConfigurationManager.AppSettings["HospitalID"];
            if (hosID == null || string.IsNullOrEmpty(hosID))
                return string.Empty;

            return string.Format(" and ({0}='{1}' or {0}='' or {0} is null) ", column, hosID);
        }


        #region outlink 用


        public DataSet ConvertToDataSet(string oneLineData)
        {
            return ConvertToDataSet(oneLineData, SplitType.PatInfo);
        }

        /// <summary>
        /// e"PatData={(PATIENTID=1147460;);(PATIENTID=1147460;);};";
        /// </summary>
        /// <param name="oneLineData"></param>
        /// <param name="splitType">条码下载时用SplitType.MzInfo</param>
        /// <returns></returns>
        public DataSet ConvertToDataSet(string oneLineData, SplitType splitType)
        {
            if (string.IsNullOrEmpty(oneLineData) || oneLineData.Contains("Data={}"))
                return null;

            if (oneLineData.StartsWith("Error"))
            {
                throw new InterfacesException(oneLineData.Replace("Error=", ""));
                // return new DataSet();
            }
            string head = GetDataHead(oneLineData);
            string dataString = GetDataString(oneLineData);

            string splitWord = splitType == SplitType.PatInfo ? ";);(" : "\r\n";
            string[] rows = dataString.Split(new string[] { splitWord }, StringSplitOptions.RemoveEmptyEntries);

            DataTable result = new DataTable(head);

            foreach (string rowOne in rows)
            {
                string recode = rowOne;
                recode = ReplaceBadData(recode, "FEENM.+Qty");
                recode = ReplaceBadData(recode, "DiagnNM.+JyBbNo");
                List<string> keyValues = new List<string>();

                if (string.IsNullOrEmpty(recode))
                    continue;
                string oneRow = GetRowDataString(recode);
                // oneRow.IndexOf("
                foreach (string item in oneRow.Split(';'))
                {
                    string[] keyValue = item.Split('=');
                    if (keyValue != null && keyValue.Length > 0)
                    {
                        keyValues.Add(keyValue[1].Trim('"').Trim());
                        //增加列
                        if (!result.Columns.Contains(keyValue[0].Trim()))
                            result.Columns.Add(keyValue[0].Trim());
                    }
                }

                //加行
                DataRow row = result.NewRow();

                for (int i = 0; i < result.Columns.Count; i++)
                {
                    row[i] = keyValues[i];
                }
                result.Rows.Add(row);

                //插入其他表
                this.HandlerOtherRows(row, EventArgs.Empty);
            }

            DataSet dsResult = new DataSet();
            dsResult.Tables.Add(result);
            return dsResult;
        }

        #endregion

        public event EventHandler OnHandlerOtherRows;
        private void HandlerOtherRows(object sender, EventArgs args)
        {
            if (OnHandlerOtherRows != null)
                OnHandlerOtherRows(sender, args);
        }

        static string GetRowDataString(string rowOne)
        {

            string oneRow = rowOne.TrimStart('(');
            if (oneRow.IndexOf(";)") > 0)
                oneRow = oneRow.Remove(oneRow.IndexOf(";)"));
            return oneRow;
        }

        /// <summary>
        /// 取报头
        /// </summary>
        /// <param name="oneLineData"></param>
        /// <returns></returns>
        static string GetDataHead(string oneLineData)
        {
            int endIndex = oneLineData.IndexOf("={(");
            if (endIndex < 0)
                return "";
            return oneLineData.Substring(0, endIndex);
        }

        /// <summary>
        /// 取数据字符串
        /// </summary>
        /// <param name="oneLineData"></param>
        /// <returns></returns>
        static string GetDataString(string oneLineData)
        {
            string startIndexStr = "{";
            string endIndexStr = ";};";

            int startIndex = oneLineData.IndexOf(startIndexStr);

            int endIndex = oneLineData.IndexOf(endIndexStr);
            if (startIndex < 0 || endIndex < 0)
                return oneLineData;
            int dataStartIndex = startIndex + startIndexStr.Length;
            return oneLineData.Substring(dataStartIndex, endIndex - dataStartIndex);
        }


        /// <summary>  
        /// 将字符型类型转换为整型值  
        /// </summary>
        /// <param name="objValue">字符型</param>  
        /// <param name="defaultValue">无法转换时的默认值</param>  
        /// <returns>整型值</returns>  
        public static int IntParse(string objValue, int defaultValue)
        {
            int returnValue = defaultValue;
            if (!string.IsNullOrEmpty(objValue))
            {
                try
                {
                    returnValue = int.Parse(objValue);
                }
                catch
                {
                    returnValue = defaultValue;
                }
            }
            return returnValue;
        }



        /// <summary>
        /// 格式化不合格的数据
        /// </summary>
        /// <param name="badData"></param>
        /// <returns></returns>
        string ReplaceBadData(string badData, string pattern)
        {
            Regex regex = new Regex(pattern);//
            Match match = regex.Match(badData);
            string str = match.Value;
            MatchCollection matchCollection = Regex.Matches(str, ";");
            if (matchCollection.Count <= 1) //如果只有一个分号,则是正确格式
                return badData;
            else //非正确格式下代替掉分号
            {
                int lastIndex = str.LastIndexOf(';');
                string content = str.Substring(0, lastIndex);
                string newcontent = content.Replace(";", "+");
                badData = badData.Replace(content, newcontent);
            }
            return badData;
        }
    }

    public abstract class IBarcodeGenerateRule
    {
        protected string GetMaxBarcodeNumber()
        {
            return "";
        }

        public virtual string GenerateBarcode(string barcode)
        {
            return barcode;
        }

        public abstract int BarcodeLength { get; }

        /// <summary>
        /// 使用2位的校验位 0=不使用 1=2位前缀 2=2位后缀 3=自定义前缀
        /// </summary>
        public int CheckCodeType { get; set; }

        /// <summary>
        /// 条码自定义前缀
        /// </summary>
        public string barcodeCustomHead { get; set; }
    }

    /// <summary>
    /// 默认条码号，10位，前2位由前10位数学相加而成，后10位自增s
    /// </summary>
    public class DefaultGenerateRule : IBarcodeGenerateRule
    {
        public override string GenerateBarcode(string barcode)
        {
            if (string.IsNullOrEmpty(barcode))
                return "";

            if (this.CheckCodeType == 0)//不使用校验位
                return barcode;

            int sum = 0;

            if (this.CheckCodeType == 3)//自定义前缀
            {
                if (!string.IsNullOrEmpty(this.barcodeCustomHead))
                {
                    return this.barcodeCustomHead + barcode;
                }
                return barcode;
            }

            foreach (char number in barcode.ToCharArray())
            {
                int iNumber = Int32.Parse(number.ToString());
                sum += iNumber;
            }

            if (this.CheckCodeType == 1)//2位前缀
                return sum.ToString("D2") + barcode;
            else if (this.CheckCodeType == 2)//2位后缀
                return barcode + sum.ToString("D2");
            else
                return sum.ToString("D2") + barcode;
        }



        public override int BarcodeLength
        {
            get { return 10; }
        }
    }

    public class InterfacesException : Exception
    {
        public InterfacesException(string message)
            : base(message)
        {

        }
    }
}
