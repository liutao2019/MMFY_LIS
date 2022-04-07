using dcl.dao.core;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoSysMessageSpeech
    {
        /// <summary>
        /// 保存系统发声信息
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        Boolean SaveMessageSpeech(EntitySysMessageSpeech queue);
    }
}
