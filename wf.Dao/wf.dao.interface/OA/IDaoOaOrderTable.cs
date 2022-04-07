using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoOaOrderTable
    {
        /// <summary>
        /// 保存单证类型
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        bool SaveOrderTable(EntityOaTable sample);

        /// <summary>
        /// 更新单证类型
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        bool UpdateOrderTable(EntityOaTable sample);

        /// <summary>
        /// 删除单证类型或者单证字段
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        bool DeleteOrderTable(EntityOaTable sample);
      
        /// <summary>
        /// 获得单证类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        List<EntityOaTable> GetOrderTable(Object obj);
     
    }
}
