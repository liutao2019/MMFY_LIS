using System;
using System.Collections.Generic;

using System.Text;
using dcl.client.frame.runsetting;

namespace dcl.client.frame.runsetting.Lab
{
    [Serializable]
    public class PatEnterLayoutControlSetting : RuntimeSettingBase
    {
        public byte[] LayoutData { get; set; }

        /// <summary>
        /// 保存组别设置
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="moduleName"></param>
        /// <param name="groupKey"></param>
        public static void SaveGroup(PatEnterLayoutControlSetting setting, string moduleName, string groupKey)
        {
            string key = moduleName + "_" + groupKey;
            RuntimeSetting.NewInstance.SaveGroup<PatEnterLayoutControlSetting>(key, setting);
        }

        /// <summary>
        /// 加载组别设置
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public static PatEnterLayoutControlSetting LoadGroup(string moduleName, string groupKey)
        {
            string key = moduleName + "_" + groupKey;
            PatEnterLayoutControlSetting setting = RuntimeSetting.NewInstance.LoadGroup<PatEnterLayoutControlSetting>(key);
            return setting;
        }

    }
}
