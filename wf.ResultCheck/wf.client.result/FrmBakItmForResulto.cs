using System;
using System.Collections.Generic;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using dcl.client.wcf;
using dcl.entity;

namespace dcl.client.result
{
    public partial class FrmBakItmForResulto : FrmCommon
    {
        /// <summary>
        /// 要备份的res_id
        /// </summary>
        public string bak_res_id { get; set; }

        public FrmBakItmForResulto()
        {
            InitializeComponent();
            InitSelection();
        }

        private void InitButtons()
        {
            sysToolBar1.BtnModify.Caption = "还原";
            sysToolBar1.SetToolButtonStyle(
                new string[] {
                     sysToolBar1.BtnModify.Name},
                    new string[] { "F1" }
                    );
        }

        private void InitSelection()
        {

            gvResInfo.ExpandAllGroups();
            //Selection = new GridCheckMarksSelection(gvResInfo);
            //Selection.CheckMarkColumn.Width = 20;
            //Selection.CheckMarkColumn.VisibleIndex = 0;
        }

        private void FrmBakItmForResulto_Load(object sender, EventArgs e)
        {
            InitButtons();
        }

        private void bsDateInfo_PositionChanged(object sender, EventArgs e)
        {
            //查询前清空列表和结果信息
            bsResInfo.DataSource = null;

            if (bsDateInfo.Position >= 0)
            {
                if (bsDateInfo.DataSource != null && bsDateInfo.DataSource is List<EntityObrResultBakItm>)
                {
                    EntityObrResultBakItm drTemp = ((List<EntityObrResultBakItm>)bsDateInfo.DataSource)[bsDateInfo.Position];

                    if (drTemp != null)
                    {
                        if ((!string.IsNullOrEmpty(drTemp.BakId)))
                        {
                            string temp_bak_id = drTemp.BakId;
                            List<EntityObrResultBakItm> listBakItm = proxyBakItm.Service.GetObrResultBakItm(temp_bak_id, 2);
                            if (listBakItm != null && listBakItm.Count > 0)
                            {
                                bsResInfo.DataSource = listBakItm;
                            }
                        }
                    }
                }
            }
        }
        ProxyObrResultBakItm proxyBakItm = new ProxyObrResultBakItm();
        private void FrmBakItmForResulto_Shown(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(bak_res_id))
            {
                List<EntityObrResultBakItm> listBakItm = proxyBakItm.Service.GetObrResultBakItm(bak_res_id, 1);

                if (listBakItm != null && listBakItm.Count > 0)
                {
                    bsDateInfo.DataSource = listBakItm;
                }
            }
        }

        /// <summary>
        /// 开始备份项目结果
        /// </summary>
        /// <param name="res_id">必填</param>
        /// <param name="res_itm_ids">可选</param>
        /// <param name="res_keys">可选</param>
        public void startBakResulto(string res_id, List<string> res_itm_ids, List<string> res_keys)
        {
            if (!string.IsNullOrEmpty(res_id))
            {
                string errorMsg = string.Empty;
                errorMsg = proxyBakItm.Service.InsertObrResultBakItm(res_id, res_itm_ids, res_keys);
                if (string.IsNullOrEmpty(errorMsg))
                {
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("备份成功", 1m);
                }
                else
                {
                    lis.client.control.MessageDialog.Show("备份失败!" + errorMsg, "提示");
                }
            }
            else
            {
                lis.client.control.MessageDialog.Show("备份失败!传入ID不能为空", "提示");
            }
        }

        /// <summary>
        /// 还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnModifyClicked(object sender, EventArgs e)
        {
            if (bsResInfo.DataSource != null && bsResInfo.DataSource is List<EntityObrResultBakItm>)
            {
                //List<EntityObrResultBakItm> listBakItm = bsResInfo.DataSource as List<EntityObrResultBakItm>;
                //listBakItm = listBakItm.FindAll(i => i.IsSelected == true);

                List<EntityObrResultBakItm> listBakItm = new List<EntityObrResultBakItm>();
                var selectIndex = this.gvResInfo.GetSelectedRows();
                foreach (int index in selectIndex)
                {
                    listBakItm.Add(gvResInfo.GetRow(index) as EntityObrResultBakItm);
                }


                if (listBakItm.Count > 0)
                {
                    if (lis.client.control.MessageDialog.Show("确定要还原项目结果?原来的结果将被覆盖", "提示", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return;
                    }

                    string temp_res_id = "";
                    List<string> resItmIds = new List<string>();
                    List<string> resKeys = new List<string>();
                    foreach (EntityObrResultBakItm index in listBakItm)
                    {
                        if (string.IsNullOrEmpty(temp_res_id)) { temp_res_id = index.ResId; }
                        resItmIds.Add(index.ResItmId);
                        resKeys.Add(index.ResKey.ToString());
                    }

                    //获取当前记录的审核状态
                    ProxyPidReportMain proxyPat = new ProxyPidReportMain();
                    string strPatState = proxyPat.Service.GetPatientState(temp_res_id);
                    if (strPatState == LIS_Const.PATIENT_FLAG.Natural || strPatState == string.Empty)//未审核
                    {
                        string errorMsg = string.Empty;
                        errorMsg = proxyBakItm.Service.InsertObrResultByBak(temp_res_id, resItmIds, resKeys);
                        if (string.IsNullOrEmpty(errorMsg))
                        {
                            lis.client.control.MessageDialog.ShowAutoCloseDialog("还原成功", 1m);
                        }
                        else
                        {
                            lis.client.control.MessageDialog.Show("还原失败!" + errorMsg, "提示");
                        }
                    }
                    else
                    {
                        lis.client.control.MessageDialog.Show("当前记录已" + LocalSetting.Current.Setting.AuditWord + "，不能再还原", "提示");
                    }
                }
                else
                {
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("请勾选需要还原的项目", 2m);
                }
            }
        }

        private void bsResInfo_DataSourceChanged(object sender, EventArgs e)
        {
            if (bsResInfo != null && bsResInfo.DataSource != null && bsResInfo.DataSource is List<EntityObrResultBakItm>)
            {
                lblMsgCount.Text = "当前数据 " + ((List<EntityObrResultBakItm>)bsResInfo.DataSource).Count + " 条";
            }
            else
            {
                lblMsgCount.Text = "当前数据 0 条";
            }
        }

    }
}
