using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

using System.Data;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IDictComMedia
    {
        [OperationContract]
        DataTable GetMicCombine();

        [OperationContract]
        DataSet GetComMedia(string strComId, string strSamId);

        [OperationContract]
        DataTable GetCulture(string strComId);

        [OperationContract]
        int SaveComMedia(DataSet dsResult);

        [OperationContract]
        int DeleteComMedia(string strComId, string strSamId);
    }
}
