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
    public partial class FrmHandOverInput : FrmCommon
    {
        public FrmHandOverInput()
        {
            InitializeComponent();
        }
     
        private void FrmHandOverMgr_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[]
                {
                    sysToolBar1.BtnAdd.Name,
                    sysToolBar1.BtnDelete.Name,
                    sysToolBar1.BtnRefresh.Name,
                    "BtnClose"
                });
           
            //ucHandOverLayout1.SetCustomVisiable(chkhr_itr_mflag.Checked);
            LoadData();

       
            gridControlItr.Focus();

           
        }

        private bool isNew = false;
        private void LoadData()
        {
            isActionSuccess = true;

            isNew = false;
            BindingList<EntityHoRecord> list = new BindingList<EntityHoRecord>();
            var aList = new ProxyOaHoRecord().Service.GetHandoverList(DateTime.Now.AddDays(-30), DateTime.Now.AddDays(1));
            foreach (EntityHoRecord entityAuditRule in aList)
            {
                list.Add(entityAuditRule);
            }
            ItrList = list;
            if (list.Count > 0)
            {
                CurrentItrRow = ItrList[ItrList.Count - 1];
            }

        }

        public BindingList<EntityHoRecord> ItrList
        {
            get
            {
                bsHo.EndEdit();
                return bsHo.DataSource as BindingList<EntityHoRecord>;
            }
            set
            {
                bsHo.SuspendBinding();
                bsHo.DataSource = value;
                bsHo.ResumeBinding();
            }
        }

        

        private EntityHoRecord CurrentItrRow
        {
            get
            {
                bsHo.EndEdit();
                return (bsHo != null && bsHo.Count > 0 &&
                        bsHo.Position > -1)
                           ? bsHo.Current as EntityHoRecord
                           : null;
            }
            set { bsHo.Position = ItrList.IndexOf(value); }
        }

        private void sysToolBar1_OnBtnAddClicked(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(LocalSetting.Current.Setting.CType_id))
            {
                MessageDialog.Show("请到[系统维护]-[本地设置] 设定实验组");
                return;
            }
            dcl.client.oa.FrmHandOverInfo info = new dcl.client.oa.FrmHandOverInfo();
            info.IsManue = true;
            info.ShowDialog();
            LoadData();
        }

       

        private void sysToolBar1_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            if (CurrentItrRow != null)
            {
                if (!CurrentItrRow.IsNew)
                {
                    DialogResult result = MessageDialog.Show("确定要删除当前所选实验组交班信息？", "提示", MessageBoxButtons.YesNo,
                                                             MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                     new ProxyOaHoRecord().Service.DeleteHandover(CurrentItrRow.HrId);
                }
                ItrList.Remove(CurrentItrRow);
                MessageDialog.ShowAutoCloseDialog("删除交班信息成功！");
            }
        }

        private void sysToolBar1_OnBtnModifyClicked(object sender, EventArgs e)
        {
        //    SetItrEditStatus(true);
        //    if (CurrentItrRow == null)
        //    {
        //        EntityDictHandOver ruleComm = new EntityDictHandOver();
        //        ruleComm.IsNew = true;
        //        isNew = true;
        //        ItrList.Add(ruleComm);
        //        CurrentItrRow = ruleComm;


        //        txtType.ClearSelect();
        //        time1.Text = "8:00:00";
        //        time2.Text = "15:00:00";
        //        time3.Text = "21:00:00";
        //        txtho_timeInter.Text = "10";
        //        txtType.Focus();
        //    }
        //    else
        //    {
        //        txtType.Readonly = true;
        //    }
        }

        private void sysToolBar1_OnBtnRefreshClicked(object sender, EventArgs e)
        {
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowAutoCloseDialog("刷新失败！" + ex.Message);
            }
        }

        private void gvItr_DoubleClick(object sender, EventArgs e)
        {
            if (CurrentItrRow != null)
            {
                dcl.client.oa.FrmHandOverInfo info = new dcl.client.oa.FrmHandOverInfo();
                info.info = CurrentItrRow ;
                info.ShowDialog();
            }
        }

        //private void sysToolBar1_OnBtnSaveClicked(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        gvItr.CloseEditor();
        //        List<EntityDictHandOver> saveList = new List<EntityDictHandOver>();
        //        isActionSuccess = false;
        //        if (string.IsNullOrEmpty(txtType.valueMember))
        //        {
        //            MessageDialog.Show(string.Format("物理组不能为空！"));
        //            txtType.Focus();
        //            return;
        //        }
        //        if (!string.IsNullOrEmpty(time1.Text) && !string.IsNullOrEmpty(time2.Text) &&
        //            time1.Time > time2.Time)
        //        {
        //            MessageDialog.Show(string.Format("班次时间段设置错误！"));
        //            return;
        //        }
        //        if (!string.IsNullOrEmpty(time1.Text) && !string.IsNullOrEmpty(time3.Text) &&
        //           time1.Time > time3.Time)
        //        {
        //            MessageDialog.Show(string.Format("班次时间段设置错误！"));
        //            return;
        //        }
        //        if (!string.IsNullOrEmpty(time2.Text) && !string.IsNullOrEmpty(time3.Text) &&
        //          time2.Time > time3.Time)
        //        {
        //            MessageDialog.Show(string.Format("班次时间段设置错误！"));
        //            return;
        //        }
        //        EntityDictHandOver ruleComm = new EntityDictHandOver();
        //        ruleComm.ho_type_id = txtType.valueMember;
        //        ruleComm.IsNew = isNew;
        //        ruleComm.ho_timeInter = txtho_timeInter.Text;
        //        ruleComm.ho_time1 = time1.Text;
        //        ruleComm.ho_time2 = time2.Text;
        //        ruleComm.ho_time3 = time3.Text;
        //        saveList.Insert(0, ruleComm);
        //        new ProxyAnnouncement().Service.UpdateDictHandoverList(saveList);

        //        SetItrEditStatus(false);
        //        LoadData();
        //        MessageDialog.ShowAutoCloseDialog("保存成功");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageDialog.Show("保存失败！" + ex.Message);
        //    }
        //}

        //private void gvItr_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        //{
        //    try
        //    {
        //        if (CurrentItrRow != null)
        //        {
        //            txtType.valueMember = CurrentItrRow.ho_type_id;
        //            txtType.displayMember = CurrentItrRow.typename;
        //            try
        //            {
        //                time1.EditValue = Convert.ToDateTime(CurrentItrRow.ho_time1);
        //                time2.EditValue = Convert.ToDateTime(CurrentItrRow.ho_time2);
        //                time3.EditValue = Convert.ToDateTime(CurrentItrRow.ho_time3);
        //            }
        //            catch
        //            { 
        //            }
        //            txtho_timeInter.Text = CurrentItrRow.ho_timeInter;
        //        }
        //        else
        //        {
        //            txtType.ClearSelect();
        //            time1.Text = "8:00:00";
        //            time2.Text = "15:00:00";
        //            time3.Text = "21:00:00";
        //            txtho_timeInter.Text = "10";
        //            txtType.Focus();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageDialog.ShowAutoCloseDialog("操作失败！" + ex.Message);
        //    }
        //}







    }
}
