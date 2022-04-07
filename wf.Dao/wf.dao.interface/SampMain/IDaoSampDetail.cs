using dcl.dao.core;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoSampDetail:IDaoBase
    {
        /// <summary>
        /// 获取条码项目明细
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        List<EntitySampDetail> GetSampDetail(String sampBarId);

        /// <summary>
        /// 批量获取条码项目明细
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        List<EntitySampDetail> GetSampDetailByListBarId(List<string> listSampBarId);

        /// <summary>
        /// 删除条码明细
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        /// <summary>
        /// 根据条码号和仪器Id获取条码信息
        /// </summary>
        /// <param name="sampBarId">条码号</param>
        /// <param name="itrId">仪器Id</param>
        /// <returns></returns>
        List<EntitySampDetailMachineCode> GetSampDetailMachineCodeByItrId(String sampBarId,string itrId);

        /// <summary>
        /// 根据条码号获取条码信息
        /// </summary>
        /// <param name="sampBarId">条码号</param>
        /// <returns></returns>
        List<EntitySampDetail> GetSampDetailByBarCode(String sampBarId);

        /// <summary>
        /// 删除条码所有项目
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        Boolean DeleteSampDetailAll(String sampBarId);

        /// <summary>
        /// 删除标本项目
        /// </summary>
        /// <param name="listSampDetail"></param>
        /// <returns></returns>
        Boolean DeleteSampDetail(List<EntitySampDetail> listSampDetail);

        /// <summary>
        /// 根据条码号和病人组合明细ID更新标志
        /// </summary>
        /// <param name="sampBarCode"></param>
        /// <param name="comId"></param>
        /// <returns></returns>
        Boolean UpdateSampDetailSampFlagByComId(string sampBarCode, List<string> listComId,string flag);
        /// <summary>
        /// 查询医嘱ID对应的条码明细
        /// </summary>
        /// <param name="listYzId"></param>
        /// <returns></returns>
        List<EntitySampDetail> GetSampDetailByYzId(List<String> listYzId, string srcName);

        /// <summary>
        /// 根据条码号和lis组合编码获取条码信息
        /// </summary>
        /// <param name="sampBarId">条码号</param>
        /// <param name="listComId">lis组合编码集合</param>
        /// <returns></returns>
        List<EntitySampDetail> GetSampDetailByBarCodeAndComId(String sampBarId, List<string> listComId);

        /// <summary>
        /// 根据条码号和所选上机标志更新上机标志
        /// </summary>
        /// <param name="commflag">上机标志 0为下机 1为上机</param>
        /// <param name="sampBarCode"></param>
        /// <param name="orderCode">his项目代码</param>
        /// <returns></returns>
        Boolean UpdateSampDetailCommFlag(string commflag, string sampBarCode, string orderCode);


        /// <summary>
        /// 根据自增ID更新标志
        /// </summary>
        /// <param name="detSn"></param>
        /// <returns></returns>
        Boolean UpdateSampFlagByDetSn(string detSn);

        /// <summary>
        /// 根据条码号更新标志
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        Boolean UpdateSampFlagByBarCode(string barCode);

        /// <summary>
        /// 根据条码号和组合id查询是否存在条码
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="listComIds"></param>
        /// <returns></returns>
        Int32 GetSampDetailCount(string barcode, List<string> listComIds);

        /// <summary>
        /// 保存标本项目
        /// </summary>
        /// <param name="listSampDetail"></param>
        /// <returns></returns>
        Boolean SaveSampDetail(List<EntitySampDetail> listSampDetail);

        /// <summary>
        /// 判断是否存在不同天的医嘱信息
        /// </summary>
        /// <param name="listSampBarId"></param>
        /// <returns></returns>
        Boolean ExistDifferentOCCDate(List<String> listSampBarId);

        /// <summary>
        /// 通过patid获取医嘱号
        /// </summary>
        /// <param name="RepId"></param>
        /// <returns></returns>
        List<string> GetPatOrderIDs(string RepId);
    }
}
