using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface ISpecialDictOperDao
    {
        List<EntityDicMicTemplate> GetDicMicTemplate(string tempType, string patKey);

    }
}
