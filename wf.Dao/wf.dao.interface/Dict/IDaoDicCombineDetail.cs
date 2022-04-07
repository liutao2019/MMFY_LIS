using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoDicCombineDetail
    {
        bool Save(EntityDicCombineDetail sample);

        bool Update(EntityDicCombineDetail sample);

        bool Delete(EntityDicCombineDetail sample);

        List<EntityDicCombineDetail> Search(Object obj);

        /// <summary>
        /// 根据 仪器ID和组合ID 获取具有缺省值的项目
        /// </summary>
        /// <param name="itrId">仪器Id</param>
        /// <param name="comId">组合ID</param>
        /// <returns></returns>
        List<EntityDicCombineDetail> GetCombineDefData(string itrId, string comId);


        /// <summary>
        /// 根据 组合ID和标本和仪器Id 获取具有缺省值的项目
        /// </summary>
        /// <param name="comId">组合ID</param>
        /// <param name="patSamId">标本ID</param>
        /// <param name="itrId">仪器Id</param>
        /// <returns></returns>
        List<EntityDicCombineDetail> GetCombineMiWdthDefault( string comId,string patSamId,string itrId);

        /// <summary>
        /// 根据报告ID获取项目名称
        /// </summary>
        /// <param name="RepId"></param>
        /// <param name="addSql"></param>
        /// <returns></returns>
        List<EntityDicCombineDetail> GetItmNameByRepId(string RepId, bool addSql);


        /// <summary>
        ///  获取病人组合项目
        /// </summary>
        /// <param name="repId"></param>
        /// <returns></returns>
        List<EntityDicCombineDetail> GetDictCombineItem(string repId);

        /// <summary>
        /// 根据组合ID集合查询组合明细
        /// </summary>
        /// <param name="listComIds"></param>
        /// <returns></returns>
        List<EntityDicCombineDetail> GetComDetailByComIds(List<string> listComIds);
    }
}
