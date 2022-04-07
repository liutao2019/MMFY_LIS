using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using lis.dto;
using dcl.svr.frame;

namespace dcl.svr.sample
{
    public class BCPrintBIZ : BIZBase
    {
        public override string MainTable
        {
            get { return BarcodeTable.Patient.TableName; }
        }

        public override string PrimaryKey
        {
            get { return BarcodeTable.Patient.ID; }
        }

        public override DataSet Search(string where)
        {
            return  doSearch(new DataSet(), SearchSQL + where);
        } 
    }
}