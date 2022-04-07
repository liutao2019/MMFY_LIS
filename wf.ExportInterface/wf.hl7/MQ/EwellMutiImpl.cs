using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace dcl.hl7
{
    public class EwellMutiImpl
    {
        public String GateKeeper;
        public String Xid;

        public void run(int ThreadNum)
        {
            for (int i = 0; i < ThreadNum; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(getMsg), i);
            }
        }

        private void getMsg(Object stateInfo)
        {
            MQFunction mq = new MQFunction();

            int i = 0;
            while (true)
            {
                while (mq.qMgr == null)
                {
                    mq.connectMQ(GateKeeper);
                }
                while (!mq.qMgr.IsConnected)
                {
                    mq.qMgr.Disconnect();
                    mq.connectMQ(GateKeeper);
                }


                String a = "", b = "";

                i++;
                mq.getMsg(Xid, 1000, ref a, ref b);


                OnMessage(a, b);
            }



        }


        public virtual object OnMessage(string msgid, string msg)
        {
            //if (msgid!="")
            //{
            //    NewMQFunction mq = new NewMQFunction();
            //    mq.connectMQ(GateKeeper);
            //    String st = "<ESBEntry><AccessControl><UserName>LIS</UserName><Password>LIS</Password><Fid>PS10028</Fid></AccessControl><MessageHeader><Fid>PS10028</Fid><SourceSysCode>S01</SourceSysCode><TargetSysCode>S42</TargetSysCode><MsgDate>2014-05-12 13:11:11</MsgDate></MessageHeader><MsgInfo><Msg>bbbbbbb</Msg></MsgInfo></ESBEntry>";
            //   mq.putMsgWithId(Fid, "0", st, msgid);
            //    mq.disconnectMQ();
            //    string filename = this.GetAssemblyPath()  + "message.txt";

            //    LogHelper Helper = new LogHelper(filename);
            //    Helper.WriteLine(System.DateTime.Now.ToString()+"|:MSGID:"+ msgid+",MSG:"+msg);
            //}


            return msg;
        }
        private string GetAssemblyPath()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            string[] strArray = codeBase.Substring(8, codeBase.Length - 8).Split(new char[] { '/' });
            string str2 = "";
            for (int i = 0; i < (strArray.Length - 1); i++)
            {
                str2 = str2 + strArray[i] + "/";
            }
            return str2;
        }
        public void init(String cionid, string xid)
        {
            GateKeeper = cionid;
            Xid = xid;
        }

    }
}
