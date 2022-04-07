using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using dcl.entity;
using dcl.client.common;

namespace dcl.client.frame.runsetting
{
    /// <summary>
    /// 病人资料录入运行时面板配置面板
    /// 分3部分
    /// 1.左侧病人列表配置
    /// 2.中间病人详细信息配置
    /// 3.右侧病人结果配置
    /// </summary>
    [Serializable]
    public class PatInputRuntimeSetting : RuntimeSettingBase
    {
        #region 保存
        /// <summary>
        /// 保存为系统设置
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="setting"></param>
        public static void SaveSystem(string formName, PatInputRuntimeSetting setting)
        {
            formName = formName.Replace("New", "");
            RuntimeSetting.NewInstance.SaveGroup<PatInputRuntimeSetting>(formName + "_SystemDefault", setting);
        }


        /// <summary>
        /// 保存用户配置
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="userName"></param>
        /// <param name="setting"></param>
        public static void SaveUser(string formName, string userName, PatInputRuntimeSetting setting)
        {
            formName = formName.Replace("New", "");
            RuntimeSetting.NewInstance.SaveUser<PatInputRuntimeSetting>(formName + "_" + userName, setting);
        }
        #endregion

        #region 加载
        /// <summary>
        /// 加载系统默认
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        public static PatInputRuntimeSetting LoadSystem(string formName)
        {
            formName = formName.Replace("New", "");

            PatInputRuntimeSetting setting = RuntimeSetting.NewInstance.LoadGroup<PatInputRuntimeSetting>(formName + "_SystemDefault");
            if (setting != null && setting.PatListPanel != null)
            {
                foreach (DataRow dr in setting.PatListPanel.GridColSetting.Rows)
                {
                    #region 配置转换
                    if (dr["FieldName"].ToString() == "pat_host_order_int")
                    {
                        dr["FieldName"] = "RepSerialNum";
                        dr["DataFieldName"] = "RepSerialNum";
                    }
                    if (dr["FieldName"].ToString() == "pat_name")
                    {
                        dr["FieldName"] = "PidName";
                        dr["DataFieldName"] = "PidName";
                    }
                    if (dr["FieldName"].ToString() == "pat_c_name")
                    {
                        dr["FieldName"] = "PidComName";
                        dr["DataFieldName"] = "PidComName";
                    }
                    if (dr["FieldName"].ToString() == "pat_sex_name")
                    {
                        dr["FieldName"] = "PidSexName";
                        dr["DataFieldName"] = "PidSexName";
                    }
                    if (dr["FieldName"].ToString() == "pat_age_exp")
                    {
                        dr["FieldName"] = "PidAgeExp";
                        dr["DataFieldName"] = "PidAgeExp";
                    }
                    if (dr["FieldName"].ToString() == "pat_flag_name")
                    {
                        dr["FieldName"] = "RepStatusName";
                        dr["DataFieldName"] = "RepStatusName";
                    }
                    if (dr["FieldName"].ToString() == "pat_in_no")
                    {
                        dr["FieldName"] = "PidInNo";
                        dr["DataFieldName"] = "PidInNo";
                    }
                    if (dr["FieldName"].ToString() == "pat_sam_name")
                    {
                        dr["FieldName"] = "SamName";
                        dr["DataFieldName"] = "SamName";
                    }
                    if (dr["FieldName"].ToString() == "pat_bed_no")
                    {
                        dr["FieldName"] = "PidBedNo";
                        dr["DataFieldName"] = "PidBedNo";
                    }
                    if (dr["FieldName"].ToString() == "pat_dep_name")
                    {
                        dr["FieldName"] = "PidDeptName";
                        dr["DataFieldName"] = "PidDeptName";
                    }
                    if (dr["FieldName"].ToString() == "pat_i_name")
                    {
                        dr["FieldName"] = "LrName";
                        dr["DataFieldName"] = "LrName";
                    }
                    if (dr["FieldName"].ToString() == "pat_check_name")
                    {
                        dr["FieldName"] = "PidChkName";
                        dr["DataFieldName"] = "PidChkName";
                    }
                    if (dr["FieldName"].ToString() == "pat_report_name")
                    {
                        dr["FieldName"] = "BgName";
                        dr["DataFieldName"] = "BgName";
                    }
                    if (dr["FieldName"].ToString() == "pat_look_name")
                    {
                        dr["FieldName"] = "PatLookName";
                        dr["DataFieldName"] = "PatLookName";
                    }
                    if (dr["FieldName"].ToString() == "pat_look_date")
                    {
                        dr["FieldName"] = "RepReadDate";
                        dr["DataFieldName"] = "RepReadDate";
                    }
                    if (dr["FieldName"].ToString() == "pat_apply_date")
                    {
                        dr["FieldName"] = "SampApplyDate";
                        dr["DataFieldName"] = "SampApplyDate";
                    }
                    if (dr["FieldName"].ToString() == "itr_mid")
                    {
                        dr["FieldName"] = "ItrEname";
                        dr["DataFieldName"] = "ItrEname";
                    }
                    if (dr["FieldName"].ToString() == "hasresult")
                    {
                        dr["FieldName"] = "HasResult";
                        dr["DataFieldName"] = "HasResult";
                    }
                    if (dr["FieldName"].ToString() == "modify_flag")
                    {
                        dr["FieldName"] = "ModifyFlag";
                        dr["DataFieldName"] = "ModifyFlag";
                    }
                    if (dr["FieldName"].ToString() == "pat_diag")
                    {
                        dr["FieldName"] = "PidDiag";
                        dr["DataFieldName"] = "PidDiag";
                    }
                    if (dr["FieldName"].ToString() == "pat_identity_name")
                    {
                        dr["FieldName"] = "PidIdentityName";
                        dr["DataFieldName"] = "PidIdentityName";
                    }
                    if (dr["FieldName"].ToString() == "Pma_micreport_flag")
                    {
                        dr["FieldName"] = "MicReportFlag";
                        dr["DataFieldName"] = "MicReportFlag";
                    }
                    #endregion
                }
                setting.PatListPanel.CreateLostColumn();
            }

            if (setting != null && setting.PatInfoPanel != null)
            {
                setting.PatInfoPanel.CreateLostColumn();
            }
            if (setting != null && setting.PatShortCut == null)
            {
                setting.PatShortCut = new PatShortCutSettingItem();
            }
            if (setting != null && setting.PatShortCut != null)
            {
                setting.PatShortCut.CreateLostColumn();
            }

            return setting;
        }

