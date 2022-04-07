using dcl.client.common;
using dcl.client.frame;
using DevExpress.XtraBars.Navigation;
using System.Windows.Forms;
using System.Drawing;
using System;
using dcl.client.cache;

namespace wf.ClientEntrance
{
    /// <summary>
    /// 菜单生成类 
    /// </summary>
    public class MeumBuilder
    {
        public void GenSysFuncMenu(MenuStrip menuStrip, EventHandler StripItem_Click)
        {
            menuStrip.Items.Clear();

            //标本管理
            BarcodeMeum(menuStrip, StripItem_Click);

            //检中
            PatientsMeum(menuStrip, StripItem_Click);

            //检验查询
            ResQueryMeum(menuStrip, StripItem_Click);

            //检后管理
            SamStoreMeum(menuStrip, StripItem_Click);

            //试剂管理
            ReagentMeum(menuStrip, StripItem_Click);

            //质控管理
            DclQCMeum(menuStrip, StripItem_Click);

            //统计分析
            StatisticsMeum(menuStrip, StripItem_Click);

            //辅助管理
            ToolsMeum(menuStrip, StripItem_Click);

            //检验事务
            OfficeMeum(menuStrip, StripItem_Click);

            //系统维护
            SystemMeum(menuStrip, StripItem_Click);

            //用户操作
            UserSettingMeum(menuStrip, StripItem_Click);

            DelSonMenu(menuStrip);
        }

        // 条码管理
        private void BarcodeMeum(MenuStrip menuStrip, EventHandler StripItem_Click)
        {
            if (!CacheClient.EnableLisFunc())
                return;

            GenParentMenu(menuStrip, "标本管理", "标本管理", null, StripItem_Click, Properties.Resources.检验申请);
            ToolStripMenuItem parent = menuStrip.Items["标本管理"] as ToolStripMenuItem;
            GenChlidMenu(parent, "eFrmBCPrint", "1.检验申请", "dcl.client.sample.FrmBCPrint", StripItem_Click);
            GenChlidMenu(parent, "eFrmBCSignIn", "2.标本流转", "dcl.client.sample.FrmBCSignIn", StripItem_Click);
            GenChlidMenu(parent, "eFrmBCManualYGPrint", "3.院感申请", "dcl.client.sample.FrmBCManualYGPrint", StripItem_Click);
            GenChlidMenu(parent, "eFrmSampleRegister", "4.标本登记", "dcl.client.result.FrmSampleRegisterNew", StripItem_Click);
            GenChlidMenu(parent, "eFrmOuterCourtRegister", "5.外院登记", "dcl.client.sample.FrmOuterCourtRegister", StripItem_Click);
            GenChlidMenu(parent, "eFrmSecondSign", "6.排样登记", "dcl.client.sample.FrmSecondSign", StripItem_Click);
            GenChlidMenu(parent, "efrmShelfSampleRegister", "7.快速排样", "dcl.client.result.frmShelfSampleRegister", StripItem_Click);
        }

        // 检中
        private void PatientsMeum(MenuStrip menuStrip, EventHandler StripItem_Click)
        {
            if (!CacheClient.EnableLisFunc()) return;

            GenParentMenu(menuStrip, "检验管理", "检验管理", null, StripItem_Click, Properties.Resources.检验管理);
            ToolStripMenuItem parent = menuStrip.Items["检验管理"] as ToolStripMenuItem;
            GenChlidMenu(parent, "eFrmPatEnter", "1.常规检验", "dcl.client.result.FrmPatEnterNew", StripItem_Click);
            GenChlidMenu(parent, "eFrmBacterialInput", "2.细菌检验", "dcl.client.result.FrmBacterialInputNew", StripItem_Click);
            GenChlidMenu(parent, "eFrmPatDescEnter", "3.其他检验", "dcl.client.result.FrmPatDescEnterNew", StripItem_Click);
            GenChlidMenu(parent, "eFrmPatDescEnter", "4.骨髓检验", "dcl.client.result.FrmMarrowInput", StripItem_Click);
        }

