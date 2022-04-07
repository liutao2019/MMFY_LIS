using System;
using System.Collections.Generic;

using System.Text;
using DevExpress.XtraGrid.Views.Grid;

namespace dcl.client.sample
{
    /// <summary>
    /// 开医嘱
    /// </summary>
    public class AdviceStep : IStep
    {
        public override bool HasNotDoPreAction()
        {
            throw new NotImplementedException();
        }

        public override string StepCode
        {
            get { return "-1"; }
        }

        public override string StepName
        {
            get { return "开医嘱"; }
        }

        public override bool HasNotDoneAction()
        {
            return false;
        }
    }
}
