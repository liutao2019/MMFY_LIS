using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 标本存储：接口类
    /// </summary>
    [ServiceContract]
    public interface ISampStock
    {
        /// <summary>
        /// 查询架子信息表数据
        /// </summary>
        /// <param name="dateTimeFrom"></param>
        /// <param name="dateTimeTo"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicSampTubeRack> GetDictRackListForSave(DateTime dateTimeFrom, DateTime dateTimeTo);

        /// <summary>
        /// 查询架子状态
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicSampStoreStatus> GetSamManageStatus();

        /// <summary>
        /// 查询冰箱数据（存储冰箱）
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicSampStore> GetIceBox();
        
        /// <summary>
        /// 查询柜子字典数据(冰箱柜子)
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicSampStoreArea> GetCups();
        
        /// <summary>
        /// 根据架子条码查询架子信息
        /// </summary>
        /// <param name="rackbarcode"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicSampTubeRack> GetDictRackInfo(string rackbarcode);

        /// <summary>
        /// 查询架子中试管的详细内容
        /// </summary>
        /// <param name="strSsid"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampStoreDetail> GetRackDetail(string strSsid);

        /// <summary>
        /// 更新归档架子标本状态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [OperationContract]
        int ModifySamDetail(EntitySampStoreDetail entity);
        
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [OperationContract]
        int AuditSamStoreRack(EntitySampStoreRack entity);
        
        /// <summary>
        /// 修改字典中试管架的状态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [OperationContract]
        int ModifyRackStatus(EntityDicSampTubeRack entity);

        /// <summary>
        /// 删除条码操作记录
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="bcStatus"></param>
        /// <returns></returns>
        [OperationContract]
        int DeleteBcSign(string barCode, string bcStatus);

        /// <summary>
        /// 取状态值
        /// </summary>
        /// <param name="strSsid"></param>
        /// <returns></returns>
        [OperationContract]
        int GetSamRackStatus(string strSsid);
    }
}
