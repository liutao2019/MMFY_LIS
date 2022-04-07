using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IOaTabelDetail
    {
        [OperationContract]
        /// <summary>
        /// 根据表单类型代码和预留字段查找表单内容
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        List<EntityOaTableDetail> GetTabDetailByTabCode(EntityOaTableDetail detail);

        /// <summary>
        /// 新增一条新的表单内容
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        [OperationContract]
        bool InsertNewTabDetail(EntityOaTableDetail detail);

        /// <summary>
        /// 更新一条表单内容
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateTabDetail(EntityOaTableDetail detail);

        /// <summary>
        /// 删除表单内容
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteTabetail(EntityOaTableDetail detail);

        /// <summary>
        /// 获取项目列
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicItmItem> GetItemList();

        /// <summary>
        /// 复制一条内容并插入数据库
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        [OperationContract]
        bool CopyOneDetailToNew(EntityOaTableDetail detail);

        /// <summary>
        /// 获取表单字段
        /// </summary>
        /// <param name="tabCode"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityOaTableField> GetOrderTableField(string tabCode);
    }
}
