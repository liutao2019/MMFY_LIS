using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.dao.core;
using dcl.entity;

namespace dcl.dao.interfaces
{
    public interface IDaoReaDeliveryDetail : IDaoBase
    {
        bool InsertNewReaDeliveryDetail(EntityReaDeliveryDetail purchase);
        List<EntityReaDeliveryDetail> GetReaDeliveryDetail(EntityReaQC reaQC);
        bool CancelReaDeliveryDetail(EntityReaDeliveryDetail detail);
        bool DeleteReaDeliveryDetail(string purchaseId, string rea_id);
        /// <summary>
        /// 根据主键ID删除项目结果
        /// </summary>
        /// <param name="obrSn"></param>
        /// <returns></returns>
        bool DeleteObrResultByObrSn(string obrSn);
    }
}
