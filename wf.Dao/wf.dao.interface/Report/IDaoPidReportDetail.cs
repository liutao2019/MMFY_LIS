using dcl.dao.core;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoPidReportDetail : IDaoBase
    {
        /// <summary>
        /// 根据标识ID删除病人明细
        /// </summary>
        /// <param name="repId"></param>
        /// <returns></returns>
        bool DeleteReportDetail(string repId);

        /// <summary>
        /// 插入病人组合明细
        /// </summary>
        /// <param name="repDetails"></param>
        /// <returns></returns>
        Boolean InsertNewPidReportDetail(EntityPidReportDetail repDetails);

        /// <summary>
        /// 上传病人组合明细
        /// </summary>
        /// <param name="repDetails"></param>
        /// <returns></returns>
        Boolean UploadNewPidReportDetail(EntityPidReportDetail repDetails);

        /// <summary>
        /// 根据病人标识ID获取病人组合明细
        /// </summary>
        /// <param name="repId"></param>
        /// <returns></returns>
        List<EntityPidReportDetail> GetPidReportDetailByRepId(string repId);

        /// <summary>
        /// 根据旧病人ID更新成新病人ID
        /// </summary>
        /// <param name="newRepId"></param>
        /// <param name="oldRepId"></param>
        /// <returns></returns>
        bool UpdateDetailRepIdByOldRedId(string newRepId, string oldRepId);

        /// <summary>
        /// 更新组合信息
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        bool UpdatePidReportDetailInfo(EntityPidReportDetail detail);

        /// <summary>
        /// 根据多个病人标识ID查询病人组合明细数据
        /// </summary>
        /// <param name="mulitRepId"></param>
        /// <returns></returns>
        List<EntityPidReportDetail> SearchPidReportDetailByMulitRepId(string mulitRepId);

        /// <summary>
        /// 根据条码号和状态查询病人组合ID
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="patFlag"></param>
        /// <returns></returns>
        List<string> GetPidReportDetailByBarcodeAndStatus(string barcode,string patFlag);

        /// <summary>
        /// 是否存在指定医嘱号的报告明细记录
        /// </summary>
        /// <param name="OrderSn"></param>
        /// <returns></returns>
        bool IsExistedOrder(string OrderSn);
    }
}
