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
    public class DataInterfaceCommandParameterBIZ : IDicDataInterfaceCommandParameter
    {
        public bool DeleteDicDataInterfaceCommandParameter(EntityDicDataInterfaceCommandParameter cmdParam)
        {
            bool isDelete = false;
            IDaoDicDataInterfaceCommandParameter dao = DclDaoFactory.DaoHandler<IDaoDicDataInterfaceCommandParameter>();
            if (dao != null)
            {
                isDelete = dao.DeleteDicDataInterfaceCommandParameter(cmdParam);
            }
            return isDelete;
        }

        public bool SaveDicDataInterfaceCommandParameter(EntityDicDataInterfaceCommandParameter cmdParam)
        {
            bool isSave = false;
            IDaoDicDataInterfaceCommandParameter dao = DclDaoFactory.DaoHandler<IDaoDicDataInterfaceCommandParameter>();
            if (dao != null)
            {
                isSave = dao.SaveDicDataInterfaceCommandParameter(cmdParam);
            }
            return isSave;
        }

        public List<EntityDicDataInterfaceCommandParameter> SearchDicDataInterfaceCommandParameter(EntityDicDataInterfaceCommandParameter cmdParam)
        {
            List<EntityDicDataInterfaceCommandParameter> listInterCommand = new List<EntityDicDataInterfaceCommandParameter>();
            IDaoDicDataInterfaceCommandParameter dao = DclDaoFactory.DaoHandler<IDaoDicDataInterfaceCommandParameter>();
            if (dao != null)
            {
                listInterCommand = dao.SearchDicDataInterfaceCommandParameter(cmdParam);
            }
            return listInterCommand;
        }

        public bool UpdateDicDataInterfaceCommandParameter(EntityDicDataInterfaceCommandParameter cmdParam)
        {
            bool isUpdate = false;
            IDaoDicDataInterfaceCommandParameter dao = DclDaoFactory.DaoHandler<IDaoDicDataInterfaceCommandParameter>();
            if (dao != null)
            {
                isUpdate = dao.UpdateDicDataInterfaceCommandParameter(cmdParam);
            }
            return isUpdate;
        }
    }
}
