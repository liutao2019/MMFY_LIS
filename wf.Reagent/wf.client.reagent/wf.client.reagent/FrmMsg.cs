using dcl.client.msgclient;
using dcl.client.wcf;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace wf.client.reagent
{
    public partial class FrmMsg : Form
    {
        public FrmMsg()
        {
            InitializeComponent();
        }

        private void FrmMsg_Load(object sender, EventArgs e)
        {
            //提示窗口是否最大化
            string IsMaximizedWindowState = GetAppValueByKey("IsMaximizedWindowState", "N");//如果为null添加默认值N

            if (IsMaximizedWindowState.ToUpper() == "Y" || IsMaximizedWindowState.ToUpper() == "YES")
            {
                //窗口最大化
                if (this.WindowState == FormWindowState.Normal || this.WindowState == FormWindowState.Minimized)
                {
                    this.WindowState = FormWindowState.Maximized;
                }
            }
            RefreshData(true);

        }
        /// <summary>
        /// 获取应用程序默认配置
        /// </summary>
        /// <param name="appKey">key值</param>
        /// <param name="defValue">默认value值(当为null)</param>
        /// <returns>value值</returns>
        private string GetAppValueByKey(string appKey, string defValue)
        {
            string kValue = null;

            try
            {
                kValue = ConfigurationManager.AppSettings[appKey];
            }
            catch
            {

            }
            //当默认值为空时,赋值-默认值
            if (string.IsNullOrEmpty(defValue))
            {
                defValue = "nullnull";
            }
            //当value为null时,赋值-默认值
            if (string.IsNullOrEmpty(kValue))
            {
                kValue = defValue;
            }

            return kValue;
        }

        void RefreshData(bool changeStatus)
        {
            List<EntityReaSetting> highlist = new List<EntityReaSetting>();
            List<EntityReaSetting> lowlist = new List<EntityReaSetting>();
            List<EntityReaStorageDetail> duesoonlist = new List<EntityReaStorageDetail>();
            List<EntityReaStorageDetail> expiredlist = new List<EntityReaStorageDetail>();

            ProxyReaSetting proxysetting = new ProxyReaSetting();
            ProxyReaStorageDetail proxydetail = new ProxyReaStorageDetail();
            
            List<EntityReaSetting> listsetting = proxysetting.Service.SearchReaSettingAll();

            List<EntityReaStorageDetail> listdetail = proxydetail.Service.getNotdelivered();

            foreach (var item in listsetting)
            {
                if (item.Drea_upper_limit !=0 && item.Rri_Count >= item.Drea_upper_limit)
                {
                    highlist.Add(item);
                }
                if (item.Drea_lower_limit != 0 && item.Rri_Count <= item.Drea_lower_limit)
                {
                    lowlist.Add(item);
                }
            }
            foreach (var item in listdetail)
            {
                if (DateTime.Now > item.Rsd_validdate)
                {
                    expiredlist.Add(item);
                }
                EntityReaSetting model = listsetting.Find(m => string.Equals(m.Drea_id,item.Rsd_reaid));
                int day = 0;
                int.TryParse(model.Drea_alarmdays, out day);
                if (DateTime.Now > item.Rsd_validdate.Date.AddDays(0-day))
                {
                    duesoonlist.Add(item);
                }
            }

            gcStoreHigh.DataSource = highlist;
            gcStoreLow.DataSource = lowlist;
            gcExpired.DataSource = expiredlist;
            gcDueSoon.DataSource = duesoonlist;

            if (highlist.Count > 0 || lowlist.Count > 0|| expiredlist.Count > 0|| duesoonlist.Count > 0)
            {
                #region 声音提示
                PlaySoundMgr.Instance.PlaySound();
                #endregion

                gcStoreHigh.RefreshDataSource();
                gcStoreLow.RefreshDataSource();
                gcExpired.RefreshDataSource();
                gcDueSoon.RefreshDataSource();
            }

        }
    }
}
