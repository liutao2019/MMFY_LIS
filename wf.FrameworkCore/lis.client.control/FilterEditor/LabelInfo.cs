
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using DevExpress.Utils.Frames;
using DevExpress.Utils.Drawing;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.XtraEditors.Drawing;
using DevExpress.Utils;
using DevExpress.Data.Filtering;
using DevExpress.Utils.Paint;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraEditors.Controls;
using System.Collections.Generic;
namespace lis.client.control
{
	public interface ILabelInfoEx : ILabelInfo  {
		Node Owner { get; }
	}
	public class FilterControlLabelInfo : ILabelInfoEx {
		Node node;
		LabelInfoTextCollection texts;
		FilterLabelInfoViewInfo viewInfo;
		int suspendTextChanges;
		public FilterControlLabelInfo(Node node) {
			this.node = node;
			this.texts = new LabelInfoTextCollection(this);
			this.viewInfo = new FilterLabelInfoViewInfo(this);
			this.suspendTextChanges = 0;
		}
		public string Text {
			get { return texts.Text; }
		}
		public LabelInfoTextCollection Texts { get { return this.texts; } }
		public void SuspendTextChanges() {
			this.suspendTextChanges++;
		}
		public void ResumeTextChanges() {
			ResumeTextChanges(true);
		}
		public void ResumeTextChanges(bool refresh) {
			this.suspendTextChanges--;
			if(this.suspendTextChanges < 0)
				this.suspendTextChanges = 0;
			if(refresh)
				TextInfoChanged();
		}
		public bool IsTextChangesSuspend { get { return this.suspendTextChanges > 0; } }
		public virtual void Refresh() {
			ViewInfo.Clear();
		}
		public void Clear() {
			texts.Clear();
			Refresh();
		}
		#region Painting
		public FilterLabelInfoViewInfo ViewInfo { get { return viewInfo; } }
		public virtual void Paint(ControlGraphicsInfoArgs info) {
			ViewInfo.Calculate(info.Graphics);
			ViewInfo.TopLine = 0;
			for(int i = 0; i < ViewInfo.Count; i++)
				ViewInfo[i].Draw(info.Cache, info.ViewInfo.Appearance.GetFont(), ViewInfo[i].InfoText.Color, info.ViewInfo.Appearance.GetStringFormat());
		}
		#endregion
		#region LabelCreator
		protected string GetDisplayText(OperandProperty property) {
			return new OperandProperty(node.OwnerControl.FilterColumns.GetDisplayPropertyName(property)).ToString();
		}
		protected string GetDisplayText(OperandProperty firstOperand, CriteriaOperator op) {
			OperandValue v = op as OperandValue;
			if(!ReferenceEquals(v, null)) {
				return node.OwnerControl.FilterColumns.GetValueScreenText(firstOperand, v.Value);
			}
			OperandProperty p = op as OperandProperty;
			if(!ReferenceEquals(p, null)) {
				return GetDisplayText(p);
			}
			return CriteriaOperator.ToString(op);
		}
		public void CreateLabelInfoTexts(Node node) {
			SuspendTextChanges();
			try {
				Texts.Clear();
				if(node is GroupNode) {
					AddLabelInfoText(OperationHelper.GetMenuStringByType(((GroupNode)node).NodeType), new FilterControlFocusInfo(node, 0), Owner.OwnerControl.AppearanceGroupOperatorColor, ElementType.Group, true);
					if(node.OwnerControl.ShowGroupCommandsIcon)
						AddLabelInfoText("@*", new FilterControlFocusInfo(node, 0), Color.Empty, ElementType.NodeAction, true);
					AddLabelInfoText("@+", new FilterControlFocusInfo(node, 0), Color.Empty, ElementType.NodeAdd, true);
				}
				ClauseNode clauseNode = node as ClauseNode;
				if(clauseNode != null) {
					AddLabelInfoText(GetDisplayText(clauseNode.FirstOperand), new FilterControlFocusInfo(clauseNode, 0), Owner.OwnerControl.AppearanceFieldNameColor, ElementType.Property, true);
					AddSpace();
					AddLabelInfoText(OperationHelper.GetMenuStringByType(clauseNode.Operation), new FilterControlFocusInfo(clauseNode, 1), Owner.OwnerControl.AppearanceOperatorColor, ElementType.Operation, true);
					AddSpace();
					AddAdditionalOperands(clauseNode);
					AddLabelInfoText("@-", new FilterControlFocusInfo(node, 0), Color.Empty, ElementType.NodeRemove, true);
				}
			}
			finally {
				ResumeTextChanges();
			}
			Invalidate();
		}
		public static bool IsCollectionClause(ClauseType type) {
			return type == ClauseType.AnyOf || type == ClauseType.NoneOf;
		}
		bool IsTwoFieldsClause(ClauseType type) {
			return type == ClauseType.Between || type == ClauseType.NotBetween;
		}
		const int MaxLength = 100;
		protected virtual string StringAdaptation(string text) {
			text = text.Replace("\r", "_").Replace("\n", "").Replace("\t", " ");
			if(text.Length > MaxLength) text = text.Substring(0, MaxLength) + "...";
			return text;
		}
		void AddAdditionalOperands(ClauseNode node) {
			List<CriteriaOperator> list = node.AdditionalOperands;
			ClauseNode cNode = node as ClauseNode;
			bool collection = IsCollectionClause(cNode.Operation);
			bool twoFields = IsTwoFieldsClause(cNode.Operation);
			if(list.Count > 1 && collection) AddLabelInfoText("(", new FilterControlFocusInfo(node, 2), Owner.OwnerControl.FilterViewInfo.PaintAppearance.ForeColor, ElementType.None, false);
			for(int i = 0; i < list.Count; i++) {
				CriteriaOperator op = list[i];
				FilterControlFocusInfo fi = new FilterControlFocusInfo(node, i + 2);
				string text = StringAdaptation(GetDisplayText(node.FirstOperand, op));
				if(text == null || text.Length == 0)
					text = "''";
				Color color;
				ElementType elementType;
				if(op is OperandProperty) {
					color = Owner.OwnerControl.AppearanceFieldNameColor;
					elementType = ElementType.Property;
				} else {
					color = Owner.OwnerControl.AppearanceValueColor;
					elementType = ElementType.Value;
					if(op.ToString() == "?") {
						text = ("<ÇëÊäÈëÖµ>");
						color = Owner.OwnerControl.AppearanceEmptyValueColor;
					}
				}
				AddLabelInfoText(text, fi, color, elementType, true);
				if(node.ShowOperandTypeIcon)
					AddLabelInfoText("@#", fi, Color.Empty, ElementType.FieldAction, true);
				if(i < list.Count - 1 && collection)
					AddLabelInfoText(", ", new FilterControlFocusInfo(node, 1), Owner.OwnerControl.FilterViewInfo.PaintAppearance.ForeColor, ElementType.None, false);
				if(i < list.Count - 1 && twoFields) {
					AddLabelInfoText(" " + Localizer.Active.GetLocalizedString(StringId.FilterClauseBetweenAnd) + " ", new FilterControlFocusInfo(node, 1), Owner.OwnerControl.AppearanceOperatorColor, ElementType.None, false);
				}
			}
			if(collection) {
				if(list.Count > 1) AddLabelInfoText(")", new FilterControlFocusInfo(node, list.Count + 1), Owner.OwnerControl.FilterViewInfo.PaintAppearance.ForeColor, ElementType.None, false);
				AddLabelInfoText("@+", new FilterControlFocusInfo(node, 0), Color.Empty, ElementType.CollectionAction, true);
			}
		}
		void AddLabelInfoText(string text, FilterControlFocusInfo focusInfo, Color activeColor, ElementType type, bool active) {
			if(active) {
				LabelInfoText info = Texts.Add(text, activeColor, true);
				info.Tag = new NodeElement(focusInfo, type);
			}
			else Texts.Add(text, activeColor, active);
		}
		void AddSpace() {
			Texts.Add(" ", false);
		}
		#endregion
		#region ILabelInfo Members
		public Node Owner { get { return node; } }
		public Size ClientSize {
			get { return node.ViewInfo.TextSize; }
		}
		public Point ClientLocation {
			get { return node.ViewInfo.TextLocation; }
		}
		public Font Font {
			get { return node.OwnerControl.Appearance.Font; }
		}
		public void Invalidate() {
			node.OwnerControl.Invalidate();
		}
		public int ScrollBarWidth {
			get { return 0; }
		}
		public void TextInfoChanged() {
			if(!IsTextChangesSuspend) {
				Refresh();
			}
		}
		#endregion
	}
	public enum ElementType { None, Property, Value, Operation, Group, FieldAction, CollectionAction, NodeRemove, NodeAction, NodeAdd }
	public class NodeElement {
		FilterControlFocusInfo focusInfo;
		ElementType type;
		public NodeElement(FilterControlFocusInfo focusInfo, ElementType type) {
			this.focusInfo = focusInfo;
			this.type = type;
		}
		public ElementType Type { get { return type; } }
		public FilterControlFocusInfo GetFocusInfo() {
			return focusInfo;
		}
	}
	public class FilterLabelInfoViewInfo : LabelInfoViewInfo {
		public FilterLabelInfoViewInfo(ILabelInfo label)
			: base(label) {
		}
		protected override LabelInfoTextViewInfoBase CreateViewInfo(LabelInfoText infoText, int width) {
			return new FilterLabelInfoTextViewInfo(this, infoText, width, this.Label.ClientLocation);
		}
		public Node Owner {
			get {
				ILabelInfoEx e = this.Label as ILabelInfoEx;
				if(e == null) return null;
				return e.Owner;
			}
		}
		public ElementType ActiveItemType {
			get {
				if(ActiveItem == null) return ElementType.None;
				NodeElement e = ActiveItem.InfoText.Tag as NodeElement;
				if(e == null) return ElementType.None;
				return e.Type;
			}
		}
	}
	public class FilterLabelInfoTextLineViewInfo {
		string text;
		int x, y, width, height;
		public FilterLabelInfoTextLineViewInfo(string text, int x, int y, int width, int height) {
			this.text = text;
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}
		public string Text { get { return text; } }
		public int X { get { return x; } }
		public int Y { get { return y; } }
		public int Width { get { return width; } }
		public int Height { get { return height; } }
		public Rectangle Bounds { get { return new Rectangle(X, Y, Width, Height); } }
	}
	public class LabelInfoHelper {
		public static bool ViewInfoEquals(FilterLabelInfoTextViewInfo vi1, FilterLabelInfoTextViewInfo vi2) {
			if(vi1 == null || vi2 == null) return ReferenceEquals(vi1, vi2);
			return ReferenceEquals(vi1.InfoText, vi2.InfoText);
		}
		public static bool EditorItem(FilterLabelInfoTextViewInfo info) {
			return EditorItem(((ILabelInfoEx)info.ViewInfo.Label).Owner.OwnerControl, info.InfoText);
		}
		public static bool EditorItem(FilterControl control, LabelInfoText text) {
			if(!control.ShowEditors
				&& control.ActiveEditor != null
				&& control.FocusedItem != null
				&& control.FocusedItemType == ElementType.Value
				&& control.FocusedItem.InfoText.Equals(text))
				return true;
			if(control.ShowEditors
				&& text.Tag != null
				&& ((NodeElement)text.Tag).Type == ElementType.Value)
				return true;
			return false;
		}
		public static Rectangle GetEditorBoundsByElement(FilterLabelInfoTextViewInfo element) {
			FilterControl control = element.FilterViewInfo.Owner.OwnerControl;
			Rectangle bounds = element.TextElement.Bounds;
			bounds.Y -= (control.FilterViewInfo.RowHeight - bounds.Height) / 2;
			bounds.Height = control.FilterViewInfo.RowHeight;
			bounds.Width = GetElementWidth(true, bounds.Width);
			return bounds;
		}
		static string specialSymbols = "*+-#";
		public static bool IsActionElement(string text) {
			if(text.Length != 2) return false;
			return (text[0] == '@' && specialSymbols.IndexOf(text[1]) > -1);
		}
		public static int ActionElementWidht {
			get {
				return FilterControl.NodeImages.ImageSize.Width + 4;
			}
		}
		public static int GetElementWidth(bool editor, int width) {
			if(editor) return  Math.Max(FilterControlViewInfo.EditorDefaultWidth, width);
			return width;
		}
		public static int GetElementWidth(bool editor, string text, GraphicsCache cache, Font font, StringFormat format) {
			int width = 0;
			if(LabelInfoHelper.IsActionElement(text))
				width = LabelInfoHelper.ActionElementWidht;
			else {
				if(!(cache.Paint is XPaintMixed))
				cache.Paint = new XPaintMixed(); 
				width = (int)cache.CalcTextSize(text, font, format, 0).Width;
			}
			return GetElementWidth(editor, width);
		}
		public static int GetFullWidth(Node node, ControlGraphicsInfoArgs info, FilterControlLabelInfo labelInfo, Font font, StringFormat format) {
			int ret = 0;
			for(int i = 0; i < labelInfo.Texts.Count; i++) {
				ret += GetElementWidth(LabelInfoHelper.EditorItem(node.OwnerControl, labelInfo.Texts[i]), labelInfo.Texts[i].Text, info.Cache, font, format);
			}
			return ret;
		}
	}
	public class FilterLabelInfoTextViewInfo : LabelInfoTextViewInfoBase {
		ArrayList list;
		Point location;
		public FilterLabelInfoTextViewInfo(FilterLabelInfoViewInfo viewInfo, LabelInfoText infoText, int width, Point location)
			: base(viewInfo, infoText, width) {
			this.location = location;
			this.list = new ArrayList();
		}
		public int Count { get { return list.Count; } }
		public FilterLabelInfoTextLineViewInfo TextElement { 
			get {
				if(this.Count > 0)
					return this[0];
				else return new FilterLabelInfoTextLineViewInfo("", 0, 0, 10, 10);
			} 
		}
		FilterLabelInfoTextLineViewInfo this[int index] { get { return list[index] as FilterLabelInfoTextLineViewInfo; } }
		public override int Bottom { get { return Count > 0 ? this[Count - 1].Bounds.Bottom : base.Bottom; } }
		public override int Top { get { return Count > 0 ? TextElement.Bounds.Top : base.Top; } }
		public override int LineCount { get { return 0; } }
		public Rectangle ItemBounds { get { return TextElement.Bounds; } }
		public override int GetScrollHeight(int lineCount) { return 0; }
		public virtual int GetLineHeight(Font font) { return (int)font.GetHeight(); }
		public override void Calculate(Graphics graphics, Font font, int fontHeight, ref int x, ref int y) {
			if(InfoText.Text == "") return;
			if(IsBreak) {
				y += (GetLineHeight(font) >> 1);
				return;
			}
			string drawText = InfoText.Text;
			while(drawText.Length > 0) {
				string words = GetNextWords(graphics, drawText, font, Width - x, x == LabelInfoViewInfo.TextIndent);
				if(words == string.Empty) return;
				int wordsWidth = 0;
				if(LabelInfoHelper.IsActionElement(this.InfoText.Text)) 
					wordsWidth = LabelInfoHelper.ActionElementWidht;
				else {
					wordsWidth = LabelInfoHelper.GetElementWidth(LabelInfoHelper.EditorItem(this), GetStringWidth(graphics, words, font));
				}
				FilterLabelInfoTextLineViewInfo item = new FilterLabelInfoTextLineViewInfo(words, location.X + x, location.Y + y, wordsWidth, fontHeight);
				list.Add(item);
				x += wordsWidth;
				if(x >= Width) {
					y += GetLineHeight(font);
					x = LabelInfoViewInfo.TextIndent;
				}
				if(words == drawText)
					break;
				drawText = drawText.Substring(words.Length, drawText.Length - words.Length);
			}
		}
		void DrawButton(GraphicsCache cache) {
			int imageIndex = -1;
			switch(this.InfoText.Text[1]) { 
				case '-':
					imageIndex = IsActive ? 1 : 0;
					break;
				case '*':
					imageIndex = IsActive ? 3 : 2;
					break;
				case '+':
					imageIndex = IsActive ? 5 : 4;
					break;
				case '#':
					NodeElement element = this.InfoText.Tag as NodeElement;
					ClauseNode node = (ClauseNode)element.GetFocusInfo().Node;
					object obj = node.AdditionalOperands[element.GetFocusInfo().ElementIndex - 2];
					imageIndex = (IsActive ? 7 : 6) + (obj is OperandValue ? 2 : 0);
					break;
			}
			if(imageIndex == -1) return;
			int y = this.TextElement.Y + (this.TextElement.Height - FilterControl.NodeImages.ImageSize.Height) / 2;
			int x = this.TextElement.X + (this.TextElement.Width - FilterControl.NodeImages.ImageSize.Width) / 2;
			x = Math.Max(x, this.TextElement.X);
			cache.Paint.DrawImage(cache.Graphics, FilterControl.NodeImages.Images[imageIndex], new Point(x, y)); 
		}
		void DrawTestEditor(GraphicsCache cache, string text) {
			if(FilterViewInfo.Owner.OwnerControl.FocusedItem == this 
				&& FilterViewInfo.Owner.OwnerControl.ActiveEditor != null) 
				return;
			ClauseNode node = FilterViewInfo.Owner as ClauseNode;
			if(node == null) return;
			RepositoryItem ri = node.OwnerControl.GetRepositoryItem(node).Clone() as RepositoryItem;
			ri.BorderStyle = FilterViewInfo.Owner.OwnerControl.FocusedItem == this ? DevExpress.XtraEditors.Controls.BorderStyles.Default : DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
			BaseEditPainter p = ri.CreatePainter();
			BaseEditViewInfo vi = ri.CreateViewInfo();
			vi.EditValue = this.TextElement.Text;
			vi.PaintAppearance.Assign(FilterViewInfo.Owner.OwnerControl.FilterViewInfo.PaintAppearance);
			vi.Bounds = LabelInfoHelper.GetEditorBoundsByElement(this);
			vi.CalcViewInfo(cache.Graphics);
			p.Draw(new ControlGraphicsInfoArgs(vi, cache, vi.Bounds));
		}
		public override void Draw(GraphicsCache cache, Font font, Color foreColor, StringFormat format) {
			if(IsBreak) return;
			if(LabelInfoHelper.IsActionElement(this.InfoText.Text)) {
				DrawButton(cache);
				return;
			}
			if(LabelInfoHelper.EditorItem(this)) {
				DrawTestEditor(cache, this.InfoText.Text);
				return;
			}
			Font activeFont = IsActive ? new Font(font, font.Style | FontStyle.Underline) : null;
			StringFormat sformat = format == null ? CreateStringFormat() : format;
			for(int i = 0; i < Count; i++) {
				Rectangle bounds = this[i].Bounds;
				bounds.Y = bounds.Y - ScrollTop;
				cache.DrawString(this[i].Text, IsActive ? activeFont : font, cache.GetSolidBrush(foreColor), bounds, sformat);
			}
			if(format == null) sformat.Dispose();
			if(activeFont != null)
				activeFont.Dispose();
		}
		StringFormat CreateStringFormat() {
			StringFormat format = new StringFormat(TextOptions.DefaultStringFormat);
			return format;
		}
		public override bool IsContains(Point pt) {
			pt.Y += ScrollTop;
			for(int i = 0; i < Count; i++) {
				if(this[i].Bounds.Contains(pt))
					return true;
			}
			return false;
		}
		string GetNextWords(Graphics graphics, string drawText, Font font, int wordsWidth, bool isStartLine) {
			string res = isStartLine ? GetNextWord(ref drawText) : "";
			string nextWord = "";
			while(drawText != "") {
				nextWord = GetNextWord(ref drawText);
				if(nextWord == "" || nextWord[0] < ' ') break;
				if(GetStringWidth(graphics, res + nextWord, font) > wordsWidth)
					break;
				res += nextWord;
			}
			return res;
		}
		int GetStringWidth(Graphics graphics, string drawText, Font font) {
			if(drawText.Length == 0) return 0;
			using(GraphicsCache cache = new GraphicsCache(graphics)) {
				using(StringFormat format = CreateStringFormat()) {
					format.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
					return LabelInfoHelper.GetElementWidth(LabelInfoHelper.EditorItem(this), drawText, cache, font, format);
				}
			}
		}
		string GetNextWord(ref string drawText) {
			if(drawText.Length == 0) return "";
			int i = 1;
			while(i < drawText.Length && !char.IsWhiteSpace(drawText[i]) && drawText[i] >= ' ')
				i++;
			string word = drawText.Substring(0, i);
			drawText = drawText.Substring(i, drawText.Length - i);
			return word;
		}
		internal FilterLabelInfoViewInfo FilterViewInfo {
			get { return this.ViewInfo as FilterLabelInfoViewInfo;  }
		}
	}
}
