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
using System.Diagnostics;
using Lib.DAC;

namespace Lib.DataInterface
{
    class DOSCmdDataInterfaceConnection : AbstractDataInterfaceConnection
    {
        public override IDataInterfaceCommand CreateCommand()
        {
            DOSCmdDataInterfaceCommand cmd = new DOSCmdDataInterfaceCommand();
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
            string pName = "cmd.exe";
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = pName;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;

                p.Start();

                success = true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;

                errorMessage += " " + pName;
            }

            return success;
        }
    }
}