        /// <summary>
        /// 模块名称
        /// </summary>
        static string FormName = string.Empty;

        /// <summary>
        /// 加载组设置
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public static PatInputRuntimeSetting LoadGroup(string formName, string groupName)
        {
            formName = formName.Replace("New", "");

            PatInputRuntimeSetting setting = RuntimeSetting.NewInstance.LoadGroup<PatInputRuntimeSetting>(formName + "_" + groupName);
            if (setting != null && setting.PatListPanel != null)
            {
                foreach (DataRow dr in setting.PatListPanel.GridColSetting.Rows)
                {
                    #region 配置转换
                    if (dr["FieldName"].ToString() == "pat_host_order_int")
                    {
                        dr["FieldName"] = "RepSerialNum";
                        dr["DataFieldName"] = "RepSerialNum";
                    }
                    if (dr["FieldName"].ToString() == "pat_name")
                    {
                        dr["FieldName"] = "PidName";
                        dr["DataFieldName"] = "PidName";
                    }
                    if (dr["FieldName"].ToString() == "pat_c_name")
                    {
                        dr["FieldName"] = "PidComName";
                        dr["DataFieldName"] = "PidComName";
                    }
                    if (dr["FieldName"].ToString() == "pat_sex_name")
                    {
                        dr["FieldName"] = "PidSexName";
                        dr["DataFieldName"] = "PidSexName";
                    }
                    if (dr["FieldName"].ToString() == "pat_age_exp")
                    {
                        dr["FieldName"] = "PidAgeExp";
                        dr["DataFieldName"] = "PidAgeExp";
                    }
                    if (dr["FieldName"].ToString() == "pat_flag_name")
                    {
                        dr["FieldName"] = "RepStatusName";
                        dr["DataFieldName"] = "RepStatusName";
                    }
                    if (dr["FieldName"].ToString() == "pat_in_no")
                    {
                        dr["FieldName"] = "PidInNo";
                        dr["DataFieldName"] = "PidInNo";
                    }
                    if (dr["FieldName"].ToString() == "pat_sam_name")
                    {
                        dr["FieldName"] = "SamName";
                        dr["DataFieldName"] = "SamName";
                    }
                    if (dr["FieldName"].ToString() == "pat_bed_no")
                    {
                        dr["FieldName"] = "PidBedNo";
                        dr["DataFieldName"] = "PidBedNo";
                    }
                    if (dr["FieldName"].ToString() == "pat_dep_name")
                    {
                        dr["FieldName"] = "PidDeptName";
                        dr["DataFieldName"] = "PidDeptName";
                    }
                    if (dr["FieldName"].ToString() == "pat_i_name")
                    {
                        dr["FieldName"] = "LrName";
                        dr["DataFieldName"] = "LrName";
                    }
                    if (dr["FieldName"].ToString() == "pat_check_name")
                    {
                        dr["FieldName"] = "PidChkName";
                        dr["DataFieldName"] = "PidChkName";
                    }
                    if (dr["FieldName"].ToString() == "pat_report_name")
                    {
                        dr["FieldName"] = "BgName";
                        dr["DataFieldName"] = "BgName";
                    }
                    if (dr["FieldName"].ToString() == "pat_look_name")
                    {
                        dr["FieldName"] = "PatLookName";
                        dr["DataFieldName"] = "PatLookName";
                    }
                    if (dr["FieldName"].ToString() == "pat_look_date")
                    {
                        dr["FieldName"] = "RepReadDate";
                        dr["DataFieldName"] = "RepReadDate";
                    }
                    if (dr["FieldName"].ToString() == "pat_apply_date")
                    {
                        dr["FieldName"] = "SampApplyDate";
                        dr["DataFieldName"] = "SampApplyDate";
                    }
                    if (dr["FieldName"].ToString() == "itr_mid")
                    {
                        dr["FieldName"] = "ItrEname";
                        dr["DataFieldName"] = "ItrEname";
                    }
                    if (dr["FieldName"].ToString() == "hasresult")
                    {
                        dr["FieldName"] = "HasResult";
                        dr["DataFieldName"] = "HasResult";
                    }
                    if (dr["FieldName"].ToString() == "modify_flag")
                    {
                        dr["FieldName"] = "ModifyFlag";
                        dr["DataFieldName"] = "ModifyFlag";
                    }
                    if (dr["FieldName"].ToString() == "pat_diag")
                    {
                        dr["FieldName"] = "PidDiag";
                        dr["DataFieldName"] = "PidDiag";
                    }
                    if (dr["FieldName"].ToString() == "pat_identity_name")
                    {
                        dr["FieldName"] = "PidIdentityName";
                        dr["DataFieldName"] = "PidIdentityName";
                    }
                    #endregion

                }
                setting.PatListPanel.CreateLostColumn();
            }

            if (setting != null && setting.PatInfoPanel != null)
            {
                setting.PatInfoPanel.CreateLostColumn();
            }
            if (setting != null && setting.PatShortCut == null)
            {
                setting.PatShortCut = new PatShortCutSettingItem();
            }
            if (setting != null && setting.PatShortCut != null)
            {
                setting.PatShortCut.CreateLostColumn();
            }
            return setting;
        }

