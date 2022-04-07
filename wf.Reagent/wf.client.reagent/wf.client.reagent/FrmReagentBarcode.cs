using DevExpress.XtraGrid;
using Lis.CustomControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using System.Threading;
using System.IO;
using dcl.client.frame;
using dcl.client.common;
using dcl.client.report;
using lis.client.control;
using dcl.entity;
using dcl.root.logon;
using dcl.client.wcf;
using dcl.client.cache;

namespace wf.client.reagent
{
    public partial class FrmReagentBarcode : FrmCommon
    {
        string xmlFile = PathManager.SettingLisPath + @"\printXml\barcodePrintConfig.xml";
        internal GridCheckSelection Selection { get; set; }
        public FrmReagentBarcode()
        {
            InitializeComponent();

            this.sysToolBar1.OnBtnSearchClicked += SysToolBar1_OnBtnSearchClicked;
            this.sysToolBar1.OnCloseClicked += SysToolBar1_OnCloseClicked;
            sysToolBar1.BtnPrintSetClick += new System.EventHandler(this.sysToolBar1_BtnPrintSetClick);
            sysToolBar1.OnBtnBCPrintClicked += new System.EventHandler(this.sysToolBar1_OnBtnBCPrintClicked);

        }
        private void InitSelection()
        {
            gvReadetail.ExpandAllGroups();
            Selection = new GridCheckSelection(gvReadetail);
            Selection.CheckMarkColumn.Width = 20;
            Selection.CheckMarkColumn.VisibleIndex = 0;
        }
        /// <summary>
        /// 打印条码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnBCPrintClicked(object sender, EventArgs e)
        {
            bool printBarcodeSuccess;
            try
            {
                printBarcodeSuccess = PrintBarcodeWithMachine();
                if (!printBarcodeSuccess)
                {
                    ShowAndClose("打印条码失败");
                    return;
                }
            }
            catch (ReportNotFoundException ex)
            {
                ShowMessage("打印条码失败:" + ex.MSG);
                return;
            }
            catch (BarcodePrinterNotFoundException ex1)
            {
                ShowMessage("打印条码失败:" + ex1.Message);
                return;
            }

            int focusedRowHandle = gvReadetail.FocusedRowHandle;
            if (sender != null)
            {
                MoveNext(focusedRowHandle);
            }
            else
            {
                MovePreBarcode();
            }
            if (printBarcodeSuccess)
                Selection.ClearSelection();
        }
        public void ShowAndClose(string msg)
        {
            MessageDialog.ShowAutoCloseDialog(msg, 1m);
        }
        private void ShowMessage(string word)
        {
            MessageDialog.Show(word, "提示");
        }
        /// <summary>
        /// 跳到下一条
        /// </summary>
        public void MoveNext(int focusedRowHandle)
        {
            //光标移动到未打印条码
            if (gvReadetail.FocusedRowHandle != gvReadetail.DataRowCount - 1)
            {
                for (int i = focusedRowHandle + 1; i < gvReadetail.RowCount; i++)
                {
                    EntityReaStorageDetail dr = (EntityReaStorageDetail)this.gvReadetail.GetRow(i);

                    gvReadetail.FocusedRowHandle = i;
                    break;
                }
            }
            else
            {
                gvReadetail.FocusedRowHandle = focusedRowHandle;
                gvReadetail.RefreshData();
            }
        }
        private bool HasNotChoose()
        {
            return Selection.SelectedCount <= 0;
        }
        /// <summary>
        /// 获取条码打印机名称
        /// </summary>
        /// <returns></returns>
        private string GetPrintMachineName(string printField = "printName")
        {
            if (File.Exists(xmlFile))
            {
                DataSet dsPrint = new DataSet();
                dsPrint.ReadXml(xmlFile);
                if (dsPrint.Tables.Count > 0)
                {
                    DataTable dt = dsPrint.Tables[0];
                    if (dt != null)
                    {
                        return dt.Rows[0][printField].ToString();
                    }
                }
            }

            return "";
        }
        /// <summary>
        /// 调用打印机打印条码
        /// </summary>
        private bool PrintBarcodeWithMachine()
        {
            string printMachineName = GetPrintMachineName();
            if (string.IsNullOrEmpty(printMachineName))
            {
                throw new BarcodePrinterNotFoundException();

            }
            ZeBraPrinter machine = new ZeBraPrinter();
            bool result = false;
            try
            {
                string template = "ReagentBarcode";
                if (HasNotChoose())
                {
                    MessageDialog.Show("没有选择条码", "提示");
                    return false;
                }
                List<EntityReaStorageDetail> rows = Selection.GetAllSelectT<EntityReaStorageDetail>();

                List<string> listBarId = new List<string>();

                foreach (EntityReaStorageDetail item in rows)
                {

                    listBarId.Add(item.Rsd_barcode.ToString());
                }

                machine.PrintInfo = new PrintInfo(listBarId);
                result = machine.PrintReaBarcode(printMachineName, template);

                if (result)
                {

                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowAutoCloseDialog("打印出错:" + ex.Message);
                Logger.WriteException("条码", "条码调用打印器打印", ex.Message + "\r\n" + ex.StackTrace);
                return false;
            }
            return result;
        }
        public void MovePreBarcode()
        {
            for (int i = 0; i < gvReadetail.RowCount; i++)
            {
                EntityReaStorageDetail dr = (EntityReaStorageDetail)this.gvReadetail.GetRow(i);

                if (dr.Rsd_barcode != string.Empty)
                { }
                else
                {
                    gvReadetail.FocusedRowHandle = i;
                    break;
                }
            }
        }
        /// <summary>
        /// 条码机设置
        /// </summary>
        private void sysToolBar1_BtnPrintSetClick(object sender, EventArgs e)
        {
            FrmPrintConfigurationV2 configer = new FrmPrintConfigurationV2();
            configer.ShowDialog();
        }
        private void SysToolBar1_OnBtnSearchClicked(object sender, EventArgs e)
        {
            EntityReaQC qc = new EntityReaQC();
            qc.DateStart = dtBegin.DateTime;
            qc.DateEnd = dtEnd.DateTime;
            qc.SupId = selectDicReaSupplier1.valueMember;
            qc.ReaId = selectDicReaSetting1.valueMember;
            qc.GrpId = selectDicReaGroup1.valueMember;
            qc.Barcode = txtBarcode.Text;
            qc.BatchNo = txtBatchNo.Text;
            qc.ReaNo = txtNo.Text;
            qc.WithTime = true;
            System.TimeSpan ts = Convert.ToDateTime(dtEnd.EditValue).Subtract(Convert.ToDateTime(dtBegin.EditValue));
            if (ts.Days > 30)
            {
                MessageDialog.Show("查询时间间隔不能超过30天！", "提示");
                return;
            }
            List<EntityReaStorageDetail> list = new ProxyReaStorageDetail().Service.GetDetail(qc);

            gcReaDetail.DataSource = list;
            gcReaDetail.RefreshDataSource();
        }

        private void SysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            this.Close();
        }
 
        private void FrmReagentBarcode_Load(object sender, EventArgs e)
        {

            dtBegin.EditValue = DateTime.Now;
            dtEnd.EditValue = DateTime.Now;
            InitSelection();

            DateTime dtServer = ServerDateTime.GetServerDateTime();
            dtBegin.EditValue = dtServer.AddDays(-30);
            sysToolBar1.OrderCustomer = true;

            sysToolBar1.SetToolButtonStyle(new string[] {
                sysToolBar1.BtnSearch.Name,
                sysToolBar1.BtnBCPrint.Name,
                sysToolBar1.BtnReset.Name,
                sysToolBar1.BtnPrintSet.Name,
                sysToolBar1.BtnClose.Name});

        }
    }
}
