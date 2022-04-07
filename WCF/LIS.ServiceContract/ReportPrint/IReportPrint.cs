using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IReportPrint
    {
        /// <summary>
        /// 获取报表数据源
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [OperationContract]
        EntityDCLPrintData GetReportSource(EntityDCLPrintParameter parameter);

        /// <summary>
        /// 多图像报表 Picture_pat特殊前缀报表
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDCLPrintData> GetPictureReportSource(EntityDCLPrintParameter parameter);

        /// <summary>
        /// 细菌报表特殊处理
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [OperationContract]
        EntityDCLPrintData GetBacReportSource(EntityDCLPrintParameter parameter);

        /// <summary>
        /// 获取PDF报告
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [OperationContract]
        String GetReportPDF(EntityDCLPrintParameter parameter);
    }
}
