using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.servececontract;
using dcl.svr.sample;

namespace dcl.svr.wcf
{
    public class Barcode4InterfaceService : IBarcode4Interface
    {
        #region IBarcode4Interface 成员

        public System.Data.DataSet DownloadBarcodeFromHIS2(lis.dto.BarCodeEntity.BarcodeDownloadInfo downloadInfo, System.Data.DataSet dsHISData)
        {
            return null;
            //return new BarcodeBIZ().DownloadBarcodeFromHIS2(downloadInfo, dsHISData);
        }


        public System.Data.DataTable GetBarcodeData_WithQueryField(string ori_id, string queryField, DateTime dateBegin, DateTime dateEnd, string bc_in_no)
        {
            return null;
            //return new BarcodeBIZ().GetBarcodeData_WithQueryField(ori_id, queryField, dateBegin, dateEnd, bc_in_no);
        }


        public void UpdateBarcodePrintStatus(long bc_id, string opCode, string opName, string opPlace)
        {
            //new BarcodeBIZ().UpdateBarcodeStatusWithSign(bc_id, opCode, opName, opPlace, "1");
        }

        #endregion
    }
}
