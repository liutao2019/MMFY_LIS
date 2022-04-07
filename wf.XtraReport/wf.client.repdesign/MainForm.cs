using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.Skins;
using DevExpress.Utils.Drawing;
using DevExpress.XtraReports.UI;
using dcl.client.frame;
using DevExpress.XtraPrinting;
using dcl.common;
using Lib.LogManager;
using dcl.client.wcf;
using dcl.entity;

namespace dcl.client.repdesign
{
    public partial class MainForm : FrmCommon
    {
        RibbonDemos.SkinGalleryHelper skinGalleryHelper;
        public MainForm()
        {
            InitializeComponent();
            skinGalleryHelper = new RibbonDemos.SkinGalleryHelper(ribbonGallerySkins);
        }

        string id;
        string path;
        string sql;
        string conn_code = null;
        public MainForm(string path, string id, string sql)
            : this(path, id, sql, null)
        {

        }

        public MainForm(string path, string id, string sql, string conncode)
        {
            InitializeComponent();
            this.path = path;
            this.id = id;
            this.sql = sql;
            this.conn_code = conncode == null ? "" : conncode;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                ProxyReportMain proxy = new ProxyReportMain();
                EntityResponse respone = proxy.Service.GetReportPar(id);
                List<EntitySysReportParameter> listPar = respone.GetResult() as List<EntitySysReportParameter>;
                DataSet ds = new DataSet();
                {
                    sql = EncryptClass.Decrypt(sql);
                    if (listPar != null && listPar.Count > 0)
                    { 
                        foreach (EntitySysReportParameter par in listPar)
                        {
                            sql = sql.Replace(par.RepParmType, par.RepParmValue);
                        }
                    }
                    DataTable dtWhere = CommonClient.CreateDT(new string[] { "id", "conn_code" }, "sql");
                    dtWhere.TableName = "sqlWhere";
                    dtWhere.Rows.Add(sql, this.conn_code);
                    DataTable dtData = proxy.Service.GetSqlResult(dtWhere);

                    dtData.TableName = "可设计字段";
                    ds.Tables.Add(dtData);
                }

                skinGalleryHelper = new RibbonDemos.SkinGalleryHelper(ribbonGallerySkins);
                try
                {
                    XtraReport xr = new XtraReport();
                    xr.LoadLayout(path);
                    xr.DataSource = ds;
                    xr.TextAlignment = TextAlignment.MiddleLeft;
                    xrDesignPanel1.OpenReport(xr);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }
            catch (Exception ex1)
            {
                Logger.LogException(ex1);
            }

        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveReport();
        }

        protected override void OnClosing(CancelEventArgs e)
        { }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (xrDesignPanel1.ReportState == DevExpress.XtraReports.UserDesigner.ReportState.Changed)
            {
                if (lis.client.control.MessageDialog.Show("模板已修改,是否保存?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SaveReport();
                }
            }
        }

        private void SaveReport()
        {
            try
            {
                xrDesignPanel1.Report.SaveLayout(path);
                xrDesignPanel1.ReportState = DevExpress.XtraReports.UserDesigner.ReportState.Saved;
                lis.client.control.MessageDialog.Show("保存成功", "提示");
            }
            catch (Exception)
            {
                lis.client.control.MessageDialog.Show("保存失败", "提示");
            }
        }

    }
}
