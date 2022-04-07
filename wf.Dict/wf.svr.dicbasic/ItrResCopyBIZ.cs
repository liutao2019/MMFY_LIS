using System;
using System.Collections.Generic;
using dcl.common;
using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;

namespace dcl.svr.dicbasic
{
    public class ItrResCopyBIZ : IDicCommon
    {
        #region ICommonBIZ 成员

        public EntityResponse New(EntityRequest request)
        {
            throw new NotImplementedException();
        }

        public EntityResponse Delete(EntityRequest request)
        {
            throw new NotImplementedException();
        }

        public EntityResponse Update(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            EntityDicInstrument type = request.GetRequestValue<EntityDicInstrument>();
            IDaoDic<EntityDicResAdjust> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicResAdjust>>();
            List<string> obj = new List<string>();
            obj.Add("ResAdjust");
            obj.Add(type.ItrOrgId);
            obj.Add(type.ItrId);
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(obj));
            }
            List<EntityDicResAdjust> dtOri = response.GetResult() as List<EntityDicResAdjust>;

            if (dtOri.Count > 0)
            {
                foreach (EntityDicResAdjust dr in dtOri)
                {
                    dr.ItrId = type.ItrId;
                    response.Scusess = dao.Save(dr);
                }
            }
            return response;
        }

        public EntityResponse Search(EntityRequest request)
        {
            throw new NotImplementedException();
        }

        public EntityResponse Other(EntityRequest request)
        {
            EntityResponse response = new EntityResponse();

            EntityDicInstrument type = request.GetRequestValue<EntityDicInstrument>();
            IDaoDic<EntityDicMachineCode> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMachineCode>>();
            List<object> obj = new List<object>();
            obj.Add(type.ItrOrgId);
            obj.Add(type.ItrId);
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(obj));
            }
            List<EntityDicMachineCode> dtOri = response.GetResult() as List<EntityDicMachineCode>;

            if (dtOri.Count > 0)
            {
                foreach (EntityDicMachineCode dr in dtOri)
                {
                    dr.ItrId = type.ItrId;
                    response.Scusess=dao.Save(dr);
                }
            }
            return response;
        }

        public EntityResponse View(EntityRequest request)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
