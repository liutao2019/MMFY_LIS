using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoReportComment
    {
        bool DeleteReportComment(string rcKey);

        List<EntityReportComment> GetReportComment(string pat_id);

        int SaveReportComment(EntityReportComment model);

    }
}
