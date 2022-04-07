using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using dcl.common;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using dcl.client.common;

namespace lis.client.control
{
    public partial class HopePopSelect : UserControl
    {
        #region 私有变量
        private bool useExtend = false;
        private bool keyUpDownMoveNext = false;
        private bool _selectOnly = true;
        private bool _saveSourceID = false;
        private String _valueMember;
        private String _displayMember;
        private String _colSeq = "";
        private String _colExtend1;
        private String _colValue;
        private String _colDisplay;
        private String _colWB;
        private String _colPY;
        private String _colInCode;
        private bool canChange = false;
        /// <summary>
        /// 根据条件是否找到了匹配项
        /// </summary>
        private bool HasSearched = false;
        #endregion

        public override Font Font
        {
            get
            {
                return this.popupContainerEdit1.Font;
            }
            set
            {
                this.popupContainerEdit1.Font = value;
            }
        }

        #region 保护变量与方法

        protected virtual GridControl getGC() { return null; }
        protected virtual GridView getGV() { return null; }
        protected virtual DataTable getDataSource()
        { return null; }
        protected virtual List<String> getSearchCol()
        { return null; }

        protected virtual void OnValueChanged(ValueChangeEventArgs args)
        {
            if (ValueChanged != null)
                ValueChanged(this, args);
        }

        protected void OnDisplayTextChanged(ValueChangeEventArgs args)
        {
            if (DisplayTextChanged != null)
                DisplayTextChanged(this, args);
        }

        /// <summary>
        /// 设置下拉表格显示的数据，默认为前匹配，名称全模糊，所有检索字段查询,
        /// </summary>
        /// <returns></returns>
        protected virtual DataRow[] setFilteredData(String searchStr)
        {
            searchStr = SQLFormater.Format(searchStr);

            String strSearch = "";
            //if (_colExtend1 != null)
            //{
            //    strSearch = String.Format(
            //    "({0} like '" + searchStr + "%' or " +
            //    "{1} like '" + searchStr + "%' or " +
            //    "{2} like '" + searchStr + "%' or " +
            //    "{3} like '%" + searchStr + "%'  or " +
            //    "{4} like '%" + searchStr + "%') "
            //    , _colInCode, _colPY, _colWB, _colDisplay, _colExtend1);
            //}
            //else
            //{
            //    strSearch = String.Format(
            //    "({0} like '" + searchStr + "%' or " +
            //    "{1} like '" + searchStr + "%' or " +
            //    "{2} like '" + searchStr + "%' or " +
            //    "{3} like '%" + searchStr + "%') ", _colInCode, _colPY, _colWB, _colDisplay);

            //}
            if (!string.IsNullOrEmpty(_colInCode))
            {
                strSearch += String.Format("{0} like '" + searchStr + "%'", _colInCode);
            }
            if (!string.IsNullOrEmpty(_colPY))
            {
                if (!string.IsNullOrEmpty(strSearch))
                {
                    strSearch += " or ";
                }
                strSearch += String.Format("{0} like '" + searchStr + "%'", _colPY);

            }
            if (!string.IsNullOrEmpty(_colWB))
            {
                if (!string.IsNullOrEmpty(strSearch))
                {
                    strSearch += " or ";
                }
                strSearch += String.Format("{0} like '" + searchStr + "%'", _colWB);

            }
            if (!string.IsNullOrEmpty(_colDisplay))
            {
                if (!string.IsNullOrEmpty(strSearch))
                {
                    strSearch += " or ";
                }
                strSearch += String.Format("{0} like '%" + searchStr + "%'", _colDisplay);

            }
            //增加登录账号过滤
            if (!string.IsNullOrEmpty(_colValue))
            {
                if (!string.IsNullOrEmpty(strSearch))
                {
                    strSearch += " or ";
                }
                strSearch += String.Format("{0} like '" + searchStr + "%'", _colValue);

            }
            if (!string.IsNullOrEmpty(_colExtend1))
            {
                if (!string.IsNullOrEmpty(strSearch))
                {
                    strSearch += " or ";
                }
                strSearch += String.Format("{0} like '%" + searchStr + "%' ", _colExtend1);

            }
            if (!string.IsNullOrEmpty(strSearch))
            {
                strSearch = string.Format("({0})", strSearch);
            }
            this.FireBeforeFilterEvent(ref strSearch);

            //if (this.onBeforeFilter != null)
            //    this.onBeforeFilter(ref strSearch);

            DataRow[] drs = dtSource.Select(strSearch);
            HasSearched = (drs.Length != 0);
            return drs;
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
        }
        #endregion

