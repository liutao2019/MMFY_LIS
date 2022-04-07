using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using dcl.client.frame;
using System.Configuration;
using dcl.common.extensions;
using dcl.client.common;


using System.Collections;
using dcl.common;
using System.IO;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.sample
{
    public partial class FrmBCQuery : FrmCommon
    {
        public FrmBCQuery()
        {
            InitializeComponent();
            this.Shown += FrmBCQuery_Shown;
            SampQueryCondition = new EntitySampQC();
        }

        private void FrmBCQuery_Shown(object sender, EventArgs e)
        {
            string path = PathManager.SettingPath + @"barcodeparam.ini";

            if (File.Exists(path))
            {
                string result = File.ReadAllText(path);

                string[] parm = result.Split(' ');

                if (parm.Length >= 2 && lueDepart.Visible)
                {
                    string strDep = parm[1];
                    lueDepart.SelectByValue(strDep);
                }
            }

            if (LocalSetting.Current.Setting.IDTypeFlag != null && LocalSetting.Current.Setting.IDTypeFlag.Trim() != "")
            {
                string[] ids = LocalSetting.Current.Setting.IDTypeFlag.Split('|')[0].Split(',');

                ArrayList alType = new ArrayList();
                foreach (string id in ids)
                {
                    alType.Add(id);
                }

                lueTypes.valueMember = alType;
                lueTypes.displayMember = LocalSetting.Current.Setting.IDTypeFlag.Split('|')[1];
            }
            else
            {
                if (LocalSetting.Current.Setting.CType_id != null && LocalSetting.Current.Setting.CType_id.Trim() != "")
                {
                    ArrayList alType = new ArrayList();
                    alType.Add(LocalSetting.Current.Setting.CType_id);
                    lueTypes.valueMember = alType;
                    lueTypes.displayMember = LocalSetting.Current.Setting.CType_name;
                }
            }

            cbStatus.Properties.Items.Clear();
            switch (Step.StepName)
            {
                case "采集":
                    cbStatus.Properties.Items.Add("采集");
                    break;
                case "收取":
                    cbStatus.Properties.Items.Add("采集");
                    cbStatus.Properties.Items.Add("二次送检");
                    break;
                case "送达":
                    cbStatus.Properties.Items.Add("收取");
                    cbStatus.Properties.Items.Add("二次送检");
                    break;
                case "签收":
                    cbStatus.Properties.Items.Add("送达");
                    cbStatus.Properties.Items.Add("收取");
                    cbStatus.Properties.Items.Add("二次送检");
                    cbStatus.Properties.Items.Add("签收");
                    break;
                case "二次送检":
                    cbStatus.Properties.Items.Add("签收");
                    break;
                case "离心":
                    cbStatus.Properties.Items.Add("签收");
                    break;
                case "标本上机":
                    cbStatus.Properties.Items.Add("离心");
                    cbStatus.Properties.Items.Add("签收");
                    break;
                case "耗材领取":
                    cbStatus.Properties.Items.Add("条码打印");
                    break;
                default:
                    break;
            }
            cbStatus.SelectedIndex = 0;
            dateSearch.DateTime = ServerDateTime.GetServerDateTime().Date;
            dateEnd.DateTime = dateSearch.DateTime.Date.AddDays(1).AddSeconds(-1);
        }

        public StepType StepType { get; set; }

        /// <summary>
        /// 查询条件
        /// </summary>
        public EntitySampQC SampQueryCondition { get; set; }

        public IStep Step
        {
            get;
            set;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnInquiry_Click(object sender, EventArgs e)
        {
            if (lueDepart.Visible && lueDepart.popupContainerEdit1.Text.Trim() == string.Empty)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("请选择科室！");
                return;
            }

            if (dateSearch.EditValue == null || dateEnd.EditValue == null)
            {
                lis.client.control.MessageDialog.Show("请输入时间!");
                return;
            }

            //判断是否只查询条码最后状态流程,如果不是,则可查询达到这一状态流程的都查出来
            if (checkBox_searchSign.Checked)
                SampQueryCondition.SearchDateType = SearchDataType.标本最后操作时间;
            else
                SampQueryCondition.SearchDateType = SearchDataType.标本生成时间;

            SampQueryCondition.StartDate = this.dateSearch.DateTime.Date.ToString(CommonValue.DateTimeFormat);
            SampQueryCondition.EndDate = this.dateEnd.DateTime.ToString(CommonValue.DateTimeFormat);
            SampQueryCondition.SearchType = cbSignerSearchType.Text;
            SampQueryCondition.SearchValue = txtSigner.Text;
            if (lueTypes.valueMember != null)
            {
                ArrayList alCtype = (ArrayList)lueTypes.valueMember;
                if (alCtype.Count > 0)
                {
                    for (int i = 0; i < alCtype.Count; i++)
                    {
                        SampQueryCondition.ListType.Add(alCtype[i].ToString());
                    }
                }
            }

            SampQueryCondition.PidDeptName = lueDepart.popupContainerEdit1.Text.Trim();

            if (ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_GatherEnableDeptFilter") == "是"
                && StepType == StepType.Sampling
                && selectDict_Depart1.popupContainerEdit1.Text.Trim() != string.Empty)
            {
                SampQueryCondition.PidDeptCode = selectDict_Depart1.valueMember;
            }

            if (checkBox_searchSign.Checked)
            {
                string strStatus = string.Empty;

                if (cbStatus.Visible)
                {
                    switch (cbStatus.Text.Trim())
                    {
                        case "采集":
                            strStatus = EnumBarcodeOperationCode.SampleCollect.ToString();
                            break;
                        case "送达":
                            strStatus = EnumBarcodeOperationCode.SampleReach.ToString();
                            break;
                        case "收取":
                            strStatus = EnumBarcodeOperationCode.SampleSend.ToString();
                            break;
                        case "二次送检":
                            strStatus = EnumBarcodeOperationCode.SampleSecondSend.ToString();
                            break;
                        case "签收":
                            strStatus = EnumBarcodeOperationCode.SampleReceive.ToString();
                            break;
                        case "条码打印":
                            strStatus = EnumBarcodeOperationCode.BarcodePrint.ToString();
                            break;
                        default:
                            break;
                    }
                    SampQueryCondition.ListSampStatusId.Add(strStatus);
                }
                else
                {
                    SampQueryCondition.ListSampStatusId.Add(EnumBarcodeOperationCode.BarcodePrint.ToString());

                    SampQueryCondition.ListSampStatusId.Add(EnumBarcodeOperationCode.SampleReturn.ToString());
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
