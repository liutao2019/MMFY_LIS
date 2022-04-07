using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using lis.client.control;

using dcl.client.wcf;
using System.Reflection;
using dcl.client.common;

using dcl.client.report;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.oa
{
    public partial class FrmHandOverNullRes : FrmCommon
    {
        /// <summary>
        /// 只查看
        /// </summary>
        public bool IsOnlyShow { get; set; }

        /// <summary>
        /// 新增
        /// </summary>
        public bool IsNwAdd { get;private  set; }

        /// <summary>
        /// 全部原结果
        /// </summary>
        public  DataTable dtRes { get; set; }

        /// <summary>
        /// 以项目分组的结果
        /// </summary>
        private DataTable dtResItm { get; set; }

        /// <summary>
        /// 以组合分组的结果
        /// </summary>
        private DataTable dtResCom { get; set; }


        public FrmHandOverNullRes(bool nwadd,bool onlyshow,DataTable dt)
        {
            InitializeComponent();

            IsOnlyShow = onlyshow;//只看
            IsNwAdd = nwadd;//新增
            dtRes = dt;
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmHandOverNullRes_Load(object sender, EventArgs e)
        {
            //不是只看
            if (!IsOnlyShow)
            {
                List<EntityDicPubEvaluate> listPubEve = CacheClient.GetCache<EntityDicPubEvaluate>();
                if (listPubEve != null && listPubEve.Count > 0)
                {
                    SetDictData(listPubEve);
                }


                if (dtRes != null && dtRes.Rows.Count > 0)
                {
                    //新增
                    if (IsNwAdd)
                    {
                        dtResItm = dtRes.Copy();

                        dtResCom = dtRes.Clone();

                        for (int i = 0; i < dtRes.Rows.Count; i++)
                        {
                            if (dtResCom.Select(string.Format("pat_id='{0}' and com_id='{1}'", dtRes.Rows[i]["pat_id"].ToString(), dtRes.Rows[i]["com_id"].ToString())).Length <= 0)
                            {
                                dtResCom.Rows.Add(dtRes.Rows[i].ItemArray);
                                dtResCom.Rows[(dtResCom.Rows.Count - 1)]["msg_flag"] = 2;
                            }
                        }

                        bsNullResult.DataSource = dtResItm;
                    }
                    else
                    {
                        //修改
                        rbtCom.Visible = false;
                        rbtItm.Visible = false;

                        if (dtRes.Rows[0]["msg_flag"].ToString() == "1")
                        {
                            gridColumn7.Visible = true;
                        }
                        else
                        {
                            gridColumn7.Visible = false;
                        }

                        bsNullResult.DataSource = dtRes;
                    }
                }
            }
            else
            {

                rbtCom.Visible = false;
                rbtItm.Visible = false;
                btnSure.Visible = false;

                this.gridColumn8.OptionsColumn.AllowEdit = false;
                this.gridColumn9.OptionsColumn.AllowEdit = false;

                if (dtRes != null && dtRes.Rows.Count > 0)
                {
                    rbtCom.Visible = false;
                    rbtItm.Visible = false;

                    if (dtRes.Rows[0]["msg_flag"].ToString() == "1")
                    {
                        gridColumn7.Visible = true;
                    }
                    else
                    {
                        gridColumn7.Visible = false;
                    }

                    bsNullResult.DataSource = dtRes;
                }
            }
        }

        public void SetDictData(List<EntityDicPubEvaluate> listPubEve)
        {
            foreach (EntityDicPubEvaluate pubEve in listPubEve)
            {
                if (pubEve.EvaFlag == "14")
                {
                    repositoryItemComboBox3.Items.Add(pubEve.EvaContent.Trim());
                    repositoryItemComboBox4.Items.Add(pubEve.EvaContent.Trim());
                }
            }
        }


       public  delegate void degSaveNullResData(DataTable dt);
       public event degSaveNullResData btnSaveNullResData;
       private void btnSure_Click(object sender, EventArgs e)
       {
           if (dtRes != null && dtRes.Rows.Count > 0)
           {
               if (btnSaveNullResData != null)
               {
                   if (IsNwAdd)
                   {
                       if (rbtItm.Checked)
                       {
                           dtResItm.AcceptChanges();
                           btnSaveNullResData(dtResItm);
                       }
                       else
                       {
                           dtResCom.AcceptChanges();
                           btnSaveNullResData(dtResCom);
                       }
                   }
                   else
                   {
                       dtRes.AcceptChanges();
                       btnSaveNullResData(dtRes);
                   }

                   this.DialogResult = DialogResult.Yes;
               }
           }
           else
           {
               lis.client.control.MessageDialog.ShowAutoCloseDialog("没有可保存的数据");
           }
       }

       private void rbtItm_CheckedChanged(object sender, EventArgs e)
       {
           if (rbtItm.Checked)
           {
               gridColumn7.Visible = true;
               if (dtRes != null && dtResItm.Rows.Count > 0)
               {
                   dtResCom.AcceptChanges();
                   bsNullResult.DataSource = dtResItm;
               }
           }
       }

       private void rbtCom_CheckedChanged(object sender, EventArgs e)
       {
           if (rbtCom.Checked)
           {
               gridColumn7.Visible = false;
               if (dtRes != null && dtResItm.Rows.Count > 0)
               {
                   dtResItm.AcceptChanges();
                   bsNullResult.DataSource = dtResCom;
               }
           }
       }
    }
}
