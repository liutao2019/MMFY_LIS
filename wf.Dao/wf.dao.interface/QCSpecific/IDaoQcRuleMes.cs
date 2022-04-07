using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{

    public interface IDaoQcRuleMes
    {
        bool Save(EntityDicQcRuleMes sample);

        bool Update(EntityDicQcRuleMes sample);

        bool Delete(EntityDicQcRuleMes sample);

        List<EntityDicQcRuleMes> Search(Object obj);
        /// <summary>
        /// 根据obr_id查询仪器质控（审核时质控时间检查使用）
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        List<EntityDicQcRuleMes> GetQcRuleMes(string pat_id);
        /// <summary>
        /// 根据仪器id查询仪器质控（审核时仪器维护检测使用）
        /// </summary>
        /// <param name="itrId"></param>
        /// <returns></returns>
        List<EntityDicQcRuleMes> GetQcRuleMesByItrId(string itrId);
    }
}
