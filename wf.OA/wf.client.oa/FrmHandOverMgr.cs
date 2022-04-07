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

namespace dcl.client.oa
{
    public partial class FrmHandOverMgr : FrmCommon
    {
        public FrmHandOverMgr()
        {
            InitializeComponent();
        }
        ucHandOverLayout ucHandOverLayout1;
        private void FrmHandOverMgr_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[]
                {
                    sysToolBar1.BtnAdd.Name,
                    sysToolBar1.BtnModify.Name,
                    sysToolBar1.BtnSave.Name,
                    sysToolBar1.BtnCancel.Name,
                    sysToolBar1.BtnDelete.Name,
                    sysToolBar1.BtnRefresh.Name,
                    "BtnClose"
                });
            ucHandOverLayout1 = new ucHandOverLayout();

            panelLayout.Controls.Add(ucHandOverLayout1);
            ucHandOverLayout1.Dock = DockStyle.Fill;
            ucHandOverLayout1.SetCustomeText("鼠标右键定制面板");
            //ucHandOverLayout1.SetCustomVisiable(chkhr_itr_mflag.Checked);
            LoadData();

            SetItrEditStatus(false);
            gridControlItr.Focus();

           
        }

        private bool isNew = false;
        private void LoadData()
        {
            isActionSuccess = true;

            isNew = false;
            BindingList<EntityDicHandOver> list = new BindingList<EntityDicHandOver>();
            List<EntityDicHandOver> aList = new ProxyOaHandOver().Service.GetDictHandoverList();
            foreach (EntityDicHandOver entityAuditRule in aList)
            {
                list.Add(entityAuditRule);
            }
            ItrList = list;
            if (list.Count > 0)
            {
                CurrentItrRow = ItrList[ItrList.Count - 1];

                ucHandOverLayout1.SetLayout(CurrentItrRow.HoTypeId);
            }

        }

        public BindingList<EntityDicHandOver> ItrList
        {
            get
            {
                bsHo.EndEdit();
                return bsHo.DataSource as BindingList<EntityDicHandOver>;
            }
            set
            {
                bsHo.SuspendBinding();
                bsHo.DataSource = value;
                bsHo.ResumeBinding();
            }
        }

        public string getTxtTypeValue()
        {
            if (txtType.valueMember != null && txtType.valueMember.Length>0)
            {
                return txtType.valueMember;
            }
            return null;
        }

        private EntityDicHandOver CurrentItrRow
        {
            get
            {
                bsHo.EndEdit();
                return (bsHo != null && bsHo.Count > 0 &&
                        bsHo.Position > -1)
                           ? bsHo.Current as EntityDicHandOver
                           : null;
            }
            set { bsHo.Position = ItrList.IndexOf(value); }
        }

        private void SetItrEditStatus(bool canEdit)
        {
            gridControlItr.Enabled = !canEdit;

            txtType.Readonly = !canEdit;
            txtho_timeInter.Properties.ReadOnly = !canEdit;
            time1.Properties.ReadOnly = !canEdit;
            time2.Properties.ReadOnly = !canEdit;
            time3.Properties.ReadOnly = !canEdit;
        }


        private void sysToolBar1_OnBtnAddClicked(object sender, EventArgs e)
        {
            SetItrEditStatus(true);
            EntityDicHandOver ruleComm = new EntityDicHandOver();
            ruleComm.IsNew = true;
            isNew = true;
            ItrList.Add(ruleComm);
            CurrentItrRow = ruleComm;


            txtType.ClearSelect();
            time1.Text = "8:00:00";
            time2.Text = "15:00:00";
            time3.Text = "21:00:00";
            txtho_timeInter.Text = "10";
            txtType.Focus();

          
        }

        private void sysToolBar1_OnBtnCancelClicked(object sender, EventArgs e)
        {
            try
            {
                SetItrEditStatus(false);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowAutoCloseDialog("操作失败！" + ex.Message);
            }
        }

        private void sysToolBar1_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            if (CurrentItrRow != null)
            {
                if (!CurrentItrRow.IsNew)
                {
                    DialogResult result = MessageDialog.Show("确定要删除当前所选物理组交班设定？", "提示", MessageBoxButtons.YesNo,
                                                             MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                     new ProxyOaHandOver().Service.DeleteDictHandover(CurrentItrRow.HoTypeId);
                }
                ItrList.Remove(CurrentItrRow);
                MessageDialog.ShowAutoCloseDialog("删除交班设定成功！");
            }
        }

        private void sysToolBar1_OnBtnModifyClicked(object sender, EventArgs e)
        {
            SetItrEditStatus(true);
            if (CurrentItrRow == null)
            {
                EntityDicHandOver ruleComm = new EntityDicHandOver();
                ruleComm.IsNew = true;
                isNew = true;
                ItrList.Add(ruleComm);
                CurrentItrRow = ruleComm;


                txtType.ClearSelect();
                time1.Text = "8:00:00";
                time2.Text = "15:00:00";
                time3.Text = "21:00:00";
                txtho_timeInter.Text = "10";
                txtType.Focus();
            }
            else
            {
                txtType.Readonly = true;
            }
        }

        private void sysToolBar1_OnBtnRefreshClicked(object sender, EventArgs e)
        {
            try
            {
                SetItrEditStatus(false);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowAutoCloseDialog("刷新失败！" + ex.Message);
            }
        }

        private void sysToolBar1_OnBtnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                gvItr.CloseEditor();
                List<EntityDicHandOver> saveList = new List<EntityDicHandOver>();
                isActionSuccess = false;
                if (string.IsNullOrEmpty(txtType.valueMember))
                {
                    MessageDialog.Show(string.Format("物理组不能为空！"));
                    txtType.Focus();
                    return;
                }
                if (!string.IsNullOrEmpty(time1.Text) && !string.IsNullOrEmpty(time2.Text) &&
                    time1.Time > time2.Time)
                {
                    MessageDialog.Show(string.Format("班次时间段设置错误！"));
                    return;
                }
                if (!string.IsNullOrEmpty(time1.Text) && !string.IsNullOrEmpty(time3.Text) &&
                   time1.Time > time3.Time)
                {
                    MessageDialog.Show(string.Format("班次时间段设置错误！"));
                    return;
                }
                if (!string.IsNullOrEmpty(time2.Text) && !string.IsNullOrEmpty(time3.Text) &&
                  time2.Time > time3.Time)
                {
                    MessageDialog.Show(string.Format("班次时间段设置错误！"));
                    return;
                }
                EntityDicHandOver ruleComm = new EntityDicHandOver();
                ruleComm.HoTypeId = txtType.valueMember;
                ruleComm.IsNew = isNew;
                ruleComm.HoTimeInter = txtho_timeInter.Text;
                ruleComm.HoTime1 = time1.Text;
                ruleComm.HoTime2 = time2.Text;
                ruleComm.HoTime3 = time3.Text;
                saveList.Insert(0, ruleComm);
                bool isSuccess=new  ProxyOaHandOver().Service.UpdateDictHandoverList(saveList);
                if (isSuccess)
                {
                    SetItrEditStatus(false);
                    LoadData();
                    MessageDialog.ShowAutoCloseDialog("保存成功");
                }

            }
            catch (Exception ex)
            {
                MessageDialog.Show("保存失败！" + ex.Message);
            }
        }

        private void gvItr_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (CurrentItrRow != null)
                {
                    txtType.valueMember = CurrentItrRow.HoTypeId;
                    txtType.displayMember = CurrentItrRow.TypeName;
                    try
                    {
                        time1.EditValue = Convert.ToDateTime(CurrentItrRow.HoTime1);
                        time2.EditValue = Convert.ToDateTime(CurrentItrRow.HoTime2);
                        time3.EditValue = Convert.ToDateTime(CurrentItrRow.HoTime3);
                    }
                    catch
                    { 
                    }
                    txtho_timeInter.Text = CurrentItrRow.HoTimeInter;
                }
                else
                {
                    txtType.ClearSelect();
                    time1.Text = "8:00:00";
                    time2.Text = "15:00:00";
                    time3.Text = "21:00:00";
                    txtho_timeInter.Text = "10";
                    txtType.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowAutoCloseDialog("操作失败！" + ex.Message);
            }
        }


        private void txtType_ValueChanged(object sender, dcl.client.control.ValueChangeEventArgs args)
        {
            panelLayout.Controls.Clear();
            ucHandOverLayout1 = new ucHandOverLayout();

            panelLayout.Controls.Add(ucHandOverLayout1);
            ucHandOverLayout1.Dock = DockStyle.Fill;
            ucHandOverLayout1.SetCustomeText("鼠标右键定制面板");
            ucHandOverLayout1.SetLayout(txtType.valueMember);
            //ucHandOverLayout1.SetCustomVisiable(chkhr_itr_mflag.Checked);
        }

        private void chkhr_itr_mflag_CheckedChanged(object sender, EventArgs e)
        {
            //ucHandOverLayout1.SetCustomVisiable(chkhr_itr_mflag.Checked);
        }



  
    }
}
