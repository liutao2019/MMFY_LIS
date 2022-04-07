using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using lis.client.control;

using dcl.client.wcf;
using System.Reflection;
using dcl.client.common;
using dcl.entity;
using System.Linq;

namespace dcl.client.oa
{
    public partial class FrmAttendance : FrmCommon
    {
        #region 全局变量
        private string strUserInfoID = null;

        private List<EntityOaWorkAttendance>  listAttdRecord = new List<EntityOaWorkAttendance>();
        ProxyAttendance proxy = new ProxyAttendance();
        bool Office_ShowDutySelect;
        #endregion

        public FrmAttendance()
        {
            InitializeComponent();

            this.DateStart.EditValue = this.DateEnd.EditValue = DateTime.Now.ToString("d");
            InitData();
            Office_ShowDutySelect = ConfigHelper.GetSysConfigValueWithoutLogin("Office_ShowDutySelect") == "是";
        }

        private void InitData()
        {
            ProxyAttendance proxy = new ProxyAttendance();
            this.listAttdRecord = proxy.Service.GetAttRecordByUID(Convert.ToDateTime(DateStart.EditValue), Convert.ToDateTime(DateEnd.EditValue));
            this.bsAttendance.DataSource = listAttdRecord;

        }

        private void FrmAttendance_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnRefresh.Name, sysToolBar1.BtnPageUp.Name, sysToolBar1.BtnPageDown.Name, sysToolBar1.BtnModify.Name, sysToolBar1.BtnSave.Name, sysToolBar1.BtnCancel.Name, sysToolBar1.BtnExport.Name, });

            sysToolBar1.BtnPageUp.Caption = "上班";
            sysToolBar1.BtnPageDown.Caption = "下班";
            sysToolBar1.OnBtnRefreshClicked += btnRefresh_Click;
            sysToolBar1.OnBtnPageDownClicked += btnOffDuty_Click;
            sysToolBar1.OnBtnPageUpClicked += btnOnDuty_Click;
            this.sysToolBar1.OnBtnModifyClicked += new System.EventHandler(this.sysToolBar_OnBtnModifyClicked);
            this.sysToolBar1.OnBtnSaveClicked += new System.EventHandler(this.sysToolBar_OnBtnSaveClicked);
            this.sysToolBar1.OnBtnCancelClicked += new System.EventHandler(this.sysToolBar_OnBtnCancelClicked);
            this.sysToolBar1.OnBtnExportClicked += new System.EventHandler(this.sysToolBar_OnBtnExportClicked);
        }

        #region 上班
        private void btnOnDuty_Click(object sender, EventArgs e)
        {
            EntityOaDicShift dutyRow =null;

            if (Office_ShowDutySelect)
            {
                FrmDutySelect selector = new FrmDutySelect();

                if (selector.ShowDialog() != DialogResult.Yes)
                    return;
                dutyRow = selector.SelectList;
            }

            FrmDialog frm = new FrmDialog(true);
            frm.SelectList = dutyRow;
            strUserInfoID = frm.UserInfoID;
            DialogResult result = frm.ShowDialog();

            if (result == DialogResult.OK)
            {
                InitData();

                frm.Close();

                return;
            }

            if (result == DialogResult.Abort)
            {
                dcl.client.frame.FrmCommon from = this.MdiParent as FrmCommon;
                MethodInfo mi = from.GetType().GetMethod("LoadForm");
                mi.Invoke(from, new object[] { "排班设置", "dcl.client.oa.FrmDutyDict" });

                frm.Close();
                return;
            }

            if (result == DialogResult.No)
            {
                frm.Close();
                return;
            }



        }
        #endregion

        #region 下班
        private void btnOffDuty_Click(object sender, EventArgs e)
        {
            FrmDialog frm = new FrmDialog(false);
            strUserInfoID = frm.UserInfoID;
            DialogResult result = frm.ShowDialog();

            if (result == DialogResult.OK)
            {
                InitData();

                frm.Close();
                gvAttdRecord_FocusedRowChanged(null, null);
                return;
            }

            if (result == DialogResult.Abort)
            {
                dcl.client.frame.FrmCommon from = this.MdiParent as FrmCommon;
                MethodInfo mi = from.GetType().GetMethod("LoadForm");
                mi.Invoke(from, new object[] { "排班设置", "dcl.client.oa.FrmDutyDict" });

                frm.Close();
                return;
            }

            if (result == DialogResult.No)
            {
                frm.Close();
                return;
            }

        }
        #endregion

        #region 对应详细信息
        private void gvAttdRecord_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                //EntityOaWorkAttendance attend = gcAtttdReocrd(EntityOaWorkAttendance)this.bsAttendance.Current;
                EntityOaWorkAttendance attend = gvAttdRecord.GetFocusedRow() as EntityOaWorkAttendance;
                if (attend != null)
                {
                    this.txtUID.Text = attend.AteUserId;
                    this.txtUName.Text = attend.AteUserName;
                    //this.txtUAge.Text = row[""].ToString();
                    //this.txtInstrument.Text = row[""].ToString();
                    //this.txtPhy.Text =row[""].ToString();
                    this.txtDutyID.Text = attend.AteId;
                    this.txtDDate.Text = Convert.ToDateTime(attend.AteDate).ToString("d");
                    if (attend.AteWorkhours.ToString() != null)
                    {
                        txtWorks.Text = attend.AteWorkhours.ToString();
                    }
                    timeE.EditValue = attend.AteEndDate;
                    timeS.EditValue = attend.AteStartDate;

                    timeGE.EditValue = attend.AteShiftEndDate;
                    timeGS.EditValue = attend.AteShiftStartDate;
                    memAExp.Text = attend.AteRemark;
                    txtUType.Text = attend.AteUserType;
                    txtDName.Text = attend.AteShiftName;
                }
                else
                {
                    this.txtUID.Text = null;
                    this.txtUName.Text = null;
                    this.txtDutyID.Text = null;
                    this.txtDDate.Text = null;
                    txtWorks.Text = null;
                    timeE.EditValue = null;
                    timeS.EditValue = null;
                    timeGE.EditValue = null;
                    timeGS.EditValue = null;
                    memAExp.Text = null;
                    txtUType.Text = null;
                    txtDName.Text = null;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }
        #endregion

        #region 动态改变中间分隔带的位置
        private void FrmAttendance_Resize(object sender, EventArgs e)
        {
            splitContainerControl1.SplitterPosition = (this.Size.Width) * 3 / 5;
        }

        #endregion

        #region 记录查询
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.listAttdRecord = proxy.Service.GetAttRecordByUID(Convert.ToDateTime(DateStart.EditValue), Convert.ToDateTime(DateEnd.EditValue));
            this.gcAtttdReocrd.DataSource = listAttdRecord;
           // proxy.Dispose();
        }
        #endregion

        #region 判断考勤日期
        /// <summary>
        /// 开始时间变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DateStart_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (Convert.ToDateTime(e.NewValue) > DateTime.Now)
            {
                e.Cancel = true;
            }
        }

        private void DateEnd_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (Convert.ToDateTime(e.NewValue) > DateTime.Now)
            {
                e.Cancel = true;
            }
        }

        #endregion

        #region 如果是选择今天考勤情况，那么时间范围就定在今天
        private void rgAttdType_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue.ToString().Contains("当日考勤"))
            {
                this.DateStart.EditValue = this.DateEnd.EditValue = DateTime.Now.ToString("d");
                this.DateStart.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                this.DateEnd.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            }

            if (e.NewValue.ToString().Contains("历史考勤"))
            {
                this.DateEnd.EditValue = DateTime.Now.ToString("d");
                this.DateStart.EditValue = DateTime.Now.AddDays(-6).ToString("d");
            }

            this.btnRefresh_Click(null, null);
        }
        #endregion

        #region 查询内容改变时
        private void txtSearch_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (listAttdRecord != null)
            {
                string strTxtSearch = this.txtSearch.Text.Trim();
                List<EntityOaWorkAttendance> listFilter = new List<EntityOaWorkAttendance>();
                listFilter = listAttdRecord.Where(w => w.AteUserId.Contains(strTxtSearch) || w.AteLoginId.Contains(strTxtSearch)
                                                                 || w.AteUserName.Contains(strTxtSearch) || w.AteShiftName.Contains(strTxtSearch)|| w.AteWbCode.Contains(strTxtSearch) || w.AtePyCode.Contains(strTxtSearch) ).ToList();

                this.gcAtttdReocrd.DataSource = listFilter;
            }

        }
        #endregion

        #region 如果查询出的结果是空的话
        private void gvAttdRecord_CustomDrawEmptyForeground(object sender, DevExpress.XtraGrid.Views.Base.CustomDrawEventArgs e)
        {
            if (gvAttdRecord.RowCount == 0)
            {
                string str = "没有你所想要的数据！请重新填写查询条件！或者重新选择日期刷新！";
                Font f = new Font("宋体", 10, FontStyle.Bold);

                Rectangle r = new Rectangle(e.Bounds.Left + 5, e.Bounds.Top + 5, e.Bounds.Width - 5, e.Bounds.Height - 5);
                e.Graphics.DrawString(str, f, Brushes.Black, r);
            }

        }
        #endregion

        #region systemBar操作
        private void sysToolBar_OnBtnModifyClicked(object sender, EventArgs e)
        {
            this.memAExp.Properties.ReadOnly = false;

        }

        private void sysToolBar_OnBtnCancelClicked(object sender, EventArgs e)
        {
            this.memAExp.Properties.ReadOnly = true;
        }

        private void sysToolBar_OnBtnSaveClicked(object sender, EventArgs e)
        {
            EntityOaWorkAttendance entity = new EntityOaWorkAttendance();
            entity.AteId = this.txtDutyID.Text;
            entity.AteDate = Convert.ToDateTime(this.txtDDate.Text);
            entity.AteRemark = this.memAExp.Text;
            entity.AteEndDate = this.timeE.Text;

            int intRet = proxy.Service.ModifyAttdRecord(entity);

            this.memAExp.Properties.ReadOnly = true;
            this.sysToolBar1.EnableButton(false);

            this.btnRefresh_Click(null, null);
        }
        #endregion

        #region 导出

        private void sysToolBar_OnBtnExportClicked(object sender, EventArgs e)
        {
            if (gcAtttdReocrd.DataSource != null)
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.DefaultExt = "xls";
                ofd.Filter = "Excel文件(*.xls)|*.xls";
                ofd.Title = "导出到Excel";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.Trim() == string.Empty)
                    {
                        MessageDialog.Show("文件名不能为空！", "提示");
                        return;
                    }

                    try
                    {
                        gcAtttdReocrd.ExportToXls(ofd.FileName.Trim());
                        MessageDialog.Show("导出成功！", "提示");
                    }
                    catch (Exception)
                    {
                    }
                }

            }
        }





















        #endregion
    }
}
