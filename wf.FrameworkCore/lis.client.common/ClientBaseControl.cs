using System;
using System.Collections.Generic;

using System.Text;
using dcl.client.frame;

using System.Data;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using dcl.common.extensions;
using DevExpress.XtraEditors;
using Lib.LogManager;
using dcl.common;


namespace dcl.client.common
{
    enum SaveAction
    {
        Add,
        Edit,
        Unknown
    }

    public class ClientBaseControl : ConCommonExt, IClientBase
    {
        protected DataTable dtSub = new DataTable();
        protected BindingSource bsSub;
        protected string subTable;
        protected GridControl gcSub;
        protected string statusColumn;
        protected string flagColumn;
        protected GridView gvSub;
        protected string otherRequireColumn;
        protected string warnOfOtherRequireColumn;
        /// <summary>
        /// 主键
        /// </summary>
        protected string primaryKeyOfSubTable;
        private SaveAction saveAction = SaveAction.Unknown;
        /// <summary>
        /// 右边主编辑框
        /// </summary>
        public Control BaseInfoContainer { get; set; }
        /// <summary>
        /// 右边副编辑框
        /// </summary>
        public Control BaseInfoContainerExt { get; set; }

        #region 支持五笔与拼音码的自动生成

        /// <summary>
        ///  名称控件
        /// </summary>
        public TextEdit NameControl { get; set; }
        /// <summary>
        /// 拼音控件
        /// </summary>
        public TextEdit SpellControl { get; set; }
        /// <summary>
        /// 五笔控件
        /// </summary>
        public TextEdit WuBiControl { get; set; }

        /// <summary>
        /// 自动生成五笔与拼音码
        /// </summary>
        private void AutoCompleteWubiAndSpell()
        {
            if (NameControl == null)
                return;

            NameControl.TextChanged += new EventHandler(NameControl_TextChanged);
        }

        void NameControl_TextChanged(object sender, EventArgs e)
        {
            SpellAndWbCodeTookit tookit = new SpellAndWbCodeTookit();
            if (SpellControl != null)
                SpellControl.Text = tookit.GetSpellCode(NameControl.Text);
            if (WuBiControl != null)
                WuBiControl.Text = tookit.GetWBCode(NameControl.Text);

        }


        #endregion

        private bool groupBy = false;
        /// <summary>
        /// 是否按组排列
        /// </summary>
        protected bool GroupBy { get { return groupBy; } set { groupBy = value; } }

        #region IBarAction 成员

        public void Add()
        {
            DataRow row = dtSub.NewRow();
            SetDefaultValue(row);
            dtSub.Rows.Add(row);
            //if (!GroupBy)
            gvSub.MoveLast();
            saveAction = SaveAction.Add;
            if (HasBaseInfo)
            {
                EnableBaseInfo(true);
                ClearBaseInfo();
            }
            else
                EnableGridView(true);
        }

        /// <summary>
        /// 设置默认值
        /// </summary>
        /// <param name="row"></param>
        protected virtual void SetDefaultValue(DataRow row)
        {

        }

        /// <summary>
        /// 清除右边主编辑的所有控件文本
        /// </summary>
        protected virtual void ClearBaseInfo()
        {
            foreach (Control item in BaseInfoContainer.Controls)
            {
                if (!(item is Label))
                {

                    if (item is TextEdit)
                        (item as TextEdit).Text = "";

                    if (item is MemoEdit)
                        (item as MemoEdit).Text = "";
                }
            }
        }

        public virtual void Save()
        {
            EndEditing();
            if (this.gvSub.FocusedRowHandle < 0 && saveAction == SaveAction.Edit)
            {
                return;
            }

            DataSet ds = new DataSet();

            #region 换成另一方法执行
            //DataRow dr = ((DataRowView)bsSub.Current).Row;

            //if (!string.IsNullOrEmpty(otherRequireColumn) && dr[otherRequireColumn].ToString() == "")
            //{
            //    MessageBox.Show(warnOfOtherRequireColumn, "提示");
            //    throw new Exception();
            //}

            //DataTable dtUpdate = this.dtSub.Clone();
            //BeforeSave(dr);
            //dtUpdate.Rows.Add(dr.ItemArray);
            #endregion


            DataSet result = new DataSet();
            if (saveAction == SaveAction.Add)
            {
                DataTable dt = this.m_dtbGetUpdateDataByAddNew();
                if (dt.TableName == "dict_interfaces")
                    dt = encryption(dt);
                ds.Tables.Add(dt);
                result = base.doNew(ds);
            }
            else if (saveAction == SaveAction.Edit)
            {
                DataTable dt = this.m_dtbGetUpdateData();
                if (dt.TableName == "dict_interfaces")
                    dt = encryption(dt);
                ds.Tables.Add(dt);
                result = base.doUpdate(ds);
            }
            else
            {
                return;
            }

            if (base.isActionSuccess)
            {
                this.dtSub.AcceptChanges();
                if (!HasBaseInfo)
                {
                    EnableGridView(false);
                }
                if (HasBaseInfo)
                {
                    EnableBaseInfo(false);
                }
                //InitData();
            }
        }

        private DataTable encryption(DataTable dt)
        {
            foreach (DataRow item in dt.Rows)
            {
                item["in_db_address"] = EncryptClass.Encrypt(item["in_db_address"].ToString());
                item["in_db_name"] = EncryptClass.Encrypt(item["in_db_name"].ToString());
                item["in_db_username"] = EncryptClass.Encrypt(item["in_db_username"].ToString());
                item["in_db_password"] = EncryptClass.Encrypt(item["in_db_password"].ToString());
            }
            return dt;
        }

        /// <summary>
        /// 获取操作的行，并添加到更新dataset里
        /// </summary>
        /// <returns></returns>
        protected virtual DataTable m_dtbGetUpdateData()
        {

            return m_dtbGetUpdateDataByAddNew();
        }

