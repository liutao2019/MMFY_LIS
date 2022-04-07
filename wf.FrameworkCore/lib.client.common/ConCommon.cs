namespace dcl.client.frame
{
    using DevExpress.XtraEditors;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Collections.Generic;

    [DesignTimeVisible(false)]
    public class ConCommon : XtraUserControl
    {
        private CommonDelegate _biz;
        private static string _endpointConfigurationName = "svc.basic";
        private static string _remoteAddress = ConfigurationManager.AppSettings["wcfAddr"];
        private IContainer components = null;
        public bool isActionSuccess = true;

        public ConCommon()
        {
            this.InitializeComponent();
        }

        protected virtual void afterDel(DataSet returnData)
        {
            if (this.isActionSuccess)
            {
                MessageBox.Show("操作成功！");
            }
        }

        protected virtual void afterDel(DataSet returnData, object userObj)
        {
            if (this.isActionSuccess)
            {
                MessageBox.Show("操作成功！");
            }
        }

        protected virtual void afterNew(DataSet returnData)
        {
            if (this.isActionSuccess)
            {
                MessageBox.Show("操作成功！");
            }
        }

        protected virtual void afterNew(DataSet returnData, object userObj)
        {
            if (this.isActionSuccess)
            {
                MessageBox.Show("操作成功！");
            }
        }

        protected virtual void afterOther(DataSet returnData)
        {
        }

        protected virtual void afterOther(DataSet returnData, object userObj)
        {
        }

        protected virtual void afterSearch(DataSet returnData)
        {
        }

        protected virtual void afterSearch(DataSet returnData, object userObj)
        {
        }

        protected virtual void afterUpdate(DataSet returnData)
        {
            if (this.isActionSuccess)
            {
                MessageBox.Show("操作成功！");
            }
        }

        protected virtual void afterUpdate(DataSet returnData, object userObj)
        {
            if (this.isActionSuccess)
            {
                MessageBox.Show("操作成功！");
            }
        }

        protected virtual void afterView(DataSet returnData)
        {
        }

        protected virtual void afterView(DataSet returnData, object userObj)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
                this.biz.Close();
            }
            base.Dispose(disposing);
        }

        private DataSet doAction(ACT action, object userObj)
        {
            DataSet ds = new DataSet();
            switch (action)
            {
                case ACT._NEW:
                    ds = this.setToNewDS();
                    break;

                case ACT.UPDATE:
                    ds = this.setToUpdateDS();
                    break;

                case ACT._VIEW:
                    ds = this.setToViewDS();
                    break;

                case ACT._DEL:
                    ds = this.setToDelDS();
                    break;

                case ACT._SEARCH:
                    ds = this.setToSearchDS();
                    break;

                case ACT._OTHER:
                    ds = this.setToOtherDS(userObj);
                    break;
            }
            if (ds == null)
            {
                return null;
            }
            ds.Tables.Add(CommonClient.getUserLogInfo());
            if (userObj is DataTable)
            {
                ds.Tables.Add((DataTable) userObj);
            }
            if (userObj is DataSet)
            {
                ds.Merge((DataSet) userObj);
            }
            DataSet returnData = new DataSet();
            switch (action)
            {
                case ACT._NEW:
                    returnData = this.biz.doNew(ds);
                    break;

                case ACT.UPDATE:
                    returnData = this.biz.doUpdate(ds);
                    break;

                case ACT._VIEW:
                    returnData = this.biz.doView(ds);
                    break;

                case ACT._DEL:
                    returnData = this.biz.doDel(ds);
                    break;

                case ACT._SEARCH:
                    returnData = this.biz.doSearch(ds);
                    break;

                case ACT._OTHER:
                    returnData = this.biz.doOther(ds);
                    break;
            }
            if ((returnData.Tables["_ERRORINFO"] != null) && (returnData.Tables["_ERRORINFO"].Rows[0]["_ERROR_MSG"].ToString() != ""))
            {
                AppError.show(returnData.Tables["_ERRORINFO"].Rows[0]["_ERROR_MSG"].ToString(), new Exception(returnData.Tables["_ERRORINFO"].Rows[0]["_ERROR_DETAIL"].ToString()));
                this.isActionSuccess = false;
            }
            //else
            //{
            //    this.isActionSuccess = true;
            //}
            switch (action)
            {
                case ACT._NEW:
                    this.afterNew(returnData);
                    return returnData;

                case ACT.UPDATE:
                    this.afterUpdate(returnData);
                    return returnData;

                case ACT._VIEW:
                    this.afterView(returnData);
                    return returnData;

                case ACT._DEL:
                    this.afterDel(returnData);
                    return returnData;

                case ACT._SEARCH:
                    this.afterSearch(returnData);
                    return returnData;

                case ACT._OTHER:
                    this.afterOther(returnData, userObj);
                    return returnData;
            }
            return returnData;
        }

        public DataSet doDel()
        {
            return this.doAction(ACT._DEL, null);
        }

        public DataSet doDel(object userObj)
        {
            return this.doAction(ACT._DEL, userObj);
        }

        public DataSet doNew()
        {
            return this.doAction(ACT._NEW, null);
        }

        public DataSet doNew(object userObj)
        {
            return this.doAction(ACT._NEW, userObj);
        }

        public DataSet doOther()
        {
            return this.doAction(ACT._OTHER, null);
        }

        public DataSet doOther(object userObj)
        {
            return this.doAction(ACT._OTHER, userObj);
        }

        public DataSet doSearch()
        {
            return this.doAction(ACT._SEARCH, null);
        }

        public DataSet doSearch(object userObj)
        {
            return this.doAction(ACT._SEARCH, userObj);
        }

        public DataSet doUpdate()
        {
            return this.doAction(ACT.UPDATE, null);
        }

        public DataSet doUpdate(object userObj)
        {
            return this.doAction(ACT.UPDATE, userObj);
        }

        public DataSet doView()
        {
            return this.doAction(ACT._VIEW, null);
        }

        public DataSet doView(object userObj)
        {
            return this.doAction(ACT._VIEW, userObj);
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Name = "ConCommon";
            base.ResumeLayout(false);
        }

        private void load()
        {
            CommonClient.setUserPower.setPower(this, base.Name);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!base.DesignMode)
            {
                this.load();
            }
            base.OnLoad(e);
        }

        public void FindGridviewControl(Control.ControlCollection controlList)
        {
            string strRef = "";
            foreach (Control ctl in controlList)
            {

                if (ctl.GetType() == typeof(DevExpress.XtraGrid.GridControl))
                {
                    foreach (DevExpress.XtraGrid.Views.Grid.GridView view in (ctl as DevExpress.XtraGrid.GridControl).Views)
                    {
                        DataRow[] drArr =FrmBase.dtbControlGridView.Select("controlname='" + view.Name + "'");
                        Dictionary<string, int> dictColSeq = new Dictionary<string, int>();
                        if (drArr.Length > 0)
                        {
                            string[] strColArr = drArr[0]["viewcolumn"].ToString().Split(',');

                            for (int i = 0; i < strColArr.Length; i++)
                            {
                                string[] strColIndexArr = strColArr[i].Split(';');

                                dictColSeq.Add(strColIndexArr[0], int.Parse(strColIndexArr[1]));
                            }

                        }
                        foreach (string strItem in dictColSeq.Keys)
                        {
                            if (view.Columns.ColumnByName(strItem) != null)
                            {
                                view.Columns.ColumnByName(strItem).VisibleIndex = dictColSeq[strItem];
                            }
                        }
                    }
                }
                else
                {
                    FindGridviewControl(ctl.Controls);
                }
            }

        }

        protected virtual DataSet setToDelDS()
        {
            return new DataSet();
        }

        protected virtual DataSet setToDelDS(object userObj)
        {
            return new DataSet();
        }

        protected virtual DataSet setToNewDS()
        {
            return new DataSet();
        }

        protected virtual DataSet setToNewDS(object userObj)
        {
            return new DataSet();
        }

        protected virtual DataSet setToOtherDS()
        {
            return new DataSet();
        }

        protected virtual DataSet setToOtherDS(object userObj)
        {
            return new DataSet();
        }

        protected virtual DataSet setToSearchDS()
        {
            return new DataSet();
        }

        protected virtual DataSet setToSearchDS(object userObj)
        {
            return new DataSet();
        }

        protected virtual DataSet setToUpdateDS()
        {
            return new DataSet();
        }

        protected virtual DataSet setToUpdateDS(object userObj)
        {
            return new DataSet();
        }

        protected virtual DataSet setToViewDS()
        {
            return new DataSet();
        }

        protected virtual DataSet setToViewDS(object userObj)
        {
            return new DataSet();
        }

        
        public CommonDelegate biz
        {
            get
            {
                if (this._biz == null)
                {
                    string remoteAddress = _remoteAddress + ConfigurationManager.AppSettings["svc." + base.Name];
                    this._biz = CommonDelegate.getDelegate(_endpointConfigurationName, remoteAddress);
                }
                return this._biz;
            }
        }

        public enum ACT
        {
            _NEW,
            UPDATE,
            _VIEW,
            _DEL,
            _SEARCH,
            _OTHER
        }
    }
}

