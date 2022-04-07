

using System;
using System.Drawing;
using System.Collections;
using DevExpress.Data.Filtering;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors.Drawing;
using DevExpress.Utils.Drawing;
using DevExpress.Utils.Frames;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using System.Collections.Specialized;
using DevExpress.Data.Filtering.Helpers;
using System.Collections.Generic;
namespace lis.client.control
{
	public abstract class NodeViewInfo {
		protected Node fOwner;
		public NodeViewInfo(Node owner)
			: base() {
			this.fOwner = owner;
			Top = FilterControlViewInfo.TopIndent;
			Left = FilterControlViewInfo.LeftIndent;
		}
		public int Top = 0;
		public int Left = 0;
		int width, textWidth = 0;
		Rectangle textBounds, nodeBounds;
		public abstract int Height { get; }
		public abstract void UpdateBounds();
		protected internal FilterControlViewInfo ControlViewInfo { get { return fOwner.OwnerControl.FilterViewInfo;  } }
		public virtual int Width {
			get { return width; }
			set { width = value; }
		}
		protected virtual int TextWidth {
			get { return textWidth; }
			set { textWidth = value; }
		}
		public Rectangle TextBounds { get { return textBounds; } }
		public Rectangle NodeBounds { get { return nodeBounds; } }
		public virtual void CalcTextBounds(ControlGraphicsInfoArgs info) {
			if(TextWidth == 0) {
				TextWidth = LabelInfoHelper.GetFullWidth(fOwner, info, fOwner.LabelInfo, info.ViewInfo.Appearance.Font, info.ViewInfo.Appearance.GetStringFormat()) + 6;
				CalcRects();
			}
		}
		public virtual void CalcRects() {
			textBounds = new Rectangle(Left, Top + ControlViewInfo.textIndent, TextWidth, ControlViewInfo.SingleLineHeight);
			nodeBounds = new Rectangle(Left, Top, TextWidth, ControlViewInfo.NodeHeight - ControlViewInfo.NodeSeparatorHeight);
			if(fOwner.LabelInfo != null) fOwner.LabelInfo.ViewInfo.Clear();
		}
		public virtual void ClearViewInfo() {
			textWidth = 0;
		}
		public Size TextSize { get { return textBounds.Size; } }
		public Point TextLocation { get { return textBounds.Location; } }
	}
	public class GroupNodeViewInfo : NodeViewInfo {
		internal int NodeWidth = 0;
		public GroupNodeViewInfo(Node owner) : base(owner) { }
		public GroupNode Owner { get { return (GroupNode)this.fOwner; } }
		public override int Height {
			get {
				int childrenHeight = 0;
				foreach(Node subNode in Owner.SubNodes) {
					childrenHeight += subNode.ViewInfo.Height;
				}
				return ControlViewInfo.NodeHeight + childrenHeight;
			}
		}
		public override int Width {
			get {
				int maxChildWidth = 40;
				foreach(Node subNode in Owner.SubNodes) {
					int subNodeWidth = subNode.ViewInfo.Width;
					if(subNodeWidth > maxChildWidth)
						maxChildWidth = subNodeWidth;
				}
				return maxChildWidth + ControlViewInfo.LevelIndent;
			}
		}
		public override void CalcTextBounds(ControlGraphicsInfoArgs info) {
			foreach(Node subNode in Owner.SubNodes) subNode.ViewInfo.CalcTextBounds(info);
			base.CalcTextBounds(info);
			NodeWidth = Width;
		}
		public override void ClearViewInfo() {
			NodeWidth = Width;
			foreach(Node subNode in Owner.SubNodes) subNode.ViewInfo.ClearViewInfo();
			base.ClearViewInfo();
		}
		public override void UpdateBounds() {
			int currentTop = Top + ControlViewInfo.NodeHeight;
			foreach(Node subNode in Owner.SubNodes) {
				subNode.ViewInfo.Top = currentTop;
				subNode.ViewInfo.Left = Left + ControlViewInfo.LevelIndent;
				subNode.ViewInfo.UpdateBounds();
				currentTop += subNode.ViewInfo.Height;
			}
		}
	}
	public class ClauseNodeViewInfo : NodeViewInfo {
		public ClauseNodeViewInfo(Node owner) : base(owner) { }
		public ClauseNode Owner { get { return (ClauseNode)this.fOwner; } }
		public override int Height { get { return ControlViewInfo.NodeHeight; } }
		public override int Width { get { return TextWidth; } }
		public override void UpdateBounds() { }
	}
	public abstract class Node {
		FilterControl owner = null;
		NodeViewInfo viewInfo;
		FilterControlLabelInfo li;
		GroupNode parentNode;
		public Node() {
			ViewInfo = CreateViewInfo();
		}
		public virtual void SetOwner(FilterControl owner, GroupNode parentNode) {
			this.parentNode = parentNode;
			this.owner = owner;
			li = new FilterControlLabelInfo(this);
			li.CreateLabelInfoTexts(this);
		}
		public virtual void RecalcLabelInfo() {
			li.Clear();
			li.CreateLabelInfoTexts(this);
		}
		public abstract string Text { get; }
		public abstract CriteriaOperator ToCriteria();
		public abstract void Paint(ControlGraphicsInfoArgs info, FilterControlPainter painter);
		protected abstract NodeViewInfo CreateViewInfo();
		public NodeViewInfo ViewInfo {
			get { return viewInfo; }
			set { viewInfo = value; }
		}
		public FilterControl OwnerControl { get { return owner; } }
		public GroupNode ParentNode { get { return parentNode; } }
		public FilterControlLabelInfo LabelInfo { get { return li; } }
		protected internal virtual void ClearLabelInfoViewInfo() {
			if(LabelInfo == null) return;
			LabelInfo.ViewInfo.Clear();
		}
		protected internal virtual void ClearActiveItem() {
			if(LabelInfo == null) return;
			LabelInfo.ViewInfo.ActiveItem = null;
		}
		protected internal virtual FilterControlLabelInfo GetLabelInfoByCoordinates(int x, int y) {
			if(ViewInfo.TextBounds.Contains(x, y)) return LabelInfo;
			return null;
		}
		public virtual void CalcSizes(ControlGraphicsInfoArgs info) { this.ViewInfo.CalcTextBounds(info); }
		public abstract int LastElementIndex { get;}
		public virtual Node GetNextNode() {
			if(ParentNode == null)
				return this;
			return ParentNode.GetNextNodeAfter(this);
		}
		public Node GetPrevNode() {
			if(ParentNode == null)
				return this.GetLastNode();
			int indexInParent = ParentNode.SubNodes.IndexOf(this);
			if(indexInParent <= 0)
				return ParentNode;
			return ((Node)ParentNode.SubNodes[indexInParent - 1]).GetLastNode();
		}
		public abstract Node GetLastNode();
	}
	public enum GroupType { And, Or, NotAnd, NotOr }
	public class GroupNode : Node {
		public GroupType NodeType;
		public ArrayList SubNodes = new ArrayList();	
		protected override NodeViewInfo CreateViewInfo() {
			return new GroupNodeViewInfo(this);
		}
		public override void SetOwner(FilterControl owner, GroupNode parentNode) {
			foreach(Node subNode in SubNodes)
				subNode.SetOwner(owner, this);
			base.SetOwner(owner, parentNode);
		}
		public override string Text { get { return NodeType.ToString(); } }
		public override CriteriaOperator ToCriteria() {
			GroupOperatorType combineStatus = (NodeType == GroupType.And || NodeType == GroupType.NotAnd) ? GroupOperatorType.And : GroupOperatorType.Or;
			CriteriaOperator result = null;
			foreach(Node subNode in SubNodes) {
				result = GroupOperator.Combine(combineStatus, result, subNode.ToCriteria());
			}
			if(NodeType == GroupType.NotAnd || NodeType == GroupType.NotOr) {
				result = new UnaryOperator(UnaryOperatorType.Not, result);
			}
			return result;
		}
		protected internal override FilterControlLabelInfo GetLabelInfoByCoordinates(int x, int y) {
			FilterControlLabelInfo li = base.GetLabelInfoByCoordinates(x, y);
			if(li != null) return li;
			foreach(Node subNode in SubNodes) {
				li = subNode.GetLabelInfoByCoordinates(x, y);
				if(li != null) return li;
			}
			return null;
		}
		protected internal override void ClearLabelInfoViewInfo() {
			base.ClearLabelInfoViewInfo();
			foreach(Node subNode in SubNodes) 
				subNode.ClearLabelInfoViewInfo();
		}
		protected internal override void ClearActiveItem() {
			base.ClearActiveItem();
			foreach(Node subNode in SubNodes)
				subNode.ClearActiveItem();
		}
		public override void Paint(ControlGraphicsInfoArgs info, FilterControlPainter painter) {
			painter.TestArea(this, info);
			painter.DrawNode(this, info);
			foreach(Node subNode in SubNodes)
				subNode.Paint(info, painter);
			painter.DrawTreeLines(this, info);
		}
		public override int LastElementIndex {
			get { return 0; }
		}
		public override Node GetNextNode() {
			if(SubNodes.Count > 0)
				return (Node)SubNodes[0];
			else
				return base.GetNextNode();
		}
		public Node GetNextNodeAfter(Node node) {
			int indexOfNode = SubNodes.IndexOf(node);
			System.Diagnostics.Debug.Assert(indexOfNode >= 0);
			int nextIndex = indexOfNode + 1;
			if(nextIndex < SubNodes.Count) {
				return (Node)SubNodes[nextIndex];
			} else {
				if(ParentNode == null)
					return this;
				else
					return ParentNode.GetNextNodeAfter(this);
			}
		}
		public override Node GetLastNode() {
			if(this.SubNodes.Count <= 0)
				return this;
			return ((Node)this.SubNodes[this.SubNodes.Count - 1]).GetLastNode();
		}
		public override void RecalcLabelInfo() {
			base.RecalcLabelInfo();
			foreach(Node subNode in SubNodes)
				subNode.RecalcLabelInfo();
		}
	}
	public enum ClauseType { Equals, DoesNotEqual, Greater, GreaterOrEqual, Less, LessOrEqual, Between, NotBetween, Contains, DoesNotContain, BeginsWith, EndsWith, Like, NotLike, IsNull, IsNotNull, AnyOf, NoneOf }
	public class ClauseNode : Node {
		public OperandProperty FirstOperand;
		public ClauseType Operation;
		public readonly List<CriteriaOperator> AdditionalOperands = new List<CriteriaOperator>();	
		protected override NodeViewInfo CreateViewInfo() {
			return new ClauseNodeViewInfo(this);
		}
		static string ExtractStringValueSafe(CriteriaOperator op) {
			OperandValue prop = op as OperandValue;
			if(!ReferenceEquals(prop, null)) {
				if(prop.Value == null)
					return string.Empty;
				else
					return prop.Value.ToString();
			}
			return CriteriaOperator.ToString(op);
		}
		static string EscapeStringValueForLike(CriteriaOperator op) {
			return LikeData.Escape(ExtractStringValueSafe(op));
		}
		public override CriteriaOperator ToCriteria() {
			switch(Operation) {
				case ClauseType.Equals:
					return FirstOperand == AdditionalOperands[0];
				case ClauseType.DoesNotEqual:
					return FirstOperand != AdditionalOperands[0];
				case ClauseType.Less:
					return FirstOperand < AdditionalOperands[0];
				case ClauseType.Greater:
					return FirstOperand > AdditionalOperands[0];
				case ClauseType.LessOrEqual:
					return FirstOperand <= AdditionalOperands[0];
				case ClauseType.GreaterOrEqual:
					return FirstOperand >= AdditionalOperands[0];
				case ClauseType.AnyOf:
					return new InOperator(FirstOperand, AdditionalOperands);
				case ClauseType.NoneOf:
					return new UnaryOperator(UnaryOperatorType.Not, new InOperator(FirstOperand, AdditionalOperands));
				case ClauseType.BeginsWith:
					return new BinaryOperator(FirstOperand, new OperandValue(EscapeStringValueForLike(AdditionalOperands[0]) + '%'), BinaryOperatorType.Like);
				case ClauseType.EndsWith:
					return new BinaryOperator(FirstOperand, new OperandValue('%' + EscapeStringValueForLike(AdditionalOperands[0])), BinaryOperatorType.Like);
				case ClauseType.Between:
					return new BetweenOperator(FirstOperand, AdditionalOperands[0], AdditionalOperands[1]);
				case ClauseType.NotBetween:
					return new UnaryOperator(UnaryOperatorType.Not, new BetweenOperator(FirstOperand, AdditionalOperands[0], AdditionalOperands[1]));
				case ClauseType.Contains:
					return new BinaryOperator(FirstOperand, new OperandValue('%' + EscapeStringValueForLike(AdditionalOperands[0]) + '%'), BinaryOperatorType.Like);
				case ClauseType.DoesNotContain:
					return new UnaryOperator(UnaryOperatorType.Not, new BinaryOperator(FirstOperand, new OperandValue('%' + EscapeStringValueForLike(AdditionalOperands[0]) + '%'), BinaryOperatorType.Like));
				case ClauseType.Like:
					return new BinaryOperator(FirstOperand, AdditionalOperands[0], BinaryOperatorType.Like);
				case ClauseType.NotLike:
					return new UnaryOperator(UnaryOperatorType.Not, new BinaryOperator(FirstOperand, AdditionalOperands[0], BinaryOperatorType.Like));
				case ClauseType.IsNull:
					return FirstOperand.IsNull();
				case ClauseType.IsNotNull:
					return FirstOperand.IsNotNull();
				default:
					throw new NotImplementedException();
			}
		}
		public override string Text {
			get {
				string str = FirstOperand.ToString() + " " + Operation.ToString();
				foreach(CriteriaOperator op in AdditionalOperands) {
					str += " " + op.ToString();
				}
				return str;
			}
		}
		public override void Paint(ControlGraphicsInfoArgs info, FilterControlPainter painter) {
			painter.TestArea(this, info);
			painter.DrawNode(this, info);
		}
		public override int LastElementIndex {
			get { return 1 + AdditionalOperands.Count; }
		}
		public override Node GetLastNode() {
			return this;
		}
		public void ValidateAdditionalOperands() {
			switch(Operation) {
				case ClauseType.Equals:
				case ClauseType.DoesNotEqual:
				case ClauseType.Less:
				case ClauseType.Greater:
				case ClauseType.LessOrEqual:
				case ClauseType.GreaterOrEqual:
				case ClauseType.BeginsWith:
				case ClauseType.EndsWith:
				case ClauseType.Contains:
				case ClauseType.DoesNotContain:
				case ClauseType.Like:
				case ClauseType.NotLike:
					ForceAdditionalParamsCount(1);
					break;
				case ClauseType.IsNull:
				case ClauseType.IsNotNull:
					ForceAdditionalParamsCount(0);
					break;
				case ClauseType.Between:
				case ClauseType.NotBetween:
					ForceAdditionalParamsCount(2);
					break;
				case ClauseType.AnyOf:
				case ClauseType.NoneOf:
					break;
				default:
					throw new NotImplementedException();
			}
			if(IsOnlyValueClauseType()) {
				for(int i = 0; i < AdditionalOperands.Count; ++i) {
					if(!(AdditionalOperands[i] is OperandValue))
						AdditionalOperands[i] = new OperandValue();
				}
			}
		}
		void ForceAdditionalParamsCount(int p) {
			if(AdditionalOperands.Count > p)
				AdditionalOperands.RemoveRange(p, AdditionalOperands.Count - p);
			while(AdditionalOperands.Count < p) {
				AdditionalOperands.Add(new OperandValue());
			}
		}
		public bool ShowOperandTypeIcon {
			get {
				return OwnerControl.ShowOperandTypeIcon && !IsOnlyValueClauseType();
			}
		}
		private bool IsOnlyValueClauseType() {
			switch(Operation) {
				case ClauseType.BeginsWith:
				case ClauseType.EndsWith:
				case ClauseType.Contains:
				case ClauseType.DoesNotContain:
					return true;
				default:
					return false;
			}
		}
	}
	public class CriteriaToTreeProcessor : IClientCriteriaVisitor {
		public readonly IList Skipped;
		protected CriteriaToTreeProcessor(IList skippedHolder) {
			Skipped = skippedHolder;
		}
		protected Node Skip(CriteriaOperator skip) {
			if(Skipped != null && !ReferenceEquals(skip, null))
				Skipped.Add(skip);
			return null;
		}
		public void Visit(OperandProperty theOperand) {
			//return Skip(theOperand);
		}
        public void Visit(AggregateOperand theOperand) {
			//return Skip(theOperand);
		}
        public void Visit(FunctionOperator theOperator) {
			//return Skip(theOperator);
		}
        public void Visit(OperandValue theOperand) {
			//return Skip(theOperand);
		}
        public void Visit(GroupOperator theOperator) {
			//GroupNode result = new GroupNode();
			//result.NodeType = (theOperator.OperatorType == GroupOperatorType.And) ? GroupType.And : GroupType.Or;
			//foreach(CriteriaOperator subOperand in theOperator.Operands) {
			//	Node nestedNode = Process(subOperand);
			//	if(nestedNode != null)
			//		result.SubNodes.Add(nestedNode);
			//}
			//return result;
		}
        public void Visit(InOperator theOperator) {
			//ClauseNode result = new ClauseNode();
			//result.Operation = ClauseType.AnyOf;
			//result.FirstOperand = theOperator.LeftOperand as OperandProperty;
			//if(ReferenceEquals(result.FirstOperand, null))
			//	return Skip(theOperator);
			//foreach(CriteriaOperator ao in theOperator.Operands) {
			//	if(ao is OperandProperty || ao is OperandValue) {
			//		result.AdditionalOperands.Add(ao);
			//	} else {
			//		return Skip(theOperator);
			//	}
			//}
			//return result;
		}
        public void Visit(UnaryOperator theOperator) {
			//if(theOperator.OperatorType == UnaryOperatorType.IsNull) {
			//	ClauseNode result = new ClauseNode();
			//	result.Operation = ClauseType.IsNull;
			//	result.FirstOperand = theOperator.Operand as OperandProperty;
			//	if(ReferenceEquals(result.FirstOperand, null))
			//		return Skip(theOperator);
			//	return result;
			//} else if(theOperator.OperatorType == UnaryOperatorType.Not) {
			//	Node subNode = Process(theOperator.Operand);
			//	if(subNode is GroupNode) {
			//		GroupNode gr = (GroupNode)subNode;
			//		switch(gr.NodeType) {
			//			case GroupType.And:
			//				gr.NodeType = GroupType.NotAnd;
			//				break;
			//			case GroupType.Or:
			//				gr.NodeType = GroupType.NotOr;
			//				break;
			//			case GroupType.NotAnd:
			//				gr.NodeType = GroupType.And;
			//				break;
			//			case GroupType.NotOr:
			//				gr.NodeType = GroupType.Or;
			//				break;
			//			default:
			//				throw new NotImplementedException(gr.NodeType.ToString());
			//		}
			//		return gr;
			//	} else if(subNode is ClauseNode) {
			//		ClauseNode clause = (ClauseNode)subNode;
			//		switch(clause.Operation) {
			//			case ClauseType.AnyOf:
			//				clause.Operation = ClauseType.NoneOf;
			//				return clause;
			//			case ClauseType.Between:
			//				clause.Operation = ClauseType.NotBetween;
			//				return clause;
			//			case ClauseType.Contains:
			//				clause.Operation = ClauseType.DoesNotContain;
			//				return clause;
			//			case ClauseType.Equals:
			//				clause.Operation = ClauseType.DoesNotEqual;
			//				return clause;
			//			case ClauseType.Greater:
			//				clause.Operation = ClauseType.LessOrEqual;
			//				return clause;
			//			case ClauseType.GreaterOrEqual:
			//				clause.Operation = ClauseType.Less;
			//				return clause;
			//			case ClauseType.Like:
			//				clause.Operation = ClauseType.NotLike;
			//				return clause;
			//			case ClauseType.IsNotNull:
			//				clause.Operation = ClauseType.IsNull;
			//				return clause;
			//			case ClauseType.IsNull:
			//				clause.Operation = ClauseType.IsNotNull;
			//				return clause;
			//			case ClauseType.Less:
			//				clause.Operation = ClauseType.GreaterOrEqual;
			//				return clause;
			//			case ClauseType.LessOrEqual:
			//				clause.Operation = ClauseType.Greater;
			//				return clause;
			//			case ClauseType.NoneOf:
			//				clause.Operation = ClauseType.AnyOf;
			//				return clause;
			//			case ClauseType.NotBetween:
			//				clause.Operation = ClauseType.Between;
			//				return clause;
			//			case ClauseType.DoesNotContain:
			//				clause.Operation = ClauseType.Contains;
			//				return clause;
			//			case ClauseType.DoesNotEqual:
			//				clause.Operation = ClauseType.Equals;
			//				return clause;
			//			case ClauseType.NotLike:
			//				clause.Operation = ClauseType.Like;
			//				return clause;
			//			default:
			//				GroupNode notNode = new GroupNode();
			//				notNode.NodeType = GroupType.NotAnd;
			//				notNode.SubNodes.Add(clause);
			//				return notNode;
			//		}
			//	} else {
			//		return Skip(theOperator);
			//	}
			//} else {
			//	return Skip(theOperator);
			//}
		}
        public void Visit(BinaryOperator theOperator) {
			ClauseNode result = new ClauseNode();
			result.FirstOperand = theOperator.LeftOperand as OperandProperty;
			if(ReferenceEquals(result.FirstOperand, null))
				return ;
			switch(theOperator.OperatorType) {
				case BinaryOperatorType.Equal:
					result.Operation = ClauseType.Equals;
					break;
				case BinaryOperatorType.Greater:
					result.Operation = ClauseType.Greater;
					break;
				case BinaryOperatorType.GreaterOrEqual:
					result.Operation = ClauseType.GreaterOrEqual;
					break;
				case BinaryOperatorType.Less:
					result.Operation = ClauseType.Less;
					break;
				case BinaryOperatorType.LessOrEqual:
					result.Operation = ClauseType.LessOrEqual;
					break;
				case BinaryOperatorType.Like:
					OperandValue patternOperand = theOperator.RightOperand as OperandValue;
					if(!ReferenceEquals(patternOperand, null)) {
						string pattern = patternOperand.Value as string;
						if(pattern != null) {
							if(pattern.Length > 2 && pattern.StartsWith("%") && pattern.EndsWith("%")) {
								string patternBody = pattern.Substring(1, pattern.Length - 2);
								string unescaped = LikeData.UnEscape(patternBody);
								if(patternBody == LikeData.Escape(unescaped)) {
									result.Operation = ClauseType.Contains;
									result.AdditionalOperands.Add(new OperandValue(unescaped));
									return ;
								}
							} else if(pattern.Length > 1 && pattern.StartsWith("%")) {
								string patternBody = pattern.Substring(1);
								string unescaped = LikeData.UnEscape(patternBody);
								if(patternBody == LikeData.Escape(unescaped)) {
									result.Operation = ClauseType.EndsWith;
									result.AdditionalOperands.Add(new OperandValue(unescaped));
									return ;
								}
							} else if(pattern.Length > 1 && pattern.EndsWith("%")) {
								string patternBody = pattern.Substring(0, pattern.Length - 1);
								string unescaped = LikeData.UnEscape(patternBody);
								if(patternBody == LikeData.Escape(unescaped)) {
									result.Operation = ClauseType.BeginsWith;
									result.AdditionalOperands.Add(new OperandValue(unescaped));
									return ;
								}
							}
						}
					}
					result.Operation = ClauseType.Like;
					break;
				case BinaryOperatorType.NotEqual:
					result.Operation = ClauseType.DoesNotEqual;
					break;
				default:
					return ;
			}
			if(!(theOperator.RightOperand is OperandValue || theOperator.RightOperand is OperandProperty))
				return ;
			result.AdditionalOperands.Add(theOperator.RightOperand);
			return ;
		}
        public void Visit(BetweenOperator theOperator)
        {
            ClauseNode result = new ClauseNode();
            result.Operation = ClauseType.Between;
            result.FirstOperand = theOperator.TestExpression as OperandProperty;
            if (ReferenceEquals(result.FirstOperand, null))
                return ;
            if (!(theOperator.BeginExpression is OperandValue || theOperator.BeginExpression is OperandProperty))
                return ;
            if (!(theOperator.EndExpression is OperandValue || theOperator.EndExpression is OperandProperty))
                return ;
            result.AdditionalOperands.Add(theOperator.BeginExpression);
            result.AdditionalOperands.Add(theOperator.EndExpression);
            return ;
        }
        Node Process(CriteriaOperator op) {
			if(ReferenceEquals(op, null))
				return null;
			return null;
		}
		public static Node GetTree(CriteriaOperator op, IList skippedCriteria) {
			Node result = new CriteriaToTreeProcessor(skippedCriteria).Process(op);
			return result;
		}
		public static bool IsConvertibleOperator(CriteriaOperator opa) {
			IList skippedList = new CriteriaOperatorCollection();
			GetTree(opa, skippedList);
			return skippedList.Count == 0;
		}

