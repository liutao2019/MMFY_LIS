using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using dcl.common.extensions;

namespace dcl.client.frame
{
    public abstract class ClientAction
    {
        protected IClientAction frmCommon = null;

        public ClientAction(IClientAction frmCommon)
        {
            this.frmCommon = frmCommon;
        }

        public DataSet DoAction(IClientAction frmCommon, object userObj)
        {
            frmCommon.IsActionSuccess = true;
            DataSet ds = new DataSet();
            ds = GetDataSet();
            if (ds == null)
            {
                return null;
            }
            ds.Tables.Add(CommonClient.getUserLogInfo());
            if (userObj is DataTable)
            {
                ds.Tables.Add((DataTable)userObj);
            }
            if (userObj is DataSet)
            {
                ds.Merge((DataSet)userObj);
            }
            DataSet returnData = new DataSet();
            if (userObj is string)
                returnData = RequestBizAction(userObj.ToString());
            else
                returnData = RequestBizAction(ds);
            if ((returnData.Tables["_ERRORINFO"] != null && returnData.Tables["_ERRORINFO"].Rows[0]["_ERROR_MSG"].ToString() != ""))
            {
                AppError.show(returnData.Tables["_ERRORINFO"].Rows[0]["_ERROR_MSG"].ToString(), new Exception(returnData.Tables["_ERRORINFO"].Rows[0]["_ERROR_DETAIL"].ToString()));
                frmCommon.IsActionSuccess = false;
            }
            AfterAction(returnData, userObj);

            return returnData;
        }

        public abstract void AfterAction(DataSet returnData, object userObj);
        public abstract DataSet RequestBizAction(DataSet ds);
        public abstract DataSet RequestBizAction(string input);
        public abstract DataSet GetDataSet();
    }

    public class Delete : ClientAction
    {
        public Delete(IClientAction frmCommExt)
            : base(frmCommExt)
        { }

        public override void AfterAction(DataSet returnData, object userObj)
        {
            frmCommon.afterDel(returnData, userObj);
        }

        public override DataSet RequestBizAction(DataSet ds)
        {
            return frmCommon.Biz.doDel(ds);
        }

        public override DataSet GetDataSet()
        {
            return frmCommon.setToDelDS();
        }

        public override DataSet RequestBizAction(string input)
        {
            throw new NotImplementedException();
        }
    }

    public class New : ClientAction
    {
        public New(IClientAction frmCommExt)
            : base(frmCommExt)
        { }
        public override void AfterAction(DataSet returnData, object userObj)
        {
            frmCommon.afterNew(returnData, userObj);
        }

        public override DataSet RequestBizAction(DataSet ds)
        {
            return frmCommon.Biz.doNew(ds);
        }

        public override DataSet GetDataSet()
        {
            return frmCommon.setToNewDS();
        }

        public override DataSet RequestBizAction(string input)
        {
            throw new NotImplementedException();
        }
    }

    public class Other : ClientAction
    {
        public Other(IClientAction frmCommExt)
            : base(frmCommExt)
        { }
        public override void AfterAction(DataSet returnData, object userObj)
        {
            frmCommon.afterOther(returnData, userObj);
        }

        public override DataSet RequestBizAction(DataSet ds)
        {
            return frmCommon.Biz.doOther(ds);
        }

        public override DataSet GetDataSet()
        {
            return frmCommon.setToOtherDS();
        }

        public override DataSet RequestBizAction(string input)
        {
            throw new NotImplementedException();
        }
    }

    public class Update : ClientAction
    {
        public Update(IClientAction frmCommExt)
            : base(frmCommExt)
        { }
        public override void AfterAction(DataSet returnData, object userObj)
        {
            frmCommon.afterUpdate(returnData, userObj);
        }

        public override DataSet RequestBizAction(DataSet ds)
        {
            return frmCommon.Biz.doUpdate(ds);
        }

        public override DataSet GetDataSet()
        {
            return frmCommon.setToUpdateDS();
        }

        public override DataSet RequestBizAction(string input)
        {
            throw new NotImplementedException();
        }
    }

    public class View : ClientAction
    {
        public View(IClientAction frmCommExt)
            : base(frmCommExt)
        { }
        public override void AfterAction(DataSet returnData, object userObj)
        {
            frmCommon.afterView(returnData, userObj);
        }

        public override DataSet RequestBizAction(DataSet ds)
        {
            return frmCommon.Biz.doView(ds);
        }

        public override DataSet GetDataSet()
        {
            return frmCommon.setToViewDS();
        }

        public override DataSet RequestBizAction(string input)
        {
            throw new NotImplementedException();
        }
    }

    public class Search : ClientAction
    {
        public Search(IClientAction frmCommExt)
            : base(frmCommExt)
        { }
        public override void AfterAction(DataSet returnData, object userObj)
        {
            frmCommon.afterSearch(returnData, userObj);
        }

        public override DataSet RequestBizAction(DataSet ds)
        {
            return frmCommon.Biz.doSearch(ds);
        }

        public override DataSet GetDataSet()
        {
            return frmCommon.setToSearchDS();
        }

        public override DataSet RequestBizAction(string input)
        {
            if (string.IsNullOrEmpty(input))
                return new DataSet();
            else
            {
                input = " WHERE " + input;
                return frmCommon.Biz.Search(input);
            }
        }
    }
}
