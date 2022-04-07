using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.common;
using Lib.LogManager;
using DevExpress.XtraBars;
using System.Reflection;

namespace dcl.client.common
{
    public partial class SysToolBar : UserControl
    {
        public SysToolBar()
        {
            InitializeComponent();
            NotWriteLogButtonNameList = new List<string>();
        }

        public List<string> NotWriteLogButtonNameList { get; set; }
        System.Windows.Forms.Form parentForm;

        #region 控件载入事件


        /// <summary>
        /// 控件载入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SysToolBar_Load(object sender, EventArgs e)
        {
            if (!DesignMode && firstLoad)
            {
                //BtnPreAudit.Image = BtnQualityAudit.Image;
                parentForm = this.FindForm();
                bar1.Appearance.BackColor = Color.Transparent;
                foreach (LinkPersistInfo item in this.bar1.LinksPersistInfo)
                {
                    //默认不显示
                    item.Item.Visibility =BarItemVisibility.Never;
                    BarLargeButtonItem bitem = (BarLargeButtonItem)item.Item;
                    bitem.ItemClick += Bitem_ItemClick;
                }

                //初始化按钮状态
                if (autoEnableButtons)
                {
                    EnableButton(false);
                }

                //注册主窗体事件
                if (quickOption)
                {
                    parentForm.KeyPreview = true;
                    ClearEvent(parentForm, "KeyDown");
                    parentForm.KeyDown += new KeyEventHandler(parentForm_KeyDown);
                }
            }
            firstLoad = false;

            if (Height < 50) Height = 60;
        }

        private void Bitem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if ((parentForm is FrmCommon && ((FrmCommon)parentForm).isActionSuccess) || (parentForm is FrmCommonExt && ((FrmCommonExt)parentForm).isActionSuccess))
            {
                BarLargeButtonItem item = (BarLargeButtonItem)e.Item;

                //关闭窗体不记录日志,其他不需记录日志的按钮也参照此处理
                if (item != BtnClose
                    && !NotWriteLogButtonNameList.Contains(item.Name)
                    && UserInfo.GetSysConfigValue("SaveOperationLog") == "是")
                {
                    //保存操作日志
                    ProxySystemLog log = new ProxySystemLog();
                    log.InsertSystemLog(item.Caption, parentForm.Text, UserInfo.loginID, UserInfo.ip, UserInfo.mac, item.Name);
                    LogMessage = "";
                }
            }
        }

        void ClearEvent(Control control, string eventname)
        {
            if (control == null) return;
            if (string.IsNullOrEmpty(eventname)) return;

            BindingFlags mPropertyFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic;
            BindingFlags mFieldFlags = BindingFlags.Static | BindingFlags.NonPublic;
            Type controlType = typeof(System.Windows.Forms.Control);
            PropertyInfo propertyInfo = controlType.GetProperty("Events", mPropertyFlags);
            EventHandlerList eventHandlerList = (EventHandlerList)propertyInfo.GetValue(control, null);
            FieldInfo fieldInfo = (typeof(Control)).GetField("Event" + eventname, mFieldFlags);
            Delegate d = eventHandlerList[fieldInfo.GetValue(control)];

            if (d == null) return;
            EventInfo eventInfo = controlType.GetEvent(eventname);

            foreach (Delegate dx in d.GetInvocationList())
                eventInfo.RemoveEventHandler(control, dx);

        }
        bool firstLoad = true;


        public string LogMessage = "";
        public string LogType = "";


        private void SysToolBar_VisibleChanged(object sender, EventArgs e)
        {
            if (autoEnableButtons)
            {
                EnableButton(false);
            }
        }


        #endregion

        public new bool DesignMode
        {
            get { return System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToLower() == "devenv"; }
        }

