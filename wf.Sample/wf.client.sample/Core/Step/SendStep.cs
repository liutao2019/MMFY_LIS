namespace dcl.client.sample
{
    /// <summary>
    /// 收取流程
    /// </summary>
    public class SendStep : IStep
    {
        public static string Name = "收取";

        public override string StepCode
        {
            get { return "3"; }
        }

        public override string StepName
        {
            get { return Name; }
        }

        public override bool HasNotDoneAction()
        {
            return BaseSampMain.SendFlag != 1;
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