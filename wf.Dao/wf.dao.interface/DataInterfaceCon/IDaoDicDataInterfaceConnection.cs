using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    /// <summary>
    /// 连接参数界面:Dao接口
    /// </summary>
    public interface IDaoDicDataInterfaceConnection
    {
        /// <summary>
        /// 保存连接参数数据
        /// </summary>
        /// <param name="dtInterCon"></param>
        /// <returns></returns>
        bool SaveDataInterfaceConnection(EntityDicDataInterfaceConnection dtInterCon);

        /// <summary>
        /// 更新连接参数数据
        /// </summary>
        /// <param name="dtInterCon"></param>
        /// <returns></returns>
        bool UpdateDataInterfaceConnection(EntityDicDataInterfaceConnection dtInterCon);

        /// <summary>
        /// 删除连接参数数据
        /// </summary>
        /// <param name="connId"></param>
        /// <returns></returns>
        bool DeleteDataInterfaceConnection(string connId);

        /// <summary>
        /// 查询接连参数数据
        /// </summary>
        /// <returns></returns>
        List<EntityDicDataInterfaceConnection> SearchDataInterfaceConnection(EntityDicDataInterfaceConnection dtInterCon);
    }
}
