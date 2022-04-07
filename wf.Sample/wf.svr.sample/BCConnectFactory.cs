using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lis.dto.BarCodeEntity;

namespace dcl.svr.sample
{
    public class BCConnectFactory
    {
        internal static IBCConnect Create(BarcodeDownloadInfo downloadInfo)
        {
            IBCConnect cnn = new NormalConnecter();

            if (downloadInfo.FetchDataType == FetchDataType.OutLink)
                cnn = new OutlinkConnecter();
            cnn.DownLoadInfo = downloadInfo;
            return cnn;
        }
    }
}
