using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoSampStock
    {
        /// <summary>
        /// 根据时间段获取架子信息
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        List<EntityDicSampTubeRack> GetDictRackList(DateTime startTime,DateTime endTime);
        
        /// <summary>
        /// 根据条码获取病人信息
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        List<EntityPidReportMain> GetPatientsInfo(string barcode);
        
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
        /// 得到dict_Rack表ID最大值
        /// </summary>
        /// <returns></returns>
        string GetMaxRack();
        /// <summary>
        /// 获取dict_rack表架子代码的最大值
        /// </summary>
        /// <returns></returns>
        string GetMaxRackCode();

        /// <summary>
        /// 获取bc_barcode表下一个最大的条码号
        /// </summary>
        /// <returns></returns>
        string GetNextMaxBarCode();

        /// <summary>
        /// 根据barcode判断是否有数据
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        bool IsRaclBarCodeExists(string barCode,bool isExists);
        /// <summary>
        /// 根据病人ID和条码号查询病人组合明细
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="patId"></param>
        /// <returns></returns>
        List<EntityPidReportDetail> GetAppendBarCode(string barCode, string patId);

        /// <summary>
        /// 根据条码号查询归档架子标本明细是否有数据
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        bool IsBarCodeUsing(string barCode);
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
        /// 获取最大的标本试管记录ID
        /// </summary>
        /// <returns></returns>
        string GetMaxSamRackID();

        /// <summary>
        /// 新增条码操作记录
        /// </summary>
        /// <param name="SampDetail"></param>
        /// <returns></returns>
        bool AddSampProcessDetail(EntitySampProcessDetail SampProcess);

        /// <summary>
        /// 删除标本流转明细信息
        /// </summary>
        /// <param name="SampDetail"></param>
        /// <returns></returns>
        bool DeleteSampProcessDetail(string barCode, string bcStatus);

        /// <summary>
        /// 更新归档架子信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool ModifySamStoreRack(EntitySampStoreRack entity);



    }
}
