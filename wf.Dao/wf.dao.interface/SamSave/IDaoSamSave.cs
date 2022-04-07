using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoSamSave
    {
        /// <summary>
        /// 查询架子信息表数据
        /// </summary>
        /// <param name="dateTimeFrom"></param>
        /// <param name="dateTimeTo"></param>
        /// <returns></returns>
        List<EntityDicSampTubeRack> GetDictRackListForSave(DateTime dateTimeFrom, DateTime dateTimeTo);

        /// <summary>
        /// 查询架子状态
        /// </summary>
        /// <returns></returns>
        List<EntityDicSampStoreStatus> GetSamManageStatus();

        /// <summary>
        /// 查询冰箱数据（存储冰箱）
        /// </summary>
        /// <returns></returns>
        List<EntityDicSampStore> GetIceBox();

        /// <summary>
        /// 查询柜子字典数据(冰箱柜子)
        /// </summary>
        /// <returns></returns>
        List<EntityDicSampStoreArea> GetCups();

        /// <summary>
        /// 根据架子条码查询架子信息
        /// </summary>
        /// <param name="rackbarcode"></param>
        /// <returns></returns>
        List<EntityDicSampTubeRack> GetDictRackInfo(string rackbarcode);

        /// <summary>
        /// 查询架子中试管的详细内容
        /// </summary>
        /// <param name="strSsid"></param>
        /// <returns></returns>
        List<EntitySampStoreDetail> GetRackDetail(string strSsid);

        /// <summary>
        /// 更新归档架子标本状态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int ModifySamDetail(EntitySampStoreDetail entity);
        
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int AuditSamStoreRack(EntitySampStoreRack entity);

        /// <summary>
        /// 修改字典中试管架的状态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int ModifyRackStatus(EntityDicSampTubeRack entity);

        /// <summary>
        /// 删除条码操作记录
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="bcStatus"></param>
        /// <returns></returns>
        int DeleteBcSign(string barCode, string bcStatus);

        /// <summary>
        /// 取状态值
        /// </summary>
        /// <param name="strSsid"></param>
        /// <returns></returns>
        int GetSamRackStatus(string strSsid);
    }
}
