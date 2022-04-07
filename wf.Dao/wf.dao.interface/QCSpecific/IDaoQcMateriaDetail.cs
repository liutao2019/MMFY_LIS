using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    /// <summary>
    /// 质控项目表：接口文件
    /// </summary>
    public interface IDaoQcMateriaDetail
    {
        /// <summary>
        /// 查询质控项目数据
        /// </summary>
        /// <param name="mat_id"></param>
        /// <returns></returns>
        List<EntityDicQcMateriaDetail> SearchQcMateriaDetail(string mat_id);

        /// <summary>
        /// 保存质控项目数据
        /// </summary>
        /// <param name="QMDetail"></param>
        /// <returns></returns>
        EntityResponse SaveQcMateriaDetail(EntityDicQcMateriaDetail QMDetail);

        /// <summary>
        /// 更新质控项目数据
        /// </summary>
        /// <param name="QMDetail"></param>
        /// <returns></returns>
        bool UpdateQcMateriaDetail(EntityDicQcMateriaDetail QMDetail);

        /// <summary>
        /// 删除质控项目数据
        /// </summary>
        /// <param name="QMDetail"></param>
        /// <returns></returns>
        bool DeleteQcMateriaDetail(EntityDicQcMateriaDetail QMDetail);

        /// <summary>
        /// 获取质控项目Id
        /// </summary>
        /// <param name="strItrId"></param>
        /// <returns></returns>
        List<EntityDicQcMateriaDetail> GetQcMateriaDetailItmId(string strItrId);

        /// <summary>
        /// 根据条件更新质控项目数据
        /// </summary>
        /// <param name="QMDetail"></param>
        /// <returns></returns>
        bool UpdateQcMateriaDetailCondition(EntityDicQcMateriaDetail QMDetail);
    }
}