        // 检验查询
        private void ResQueryMeum(MenuStrip menuStrip, EventHandler StripItem_Click)
        {
            if (!CacheClient.EnableLisFunc()) return;

            GenParentMenu(menuStrip, "检验查询", "检验查询", null, StripItem_Click, Properties.Resources.检验查询);
            ToolStripMenuItem parent = menuStrip.Items["检验查询"] as ToolStripMenuItem;
            GenChlidMenu(parent, "eFrmCombineModeSel", "1.检验报告查询", "dcl.client.resultquery.FrmCombineModeSel", StripItem_Click);
            GenChlidMenu(parent, "eFrmSummaryPrint", "2.汇总报告打印", "dcl.client.ananlyse.FrmSummaryPrint", StripItem_Click);
            GenChlidMenu(parent, "eFrmItemSort", "3.项目分类查询", "dcl.client.resultquery.FrmItemSort", StripItem_Click);
            GenChlidMenu(parent, "FrmRealTimeResultView", "4.仪器原始数据", "dcl.client.result.FrmRealTimeResultView", StripItem_Click);
            GenChlidMenu(parent, "eWebQuery", "5.WEB报告查询", "eWebQuery", StripItem_Click);
            GenChlidMenu(parent, "eFrmBCSearch", "6.标本信息查询", "dcl.client.sample.FrmBCSearch", StripItem_Click);
            GenChlidMenu(parent, "FrmTwoWaySelect", "7.标本上机查询", "dcl.client.resultquery.FrmTwoWaySelect", StripItem_Click);
            GenChlidMenu(parent, "acFrmBCMonitor", "8.标本进程监控", "dcl.client.sample.FrmBCMonitor", StripItem_Click);
            GenChlidMenu(parent, "acFrmSampMonitor", "9.条码监控平台", "dcl.client.sample.FrmSampMonitor", StripItem_Click);
        }

        // 检后管理
        private void SamStoreMeum(MenuStrip menuStrip, EventHandler StripItem_Click)
        {
            if (!CacheClient.EnableLisFunc()) return;

            GenParentMenu(menuStrip, "检后管理", "检后管理", null, StripItem_Click, Properties.Resources.标本归档);
            ToolStripMenuItem parent = menuStrip.Items["检后管理"] as ToolStripMenuItem;
            GenChlidMenu(parent, "acFrmSamStoreRecord", "1.标本归档", "dcl.client.samstock.FrmSamStoreRecord", StripItem_Click);
            GenChlidMenu(parent, "acFrmSamSave", "2.标本存储", "dcl.client.samstock.FrmSamSave", StripItem_Click);
            GenChlidMenu(parent, "acFrmSamDestory", "3.标本销毁", "dcl.client.samstock.FrmSamDestory", StripItem_Click);
            GenChlidMenu(parent, "acFrmSamSearch", "4.归档查询", "dcl.client.samstock.FrmSamSearch", StripItem_Click);
        }

        // 试剂管理
        private void ReagentMeum(MenuStrip menuStrip, EventHandler StripItem_Click)
        {
            if (!CacheClient.EnableLisFunc()) return;

            GenParentMenu(menuStrip, "试剂管理", "试剂管理", null, StripItem_Click, Properties.Resources.标本归档);
            ToolStripMenuItem parent = menuStrip.Items["试剂管理"] as ToolStripMenuItem;
            GenChlidMenu(parent, "acFrmReagentSetting", "1.试剂库管理", "wf.client.reagent.FrmReagentSetting", StripItem_Click);
            GenChlidMenu(parent, "acFrmReagentApplication", "2.试剂申领", "wf.client.reagent.FrmReagentApplication", StripItem_Click);
            GenChlidMenu(parent, "acFrmReagentSubscribe", "3.试剂申购", "wf.client.reagent.FrmReagentSubscribe", StripItem_Click);
            GenChlidMenu(parent, "acFrmReagentPurchase", "4.采购计划", "wf.client.reagent.FrmReagentPurchase", StripItem_Click);
            GenChlidMenu(parent, "acFrmReagentStorage", "5.试剂入库", "wf.client.reagent.FrmReagentStorage", StripItem_Click);
            GenChlidMenu(parent, "acFrmReagentDelivery", "6.试剂出库", "wf.client.reagent.FrmReagentDelivery", StripItem_Click);
            GenChlidMenu(parent, "acFrmReagentLossReport", "7.试剂报损", "wf.client.reagent.FrmReagentLossReport", StripItem_Click);
            GenChlidMenu(parent, "acFrmReagentStatistics", "8.统计查询", "dcl.client.statistical.FrmReagentStatistics", StripItem_Click);
            GenChlidMenu(parent, "acFrmReagentBarcode", "9.试剂条码查询", "wf.client.reagent.FrmReagentBarcode", StripItem_Click);
        }

