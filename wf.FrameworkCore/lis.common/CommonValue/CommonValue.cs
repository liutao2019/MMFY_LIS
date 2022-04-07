using dcl.entity;
using System;
using System.Collections.Generic;
using System.Data;

namespace dcl.common
{
    public class CommonValue
    {
        #region StaticString


        public readonly static string DateTimeFormat = "yyyy-MM-dd HH:mm";
        public readonly static string DateFormat = "yyyy-MM-dd";
        public readonly static string DateTimeLongFormat = "yyyy-MM-dd HH:mm:ss";
        public readonly static string OutlinkDateTimeFormat = "yyyy/MM/dd HH:mm";
        public readonly static string OutlinkDateTimeLongFormat = "yyyy/MM/dd HH:mm:ss";

        #endregion

        #region enum

        public enum RuleType
        {
            None,
            Age,
            Sex,
            性别_1男2女,
            性别_0男1女,
            性别_男女,
            年龄_出生日期_转YMDHI,
            年龄_出生日期_转YMDHI_不满6岁显月份,
            年龄_中文年月日出生日期_转YMDHI_不满4岁显月份,
            年龄_岁_转YMDHI,
            年龄_Y岁或M月_转YMDHI,
            年龄_简单YMDHI转YMDHI_单位前后缀_取整,
            年龄_岁年月个月天日时小时分分钟转YMDHI_单位前后缀_取整,
            年龄_中文转YMDHI,
            年龄_出生日期_or_中文_转YMDHI,
            条码_性别_1男2女,
            布尔_1true_0false,
            布尔_是true_否false
        }

        #endregion

        #region ValueDisplayObject


        /// <summary>
        /// 绑定过滤ComboBox(微生物管理主界面)
        /// </summary>
        public static List<ValueDisplayObject> FliterList
        {
            get
            {
                List<ValueDisplayObject> fliterList = new List<ValueDisplayObject>();
                fliterList.Add(new ValueDisplayObject("0", "全部"));
                fliterList.Add(new ValueDisplayObject("rep_bar_code", "条码号"));
                fliterList.Add(new ValueDisplayObject("pid_in_no", "病案号"));
                fliterList.Add(new ValueDisplayObject("rep_sid", "样本号"));
                fliterList.Add(new ValueDisplayObject("pid_name", "姓名"));
                fliterList.Add(new ValueDisplayObject("ino_media_barcode", "预置码"));
                fliterList.Add(new ValueDisplayObject("rep_sno", "实验序号"));
                fliterList.Add(new ValueDisplayObject("rep_sno_c", "C实验序号"));
                fliterList.Add(new ValueDisplayObject("rep_sno_p", "培养序号"));
                return fliterList;
            }
        }

        #endregion

        #region DataTable

        /// <summary>
        /// 删除标记
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDelFlag()
        {
            DataTable result = new DataTable();
            result = CreateDT(new String[] { "id", "value" }, "delflag");
            result.Rows.Add(new Object[] { "0", "启用" });
            result.Rows.Add(new Object[] { "1", "停用" });
            return result;
        }

        /// <summary>
        /// sql条件符号
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTiaojian()
        {
            DataTable result = new DataTable();
            result = CreateDT(new String[] { "value" }, "tiaojian");
            result.Rows.Add(new Object[] { ">" });
            result.Rows.Add(new Object[] { ">=" });
            result.Rows.Add(new Object[] { "<" });
            result.Rows.Add(new Object[] { "<=" });
            result.Rows.Add(new Object[] { "=" });
            result.Rows.Add(new Object[] { "<>" });
            result.Rows.Add(new Object[] { "like" });
            result.Rows.Add(new Object[] { "not like" });
            return result;
        }

        /// <summary>
        /// 报告类型
        /// </summary>
        /// <returns></returns>
        public static DataTable GetPatCtype()
        {
            DataTable result = new DataTable();
            result = CreateDT(new String[] { "id", "value", "py", "wb" }, "pat_ctype");
            result.Rows.Add(new Object[] { "-1", "全部" });
            result.Rows.Add(new Object[] { "1", "常规", "CG", "" });
            result.Rows.Add(new Object[] { "2", "急查", "JC", "" });
            result.Rows.Add(new Object[] { "3", "保密", "BM", "" });
            result.Rows.Add(new Object[] { "4", "溶栓", "NZZ", "" });
            return result;
        }

        /// <summary>
        /// 报告状态
        /// 全部、未审核、未报告、未打印、已打印
        /// </summary>
        /// <returns></returns>
        public static DataTable GetPatFlag()
        {
            DataTable result = new DataTable();
            result = CreateDT(new String[] { "id", "value" }, "pat_flag");
            result.Rows.Add(new Object[] { "-1", "全部" });
            result.Rows.Add(new Object[] { "2", "未审核" });
            result.Rows.Add(new Object[] { "1", "未报告" });
            result.Rows.Add(new Object[] { "3", "未打印" });
            result.Rows.Add(new Object[] { "4", "已打印" });
            return result;
        }

