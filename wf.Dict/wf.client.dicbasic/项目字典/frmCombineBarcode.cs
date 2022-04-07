using dcl.client.common;
using dcl.client.frame;
using dcl.entity;
using DevExpress.XtraBars;
using lis.client.control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace dcl.client.dicbasic
{
    public partial class frmCombineBarcode : FrmCommon
    {
        ProxyCommonDic proxy = new ProxyCommonDic("svc.ConItemCombineBarcode");
        public List<EntitySampMergeRule> InputData { get; internal set; }
        public EntityDicCombine inputDataRow { get; internal set; }
        public DataTable inputDataTable { get; internal set; }
        public Control parentControl;
        public frmCombineBarcode()
        {
            InitializeComponent();
        }
        //增加控制列表的项目
        Dictionary<string, Boolean> controlsList = new Dictionary<string, Boolean>();
        public Boolean defaultEnableStatus = false;
   

        private void frmCombineBarcode_Load(object sender, EventArgs e)
        {
            sysToolBar2.SetToolButtonStyle(new string[] { "BtnAdd","BtnModify", "BtnSave", "BtnDelete","BtnCancel", "BtnRefresh" });

            this.bsBCCombine.DataSource = InputData;
            
            parentControl = plCombines;

            EnableControls(parentControl);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            EntityDicCombine drCombine = inputDataRow;
            if (!plCombines.Enabled)
                plCombines.Enabled = true;

            if (drCombine.ComId != null && drCombine.ComId != "")
            {
                //xtraTabControl1.SelectedTabPage = tabBarcodeCombines;
                EntitySampMergeRule drBCCombine = (EntitySampMergeRule)bsBCCombine.AddNew();
                drBCCombine.ComSplitCode = "";
                //绑定编号与费用类别
                drBCCombine.ComId = drCombine.ComId;
                drBCCombine.ComHisFeeCode = drCombine.ComHisCode;
                spinPrintCount.EditValue = 1;//打印次数默认为1
                                             // ceSplit.Checked = true; //拆分标志默认有
                                             //drBCCombine["com_print_name"] = drCombine["com_name"]; //条码打印名称默认为组合名称
                if (cbBarcodeType.SelectedIndex > -1) //默认为0:自动条码  为1时是预置条码
                    drBCCombine.ComBarcodeType = cbBarcodeType.SelectedIndex;
                bsBCCombine.EndEdit();
                bsBCCombine.ResetCurrentItem();
                // SetDefaultOrigin();
                this.cbeSeq.Focus();
            }
            EnterEditingState(true);
            EnableControls(parentControl);
            Edit();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lis.client.control.MessageDialog.Show("是否删除？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }
            else {
                EntitySampMergeRule rule = (EntitySampMergeRule)bsBCCombine.Current;
                EntityRequest request = new EntityRequest();
                if (rule.ComRulId == 0)
                {
                    return;
                }
                request.SetRequestValue(rule);
               EntityResponse response=proxy.Delete(request);
                if (response.Scusess)
                {
                    MessageDialog.ShowAutoCloseDialog("删除成功");
                    //InputData.Remove(rule);
                    //bsBCCombine.DataSource = InputData;
                    //gcBCCombines.RefreshDataSource();
                    LoadBCCombines();
                }

            }
                
        }
        /// <summary>
        ///修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            EnterEditingState(true);
            EnableControls(parentControl);
            Edit();
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save(object sender, EventArgs e)
        {
            this.isActionSuccess = false;
            this.bsBCCombine.EndEdit();

            EntityRequest request = new EntityRequest();

            EntityDicCombine dr = inputDataRow;

            String com_id = dr.ComId;

            List<EntitySampMergeRule> rules = bsBCCombine.DataSource as List<EntitySampMergeRule>;
            var listNew = new List<EntitySampMergeRule>();
            var listUpdate = new List<EntitySampMergeRule>();
            if (rules.Count == 0)
            {
                lis.client.control.MessageDialog.Show("没有保存的项!", "提示");
                return;
            }
            foreach (EntitySampMergeRule item in rules)
            {
                item.ComBarcodeType = cbBarcodeType.SelectedIndex;
                item.ComSortNo = cbeSeq.Text.ToString().Trim();
                item.ComDelFlag = "0";
                if (item.ComRulId == 0)
                {
                    listNew.Add(item);
                }
                else
                    listUpdate.Add(item);
            }

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("new", listNew);
            dict.Add("update", listUpdate);

            request.SetRequestValue(dict);
            EntityResponse result = new EntityResponse();

            result = proxy.Other(request);
            if (result.Scusess)
            {
                lis.client.control.MessageDialog.Show("操作成功");
                EnableButton(false);
                EnterEditingState(false);
                EnableControls(parentControl);
                LoadBCCombines();
            }
            else
            {
                lis.client.control.MessageDialog.Show("操作失败");
            }
            if (this.isActionSuccess)
            {
                EnterEditingState(false);
                EnableControls(parentControl);
            }      
        }
        private void LoadBCCombines()
        {

            if (bsBCCombine.Current != null)
            {
                EntityRequest request = new EntityRequest();
                EntityDicCombine dr =inputDataRow;


                EntityResponse result = new EntityResponse();

                String comId = dr.ComId;

                request.SetRequestValue(comId);

                result = proxy.Search(request);

                if (result.Scusess)
                {
                    List<EntitySampMergeRule> mergeRuleList = result.GetResult() as List<EntitySampMergeRule>;
                    this.bsBCCombine.DataSource = mergeRuleList;
                }
            }
            else
            {
                //bsBCCombine.Filter = "1<>1";
                bsBCCombine.DataSource = new List<EntitySampMergeRule>();
            }

        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            EnterEditingState(false);
            EnableControls(parentControl);
            EnableButton(false);
        }
        /// <summary>
        /// 设置列表控件自带按钮Enable属性
        /// </summary>
        private void EnableButton(bool enable)
        {
            sysToolBar2.BtnSave.Enabled = enable;
            sysToolBar2.BtnAdd.Enabled = !enable;
            sysToolBar2.BtnCancel.Enabled = enable;
            sysToolBar2.BtnModify.Enabled = !enable;
            sysToolBar2.BtnDelete.Enabled = !enable;
        }
        private void onRefresh(object sender, EventArgs e)
        {
            
        }

        public void Edit()
        {
            // gcBCCombines.Enabled = true;
            EnableButton(true);
            //xtraTabControl1.Enabled = true;


            if (gvBCCombines.RowCount == 0)
                plCombines.Enabled = false;
            else
                plCombines.Enabled = true;

            ceFatherFlag.Enabled = ceSplit.Checked;


        }
        //遍历所有控件，以控制所有控件显示是否可编辑状态
        private void EnableControls(Control parentControl)
        {
            foreach (Control c in parentControl.Controls)
            {
                if (c.Name == "cmbSort" || c.Name == "txtSort" || c.Name == "sysItem" || c.Name == "radioGroup_Item" || c.Name == "btnSynchronous" || c.Name == "radioGroup_split" || c.Name == "txtFilter")
                {
                    c.Enabled = true;
                    if (c is DevExpress.XtraEditors.TextEdit)
                    {
                        ((DevExpress.XtraEditors.TextEdit)c).Properties.ReadOnly = false;
                    }
                    continue;
                }

                if (c.Controls.Count > 0)
                {
                    if (c is DevExpress.XtraGrid.GridControl)
                    {
                        if (controlsList.ContainsKey(c.Name))
                            c.Enabled = controlsList[c.Name];
                        else
                        {
                            c.Enabled = defaultEnableStatus;
                            //(c as DevExpress.XtraGrid.GridControl)
                        }
                    }
                    else
                    {
                        if (c is DevExpress.XtraEditors.TextEdit || c is lis.client.control.HopePopSelect)
                        {
                            if (c is DevExpress.XtraEditors.TextEdit)
                            {
                                ((DevExpress.XtraEditors.TextEdit)c).Properties.ReadOnly = !defaultEnableStatus;
                            }

                            if (c is lis.client.control.HopePopSelect)
                            {
                                ((lis.client.control.HopePopSelect)c).Readonly = !defaultEnableStatus;
                            }

                        }
                        else
                        {
                            EnableControls(c);
                        }
                    }
                }
                else
                {
                    if (controlsList.ContainsKey(c.Name))
                        c.Enabled = controlsList[c.Name];
                    else
                    {
                        if (c is DevExpress.XtraEditors.LabelControl)
                            c.Enabled = true;
                        else
                        {
                            if (c is DevExpress.XtraEditors.TextEdit || c is lis.client.control.HopePopSelect)
                            {
                                if (c is DevExpress.XtraEditors.TextEdit)
                                {
                                    ((DevExpress.XtraEditors.TextEdit)c).Properties.ReadOnly = !defaultEnableStatus;
                                }

                                if (c is lis.client.control.HopePopSelect)
                                {
                                    ((lis.client.control.HopePopSelect)c).Readonly = !defaultEnableStatus;
                                }

                            }
                            else
                            {
                                c.Enabled = defaultEnableStatus;
                            }
                        }
                    }
                }
            }
        }

        private void EnterEditingState(Boolean enable)
        {
            defaultEnableStatus = enable;
        }

        /// <summary>
        /// 拆分代码相关的控件Text有变时
        /// </summary>
        /// <param name="oldRow"></param>
        private void AfterSplitCodeControlChange(EntityDicPubProfession oldRow)
        {
            GenerateTypeCode();
        }
        private void selectDict_Cuv1_onAfterChange(EntityDicTestTube oldRow)
        {
            GenerateTypeCode();
        }
        private void selectDict_Sample_type1_onAfterChange(EntityDicSample oldRow)
        {
            GenerateTypeCode();
        }

        private void cbeSeq_SelectedValueChanged(object sender, EventArgs e)
        {
            GenerateTypeCode();
        }
        private void GenerateTypeCode()
        {
            string type = FomartSplitSubCode(selectDict_Type2.valueMember);
            string cuv = FomartSplitSubCode(selectDict_Cuv1.valueMember);
            string sampleType = FomartSplitSubCode(selectDict_Sample_type1.valueMember);
            string seq = FomartSplitSubCode(cbeSeq.Text);
            //全部为空时不生成
            if (string.IsNullOrEmpty(selectDict_Type2.valueMember) && string.IsNullOrEmpty(selectDict_Cuv1.valueMember)
                && string.IsNullOrEmpty(selectDict_Sample_type1.valueMember) && string.IsNullOrEmpty(cbeSeq.Text))
            {
                txtSplitCode.EditValue = "";
                if (bsBCCombine.Current != null)
                {
                    EntitySampMergeRule rule = bsBCCombine.Current as EntitySampMergeRule;
                    rule.ComSplitCode = "";
                }
            }

            else
            {
                txtSplitCode.EditValue = type + cuv + sampleType + seq.Trim();
                if (bsBCCombine.Current != null)
                {
                    EntitySampMergeRule rule = bsBCCombine.Current as EntitySampMergeRule;
                    rule.ComSplitCode = type + cuv + sampleType + seq.Trim();
                }
            }

        }

        /// <summary>
        /// 如果为空时,
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string FomartSplitSubCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return "A";
            else
                return code;
        }
        //选中不同规则是改变条码类型
        public void ChangeSeleteType()
        {
            if (bsBCCombine.Current == null) return;
            EntitySampMergeRule rule = bsBCCombine.Current as EntitySampMergeRule;
            cbBarcodeType.Text = rule.ComBarcodeType.Value == 0 ? "自动条码" : "预置条码";
            if (rule.ComRulId == 0)
                GenerateTypeCode();
        }

        private void ceSplit_CheckedChanged(object sender, EventArgs e)
        {
            if (ceSplit.Checked) //拆分时清空合并控件
            {
                ClearSplitSubCodeControls();
            }
            else
            {
                //如果不拆分时，清掉‘拆分保存大组合(特殊合并)’标记
                ceFatherFlag.Checked = false;
            }

            //需要拆分时不用显示合并代码
            txtSplitCode.Enabled = selectDict_Cuv1.Enabled = selectDict_Sample_type1.Enabled = cbeSeq.Enabled = selectDict_Type2.Enabled = !ceSplit.Checked;
            ceFatherFlag.Enabled = ceSplit.Checked;//‘拆分保存大组合(特殊合并)’标记

        }

        /// <summary>
        /// 清除生成拆分条码的控件文本
        /// </summary>
        private void ClearSplitSubCodeControls()
        {
            selectDict_Type2.ClearSelect();
            selectDict_Cuv1.ClearSelect();
            selectDict_Sample_type1.ClearSelect();
            cbeSeq.Text = "";
            txtSplitCode.Text = "";
        }

        private void gvBCCombines_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            ChangeSeleteType();
        }
    }
}
