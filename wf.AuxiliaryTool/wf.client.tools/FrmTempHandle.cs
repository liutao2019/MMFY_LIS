using dcl.client.common;
using dcl.client.frame;
using dcl.client.wcf;
using dcl.entity;
using DevExpress.XtraGauges.Win;
using lis.client.control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace dcl.client.tools
{
    public partial class FrmTempHandle : FrmCommon
    {
        #region 全局变量
        List<EntityTemHarvester> listTemHar;
        ProxyTempHandle proxy;
        Timer tickTime = new Timer();
        List<ConGauge> listCon = new List<ConGauge>();
        bool IsProChange = false;
        bool IsFirstLoad = false;//第一次改变JKEdit的值时不触发查找事件
        #endregion

        public FrmTempHandle()
        {
            InitializeComponent();
            this.Load += FrmTempHandle_Load;
            proxy = new ProxyTempHandle();
        }


        private void FrmTempHandle_Load(object sender, EventArgs e)
        {
            IsFirstLoad = true;
            JKDate.EditValue = DateTime.Now.Date;
            sysToolBar1.SetToolButtonStyle(new string[]{sysToolBar1.BtnRefresh.Name,sysToolBar1.BtnClose.Name,
            sysToolBar1.BtnSearch.Name});
            sysToolBar1.OnBtnSearchClicked += SysToolBar1_OnBtnSearchClicked;
            tickTime.Tick += TickTime_Tick;
            tickTime.Interval = 120000;
            IsProChange = true;
            IsFirstLoad = false;

            SearchData();
            tickTime.Start();
        }

        private void TickTime_Tick(object sender, EventArgs e)
        {
            SearchData();
        }

        private void SysToolBar1_OnBtnSearchClicked(object sender, EventArgs e)
        {
            SearchData();
        }

        private void SearchData()
        {
            string proId = selectDicLabProfession1.valueMember;
            DateTime dateTime = Convert.ToDateTime(JKDate.EditValue);
            listTemHar = proxy.Service.GetTempHarvesterByProId(proId, dateTime);
            // GetDataSource();
            CreatePicPanel(listTemHar.Count);
            LoadData(listTemHar);
        }

        /// <summary>
        /// 生成Panel
        /// </summary>
        /// <param name="count"></param>
        private void CreatePicPanel(int count)
        {
            if (IsProChange || listCon.Count == 0)
            {
                picPanel.Controls.Clear();
                listCon.Clear();

                int width = picPanel.Width;
                int num = width / 170;
                int x = 50;
                int y = 30;
                //动态加载温控图形控件
                for (int i = 1; i <= count; i++)
                {
                    Panel panelNew = new Panel();
                    panelNew.Width = 150;
                    panelNew.Height = 150;
                    panelNew.Location = new Point(x, y);
                    panelNew.Name = "panel" + i.ToString();
                    panelNew.BorderStyle = BorderStyle.None;

                    ConGauge gauge = new ConGauge();
                    gauge.AfterClicked += Gauge_AfterClicked;
                    gauge.MouseEntered += Gauge_MouseEntered;
                    gauge.Name = "gauge" + i.ToString();
                    panelNew.Controls.Add(gauge);
                    listCon.Add(gauge);
                    gauge.Dock = DockStyle.Fill;

                    if (i % num == 0)
                    {
                        x = 50;
                        y += 170;
                    }
                    else
                    {
                        x += 170;
                    }

                    picPanel.Controls.Add(panelNew);
                }
            }
        }

        private void Gauge_MouseEntered(object sender, EventArgs e)
        {
            GaugeControl gauge = sender as GaugeControl;
            gauge.Cursor = Cursors.Hand;
        }

        private void Gauge_AfterClicked(object sender, EventArgs e)
        {
            GaugeControl gauge = sender as GaugeControl;
            ConGauge conGauge = gauge.Parent as ConGauge;
            if (conGauge != null)
            {
                FrmTempLine temLine = new FrmTempLine();
                temLine.conTLine1.DhId = conGauge.tempHarvester.ThHId;
                temLine.Size = new Size(Convert.ToInt32(this.Width * 0.6), Convert.ToInt32(this.Height * 0.6));
                temLine.ShowDialog(this);
            }

        }

        /// <summary>
        /// 向panel添加数据
        /// </summary>
        /// <param name="listTempHar"></param>
        private void LoadData(List<EntityTemHarvester> listTempHar)
        {
            if (listTempHar != null && listTempHar.Count > 0)
            {
                for (int i = 0; i < listTempHar.Count; i++)
                {
                    listCon[i].tempHarvester = listTempHar[i];
                    listCon[i].LoadData();
                }
            }
            IsProChange = false;
        }

        private void selectDicLabProfession1_onAfterChange(EntityDicPubProfession oldRow)
        {
            if (!string.IsNullOrEmpty(selectDicLabProfession1.valueMember))
            {
                IsProChange = true;
                SearchData();
            }
            else
            {
                MessageDialog.Show("请选择一个实验室");
                return;
            }
        }

        private void JKDate_EditValueChanged(object sender, EventArgs e)
        {
            if (IsFirstLoad)
                return;
            SearchData();
        }

        private void FrmTempHandle_FormClosed(object sender, FormClosedEventArgs e)
        {
            tickTime.Stop();
        }

    }
}
