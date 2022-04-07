using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 系统功能表
    /// 旧表名:Sys_function 新表名:Base_function
    /// </summary>
    [Serializable]
    public class EntitySysFunction : EntityBase
    {
        public EntitySysFunction()
        {
            FuncSortNo = 0;
            FuncParentId = -1;
            FuncQuickFlag = -1;
        }
        /// <summary>
        ///功能编码
        ///业务主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "funcInfoId", MedName = "func_id", WFName = "Bfunc_id")]
        public Int32 FuncId { get; set; }

        /// <summary>
        ///功能代码
        ///对应窗体的完整路径
        /// </summary>   
        [FieldMapAttribute(ClabName = "funcCode", MedName = "func_code", WFName = "Bfunc_code")]
        public String FuncCode { get; set; }

        /// <summary>
        ///功能名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "funcName", MedName = "func_name", WFName = "Bfunc_name")]
        public String FuncName { get; set; }

        /// <summary>
        ///类别
        ///窗体/功能/分隔符/自定义
        /// </summary>   
        [FieldMapAttribute(ClabName = "funcType", MedName = "func_type", WFName = "Bfunc_type")]
        public String FuncType { get; set; }

        /// <summary>
        ///父节点编号，对应funcInfoId
        /// </summary>   
        [FieldMapAttribute(ClabName = "parentId", MedName = "func_parentId", WFName = "Bfunc_parent_Bfunc_id")]
        public Int32 FuncParentId { get; set; }

        /// <summary>
        ///子功能名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "moduleName", MedName = "func_child_name", WFName = "Bfunc_child_name")]
        public String FuncChildName { get; set; }

        /// <summary>
        ///顺序号 对同一parentId下的节点有效
        /// </summary>   
        [FieldMapAttribute(ClabName = "sort", MedName = "sort_no", WFName = "sort_no")]
        public Int32 FuncSortNo { get; set; }

        /// <summary>
        ///显示在快捷工具条中的顺序号 -1为不显示
        /// </summary>   
        [FieldMapAttribute(ClabName = "quickLoad", MedName = "func_quick_flag", WFName = "Bfunc_quick_flag")]
        public Int32 FuncQuickFlag { get; set; }

        /// <summary>
        ///显示在快捷工具条中的图片资源
        /// </summary>   
        [FieldMapAttribute(ClabName = "imageSource", MedName = "func_image", WFName = "Bfunc_image")]
        public String FuncImage { get; set; }

        /// <summary>
        ///保存该模块用到的数据字典
        /// </summary>   
        [FieldMapAttribute(ClabName = "dictCache", MedName = "func_dictcache", WFName = "Bfunc_dictcache")]
        public String FuncDictcache { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "funcValidateUser", MedName = "func_valiuser", WFName = "Bfunc_valiuser")]
        public String FuncValiuser { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "funcShortcutKeys", MedName = "func_shortcut", WFName = "Bfunc_shortcut")]
        public String FuncShortcut { get; set; }

        /// <summary>
        /// 系统模块名
        /// </summary>
        [FieldMapAttribute(ClabName = "sys_module", MedName = "func_module", WFName = "Bfunc_module")]
        public String FuncModule { get; set; }
    }
}
