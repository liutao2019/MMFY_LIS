using System;
using System.Collections.Generic;

using System.Text;
using dcl.client.frame.runsetting;

namespace dcl.client.result.Interface
{
    public interface IPatPanelConfig
    {
        void ApplySetting(PatInputRuntimeSetting setting);
        string Text { get; set; }

    }
}
