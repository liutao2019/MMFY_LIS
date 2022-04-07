using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    public enum InterfaceType
    {
        /// <summary>
        /// 门诊医嘱下载
        /// </summary>
        MZDownload,
        /// <summary>
        /// 住院医嘱下载
        /// </summary>
        ZYDownload,
        /// <summary>
        /// 体检医嘱下载
        /// </summary>
        TJDownload,
        /// <summary>
        /// 外部条码下载
        /// </summary>
        OutsideDownload,
        /// <summary>
        /// 门诊病人资料获取
        /// </summary>
        MZPatient,
        /// <summary>
        /// 住院病人资料获取
        /// </summary>
        ZYPatient,
        /// <summary>
        /// 体检病人资料获取
        /// </summary>
        TJPatient,
        /// <summary>
        /// 条码资料获取
        /// </summary>
        BarcodePatient,
        /// <summary>
        /// 医生资料同步
        /// </summary>
        DoctorInterface,
        /// <summary>
        /// 科室资料同步
        /// </summary>
        DepartInterface,
        /// <summary>
        /// 用户资料同步
        /// </summary>
        LoginInterface
    }
}
