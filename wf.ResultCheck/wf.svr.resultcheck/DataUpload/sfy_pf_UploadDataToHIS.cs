using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using dcl.root.logon;

namespace dcl.svr.resultcheck.DataUpload
{
    class sfy_pf_UploadDataToHIS
    {
        string pat_id;

        public sfy_pf_UploadDataToHIS(string pat_id)
        {
            this.pat_id = pat_id;
        }

        public void Upload()
        {
            Thread t = new Thread(ThredUpload);
            t.Start();
        }

        void ThredUpload()
        {
            try
            {
                new Lis.SendDataToMid.SFY.PY.DataHelper().Upload(this.pat_id);
                //fs.lis2emr.MidDataController ds = new fs.lis2emr.MidDataController();
                //ds.Insert(pat_id);
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "数据上传", ex.ToString());
            }
        }
    }
}
