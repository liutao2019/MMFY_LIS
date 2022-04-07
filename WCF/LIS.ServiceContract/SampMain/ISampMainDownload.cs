using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface ISampMainDownload
    {
        /// <summary>
        /// 条码下载
        /// </summary>
        /// <param name="downloadInfo"></param>
        [OperationContract]
        void DownloadBarcode(EntityInterfaceExtParameter downloadInfo);

        /// <summary>
        /// 外送条码下载
        /// </summary>
        /// <param name="parameter"></param>
        [OperationContract]
        void DownloadOutsideBarcode(EntityInterfaceExtParameter downloadInfo);


        /// <summary>
        /// 医嘱查询
        /// </summary>
        /// <param name="downloadInfo"></param>
        [OperationContract]
        List<EntitySampMain> DownloadOrderData(EntityInterfaceExtParameter downloadInfo);

    }
}
