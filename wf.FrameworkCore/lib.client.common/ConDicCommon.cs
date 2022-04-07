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
    using dcl.entity;

    [DesignTimeVisible(false)]
    public class ConDicCommon : XtraUserControl
    {
        private ProxyCommonDic _biz;
        private IContainer components = null;
        public bool isActionSuccess = true;

        public ConDicCommon()
        {
            this.InitializeComponent();
        }

        public dcl.entity.EntityResponse New(dcl.entity.EntityRequest request)
        {
            return this.doAction("New", request);
        }

        public dcl.entity.EntityResponse Delete(dcl.entity.EntityRequest request)
        {
            return this.doAction("Delete", request);
        }

        public dcl.entity.EntityResponse Update(dcl.entity.EntityRequest request)
        {
            return this.doAction("Update", request);
        }

        public dcl.entity.EntityResponse Search(dcl.entity.EntityRequest request)
        {
            return this.doAction("Search", request);
        }

        public dcl.entity.EntityResponse Other(dcl.entity.EntityRequest request)
        {
            return this.doAction("Other", request);
        }

        public dcl.entity.EntityResponse View(dcl.entity.EntityRequest request)
        {
            return this.doAction("View", request);
        }

        /// <summary>
        /// 具体服务操作
        /// </summary>
        /// <param name="action"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private EntityResponse doAction(string action, EntityRequest request)
        {
            EntityResponse response = new EntityResponse();
            switch (action)
            {
                case "New":
                    response = biz.New(request);
                    break;
                case "Delete":
                    response = biz.Delete(request);
                    break;
                case "Update":
                    response = biz.Update(request);
                    break;
                case "Search":
                    response = biz.Search(request);
                    break;
                case "Other":
                    response = biz.Other(request);
                    break;
                case "View":
                    response = biz.View(request);
                    break;
                default:
                    break;
            }

            this.isActionSuccess = response.Scusess;

            return response;
        }

        /// <summary>
        /// 关闭的时候，断开连接，释放资源
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
                //this.biz.Close();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 初始化布局
        /// </summary>
        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Name = "ConCommon";
            base.ResumeLayout(false);
        }

        /// <summary>
        /// 获取通用服务端
        /// </summary>
        public ProxyCommonDic biz
        {
            get
            {
                if (this._biz == null)
                {
                    ProxyCommonDic proxy = new ProxyCommonDic("svc." + base.Name);
                    this._biz = proxy;
                }
                return this._biz;
            }
        }
    }
}