        // 质控管理
        private void DclQCMeum(MenuStrip menuStrip, EventHandler StripItem_Click)
        {
            if (!CacheClient.EnableLisFunc()) return;

            GenParentMenu(menuStrip, "质控管理", "质控管理", null, StripItem_Click, Properties.Resources.质控管理);
            ToolStripMenuItem parent = menuStrip.Items["质控管理"] as ToolStripMenuItem;
            GenChlidMenu(parent, "eFrmChart", "1.质控图表", "dcl.client.qc.FrmChart", StripItem_Click);
            GenChlidMenu(parent, "eFrmQcDataAnalyse", "2.质控统计", "dcl.client.qc.FrmQcDataAnalyse", StripItem_Click);
            GenChlidMenu(parent, "eFrmRoom", "3.室间质控", "dcl.client.qc.FrmRoom", StripItem_Click);
            GenChlidMenu(parent, "eFrmReagentsCompare", "4.数据对比", "dcl.client.qc.FrmReagentsCompare", StripItem_Click);
            GenChlidMenu(parent, "eFrmParameter", "5.质控参数", "dcl.client.qc.FrmQutityDict", StripItem_Click);
        }

        // 统计分析
        private void StatisticsMeum(MenuStrip menuStrip, EventHandler StripItem_Click)
        {
            if (!CacheClient.EnableLisFunc()) return;

            GenParentMenu(menuStrip, "统计分析", "统计分析", null, StripItem_Click, Properties.Resources.统计分析);
            ToolStripMenuItem parent = menuStrip.Items["统计分析"] as ToolStripMenuItem;

            GenChlidMenu(parent, "eFrmGeneralStatistics", "1.统计报表", "dcl.client.statistical.FrmGeneralStatistics", StripItem_Click);
            GenChlidMenu(parent, "eFrmDateBasicAnalyse", "2.数据分析", "dcl.client.ananlyse.FrmDateBasicAnalyse", StripItem_Click);
            GenChlidMenu(parent, "eFrmBacilliBasicAnalyse", "3.细菌统计", "dcl.client.ananlyse.FrmBacilliBasicAnalyse", StripItem_Click);
            GenChlidMenu(parent, "eFrmTimeAnalyse", "4.检验TAT", "dcl.client.ananlyse.FrmTimeAnalyse", StripItem_Click);
        }

        // 辅助管理
        private void ToolsMeum(MenuStrip menuStrip, EventHandler StripItem_Click)
        {
            if (!CacheClient.EnableLisFunc()) return;

            GenParentMenu(menuStrip, "辅助管理", "辅助管理", null, StripItem_Click, Properties.Resources.辅助管理);
            ToolStripMenuItem parent = menuStrip.Items["辅助管理"] as ToolStripMenuItem;

            GenChlidMenu(parent, "eFrmResultTemplate", "1.结果模板", "dcl.client.result.FrmResultTemplate", StripItem_Click);
            GenChlidMenu(parent, "eFrmPatientTemplateInput", "2.资料模板", "dcl.client.result.FrmPatientTemplateInput", StripItem_Click);
            GenChlidMenu(parent, "acWhonet", "3.WHONET导出", "dcl.client.whonet.FrmWhonet", StripItem_Click);
            GenChlidMenu(parent, "acFrmEiasaAnalyse", "4.酶标检验", "dcl.client.elisa.FrmEiasaAnalyse", StripItem_Click);
            GenChlidMenu(parent, "acFrmBatchEdit", "5.资料修改", "dcl.client.tools.FrmBatchEdit", StripItem_Click);
            GenChlidMenu(parent, "acFrmMergeResultNew", "6.结果合并", "dcl.client.tools.FrmMergeResultNew", StripItem_Click);
            GenChlidMenu(parent, "acFrmTempHandle", "7.环境监控", "dcl.client.tools.FrmTempHandle", StripItem_Click);
            GenChlidMenu(parent, "acFrmTempDetail", "8.环境监控日志", "dcl.client.tools.FrmTempDetail", StripItem_Click);
            GenChlidMenu(parent, "acFrmImportLisResult", "9.资料导入", "自定义", StripItem_Click);
            GenChlidMenu(parent, "acFrmLisDataReUpLoad", "10.数据重传", "dcl.client.tools.FrmLisDataReUpLoad", StripItem_Click);
            GenChlidMenu(parent, "eFrmLisDocManager", "11.检验文档", "dcl.client.tools.FrmLisDocManager", StripItem_Click);

        }

