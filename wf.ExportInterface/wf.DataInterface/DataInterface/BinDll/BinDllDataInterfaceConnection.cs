using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Services.Description;
using System.Net;
using System.Xml.Serialization;
using System.IO;
using System.CodeDom.Compiler;
using System.Reflection;
using System.CodeDom;
using Lib.DAC;

namespace Lib.DataInterface
{
    class BinDllDataInterfaceConnection : AbstractDataInterfaceConnection
    {
        public override IDataInterfaceCommand CreateCommand()
        {
            BinDllDataInterfaceCommand cmd = new BinDllDataInterfaceCommand();
            //cmd.invokeType = CreateWSAssembly();
            return cmd;
        }

        public override SqlHelper GetSqlHelper()
        {
            throw new NotImplementedException();
        }

        public override bool TestConnection(out string errorMessage)
        {
            bool success = false;
            errorMessage = null;

            try
            {

                success = true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return success;
        }


    }

}
