using System;
using System.Collections.Generic;
using System.Text;

namespace dcl.hl7
{
    public  class QueueTools :EwellMutiImpl
    {
    
        private MQFunction singleImpl;
        public QueueTools()
        {
            singleImpl = new MQFunction();
          
        }
        public long connectMQ()
        {
            return singleImpl.connectMQ();
        }
        public long connectMQ(string cionid)
        {
            return singleImpl.connectMQ(cionid);
        }
        public void disconnectMQ()
        {
            singleImpl.disconnectMQ();
        }

    

        public long getMsg(string fid, string mid, int waitInterval, ref string msgID, ref string Msg)
        {
            return singleImpl.getMsg(fid, mid, waitInterval, ref msgID,ref Msg);
        }
        public long getMsg2(string strQueueName, int waitInterval, ref string msgID, ref string Msg)
        {
            return singleImpl.getMsg2(strQueueName, waitInterval, ref msgID, ref Msg);
        }
        public long getMsgById(string fid, string mid, string msgID, int waitInterval, ref string Msg)
        {
           return singleImpl.getMsgById(fid, mid, msgID, waitInterval,ref Msg);
        }
        public long putMsg(string fid, string mid, string inmsg, ref string msgid)
        {
            return singleImpl.putMsg(fid, mid, inmsg, ref msgid);
        }
        public long putMsgWithId(string fid, string mid, string inmsg, string inmsgId)
        {
            return singleImpl.putMsgWithId(fid, mid, inmsg, inmsgId);
        }

        public long putMsgWithId2(string strQueueName, string inmsg, string inmsgId)
        {
            return singleImpl.putMsgWithId2(strQueueName, inmsg, inmsgId);
        }

        

        public long browseMsg(string xid, int waitInterval, ref string msgID, ref string Msg)
        {
            return singleImpl.browseMsg(xid, waitInterval, ref msgID, ref Msg);
        }

        public long browseMsgById(string xid, string msgID, int waitInterval, ref string Msg)
        {
            return singleImpl.browseMsgById(xid, msgID, waitInterval, ref Msg);
        }
    }
}
