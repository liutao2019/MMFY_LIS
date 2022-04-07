using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using lis.dto;
using dcl.common.extensions;
using dcl.root.dac;
using dcl.common;
using System.Collections;
using dcl.svr.cache;
using dcl.svr.root.com;
using dcl.root.dto;

namespace dcl.svr.sample
{
    public class BCCombineSplitBIZ : ICommonBIZ
    {

        private DbBase dao = DbBase.InConn();

        public string MainTable
        {
            get { return BarcodeTable.Combine.TableName; }
        }

        public  string PrimaryKey
        {
            get { return BarcodeTable.Combine.ID; }
        }     

        public  DataSet doUpdate(DataSet dsData)
        {
            DataSet result = new DataSet();
            try
            {
                if (dsData == null)
                    return null;
                DataTable dtCombineDetail = dsData.Tables["dict_combine_split"];
                DataTable dtComID = dsData.Tables["ComID"];
                String oldComId = dtComID.Rows[0]["com_id"].ToString();
                ArrayList arr = new ArrayList();
                arr.Add(String.Format(@"delete from dict_combine_split where com_id='{0}' ",
                    new string[] { oldComId }));             
            
                arr.AddRange(dao.GetInsertSql(dtCombineDetail));
                dao.DoTran(arr);
             
                result.Tables.Add(dtCombineDetail.Copy());

                DictCombineSplitCache.Current.Refresh();

                return result;
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("更新信息出错!", ex.ToString())); ;
            }
            return result;
        }

        #region ICommonBIZ 成员

        public DataSet doDel(DataSet ds)
        {
            throw new NotImplementedException();
        }

        public DataSet doNew(DataSet ds)
        {
            throw new NotImplementedException();
        }

        public DataSet doOther(DataSet ds)
        {
            throw new NotImplementedException();
        }

        public DataSet doSearch(DataSet ds)
        {
            throw new NotImplementedException();
        }

        public DataSet doView(DataSet ds)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}