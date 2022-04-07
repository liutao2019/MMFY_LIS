using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace dcl.entity
{
    /// <summary>
    /// 系统操作状态枚举类，条码部分需要跟bc_status表同步
    /// </summary>
    public class EnumReagentOperationCode
    {
        /// <summary>
        /// 试剂相关流程信息保存
        /// </summary>
        [FieldNamingAttribute(Desc = "保存资料")]
        public const int ReagentSave = 600;
        /// <summary>
        /// 试剂相关流程信息审核
        /// </summary>
        [FieldNamingAttribute(Desc = "审核资料")]
        public const int ReagentAudit = 601;
        /// <summary>
        /// 试剂相关流程信息取消审核
        /// </summary>
        [FieldNamingAttribute(Desc = "取消审核资料")]
        public const int ReagentUnAudit = 602;
        /// <summary>
        /// 删除资料明细
        /// </summary>
        [FieldNamingAttribute(Desc = "删除资料明细")]
        public const int DeleteReaDetail = 603;
        /// <summary>
        /// 撤销操作
        /// </summary>
        [FieldNamingAttribute(Desc = "撤销操作")]
        public const int DeleteReaGent = 604;
        /// <summary>
        /// 回退操作
        /// </summary>
        [FieldNamingAttribute(Desc = "回退操作")]
        public const int ReturnReaGent = 605;
        /// <summary>
        /// 打印操作
        /// </summary>
        [FieldNamingAttribute(Desc = "打印操作")]
        public const int PrintReaGent = 606;
        /// <summary>
        /// 修改操作
        /// </summary>
        [FieldNamingAttribute(Desc = "修改操作")]
        public const int ModifyReaGent = 607;

        public static string GetNameByCode(string code)
        {
            string ret = null;

            FieldInfo[] fields = typeof(EnumBarcodeOperationCode).GetFields();
            FieldInfo foundfield = null;

            foreach (FieldInfo item in fields)
            {
                object val = item.GetValue(null);

                if (val != null && val.ToString() == code)
                {
                    foundfield = item;
                    break;
                }
            }

            if (foundfield != null)
            {
                FieldNamingAttribute[] attrs = foundfield.GetCustomAttributes(typeof(FieldNamingAttribute), true) as FieldNamingAttribute[];

                if (attrs != null && attrs.Length > 0)
                {
                    ret = attrs[0].Desc;
                }
            }

            return ret;
        }

        class FieldNamingAttribute : Attribute
        {
            public string Desc { get; set; }
        }
    }
}
