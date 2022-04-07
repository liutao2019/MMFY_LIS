using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 质控项目表接口
    /// </summary>
    [ServiceContract]
    public interface IDicQcMateriaDetail
    {
        /// <summary>
        /// 查询质控项目数据
        /// </summary>
        /// <param name="mat_id"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicQcMateriaDetail> SearchQcMateriaDetail(string mat_id);

        /// <summary>
        /// 保存质控项目数据
        /// </summary>
        /// <param name="QMDetail"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse SaveQcMateriaDetail(EntityDicQcMateriaDetail QMDetail);

        /// <summary>
        /// 更新质控项目数据
        /// </summary>
        /// <param name="QMDetail"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateQcMateriaDetail(EntityDicQcMateriaDetail QMDetail);

        /// <summary>
        /// 删除质控项目数据
        /// </summary>
        /// <param name="QMDetail"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteQcMateriaDetail(EntityDicQcMateriaDetail QMDetail);

        /// <summary>
        /// 获取质控项目项目ID
        /// </summary>
        /// <param name="strItrId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicQcMateriaDetail> GetQcMateriaDetailItmId(string strItrId);

        /// <summary>
        /// 更新质控项目数据
        /// </summary>
        /// <param name="listQMDetail"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateQcMateriaDetailCondition(List<EntityDicQcMateriaDetail> listQMDetail);

    }
}
