using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoDicCombine
    {
        bool Save(EntityDicCombine sample);

        bool Update(EntityDicCombine sample);

        bool Delete(EntityDicCombine sample);

        List<EntityDicCombine> Search(string hosId);

        /// <summary>
        /// 获取无菌涂片组合
        /// </summary>
        /// <returns></returns>
        List<EntityDicCombine> GetNoBactCombine();
        /// <summary>
        /// 获取大小组合
        /// </summary>
        /// <returns></returns>
        List<EntityDicCombine> GetCombineSplit();
    }
}
