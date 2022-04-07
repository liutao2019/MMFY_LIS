using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoSampStoreDetail
    {
        int ModifySamDetail(EntitySampStoreDetail entity);

        List<EntitySampStoreDetail> GetRackDetail(string strSsid);

        /// <summary>
        /// 更新试管存储信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int UpdateSamDetail(EntitySampStoreDetail entity);

        /// <summary>
        /// 根据条件查询试管存储信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        List<EntitySampStoreDetail> GetSamDetail(DateTime date, string BatchItr, string SamFrom, string SamTo, int selectIndex);
        /// <summary>
        /// 获取归档架子样本信息
        /// </summary>
        /// <returns></returns>
        List<EntitySampStoreDetail> GetSimpleRackDetail();
        /// <summary>
        /// 将数据归档到表SamStore_RackDetail
        /// </summary>
        /// <param name="SampDetail"></param>
        /// <returns></returns>
        bool AddSampStoreDetail(EntitySampStoreDetail SampDetail);

        /// <summary>
        /// 删除试管存储信息
        /// </summary>
        /// <param name="strSsid"></param>
        /// <param name="rackID"></param>
        /// <param name="barcodeList"></param>
        /// <returns></returns>
        int DeleteSampStoreDetail(string strSsid, string barcode);

        /// <summary>
        /// 根据ID获取数据数量
        /// </summary>
        /// <param name="strSsid"></param>
        /// <param name="rackID"></param>
        /// <param name="barcodeList"></param>
        /// <returns></returns>
        int GetSampStoreDetailCount(string strSsid);
    }
}
