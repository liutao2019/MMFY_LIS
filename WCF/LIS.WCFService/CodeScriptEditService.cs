using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.servececontract;
using Lis.Biz.CodeScript;

namespace dcl.svr.wcf
{
    public class CodeScriptEditService:ICodeScriptEdit
    {
        #region ICodeScriptEdit 成员

        public System.Data.DataTable m_mthSearchScript()
        {
            return new CodeScriptEditBIZ().m_mthSearchScript();
        }

        public System.Data.DataTable m_dtbAddScriptdata(System.Data.DataTable p_dtbdata)
        {
            return new CodeScriptEditBIZ().m_dtbAddScriptdata(p_dtbdata);
        }


        #endregion

        #region ICodeScriptEdit 成员


        public System.Data.DataTable m_dtbUpdateScrinpt(System.Data.DataTable p_dtbData)
        {
            return new CodeScriptEditBIZ().m_dtbUpdateScrinpt(p_dtbData);
        }

        #endregion
    }
}
