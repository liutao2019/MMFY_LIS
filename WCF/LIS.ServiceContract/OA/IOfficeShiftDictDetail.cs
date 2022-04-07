using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 排班计划操作接口
    /// </summary>
    [ServiceContract]
    public interface IOfficeShiftDictDetail
    {

        /// <summary>
        /// 获得排班计划表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityOaDicShiftDetail> GetShiftPlan(DateTime sDate, DateTime eDate, string strType);

        /// <summary>
        /// 登记时插入排班计划
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [OperationContract]
        int InsertShiftPlan(EntityOaDicShiftDetail entity);

        ///// <summary>
        ///// 删除排班计划
        ///// </summary>
        ///// <returns></returns>
        //bool DeleteShiftPlan(string isshow, string timeFrom, string timeTo, string typeId);

        /// <summary>
        /// 更新排班计划
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateShiftPlan(EntityRequest request);

        /// <summary>
        /// 复制排班计划
        /// </summary>
        /// <param name="sFrom"></param>
        /// <param name="sTo"></param>
        /// <param name="timeFrom"></param>
        /// <param name="timeTo"></param>
        /// <returns></returns>
        [OperationContract]
        bool CopyShiftPlan(string sFrom ,string sTo,string timeFrom,string timeTo);

    }
}
