using System;
using System.Collections.Generic;
using System.Windows.Forms;
using dcl.client.wcf;
using lis.client.control;
using dcl.client.common;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.sample
{
    public partial class FrmReturnMessageV2 : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 科室代码
        /// </summary>
        public string DeptCode { get; set; }
        public IStep Step
        {
            get;
            set;
        }
        /// <summary>
        /// 是否独立客户端
        /// </summary>
        public bool IsAlone { get; set; }

        public bool IsScrollingText { get; set; }

        public FrmReturnMessageV2()
        {
            InitializeComponent();
        }
        public List<EntitySampReturn> ReturnMessages { get; set; }
        public bool Start { get; set; }
        /// <summary>
        /// 处理回退条码时是否需要身份验证
        /// </summary>
        private bool IsCheckPassword = false;
        private void FrmReturnMessageV2_Load(object sender, EventArgs e)
        {
            sysToolBar1.BtnExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            sysToolBar1.BtnGetVersion.Caption = "处理";
            //panel2.Visible = true;
            //sysToolBar1.btnReturn.Caption = "解锁";
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnGetVersion.Name, sysToolBar1.BtnExport.Name }, new string[] { "F3" });
            sysToolBar1.BtnGetVersion.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            if (IsScrollingText)//从滚动条点击条码回退信息
            {
                //panel1.Visible = false;
                panel2.Visible = true;
                sysToolBar1.BtnGetVersion.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                //sysToolBar1.BtnExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                BindData();
                //[处理回退条码]时是否要身份验证
                IsCheckPassword = (ConfigHelper.GetSysConfigValueWithoutLogin("IsCheckPw_DisposeReturnBC") == "是");
            }
        }
        private void radioGroup2_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }
        public void BindData()
        {
            List<EntitySampReturn> msgs = new List<EntitySampReturn>();
            if (this.radioGroup2.EditValue.ToString() == "全部")
            {
                if (this.ReturnMessages.Count > 0)
                {
                    msgs.AddRange(this.ReturnMessages);
                }
            }
            else if (this.radioGroup2.EditValue.ToString() == "门诊")
            {
                foreach (EntitySampReturn item in this.ReturnMessages)
                {
                    if (item.PidSrcId == "107")
                    {
                        msgs.Add(item);
                    }
                }
            }
            else if (this.radioGroup2.EditValue.ToString() == "住院")
            {
                foreach (EntitySampReturn item in this.ReturnMessages)
                {
                    if (item.PidSrcId == "108")
                    {
                        msgs.Add(item);
                    }
                }
            }
            else if (this.radioGroup2.EditValue.ToString() == "体检")
            {
                foreach (EntitySampReturn item in this.ReturnMessages)
                {
                    if (item.PidSrcId == "109")
                    {
                        msgs.Add(item);
                    }
                }
            }

            this.bsReturnMessage.DataSource = msgs;
        }

        /// <summary>
        /// 锁定并处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_BtnGetVersionClick(object sender, EventArgs e)
        {
            if (NotData())
            {
                ShowMessage("没有回退标本!");
                return;
            }

            EntitySampMain oneBarcode = new EntitySampMain();
            EntitySampReturn message = GetCurrentReturnMessages();

            oneBarcode.SampSn = message.ReturnSampSn;
            if (message == null || oneBarcode.SampSn <= 0)
            {
                ShowMessage("获取回退条码不成功");
                return;
            }
            EntitySampProcessDetail detail = new EntitySampProcessDetail();
            #region 处理时身份验证

            //是否需要身份验证
            if (IsCheckPassword)
            {
                FrmCheckPassword frm = new FrmCheckPassword();
               
                if (frm.ShowDialog() == DialogResult.OK)//身份验证
                {
                    string strOperatorID = frm.OperatorID;
                    string strOperatorName = frm.OperatorName;
                    detail.ProcDate = ServerDateTime.GetServerDateTime();
                    detail.ProcStatus = "9";
                    detail.ProcUsercode = strOperatorID;
                    detail.ProcUsername = strOperatorName;
                    detail.ProcBarno = message.SampBarId;
                    detail.ProcBarcode = message.SampBarCode;
                    detail.ProcContent = "处理回退条码_" + message.SampBarId;
                    detail.ProcTimes = 1;
                        }
                else
                {
                    return;
                }
            }

            #endregion
            oneBarcode.SampReturnFlag = true;
            ProxySampReturn proxy = new ProxySampReturn();
            bool result = proxy.Service.UpdateSampReturnFlag(oneBarcode);

            message.ReturnProcFlag = true;
            result = proxy.Service.UpdateReturnMessage(message);

            if (result == true)
            {
                //处理回退条码之后刷新缓存
                proxy.Service.RefereshReturnMessage();
                this.Visible = false;
                Start = false;
                NeedHandle = true;
                if(IsCheckPassword)
                {
                    ProxySampProcessDetail proxyProcess = new ProxySampProcessDetail();
                    result = proxyProcess.Service.SaveSampProcessDetailWithoutInterface(detail);
                }
                //if (ReturnMessageHandled != null)
                //{
                //    ReturnMessageHandled();
                //}
            }
            else
                ShowMessage("处理失败");
        }

        public EntitySampReturn GetCurrentReturnMessages()
        {
            if (bsReturnMessage != null)
                return bsReturnMessage.Current as EntitySampReturn;
            else
                return null;
     
        }
        private void ShowMessage(string word)
        {
            lis.client.control.MessageDialog.Show(word, "提示");
        }

        public bool NeedHandle { get; set; }

        public bool NotData()
        {
            return ReturnMessages == null || ReturnMessages.Count == 0;
        }
        private void btnSel_Click(object sender, EventArgs e)
        {
            DateTime dtiS = dtpStart.Value.Date;//开始时间
            DateTime dtiE = dtpEnd.Value.Date.AddDays(1);//结束时间

            if (dtiS >= dtiE)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("请选取正确的日期范围");
                return;
            }

            EntitySampQC sampQc = new EntitySampQC();
            sampQc.StartDate = dtiS.ToString();
            sampQc.EndDate = dtiE.ToString();

            if (this.radioGroup1.EditValue.ToString() == "已处理")
                sampQc.HandleProc = ReturnProc.已处理;
            else if (this.radioGroup1.EditValue.ToString() == "未处理")
                sampQc.HandleProc = ReturnProc.未处理;

            sampQc.PidDeptCode = DeptCode;

            try
            {
                ProxySampReturn proxy = new ProxySampReturn();
                List<EntitySampReturn> listReturn = proxy.Service.GetSampReturn(sampQc);

                if (listReturn != null && listReturn.Count > 0)
                {
                    bsReturnMessage.DataSource = listReturn;
                }
                else
                {
                    bsReturnMessage.DataSource = null;
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("找不到想要的数据");
                }
            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException(this.Name, "查询遇到错误", ex.ToString());
                lis.client.control.MessageDialog.Show(ex.Message + "\r\n详情请查看日志");
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnExportClicked(object sender, EventArgs e)
        {
            if (gridControl1.DataSource != null)
            {
                setExcel(gridControl1);
            }
            else
            {
                MessageBox.Show("没有可导出的数据");
            }
        }

        /// <summary>
        /// 导出Excel方法
        /// </summary>
        /// <param name="gcExcel"></param>
        private void setExcel(DevExpress.XtraGrid.GridControl gcExcel)
        {
            if (gcExcel.DataSource != null)
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.DefaultExt = "xls";
                ofd.Filter = "Excel文件(*.xls)|*.xls";
                ofd.Title = "导出到Excel";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.Trim() == string.Empty)
                    {
                        MessageBox.Show("文件名不能为空！");
                        return;
                    }

                    try
                    {
                        gcExcel.ExportToXls(ofd.FileName.Trim());

                        MessageBox.Show("导出成功！");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }
        }

    }
}
