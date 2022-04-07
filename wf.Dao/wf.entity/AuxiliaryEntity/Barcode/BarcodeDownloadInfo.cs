using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace dcl.entity
{
    /// <summary>
    /// 下载条码参数类
    /// </summary>
    [Serializable]
    public class BarcodeDownloadInfo
    {
        public BarcodeDownloadInfo()
        {
            GUID = Guid.NewGuid().ToString();
        }

        public string GUID { get; }

        /// <summary>
        /// 病人ID
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 0)]
        public string PatientID { get; set; }
        /// <summary>
        /// 病人姓名
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 1)]
        public string PatientName { get; set; }
        /// <summary>
        /// 科室ID
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 2)]
        public string DeptID { get; set; }
        /// <summary>
        /// 查询开始时间
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 3)]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 查询结束时间
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 4)]
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 发票号
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 5)]
        public string InvoiceID { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 11)]
        public string CarID { get; set; }
        /// <summary>
        /// 住院/门诊
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 6)]
        public PrintType DownloadType { get; set; }

        /// <summary>
        /// 取HIS数据方式
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 7)]
        public FetchDataType FetchDataType { get; set; }

        /// <summary>
        /// 是否合并
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 8)]
        public bool ShouldMerge { get { return shouldMerge; } set { shouldMerge = value; } }

        /// <summary>
        /// 是否单独的界面,如果是,调用外部DLL
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 9)]
        public bool IsAlone { get { return isAlone; } set { isAlone = value; } }

        public string Conn_search_para { get; set; }
        public string Conn_search_para_text { get; set; }
        public string Conn_LisSearchColumn { get; set; }
        /// <summary>
        /// 是否单独的界面,如果是,调用外部DLL
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 10)]
        public bool IsPrePlaceBarcode { get { return isPrePlaceBarcode; } set { isPrePlaceBarcode = value; } }

        /// <summary>
        /// 执行人
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 12)]
        public string OperationName { get; set; }

        /// <summary>
        /// 执行科室
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, Order = 13)]
        public string OperationDepId { get; set; }

        bool isAlone = false;
        bool shouldMerge = false;
        bool isPrePlaceBarcode = true;

        public string GetDownloadTypeString()
        {
            string result = "";


            switch (this.DownloadType)
            {
                case PrintType.Inpatient:
                    result = "住院条码";
                    break;
                case PrintType.Outpatient:
                    result = "门诊条码";
                    break;
                case PrintType.Manual:
                    break;
                case PrintType.TJ:
                    result = "体检条码";
                    break;
                case PrintType.TJSecond:
                    result = "第二体检条码";
                    break;
                case PrintType.TJPacs:
                    result = "体检检查条码";
                    break;
                case PrintType.MZInterface:
                    result = "门诊接口";
                    break;
                case PrintType.ZYInterface:
                    result = "住院接口";
                    break;
                case PrintType.TJInterface:
                    result = "体检接口";
                    break;
                case PrintType.DoctorInterface:
                    result = "医生资料";
                    break;
                case PrintType.DepartInterface:
                    result = "科室资料";
                    break;
                case PrintType.LoginInterface:
                    result = "用户资料";
                    break;
                case PrintType.SZSYTJ:
                    result = "体检条码";
                    break;
                case PrintType.WSPY:
                    result = "外送平台";
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// 获取来源ID
        /// </summary>
        /// <returns></returns>
        public string GetDownloadTypeId()
        {
            string result = "";


            switch (this.DownloadType)
            {
                case PrintType.Inpatient:
                    result = "108";
                    break;
                case PrintType.Outpatient:
                    result = "107";
                    break;
                case PrintType.Manual:
                    break;
                case PrintType.TJ:
                    result = "109";
                    break;
                case PrintType.TJSecond:
                    result = "109";
                    break;
                case PrintType.SZSYTJ:
                    result = "109";
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// 获取ID类型（门诊号、住院号、体检号）
        /// </summary>
        /// <returns></returns>
        public string GetIdtId()
        {
            if (this.DownloadType == PrintType.Inpatient) //住院：106 门诊：107
                return "106";
            else if (this.DownloadType == PrintType.Outpatient)
                return "107";
            else if (this.DownloadType == PrintType.TJ || this.DownloadType == PrintType.SZSYTJ)
                return "110";
            else if (this.DownloadType == PrintType.TJSecond)
                return "110";

            return "";
        }


        /// <summary>
        /// 获取调用参数
        /// </summary>
        /// <returns></returns>
        public List<string> GetDownloadParameter()
        {
            List<string> listPara = new List<string>();

            listPara.Add(PatientID);
            listPara.Add(StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
            listPara.Add(EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
            listPara.Add(PatientName);
            listPara.Add(DeptID);
            listPara.Add(InvoiceID);

            return listPara;
        }
    }

    /// <summary>
    /// 从HIS取数据方式
    /// </summary>
    [Serializable]
    public enum FetchDataType
    {
        Normal, //中间表
        OutLink //佛山市一outlink
    }


}
