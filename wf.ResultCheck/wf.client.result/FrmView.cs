using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList.Nodes.Operations;
using System.Collections;
using DevExpress.XtraTreeList;
using System.Data.SqlClient;
using dcl.client.common;
using dcl.client.wcf;

namespace dcl.client.result
{
    public partial class FrmView : FrmCommon
    {
        public FrmView()
        {
            InitializeComponent();
        }

        string date = "";
        string itr_id = "";

        public FrmView(string date,string itr_id)
        {
            InitializeComponent();
            this.date = date;
            this.itr_id = itr_id;
        }

        private void FrmView_Load(object sender, EventArgs e)
        {
            tlPat.DataSource = new ProxyMicEnter().Service.GetMicViewList(Convert.ToDateTime(date), itr_id);
            tlPat.ExpandAll();
        }



        public class CollapseExceptSpecifiedOperation : TreeListOperation
        {
            TreeListNode visibleNode;
            public CollapseExceptSpecifiedOperation(TreeListNode visibleNode)
                : base()
            {
                this.visibleNode = visibleNode;
            }
            public override void Execute(TreeListNode node)
            {
                    node.Expanded = true;
            }
            public override bool NeedsFullIteration { get { return false; } }
        }

    }


}
