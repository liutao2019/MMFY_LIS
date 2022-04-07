namespace dcl.client.frame
{
    using DevExpress.XtraEditors;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;

    public class ConCommonExt : XtraUserControl, IClientAction
    {
        private CommonDelegateExt _biz;
        private static string _endpointConfigurationName = "svc.basic";
        private static string _remoteAddress = ConfigurationManager.AppSettings["wcfAddr"];
        private IContainer components = null;
        public bool isActionSuccess = true;

        public ConCommonExt()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Name = "ConCommonExt";
            base.ResumeLayout(false);
        }

        private void load()
        {
            //CommonClient.setUserPower.setPower(this, base.Name);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
                this.Biz.Close();
            }
            base.Dispose(disposing);
        }

        public CommonDelegateExt Biz
        {
            get
            {
                if (this._biz == null)
                {
                    string remoteAddress = _remoteAddress + ConfigurationManager.AppSettings["svc." + base.Name];
                    this._biz = CommonDelegateExt.getDelegate(_endpointConfigurationName, remoteAddress);
                }
                return this._biz;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!base.DesignMode)
            {
                this.load();
            }
            base.OnLoad(e);
        }

        #region AfterAction

        public virtual void afterDel(DataSet returnData)
        {
            Succeed();
        }

        private void Succeed()
        {
            if (this.isActionSuccess)
            {
                MessageBox.Show("操作成功！");
            }
        }

        public virtual void afterDel(DataSet returnData, object userObj)
        {
            Succeed();
        }

        public virtual void afterNew(DataSet returnData)
        {
            Succeed();
        }

        public virtual void afterNew(DataSet returnData, object userObj)
        {
            Succeed();
        }

        public virtual void afterOther(DataSet returnData)
        {
        }

        public virtual void afterOther(DataSet returnData, object userObj)
        {
        }

        public virtual void afterSearch(DataSet returnData)
        {
        }

        public virtual void afterSearch(DataSet returnData, object userObj)
        {
        }

        public virtual void afterUpdate(DataSet returnData)
        {
            Succeed();
        }

        public virtual void afterUpdate(DataSet returnData, object userObj)
        {
            Succeed();
        }

        public virtual void afterView(DataSet returnData)
        {
        }

        public virtual void afterView(DataSet returnData, object userObj)
        {
        }

        #endregion

        #region DoAction

        public DataSet doDel()
        {
            return new Delete(this).DoAction(this, null);
        }

        public DataSet doDel(object userObj)
        {
            return new Delete(this).DoAction(this, userObj);
        }

        public DataSet doNew()
        {
            return new New(this).DoAction(this, null);
        }

        public DataSet doNew(object userObj)
        {
            return new New(this).DoAction(this, userObj);
        }

        public DataSet doOther()
        {
            return new Other(this).DoAction(this, null);
        }

        public DataSet doOther(object userObj)
        {
            return new Other(this).DoAction(this, userObj);
        }

        public DataSet doSearch(string where)
        {
            return new Search(this).DoAction(this, where);
        }

        public DataSet doSearch()
        {
            return new Search(this).DoAction(this, null);
        }

        public DataSet doSearch(object userObj)
        {
            return new Search(this).DoAction(this, userObj);
        }

        public DataSet doUpdate()
        {
            return new Update(this).DoAction(this, null);
        }

        public DataSet doUpdate(object userObj)
        {
            return new Update(this).DoAction(this, userObj);
        }

        public DataSet doView()
        {
            return new View(this).DoAction(this, null);
        }

        public DataSet doView(object userObj)
        {
            return new View(this).DoAction(this, userObj);
        }

        #endregion

        #region SetDataSet

        public virtual DataSet setToDelDS()
        {
            return new DataSet();
        }

        public virtual DataSet setToDelDS(object userObj)
        {
            return new DataSet();
        }

        public virtual DataSet setToNewDS()
        {
            return new DataSet();
        }

        public virtual DataSet setToNewDS(object userObj)
        {
            return new DataSet();
        }

        public virtual DataSet setToOtherDS()
        {
            return new DataSet();
        }

        public virtual DataSet setToOtherDS(object userObj)
        {
            return new DataSet();
        }

        public virtual DataSet setToSearchDS()
        {
            return new DataSet();
        }

        public virtual DataSet setToSearchDS(object userObj)
        {
            return new DataSet();
        }

        public virtual DataSet setToUpdateDS()
        {
            return new DataSet();
        }

        public virtual DataSet setToUpdateDS(object userObj)
        {
            return new DataSet();
        }

        public virtual DataSet setToViewDS()
        {
            return new DataSet();
        }

        public virtual DataSet setToViewDS(object userObj)
        {
            return new DataSet();
        }

        #endregion

        #region IClientAction 成员


        public bool IsActionSuccess
        {
            get {return this.isActionSuccess; }
            set { this.isActionSuccess = value; }
        }

        #endregion
    }
}

