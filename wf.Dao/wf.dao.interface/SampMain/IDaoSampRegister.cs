using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoSampRegister
    {
        /// <summary>
        /// 保存试管架条码信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns>成功状态 1保存成功  -1条码号存在  -2顺序号存在  -3保存异常</returns>
        int SaveShelfBarcode(EntitySampRegister data);

        /// <summary>
        /// 删除试管架条码信息
        /// </summary>
        /// <param name="RegSn"></param>
        /// <returns></returns>
        Boolean DeleteShelfBarcode(Int64 RegSn);

        /// <summary>
        /// 根据主键ID获取条码登记表信息
        /// </summary>
        /// <param name="RegSn"></param>
        /// <returns></returns>
        List<EntitySampRegister> GetSampRegister(long RegSn);

        /// <summary>
        /// 试管条码病人登记
        /// </summary>
        /// <param name="receviceDeptID">接收室Id</param>
        /// <param name="regDateFrom">开始登记日期</param>
        /// <param name="regDateTo">结束登记日期</param>
        /// <param name="shelfNoFrom">开始架子号</param>
        /// <param name="shelfNoTo">结束架子号</param>
        /// <param name="seqFrom">开始编号</param>
        /// <param name="seqTo">结束编号</param>
        /// <returns></returns>
        List<EntitySampRegister> GetCuvetteShelfInfo(string receviceDeptID, DateTime regDateFrom, DateTime regDateTo, int? shelfNoFrom, int? shelfNoTo, int? seqFrom, int? seqTo);

        /// <summary>
        /// 获取当天登记试管架
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="depTime"></param>
        /// <returns></returns>
        List<EntitySampRegister> GetCuvetteRegisteredBarcodeInfo(string deptid, DateTime depTime);


        /// <summary>
        /// 排样登记查询统计信息
        /// </summary>
        /// <param name="StatQc"></param>
        /// <returns></returns>
        EntityDCLPrintData GetReportData(EntityStatisticsQC StatQc);
        /// <summary>
        /// 获取条码登记表最大ID
        /// </summary>
        /// <returns></returns>
        int GetSampRegisterMaxId();
    }
}
