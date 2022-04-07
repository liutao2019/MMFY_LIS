using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.result.Interface;
using dcl.client.result.DictToolkit;
using dcl.client.result.CommonPatientInput;
using dcl.client.frame;
using lis.client.control;
using dcl.entity;
using dcl.client.common;
using dcl.client.cache;

namespace dcl.client.result.PatControl
{
    public partial class CombineEditor : UserControl, ICombineEditor
    {
        public CombineEditor()
        {
            InitializeComponent();

            bShowUpDownButton = false;

            //***************************************************************************//
            //项目组合显示字体加粗
            this.txtCombineEdit.Font = new Font("宋体", 10f, FontStyle.Bold);

            //***************************************************************************//
        }

        public override string Text
        {
            get
            {
                return this.txtCombineEdit.Text;
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

        private bool bShowUpDownButton;

        /// <summary>
        /// 是否显示上下按钮
        /// </summary>
        public bool ShowUpDownButton
        {
            get
            {
                return bShowUpDownButton;
            }
            set
            {
                bShowUpDownButton = value;
                this.txtCombineEdit.Properties.Buttons[0].Visible = value;
                this.txtCombineEdit.Properties.Buttons[1].Visible = value;
            }
        }

        private string ptype;

        /// <summary>
        /// 专业组ID
        /// </summary>
        public string PTypeID { get; set; }
        //{
        //    get
        //    {
        //        return ptype;
        //    }
        //    set
        //    {
        //        if (ptype != value)
        //        {
        //            ptype = value;
        //            if (ptype == null && ptype.Trim(null) == string.Empty)
        //            {
        //                PTypeName = string.Empty;
        //            }
        //            else
        //            {
        //                PTypeName = DictType.Instance.GetTypeName(ptype);
        //            }
        //        }
        //    }
        //}
        public string PTypeName { get; private set; }

        private string ctype;

        /// <summary>
        /// 物理组ID
        /// </summary>
        public string CTypeID
        {
            get
            {
                return ctype;
            }
            set
            {
                if (ctype != value)
                {
                    ctype = value;
                    if (ctype == null || ctype.Trim(null) == string.Empty)
                    {
                        CTypeName = string.Empty;
                    }
                    else
                    {
                        CTypeName = DictType.Instance.GetTypeName(ctype);
                    }
                }
            }
        }
        public string CTypeName { get; set; }

        string _itr_id;

        /// <summary>
        /// 仪器ID
        /// </summary>
        public string ItrID
        {
            get
            {
                return _itr_id;
            }
            set
            {
                if (_itr_id != value)
                {
                    _itr_id = value;

                    if (_itr_id == null || _itr_id.Trim(null) == string.Empty)
                    {
                        ItrName = string.Empty;
                    }
                    else
                    {
                        EntityDicInstrument drInst = DictInstrmt.Instance.GetItr(_itr_id);
                        if (drInst != null)
                        {
                            ItrName = drInst.ItrName.ToString();
                        }
                        else
                        {
                            ItrName = string.Empty;
                        }

                        this.PTypeID = DictInstrmt.Instance.GetItrPTypeID(_itr_id);
                        this.PTypeName = DictType.Instance.GetTypeName(this.PTypeID);
                    }
                }
            }
        }

        public string ItrName { get; set; }

        /// <summary>
        /// 点击按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCombineEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OnButtonClicked(e.Button.Kind.ToString());
            if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Plus)//"+"
            {
                ShowCombineSelectForm();
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
        private void txtCombineEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 187 || e.KeyValue == 107 || e.KeyValue == 109 || e.KeyValue == 32 || e.KeyCode == Keys.Enter)//按下+ 或 空格
            {
                ShowCombineSelectForm();
            }
            else if (e.KeyValue == 8)
            {
                if (this.listRepDetail != null && this.listRepDetail.Count > 0)
                {
                    #region 获取光标上选择的项目组合来删除 by yb
                    //文本上获取所选择的项目的名称
                    string strSelectCom_name = this.txtCombineEdit.SelectedText.Trim('+').Trim();

                    //对比最有项目，获取选中的项目
                    List<EntityPidReportDetail> selectListDetail = listRepDetail.FindAll(i => i.PatComName == strSelectCom_name);

                    if (selectListDetail.Count > 0 && selectListDetail.Count == 1)//如果只有一个项目组合
                    {
                        string com_name = selectListDetail[0].PatComName.ToString();
                        string com_id = selectListDetail[0].ComId.ToString();
                        if (lis.client.control.MessageDialog.Show(string.Format("您确定要删除组合[{0}]吗？", com_name), "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            RemoveCombine(com_id);
                        }
                    }
                    else if (selectListDetail.Count > 0 && selectListDetail.Count > 1)//如果选择的项目有相同的其它组合
                    {
                        int intSelectStart = this.txtCombineEdit.SelectionStart;
                        int intSelectLength = this.txtCombineEdit.SelectionLength;

                        int intTextIndex = 0;
                        for (int i1 = 0; i1 < selectListDetail.Count; i1++)
                        {
                            string strText = this.txtCombineEdit.Text.TrimEnd();
                            intTextIndex = strText.IndexOf(strSelectCom_name, intTextIndex);
                            if (intSelectStart == intTextIndex)
                            {
                                string com_name = selectListDetail[i1].PatComName.ToString();
                                string com_id = selectListDetail[i1].ComId.ToString();
                                if (lis.client.control.MessageDialog.Show(string.Format("您确定要删除组合[{0}]吗？", com_name), "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {


                                    RemoveCombine(com_id);
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
                    //string com_id = this.PatientsMi.Rows[this.PatientsMi.Rows.Count - 1]["pat_com_id"].ToString();
                    //if (lis.client.control.MessageDialog.Show(string.Format("您确定要删除组合[{0}]吗？", com_name), "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    //{
                    //    RemoveCombine(com_id);
                    //}
                    #endregion


                }
            }
        }

        FrmCombineManagerV2 frmCombineSelectForm = null;

        /// <summary>
        /// 显示组合选择框
        /// </summary>
        private void ShowCombineSelectForm()
        {
            if (this.listRepDetail == null)
                listRepDetail = new List<EntityPidReportDetail>();

            CombineFilterParam param = new CombineFilterParam();
            param.com_ctype = this.ctype;
            param.ctype_name = this.CTypeName;
            param.itr_id = this.ItrID;
            param.itr_name = this.ItrName;
            param.ptype_id = this.PTypeID;
            param.ptype_name = this.PTypeName;

            if (frmCombineSelectForm == null)
            {
                frmCombineSelectForm = new FrmCombineManagerV2();
                frmCombineSelectForm.CombineSelected += new FrmCombineManagerV2.CombineSelectedEventHandler(frm_CombineSelected);
            }

            frmCombineSelectForm.SetFilterParam(param);

            //给FrmCombineManagerV2界面的combine列表gridview赋值
            List<EntityDicCombine> combList = new List<EntityDicCombine>();
            foreach (EntityPidReportDetail item in listRepDetail)
            {
                EntityDicCombine combine = new EntityDicCombine();
                combine.ComId = item.ComId;
                combine.ComName = item.PatComName;

                combList.Add(combine);
            }
            frmCombineSelectForm.CurrentCombine = combList;


            if (frmCombineSelectForm.Visible == false)
            {
                frmCombineSelectForm.Location = this.txtCombineEdit.Location;
                //自定义弹出窗口的宽高
                if (PopupFormWidth > 0)
                    frmCombineSelectForm.Width = PopupFormWidth;
                if (PopupFormHeight > 0)
                    frmCombineSelectForm.Height = PopupFormHeight;
                //定位弹出窗口
                frmCombineSelectForm.Left = this.txtCombineEdit.PointToScreen(new Point(0, 0)).X - PopupFormWidth; ;
                frmCombineSelectForm.Top = GetYForPopupForm(frmCombineSelectForm);

                frmCombineSelectForm.Visible = true;
            }

            //frmCombineSelectForm.Show();
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

        private int GetYForPopupForm(FrmCombineManagerV2 frm)
        {
            if ((AnchorForPopupForm & AnchorStyles.Bottom) == AnchorStyles.Bottom)
                return this.txtCombineEdit.PointToScreen(new Point(0, this.txtCombineEdit.Height)).Y;
            else if ((AnchorForPopupForm & AnchorStyles.Top) == AnchorStyles.Top)
                return this.txtCombineEdit.PointToScreen(new Point(0, 0)).Y - frm.Height;
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

        private int GetXForPopupForm(FrmCombineManagerV2 frm)
        {
            if ((AnchorForPopupForm & AnchorStyles.Right) == AnchorStyles.Right)
                return this.txtCombineEdit.PointToScreen(new Point(0, 0)).X + this.txtCombineEdit.Width - frm.Width;
            else if ((AnchorForPopupForm & AnchorStyles.Left) == AnchorStyles.Left)
                return this.txtCombineEdit.PointToScreen(new Point(0, 0)).X;
            else
                return 0;
        }

        void frm_CombineSelected(object sender, string com_id, CombineEditor.CombineEditorAction action)
        {
            if (this.listRepDetail != null)
            {
                if (!ExistCombine(com_id) && action == CombineEditorAction.Add)
                {
                    AddCombine(com_id);
                }
                else if (ExistCombine(com_id) && action == CombineEditorAction.Remove)
                {
                    RemoveCombine(com_id);
                    if (frmCombineSelectForm.Visible == false && listRepDetail.Count > 0)
                    {
                        frmCombineSelectForm.Location = this.txtCombineEdit.Location;
                        //自定义弹出窗口的宽高
                        if (PopupFormWidth > 0)
                            frmCombineSelectForm.Width = PopupFormWidth;
                        if (PopupFormHeight > 0)
                            frmCombineSelectForm.Height = PopupFormHeight;
                        //定位弹出窗口
                        frmCombineSelectForm.Left = this.txtCombineEdit.PointToScreen(new Point(0, 0)).X - PopupFormWidth;
                        frmCombineSelectForm.Top = GetYForPopupForm(frmCombineSelectForm);

                        frmCombineSelectForm.Visible = true;
                    }
                }

                if (this.selectMode == CombineSelectMode.Single)
                {
                    (sender as System.Windows.Forms.Form).Visible = false;
                }
            }
        }

        /// <summary>
        /// 当前病人组合中是否存在指定的组合
        /// </summary>
        /// <param name="com_id"></param>
        /// <returns></returns>
        private bool ExistCombine(string com_id)
        {
            if (this.listRepDetail != null && this.listRepDetail.Count > 0)
            {
                if (this.listRepDetail.FindAll(i => i.ComId == com_id).Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        #region IPatientCombineEditor 成员

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

        private List<EntityPidReportDetail> listPatMi;
        public List<EntityPidReportDetail> listRepDetail
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
            this.txtCombineEdit.EditValue = string.Empty;

            StringBuilder sb = new StringBuilder();

            if (listRepDetail != null && listRepDetail.Count > 0)
            {
                //***************************************************************************//
                //根据组合进行排序，以达到在显示框中组合的名字能跟字典设置一样。
                //if (PatientsMi.Columns.Contains("com_seq"))
                //{
                int[] a = new int[listRepDetail.Count];
                for (int i = 0; i < a.Length; i++)
                {
                    if (string.IsNullOrEmpty(listRepDetail[i].ComSeq))
                        a[i] = 99999;
                    else
                        a[i] = Convert.ToInt32(listRepDetail[i].ComSeq);
                }
                a = SortCombine(a);

                for (int i = 0; i < listRepDetail.Count; i++)
                {
                    sb.Append(" + ");
                    sb.Append(listRepDetail[a[i]].PatComName);
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

                //*************************************************************************//
                #region 旧代码
                //foreach (DataRow item in this.PatientsMi.Rows)
                //{
                //    sb.Append(" + ");
                //    sb.Append(item["pat_com_name"].ToString());
                //}
                #endregion
            }
            if (sb.Length > 3)
            {
                sb.Remove(0, 3);
            }
            this.txtCombineEdit.EditValue = sb.ToString();
            this.txtCombineEdit.SelectionStart = this.txtCombineEdit.Text.Length;
        }


        public event CombineAddedEventHandler CombineAdded;
        public event CombineRemovedEventHandler CombineRemoved;


        public void OnCombineAdded(string com_id, int com_seq)
        {
            if (CombineAdded != null)
            {
                CombineAdded(this, com_id, com_seq);
            }
        }

        public void OnCombineRemoved(string com_id)
        {
            if (CombineRemoved != null)
            {
                CombineRemoved(this, com_id);
            }
        }

        public void RemoveCombine(string com_id)
        {
            List<EntityPidReportDetail> partListDetail = listRepDetail.FindAll(i => i.ComId == com_id);
            foreach (EntityPidReportDetail dr in partListDetail)
            {
                listRepDetail.Remove(dr);
            }

            //移除组合后重新编排顺序
            int seq = 0;
            foreach (EntityPidReportDetail dr in listRepDetail)
            {
                dr.SortNo = seq;
                seq++;
            }

            OnCombineRemoved(com_id);
            RefreshEditBoxText();
        }

        public void AddCombine(string com_id)
        {
            AddCombine(com_id, null, null, null);
        }

        public void AddCombine(string com_id, string yz_id, decimal? price, string bar_code)
        {
            if (listRepDetail != null && listRepDetail.FindAll(i => i.ComId == com_id).Count == 0)
            {
                EntityDicCombine drCombine = CacheClient.GetCache<EntityDicCombine>().Find(i => i.ComId == com_id);

                if (drCombine != null)
                {
                    int com_seq = listRepDetail.Count + 1;//13-5-17
                    EntityPidReportDetail drPatComMi = new EntityPidReportDetail();
                    drPatComMi.ComId = com_id;
                    drPatComMi.PatComName = drCombine.ComName;
                    if (!string.IsNullOrEmpty(price.ToString()))
                    {
                        drPatComMi.OrderPrice = price.ToString();//价格采用His设置的价格
                    }
                    else {
                        drPatComMi.OrderPrice = drCombine.ComPrice.ToString();//价格采用Lis设置的价格
                    }
                    drPatComMi.ComSeq = drCombine.ComSortNo.ToString();//组合排序号
                    drPatComMi.OrderSn = yz_id;
                    drPatComMi.SortNo = com_seq;
                    drPatComMi.RepBarCode = bar_code;
                    listRepDetail.Add(drPatComMi);
                    OnCombineAdded(com_id, com_seq);
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

        public event CombineEditBoxButtonClick ButtonClicked;

        private void OnButtonClicked(string buttonType)
        {
            if (ButtonClicked != null)
            {
                ButtonClicked(this, buttonType);
            }
        }

        private void CombineEditor_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            string val = UserInfo.GetSysConfigValue("CombineBoxSelectMode");

            if (val == "单选择")
            {
                selectMode = CombineSelectMode.Single;
            }
            else
            {
                selectMode = CombineSelectMode.Multiple;
            }
        }


        private CombineSelectMode selectMode;

        private enum CombineSelectMode
        {
            Single,
            Multiple,
        }

        public enum CombineEditorAction
        {
            Add,
            Remove,
        }

        private void txtCombineEdit_DoubleClick(object sender, EventArgs e)
        {
            ShowCombineSelectForm();
        }

        //*******************************************************************************//
        //冒泡排序
        private int[] SortCombine(int[] a)
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
