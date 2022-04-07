using System;
using System.Collections.Generic;

using System.Text;

namespace dcl.client.sample
{
    /// <summary>
    /// 流程工厂
    /// </summary>
    public class StepFactory
    {
        /// <summary>
        /// 生成Step类
        /// </summary>
        /// <param name="stepType"></param>
        /// <returns></returns>
        public static IStep CreateStep(StepType stepType)
        {
            switch (stepType)
            {
                case StepType.Advice:
                    return new AdviceStep();
                case StepType.Send:
                    return new SendStep();
                case StepType.Reach:
                    return new ReachStep();
                case StepType.Confirm:
                    return new ReceiveStep();
                case StepType.Print:
                    return new PrintStep();
                case StepType.Sampling:
                    return new SamplingStep();
                case StepType.SecondSend:
                    return new SecondSendStep();
                case StepType.Select:
                    return new SelectStep();
                case StepType.Centrifugate:
                    return new CentrifugateStep();
                case StepType.InLab:
                    return new InLabStep();
                case StepType.Ren:
                    return new RenStep();
                case StepType.Hstq:
                    return new HSTQStep();
                case StepType.Hsdlkz:
                    return new HSDLKZStep();
                case StepType.HandOver:
                    return new HandOverStep();
                default:
                    return null;
            }
        }

        /// <summary>
        /// 根据流程,生成当前步骤的前一个步骤
        /// </summary>
        /// <param name="currentStep">当前步骤</param>
        /// <returns></returns>
        public static IStep CreatePrevStep(IStep currentStep)
        {
            if (StepList == null || StepList.Count <= 0)
                return null;

            int index = -1;

            for (int i = 0; i < StepList.Count; i++)
            {
                if (StepList[i].GetType() == currentStep.GetType())
                {
                    index = i;
                    break;
                }
            }

            if (index <= 0)//找不到或是第一个
                return null;
            else
                return StepList[index - 1];
        }

        /// <summary>
        /// 流程列表
        /// </summary>
        public static List<IStep> StepList { get; set; }


        /// <summary>
        /// 根据流程,生成当前步骤的前一个步骤
        /// </summary>
        /// <param name="currentStep">当前步骤</param>
        /// <returns></returns>
        public static IStep CreateNextStep(IStep currentStep)
        {
            if (StepList == null || StepList.Count <= 0)
                return null;

            int index = -1;

            for (int i = 0; i < StepList.Count; i++)
            {
                if (StepList[i].GetType() == currentStep.GetType())
                {
                    index = i;
                    break;
                }
            }

            if (index < 0 || index >= StepList.Count)//找不到或是第一个
                return null;
            else
                return StepList[index + 1];

        }
    }

    /// <summary>
    /// 流程Eumn类,用来辅助UserControl选择当前的流程
    /// </summary>
    public enum StepType
    {
        /// <summary>
        /// 采样
        /// </summary>
        Sampling,

        /// <summary>
        /// 送检
        /// </summary>
        Send,

        /// <summary>
        /// 送达
        /// </summary>
        Reach,

        /// <summary>
        /// 签收
        /// </summary>
        Confirm,

        /// <summary>
        /// 离心
        /// </summary>
        Centrifugate,

        /// <summary>
        /// 标本上机
        /// </summary>
        InLab,
        /// <summary>
        /// 耗材领取
        /// </summary>
        Ren,
        /// <summary>
        /// 核酸提取
        /// </summary>
        Hstq,
        /// <summary>
        /// 核酸定量扩增
        /// </summary>
        Hsdlkz,

        /// <summary>
        /// 打印
        /// </summary>
        Print,

        /// <summary>
        /// 
        /// </summary>
        Advice,

        /// <summary>
        /// 二次送检
        /// </summary>
        SecondSend,


        /// <summary>
        /// 查询
        /// </summary>
        Select,

        /// <summary>
        /// 标本交接
        /// </summary>
        HandOver,
    }

    /// <summary>
    /// 流程Eumn类,用来辅助UserControl排序
    /// </summary>
    public enum  SelectType
    {
        /// <summary>
        /// 生成
        /// </summary>
        Create,

        /// <summary>
        /// 采集
        /// </summary>
        Sampling,

        /// <summary>
        /// 收取
        /// </summary>
        Confirm,

        /// <summary>
        /// 送达
        /// </summary>
        Send,

        /// <summary>
        /// 签收
        /// </summary>
        Reach,

        /// <summary>
        /// 二次送检
        /// </summary>
        SecondSend,

        /// <summary>
        /// 离心
        /// </summary>
        Centrifugate,

        /// <summary>
        /// 标本上机
        /// </summary>
        InLab
    }
}
