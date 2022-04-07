using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Data;

namespace dcl.servececontract
{
    /// <summary>
    /// 报表设计操作接口
    /// </summary>
    [ServiceContract]
    public interface IReportMain
    {
        /// <summary>
        /// 新增报表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse NewReport(EntityRequest request);

        /// <summary>
        /// 新增参数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse NewReportParameter(EntityRequest request);

        /// <summary>
        /// 删除报表以及参数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse DeleteReport(EntityRequest request);

        /// <summary>
        /// 更新报表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse UpdateReport(EntityRequest request);

        /// <summary>
        /// 获得报表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse GetReport(EntityRequest request);

        /// <summary>
        /// 获得报表参数
        /// </summary>
        /// <param name="id">报表id</param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse GetReportPar(string id);

        /// <summary>
        /// 根据报表代码集合获取报表地址
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySysReport> GetRepLocationByListCode(List<string> strCode);

        /// <summary>
        /// 根据报表代码获取报表信息
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        [OperationContract]
         EntitySysReport GetReportByRepCode(string strCode);

        /// <summary>
        /// 返回报表sql执行的datatable结果
        /// </summary>
        /// <param name="dtWhere"></param>
        /// <returns></returns>
        [OperationContract]
        DataTable GetSqlResult(DataTable dtWhere);


        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileNameFullPath"></param>
        /// <param name="strUrlDirPath"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpLoadReportFile(EntityRequest request);

    }
}
