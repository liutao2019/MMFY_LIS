using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using dcl.entity;

namespace dcl.client.qc
{
    public partial class FrmQcParameterRuleInstNew : FrmCommon
    {
        List<EntityDicQcMateria> ds = new List<EntityDicQcMateria>();
        ProxyCommonDic proxy = new ProxyCommonDic("svc.FrmQcRuleInst");

        public FrmQcParameterRuleInstNew(List<EntityDicQcMateria> dsResult)  //(DataSet dsResult)
        {
            ds = dsResult;
            InitializeComponent();
        }

        private void FrmQcRuleInst_Load(object sender, EventArgs e)
        {
            //gcHistoryQcInterface.DataSource = ds.Tables["qc_rule_time"];
            gcHistoryQcInterface.DataSource = ds;

            Reflesh();
            sysInstrmt.SetToolButtonStyle(new string[] {sysInstrmt.BtnAdd.Name,sysInstrmt.BtnSave.Name,sysInstrmt.BtnModify.Name,
                sysInstrmt.BtnDelete.Name,sysInstrmt.BtnClose.Name,sysInstrmt.BtnCancel.Name });
            isCheckControl(false);

        }

        private void Reflesh()
        {
            //DataTable dtParameter = CommonClient.CreateDT(new string[] { "qrt_itr_id" }, "parameter");
            //dtParameter.Rows.Add(ds.Tables["qc_rule_time"].Rows[0]["qcm_mid"]);
            //bdRuleTime.DataSource = null;
            //DataSet dsSearch = new DataSet();
            //dsSearch.Tables.Add(dtParameter);

            //DataSet result = doAction.DoOther("quality/QcRuleInstBIZ.svc", dsSearch);
            //bdRuleTime.DataSource = result.Tables["qc_rule_time"];

            EntityDicQcRuleTime ruleTime = new EntityDicQcRuleTime();
            ruleTime.QrtItrId = ds[0].QcmMid;

            EntityRequest req = new EntityRequest();
            req.SetRequestValue(ruleTime);

            EntityResponse result = proxy.Other(req);
            List<EntityDicQcRuleTime> ruleTimeList = result.GetResult() as List<EntityDicQcRuleTime>;

            bdRuleTime.DataSource = ruleTimeList;

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
            gcHistoryQcInterface.Enabled = !isCheck;
            sysInstrmt.BtnSave.Enabled = isCheck;
            sysInstrmt.BtnCancel.Enabled = isCheck;
            sysInstrmt.BtnAdd.Enabled = !isCheck;
            sysInstrmt.BtnModify.Enabled = !isCheck;
            sysInstrmt.BtnDelete.Enabled = !isCheck;
        }

        private void gvRuleTime_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            this.gvQcInterface.FocusedRowChanged -= new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvQcInterface_FocusedRowChanged);

            if (bdRuleTime.Current != null)
            {
                //DataRow drRuleTime = ((DataRowView)bdRuleTime.Current).Row;
                EntityDicQcRuleTime drRuleTime = (EntityDicQcRuleTime)bdRuleTime.Current;

                //if (drRuleTime["qrt_id"] != null && drRuleTime["qrt_id"].ToString().Trim() != "")
                //{
                //    deStartTime.EditValue = drRuleTime["qrt_start_time"];
                //    deEndTime.EditValue = drRuleTime["qrt_end_time"];
                //    txtDay.EditValue = drRuleTime["qrt_day"];
                //}
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

                //DataTable dtParameter = CommonClient.CreateDT(new string[] { "qcm_mid", "qcm_time_id" }, "parameter");

                //dtParameter.Rows.Add(gvHistoryQcInterface.GetFocusedDataRow()["qcm_mid"], drRuleTime["qrt_id"]);
                //DataSet dsSearch = new DataSet();
                //dsSearch.Tables.Add(dtParameter);
                //DataSet result = doAction.DoSearch("quality/QcRuleInstBIZ.svc", dsSearch);
                //gcQcInterface.DataSource = result.Tables["qc_interface"];

                EntityDicQcMateria drInstrmt = gvHistoryQcInterface.GetFocusedRow() as EntityDicQcMateria;

                EntityDicQcInstrmtChannel channel = new EntityDicQcInstrmtChannel();
                channel.ItrIdNew = drInstrmt.QcmMid;
                channel.MatTimeId = drRuleTime.QrtId.ToString();

                EntityRequest req = new EntityRequest();
                req.SetRequestValue(channel);

                EntityResponse result = proxy.Search(req);

                List<EntityDicQcInstrmtChannel> channelList = result.GetResult() as List<EntityDicQcInstrmtChannel>;
                gcQcInterface.DataSource = channelList;

                gvQcInterface.FocusedRowHandle = channelList.Count - 1;//锁定到最后一行（即新增的那一行）
            }
            else
            {
                gcQcInterface.DataSource = null;
            }

