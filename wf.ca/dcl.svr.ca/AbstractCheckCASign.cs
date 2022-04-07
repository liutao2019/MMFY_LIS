using System;
using System.Data;

namespace dcl.svr.ca
{
    public abstract class AbstractCheckCASignClass
    {
        internal DataTable dtbCASignContent = new DataTable("CASignContent");

        public AbstractCheckCASignClass()
        {
            this.dtbCASignContent.Columns.Add("pat_id");
            this.dtbCASignContent.Columns.Add("SourceContent");
            this.dtbCASignContent.Columns.Add("SignContent");
            this.dtbCASignContent.Columns.Add("SignerImage");
            this.dtbCASignContent.Columns.Add("SignType");
        }
    }
}
