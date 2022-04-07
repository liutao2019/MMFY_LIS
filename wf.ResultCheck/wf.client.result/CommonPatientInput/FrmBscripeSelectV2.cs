using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.result.Interface;
using dcl.client.result.CommonPatientInput;
using dcl.client.wcf;
using dcl.client.common;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.result
{
    public partial class FrmBscripeSelectV2 : FrmCommon
    {
        public FrmBscripeSelectV2()
        {
            InitializeComponent();
        }

        IDescTemplateAccepter descParent = null;
        public delegate string GetCurInstrmtIDEventHandler();
        public event GetCurInstrmtIDEventHandler GetCurInstrmtID = null;

        public delegate void SelectExp(string exp);
        public event SelectExp GetExp = null;

        FrmPatInputBaseNew fbanew = null;
        string ftype = "";
        bool showCurrentExp = false;
        string currentExp = string.Empty;

        public FrmBscripeSelectV2(FrmPatInputBaseNew fb, string type)
        {
            InitializeComponent();
            fbanew = fb;
            ftype = type;
        }
        public FrmBscripeSelectV2(FrmPatInputBaseNew fb, string type,bool showExp,string Exp)
        {
            InitializeComponent();
            fbanew = fb;
            ftype = type;
            showCurrentExp = showExp;
            currentExp = Exp;
        }
        public FrmBscripeSelectV2(IDescTemplateAccepter descContainer, string type)
        {
            InitializeComponent();
            descParent = descContainer;
            ftype = type;
        }

        public FrmBscripeSelectV2(string type)
        {
            InitializeComponent();
            ftype = type;
        }

        private void FrmBscripe_Load(object sender, EventArgs e)
        {
            string instrmtID = string.Empty;
            if (GetCurInstrmtID != null)
            {
                instrmtID = GetCurInstrmtID();
            }

            List<EntityDicPubEvaluate> listEvaluate = CacheClient.GetCache<EntityDicPubEvaluate>()
                .FindAll(i => (i.EvaUserId == UserInfo.loginID || string.IsNullOrEmpty(i.EvaUserId)) && i.EvaFlag == ftype).OrderBy(i => i.EvaSortNo).ToList();
            if (showCurrentExp)
            {
                gpCurrentExp.Visible = true;
                memoEdit2.Text = currentExp;
                btnSave.Visible = true;
            }

            List<EntityDicPubEvaluate> desData = new List<EntityDicPubEvaluate>();
            if (!string.IsNullOrEmpty(instrmtID))
            {
                foreach (EntityDicPubEvaluate item in listEvaluate)
                {
                    if (!string.IsNullOrEmpty(item.EvaItrId))
                    {
                        string[] array = item.EvaItrId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (array.Length > 0)
                        {
                            List<string> list = new List<string>(array);
                            if (list.Contains(instrmtID))
                            {
                                desData.Add(item);
                            }
                        }
                    }
                    else
                    {
                        desData.Add(item);
                    }
                }
            }
            else
            {
                desData = listEvaluate;
            }

            this.bindingSource1.DataSource = desData;
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            GetBscripe();
            this.Close();
        }

        private void GetBscripe()
        {
            string str = memoEdit1.Text.Trim();
            if (showCurrentExp)
            {
                memoEdit2.Text += str;
            }
            if (str != string.Empty)
            {
                if (fbanew != null)
                {
                    if (!string.IsNullOrEmpty(memoEdit2.Text))
                    {
                        fbanew.getBscripe(memoEdit2.Text, ftype);
                    }
                    else
                    {
                        fbanew.getBscripe(str, ftype);
                    }
                  
                }
                if (descParent != null)
                {
                    descParent.ApplyDescriptionTemplate(str, ftype);
                }
                if (GetExp != null)
                {
                    GetExp(str);
                }

            }
        }




        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            EntityDicPubEvaluate dr = this.gridView1.GetFocusedRow() as EntityDicPubEvaluate;
            if (!string.IsNullOrEmpty(dr.EvaContent))
            {
                this.memoEdit1.EditValue = dr.EvaContent;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            GetBscripe();
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string str = memoEdit2.Text.Trim();
            if (str != string.Empty)
            {
                fbanew.getBscripe(str, "3");
            }
            this.Close();
        }
    }
}
