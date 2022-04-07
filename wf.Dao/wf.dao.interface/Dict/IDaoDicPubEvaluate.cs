using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoDicPubEvaluate
    {
        bool Save(EntityDicPubEvaluate sample);

        bool Update(EntityDicPubEvaluate sample);

        bool Delete(EntityDicPubEvaluate sample);

        List<EntityDicPubEvaluate> Search(Object obj);
        /// <summary>
        /// 查询描述评价内容
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        List<EntityDicPubEvaluate> SearchContent();
    }
}