        /// <summary>
        /// 加载用户设置
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static PatInputRuntimeSetting LoadUser(string formName, string userName)
        {
            formName = formName.Replace("New", "");

            PatInputRuntimeSetting setting = RuntimeSetting.NewInstance.LoadUser<PatInputRuntimeSetting>(formName + "_" + userName);
            if (setting != null && setting.PatListPanel != null)
            {
                foreach (DataRow dr in setting.PatListPanel.GridColSetting.Rows)
                {
                    #region 配置转换
                    if (dr["FieldName"].ToString() == "pat_host_order_int")
                    {
                        dr["FieldName"] = "RepSerialNum";
                        dr["DataFieldName"] = "RepSerialNum";
                    }
                    if (dr["FieldName"].ToString() == "pat_name")
                    {
                        dr["FieldName"] = "PidName";
                        dr["DataFieldName"] = "PidName";
                    }
                    if (dr["FieldName"].ToString() == "pat_c_name")
                    {
                        dr["FieldName"] = "PidComName";
                        dr["DataFieldName"] = "PidComName";
                    }
                    if (dr["FieldName"].ToString() == "pat_sex_name")
                    {
                        dr["FieldName"] = "PidSexName";
                        dr["DataFieldName"] = "PidSexName";
                    }
                    if (dr["FieldName"].ToString() == "pat_age_exp")
                    {
                        dr["FieldName"] = "PidAgeExp";
                        dr["DataFieldName"] = "PidAgeExp";
                    }
                    if (dr["FieldName"].ToString() == "pat_flag_name")
                    {
                        dr["FieldName"] = "RepStatusName";
                        dr["DataFieldName"] = "RepStatusName";
                    }
                    if (dr["FieldName"].ToString() == "pat_in_no")
                    {
                        dr["FieldName"] = "PidInNo";
                        dr["DataFieldName"] = "PidInNo";
                    }
                    if (dr["FieldName"].ToString() == "pat_sam_name")
                    {
                        dr["FieldName"] = "SamName";
                        dr["DataFieldName"] = "SamName";
                    }
                    if (dr["FieldName"].ToString() == "pat_bed_no")
                    {
                        dr["FieldName"] = "PidBedNo";
                        dr["DataFieldName"] = "PidBedNo";
                    }
                    if (dr["FieldName"].ToString() == "pat_dep_name")
                    {
                        dr["FieldName"] = "PidDeptName";
                        dr["DataFieldName"] = "PidDeptName";
                    }
                    if (dr["FieldName"].ToString() == "pat_i_name")
                    {
                        dr["FieldName"] = "LrName";
                        dr["DataFieldName"] = "LrName";
                    }
                    if (dr["FieldName"].ToString() == "pat_check_name")
                    {
                        dr["FieldName"] = "PidChkName";
                        dr["DataFieldName"] = "PidChkName";
                    }
                    if (dr["FieldName"].ToString() == "pat_report_name")
                    {
                        dr["FieldName"] = "BgName";
                        dr["DataFieldName"] = "BgName";
                    }
                    if (dr["FieldName"].ToString() == "pat_look_name")
                    {
                        dr["FieldName"] = "PatLookName";
                        dr["DataFieldName"] = "PatLookName";
                    }
                    if (dr["FieldName"].ToString() == "pat_look_date")
                    {
                        dr["FieldName"] = "RepReadDate";
                        dr["DataFieldName"] = "RepReadDate";
                    }
                    if (dr["FieldName"].ToString() == "pat_apply_date")
                    {
                        dr["FieldName"] = "SampApplyDate";
                        dr["DataFieldName"] = "SampApplyDate";
                    }
                    if (dr["FieldName"].ToString() == "itr_mid")
                    {
                        dr["FieldName"] = "ItrEname";
                        dr["DataFieldName"] = "ItrEname";
                    }
                    if (dr["FieldName"].ToString() == "hasresult")
                    {
                        dr["FieldName"] = "HasResult";
                        dr["DataFieldName"] = "HasResult";
                    }
                    if (dr["FieldName"].ToString() == "modify_flag")
                    {
                        dr["FieldName"] = "ModifyFlag";
                        dr["DataFieldName"] = "ModifyFlag";
                    }
                    if (dr["FieldName"].ToString() == "pat_diag")
                    {
                        dr["FieldName"] = "PidDiag";
                        dr["DataFieldName"] = "PidDiag";
                    }
                    if (dr["FieldName"].ToString() == "pat_identity_name")
                    {
                        dr["FieldName"] = "PidIdentityName";
                        dr["DataFieldName"] = "PidIdentityName";
                    }
                    #endregion
                }
                setting.PatListPanel.CreateLostColumn();
            }

            if (setting != null && setting.PatInfoPanel != null)
            {
                setting.PatInfoPanel.CreateLostColumn();
            }

            if (setting != null && setting.PatShortCut == null)
            {
                setting.PatShortCut = new PatShortCutSettingItem();
            }
            if (setting != null && setting.PatShortCut != null)
            {
                setting.PatShortCut.CreateLostColumn();
            }
            return setting;
        }