        // 检验事务
        private void OfficeMeum(MenuStrip menuStrip, EventHandler StripItem_Click)
        {
            if (!CacheClient.EnableLisFunc()) return;

            GenParentMenu(menuStrip, "检验事务", "检验事务", null, StripItem_Click, Properties.Resources.检验事务);
            ToolStripMenuItem parent = menuStrip.Items["检验事务"] as ToolStripMenuItem;

            GenChlidMenu(parent, "acFrmOfficeBusiness", "1.科室事务", "dcl.client.oa.FrmOfficeBusiness", StripItem_Click);
            GenChlidMenu(parent, "acFrmStaffManage", "2.人员档案", "dcl.client.oa.FrmStaffManage", StripItem_Click);
            GenChlidMenu(parent, "acFrmDocManage", "3.文档管理", "dcl.client.oa.FrmDocManage", StripItem_Click);
            GenChlidMenu(parent, "ACFrmOfficeMessage", "4.通知管理", "dcl.client.oa.FrmOfficeMessage", StripItem_Click);
            GenChlidMenu(parent, "ACFrmInstrmtBusiness", "5.仪器事务", "dcl.client.instrmt.FrmInstrmtMain", StripItem_Click);
            GenChlidMenu(parent, "ACFrmOrderSearch", "6.事务统计", "dcl.client.oa.FrmOrderSearch", StripItem_Click);
            GenChlidMenu(parent, "acFrmOfficePlan", "7.科室排班", "dcl.client.oa.FrmOfficePlan", StripItem_Click);
            GenChlidMenu(parent, "acFrmDutyDict", "8.排班设置", "dcl.client.oa.FrmDutyDict", StripItem_Click);
            GenChlidMenu(parent, "acFrmAttendance", "9.人员考勤", "dcl.client.oa.FrmAttendance", StripItem_Click);
            GenChlidMenu(parent, "acFrmAnnuncementMgr", "10.科室公告", "dcl.client.oa.FrmAnnuncementMgr", StripItem_Click);
            GenChlidMenu(parent, "acFrmOrderType", "11.事务维护", "dcl.client.oa.FrmOrderType", StripItem_Click);
            GenChlidMenu(parent, "acFrmHandOverMgr", "12.交班设定", "dcl.client.oa.FrmHandOverMgr", StripItem_Click);
            GenChlidMenu(parent, "acFrmHandOverInput", "13.交班管理", "dcl.client.oa.FrmHandOverInput", StripItem_Click);
        }

