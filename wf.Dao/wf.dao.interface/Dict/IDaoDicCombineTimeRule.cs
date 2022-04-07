using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoDicCombineTimeRule
    {
        bool Save(EntityDicCombineTimeRule sample);

        bool Update(EntityDicCombineTimeRule sample);

        bool Delete(EntityDicCombineTimeRule sample);

        List<EntityDicCombineTimeRule> Search(Object obj);


        List<EntityDicCombineTimeRule> GetTATComTimeByRepId(List<string> listRepId);

    }
}
