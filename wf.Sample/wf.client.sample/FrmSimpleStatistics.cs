using dcl.client.control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;
using dcl.entity;
using dcl.client.wcf;
using dcl.client.common;
using dcl.client.cache;

namespace dcl.client.sample
{
    public partial class FrmSimpleStatistics : BaseButtonFormEx
    {
        public FrmSimpleStatistics()
        {
            InitializeComponent();
            GenComandButton();
            this.Load += FrmSimpleStatistics_Load;
        }

        private void FrmSimpleStatistics_Load(object sender, EventArgs e)
        {
            deStart.EditValue = ServerDateTime.GetServerDateTime().Date;
            deEnd.EditValue = ServerDateTime.GetServerDateTime().Date.AddDays(1).AddSeconds(-1); ;

        }

        private void GenComandButton()
        {
            try
            {
                AddComandButton("统计(F1)", dcl.client.common.Properties.Resources._32_统计, Count_Click, (int)Keys.F1);
                AddComandButton("关闭(F2)", dcl.client.common.Properties.Resources._32_关闭, Close_Click, (int)Keys.F2);
            }
            catch (Exception ex)
            {

            }
        }

        private void Count_Click(object sender, ItemClickEventArgs e)
        {
            EntitySampQC sampQuery = new EntitySampQC();
            sampQuery.StartDate = Convert.ToDateTime(deStart.EditValue).ToString("yyyy-MM-dd HH:mm:ss");
            sampQuery.EndDate = Convert.ToDateTime(deEnd.EditValue).ToString("yyyy-MM-dd HH:mm:ss");

            string OperUserID = string.Empty;
            if (selectAuditOper.valueMember != null && selectAuditOper.valueMember != "")
                OperUserID = selectAuditOper.valueMember.ToString();

            switch (OperType.Text)
            {
                case "采集者":
                    sampQuery.SerchDateStatus = "2";
                    if(!string.IsNullOrEmpty(OperUserID)) sampQuery.CollectionUserID = OperUserID;
                    break;
                case "送检者"://收取
                    sampQuery.SerchDateStatus = "3";
                    if (!string.IsNullOrEmpty(OperUserID)) sampQuery.SendUserID = OperUserID;
                    break;
                case "送达者":
                    sampQuery.SerchDateStatus = "4";
                    if (!string.IsNullOrEmpty(OperUserID)) sampQuery.ReachUserID = OperUserID;
                    break;
                case "签收者":
                    sampQuery.SerchDateStatus = "5";
                    if (!string.IsNullOrEmpty(OperUserID)) sampQuery.ReceivedUserID = OperUserID;
                    break;
            }

            ProxySampMain proxy = new ProxySampMain();
            List<EntitySampMain> listSamp = proxy.Service.SimpleStatisticSamp(sampQuery);
            bsPatient.DataSource = listSamp;
            Count(listSamp);
        }


        private void Count(List<EntitySampMain> listSamp)
        {
            List<EntitySampMain> countList = new List<EntitySampMain>();
            foreach (var item in listSamp)
            {
                if (countList.Exists(i=>i.PidInNo == item.PidInNo))
                    continue;

                countList.Add(item);
            }

            lcCount.Text = countList.Count.ToString();
        }

        private void Close_Click(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

    }
}
