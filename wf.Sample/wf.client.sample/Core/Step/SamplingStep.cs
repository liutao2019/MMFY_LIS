using System;
using System.Collections.Generic;

using System.Text;
using System.Data;
using dcl.client.common;
using dcl.client.frame;

namespace dcl.client.sample
{
    /// <summary>
    /// 采集流程
    /// </summary>
    public class SamplingStep : IStep
    {

        public override string WarnForTimeOut
        {
            get
            {
                return string.Format("标本采集时间与当前时间超过{0},是否继续?", TimeOutString);
            }
        }

        public override bool CanSendMsg
        {
            get
            {
                return true;
            }
        }

        public override string TimeOutString
        {
            get
            {
                return "1小时";
            }
        }

        public override string StepCode
        {
            get { return "2"; }
        }

        public override string StepName
        {
            get { return "采集"; }
        }


        public override bool HasNotDoneAction()
        {
            return BaseSampMain.CollectionFlag != 1;
        }

        public override bool ShouldShowSimpleSearchPanel
        {
            get
            {
                return true;
            }
        }
    }
}
