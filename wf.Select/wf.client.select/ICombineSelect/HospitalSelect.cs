using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace dcl.client.resultquery
{
    public class HospitalSelect : IReportSelect
    {
        public override bool PlSidVisible
        {
            get { return false; }
        }

        public override bool PlAgeVisible
        {
            get { return false; }
        }

        public override bool PlIdTypeVisible
        {
            get
            {
                return false;
            }
        }
        public override bool PlOrderVisible
        {
            get { return false; }
        }

        public override bool PlInstrmtVisible
        {
            get { return false; }
        }

        public override bool PlInstrmt2Visible
        {
            get { return false; }
        }

        public override bool PlSexVisible
        {
            get { return false; }
        }

        public override bool PlItemVisible
        {
            get { return false; }
        }

        public override bool PlBtypeVisible
        {
            get { return false; }
        }

        public override bool PlCtypeVisible
        {
            get { return false; }
        }

        public override bool PlSamPleVisible
        {
            get { return false; }
        }

        public override bool PlOriginVisible
        {

            get
            {
                //系统配置：住院报告查询显示[来源类型]查询条件
                if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("HospitalSelect_showWhereOrigin") == "是")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public override bool PlBarCodeVisible
        {
            get { return false; }
        }

        public override bool plTelVisible
        {
            get { return false; }
        }

        public override bool PlInpatientMessageVisible
        {
            get { return false; }
        }

        public override bool PlYQVisible
        {
            get { return false; }
        }

        public override bool PlNo1AndiVisible
        {
            get { return false; }
        }

        public override bool PlNo2AndiVisible
        {
            get { return false; }
        }

        public override bool PlDoctorVisible
        {
            get { return true; }
        }

        public override System.Windows.Forms.DockStyle PanParDock
        {
            get { return System.Windows.Forms.DockStyle.Top; }
        }

        public override int PanParHeight
        {
            get { return 181 - 32; }
        }

        public override System.Windows.Forms.DockStyle PlBottomDock
        {
            get { return System.Windows.Forms.DockStyle.Fill; }
        }

        public override bool PlBottomVisible
        {
            get { return true; }
        }

        public override string[] ToolButton
        {
            get
            {
                List<string> lisButton = new List<string>();
                lisButton.Add("BtnSearch");

                if (ConfigurationManager.AppSettings["ShowPrintButton"] == null
                   || ConfigurationManager.AppSettings["ShowPrintButton"].ToString().Trim() != "FALSE")
                {
                    if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("ReportSelectPrintType") != "只显示批量打印")
                        lisButton.Add("BtnSinglePrint");
                    lisButton.Add("BtnPrintSet");
                }
                if (ConfigurationManager.AppSettings["ShowExportButton"] != null
                 && ConfigurationManager.AppSettings["ShowExportButton"].ToString() == "Y")
                {
                    lisButton.Add("BtnExport");
                }

                if (DepartValidate)
                {
                    lisButton.Add("btnAntibiotics");
                    lisButton.Add("BtnModify");
                }
                if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("checkBatchPrint") == "是")
                {
                    lisButton.Add("BtnPrintBatch");
                    lisButton.Add("BtnPrintBatchPview");
                }
                //系统配置：批量打印时需要设置打印人
                if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Repselect_setPrintPerson") == "是")
                    lisButton.Add("BtnQuickEntry");
                //lisButton.Add("BtnStat");
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
