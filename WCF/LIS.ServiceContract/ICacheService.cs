using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Data;
using Lib.DataInterface.Implement;
using dcl.entity;

namespace dcl.servececontract
{
    /// <summary>
    ///  模块服务契约
    /// </summary>
    [ServiceContract]
    public interface ICacheService
    {
        [OperationContract]
        List<EntityDictDataInterfaceConnection> GetAllDataInterfaceConnection();

        [OperationContract]
        DateTime GetServerDateTime();


        [OperationContract]
        List<EntityDicItemSample> GetAllDictItemSam();
    }
}
