using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.result.Interface;

using dcl.client.result.CommonPatientInput;
using dcl.client.frame;
using dcl.client.wcf;
using dcl.client.common;
using lis.client.control;
using dcl.entity;

namespace dcl.client.result.PatControl
{
    public partial class ControlPatDescResult : UserControl, IDescTemplateAccepter
    {
        public ControlPatDescResult()
        {
            InitializeComponent();
            PatID = string.Empty;
        }

        private string PatID;

        /// <summary>
        /// 加载描述结果
        /// </summary>
        /// <param name="pat_id"></param>
        public void LoadDescResult(string pat_id)
        {
            PatID = pat_id;
            ProxyObrResultDesc proxy = new ProxyObrResultDesc();
            List<EntityObrResultDesc> dt = proxy.Service.GetDescResultById(pat_id);

            LoadDescResult(dt);
        }

        /// <summary>
        /// 加载描述结果
        /// </summary>
        /// <param name="dtPatResult"></param>
        public void LoadDescResult(List<EntityObrResultDesc> dtPatResult)
        {
            if (dtPatResult != null && dtPatResult.Count > 0 && !string.IsNullOrEmpty(dtPatResult[0].ObrDescribe))
            {
                this.txtDesc.EditValue = dtPatResult[0].ObrDescribe;
                PatBarDesIsEnpty = false;
            }
            else
            {
                this.txtDesc.EditValue = string.Empty;
                PatBarDesIsEnpty = true;
            }
        }

        public bool PatBarDesIsEnpty { get; set; }
        public string GetBsr_describe()
        {
            return this.txtDesc.Text;
        }

        public List<EntityObrResultDesc> GetResult()
        {
            schema.Clear();
            EntityObrResultDesc dr = new EntityObrResultDesc();
            dr.ObrId = PatID;
            dr.ObrFlag = 1;
            if (this.txtDesc.EditValue != null)
            {
                dr.ObrDescribe = this.txtDesc.EditValue.ToString();
            }
            schema.Add(dr);
            return schema;
        }

        /// <summary>
        /// 更新样本号
        /// </summary>
        /// <param name="new_sid"></param>
        public void UpdateCurrentSID(string new_sid)
        {

        }

        private List<EntityObrResultDesc> schema;
        private void ControlPatDescResult_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                schema = new List<EntityObrResultDesc>();
            }
        }

        public void Reset()
        {
            this.txtDesc.EditValue = string.Empty;
            PatBarDesIsEnpty = true;
        }

        /// <summary>
        /// 描述报告范文选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTemplateSelect_Click(object sender, EventArgs e)
        {
            string pat_flag = string.Empty;
            if (!string.IsNullOrEmpty(this.PatID))
            {
                pat_flag = new ProxyPidReportMain().Service.GetPatientState(this.PatID);
            }
            if (pat_flag == string.Empty || pat_flag == LIS_Const.PATIENT_FLAG.Natural)
            {
                FrmBscripeSelectV2 fb = new FrmBscripeSelectV2(this, "3");
                fb.GetCurInstrmtID+=new FrmBscripeSelectV2.GetCurInstrmtIDEventHandler(fb_GetCurInstrmtID);
                fb.ShowDialog();
            }
            else
            {
                MessageDialog.Show("当前记录已" + LocalSetting.Current.Setting.AuditWord, "提示");
            }
            //}
        }
        public event dcl.client.result.FrmBscripeSelectV2.GetCurInstrmtIDEventHandler GetCurInstrmtID = null;

        string fb_GetCurInstrmtID()
        {
            if (GetCurInstrmtID != null)
            {
                return GetCurInstrmtID();
            }
            return string.Empty;
        }

        #region IDescriptionTemplate 成员

        public void ApplyDescriptionTemplate(string text, string type)
        {
            this.txtDesc.Text += text;
        }

        #endregion

        /// <summary>
        /// 保存当前内容为模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveAsTemplete_Click(object sender, EventArgs e)
        {
            if (this.txtDesc.Text.Trim() != string.Empty)
            {
                FrmDescTempleteConfirm frm = new FrmDescTempleteConfirm();
                DialogResult digResult = frm.ShowDialog();

                ProxyCommonDic proxy = new ProxyCommonDic("svc.ConBscripe");
                EntityRequest request = new EntityRequest();
                EntityDicPubEvaluate eva = new EntityDicPubEvaluate();
                eva.EvaContent = this.txtDesc.Text;
                eva.EvaFlag = "3";

                if (digResult == DialogResult.OK)
                {
                    if (frm.SaveAsPublic)
                    {
                        eva.EvaUserId = "";
                    }
                    else
                    {
                        eva.EvaUserId = UserInfo.loginID;
                    }
                    request.SetRequestValue(eva);
                    proxy.New(request);
                }
            }
            else
            {
                lis.client.control.MessageDialog.Show("当前内容为空", "提示");
            }
        }
    }
}
