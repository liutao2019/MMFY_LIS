using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.entity;

namespace dcl.client.qc
{
    public partial class FrmQcRuleInst : ConCommon
    {
        #region 全局变量
        ProxyCommonDic proxy = new ProxyCommonDic("svc.FrmQcRuleInst");
        #endregion
        public FrmQcRuleInst()
        {
            InitializeComponent();
        }

        private void FrmQcRuleInst_Load(object sender, EventArgs e)
        {
            EntityResponse res = proxy.View(new EntityRequest());
            List<EntityDicInstrument> instr = res.GetResult() as List<EntityDicInstrument>;
            gcInstrmt.DataSource = instr;

            gvInstrmt.ExpandAllGroups();
            sysInstrmt.SetToolButtonStyle(new string[] {sysInstrmt.BtnAdd.Name,sysInstrmt.BtnSave.Name,sysInstrmt.BtnModify.Name,
                sysInstrmt.BtnDelete.Name,sysInstrmt.BtnClose.Name,sysInstrmt.BtnCancel.Name });
            isCheckControl(false);


        }


        private void isCheckControl(bool isCheck)
        {
            deStartTime.Properties.ReadOnly = !isCheck;
            deEndTime.Properties.ReadOnly = !isCheck;
            txtDay.Properties.ReadOnly = !isCheck;
            txtSample.Properties.ReadOnly = !isCheck;
            ckCode.Properties.ReadOnly = !isCheck;
            ckSid.Properties.ReadOnly = !isCheck;
            gcRuleTime.Enabled = !isCheck;
            sysInstrmt.BtnSave.Enabled = isCheck;
            sysInstrmt.BtnCancel.Enabled = isCheck;
            sysInstrmt.BtnAdd.Enabled = !isCheck;
            sysInstrmt.BtnModify.Enabled = !isCheck;
            sysInstrmt.BtnDelete.Enabled = !isCheck;
        }

