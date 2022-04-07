using System;
using System.Collections.Generic;

using System.Text;
using System.Data;
namespace dcl.client.frame
{
    public interface IClientAction
    {
        #region AfterAction
        void afterDel(DataSet returnData);
        void afterDel(DataSet returnData, object userObj);
        void afterNew(DataSet returnData);
        void afterNew(DataSet returnData, object userObj);
        void afterOther(DataSet returnData);
        void afterOther(DataSet returnData, object userObj);
        void afterSearch(DataSet returnData);
        void afterSearch(DataSet returnData, object userObj);
        void afterUpdate(DataSet returnData);
        void afterUpdate(DataSet returnData, object userObj);
        void afterView(DataSet returnData);
        void afterView(DataSet returnData, object userObj);
        #endregion

        #region DoAction
         DataSet doDel();
         DataSet doDel(object userObj);
         DataSet doNew();
         DataSet doNew(object userObj);
         DataSet doOther();
         DataSet doOther(object userObj);
         DataSet doSearch(string where);
         DataSet doSearch();
         DataSet doSearch(object userObj);
         DataSet doUpdate();
         DataSet doUpdate(object userObj);
         DataSet doView();
         DataSet doView(object userObj);
        #endregion

        #region SetDataSet
        DataSet setToDelDS();
        DataSet setToDelDS(object userObj);
        DataSet setToNewDS();
        DataSet setToNewDS(object userObj);
        DataSet setToOtherDS();
        DataSet setToOtherDS(object userObj);
        DataSet setToSearchDS();
        DataSet setToSearchDS(object userObj);
        DataSet setToUpdateDS();
        DataSet setToUpdateDS(object userObj);
        DataSet setToViewDS();
        DataSet setToViewDS(object userObj);
        #endregion

        bool IsActionSuccess { get; set; }

        CommonDelegateExt Biz { get; }
    }
}
