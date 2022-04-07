using System;
using System.Collections.Generic;
using System.Text;
using IBM.WMQ;
using System.Xml;
using System.Reflection;
using Lib.LogManager;

namespace dcl.hl7
{
    public class MQFunction
    {
        //private int openOptions;
        private int pOpenOptions;
        private int gOpenOptions;
        private int rOpenOptions;
        public MQQueueManager qMgr;
        public MQQueue system_default_local_queue;
        public string Strcionid;

        public MQFunction()
        {
            this.qMgr = null;
            this.system_default_local_queue = null;
            //this.openOptions = 17;
            //发送消息时连接队列的设置
            this.pOpenOptions = MQC.MQOO_FAIL_IF_QUIESCING | MQC.MQOO_OUTPUT;
            //获取消息时连接队列的设置
            this.gOpenOptions = MQC.MQOO_FAIL_IF_QUIESCING | MQC.MQOO_INPUT_AS_Q_DEF;
            //读取消息时连接队列的设置
            this.rOpenOptions = MQC.MQOO_FAIL_IF_QUIESCING | MQC.MQOO_BROWSE;

        }
        public long connectMQ()
        {
            Strcionid = "cion";
            string filename = this.GetAssemblyPath() + "ESBMQClientProperty.xml";
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            MQEnvironment.Hostname = document.SelectSingleNode("root/" + Strcionid + "/hostname").Attributes["connectionString"].Value.ToString();
            MQEnvironment.Channel = document.SelectSingleNode("root/" + Strcionid + "/channel").Attributes["connectionString"].Value.ToString();
            MQEnvironment.Port = int.Parse(document.SelectSingleNode("root/" + Strcionid + "/port").Attributes["connectionString"].Value.ToString());

            try
            {
                this.qMgr = new MQQueueManager(document.SelectSingleNode("root/" + Strcionid + "/qManager").Attributes["connectionString"].Value.ToString());
                return 1;

            }

            catch (MQException exception)
            {
                if (this.qMgr != null)
                {

                    this.qMgr.Disconnect();
                }

                Logger.LogInfo(System.DateTime.Now.ToString() + "|ERROR:" + exception.Message);

                return exception.Reason;
            }
        }
        public long connectMQ(string cionid)
        {
            Strcionid = cionid;
            string filename = this.GetAssemblyPath() + "ESBMQClientProperty.xml";
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            MQEnvironment.Hostname = document.SelectSingleNode("root/" + Strcionid + "/hostname").Attributes["connectionString"].Value.ToString();
            MQEnvironment.Channel = document.SelectSingleNode("root/" + Strcionid + "/channel").Attributes["connectionString"].Value.ToString();
            MQEnvironment.Port = int.Parse(document.SelectSingleNode("root/" + Strcionid + "/port").Attributes["connectionString"].Value.ToString());
            try
            {
                this.qMgr = new MQQueueManager(document.SelectSingleNode("root/" + Strcionid + "/qManager").Attributes["connectionString"].Value.ToString());
                return 1;

            }

            catch (MQException exception)
            {
                if (this.qMgr != null)
                {

                    this.qMgr.Disconnect();
                }
                Logger.LogInfo(System.DateTime.Now.ToString() + "|ERROR:" + exception.Message);

                return exception.Reason;
            }
        }



