using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.client.dicmic
{
    /// <summary>
    /// 为GridControl补充一些汉化内容（FindPanel汉化）
    /// </summary>
    public class Zh_HansGridLocalizer : DevExpress.XtraGrid.Localization.GridLocalizer
    {
        Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> CusLocalizedKeyValue = null;
        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="cusLocalizedKeyValue">需要转移的GridStringId，其对应的文字描述</param>
        public Zh_HansGridLocalizer(Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> cusLocalizedKeyValue)
        {
            CusLocalizedKeyValue = cusLocalizedKeyValue;
        }
        /// <summary>
        /// 重载
        /// </summary>
        /// <param name="id">GridStringId</param>
        /// <returns>string</returns>
        public override string GetLocalizedString(DevExpress.XtraGrid.Localization.GridStringId id)
        {
            if (CusLocalizedKeyValue != null)
            {
                string _gridStringDisplay = string.Empty;
                foreach (KeyValuePair<DevExpress.XtraGrid.Localization.GridStringId, string> gridLocalizer in CusLocalizedKeyValue)
                {
                    if (gridLocalizer.Key.Equals(id))
                    {
                        _gridStringDisplay = gridLocalizer.Value;
                        break;
                    }
                }
                return _gridStringDisplay;
            }
            return base.GetLocalizedString(id);
        }

    }


    public class Hans_GridHelper
    {
        public static void HansButtonText(DevExpress.XtraGrid.Views.Grid.GridView girdview, Dictionary<DevExpress.XtraGrid.Localization.GridStringId, string> cusLocalizedKeyValue)
        {
            Zh_HansGridLocalizer _bGridLocalizer = new Zh_HansGridLocalizer(cusLocalizedKeyValue);
            DevExpress.XtraGrid.Localization.GridLocalizer.Active = _bGridLocalizer;
        }
    }
}
