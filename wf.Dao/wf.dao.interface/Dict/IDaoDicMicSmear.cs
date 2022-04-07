using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoDicMicSmear
    {
        bool Save(EntityDicMicSmear sample);

        bool Update(EntityDicMicSmear sample);

        bool Delete(EntityDicMicSmear sample);

        List<EntityDicMicSmear> Search(Object obj);

        /// <summary>
        /// 获取细菌无菌涂片信息
        /// </summary>
        /// <returns></returns>
        List<EntityDicMicSmear> SearchMicSmear();
    }
}
