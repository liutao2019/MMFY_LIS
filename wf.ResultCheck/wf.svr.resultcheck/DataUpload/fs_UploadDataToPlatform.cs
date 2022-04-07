using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using dcl.root.logon;

namespace dcl.svr.resultcheck.Updater
{
    class fs_UploadDataToPlatform
    {
        string pat_id;

        public fs_UploadDataToPlatform(string pat_id)
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
                fs.lis2emr.MidDataController ds = new fs.lis2emr.MidDataController();
                ds.Insert(pat_id);
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "数据上传", ex.ToString());
            }
        }
    }
}
