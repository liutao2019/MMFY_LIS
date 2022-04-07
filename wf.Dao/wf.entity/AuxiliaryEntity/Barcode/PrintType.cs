using System;
using System.Collections.Generic;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 打印类型
    /// </summary>
    public enum PrintType
    {
        /// <summary>
        /// 住院
        /// </summary>
        Inpatient,
        /// <summary>
        /// 门诊
        /// </summary>
        Outpatient,
        Manual,
        /// <summary>
        /// 体检
        /// </summary>
        TJ,
        /// <summary>
        /// 第二体检系统
        /// </summary>
        TJSecond,
        /// <summary>
        /// 体检－检查
        /// </summary>
        TJPacs,
        /// <summary>
        /// 医生资料
        /// </summary>
        DoctorInterface,
        /// <summary>
        /// 科室资料
        /// </summary>
        DepartInterface,
        /// <summary>
        /// 用户资料
        /// </summary>
        LoginInterface,

        MZInterface,
        ZYInterface,
        TJInterface,
        /// <summary>
        /// 深圳市一体检
        /// </summary>
        SZSYTJ,
        /// <summary>
        /// 外送平台
        /// </summary>
        WSPY
    }

}
