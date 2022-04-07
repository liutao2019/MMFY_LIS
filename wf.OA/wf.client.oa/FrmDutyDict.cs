using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using dcl.client.wcf;

using dcl.entity;
using System.Linq;

namespace dcl.client.oa
{
    public partial class FrmDutyDict : FrmCommon
    {
        #region 全局变量
        /// <summary>
        /// 是否为新增操作
        /// </summary>
        private bool IsAddClick = false;
        /// <summary>
        /// 是否为修改操作
        /// </summary>
        private bool IsModifyClick = false;
        /// <summary>
        /// 是否是保存操作引发的
        /// </summary>
        private bool IsSaveClick = false;
        /// <summary>
        /// 先前FocusHandler
        /// </summary>
        private int intPreHandle = 0;
        /// <summary>
        /// 当前FocusHandler
        /// </summary>
        private int intFocueHandle = 0;
        /// <summary>
        /// 班次表
        /// </summary>
        private List<EntityOaDicShift> listDuty = new List<EntityOaDicShift>();
        /// <summary>
        /// 全部的物理组
        /// </summary>
        List<EntityDicPubProfession> listchklisbox = null;
        ///// <summary>
        ///// 物理组别
        ///// </summary>
        //DataTable tbPhyic = null;
        #endregion
        ProxyOaShiftDict proxy = new ProxyOaShiftDict();

        public FrmDutyDict()
        {
            InitializeComponent();
            InitData();

        }

        #region 初始化数据

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            listchklisbox = proxy.Service.GetPhyic();
            this.bsDicPub.DataSource = listchklisbox;
            listDuty = proxy.Service.GetDutyData();
            this.bsDutyDict.DataSource = listDuty;
        }

