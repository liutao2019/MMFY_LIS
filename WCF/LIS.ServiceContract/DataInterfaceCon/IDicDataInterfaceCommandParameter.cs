using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 接口参数界面：参数分组服务接口
    /// </summary>
    [ServiceContract]
    public interface IDicDataInterfaceCommandParameter
    {
        /// <summary>
        /// 保存接口参数(分组)数据
        /// </summary>
        /// <param name="cmdParam"></param>
        /// <returns></returns>
        [OperationContract]
        bool SaveDicDataInterfaceCommandParameter(EntityDicDataInterfaceCommandParameter cmdParam);

        /// <summary>
        /// 更新接口参数(分组)数据
        /// </summary>
        /// <param name="cmdParam"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateDicDataInterfaceCommandParameter(EntityDicDataInterfaceCommandParameter cmdParam);

        /// <summary>
        /// 删除接口参数(分组)数据
        /// </summary>
        /// <param name="cmdParam"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteDicDataInterfaceCommandParameter(EntityDicDataInterfaceCommandParameter cmdParam);

        /// <summary>
        /// 查询接口参数(分组)数据
        /// </summary>
        /// <param name="cmdParam"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicDataInterfaceCommandParameter> SearchDicDataInterfaceCommandParameter(EntityDicDataInterfaceCommandParameter cmdParam);

    }
}