        // 系统维护
        private void SystemMeum(MenuStrip menuStrip, EventHandler StripItem_Click)
        {
            GenParentMenu(menuStrip, "系统维护", "系统维护", null, StripItem_Click, Properties.Resources.系统维护);
            ToolStripMenuItem parent = menuStrip.Items["系统维护"] as ToolStripMenuItem;

            GenChlidMenu(parent, "acFrmDictMain", "1.字典中心", "dcl.client.dicbasic.FrmDictMainDev", StripItem_Click);
            GenChlidMenu(parent, "acFrmUserManagePro", "2.用户管理", "dcl.client.users.FrmUserManagePro", StripItem_Click);
            GenChlidMenu(parent, "acFrmRoleManagePro", "3.角色管理", "dcl.client.users.FrmRoleManagePro", StripItem_Click);
            GenChlidMenu(parent, "acFrmFuncManagePro", "4.模块管理", "dcl.client.users.FrmFuncManagePro", StripItem_Click);
            GenChlidMenu(parent, "acFrmReporMain", "5.报表设计", "dcl.client.report.FrmReporMain", StripItem_Click);
            GenChlidMenu(parent, "acFrmSystemConfig", "6.系统参数", "dcl.client.users.FrmSystemConfig", StripItem_Click);
            GenChlidMenu(parent, "acFrmHIS", "7.系统接口", "dcl.client.interfaces.FrmHIS", StripItem_Click);
            GenChlidMenu(parent, "eFrmOperationLog", "8.修改日志", "dcl.client.users.FrmOperationLog", StripItem_Click);
            GenChlidMenu(parent, "acFrmSysLog", "9.系统日志", "dcl.client.users.FrmSysLog", StripItem_Click);
            GenChlidMenu(parent, "acFrmPrintConfiguration", "10.打印设置", "自定义", StripItem_Click);
            GenChlidMenu(parent, "acfrmLocalSetting", "11.本地设置", "自定义", StripItem_Click);
            GenChlidMenu(parent, "acFrmSysOperationLog", "12.操作记录", "dcl.client.users.FrmSysOperationLog", StripItem_Click);
            GenChlidMenu(parent, "acFrmDictOperationLog", "13.字典操作记录", "dcl.client.users.FrmDictOperationLog", StripItem_Click);
        }

        // 用户操作
        private void UserSettingMeum(MenuStrip menuStrip, EventHandler StripItem_Click)
        {
            GenParentMenu(menuStrip, "用户操作", "用户操作", null, StripItem_Click, Properties.Resources.用户操作);
            ToolStripMenuItem parent = menuStrip.Items["用户操作"] as ToolStripMenuItem;

            GenChlidMenu(parent, "acFrmrefrashcache", "1.刷新字典", "自定义", StripItem_Click);
            GenChlidMenu(parent, "eFrmpswchange", "2.修改密码", "自定义", StripItem_Click);
            GenChlidMenu(parent, "eFrmrelogin", "3.注销用户", "自定义", StripItem_Click);
            GenChlidMenu(parent, "aclocks", "4.锁定系统", "自定义", StripItem_Click);
            GenChlidMenu(parent, "aclogout", "5.帮助", "自定义", StripItem_Click);
            GenChlidMenu(parent, "aclogout", "6.退出", "自定义", StripItem_Click);
        }


        #region HelpMethod



        //添加父节点
        private void GenParentMenu(MenuStrip MainMenu, string name,
            string text, string tag, EventHandler onClick, Image image = null)
        {
            ToolStripMenuItem MenuItem = new ToolStripMenuItem(text, null, onClick);
            MenuItem.Name = name;
            MenuItem.Tag = tag;
            MenuItem.Image = image;
            MainMenu.Items.Add(MenuItem);
        }

        //添加子节点
        private void GenChlidMenu(ToolStripMenuItem ParentMenu, string name,
           string text, string tag, EventHandler onClick, Image image = null)
        {
            if (ParentMenu == null)
                return;
            if (!HasResource(tag))
                return;
            ToolStripMenuItem menuItem = new ToolStripMenuItem(text, null, onClick);
            menuItem.Name = name;
            menuItem.Tag = tag;
            ParentMenu.DropDownItems.Add(menuItem);
        }

        private void DelSonMenu(MenuStrip MainMenu)
        {
            foreach (ToolStripMenuItem item in MainMenu.Items)
            {
                if (item.DropDownItems.Count <= 0)
                {
                    item.Visible = false;
                }
            }
        }

        //验证是否有权限
        public bool HasResource(string funcId)
        {
            if (string.IsNullOrEmpty(funcId))
                return false;

            if (UserInfo.HaveFunctionByCode(ConvertToDataBaseName.ConvertToDBName(funcId)))
                return true;

            if (funcId == "自定义")
            {
                return true;
            }

            return false;
        }

        #endregion

    }
}