            this.gvQcInterface.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvQcInterface_FocusedRowChanged);
            gvQcInterface_FocusedRowChanged(null, null);
        }

        private void gvQcInterface_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gvQcInterface.GetFocusedRow() != null)
            {
                //DataRow drQcInterface = gvQcInterface.GetFocusedDataRow();
                EntityDicQcInstrmtChannel drQcInterface = gvQcInterface.GetFocusedRow() as EntityDicQcInstrmtChannel;

                //if (drQcInterface["qcm_type"] != null && drQcInterface["qcm_type"].ToString().Trim() == "0")
                if (drQcInterface.ChannelType.ToString().Trim() == "0")
                    ckSid.Checked = true;
                else
                    ckCode.Checked = true;

                //txtSample.EditValue = drQcInterface["qcm_no_sid"];
                txtSample.EditValue = drQcInterface.SidIdent;
            }
            else
            {
                txtSample.EditValue = null;
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
            bdRuleTime.EndEdit();
            Reflesh();
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
                    //DataRow drQcInterface = gvQcInterface.GetFocusedDataRow();
                    //drQcInterface["qcm_no_sid"] = txtSample.EditValue;
                    EntityDicQcInstrmtChannel drQcInterface = gvQcInterface.GetFocusedRow() as EntityDicQcInstrmtChannel;
                    drQcInterface.SidIdent = txtSample.EditValue.ToString();
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
                    //DataRow drQcInterface = gvQcInterface.GetFocusedDataRow();
                    //drQcInterface["qcm_type"] = ckSid.Checked == true ? "0" : "1";
                    //drQcInterface["qcm_type_name"] = ckSid.Checked == true ? "样本号" : "质控标识";

                    EntityDicQcInstrmtChannel drQcInterface = gvQcInterface.GetFocusedRow() as EntityDicQcInstrmtChannel;
                    drQcInterface.ChannelTypeNew = (ckSid.Checked == true ? 0 : 1);
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


            ///////////////////////////////////////////////
            //DataTable dtRuleTime = ((DataTable)bdRuleTime.DataSource).Clone();
            //dtRuleTime.TableName = "qc_rule_time";

            //DataRow drRuleTime = ((DataRowView)bdRuleTime.Current).Row;

            //drRuleTime["qrt_start_time"] = deStartTime.EditValue;
            //drRuleTime["qrt_end_time"] = deEndTime.EditValue;
            //drRuleTime["qrt_day"] = txtDay.EditValue;
            //drRuleTime["qrt_itr_id"] = gvHistoryQcInterface.GetFocusedDataRow()["qcm_mid"];

            //dtRuleTime.Rows.Add(drRuleTime.ItemArray);

            List<EntityDicQcRuleTime> dtRuleTime = new List<EntityDicQcRuleTime>();

            EntityDicQcMateria drInstrmt = gvHistoryQcInterface.GetFocusedRow() as EntityDicQcMateria;
            EntityDicQcRuleTime drRuleTime = bdRuleTime.Current as EntityDicQcRuleTime;
            drRuleTime.QrtStartTime = Convert.ToDateTime(deStartTime.EditValue);
            drRuleTime.QrtEndTime = Convert.ToDateTime(deEndTime.EditValue);
            drRuleTime.QrtDay = Convert.ToInt32(txtDay.EditValue);
            drRuleTime.QrtItrId = drInstrmt.QcmMid;

            dtRuleTime.Add(drRuleTime);

            //DataTable dtQcInterfaceSoure = (DataTable)gcQcInterface.DataSource;

            //DataTable dtQcInterface = dtQcInterfaceSoure.Clone();
            //dtQcInterface.TableName = "qc_interface";
            List<EntityDicQcInstrmtChannel> dtQcInterfaceSoure = gcQcInterface.DataSource as List<EntityDicQcInstrmtChannel>;

            List<EntityDicQcInstrmtChannel> dtQcInterface = new List<EntityDicQcInstrmtChannel>();

            //foreach (DataRow drQcInterface in dtQcInterfaceSoure.Rows)
            //{
            //    if (drQcInterface["qcm_no_sid"] != null && drQcInterface["qcm_no_sid"].ToString().Trim() != "")
            //    {
            //        drQcInterface["qcm_time_id"] = drRuleTime["qrt_id"];
            //        dtQcInterface.Rows.Add(drQcInterface.ItemArray);
            //    }
            //}
            foreach (var drQcInterface in dtQcInterfaceSoure)
            {
                if (drQcInterface.SidIdent != null && drQcInterface.SidIdent.ToString().Trim() != "")
                {
                    if (drRuleTime.QrtId == 0)
                    {
                        drQcInterface.MatTimeId = null;
                    }
                    else
                    {
                        drQcInterface.MatTimeId = drRuleTime.QrtId.ToString();
                    }

                    dtQcInterface.Add(drQcInterface);
                }
            }

            //DataSet dsResult = new DataSet();
            //dsResult.Tables.Add(dtRuleTime);
            //dsResult.Tables.Add(dtQcInterface);
            //DataSet ds = new DataSet();

            //ds = doAction.DoUpdate("quality/QcRuleInstBIZ.svc", dsResult);
            //  //base.doUpdate(dsResult);
            List<Object> objList = new List<Object>();
            objList.Add(dtRuleTime);
            objList.Add(dtQcInterface);

            EntityResponse result = new EntityResponse();

            EntityRequest ds = new EntityRequest();
            ds.SetRequestValue(objList);

            result = proxy.Update(ds);
            base.isActionSuccess = result.Scusess;

            if (!base.isActionSuccess)
                lis.client.control.MessageDialog.Show("保存失败！", "提示");

            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //    lis.client.control.MessageDialog.Show("保存失败！", "提示");
            //else
            //    lis.client.control.MessageDialog.Show("操作成功！");

            Reflesh();
            sysInstrmt.BtnAdd.Enabled = true;
            sysInstrmt.BtnModify.Enabled = true;
            sysInstrmt.BtnDelete.Enabled = true;
            sysInstrmt.BtnSave.Enabled = false;
            sysInstrmt.BtnCancel.Enabled = false;
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
                        //DataTable dtRuleTime = ((DataTable)bdRuleTime.DataSource).Clone();
                        //dtRuleTime.TableName = "qc_rule_time";
                        List<EntityDicQcRuleTime> dtRuleTime = new List<EntityDicQcRuleTime>();

                        //DataRow drRuleTime = ((DataRowView)bdRuleTime.Current).Row;
                        //drRuleTime["qrt_start_time"] = deStartTime.EditValue;
                        //drRuleTime["qrt_end_time"] = deEndTime.EditValue;
                        //drRuleTime["qrt_day"] = txtDay.EditValue;
                        //drRuleTime["qrt_itr_id"] = gvHistoryQcInterface.GetFocusedDataRow()["qcm_mid"];

                        //dtRuleTime.Rows.Add(drRuleTime.ItemArray);

                        EntityDicQcMateria drInstrmt = gvHistoryQcInterface.GetFocusedRow() as EntityDicQcMateria;
                        EntityDicQcRuleTime drRuleTime = bdRuleTime.Current as EntityDicQcRuleTime;

                        drRuleTime.QrtStartTime = Convert.ToDateTime(deStartTime.EditValue);
                        drRuleTime.QrtEndTime = Convert.ToDateTime(deEndTime.EditValue);
                        drRuleTime.QrtDay = Convert.ToInt32(txtDay.EditValue);
                        drRuleTime.QrtItrId = drInstrmt.QcmMid;

                        dtRuleTime.Add(drRuleTime);

                        //DataSet dsResult = new DataSet();
                        //dsResult.Tables.Add(dtRuleTime);
                        //DataSet ds = new DataSet();
                        //ds = doAction.DoDel("quality/QcRuleInstBIZ.svc", dsResult);

                        EntityResponse result = new EntityResponse();
                        EntityRequest res = new EntityRequest();
                        res.SetRequestValue(dtRuleTime);

                        result = proxy.Delete(res);
                        if (result.Scusess)
                        {
                            lis.client.control.MessageDialog.Show("操作成功！");
                        }

                        Reflesh();
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
