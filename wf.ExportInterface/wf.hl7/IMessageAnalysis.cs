using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.hl7
{
    public interface IMessageAnalysis
    {
        List<EntitySampOrderHL7> MessageAnalysis(string msgXml);
    }
}
