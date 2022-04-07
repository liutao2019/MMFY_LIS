using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoDicInstrument
    {
        bool Save(EntityDicInstrument sample);

        bool Update(EntityDicInstrument sample);

        bool Delete(EntityDicInstrument sample);

        List<EntityDicInstrument> Search(Object obj);

        /// <summary>
        /// 根据组合ID获取仪器信息
        /// </summary>
        /// <param name="ComIdList"></param>
        /// <returns></returns>
        List<EntityDicInstrument> GetInstrumentByComIds(List<string> ComIdList);

        /// <summary>
        /// 获取仪器通讯类型 1单向 2双向
        /// </summary>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        Int32 GetItrHostFlag(string itr_id);

        /// <summary>
        /// 根据仪器ID或者仪器物理组别获取仪器
        /// </summary>
        /// <param name="itrId"></param>
        /// <param name="itrType"></param>
        /// <returns></returns>
        List<EntityDicInstrument> GetInstrumentByItridOrItrType(string itrId = "", string itrType = "");

        /// <summary>
        /// 历史结果关联项目专业组
        /// </summary>
        /// <param name="repId"></param>
        /// <returns></returns>
        List<EntityDicInstrument> GetHistoryReletedInstrumentByRepId(string repId);

        EntityDicInstrument GetInstrumentByItrid(string Itrid);

        /// <summary>
        /// 根据条码号获取可用的仪器列表
        /// </summary>
        /// <param name="Barcode"></param>
        /// <returns></returns>
        List<EntityDicInstrument> GetInstrumentByBarcode(string Barcode);
    }
}
