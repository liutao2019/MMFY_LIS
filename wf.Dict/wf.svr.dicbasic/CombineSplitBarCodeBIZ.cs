using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;

namespace dcl.svr.dicbasic
{
    public class CombineSplitBarCodeBIZ : ICombineSplitBarCode
    {
        public List<EntitySampMergeRule> GetAllCombineSplitBarCode()
        {
            List<EntitySampMergeRule> listRule = new List<EntitySampMergeRule>();
            listRule = new ItemCombineBarcodeBIZ().GetAllCombineSplitBarCode();
            return listRule;
        }
    }
}