        /// <summary>
        /// 加载程序设置
        /// </summary>
        /// <returns></returns>
        public static PatInputRuntimeSetting LoadPrograme()
        {
            return new PatInputRuntimeSetting();
        }

        /// <summary>
        /// 加载配置
        /// </summary>FrmPatDescEnter
        /// <param name="formName"></param>
        /// <param name="groupName"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static PatInputRuntimeSetting Load(string formName, string groupName, string userName)
        {
            formName = formName.Replace("New", "");
            PatInputRuntimeSetting setting = null;

            //加载用户
            setting = LoadUser(formName, userName);

            if (setting == null && groupName != null && groupName != string.Empty)
            {
                //加载组
                setting = LoadGroup(formName, groupName);
            }

            if (setting == null)
            {
                //加载系统
                setting = LoadSystem(formName);
            }

            if (setting != null && setting.PatListPanel != null)
            {
                setting.PatListPanel.CreateLostColumn();
            }
            if (setting != null && setting.PatShortCut == null)
            {
                setting.PatShortCut = new PatShortCutSettingItem();
            }
            if (setting != null && setting.PatShortCut != null)
            {
                setting.PatShortCut.CreateLostColumn();
            }

            if (setting == null)
            {
                //加载程序
                setting = LoadPrograme();
            }
            return setting;
        }
        #endregion

        /// <summary>
        /// 病人列表面板配置
        /// </summary>
        public PatListPanelSettingItem PatListPanel { get; set; }


        /// <summary>
        /// 病人结果面板配置
        /// </summary>
        public PatInputPatResultSettingItem PatResultPanel { get; set; }

        /// <summary>
        /// 病人资料明细面板配置
        /// </summary>
        public PatInputPatInfoSettingItem PatInfoPanel { get; set; }

        /// <summary>
        /// 病人快捷键面板配置
        /// </summary>
        public PatShortCutSettingItem PatShortCut { get; set; }

        public PatInputRuntimeSetting()
        {
            this.PatListPanel = new PatListPanelSettingItem();
            this.PatResultPanel = new PatInputPatResultSettingItem();
            this.PatInfoPanel = new PatInputPatInfoSettingItem();
            PatShortCut = new PatShortCutSettingItem();
        }
    }

    /// <summary>
    /// 病人列表面板配置类
    /// </summary>
    [Serializable]
    public class PatListPanelSettingItem
    {
        /// <summary>
        /// 原始记录背景颜色
        /// </summary>
        public Color BackColorNormal { get; set; }
        /// <summary>
        /// 已完成记录背景颜色
        /// </summary>
        public Color BackColorDone { get; set; }

        /// <summary>
        /// 已完成记录前景颜色
        /// </summary>
        public Color ForeColorDone { get; set; }
        /// <summary>
        /// 原始记录前景颜色
        /// </summary>
        public Color ForeColorNormal { get; set; }
        /// <summary>
        /// 回退记录背景颜色
        /// </summary>
        public Color BackColorReturn { get; set; }

        /// <summary>
        /// 回退记录前景颜色
        /// </summary>
        public Color ForeColorReturn { get; set; }
        /// <summary>
        /// 已审核记录背景颜色
        /// </summary>
        public Color BackColorAudited { get; set; }

