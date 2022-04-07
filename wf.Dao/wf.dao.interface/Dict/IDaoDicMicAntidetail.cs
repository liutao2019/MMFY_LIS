using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoDicMicAntidetail
    {
        bool Save(EntityDicMicAntidetail sample);

        bool Update(EntityDicMicAntidetail sample);

        bool Delete(EntityDicMicAntidetail sample);

        List<EntityDicMicAntidetail> Search(Object obj);

        /// <summary>
        /// 根据抗生素编码更新药敏卡抗生素的删除标志
        /// </summary>
        /// <returns></returns>
        bool UpdateDelFlagByAntiCode(string  antiCode);
    }
}
