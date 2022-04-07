using System.Collections.Generic;
using dcl.servececontract;
using dcl.entity;
using dcl.svr.sample;

namespace dcl.svr.resultquery
{
    public class TwoWaySelectNewBIZ : ITwoWaySelect
    {
        SampDetailBIZ samp = new SampDetailBIZ();
        public EntityResponse GetBarcodeData(string sampBarcode, string itrId)
        {
            EntityResponse respone = new EntityResponse();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            List<EntitySampDetail> listSampDetail = new List<EntitySampDetail>();
            List<EntitySampDetailMachineCode> listTwoWays = new List<EntitySampDetailMachineCode>();
            listSampDetail=samp.GetSampDetailByBarCode(sampBarcode);
            listTwoWays = samp.GetSampDetailMachineCodeByItrId(sampBarcode, itrId);
            dict.Add("SampDetail", listSampDetail);
            dict.Add("TwoWays", listTwoWays);
            respone.SetResult(dict);
            return respone;
        }

        public bool UpdateSampFlag(string commFlag, string sampBarcode)
        {
            bool result = false;
            result = samp.UpdateSampDetailCommFlag(commFlag, sampBarcode);
            return result;
        }

        public bool UpdateSampFlagByCode(string commFlag, string sampBarcode, string orderCode)
        {
            bool result = false;
            result = samp.UpdateSampDetailCommFlagByCode(commFlag, sampBarcode, orderCode);
            return result;
        }
    }
}