        #region 公开变量与事件
        public DataRow selectRow;
        public delegate void afterSelected(Object sender, EventArgs e);
        public event afterSelected onAfterSelected;
        public delegate void afterChange(DataRow oldRow);
        public event afterChange onAfterChange;

        public delegate void beforeFilter(ref String strFilter);
        public event beforeFilter onBeforeFilter;

        protected virtual void FireBeforeFilterEvent(ref string strFilter)
        {
            if (onBeforeFilter != null)
            {
                onBeforeFilter(ref strFilter);
            }
        }

        public delegate void ValueChangedEventHandler(object sender, ValueChangeEventArgs args);
        public event ValueChangedEventHandler ValueChanged;
        public delegate void TextChangedEventHandler(object sender, ValueChangeEventArgs args);
        public event TextChangedEventHandler DisplayTextChanged;
        public DataTable dtSource = new DataTable();


        HopePopSelectMode _selectmode = HopePopSelectMode.SingleClick;
        public HopePopSelectMode SelectMode
        {
            get
            {
                return _selectmode;
            }
            set
            {
                _selectmode = value;
            }
        }
        #endregion

        public HopePopSelect()
        {
            InitializeComponent();

            LoadDataOnDesignMode = true;
            popupContainerEdit1.EnterMoveNextControl = true;
            Height = this.popupContainerEdit1.Height + 4;
        }

        #region 属性

        public bool LoadDataOnDesignMode { get; set; }

        public bool Readonly
        {
            get { return popupContainerEdit1.Properties.ReadOnly; }
            set { popupContainerEdit1.Properties.ReadOnly = value; }
        }

        [Browsable(true), Category("HopeSelect"), Bindable(true), Description("检索方式：输入码，一般是由由数字组成")]
        public String colInCode
        {
            get { return _colInCode; }
            set { _colInCode = value; }
        }

        [Browsable(true), Category("HopeSelect"), Bindable(true), Description("检索方式：拼音码")]
        public String colPY
        {
            get { return _colPY; }
            set { _colPY = value; }
        }

        [Browsable(true), Category("HopeSelect"), Bindable(true), Description("检索方式：五笔")]
        public String colWB
        {
            get { return _colWB; }
            set { _colWB = value; }
        }

        [Browsable(true), Category("HopeSelect"), Bindable(true), Description("最终显示在输入框中的内容对应的字段")]
        public String colDisplay
        {
            get { return _colDisplay; }
            set { _colDisplay = value; }
        }

        [Browsable(true), Category("HopeSelect"), Bindable(true), Description("最终保存的值对应的字段")]
        public String colValue
        {
            get { return _colValue; }
            set { _colValue = value; }
        }

        /// <summary>
        /// 扩展检索字段，按需设置,(做为HIS 编码用) 
        /// </summary>
        [Browsable(true), Category("HopeSelect"), Bindable(true), Description("扩展检索字段，按需设置")]
        public String colExtend1
        {
            get { return _colExtend1; }
            set { _colExtend1 = value; }
        }
        [Browsable(true), Category("HopeSelect"), Description("边框")]
        public DevExpress.XtraEditors.Controls.BorderStyles PBorderStyle
        {
            get { return popupContainerEdit1.BorderStyle; }
            set { popupContainerEdit1.BorderStyle = value; }
        }
        [Browsable(true), Category("HopeSelect"), Description("PFont")]
        public Font PFont
        {
            get { return popupContainerEdit1.Font; }
            set { popupContainerEdit1.Font = value; }
        }