        /// <summary>
        /// 已审核记录前景颜色
        /// </summary>
        public Color ForeColorAudited { get; set; }

        /// <summary>
        /// 已报告记录背景颜色
        /// </summary>
        public Color BackColorReported { get; set; }

        /// <summary>
        /// 已报告记录背景颜色
        /// </summary>
        public Color ForeColorReported { get; set; }

        /// <summary>
        /// 已中期报告记录背景颜色
        /// </summary>
        public Color BackColorPreReported { get; set; }

        /// <summary>
        /// 已中期报告记录字体颜色
        /// </summary>
        public Color ForeColorPreReported { get; set; }

        /// <summary>
        /// 已打印记录背景颜色
        /// </summary>
        public Color BackColorPrinted { get; set; }

        /// <summary>
        /// 已打印记录前景颜色
        /// </summary>
        public Color ForeColorPrinted { get; set; }

        /// <summary>
        /// 已看危急值记录背景颜色
        /// </summary>
        public Color BackColorUrgent { get; set; }

        /// <summary>
        /// 未看危急值记录背景颜色
        /// </summary>
        public Color BackColorNUrgent { get; set; }

        /// <summary>
        /// 已登记危急值记录
        /// </summary>
        public Color BackColorUrgentRecord { get; set; }

        /// <summary>
        /// 未登记危急值记录
        /// </summary>
        public Color BackColorNUrgentRecord { get; set; }

