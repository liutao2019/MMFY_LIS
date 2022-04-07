using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 执行接口参数
    /// </summary>
    public class EntityInterfaceExtParameter : EntityBase
    {
        public EntityInterfaceExtParameter()
        {
            GUID = Guid.NewGuid().ToString();
            OutlinkInterface = false;
        }

        /// <summary>
        /// 客户端调用标识ID,用于防止医嘱下载并发
        /// </summary>
        public string GUID { get; }

        /// <summary>
        /// 执行开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 执行结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 科室ID
        /// </summary>
        public string DeptID { get; set; }

        /// <summary>
        /// 病人名称
        /// </summary>
        public string PatientName { get; set; }

        /// <summary>
        /// 病人ID
        /// </summary>
        public string PatientID { get; set; }

        /// <summary>
        /// 发票号
        /// </summary>
        public string InvoiceID { get; set; }

        /// <summary>
        /// 接口类型
        /// </summary>
        public InterfaceType DownloadType { get; set; }

        /// <summary>
        /// 执行人
        /// </summary>
        public string OperationName { get; set; }

        /// <summary>
        /// 病人ID检索类型
        /// </summary>
        public string LisSearchColumn { get; set; }

        /// <summary>
        /// Outlink接口
        /// </summary>
        public bool OutlinkInterface { get; set; }

        /// <summary>
        /// Outlink接口返回数据
        /// </summary>
        public DataSet OutlinkData { get; set; }

        /// <summary>
        /// 获取来源ID
        /// </summary>
        /// <returns></returns>
        public string GetSrcId()
        {
            string result = "";

            switch (this.DownloadType)
            {
                case InterfaceType.MZDownload:
                    result = "107";
                    break;
                case InterfaceType.ZYDownload:
                    result = "108";
                    break;
                case InterfaceType.TJDownload:
                    result = "109";
                    break;
                case InterfaceType.OutsideDownload:
                    result = "110";
                    break;
                case InterfaceType.MZPatient:
                    result = "107";
                    break;
                case InterfaceType.ZYPatient:
                    result = "108";
                    break;
                case InterfaceType.TJPatient:
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
            if (this.DownloadType == InterfaceType.ZYDownload) //住院：106 门诊：107
                return "106";
            else if (this.DownloadType == InterfaceType.MZDownload)
                return "107";
            else if (this.DownloadType == InterfaceType.TJDownload) //InterfaceType.TJPatient
                return "110";
            else if (this.DownloadType == InterfaceType.OutsideDownload) //外院
                return "109";
            return "";
        }

        /// <summary>
        /// 获取调用接口名称
        /// </summary>
        /// <returns></returns>
        public string GetInterfaceTypeString()
        {
            string result = "";

            switch (this.DownloadType)
            {
                case InterfaceType.MZDownload:
                    result = "门诊条码";
                    break;
                case InterfaceType.ZYDownload:
                    result = "住院条码";
                    break;
                case InterfaceType.TJDownload:
                    result = "体检条码";
                    break;
                case InterfaceType.OutsideDownload:
                    result = "外部条码";
                    break;
                case InterfaceType.MZPatient:
                    result = "门诊接口";
                    break;
                case InterfaceType.ZYPatient:
                    result = "住院接口";
                    break;
                case InterfaceType.TJPatient:
                    result = "体检接口";
                    break;
                case InterfaceType.DoctorInterface:
                    result = "医生资料";
                    break;
                case InterfaceType.DepartInterface:
                    result = "科室资料";
                    break;
                case InterfaceType.LoginInterface:
                    result = "用户资料";
                    break;
                default:
                    break;
            }

            return result;
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

        /// <summary>
        ///需要过滤的标本类别
        /// </summary>
        public string MzFiterSams { get; set; }

    }
}
