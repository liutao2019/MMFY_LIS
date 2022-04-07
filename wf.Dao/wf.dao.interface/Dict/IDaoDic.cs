using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoDic<T>
    {
        bool Save(T sample);

        bool Update(T sample);

        bool Delete(T sample);

        List<T> Search(Object obj);
    }
}
