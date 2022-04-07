using dcl.entity;
using System;


namespace dcl.client.common
{
    public class LisSysParam
    {
        private static LisSysParam instant()
        {
            return new LisSysParam();
        }

        /// <summary>
        /// 检验信息删除参数
        /// </summary>
        public static String lab_del_flag = LIS_Const.STRUCT_LAB_DEL_FLAG.DEL_ALTER;

        /// <summary>
        /// 病人资料录入:报告按钮是否显示1为显示，0为不显示
        /// </summary>
        public static String lab_report_btn_visable = "1";
    }
}
