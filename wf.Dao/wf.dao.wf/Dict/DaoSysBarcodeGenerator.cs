using dcl.dao.core;
using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoSysBarcodeGenerator))]
    public class DaoSysBarcodeGenerator : IDaoSysBarcodeGenerator
    {
        public string GetNextMaxBarCode()
        { 
            DBManager helper = new DBManager();
            string sql;

            sql = "update Base_barcode_generator set Bbargen_no=Bbargen_no+1 where  Bbargen_id='5' ";
            helper.ExecSql(sql);

            sql = "select Bbargen_no from Base_barcode_generator  where Bbargen_id='5' ";
            DataTable obj = helper.ExecSel(sql);
            var intSeq = Int64.Parse(obj.Rows[0][0].ToString());
            string seq;
            if (intSeq > 999999)
            {
                seq = intSeq.ToString("00000000");
            }
            else
            {
                seq = intSeq.ToString("000000");
            }
            int jy = 0;
            foreach (char c in seq.ToArray())
            {
                jy += int.Parse(c.ToString());
            }
            return jy.ToString("00") + seq;
        }
    }
}
