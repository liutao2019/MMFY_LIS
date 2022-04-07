using System;
using dcl.entity;

namespace dcl.client.sample.Core.Step
{
    /// <summary>
    /// 标本登记步骤
    /// </summary>
    public class SampleRegisterStep : IStep
    {
        public override string StepCode
        {
            get { return EnumBarcodeOperationCode.SampleRegister.ToString(); }
        }

        public override string StepName
        {
            get { return "标本登记"; }
        }

        public override bool HasNotDoneAction()
        {
            return Convert.ToInt32(BaseSampMain.SampStatusId) < 20;
        }
    }
}