        [Browsable(true), Category("HopeSelect"), Bindable(true), Description("排序字段，按需设置")]
        public String colSeq
        {
            get { return _colSeq; }
            set
            {
                if (value != "")
                {
                    _colSeq = value;
                }
            }
        }

        public void setDataSource(DataTable dt)
        {
            this.getGC().DataSource = dtSource = dt;
            this.getGV().OptionsBehavior.AllowIncrementalSearch = true;
        }

        [Browsable(true), Category("HopeSelect"), Bindable(true), Description("检索方式：输入码，一般是由由数字组成")]
        public String displayMember
        {
            get
            {
                if (!string.IsNullOrEmpty(popupContainerEdit1.Text) && SelectOnly == false) //可输时
                    return popupContainerEdit1.Text;
                else
                    return _displayMember;
            }
            set
            {
                string old = _displayMember;
                _displayMember = value;
                if (popupContainerEdit1.Text != value)
                    this.popupContainerEdit1.Text = value;

                if (old != value)
                    OnDisplayTextChanged(new ValueChangeEventArgs(value));
            }
        }

        [Browsable(true), Category("HopeSelect"), Bindable(true), Description("检索方式：拼音码")]
        public String valueMember
        {
            get { return _valueMember; }
            set
            {
                string v = _valueMember;
                _valueMember = value;
                if (value == null)
                {
                    value = string.Empty;
                }
                if (BindByValue)// && string.IsNullOrEmpty(displayMember)
                {
                    BindingByID(value,v);
                }

                if (v != null && v != value)
                {
                    OnValueChanged(new ValueChangeEventArgs(value));
                }
            }
        }

        private void BindingByID(string code,string v)
        {
            if (string.IsNullOrEmpty(code))
            {
                ClearSelect();
                ClosePop();
                selectRow = null;
                return;
            }

            //选择代码所在的行
            GridView gridView = getGV();
            if (gridView == null)
                return;

            DataRow dr = SelectRowByID(code);//根据ID来查找行
            if (dr == null)
            {
                this._valueMember = null;
                this.displayMember = null;
                this.popupContainerEdit1.Text = string.Empty;
                return;
            }
            this.popupContainerEdit1.Text = dr[_colDisplay].ToString();
            _displayMember = dr[_colDisplay].ToString();
            _valueMember = dr[_colValue].ToString();
            if (selectRow == null || this.selectRow[_colValue] != dr[_colValue])
            {
                selectRow = dr;//2009 11 17
                if (this.onAfterChange != null) this.onAfterChange(selectRow);
            }

            this.selectRow = dr;//再次强制设定值保证selectRow有数据
            this.popupContainerEdit1.ClosePopup();
            if (v != code)
            {
                OnValueChanged(new ValueChangeEventArgs(code));
            }
        }

        [Browsable(true), Category("HopeSelect"), Bindable(true), Description("是否只通过ID绑定")]
        private bool bindByValue = false;
        public bool BindByValue { get { return bindByValue; } set { bindByValue = value; } }

        private DataRow SelectRowByID(string value)
        {
            string column = _colValue;

            if (string.IsNullOrEmpty(column))
                return null;

            DataRow[] drResult = dtSource.Select(string.Format("{0} = '{1}'", column, value));
            if (drResult == null || drResult.Length <= 0)
                return null;

            return drResult[0];
        }

        /// <summary>
        /// 根据文本来查询
        /// </summary>
        /// <param name="text"></param>
        public void SelectByDispaly(string text)
        {
            SelectByValue(text, false);
        }

        /// <summary>
        /// 根据HISCode来查询
        /// </summary>
        /// <param name="code"></param>
        public void SelectByValue(string code)
        {
            SelectByValue(code, true);
        }

        /// <summary>
        /// 根据ID来查询
        /// </summary>
        /// <param name="code"></param>
        public void SelectByID(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                ClearSelect();
                ClosePop();
                selectRow = null;
                return;
            }

