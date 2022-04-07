using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using dcl.client.wcf;

using dcl.client.frame;
using dcl.client.common;
using lis.client.control;
using dcl.client.report;
using dcl.entity;

namespace dcl.client.samstock
{
    public partial class FrmPrintTempBarCode : FrmCommon
    {
        public string RackBarCode { get; set; }
        private string cuvShelfID;
        private string opName;
        private string TypeName;
        private string Typeid;
        public FrmPrintTempBarCode(string barCode, string cuvShelfTxt, string cuvShelfValue, string typename, string typeid, string operatorNameForBatch)
        {
            InitializeComponent();
            txtBarcode.EditValue = barCode;
            //txtRemark.EditValue = cuvShelfTxt;
            this.cuvShelfID = cuvShelfValue;
            opName = operatorNameForBatch;
            TypeName = typename;
            Typeid = typeid;
            txtCtype.Text = typename;
            dateOpTime.EditValue = DateTime.Now;
            this.btnPrint.Focus();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DialogResult=DialogResult.Cancel;
            Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintBarCode(txtBarcode.Text);
        }

        public void PrintBarCode(string rackbarcode)
        {
            try
            {
                string prtTemplate = "Temp_Rack_Barcode";
                string machineName = GetPrintMachineName();
                List<string> barcodeList = new List<string>();
                FrmReportPrint pForm = new FrmReportPrint();
                ProxySamManage proxy = new ProxySamManage();
                bool isSave = false;
                RackBarCode = rackbarcode;
                if (!string.IsNullOrEmpty(rackbarcode) && proxy.Service.IsRaclBarCodeExists(rackbarcode))
                {
                    isSave = true;
                    //if (proxy.Service.IsRaclBarCodePrint(rackbarcode))
                    //{
                    DialogResult result = MessageDialog.Show("此架子号已经存在，是否打印标签！", "提示", MessageBoxButtons.YesNo,
                                                                 MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        if (result != DialogResult.Yes)
                        {
                            return;
                        }
                    //}
                }
                if (string.IsNullOrEmpty(rackbarcode))
                {
                     rackbarcode = proxy.Service.GetNextMaxBarCode();
                    txtBarcode.Text = rackbarcode;
                }
                barcodeList.Add(rackbarcode);
                string maxID = proxy.Service.GetMaxRack();
                foreach (string barcode in barcodeList)
                {
                    //Hashtable htPatWhere = new Hashtable();

                    //htPatWhere.Add("&barcode&", barcode);
                    //htPatWhere.Add("&remark&", "");
                    //htPatWhere.Add("&ctypename&", TypeName);
                    //htPatWhere.Add("&opname&", opName);
                    //htPatWhere.Add("&optime&", ((DateTime) dateOpTime.EditValue).ToString("yyyy-MM-dd HH:mm:ss"));

                    //Hashtable htPrint = new Hashtable();
                    //htPrint.Add(prtTemplate, htPatWhere);
                    //pForm.Print2(htPrint, machineName);

                    EntityDCLPrintParameter printPara = new EntityDCLPrintParameter();
                    printPara.ReportCode = prtTemplate;
                    printPara.ListRackBarCode.Add(barcode);

                    DCLReportPrint.Print(printPara);
                }
                if (!isSave)
                {
                    string maxRackCode = proxy.Service.GetMaxRackCode();
                    EntityDicSampTubeRack entity = new EntityDicSampTubeRack();
                    entity.RackCode = (int.Parse(maxRackCode) + 1).ToString();
                    entity.RackSpec = cuvShelfID;
                    entity.RackType = Typeid;
                    entity.RackStatus = 0;
                    entity.RackColour = "无";
                    entity.RackBarcode = rackbarcode;

                    entity.RackId = maxID;

                    bool intRet = proxy.Service.InsertIntoRack(entity);

                    if (intRet)
                    {
                        EntitySampStoreRack SamEntity = new EntitySampStoreRack();
                        SamEntity.SrId = proxy.Service.GetMaxSamRackID();
                        SamEntity.SrRackId = entity.RackId;
                        SamEntity.SrStatus = entity.RackStatus;
                        SamEntity.SrAmount = 0;
                        intRet = proxy.Service.InsertSamRack(SamEntity);

                        if (false)
                        {
                            lis.client.control.MessageDialog.Show("添加一条记录到对应的SamStore_Rack表失败！", "提示", MessageBoxButtons.OK,
                                                       MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            return;
                        }
                    }
                }


                RackBarCode = rackbarcode;
                DialogResult = DialogResult.OK;
                this.Close();
                proxy.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        string xmlFile = PathManager.SettingLisPath + @"\printXml\barcodePrintConfig.xml";
        /// <summary>
        /// 获取条码打印机名称
        /// </summary>
        /// <returns></returns>
        private string GetPrintMachineName()
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
                        return dt.Rows[0]["printName"].ToString();
                    }
                }
            }

            return "";
        }

        private void FrmPrintTempBarCode_Shown(object sender, EventArgs e)
        {
            ProxySamManage proxy = new ProxySamManage();
            if (string.IsNullOrEmpty(txtBarcode.Text))
            {
                txtBarcode.Text = proxy.Service.GetNextMaxBarCode();
            }
            proxy.Dispose();
        }

    }
}