        public void disconnectMQ()
        {
            if (this.qMgr != null)
            {
                this.qMgr.Disconnect();
            }
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

        public long getMsg(string fid, string mid, int waitInterval, ref string msgID, ref string Msg)
        {
            try
            {

                string filename = this.GetAssemblyPath() + "ESBMQClientProperty.xml";
                XmlDocument document = new XmlDocument();
                document.Load(filename);
                string str2 = fid + "_" + mid;
                this.system_default_local_queue = this.qMgr.AccessQueue(document.SelectSingleNode("root/" + Strcionid + "/" + str2).Attributes["connectionString"].Value.ToString(), this.gOpenOptions);
                MQMessage message = new MQMessage();
                MQGetMessageOptions gmo = new MQGetMessageOptions
                {
                    WaitInterval = waitInterval,
                    Options = 1
                };
                this.system_default_local_queue.Get(message, gmo);
                msgID = BitConverter.ToString(message.MessageId).Replace("-", "");
                message.CharacterSet = 1386;
                message.Format = "MQSTR";
                Msg = message.ReadString(message.MessageLength);
                return 1;

            }
            catch (MQException exception)
            {
                Logger.LogInfo(System.DateTime.Now.ToString() + "|ERROR:" + exception.Message);
                return exception.Reason;
            }
            finally
            {
                try
                {
                    if (system_default_local_queue != null)
                    {
                        system_default_local_queue.Close();
                    }
                }
                catch { }
            }
        }
        public long getMsg(string xid, int waitInterval, ref string msgID, ref string Msg)
        {
            try
            {

                string filename = this.GetAssemblyPath() + "ESBMQClientProperty.xml";
                XmlDocument document = new XmlDocument();
                document.Load(filename);
                string str2 = xid;
                this.system_default_local_queue = this.qMgr.AccessQueue(document.SelectSingleNode("root/" + Strcionid + "/" + str2).Attributes["connectionString"].Value.ToString(), this.gOpenOptions);
                MQMessage message = new MQMessage();
                MQGetMessageOptions gmo = new MQGetMessageOptions
                {
                    WaitInterval = waitInterval,
                    Options = 1
                };
                this.system_default_local_queue.Get(message, gmo);
                msgID = BitConverter.ToString(message.MessageId).Replace("-", "");
                message.CharacterSet = 1386;
                message.Format = "MQSTR";
                Msg = message.ReadString(message.MessageLength);
                return 1;

            }
            catch (MQException exception)
            {
                Logger.LogInfo(System.DateTime.Now.ToString() + "|ERROR:" + exception.Message);
                return exception.Reason;
            }
            finally
            {
                try
                {
                    if (system_default_local_queue != null)
                    {
                        system_default_local_queue.Close();
                    }
                }
                catch { }
            }
        }

        public long getMsg2(string strQueueName, int waitInterval, ref string msgID, ref string Msg)
        {
            try
            {
                string filename = this.GetAssemblyPath() + "ESBMQClientProperty.xml";
                XmlDocument document = new XmlDocument();
                document.Load(filename);
                this.system_default_local_queue = this.qMgr.AccessQueue(strQueueName, this.gOpenOptions);
                MQMessage message = new MQMessage();
                MQGetMessageOptions gmo = new MQGetMessageOptions
                {
                    WaitInterval = waitInterval,
                    Options = 1
                };
                this.system_default_local_queue.Get(message, gmo);
                msgID = BitConverter.ToString(message.MessageId).Replace("-", "");
                message.CharacterSet = 1386;
                message.Format = "MQSTR";
                Msg = message.ReadString(message.MessageLength);
                return 1;

            }
            catch (MQException exception)
            {
                Logger.LogInfo(System.DateTime.Now.ToString() + "|ERROR:" + exception.Message);
                return exception.Reason;
            }
            finally
            {
                try
                {
                    if (system_default_local_queue != null)
                    {
                        system_default_local_queue.Close();
                    }
                }
                catch { }
            }
        }

        public long getMsgById(string fid, string mid, string msgID, int waitInterval, ref string Msg)
        {
            try
            {
                string filename = this.GetAssemblyPath() + "ESBMQClientProperty.xml";
                XmlDocument document = new XmlDocument();
                document.Load(filename);
                string str2 = fid + "_" + mid;
                this.system_default_local_queue = this.qMgr.AccessQueue(document.SelectSingleNode("root/" + Strcionid + "/" + str2).Attributes["connectionString"].Value.ToString(), this.gOpenOptions);
                MQMessage message = new MQMessage
                {
                    CorrelationId = ConvertHexToBytes(msgID)
                };
                MQGetMessageOptions gmo = new MQGetMessageOptions
                {
                    WaitInterval = waitInterval,
                    Options = 1
                };
                this.system_default_local_queue.Get(message, gmo);

                message.CharacterSet = 1386;
                message.Format = "MQSTR";
                Msg = message.ReadString(message.MessageLength);
                return 1;
            }
            catch (MQException exception)
            {
                Logger.LogInfo(System.DateTime.Now.ToString() + "|ERROR:" + exception.Message);
                return exception.Reason;
            }
            finally
            {
                try
                {
                    if (system_default_local_queue != null)
                    {
                        system_default_local_queue.Close();
                    }
                }
                catch { }
            }
        }
        public long putMsg(string fid, string mid, string inmsg, ref string msgid)
        {
            try
            {
                string filename = this.GetAssemblyPath() + "ESBMQClientProperty.xml";
                XmlDocument document = new XmlDocument();
                document.Load(filename);
                string str2 = fid + "_" + mid;
                this.system_default_local_queue = this.qMgr.AccessQueue(document.SelectSingleNode("root/" + Strcionid + "/" + str2).Attributes["connectionString"].Value.ToString(), this.pOpenOptions);
                MQMessage message = new MQMessage
                {
                    CharacterSet = 1386,
                    Format = "MQSTR"
                };
                int length = Encoding.Default.GetBytes(inmsg).Length;
                message.Write(Encoding.Default.GetBytes(inmsg), 0, length);
                MQPutMessageOptions pmo = new MQPutMessageOptions();
                this.system_default_local_queue.Put(message, pmo);
                msgid = BitConverter.ToString(message.MessageId).Replace("-", "");
                return 1;
            }
            catch (MQException exception)
            {
                Logger.LogInfo(System.DateTime.Now.ToString() + "|ERROR:" + exception.Message);
                return exception.Reason;
            }
            finally
            {
                try
                {
                    if (system_default_local_queue != null)
                    {
                        system_default_local_queue.Close();
                    }
                }
                catch { }
            }
        }
        public long putMsgWithId(string fid, string mid, string inmsg, string inmsgId)
        {
            try
            {
                string filename = this.GetAssemblyPath() + "ESBMQClientProperty.xml";
                XmlDocument document = new XmlDocument();
                document.Load(filename);
                string str2 = fid + "_" + mid;
                this.system_default_local_queue = this.qMgr.AccessQueue(document.SelectSingleNode("root/" + Strcionid + "/" + str2).Attributes["connectionString"].Value.ToString(), this.pOpenOptions);
                MQMessage message = new MQMessage
                {
                    CorrelationId = ConvertHexToBytes(inmsgId),
                    CharacterSet = 1386,
                    Format = "MQSTR"
                };

                int length = Encoding.Default.GetBytes(inmsg).Length;
                message.Write(Encoding.Default.GetBytes(inmsg), 0, length);
                MQPutMessageOptions pmo = new MQPutMessageOptions();
                this.system_default_local_queue.Put(message, pmo);
                return 1;
            }
            catch (MQException exception)
            {
                Logger.LogInfo(System.DateTime.Now.ToString() + "|ERROR:" + exception.Message);
                return exception.Reason;
            }
            finally
            {
                try
                {
                    if (system_default_local_queue != null)
                    {
                        system_default_local_queue.Close();
                    }
                }
                catch { }
            }
        }

        public long putMsgWithId2(string strQueueName, string inmsg, string inmsgId)
        {
            try
            {
                this.system_default_local_queue = this.qMgr.AccessQueue(strQueueName, this.pOpenOptions);
                MQMessage message = new MQMessage
                {
                    CorrelationId = ConvertHexToBytes(inmsgId),
                    CharacterSet = 1386,
                    Format = "MQSTR"
                };

                int length = Encoding.Default.GetBytes(inmsg).Length;
                message.Write(Encoding.Default.GetBytes(inmsg), 0, length);
                MQPutMessageOptions pmo = new MQPutMessageOptions();
                this.system_default_local_queue.Put(message, pmo);
                return 1;
            }
            catch (MQException exception)
            {
                Logger.LogInfo(System.DateTime.Now.ToString() + "|ERROR:" + exception.Message);
                return exception.Reason;
            }
            finally
            {
                try
                {
                    if (system_default_local_queue != null)
                    {
                        system_default_local_queue.Close();
                    }
                }
                catch { }
            }
        }

        public long browseMsg(string fid, int waitInterval, ref string msgID, ref string Msg)
        {
            try
            {

                string filename = this.GetAssemblyPath() + "ESBMQClientProperty.xml";
                XmlDocument document = new XmlDocument();
                document.Load(filename);
                string str2 = fid;
                this.system_default_local_queue = this.qMgr.AccessQueue(document.SelectSingleNode("root/" + Strcionid + "/" + str2).Attributes["connectionString"].Value.ToString(), this.rOpenOptions);
                MQMessage message = new MQMessage();
                MQGetMessageOptions gmo = new MQGetMessageOptions
                {
                    WaitInterval = waitInterval,
                    Options = MQC.MQGMO_WAIT | MQC.MQGMO_BROWSE_FIRST
                };
                this.system_default_local_queue.Get(message, gmo);
                msgID = BitConverter.ToString(message.MessageId).Replace("-", "");
                message.CharacterSet = 1386;
                message.Format = "MQSTR";
                Msg = message.ReadString(message.MessageLength);
                return 1;

            }
            catch (MQException exception)
            {
                Logger.LogInfo(System.DateTime.Now.ToString() + "|ERROR:" + exception.Message);
                return exception.Reason;
            }
            finally
            {
                try
                {
                    if (system_default_local_queue != null)
                    {
                        system_default_local_queue.Close();
                    }
                }
                catch
                {

                }
            }
        }

        public long browseMsgById(string fid, string msgID, int waitInterval, ref string Msg)
        {
            try
            {
                string filename = this.GetAssemblyPath() + "ESBMQClientProperty.xml";
                XmlDocument document = new XmlDocument();
                document.Load(filename);
                string str2 = fid;
                this.system_default_local_queue = this.qMgr.AccessQueue(document.SelectSingleNode("root/" + Strcionid + "/" + str2).Attributes["connectionString"].Value.ToString(), this.rOpenOptions);
                MQMessage message = new MQMessage
                {
                    CorrelationId = ConvertHexToBytes(msgID)
                };
                MQGetMessageOptions gmo = new MQGetMessageOptions
                {
                    WaitInterval = waitInterval,
                    Options = MQC.MQGMO_WAIT | MQC.MQGMO_BROWSE_FIRST
                };
                this.system_default_local_queue.Get(message, gmo);

                message.CharacterSet = 1386;
                message.Format = "MQSTR";
                Msg = message.ReadString(message.MessageLength);
                return 1;
            }
            catch (MQException exception)
            {
                Logger.LogInfo(System.DateTime.Now.ToString() + "|ERROR:" + exception.Message);
                return exception.Reason;
            }
            finally
            {
                try
                {
                    if (system_default_local_queue != null)
                    {
                        system_default_local_queue.Close();
                    }
                }
                catch (MQException exception)
                {

                }
            }
        }
        public static string ConvertBytesToHex(byte[] arrByte, bool reverse)
        {
            StringBuilder sb = new StringBuilder();
            if (reverse)
                Array.Reverse(arrByte);
            foreach (byte b in arrByte)
                sb.AppendFormat("{0:x2}", b);
            return sb.ToString();
        }
        public static byte[] ConvertHexToBytes(string value)
        {
            int len = value.Length / 2;
            byte[] ret = new byte[len];
            for (int i = 0; i < len; i++)
                ret[i] = (byte)(Convert.ToInt32(value.Substring(i * 2, 2), 16));
            return ret;
        }
















    }
}
