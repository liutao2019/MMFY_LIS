using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 质控物明细表:接口
    /// </summary>
    [ServiceContract]
    public interface IDicQcMateria
    {
        /// <summary>
        /// 根据仪器ID查询质控物明细数据
        /// </summary>
        /// <param name="matId"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicQcMateria> SearchQcMateriaByMatId(string matId, string startDate);
        
        /// <summary>
        /// 查询该仪器时间范围内存在的质控物主键
        /// </summary>
        /// <param name="qcMateria"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicQcMateria> SearchMatSnInQcMateria(EntityDicQcMateria qcMateria);

        /// <summary>
        /// 查询该仪器时间范围历史质控批号
        /// </summary>
        /// <param name="matId"></param>
        /// <param name="matLevel"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicQcMateria> SearchQcMateriaLeftRuleTimeAndInterface(string matId, string matLevel);

        /// <summary>
        /// 保存质控物明细数据
        /// </summary>
        /// <param name="qcMateria"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse SaveQcMateria(EntityDicQcMateria qcMateria);

        /// <summary>
        /// 更新质控物明细数据
        /// </summary>
        /// <param name="qcMateria"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateQcMateria(EntityDicQcMateria qcMateria);

        /// <summary>
        /// 删除质控物明细数据
        /// </summary>
        /// <param name="qcMateria"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteQcMateria(EntityDicQcMateria qcMateria);

        /// <summary>
        /// 查询质控物数据(所有)
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicQcMateria> SearchQcMateriaAll();

        /// <summary>
        /// 质控物复制方法
        /// </summary>
        /// <param name="eyQcMateria"></param>
        /// <returns></returns>
        [OperationContract]
        bool CopyMethodQcMateria(EntityDicQcMateria eyQcMateria);

        /// <summary>
        /// 测试数据模块使用查询质控批号信息
        /// </summary>
        /// <param name="QcItrId"></param>
        /// <param name="dtTime"></param>
        /// <param name="QcItmId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicQcMateria> GetMatSnByItem(string QcItrId,DateTime dtStartTime, DateTime dtEndTime, string QcItmId);
    }
}
