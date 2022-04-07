using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using dcl.client.frame;
using dcl.client.common;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.result.CommonPatientInput
{
    public partial class FrmItemInfo : FrmCommon
    {
        public FrmItemInfo()
        {
            InitializeComponent();
        }

        //public void LoadPropInfo(DataRow dr)
        //{
        //    if (dr != null)
        //    {
        //        this.txtItemHInfo.EditValue = dr["itm_h_info"];//偏高提示
        //        this.txtItemLInfo.EditValue = dr["itm_l_info"];//偏低提示
        //        this.txtItemSign.EditValue = dr["itm_sign"];//临床意义
        //    }
        //    else
        //    {
        //        this.txtItemHInfo.EditValue = string.Empty;
        //        this.txtItemLInfo.EditValue = string.Empty;
        //        this.txtItemSign.EditValue = string.Empty;
        //    }
        //}

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="itm_id"></param>
        public void LoadPropInfo(string itm_id, string sam_id)
        {
            List<EntityDicItemSample> listItmSample = CacheClient.GetCache<EntityDicItemSample>();
            int itmSamIndex = listItmSample.FindIndex(i => i.ItmId == itm_id && i.ItmSamId == sam_id);
            if (itmSamIndex != -1)
            {
                EntityDicItemSample itmSam = listItmSample[itmSamIndex];
                this.txtItemHInfo.EditValue = itmSam.ItmUpperTips;//偏高提示
                this.txtItemLInfo.EditValue = itmSam.ItmLowerTips;//偏低提示
                this.txtItemSign.EditValue = itmSam.ItmMeaning;//临床意义
            }
            else
            {
                this.txtItemHInfo.EditValue = string.Empty;
                this.txtItemLInfo.EditValue = string.Empty;
                this.txtItemSign.EditValue = string.Empty;
            }
        }

        private void FrmItemInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            this.Hide();
        }

        private void FrmItemInfo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27)
            {
                this.Hide();
            }
        }
    }
}
