using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace dcl.client.resultquery
{
    public class NormalSelect : IReportSelect
    {
        public override bool PlInpatientMessageVisible
        {
            get { return false; }
        }


        /// <summary>
        /// 是否显示检验科相关操作人
        /// </summary>
        public override bool plOpererVisible
        {
            get { return true; }
        }

        public override bool PlYQVisible
        {
            get { return false; }
        }

        public override string[] ToolButton
        {
            get
            {
                List<string> lisButton = new List<string>();
                lisButton.Add("BtnSearch");
                if (dcl.client.frame.UserInfo.GetSysConfigValue("ReportSelectPrintType") != "只显示批量打印")
                    lisButton.Add("BtnSinglePrint");
                lisButton.Add("btnReturn");
                if (dcl.client.frame.UserInfo.GetSysConfigValue("checkSelectDelete") == "是")
                    lisButton.Add("BtnDelete");
                if (dcl.client.frame.UserInfo.GetSysConfigValue("checkPrintList") == "是")
                    lisButton.Add("BtnPrintList");
                if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("checkBatchPrint") == "是")
                {
                    lisButton.Add("BtnPrintBatch");
                    lisButton.Add("BtnPrintBatchPview");
                }
                if (ConfigurationManager.AppSettings["ShowExportButton"] != null
                && ConfigurationManager.AppSettings["ShowExportButton"].ToString() == "Y")
                {
                    lisButton.Add("BtnExport");
                }
                //系统配置：批量打印时需要设置打印人
                if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Repselect_setPrintPerson") == "是")
                    lisButton.Add("BtnQuickEntry");
                if (dcl.client.frame.UserInfo.GetSysConfigValue("ReportSelectPrintType") != "只显示批量打印")
                    lisButton.Add("BtnQualityOut");
                //系统配置：方正电子病历接口地址
                if (!string.IsNullOrEmpty(dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("RepSel_FounderCisAddress")))
                    lisButton.Add("BtnResultView");
                //系统配置：取消二审时检查是否已阅读
                if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("UndoAudit_Second_CheckLookcode") == "是")
                {
                    lisButton.Add("BtnUndo");
                }
                //系统配置：读卡器类型
                if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_CardReaderDriver") == "华大读写器")
                {
                    lisButton.Add("BtnQualityAudit");
                }
                lisButton.Add("BtnClose");
                return lisButton.ToArray();
            }
        }
    }
}
