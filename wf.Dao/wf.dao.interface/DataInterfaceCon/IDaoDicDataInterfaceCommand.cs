using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    /// <summary>
    /// 接口参数界面：Dao接口(组)
    /// </summary>
    public interface IDaoDicDataInterfaceCommand
    {
        /// <summary>
        /// 保存接口参数(组)数据
        /// </summary>
        /// <param name="interCommand"></param>
        /// <returns></returns>
        bool SaveDicDataInterfaceCommand(EntityDicDataInterfaceCommand interCommand);

        /// <summary>
        /// 更新接口参数(组)数据
        /// </summary>
        /// <param name="interCommand"></param>
        /// <returns></returns>
        bool UpdateDicDataInterfaceCommand(EntityDicDataInterfaceCommand interCommand);

        /// <summary>
        /// 删除接口参数(组)数据
        /// </summary>
        /// <param name="cmdID"></param>
        /// <returns></returns>
        bool DeleteDicDataInterfaceCommand(string cmdID);

        /// <summary>
        /// 查询接口参数(组)数据
        /// </summary>
        /// <param name="interCommand"></param>
        /// <returns></returns>
        List<EntityDicDataInterfaceCommand> SearchDicDataInterfaceCommand(EntityDicDataInterfaceCommand interCommand);

    }
}
