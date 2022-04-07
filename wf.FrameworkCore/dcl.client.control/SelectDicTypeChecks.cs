using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.control
{
    public partial class SelectDicTypeChecks : UserControl
    {
        public SelectDicTypeChecks()
        {
            InitializeComponent();
        }

        private void SelectDicTypeChecks_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                tlType.DataSource=CacheClient.GetCache<EntityTypeBarcode>();
                tlType.ExpandAll();
                if (valueMember != null && valueMember.Count > 0)
                    setTypeCheck(valueMember);
            }
        }

        /// <summary>
        /// 实际值
        /// </summary>
        public ArrayList valueMember
        {
            get { return (ArrayList)lueType.Tag; }
            set
            {
                lueType.Tag = value;
                setTypeCheck(value);
            }
        }

        [Browsable(true), Category("HopeSelect"), Description("边框")]
        public DevExpress.XtraEditors.Controls.BorderStyles PBorderStyle
        {
            get { return lueType.BorderStyle; }
            set
            {

                lueType.BorderStyle = value;

                if (value == DevExpress.XtraEditors.Controls.BorderStyles.NoBorder)
                {
                    lueType.Properties.Buttons.Clear();
                }
            }
        }

        /// <summary>
        /// 设置物理组选中
        /// </summary>
        /// <param name="value"></param>
        private void setTypeCheck(ArrayList value)
        {
            for (int i = 0; i < tlType.AllNodesCount; i++)
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tlType.FindNodeByID(i);
                string userTypeId = tn.GetValue(colTypeId).ToString();
                if (value.Contains(userTypeId) == true)
                    tn.Checked = true;
            }
        }

        /// <summary>
        /// 显示值
        /// </summary>
        public String displayMember
        {
            get { return lueType.Text; }
            set { lueType.Text = value; }
        }

        /// <summary>
        /// 选择父节点，所有子节点都选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlType_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            DevExpress.XtraTreeList.Nodes.TreeListNode tn = e.Node;
            SetCheckedChildNodes(tn, tn.Checked);
        }

        /// <summary>
        /// 递归选择子节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="check"></param>
        private void SetCheckedChildNodes(DevExpress.XtraTreeList.Nodes.TreeListNode node, bool check)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                node.Nodes[i].Checked = check;
                SetCheckedChildNodes(node.Nodes[i], check);
            }
        }

        /// <summary>
        /// 赋值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueType_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            ArrayList arrayTypeId = new ArrayList();
            string toType = "";

            for (int i = 0; i < tlType.AllNodesCount; i++)
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode tn = tlType.FindNodeByID(i);
                int userTypeId = int.Parse(tn.GetValue(colTypeId).ToString());
                string typeName = tn.GetValue(colTypeName).ToString();

                if (tn.Checked == true && userTypeId != 0 && arrayTypeId.Contains(userTypeId) == false)
                {
                    arrayTypeId.Add(userTypeId);
                    toType += typeName + ",";
                }
            }

            if (toType != "")
            {
                toType = toType.Substring(0, toType.Length - 1);
                lueType.Text = toType;
                lueType.Tag = arrayTypeId;
            }
            else
            {
                lueType.Text = "";
                lueType.Tag = null;
            }
        }
    }
}
