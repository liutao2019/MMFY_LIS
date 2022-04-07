using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 接口返回数据
    /// </summary>
    [Serializable]
    public class EntityInterfaceData : EntityBase
    {
        public EntityInterfaceData(string strInterfaceID)
        {
            InterfaceID = strInterfaceID;
            InterfaceData = new DataSet();
        }

        /// <summary>
        /// 接口数据
        /// </summary>
        public DataSet InterfaceData { get; set; }

        /// <summary>
        /// 接口ID
        /// </summary>
        public String InterfaceID { get; set; }


        /// <summary>
        /// 接口返回信息
        /// </summary>
        public EntityInterfaceResponse InterfaceResponse { get; set; }
    }
}
