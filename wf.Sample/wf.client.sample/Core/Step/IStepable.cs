using System;
using System.Collections.Generic;

using System.Text;

namespace dcl.client.sample
{
    /// <summary>
    /// 打印条码客户端接口
    /// </summary>
    public interface IStepable
    {
        IStep Step { get; set; }
        StepType StepType { get; set; }
    }
}
