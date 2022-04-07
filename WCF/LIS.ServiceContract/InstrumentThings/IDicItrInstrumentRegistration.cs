using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IDicItrInstrumentRegistration
    {
        /// <summary>
        /// 查询保养登记信息
        /// </summary>
        /// <param name="strItrId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicInstrmtMaintainRegistration> GetRegistration(string strItrId);

        /// <summary>
        /// 查询保养记录信息
        /// </summary>
        /// <param name="strmai_id"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicInstrmtMaintainRegistration> GetRegistrationByDate(string strmai_id);

        /// <summary>
        /// 插入保养登记信息数据
        /// </summary>
        /// <param name="listRegis"></param>
        /// <returns></returns>
        [OperationContract]
        int MaintainRegistration(List<EntityDicInstrmtMaintainRegistration> listRegis);

        /// <summary>
        /// 查询仪器保养维修信息
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicInstrmtMaintainRegistration> GetMaintainData(EntityDicInstrmtMaintainRegistration registration);
        
    }
}
