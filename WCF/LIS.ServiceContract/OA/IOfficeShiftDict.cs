using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 人员管理操作接口
    /// </summary>
    [ServiceContract]
    public interface IOfficeShiftDict
    {

        /// <summary>
        /// 获得当前存在的班次信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityOaDicShift> GetDutyData();

        /// <summary>
        /// 获得当前最大的ID
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        string GetMaxDutyID();

        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        int InsertIntoDuty(EntityOaDicShift sample);

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        int ModifyDutyRecord(EntityOaDicShift sample);

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        int DeleteDutyRecord(EntityOaDicShift sample);

        /// <summary>
        /// 获得科室
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicPubDept> GetDepartment();

        /// <summary>
        /// 获得物理组
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicPubProfession> GetPhyic();

        /// <summary>
        /// 获得排班计划表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityOaDicShiftDetail> GetDutyPlan(DateTime sDate, DateTime eDate, string strType);

        /// <summary>
        /// 登记时插入排班计划
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [OperationContract]
        int InsertDutyPlan(EntityOaDicShiftDetail entity);

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySysUser> GetUser(string p);


        /// <summary>
        /// 获得排班模板
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityOaShiftTemplate> GetTemplateData();


    }
}
