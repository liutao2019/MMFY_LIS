using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoOaDicShift
    {


        /// <summary>
        ///  获取当前最大的ID值
        /// </summary>
        /// <returns></returns>
        string GetMaxDutyID();
        /// <summary>
        /// 获得当前存在的班次信息
        /// </summary>
        /// <returns></returns>
        List<EntityOaDicShift>  GetDutyData();

        /// <summary>
        /// 修改操作
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        int ModifyDutyRecord(EntityOaDicShift sample);

        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        int InsertIntoDuty(EntityOaDicShift sample);

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        int DeleteDutyRecord(EntityOaDicShift sample);


    }
}