        private void gvInstrmt_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gvInstrmt.GetFocusedRow() != null)
            {
                EntityDicInstrument drInstrmt = gvInstrmt.GetFocusedRow() as EntityDicInstrument;

                EntityDicQcRuleTime ruleTime = new EntityDicQcRuleTime();
                ruleTime.QrtItrId = drInstrmt.ItrId;
                EntityRequest req = new EntityRequest();
                req.SetRequestValue(ruleTime);

                EntityResponse result = proxy.Other(req);

                List<EntityDicQcRuleTime> ruleTimeList = result.GetResult() as List<EntityDicQcRuleTime>;

                bdRuleTime.DataSource = null;
                bdRuleTime.DataSource = ruleTimeList;
                gvQcInterface_FocusedRowChanged(null, null);
            }
        }

        private void gvRuleTime_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //‘-=’不执行事件，与本方法尾行的+事件呼应
            this.gvQcInterface.FocusedRowChanged -= new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvQcInterface_FocusedRowChanged);

            if (bdRuleTime.Current != null)
            {
                EntityDicQcRuleTime drRuleTime = bdRuleTime.Current as EntityDicQcRuleTime;

                if (drRuleTime.QrtId.ToString().Trim() != "0")
                {
                    deStartTime.EditValue = drRuleTime.QrtStartTime;
                    deEndTime.EditValue = drRuleTime.QrtEndTime;
                    txtDay.EditValue = drRuleTime.QrtDay;
                }
                else
                {
                    deStartTime.EditValue = DateTime.Now.Date;
                    deEndTime.EditValue = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                    txtDay.EditValue = 0;
                }

                EntityDicInstrument drInstrmt = gvInstrmt.GetFocusedRow() as EntityDicInstrument;
                EntityDicQcInstrmtChannel channel = new EntityDicQcInstrmtChannel();
                channel.ItrIdNew = drInstrmt.ItrId;
                channel.MatTimeId = drRuleTime.QrtId.ToString();

                EntityRequest req = new EntityRequest();
                req.SetRequestValue(channel);

                EntityResponse result = proxy.Search(req);

                List<EntityDicQcInstrmtChannel> channelList = result.GetResult() as List<EntityDicQcInstrmtChannel>;
                gcQcInterface.DataSource = channelList;
            }
            else
            {
                gcQcInterface.DataSource = null;
            }

            //下两者缺一不可，目的：防止数据行相同时导致行没有变化，就不会执行相应FocusedRowChanged事件
            this.gvQcInterface.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvQcInterface_FocusedRowChanged);
            gvQcInterface_FocusedRowChanged(null, null);
        }

        private void gvQcInterface_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gvQcInterface.GetFocusedRow() != null)
            {
                EntityDicQcInstrmtChannel drQcInterface = gvQcInterface.GetFocusedRow() as EntityDicQcInstrmtChannel;

                if (drQcInterface.ChannelTypeNew.ToString() != null && drQcInterface.ChannelTypeNew.ToString().Trim() == "0")
                    ckSid.Checked = true;
                else
                    ckCode.Checked = true;

                txtSample.EditValue = drQcInterface.SidIdent;
            }
            else
            {
                txtSample.Text = string.Empty;
            }
        }

        private void sysInstrmt_OnBtnModifyClicked(object sender, EventArgs e)
        {
            if (bdRuleTime.Current != null)
            {
                isCheckControl(true);
            }
            else
            {
                lis.client.control.MessageDialog.Show("没有质控时间，请先新增一个时间段!", "提示");
                return;
            }
        }

        private void sysInstrmt_OnBtnCancelClicked(object sender, EventArgs e)
        {
            gvInstrmt_FocusedRowChanged(null, null);
            gvRuleTime_FocusedRowChanged(null, null);
            isCheckControl(false);
        }

        private void sysInstrmt_OnBtnAddClicked(object sender, EventArgs e)
        {
            bdRuleTime.AddNew();
            isCheckControl(true);
        }

        private void txtSample_EditValueChanged(object sender, EventArgs e)
        {
            if (gvQcInterface.GetFocusedRow() != null)
            {
                if (!txtSample.Properties.ReadOnly)
                {
                    EntityDicQcInstrmtChannel drQcInterface = gvQcInterface.GetFocusedRow() as EntityDicQcInstrmtChannel;
                    drQcInterface.SidIdent = (string)txtSample.EditValue;
                    this.gcQcInterface.RefreshDataSource();
                }
            }
        }

        private void ckSid_EditValueChanged(object sender, EventArgs e)
        {
            if (gvQcInterface.GetFocusedRow() != null)
            {
                if (!ckSid.Properties.ReadOnly)
                {
                    EntityDicQcInstrmtChannel drQcInterface = gvQcInterface.GetFocusedRow() as EntityDicQcInstrmtChannel;
                    drQcInterface.ChannelTypeNew = Convert.ToInt32(ckSid.Checked == true ? "0" : "1");
                    drQcInterface.ChannelTypeName = (ckSid.Checked == true ? "样本号" : "质控标识");
                    this.gcQcInterface.RefreshDataSource();
                }
            }
        }

        private void sysInstrmt_OnBtnSaveClicked(object sender, EventArgs e)
        {
            if (txtDay.Text.Trim() == "0")
            {
                if (Convert.ToDateTime(deEndTime.EditValue).CompareTo(Convert.ToDateTime(deStartTime.EditValue)) < 0)
                {
                    lis.client.control.MessageDialog.Show("结束时间不能小于开始时间！", "提示");
                    deEndTime.Focus();
                    return;
                }
            }
            if (bdRuleTime.Current == null)
            {
                lis.client.control.MessageDialog.Show("没有质控时间，请先新增一个时间段！", "提示");
                return;
            }

            List<EntityDicQcRuleTime> dtRuleTime = new List<EntityDicQcRuleTime>();

            EntityDicInstrument drInstrmt = gvInstrmt.GetFocusedRow() as EntityDicInstrument;
            EntityDicQcRuleTime drRuleTime = bdRuleTime.Current as EntityDicQcRuleTime;
            drRuleTime.QrtStartTime = Convert.ToDateTime(deStartTime.EditValue);
            drRuleTime.QrtEndTime = Convert.ToDateTime(deEndTime.EditValue);
            drRuleTime.QrtDay = Convert.ToInt32(txtDay.EditValue);
            drRuleTime.QrtItrId = drInstrmt.ItrId;

            dtRuleTime.Add(drRuleTime);

            List<EntityDicQcInstrmtChannel> dtQcInterfaceSoure = gcQcInterface.DataSource as List<EntityDicQcInstrmtChannel>;

            List<EntityDicQcInstrmtChannel> dtQcInterface = new List<EntityDicQcInstrmtChannel>();

            foreach (var drQcInterface in dtQcInterfaceSoure)
            {
                if (drQcInterface.SidIdent != null && drQcInterface.SidIdent.ToString().Trim() != "")
                {
                    drQcInterface.MatTimeId = drRuleTime.QrtId.ToString();
                    dtQcInterface.Add(drQcInterface);
                }
            }

            List<Object> objList = new List<Object>();
            objList.Add(dtRuleTime);
            objList.Add(dtQcInterface);

            EntityResponse result = new EntityResponse();

            EntityRequest dsResult = new EntityRequest();
            dsResult.SetRequestValue(objList);

            result = proxy.Update(dsResult);
            base.isActionSuccess = result.Scusess;

            if (!base.isActionSuccess)
                lis.client.control.MessageDialog.Show("保存失败！", "提示");

            sysInstrmt_OnBtnCancelClicked(null, null);
            isCheckControl(false);
        }

        private void sysInstrmt_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            if (bdRuleTime.Current != null)
            {
                if (lis.client.control.MessageDialog.Show("是要删除该时间段？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (lis.client.control.MessageDialog.Show("删除该时间段会删除该时间段下的仪器通道，是否继续？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        List<EntityDicQcRuleTime> dtRuleTime = new List<EntityDicQcRuleTime>();

                        EntityDicInstrument drInstrmt = gvInstrmt.GetFocusedRow() as EntityDicInstrument;
                        EntityDicQcRuleTime drRuleTime = bdRuleTime.Current as EntityDicQcRuleTime;

                        drRuleTime.QrtStartTime = Convert.ToDateTime(deStartTime.EditValue);
                        drRuleTime.QrtEndTime = Convert.ToDateTime(deEndTime.EditValue);
                        drRuleTime.QrtDay = Convert.ToInt32(txtDay.EditValue);
                        drRuleTime.QrtItrId = drInstrmt.ItrId;

                        dtRuleTime.Add(drRuleTime);

                        EntityRequest res = new EntityRequest();
                        res.SetRequestValue(dtRuleTime);

                        proxy.Delete(res);

                        sysInstrmt_OnBtnCancelClicked(null, null);
                        isCheckControl(false);
                    }
                }
            }
            else
                lis.client.control.MessageDialog.Show("请选择你要删除的时间！", "提示");
        }
    }
}
