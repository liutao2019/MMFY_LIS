using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;
using System.Collections;

namespace dcl.dao.interfaces
{
    public interface IDaoAnalyse
    {
        /// <summary>
        /// 获取报告数据信息
        /// </summary>
        /// <param name="statQC"></param>
        /// <returns></returns>
        DataSet GetReportData(EntityStatisticsQC statQC);
    }
}
