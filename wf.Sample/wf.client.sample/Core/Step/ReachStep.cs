using System;
using System.Collections.Generic;

using System.Text;
using DevExpress.XtraGrid.Views.Grid;

namespace dcl.client.sample
{
    /// <summary>
    /// 送检流程
    /// </summary>
    public class ReachStep : IStep
    {
        public override string StepCode
        {
            get { return "4"; }
        }

        public override string StepName
        {
            get { return "送达"; }
        }


        public override bool HasNotDoneAction()
        {
            return BaseSampMain.ReachFlag  != 1;
        }

        public override bool ShouldShowSimpleSearchPanel
        {
            get
            {
                return true;
            }
        }

        public override bool CanSendMsg
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
                return base.Audit;
            }
        }
    }
}
