using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 事务维护操作接口
    /// </summary>
    [ServiceContract]
    public interface IOrderTable
    {
        /// <summary>
        /// 新增单证类型
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse NewOrderTable(EntityRequest request);

        /// <summary>
        /// 新增单证字段
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse NewOrderTableField(EntityRequest request);

        /// <summary>
        /// 删除单证类型以及单证字段
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse DeleteOrder(EntityRequest request);

        /// <summary>
        /// 更新单证类型
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse UpdateOrderTable(EntityRequest request);

        /// <summary>
        /// 更新单证字段
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse UpdateOrderTableField(EntityRequest request);

        /// <summary>
        /// 获得单证类型
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse GetOrderTable(EntityRequest request);

        /// <summary>
        /// 获得单证字段
        /// </summary>
        /// <param name="id">表单类型代码</param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse GetOrderTableField(string id);


        /// <summary>
        /// 上下移动单证字段的index
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse UpdateOrderTableFieldIndex(EntityRequest request);

    }
}
