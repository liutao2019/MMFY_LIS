using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoMitmNoResultView
    {
        List<EntityDicObrResultOriginal> GetInstructmentResult2(DateTime date, string itr_id, int result_type, string filter);
        
        EntityResponse SaveOrUpdateMitmNo(List<EntityDicMachineCode> ds);
        
    }
}
