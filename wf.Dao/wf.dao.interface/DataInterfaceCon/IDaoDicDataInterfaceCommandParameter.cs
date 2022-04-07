using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    /// <summary>
    /// 接口参数界面：参数分组Dao接口
    /// </summary>
    public interface IDaoDicDataInterfaceCommandParameter
    {
        /// <summary>
        /// 保存接口参数(分组)数据
        /// </summary>
        /// <param name="cmdParam"></param>
        /// <returns></returns>
        bool SaveDicDataInterfaceCommandParameter(EntityDicDataInterfaceCommandParameter cmdParam);

        /// <summary>
        /// 更新接口参数(分组)数据
        /// </summary>
        /// <param name="cmdParam"></param>
        /// <returns></returns>
        bool UpdateDicDataInterfaceCommandParameter(EntityDicDataInterfaceCommandParameter cmdParam);

        /// <summary>
        /// 删除接口参数(分组)数据
        /// </summary>
        /// <param name="cmdParam"></param>
        /// <returns></returns>
        bool DeleteDicDataInterfaceCommandParameter(EntityDicDataInterfaceCommandParameter cmdParam);

        /// <summary>
        /// 查询接口参数(分组)数据
        /// </summary>
        /// <param name="cmdParam"></param>
        /// <returns></returns>
        List<EntityDicDataInterfaceCommandParameter> SearchDicDataInterfaceCommandParameter(EntityDicDataInterfaceCommandParameter cmdParam);

    }
}