            //选择代码所在的行
            GridView gridView = getGV();
            if (gridView == null)
            {
                return;
            }

            DataRow dr = SelectRowBy(code, _colValue);//根据Value来查找行
            if (dr == null)
            {
                displayMember = null;
                valueMember = null;
                return;
            }
            this.popupContainerEdit1.Text = dr[_colDisplay].ToString();
            displayMember = dr[_colDisplay].ToString();
            valueMember = dr[_colValue].ToString();
            if (selectRow == null || this.selectRow[_colValue] != dr[_colValue])
            {
                selectRow = dr;//2009 11 17
                if (this.onAfterChange != null) this.onAfterChange(selectRow);
            }

            this.selectRow = dr;//再次强制设定值保证selectRow有数据
            this.popupContainerEdit1.ClosePopup();
            OnValueChanged(new ValueChangeEventArgs(code));
        }

        public void SelectByValue(string code, bool byCode)
        {
            if (string.IsNullOrEmpty(code))
            {
                ClearSelect();
                ClosePop();
                selectRow = null;
                return;
            }

            //选择代码所在的行
            GridView gridView = getGV();
            if (gridView == null)
                return;

            DataRow dr = SelectRowBy(code, byCode);//根据Value来查找行
            if (dr == null)
            {
                _displayMember = null;
                _valueMember = null;
                return;
            }
            this.popupContainerEdit1.Text = dr[_colDisplay].ToString();
            displayMember = dr[_colDisplay].ToString();
            valueMember = dr[_colValue].ToString();
            if (selectRow == null || this.selectRow[_colValue] != dr[_colValue])
            {
                selectRow = dr;//2009 11 17
                if (this.onAfterChange != null) this.onAfterChange(selectRow);
            }

            this.selectRow = dr;//再次强制设定值保证selectRow有数据
            this.popupContainerEdit1.ClosePopup();
            OnValueChanged(new ValueChangeEventArgs(code));
        }

        /// <summary>
        /// 选中下一行
        /// </summary>
        public void MoveNext()
        {
            GridView gridView = getGV();
            if (gridView != null)
            {
                gridView.MoveNext();
                DataRow drFocused = gridView.GetFocusedDataRow();
                if (drFocused != null)
                {
                    displayMember = drFocused[_colDisplay].ToString();
                    valueMember = drFocused[_colValue].ToString();
                    this.selectRow = drFocused;
                }
            }
        }

        /// <summary>
        /// 选中上一行
        /// </summary>
        public void MovePrev()
        {
            GridView gridView = getGV();
            if (gridView != null)
            {
                gridView.MovePrev();
                DataRow drFocused = gridView.GetFocusedDataRow();
                if (drFocused != null)
                {
                    displayMember = drFocused[_colDisplay].ToString();
                    valueMember = drFocused[_colValue].ToString();
                    this.selectRow = drFocused;
                }
            }
        }

        /// <summary>
        /// 根据Value来查找行
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private DataRow SelectRowBy(string value, bool byCode)
        {
            string column = byCode ? _colExtend1 : _colDisplay;

            return SelectRowBy(value, column);
        }

        private DataRow SelectRowBy(string value, string column)
        {
            if (string.IsNullOrEmpty(column))
                return null;

            DataRow[] drResult = dtSource.Select(string.Format("{0} = '{1}'", column, value));
            if (drResult == null || drResult.Length <= 0)
                return null;

            return drResult[0];
        }

        /// <summary>
        /// 数据过滤
        /// </summary>
        [Browsable(true), Category("HopeSelect"), Description("数据过滤")]
        public string SelectFilter { get; set; }

        [Browsable(true), Category("HopeSelect")]
        public bool EnterMoveNext
        {
            get { return this.popupContainerEdit1.EnterMoveNextControl; }
            set
            {
                popupContainerEdit1.EnterMoveNextControl = value;
            }
        }

