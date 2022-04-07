using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoOaDicOaShiftTemplate
    {

        /// <summary>
        ///  获取当前最大的ID值
        /// </summary>
        /// <returns></returns>
        //string GetMaxTemplateID();
        /// <summary>
        /// 获得当前存在的模板信息
        /// </summary>
        /// <returns></returns>
        List<EntityOaShiftTemplate> GetTemplateData();

        /// <summary>
        /// 修改模板
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        //int ModifyTemplateRecord(EntityOaShiftTemplate sample);

        /// <summary>
        /// 插入一条模板记录
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        //int InsertIntoTemplate(EntityOaShiftTemplate sample);




    }
}
