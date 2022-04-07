using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.dao.core;
using dcl.entity;

namespace dcl.dao.interfaces
{
    public interface IDaoReaStorageDetail : IDaoBase
    {
        bool InsertNewReaStorageDetail(EntityReaStorageDetail purchase);
        List<EntityReaStorageDetail> GetReaStorageDetail(EntityReaQC reaQC);
        bool CancelReaStorageDetail(EntityReaStorageDetail detail);
        bool DeleteReaStorageDetail(string purchaseId, string rea_id);
        /// <summary>
        /// 根据主键ID删除项目结果
        /// </summary>
        /// <param name="obrSn"></param>
        /// <returns></returns>
        bool DeleteObrResultByObrSn(string obrSn);
        List<EntityReaStorageDetail> GetNotdelivered();
        List<EntityReaStorageDetail> GetNotdeliveredByID(string reaid, int num);
        void UpdateDetailStatus(string barcode, int status, int count, string rsdno);
        List<EntityReaStorageDetail> QueryListByDate(DateTime date, string reaid);
    }
}
