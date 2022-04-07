using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface ISampRegister
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
        /// 获取条码登记表的最大ID
        /// </summary>
        [OperationContract]
        int GetSampRegisterMaxId();
    }
}
