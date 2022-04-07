using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.sample
{
    /// <summary>
    /// 标本进程监控BIZ文件
    /// </summary>
    public class SampProcessMonitorBIZ : IDicSampProcessMonitor
    {
        public static List<EntitySampProcessMonitor> listBarcode;

        public List<EntitySampProcessMonitor> GetBCPatients()
        {
            List<EntitySampProcessMonitor> listSampProMonitor = new List<EntitySampProcessMonitor>();
            
            //系统配置:条码监控几分钟为超时
            string valueOutTime = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("BCMonitor_Timeout");

            if (!string.IsNullOrEmpty(valueOutTime))
            {
                if (valueOutTime == "20分钟")
                {
                    valueOutTime = "20";//设置为20分钟
                }
                else
                {
                    valueOutTime = "30";//默认30分钟
                }
            }
            else
            {
                valueOutTime = "30";//默认30分钟
            }

            IDaoSampProcessMonitor dao = DclDaoFactory.DaoHandler<IDaoSampProcessMonitor>();
            if (dao != null)
            {
                listSampProMonitor = dao.GetBCPatients(valueOutTime);
            }

            listBarcode = listSampProMonitor;//给全局变量赋值

            return listSampProMonitor;
        }
        /// <summary>
        /// 统计常规标本监控
        /// </summary>
        /// <param name="OperationCode">操作步骤</param>
        /// <returns></returns>
        public List<EntitySampProcessMonitor> GetSampCount(int OperationCode)
        {
            List<EntitySampProcessMonitor> listSampCount = new List<EntitySampProcessMonitor>();
            IDaoSampProcessMonitor dao = DclDaoFactory.DaoHandler<IDaoSampProcessMonitor>();
            if (dao != null)
            {
                listSampCount = dao.GetSampCount(OperationCode);
            }
            return listSampCount;
        }
    }
}