        /// <summary>
        /// 获取操作的行，并添加到新加dataset里
        /// </summary>
        /// <returns></returns>
        protected virtual DataTable m_dtbGetUpdateDataByAddNew()
        {
            DataRow dr = ((DataRowView)bsSub.Current).Row;

            if (!string.IsNullOrEmpty(otherRequireColumn) && dr[otherRequireColumn].ToString() == "")
            {
                MessageBox.Show(warnOfOtherRequireColumn, "提示");
                throw new Exception();
            }

            DataTable dtUpdate = this.dtSub.Clone();
            BeforeSave(dr);
            dtUpdate.Rows.Add(dr.ItemArray);
            return dtUpdate;
        }

        private void EndEditing()
        {
            gvSub.CloseEditor();
            bsSub.EndEdit();
        }

        protected virtual void BeforeSave(DataRow dataRow)
        {

        }

        public virtual void Cancel()
        {
            EndEditing();
            InitData();
            if (HasBaseInfo)
                EnableBaseInfo(false);
            else
                EnableGridView(false);
        }

        /// <summary>
        /// 使主数据表可用
        /// </summary>
        public virtual bool EnableGridView(bool enable)
        {
            return gvSub.OptionsBehavior.Editable = enable;
        }

        public void Edit()
        {
            if (HasBaseInfo)
                EnableBaseInfo(true);
            else
                EnableGridView(true);
            saveAction = SaveAction.Edit;
        }

        public void Close()
        {
            this.ParentForm.Close();
        }

        /// <summary>
        /// 删除
        /// </summary>
        public void Delete()
        {
            try
            {
                this.bsSub.EndEdit();
                if (this.gvSub.FocusedRowHandle < 0)
                {
                    MessageBox.Show("未选择需删除行！", "提示");
                    return;
                }

                DataSet ds = new DataSet();
                DataRow dr = this.gvSub.GetFocusedDataRow();

                DataTable dtUpdate = this.dtSub.Clone();
                //dtUpdate.Rows.Add(dtSub.Rows[this.gvSub.FocusedRowHandle].ItemArray);
                dtUpdate.Rows.Add(dr.ItemArray);

                ds.Tables.Add(dtUpdate);
                DataSet result = new DataSet();

                DialogResult dresult = MessageBox.Show("确定要删除该记录吗? ", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                switch (dresult)
                {
                    case DialogResult.OK:
                        base.doDel(ds);
                        break;
                    case DialogResult.Cancel:
                        return;
                }

                if (base.isActionSuccess)
                {
                    dtSub.Rows.Remove(dr);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("删除出错", ex);
            }
        }

        public virtual void AddActiveCtrls(ref Dictionary<string, bool> controlsList) { }

        /// <summary>
        ///初始化参数
        /// </summary>
        public virtual void InitParamters() { }

        public void DoRefresh()
        {
            InitData();
        }

        public Dictionary<string, bool> GetActiveCtrls()
        {
            Dictionary<string, Boolean> dlist = new Dictionary<string, bool>();
            dlist.Clear();
            AddActiveCtrls(ref dlist);
            return dlist;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode)
                Init();

            base.OnLoad(e);

            if (!DesignMode)
                InitData();

            AutoCompleteWubiAndSpell();
        }


        #region DesignMode

        protected new bool DesignMode
        {
            get
            {
                bool returnFlag = false;
#if DEBUG
                if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                    returnFlag = true;
                else if (System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToUpper().Equals("DEVENV"))
                    returnFlag = true;
#endif
                return returnFlag;
            }
        }


        #endregion


        public virtual void InitData()
        {
            if (DesignMode)
                return;

            DataSet ds = base.doSearch();

            if (isActionSuccess)
            {
                dtSub.AcceptChanges();
                bsSub.DataSource = dtSub = ds.Tables[subTable];

                InitOtherData();
                if (HasBaseInfo)
                    EnableBaseInfo(false);

                EnableGridView(false);
            }
        }

        /// <summary>
        /// 修改基础控件是否可编辑
        /// </summary>
        protected virtual void EnableBaseInfo(bool enable)
        {
            foreach (Control item in BaseInfoContainer.Controls)
            {
                if (!(item is Label))
                {
                    if (item is TextEdit)
                        (item as TextEdit).Properties.ReadOnly = !enable;

                    if (item is MemoEdit)
                        (item as MemoEdit).Properties.ReadOnly = !enable;

                }
            }

            if (BaseInfoContainerExt != null)
                foreach (Control item in BaseInfoContainerExt.Controls)
                {
                    if (!(item is Label))
                    {
                        if (item is SimpleButton)
                            (item as SimpleButton).Enabled = enable;
                    }
                }

            ControlsEnable = enable;
        }

        public bool ControlsEnable { get; set; }

        /// <summary>
        /// 有右边主编辑框
        /// </summary>
        protected virtual bool HasBaseInfo
        {
            get { return BaseInfoContainer != null; }
        }

        protected virtual void InitOtherData()
        {

        }

        private void Init()
        {
            InitParamters();
        }

        #endregion

        /// <summary>
        /// 绑定文本
        /// </summary>
        /// <param name="textbox"></param>
        /// <param name="column"></param>
        /// <param name="bs"></param>
        protected void BindingText(Control textbox, string column, BindingSource bs)
        {
            if (textbox.DataBindings.Count == 0)
                textbox.DataBindings.Add(new System.Windows.Forms.Binding("Text", bs, column, true));
        }

        #region IBarActionExt 成员


        public void MoveNext()
        {
            gvSub.MoveNext();
        }

        public void MovePrev()
        {
            gvSub.MovePrev();
        }

        #endregion
    }
}
