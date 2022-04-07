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
    public class DataInterfaceCommandBIZ : IDicDataInterfaceCommand
    {
        public bool DeleteDicDataInterCommandAndParm(string cmdID)
        {
            bool isDelete = false;
            IDaoDicDataInterfaceCommand dao = DclDaoFactory.DaoHandler<IDaoDicDataInterfaceCommand>();
            if (dao != null)
            {
                if (dao.DeleteDicDataInterfaceCommand(cmdID)) //删除主表
                {
                    EntityDicDataInterfaceCommandParameter entityParm = new EntityDicDataInterfaceCommandParameter();
                    entityParm.CmdId = cmdID;
                    isDelete = new DataInterfaceCommandParameterBIZ().DeleteDicDataInterfaceCommandParameter(entityParm); //删除明细
                }
            }
            return isDelete;
        }

        public List<EntityDicDataInterfaceCommandParameter> GetParametersByCmdID(string cmdID)
        {
            List<EntityDicDataInterfaceCommandParameter> listInterParam = new List<EntityDicDataInterfaceCommandParameter>();
            if (!string.IsNullOrEmpty(cmdID))
            {
                EntityDicDataInterfaceCommandParameter entityParam = new EntityDicDataInterfaceCommandParameter();
                entityParam.CmdId = cmdID;
                listInterParam = new DataInterfaceCommandParameterBIZ().SearchDicDataInterfaceCommandParameter(entityParam);
            }
            return listInterParam;
        }

        public bool SaveDicDataInterCommandAndParm(EntityRequest request)
        {
            bool isSave = false;
            try
            {
                List<Object> listAll = request.GetRequestValue() as List<Object>;
                EntityDicDataInterfaceCommand dataComd = listAll[0] as EntityDicDataInterfaceCommand;
                List<EntityDicDataInterfaceCommandParameter> listInterParm = listAll[1] as List<EntityDicDataInterfaceCommandParameter>;

                IDaoDicDataInterfaceCommand dao = DclDaoFactory.DaoHandler<IDaoDicDataInterfaceCommand>();
                if (dao != null)
                {
                    if (dao.SaveDicDataInterfaceCommand(dataComd)) //保存主表数据
                    {
                        foreach (var info in listInterParm)
                        {
                            info.CmdId = dataComd.CmdId;
                        }

                        EntityDicDataInterfaceCommandParameter eytityParm = new EntityDicDataInterfaceCommandParameter();
                        eytityParm.CmdId = dataComd.CmdId;
                        new DataInterfaceCommandParameterBIZ().DeleteDicDataInterfaceCommandParameter(eytityParm); //先删除明细

                        foreach (var infoParm in listInterParm)
                        {
                            new DataInterfaceCommandParameterBIZ().SaveDicDataInterfaceCommandParameter(infoParm); //后保存明细
                        }
                        isSave = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return isSave;
        }

        public List<EntityDicDataInterfaceCommand> SearchDicDataInterfaceCommand(EntityDicDataInterfaceCommand interCommand)
        {
            List<EntityDicDataInterfaceCommand> listDataInterConn = new List<EntityDicDataInterfaceCommand>();
            IDaoDicDataInterfaceCommand dao = DclDaoFactory.DaoHandler<IDaoDicDataInterfaceCommand>();
            if (dao != null)
            {
                listDataInterConn = dao.SearchDicDataInterfaceCommand(interCommand);
            }
            return listDataInterConn;
        }

        public bool UpdateDicDataInterCommandAndParm(EntityRequest request)
        {
            bool isUpdate = false;
            try
            {
                List<Object> listAll = request.GetRequestValue() as List<Object>;
                EntityDicDataInterfaceCommand dataComd = listAll[0] as EntityDicDataInterfaceCommand;
                List<EntityDicDataInterfaceCommandParameter> listInterParm = listAll[1] as List<EntityDicDataInterfaceCommandParameter>;

                IDaoDicDataInterfaceCommand dao = DclDaoFactory.DaoHandler<IDaoDicDataInterfaceCommand>();
                if (dao != null)
                {
                    if (dao.UpdateDicDataInterfaceCommand(dataComd)) //更新主表数据
                    {
                        foreach (var info in listInterParm)
                        {
                            info.CmdId = dataComd.CmdId;
                        }

                        EntityDicDataInterfaceCommandParameter eytityParm = new EntityDicDataInterfaceCommandParameter();
                        eytityParm.CmdId = dataComd.CmdId;
                        new DataInterfaceCommandParameterBIZ().DeleteDicDataInterfaceCommandParameter(eytityParm); //先删除明细

                        foreach (var infoParm in listInterParm)
                        {
                            new DataInterfaceCommandParameterBIZ().SaveDicDataInterfaceCommandParameter(infoParm); //后保存明细
                        }
                        isUpdate = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return isUpdate;
        }
    }
}
