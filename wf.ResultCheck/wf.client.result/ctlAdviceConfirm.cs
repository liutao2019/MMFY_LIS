using System;
using System.Collections.Generic;
using System.Text;
using dcl.pub.entities;
using dcl.client.wcf;
using lis.client.control;
using System.Windows.Forms;
using dcl.pub.entities.dict;
using dcl.entity;

namespace dcl.client.result
{
    public class ctlAdviceConfirm
    {
        private string pat_in_no;
        private List<string> AdviceToConfirm;
        private string pat_ori_id;

        private string userCode;
        private string userName;
        private string bc_times;

        private bool bNeedUserCheck;

        public ctlAdviceConfirm(List<string> adviceToConfirm, string _pat_in_no, string ori_id, string userLoginID, string userLoginName, bool bNeedUserCheck)
        {
            this.AdviceToConfirm = adviceToConfirm;
            this.pat_in_no = _pat_in_no;
            this.pat_ori_id = ori_id;

            this.userCode = userLoginID;
            this.userName = userLoginName;
            this.bNeedUserCheck = bNeedUserCheck;
        }

        public ctlAdviceConfirm(List<EntityPidReportDetail> combineToConfirm, string _pat_in_no, string ori_id, string userLoginID, string userLoginName, bool bNeedUserCheck)
        {
            List<string> adviceToConfirm = new List<string>();

            foreach (EntityPidReportDetail item in combineToConfirm)
            {
                if (!string.IsNullOrEmpty(item.OrderSn))
                {
                    adviceToConfirm.Add(item.OrderSn);
                }
            }

            this.AdviceToConfirm = adviceToConfirm;
            this.pat_in_no = _pat_in_no;
            this.pat_ori_id = ori_id;

            this.userCode = userLoginID;
            this.userName = userLoginName;
            this.bNeedUserCheck = bNeedUserCheck;
        }
        public ctlAdviceConfirm(List<EntityPidReportDetail> combineToConfirm, string _pat_in_no, string ori_id, string userLoginID, string userLoginName,string bctimes, bool bNeedUserCheck)
        {
            List<string> adviceToConfirm = new List<string>();

            if (combineToConfirm != null)
            {
                foreach (EntityPidReportDetail item in combineToConfirm)
                {
                    if (!string.IsNullOrEmpty(item.OrderSn))
                    {
                        adviceToConfirm.Add(item.OrderSn);
                    }
                }
            }
            else
            {
                adviceToConfirm.Add(string.Empty);
            }

            this.AdviceToConfirm = adviceToConfirm;
            this.pat_in_no = _pat_in_no;
            this.pat_ori_id = ori_id;
            bc_times = bctimes;
            this.userCode = userLoginID;
            this.userName = userLoginName;
            this.bNeedUserCheck = bNeedUserCheck;
        }

        public void Confirm()
        {
            if (bNeedUserCheck)
            {
                //身份确认
                FrmCheckPassword frmCheck = new FrmCheckPassword();
                DialogResult dig = frmCheck.ShowDialog();

                if (dig == DialogResult.OK)
                {
                    this.userCode = frmCheck.OperatorID;
                    this.userName = frmCheck.OperatorName;
                }
                else
                {
                    return;
                }
            }

            ProxyAdviceConfirm proxyConfirm = new ProxyAdviceConfirm();

            foreach (string advice_id in this.AdviceToConfirm)
            {
                EntityDataInterfaceAdviceConfirmParameter p = new EntityDataInterfaceAdviceConfirmParameter();
                p.bc_in_no = this.pat_in_no;
                p.bc_yz_id = advice_id;
                p.op_code = userCode;
                p.op_name = userName;
                p.bc_times = bc_times;

                if (pat_ori_id == "107")//门诊
                {
                    proxyConfirm.Service.AdviceConfirm_MZ(p);
                    //proxyConfirm.Service.AdviceConfirm_MZ(this.pat_in_no, advice_id, userCode, userName);
                }
                else if (pat_ori_id == "108")//住院
                {
                    //proxyConfirm.Service.AdviceConfirm_ZY(this.pat_in_no, advice_id, userCode, userName);
                    proxyConfirm.Service.AdviceConfirm_ZY(p);
                }
                else if (pat_ori_id == "109")//体检
                {
                    proxyConfirm.Service.AdviceConfirm_TJ(p);
                }
            }
        }

        public bool ConfirmFee()
        {

            ProxyAdviceConfirm proxyConfirm = new ProxyAdviceConfirm();

            bool isSucc = true;
            foreach (string advice_id in this.AdviceToConfirm)
            {
                EntityDataInterfaceAdviceConfirmParameter p = new EntityDataInterfaceAdviceConfirmParameter();
                p.bc_in_no = this.pat_in_no;
                p.bc_yz_id = advice_id;
                p.op_code = userCode;
                p.op_name = userName;
                p.bc_times = bc_times;
                p.bc_name = "HN";
                if (pat_ori_id == "107")//门诊
                {
                  string ret  =proxyConfirm.Service.AdviceConfirm_MZ_WithRet(p);
                    if (!string.IsNullOrEmpty(ret) && ret.ToUpper() != "OK")
                    {
                        isSucc= false;
                    }
                    //proxyConfirm.Service.AdviceConfirm_MZ(this.pat_in_no, advice_id, userCode, userName);
                }
                else if (pat_ori_id == "108")//住院
                {
                    //proxyConfirm.Service.AdviceConfirm_ZY(this.pat_in_no, advice_id, userCode, userName);
                     string ret  = proxyConfirm.Service.AdviceConfirm_ZY_WithRet(p);
                     if (!string.IsNullOrEmpty(ret) && ret.ToUpper() != "OK")
                     {
                         isSucc = false;
                     }
                }
                else if (pat_ori_id == "109")//体检
                {
                      string ret  =proxyConfirm.Service.AdviceConfirm_TJ_WithRet(p);
                      if (!string.IsNullOrEmpty(ret) && ret.ToUpper() != "OK")
                      {
                          isSucc = false;
                      }
                }
            }
            return isSucc;
        }


        public void UnConfirm()
        {
            if (bNeedUserCheck)
            {
                //身份确认
                FrmCheckPassword frmCheck = new FrmCheckPassword();
                DialogResult dig = frmCheck.ShowDialog();

                if (dig == DialogResult.OK)
                {
                    this.userCode = frmCheck.OperatorID;
                    this.userName = frmCheck.OperatorName;
                }
                else
                {
                    return;
                }
            }

            ProxyAdviceConfirm proxyConfirm = new ProxyAdviceConfirm();

            foreach (string advice_id in this.AdviceToConfirm)
            {
                EntityDataInterfaceAdviceConfirmParameter p = new EntityDataInterfaceAdviceConfirmParameter();
                if (pat_ori_id == "107")//门诊
                {
                    proxyConfirm.Service.AdviceCancelConfirm_MZ(p);
                }
                else if (pat_ori_id == "108")//住院
                {
                    proxyConfirm.Service.AdviceCancelConfirm_ZY(p);
                }
            }
        }
    }
}
