using System.Configuration;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using dcl.client.wcf;

namespace dcl.client.frame.runsetting
{
    public class RuntimeSetting
    {
        public static RuntimeSetting NewInstance
        {
            get
            {
                string _endpointConfigurationName = "runtimesetting";
                string _remoteAddress = ConfigurationManager.AppSettings["wcfAddr"];
                string remoteAddress = _remoteAddress + ConfigurationManager.AppSettings["svc.RuntimeSetting"];


                RuntimeSettingClient serviceClient = new RuntimeSettingClient(_endpointConfigurationName, remoteAddress);

                RuntimeSetting setting = new RuntimeSetting(serviceClient);

                return setting;
            }
        }

        private RuntimeSettingClient ServiceClient { get; set; }

        private RuntimeSetting(RuntimeSettingClient service)
        {
            this.ServiceClient = service;
        }

        private string GetFullKey(string prefix, string type, string key)
        {
            string fullkey = string.Format("{0}-{1}-{2}", prefix, type, key);
            return fullkey;
        }
        ProxyRuntimeSetting proxy = new ProxyRuntimeSetting();
        private void Save<Setting>(string key, string prefix, Setting setting) where Setting : RuntimeSettingBase
        {
            string type = typeof(Setting).FullName;
            string fullkey = GetFullKey(prefix, type, key);
            byte[] data = null;

            if (setting != null)
            {
                data = Serialize<Setting>(setting);
            }
            proxy.Service.Save(fullkey, data);
        }

        /// <summary>
        /// 保存用户配置
        /// </summary>
        /// <typeparam name="Setting">配置类，请确保可序列化</typeparam>
        /// <param name="key">键值</param>
        /// <param name="setting">配置实体</param>
        public void SaveUser<Setting>(string key, Setting setting) where Setting : RuntimeSettingBase
        {
            Save<Setting>(key, prefixUser, setting);
        }

        /// <summary>
        /// 保存组配置
        /// </summary>
        /// <typeparam name="Setting">配置类，请确保可序列化</typeparam>
        /// <param name="key">键值</param>
        /// <param name="setting">配置实体</param>
        public void SaveGroup<Setting>(string key, Setting setting) where Setting : RuntimeSettingBase
        {
            Save<Setting>(key, prefixGroup, setting);
        }

        /// <summary>
        /// 保存系统配置
        /// </summary>
        /// <typeparam name="Setting">配置类，请确保可序列化</typeparam>
        /// <param name="setting">配置实体</param>
        public void SaveSystem<Setting>(Setting setting) where Setting : RuntimeSettingBase
        {
            Save<Setting>(string.Empty, prefixSystem, setting);
        }


        /// <summary>
        /// 加载用户配置
        /// </summary>
        /// <typeparam name="Setting">配置类，请确保可序列化</typeparam>
        /// <param name="key">键值</param>
        /// <returns>配置实体</returns>
        public Setting LoadUser<Setting>(string key) where Setting : RuntimeSettingBase
        {
            return Load<Setting>(prefixUser, key);
        }

        /// <summary>
        /// 加载组配置
        /// </summary>
        /// <typeparam name="Setting">配置类，请确保可序列化</typeparam>
        /// <param name="key">键值</param>
        /// <returns>配置实体</returns>
        public Setting LoadGroup<Setting>(string key) where Setting : RuntimeSettingBase
        {
            return Load<Setting>(prefixGroup, key);
        }


        private Setting Load<Setting>(string prefix, string key) where Setting : RuntimeSettingBase
        {
            string type = typeof(Setting).FullName;
            string fullkey = string.Format("{0}-{1}-{2}",prefix, type , key);
            byte[] data = proxy.Service.Load(fullkey);
            Setting setting = Deserialize<Setting>(data);
            return setting;
        }

        /// <summary>
        /// 删除用户配置
        /// </summary>
        /// <param name="userID"></param>
        public void DeleteUserSetting<Setting>(string userID) where Setting : RuntimeSettingBase
        {
            DeleteSetting<Setting>(userID, prefixUser);
        }

        /// <summary>
        /// 删除组配置
        /// </summary>
        public void DeleteGroupSetting<Setting>(string groupKey) where Setting : RuntimeSettingBase
        {
            DeleteSetting<Setting>(groupKey, prefixGroup);
        }

        /// <summary>
        /// 删除系统配置
        /// </summary>
        public void DeleteSystemSetting<Setting>() where Setting : RuntimeSettingBase
        {
            DeleteSetting<Setting>(string.Empty, prefixSystem);
        }


        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="key"></param>
        private void DeleteSetting<Setting>(string key,string prefix) where Setting : RuntimeSettingBase
        {
            string fullKey = GetFullKey(prefix, typeof(Setting).FullName, key);
            proxy.Service.Delete(fullKey);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="Setting"></typeparam>
        /// <param name="setting"></param>
        /// <returns></returns>
        private byte[] Serialize<Setting>(Setting setting) where Setting : RuntimeSettingBase
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, setting);

            byte[] buff = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(buff, 0, (int)stream.Length);
            stream.Close();
            return buff;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="Setting"></typeparam>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static Setting Deserialize<Setting>(byte[] buff) where Setting : RuntimeSettingBase
        {
            if (buff == null)
            {
                return null;
            }

            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(buff);

            try
            {
                Setting entity = formatter.Deserialize(stream) as Setting;
                stream.Close();
                return entity;
            }
            catch
            {
                stream.Close();
                return null;
            }
        }


        //标识
        private const string prefixUser = "User";
        private const string prefixGroup = "Group";
        private const string prefixSystem = "System";

    }


}
