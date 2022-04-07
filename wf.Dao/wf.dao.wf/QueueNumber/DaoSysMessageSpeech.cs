using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoSysMessageSpeech))]
    ///电视机系统用
    public class DaoSysMessageSpeech : IDaoSysMessageSpeech
    {
       
        public bool SaveMessageSpeech(EntitySysMessageSpeech speech)
        {
            try
            {
                DBManager helper = new DBManager();
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("speech_text", speech.SpeechText);
                values.Add("status", speech.Status);
                values.Add("data", speech.Data);
                values.Add("create_date", speech.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"));
                helper.InsertOperation("bl_sys_message_speech", values);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

    }
}
