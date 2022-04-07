using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoOaOrderTableField
    {
        /// <summary>
        /// 保存单证字段
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool SaveOrderTableFiled(EntityOaTableField sample);
        /// <summary>
        /// 更新单证字段
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool UpdateOrderTableFiled(EntityOaTableField sample);
        /// <summary>
        /// 删除单证字段
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool DeleteOrderTableFiled(EntityOaTableField sample);

        /// <summary>
        /// 获得单证字段
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        List<EntityOaTableField> GetOrderTableFiled(string obj);

        /// <summary>
        /// 单证字段上下移动
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool UpdateOrderTableFiledIndex(EntityOaTableField sample);

        /// <summary>
        /// 删除单证字段根据单证类型删除
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool DeleteOrderTableFiledByTypeCode(string  typeCode);

    }
}
