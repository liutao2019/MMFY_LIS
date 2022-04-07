using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.msg
{
    /// <summary>
    /// 组合TAT数据
    /// </summary>
    public class CombineTATMsgBIZ : IDicCombineTATMsg
    {
        public List<EntityDicMsgTAT> GetBcComLabTATMessage()
        {
            return dcl.svr.msg.CombineTATMsgCache.Current.cacheBcLab;
        }

        public List<EntityDicMsgTAT> GetBcComLabTAtMsgToCacheDao()
        {
            List<EntityDicMsgTAT> listMsgLabTAT = new List<EntityDicMsgTAT>();
            IDaoCombineTATMsg dao = DclDaoFactory.DaoHandler<IDaoCombineTATMsg>();
            if (dao != null)
            {
                listMsgLabTAT = dao.GetBcComLabTAtMsgToCacheDao();
            }
            return listMsgLabTAT;
        }

        public List<EntityDicMsgTAT> GetBcComTATMessage()
        {
            return dcl.svr.msg.CombineTATMsgCache.Current.cacheBc;
        }

        public List<EntityDicMsgTAT> GetBcComTAtMsgToCacheDao()
        {
            List<EntityDicMsgTAT> listMsgTAT = new List<EntityDicMsgTAT>();
            IDaoCombineTATMsg dao = DclDaoFactory.DaoHandler<IDaoCombineTATMsg>();
            if(dao!=null)
            {
                listMsgTAT = dao.GetBcComTAtMsgToCacheDao();
            }
            return listMsgTAT;
        }

        public List<EntityDicMsgTAT> GetComTATMessage()
        {
            return dcl.svr.msg.CombineTATMsgCache.Current.cache;
        }

        public List<EntityDicMsgTAT> GetCombineTATmsgToCacheDao(bool isOutLink)
        {
            List<EntityDicMsgTAT> listComTATMsg = new List<EntityDicMsgTAT>();
            IDaoCombineTATMsg dao = DclDaoFactory.DaoHandler<IDaoCombineTATMsg>();
            if (dao != null)
            {
                listComTATMsg = dao.GetCombineTATmsgToCacheDao(isOutLink);
            }
            return listComTATMsg;
        }

        public List<EntityDicMsgTAT> GetBcBcSamplToReceiveTATMessage()
        {
            return dcl.svr.msg.CombineTATMsgCache.Current.cacheBcSamplToReceive;
        }

        public List<EntityDicMsgTAT> GetBcSamplToReceiveTAtMsgToCacheDao()
        {
            List<EntityDicMsgTAT> listBcSamToReceive = new List<EntityDicMsgTAT>();
            IDaoCombineTATMsg dao = DclDaoFactory.DaoHandler<IDaoCombineTATMsg>();
            if (dao != null)
            {
                listBcSamToReceive = dao.GetBcSamplToReceiveTAtMsgToCacheDao();
            }
            return listBcSamToReceive;
        }
    }
}
