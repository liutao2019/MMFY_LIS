using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using wf.client.reagent.Interface;
using dcl.entity;
using dcl.client.cache;
using lis.client.control;
using dcl.client.frame;

namespace wf.client.reagent.ReaControl
{
    public partial class ReagentPurchaseEditor : UserControl
    {
        public ReagentPurchaseEditor()
        {
            InitializeComponent();

            //***************************************************************************//
            //项目组合显示字体加粗
            this.txtReagentEdit.Font = new Font("宋体", 10f, FontStyle.Bold);

            //***************************************************************************//
        }
        public ReagentPurchaseEditor(Istep istep)
        {
            InitializeComponent();

            step = istep;
            //***************************************************************************//
            //项目组合显示字体加粗
            this.txtReagentEdit.Font = new Font("宋体", 10f, FontStyle.Bold);

            //***************************************************************************//
        }
        Istep step
        {
            get;
            set;
        }

        public override string Text
        {
            get
            {
                return this.txtReagentEdit.Text;
            }
        }

        /// <summary>
        /// 标题文字
        /// </summary>
        public string Caption
        {
            get
            {
                return this.lbCaption.Text;
            }
            set
            {
                this.lbCaption.Text = value;
            }
        }

        /// <summary>
        /// 是否显示标题
        /// </summary>
        public bool ShowCaption
        {
            get
            {
                return this.panel1.Visible;
            }
            set
            {
                this.panel1.Visible = value;
            }
        }

        string _group_id;

        public string GroupID {
            get
            {
                return _group_id;
            }
            set
            {
                if (_group_id != value)
                {
                    _group_id = value;

                    if (_group_id == null || _group_id.Trim(null) == string.Empty)
                    {
                        GroupName = string.Empty;
                    }
                    else
                    {
                        EntityReaSetting drRea = DictReagent.Instance.GetRea(_group_id);
                        if (drRea != null)
                        {
                            GroupName = drRea.Rea_group.ToString();
                        }
                        else
                        {
                            GroupName = string.Empty;
                        }
                    }
                }
            }
        }

        public string GroupName { get; set; }

        string _sup_id;

        public string SupID
        {
            get
            {
                return _sup_id;
            }
            set
            {
                if (_sup_id != value)
                {
                    _sup_id = value;

                    if (_sup_id == null || _sup_id.Trim(null) == string.Empty)
                    {
                        SupName = string.Empty;
                    }
                    else
                    {
                        EntityReaSetting drRea = DictReagent.Instance.GetReaBySupID(_sup_id);
                        if (drRea != null)
                        {
                            SupName = drRea.Rsupplier_name.ToString();
                        }
                        else
                        {
                            SupName = string.Empty;
                        }
                    }
                }
            }
        }

        public string SupName { get; set; }

