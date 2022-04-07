using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 接口参数界面:服务接口
    /// </summary>
    [ServiceContract]
    public interface IDicDataInterfaceCommand
    {
        /// <summary>
        /// 保存接口参数(命令和参数集)数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        bool SaveDicDataInterCommandAndParm(EntityRequest request);

        /// <summary>
        /// 更新接口参数(命令和参数集)数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateDicDataInterCommandAndParm(EntityRequest request);

        /// <summary>
        /// 删除接口参数(组)数据
        /// </summary>
        /// <param name="cmdID"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteDicDataInterCommandAndParm(string cmdID);

        /// <summary>
        /// 查询接口参数(组)数据
        /// </summary>
        /// <param name="interCommand"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicDataInterfaceCommand> SearchDicDataInterfaceCommand(EntityDicDataInterfaceCommand interCommand);

        /// <summary>
        /// 根据ID获取参数数据
        /// </summary>
        /// <param name="cmdID"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicDataInterfaceCommandParameter> GetParametersByCmdID(string cmdID);

    }
}
