using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using dcl.client.common;
using System.Collections;
using dcl.entity;
using dcl.client.cache;

namespace lis.client.control
{
    public partial class SelectDict_TypeChecks : UserControl
    {
        public SelectDict_TypeChecks()
        {
            InitializeComponent();
        }

        private void SelectDict_TypeChecks_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                DataTable dt = new DataTable("dicType");
                dt.Columns.Add("type_nodeId");
                dt.Columns.Add("type_name");
                dt.Columns.Add("type_id");
                dt.Columns.Add("type_node");

                List<EntityDicPubOrganize> listOrg = CacheClient.GetCache<EntityDicPubOrganize>();
                List<EntityDicPubProfession> listPro = CacheClient.GetCache<EntityDicPubProfession>();

                foreach (EntityDicPubOrganize org in listOrg)
                {
                    dt.Rows.Add(org.OrgId, org.OrgName, "0", org.OrgId);
                }
                foreach (EntityDicPubProfession pro in listPro)
                {
                    if (pro.ProType == 1)
                    {
                        string x = pro.ProId + "_" + pro.ProOrgId;
                        dt.Rows.Add(pro.ProId + "_" + pro.ProOrgId, pro.ProName, pro.ProId, pro.ProOrgId);
                    }
                }

                tlType.DataSource = dt;
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
