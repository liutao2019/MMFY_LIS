using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

using System.Data;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IDictMedia
    {
        [OperationContract]
        DataTable GetMicMedia();

        [OperationContract]
        DataSet GetWorksheetInfo(string strMediaId);

        [OperationContract]
        DataTable GetWorksheetItem(string strWorksheetId);

        [OperationContract]
        bool SaveMedia(DataTable dtMedia);

        [OperationContract]
        bool UpdateMedia(DataSet dsMediaInfo);

        
    }
}
