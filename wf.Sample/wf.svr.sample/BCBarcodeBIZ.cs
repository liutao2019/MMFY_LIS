using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using lis.dto;
using dcl.common.extensions;
using dcl.root.dac;
using dcl.svr.frame;
using lis.common.extensions;

namespace dcl.svr.sample
{
    public class BCBarcodeBIZ : BIZBase
    {
        public override string MainTable
        {
            get { return BarcodeTable.Barcode.TableName; }
        }

        public override string PrimaryKey
        {
            get { return BarcodeTable.Barcode.ID; }
        }

        /// <summary>
        /// 获得最新的条码号(条码号自增1)
        /// </summary>
        /// <returns></returns>
        internal string GetNewBarcodeOld()
        {
            int updateResult = Update(string.Format("{0} = {0} + 1", BarcodeTable.Barcode.BarcodeID), " 1 = 1 ");
            if (updateResult <= 0)
                return "";

            string newBarcode = "";
            DataSet dsResult = Search("");
            if (dsResult.IsNotEmpty())
            {
                DataTable dtResult = dsResult.Tables[0];
                newBarcode = dtResult.Rows[0][BarcodeTable.Barcode.BarcodeID].ToString();
            }

            return newBarcode;
        }

        /// <summary>
        /// 获得最新的条码号(条码号自增1) //加锁
        /// </summary>
        /// <returns></returns>
        public string GetNewBarcode()
        {
            DBHelper helper = new DBHelper();

            string newBarcode = "";
            DataSet dsResult = helper.ExecuteSql("sp_get_new_barcode");
            if (dsResult.IsNotEmpty())
            {
                DataTable dtResult = dsResult.Tables[0];
                newBarcode = dtResult.Rows[0][0].ToString();
            }

            return newBarcode;
        }


        // sp_get_new_barcode


        /// <summary>
        /// 获得最新的总批次条码号(条码号自增1) //加锁
        /// </summary>
        /// <returns></returns>
        public string GetNewBatchBarcode()
        {
            try
            {
                DBHelper helper = new DBHelper();

                string newBarcode = "";
                DataSet dsResult = helper.ExecuteSql("sp_get_batchbarcode");
                if (dsResult.IsNotEmpty())
                {
                    DataTable dtResult = dsResult.Tables[0];
                    newBarcode = dtResult.Rows[0][0].ToString();
                }

                return newBarcode;
            }
            catch (Exception ex)
            {
                
                Lib.LogManager.Logger.LogException(ex);

                throw;
            }
            
        }

        public override DataSet Search(string where)
        {
            return doSearch(new DataSet(), SearchSQL);
        }
    }
}