        [Browsable(true), Category("HopeSelect"), Description("为true返回字典表的source_id字段值,false返回字典表的主键")]
        public bool SaveSourceID
        {
            get { return _saveSourceID; }
            set
            {
                _saveSourceID = value;
            }
        }

        [Browsable(true), Category("HopeSelect"), Description("为true只能从列表选择,false可以不来自列表")]
        public bool SelectOnly
        {
            get { return _selectOnly; }
            set
            {
                _selectOnly = value;
            }
        }

        /// <summary>
        /// 上下键跳转
        /// </summary>
        [Browsable(true), Category("HopeSelect"), Description("为true上下键跳转,false不跳转")]
        public bool KeyUpDownMoveNext { get { return keyUpDownMoveNext; } set { keyUpDownMoveNext = value; } }

        /// <summary>
        /// 是否使用HIS 编号
        /// </summary>
        [Browsable(true), Category("HopeSelect"), Description("为true使用HIS编号,false不使用HIS编号")]
        public bool UseExtend { get { return useExtend; } set { useExtend = value; } }


        #endregion

        #region 公有方法
        public void showPopup()
        {
            this.popupContainerEdit1.ShowPopup();
        }

        public void LoadDataSource(DataTable dt)
        {
            if (!(dt.Rows.Count > 0 && dt.Rows[0][this.DataBindings["colDisplay"].BindingMemberInfo.BindingField].ToString() == "" && dt.Rows[0][this.DataBindings["colValue"].BindingMemberInfo.BindingField].ToString() == ""))
            {
                DataRow dr = dt.NewRow();
                dt.Rows.InsertAt(dr, 0);
            }

            this.dtSource = dt;
            if (this.DataBindings["colInCode"] != null)
                _colInCode = this.DataBindings["colInCode"].BindingMemberInfo.BindingField;
            _colDisplay = this.DataBindings["colDisplay"].BindingMemberInfo.BindingField;
            if (this.DataBindings["colPY"] != null)
                _colPY = this.DataBindings["colPY"].BindingMemberInfo.BindingField;
            if (this.DataBindings["colWB"] != null)
                _colWB = this.DataBindings["colWB"].BindingMemberInfo.BindingField;
            _colValue = this.DataBindings["colValue"].BindingMemberInfo.BindingField;
            if (this.DataBindings["colExtend1"] != null && UseExtend)
                _colExtend1 = this.DataBindings["colExtend1"].BindingMemberInfo.BindingField;

        }



        #endregion

        public new bool DesignMode
        {
            get { return System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToLower() == "devenv"; }
        }

        #region 事件
        //控件加载时
        private void HopePopSelect_Load(object sender, EventArgs e)
        {
            if (!DesignMode && LoadDataOnDesignMode)
            {
                DataTable dt = getDataSource();

                if (dt != null && !string.IsNullOrEmpty(SelectFilter))
                {
                    DataTable result = dt.Clone();
                    DataRow[] rows = dt.Select(SelectFilter);
                    foreach (DataRow item in rows)
                    {
                        result.Rows.Add(item.ItemArray);
                    }
                    dt = result;

                }

                if (!(dt.Rows.Count > 0 && dt.Rows[0][this.DataBindings["colDisplay"].BindingMemberInfo.BindingField].ToString() == "" && dt.Rows[0][this.DataBindings["colValue"].BindingMemberInfo.BindingField].ToString() == ""))
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.InsertAt(dr, 0);
                }

                LoadDataSource(dt);

                GridView gridView = getGV();
                if (gridView != null)
                {
                    gridView.DoubleClick += this.gridControl1_DoubleClick;
                    gridView.MouseUp += new MouseEventHandler(HopePopSelect_MouseDown);
                }
                gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
                for (int i = 0; i < gridView.Columns.Count; i++)
                {
                    if (gridView.Columns[i].Caption.IndexOf("拼音") != -1 || gridView.Columns[i].Caption.IndexOf("五笔") != -1 || gridView.Columns[i].Caption.IndexOf("输入码") != -1)
                        gridView.Columns[i].Visible = false;
                }
            }

