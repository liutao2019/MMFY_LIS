using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface ISecondSign
    {
        /// <summary>
        /// 保存试管架条码信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns>成功状态 1保存成功  -1条码号存在  -2顺序号存在  -3保存异常</returns>
        [OperationContract]
        int SaveShelfBarcode(EntitySampRegister data);

        /// <summary>
        /// 删除试管架条码信息
        /// </summary>
        /// <param name="RegSn"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean DeleteShelfBarcode(Int64 RegSn);

        /// <summary>
        /// 根据主键ID获取条码登记表信息
        /// </summary>
        /// <param name="RegSn"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampRegister> GetSampRegister(long RegSn);

        /// <summary>
        /// 获取当天登记试管架
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="depTime"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampRegister> GetCuvetteRegisteredBarcodeInfo(string deptid, DateTime depTime);

        /// <summary>
        /// 根据组合ID获取仪器信息
        /// </summary>
        /// <param name="ComIdList"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicInstrument> GetInstrumentByComIds(List<string> ComIdList);

        /// <summary>
        /// 排样登记查询统计信息
        /// </summary>
        /// <param name="StatQc"></param>
        /// <returns></returns>
        [OperationContract]
        EntityDCLPrintData GetReportData(EntityStatisticsQC StatQc);

        /// <summary>
        /// 获取条码登记表的最大ID
        /// </summary>
        [OperationContract]
        int GetSampRegisterMaxId();

        /// <summary>
        /// 获取最新条码操作明细
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="name"></param>
        /// <param name="time"></param>
        /// <param name="remark"></param>
        /// <param name="status"></param>
        [OperationContract]
        void GetLastBarcodeAction(string barcode, out string name, out string time, out string remark, int status);

    }
}