        private void FrmDutyDict_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnAdd.Name,sysToolBar1.BtnModify.Name,sysToolBar1.BtnSave.Name,sysToolBar1.BtnDelete.Name,
                sysToolBar1.BtnCancel.Name });
        }

        #endregion

        #region 新增班次
        private void sysToolBar1_OnBtnAddClicked(object sender, EventArgs e)
        {
            #region 新增一空行
            this.gvdutys.AddNewRow();
            this.gvdutys.RefreshData();
            gvdutys.MoveLastVisible();
            #endregion

            #region 其他选项为可以编辑状态
            ModifyDutyStatus(false);
            #endregion

            chklisbox.DataSource = listchklisbox;
            chklisbox.UnCheckAll();
            this.IsAddClick = true;
            this.txtDpy.Text = "";
            this.txtDwb.Text = "";
            ChkBoxStatus(true);

        }



        #endregion

        #region 改变编辑状态
        private void ModifyDutyStatus(bool p)
        {
            #region 其他选项为可以编辑状态
            this.txtDName.Properties.ReadOnly = p;
            this.tetSTime.Properties.ReadOnly = p;
            this.tetETime.Properties.ReadOnly = p;
            this.lueDType.Properties.ReadOnly = p;
            this.txtPhyType.Properties.ReadOnly = p;
            this.metExport.Properties.ReadOnly = p;
            #endregion
        }
        #endregion

        #region 保存
        private void sysToolBar1_OnBtnSaveClicked(object sender, EventArgs e)
        {
            IsSaveClick = true;
            int intRet = 0;
            //判断必填项是否为空
            if (string.IsNullOrEmpty(this.txtDName.Text))
            {

                sysToolBar1.EnableButton(false);
                ModifyDutyStatus(true);

                ChkBoxStatus(false);
                lis.client.control.MessageDialog.Show("班次名称不能为空！");
                if (IsAddClick)
                {
                    gvdutys.DeleteRow(gvdutys.RowCount - 1);
                }

                IsSaveClick = false;
                IsAddClick = false;
                IsModifyClick = false;
                return;
            }
            if (string.IsNullOrEmpty(tetSTime.EditValue.ToString()) || string.IsNullOrEmpty(tetETime.EditValue.ToString()))
            {
                sysToolBar1.EnableButton(false);
                ModifyDutyStatus(true);
                ChkBoxStatus(false);
                lis.client.control.MessageDialog.Show("请选择时间!");
                if (IsAddClick)
                {
                    gvdutys.DeleteRow(gvdutys.RowCount - 1);
                }
                IsSaveClick = false;
                IsAddClick = false;
                IsModifyClick = false;
                return;
            }
            if (chklisbox.CheckedItems.Count < 1)
            {
                IsSaveClick = false;
                IsAddClick = false;
                IsModifyClick = false;
                sysToolBar1.EnableButton(false);
                ModifyDutyStatus(true);
                ChkBoxStatus(false);
                lis.client.control.MessageDialog.Show("请选择本班次所属部门类别!");

                return;
            }

            #region 单个科室的操作
            //if (string.IsNullOrEmpty(this.lueDType.Text))
            //{
            //    IsAddClick = false;
            //    IsModifyClick = false;
            //    sysToolBar1.EnableButton(false);
            //    ModifyDutyStatus(true);

            //    lis.client.control.MessageDialog.Show("请选择本班次所属部门类别！");
            //    gvdutys.DeleteRow(gvdutys.RowCount - 1);

            //    return;
            //}
            //EntityDictDuty entity = new EntityDictDuty();
            ////entity.duty_id = this.txtDID.Text;
            //entity.duty_name = this.txtDName.Text;
            //entity.duty_sdate = this.tetSTime.Text;
            //entity.duty_edate = this.tetETime.Text;
            ////entity.duty_dept_id = lueDType.EditValue.ToString();
            //entity.duty_exp = this.metExport.Text.Trim();
            //entity.duty_del = "0";
            //entity.duty_flag = 0;
            //entity.duty_py = this.txtDpy.Text;
            //entity.duty_wb = this.txtDwb.Text;
            //ProxyDutyDict proxy = new ProxyDutyDict();


            //entity.duty_dept_id = lueDType.EditValue.ToString();

            //if (IsAddClick)
            //{
            //    #region 添加保存

            //    #region 新增班次的ID值是自己增长的
            //    entity.duty_id = proxy.Service.GetMaxDutyID();
            //    #endregion

            //    intRet += proxy.Service.InsertIntoDuty(entity);

            //    #endregion
            //}
            //if (IsModifyClick)
            //{
            //    #region 修改保存
            //    entity.duty_id = txtDID.Text.ToString();
            //    intRet += proxy.Service.ModifyDutyRecord(entity);
            //    #endregion
            //}                   

            //#region 操作之后，成功的话就更新GridView
            //DataRow row = null;
            //if (intRet < 1)
            //{
            //    lis.client.control.MessageDialog.Show("操作失败！");
            //}
            //else if (IsAddClick)
            //{//插入成功之后,显示在gridview中并且重置工具栏状态

            //    row = gvdutys.GetDataRow(gvdutys.RowCount - 1);

            //    //gvdutys.MoveLast();
            //}
            //else if (IsModifyClick)
            //{
            //    if (sender is bool)
            //    {
            //        row = gvdutys.GetDataRow(intPreHandle);
            //    }
            //    else
            //    {
            //        row = gvdutys.GetFocusedDataRow();
            //    }
            //}

            //if (row != null)
            //{
            //    row["duty_id"] = entity.duty_id;
            //    row["duty_name"] = entity.duty_name;
            //    row["duty_sdate"] = entity.duty_sdate;
            //    row["duty_edate"] = entity.duty_edate;
            //    row["duty_wb"] = entity.duty_wb;
            //    row["duty_py"] = entity.duty_py;
            //    row["duty_exp"] = entity.duty_exp;
            //    row["duty_dept_id"] = entity.duty_dept_id;
            //    row["duty_flag"] = entity.duty_flag;
            //    row["duty_del"] = entity.duty_del;
            //}
            ////gvdutys.RefreshData();


            //#endregion          



            #endregion

            #region 批量操作

            EntityOaDicShift entity = new EntityOaDicShift();

            entity.ShiftName = this.txtDName.Text;
            if (tetSTime.EditValue != null && tetETime.EditValue != null)
            {
                entity.ShiftStartDate = Convert.ToDateTime(this.tetSTime.EditValue).ToString("HH:mm:ss");
                entity.ShiftEndDate = Convert.ToDateTime(this.tetETime.EditValue).ToString("HH:mm:ss");
            }
            entity.ShiftRemark = this.metExport.Text.Trim();
            entity.DelFlag = "0";
            entity.ShiftFlag = 0;
            entity.PyCode = this.txtDpy.Text;
            entity.WbCode = this.txtDwb.Text;
            //从chklistBox中获得duty_id，duty_name，duty_dept_id
            #region 添加
            if (IsAddClick)
            {
                for (int i = 0; i < chklisbox.CheckedItems.Count; i++)
                {
                    entity.ShiftId = proxy.Service.GetMaxDutyID();

                    entity.ShiftDeptId = chklisbox.CheckedItems[i].ToString();

                    intRet += proxy.Service.InsertIntoDuty(entity);
                }
            }
            #endregion

            #region 修改
            if (IsModifyClick)
            {
                for (int i = 0; i < chklisbox.CheckedItems.Count; i++)
                {
                    entity.ShiftDeptId = chklisbox.CheckedItems[i].ToString();
                    entity.ShiftId = txtDID.Text;
                    intRet += proxy.Service.ModifyDutyRecord(entity);
                }

            }
            #endregion

            #endregion

            #region 刷新表格内容
            listDuty = proxy.Service.GetDutyData();
            this.bsDutyDict.DataSource = listDuty;
            #endregion
            IsAddClick = false;
            IsModifyClick = false;
            sysToolBar1.EnableButton(false);
            ModifyDutyStatus(true);
            ChkBoxStatus(false);

            // proxy.Dispose();
            IsSaveClick = false;
        }

        #endregion

        #region 拼音码和五笔码
        private void txtDName_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            SpellAndWbCodeTookit spellcode = new SpellAndWbCodeTookit();
            this.txtDpy.Text = spellcode.GetSpellCode(this.txtDName.Text);
            this.txtDwb.Text = spellcode.GetWBCode(txtDName.Text);
        }
        private dcl.client.common.SpellAndWbCodeTookit tookit = new dcl.client.common.SpellAndWbCodeTookit();
        private void txtDName_Leave(object sender, EventArgs e)
        {
            if (bsDutyDict.Current != null)
            {
                EntityOaDicShift dr = (EntityOaDicShift)bsDutyDict.Current;

                dr.PyCode = tookit.GetSpellCode(this.txtDName.Text);
                dr.WbCode = tookit.GetWBCode(this.txtDName.Text);
            }
        }
        #endregion

        #region 窗体Focus和详细内容的联动
        private void gvdutys_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            intPreHandle = e.PrevFocusedRowHandle;
            intFocueHandle = e.FocusedRowHandle;
            if (IsModifyClick && !IsSaveClick)
            {
                DialogResult dresult = lis.client.control.MessageDialog.Show("是否保存修改内容？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (dresult == DialogResult.Yes)
                {
                    sysToolBar1_OnBtnSaveClicked(true, null);
                }
                else if (dresult == DialogResult.No)
                {
                    IsModifyClick = false;
                    sysToolBar1.EnableButton(false);
                    ModifyDutyStatus(true);
                    ChkBoxStatus(false);
                    return;
                }
            }
            if (IsAddClick && !IsSaveClick)
            {
                DialogResult dresult = lis.client.control.MessageDialog.Show("是否保存新增数据？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (dresult == DialogResult.Yes)
                {
                    sysToolBar1_OnBtnSaveClicked(null, null);
                }
                else if (dresult == DialogResult.No)
                {
                    gvdutys.DeleteRow(intPreHandle);
                    IsAddClick = false;
                    sysToolBar1.EnableButton(false);
                    ChkBoxStatus(false);
                    //return;
                }
            }
            EntityOaDicShift duty = (EntityOaDicShift)bsDutyDict.Current;
        }
        #endregion

        #region 撤销操作
        private void sysToolBar1_OnBtnCancelClicked(object sender, EventArgs e)
        {

            if (IsAddClick)
            {
                IsAddClick = false;
                //gvdutys.MoveLast();//新增的行不算？？？？？？？
                gvdutys.DeleteSelectedRows();

            }
            if (IsModifyClick)
            {
                EntityOaDicShift duty = this.bsDutyDict.Current as EntityOaDicShift;
                if (duty != null)
                {
                    this.txtDID.Text = duty.ShiftId;
                    this.txtDName.Text = duty.ShiftName;
                    this.tetSTime.EditValue = duty.ShiftStartDate;
                    this.tetETime.EditValue = duty.ShiftEndDate;
                    this.txtDpy.Text = duty.PyCode;
                    this.txtDwb.Text = duty.WbCode;
                    this.metExport.Text = duty.ShiftRemark;
                    this.lueDType.EditValue = duty.ShiftDeptId;
                }
                IsModifyClick = false;
            }

            ModifyDutyStatus(true);
            ChkBoxStatus(false);

        }
        #endregion

        #region 删除操作
        private void sysToolBar1_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            DialogResult result = lis.client.control.MessageDialog.Show("确定要删除此班次？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.OK)
            {
                //删除此行数据
                EntityOaDicShift duty = (EntityOaDicShift)bsDutyDict.Current;

                proxy.Service.DeleteDutyRecord(duty);

                gvdutys.DeleteSelectedRows();

            }
        }
        #endregion

        #region 修改操作
        private void sysToolBar1_OnBtnModifyClicked(object sender, EventArgs e)
        {
            //如何进行批量修改
            ModifyDutyStatus(false);
            ChkBoxStatus(true);
            IsModifyClick = true;

            for (int i = 0; i < chklisbox.ItemCount; i++)
            {
                EntityDicPubProfession pub = (EntityDicPubProfession)chklisbox.GetItem(i);
                EntityOaDicShift shift = (EntityOaDicShift)bsDutyDict.Current;
                if (pub.ProName == shift.TypeName)
                {
                    chklisbox.SetItemChecked(i, true);
                }
                else
                {
                    chklisbox.SetItemChecked(i, false);
                }
            }
            //ResetChkListBox();

        }


        #endregion

        #region 查询操作
        private void txtSearch_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            List<EntityOaDicShift> list = null;
            if (e.NewValue != null)
            {
                string str = e.NewValue.ToString().Trim();
                //string strFilter = string.Format(@"duty_id like '%{0}%' or duty_name like '%{0}%' or duty_wb like '%{0}%' or duty_py like '%{0}%'  or type_py like '%{0}%' or type_wb like '%{0}%' or type_name like '%{0}%'", str);
                list = listDuty.Where(w => w.ShiftId.Contains(str) || w.ShiftName.Contains(str) || w.WbCode.Contains(str)
                                                 || w.PyCode.Contains(str)).ToList();
                this.gcDutys.DataSource = list;
            }
        }
        #endregion

        #region 窗体大小变化时，左右两模块的比率变化
        private void FrmDutyDict_Resize(object sender, EventArgs e)
        {
            splitContainerControl1.SplitterPosition = (this.Size.Width) * 3 / 5;
        }
        #endregion

        #region 选择物理组
        private void lueDType_EditValueChanged(object sender, EventArgs e)
        {
            if (lueDType.Text.Contains("部分"))
            {
                //this.layoutControlItem9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                //this.txtDName.Properties.ReadOnly = true;
                List<EntityOaDicShift> list = listDuty.Where(w => w.ShiftName.Contains(txtDName.Text)).ToList();
                string strPhys = "(";
                for (int i = 0; i < list.Count; i++)
                {
                    strPhys += "'" + list[i].ShiftDeptId + "',";
                }
                strPhys += "'" + list[list.Count].ShiftDeptId + "')";

                List<EntityDicPubProfession> listPro = listchklisbox.Where(W => W.ProId.Contains(strPhys)).ToList();
                this.bsDicPub.DataSource = listPro;
            }
            else
            {
                //this.layoutControlItem9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }
        #endregion

        #region 选择的条件变动时
        private void btnAll_Click(object sender, EventArgs e)
        {

            int i = 0;
            while (chklisbox.GetItem(i) != null)
            {
                chklisbox.SetItemCheckState(i, CheckState.Checked);
                i++;
            }

        }

        private void btnOther_Click(object sender, EventArgs e)
        {
            int i = 0;
            while (chklisbox.GetItem(i) != null)
            {
                chklisbox.SetItemCheckState(i, (chklisbox.GetItemChecked(i) ? CheckState.Unchecked : CheckState.Checked));
                i++;
            }

        }

        private void btnReset_Click(object sender, EventArgs e)
        {

            int i = 0;
            while (chklisbox.GetItem(i) != null)
            {
                chklisbox.SetItemCheckState(i, CheckState.Unchecked);
                i++;
            }

        }
        #endregion

        #region 班次名称改变时
        /// <summary>
        /// 如果是修改操作，获得对应此名称的物理组别信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDName_EditValueChanged(object sender, EventArgs e)
        {
            if (IsModifyClick)//如果是修改操作，获得对应此名称的物理组别信息
            {
                //ResetChkListBox();
            }
        }
        #endregion

        #region 重新填充ListBox内容
        /// <summary>
        /// 班次名称改变时
        /// </summary>
        private void ResetChkListBox()
        {
            List<EntityOaDicShift> list = new List<EntityOaDicShift>();
            list = listDuty.Where(w => w.ShiftName.Contains(txtDName.Text)).ToList();

            if (list.Count < 1)
            {
                this.chklisbox.DataSource = null;
                return;
            }
            string[] strPhy = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                strPhy[i] = list[i].ShiftDeptId;
            }
            List<EntityDicPubProfession> listPro = (from x in listchklisbox where strPhy.Contains(x.ProId) select x).ToList();
            this.chklisbox.DataSource = listPro;
        }
        #endregion

        #region 编辑物理组别
        private void ChkBoxStatus(bool p)
        {
            if (p)
            {
                this.layBtnAll.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                this.layBtnOther.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                this.layBtnReset.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                this.laychkbox.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                this.layPhyType.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            else
            {
                this.layBtnAll.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.layBtnOther.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.layBtnReset.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.laychkbox.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.layPhyType.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

            }

        }










        #endregion
    }
}
