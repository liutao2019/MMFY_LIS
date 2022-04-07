using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 接口参数界面：Dao接口(组),服务接口
    /// </summary>
    [ServiceContract]
    public interface IDicDataInterfaceConnection
    {
        /// <summary>
        /// 保存连接参数数据
        /// </summary>
        /// <param name="dtInterCon"></param>
        /// <returns></returns>
        [OperationContract]
        bool SaveDataInterfaceConnection(EntityDicDataInterfaceConnection dtInterCon);

        /// <summary>
        /// 更新连接参数数据
        /// </summary>
        /// <param name="dtInterCon"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateDataInterfaceConnection(EntityDicDataInterfaceConnection dtInterCon);

        /// <summary>
        /// 删除连接参数数据
        /// </summary>
        /// <param name="connId"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteDataInterfaceConnection(string connId);

        /// <summary>
        /// 查询接连参数数据
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicDataInterfaceConnection> SearchDataInterfaceConnection(EntityDicDataInterfaceConnection dtInterCon);

        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="dtInterCon"></param>
        /// <returns>连接失败返回非空字符串</returns>
        [OperationContract]
        string TestConnection(EntityDicDataInterfaceConnection dtInterCon);
    }
}