        public void Visit(JoinOperand theOperand)
        {
            throw new NotImplementedException();
        }

        #region IClientCriteriaVisitor 成员


        #endregion
    }
	public class OperationHelper {
		public static string GetMenuStringByType(GroupType type) {
			switch(type) {
				case GroupType.And: return "并且";
				case GroupType.NotAnd: return "与非";
				case GroupType.NotOr: return "或非";
				case GroupType.Or: return "或";
			}
			return type.ToString();
		}
		public static string GetMenuStringByType(ClauseType type) {
			switch(type) {
				case ClauseType.AnyOf: return "在此列中";
				case ClauseType.BeginsWith: return "以此开始";
				case ClauseType.Between: return "在此之间";
				case ClauseType.Contains: return "包含";
                case ClauseType.EndsWith: return "以此结束";
				case ClauseType.Equals: return "等于";
				case ClauseType.Greater: return "大于";
				case ClauseType.GreaterOrEqual: return "大于或等于";
				case ClauseType.IsNotNull: return "不为空";
				case ClauseType.IsNull: return "为空";
				case ClauseType.Less: return "小于";
				case ClauseType.LessOrEqual: return "小于或等于";
				case ClauseType.Like: return "相似";
				case ClauseType.NoneOf: return "不在此列";
                case ClauseType.NotBetween: return "不在此之间";
				case ClauseType.DoesNotContain: return "不包含";
                case ClauseType.DoesNotEqual: return  "不等于";
                case ClauseType.NotLike: return  "不相似";
			}
			return type.ToString();
		}
	}
	public enum FilterColumnClauseClass { Generic, String, Lookup, Blob }
	public abstract class FilterColumn : IDisposable {
		public abstract string ColumnCaption { get; }
		public abstract string FieldName { get; }
		public abstract Type ColumnType { get; }
		public abstract FilterColumnClauseClass ClauseClass { get; }
		public abstract RepositoryItem ColumnEditor { get; }
		public virtual bool IsValidClause(ClauseType clause) {
			switch(clause) {
				case ClauseType.IsNotNull:
				case ClauseType.IsNull:
					return true;
				case ClauseType.AnyOf:
				case ClauseType.NoneOf:
				case ClauseType.Equals:
				case ClauseType.DoesNotEqual:
					return ClauseClass != FilterColumnClauseClass.Blob;
				case ClauseType.Contains:
				case ClauseType.DoesNotContain:
				case ClauseType.BeginsWith:
				case ClauseType.EndsWith:
				case ClauseType.Like:
				case ClauseType.NotLike:
					return ClauseClass == FilterColumnClauseClass.String;
				default:
					return ClauseClass == FilterColumnClauseClass.String || ClauseClass == FilterColumnClauseClass.Generic;
			}
		}
		public abstract Image Image { get; }
		public virtual void SetColumnEditor(RepositoryItem item) {
		}
		public virtual void SetColumnCaption(string caption) {
		}
		public virtual void SetImage(Image image) {
		}
		public virtual void Dispose() { }
	}
	public class FilterColumnCollection : CollectionBase, IDisposable {
		public FilterColumn this[int index] {
			get {
				return (FilterColumn)List[index];
			}
			set {
				List[index] = value;
			}
		}
		public int Add(FilterColumn value) {
			return List.Add(value);
		}
		public int IndexOf(FilterColumn value) {
			return List.IndexOf(value);
		}
		public void Insert(int index, FilterColumn value) {
			List.Insert(index, value);
		}
		public void Remove(FilterColumn value) {
			List.Remove(value);
		}
		public bool Contains(FilterColumn value) {
			return List.Contains(value);
		}
		public OperandProperty CreateDefaultProperty(FilterColumn column) {
			if(this.Count > 0) {
				if(column != null)
					return new OperandProperty(column.FieldName);
				else
					return new OperandProperty(this[0].FieldName);
			}
			else
				return new OperandProperty(string.Empty);
		}
		public FilterColumn this[string fieldName] {
			get {
				foreach(FilterColumn col in this) {
					if(col.FieldName == fieldName)
						return col;
				}
				return null;
			}
		}
		public FilterColumn this[OperandProperty property] {
			get {
				if(ReferenceEquals(property, null))
					return null;
				return this[property.PropertyName];
			}
		}
		public ClauseType GetDefaultOperation(OperandProperty operandProperty) {
			FilterColumn column = this[operandProperty];
			if(column == null)
				return ClauseType.Equals;
			switch(column.ClauseClass) {
				case FilterColumnClauseClass.Blob:
					return ClauseType.IsNotNull;
				case FilterColumnClauseClass.String:
					return ClauseType.BeginsWith;
				default:
					return ClauseType.Equals;
			}
		}
		public ClauseNode CreateDefaultClauseNode(FilterColumn column) {
			ClauseNode cond = new ClauseNode();
			cond.FirstOperand = CreateDefaultProperty(column);
			cond.Operation = GetDefaultOperation(cond.FirstOperand);
			cond.ValidateAdditionalOperands();
			return cond;
		}
		public virtual string GetDisplayPropertyName(OperandProperty property) {
			FilterColumn col = this[property];
			if(col != null)
				return col.ColumnCaption;
			else
				return property.PropertyName;
		}
		public virtual string GetValueScreenText(OperandProperty property, object value) {
			if(value == null)
				return "<空值>";
			FilterColumn col = this[property];
			if(col != null)
				return col.ColumnEditor.GetDisplayText(value);
			else
				return value.ToString();
		}
		public virtual void Dispose() {
			foreach(FilterColumn column in this) {
				column.Dispose();
			}
		}
		public void Sort() {
			InnerList.Sort(new FilterColumnCollectionSorter());
		}
		class FilterColumnCollectionSorter : IComparer {
			int IComparer.Compare(object x, object y) {
				FilterColumn col1 = (FilterColumn)x, col2 = (FilterColumn)y;
				return Comparer.Default.Compare(col1.ColumnCaption, col2.ColumnCaption);
			}
		}
	}
	public class ColumnsContainedCollector : IClientCriteriaVisitor {
		protected readonly IDictionary Columns = new HybridDictionary();
		//object IClientCriteriaVisitor.Visit(OperandProperty theOperand) {
		//	Columns[theOperand] = theOperand;
		//	return null;
		//}
		//object IClientCriteriaVisitor.Visit(AggregateOperand theOperand) {
		//	Process(theOperand.CollectionProperty);
		//	Process(theOperand.Condition);
		//	return null;
		//}
		//object ICriteriaVisitor.Visit(FunctionOperator theOperator) {
		//	foreach(CriteriaOperator op in theOperator.Operands)
		//		Process(op);
		//	return null;
		//}
		//object ICriteriaVisitor.Visit(OperandValue theOperand) {
		//	return null;
		//}
		//object ICriteriaVisitor.Visit(GroupOperator theOperator) {
		//	foreach(CriteriaOperator op in theOperator.Operands)
		//		Process(op);
		//	return null;
		//}
		//object ICriteriaVisitor.Visit(InOperator theOperator) {
		//	Process(theOperator.LeftOperand);
		//	foreach(CriteriaOperator op in theOperator.Operands)
		//		Process(op);
		//	return null;
		//}
		//object ICriteriaVisitor.Visit(UnaryOperator theOperator) {
		//	Process(theOperator.Operand);
		//	return null;
		//}
		//object ICriteriaVisitor.Visit(BinaryOperator theOperator) {
		//	Process(theOperator.LeftOperand);
		//	Process(theOperator.RightOperand);
		//	return null;
		//}
		//object ICriteriaVisitor.Visit(BetweenOperator theOperator) {
		//	Process(theOperator.TestExpression);
		//	Process(theOperator.BeginExpression);
		//	Process(theOperator.EndExpression);
		//	return null;
		//}
		void Process(CriteriaOperator op) {
			if(ReferenceEquals(op, null))
				return;
			op.Accept(this);
		}
		public static ICollection CollectColumns(CriteriaOperator criteria) {
			ColumnsContainedCollector collector = new ColumnsContainedCollector();
			collector.Process(criteria);
			return collector.Columns.Keys;
		}
		public static bool IsContained(CriteriaOperator criteria, OperandProperty op) {
			ColumnsContainedCollector collector = new ColumnsContainedCollector();
			collector.Process(criteria);
			return collector.Columns.Contains(op);
		}

