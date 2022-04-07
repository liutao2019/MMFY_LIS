using dcl.entity;
using System;
using System.Collections.Generic;
using System.Text;


namespace dcl.common
{
    public class PatientsHelper
    {
        /// <summary>
        /// 是否已审核或打印,报告
        /// </summary>
        /// <param name="patFlag"></param>
        /// <returns></returns>
        public static bool HasAudited(string patFlag)
        {
            return patFlag == LIS_Const.PATIENT_FLAG.Audited
                                    || patFlag == LIS_Const.PATIENT_FLAG.Printed
                                     || patFlag == LIS_Const.PATIENT_FLAG.Reported;
        }
    }
}
