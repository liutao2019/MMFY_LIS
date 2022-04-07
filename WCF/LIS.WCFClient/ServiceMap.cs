using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace dcl.client.wcf
{
    /// <summary>
    /// 服务地址类
    /// </summary>
    public class ServiceMap
    {
        static ServiceMap()
        {
            try
            {
                InitConfig();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
        }

        /// <summary>
        /// 服务地址
        /// </summary>
        public static NameValueCollection AppSettings { get; private set; }

        private static void InitConfig()
        {
            AppSettings = new NameValueCollection();

            AppSettings.Add("svc.basic", "common/CommonBIZ.svc");
            //Demo
            AppSettings.Add("svc.FrmProductManage", "demo/ProductBIZ.svc");
            AppSettings.Add("svc.FrmorderManage", "demo/OrderBIZ.svc");
            AppSettings.Add("svc.FrmOrderEdit", "demo/OrderBIZ.svc");
            //权限
            //AppSettings.Add("svc.FrmFuncManagePro", "power/FuncManageBIZ.svc");
            AppSettings.Add("svc.FrmFuncManagePro", "power/FuncManageProBIZ.svc");
            AppSettings.Add("svc.FrmRoleManagePro", "power/RoleManageProBIZ.svc");
            AppSettings.Add("svc.FrmUserManagePro", "power/UserManageBIZ.svc");
            AppSettings.Add("svc.FrmChangePassword", "power/UserManageBIZ.svc");
            AppSettings.Add("svc.FrmCheckDepart", "power/FuncManageBIZ.svc");
            AppSettings.Add("svc.FrmCheckPassword", "power/FuncManageBIZ.svc");
            AppSettings.Add("svc.FrmSystemConfig", "power/SystemConfigBIZ.svc");
            //日志
            AppSettings.Add("svc.FrmSysLog", "power/SysLogBIZ.svc");
            AppSettings.Add("svc.FrmOperationLog", "power/SysOperationLogBIZ.svc");
            //操作记录
            AppSettings.Add("svc.SysInterfaceLog", "SysInterfaceLog/SysInterfaceLogBIZ.svc");
            //登陆
            AppSettings.Add("svc.FrmLogin", "UserLoginBIZ.svc");
            //字典
            AppSettings.Add("svc.DictService", "dict/DictService.svc");
            //酶标分类
            AppSettings.Add("svc.ConElisaModControl", "dict/DictModBIZ.svc");
            AppSettings.Add("svc.ConElisaCalcControl", "dict/DictCalcBIZ.svc");
            AppSettings.Add("svc.ConElisaJudgeControl", "dict/DictJudgeBIZ.svc");
            AppSettings.Add("svc.FrmEiasaAnalyse", "eiasa/FrmEiasaAnalyse.svc");
            //酶标检验
            AppSettings.Add("svc.ElisaAnalyse", "eiasa/ElisaAnalyse.svc");
            //条码
            AppSettings.Add("svc.BCPrintControl", "barcode/BCPrintBIZ.svc");
            AppSettings.Add("svc.PatientControl", "barcode/BCPatientBIZ.svc");
            AppSettings.Add("svc.PatientControl1", "barcode/BCPatientBIZ.svc");
            AppSettings.Add("svc.CName", "barcode/BCCNameBIZ.svc");
            AppSettings.Add("svc.BCSign", "barcode/BCSignBIZ.svc");
            AppSettings.Add("svc.FrmBCMonitor", "barcode/BCMonitorBIZ.svc");
            AppSettings.Add("svc.ConItemCombineBarcode", "dict/ItemCombineBarcodeBIZ.svc");//合并规则
            AppSettings.Add("svc.frmShelfSampleRegister", "SampMain/ShelfSampRegisterBIZ.svc");
            //科室事务
            AppSettings.Add("svc.OaTableDetail", "office/OaTableDetailBIZ.svc");
            //用户信息
            AppSettings.Add("svc.SysUserInfo", "power/SysUserInfoBIZ.svc");

            //归档
            AppSettings.Add("svc.ConDictRack", "dict/DictRackBIZ.svc");//架子设定
            AppSettings.Add("svc.ConIceBox", "dict/IceBoxBIZ.svc");//冰箱设定
            AppSettings.Add("svc.ConCups", "dict/CupsBIZ.svc");//冰箱设定
            //实验码打印
            AppSettings.Add("svc.FrmLabcodePrint", "barcode/LabcodePrintBIZ.svc");
            //院网接口
            AppSettings.Add("svc.ConHISInterfaces", "interfaces/HISInterfacesBIZ.svc");
            AppSettings.Add("svc.ConContrastDefine", "interfaces/ContrastDefineBIZ.svc");
            //条码字典
            AppSettings.Add("svc.MessageControl", "barcode/BCMessageBIZ.svc");
            AppSettings.Add("svc.ConBCCuvette", "dict/BCCuvetteBIZ.svc");
            AppSettings.Add("svc.BCCombineControl", "barcode/BCCombineBIZ.svc");
            AppSettings.Add("svc.BCCuvetteShelfControl", "barcode/BCCuvetteShelfBIZ.svc");
            //标本统计
            AppSettings.Add("svc.FrmSecondStat", "barcode/SecondStatBIZ.svc");
            //抗生素
            AppSettings.Add("svc.FrmDict_Antibio1", "lab/Dict_AntibioBIZ.svc");
            AppSettings.Add("svc.ConHospital", "dict/HospitalBIZ.svc");
            //医院
            AppSettings.Add("svc.ConType", "dict/TypeBIZ.svc");
            //标本
            AppSettings.Add("svc.ConSample", "dict/SampleBIZ.svc");
            //（常规检验按钮）报告复制
            AppSettings.Add("svc.ReportCopyService", "ReportCopy/ReportCopyBIZ.svc");
            //新标本类别
            AppSettings.Add("svc.ConItem_Prop", "dict/Item_PropBIZ.svc");
            //仪器结果复制
            AppSettings.Add("svc.FrmMitmOrAdjustCopy", "dict/ItrResCopyBIZ.svc");
            //项目特征
            AppSettings.Add("svc.ConS_State", "dict/S_StateBIZ.svc");
            //仪器字典
            AppSettings.Add("svc.ConInstrmt", "dict/InstrmtBIZ.svc");
            //项目字典
            AppSettings.Add("svc.ConItemPro", "dict/ItemBIZ.svc");
            AppSettings.Add("svc.ConItemProInfo", "dict/ItemBIZ.svc");
            //温控字典
            AppSettings.Add("svc.ConTemperature", "dict/TemperatureBIZ.svc");
            AppSettings.Add("svc.ConDictHarvester", "dict/DictHarvesterBIZ.svc");
            //item
            AppSettings.Add("svc.ConCombine", "dict/ItemCombineBIZ.svc");
            //描述评价字典
            AppSettings.Add("svc.ConBscripe", "dict/BscripeBIZ.svc");
            //诊断字典
            AppSettings.Add("svc.ConDiagnos", "dict/DiagnosBIZ.svc");
            //诊断组合关联
            AppSettings.Add("svc.ConIcdCombine", "dict/DicIcdCombineBIZ.svc");
            //
            AppSettings.Add("svc.ConCheckb", "dict/CheckbBIZ.svc");
            //
            AppSettings.Add("svc.ConOrigin", "dict/OriginBIZ.svc");
            //
            AppSettings.Add("svc.ConNo_Type", "dict/No_TypeBIZ.svc");
            //depart
            AppSettings.Add("svc.ConDepart", "dict/DepartBIZ.svc");
            //镜检字典
            AppSettings.Add("svc.ConUGR_Type", "dict/UGR_TypeBIZ.svc");
            //条码拆分字典
            AppSettings.Add("svc.frmCombineView", "dict/CombineSplitBarCodeBIZ.svc");
            //镜检字典
            //计算字典
            AppSettings.Add("svc.ConClItem", "dict/ClItemBIZ.svc");
            //医生字典
            AppSettings.Add("svc.ConDoctor", "dict/DoctorBIZ.svc");
            AppSettings.Add("svc.ConInspect", "dict/InspectBIZ.svc");
            //仪器通道
            AppSettings.Add("svc.ConMitm_No", "dict/Mitm_NoBIZ.svc");
            //仪器通道->结果视图
            AppSettings.Add("svc.MitmNoResultViewService", "dict/DictMitmNoResultViewBIZ.svc");
            //仪器结果修正
            AppSettings.Add("svc.ConResAdjust", "dict/ConResAdjustBIZ.svc");
            //报告模板
            AppSettings.Add("svc.SaveStatTemp", "stat/StatTempBIZ.svc");
            //检验单
            AppSettings.Add("svc.FrmNewLabSingle", "lab/NewLabSingleBIZ.svc");
            AppSettings.Add("svc.FrmPatientInput", "lab/PatientInput.svc");
            AppSettings.Add("svc.FrmPatientDescribeEnter", "lab/PatientInput.svc");
            AppSettings.Add("svc.PatientEnter", "lab/PatientEnterService.svc");
            //酶标结果视窗
            AppSettings.Add("svc.ResultMerge", "lab/ObrResultMergeBIZ.svc");
            //结果模板录入
            AppSettings.Add("svc.FrmResultTemplate", "lab/ResulTempBIZ.svc");
            //资料模板
            AppSettings.Add("svc.FrmPatientTemplateInput", "lab/PatTempInputBIZ.svc");
            //细菌录入
            AppSettings.Add("svc.FrmBacterialInput", "lab/BacterialInputBIZ.svc");
            AppSettings.Add("svc.FrmBacterialInputTest", "lab/BacterialInputBIZ.svc");
            //病人结果复查
            AppSettings.Add("svc.PatientRecheck", "select/PatientRecheckBIZ.svc");
            AppSettings.Add("svc.PatResult", "select/PatResultBIZ.svc");
            //组合模式查询
            AppSettings.Add("svc.FrmCombineModeSel", "select/CombModelSelBIZ.svc");
            //细菌字典
            AppSettings.Add("svc.ConDict_Bacteri", "bac/Dict_BacteriBIZ.svc");
            //细菌分类
            AppSettings.Add("svc.ConDict_Btype", "bac/Dict_BtypeBIZ.svc");
            AppSettings.Add("svc.ConDict_Bacteri_new", "bac/Dict_BtypeBIZ.svc");
            //药敏分类
            AppSettings.Add("svc.ConDict_An_Stype", "bac/Dict_An_StypeBIZ.svc");
            //抗生素字典
            AppSettings.Add("svc.ConDict_Antibio", "bac/Dict_AntibioBIZ.svc");
            //药敏标准表
            AppSettings.Add("svc.ConDict_An_Sstd", "bac/Dict_An_SstdBIZ.svc");
            //病人资料批量修改
            AppSettings.Add("svc.FrmBatchEdit", "tool/BatchEditNewBIZ.svc");
            //质控参数 新旧库 
            AppSettings.Add("svc.FrmParameterService", "quality/FrmParameterBIZ.svc");
            //数据装换设置
            AppSettings.Add("svc.FrmDiversion", "quality/DiversionBIZ.svc");
            //质控仪器通道总
            AppSettings.Add("svc.FrmQcRuleInst", "quality/QcRuleInstBIZ.svc");
            //质控项目录入 改造
            AppSettings.Add("svc.FastAddQcItemService", "QCSpecific/FastAddQcItemBIZ.svc");
            //质控规则
            AppSettings.Add("svc.FrmCriterion", "quality/CriterionBIZ.svc");
            //质控项目表（质控参数）
            AppSettings.Add("svc.QcMateriaDetailService", "QCSpecific/QcMateriaDetailBIZ.svc");
            //质控物明细表（质控参数）
            AppSettings.Add("svc.QcMateriaService", "QCSpecific/QcMateriaBIZ.svc");
            //质控规则关联表（质控参数）
            AppSettings.Add("svc.QcMateriaRuleService", "QCSpecific/QcMateriaRuleBIZ.svc");
            //描述评价
            AppSettings.Add("svc.FrmView", "lab/ViewBIZ.svc");
            //添加细菌
            AppSettings.Add("svc.FrmBscripe", "lab/ViewBIZ.svc");
            AppSettings.Add("svc.FrmBscripeSelect", "lab/ViewBIZ.svc");
            //无菌和涂片
            AppSettings.Add("svc.ConNobact", "dict/NobactBIZ.svc");
            //报表
            AppSettings.Add("svc.FrmReporMain", "report/ReporMainBIZ.svc");
            //条码明细表
            AppSettings.Add("svc.FrmReporDetail", "select/PidReportDetailBIZ.svc");
            //新增 病人信息
            AppSettings.Add("svc.ProxyPidReportMain", "lab/PidReportMainBIZ.svc");
            //病人检验结果
            AppSettings.Add("svc.ObrResult", "select/ObrResultBIZ.svc");
            //病人描述结果
            AppSettings.Add("svc.ObrResultDesc", "select/ObrResultDescBIZ.svc");
            //事务
            AppSettings.Add("svc.FrmSWMian", "sw/SWMianBIZ.svc");
            //科室事务
            AppSettings.Add("svc.FrmOrderType", "office/OrderTableBIZ.svc");
            AppSettings.Add("svc.FrmOfficePlan", "office/OfficeShiftPlanBIZ.svc");
            AppSettings.Add("svc.FrmAttendance", "office/OfficeAttendanceBIZ.svc");
            AppSettings.Add("svc.FrmDutyDict", "office/OfficeShiftDictBIZ.svc");
            //通知管理
            AppSettings.Add("svc.NoticeManage", "NoticeManage/NoticeManageBIZ.svc");
            //用户危急值信息
            AppSettings.Add("svc.UserMessageService", "Message/UserMessageBIZ.svc");
            //报表封装
            AppSettings.Add("svc.FrmReportPrint", "report/ReportPrintBIZ.svc");
            //项目分类查询
            AppSettings.Add("svc.FrmItemSort", "select/ItemSortBIZ.svc");
            //TAT时间统计分析
            AppSettings.Add("svc.FrmTATAnalyse", "analyse/TATAnalyseBIZ.svc");
            //综合统计分析
            AppSettings.Add("svc.FrmGeneralStatistics", "stat/GeneralStatisticsBIZ.svc");
            AppSettings.Add("svc.RuntimeSetting", "common/RunTimeSetting.svc");
            //汇总打印
            AppSettings.Add("svc.FrmSummaryPrint", "analyse/SummaryPrintBIZ.svc");
            //打印设置
            AppSettings.Add("svc.FrmPrintConfiguration", "report/PrintConfigurationBIZ.svc");
            //获取服务器版本
            AppSettings.Add("svc.FrmServerReport", "report/ServerReportBIZ.svc");
            //标本备注
            AppSettings.Add("svc.ConSamRemarks", "dict/SamRemarksBIZ.svc");
            //参考值名称
            AppSettings.Add("svc.ConReferenceName", "dict/ReferenceNameBIZ.svc");
            //费用类别
            AppSettings.Add("svc.ConFeesType", "dict/FeesTypeBIZ.svc");
            //仪器组合
            AppSettings.Add("svc.ConInstrmtCom", "dict/InstrmtComBIZ.svc");
            //消息
            AppSettings.Add("svc.MessageBIZ", "tool/MessageBIZ.svc");
            //医嘱确认客户端调用
            AppSettings.Add("svc.AdviceConfirmService", "interfaces/AdviceConfirmBIZ.svc");
            //双向查询
            AppSettings.Add("svc.FrmTwoWaySelect", "select/TwoWaySelectNewBIZ.svc");
            //病人信息修改工具
            AppSettings.Add("svc.FrmPatientEditTool", "lab/PatientEditToolBIZ.svc");
            //设备管理系统  保养字典(实体改造)
            AppSettings.Add("svc.ItrInstrumentMaintain", "InstrumentThings/DicItrInstrumentMaintainBIZ.svc");
            //设备管理系统  保养登记(实体改造)
            AppSettings.Add("svc.ItrInstrumentRegistration", "InstrumentThings/DicItrInstrumentRegistrationBIZ.svc");
            //设备管理系统  保养登记查询(实体改造)
            AppSettings.Add("svc.ItrInstrumentServicing", "InstrumentThings/DicItrInstrumentServicingBIZ.svc");
            //取报告时间字典
            AppSettings.Add("svc.DicGetReportTime", "dict/DicGetReportTimeBIZ.svc");
            //药敏卡明细
            AppSettings.Add("svc.DictAnSstd", "bac/DictAnSstdBIZ.svc");
            //通用院网接口2.0
            AppSettings.Add("svc.DataInterface", "interfaces/DataInterfaceBIZ.svc");
            //Lis院网接口
            AppSettings.Add("svc.LisDataInterface", "interfaces/LisDataInterfaceBIZ.svc");
            //缓存服务
            AppSettings.Add("svc.CacheService", "common/CacheServiceBIZ.svc");
            //组合时间限定
            AppSettings.Add("svc.ConCombineTimerule", "dict/CombineTimeruleBIZ.svc");
            //插件接口
            AppSettings.Add("svc.PluginInterface", "PluginInterface/PluginInterface.svc");
            //TAT统计
            AppSettings.Add("svc.ProxyTATStatistics", "stat/TATStatistics.svc");
            //TAT监控
            AppSettings.Add("svc.ProxyTATMonitor", "stat/TATMonitor.svc");
            //标本管理
            AppSettings.Add("svc.ProxySamManage", "SamManage/SamStore.svc");
            //标本管理 标本归档
            AppSettings.Add("svc.SampStoreRecord", "SamManage/SampStoreRecord.svc");
            //标本管理 标本存储
            AppSettings.Add("svc.SampStockService", "SamManage/SampStock.svc");

            //标本进程监控
            AppSettings.Add("svc.SampProcessMonitor", "Monitor/SampProcessMonitorBIZ.svc");
            //标本信息扩展表
            AppSettings.Add("svc.SampOperateDetailService", "SampMain/SampOperateDetailBIZ.svc");

            //标本管理 标本销毁
            AppSettings.Add("svc.SamDestoryService", "SamManage/SamDestory.svc");
            //标本管理 归档查询
            AppSettings.Add("svc.SamSearchService", "SamManage/SamSearch.svc");
            //科室公告
            AppSettings.Add("svc.ProxyOfficAnnouncement", "office/OfficeAnnouncement.svc");
            //结果合并
            AppSettings.Add("svc.ResultMergeNew", "lab/ResultMergeNewBiz.svc");
            //图像浏览(图像报告表)
            AppSettings.Add("svc.ObrResultImageService", "Result/ObrResultImageBIZ.svc");
            //本地配置模块-借用统计模块服务端
            AppSettings.Add("svc.frmLocalSetting", "stat/GeneralStatisticsBIZ.svc");
            //效验字典
            AppSettings.Add("svc.ConEfficacy", "dict/EfficacyBIZ.svc");
            //备份与还原项目结果
            AppSettings.Add("svc.FrmBakItmForResulto", "lab/BakItmForResultoBIZ.svc");
            //血型复核
            AppSettings.Add("svc.FrmCheckBloodType", "lab/CheckBloodTypeBIZ.svc");
            //血型复核查询
            AppSettings.Add("svc.FrmCheckBloodTypeSel", "lab/CheckBloodTypeBIZ.svc");
            //细菌培养基字典
            AppSettings.Add("svc.DictMedia", "bac/DictMediaBIZ.svc");
            //条码图片
            AppSettings.Add("svc.frmBCImageView", "barcode/BCImageViewBIZ.svc");
            //细菌培养基时限字典
            AppSettings.Add("svc.DictComMedia", "bac/DictComMediaBIZ.svc");
            //质控监控
            AppSettings.Add("svc.FrmQcMonitor", "quality/QcMonitorBIZ.svc");
            //细菌
            AppSettings.Add("svc.FrmBacterialInputNew", "lab/BacterialInputBIZ.svc");
            //新条码操作
            AppSettings.Add("svc.SampMainService", "SampMain/SampMainBIZ.svc");
            //危急值字典
            AppSettings.Add("svc.conItemUrgentValue", "dict/DictItemUrgentValueBIZ.svc");
            //大小组合
            AppSettings.Add("svc.ConBCCombineSplit", "dict/CombineSplitBIZ.svc");

            //新增 危急值消息  
            AppSettings.Add("svc.ObrMessageService", "Message/ObrMessageBIZ.svc");
            //新增 危急值处理表 
            AppSettings.Add("svc.ObrMessageReceive", "Message/ObrMessageReceiveBIZ.svc");
            //新增 危急值消息表
            AppSettings.Add("svc.ObrMessageContent", "Message/ObrMessageContentBIZ.svc");
            //新增 危急值数据
            AppSettings.Add("svc.UrgentObrMessage", "Message/UrgentObrMessageBIZ.svc");
            //新增 组合TAT数据
            AppSettings.Add("svc.CombineTATMsgService", "Message/CombineTATMsgBIZ.svc");
            //新增 仪器危急值数据
            AppSettings.Add("svc.InstrmtUrgentTATMsgService", "Message/InstrmtUrgentTATMsgBIZ.svc");
            //新增 病人信息扩展表数据
            AppSettings.Add("svc.PidReportMainExtService", "Message/PidReportMainExtBIZ.svc");

            //新增 回退信息
            AppSettings.Add("svc.ConSampReturn", "dict/SampReturnBIZ.svc");
            //新增 试管架
            AppSettings.Add("svc.ConTubeRack", "dict/TubeRackBIZ.svc");
            //新增 报告时间
            AppSettings.Add("svc.ConComReptime", "dict/ComReptimeBIZ.svc");
            //新增 仪器原始数据
            AppSettings.Add("svc.FrmRealTimeResultView", "lab/RealTimeResultViewBIZ.svc");

            #region 酶标字典
            //项目设置
            AppSettings.Add("svc.ConElisaItemHole", "dict/DicItemHoleBIZ.svc");
            //孔位序号
            AppSettings.Add("svc.ConElisaHoleMode", "dict/DicHoleModeBIZ.svc");
            //孔位状态
            AppSettings.Add("svc.ConElisaHoleStatus", "dict/DicHoleStatusBIZ.svc");
            #endregion

            //新增 缓存数据
            AppSettings.Add("svc.ConCacheData", "common/CacheDataBIZ.svc");

            //新增 条码明细
            AppSettings.Add("svc.SampDetailService", "SampMain/SampDetailBIZ.svc");

            //新增 排样登记
            AppSettings.Add("svc.SecondSign", "SampMain/SecondSignBIZ.svc");

            //新增 条码流程
            AppSettings.Add("svc.SampProcessDetialService", "SampMain/SampProcessDetailBIZ.svc");

            //新增 条码回退
            AppSettings.Add("svc.SampReturnService", "SampMain/SampReturnBIZ.svc");

            //新增 条码下载
            AppSettings.Add("svc.SampMainDownloadService", "SampMain/SampMainDownloadBIZ.svc");

            //新增 报表打印
            AppSettings.Add("svc.ReportPrint", "report/DCLReportPrintBIZ.svc");

            //新增 相关结果
            AppSettings.Add("svc.PatRelateResult", "select/ObrRelateResultBIZ.svc");

            //新增 常规检验保存
            AppSettings.Add("svc.FrmPatEnterNew", "select/PatEnterNewBIZ.svc");

            //新增 质控图表显示
            AppSettings.Add("svc.QcResultService", "QCSpecific/QcResultBIZ.svc");

            //新增 细菌报告模块保存
            AppSettings.Add("svc.MicEnterNew", "Result/MicEnterNewBIZ.svc");

            //新增 骨髓报告模块保存
            AppSettings.Add("svc.MarrowEnter", "Result/MarrowEnterBIZ.svc");

            //新增 交班管理
            AppSettings.Add("svc.FrmHandOverInput", "office/OfficeHoRecordBIZ.svc");

            //新增 交班管理
            AppSettings.Add("svc.FrmHandOverMgr", "office/OfficeHandOverBIZ.svc");

            //仪器报警信息
            AppSettings.Add("svc.InstrmtWardingMsg", "lab/InstrmtWardingMsgBIZ.svc");

            //新增 标本进程
            AppSettings.Add("svc.FrmMonitor", "select/PatMonitorBIZ.svc");
            //结果备份
            AppSettings.Add("svc.ObrResultBakItm", "lab/ObrResultBakItmBIZ.svc");
            //新版院网接口
            AppSettings.Add("svc.SMHisInterfaces", "interfaces/SMHisInterfacesBIZ.svc");
            //温控
            AppSettings.Add("svc.FrmTempHandle", "tool/TempHandleBIZ.svc");
            //结果合并
            AppSettings.Add("svc.MergeResultService", "tool/MergeResultBIZ.svc");
            //自助打印
            AppSettings.Add("svc.TouchPrintService", "report/TouchPrintBIZ.svc");
            //自助打印
            AppSettings.Add("svc.RuntimeSettingService", "setting/RuntimeSettingBIZ.svc");
            //系统接口：连接参数
            AppSettings.Add("svc.DataInterfaceConnectionService", "DataInterfaceCon/DataInterfaceConnectionBIZ.svc");
            //系统接口：接口参数
            AppSettings.Add("svc.DataInterfaceCommandService", "DataInterfaceCon/DataInterfaceCommandBIZ.svc");
            //从接口获取病人信息
            AppSettings.Add("svc.PidReportMainInterface", "select/PidReportMainInterfaceBIZ.svc");
            //审核
            AppSettings.Add("svc.PidReportMainAudit", "lab/PidReportMainAuditBIZ.svc");
            //排队取号
            AppSettings.Add("svc.QueueNumber", "QueueNumber/QueueNumberBIZ.svc");
            //接口工具服务
            AppSettings.Add("svc.DCLInterfacesTool", "interfaces/DCLInterfacesToolBIZ.svc");
            //whonet导出
            AppSettings.Add("svc.FrmWhonet", "Whonet/whonetBIZ.svc");
            //质控评价
            AppSettings.Add("svc.QcAnalysisService", "quality/QcAnalysisBIZ.svc");

            //检验文档
            AppSettings.Add("svc.LisDocManager", "tool/LisDocManagerBIZ.svc");

            #region 微生物

            //抗生素大类字典
            AppSettings.Add("svc.ConDic_Antibio_Type", "MicDict/Dic_Antibio_TypeBIZ.svc");
            //抗生素分类定义字典
            AppSettings.Add("svc.ConDefAntiType", "MicDict/DefAntiTypeBIZ.svc");

            #endregion

            #region 试剂管理
            AppSettings.Add("svc.ConReaUnit", "dict/DictReagentUnit.svc");
            AppSettings.Add("svc.ConReaProduct", "dict/DictReaProductBIZ.svc");
            AppSettings.Add("svc.ConReaClaimant", "dict/DictReaClaimant.svc");
            AppSettings.Add("svc.ConReaDept", "dict/DictReaDept.svc");
            AppSettings.Add("svc.ConReaGroup", "dict/DictReaGroup.svc");
            AppSettings.Add("svc.ConReaStoreCondition", "dict/DictReaStoreConditionBIZ.svc");
            AppSettings.Add("svc.ConReaStorePosition", "dict/DictReaStorePosition.svc");
            AppSettings.Add("svc.ConReaSupplier", "dict/DictReaSupplier.svc");
            AppSettings.Add("svc.FrmReagentSetting", "Reagent/ReagentSettingBIZ.svc");
            AppSettings.Add("svc.ReaApplyService", "Reagent/ReagentApplyBIZ.svc");
            AppSettings.Add("svc.ReaApplyDetailService", "Reagent/ReagentApplyDetailBIZ.svc");
            AppSettings.Add("svc.ReaPurchaseService", "Reagent/ReagentPurchaseBIZ.svc");
            AppSettings.Add("svc.ReaPurchaseDetailService", "Reagent/ReagentPurchaseDetailBIZ.svc");
            AppSettings.Add("svc.ReaSubscribeService", "Reagent/ReagentSubscribeBIZ.svc");
            AppSettings.Add("svc.ReaSubscribeDetailService", "Reagent/ReagentSubscribeDetailBIZ.svc");
            AppSettings.Add("svc.ReaStorageService", "Reagent/ReagentStorageBIZ.svc");
            AppSettings.Add("svc.ReaStorageDetailService", "Reagent/ReagentStorageDetailBIZ.svc");
            AppSettings.Add("svc.ReaDeliveryService", "Reagent/ReagentDeliveryBIZ.svc");
            AppSettings.Add("svc.ReaDeliveryDetailService", "Reagent/ReagentDeliveryDetailBIZ.svc");
            AppSettings.Add("svc.ReaLossReportService", "Reagent/ReagentLossReportBIZ.svc");
            AppSettings.Add("svc.ReaLossReportDetailService", "Reagent/ReagentLossReportDetailBIZ.svc");
            AppSettings.Add("svc.ConReaReturn", "dict/ReaReturnBIZ.svc");
            #endregion
        }


    }
}
