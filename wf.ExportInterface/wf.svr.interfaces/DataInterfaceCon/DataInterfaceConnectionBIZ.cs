using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;

namespace dcl.svr.interfaces
{
    public class DataInterfaceConnectionBIZ : IDicDataInterfaceConnection
    {
        public bool DeleteDataInterfaceConnection(string connId)
        {
            bool isDelete = false;
            IDaoDicDataInterfaceConnection dao = DclDaoFactory.DaoHandler<IDaoDicDataInterfaceConnection>();
            if (dao != null)
            {
                isDelete = dao.DeleteDataInterfaceConnection(connId);
            }
            return isDelete;
        }

        public bool SaveDataInterfaceConnection(EntityDicDataInterfaceConnection dtInterCon)
        {
            bool isSave = false;
            IDaoDicDataInterfaceConnection dao = DclDaoFactory.DaoHandler<IDaoDicDataInterfaceConnection>();
            if (dao != null)
            {
                isSave = dao.SaveDataInterfaceConnection(dtInterCon);
            }
            return isSave;
        }

        public List<EntityDicDataInterfaceConnection> SearchDataInterfaceConnection(EntityDicDataInterfaceConnection dtInterCon)
        {
            List<EntityDicDataInterfaceConnection> listDataInterConn = new List<EntityDicDataInterfaceConnection>();
            IDaoDicDataInterfaceConnection dao = DclDaoFactory.DaoHandler<IDaoDicDataInterfaceConnection>();
            if (dao != null)
            {
                listDataInterConn = dao.SearchDataInterfaceConnection(dtInterCon);
            }
            return listDataInterConn;
        }

        public string TestConnection(EntityDicDataInterfaceConnection dtInterCon)
        {
            string msg = string.Empty;
            //待修改
            //if (dtInterCon != null)
            //{
            //    //测试接口连接
            //    if(dtInterCon.ConnRunningSide=="Server"|| dtInterCon.ConnRunningSide=="Client")
            //    {
            //        DataInterfaceConnectionNew conn = DataInterfaceConnectionNew.FromDTO(dtInterCon);
            //        conn.TestConnection(out msg);
            //    }
            //    else
            //    {
            //        throw new NotSupportedException("未指定执行端");
            //    }
            //}
            return msg;
        }

        public bool UpdateDataInterfaceConnection(EntityDicDataInterfaceConnection dtInterCon)
        {
            bool isUpdate = false;
            IDaoDicDataInterfaceConnection dao = DclDaoFactory.DaoHandler<IDaoDicDataInterfaceConnection>();
            if (dao != null)
            {
                isUpdate = dao.UpdateDataInterfaceConnection(dtInterCon);
            }
            return isUpdate;
        }
    }
}
