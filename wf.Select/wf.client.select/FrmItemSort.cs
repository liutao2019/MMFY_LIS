using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using System.Collections;
using dcl.client.report;
using dcl.entity;
using dcl.client.wcf;
using DevExpress.XtraReports.UI;
using lis.client.control;
using System.IO;
using dcl.client.common;

namespace dcl.client.resultquery
{
    public partial class FrmItemSort : FrmCommon
    {
        //装载未停用的数据
        List<EntityDicItmItem> listItmItem = new List<EntityDicItmItem>();
        public FrmItemSort()
        {
            InitializeComponent();
        }

        private void FrmItemSort_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnSearch.Name, sysToolBar1.BtnPrint.Name, sysToolBar1.BtnExport.Name, sysToolBar1.BtnClose.Name });

            this.deStart.EditValue = DateTime.Now.Date;
            this.deEnd.EditValue = DateTime.Now.Date;
            listItmItem = this.lueItem.getDataSource();
            listItmItem = listItmItem.FindAll(w => w.ItmDelFlag == "0"); //过滤停用的项目
            this.lueItem.SetFilter(listItmItem);
        }

        private void sysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sysToolBar1_OnBtnSearchClicked(object sender, EventArgs e)
        {
            this.gcSort.Focus();
            if (this.deStart.EditValue == null || this.deEnd.EditValue == null)
            {
                lis.client.control.MessageDialog.Show("请输入起始日期！", "提示");
                return;
            }
            System.TimeSpan ts = Convert.ToDateTime(deEnd.EditValue).Subtract(Convert.ToDateTime(deStart.EditValue));
            int day = ts.Days;
            if (day < 0 || day > 61)
            {
                lis.client.control.MessageDialog.Show("日期范围不能超过两个月且结束日期大于开始日期！", "提示");
                return;
            }
            EntityAnanlyseQC query = GetQcEntity();

            printer = proxy.Service.GetReportData(query);

            if (string.IsNullOrEmpty(printer.ReportName))
            {
                Lib.LogManager.Logger.LogInfo("生成报表错误", string.Format("数据表report中未找到报表"));
                MessageDialog.Show(string.Format("数据表中未找到报表"));
                return;
            }
            string localPath = PathManager.ReportPath;
            string pathStr = localPath + printer.ReportName + printer.ReportSuffix;
            if (!File.Exists(pathStr))
            {
                Lib.LogManager.Logger.LogInfo("生成报表错误", string.Format("报表不存在!{0}", pathStr));
                lis.client.control.MessageDialog.Show(string.Format("报表不存在!"));
                return;
            }
            gcSort.DataSource = printer.ReportData.Tables["ItemSort"];

            foreach (DevExpress.XtraGrid.Columns.GridColumn dc in gridView1.Columns)
            {
                dc.OptionsColumn.AllowEdit = false;
                dc.Width = 75;
            }
        }
        ProxyItemSort proxy = new ProxyItemSort();
        EntityDCLPrintData printer = new EntityDCLPrintData();

        private EntityAnanlyseQC GetQcEntity()
        {

            EntityAnanlyseQC query = new EntityAnanlyseQC();
            query.listSid = new List<EntitySid>();
            query.listSort = new List<EntitySortNo>();
            EntitySid sid = new EntitySid();
            EntitySortNo sortNo = new EntitySortNo();
            if (this.deStart.EditValue != null)
            {
                query.DateStart = Convert.ToDateTime(deStart.EditValue);
            }
            if (this.deEnd.EditValue != null)
            {
                query.DateEnd = Convert.ToDateTime(deEnd.EditValue).AddDays(1);
            }
            if (this.txtSwatchStart.EditValue != null)
            {
                if (Convert.ToInt32(txtSwatchStart.EditValue) > 0)
                {
                    sid.StartSid = Convert.ToInt32(txtSwatchStart.EditValue.ToString());
                }
            }
            if (this.txtSwatchEnd.EditValue != null)
            {
                if (Convert.ToInt32(txtSwatchEnd.EditValue) > 0)
                    sid.EndSid = Convert.ToInt32(txtSwatchEnd.EditValue);
            }
            query.listSid.Add(sid);

            //序号
            if (this.txtXHStart.EditValue.ToString() != "")
            {
                if (Convert.ToInt32(txtXHStart.EditValue) > 0)
                {
                    sortNo.StartNo = Convert.ToInt32(txtXHStart.EditValue);
                }
            }
            if (this.txtXHEnd.EditValue.ToString() != "")
            {
                if (Convert.ToInt32(txtXHEnd.EditValue) > 0)
                {
                    sortNo.EndNo = Convert.ToInt32(txtXHEnd.EditValue);
                }
            }
            query.listSort.Add(sortNo);

            if (this.lueInstrmt.valueMember != null && this.lueInstrmt.valueMember != "")
            {
                query.ItrId = lueInstrmt.valueMember;
            }
            if (this.lueItem.valueMember != null && this.lueItem.valueMember != "")
            {
                query.ItmId = lueItem.valueMember;
            }

            if (this.lueDepart.valueMember != null && this.lueDepart.valueMember != "")
            {
                query.DepId = lueDepart.valueMember;
            }
            return query;
        }



        private void sysToolBar1_OnBtnExportClicked(object sender, EventArgs e)
        {
            if (gcSort.DataSource != null)
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.DefaultExt = "xls";
                ofd.Filter = "Excel文件(*.xls)|*.xls";
                ofd.Title = "导出到Excel";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.Trim() == string.Empty)
                    {
                        lis.client.control.MessageDialog.Show("文件名不能为空！", "提示");
                        return;
                    }

                    try
                    {
                        gcSort.ExportToXlsx(ofd.FileName.Trim());
                        lis.client.control.MessageDialog.Show("导出成功！", "提示");
                    }
                    catch (Exception ex)
                    {
                        Lib.LogManager.Logger.LogException(ex);
                    }
                }

            }
        }

        private void sysToolBar1_OnBtnPrintClicked(object sender, EventArgs e)
        {
            if (printer == null)
                return;
            try
            {
                DCLReportPrint.PrintByData(printer);
            }
            catch (ReportNotFoundException ex1)
            {
                lis.client.control.MessageDialog.Show(ex1.MSG);
            }
            catch (Exception ex2)
            {

            }
        }

        private void sysToolBar1_OnPrintPreviewClicked(object sender, EventArgs e)
        {
            if (printer == null)
                return;
            try
            {
                DCLReportPrint.PrintPreviewByData(printer);
            }
            catch (ReportNotFoundException ex1)
            {
                lis.client.control.MessageDialog.Show(ex1.MSG);
            }
            catch (Exception ex2)
            {

            }
        }

        private void lueInstrmt_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            if (!string.IsNullOrEmpty(lueInstrmt.valueMember))
            {
                List<EntityDicItmItem> listFilter = new List<EntityDicItmItem>();
                listFilter = listItmItem.FindAll(w => w.ItmPriId == lueInstrmt.selectRow.ItrProId);
                this.lueItem.SetFilter(listFilter);
            }
            else
            {
                List<EntityDicItmItem> listFilterAll = new List<EntityDicItmItem>();
                if (listItmItem[0].ItmId == null)
                    listItmItem.Remove(listItmItem[0]);
                listFilterAll = EntityManager<EntityDicItmItem>.ListClone(listItmItem);
                this.lueItem.SetFilter(listFilterAll);
            }
        }

    }
}
