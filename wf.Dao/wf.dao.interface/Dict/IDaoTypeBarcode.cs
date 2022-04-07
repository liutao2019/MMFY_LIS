using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    /// <summary>
    /// 条码确认目的地控件数据读取接口文件
    /// </summary>
    public interface IDaoTypeBarcode
    {
        /// <summary>
        /// 查询条码确认目的地数据
        /// </summary>
        /// <param name="hosID"></param>
        /// <returns></returns>
        List<EntityTypeBarcode> SearchTypeBarcode(string hosID);
    }
}
