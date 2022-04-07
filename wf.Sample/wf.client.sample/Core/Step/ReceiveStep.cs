using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using dcl.client.common;
using dcl.common;

namespace dcl.client.sample
{
    /// <summary>
    /// 签收流程
    /// </summary>
    public class ReceiveStep : IStep
    {
        public override string StepCode
        {
            get { return "5"; }
        }

        public override string StepName
        {
            get { return "签收"; }
        }

        public override bool HasNotDoneAction()
        {
            return BaseSampMain.ReceiverFlag != 1;
        }

        public override bool CanSendMsg
        {
            get
            {
                return true;
            }
        }

        public override bool ShouldShowSimpleSearchPanel
        {
            get
            {
                return true;
            }
        }

        public override IAudit Audit
        {
            get
            {
                if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Interface_HospitalInterfaceMode") != "通用")
                {
                    return new HISAudit();
                }
                else  
                {
                    return new LisAudit();
                }
            }
        }
    }
}
