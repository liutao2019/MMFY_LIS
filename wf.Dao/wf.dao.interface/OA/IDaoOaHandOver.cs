using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoOaHandOver
    {
        /// <summary>
        /// 获取交班设定
        /// </summary>
        /// <returns></returns>
        List<EntityDicHandOver> GetDictHandoverList();

        /// <summary>
        /// 更新交班设定
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool UpdateDictHandoverList(List<EntityDicHandOver> list);

        /// <summary>
        /// 删除交班设定
        /// </summary>
        /// <param name="typeid"></param>
        /// <returns></returns>
        bool DeleteDictHandover(string typeid);
    }
}
