using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    /// <summary>
    /// 质控物明细表:接口
    /// </summary>
    public interface IDaoQcMateria
    {
        /// <summary>
        /// 根据仪器ID查询质控物明细数据
        /// </summary>
        /// <param name="matId"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        List<EntityDicQcMateria> SearchQcMateriaByMatId(string matId, string startDate);

        /// <summary>
        /// 查询质控物数据(所有)
        /// </summary>
        /// <returns></returns>
        List<EntityDicQcMateria> SearchQcMateriaAll();

        /// <summary>
        /// 查询该仪器时间范围内存在的质控物主键
        /// </summary>
        /// <param name="qcMateria"></param>
        /// <returns></returns>
        List<EntityDicQcMateria> SearchMatSnInQcMateria(EntityDicQcMateria qcMateria);

        /// <summary>
        /// 查询该仪器时间范围历史质控批号
        /// </summary>
        /// <param name="matId"></param>
        /// <param name="matLevel"></param>
        /// <returns></returns>
        List<EntityDicQcMateria> SearchQcMateriaLeftRuleTimeAndInterface(string matId, string matLevel);

        /// <summary>
        /// 保存质控物明细数据
        /// </summary>
        /// <param name="qcMateria"></param>
        /// <returns></returns>
        EntityResponse SaveQcMateria(EntityDicQcMateria qcMateria);

        /// <summary>
        /// 更新质控物明细数据
        /// </summary>
        /// <param name="qcMateria"></param>
        /// <returns></returns>
        bool UpdateQcMateria(EntityDicQcMateria qcMateria);

        /// <summary>
        /// 删除质控物明细数据
        /// </summary>
        /// <param name="qcMateria"></param>
        /// <returns></returns>
        bool DeleteQcMateria(EntityDicQcMateria qcMateria);

        /// <summary>
        /// 根据条件获取质控物明细表主键
        /// </summary>
        /// <param name="QcItrId"></param>
        /// <param name="QcNoType"></param>
        /// <param name="QcItmId"></param>
        /// <returns></returns>
        List<EntityDicQcMateria> GetMatSn(string QcItrId, string QcNoType, string QcItmId);


        List<EntityDicQcMateria> GetMatSn(string QcItrId,DateTime dtStartTime, DateTime dtEndTime, string QcItmId);
    }
}