        #region IClientCriteriaVisitor 成员

        public object Visit(JoinOperand theOperand)
        {
            throw new NotImplementedException();
        }

        public void Visit(AggregateOperand theOperand)
        {
            throw new NotImplementedException();
        }

        public void Visit(OperandProperty theOperand)
        {
            throw new NotImplementedException();
        }

        void IClientCriteriaVisitor.Visit(JoinOperand theOperand)
        {
            throw new NotImplementedException();
        }

        public void Visit(BetweenOperator theOperator)
        {
            throw new NotImplementedException();
        }

        public void Visit(BinaryOperator theOperator)
        {
            throw new NotImplementedException();
        }

        public void Visit(UnaryOperator theOperator)
        {
            throw new NotImplementedException();
        }

        public void Visit(InOperator theOperator)
        {
            throw new NotImplementedException();
        }

        public void Visit(GroupOperator theOperator)
        {
            throw new NotImplementedException();
        }

        public void Visit(OperandValue theOperand)
        {
            throw new NotImplementedException();
        }

        public void Visit(FunctionOperator theOperator)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
	public class DisplayCriteriaGenerator : IClientCriteriaVisitor {
		public readonly FilterColumnCollection Columns;
		protected DisplayCriteriaGenerator(FilterColumnCollection columns) {
			this.Columns = columns;
		}
		protected virtual CriteriaOperator ProcessPossibleValue(CriteriaOperator possibleProperty, CriteriaOperator possibleValue) {
			OperandProperty prop = possibleProperty as OperandProperty;
			if(!ReferenceEquals(prop, null)) {
				OperandValue val = possibleValue as OperandValue;
				if(!ReferenceEquals(val, null)) {
					return new OperandValue(ProcessValue(prop, val.Value));
				}
			}
			return Process(possibleValue);
		}
		protected virtual OperandProperty Convert(OperandProperty theOperand) {
			if(ReferenceEquals(theOperand, null))
				return null;
			return new OperandProperty(Columns.GetDisplayPropertyName(theOperand));
		}
		protected virtual object ProcessValue(OperandProperty originalProperty, object originalValue) {
			return Columns.GetValueScreenText(originalProperty, originalValue);
		}
		public virtual object Visit(OperandProperty theOperand) {
			return Convert(theOperand);
		}
		public virtual object Visit(AggregateOperand theOperand) {
			return new AggregateOperand(Convert(theOperand.CollectionProperty), CriteriaOperator.Clone(theOperand.AggregatedExpression), theOperand.AggregateType, Process(theOperand.Condition));
		}
		public virtual object Visit(FunctionOperator theOperator) {
			FunctionOperator result = new FunctionOperator(theOperator.OperatorType);
			foreach(CriteriaOperator op in theOperator.Operands)
				result.Operands.Add(Process(op));
			return result;
		}
		public virtual object Visit(OperandValue theOperand) {
			return new OperandValue(CriteriaOperator.ToString(theOperand));
		}
		public virtual object Visit(GroupOperator theOperator) {
			CriteriaOperator result = null;
			foreach(CriteriaOperator op in theOperator.Operands) {
				result = GroupOperator.Combine(theOperator.OperatorType, result, Process(op));
			}
			return result;
		}
		public virtual object Visit(InOperator theOperator) {
			InOperator result = new InOperator(Process(theOperator.LeftOperand));
			foreach(CriteriaOperator op in theOperator.Operands)
				result.Operands.Add(ProcessPossibleValue(theOperator.LeftOperand, op));
			return result;
		}
		public virtual object Visit(UnaryOperator theOperator) {
			return new UnaryOperator(theOperator.OperatorType, Process(theOperator.Operand));
		}
		public virtual object Visit(BinaryOperator theOperator) {
			return new BinaryOperator(Process(theOperator.LeftOperand), ProcessPossibleValue(theOperator.LeftOperand, theOperator.RightOperand), theOperator.OperatorType);
		}
		public virtual object Visit(BetweenOperator theOperator) {
			return new BetweenOperator(Process(theOperator.TestExpression), ProcessPossibleValue(theOperator.TestExpression, theOperator.BeginExpression), ProcessPossibleValue(theOperator.TestExpression, theOperator.EndExpression));
		}
		protected virtual CriteriaOperator Process(CriteriaOperator inputValue) {
			
				return null;
			
		}
		public static CriteriaOperator Process(FilterColumnCollection filterColumns, CriteriaOperator op) {
			return new DisplayCriteriaGenerator(filterColumns).Process(op);
		}

        