        /// <summary>
        /// 性别
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSex()
        {
            DataTable result = new DataTable();
            result = CreateDT(new String[] { "id", "value" }, "sex");
            result.Rows.Add(new Object[] { "0", "" });
            result.Rows.Add(new Object[] { "1", "男" });
            result.Rows.Add(new Object[] { "2", "女" });
            return result;
        }

        /// <summary>
        /// 项目类型
        /// </summary>
        /// <returns></returns>
        public static DataTable GetItmFlag()
        {
            DataTable result = new DataTable();
            result = CreateDT(new String[] { "id", "value" }, "itm_flag");
            result.Rows.Add(new Object[] { "0", "常规" });
            result.Rows.Add(new Object[] { "1", "分期" });
            return result;
        }

        public static DataTable GetEfficacyPatients()
        {
            DataTable result = new DataTable();
            result = CreateDT(new String[] { "column_id", "column_name" }, "Patients_Column");
            result.Rows.Add("pat_name", "姓名");
            result.Rows.Add("pat_sex", "性别");
            result.Rows.Add("pat_age", "年龄");
            return result;
        }

        /// <summary>
        /// 标本结果类型
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSampleResultType()
        {
            DataTable result = new DataTable();
            result = CreateDT(new String[] { "id", "value" }, "samdtype");
            result.Rows.Add(new Object[] { "", "" });
            result.Rows.Add(new Object[] { "数值", "数值" });
            result.Rows.Add(new Object[] { "字符", "字符" });
            result.Rows.Add(new Object[] { "二值", "二值" });
            result.Rows.Add(new Object[] { "尿液", "尿液" });
            result.Rows.Add(new Object[] { "中文", "中文" });
            return result;
        }

        /// <summary>
        /// 报告类型
        /// </summary>
        /// <returns></returns>
        public static DataTable GetRepFlag()
        {
            DataTable result = new DataTable();
            result = CreateDT(new String[] { "id", "value" }, "Rep_flag");
            result.Rows.Add(new Object[] { "1", "普通" });
            result.Rows.Add(new Object[] { "2", "酶标" });
            result.Rows.Add(new Object[] { "3", "细菌" });
            result.Rows.Add(new Object[] { "4", "描述" });
            result.Rows.Add(new Object[] { "5", "过敏原" });
            result.Rows.Add(new Object[] { "6", "新生儿筛查" });
            result.Rows.Add(new Object[] { "7", "骨髓" });
            return result;
        }

        /// <summary>
        /// 仪器通信类型
        /// </summary>
        /// <returns></returns>
        public static DataTable GetHostFlag()
        {
            DataTable result = new DataTable();
            result = CreateDT(new String[] { "id", "value" }, "Host_flag");
            result.Rows.Add(new Object[] { "1", "单向" });
            result.Rows.Add(new Object[] { "2", "双向" });
            return result;
        }

        /// <summary>
        /// 校验类型
        /// </summary>
        /// <returns></returns>
        public static DataTable GetClItemType()
        {
            DataTable result = new DataTable();
            result = CreateDT(new String[] { "id", "value" }, "ClItem_Type");
            result.Rows.Add(new Object[] { "1", "计算" });
            result.Rows.Add(new Object[] { "2", "校验" });
            result.Rows.Add(new Object[] { "3", "酶标" });
            return result;
        }

        /// <summary>
        /// 计算类型
        /// </summary>
        /// <returns></returns>
        public static DataTable GetSpecClFormula()
        {
            DataTable result = new DataTable();
            result = CreateDT(new String[] { "id", "value", "remark" }, "Spec_cl_formula");
            result.Rows.Add(new Object[] { "EPI-GRE(Scr)", "EPI-GRE(Scr)", "只计算Scr结果,公式:144x(Scr/0.7)-0.329x(0.993)age" });
            result.Rows.Add(new Object[] { "EPI-GRE(SCysC)", "EPI-GRE(SCysC)", "只计算SCysC结果,公式:133x(SCysC/0.8)-0.499X(0.996)age" });
            result.Rows.Add(new Object[] { "EPI-GRE(both)", "EPI-GRE(both)", "公式:130x(Scr/0.7)-0.329x(SCysC/0.8)-0.499X(0.995)age" });
            result.Rows.Add(new Object[] { "eGFR", "eGFR", "186x(SCR/88.4)-1.154x(年龄)-0.203" });
            result.Rows.Add(new Object[] { "eGFR-EPI", "eGFR-EPI", "" });
            result.Rows.Add(new Object[] { "B102", "B102", "" });
            return result;
        }


        #endregion

        #region Helper

        internal static DataTable CreateDT(string[] cols, string tableName)
        {
            DataTable table = new DataTable(tableName);
            foreach (string str in cols)
            {
                table.Columns.Add(str);
            }
            return table;
        }

        #endregion
    }
}