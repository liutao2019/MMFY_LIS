using dcl.entity;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IDCLInterfacesTool
    {
        /// <summary>
        /// 上传数据
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse UploadDCLReport(int Number);

        /// <summary>
        /// 重新上传数据
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse ReUploadDCLReport(List<string> RepId);

        [OperationContract]
        EntityResponse ReChargeBarcode(List<EntitySampMain> Barids);
    }
}
