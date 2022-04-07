using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoReport
    {
        /// <summary>
        /// 保存报表
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        bool SaveReport(EntitySysReport sample);

        /// <summary>
        /// 更新报表
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        bool UpdateReport(EntitySysReport sample);

        /// <summary>
        /// 删除报表以及参数
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        bool DeleteReport(EntitySysReport sample);
        /// <summary>
        /// 保存报表参数
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool SaveReportParameter(EntitySysReportParameter parameter);
        /// <summary>
        /// 更新报表参数
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool UpdateReportParameter(EntitySysReportParameter parameter);
        /// <summary>
        /// 删除报表参数
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool DeleteReportParameter(EntitySysReportParameter parameter);
        /// <summary>
        /// 获得报表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        List<EntitySysReport> GetReport();
        /// <summary>
        /// 获得报表参数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        List<EntitySysReportParameter> GetReportParameter(int obj);
        
        /// <summary>
        /// 根据报表代码集合获取报表地址
        /// </summary>
        /// <param name="listCode"></param>
        /// <returns></returns>
        List<EntitySysReport> GetRepLocationByListCode(List<string> listCode);

        /// <summary>
        /// 根据报表代码获取报表信息
        /// </summary>
        /// <param name="repCode"></param>
        /// <returns></returns>
        EntitySysReport GetReportByRepCode(string repCode);


        /// <summary>
        /// 上传报表文件
        /// </summary>
        /// <param name="sysRep"></param>
        /// <returns></returns>
        bool UpLoadReportFile(EntitySysReport sysRep);

    }
}
