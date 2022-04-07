using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoDicItmItem
    {
        bool Save(EntityDicItmItem sample);

        bool Update(EntityDicItmItem sample);

        bool Delete(EntityDicItmItem sample);

        List<EntityDicItmItem> Search(string itmId,string hosId);

        List<EntityItmRefInfo> GetItemRefInfo(List<string> itemsID, string sam_id, int age_minutes, string sex, string sam_rem, string itm_itr_id, string pat_depcode);
        /// <summary>
        /// 根据组合id获取项目信息
        /// </summary>
        /// <param name="comId"></param>
        /// <returns></returns>
        List<EntityDicItmItem> GetLisSubItemsByComId(string comId);

        /// <summary>
        /// 从Lis数据库获取项目数据(外送接口用)
        /// </summary>
        /// <returns></returns>
        List<EntityDicItmItem> GetLisSubItems();
    }
}
