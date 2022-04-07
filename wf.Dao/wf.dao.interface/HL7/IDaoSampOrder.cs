﻿using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoSampOrder
    {
        bool SaveSampOrder(List<EntitySampOrderHL7> listSampOrder);

        bool DeleteSampOrder(string strOrderSn);
    }
}
