using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.result
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPatientInterface<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="patientID"></param>
        /// <returns>pat_name</returns>
        T Get(string code,string interfaceID);
    }
}
