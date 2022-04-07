using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Data.Filtering;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Drawing;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.Utils.Frames;
using DevExpress.Utils.Paint;
using DevExpress.XtraEditors.Repository;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors.Controls;
using DevExpress.Data;
using DevExpress.Data.Filtering.Helpers;
namespace lis.client.control
{
    using lis.client.control;
    public enum FilterChangedAction { ValueChanged, FieldNameChange, OperationChanged, GroupTypeChanged, RemoveNode, AddConditionNode, AddGroupNode, ClearAll };
    public delegate void FilterChangedEventHandler(object sender, FilterChangedEventArgs e);
    public class FilterChangedEventArgs : EventArgs
    {
        private FilterChangedAction action;
        private Node node;
        public FilterChangedEventArgs(FilterChangedAction action, Node node)
        {
            this.action = action;
            this.node = node;
        }
        public FilterChangedAction Action
        {
            get { return action; }
        }
        public Node CurrentNode
        {
            get { return node; }
        }
    }
    [ToolboxItem(true),
     ToolboxBitmap(typeof(DevExpress.XtraEditors.BaseEdit), DevExpress.Utils.ControlConstants.BitmapPath + "FilterControl.bmp"),
     Description("Allows an end-user to construct complex filter criteria, and apply them to controls."),
     Designer(typeof(DevExpress.Utils.Design.BaseDesigner))
    ]
    public class FilterControl : BaseStyleControl
    {
        object sourceControl = null;
        FilterColumn defaultColumn = null;
        GroupNode rootNode;
        FilterControlFocusInfo focusInfo;
        FilterControlLabelInfo hotTrackLabelInfo;
        bool showOperandTypeIcon = false, showEditors = false, showEditorOnFocus = false, showGroupCommandsIcon = false;
        AppearanceObject appearanceTreeLine;
        FilterColumnCollection filterColumns = new FilterColumnCollection();
        IDXMenuManager menuManager = null;
        BaseEdit activeEditor = null;
        Color groupColor, propertyColor, clauseColor, valueColor, emptyValueColor;
        [ThreadStatic]
        static ImageCollection nodeElements;
        [ThreadStatic]
        static ImageCollection clauseElements;
        [ThreadStatic]
        static ImageCollection groupElements;
        bool refreshEditorPosition = false;
        bool suspendEditorCreate = false;
        bool activeItemInvalidate = false;
        bool sortFilterColumns = true;
        internal static Color[] DefaultColorValues = new Color[] { Color.Red, Color.Blue, Color.Green, Color.Black, Color.Gray };
        DevExpress.XtraEditors.VScrollBar vScrollBar;
        DevExpress.XtraEditors.HScrollBar hScrollBar;
        private static readonly object filterChanged = new object();
        public event FilterChangedEventHandler FilterChanged
        {
            add { this.Events.AddHandler(filterChanged, value); }
            remove { this.Events.RemoveHandler(filterChanged, value); }
        }
        protected internal virtual void RaiseFilterChanged(FilterChangedEventArgs e)
        {
            FilterChangedEventHandler handler = (FilterChangedEventHandler)this.Events[filterChanged];
            if (handler != null) handler(this, e);
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            FilterViewInfo.OnHandleCreated();
        }
        protected override Size DefaultSize { get { return new Size(200, 100); } }
        public FilterControl()
        {
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.UserMouse, true);
            this.SetStyle(ControlStyles.ContainerControl, true);
            this.FilterCriteria = null;
            appearanceTreeLine = CreateAppearance();
            InitColors();
            InitScrollBars();
        }
        void InitColors()
        {
            groupColor = DefaultColorValues[0];
            propertyColor = DefaultColorValues[1];
            clauseColor = DefaultColorValues[2];
            valueColor = DefaultColorValues[3];
            emptyValueColor = DefaultColorValues[4];
        }
        internal static ImageCollection NodeImages
        {
            get
            {
                if (nodeElements == null)
                    nodeElements = DevExpress.Utils.Controls.ImageHelper.CreateImageCollectionFromResources("lis.client.control.FilterEditor.Images.NodeImages.png", typeof(FilterControl).Assembly, new Size(13, 13), Color.Magenta);
                return nodeElements;
            }
        }
        internal static ImageCollection ClauseImages
        {
            get
            {
                if (clauseElements == null)
                    clauseElements = DevExpress.Utils.Controls.ImageHelper.CreateImageCollectionFromResources("lis.client.control.FilterEditor.Images.ClauseImages.png", typeof(FilterControl).Assembly, new Size(13, 13), Color.Magenta);
                return clauseElements;
            }
        }
        internal static ImageCollection GroupImages
        {
            get
            {
                if (groupElements == null)
                    groupElements = DevExpress.Utils.Controls.ImageHelper.CreateImageCollectionFromResources("lis.client.control.FilterEditor.Images.GroupImages.png", typeof(FilterControl).Assembly, new Size(13, 13), Color.Magenta);
                return groupElements;
            }
        }
        protected FilterColumn GetDefaultColumn()
        {
            return defaultColumn;
        }
        public void SetDefaultColumn(FilterColumn column)
        {
            this.defaultColumn = column;
        }
        #region Editors
        [Browsable(false)]
        public BaseEdit ActiveEditor
        {
            get { return activeEditor; }
        }
        protected internal void DisposeActiveEditor()
        {
            if (activeEditor != null)
            {
                activeEditor.Leave -= new EventHandler(OnEditorLeave);
                activeEditor.Validated -= new EventHandler(OnEditorValidated);
                activeEditor.Dispose();
                activeEditor = null;
                invalidateScrollPosition = true;
                Refresh(true, false);
            }
        }
        protected internal virtual RepositoryItem GetRepositoryItem(ClauseNode node)
        {
            FilterColumn column = FilterColumns[node.FirstOperand];
            if (column == null) return new RepositoryItemTextEdit();
            return column.ColumnEditor;
        }
        protected virtual void InitRepositoryProperties(RepositoryItem ri)
        {
            ri.AllowFocused = true;
            ri.ReadOnly = false;
            RepositoryItemButtonEdit ribe = ri as RepositoryItemButtonEdit;
            if (ribe != null)
                ribe.TextEditStyle = TextEditStyles.Standard;
            RepositoryItemTextEdit rite = ri as RepositoryItemTextEdit;
            if (rite != null)
            {
                rite.AllowNullInput = DefaultBoolean.False;
                rite.ValidateOnEnterKey = false;
            }
        }
        protected internal void CreateActiveEditor()
        {
            if (activeEditor != null) return;
            if (FocusedItemType != ElementType.Value) return;
            ClauseNode node = (ClauseNode)FocusInfo.Node;
            RepositoryItem ri = GetRepositoryItem(node);
            activeEditor = ri.CreateEditor();
            activeEditor.Properties.Assign(ri);
            activeEditor.Properties.Appearance.Assign(this.FilterViewInfo.PaintAppearance);
            activeEditor.Properties.Appearance.ForeColor = AppearanceValueColor;
            InitRepositoryProperties(activeEditor.Properties);
            activeEditor.CausesValidation = false;
            FocusInfo.Node.ViewInfo.ClearViewInfo();
            activeEditor.Bounds = LabelInfoHelper.GetEditorBoundsByElement(FocusedItem);
            OperandValue ov = (OperandValue)node.AdditionalOperands[FocusInfo.ElementIndex - 2];
            activeEditor.EditValue = ov.Value;
            activeEditor.Parent = this;
            activeEditor.Focus();
            activeEditor.Validated += new EventHandler(OnEditorValidated);
            activeEditor.Leave += new EventHandler(OnEditorLeave);
            prevEditorFocusInfo = FocusInfo;
            activeEditor.Show();
            createEditor = true;
            invalidateScrollPosition = true;
        }
        void DoEditorValidate()
        {
            if (ActiveEditor != null)
            {
                ActiveEditor.DoValidate();
                DisposeActiveEditor();
            }
        }
        void OnEditorLeave(object sender, EventArgs e)
        {
            DoEditorValidate();
        }
        void OnEditorValidated(object sender, EventArgs e)
        {
            ClauseNode node = (ClauseNode)FocusInfo.Node;
            FilterColumn column = this.FilterColumns[node.FirstOperand];
            object resultValue = ActiveEditor.EditValue;
            if (column != null)
            {
                resultValue = DevExpress.Data.Helpers.FilterHelper.CorrectFilterValueType(column.ColumnType, resultValue);
            }
            node.AdditionalOperands[FocusInfo.ElementIndex - 2] = new OperandValue(resultValue);
            this.RefreshTreeAfterNodeChange();
            RaiseFilterChanged(new FilterChangedEventArgs(FilterChangedAction.ValueChanged, node));
            DisposeActiveEditor();
        }
        internal void RefreshEditorPosition()
        {
            if (!refreshEditorPosition) return;
            if (ActiveEditor != null && FocusedItem != null)
                if (ActiveEditor.Bounds != LabelInfoHelper.GetEditorBoundsByElement(FocusedItem))
                {
                    ActiveEditor.Bounds = LabelInfoHelper.GetEditorBoundsByElement(FocusedItem);
                }
            refreshEditorPosition = false;
        }
        public void SetFilterColumnsCollection(FilterColumnCollection columns)
        {
            SetFilterColumnsCollection(columns, null);
        }
        public void SetFilterColumnsCollection(FilterColumnCollection columns, IDXMenuManager manager)
        {
            if (columns == null)
                columns = new FilterColumnCollection();
            if (filterColumns != null)
                filterColumns.Dispose();
            filterColumns = columns;
            if (SortFilterColumns) filterColumns.Sort();
            if (manager != null)
                SetMenuManager(manager);
            FilterViewInfo.ClearColumnMenu();
        }
        [DefaultValue(null), Category(CategoryName.BarManager)]
        public IDXMenuManager MenuManager
        {
            get { return menuManager; }
            set
            {
                if (MenuManager == value) return;
                menuManager = value;
            }
        }
        void SetMenuManager(IDXMenuManager manager)
        {
            menuManager = manager != null ? manager.Clone(FindForm()) : null;
        }
        #endregion
        #region IFilterControl
        [DefaultValue(null), TypeConverter(typeof(FilterControlConverter))]
        public object SourceControl
        {
            get { return sourceControl; }
            set
            {
                if (SourceControl == value) return;
                sourceControl = value;
                UpdateFilterSourceControl();
            }
        }
        void UpdateFilterSourceControl()
        {
            if (SourceControl is DataTable)
                FilterSourceControl = new BindingListFilterProxy(((DataTable)SourceControl).DefaultView);
            else if (SourceControl is IBindingListView || SourceControl is IFilteredXtraBindingList)
                FilterSourceControl = new BindingListFilterProxy((IBindingList)SourceControl);
            else
                FilterSourceControl = SourceControl as IFilteredComponent;
        }
        IFilteredComponent filterSourceControl;
        IFilteredComponent FilterSourceControl
        {
            get
            {
                return filterSourceControl;
            }
            set
            {
                if (FilterSourceControl == value) return;
                if (FilterSourceControl != null)
                {
                    FilterSourceControl.PropertiesChanged -= new EventHandler(SourceControl_DataSourceChanged);
                    FilterSourceControl.RowFilterChanged -= new EventHandler(SourceControl_FilterChanged);
                }
                filterSourceControl = value;
                if (FilterSourceControl != null)
                {
                    FilterSourceControl.PropertiesChanged += new EventHandler(SourceControl_DataSourceChanged);
                    FilterSourceControl.RowFilterChanged += new EventHandler(SourceControl_FilterChanged);
                }
                CreateFilterColumnCollection();
            }
        }
        void CreateFilterColumnCollection()
        {
            if (FilterSourceControl == null) return;
            this.SetFilterColumnsCollection(FilterSourceControl.CreateFilterColumnCollection());
        }
        void SourceControl_DataSourceChanged(object sender, EventArgs e)
        {
            CreateFilterColumnCollection();
        }
        void SourceControl_FilterChanged(object sender, EventArgs e)
        {
            if (FilterSourceControl == null) return;
            this.FilterCriteria = FilterSourceControl.RowCriteria;
        }
        public void ApplyFilter()
        {
            if (FilterSourceControl == null) return;
            FilterSourceControl.RowCriteria = this.FilterCriteria;
        }
        #endregion
        #region Properties
        [Browsable(false)]
        public FilterColumnCollection FilterColumns { get { return filterColumns; } }
        [Browsable(false)]
        protected internal GroupNode RootNode { get { return rootNode; } }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FilterString
        {
            get { return CriteriaOperator.ToString(FilterCriteria); }
            set { this.FilterCriteria = CriteriaOperator.Parse(value); }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CriteriaOperator FilterCriteria
        {
            get { return rootNode.ToCriteria(); }
            set
            {
                CreateTree(value);
            }
        }
        [DefaultValue(20)]
        public int LevelIndent
        {
            get { return FilterViewInfo.LevelIndent; }
            set
            {
                if (value < 0) value = 0;
                if (value > 100) value = 100;
                if (FilterViewInfo.LevelIndent == value) return;
                FilterViewInfo.LevelIndent = value;
                Refresh(true, false);
            }
        }
        [DefaultValue(0)]
        public int NodeSeparatorHeight
        {
            get { return FilterViewInfo.NodeSeparatorHeight; }
            set
            {
                if (value < 0) value = 0;
                if (value > 10) value = 10;
                if (FilterViewInfo.NodeSeparatorHeight == value) return;
                FilterViewInfo.NodeSeparatorHeight = value;
                invalidateScrollPosition = true;
                Refresh(true, false);
            }
        }
        [DefaultValue(true)]
        public bool SortFilterColumns
        {
            get { return sortFilterColumns; }
            set { sortFilterColumns = value; }
        }
        [DefaultValue(false)]
        public bool ShowOperandTypeIcon
        {
            get { return showOperandTypeIcon; }
            set
            {
                showOperandTypeIcon = value;
                RefreshTreeAfterNodeChange();
            }
        }
        [DefaultValue(false)]
        public bool ShowGroupCommandsIcon
        {
            get { return showGroupCommandsIcon; }
            set
            {
                showGroupCommandsIcon = value;
                FilterViewInfo.ClearGroupMenu();
                RefreshTreeAfterNodeChange();
            }
        }
        [DefaultValue(false)]
        internal bool ShowEditors
        {
            get { return showEditors; }
            set
            {
                if (showEditors == value) return;
                showEditors = value;
                Refresh(true, false);
            }
        }
        [DefaultValue(false)]
        internal bool ShowEditorOnFocus
        {
            get { return showEditorOnFocus; }
            set
            {
                if (showEditorOnFocus == value) return;
                showEditorOnFocus = value;
            }
        }
        [Category("Appearance"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual AppearanceObject AppearanceTreeLine
        {
            get { return appearanceTreeLine; }
        }
        bool ShouldSerializeAppearanceTreeLine() { return AppearanceTreeLine.ShouldSerialize(); }
        [Browsable(false)]
        public FilterControlViewInfo FilterViewInfo
        {
            get { return (FilterControlViewInfo)this.ViewInfo; }
        }
        [Browsable(false)]
        internal LabelInfoViewInfo HotTrackLabelInfoViewInfo
        {
            get
            {
                if (hotTrackLabelInfo != null)
                    return hotTrackLabelInfo.ViewInfo;
                return null;
            }
        }
        [Browsable(false)]
        internal FilterLabelInfoTextViewInfo HotTrackItem
        {
            get
            {
                if (HotTrackLabelInfoViewInfo != null)
                    return HotTrackLabelInfoViewInfo.ActiveItem as FilterLabelInfoTextViewInfo;
                return null;
            }
        }
        [Browsable(false)]
        internal FilterLabelInfoTextViewInfo FocusedItem
        {
            get
            {
                if (FocusInfo.Node == null) return null;
                for (int i = 0; i < FocusInfo.Node.LabelInfo.ViewInfo.Count; i++)
                {
                    FilterLabelInfoTextViewInfo element = FocusInfo.Node.LabelInfo.ViewInfo[i] as FilterLabelInfoTextViewInfo;
                    NodeElement nodeElement = element.InfoText.Tag as NodeElement;
                    if (nodeElement != null && nodeElement.GetFocusInfo() == FocusInfo)
                    {
                        return element;
                    }
                }
                return null;
            }
        }
        [Browsable(false)]
        internal ElementType FocusedItemType
        {
            get
            {
                if (FocusedItem == null) return ElementType.None;
                NodeElement element = FocusedItem.InfoText.Tag as NodeElement;
                if (element == null) return ElementType.None;
                return element.Type;
            }
        }
        FilterControlFocusInfo prevEditorFocusInfo = new FilterControlFocusInfo();
        protected internal FilterControlFocusInfo FocusInfo
        {
            get { return focusInfo; }
            set
            {
                if (FocusInfo == value)
                    return;
                focusInfo = value;
                OnFocusedElementChanged();
                this.Invalidate();
            }
        }
        protected virtual void OnFocusedElementChanged()
        {
            DisposeActiveEditor();
            if (!suspendEditorCreate && ShowEditorOnFocus && FocusedItemType == ElementType.Value)
            {
                refreshEditorPosition = prevEditorFocusInfo.Node == focusInfo.Node;
                CreateActiveEditor();
            }
            MakeFocusedNodeVisible();
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public override IStyleController StyleController
        {
            get { return null; }
            set { }
        }
        #endregion
        #region Colors
        [Category("Appearance")]
        public Color AppearanceGroupOperatorColor
        {
            get { return groupColor; }
            set
            {
                if (groupColor == value) return;
                groupColor = value;
                RefreshTreeAfterNodeChange();
            }
        }
        bool ShouldSerializeAppearanceGroupOperatorColor() { return AppearanceGroupOperatorColor != DefaultColorValues[0]; }
        void ResetAppearanceGroupOperatorColor() { AppearanceGroupOperatorColor = DefaultColorValues[0]; }
        [Category("Appearance")]
        public Color AppearanceFieldNameColor
        {
            get { return propertyColor; }
            set
            {
                if (propertyColor == value) return;
                propertyColor = value;
                RefreshTreeAfterNodeChange();
            }
        }
        bool ShouldSerializeAppearanceFieldNameColor() { return AppearanceFieldNameColor != DefaultColorValues[1]; }
        void ResetAppearanceFieldNameColor() { AppearanceFieldNameColor = DefaultColorValues[1]; }
        [Category("Appearance")]
        public Color AppearanceOperatorColor
        {
            get { return clauseColor; }
            set
            {
                if (clauseColor == value) return;
                clauseColor = value;
                RefreshTreeAfterNodeChange();
            }
        }
        bool ShouldSerializeAppearanceOperatorColor() { return AppearanceOperatorColor != DefaultColorValues[2]; }
        void ResetAppearanceOperatorColor() { AppearanceOperatorColor = DefaultColorValues[2]; }
        [Category("Appearance")]
        public Color AppearanceValueColor
        {
            get { return valueColor; }
            set
            {
                if (valueColor == value) return;
                valueColor = value;
                RefreshTreeAfterNodeChange();
            }
        }
        bool ShouldSerializeAppearanceValueColor() { return AppearanceValueColor != DefaultColorValues[3]; }
        void ResetAppearanceValueColor() { AppearanceValueColor = DefaultColorValues[3]; }
        [Category("Appearance")]
        public Color AppearanceEmptyValueColor
        {
            get { return emptyValueColor; }
            set
            {
                if (emptyValueColor == value) return;
                emptyValueColor = value;
                RefreshTreeAfterNodeChange();
            }
        }
        bool ShouldSerializeAppearanceEmptyValueColor() { return AppearanceEmptyValueColor != DefaultColorValues[4]; }
        void ResetAppearanceEmptyValueColor() { AppearanceEmptyValueColor = DefaultColorValues[4]; }
        #endregion
        #region Resizeable
        protected override bool SizeableIsCaptionVisible
        {
            get
            {
                return false;
            }
        }
        protected override Size CalcSizeableMaxSize()
        {
            return Size.Empty;
        }
        protected override Size CalcSizeableMinSize()
        {
            return new Size(((GroupNodeViewInfo)this.RootNode.ViewInfo).NodeWidth + FilterControlViewInfo.LeftIndent, this.RootNode.ViewInfo.Height + FilterControlViewInfo.TopIndent);
        }
        #endregion
        internal void SetActiveLabelInfo(int x, int y)
        {
            SetActiveLabelInfo(new Point(x, y));
        }
        internal void SetActiveLabelInfo(Point p)
        {
            FilterControlLabelInfo li = RootNode.GetLabelInfoByCoordinates(p.X, p.Y);
            if (li == hotTrackLabelInfo) return;
            if (hotTrackLabelInfo != null)
                hotTrackLabelInfo.ViewInfo.OnMouseLeave();
            hotTrackLabelInfo = li;
            if (hotTrackLabelInfo == null) this.Invalidate();
        }
        protected override void Dispose(bool disposing)
        {
            fDisposing = true;
            if (disposing)
            {
                if (menuManager != null) menuManager.DisposeManager();
                this.menuManager = null;
                DestroyAppearance(AppearanceTreeLine);
                if (filterColumns != null)
                    filterColumns.Dispose();
            }
            base.Dispose(disposing);
        }
        protected override void OnStyleChanged(object sender, EventArgs e)
        {
            base.OnStyleChanged(sender, e);
            FilterViewInfo.ClearLineBrush();
            FilterViewInfo.ClearViewInfo();
            RefreshTreeAfterNodeChange();
        }
        protected virtual Node CreateDefaultRootNode()
        {
            GroupNode node = new GroupNode();
            node.NodeType = GroupType.And;
            return node;
        }
        void CreateTree(CriteriaOperator criteria)
        {
            invalidateScrollPosition = true;
            Node newRootNode = CriteriaToTreeProcessor.GetTree(criteria, null);
            if (newRootNode == null)
                newRootNode = CreateDefaultRootNode();
            if (!(newRootNode is GroupNode))
            {
                GroupNode wrapper = new GroupNode();
                wrapper.NodeType = GroupType.And;
                wrapper.SubNodes.Add(newRootNode);
                newRootNode = wrapper;
            }
            this.rootNode = (GroupNode)newRootNode;
            this.rootNode.SetOwner(this, null);
            this.FocusInfo = new FilterControlFocusInfo(this.rootNode, 0);
            Refresh(false, false);
        }
        void RefreshTreeAfterNodeChange()
        {
            this.RootNode.RecalcLabelInfo();
            invalidateScrollPosition = true;
            Refresh(true, false);
        }
        public void RefreshNodes()
        {
            RefreshTreeAfterNodeChange();
        }
        protected internal virtual void Refresh(bool clearNodesViewInfo, bool clearViewInfo)
        {
            if (clearViewInfo) FilterViewInfo.ClearViewInfo();
            if (clearNodesViewInfo)
            {
                RootNode.ViewInfo.ClearViewInfo();
                activeItemInvalidate = true;
            }
            RootNode.ViewInfo.UpdateBounds();
            LayoutChanged();
        }
        internal void UpdateHotTrackItem()
        {
            if (!activeItemInvalidate) return;
            activeItemInvalidate = false;
            UpdateHotTrackItem(this.PointToClient(MousePosition));
        }
        void UpdateHotTrackItem(Point p)
        {
            SetActiveLabelInfo(p);
            if (HotTrackLabelInfoViewInfo == null)
            {
                Cursor = Cursors.Arrow;
                return;
            }
            HotTrackLabelInfoViewInfo.OnMouseMove(p.X, p.Y);
            Cursor = HotTrackItem != null ? Cursors.Hand : Cursors.Arrow;
        }
        void AddElementCollection()
        {
            ((ClauseNode)FocusInfo.Node).AdditionalOperands.Add(new OperandValue());
            FocusInfo = new FilterControlFocusInfo(FocusInfo.Node, ((ClauseNode)FocusInfo.Node).AdditionalOperands.Count + 1);
            RefreshTreeAfterNodeChange();
        }
        private void DeleteElementCollection()
        {
            ClauseNode node = FocusInfo.Node as ClauseNode;
            if (node == null) return;
            node.AdditionalOperands.RemoveAt(FocusInfo.ElementIndex - 2);
            FocusInfo = new FilterControlFocusInfo(FocusInfo.Node, ((ClauseNode)FocusInfo.Node).AdditionalOperands.Count + 1);
            RefreshTreeAfterNodeChange();
        }
        void SwapPropertyValueActionMenu()
        {
            ClauseNode node = (ClauseNode)FocusInfo.Node;
            int index = FocusInfo.ElementIndex - 2;
            if (node.AdditionalOperands[index] is OperandProperty)
            {
                node.AdditionalOperands[index] = new OperandValue();
            }
            else {
                node.AdditionalOperands[index] = FilterColumns.CreateDefaultProperty(defaultColumn);
            }
            RefreshTreeAfterNodeChange();
        }
        #region BaseStyleControl Members
        protected override BaseControlPainter CreatePainter()
        {
            return new FilterControlPainter(this);
        }
        protected override BaseStyleControlViewInfo CreateViewInfo()
        {
            return new FilterControlViewInfo(this);
        }
        #endregion
        #region Events
        static readonly object itemClick = new object();
        static readonly object itemDoubleClick = new object();
        public event LabelInfoItemClickEvent ItemClick
        {
            add { Events.AddHandler(itemClick, value); }
            remove { Events.RemoveHandler(itemClick, value); }
        }
        public event LabelInfoItemClickEvent ItemDoubleClick
        {
            add { Events.AddHandler(itemDoubleClick, value); }
            remove { Events.RemoveHandler(itemDoubleClick, value); }
        }
        protected virtual void ShowElementMenu(ElementType type)
        {
            bool allowAction = downActiveItem == HotTrackItem;
            switch (type)
            {
                case ElementType.Property:
                    ShowPropertyMenu();
                    break;
                case ElementType.Group:
                    ShowGroupMenu();
                    break;
                case ElementType.Operation:
                    ShowClauseMenu();
                    break;
                case ElementType.Value:
                    CreateActiveEditor();
                    break;
                case ElementType.NodeRemove:
                    if (allowAction)
                        OnRemoveNode(this, EventArgs.Empty);
                    break;
                case ElementType.NodeAction:
                    ShowNodeActionMenu();
                    break;
                case ElementType.FieldAction:
                    if (allowAction)
                        SwapPropertyValueActionMenu();
                    break;
                case ElementType.CollectionAction:
                    if (allowAction)
                        AddElementCollection();
                    break;
                case ElementType.NodeAdd:
                    if (allowAction)
                        AddClauseNodeAndFocusIt((GroupNode)FocusInfo.Node);
                    break;
            }
        }
        protected virtual void OnItemClick(LabelInfoItemClickEventArgs e)
        {
            LabelInfoItemClickEvent handler = (LabelInfoItemClickEvent)this.Events[itemClick];
            if (handler != null) handler(this, e);
            NodeElement element = e.InfoText.Tag as NodeElement;
            if (element == null) return;
            if (element.Type == ElementType.FieldAction || element.Type == ElementType.CollectionAction)
                suspendEditorCreate = true;
            this.FocusInfo = element.GetFocusInfo();
            suspendEditorCreate = false;
            ShowElementMenu(element.Type);
        }
        protected virtual void OnItemDoubleClick(LabelInfoItemClickEventArgs e)
        {
            LabelInfoItemClickEvent handler = (LabelInfoItemClickEvent)this.Events[itemDoubleClick];
            if (handler != null) handler(this, e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            UpdateHotTrackItem(new Point(e.X, e.Y));
        }
        FilterLabelInfoTextViewInfo downActiveItem = null;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            downActiveItem = HotTrackItem;
            if (HotTrackLabelInfoViewInfo == null) return;
            if (e.Button != MouseButtons.Left) return;
            HotTrackLabelInfoViewInfo.OnMouseDown(e.X, e.Y);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (HotTrackLabelInfoViewInfo == null) return;
            if (e.Button != MouseButtons.Left) return;
            HotTrackLabelInfoViewInfo.OnMouseUp(e.X, e.Y);
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (HotTrackLabelInfoViewInfo == null) return;
            if (HotTrackItem != null)
                OnItemClick(new LabelInfoItemClickEventArgs(HotTrackItem.InfoText));
        }
        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            if (HotTrackLabelInfoViewInfo == null) return;
            if (HotTrackItem != null)
                OnItemDoubleClick(new LabelInfoItemClickEventArgs(HotTrackItem.InfoText));
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            RootNode.ClearActiveItem();
            Cursor = Cursors.Arrow;
            base.OnMouseLeave(e);
        }
        protected override void OnResize(EventArgs e)
        {
            invalidateScrollPosition = true;
            base.OnResize(e);
            this.Invalidate();
        }
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            LayoutChanged();
        }
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            LayoutChanged();
        }
        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                case Keys.Enter:
                case Keys.Space:
                case Keys.Escape:
                    return true;
                default:
                    return base.IsInputKey(keyData);
            }
        }
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (IsInputKey(keyData))
                return false;
            else
                return base.ProcessDialogKey(keyData);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            ProcessKeys(e);
            if (e.Handled)
                return;
            if (ActiveEditor != null)
                return;
            if (e.Modifiers != Keys.None && e.KeyCode != Keys.ShiftKey)
                return;
            if (IsInputKey(e.KeyCode))
                return;
            CreateActiveEditor();
            if (ActiveEditor != null)
                ActiveEditor.SendKey(e);
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (e.Handled)
                return;
            switch (e.KeyChar)
            {
                case ' ':
                case '\r':
                    if (ActiveEditor == null)
                    {
                        ShowElementMenu(FocusedItemType);
                    }
                    e.Handled = true;
                    break;
                case '\x1b':
                    e.Handled = true;
                    break;
                default:
                    if (ActiveEditor != null)
                    {
                        ActiveEditor.SendKey(null);
                    }
                    break;
            }
        }
        protected override bool ProcessKeyPreview(ref Message m)
        {
            if (m.Msg == 0x0100 || m.Msg == 0x0104)
            {
                KeyEventArgs keys = new KeyEventArgs((Keys)((int)Control.ModifierKeys | (m.WParam.ToInt32() & (int)Keys.KeyCode)));
                if (ProcessKeys(keys))
                    return true;
            }
            return base.ProcessKeyPreview(ref m);
        }
        protected virtual bool ProcessKeys(KeyEventArgs e)
        {
            if (e.Handled)
                return false;
            if (ActiveEditor != null)
            {
                if (ActiveEditor.IsNeededKey(e))
                    return false;
            }
            switch (e.KeyData)
            {
                case Keys.Up:
                    return DoKeyboardRefocus(e, this.FocusInfo.OnUp());
                case Keys.Down:
                    return DoKeyboardRefocus(e, this.FocusInfo.OnDown());
                case Keys.Left:
                    return DoKeyboardRefocus(e, this.FocusInfo.OnLeft());
                case Keys.Right:
                    return DoKeyboardRefocus(e, this.FocusInfo.OnRight());
                case Keys.Enter:
                    if (ActiveEditor != null)
                    {
                        ActiveEditor.DoValidate();
                        DisposeActiveEditor();
                        return true;
                    }
                    else {
                        return false;
                    }
                case Keys.Apps:
                case Keys.Alt | Keys.Down:
                    if (ActiveEditor == null)
                    {
                        e.Handled = true;
                        ShowElementMenu(FocusedItemType);
                        return true;
                    }
                    return false;
                case Keys.Escape:
                    if (ActiveEditor != null)
                    {
                        e.Handled = true;
                        DisposeActiveEditor();
                        return true;
                    }
                    return false;
                case Keys.Insert:
                case Keys.Add:
                    return DoAddElement();
                case Keys.Delete:
                case Keys.Subtract:
                    return DoDeleteElement();
                case Keys.Control | Keys.Insert:
                case Keys.Control | Keys.C:
                    return DoCopyElement();
                case Keys.Control | Keys.V:
                case Keys.Shift | Keys.Insert:
                    return DoPasteElement();
                case Keys.Control | Keys.X:
                case Keys.Shift | Keys.Delete:
                    return DoCutElement();
                case Keys.Control | Keys.Add:
                    return DoAddGroup();
                case Keys.Control | Keys.Q:
                    return DoSwapPropertyValue();
                default:
                    return false;
            }
        }
        bool DoCopyElement()
        {
            Node node = FocusInfo.Node;
            CriteriaOperator op = node.ToCriteria();
            if (ReferenceEquals(op, null))
                return false;
            try
            {
                Clipboard.SetDataObject(CriteriaOperator.ToString(op));
                return true;
            }
            catch
            {
                return false;
            }
        }
        bool DoPasteElement()
        {
            GroupNode addTo = FocusInfo.Node as GroupNode;
            if (addTo == null)
                addTo = FocusInfo.Node.ParentNode;
            try
            {
                string clpBrdTxt = DevExpress.XtraEditors.Mask.MaskBox.GetClipboardText();
                CriteriaOperator op = CriteriaOperator.Parse(clpBrdTxt);
                if (ReferenceEquals(op, null))
                    return false;
                Node cond = CriteriaToTreeProcessor.GetTree(op, null);
                addTo.SubNodes.Add(cond);
                cond.SetOwner(addTo.OwnerControl, addTo);
                FocusInfo = new FilterControlFocusInfo(cond, 0);
                RefreshTreeAfterNodeChange();
                MakeFocusedNodeVisible();
                return true;
            }
            catch
            {
                return false;
            }
        }
        bool DoCutElement()
        {
            return DoCopyElement() && DoDeleteElement();
        }
        bool CanCopyElement()
        {
            return !ReferenceEquals(FocusInfo.Node.ToCriteria(), null);
        }
        bool CanPasteElement()
        {
            try
            {
                string clp = DevExpress.XtraEditors.Mask.MaskBox.GetClipboardText();
                return !ReferenceEquals(CriteriaOperator.Parse(clp), null);
            }
            catch
            {
                return false;
            }
        }
        bool DoAddElement()
        {
            ClauseNode node = FocusInfo.Node as ClauseNode;
            if (node != null && FilterControlLabelInfo.IsCollectionClause(node.Operation) && FocusInfo.ElementIndex > 0)
            {
                AddElementCollection();
                return true;
            }
            if (node == null)
                AddClauseNodeAndFocusIt((GroupNode)FocusInfo.Node);
            else
                AddClauseNodeAndFocusIt(node.ParentNode);
            return true;
        }
        bool DoDeleteElement()
        {
            if (FocusInfo.Node == RootNode)
            {
                OnClearAll(this, EventArgs.Empty);
                return true;
            }
            ClauseNode node = FocusInfo.Node as ClauseNode;
            if (node != null && FilterControlLabelInfo.IsCollectionClause(node.Operation) && FocusInfo.ElementIndex > 1)
            {
                DeleteElementCollection();
                return true;
            }
            OnRemoveNode(this, EventArgs.Empty);
            return true;
        }
        bool DoAddGroup()
        {
            ClauseNode node = FocusInfo.Node as ClauseNode;
            if (node == null)
                AddGroup((GroupNode)FocusInfo.Node);
            else
                AddGroup(node.ParentNode);
            return true;
        }
        bool DoSwapPropertyValue()
        {
            if (!showOperandTypeIcon) return false;
            ClauseNode node = FocusInfo.Node as ClauseNode;
            if (node == null) return false;
            if (FocusInfo.ElementIndex < 2) return false;
            SwapPropertyValueActionMenu();
            return true;
        }
        bool DoKeyboardRefocus(KeyEventArgs e, FilterControlFocusInfo newFocusInfo)
        {
            e.Handled = true;
            if (ActiveEditor != null)
                ActiveEditor.DoValidate();
            this.FocusInfo = newFocusInfo;
            return true;
        }
        #endregion
        #region Property Actions
        internal void OnPropertyClick(object sender, EventArgs e)
        {
            DXMenuItem item = sender as DXMenuItem;
            if (item == null)
                return;
            MenuItemPropertyClick(((FilterColumn)item.Tag).FieldName);
        }
        protected virtual void MenuItemPropertyClick(string propertyName)
        {
            OperandProperty newProp = new OperandProperty(propertyName);
            ClauseNode node = (ClauseNode)FocusInfo.Node;
            if (FocusInfo.ElementIndex == 0)
            {
                ClauseType oldDefaultOp = FilterColumns.GetDefaultOperation(node.FirstOperand);
                node.FirstOperand = newProp;
                FilterColumn newColumn = FilterColumns[newProp];
                if (oldDefaultOp == node.Operation || (newColumn != null && !newColumn.IsValidClause(node.Operation)))
                {
                    node.Operation = FilterColumns.GetDefaultOperation(newProp);
                    node.ValidateAdditionalOperands();
                }
            }
            else {
                node.AdditionalOperands[FocusInfo.ElementIndex - 2] = newProp;
            }
            RefreshTreeAfterNodeChange();
            RaiseFilterChanged(new FilterChangedEventArgs(FilterChangedAction.FieldNameChange, node));
            FilterControlFocusInfo fi = FocusInfo.OnRight();
            if (fi.Node == FocusInfo.Node)
                FocusInfo = fi;
        }
        internal void OnGroupClick(object sender, EventArgs e)
        {
            DXMenuItem item = sender as DXMenuItem;
            if (item == null)
                return;
            ((GroupNode)FocusInfo.Node).NodeType = (GroupType)item.Tag;
            RefreshTreeAfterNodeChange();
            RaiseFilterChanged(new FilterChangedEventArgs(FilterChangedAction.GroupTypeChanged, FocusInfo.Node));
        }
        internal void OnClauseClick(object sender, EventArgs e)
        {
            DXMenuItem item = sender as DXMenuItem;
            if (item == null)
                return;
            ClauseNode node = (ClauseNode)FocusInfo.Node;
            node.Operation = (ClauseType)item.Tag;
            node.ValidateAdditionalOperands();
            RefreshTreeAfterNodeChange();
            RaiseFilterChanged(new FilterChangedEventArgs(FilterChangedAction.OperationChanged, node));
            FilterControlFocusInfo fi = FocusInfo.OnRight();
            if (fi.Node == FocusInfo.Node)
                FocusInfo = fi;
        }
        internal void OnRemoveNode(object sender, EventArgs e)
        {
            GroupNode parent = FocusInfo.Node.ParentNode;
            if (parent == null)
                return;
            int indexOfFocusedNode = parent.SubNodes.IndexOf(FocusInfo.Node);
            Node removedNode = FocusInfo.Node;
            System.Diagnostics.Debug.Assert(indexOfFocusedNode >= 0);
            parent.SubNodes.RemoveAt(indexOfFocusedNode);
            if (indexOfFocusedNode >= parent.SubNodes.Count)
                indexOfFocusedNode = parent.SubNodes.Count - 1;
            if (indexOfFocusedNode >= 0)
            {
                FocusInfo = new FilterControlFocusInfo((Node)parent.SubNodes[indexOfFocusedNode], 0);
            }
            else {
                FocusInfo = new FilterControlFocusInfo(parent, 0);
            }
            RefreshTreeAfterNodeChange();
            RaiseFilterChanged(new FilterChangedEventArgs(FilterChangedAction.RemoveNode, removedNode));
        }
        protected void AddClauseNodeAndFocusIt(GroupNode addTo)
        {
            ClauseNode cond = FilterColumns.CreateDefaultClauseNode(defaultColumn);
            addTo.SubNodes.Add(cond);
            cond.SetOwner(addTo.OwnerControl, addTo);
            FocusInfo = new FilterControlFocusInfo(cond, 0);
            RefreshTreeAfterNodeChange();
            RaiseFilterChanged(new FilterChangedEventArgs(FilterChangedAction.AddConditionNode, cond));
            MakeFocusedNodeVisible();
        }
        internal void OnAddCondition(object sender, EventArgs e)
        {
            GroupNode currentNode = (GroupNode)FocusInfo.Node;
            AddClauseNodeAndFocusIt(currentNode);
        }
        internal void OnAddGroup(object sender, EventArgs e)
        {
            AddGroup((GroupNode)FocusInfo.Node);
        }
        void AddGroup(GroupNode currentNode)
        {
            GroupNode gr = new GroupNode();
            gr.NodeType = GroupType.And;
            currentNode.SubNodes.Add(gr);
            RaiseFilterChanged(new FilterChangedEventArgs(FilterChangedAction.AddGroupNode, gr));
            gr.SetOwner(currentNode.OwnerControl, currentNode);
            AddClauseNodeAndFocusIt(gr);
        }
        internal void OnClearAll(object sender, EventArgs e)
        {
            ((GroupNode)FocusInfo.Node).SubNodes.Clear();
            RefreshTreeAfterNodeChange();
            RaiseFilterChanged(new FilterChangedEventArgs(FilterChangedAction.ClearAll, null));
        }
        void ShowMenu(DXPopupMenu menu)
        {
            if (FocusedItem == null) return;
            Point p = new Point(FocusedItem.ItemBounds.X, FocusedItem.Bottom);
            MenuManagerHelper.ShowMenu(menu, this.LookAndFeel, menuManager, this, p);
        }
        void ShowPropertyMenu()
        {
            ShowMenu(FilterViewInfo.ColumnMenu);
        }
        void ShowGroupMenu()
        {
            if (!ShowGroupCommandsIcon)
                FilterViewInfo.ClearGroupMenu();
            ShowMenu(FilterViewInfo.GroupMenu);
        }
        void ShowNodeActionMenu()
        {
            FilterViewInfo.ClearNodeActionMenu();
            ShowMenu(FilterViewInfo.NodeActionMenu);
        }
        void ShowClauseMenu()
        {
            FilterViewInfo.ClearClauseMenu();
            ShowMenu(FilterViewInfo.ClauseMenu);
        }
        #endregion
        #region ToolTips
        protected override ToolTipControlInfo GetToolTipInfo(Point point)
        {
            FilterControlLabelInfo li = RootNode.GetLabelInfoByCoordinates(point.X, point.Y);
            if (li == null || li.ViewInfo == null) return null;
            ToolTipInfo info = GetToolTipInfo(li.ViewInfo.ActiveItem as FilterLabelInfoTextViewInfo);
            if (info == null) return null;
            return new ToolTipControlInfo(li.ViewInfo.ActiveItem, info.Text, info.Title, false, ToolTipIconType.None);
        }
        ToolTipInfo GetToolTipInfo(FilterLabelInfoTextViewInfo item)
        {
            if (item == null) return null;
            switch (item.FilterViewInfo.ActiveItemType)
            {
                case ElementType.NodeAdd:
                    return new ToolTipInfo("增加一个新条件到当前分组", "");
                case ElementType.NodeRemove:
                    return new ToolTipInfo("删除条件", "");
                case ElementType.NodeAction:
                    return new ToolTipInfo("操作", "");
                case ElementType.FieldAction:
                    return new ToolTipInfo("比较两个字段的值", "");
                case ElementType.CollectionAction:
                    return new ToolTipInfo("增加项到列表", "");
            }
            return null;
        }
        class ToolTipInfo
        {
            string text, title;
            public ToolTipInfo(string text, string title)
            {
                this.text = text;
                this.title = title;
            }
            public string Text { get { return text; } }
            public string Title { get { return title; } }
        }
        #endregion
        #region ScrollBars
        bool postponedHorzScroll = false;
        bool suspendDisposeEditor = false;
        bool createEditor = false;
        bool invalidateScrollPosition = false;
        internal void InvalidateHorzScroll()
        {
            if (postponedHorzScroll) MakeFocusedNodeVisible();
            postponedHorzScroll = false;
            if (ActiveEditor != null) MakeFocusedActiveEditorVisible();
        }
        void MakeFocusedActiveEditorVisible()
        {
            if (!createEditor) return;
            suspendDisposeEditor = true;
            int dh = this.RootNode.ViewInfo.Left;
            if (HScrollJustification())
            {
                ActiveEditor.Left = FocusedItem.ItemBounds.X + (this.RootNode.ViewInfo.Left - dh);
            }
            suspendDisposeEditor = false;
            createEditor = false;
        }
        void MakeFocusedNodeVisible()
        {
            if (vScrollBar == null) return;
            VScrollJustification();
            HScrollJustification();
        }
        void VScrollJustification()
        {
            if (!vScrollBar.Visible) return;
            int line = (FocusInfo.Node.ViewInfo.Top - this.RootNode.ViewInfo.Top) / VScrollStep;
            if (FocusInfo.Node.ViewInfo.Top + VScrollStep >= VScrollHeight)
                this.vScrollBar.Value = line - VisibleRowCount + 1;
            if (FocusInfo.Node.ViewInfo.Top < 0)
                this.vScrollBar.Value = line;
        }
        bool HScrollJustification()
        {
            if (!hScrollBar.Visible) return false;
            if (FocusedItem == null)
            {
                postponedHorzScroll = true;
                return false;
            }
            int columnFirst = (FocusedItem.TextElement.X - this.RootNode.ViewInfo.Left) / HScrollStep;
            int columnLast = ((FocusedItem.TextElement.X + FocusedItem.TextElement.Width) - this.RootNode.ViewInfo.Left) / HScrollStep;
            if (FocusedItem.TextElement.X + FocusedItem.TextElement.Width >= HScrollWidth)
            {
                this.hScrollBar.Value = columnLast - VisibleColumnCount + 1;
                return true;
            }
            if (FocusedItem.TextElement.X < 0)
            {
                this.hScrollBar.Value = columnFirst;
                return true;
            }
            return false;
        }
        internal Rectangle ScrollSquareRectangle
        {
            get
            {
                if (VScrollVisible && HScrollVisible)
                    return new Rectangle(
                        this.ViewInfo.ClientRect.X + this.ViewInfo.ClientRect.Width - SystemInformation.VerticalScrollBarWidth,
                        this.ViewInfo.ClientRect.Y + this.ViewInfo.ClientRect.Height - SystemInformation.HorizontalScrollBarHeight,
                        SystemInformation.VerticalScrollBarWidth, SystemInformation.HorizontalScrollBarHeight);
                return Rectangle.Empty;
            }
        }
        bool VScrollVisible
        {
            get { return this.vScrollBar.Visible; }
            set
            {
                if (VScrollVisible == value) return;
                this.vScrollBar.Visible = value;
                if (!value) this.vScrollBar.Value = 0;
                InvalidateScrollBarPosition(true);
                if (value && ActiveEditor == null)
                    MakeFocusedNodeVisible();
            }
        }
        bool HScrollVisible
        {
            get { return this.hScrollBar.Visible; }
            set
            {
                if (HScrollVisible == value) return;
                this.hScrollBar.Visible = value;
                if (!value) this.hScrollBar.Value = 0;
                InvalidateScrollBarPosition(true);
                if (value && ActiveEditor == null)
                    MakeFocusedNodeVisible();
            }
        }
        int VScrollHeight { get { return this.ViewInfo.ClientRect.Height - (HScrollVisible ? SystemInformation.HorizontalScrollBarHeight : 0); } }
        int HScrollWidth { get { return this.ViewInfo.ClientRect.Width - (VScrollVisible ? SystemInformation.VerticalScrollBarWidth : 0); } }
        int VScrollStep { get { return FilterViewInfo.NodeHeight; } }
        int HScrollStep { get { return 10; } }
        int VisibleRowCount { get { return (VScrollHeight - FilterControlViewInfo.TopIndent / 2) / VScrollStep; } }
        int RowCount { get { return (this.RootNode.ViewInfo.Height + FilterControlViewInfo.TopIndent) / VScrollStep; } }
        int VisibleColumnCount { get { return (HScrollWidth - FilterControlViewInfo.LeftIndent / 2) / HScrollStep; } }
        int ColumnCount { get { return (((GroupNodeViewInfo)this.RootNode.ViewInfo).NodeWidth + FilterControlViewInfo.LeftIndent) / HScrollStep; } }
        int GetMaxValue(ScrollBarBase sb)
        {
            return sb.Maximum - sb.LargeChange + 1;
        }
        void CorrectScrollValue(int vScrollValue, int hScrollValue)
        {
            if (this.vScrollBar.Value > vScrollValue) this.vScrollBar.Value = vScrollValue;
            if (this.hScrollBar.Value > hScrollValue) this.hScrollBar.Value = hScrollValue;
        }
        internal void InvalidateScrollBarPosition()
        {
            InvalidateScrollBarPosition(false);
        }
        void InvalidateScrollBarPosition(bool immediately)
        {
            if (!invalidateScrollPosition && !immediately) return;
            this.vScrollBar.Bounds = new Rectangle(this.ViewInfo.ClientRect.X + this.ViewInfo.ClientRect.Width - SystemInformation.VerticalScrollBarWidth,
                this.ViewInfo.ClientRect.Y, SystemInformation.VerticalScrollBarWidth, VScrollHeight);
            this.hScrollBar.Bounds = new Rectangle(this.ViewInfo.ClientRect.X,
                this.ViewInfo.ClientRect.Y + this.ViewInfo.ClientRect.Height - SystemInformation.HorizontalScrollBarHeight,
                HScrollWidth, SystemInformation.HorizontalScrollBarHeight);
            VScrollVisible = RowCount > VisibleRowCount;
            HScrollVisible = ColumnCount > VisibleColumnCount;
            this.vScrollBar.Maximum = RowCount;
            this.vScrollBar.LargeChange = VisibleRowCount + 1;
            this.hScrollBar.Maximum = ColumnCount;
            this.hScrollBar.LargeChange = VisibleColumnCount + 1;
            CorrectScrollValue(GetMaxValue(this.vScrollBar), GetMaxValue(this.hScrollBar));
            invalidateScrollPosition = false;
        }
        void InitScrollBars()
        {
            this.vScrollBar = new DevExpress.XtraEditors.VScrollBar();
            this.vScrollBar.Parent = this;
            this.vScrollBar.Minimum = 0;
            this.hScrollBar = new DevExpress.XtraEditors.HScrollBar();
            this.hScrollBar.Parent = this;
            this.hScrollBar.Minimum = 0;
            UpdateScrollLookAndFeel();
            this.vScrollBar.ValueChanged += new EventHandler(OnScrollBarValueChanged);
            this.hScrollBar.ValueChanged += new EventHandler(OnScrollBarValueChanged);
            invalidateScrollPosition = true;
        }
        protected override void OnLookAndFeelChanged(object sender, EventArgs e)
        {
            base.OnLookAndFeelChanged(sender, e);
            UpdateScrollLookAndFeel();
        }
        void UpdateScrollLookAndFeel()
        {
            this.hScrollBar.LookAndFeel.Assign(this.LookAndFeel);
            this.vScrollBar.LookAndFeel.Assign(this.LookAndFeel);
        }
        void OnScrollBarValueChanged(object sender, EventArgs e)
        {
            if (!suspendDisposeEditor)
                DoEditorValidate();
            this.RootNode.ViewInfo.Top = FilterControlViewInfo.TopIndent;
            if (this.vScrollBar.Visible) this.RootNode.ViewInfo.Top -= this.vScrollBar.Value * VScrollStep;
            this.RootNode.ViewInfo.Left = FilterControlViewInfo.LeftIndent;
            if (this.hScrollBar.Visible) this.RootNode.ViewInfo.Left -= this.hScrollBar.Value * HScrollStep;
            Refresh(true, false);
        }
        #endregion
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
        }
    }
}
namespace lis.client.control
{
    public interface IFilteredComponent
    {
        event EventHandler RowFilterChanged;
        event EventHandler PropertiesChanged;
        CriteriaOperator RowCriteria { get; set; }
        FilterColumnCollection CreateFilterColumnCollection();
    }
    public interface IFilteredComponentsProvider
    {
        ICollection GetFilteredComponents();
    }
    public class FilterControlConverter : ReferenceConverter
    {
        public FilterControlConverter()
            : base(typeof(IListSource))
        {
        }
        protected virtual IContainer GetContainer(ITypeDescriptorContext context)
        {
            if (context == null) return null;
            if (context.Container != null) return context.Container;
            return null;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            IContainer container = GetContainer(context);
            if (container == null) return null;
            ArrayList array = new ArrayList();
            array.Add(null);
            foreach (IComponent component in container.Components)
            {
                if (component is IFilteredComponent || component is IFilteredXtraBindingList || component is DataTable || component is IBindingListView)
                {
                    array.Add(component);
                }
                else if (component is IFilteredComponentsProvider)
                {
                    IFilteredComponentsProvider provider = (IFilteredComponentsProvider)component;
                    array.AddRange(provider.GetFilteredComponents());
                }
            }
            TypeConverter.StandardValuesCollection collection = base.GetStandardValues(context);
            foreach (object obj in collection)
            {
                if (obj is IFilteredXtraBindingList || obj is DataTable || obj is IBindingListView)
                {
                    if (!array.Contains(obj))
                        array.Add(obj);
                }
            }
            return new StandardValuesCollection(array);
        }
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
    public class FilterControlViewInfo : BaseStyleControlViewInfo
    {
        internal static int TopIndent = 3;
        internal static int LeftIndent = 5;
        int levelIndent = 20;
        int textIndentHeight = 0;
        int nodeSeparator = 0;
        int singleLineHeight = 0;
        internal int textIndent = 0;
        public static int EditorDefaultWidth = 100;
        AppearanceObject paintAppeareanceTreeLine;
        Brush lineBrush = null;
        DXPopupMenu columnMenu, groupMenu, clauseMenu, nodeActionMenu;
        public FilterControlViewInfo(FilterControl owner)
            : base(owner)
        {
            this.paintAppeareanceTreeLine = new AppearanceObject();
        }
        public new FilterControl OwnerControl { get { return base.OwnerControl as FilterControl; } }
        public int NodeHeight { get { return RowHeight + nodeSeparator; } }
        internal int RowHeight { get { return SingleLineHeight + textIndent * 2; } }
        public int SingleLineHeight
        {
            get
            {
                if (singleLineHeight == 0)
                {
                    int editorMinHeight = 0;
                    GraphicsInfo.Default.AddGraphics(null);
                    try
                    {
                        using (RepositoryItemButtonEdit be = new RepositoryItemButtonEdit())
                        {
                            BaseEditViewInfo vi = be.CreateViewInfo();
                            vi.PaintAppearance.Assign(PaintAppearance);
                            editorMinHeight = vi.CalcMinHeight(GraphicsInfo.Default.Graphics);
                        }
                    }
                    finally
                    {
                        GraphicsInfo.Default.ReleaseGraphics();
                    }
                    singleLineHeight = PaintAppearance.FontHeight;
                    textIndent = (editorMinHeight - singleLineHeight) / 2 + textIndentHeight;
                }
                return singleLineHeight;
            }
        }
        public int LevelIndent
        {
            get { return levelIndent; }
            set
            {
                levelIndent = value;
            }
        }
        public int NodeSeparatorHeight
        {
            get { return nodeSeparator; }
            set
            {
                nodeSeparator = value;
            }
        }
        public Brush LineBrush
        {
            get
            {
                if (lineBrush == null)
                    lineBrush = new HatchBrush(HatchStyle.Percent50, PaintAppearanceTreeLine.ForeColor, PaintAppearanceTreeLine.BackColor);
                return lineBrush;
            }
        }
        protected internal void ClearLineBrush()
        {
            if (lineBrush == null) return;
            lineBrush.Dispose();
            lineBrush = null;
        }
        public DXPopupMenu ColumnMenu
        {
            get
            {
                if (columnMenu == null)
                {
                    columnMenu = new DXPopupMenu();
                    foreach (FilterColumn column in OwnerControl.FilterColumns)
                    {
                        string name = column.ColumnCaption;
                        DXMenuItem item = new DXMenuItem(name, new EventHandler(OwnerControl.OnPropertyClick), column.Image);
                        item.Tag = column;
                        columnMenu.Items.Add(item);
                    }
                }
                return columnMenu;
            }
        }
        public DXPopupMenu GroupMenu
        {
            get
            {
                if (groupMenu == null)
                {
                    groupMenu = new DXPopupMenu();
                    foreach (GroupType type in Enum.GetValues(typeof(GroupType)))
                    {
                        string menuTitle = OperationHelper.GetMenuStringByType(type);
                        DXMenuItem menuItem = new DXMenuItem(menuTitle, new EventHandler(OwnerControl.OnGroupClick), GetGroupImageByType(type));
                        menuItem.Tag = type;
                        groupMenu.Items.Add(menuItem);
                    }
                    if (!OwnerControl.ShowGroupCommandsIcon)
                    {
                        for (int i = 0; i < NodeActionMenu.Items.Count; i++)
                        {
                            DXMenuItem item = NodeActionMenu.Items[i];
                            if (i == 0) item.BeginGroup = true;
                            groupMenu.Items.Add(item);
                        }
                    }
                }
                return groupMenu;
            }
        }
        public DXPopupMenu ClauseMenu
        {
            get
            {
                if (clauseMenu == null)
                {
                    clauseMenu = new DXPopupMenu();
                    ClauseNode node = (ClauseNode)OwnerControl.FocusInfo.Node;
                    FilterColumn column = OwnerControl.FilterColumns[node.FirstOperand];
                    foreach (ClauseType type in Enum.GetValues(typeof(ClauseType)))
                    {
                        if (column == null || !column.IsValidClause(type))
                            continue;
                        string operationName = OperationHelper.GetMenuStringByType(type);
                        DXMenuItem item = new DXMenuItem(operationName, new EventHandler(OwnerControl.OnClauseClick), GetClauseImageByType(type));
                        item.Tag = type;
                        clauseMenu.Items.Add(item);
                    }
                }
                return clauseMenu;
            }
        }
        private Image GetClauseImageByType(ClauseType type)
        {
            return FilterControl.ClauseImages.Images[(int)type];
        }
        private Image GetGroupImageByType(GroupType type)
        {
            return FilterControl.GroupImages.Images[(int)type];
        }
        public DXPopupMenu NodeActionMenu
        {
            get
            {
                if (nodeActionMenu == null)
                {
                    nodeActionMenu = new DXPopupMenu();
                    DXMenuItem item = new DXMenuItem("增加条件", new EventHandler(OwnerControl.OnAddCondition), FilterControl.GroupImages.Images[4]);
                    nodeActionMenu.Items.Add(item);
                    item = new DXMenuItem("增加条件分组", new EventHandler(OwnerControl.OnAddGroup), FilterControl.GroupImages.Images[5]);
                    nodeActionMenu.Items.Add(item);
                    if (OwnerControl.RootNode.SubNodes.Count != 0)
                    {
                        if (OwnerControl.FocusInfo.Node == OwnerControl.RootNode)
                            item = new DXMenuItem("清除所有条件", new EventHandler(OwnerControl.OnClearAll), FilterControl.GroupImages.Images[6]);
                        else
                            item = new DXMenuItem("删除条件分组", new EventHandler(OwnerControl.OnRemoveNode), FilterControl.GroupImages.Images[7]);
                        item.BeginGroup = true;
                        nodeActionMenu.Items.Add(item);
                    }
                }
                return nodeActionMenu;
            }
        }
        protected internal void ClearGroupMenu()
        {
            ClearNodeActionMenu();
            if (groupMenu == null) return;
            groupMenu.Dispose();
            groupMenu = null;
        }
        protected internal void ClearNodeActionMenu()
        {
            if (nodeActionMenu == null) return;
            nodeActionMenu.Dispose();
            nodeActionMenu = null;
        }
        protected internal void ClearColumnMenu()
        {
            if (columnMenu == null) return;
            columnMenu.Dispose();
            columnMenu = null;
        }
        protected internal void ClearClauseMenu()
        {
            if (clauseMenu == null) return;
            clauseMenu.Dispose();
            clauseMenu = null;
        }
        protected override AppearanceDefault CreateDefaultAppearance()
        {
            return new AppearanceDefault(GetSystemColor(SystemColors.WindowText), GetSystemColor(SystemColors.Window));
        }
        public AppearanceObject PaintAppearanceTreeLine { get { return paintAppeareanceTreeLine; } }
        public override void UpdatePaintAppearance()
        {
            base.UpdatePaintAppearance();
            AppearanceHelper.Combine(PaintAppearanceTreeLine, new AppearanceObject[] { OwnerControl.AppearanceTreeLine },
                new AppearanceDefault(Color.Gray, Color.White));
            if (!viewInfoCreated) return;
            ClearViewInfo();
            if (this.OwnerControl != null && this.OwnerControl.RootNode != null)
                this.OwnerControl.RootNode.RecalcLabelInfo();
            viewInfoCreated = false;
        }
        public void ClearViewInfo()
        {
            singleLineHeight = 0;
        }
        bool viewInfoCreated = false;
        internal void OnHandleCreated()
        {
            viewInfoCreated = true;
        }
    }
    public class FilterControlPainter : BaseControlPainter
    {
        FilterControl owner;
        public FilterControlPainter(FilterControl owner)
            : base()
        {
            this.owner = owner;
        }
        public FilterControl Owner { get { return owner; } }
        public override void Draw(ControlGraphicsInfoArgs info)
        {
            Owner.RootNode.CalcSizes(info);
            Owner.RootNode.Paint(info, this);
            Owner.UpdateHotTrackItem();
            Owner.RefreshEditorPosition();
            Owner.InvalidateScrollBarPosition();
            Owner.InvalidateHorzScroll();
            DrawFocusRectangle(info);
            BorderHelper.GetPainter(owner.BorderStyle, owner.LookAndFeel).DrawObject(new BorderObjectInfoArgs(info.Cache, owner.FilterViewInfo.PaintAppearance, owner.FilterViewInfo.Bounds, owner.FilterViewInfo.State));
            DrawScrollSquareRectangle(owner.ScrollSquareRectangle, info);
        }
        internal void TestArea(Node node, ControlGraphicsInfoArgs info)
        {
        }
        public virtual void DrawNode(Node node, ControlGraphicsInfoArgs info)
        {
            if (node.LabelInfo == null) return;
            node.LabelInfo.Paint(info);
        }
        public virtual void DrawTreeLines(GroupNode node, ControlGraphicsInfoArgs info)
        {
            int textWidht = ((FilterLabelInfoTextViewInfo)node.LabelInfo.ViewInfo[0]).ItemBounds.Width;
            int x = node.ViewInfo.TextLocation.X + Math.Min(textWidht / 2, Owner.LevelIndent / 2);
            int y = node.ViewInfo.NodeBounds.Bottom;
            int height = 0;
            if (node.SubNodes.Count > 0)
                height = ((Node)node.SubNodes[node.SubNodes.Count - 1]).ViewInfo.TextBounds.Top + Owner.FilterViewInfo.SingleLineHeight / 2 - y;
            XPaint.Graphics.FillRectangle(info.Graphics, Owner.FilterViewInfo.LineBrush, new Rectangle(x, y, 1, height));
            foreach (Node subNode in node.SubNodes)
            {
                y = subNode.ViewInfo.TextBounds.Top + Owner.FilterViewInfo.SingleLineHeight / 2;
                int width = subNode.ViewInfo.Left - x;
                XPaint.Graphics.FillRectangle(info.Graphics, Owner.FilterViewInfo.LineBrush, new Rectangle(x, y, width, 1));
            }
        }
        public virtual void DrawFocusRectangle(ControlGraphicsInfoArgs info)
        {
            if (Owner.ActiveEditor != null) return;
            Color focusRectColor = SystemColors.Control;
            if (Owner.Focused) focusRectColor = SystemColors.ControlText;
            if (Owner.ShowEditors && Owner.FocusedItemType == ElementType.Value) return;
            if (Owner.FocusedItem != null)
            {
                Rectangle rect = Owner.FocusedItem.ItemBounds;
                rect.Inflate(2, 3);
                rect.Width--;
                info.Paint.DrawFocusRectangle(info.Graphics, rect, focusRectColor, Color.Empty);
            }
        }
        public virtual void DrawScrollSquareRectangle(Rectangle rect, ControlGraphicsInfoArgs info)
        {
            if (rect.IsEmpty) return;
            info.ViewInfo.PaintAppearance.FillRectangle(info.Cache, rect);
        }
    }
    public struct FilterControlFocusInfo
    {
        public readonly Node Node;
        public readonly int ElementIndex;
        public FilterControlFocusInfo(Node node, int elementIndex)
        {
            this.Node = node;
            this.ElementIndex = elementIndex;
        }
        public static bool operator ==(FilterControlFocusInfo fi1, FilterControlFocusInfo fi2)
        {
            return fi1.Node == fi2.Node && fi1.ElementIndex == fi2.ElementIndex;
        }
        public static bool operator !=(FilterControlFocusInfo fi1, FilterControlFocusInfo fi2)
        {
            return !(fi1 == fi2);
        }
        public override int GetHashCode()
        {
            return Node.GetHashCode() ^ ElementIndex;
        }
        public override bool Equals(object obj)
        {
            if (obj is FilterControlFocusInfo)
                return false;
            FilterControlFocusInfo another = (FilterControlFocusInfo)obj;
            return this == another;
        }
        public FilterControlFocusInfo OnRight()
        {
            if (ElementIndex >= Node.LastElementIndex)
                return OnDown();
            else
                return new FilterControlFocusInfo(Node, ElementIndex + 1);
        }
        public FilterControlFocusInfo OnLeft()
        {
            if (ElementIndex > 0)
            {
                return new FilterControlFocusInfo(Node, ElementIndex - 1);
            }
            else {
                Node prevNode = Node.GetPrevNode();
                return new FilterControlFocusInfo(prevNode, prevNode.LastElementIndex);
            }
        }
        public FilterControlFocusInfo OnUp()
        {
            return new FilterControlFocusInfo(Node.GetPrevNode(), 0);
        }
        public FilterControlFocusInfo OnDown()
        {
            return new FilterControlFocusInfo(Node.GetNextNode(), 0);
        }
    }
    class BindingListFilterProxy : IFilteredComponent
    {
        IBindingList dataSource = null;
        public BindingListFilterProxy(IBindingList dataSource)
        {
            this.dataSource = dataSource;
        }
        public IBindingList DataSource
        {
            get { return dataSource; }
        }
        void DS_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.Reset:
                    OnFilterChanged();
                    break;
                case ListChangedType.PropertyDescriptorAdded:
                case ListChangedType.PropertyDescriptorChanged:
                case ListChangedType.PropertyDescriptorDeleted:
                    OnPropertiesChanged();
                    break;
            }
        }
        void OnFilterChanged()
        {
            if (filterChanged != null)
                filterChanged(this, EventArgs.Empty);
        }
        void OnPropertiesChanged()
        {
            if (propertiesChanged != null)
                propertiesChanged(this, EventArgs.Empty);
        }
        EventHandler filterChanged, propertiesChanged;
        void BeforeAddEvent()
        {
            if (DataSource == null)
                return;
            if (filterChanged == null && propertiesChanged == null)
                DataSource.ListChanged += new ListChangedEventHandler(DS_ListChanged);
        }
        void AfterRemoveEvent()
        {
            if (DataSource == null)
                return;
            if (filterChanged == null && propertiesChanged == null)
                DataSource.ListChanged -= new ListChangedEventHandler(DS_ListChanged);
        }
        event EventHandler IFilteredComponent.RowFilterChanged
        {
            add
            {
                BeforeAddEvent();
                filterChanged += value;
            }
            remove
            {
                filterChanged -= value;
                AfterRemoveEvent();
            }
        }
        event EventHandler IFilteredComponent.PropertiesChanged
        {
            add
            {
                BeforeAddEvent();
                propertiesChanged += value;
            }
            remove
            {
                propertiesChanged -= value;
                AfterRemoveEvent();
            }
        }
        CriteriaOperator IFilteredComponent.RowCriteria
        {
            get
            {
                if (DataSource is IFilteredXtraBindingList)
                {
                    return ((IFilteredXtraBindingList)DataSource).Filter;
                }
                else if (DataSource is IBindingListView)
                {
                    return CriteriaOperator.TryParse(((IBindingListView)DataSource).Filter);
                }
                else
                    return null;
            }
            set
            {
                if (DataSource is IFilteredXtraBindingList)
                {
                    ((IFilteredXtraBindingList)DataSource).Filter = CriteriaOperator.Clone(value);
                }
                else if (DataSource is IBindingListView)
                {
                    ((IBindingListView)DataSource).Filter = CriteriaOperator.LegacyToString(value);
                }
                else {
                }
            }
        }
        FilterColumnCollection IFilteredComponent.CreateFilterColumnCollection()
        {
            return new DataColumnInfoFilterColumnCollection(DataSource);
        }
    }
    public class DataColumnInfoFilterColumn : FilterColumn
    {
        public readonly DataColumnInfo Column;
        RepositoryItem resolvedEditor;
        string resolvedCaption;
        Image resolvedImage = null;
        public DataColumnInfoFilterColumn(DataColumnInfo column)
            : base()
        {
            this.Column = column;
            resolvedCaption = this.Column.Caption;
        }
        public override RepositoryItem ColumnEditor
        {
            get
            {
                if (resolvedEditor == null)
                    resolvedEditor = CreateRepository();
                return resolvedEditor;
            }
        }
        public override FilterColumnClauseClass ClauseClass
        {
            get
            {
                if (ColumnType == typeof(string))
                {
                    return FilterColumnClauseClass.String;
                }
                else {
                    return FilterColumnClauseClass.Generic;
                }
            }
        }
        public override void Dispose()
        {
            base.Dispose();
            if (resolvedEditor != null)
            {
                resolvedEditor.Dispose();
                resolvedEditor = null;
            }
        }
        protected virtual RepositoryItem CreateRepository()
        {
            if (ColumnType == typeof(Boolean) || ColumnType == typeof(Boolean?))
                return new RepositoryItemCheckEdit();
            if (ColumnType == typeof(DateTime) || ColumnType == typeof(DateTime?))
                return new RepositoryItemDateEdit();
            return new RepositoryItemTextEdit();
        }
        public override string ColumnCaption
        {
            get { return resolvedCaption; }
        }
        public override Type ColumnType
        {
            get { return this.Column.Type; }
        }
        public override string FieldName
        {
            get { return this.Column.Name; }
        }
        public override Image Image
        {
            get { return resolvedImage; }
        }
        public override void SetColumnEditor(RepositoryItem item)
        {
            resolvedEditor = item;
        }
        public override void SetColumnCaption(string caption)
        {
            resolvedCaption = caption;
        }
        public override void SetImage(Image image)
        {
            resolvedImage = image;
        }
    }
    public class DataColumnInfoFilterColumnCollection : FilterColumnCollection
    {
        public DataColumnInfoFilterColumnCollection(DataColumnInfo[] columns)
        {
            Fill(columns);
        }
        public DataColumnInfoFilterColumnCollection(BindingContext context, object dataSource, string dataMember)
            : this(new DevExpress.Data.Helpers.MasterDetailHelper().GetDataColumnInfo(context, dataSource, dataMember))
        {
        }
        public DataColumnInfoFilterColumnCollection(object dataSource) : this(null, dataSource, null) { }
        protected virtual FilterColumn CreateFilterColumn(DataColumnInfo column)
        {
            return new DataColumnInfoFilterColumn(column);
        }
        protected virtual void Fill(DataColumnInfo[] columns)
        {
            if (columns == null) return;
            foreach (DataColumnInfo column in columns)
            {
                this.Add(CreateFilterColumn(column));
            }
        }
    }
    public class UnboundFilterColumn : FilterColumn
    {
        string columnCaption, fieldName;
        Image columnImage = null;
        Type columnType;
        RepositoryItem columnEdit;
        FilterColumnClauseClass clauseClass;
        public UnboundFilterColumn(string columnCaption, string fieldName, Type columnType, RepositoryItem columnEdit, FilterColumnClauseClass clauseClass)
        {
            this.columnEdit = columnEdit;
            this.clauseClass = clauseClass;
            this.columnCaption = columnCaption;
            this.columnType = columnType;
            this.fieldName = fieldName;
        }
        public override FilterColumnClauseClass ClauseClass
        {
            get { return clauseClass; }
        }
        public override RepositoryItem ColumnEditor
        {
            get { return columnEdit; }
        }
        public override string ColumnCaption
        {
            get { return columnCaption; }
        }
        public override Type ColumnType
        {
            get { return columnType; }
        }
        public override string FieldName
        {
            get { return fieldName; }
        }
        public override Image Image
        {
            get { return columnImage; }
        }
        public override void SetColumnEditor(RepositoryItem item)
        {
            columnEdit = item;
        }
        public override void SetColumnCaption(string caption)
        {
            columnCaption = caption;
        }
        public override void SetImage(Image image)
        {
            columnImage = image;
        }
    }
}
