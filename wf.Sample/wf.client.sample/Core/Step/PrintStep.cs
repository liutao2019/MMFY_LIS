namespace dcl.client.sample
{
    /// <summary>
    /// 条码打印流程
    /// </summary>
    public class PrintStep : IStep
    {
        public override string StepCode
        {
            get { return "1"; }
        }

        public override string StepName
        {
            get { return "打印"; }
        }

        public override bool HasNotDoneAction()
        {
            return BaseSampMain.SampPrintFlag != 1;
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