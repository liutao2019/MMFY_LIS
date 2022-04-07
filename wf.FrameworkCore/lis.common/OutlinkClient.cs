using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dcl.common.extensions;

namespace dcl.common
{
    public class OutlinkClient
    {
        /// <summary>
        /// OutLink调用门诊病人资料
        /// </summary>
        /// <param name="patID"></param>
        /// <returns></returns>
        public DataTable GetMZPatient(string patID)
        {
            string param = Outlink.GenerateMZInput(patID);
            if (string.IsNullOrEmpty(param))
                return new DataTable();
            string result = Outlink.GetClinPat(param);
            ConvertHelper helper = new ConvertHelper();
            DataSet ds = helper.ConvertToDataSet(result, SplitType.MzInfo);
            if (Extensions.IsEmpty(ds))
                return new DataTable();
            return ds.Tables[0];
        }

        /// <summary>
        /// OutLink调用住院病人资料
        /// </summary>
        /// <param name="patID"></param>
        /// <returns></returns>
        public DataTable GetZYPatient(string patID)
        {
            string param = Outlink.GenerateZYInput(patID);
            if (string.IsNullOrEmpty(param))
                return new DataTable();
            string result = Outlink.GetWardPat(param);
            ConvertHelper helper = new ConvertHelper();
            DataSet ds = helper.ConvertToDataSet(result, SplitType.PatInfo);
            if (Extensions.IsEmpty(ds))
                return new DataTable();
            return ds.Tables[0];
        }

        


    }
}