        /// <summary>
        /// 点击按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtReagentEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OnButtonClicked(e.Button.Kind.ToString());
            if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Plus)//"+"
            {
                ShowReagentSelectForm();
            }
            else if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Up)//上
            {

            }
            else if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Down)//下
            {

            }
        }

        /// <summary>
        /// 按下按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtReagentEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 187 || e.KeyValue == 107 || e.KeyValue == 109 || e.KeyValue == 32 || e.KeyCode == Keys.Enter)//按下+ 或 空格
            {
                ShowReagentSelectForm();
            }
            else if (e.KeyValue == 8)
            {
                if (this.listReaDetail != null && this.listReaDetail.Count > 0)
                {
                    #region 获取光标上选择的项目组合来删除 by yb
                    //文本上获取所选择的项目的名称
                    string strSelectRea_name = this.txtReagentEdit.SelectedText.Trim('+').Trim();

                    //对比最有项目，获取选中的项目
                    List<EntityReaPurchaseDetail> selectListDetail = listReaDetail.FindAll(i => i.ReagentName == strSelectRea_name);

                    if (selectListDetail.Count > 0 && selectListDetail.Count == 1)//如果只有一个项目组合
                    {
                        string rea_name = selectListDetail[0].ReagentName.ToString();
                        string rea_id = selectListDetail[0].Rpcd_reaid.ToString();
                        if (MessageDialog.Show(string.Format("您确定要删除试剂[{0}]吗？", rea_name), "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            RemoveReagent(rea_id);
                        }
                    }
                    else if (selectListDetail.Count > 0 && selectListDetail.Count > 1)//如果选择的项目有相同的其它组合
                    {
                        int intSelectStart = this.txtReagentEdit.SelectionStart;
                        int intSelectLength = this.txtReagentEdit.SelectionLength;

                        int intTextIndex = 0;
                        for (int i1 = 0; i1 < selectListDetail.Count; i1++)
                        {
                            string strText = this.txtReagentEdit.Text.TrimEnd();
                            intTextIndex = strText.IndexOf(strSelectRea_name, intTextIndex);
                            if (intSelectStart == intTextIndex)
                            {
                                string com_name = selectListDetail[i1].ReagentName.ToString();
                                string rea_id = selectListDetail[i1].Rpcd_reaid.ToString();
                                if (MessageDialog.Show(string.Format("您确定要删除试剂[{0}]吗？", com_name), "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {


                                    RemoveReagent(rea_id);
                                }
                                break;
                            }
                            else
                            {
                                intTextIndex = intTextIndex + intSelectLength;
                            }


                        }



                    }

                    #endregion

                    #region 旧处理方式删除最后一项组合
                    //string com_name = this.PatientsMi.Rows[this.PatientsMi.Rows.Count - 1]["pat_com_name"].ToString();
                    //string rea_id = this.PatientsMi.Rows[this.PatientsMi.Rows.Count - 1]["pat_rea_id"].ToString();
                    //if (wf.auxiliary.control.MessageDialog.Show(string.Format("您确定要删除组合[{0}]吗？", com_name), "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    //{
                    //    RemoveReagent(rea_id);
                    //}
                    #endregion
                }
            }
        }

        FrmReagentManagerV2 frmReagentSelectForm = null;

        /// <summary>
        /// 显示组合选择框
        /// </summary>
        private void ShowReagentSelectForm()
        {
            if (this.listReaDetail == null)
                listReaDetail = new List<EntityReaPurchaseDetail>();

            ReagentFilterParam param = new ReagentFilterParam();
            param.group_id = this.GroupID;
            param.group_name = this.GroupName;
            param.sup_id = this.SupID;
            param.sup_name = this.SupName;

            if (frmReagentSelectForm == null)
            {
                frmReagentSelectForm = new FrmReagentManagerV2();
                frmReagentSelectForm.ReagentSelected += new FrmReagentManagerV2.ReagentSelectedEventHandler(frm_ReagentSelected);
            }

            frmReagentSelectForm.SetFilterParam(param);

            //给FrmReagentManagerV2界面的Reagent列表gridview赋值
            List<EntityReaSetting> reaList = new List<EntityReaSetting>();

            foreach (EntityReaPurchaseDetail item in listReaDetail)
            {
                EntityReaSetting Reagent = new EntityReaSetting();
                Reagent.Drea_id = item.Rpcd_reaid;
                Reagent.Drea_name = item.ReagentName;
                Reagent.Drea_supplier = item.sup_id;

                reaList.Add(Reagent);
            }
            frmReagentSelectForm.CurrentReaSetting = reaList;


            if (frmReagentSelectForm.Visible == false)
            {
                frmReagentSelectForm.Location = this.txtReagentEdit.Location;
                //自定义弹出窗口的宽高
                if (PopupFormWidth > 0)
                    frmReagentSelectForm.Width = PopupFormWidth;
                if (PopupFormHeight > 0)
                    frmReagentSelectForm.Height = PopupFormHeight;
                //定位弹出窗口
                frmReagentSelectForm.Left = this.txtReagentEdit.PointToScreen(new Point(0, 0)).X - PopupFormWidth; ;
                frmReagentSelectForm.Top = GetYForPopupForm(frmReagentSelectForm);

                frmReagentSelectForm.Visible = true;
            }

            //frmReagentSelectForm.Show();
        }

        /// <summary>
        /// 弹出窗口的宽
        /// </summary>
        [Description("弹出窗口的宽")]
        public int PopupFormWidth { get; set; }
        /// <summary>
        /// 弹出窗口的高
        /// </summary>
        [Description("弹出窗口的高")]
        public int PopupFormHeight { get; set; }

        private int GetYForPopupForm(FrmReagentManagerV2 frm)
        {
            if ((AnchorForPopupForm & AnchorStyles.Bottom) == AnchorStyles.Bottom)
                return this.txtReagentEdit.PointToScreen(new Point(0, this.txtReagentEdit.Height)).Y;
            else if ((AnchorForPopupForm & AnchorStyles.Top) == AnchorStyles.Top)
                return this.txtReagentEdit.PointToScreen(new Point(0, 0)).Y - frm.Height;
            else
                return 0;
        }

        private AnchorStyles anchorForPopupForm = AnchorStyles.Right | AnchorStyles.Bottom;

        /// <summary>
        /// 组合窗口的弹出位置
        /// </summary>
        [Localizable(true), DefaultValue(10)]
        [Description("组合窗口的弹出位置")]
        public AnchorStyles AnchorForPopupForm
        {
            get { return anchorForPopupForm; }
            set { anchorForPopupForm = value; }
        }

        private int GetXForPopupForm(FrmReagentManagerV2 frm)
        {
            if ((AnchorForPopupForm & AnchorStyles.Right) == AnchorStyles.Right)
                return this.txtReagentEdit.PointToScreen(new Point(0, 0)).X + this.txtReagentEdit.Width - frm.Width;
            else if ((AnchorForPopupForm & AnchorStyles.Left) == AnchorStyles.Left)
                return this.txtReagentEdit.PointToScreen(new Point(0, 0)).X;
            else
                return 0;
        }

        void frm_ReagentSelected(object sender, string rea_id, EnumAction.ReagentEditorAction action)
        {
            if (this.listReaDetail != null)
            {
                if (!ExistReagent(rea_id) && action == EnumAction.ReagentEditorAction.Add)
                {
                    AddReagent(rea_id);
                }
                else if (ExistReagent(rea_id) && action == EnumAction.ReagentEditorAction.Remove)
                {
                    RemoveReagent(rea_id);
                    if (frmReagentSelectForm.Visible == false && listReaDetail.Count > 0)
                    {
                        frmReagentSelectForm.Location = this.txtReagentEdit.Location;
                        //自定义弹出窗口的宽高
                        if (PopupFormWidth > 0)
                            frmReagentSelectForm.Width = PopupFormWidth;
                        if (PopupFormHeight > 0)
                            frmReagentSelectForm.Height = PopupFormHeight;
                        //定位弹出窗口
                        frmReagentSelectForm.Left = this.txtReagentEdit.PointToScreen(new Point(0, 0)).X - PopupFormWidth;
                        frmReagentSelectForm.Top = GetYForPopupForm(frmReagentSelectForm);

                        frmReagentSelectForm.Visible = true;
                    }
                }

                if (this.selectMode == ReagentSelectMode.Single)
                {
                    (sender as System.Windows.Forms.Form).Visible = false;
                }
            }
        }

        /// <summary>
        /// 当前病人组合中是否存在指定的组合
        /// </summary>
        /// <param name="rea_id"></param>
        /// <returns></returns>
        private bool ExistReagent(string rea_id)
        {
            if (this.listReaDetail != null && this.listReaDetail.Count > 0)
            {
                if (this.listReaDetail.FindAll(i => i.Rpcd_reaid == rea_id).Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        #region IPatientReagentEditor 成员

        private DataTable dtPatMi;
        public DataTable PatientsMi
        {
            get
            {
                return dtPatMi;
            }
            set
            {
                dtPatMi = value;
                RefreshEditBoxText();
            }
        }

        #endregion

        private List<EntityReaPurchaseDetail> listPatMi;
        public List<EntityReaPurchaseDetail> listReaDetail
        {
            get
            {

                return listPatMi;
            }
            set
            {
                listPatMi = value;
                RefreshEditBoxText();
            }
        }

        public void RefreshEditBoxText()
        {
            this.txtReagentEdit.EditValue = string.Empty;

            StringBuilder sb = new StringBuilder();

            if (listReaDetail != null && listReaDetail.Count > 0)
            {


                for (int i = 0; i < listReaDetail.Count; i++)
                {
                    sb.Append(" + ");
                    sb.Append(listReaDetail[i].ReagentName);
                }
                //}
                //else
                //{
                //    foreach (DataRow item in this.PatientsMi.Rows)
                //    {
                //        sb.Append(" + ");
                //        sb.Append(item["pat_com_name"].ToString());
                //    }
                //}
            }
            if (sb.Length > 3)
            {
                sb.Remove(0, 3);
            }
            this.txtReagentEdit.EditValue = sb.ToString();
            this.txtReagentEdit.SelectionStart = this.txtReagentEdit.Text.Length;
        }


        public event ReagentAddedEventHandler ReagentAdded;
        public event ReagentRemovedEventHandler ReagentRemoved;


        public void OnReagentAdded(string rea_id, int com_seq)
        {
            if (ReagentAdded != null)
            {
                ReagentAdded(this, rea_id, com_seq);
            }
        }

        public void OnReagentRemoved(string rea_id)
        {
            if (ReagentRemoved != null)
            {
                ReagentRemoved(this, rea_id);
            }
        }

        public void RemoveReagent(string rea_id)
        {
            List<EntityReaPurchaseDetail> reaListDetail = listReaDetail.FindAll(i => i.Rpcd_reaid == rea_id);
            foreach (EntityReaPurchaseDetail dr in reaListDetail)
            {
                dr.IsNew = 2;
                listReaDetail.Remove(dr);
            }

            OnReagentRemoved(rea_id);
            RefreshEditBoxText();
        }

        public void AddReagent(string rea_id)
        {
            AddReagent(rea_id, null, null, null);
        }

        public void AddReagent(string rea_id, string yz_id, decimal? price, string bar_code)
        {
            if (listReaDetail != null && listReaDetail.FindAll(i => i.Rpcd_reaid == rea_id).Count == 0)
            {
                EntityReaSetting drReagent = CacheClient.GetCache<EntityReaSetting>().Find(i => i.Drea_id == rea_id);

                if (drReagent != null)
                {
                    int rea_seq = listReaDetail.Count + 1;//13-5-17
                    EntityReaPurchaseDetail drPatReaMi = new EntityReaPurchaseDetail();
                    drPatReaMi.Rpcd_reaid = rea_id;
                    drPatReaMi.ReagentName = drReagent.Drea_name;
                    drPatReaMi.sup_id = drReagent.Drea_supplier;
                    drPatReaMi.IsNew = 1;
                    listReaDetail.Add(drPatReaMi);
                    OnReagentAdded(rea_id, rea_seq);
                    RefreshEditBoxText();
                }
            }
        }

        public void AddReagent2(string rea_id, string yz_id, decimal? price, string bar_code)
        {
            if (listReaDetail != null )
            {
                EntityReaSetting drReagent = CacheClient.GetCache<EntityReaSetting>().Find(i => i.Drea_id == rea_id);

                if (drReagent != null)
                {
                    int rea_seq = listReaDetail.Count + 1;//13-5-17
                    EntityReaPurchaseDetail drPatReaMi = new EntityReaPurchaseDetail();
                    drPatReaMi.Rpcd_reaid = rea_id;
                    drPatReaMi.ReagentName = drReagent.Drea_name;
                    drPatReaMi.sup_id = drReagent.Drea_supplier;
                    listReaDetail.Add(drPatReaMi);
                    OnReagentAdded(rea_id, rea_seq);
                    RefreshEditBoxText();
                }
            }
        }

        public event EventHandler Reseted;

        public void Reset()
        {
            if (this.listPatMi != null)
            {
                this.listPatMi.Clear();
            }

            if (Reseted != null)
            {
                Reseted(this, EventArgs.Empty);
            }
        }

        public event ReagentEditBoxButtonClick ButtonClicked;

        private void OnButtonClicked(string buttonType)
        {
            if (ButtonClicked != null)
            {
                ButtonClicked(this, buttonType);
            }
        }

        private void ReagentEditor_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            string val = UserInfo.GetSysConfigValue("ReagentBoxSelectMode");

            if (val == "单选择")
            {
                selectMode = ReagentSelectMode.Single;
            }
            else
            {
                selectMode = ReagentSelectMode.Multiple;
            }
        }


        private ReagentSelectMode selectMode;

        private enum ReagentSelectMode
        {
            Single,
            Multiple,
        }

        

        private void txtReagentEdit_DoubleClick(object sender, EventArgs e)
        {
            ShowReagentSelectForm();
        }

        //*******************************************************************************//
        //冒泡排序
        private int[] SortReagent(int[] a)
        {
            int[] tempArry = new int[a.Length];
            for (int i = 0; i < tempArry.Length; i++)
            {
                tempArry[i] = i;
            }
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < a.Length - 1; j++)
                {
                    if (a[j] > a[j + 1])
                    {
                        int temp = a[j];
                        a[j] = a[j + 1];
                        a[j + 1] = temp;

                        int temp1 = tempArry[j];
                        tempArry[j] = tempArry[j + 1];
                        tempArry[j + 1] = temp1;
                    }
                }
            }

            return tempArry;
        }

        //********************************************************************************//
    }


}
