using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoOaDicShiftDetail
    {

        /// <summary>
        /// 获得当前排班信息
        /// </summary>
        /// <returns></returns>
        List<EntityOaDicShiftDetail> GetShiftPlan(DateTime sDate, DateTime eDate, string strType);

        /// <summary>
        /// 登记时插入班次计划
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        int InsertShiftPlan(EntityOaDicShiftDetail sample);


        /// <summary>
        /// 删除排班计划
        /// </summary>
        /// <returns></returns>
        bool DeleteShiftPlan(string isshow, string timeFrom, string timeTo, string typeId);


        /// <summary>
        /// 复制排班计划
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool CopyShiftPlan(string timeFrom,string timeTo);

    }
}
