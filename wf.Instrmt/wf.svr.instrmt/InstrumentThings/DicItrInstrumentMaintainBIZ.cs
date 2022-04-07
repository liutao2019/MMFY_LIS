using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.servececontract;
using dcl.svr.dicbasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.instrmt
{
    public class DicItrInstrumentMaintainBIZ: IDicItrInstrumentMaintain
    {
        public bool AddInstrmtMaintain(EntityDicItrInstrumentMaintain instrmtMaintain)
        {
            IDaoItrInstrumentMaintain dao = DclDaoFactory.DaoHandler<IDaoItrInstrumentMaintain>();
            bool isInsertTrue = false;
            if (dao != null)
            {
                isInsertTrue = dao.AddInstrmtMaintain(instrmtMaintain);
            }
            return isInsertTrue;
        }

        public bool UpdateInstrmtMaintain(EntityDicItrInstrumentMaintain instrmtMaintain)
        {
            IDaoItrInstrumentMaintain dao = DclDaoFactory.DaoHandler<IDaoItrInstrumentMaintain>();
            bool isUpdateTrue = false;
            if (dao != null)
            {
                isUpdateTrue = dao.UpdateInstrmtMaintain(instrmtMaintain);
            }
            return isUpdateTrue;
        }

        public bool DeleteInstrmtMaintainByID(int mai_id)
        {
            IDaoItrInstrumentMaintain dao = DclDaoFactory.DaoHandler<IDaoItrInstrumentMaintain>();
            bool isDeleteTrue = false;
            if (dao != null)
            {
                isDeleteTrue = dao.DeleteInstrmtMaintainByID(mai_id);
            }
            return isDeleteTrue;
        }

        public List<EntityDicItrInstrumentMaintain> GetInstrmtMaintains(string itr_id)
        {
            IDaoItrInstrumentMaintain dao = DclDaoFactory.DaoHandler<IDaoItrInstrumentMaintain>();
            List<EntityDicItrInstrumentMaintain> list = new List<EntityDicItrInstrumentMaintain>();
            if (dao != null)
            {
                list = dao.GetInstrmtMaintains(itr_id);
            }
            return list;
        }

        /// <summary>
        /// 检索组别字典信息
        /// </summary>
        /// <returns></returns>
        public List<EntityDicPubProfession> SearchDicPubProfession()
        {
            List<EntityDicPubProfession> list = new List<EntityDicPubProfession>();

            EntityResponse result = new EntityResponse();

            EntityRequest req = new EntityRequest();
            req.SetRequestValue(new EntityDicPubProfession());

            result=new TypeBIZ().Search(req);
            
            list = result.GetResult() as List<EntityDicPubProfession>;

            return list;
        }

        /// <summary>
        /// 查询组别字典信息数据
        /// </summary>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        public List<EntityDicInstrument> GetInstrmts(string itr_id)
        {
            List<EntityDicInstrument> list = new List<EntityDicInstrument>();

            EntityResponse result = new EntityResponse();

            EntityRequest req = new EntityRequest();
            if(itr_id == String.Empty)
            {
                EntityDicInstrument instr = new EntityDicInstrument();
                req.SetRequestValue(instr);
            }
           
            result=new InstrmtBIZ().Search(req);

            list = result.GetResult() as List<EntityDicInstrument>;
            return list;
        }
    }
}
