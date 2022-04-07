namespace dcl.client.frame
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Windows.Forms;
    using Lib.LogManager;

    public class FrmCommon : FrmBase
    {
        private IContainer components = null;
        public bool isActionSuccess = true;
        bool showSucessMessage = true;
        public bool ShowSucessMessage
        {
            get { return showSucessMessage; }
            set { showSucessMessage = value; }
        }

        public FrmCommon()
        {
            this.InitializeComponent();

            this.Tag = this.GetType().FullName;
        }

        protected virtual void afterDel(DataSet returnData)
        {
            if (this.isActionSuccess && showSucessMessage)
            {
                MessageBox.Show("操作成功！");
            }
        }

        protected virtual void afterDel(DataSet returnData, object userObj)
        {
            if (this.isActionSuccess && showSucessMessage)
            {
                MessageBox.Show("操作成功！");
            }
        }

        protected virtual void afterNew(DataSet returnData)
        {
            if (this.isActionSuccess && showSucessMessage)
            {
                MessageBox.Show("操作成功！");
            }
        }

        protected virtual void afterNew(DataSet returnData, object userObj)
        {
            if (this.isActionSuccess && showSucessMessage)
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
            if (this.isActionSuccess && showSucessMessage)
            {
                MessageBox.Show("操作成功！");
            }
        }

        protected virtual void afterUpdate(DataSet returnData, object userObj)
        {
            if (this.isActionSuccess && showSucessMessage)
            {
                MessageBox.Show("操作成功！");
            }
        }

        protected virtual void afterView(DataSet returnData)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private DataSet doAction(ACT action, object userObj)
        {
            this.isActionSuccess = true;
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
                    try
                    {
                        ds = this.setToSearchDS();
                    }
                    catch
                    { }
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
                ds.Tables.Add((DataTable)userObj);
            }
            if (userObj is DataSet)
            {
                ds.Merge((DataSet)userObj);
            }
            DataSet returnData = new DataSet();
            switch (action)
            {
                case ACT._NEW:
                    try
                    {
                        returnData = base.biz.doNew(ds);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException("New", ex);
                    }
                    break;

                case ACT.UPDATE:
                    try
                    {
                        returnData = base.biz.doUpdate(ds);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException("Update", ex);
                    }
                    break;

                case ACT._VIEW:
                    try
                    {
                        returnData = base.biz.doView(ds);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException("View", ex);
                    }
                    break;

                case ACT._DEL:
                    try
                    {
                        returnData = base.biz.doDel(ds);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException("Del", ex);
                    }
                    break;

                case ACT._SEARCH:
                    try
                    {
                        returnData = base.biz.doSearch(ds);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException("Search", ex);
                    }
                    break;

                case ACT._OTHER:
                    try
                    {
                        returnData = base.biz.doOther(ds);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException("Other", ex);
                    }
                    break;
            }
            if ((returnData.Tables["_ERRORINFO"] != null && returnData.Tables["_ERRORINFO"].Rows[0]["_ERROR_MSG"].ToString() != ""))
            {
                AppError.show(returnData.Tables["_ERRORINFO"].Rows[0]["_ERROR_MSG"].ToString(), new Exception(returnData.Tables["_ERRORINFO"].Rows[0]["_ERROR_DETAIL"].ToString()));
                this.isActionSuccess = false;
            }
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
                    try
                    {
                        this.afterSearch(returnData);

                    }
                    catch (Exception ex)
                    {
                        Logger.LogException("执行错误", ex);
                    }
                    return returnData;

                case ACT._OTHER:
                    this.afterOther(returnData, userObj);
                    return returnData;
            }
            return returnData;
        }



        public DataSet doNew(object userObj)
        {
            return this.doAction(ACT._NEW, userObj);
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

        public DataSet doUpdate(object userObj)
        {
            return this.doAction(ACT.UPDATE, userObj);
        }


        public DataSet doView(object userObj)
        {
            return this.doAction(ACT._VIEW, userObj);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCommon));
            this.SuspendLayout();
            // 
            // FrmCommon
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmCommon";
            this.Load += new System.EventHandler(this.FrmCommon_Load);
            this.ResumeLayout(false);

        }

        protected virtual DataSet setToDelDS()
        {
            return new DataSet();
        }

        protected virtual DataSet setToNewDS()
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


        protected virtual DataSet setToUpdateDS()
        {
            return new DataSet();
        }


        protected virtual DataSet setToViewDS()
        {
            return new DataSet();
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

        private void FrmCommon_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            //string style = UserInfo.GetUserConfigValue("MainStyle");
            //if (style != "")
            //{
            //    this.LookAndFeel.SetStyle(DevExpress.LookAndFeel.LookAndFeelStyle.Skin, false, false, style);
            //}
            //else
            //{
            //    this.LookAndFeel.SetStyle(DevExpress.LookAndFeel.LookAndFeelStyle.Skin, false, false, "Office 2010 Blue");
            //}

            this.ImeMode = System.Windows.Forms.ImeMode.OnHalf;
        }

    }
}