        #region 菜单快捷键,使用按钮的TAG值控制
        /// <summary>
        /// 菜单快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parentForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                foreach (LinkPersistInfo item in this.bar1.LinksPersistInfo)
                {
                    if (item.Item.Visibility==BarItemVisibility.Always && item.Item.Tag != null)
                    {
                        if (e.KeyCode.ToString() == item.Item.Tag.ToString())
                        {
                            item.Item.PerformClick();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("parentForm_KeyDown", ex);
            }
        }

        /// <summary>
        /// 是否允许新增/编辑/保存/放弃 按钮使用快捷键。
        /// </summary>
        [Description("是否允许新增/编辑/保存/放弃 按钮使用快捷键。")]
        [DefaultValue(true)]
        public bool QuickOption
        {
            get { return quickOption; }
            set { quickOption = value; }
        }

        private bool quickOption = true;
        #endregion

        #region 设置要显示的工具栏按钮并检查权限

        /// <summary>
        /// 设置要显示的工具栏按钮
        /// </summary>
        /// <param name="toolButtons">示例 SetToolButtonStyle(new string[] { "BtnAdd", "BtnModify", "BtnDelete"});</param>
        public void SetToolButtonStyle(string[] toolButtons)
        {
            SetToolButtonStyle(toolButtons, null);
        }

        /// <summary>
        /// 设置要显示的工具栏按钮示例 sysToolBar1.SetToolButtonStyle(new string[] { "BtnModify", "BtnAdd" }, new string[] { "F3", "F4" });
        /// </summary>
        /// <param name="toolButtons">按钮名列表</param>
        /// <param name="shortcuts">自定义快捷键</param>
        public void SetToolButtonStyle(string[] toolButtons, string[] shortcuts)
        {
            if (DesignMode)
                return;

            for (int i = 0; i < toolButtons.Length; i++)
            {
                foreach (LinkPersistInfo Sitem in bar1.LinksPersistInfo)
                {
                    var bitem = Sitem.Item;
                    if (bitem.Name== toolButtons[i])
                    {
                        if (AutoShortCuts)//自动快捷键
                        {
                            bitem.Tag = "F" + (i + 1).ToString();
                        }
                        else if 
                            (shortcuts != null && shortcuts.Length > 0 && shortcuts.Length >= i + 1)
                            bitem.Tag = shortcuts[i];


                        if (parentForm != null )//&& parentForm.Tag != null
                        {
                            FormatItem(bitem);
                        }
                        if (OrderCustomer)//指定排序位置
                        {
                            bar1.RemoveLink(Sitem.Link);
                            bar1.AddItem(bitem);
                        }

                        break;

                    }
                }
            }
            //显示"关闭"按钮
            if (AutoCloseButton)
            {
                FormatItem(this.BtnClose);
                try
                {
                    bar1.RemoveLink(BtnClose.Links[0]);
                    bar1.AddItem((BarItem)BtnClose);
                }
                catch
                { }
            }
        }

        bool orderCustomer = false;
        bool autoShortCuts = false;
        /// <summary>
        /// 是否自动添加快捷键，除关闭按钮。
        /// </summary>
        [Description("是否自动添加快捷键，除关闭按钮。")]
        [DefaultValue(false)]
        public bool AutoShortCuts { get { return autoShortCuts; } set { autoShortCuts = value; } }


        /// <summary>
        /// 指定位置排序(默认为False),若为True,将按SetToolButtonStyle(string[])的string[]参数顺序排序;若为False,将按此控件的Item默认顺序排序
        /// </summary>
        [Description("指定位置排序(默认为False),若True,按SetToolButtonStyle(string[])的string[]参数顺序排序;若False,按此控件的Items默认顺序排序")]
        [DefaultValue(false)]
        public bool OrderCustomer { get { return orderCustomer; } set { orderCustomer = value; } }

        private bool autoCloseButton = true;
        [Description("是否自动生成关闭按钮(默认为True)")]
        public bool AutoCloseButton { get { return autoCloseButton; } set { autoCloseButton = value; } }
        /// <summary>
        /// 格式化Item
        /// </summary>
        /// <param name="item">按钮</param>
        private void FormatItem(BarItem item)
        {
            item.Visibility =BarItemVisibility.Always;

            if (item.Tag != null && item.Tag.ToString() != "")
            {
                int index = item.Caption.IndexOf("(");
                if (index != -1)
                {
                    item.Caption = item.Caption.Substring(0, index);
                }
                if (quickOption == true)
                {
                    item.Caption = item.Caption + "(" + item.Tag.ToString() + ")";
                }
            }
        }

        public bool CheckPower = true;

        /// <summary>
        /// 设置要显示的工具栏按钮,默认为"BtnAdd", "BtnModify", "BtnDelete", "BtnSave", "BtnCancel", "BtnRefresh" 
        /// </summary>      
        public void SetToolButtonStyle()
        {
            SetToolButtonStyle(new string[] { "BtnAdd", "BtnModify", "BtnDelete", "BtnSave", "BtnCancel", "BtnRefresh" });
        }


        #endregion

        #region 按钮事件与公共事件关联
        /// <summary>
        /// 是否允许新增/编辑/保存/放弃 按钮的相互控制(自动Enable)。
        /// </summary>
        [Description("是否允许新增/编辑/保存/放弃 按钮的相互控制(自动Enable)。")]
        [DefaultValue(true)]
        public bool AutoEnableButtons
        {
            get { return autoEnableButtons; }
            set { autoEnableButtons = value; }
        }

        private bool autoEnableButtons = true;

        /// <summary>
        /// 重设置新增/编辑/保存/放弃 按钮的相互状态
        /// </summary>
        public void EnableButton(Boolean enable)
        {
            BtnAdd.Enabled = !enable;
            BtnModify.Enabled = !enable;
            BtnRefresh.Enabled = !enable;
            BtnDelete.Enabled = !enable;
            BtnSave.Enabled = enable;
            BtnCancel.Enabled = enable;
        }


        #endregion


        public bool ShowItemToolTips
        {
            get
            {
                return false;
            }
            set
            {
                //this.bar1. = value;
            }
        }


   
        /// <summary>
        /// 过号
        /// </summary>
        public event EventHandler OnBtnOverNumberClicked;
        /// <summary>
        /// 过号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOverNumber_Click(object sender, EventArgs e)
        {
            if (OnBtnOverNumberClicked != null)
            {
                OnBtnOverNumberClicked(sender, e);
            }
        }

        /// <summary>
        /// 过号
        /// </summary>
        public event EventHandler OnBtnHeavyCallClicked;
        /// <summary>
        /// 过号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnHeavyCall_Click(object sender, EventArgs e)
        {
            if (OnBtnHeavyCallClicked != null)
            {
                OnBtnHeavyCallClicked(sender, e);
            }
        }


        /// <summary>
        /// 按钮事件_查询
        /// </summary>
        public event EventHandler OnBtnSearchClicked;
        private void BtnSearch_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnSearchClicked != null)
            {
                this.OnBtnSearchClicked(sender, e);
            }
        }

        /// <summary>
        /// 按钮事件_新增
        /// </summary>
        public event EventHandler OnBtnAddClicked;
        private void BtnAdd_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnAddClicked != null)
            {
                this.OnBtnAddClicked(sender, e);
            }

            if (autoEnableButtons)
            {
                EnableButton(true);
            }
        }

        /// <summary>
        /// 按钮事件_编辑
        /// </summary>
        public event EventHandler OnBtnModifyClicked;
        private void BtnModify_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (parentForm is FrmCommon)
            {
                ((FrmCommon)parentForm).isActionSuccess = true;
            }

            if (OnBtnModifyClicked != null)
            {
                this.OnBtnModifyClicked(sender, e);
            }

            if (autoEnableButtons)
            {
                if (parentForm is FrmCommon && ((FrmCommon)parentForm).isActionSuccess)
                {
                    EnableButton(true);
                }
            }
        }


        /// <summary>
        /// 按钮事件-打印条码
        /// </summary>
        public event EventHandler OnBtnBCPrintClicked;
        private void btnBCPrint_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnBCPrintClicked != null)
            {
                this.OnBtnBCPrintClicked(sender, e);
            }
        }

        /// <summary>
        /// 打印条码回执事件
        /// </summary>
        public event EventHandler OnBtnBCPrintReturnClicked;
        private void BtnBCPrintReturn_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnBCPrintReturnClicked != null)
            {
                this.OnBtnBCPrintReturnClicked(sender, e);
            }
        }

        /// <summary>
        /// 计算
        /// </summary>
        public event EventHandler BtnCalculationClick;
        private void btnCalculation_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BtnCalculationClick != null)
            {
                this.BtnCalculationClick(sender, e);
            }
        }

        /// <summary>
        /// 按钮事件-打印清单
        /// </summary>
        public event EventHandler OnBtnPrintListClicked;
        private void barLargeButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnPrintListClicked != null)
            {
                this.OnBtnPrintListClicked(sender, e);
            }
        }

        /// <summary>
        /// 按钮事件-打印细菌清单
        /// </summary>
        public event EventHandler OnBtnPrintListGermClicked;
        private void btnPrintListGerm_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnPrintListGermClicked != null)
            {
                this.OnBtnPrintListGermClicked(sender, e);
            }
        }

        /// <summary>
        /// 按钮事件-酶标分析－结果判定
        /// </summary>
        public event EventHandler OnBtnResultJudgeClicked;
        private void BtnResultJudge_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnResultJudgeClicked != null)
            {
                this.OnBtnResultJudgeClicked(sender, e);
            }
        }


        /// <summary>
        /// 按钮事件_保存
        /// </summary>
        public event EventHandler OnBtnSaveClicked;
        private void BtnSave_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (parentForm is FrmCommon)
            {
                ((FrmCommon)parentForm).isActionSuccess = false;
            }

            if (OnBtnSaveClicked != null)
            {
                this.OnBtnSaveClicked(sender, e);
            }

            if (autoEnableButtons)
            {
                if (parentForm is FrmCommon && ((FrmCommon)parentForm).isActionSuccess)
                {
                    EnableButton(false);
                }
            }
        }

        /// <summary>
        /// 保存模板
        /// </summary>
        public event EventHandler BtnSaveTemplateClick;
        private void btnSaveTemplate_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BtnSaveTemplateClick != null)
            {
                this.BtnSaveTemplateClick(sender, e);
            }
        }


        /// <summary>
        /// 按钮事件-统计
        /// </summary>
        public event EventHandler OnBtnStatClicked;
        private void BtnStat_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnStatClicked != null)
            {
                this.OnBtnStatClicked(sender, e);
            }
        }


        /// <summary>
        /// 修正
        /// </summary>
        public event EventHandler BtnAmendmentClick;
        private void btnAmendment_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BtnAmendmentClick != null)
            {
                this.BtnAmendmentClick(sender, e);
            }
        }

        /// <summary>
        /// 按钮事件_删除
        /// </summary>
        public event EventHandler OnBtnDeleteClicked;
        private void BtnDelete_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnDeleteClicked != null)
            {
                this.OnBtnDeleteClicked(sender, e);
            }

            if (autoEnableButtons)
            {
                EnableButton(false);
            }
        }


        /// <summary>
        /// 按钮事件_批量删除
        /// </summary>
        public event EventHandler OnBtnDeleteBatchClicked;
        private void btnDeleteBatch_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnDeleteBatchClicked != null)
            {
                this.OnBtnDeleteBatchClicked(sender, e);
            }

            if (autoEnableButtons)
            {
                EnableButton(false);
            }
        }


        /// <summary>
        /// 删除子项
        /// </summary>
        public event EventHandler BtnDeleteSubClick;
        private void BtnDeleteSub_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BtnDeleteSubClick != null)
            {
                this.BtnDeleteSubClick(sender, e);
            }
        }


        /// <summary>
        /// 按钮事件_放弃
        /// </summary>
        public event EventHandler OnBtnCancelClicked;
        private void BtnCancel_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnCancelClicked != null)
            {
                this.OnBtnCancelClicked(sender, e);
            }

            if (autoEnableButtons)
            {
                EnableButton(false);
            }
        }


        /// <summary>
        /// 按钮事件_刷新
        /// </summary>
        public event EventHandler OnBtnRefreshClicked;
        private void BtnRefresh_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnRefreshClicked != null)
            {
                this.OnBtnRefreshClicked(sender, e);
            }

            if (autoEnableButtons)
            {
                EnableButton(false);
            }
        }

        /// <summary>
        /// 按钮事件_上一条
        /// </summary>
        public event EventHandler OnBtnPageUpClicked;
        private void BtnPageUp_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnPageUpClicked != null)
            {
                this.OnBtnPageUpClicked(sender, e);
            }
        }


        /// <summary>
        /// 按钮事件_下一条
        /// </summary>
        public event EventHandler OnBtnPageDownClicked;
        private void BtnPageDown_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnPageDownClicked != null)
            {
                this.OnBtnPageDownClicked(sender, e);
            }
        }


        /// <summary>
        /// 按钮事件-单个审核按钮
        /// </summary>
        public event EventHandler OnBtnSingleAuditClicked;
        private void BtnSingleAudit_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnSingleAuditClicked != null)
            {
                this.OnBtnSingleAuditClicked(sender, e);
            }
        }


        /// <summary>
        /// 按钮事件-确认
        /// </summary>
        public event EventHandler OnBtnConfirmClicked;
        private void BtnConfirm_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnConfirmClicked != null)
            {
                this.OnBtnConfirmClicked(sender, e);
            }
        }

        /// <summary>
        /// 按钮事件-审核
        /// </summary>
        public event EventHandler OnAuditClicked;
        private void BtnAudit_ButtonClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnAuditClicked != null)
            {
                this.OnAuditClicked(sender, e);
            }
        }

        /// <summary>
        /// 按钮事件-反审核
        /// </summary>
        public event EventHandler OnUndoAuditClicked;
        private void BtnUndoAudit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnUndoAuditClicked != null)
            {
                this.OnUndoAuditClicked(sender, e);
            }
        }

        /// <summary>
        /// 按钮事件-报告
        /// </summary>
        public event EventHandler OnReportClicked;
        private void BtnReport_ButtonClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnReportClicked != null)
            {
                this.OnReportClicked(sender, e);
            }
        }

        /// <summary>
        /// 按钮事件-取消报告
        /// </summary>
        public event EventHandler OnUndoReportClicked;
        private void BtnUndoReport_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnUndoReportClicked != null)
            {
                this.OnUndoReportClicked(sender, e);
            }
        }


        /// <summary>
        /// 按钮事件-样本进程
        /// </summary>
        public event EventHandler OnSampleMonitorClicked;
        private void BtnSampleMonitor_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnSampleMonitorClicked != null)
            {
                this.OnSampleMonitorClicked(sender, e);
            }
        }

        /// <summary>
        /// 拆分条码
        /// </summary>
        public event EventHandler BtnSperateBarcodeClick;
        private void BtnSperateBarcode_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BtnSperateBarcodeClick != null)
            {
                this.BtnSperateBarcodeClick(sender, e);
            }
        }

        /// <summary>
        /// 撤消事件
        /// </summary>
        public event EventHandler BtnUndoClick;
        private void BtnUndo_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BtnUndoClick != null)
            {
                this.BtnUndoClick(sender, e);
            }
        }

        /// <summary>
        /// 撤消事件
        /// </summary>
        public event EventHandler BtnUndo2Click;
        private void BtnUndo2_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BtnUndo2Click != null)
            {
                this.BtnUndo2Click(sender, e);
            }
        }


        /// <summary>
        /// 按钮事件-取消
        /// </summary>
        public event EventHandler OnBtnReturnClicked;
        private void btnReturn_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnReturnClicked != null)
            {
                this.OnBtnReturnClicked(sender, e);
            }
        }

        /// <summary>
        /// 按钮事件-获得服务器版本
        /// </summary>
        public event EventHandler BtnGetVersionClick;
        private void BtnGetVersion_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BtnGetVersionClick != null)
            {
                this.BtnGetVersionClick(sender, e);
            }
        }


        /// <summary>
        /// 清除事件
        /// </summary>
        public event EventHandler BtnClearClick;
        private void BtnClear_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BtnClearClick != null)
            {
                this.BtnClearClick(sender, e);
            }
        }


        /// <summary>
        /// 按钮事件-设计
        /// </summary>
        public event EventHandler BtnDesignClick;
        private void BtnDesign_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BtnDesignClick != null)
            {
                this.BtnDesignClick(sender, e);
            }
        }


        /// <summary>
        /// 按钮事件-结果视窗
        /// </summary>
        public event EventHandler OnResultViewClicked;
        private void BtnResultView_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnResultViewClicked != null)
            {
                this.OnResultViewClicked(sender, e);
            }
        }


        /// <summary>
        /// 选择抗生素
        /// </summary>
        public event EventHandler BtnAntibioticsClick;
        private void btnAntibiotics_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BtnAntibioticsClick != null)
            {
                this.BtnAntibioticsClick(sender, e);
            }
        }


        /// <summary>
        /// 按钮事件-重置
        /// </summary>
        public event EventHandler BtnResetClick;
        private void BtnReset_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BtnResetClick != null)
            {
                this.BtnResetClick(sender, e);
            }
        }


        /// <summary>
        /// 按钮事件-单独的打印按钮
        /// </summary>
        public event EventHandler OnBtnSinglePrintClicked;
        private void BtnSinglePrint_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnSinglePrintClicked != null)
            {
                this.OnBtnSinglePrintClicked(sender, e);
            }
        }


        /// <summary>
        /// 点击批量打印
        /// </summary>
        public event EventHandler OnBtnPrintBatchClicked;
        private void BtnPrintBatch_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnPrintBatchClicked != null)
            {
                OnBtnPrintBatchClicked(sender, e);
            }
        }



        /// <summary>
        /// 批量打印预览
        /// </summary>
        public event EventHandler OnBtnPrintBatchPviewClicked;
        private void BtnPrintBatchPview_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnPrintBatchPviewClicked != null)
            {
                OnBtnPrintBatchPviewClicked(sender, e);
            }
        }


        /// <summary>
        /// 按钮事件_打印
        /// </summary>
        public event EventHandler OnBtnPrintClicked;
        private void BtnPrint_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnPrintClicked != null)
            {
                this.OnBtnPrintClicked(sender, e);
            }
        }


        /// <summary>
        /// 按钮事件-打印预览
        /// </summary>
        public event EventHandler OnPrintPreviewClicked;
        private void BtnPrintPreview_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnPrintPreviewClicked != null)
            {
                this.OnPrintPreviewClicked(sender, e);
            }
        }


        /// <summary>
        /// 按钮事件-导入
        /// </summary>
        public event EventHandler OnBtnImportClicked;
        private void BtnImport_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnImportClicked != null)
            {
                this.OnBtnImportClicked(sender, e);
            }
        }



        /// <summary>
        /// 按钮事件-导出
        /// </summary>
        public event EventHandler OnBtnExportClicked;
        private void BtnExport_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnExportClicked != null)
            {
                this.OnBtnExportClicked(sender, e);
            }
        }


        /// <summary>
        /// 按钮事件-质控规则
        /// </summary>
        public event EventHandler OnBtnQualityRuleClicked;
        private void BtnQualityRule_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnQualityRuleClicked != null)
            {
                this.OnBtnQualityRuleClicked(sender, e);
            }
        }


        /// <summary>
        /// 按钮事件-测定数据
        /// </summary>
        public event EventHandler OnBtnQualityTestClicked;
        private void BtnQualityTest_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnQualityTestClicked != null)
            {
                this.OnBtnQualityTestClicked(sender, e);
            }
        }



        /// <summary>
        /// 按钮事件-审核数据
        /// </summary>
        public event EventHandler OnBtnQualityAuditClicked;
        private void BtnQualityAudit_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnQualityAuditClicked != null)
            {
                this.OnBtnQualityAuditClicked(sender, e);
            }
        }


        /// <summary>
        /// 按钮事件-失控数据
        /// </summary>
        public event EventHandler OnBtnQualityOutClicked;
        private void BtnQualityOut_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnQualityOutClicked != null)
            {
                this.OnBtnQualityOutClicked(sender, e);
            }
        }



        /// <summary>
        /// 按钮事件-数据转换
        /// </summary>
        public event EventHandler OnBtnQualityDataClicked;
        private void BtnQualityData_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnQualityDataClicked != null)
            {
                this.OnBtnQualityDataClicked(sender, e);
            }
        }


        /// <summary>
        /// 按钮事件-复制
        /// </summary>
        public event EventHandler BtnCopyClick;
        private void BtnCopy_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BtnCopyClick != null)
            {
                this.BtnCopyClick(sender, e);
            }
        }



        /// <summary>
        /// 按钮事件-上传服务器版本
        /// </summary>
        public event EventHandler BtnUploadVersionClick;
        private void BtnUploadVersion_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BtnUploadVersionClick != null)
            {
                this.BtnUploadVersionClick(sender, e);
            }
        }


        /// <summary>
        /// 浏览
        /// </summary>
        public event EventHandler BtnBrowseClick;
        private void btnBrowse_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BtnBrowseClick != null)
            {
                this.BtnBrowseClick(sender, e);
            }
        }


        /// <summary>
        /// 回复
        /// </summary>
        public event EventHandler BtnAnswerClick;
        private void BtnAnswer_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BtnAnswerClick != null)
            {
                this.BtnAnswerClick(sender, e);
            }
        }


        /// <summary>
        /// 按钮事件-质控图
        /// </summary>
        public event EventHandler OnBtnQualityImageClicked;
        private void BtnQualityImage_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnBtnQualityImageClicked != null)
            {
                this.OnBtnQualityImageClicked(sender, e);
            }
        }

        /// <summary>
        /// 调用模板
        /// </summary>
        public event EventHandler BtnSelectTemplateClick;
        private void BtnSelectTemplate_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BtnSelectTemplateClick != null)
            {
                this.BtnSelectTemplateClick(sender, e);
            }
        }


        /// <summary>
        /// 快速录入
        /// </summary>
        public event EventHandler BtnQuickEntryClick;
        private void BtnQuickEntry_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BtnQuickEntryClick != null)
            {
                this.BtnQuickEntryClick(sender, e);
            }
        }


        /// <summary>
        /// 保存默认值
        /// </summary>
        public event EventHandler BtnSaveDefaultClick;
        private void BtnSaveDefault_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BtnSaveDefaultClick != null)
            {
                BtnSaveDefaultClick(sender, e);
            }
        }


        /// <summary>
        /// 还原默认值
        /// </summary>
        public event EventHandler BtnRevertDefaultClick;
        private void BtnRevertDefault_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BtnRevertDefaultClick != null)
            {
                BtnRevertDefaultClick(sender, e);
            }
        }


        /// <summary>
        /// 打印机设置
        /// </summary>
        public event EventHandler BtnPrintSetClick;
        private void BtnPrintSet_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BtnPrintSetClick != null)
            {
                BtnPrintSetClick(sender, e);
            }
        }


        /// <summary>
        /// 默认标本
        /// </summary>
        public event EventHandler BtnDeSpeClick;
        private void BtnDeSpe_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BtnDeSpeClick != null)
            {
                BtnDeSpeClick(sender, e);
            }
        }


        /// <summary>
        /// 默认参考值
        /// </summary>
        public event EventHandler BtnDeRefClick;
        private void BtnDeRef_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (BtnDeRefClick != null)
            {
                BtnDeRefClick(sender, e);
            }
        }


        /// <summary>
        /// 按钮事件-关闭
        /// </summary>
        public event EventHandler OnCloseClicked;
        private void BtnClose_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (OnCloseClicked != null)
            {
                this.OnCloseClicked(sender, e);
            }

            if (parentForm is FrmCommon)
            {
                ((FrmCommon)parentForm).Close();
            }
        }
    }
}