        void IClientCriteriaVisitor.Visit(AggregateOperand theOperand)
        {
            throw new NotImplementedException();
        }

        void IClientCriteriaVisitor.Visit(OperandProperty theOperand)
        {
            throw new NotImplementedException();
        }

        public void Visit(JoinOperand theOperand)
        {
            throw new NotImplementedException();
        }

        void ICriteriaVisitor.Visit(BetweenOperator theOperator)
        {
            throw new NotImplementedException();
        }

        void ICriteriaVisitor.Visit(BinaryOperator theOperator)
        {
            throw new NotImplementedException();
        }

        void ICriteriaVisitor.Visit(UnaryOperator theOperator)
        {
            throw new NotImplementedException();
        }

        void ICriteriaVisitor.Visit(InOperator theOperator)
        {
            throw new NotImplementedException();
        }

        void ICriteriaVisitor.Visit(GroupOperator theOperator)
        {
            throw new NotImplementedException();
        }

        void ICriteriaVisitor.Visit(OperandValue theOperand)
        {
            throw new NotImplementedException();
        }

        void ICriteriaVisitor.Visit(FunctionOperator theOperator)
        {
            throw new NotImplementedException();
        }

       
    }
	public class LocalaizableCriteriaToStringProcessor : CriteriaToStringBase {
		public readonly Localizer Localizer;
		protected LocalaizableCriteriaToStringProcessor(Localizer localizer) {
			this.Localizer = localizer;
		}
		public override string GetOperatorString(GroupOperatorType opType) {
			switch(opType) {
				case GroupOperatorType.And:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringGroupOperatorAnd);
				case GroupOperatorType.Or:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringGroupOperatorOr);
				default:
					return opType.ToString();
			}
		}
		protected override string GetIsNullText() {
			return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringUnaryOperatorIsNull);
		}
		public override string GetOperatorString(UnaryOperatorType opType) {
			switch(opType) {
				case UnaryOperatorType.BitwiseNot:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringUnaryOperatorBitwiseNot);
				case UnaryOperatorType.IsNull:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringUnaryOperatorIsNull);
				case UnaryOperatorType.Minus:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringUnaryOperatorMinus);
				case UnaryOperatorType.Not:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringUnaryOperatorNot);
				case UnaryOperatorType.Plus:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringUnaryOperatorPlus);
				default:
					return opType.ToString();
			}
		}
		public override string GetOperatorString(BinaryOperatorType opType) {
			switch(opType) {
				case BinaryOperatorType.BitwiseAnd:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringBinaryOperatorBitwiseAnd);
				case BinaryOperatorType.BitwiseOr:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringBinaryOperatorBitwiseOr);
				case BinaryOperatorType.BitwiseXor:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringBinaryOperatorBitwiseXor);
				case BinaryOperatorType.Divide:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringBinaryOperatorDivide);
				case BinaryOperatorType.Equal:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringBinaryOperatorEqual);
				case BinaryOperatorType.Greater:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringBinaryOperatorGreater);
				case BinaryOperatorType.GreaterOrEqual:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringBinaryOperatorGreaterOrEqual);
				case BinaryOperatorType.Less:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringBinaryOperatorLess);
				case BinaryOperatorType.LessOrEqual:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringBinaryOperatorLessOrEqual);
				case BinaryOperatorType.Like:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringBinaryOperatorLike);
				case BinaryOperatorType.Minus:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringBinaryOperatorMinus);
				case BinaryOperatorType.Modulo:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringBinaryOperatorModulo);
				case BinaryOperatorType.Multiply:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringBinaryOperatorMultiply);
				case BinaryOperatorType.NotEqual:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringBinaryOperatorNotEqual);
				case BinaryOperatorType.Plus:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringBinaryOperatorPlus);
				default:
					return opType.ToString();
			}
		}
		//public override object Visit(OperandValue operand) {
		//	return CriteriaToStringParameterlessProcessor.ValueToCriteriaToStringVisitResult(operand);
		//}
		protected override string GetBetweenText() {
			return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringBetween);
		}
		protected override string GetFunctionText(FunctionOperatorType operandType) {
			switch(operandType) {
				case FunctionOperatorType.Iif:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringFunctionIif);
				case FunctionOperatorType.IsNull:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringFunctionIsNull);
				case FunctionOperatorType.Len:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringFunctionLen);
				case FunctionOperatorType.Lower:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringFunctionLower);
				case FunctionOperatorType.None:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringFunctionNone);
				case FunctionOperatorType.Substring:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringFunctionSubstring);
				case FunctionOperatorType.Trim:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringFunctionTrim);
				case FunctionOperatorType.Upper:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringFunctionUpper);
				case FunctionOperatorType.Custom:
					return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringFunctionCustom);
				default:
					return base.GetFunctionText(operandType);
			}
		}
		protected override string GetInText() {
			return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringIn);
		}
		protected override string GetIsNotNullText() {
			return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringIsNotNull);
		}
		protected override string GetNotLikeText() {
			return Localizer.GetLocalizedString(StringId.FilterCriteriaToStringNotLike);
		}
		protected override string GetOperatorString(Aggregate operandType) {
			return base.GetOperatorString(operandType);
		}
		public static string Process(Localizer localizer, CriteriaOperator op) {
			if(ReferenceEquals(op, null))
				return string.Empty;
			return new LocalaizableCriteriaToStringProcessor(localizer).Process(op).Result;
		}

        public override CriteriaToStringVisitResult Visit(OperandValue operand)
        {
            throw new NotImplementedException();
        }
    }
}
