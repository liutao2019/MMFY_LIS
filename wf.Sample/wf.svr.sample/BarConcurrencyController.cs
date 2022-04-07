using dcl.entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.sample
{
    /// <summary>
    /// 条码并发控制
    /// </summary>
    internal class BarConcurrencyController
    {
        private static object SynObj = new object();

        private static Dictionary<string, string> Keys = new Dictionary<string, string>();

        public static KeyResult InsertKey(EntityInterfaceExtParameter barDownloadInfo)
        {

            lock (SynObj)
            {
                string key = GetKey(barDownloadInfo);
                if (string.IsNullOrEmpty(key))
                {
                    return new KeyResult() { Ok = true };
                }
                if (Keys.Keys.Contains(key))
                {
                    return new KeyResult() { Ok = false, Key = key };
                }
                else
                {
                    Keys.Add(key, barDownloadInfo.GUID);
                    return new KeyResult() { Ok = true, Key = key, GUID = barDownloadInfo.GUID }; ;
                }
            }
        }

        private static string GetKey(EntityInterfaceExtParameter barDownloadInfo)
        {
            string key = string.Empty;
            if (!string.IsNullOrEmpty(barDownloadInfo.DeptID)
                && string.IsNullOrEmpty(barDownloadInfo.PatientID))
            {
                key = string.Format("Dept_{0}_{1}", barDownloadInfo.DownloadType.ToString(), barDownloadInfo.DeptID);
            }
            else if (!string.IsNullOrEmpty(barDownloadInfo.PatientID))
            {
                key = string.Format("PatID_{0}_{1}", barDownloadInfo.DownloadType.ToString(), barDownloadInfo.PatientID);

            }
            return key;
        }

        public static void RemoveKey(EntityInterfaceExtParameter barDownloadInfo)
        {
            lock (SynObj)
            {
                string key = GetKey(barDownloadInfo);
                if (Keys.Keys.Contains(key) && Keys[key] == barDownloadInfo.GUID)
                {
                    Keys.Remove(key);
                }
            }
        }
    }

    internal class KeyResult
    {
        public bool Ok { get; set; }
        public string Key { get; set; }

        public string GUID { get; set; }
    }
}