        /// <summary>
        /// 网格列定义
        /// </summary>
        public DataTable GridColSetting { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public PatListPanelSettingItem()
        {
            BackColorNormal = Color.Transparent;
            BackColorAudited = Color.Transparent;
            BackColorReported = Color.Transparent;
            BackColorPreReported = Color.Transparent;
            BackColorPrinted = Color.Transparent;
            BackColorUrgent = Color.FromArgb(192, 255, 255);
            BackColorNUrgent = Color.FromArgb(192, 255, 255);
            BackColorUrgentRecord = Color.Transparent;
            BackColorNUrgentRecord = Color.Transparent;

            ForeColorNormal = Color.Black;
            ForeColorAudited = Color.Green;
            ForeColorReported = Color.Blue;
            ForeColorPreReported = Color.Orange;//ColorTranslator.FromHtml("#FF8000");
            ForeColorPrinted = Color.Red;

            this.GridColSetting = InitGridColumn();
        }

        /// <summary>
        /// 初始化程序默认列配置
        /// </summary>
        private DataTable InitGridColumn()
        {
            DataTable table = new DataTable();
            table.Columns.Add("FieldName");
            table.Columns.Add("DataFieldName");
            table.Columns.Add("HeaderText");
            table.Columns.Add("ColumnWidth", typeof(int));
            table.Columns.Add("Visible", typeof(bool));
            table.Columns.Add("VisibleIndex", typeof(int));
            table.Columns.Add("UserCanEdit", typeof(bool));

            //GridColSetting.Rows.Add(new object[] { "pat_sid_int", "样本号", true, 1, false });
            table.Rows.Add(new object[] { "RepSerialNum", "RepSerialNum", "序号", 40, true, 1, true });
            table.Rows.Add(new object[] { "PidName", "PidName", "姓名", 65, true, 2, true });
            table.Rows.Add(new object[] { "PidComName", "PidComName", "检验组合", 120, true, 3, true });
            table.Rows.Add(new object[] { "PidIdentityName", "PidIdentityName", "病人身份", 65, false, 4, true });
            table.Rows.Add(new object[] { "PidSexName", "PidSexName", "性别", 52, true, 5, true });
            table.Rows.Add(new object[] { "PidAgeExp", "PidAgeExp", "年龄", 60, true, 6, true });
            table.Rows.Add(new object[] { "RepStatusName", "RepStatusName", "状态", 7, true, 7, true });
            table.Rows.Add(new object[] { "PidInNo", "PidInNo", "病人ID", 65, true, 8, true });
            table.Rows.Add(new object[] { "SamName", "SamName", "标本", 80, true, 9, true });
            table.Rows.Add(new object[] { "PidBedNo", "PidBedNo", "病床号", 70, true, 10, true });
            table.Rows.Add(new object[] { "PidDeptName", "PidDeptName", "科室", 80, true, 11, true });
            table.Rows.Add(new object[] { "LrName", "LrName", "录入人", 80, true, 12, true });
            table.Rows.Add(new object[] { "PidChkName", "PidChkName", "审核人", 80, true, 13, true });
            table.Rows.Add(new object[] { "BgName", "BgName", "报告人", 80, true, 14, true });
            table.Rows.Add(new object[] { "PatLookName", "PatLookName", "查看人", 80, true, 15, true });
            table.Rows.Add(new object[] { "RepReadDate", "RepReadDate", "查看时间", 80, true, 16, true });
            table.Rows.Add(new object[] { "ItrEname", "ItrEname", "仪器代码", 80, true, 17, true });
            table.Rows.Add(new object[] { "SampApplyDate", "SampApplyDate", "接收时间", 80, true, 18, true });
            table.Rows.Add(new object[] { "HasResult", "HasResult", "旗子状态", 40, false, 19, true });
            table.Rows.Add(new object[] { "ModifyFlag", "ModifyFlag", "上机标志", 60, false, 20, true });
            table.Rows.Add(new object[] { "PidDiag", "PidDiag", "诊断", 120, false, 21, true });
            table.Rows.Add(new object[] { "TatTime", "TatTime", "TAT(分钟)", 70, false, 22, true });
            table.Rows.Add(new object[] { "RepReportDate", "RepReportDate", "审核时间", 80, true, 23, true });
            table.Rows.Add(new object[] { "MicReportFlag", "MicReportFlag", "中期报告", 80, false, 24, true });
            return table;
        }

        /// <summary>
        /// 补全缺失的列
        /// </summary>
        public void CreateLostColumn()
        {
            DataTable tableTemplate = InitGridColumn();

            if (this.GridColSetting.Rows.Count != tableTemplate.Rows.Count)
            {
                foreach (DataRow rowTemplate in tableTemplate.Rows)
                {
                    string fieldName = rowTemplate["FieldName"].ToString();
                    if (this.GridColSetting.Select(string.Format("FieldName = '{0}'", fieldName)).Length == 0)
                    {
                        this.GridColSetting.Rows.Add(rowTemplate.ItemArray);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 病人结果表网格列定义
    /// </summary>
    [Serializable]
    public class PatInputPatResultSettingItem
    {

        #region 颜色提示

        #region 背景颜色
        /// <summary>
        /// 高于参考值上限背景颜色
        /// </summary>
        public Color BackColorGreaterThanRef { get; set; }

        /// <summary>
        /// 低于参考值下限背景颜色
        /// </summary>
        public Color BackColorLowerThanRef { get; set; }

        /// <summary>
        /// 高于极大值上限背景颜色
        /// </summary>
        public Color BackColorGreaterThanMax { get; set; }

        /// <summary>
        /// 低于极小值下限背景颜色
        /// </summary>
        public Color BackColorLowerThanMin { get; set; }

        /// <summary>
        /// 高于危急值上限背景颜色
        /// </summary>
        public Color BackColorGreaterThanPan { get; set; }

        /// <summary>
        /// 低于危急值下限背景颜色
        /// </summary>
        public Color BackColorLowerThanPan { get; set; }
        #endregion

        #region 字体颜色
        /// <summary>
        /// 高于参考值上限背景颜色
        /// </summary>
        public Color ForeColorGreaterThanRef { get; set; }

        /// <summary>
        /// 低于参考值下限背景颜色
        /// </summary>
        public Color ForeColorLowerThanRef { get; set; }

        /// <summary>
        /// 高于极大值上限背景颜色
        /// </summary>
        public Color ForeColorGreaterThanMax { get; set; }

        /// <summary>
        /// 低于极小值下限背景颜色
        /// </summary>
        public Color ForeColorLowerThanMin { get; set; }

        /// <summary>
        /// 高于危急值上限背景颜色
        /// </summary>
        public Color ForeColorGreaterThanPan { get; set; }

        /// <summary>
        /// 低于危急值下限背景颜色
        /// </summary>
        public Color ForeColorLowerThanPan { get; set; }

        /// <summary>
        /// 优先以项目顺序排序
        /// </summary>
        public bool OrderByItmSeq { get; set; }
        #endregion

        /// <summary>
        /// 默认保存病人信息后不跳转下一条
        /// </summary>
        public bool SavePatInfoNoNext { get; set; }

        /// <summary>
        ///定位到结果
        /// </summary>
        public bool SaveFocusResultColumn { get; set; }

        public int LeftGridMaxRow { get; set; }
        #endregion

        /// <summary>
        /// 显示方式
        /// 0=单列
        /// 1=双列
        /// 2=树型
        /// </summary>
        public int VisibleStyle { get; set; }

        /// <summary>
        /// 网格列定义
        /// </summary>
        public DataTable GridColSetting { get; set; }

        public bool EachViewOnlyTwoCombine { get; set; }

        public PatInputPatResultSettingItem()
        {
            this.VisibleStyle = LIS_Const.ResultGridVisibleStyle.Single;
            this.LeftGridMaxRow = 23;
            this.EachViewOnlyTwoCombine = true;

            this.BackColorGreaterThanRef = Color.Transparent;
            this.BackColorGreaterThanMax = Color.Transparent;
            this.BackColorGreaterThanPan = Color.Transparent;

            this.BackColorLowerThanRef = Color.Transparent;
            this.BackColorLowerThanMin = Color.Transparent;
            this.BackColorLowerThanPan = Color.Transparent;

            this.ForeColorGreaterThanRef = Color.Red;
            this.ForeColorGreaterThanMax = Color.Red;
            this.ForeColorGreaterThanPan = Color.Red;

            this.ForeColorLowerThanRef = Color.Blue;
            this.ForeColorLowerThanMin = Color.Blue;
            this.ForeColorLowerThanPan = Color.Blue;

            this.OrderByItmSeq = false;

            InitGridColumn();
        }

        private void InitGridColumn()
        {
            GridColSetting = new DataTable();
            GridColSetting.Columns.Add("FieldName");
            GridColSetting.Columns.Add("DataFieldName");
            GridColSetting.Columns.Add("HeaderText");
            GridColSetting.Columns.Add("ColumnWidth", typeof(int));
            GridColSetting.Columns.Add("Visible", typeof(bool));
            GridColSetting.Columns.Add("VisibleIndex", typeof(int));
            GridColSetting.Columns.Add("UserCanEdit", typeof(bool));

            GridColSetting.Rows.Add(new object[] { "res_itm_name", "res_itm_name", "项目名称", 100, true, 1, true });
            GridColSetting.Rows.Add(new object[] { "res_itm_ecd", "res_itm_ecd", "项目代码", 78, true, 2, true });
            GridColSetting.Rows.Add(new object[] { "res_chr", "res_chr", "结果", 70, true, 3, true });
            //OD结果？
            GridColSetting.Rows.Add(new object[] { "history_result1", "history_result1", "历史结果1", 75, true, 4, true });
            GridColSetting.Rows.Add(new object[] { "history_result2", "history_result2", "历史结果2", 75, true, 5, true });
            GridColSetting.Rows.Add(new object[] { "history_result3", "history_result3", "历史结果3", 75, true, 6, true });
            GridColSetting.Rows.Add(new object[] { "res_unit", "res_unit", "单位", 47, true, 7, true });
            GridColSetting.Rows.Add(new object[] { "res_ref_h", "res_ref_h", "参考上限", 62, true, 8, true });
            GridColSetting.Rows.Add(new object[] { "res_ref_l", "res_ref_l", "参考下限", 60, true, 9, true });
            GridColSetting.Rows.Add(new object[] { "res_ref_flag", "res_ref_flag", "提示", 50, true, 10, true });
            GridColSetting.Rows.Add(new object[] { "res_date", "res_date", "结果时间", 135, true, 11, true });
            GridColSetting.Rows.Add(new object[] { "res_type", "res_date", "结果类型", 75, true, 12, true });
        }
    }

    /// <summary>
    /// 病人明细信息自定义
    /// </summary>
    [Serializable]
    public class PatInputPatInfoSettingItem
    {
        /// <summary>
        /// 控件显示方式定义
        /// </summary>
        public DataTable Items { get; set; }

        private Control FindControl(string controlname, Control parentControl)
        {
            foreach (Control ctrl in parentControl.Controls)
            {
                if (ctrl.Name == controlname)
                {
                    return ctrl;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取"新增时获得焦点"控件名字
        /// </summary>
        /// <returns></returns>
        public string FindFocusOnAddNewControlName()
        {
            foreach (DataRow drItem in Items.Rows)
            {
                string controlName = drItem["ControlName"].ToString();

                if (Convert.ToBoolean(drItem["FocusOnAddNew"]) == true)
                {
                    return controlName;
                }
            }
            return string.Empty;
        }


        public bool IsPreserveOnNext(string controlName)
        {
            DataRow[] drs = this.Items.Select(string.Format("ControlName='{0}'", controlName));

            if (drs.Length > 0)
            {
                return Convert.ToBoolean(drs[0]["PreserveOnNext"]);
            }
            else
            {
                return false;
            }
        }

        private DataTable InitSetting()
        {
            DataTable setting = new DataTable();
            setting.Columns.Add("ControlName");
            setting.Columns.Add("Caption");
            setting.Columns.Add("Visible", typeof(bool));
            setting.Columns.Add("VisibleIndex", typeof(int));
            setting.Columns.Add("TabIndex", typeof(int));
            setting.Columns.Add("NextToSave", typeof(bool));
            setting.Columns.Add("PreserveOnNext", typeof(bool));
            setting.Columns.Add("FocusOnAddNew", typeof(bool));

            setting.Rows.Add(new object[] { "txtPatDate", "录入日期", true, 1, -1, false, true, false });
            setting.Rows.Add(new object[] { "txtPatType", "物理组别", true, 2, -1, false, true, false });
            setting.Rows.Add(new object[] { "txtPatInstructment", "测定仪器", true, 3, -1, false, true, false });
            setting.Rows.Add(new object[] { "txtPatSid", "样本号", true, 4, 0, false, false, false });
            setting.Rows.Add(new object[] { "txtPatIdType", "ID类型", true, 5, 1, false, true, false });

            setting.Rows.Add(new object[] { "txtPatID", "病人ID", true, 6, 2, false, false, true });
            setting.Rows.Add(new object[] { "txtPatChkType", "检查类型", true, 7, 3, false, false, false });
            setting.Rows.Add(new object[] { "txtPatDeptId", "送检科室", true, 8, 4, false, false, false });

            setting.Rows.Add(new object[] { "txtPatBedNo", "病床号", true, 9, -1, false, false, false });
            setting.Rows.Add(new object[] { "txtPatName", "姓名", true, 10, 5, false, false, false });
            setting.Rows.Add(new object[] { "txtPatSex", "性别", true, 11, 6, false, true, false });

            setting.Rows.Add(new object[] { "textAgeInput1", "年龄", true, 12, 7, false, false, false });
            setting.Rows.Add(new object[] { "txtPatSampleType", "标本类别", true, 13, 8, true, false, false });
            setting.Rows.Add(new object[] { "txtPatDiag", "临床诊断", true, 14, -1, false, false, false });
            setting.Rows.Add(new object[] { "txtPatSampleState", "标本状态", true, 15, -1, false, false, false });
            setting.Rows.Add(new object[] { "txtPatSource", "病人来源", true, 16, -1, false, false, false });

            setting.Rows.Add(new object[] { "txtPatDoc", "送检者", true, 17, -1, false, false, false });
            setting.Rows.Add(new object[] { "txtPat_social_no", "HIS卡号", true, 18, -1, false, false, false });
            setting.Rows.Add(new object[] { "txtPatInspetor", "检验者", true, 19, -1, false, true, false });
            setting.Rows.Add(new object[] { "txtPat_chk_purpose", "检查目的", true, 20, -1, false, false, false });

            //Items.Rows.Add(new object[] { "txtPatSampleDate", "采样时间", true, 21, -1, false, false, false });
            //Items.Rows.Add(new object[] { "txtPatSDate", "送检时间", true, 22, -1, false, false, false });
            //Items.Rows.Add(new object[] { "txtPatReceiveDate", "接收时间", true, 23, -1, false, false, false });
            //Items.Rows.Add(new object[] { "txtPatRecDate", "检验时间", true, 24, -1, false, false, false });
            setting.Rows.Add(new object[] { "txtPatFeeType", "费用类别", true, 25, -1, false, false, false });
            setting.Rows.Add(new object[] { "txtCombine", "套餐", true, 26, -1, false, false, false });
            return setting;
        }

        public PatInputPatInfoSettingItem()
        {
            this.Items = InitSetting();
        }

        public void CreateLostColumn()
        {
            DataTable tbOriginSetting = InitSetting();
            if (this.Items.Rows.Count < tbOriginSetting.Rows.Count)
            {
                foreach (DataRow rowOrigin in tbOriginSetting.Rows)
                {
                    if (this.Items.Select(string.Format("ControlName = '{0}'", rowOrigin["ControlName"].ToString())).Length == 0)
                    {
                        this.Items.Rows.Add(rowOrigin.ItemArray);
                    }
                }
            }
        }
    }


    /// <summary>
    /// 病人列表面板配置类
    /// </summary>
    [Serializable]
    public class PatShortCutSettingItem
    {

        /// <summary>
        /// 网格列定义
        /// </summary>
        public DataTable ShortCutSetting { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public PatShortCutSettingItem()
        {

            this.ShortCutSetting = InitGridColumn();
        }

        /// <summary>
        /// 初始化程序默认列配置
        /// </summary>
        private DataTable InitGridColumn()
        {
            DataTable table = new DataTable();
            table.Columns.Add("ButtonText");
            table.Columns.Add("ShortCut");
            string auditWord = LocalSetting.Current.Setting.AuditWord;
            string reportWord = LocalSetting.Current.Setting.ReportWord;


            table.Rows.Add(new object[] { "新增", "F1" });
            table.Rows.Add(new object[] { "", "F2" });
            table.Rows.Add(new object[] { "保存", "F3" });
            table.Rows.Add(new object[] { "删除", "F4" });
            table.Rows.Add(new object[] { "刷新", "F5" });

            table.Rows.Add(new object[] { string.IsNullOrEmpty(auditWord) ? "审核" : auditWord, "F6" });
            table.Rows.Add(new object[] { string.IsNullOrEmpty(reportWord) ? "报告" : reportWord, "F7" });
            table.Rows.Add(new object[] { "样本进程", "F8" });
            table.Rows.Add(new object[] { "结果视窗", "F9" });
            table.Rows.Add(new object[] { "打印", "F10" });
            table.Rows.Add(new object[] { "质控图", "F11" });
            table.Rows.Add(new object[] { "关闭", "F12" });

            return table;
        }

        /// <summary>
        /// 补全缺失的列
        /// </summary>
        public void CreateLostColumn()
        {
            DataTable tableTemplate = InitGridColumn();

            if (ShortCutSetting.Rows.Count != tableTemplate.Rows.Count)
            {
                foreach (DataRow rowTemplate in tableTemplate.Rows)
                {
                    string fieldName = rowTemplate["ShortCut"].ToString();
                    if (this.ShortCutSetting.Select(string.Format("ShortCut = '{0}'", fieldName)).Length == 0)
                    {
                        this.ShortCutSetting.Rows.Add(rowTemplate.ItemArray);
                    }
                }
            }
        }
    }


}