            if (!DesignMode)
            {
                try
                {
                    string config_selectmode = dcl.client.frame.UserInfo.GetSysConfigValue("HopePopSelectMode");

                    if (config_selectmode == "单击")
                    {
                        this._selectmode = HopePopSelectMode.SingleClick;
                    }
                    else if (config_selectmode == "双击")
                    {
                        this._selectmode = HopePopSelectMode.DoubleClick;
                    }
                    else
                    {
                        this._selectmode = HopePopSelectMode.SingleClick;
                    }
                }
                catch (Exception)
                {

                }
            }
        }
        //进入时
        private void popupContainerEdit1_Enter(object sender, EventArgs e)
        {
            this.popupContainerEdit1.SelectAll();
        }
        //编辑值更改
        private void popupContainerEdit1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (canChange)
                {
                    popupContainerEdit1.ShowPopup();
                    popupContainerEdit1.Focus();
                    DataTable dt = this.dtSource.Clone();
                    if (popupContainerEdit1.Text.Trim() == "" && this.onBeforeFilter == null)
                    {
                        dt = this.dtSource.Copy();
                    }
                    else
                    {
                        dt.ImportRow(this.dtSource.Copy().Rows[0]);
                        DataRow[] drs = this.setFilteredData(popupContainerEdit1.Text.Trim());
                        foreach (DataRow dr in drs)
                            dt.ImportRow(dr);
                    }

                    dt.DefaultView.Sort = (_colSeq == "") ? _colValue : _colSeq;
                    this.getGC().DataSource = dt.DefaultView;
                    this.getGV().CollapseAllGroups();
                    if (dt.DefaultView.Count > 1)
                    {
                        if (popupContainerEdit1.Text == "" && SelectOnly) //文本为空时,清空
                        {
                            ClearSelect();
                        }
                        else
                            this.getGV().FocusedRowHandle = 1;
                    }
                }
            }
            finally
            {
                this.canChange = false;
            }
        }
        //按键按下
        private void popupContainerEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)(Keys.Enter))
            {
                this.canChange = true;
                return;
            }

            if (popupContainerEdit1.Properties.PopupControl.Visible)
            {
                DataRow dr = this.getGV().GetFocusedDataRow();
                if (dr != null)
                {
                    this.popupContainerEdit1.Text = this.popupContainerEdit1.Text.Trim();
                    if (!HasSearched && !this.SelectOnly) //如果没有找到匹配项并可手工输
                    {
                        if (!string.IsNullOrEmpty(dr[_colDisplay].ToString().Trim(null)))
                        {
                            this.popupContainerEdit1.Text = dr[_colDisplay].ToString().Trim(null);
                            _displayMember = dr[_colDisplay].ToString().Trim(null);
                        }
                    }
                    else
                    {
                        this.popupContainerEdit1.Text = dr[_colDisplay].ToString();
                        _displayMember = dr[_colDisplay].ToString();
                    }

                    string valueCopy = _valueMember;
                    _valueMember = dr[_colValue].ToString();

                    if (selectRow == null || this.selectRow[_colValue] != dr[_colValue])
                    {
                        this.selectRow = dr;//2009 11 17
                        if (this.onAfterChange != null) this.onAfterChange(this.selectRow);
                    }

                    //强制设定值
                    this.selectRow = dr;

                    if (valueCopy != _valueMember)
                    {
                        this.OnValueChanged(new ValueChangeEventArgs(_valueMember));
                    }
                }
                else
                {
                    _valueMember = DBNull.Value.ToString();
                    _displayMember = this.popupContainerEdit1.Text;
                }

                this.popupContainerEdit1.ClosePopup();
                this.popupContainerEdit1.SelectionStart = this.popupContainerEdit1.Text.Length;
            }

            this.OnKeyPress(e);
        }

        //按键按下前
        private void popupContainerEdit1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == (Keys.Down))
            {
                if (popupContainerEdit1.Properties.PopupControl.Visible)
                    this.getGV().MoveNext();
                else if (KeyUpDownMoveNext)
                    KeysHelper.Jump(e.KeyCode);    //跳转到下个控件                  
                else
                    this.popupContainerEdit1.ShowPopup();

            }
            else if (e.KeyCode == (Keys.Up))
            {
                if (popupContainerEdit1.Properties.PopupControl.Visible)
                    this.getGV().MovePrev();
                else if (KeyUpDownMoveNext)
                    KeysHelper.Jump(e.KeyCode);   //跳转到上个控件
            }
            else if (e.KeyCode == Keys.Escape || (e.KeyCode == Keys.Back && string.IsNullOrEmpty(popupContainerEdit1.Text)))//按Esc键和Backspace键并且
            {
                this.displayMember = "";
                this.valueMember = "";
                if (selectRow != null)
                    selectRow = null;
                this.popupContainerEdit1.ClosePopup();
            }

            this.OnPreviewKeyDown(e);
        }

        //GridControl双击时
        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            if (this._selectmode == HopePopSelectMode.DoubleClick)
            {
                DataRow dr = this.getGV().GetFocusedDataRow();
                if (dr != null)
                {
                    this.popupContainerEdit1.Text = dr[_colDisplay].ToString();
                    _displayMember = dr[_colDisplay].ToString();

                    string valueMenberCopy = _valueMember;
                    _valueMember = dr[_colValue].ToString();

                    if (selectRow == null || this.selectRow[_colValue] != dr[_colValue])
                    {
                        this.selectRow = dr;//2009 11 17
                        if (this.onAfterChange != null) this.onAfterChange(this.selectRow);
                    }

                    //再次强制设定值保证selectRow有数据
                    this.selectRow = dr;

                    if (valueMenberCopy != _valueMember)
                    {
                        this.OnValueChanged(new ValueChangeEventArgs(_valueMember));
                    }

                    this.popupContainerEdit1.ClosePopup();
                }
            }
        }

        void HopePopSelect_MouseDown(object sender, MouseEventArgs e)
        {
            if (this._selectmode == HopePopSelectMode.SingleClick && e.Button == MouseButtons.Left)
            {
                GridView gv = this.getGV();

                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hInfo = gv.CalcHitInfo(new Point(e.X, e.Y));

                if (hInfo.InRow || hInfo.InRowCell)
                {
                    DataRow dr = this.getGV().GetFocusedDataRow();
                    if (dr != null)
                    {
                        this.popupContainerEdit1.Text = dr[_colDisplay].ToString();
                        _displayMember = dr[_colDisplay].ToString();

                        string valueMenberCopy = _valueMember;
                        _valueMember = dr[_colValue].ToString();

                        if (selectRow == null || this.selectRow[_colValue] != dr[_colValue])
                        {
                            this.selectRow = dr;//2009 11 17
                            if (this.onAfterChange != null) this.onAfterChange(this.selectRow);
                        }

                        //再次强制设定值保证selectRow有数据
                        this.selectRow = dr;

                        if (valueMenberCopy != _valueMember)
                        {
                            this.OnValueChanged(new ValueChangeEventArgs(_valueMember));
                        }

                        this.popupContainerEdit1.ClosePopup();
                    }
                }
            }
        }

        ////GridControl单击时
        //private void gridControl1_Click(object sender, EventArgs e)
        //{
        //    GridView gv = this.getGV();
        //    DataRow dr = this.getGV().GetFocusedDataRow();
        //    if (dr != null)
        //    {
        //        this.popupContainerEdit1.Text = dr[_colDisplay].ToString();
        //        _displayMember = dr[_colDisplay].ToString();

        //        string valueMenberCopy = _valueMember;
        //        _valueMember = dr[_colValue].ToString();

        //        if (selectRow == null || this.selectRow[_colValue] != dr[_colValue])
        //        {
        //            this.selectRow = dr;//2009 11 17
        //            if (this.onAfterChange != null) this.onAfterChange(this.selectRow);
        //        }

        //        //再次强制设定值保证selectRow有数据
        //        this.selectRow = dr;

        //        if (valueMenberCopy != _valueMember)
        //        {
        //            this.OnValueChanged(new ValueChangeEventArgs(_valueMember));
        //        }

        //        this.popupContainerEdit1.ClosePopup();
        //    }
        //}


        //弹出列表
        private void popupContainerEdit1_Popup(object sender, EventArgs e)
        {
            DataTable dtTempSource = this.dtSource.Clone();

            if (this.dtSource.Rows.Count == 0)
                return;

            if (this.onBeforeFilter != null)
            {
                dtTempSource.ImportRow(this.dtSource.Copy().Rows[0]);
                DataRow[] drs = this.setFilteredData("");
                foreach (DataRow dr in drs)
                    dtTempSource.ImportRow(dr);

                dtTempSource.DefaultView.Sort = (_colSeq == "") ? _colValue : _colSeq;
            }
            else
            {
                dtTempSource = dtSource.Copy();
            }

            GridControl gridControl = getGC();
            GridView gridView = this.getGV();
            gridView.CollapseAllGroups();
            gridControl.DataSource = dtTempSource.DefaultView;
            popupContainerEdit1.Focus();
            dtTempSource.BeginLoadData();


            try
            {
                if (_valueMember != null && _valueMember.Trim() != "")
                {
                    //寻找并指定焦点行
                    int i = 0;
                    foreach (DataRowView dr in dtTempSource.DefaultView)
                    {
                        if (dr[_colValue].ToString() == _valueMember)
                        {
                            gridView.FocusedRowHandle = gridView.GetRowHandle(i);
                            break;
                        }
                        i++;
                    }
                }
            }
            finally
            {
                dtTempSource.EndLoadData();
            }
        }
        //列表中双击
        private void popupContainerEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            { e.Handled = true; }
            this.OnKeyDown(e);
        }
        //列表中按钮单击
        private void popupContainerEdit1_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Delete)
            {
                ClearSelect();
                ClosePop();
            }
        }

        //清空
        public void ClearSelect()
        {
            popupContainerEdit1.Text = DBNull.Value.ToString();
            _valueMember = DBNull.Value.ToString();
            _displayMember = DBNull.Value.ToString();
        }

        private void ClosePop()
        {
            if (popupContainerEdit1.Properties.PopupControl.Visible)
                popupContainerEdit1.ClosePopup();
        }

        //控件大小变动
        private void HopePopSelect_Resize(object sender, EventArgs e)
        {
            this.popupContainerEdit1.Width = this.Width - 5;
        }
        //下拉列表关闭
        private void popupContainerEdit1_Closed(object sender, ClosedEventArgs e)
        {
            if (this.onAfterSelected != null)
                this.onAfterSelected(this, new EventArgs());
        }
        //焦点离开
        protected override void OnLeave(EventArgs e)
        {
            //if (this.ParentForm == null || this.ParentForm.Disposing || this.ParentForm.IsDisposed)
            //    return;

            //if (SelectOnly == false)//可输时不清空
            //{ }
            //else if (string.IsNullOrEmpty(valueMember) || string.IsNullOrEmpty(popupContainerEdit1.Text))
            //{//如果值为空，则清空
            //    this.valueMember = null;
            //    this.displayMember = string.Empty;
            //}
            //else  //如果值不为空并非可输时           
            //    this.popupContainerEdit1.Text = this.displayMember;


            base.OnLeave(e);
        }
        //文本更改时
        private void popupContainerEdit1_TextChanged(object sender, EventArgs e)
        {
            if (this.DisplayTextChanged != null)
                OnDisplayTextChanged(new ValueChangeEventArgs(popupContainerEdit1.EditValue));
        }

        #endregion
    }

    public class ValueChangeEventArgs : EventArgs
    {
        public ValueChangeEventArgs(object value)
        {
            this.Value = value;
        }
        public object Value { get; private set; }
    }

    public enum HopePopSelectMode
    {
        SingleClick,
        DoubleClick,
    }
}
